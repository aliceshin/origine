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

using BISTel.eSPC.Common;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class RuleConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

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

        private ConfigMode _cofingMode;
        private string _sConfigRawID;

        private DataTable _dtConfig;
        private DataTable _dtRuleOpt;

        private int _iSelectedRowIdx = -1;

        public bool _sOCAPOfSIngle = false;
        private bool _bUseComma = false;

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
            //BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            //this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }

        //public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        //{
        //    return new BISTel.eSPC.Condition.Modeling.SPCModelCondition();			
        //}

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
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        private void InitializeCode()
        {
            //LinkedList lk = new LinkedList();
            //lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_FAULT_LEVEL);

            //DataSet _dsFaultLevelCode = this._wsSPC.GetCodeData(lk.GetSerialData());
            //this._faultlevelnames = _ComUtil.convertdatacolumnintoarray(_dsfaultlevelcode.tables[0], "name");
            //this._faultlevelcodes = _ComUtil.convertdatacolumnintoarray(_dsfaultlevelcode.tables[0], "code");
        }


        public void InitializeDataButton()
        {
            this._slBtnListIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprRule, Definition.PAGE_KEY_SPC_CONFIGURATION, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE, this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
            //this.ApplyAuthory(this.bbtnList);
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

            //this._Initialization.SetCheckColumnHeader(this.bsprRule, 0);

            this.bsprRule.DataSet = this.bsprRule.DataSet;

            this.bsprRule.ActiveSheet.Columns[this._ColIdx_USE_MASTER].Visible = false;
            this.bsprRule.ActiveSheet.Columns[this._ColIdx_USE_MASTER_YN].Visible = false;

            this.bsprRule.SelectionBlockOptions = FarPoint.Win.Spread.SelectionBlockOptions.None;
        }





        #endregion




        


        #region :DATA PROPERTY
        //DATA PROPERTY
        public ConfigMode CONFIG_MODE
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
            get { return this.btxtUpperSpec.Text; }
            set { this.btxtUpperSpec.Text = value; }
        }

        public string LOWER_SPEC
        {
            get { return this.btxtLowerSpec.Text; }
            set { this.btxtLowerSpec.Text = value; }
        }

        public string TARGET
        {
            get { return this.btxtTarget.Text; }
            set { this.btxtTarget.Text = value; }
        }


        public string UPPER_CONTROL
        {
            get { return this.btxtUCLMean.Text; }
            set { this.btxtUCLMean.Text = value; }
        }

        public string LOWER_CONTROL
        {
            get { return this.btxtLCLMean.Text; }
            set { this.btxtLCLMean.Text = value; }
        }

        public string EWMA_LAMBDA
        {
            get { return this.btxtEWMALambda.Text; }
            set { this.btxtEWMALambda.Text = value; }
        }

        public string MOVING_COUNT
        {
            get { return this.btxtMovingCount.Text; }
            set { this.btxtMovingCount.Text = value; }
        }

        public string CENTER_LINE
        {
            get { return this.btxtCenterMean.Text; }
            set { this.btxtCenterMean.Text = value; }
        }

        public string STD
        {
            get { return this.btxtSTD.Text; }
            set { this.btxtSTD.Text = value; }
        }

        public string UCL_RAW
        {
            get { return this.btxtUCLRaw.Text; }
            set { this.btxtUCLRaw.Text = value; }
        }

        public string LCL_RAW
        {
            get { return this.btxtLCLRaw.Text; }
            set { this.btxtLCLRaw.Text = value; }
        }

        public string CENTER_RAW
        {
            get { return this.btxtCenterRaw.Text; }
            set { this.btxtCenterRaw.Text = value; }
        }

        public string UCL_STD
        {
            get { return this.btxtUCLSTD.Text; }
            set { this.btxtUCLSTD.Text = value; }
        }

        public string LCL_STD
        {
            get { return this.btxtLCLSTD.Text; }
            set { this.btxtLCLSTD.Text = value; }
        }

        public string CENTER_STD
        {
            get { return this.btxtCenterSTD.Text; }
            set { this.btxtCenterSTD.Text = value; }
        }

        public string UCL_RANGE
        {
            get { return this.btxtUCLRange.Text; }
            set { this.btxtUCLRange.Text = value; }
        }

        public string LCL_RANGE
        {
            get { return this.btxtLCLRange.Text; }
            set { this.btxtLCLRange.Text = value; }
        }

        public string CENTER_RANGE
        {
            get { return this.btxtCenterRange.Text; }
            set { this.btxtCenterRange.Text = value; }
        }

        public string UCL_EWMAMEAN
        {
            get { return this.btxtUCLEWMAMean.Text; }
            set { this.btxtUCLEWMAMean.Text = value; }
        }

        public string LCL_EWMAMEAN
        {
            get { return this.btxtLCLEWMAMean.Text; }
            set { this.btxtLCLEWMAMean.Text = value; }
        }

        public string CENTER_EWMAMEAN
        {
            get { return this.btxtCenterEWMAMean.Text; }
            set { this.btxtCenterEWMAMean.Text = value; }
        }

        public string UCL_EWMARANGE
        {
            get { return this.btxtUCLEWMARange.Text; }
            set { this.btxtUCLEWMARange.Text = value; }
        }

        public string LCL_EWMARANGE
        {
            get { return this.btxtLCLEWMARange.Text; }
            set { this.btxtLCLEWMARange.Text = value; }
        }

        public string CENTER_EWMARANGE
        {
            get { return this.btxtCenterEWMARange.Text; }
            set { this.btxtCenterEWMARange.Text = value; }
        }

        public string UCL_EWMASTD
        {
            get { return this.btxtUCLEWMASTD.Text; }
            set { this.btxtUCLEWMASTD.Text = value; }
        }

        public string LCL_EWMASTD
        {
            get { return this.btxtLCLEWMASTD.Text; }
            set { this.btxtLCLEWMASTD.Text = value; }
        }

        public string CENTER_EWMASTD
        {
            get { return this.btxtCenterEWMASTD.Text; }
            set { this.btxtCenterEWMASTD.Text = value; }
        }

        public string UCL_MA
        {
            get { return this.btxtUCLMA.Text; }
            set { this.btxtUCLMA.Text = value; }
        }

        public string LCL_MA
        {
            get { return this.btxtLCLMA.Text; }
            set { this.btxtLCLMA.Text = value; }
        }

        public string CENTER_MA
        {
            get { return this.btxtCenterMA.Text; }
            set { this.btxtCenterMA.Text = value; }
        }

        public string UCL_MS
        {
            get { return this.btxtUCLMS.Text; }
            set { this.btxtUCLMS.Text = value; }
        }

        public string LCL_MS
        {
            get { return this.btxtLCLMS.Text; }
            set { this.btxtLCLMS.Text = value; }
        }

        public string CENTER_MS
        {
            get { return this.btxtCenterMS.Text; }
            set { this.btxtCenterMS.Text = value; }
        }

        public string UCL_MR
        {
            get { return this.btxtUCLMR.Text; }
            set { this.btxtUCLMR.Text = value; }
        }

        public string LCL_MR
        {
            get { return this.btxtLCLMR.Text; }
            set { this.btxtLCLMR.Text = value; }
        }

        public string CENTER_MR
        {
            get { return this.btxtCenterMR.Text; }
            set { this.btxtCenterMR.Text = value; }
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

                    //# Default 값을 입력
                    if (_dtConfig.Rows[0][COLUMN.EWMA_LAMBDA].Equals(DBNull.Value))
                        _dtConfig.Rows[0][COLUMN.EWMA_LAMBDA] = "0.25";

                    if (_dtConfig.Rows[0][COLUMN.MOVING_COUNT].Equals(DBNull.Value))
                        _dtConfig.Rows[0][COLUMN.MOVING_COUNT] = "10";

                    //# Control에 DataBinding 하기

                    //UPPER_SPEC
                    this.btxtUpperSpec.DataBindings.Add("Text", _dtConfig, COLUMN.UPPER_SPEC, true, DataSourceUpdateMode.Never);
                    //LOWER_SPEC
                    this.btxtLowerSpec.DataBindings.Add("Text", _dtConfig, COLUMN.LOWER_SPEC, true, DataSourceUpdateMode.Never);

                    //TARGET
                    this.btxtTarget.DataBindings.Add("Text", _dtConfig, COLUMN.TARGET, true, DataSourceUpdateMode.Never);

                    //UPPER_CONTROL
                    this.btxtUCLMean.DataBindings.Add("Text", _dtConfig, COLUMN.UPPER_CONTROL, true, DataSourceUpdateMode.Never);
                    //LOWER_CONTROL
                    this.btxtLCLMean.DataBindings.Add("Text", _dtConfig, COLUMN.LOWER_CONTROL, true, DataSourceUpdateMode.Never);

                    //CENTER_LINE
                    this.btxtCenterMean.DataBindings.Add("Text", _dtConfig, COLUMN.CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //EWMA_LAMBDA
                    this.btxtEWMALambda.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_LAMBDA);
                    //MOVING_COUNT
                    this.btxtMovingCount.DataBindings.Add("Text", _dtConfig, COLUMN.MOVING_COUNT);

                    //STD
                    this.btxtSTD.DataBindings.Add("Text", _dtConfig, COLUMN.STD, true, DataSourceUpdateMode.Never);

                    //UCL_RAW
                    this.btxtUCLRaw.DataBindings.Add("Text", _dtConfig, COLUMN.RAW_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_RAW
                    this.btxtLCLRaw.DataBindings.Add("Text", _dtConfig, COLUMN.RAW_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_RAW
                    this.btxtCenterRaw.DataBindings.Add("Text", _dtConfig, COLUMN.RAW_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_STD
                    this.btxtUCLSTD.DataBindings.Add("Text", _dtConfig, COLUMN.STD_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_STD
                    this.btxtLCLSTD.DataBindings.Add("Text", _dtConfig, COLUMN.STD_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_STD
                    this.btxtCenterSTD.DataBindings.Add("Text", _dtConfig, COLUMN.STD_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_RANGE
                    this.btxtUCLRange.DataBindings.Add("Text", _dtConfig, COLUMN.RANGE_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_RANGE
                    this.btxtLCLRange.DataBindings.Add("Text", _dtConfig, COLUMN.RANGE_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_RANGE
                    this.btxtCenterRange.DataBindings.Add("Text", _dtConfig, COLUMN.RANGE_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_EWMA_M
                    this.btxtUCLEWMAMean.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_M_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_EWMA_M
                    this.btxtLCLEWMAMean.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_M_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_EWMA_M
                    this.btxtCenterEWMAMean.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_M_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_EWMA_R
                    this.btxtUCLEWMARange.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_R_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_EWMA_R
                    this.btxtLCLEWMARange.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_R_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_EWMA_R
                    this.btxtCenterEWMARange.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_R_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_EWMA_S
                    this.btxtUCLEWMASTD.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_S_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_EWMA_S
                    this.btxtLCLEWMASTD.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_S_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_EWMA_S
                    this.btxtCenterEWMASTD.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_S_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_MA
                    this.btxtUCLMA.DataBindings.Add("Text", _dtConfig, COLUMN.MA_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_MA
                    this.btxtLCLMA.DataBindings.Add("Text", _dtConfig, COLUMN.MA_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_MA
                    this.btxtCenterMA.DataBindings.Add("Text", _dtConfig, COLUMN.MA_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_MS
                    this.btxtUCLMS.DataBindings.Add("Text", _dtConfig, COLUMN.MS_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_MS
                    this.btxtLCLMS.DataBindings.Add("Text", _dtConfig, COLUMN.MS_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_MS
                    this.btxtCenterMS.DataBindings.Add("Text", _dtConfig, COLUMN.MS_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_MR
                    this.btxtUCLMR.DataBindings.Add("Text", _dtConfig, COLUMN.MR_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_MR
                    this.btxtLCLMR.DataBindings.Add("Text", _dtConfig, COLUMN.MR_LCL, true, DataSourceUpdateMode.Never);
                    //CENTER_MR
                    this.btxtCenterMR.DataBindings.Add("Text", _dtConfig, COLUMN.MR_CENTER_LINE, true, DataSourceUpdateMode.Never);

                    //UCL_ZONEA
                    this.btxtUCLZoneA.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_A_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_ZONEA
                    this.btxtLCLZoneA.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_A_LCL, true, DataSourceUpdateMode.Never);

                    //UCL_ZONEB
                    this.btxtUCLZoneB.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_B_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_ZONEB
                    this.btxtLCLZoneB.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_B_LCL, true, DataSourceUpdateMode.Never);

                    //UCL_ZONEC
                    this.btxtUCLZoneC.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_C_UCL, true, DataSourceUpdateMode.Never);
                    //LCL_ZONEC
                    this.btxtLCLZoneC.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_C_LCL, true, DataSourceUpdateMode.Never);


                    /*----------------------------------- Chris : 2010-06-18 ------------------------------------------------------------------------*/

                    //SPEC_UCL_OFFSET
                    this.btx_SpecUpperOffer.DataBindings.Add("Text", _dtConfig, COLUMN.SPEC_USL_OFFSET, true, DataSourceUpdateMode.Never);
                    //SPEC_LCL_OFFSET
                    this.btx_SpecLowerOffer.DataBindings.Add("Text", _dtConfig, COLUMN.SPEC_LSL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////SPEC_OFFSET_YN
                    //Binding bdSpecOffsetYN = new Binding("Checked", _dtConfig, COLUMN.SPEC_OFFSET_YN);
                    //bdSpecOffsetYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdSpecOffsetYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_OffsetYN.DataBindings.Add(bdSpecOffsetYN);

                    //RAW_UCL_OFFSET
                    this.btx_RawUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.RAW_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //RAW_LCL_OFFSET
                    this.btx_RawLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.RAW_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////RAW_OFFSET_YN
                    //Binding bdRawOffsetYN = new Binding("Checked", _dtConfig, COLUMN.RAW_OFFSET_YN);
                    //bdRawOffsetYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdRawOffsetYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_RawOffsetYN.DataBindings.Add(bdRawOffsetYN);

                    //MEAN_UCL_OFFSET
                    this.btx_MeanUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MEAN_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //MEAN_LCL_OFFSET
                    this.btx_MeanLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MEAN_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////MEAN_LCL_OFFSET
                    //Binding bdMeanOffsetYN = new Binding("Checked", _dtConfig, COLUMN.MEAN_OFFSET_YN);
                    //bdMeanOffsetYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMeanOffsetYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_MeanOffsetYN.DataBindings.Add(bdMeanOffsetYN);

                    //STD_UCL_OFFSET
                    this.btx_STDUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.STD_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //STD_LCL_OFFSET
                    this.btx_STDLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.STD_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////STD_OFFSET_YN
                    //Binding bdSTDOffsetYN = new Binding("Checked", _dtConfig, COLUMN.STD_OFFSET_YN);
                    //bdSTDOffsetYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdSTDOffsetYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_STDOffsetYN.DataBindings.Add(bdSTDOffsetYN);



                    //RANGE_UCL_OFFSET
                    this.btx_RangeUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.RANGE_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //RANGE_LCL_OFFSET
                    this.btx_RangeLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.RANGE_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////RANGE_OFFSET_YN
                    //Binding bdRangeOffsetYN = new Binding("Checked", _dtConfig, COLUMN.RANGE_OFFSET_YN);
                    //bdRangeOffsetYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdRangeOffsetYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_RangeOffsetYN.DataBindings.Add(bdRangeOffsetYN);


                    //EWMA_M_UCL_OFFSET
                    this.btx_EWMAMeanUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_M_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //EWMA_M_LCL_OFFSET
                    this.btx_EWMAMeanLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_M_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////EWMA_M_OFFSET_YN
                    //Binding bdEWMAMeanUseYN = new Binding("Checked", _dtConfig, COLUMN.EWMA_M_OFFSET_YN);
                    //bdEWMAMeanUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMAMeanUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_EWMAMeanUseYN.DataBindings.Add(bdEWMAMeanUseYN);

                    //EWMA_S_UCL_OFFSET
                    this.btx_EWMASTDUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_S_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //EWMA_S_LCL_OFFSET
                    this.btx_EWMASTDLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_S_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////EWMA_S_OFFSET_YN
                    //Binding bdEWMASTDUseYN = new Binding("Checked", _dtConfig, COLUMN.EWMA_S_OFFSET_YN);
                    //bdEWMASTDUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMASTDUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_EWMASTDUseYN.DataBindings.Add(bdEWMASTDUseYN);

                    //EWMA_R_UCL_OFFSET
                    this.btx_EWMARangeUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_R_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //EWMA_R_LCL_OFFSET
                    this.btx_EWMARangeLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.EWMA_R_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////EWMA_R_OFFSET_YN
                    //Binding bdEWMARangeUseYN = new Binding("Checked", _dtConfig, COLUMN.EWMA_R_OFFSET_YN);
                    //bdEWMARangeUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMARangeUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_EWMARangeUseYN.DataBindings.Add(bdEWMARangeUseYN);

                    //MA_UCL_OFFSET
                    this.btx_MAUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MA_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //MA_LCL_OFFSET
                    this.btx_MALowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MA_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////MA_OFFSET_YN
                    //Binding bdMAUseYN = new Binding("Checked", _dtConfig, COLUMN.MA_OFFSET_YN);
                    //bdMAUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMAUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_MAUseYN.DataBindings.Add(bdMAUseYN);

                    //MS_UCL_OFFSET
                    this.btx_MSUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MS_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //MS_LCL_OFFSET
                    this.btx_MSLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MS_LCL_OFFSET, true, DataSourceUpdateMode.Never);

                    //MR_UCL_OFFSET
                    this.btx_MRUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MR_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //MR_LCL_OFFSET
                    this.btx_MRLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.MR_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////MS_OFFSET_YN
                    //Binding bdMSUseYN = new Binding("Checked", _dtConfig, COLUMN.MS_OFFSET_YN);
                    //bdMSUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMSUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkbox_MSUseYN.DataBindings.Add(bdMSUseYN);

                    //ZONE_A_UCL_OFFSET
                    this.btx_ZoneAUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_A_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //ZONE_A_LCL_OFFSET
                    this.btx_ZoneALowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_A_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////ZONE_A_OFFSET_YN
                    //Binding bdZoneAUseYN = new Binding("Checked", _dtConfig, COLUMN.ZONE_A_OFFSET_YN);
                    //bdZoneAUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdZoneAUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchbox_ZoneAUseYN.DataBindings.Add(bdZoneAUseYN);

                    //ZONE_B_UCL_OFFSET
                    this.btx_ZoneBUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_B_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //ZONE_B_LCL_OFFSET
                    this.btx_ZoneBLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_B_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////ZONE_B_OFFSET_YN
                    //Binding bdZoneBUseYN = new Binding("Checked", _dtConfig, COLUMN.ZONE_B_OFFSET_YN);
                    //bdZoneBUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdZoneBUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchbox_ZoneBUseYN.DataBindings.Add(bdZoneBUseYN);


                    //ZONE_C_UCL_OFFSET
                    this.btx_ZoneCUpperOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_C_UCL_OFFSET, true, DataSourceUpdateMode.Never);
                    //ZONE_C_LCL_OFFSET
                    this.btx_ZoneCLowerOffset.DataBindings.Add("Text", _dtConfig, COLUMN.ZONE_C_LCL_OFFSET, true, DataSourceUpdateMode.Never);
                    ////ZONE_C_OFFSET_YN
                    //Binding bdZoneCUseYN = new Binding("Checked", _dtConfig, COLUMN.ZONE_C_OFFSET_YN);
                    //bdZoneCUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdZoneCUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchbox_ZoneCUseYN.DataBindings.Add(bdZoneCUseYN);


                    //UPPER_FILTER
                    this.btx_RawFilterUpper.DataBindings.Add("Text", _dtConfig, COLUMN.UPPER_FILTER, true, DataSourceUpdateMode.Never);
                    //LOWER_FILTER
                    this.btx_RawFilterLower.DataBindings.Add("Text", _dtConfig, COLUMN.LOWER_FILTER, true, DataSourceUpdateMode.Never);

                    //UTL
                    this.btxtUTL.DataBindings.Add("Text", _dtConfig, COLUMN.UPPER_TECHNICAL_LIMIT, true, DataSourceUpdateMode.Never);
                    //LTL
                    this.btxtLTL.DataBindings.Add("Text", _dtConfig, COLUMN.LOWER_TECHNICAL_LIMIT, true, DataSourceUpdateMode.Never);

                    //OFFSET_YN
                    Binding bdUseYN = new Binding("Checked", _dtConfig, COLUMN.OFFSET_YN);
                    bdUseYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkbox_OffsetYN.DataBindings.Add(bdUseYN);

                    


                    /*------------------------------------------------------------------------------------------------------------------------------*/
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


        public void InitializeLayout(ConfigMode configMode)
        {
            switch (configMode)
            {
                case ConfigMode.DEFAULT:
                case ConfigMode.CREATE_MAIN:
                case ConfigMode.CREATE_MAIN_FROM:
                case ConfigMode.CREATE_SUB:
                case ConfigMode.MODIFY:
                case ConfigMode.SAVE_AS:
                    break;

                case ConfigMode.VIEW:
                    this.gbMasterSpecLimit.Enabled = false;
                    this.gbRaw.Enabled = false;

                    this.gbMean.Enabled = false;
                    this.gbSTD.Enabled = false;
                    this.gbRange.Enabled = false;
                    this.gbEWMAMean.Enabled = false;
                    this.gbEWMASTD.Enabled = false;
                    this.gbEWMARange.Enabled = false;
                    this.gbMA.Enabled = false;
                    this.gbMS.Enabled = false;
                    this.gbMR.Enabled = false;
                    this.gbZoneSTD.Enabled = false;
                    this.gbZoneA.Enabled = false;
                    this.gbZoneB.Enabled = false;
                    this.gbZoneC.Enabled = false;

                    this.gbMovingOptions.Enabled = false;
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
                this.btxtUpperSpec.Enabled = value;
                this.btxtLowerSpec.Enabled = value;
                this.btxtTarget.Enabled = value;
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
                
                //SPC-736 OCAP action(option) 2012.02.10 by louis you
                spcRuleMstPopup._sOCAPOfSingle = this._sOCAPOfSIngle;

                spcRuleMstPopup.SESSIONDATA = this.sessionData;
                spcRuleMstPopup.URL = this.URL;
                spcRuleMstPopup.PORT = this.Port;
                spcRuleMstPopup.CONFIG_MODE = ConfigMode.CREATE_SUB;
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
                spcRuleMstPopup.CONFIG_MODE = ConfigMode.MODIFY;
                spcRuleMstPopup.CONFIG_RAWID = _sConfigRawID;
                spcRuleMstPopup.RULE_NO = this.bsprRule.GetCellText(_iSelectedRowIdx, _ColIdx_RULE_NO);
                spcRuleMstPopup.SPCRULE_DATATABLE = ((DataSet)this.bsprRule.DataSet).Tables[0];
                spcRuleMstPopup.SPCRULEOPT_DATATABLE = this._dtRuleOpt;
                spcRuleMstPopup.USE_COMMA = _bUseComma;
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
                    //SPC-787 By Louis
                    if (_iSelectedRowIdx < 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                        return;
                    }
                    /////

                    string sSelectRuleNO = bsprRule.GetCellText(bsprRule.ActiveSheet.ActiveRowIndex, _ColIdx_RULE_NO);

                    //#01. RULE 삭제
                    bsprRule.RemoveRow(bsprRule.ActiveSheet.ActiveRowIndex, bsprRule.ActiveSheet.ActiveRowIndex);

                    //#02. RULE Option 삭제
                    DataRow[] drSelectRuleOPTs = this._dtRuleOpt.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, sSelectRuleNO));

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

            double iUpper_ZONEA = 0;
            double iLower_ZONEA = 0;

            double iUpper_ZONEB = 0;
            double iLower_ZONEB = 0;

            double iUpper_ZONEC = 0;
            double iLower_ZONEC = 0;

            if (btxtMst.Name.Equals(this.btxtUpperSpec.Name) || btxtMst.Name.Equals(this.btxtLowerSpec.Name) || btxtMst.Name.Equals(this.btxtTarget.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUpperSpec.Name))
                    this.SetConfigRow(COLUMN.UPPER_SPEC, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLowerSpec.Name))
                    this.SetConfigRow(COLUMN.LOWER_SPEC, btxtMst);

                if (this.btxtUpperSpec.Text.Length > 0 && this.btxtLowerSpec.Text.Length > 0 && this.btxtTarget.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUpperSpec.Text) ? 0 : double.Parse(this.btxtUpperSpec.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLowerSpec.Text) ? 0 : double.Parse(this.btxtLowerSpec.Text);

                    this.btxtTarget.Text = ((iUpper + iLower) / 2).ToString();

                    _dtConfig.Rows[0][COLUMN.TARGET] = double.Parse(this.btxtTarget.Text);
                }
                else if (this.btxtTarget.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.TARGET] = DBNull.Value;
                    this.btxtTarget.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.TARGET] = double.Parse(this.btxtTarget.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.TARGET);
                this.UpdateChangedLColumnList(COLUMN.UPPER_SPEC);
                this.UpdateChangedLColumnList(COLUMN.LOWER_SPEC);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLMean.Name) || btxtMst.Name.Equals(this.btxtLCLMean.Name) || btxtMst.Name.Equals(this.btxtCenterMean.Name))
            {
                //추가된 부분(Create Model 한 뒤에 Rule Tab에서 Control Limit을 입력한 후 다른 작업을 하면 입력한 control limit 값이 사라지는 현상수정)
                if (btxtMst.Name.Equals(this.btxtUCLMean.Name))
                    this.SetConfigRow(COLUMN.UPPER_CONTROL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLMean.Name))
                    this.SetConfigRow(COLUMN.LOWER_CONTROL, btxtMst);
                //추가 완료

                if (this.btxtUCLMean.Text.Length > 0 && this.btxtLCLMean.Text.Length > 0 && this.btxtCenterMean.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLMean.Text) ? 0 : double.Parse(this.btxtUCLMean.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLMean.Text) ? 0 : double.Parse(this.btxtLCLMean.Text);
                    this.btxtCenterMean.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.CENTER_LINE] = double.Parse(this.btxtCenterMean.Text);
                    if (this.btxtCenterMean.Text.Length > 0 && this.btxtSTD.Text.Length > 0)
                    {
                        iUpper_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 3 * double.Parse(this.btxtSTD.Text);
                        iLower_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 3 * double.Parse(this.btxtSTD.Text);
                        iUpper_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 2 * double.Parse(this.btxtSTD.Text);
                        iLower_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 2 * double.Parse(this.btxtSTD.Text);
                        iUpper_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + double.Parse(this.btxtSTD.Text);
                        iLower_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - double.Parse(this.btxtSTD.Text);

                        this.btxtUCLZoneA.Text = iUpper_ZONEA.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = double.Parse(this.btxtUCLZoneA.Text);
                        this.btxtUCLZoneB.Text = iUpper_ZONEB.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = double.Parse(this.btxtUCLZoneB.Text);
                        this.btxtUCLZoneC.Text = iUpper_ZONEC.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = double.Parse(this.btxtUCLZoneC.Text);
                        this.btxtLCLZoneA.Text = iLower_ZONEA.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = double.Parse(this.btxtLCLZoneA.Text);
                        this.btxtLCLZoneB.Text = iLower_ZONEB.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = double.Parse(this.btxtLCLZoneB.Text);
                        this.btxtLCLZoneC.Text = iLower_ZONEC.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = double.Parse(this.btxtLCLZoneC.Text);
                    }
                    else
                    {
                        this.btxtUCLZoneA.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = DBNull.Value;
                        this.btxtUCLZoneB.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = DBNull.Value;
                        this.btxtUCLZoneC.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = DBNull.Value;
                        this.btxtLCLZoneA.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = DBNull.Value;
                        this.btxtLCLZoneB.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = DBNull.Value;
                        this.btxtLCLZoneC.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = DBNull.Value;
                    }

                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL);
                }
                else if (this.btxtCenterMean.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.CENTER_LINE] = DBNull.Value;
                    this.btxtCenterMean.Text = null;

                    this.btxtUCLZoneA.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = DBNull.Value;
                    this.btxtUCLZoneB.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = DBNull.Value;
                    this.btxtUCLZoneC.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = DBNull.Value;
                    this.btxtLCLZoneA.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = DBNull.Value;
                    this.btxtLCLZoneB.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = DBNull.Value;
                    this.btxtLCLZoneC.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = DBNull.Value;

                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL);
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.CENTER_LINE] = double.Parse(this.btxtCenterMean.Text);
                    if (this.btxtCenterMean.Text.Length > 0 && this.btxtSTD.Text.Length > 0)
                    {
                        iUpper_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 3 * double.Parse(this.btxtSTD.Text);
                        iLower_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 3 * double.Parse(this.btxtSTD.Text);
                        iUpper_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 2 * double.Parse(this.btxtSTD.Text);
                        iLower_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 2 * double.Parse(this.btxtSTD.Text);
                        iUpper_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + double.Parse(this.btxtSTD.Text);
                        iLower_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - double.Parse(this.btxtSTD.Text);

                        this.btxtUCLZoneA.Text = iUpper_ZONEA.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = double.Parse(this.btxtUCLZoneA.Text);
                        this.btxtUCLZoneB.Text = iUpper_ZONEB.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = double.Parse(this.btxtUCLZoneB.Text);
                        this.btxtUCLZoneC.Text = iUpper_ZONEC.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = double.Parse(this.btxtUCLZoneC.Text);
                        this.btxtLCLZoneA.Text = iLower_ZONEA.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = double.Parse(this.btxtLCLZoneA.Text);
                        this.btxtLCLZoneB.Text = iLower_ZONEB.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = double.Parse(this.btxtLCLZoneB.Text);
                        this.btxtLCLZoneC.Text = iLower_ZONEC.ToString();
                        _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = double.Parse(this.btxtLCLZoneC.Text);
                    }
                    else
                    {
                        this.btxtUCLZoneA.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = DBNull.Value;
                        this.btxtUCLZoneB.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = DBNull.Value;
                        this.btxtUCLZoneC.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = DBNull.Value;
                        this.btxtLCLZoneA.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = DBNull.Value;
                        this.btxtLCLZoneB.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = DBNull.Value;
                        this.btxtLCLZoneC.Text = null;
                        _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = DBNull.Value;
                    }
                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL);
                    this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL);
                }

                this.UpdateChangedLColumnList(COLUMN.CENTER_LINE);
                this.UpdateChangedLColumnList(COLUMN.UPPER_CONTROL);
                this.UpdateChangedLColumnList(COLUMN.LOWER_CONTROL);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLRaw.Name) || btxtMst.Name.Equals(this.btxtLCLRaw.Name) || btxtMst.Name.Equals(this.btxtCenterRaw.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLRaw.Name))
                    this.SetConfigRow(COLUMN.RAW_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLRaw.Name))
                    this.SetConfigRow(COLUMN.RAW_LCL, btxtMst);

                if (this.btxtUCLRaw.Text.Length > 0 && this.btxtLCLRaw.Text.Length > 0 && this.btxtCenterRaw.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLRaw.Text) ? 0 : double.Parse(this.btxtUCLRaw.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLRaw.Text) ? 0 : double.Parse(this.btxtLCLRaw.Text);
                    this.btxtCenterRaw.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.RAW_CENTER_LINE] = double.Parse(this.btxtCenterRaw.Text);
                }
                else if (this.btxtCenterRaw.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.RAW_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterRaw.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.RAW_CENTER_LINE] = double.Parse(this.btxtCenterRaw.Text);
                }


                this.UpdateChangedLColumnList(COLUMN.RAW_UCL);
                this.UpdateChangedLColumnList(COLUMN.RAW_LCL);
                this.UpdateChangedLColumnList(COLUMN.RAW_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLSTD.Name) || btxtMst.Name.Equals(this.btxtLCLSTD.Name) || btxtMst.Name.Equals(this.btxtCenterSTD.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLSTD.Name))
                    this.SetConfigRow(COLUMN.STD_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLSTD.Name))
                    this.SetConfigRow(COLUMN.STD_LCL, btxtMst);

                if (this.btxtUCLSTD.Text.Length > 0 && this.btxtLCLSTD.Text.Length > 0 && this.btxtCenterSTD.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLSTD.Text) ? 0 : double.Parse(this.btxtUCLSTD.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLSTD.Text) ? 0 : double.Parse(this.btxtLCLSTD.Text);
                    this.btxtCenterSTD.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.STD_CENTER_LINE] = double.Parse(this.btxtCenterSTD.Text);
                }
                else if (this.btxtCenterSTD.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.STD_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterSTD.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.STD_CENTER_LINE] = double.Parse(this.btxtCenterSTD.Text);
                }


                this.UpdateChangedLColumnList(COLUMN.STD_UCL);
                this.UpdateChangedLColumnList(COLUMN.STD_LCL);
                this.UpdateChangedLColumnList(COLUMN.STD_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLRange.Name) || btxtMst.Name.Equals(this.btxtLCLRange.Name) || btxtMst.Name.Equals(this.btxtCenterRange.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLRange.Name))
                    this.SetConfigRow(COLUMN.RANGE_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLRange.Name))
                    this.SetConfigRow(COLUMN.RANGE_LCL, btxtMst);

                if (this.btxtUCLRange.Text.Length > 0 && this.btxtLCLRange.Text.Length > 0 && this.btxtCenterRange.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLRange.Text) ? 0 : double.Parse(this.btxtUCLRange.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLRange.Text) ? 0 : double.Parse(this.btxtLCLRange.Text);
                    this.btxtCenterRange.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.RANGE_CENTER_LINE] = double.Parse(this.btxtCenterRange.Text);
                }
                else if (this.btxtCenterRange.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.RANGE_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterRange.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.RANGE_CENTER_LINE] = double.Parse(this.btxtCenterRange.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.RANGE_UCL);
                this.UpdateChangedLColumnList(COLUMN.RANGE_LCL);
                this.UpdateChangedLColumnList(COLUMN.RANGE_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLEWMAMean.Name) || btxtMst.Name.Equals(this.btxtLCLEWMAMean.Name) || btxtMst.Name.Equals(this.btxtCenterEWMAMean.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLEWMAMean.Name))
                    this.SetConfigRow(COLUMN.EWMA_M_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLEWMAMean.Name))
                    this.SetConfigRow(COLUMN.EWMA_M_LCL, btxtMst);

                if (this.btxtUCLEWMAMean.Text.Length > 0 && this.btxtLCLEWMAMean.Text.Length > 0 && this.btxtCenterEWMAMean.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLEWMAMean.Text) ? 0 : double.Parse(this.btxtUCLEWMAMean.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLEWMAMean.Text) ? 0 : double.Parse(this.btxtLCLEWMAMean.Text);
                    this.btxtCenterEWMAMean.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.EWMA_M_CENTER_LINE] = double.Parse(this.btxtCenterEWMAMean.Text);
                }
                else if (this.btxtCenterEWMAMean.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_M_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterEWMAMean.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_M_CENTER_LINE] = double.Parse(this.btxtCenterEWMAMean.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.EWMA_M_UCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_M_LCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_M_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLEWMARange.Name) || btxtMst.Name.Equals(this.btxtLCLEWMARange.Name) || btxtMst.Name.Equals(this.btxtCenterEWMARange.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLEWMARange.Name))
                    this.SetConfigRow(COLUMN.EWMA_R_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLEWMARange.Name))
                    this.SetConfigRow(COLUMN.EWMA_R_LCL, btxtMst);

                if (this.btxtUCLEWMARange.Text.Length > 0 && this.btxtLCLEWMARange.Text.Length > 0 && this.btxtCenterEWMARange.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLEWMARange.Text) ? 0 : double.Parse(this.btxtUCLEWMARange.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLEWMARange.Text) ? 0 : double.Parse(this.btxtLCLEWMARange.Text);
                    this.btxtCenterEWMARange.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.EWMA_R_CENTER_LINE] = double.Parse(this.btxtCenterEWMARange.Text);
                }
                else if (this.btxtCenterEWMARange.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_R_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterEWMARange.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_R_CENTER_LINE] = double.Parse(this.btxtCenterEWMARange.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.EWMA_R_UCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_R_LCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_R_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLEWMASTD.Name) || btxtMst.Name.Equals(this.btxtLCLEWMASTD.Name) || btxtMst.Name.Equals(this.btxtCenterEWMASTD.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLEWMASTD.Name))
                    this.SetConfigRow(COLUMN.EWMA_S_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLEWMASTD.Name))
                    this.SetConfigRow(COLUMN.EWMA_S_LCL, btxtMst);

                if (this.btxtUCLEWMASTD.Text.Length > 0 && this.btxtLCLEWMASTD.Text.Length > 0 && this.btxtCenterEWMASTD.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLEWMASTD.Text) ? 0 : double.Parse(this.btxtUCLEWMASTD.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLEWMASTD.Text) ? 0 : double.Parse(this.btxtLCLEWMASTD.Text);
                    this.btxtCenterEWMASTD.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.EWMA_S_CENTER_LINE] = double.Parse(this.btxtCenterEWMASTD.Text);
                }
                else if (this.btxtCenterEWMASTD.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_S_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterEWMASTD.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.EWMA_S_CENTER_LINE] = double.Parse(this.btxtCenterEWMASTD.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.EWMA_S_UCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_S_LCL);
                this.UpdateChangedLColumnList(COLUMN.EWMA_S_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLMA.Name) || btxtMst.Name.Equals(this.btxtLCLMA.Name) || btxtMst.Name.Equals(this.btxtCenterMA.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLMA.Name))
                    this.SetConfigRow(COLUMN.MA_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLMA.Name))
                    this.SetConfigRow(COLUMN.MA_LCL, btxtMst);

                if (this.btxtUCLMA.Text.Length > 0 && this.btxtLCLMA.Text.Length > 0 && this.btxtCenterMA.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLMA.Text) ? 0 : double.Parse(this.btxtUCLMA.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLMA.Text) ? 0 : double.Parse(this.btxtLCLMA.Text);
                    this.btxtCenterMA.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.MA_CENTER_LINE] = double.Parse(this.btxtCenterMA.Text);
                }
                else if (this.btxtCenterMA.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.MA_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterMA.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.MA_CENTER_LINE] = double.Parse(this.btxtCenterMA.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.MA_UCL);
                this.UpdateChangedLColumnList(COLUMN.MA_LCL);
                this.UpdateChangedLColumnList(COLUMN.MA_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLMS.Name) || btxtMst.Name.Equals(this.btxtLCLMS.Name) || btxtMst.Name.Equals(this.btxtCenterMS.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLMS.Name))
                    this.SetConfigRow(COLUMN.MS_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLMS.Name))
                    this.SetConfigRow(COLUMN.MS_LCL, btxtMst);

                if (this.btxtUCLMS.Text.Length > 0 && this.btxtLCLMS.Text.Length > 0 && this.btxtCenterMS.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLMS.Text) ? 0 : double.Parse(this.btxtUCLMS.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLMS.Text) ? 0 : double.Parse(this.btxtLCLMS.Text);
                    this.btxtCenterMS.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.MS_CENTER_LINE] = double.Parse(this.btxtCenterMS.Text);
                }
                else if (this.btxtCenterMS.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.MS_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterMS.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.MS_CENTER_LINE] = double.Parse(this.btxtCenterMS.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.MS_UCL);
                this.UpdateChangedLColumnList(COLUMN.MS_LCL);
                this.UpdateChangedLColumnList(COLUMN.MS_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtUCLMR.Name) || btxtMst.Name.Equals(this.btxtLCLMR.Name) || btxtMst.Name.Equals(this.btxtCenterMR.Name))
            {
                if (btxtMst.Name.Equals(this.btxtUCLMR.Name))
                    this.SetConfigRow(COLUMN.MR_UCL, btxtMst);
                else if (btxtMst.Name.Equals(this.btxtLCLMR.Name))
                    this.SetConfigRow(COLUMN.MR_LCL, btxtMst);

                if (this.btxtUCLMR.Text.Length > 0 && this.btxtLCLMR.Text.Length > 0 && this.btxtCenterMR.Text.Length == 0)
                {
                    iUpper = string.IsNullOrEmpty(this.btxtUCLMR.Text) ? 0 : double.Parse(this.btxtUCLMR.Text);
                    iLower = string.IsNullOrEmpty(this.btxtLCLMR.Text) ? 0 : double.Parse(this.btxtLCLMR.Text);
                    this.btxtCenterMR.Text = ((iUpper + iLower) / 2).ToString();
                    _dtConfig.Rows[0][COLUMN.MR_CENTER_LINE] = double.Parse(this.btxtCenterMR.Text);
                }
                else if (this.btxtCenterMR.Text.Length == 0)
                {
                    _dtConfig.Rows[0][COLUMN.MR_CENTER_LINE] = DBNull.Value;
                    this.btxtCenterMR.Text = null;
                }
                else
                {
                    _dtConfig.Rows[0][COLUMN.MR_CENTER_LINE] = double.Parse(this.btxtCenterMR.Text);
                }

                this.UpdateChangedLColumnList(COLUMN.MR_UCL);
                this.UpdateChangedLColumnList(COLUMN.MR_LCL);
                this.UpdateChangedLColumnList(COLUMN.MR_CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(this.btxtTarget.Name))
            {
                this.SetConfigRow(COLUMN.TARGET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.TARGET);
            }
            else if (btxtMst.Name.Equals(btxtEWMALambda.Name))
            {
                this.UpdateChangedLColumnList(COLUMN.EWMA_LAMBDA);
            }
            else if (btxtMst.Name.Equals(btxtMovingCount.Name))
            {
                this.UpdateChangedLColumnList(COLUMN.MOVING_COUNT);
            }
            else if (btxtMst.Name.Equals(btxtCenterMean.Name))
            {
                this.SetConfigRow(COLUMN.CENTER_LINE, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.CENTER_LINE);
            }
            else if (btxtMst.Name.Equals(btxtSTD.Name))
            {
                if (btxtMst.Name.Equals(this.btxtSTD.Name))
                    this.SetConfigRow(COLUMN.STD, btxtMst);

                if (this.btxtCenterMean.Text.Length > 0 && this.btxtSTD.Text.Length > 0)
                {
                    iUpper_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 3 * double.Parse(this.btxtSTD.Text);
                    iLower_ZONEA = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 3 * double.Parse(this.btxtSTD.Text);
                    iUpper_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + 2 * double.Parse(this.btxtSTD.Text);
                    iLower_ZONEB = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - 2 * double.Parse(this.btxtSTD.Text);
                    iUpper_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) + double.Parse(this.btxtSTD.Text);
                    iLower_ZONEC = string.IsNullOrEmpty(this.btxtCenterMean.Text) ? 0 : double.Parse(this.btxtCenterMean.Text) - double.Parse(this.btxtSTD.Text);

                    this.btxtUCLZoneA.Text = iUpper_ZONEA.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = double.Parse(this.btxtUCLZoneA.Text);
                    this.btxtUCLZoneB.Text = iUpper_ZONEB.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = double.Parse(this.btxtUCLZoneB.Text);
                    this.btxtUCLZoneC.Text = iUpper_ZONEC.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = double.Parse(this.btxtUCLZoneC.Text);
                    this.btxtLCLZoneA.Text = iLower_ZONEA.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = double.Parse(this.btxtLCLZoneA.Text);
                    this.btxtLCLZoneB.Text = iLower_ZONEB.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = double.Parse(this.btxtLCLZoneB.Text);
                    this.btxtLCLZoneC.Text = iLower_ZONEC.ToString();
                    _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = double.Parse(this.btxtLCLZoneC.Text);
                }
                else
                {
                    this.btxtUCLZoneA.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_A_UCL] = DBNull.Value;
                    this.btxtUCLZoneB.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_B_UCL] = DBNull.Value;
                    this.btxtUCLZoneC.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_C_UCL] = DBNull.Value;
                    this.btxtLCLZoneA.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_A_LCL] = DBNull.Value;
                    this.btxtLCLZoneB.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_B_LCL] = DBNull.Value;
                    this.btxtLCLZoneC.Text = null;
                    _dtConfig.Rows[0][COLUMN.ZONE_C_LCL] = DBNull.Value;
                }

                this.UpdateChangedLColumnList(COLUMN.STD);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL);
            }
            else if (btxtMst.Name.Equals(btxtUCLZoneA.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_A_UCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL);
            }
            else if (btxtMst.Name.Equals(btxtUCLZoneB.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_B_UCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL);
            }
            else if (btxtMst.Name.Equals(btxtUCLZoneC.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_C_UCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL);
            }
            else if (btxtMst.Name.Equals(btxtLCLZoneA.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_A_LCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL);
            }
            else if (btxtMst.Name.Equals(btxtLCLZoneB.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_B_LCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL);
            }
            else if (btxtMst.Name.Equals(btxtUCLZoneC.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_C_LCL, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL);
            }


            /* Chris : 2010-06-18 */
            else if (btxtMst.Name.Equals(btx_SpecUpperOffer.Name))
            {
                this.SetConfigRow(COLUMN.SPEC_USL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.SPEC_USL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_SpecLowerOffer.Name))
            {
                this.SetConfigRow(COLUMN.SPEC_LSL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.SPEC_LSL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_RawUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.RAW_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.RAW_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_RawLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.RAW_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.RAW_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_MeanUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.MEAN_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MEAN_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_MeanLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.MEAN_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MEAN_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_STDUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.STD_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.STD_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_STDLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.STD_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.STD_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_RangeUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.RANGE_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.RANGE_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_RangeLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.RANGE_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.RANGE_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMAMeanUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_M_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_M_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMAMeanLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_M_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_M_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMARangeUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_R_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_R_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMARangeLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_R_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_R_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMASTDUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_S_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_S_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_EWMASTDLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.EWMA_S_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.EWMA_S_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_MAUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.MA_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MA_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_MALowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.MA_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MA_LCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_MSUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.MS_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MS_UCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_MSLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.MS_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MS_LCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_MRUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.MR_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MR_UCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_MRLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.MR_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.MR_LCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_ZoneAUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_A_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_UCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_ZoneALowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_A_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_A_LCL_OFFSET);
            }
            else if (btxtMst.Name.Equals(btx_ZoneBUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_B_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_ZoneBLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_B_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_B_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_ZoneCUpperOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_C_UCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_UCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_ZoneCLowerOffset.Name))
            {
                this.SetConfigRow(COLUMN.ZONE_C_LCL_OFFSET, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.ZONE_C_LCL_OFFSET);
            }

            else if (btxtMst.Name.Equals(btx_RawFilterUpper.Name))
            {
                this.SetConfigRow(COLUMN.UPPER_FILTER, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.UPPER_FILTER);
            }

            else if (btxtMst.Name.Equals(btx_RawFilterLower.Name))
            {
                this.SetConfigRow(COLUMN.LOWER_FILTER, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.LOWER_FILTER);
            }

            else if (btxtMst.Name.Equals(btxtUTL.Name))
            {
                this.SetConfigRow(COLUMN.UPPER_TECHNICAL_LIMIT, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.UPPER_TECHNICAL_LIMIT);
            }

            else if (btxtMst.Name.Equals(btxtLTL.Name))
            {
                this.SetConfigRow(COLUMN.LOWER_TECHNICAL_LIMIT, btxtMst);
                this.UpdateChangedLColumnList(COLUMN.LOWER_TECHNICAL_LIMIT);
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

        public bool InputValidation()
        {
            bool result = true;

            double dUpper = 0;
            double dLower = 0;
            double dCenter = 0;


            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtLowerSpec.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "LSL"}, null);
                    return false;
                }
            }

            #region usl & ucl(raw,mean)

            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtUCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtUCLMean.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "MEAN UCL"}, null);
                    return false;
                }
            }

            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtUCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtUCLRaw.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "RAW UCL"}, null);
                    return false;
                }
            }

            #endregion

            #region lsl & lcl(raw,mean)

            if (this.btxtLowerSpec.Text.Length > 0 && this.btxtLCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtLCLMean.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text); 

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MEAN LCL", "LSL"}, null);
                    return false;
                }
            }

            if (this.btxtLowerSpec.Text.Length > 0 && this.btxtLCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtLCLRaw.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"RAW LCL", "LSL"}, null);
                    return false;
                }
            }
            #endregion

            #region lsl & ucl(raw,mean)
            if (this.btxtLowerSpec.Text.Length > 0 && this.btxtUCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMean.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MEAN UCL", "LSL"}, null);
                    return false;
                }
            }

            if (this.btxtLowerSpec.Text.Length > 0 && this.btxtUCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLRaw.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"RAW UCL", "LSL"}, null);
                    return false;
                }
            }
            #endregion

            #region usl & lcl(raw,mean)
            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtLCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtLCLMean.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "MEAN LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtLCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtLCLRaw.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "RAW LCL"}, null);
                    return false;
                }
            }
            #endregion

            if (this.btxtUCLRaw.Text.Length > 0 && this.btxtLCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLRaw.Text);
                dLower = double.Parse(this.btxtLCLRaw.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"Raw UCL","Raw LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLMean.Text.Length > 0 && this.btxtLCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMean.Text);
                dLower = double.Parse(this.btxtLCLMean.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"Mean UCL", "Mean LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLSTD.Text.Length > 0 && this.btxtLCLSTD.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLSTD.Text);
                dLower = double.Parse(this.btxtLCLSTD.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"STD UCL", "STD LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLRange.Text.Length > 0 && this.btxtLCLRange.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLRange.Text);
                dLower = double.Parse(this.btxtLCLRange.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"Range UCL", "Range LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMAMean.Text.Length > 0 && this.btxtLCLEWMAMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMAMean.Text);
                dLower = double.Parse(this.btxtLCLEWMAMean.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"EWMA(Mean) UCL", "EWMA(Mean) LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMAMean.Text.Length > 0 && this.btxtLCLEWMAMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMAMean.Text);
                dLower = double.Parse(this.btxtLCLEWMAMean.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"EWMA(Mean) UCL", "EWMA(Mean) LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMASTD.Text.Length > 0 && this.btxtLCLEWMASTD.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMASTD.Text);
                dLower = double.Parse(this.btxtLCLEWMASTD.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"EWMA(STD) UCL", "EWMA(STD) LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMARange.Text.Length > 0 && this.btxtLCLEWMARange.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMARange.Text);
                dLower = double.Parse(this.btxtLCLEWMARange.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"EWMA(Range) UCL", "EWMA(Range) LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLMA.Text.Length > 0 && this.btxtLCLMA.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMA.Text);
                dLower = double.Parse(this.btxtLCLMA.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MA UCL", "MA LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLMS.Text.Length > 0 && this.btxtLCLMS.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMS.Text);
                dLower = double.Parse(this.btxtLCLMS.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MS UCL", "MS LCL"}, null);
                    return false;
                }
            }

            if (this.btxtUCLMR.Text.Length > 0 && this.btxtLCLMR.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMR.Text);
                dLower = double.Parse(this.btxtLCLMR.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{ "MR UCL", "MR LCL"}, null);
                    return false;
                }
            }

            #endregion

            #region usl & utl

            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtUTL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUpperSpec.Text);
                dLower = double.Parse(this.btxtUTL.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "UTL"}, null);
                    return false;
                }
            }

            #endregion

            #region lsl & ltl

            if (this.btxtLowerSpec.Text.Length > 0 && this.btxtLTL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtLTL.Text);
                dLower = double.Parse(this.btxtLowerSpec.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"LTL", "LSL"}, null);
                    return false;
                }
            }

            #endregion

            //Technical Limit
            if (this.btxtUTL.Text.Length > 0 && this.btxtLTL.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUTL.Text);
                dLower = double.Parse(this.btxtLTL.Text);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"UTL", "LTL"}, null);
                    return false;
                }
            }

            if (this.btxtUpperSpec.Text.Length > 0 && this.btxtLowerSpec.Text.Length > 0)
            {
                if (this.btxtUpperSpec.Text.Length > 0)
                    dUpper = double.Parse(this.btxtUpperSpec.Text);
                else
                    dUpper = 0;

                if (this.btxtLowerSpec.Text.Length > 0)
                    dLower = double.Parse(this.btxtLowerSpec.Text);
                else
                    dLower = 0;

                if (this.btxtTarget.Text.Length > 0)
                    dCenter = double.Parse(this.btxtTarget.Text);
                else
                    dCenter = 0;

                if (dCenter > dUpper && this.btxtUpperSpec.Text.Length > 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[]{"Target", "USL"}, null);
                    return false;
                }
                else if (dCenter < dLower && this.btxtLowerSpec.Text.Length > 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[]{"Target", "LSL"}, null);
                    return false;
                }
            }

            //spc-1219
            if (this.btxtUCLRaw.Text.Length > 0 && this.btxtLCLRaw.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLRaw.Text);
                dLower = double.Parse(this.btxtLCLRaw.Text);
                dCenter = double.Parse(this.btxtCenterRaw.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLRaw.Text.Length == 0 && this.btxtCenterRaw.Text.Length > 0) || (this.btxtLCLRaw.Text.Length == 0 && this.btxtCenterRaw.Text.Length > 0))
            {
                if (this.btxtUCLRaw.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLMean.Text.Length > 0 && this.btxtLCLMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMean.Text);
                dLower = double.Parse(this.btxtLCLMean.Text);
                dCenter = double.Parse(this.btxtCenterMean.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLMean.Text.Length == 0 && this.btxtCenterMean.Text.Length > 0) || (this.btxtLCLMean.Text.Length == 0 && this.btxtCenterMean.Text.Length > 0))
            {
                if (this.btxtUCLMean.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }


            if (this.btxtUCLSTD.Text.Length > 0 && this.btxtLCLSTD.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLSTD.Text);
                dLower = double.Parse(this.btxtLCLSTD.Text);
                dCenter = double.Parse(this.btxtCenterSTD.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_BIGGER", new string[]{"LCL", "UCL"}, null);
                    return false;
                }
            }
            else if ((this.btxtUCLSTD.Text.Length == 0 && this.btxtCenterSTD.Text.Length > 0) || (this.btxtLCLSTD.Text.Length == 0 && this.btxtCenterSTD.Text.Length > 0))
            {
                if (this.btxtUCLSTD.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLRange.Text.Length > 0 && this.btxtLCLRange.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLRange.Text);
                dLower = double.Parse(this.btxtLCLRange.Text);
                dCenter = double.Parse(this.btxtCenterRange.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLRange.Text.Length == 0 && this.btxtCenterRange.Text.Length > 0) || (this.btxtLCLRange.Text.Length == 0 && this.btxtCenterRange.Text.Length > 0))
            {
                if (this.btxtUCLRange.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMAMean.Text.Length > 0 && this.btxtLCLEWMAMean.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMAMean.Text);
                dLower = double.Parse(this.btxtLCLEWMAMean.Text);
                dCenter = double.Parse(this.btxtCenterEWMAMean.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLEWMAMean.Text.Length == 0 && this.btxtCenterEWMAMean.Text.Length > 0) || (this.btxtLCLEWMAMean.Text.Length == 0 && this.btxtCenterEWMAMean.Text.Length > 0))
            {
                if (this.btxtUCLEWMAMean.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMASTD.Text.Length > 0 && this.btxtLCLEWMASTD.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMASTD.Text);
                dLower = double.Parse(this.btxtLCLEWMASTD.Text);
                dCenter = double.Parse(this.btxtCenterEWMASTD.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLEWMASTD.Text.Length == 0 && this.btxtCenterEWMASTD.Text.Length > 0) || (this.btxtLCLEWMASTD.Text.Length == 0 && this.btxtCenterEWMASTD.Text.Length > 0))
            {
                if (this.btxtUCLEWMASTD.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLEWMARange.Text.Length > 0 && this.btxtLCLEWMARange.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLEWMARange.Text);
                dLower = double.Parse(this.btxtLCLEWMARange.Text);
                dCenter = double.Parse(this.btxtCenterEWMARange.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLEWMARange.Text.Length == 0 && this.btxtCenterEWMARange.Text.Length > 0) || (this.btxtLCLEWMARange.Text.Length == 0 && this.btxtCenterEWMARange.Text.Length > 0))
            {
                if (this.btxtUCLEWMARange.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLMA.Text.Length > 0 && this.btxtLCLMA.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMA.Text);
                dLower = double.Parse(this.btxtLCLMA.Text);
                dCenter = double.Parse(this.btxtCenterMA.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLMA.Text.Length == 0 && this.btxtCenterMA.Text.Length > 0) || (this.btxtLCLMA.Text.Length == 0 && this.btxtCenterMA.Text.Length > 0))
            {
                if (this.btxtUCLMA.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLMS.Text.Length > 0 && this.btxtLCLMS.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMS.Text);
                dLower = double.Parse(this.btxtLCLMS.Text);
                dCenter = double.Parse(this.btxtCenterMS.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLMS.Text.Length == 0 && this.btxtCenterMS.Text.Length > 0) || (this.btxtLCLMS.Text.Length == 0 && this.btxtCenterMS.Text.Length > 0))
            {
                if (this.btxtUCLMS.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            if (this.btxtUCLMR.Text.Length > 0 && this.btxtLCLMR.Text.Length > 0)
            {
                dUpper = double.Parse(this.btxtUCLMR.Text);
                dLower = double.Parse(this.btxtLCLMR.Text);
                dCenter = double.Parse(this.btxtCenterMR.Text);

                if (dCenter > dUpper || dCenter < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_TARGET_VALUE", null, null);
                    return false;
                }
            }
            else if ((this.btxtUCLMR.Text.Length == 0 && this.btxtCenterMR.Text.Length > 0) || (this.btxtLCLMR.Text.Length == 0 && this.btxtCenterMR.Text.Length > 0))
            {
                if (this.btxtUCLMR.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_UCL_IS_EMPTY", null, null);
                    return false;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_LCL_IS_EMPTY", null, null);
                    return false;
                }
            }

            return result;
        }

        private void bchkbox_OffsetYN_CheckedChanged(object sender, EventArgs e)
        {
            if (!bchkbox_OffsetYN.Checked)
            {
                btx_SpecUpperOffer.ReadOnly = true;
                btx_SpecLowerOffer.ReadOnly = true;
                btx_RawUpperOffset.ReadOnly = true;
                btx_RawLowerOffset.ReadOnly = true;                
                btx_RangeUpperOffset.ReadOnly = true;
                btx_RangeLowerOffset.ReadOnly = true;
                btx_STDUpperOffset.ReadOnly = true;
                btx_STDLowerOffset.ReadOnly = true;
                btx_MeanUpperOffset.ReadOnly = true;
                btx_MeanLowerOffset.ReadOnly = true;
                btx_EWMAMeanUpperOffset.ReadOnly = true;
                btx_EWMAMeanLowerOffset.ReadOnly = true;
                btx_EWMARangeUpperOffset.ReadOnly = true;
                btx_EWMARangeLowerOffset.ReadOnly = true;
                btx_EWMASTDUpperOffset.ReadOnly = true;
                btx_EWMASTDLowerOffset.ReadOnly = true;
                btx_MAUpperOffset.ReadOnly = true;
                btx_MALowerOffset.ReadOnly = true;
                btx_MSUpperOffset.ReadOnly = true;
                btx_MSLowerOffset.ReadOnly = true;
                btx_MRUpperOffset.ReadOnly = true;
                btx_MRLowerOffset.ReadOnly = true;
                btx_ZoneAUpperOffset.ReadOnly = true;
                btx_ZoneALowerOffset.ReadOnly = true;
                btx_ZoneBUpperOffset.ReadOnly = true;
                btx_ZoneBLowerOffset.ReadOnly = true;
                btx_ZoneCUpperOffset.ReadOnly = true;
                btx_ZoneCLowerOffset.ReadOnly = true;
            }
            else
            {
                btx_SpecUpperOffer.ReadOnly = false;
                btx_SpecLowerOffer.ReadOnly = false;
                btx_RawUpperOffset.ReadOnly = false;
                btx_RawLowerOffset.ReadOnly = false;
                btx_RangeUpperOffset.ReadOnly = false;
                btx_RangeLowerOffset.ReadOnly = false;
                btx_STDUpperOffset.ReadOnly = false;
                btx_STDLowerOffset.ReadOnly = false;
                btx_MeanUpperOffset.ReadOnly = false;
                btx_MeanLowerOffset.ReadOnly = false;
                btx_EWMAMeanUpperOffset.ReadOnly = false;
                btx_EWMAMeanLowerOffset.ReadOnly = false;
                btx_EWMARangeUpperOffset.ReadOnly = false;
                btx_EWMARangeLowerOffset.ReadOnly = false;
                btx_EWMASTDUpperOffset.ReadOnly = false;
                btx_EWMASTDLowerOffset.ReadOnly = false;
                btx_MAUpperOffset.ReadOnly = false;
                btx_MALowerOffset.ReadOnly = false;
                btx_MSUpperOffset.ReadOnly = false;
                btx_MSLowerOffset.ReadOnly = false;
                btx_MRUpperOffset.ReadOnly = false;
                btx_MRLowerOffset.ReadOnly = false;
                btx_ZoneAUpperOffset.ReadOnly = false;
                btx_ZoneALowerOffset.ReadOnly = false;
                btx_ZoneBUpperOffset.ReadOnly = false;
                btx_ZoneBLowerOffset.ReadOnly = false;
                btx_ZoneCUpperOffset.ReadOnly = false;
                btx_ZoneCLowerOffset.ReadOnly = false;
            }
        }








    }

}
