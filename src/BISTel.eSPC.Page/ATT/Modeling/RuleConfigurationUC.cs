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
    public partial class RuleConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

        private SortedList _slColumnIndex = new SortedList();
        private SortedList _slBtnListIndex = new SortedList();

        List<string> _lstChangedColumnList = new List<string>();

        private int _ColIdx_SELECT = 0;

        private int _ColIdx_RULE_NO;
        private int _ColIdx_RULE_OPTION;
        private int _ColIdx_USE_MASTER;
        private int _ColIdx_USE_MASTER_YN;
        private int _ColIdx_OCAP;
        private int _ColIdx_DESCRIPTION;

        private BISTel.eSPC.Common.ConfigMode _cofingMode;
        private string _sConfigRawID;

        private DataTable _dtConfig;
        private DataTable _dtRuleOpt;

        private int _iSelectedRowIdx = -1;

        public bool _sOCAPOfSIngle = false;
        private bool _bUseComma;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public RuleConfigurationUC()
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
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        private void InitializeCode()
        {
        }


        public void InitializeDataButton()
        {
            this._slBtnListIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprRule, Definition.PAGE_KEY_SPC_CONFIGURATION, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE, this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
        }



        public void InitializeBSpread()
        {
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprRule, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE);
            this.bsprRule.UseHeadColor = true;

            this._ColIdx_SELECT = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_RULE_NO = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RULE_NO)];
            this._ColIdx_RULE_OPTION = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RULE_OPTION)];
            this._ColIdx_USE_MASTER = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_USE_MASTER)];
            this._ColIdx_USE_MASTER_YN = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_USE_MASTER_YN)];
            this._ColIdx_OCAP = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_OCAP)];
            this._ColIdx_DESCRIPTION = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_DESCRIPTION)];
            this.bsprRule.DataSet = this.bsprRule.DataSet;
            this.bsprRule.ActiveSheet.Columns[this._ColIdx_USE_MASTER].Visible = false;
            this.bsprRule.ActiveSheet.Columns[this._ColIdx_USE_MASTER_YN].Visible = false;
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

        public string UPPER_SPEC
        {
            get { return this.btxtPNUpperSpec.Text; }
            set { this.btxtPNUpperSpec.Text = value; }
        }

        public string LOWER_SPEC
        {
            get { return this.btxtPNLowerSpec.Text; }
            set { this.btxtPNLowerSpec.Text = value; }
        }

        public string TARGET
        {
            get { return this.btxtPNTarget.Text; }
            set { this.btxtPNTarget.Text = value; }
        }


        public string UPPER_CONTROL
        {
            get { return this.btxtPUCL.Text; }
            set { this.btxtPUCL.Text = value; }
        }

        public string LOWER_CONTROL
        {
            get { return this.btxtPLCL.Text; }
            set { this.btxtPLCL.Text = value; }
        }

       

        public string CENTER_LINE
        {
            get { return this.btxtPCenter.Text; }
            set { this.btxtPCenter.Text = value; }
        }


        public string UCL_RAW
        {
            get { return this.btxtPNUCL.Text; }
            set { this.btxtPNUCL.Text = value; }
        }

        public string LCL_RAW
        {
            get { return this.btxtPNLCL.Text; }
            set { this.btxtPNLCL.Text = value; }
        }

        public string CENTER_RAW
        {
            get { return this.btxtPNCenter.Text; }
            set { this.btxtPNCenter.Text = value; }
        }

        public string UCL_STD
        {
            get { return this.btxtCUSL.Text; }
            set { this.btxtCUSL.Text = value; }
        }

        public string LCL_STD
        {
            get { return this.btxtCLSL.Text; }
            set { this.btxtCLSL.Text = value; }
        }

        public string CENTER_STD
        {
            get { return this.btxtCenterSTD.Text; }
            set { this.btxtCenterSTD.Text = value; }
        }

        public string UCL_RANGE
        {
            get { return this.btxtCUCL.Text; }
            set { this.btxtCUCL.Text = value; }
        }

        public string LCL_RANGE
        {
            get { return this.btxtCLCL.Text; }
            set { this.btxtCLCL.Text = value; }
        }

        public string CENTER_RANGE
        {
            get { return this.btxtCenterRange.Text; }
            set { this.btxtCenterRange.Text = value; }
        }

        public string UCL_EWMAMEAN
        {
            get { return this.btxtUUCL.Text; }
            set { this.btxtUUCL.Text = value; }
        }

        public string LCL_EWMAMEAN
        {
            get { return this.btxtULCL.Text; }
            set { this.btxtULCL.Text = value; }
        }

        public string CENTER_EWMAMEAN
        {
            get { return this.btxtUCenter.Text; }
            set { this.btxtUCenter.Text = value; }
        }

       

        public DataTable RULE_DATASET
        {
            get
            {
                if (this.bsprRule.DataSet != null)
                {
                    this.bsprRule.LeaveCellAction();
                    return ((DataSet)this.bsprRule.DataSet).Tables[0];
                }
                else
                    return null;
            }
            set
            {
                this.bsprRule.DataSet = value;

                for (int i = 0; i < this.bsprRule.ActiveSheet.RowCount; i++)
                {
                    this.bsprRule.ActiveSheet.Cells[i, _ColIdx_RULE_NO].Note = this.bsprRule.GetCellText(i, _ColIdx_DESCRIPTION);
                }
            }
        }


        public DataTable CONFIG_DATASET
        {
            get { return _dtConfig; }
            set
            {
                _dtConfig = value;

                if (_dtConfig != null)
                {
                    //# Data가 없을 경우 Row를 하나 생성한다.
                    if (_dtConfig.Rows.Count.Equals(0))
                    {
                        DataRow newRow = _dtConfig.NewRow();
                        _dtConfig.Rows.Add(newRow);
                    }

                    //# Control에 DataBinding 하기

                    //UPPER_SPEC
                    this.btxtPNUpperSpec.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC, true, DataSourceUpdateMode.Never);
                    //LOWER_SPEC
                    this.btxtPNLowerSpec.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC, true, DataSourceUpdateMode.Never);

                    //TARGET
                    this.btxtPNTarget.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_TARGET, true, DataSourceUpdateMode.Never);

                    //UPPER_CONTROL
                    this.btxtPUCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.P_UCL, true, DataSourceUpdateMode.Never);
                    //LOWER_CONTROL
                    this.btxtPLCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.P_LCL, true, DataSourceUpdateMode.Never);

                    //CENTER_LINE
                    this.btxtPCenter.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.P_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //EWMA_LAMBDA

                    //UCL_RAW
                    this.btxtPNUCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_RAW
                    this.btxtPNLCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_RAW
                    this.btxtPNCenter.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_STD
                    this.btxtCUSL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_UPPERSPEC, true, DataSourceUpdateMode.Never);
                    //LCL_STD
                    this.btxtCLSL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_LOWERSPEC, true, DataSourceUpdateMode.Never);
                    //CENTER_STD
                    this.btxtCenterSTD.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_TARGET, true, DataSourceUpdateMode.Never);

                    //UCL_RANGE
                    this.btxtCUCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_RANGE
                    this.btxtCLCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_RANGE
                    this.btxtCenterRange.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_EWMA_M
                    this.btxtUUCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.U_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_EWMA_M
                    this.btxtULCL.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.U_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_EWMA_M
                    this.btxtUCenter.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.U_CENTER_LINE, true, DataSourceUpdateMode.Never);
                    //SPEC_UCL_OFFSET
                    this.btx_PNSpecUpperOffer.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.SPEC_PN_USL_OFFSET, true, DataSourceUpdateMode.Never);
                    //SPEC_LCL_OFFSET
                    this.btx_PNSpecLowerOffer.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.SPEC_PN_LSL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////SPEC_OFFSET_YN

                    //PN_UCL_OFFSET
                    this.btx_PNUCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //PN_LCL_OFFSET
                    this.btx_PNLCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PN_LCL_OFFSET, true, DataSourceUpdateMode.Never);

                    //P_UCL_OFFSET
                    this.btx_PUCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.P_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //P_LCL_OFFSET
                    this.btx_PLCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.P_LCL_OFFSET, true, DataSourceUpdateMode.Never);

                    //C_USL_OFFSET
                    this.btx_CUSLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.SPEC_C_USL_OFFSET, true, DataSourceUpdateMode.Never);
                    //C_LSL_OFFSET
                    this.btx_CLSLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.SPEC_C_LSL_OFFSET, true, DataSourceUpdateMode.Never);

                    //C_UCL_OFFSET
                    this.btx_CUCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //C_LCL_OFFSET
                    this.btx_CLCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.C_LCL_OFFSET, true, DataSourceUpdateMode.Never);

                    //U_UCL_OFFSET
                    this.btx_UUCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.U_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //U_LCL_OFFSET
                    this.btx_ULCLOffset.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.U_LCL_OFFSET, true, DataSourceUpdateMode.Never);

                    //OFFSET_YN
                    Binding bdUseYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.OFFSET_YN);
                    bdUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkbox_OffsetYN.DataBindings.Add(bdUseYN);

                }
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
                    this.gbPNSpecLimit.Enabled = false;
                    this.gbPNControlLimit.Enabled = false;
                    this.gbPControlLimit.Enabled = false;
                    this.gbCSpecLimit.Enabled = false;
                    this.gbCControlLimit.Enabled = false;
                    this.gbUControlLimit.Enabled = false;                   
                    this.gbRuleSelection.Enabled = false;
                    this.bbtnList.Visible = false;

                    break;
            }
        }

        public DataTable RULE_OPT_DATASET
        {
            get { return this._dtRuleOpt; }
            set { this._dtRuleOpt = value; }
        }


        public Boolean SPEC_LIMIT_ENABLED
        {
            set
            {
                this.btxtPNUpperSpec.Enabled = value;
                this.btxtPNLowerSpec.Enabled = value;
                this.btxtPNTarget.Enabled = value;
            }
        }

        public List<string> CHANGED_MASTER_COL_LIST
        {
            get { return _lstChangedColumnList; }
        }

        private void SetConfigRow(string argColumn, BTextBox bTxtBox)
        {
            if (bTxtBox.Text.Length > 0)
            {
                try
                {
                    _dtConfig.Rows[0][argColumn] = double.Parse(bTxtBox.Text);
                }
                catch
                {
                    _dtConfig.Rows[0][argColumn] = DBNull.Value;
                    bTxtBox.Text = null;
                }
            }
            else
                _dtConfig.Rows[0][argColumn] = DBNull.Value;
        }        

        #endregion

        #region ::: EventHandler

        private void bsprRule_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == (int)_ColIdx_SELECT)
            {
                if ((bool)this.bsprRule.GetCellValue(e.Row, (int)_ColIdx_SELECT) == true)
                {
                    for (int i = 0; i < this.bsprRule.ActiveSheet.RowCount; i++)
                    {
                        if (e.Row == i)
                        {
                            if (e.Column != (int)_ColIdx_SELECT)
                                this.bsprRule.ActiveSheet.Cells[i, (int)_ColIdx_SELECT].Value = 1;

                            this._iSelectedRowIdx = e.Row;

                            continue;
                        }

                        this.bsprRule.ActiveSheet.Cells[i, (int)_ColIdx_SELECT].Value = 0;
                    }
                }
                else
                    this._iSelectedRowIdx = -1;
            }
            else if (e.Column == (int)_ColIdx_USE_MASTER)
            {
                if ((bool)this.bsprRule.GetCellValue(e.Row, (int)_ColIdx_USE_MASTER) == true)
                {
                    this.bsprRule.SetCellValue(e.Row, (int)_ColIdx_USE_MASTER_YN, "Y");
                }
                else
                {
                    this.bsprRule.SetCellValue(e.Row, (int)_ColIdx_USE_MASTER_YN, "N");
                }
            }
        }


        private void bbtnList_ButtonClick(string name)
        {
            if (name.ToUpper().Equals(Definition.BUTTON_KEY_ADD_PROP))
            {
                SPCRuleMasterPopup spcRuleMstPopup = new SPCRuleMasterPopup();
                
                spcRuleMstPopup._sOCAPOfSingle = this._sOCAPOfSIngle;
                spcRuleMstPopup.SESSIONDATA = this.sessionData;
                spcRuleMstPopup.URL = this.URL;
                spcRuleMstPopup.PORT = this.Port;
                spcRuleMstPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_SUB;
                spcRuleMstPopup.CONFIG_RAWID = _sConfigRawID;
                spcRuleMstPopup.SPCRULE_DATATABLE = ((DataSet)this.bsprRule.DataSet).Tables[0];
                spcRuleMstPopup.SPCRULEOPT_DATATABLE = this._dtRuleOpt;
                spcRuleMstPopup.USE_COMMA = _bUseComma;
                spcRuleMstPopup.InitializePopup();
                DialogResult dResult = spcRuleMstPopup.ShowDialog();

                if (dResult == DialogResult.OK)
                {
                    for (int i = 0; i < this.bsprRule.ActiveSheet.RowCount; i++)
                    {
                        this.bsprRule.ActiveSheet.Cells[i, _ColIdx_RULE_NO].Note = this.bsprRule.GetCellText(i, _ColIdx_DESCRIPTION);
                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_MODIFY))
            {
                if (_iSelectedRowIdx < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_SELECT_RULE));
                    return;
                }

                SPCRuleMasterPopup spcRuleMstPopup = new SPCRuleMasterPopup();

                spcRuleMstPopup._sOCAPOfSingle = this._sOCAPOfSIngle;
                spcRuleMstPopup.SESSIONDATA = this.sessionData;
                spcRuleMstPopup.URL = this.URL;
                spcRuleMstPopup.PORT = this.Port;
                spcRuleMstPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.MODIFY;
                spcRuleMstPopup.CONFIG_RAWID = _sConfigRawID;
                spcRuleMstPopup.RULE_NO = this.bsprRule.GetCellText(_iSelectedRowIdx, _ColIdx_RULE_NO);
                spcRuleMstPopup.SPCRULE_DATATABLE = ((DataSet)this.bsprRule.DataSet).Tables[0];
                spcRuleMstPopup.SPCRULEOPT_DATATABLE = this._dtRuleOpt;
                spcRuleMstPopup.InitializePopup();
                DialogResult dResult = spcRuleMstPopup.ShowDialog();

                if (dResult == DialogResult.OK)
                {

                }

            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_REMOVE))
            {


                if (bsprRule.ActiveSheet.RowCount > 0)
                {
                    if (_iSelectedRowIdx < 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                        return;
                    }

                    string sSelectRuleNO = bsprRule.GetCellText(bsprRule.ActiveSheet.ActiveRowIndex, _ColIdx_RULE_NO);

                    //#01. RULE 삭제
                    bsprRule.RemoveRow(bsprRule.ActiveSheet.ActiveRowIndex, bsprRule.ActiveSheet.ActiveRowIndex);

                    //#02. RULE Option 삭제
                    DataRow[] drSelectRuleOPTs = this._dtRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.SPC_RULE_NO, sSelectRuleNO));

                    for (int i = 0; i < drSelectRuleOPTs.Length; i++)
                    {
                        drSelectRuleOPTs[i].Delete();
                    }

                    _iSelectedRowIdx = -1;
                }
            }
        }

        private void btxtSpec_Validated(object sender, EventArgs e)
        {
            BTextBox btxtMst = (BTextBox)sender;


            double iUpper = 0;
            double iLower = 0;

            if (btxtMst.Name.Equals(this.btxtPNUpperSpec.Name) || btxtMst.Name.Equals(this.btxtPNLowerSpec.Name))
            {
                if (btxtMst.Name.Equals(this.btxtPNUpperSpec.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtPNLowerSpec.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC, btxtMst);

                if (this.btxtPNUpperSpec.Text.Length > 0 && this.btxtPNLowerSpec.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtPNUpperSpec.Text) ? 0 : double.Parse(this.btxtPNUpperSpec.Text);
                    iLower = string.IsNullOrEmpty(this.btxtPNLowerSpec.Text) ? 0 : double.Parse(this.btxtPNLowerSpec.Text);

                    this.btxtPNTarget.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.PN_TARGET] = double.Parse(this.btxtPNTarget.Text);
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.PN_TARGET] = DBNull.Value;
                    this.btxtPNTarget.Text = null;
                }

                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_TARGET);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC);
            }
            else if (btxtMst.Name.Equals(this.btxtPUCL.Name) || btxtMst.Name.Equals(this.btxtPLCL.Name))
            {
                //추가된 부분(Create Model 한 뒤에 Rule Tab에서 Control Limit을 입력한 후 다른 작업을 하면 입력한 control limit 값이 사라지는 현상수정)
                if (btxtMst.Name.Equals(this.btxtPUCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.P_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtPLCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.P_LCL, btxtMst);
                //추가 완료

                if (this.btxtPUCL.Text.Length > 0 && this.btxtPLCL.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtPUCL.Text) ? 0 : double.Parse(this.btxtPUCL.Text);
                    iLower = string.IsNullOrEmpty(this.btxtPLCL.Text) ? 0 : double.Parse(this.btxtPLCL.Text);
                    this.btxtPCenter.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.P_CENTER_LINE] = double.Parse(this.btxtPCenter.Text);                    
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.P_CENTER_LINE] = DBNull.Value;
                    this.btxtPCenter.Text = null;
                  
                }


                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_UCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_LCL);
            }
            else if (btxtMst.Name.Equals(this.btxtPNUCL.Name) || btxtMst.Name.Equals(this.btxtPNLCL.Name))
            {
                if (btxtMst.Name.Equals(this.btxtPNUCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtPNLCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_LCL, btxtMst);

                if (this.btxtPNUCL.Text.Length > 0 && this.btxtPNLCL.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtPNUCL.Text) ? 0 : double.Parse(this.btxtPNUCL.Text);
                    iLower = string.IsNullOrEmpty(this.btxtPNLCL.Text) ? 0 : double.Parse(this.btxtPNLCL.Text);
                    this.btxtPNCenter.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE] = double.Parse(this.btxtPNCenter.Text);
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE] = DBNull.Value;
                    this.btxtPNCenter.Text = null;
                }


                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_UCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_LCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtCUSL.Name) || btxtMst.Name.Equals(this.btxtCLSL.Name))
            {
                if (btxtMst.Name.Equals(this.btxtCUSL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtCLSL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC, btxtMst);

                if (this.btxtCUSL.Text.Length > 0 && this.btxtCLSL.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtCUSL.Text) ? 0 : double.Parse(this.btxtCUSL.Text);
                    iLower = string.IsNullOrEmpty(this.btxtCLSL.Text) ? 0 : double.Parse(this.btxtCLSL.Text);
                    this.btxtCenterSTD.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.C_TARGET] = double.Parse(this.btxtCenterSTD.Text);
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.C_TARGET] = DBNull.Value;
                    this.btxtCenterSTD.Text = null;
                }


                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_TARGET);
            }
            else if (btxtMst.Name.Equals(this.btxtCUCL.Name) || btxtMst.Name.Equals(this.btxtCLCL.Name))
            {
                if (btxtMst.Name.Equals(this.btxtCUCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtCLCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_LCL, btxtMst);

                if (this.btxtCUCL.Text.Length > 0 && this.btxtCLCL.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtCUCL.Text) ? 0 : double.Parse(this.btxtCUCL.Text);
                    iLower = string.IsNullOrEmpty(this.btxtCLCL.Text) ? 0 : double.Parse(this.btxtCLCL.Text);
                    this.btxtCenterRange.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.C_CENTER_LINE] = double.Parse(this.btxtCenterRange.Text);
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.C_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterRange.Text = null;
                }


                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_UCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_LCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUUCL.Name) || btxtMst.Name.Equals(this.btxtULCL.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUUCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.U_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtULCL.Name))
                    this.SetConfigRow(BISTel.eSPC.Common.COLUMN.U_LCL, btxtMst);

                if (this.btxtUUCL.Text.Length > 0 && this.btxtULCL.Text.Length > 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUUCL.Text) ? 0 : double.Parse(this.btxtUUCL.Text);
                    iLower = string.IsNullOrEmpty(this.btxtULCL.Text) ? 0 : double.Parse(this.btxtULCL.Text);
                    this.btxtUCenter.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.U_CENTER_LINE] = double.Parse(this.btxtUCenter.Text);
                }
                else
                {
                    _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.U_CENTER_LINE] = DBNull.Value;
                    this.btxtUCenter.Text = null;
                }


                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.U_UCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.U_LCL);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.U_CENTER_LINE);
            }
            
            else if (btxtMst.Name.Equals(this.btxtPNTarget.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_TARGET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_TARGET);
            }
            
            else if (btxtMst.Name.Equals(btxtPCenter.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE);
            }            
            

            else if (btxtMst.Name.Equals(btx_PNSpecUpperOffer.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_USL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_USL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_PNSpecLowerOffer.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_LSL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_LSL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_PNUCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_PNLCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.PN_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.PN_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_PUCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.P_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_PLCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.P_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.P_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_CUSLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_USL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_USL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_CLSLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_LSL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_LSL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_CUCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_CLCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.C_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.C_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_UUCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.U_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.U_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_ULCLOffset.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.U_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(BISTel.eSPC.Common.COLUMN.U_LCL_OFFSET);
            }           
           

        }
        #endregion


        #region ::: User Defined Method.

        public void UpdateChangedLColumnList(string columnName)
        {
            if (_dtConfig.Rows[0].RowState.Equals(DataRowState.Added))
                return;

            if (_dtConfig.Rows[0][columnName, DataRowVersion.Current].Equals(_dtConfig.Rows[0][columnName, DataRowVersion.Default]))
            {
                if (_lstChangedColumnList.Contains(columnName))
                    _lstChangedColumnList.Remove(columnName);
            }
            else if (!_lstChangedColumnList.Contains(columnName))
            {
                _lstChangedColumnList.Add(columnName);
            }
        }

        public void SetCheckBoxColumn()
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType cbcT = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            if (this.bsprRule != null && this.bsprRule.ActiveSheet.Rows.Count > 0)
            {
                this.bsprRule.ActiveSheet.Columns[this._ColIdx_USE_MASTER].CellType = cbcT;
            }
        }
        #endregion

        public bool InputValidation()
        {
            bool result = true;

            double dUpper = 0;
            double dLower = 0;

            #region PN usl & ucl
                      
            if (this.btxtPNUpperSpec.Text.Length > 0 && this.btxtPNLowerSpec.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtPNUpperSpec.Text);
                dLower = double.Parse(this.btxtPNLowerSpec.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {

                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_PN_VALUE", new string[] {"USL", "LSL"}, null);
                        return false;
                    }
                }
            }

            if (this.btxtPNUpperSpec.Text.Length > 0 && this.btxtPNUCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtPNUpperSpec.Text);
                dLower = double.Parse(this.btxtPNUCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_PN_VALUE", new string[] { "USL", "UCL" }, null);
                        return false;
                    }
                }
            }

            if (this.btxtPNLowerSpec.Text.Length > 0 && this.btxtPNLCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtPNLowerSpec.Text);
                dLower = double.Parse(this.btxtPNLCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper > dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_PN_VALUE", new string[] { "LCL", "LSL" }, null);
                        return false;
                    }
                }
            }

            if (this.btxtPNUCL.Text.Length > 0 && this.btxtPNLCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtPNUCL.Text);
                dLower = double.Parse(this.btxtPNLCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_PN_VALUE", new string[] { "UCL", "LCL" }, null);
                        return false;
                    }
                }
            }

            #endregion

            #region P lsl & lcl

            if (this.btxtPUCL.Text.Length > 0 && this.btxtPLCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtPUCL.Text);
                dLower = double.Parse(this.btxtPLCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_P_VALUE", new string[] {"UCL", "LSL"}, null);
                        return false;
                    }
                }
            }
           
            #endregion

            #region C usl & ucl
            if (this.btxtCUSL.Text.Length > 0 && this.btxtCLSL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtCUSL.Text);
                dLower = double.Parse(this.btxtCLSL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_C_VALUE", new string[] { "USL", "LSL" }, null);
                        return false;
                    }
                }
            }

            if (this.btxtCUSL.Text.Length > 0 && this.btxtCUCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtCUSL.Text);
                dLower = double.Parse(this.btxtCUCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_C_VALUE", new string[] { "USL", "UCL" }, null);
                        return false;
                    }
                }
            }

            if (this.btxtCLSL.Text.Length > 0 && this.btxtCLCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtCLSL.Text);
                dLower = double.Parse(this.btxtCUCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper > dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_C_VALUE", new string[] { "UCL", "LSL" }, null);
                        return false;
                    }
                }
            }

            if (this.btxtCUCL.Text.Length > 0 && this.btxtCLCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtCUCL.Text);
                dLower = double.Parse(this.btxtCLCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_C_VALUE", new string[] { "UCL", "LCL" }, null);
                        return false;
                    }
                }
            }

            #endregion

            #region U Control Limit

            if (this.btxtUUCL.Text.Length > 0 && this.btxtULCL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUUCL.Text);
                dLower = double.Parse(this.btxtULCL.Text);

                if (dUpper < 0 || dLower < 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO", null, null);
                    return false;
                }
                else
                {
                    if (dUpper < dLower)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_U_VALUE", new string[] { "UCL", "LCL" }, null);
                        return false;
                    }
                }
            }
            #endregion

            return result;
        }

        private void bchkbox_OffsetYN_CheckedChanged(object sender, EventArgs e)
        {
            if (!bchkbox_OffsetYN.Checked)
            {
                btx_PNSpecUpperOffer.ReadOnly = true;
                btx_PNSpecLowerOffer.ReadOnly = true;
                btx_PNUCLOffset.ReadOnly = true;
                btx_PNLCLOffset.ReadOnly = true;                
                btx_CUCLOffset.ReadOnly = true;
                btx_CLCLOffset.ReadOnly = true;
                btx_CUSLOffset.ReadOnly = true;
                btx_CLSLOffset.ReadOnly = true;
                btx_PUCLOffset.ReadOnly = true;
                btx_PLCLOffset.ReadOnly = true;
                btx_UUCLOffset.ReadOnly = true;
                btx_ULCLOffset.ReadOnly = true;
            }
            else
            {
                btx_PNSpecUpperOffer.ReadOnly = false;
                btx_PNSpecLowerOffer.ReadOnly = false;
                btx_PNUCLOffset.ReadOnly = false;
                btx_PNLCLOffset.ReadOnly = false;
                btx_CUCLOffset.ReadOnly = false;
                btx_CLCLOffset.ReadOnly = false;
                btx_CUSLOffset.ReadOnly = false;
                btx_CLSLOffset.ReadOnly = false;
                btx_PUCLOffset.ReadOnly = false;
                btx_PLCLOffset.ReadOnly = false;
                btx_UUCLOffset.ReadOnly = false;
                btx_ULCLOffset.ReadOnly = false;
            }
        }

    }

}
