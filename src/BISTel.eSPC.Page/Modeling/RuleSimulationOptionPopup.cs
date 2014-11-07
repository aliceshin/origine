using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class RuleSimulationOptionPopup : BasePopupFrm
    {
        #region : Field

        private double _dUSL;
        private double _dLSL;
        private double _dUCL;
        private double _dLCL;

        private double _dStd;
        private double _dTrendLimit;
        private double _dUTL; //Upper Technical Limit
        private double _dLTL; //Lower Technical Limit
        private double _dRawUCL;
        private double _dRawLCL;

        private string _sRuleDescription;
        private string _sValueType;
        private DataTable _dtChartData;

        private int _iColIdxRuleOptName;
        private int _iColIdxValue;

        private ArrayList _alViolatedDataList;
        private int _iRealCount;

        private Initialization _Initialization;
        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _lang;

        private Control _ctlCallingThis;

        #endregion


        #region : Property

        public double USL
        {
            get { return _dUSL; }
            set { _dUSL = value; }
        }
        public double LSL
        {
            get { return _dLSL; }
            set { _dLSL = value; }
        }
        public double UCL
        {
            get { return _dUCL; }
            set { _dUCL = value; }
        }
        public double LCL
        {
            get { return _dLCL; }
            set { _dLCL = value; }
        }

        public double STD
        {
            get { return _dStd; }
            set { _dStd = value; }
        }
        public double UTL
        {
            get { return _dUTL; }
            set { _dUTL = value; }
        }
        public double LTL
        {
            get { return _dLTL; }
            set { _dLTL = value; }
        }
        public double TREND_LIMIT
        {
            get { return _dTrendLimit; }
            set { _dTrendLimit = value; }
        }
        public double RAW_UCL
        {
            get { return _dRawUCL; }
            set { _dRawUCL = value; }
        }
        public double RAW_LCL
        {
            get { return _dRawLCL; }
            set { _dRawLCL = value; }
        }


        public string RULE_DESCRIPTION
        {
            set { _sRuleDescription = value; }
        }
        public string VALUETYPE
        {
            set { _sValueType = value; }
        }

        public DataTable CHART_DATA
        {
            get { return _dtChartData; }
            set { _dtChartData = value; }
        }

        public ArrayList VIOLATED_DATA_LIST
        {
            get { return _alViolatedDataList; }
            set { _alViolatedDataList = value; }
        }
        public int REAL_COUNT
        {
            get { return _iRealCount; }
            set { _iRealCount = value; }
        }

        public Control CONTROL_CALLING_THIS
        {
            set { _ctlCallingThis = value; }
        }

        #endregion


        #region : Constructor, Initialize

        public RuleSimulationOptionPopup()
        {
            InitializeComponent();
        }

        public void InitializePopup()
        {
            this.Title = Definition.POPUP_TITLE_RULE_SIMULATION_OPTION;
            this.lblRule.Text = _sRuleDescription;

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._wsSPC = new eSPCWebService.eSPCWebService();
            this._lang = MultiLanguageHandler.getInstance();

            InitializeBSpread();
        }

        private void InitializeBSpread()
        {
            this._Initialization.InitializeColumnHeader(ref bsprRuleOpt, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE_MASTER);
            this.bsprRuleOpt.UseHeadColor = true;
            this.bsprRuleOpt.UseAutoSort = false;
            this.bsprRuleOpt.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprRuleOpt.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            string currentRuleNo = _sRuleDescription.Split('.')[0];

            _iColIdxRuleOptName = ((DataSet)bsprRuleOpt.DataSet).Tables[0].Columns.IndexOf(Definition.OPTION_NAME);
            _iColIdxValue = ((DataSet)bsprRuleOpt.DataSet).Tables[0].Columns.IndexOf(Definition.RULE_OPTION_VALUE);

            this.bsprRuleOpt.UseGeneralContextMenu = false;
            this.bsprRuleOpt.ActiveSheet.Columns[_iColIdxValue].BackColor = Color.Yellow;

            DataSet dsRuleOpt = _wsSPC.GetSPCRuleMasterData(null);
            DataTable dtTempRuleOpt = dsRuleOpt.Tables[TABLE.RULE_OPT_MST_SPC];
            DataTable dtRuleOpt = dtTempRuleOpt.Clone();
            DataRow[] drRuleOpts = dtTempRuleOpt.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, currentRuleNo), COLUMN.RULE_OPTION_NO);

            foreach (DataRow drRuleOpt in drRuleOpts)
            {
                dtRuleOpt.ImportRow(drRuleOpt);
            }

            this.bsprRuleOpt.DataSet = dtRuleOpt;

            if (currentRuleNo.Equals("13") || currentRuleNo.Equals("14") || currentRuleNo.Equals("15") ||
                currentRuleNo.Equals("16") || currentRuleNo.Equals("17"))
            {
                DataRow dr = dtRuleOpt.NewRow();
                dr[Definition.OPTION_NAME] = Definition.CHART_COLUMN.STDDEV;
                dr[Definition.DESCRIPTION] = "Standard Deviation (Default = " + _dStd.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dStd.ToString();

                dtRuleOpt.Rows.Add(dr);
            }
            else if (currentRuleNo.Equals("41"))
            {
                DataRow dr = dtRuleOpt.NewRow();
                dr[Definition.OPTION_NAME] = Definition.CHART_COLUMN.RAW_UCL;
                dr[Definition.DESCRIPTION] = "Raw UCL (Default = " + _dRawUCL.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dRawUCL.ToString();

                dtRuleOpt.Rows.Add(dr);
            }
            else if (currentRuleNo.Equals("42"))
            {
                DataRow dr = dtRuleOpt.NewRow();
                dr[Definition.OPTION_NAME] = Definition.CHART_COLUMN.RAW_LCL;
                dr[Definition.DESCRIPTION] = "Raw LCL (Default = " + _dRawLCL.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dRawLCL.ToString();

                dtRuleOpt.Rows.Add(dr);
            }
            else if (currentRuleNo.Equals("43") || currentRuleNo.Equals("45"))
            {
                DataRow dr = dtRuleOpt.Rows[0];
                dr[Definition.DESCRIPTION] = dr[Definition.DESCRIPTION].ToString() + " (Default = " + _dUTL.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dUTL.ToString();
            }
            else if (currentRuleNo.Equals("44") || currentRuleNo.Equals("46"))
            {
                DataRow dr = dtRuleOpt.Rows[0];
                dr[Definition.DESCRIPTION] = dr[Definition.DESCRIPTION].ToString() + " (Default = " + _dLTL.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dLTL.ToString();
            }
            else if (currentRuleNo.Equals("9") || currentRuleNo.Equals("10") || currentRuleNo.Equals("47") ||
                currentRuleNo.Equals("48"))
            {
                int trendValueRowIndex = 0;

                for (int i = 0; i < dtRuleOpt.Rows.Count; i++)
                {
                    DataRow drTemp = dtRuleOpt.Rows[i];

                    if (drTemp[Definition.OPTION_NAME].ToString().Contains(Definition.CHART_COLUMN.TREND_LIMIT))
                    {
                        trendValueRowIndex = i;
                        break;
                    }
                }

                DataRow dr = dtRuleOpt.Rows[trendValueRowIndex];
                //dr[Definition.OPTION_NAME] = Definition.CHART_COLUMN.TREND_LIMIT;

                dr[Definition.DESCRIPTION] = dr[Definition.DESCRIPTION].ToString() + " (Default = " + _dTrendLimit.ToString() + ")";
                dr[Definition.RULE_OPTION_VALUE] = _dTrendLimit.ToString();
            }

            bsprRuleOpt.DataSet = dtRuleOpt;

            //입력이 안 되어야 하는 것들은 날려버리기
            if (bsprRuleOpt.ActiveSheet.RowCount > 0)
            {
                //int count = bsprRuleOpt.ActiveSheet.RowCount;

                for (int i = 0; i < bsprRuleOpt.ActiveSheet.RowCount; i++)
                {
                    string optionName = this.bsprRuleOpt.ActiveSheet.Cells[i, _iColIdxRuleOptName].Text;

                    if (optionName == "Center line" || optionName == "LCL" || optionName == "UCL" ||
                        optionName == "USL" || optionName == "LSL" || optionName == "zone A" ||
                        optionName == "zone B" || optionName == "zone C" || optionName == "lambda")
                    {
                        this.bsprRuleOpt.ActiveSheet.RemoveRows(i, 1);
                        i--;
                    }
                    else
                    {
                        this.bsprRuleOpt.ActiveSheet.Cells[i, _iColIdxValue].Locked = false;
                    }
                }
            }
        }

        #endregion

        private void bbtnOk_Click(object sender, EventArgs e)
        {
            RuleSimulation rs = new RuleSimulation();
            rs.Initialize();

            //값을 넘겨주는 부분
            rs.USL = _dUSL;
            rs.LSL = _dLSL;
            rs.UCL = _dUCL;
            rs.LCL = _dLCL;
            rs.VALUETYPE = _sValueType;
            rs.MODULE_MODE = this.bchkModuleMode.Checked;
            rs.CHART_DATA = _dtChartData;

            if (bsprRuleOpt.ActiveSheet.RowCount > 0)
            {
                int n = 0;
                for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
                {
                    string optionName = this.bsprRuleOpt.ActiveSheet.Cells[i, _iColIdxRuleOptName].Text;
                    string valueText = this.bsprRuleOpt.ActiveSheet.Cells[i, _iColIdxValue].Text;
                    int iValue = 0;
                    double dValue = 0.0;

                    //Row를 삭제하면, 해당 row의 모든 값이 false가 됨.
                    if (optionName.Equals(Definition.VARIABLE_FALSE))
                    {
                        optionName = this.bsprRuleOpt.ActiveSheet.Cells[i + 1, _iColIdxRuleOptName].Text;
                        valueText = this.bsprRuleOpt.ActiveSheet.Cells[i + 1, _iColIdxValue].Text;
                    }

                    if (optionName.ToUpper().Equals("N") || optionName.ToUpper().Equals("M"))
                    {
                        if (Int32.TryParse(valueText, out iValue))
                        {
                            if (iValue <= 0)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { optionName }, null);
                                return;
                            }
                        }
                        else
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { optionName }, null);
                            return;
                        }
                    }
                    else
                    {
                        if (!double.TryParse(valueText, out dValue))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[] { optionName }, null);
                            return;
                        }
                    }

                    if (optionName.ToUpper().Equals("N"))
                    {
                        n = iValue;
                        rs.N = iValue;
                    }
                    else if (optionName.ToUpper().Equals("M"))
                    {
                        if (n > iValue)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { "N", "M" }, null);
                            return;
                        }

                        rs.M = iValue;
                    }
                    else if (optionName.ToUpper().Equals(Definition.CHART_COLUMN.STDDEV))
                    {
                        if (dValue < 0.0)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { optionName }, null);
                            return;
                        }

                        rs.STD = dValue;
                    }
                    else if (optionName.ToUpper().Equals(Definition.CHART_COLUMN.RAW_UCL))
                    {
                        rs.RAW_UCL = dValue;
                    }
                    else if (optionName.ToUpper().Equals(Definition.CHART_COLUMN.RAW_LCL))
                    {
                        rs.RAW_LCL = dValue;
                    }
                    else if (optionName.ToUpper().Equals(Definition.CHART_COLUMN.UTL))
                    {
                        rs.UTL = dValue;
                    }
                    else if (optionName.ToUpper().Equals(Definition.CHART_COLUMN.LTL))
                    {
                        rs.LTL = dValue;
                    }
                    else if (optionName.Contains(Definition.CHART_COLUMN.TREND_LIMIT))
                    {
                        rs.TRENDLIMIT = dValue;
                    }
                }
            }

            EESProgressBar.ShowProgress(_ctlCallingThis, this._lang.GetMessage(Definition.VALIDATING_RULE), true);

            //값을 받는 부분
            _alViolatedDataList = rs.Simulate(_sRuleDescription.Split('.')[0]);
            _iRealCount = rs.REAL_COUNT;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
