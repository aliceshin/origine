using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common.ATT;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class OptionalConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

        private SortedList _slColumnIndex = new SortedList();

        private int _ColIdx_SELECT;

        private int _ColIdx_CODE;
        private int _ColIdx_NAME;

        private BISTel.eSPC.Common.ConfigMode _cofingMode;
        private string _sConfigRawID;

        private DataTable _dtConfigOPT;

        private string _sDefaultChartList;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public OptionalConfigurationUC()
        {
            InitializeComponent();

        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            this.InitializePage();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }


        public void InitializeLayout()
        {
        }

        private void InitializeCode()
        {
            LinkedList llconidtion = new LinkedList();

            //PARAM TYPE
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PARAM_CATEGORY);

            DataSet _dsParamCategory = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboParamCategory, _dsParamCategory, BISTel.eSPC.Common.COLUMN.NAME, BISTel.eSPC.Common.COLUMN.CODE, "", true);

            //AUTO TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PRIORITY);

            DataSet _dsPriority = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboPriority, _dsPriority, BISTel.eSPC.Common.COLUMN.NAME, BISTel.eSPC.Common.COLUMN.CODE, "", true);
        }


        public void InitializeDataButton()
        {
            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
        }



        public void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();

            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprDefaultChart, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_DEFAULT_CHART);
            this.bsprDefaultChart.UseHeadColor = true;
            this.bsprDefaultChart.UseAutoSort = false;

            this._ColIdx_SELECT = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_CODE = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_CODE)];
            this._ColIdx_NAME = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_NAME)];
            
            //OCAP LIST Binding
            LinkedList llconidtion = new LinkedList();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_ATT_CHART);

            DataSet dsControlChart = this._wsSPC.GetATTCodeData(llconidtion.GetSerialData());

            this.bsprDefaultChart.DataSet = dsControlChart;

            //Default로 모든 Chart를 선택
            for (int i = 0; i < bsprDefaultChart.ActiveSheet.RowCount; i++)
            {
                this.bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, true);
                this.bsprDefaultChart.ActiveSheet.Rows[i].BackColor = Color.Yellow;
            }
        }

        private void bsprDefaultChart_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)_ColIdx_SELECT)
                return;

            //SPC-762 by Louisyou
            ArrayList alSelectedRows = this.bsprDefaultChart.GetCheckedList((int)_ColIdx_SELECT);

            for (int idxTmpChartList = 0; idxTmpChartList < this.bsprDefaultChart.ActiveSheet.RowCount; idxTmpChartList++)
            {
                if(alSelectedRows.Contains(idxTmpChartList))
                    this.bsprDefaultChart.ActiveSheet.Rows[idxTmpChartList].BackColor = Color.Yellow;
                else
                    this.bsprDefaultChart.ActiveSheet.Rows[idxTmpChartList].BackColor = Color.White;
            }
        }



        #endregion



        #region :DATA PROPERTY
        //DATA PROPERTY
        public BISTel.eSPC.Common.ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set { _cofingMode = value; }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
        }

        public string SPC_PARAM_CATEGORY_CD
        {
            get
            {
                if (this.bcboParamCategory.SelectedValue != null)
                    return this.bcboParamCategory.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboParamCategory.SelectedValue = value; }
        }

        public string SPC_PRIORITY_CD
        {
            get
            {
                if (this.bcboPriority.SelectedValue != null)
                    return this.bcboPriority.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboPriority.SelectedValue = value; }
        }

        public string AUTO_CPK_YN
        {
            get
            {
                if (this.bchkAutoCpkYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkAutoCpkYN.Checked = true;
                else
                    this.bchkAutoCpkYN.Checked = false;
            }
        }

        public string SAMPLE_COUNT
        {
            get { return this.btxtRestrictSample.Text; }
            set { this.btxtRestrictSample.Text = value; }
        }

        public string DAYS
        {
            get { return this.btxtRestrictDays.Text; }
            set { this.btxtRestrictDays.Text = value; }
        }



        public string DEFAULT_CHART_LIST
        {
            get
            {
                if (bsprDefaultChart.DataSet != null)
                {
                    if (bsprDefaultChart.ActiveSheet.RowCount > 0)
                    {
                        DataSet dsSelectChartList = _SpreadUtil.GetSelectedDataSet(bsprDefaultChart, _ColIdx_SELECT);
                        _sDefaultChartList = _ComUtil.ConvertDataColumnIntoStringList(dsSelectChartList.Tables[0], BISTel.eSPC.Common.COLUMN.CODE, ";");
                    }
                }

                return _sDefaultChartList;
            }
            set
            {
                _sDefaultChartList = value;

                if (_sDefaultChartList != null && _sDefaultChartList.Length > 0)
                {
                    List<string> lstChartList = new List<string>(_sDefaultChartList.Split(';'));

                    string sChartCode = string.Empty;

                    for (int i = 0; i < bsprDefaultChart.ActiveSheet.RowCount; i++)
                    {
                        sChartCode = bsprDefaultChart.GetCellText(i, _ColIdx_CODE);

                        if (lstChartList.Contains(sChartCode))
                        {
                            bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, true);
                            bsprDefaultChart.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                        }
                        else
                        {
                            bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, false);
                            bsprDefaultChart.ActiveSheet.Rows[i].BackColor = Color.White;
                        }
                    }
                }
            }
        }


        public DataTable CONFIG_OPT_DATASET
        {
            get
            {
                if (_dtConfigOPT != null && _dtConfigOPT.Rows.Count > 0)
                    _dtConfigOPT.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST] = this.DEFAULT_CHART_LIST;

                return _dtConfigOPT;
            }
            set
            {
                _dtConfigOPT = value;

                if (_dtConfigOPT != null)
                {
                    //# Data가 없을 경우 Row를 하나 생성한다.
                    if (_dtConfigOPT.Rows.Count.Equals(0))
                    {
                        DataRow newRow = _dtConfigOPT.NewRow();
                        newRow[BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);

                        _dtConfigOPT.Rows.Add(newRow);
                    }

                    //# Control에 DataBinding 하기

                    //SPC_PARAM_CATEGORY_CD
                    this.bcboParamCategory.DataBindings.Add("SelectedValue", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.SPC_PARAM_CATEGORY_CD);

                    //SPC_PRIORITY_CD
                    this.bcboPriority.DataBindings.Add("SelectedValue", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.SPC_PRIORITY_CD);

                    //AUTO_CPK_YN
                    Binding bdAutoCPKYN = new Binding("Checked", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.AUTO_CPK_YN);
                    bdAutoCPKYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoCPKYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCpkYN.DataBindings.Add(bdAutoCPKYN);

                    //RESTRICT_SAMPLE_COUNT
                    this.btxtRestrictSample.DataBindings.Add("Text", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.RESTRICT_SAMPLE_COUNT);

                    //RESTRICT_SAMPLE_DAYS
                    this.btxtRestrictDays.DataBindings.Add("Text", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.RESTRICT_SAMPLE_DAYS);

                    //DEFAULT_CHART_LIST
                    this.DEFAULT_CHART_LIST = _dtConfigOPT.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST].ToString();

                    //RESTRINT_SAMPLE_HOURS
                    this.btxtRestrictHours.DataBindings.Add("Text", _dtConfigOPT, BISTel.eSPC.Common.COLUMN.RESTRICT_SAMPLE_HOURS);
                }
            }
        }

        public void InitializeLayout(BISTel.eSPC.Common.ConfigMode configMode)
        {
            switch (configMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                    break;

                case BISTel.eSPC.Common.ConfigMode.VIEW:
                    this.gbOptionSelection.Enabled = false;
                    this.bcboParamCategory.DropDownStyle = ComboBoxStyle.Simple;
                    this.bcboPriority.DropDownStyle = ComboBoxStyle.Simple;

                    this.gbDefaultChart.Enabled = false;
                    break;
            }
        }

        private void StringYNToBoolean(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType != typeof(Boolean)) return;

            if (cevent.Value.Equals("Y"))
                cevent.Value = true;
            else
                cevent.Value = false;
        }

        private void BooleanToStringYN(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType != typeof(string)) return;

            if (cevent.Value.Equals(true))
                cevent.Value = "Y";
            else
                cevent.Value = "N";
        }


        #endregion

   
        #region ::: User Defined Method.

        public bool InputValidation()
        {
            bool result = true;

            double dDay = 0;
            double dHour = 0;

            if (this.btxtRestrictDays.Text != null && this.btxtRestrictDays.Text.Length > 0)
            {
                dDay = double.Parse(this.btxtRestrictDays.Text);

                if (dDay > 30)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_DAY", null, null);
                    return false;
                }
            }

            if (this.btxtRestrictHours.Text != null && this.btxtRestrictHours.Text.Length > 0)
            {
                dHour = double.Parse(this.btxtRestrictHours.Text);

                if (dHour > 100)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_HOUR", null, null);
                    return false;
                }
            }

            return result;
        }

        #endregion

        #region ::: EventHandler

        #endregion

    }

}
