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
using System.Reflection;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class AutoCalcConfigurationUC : BasePageUCtrl
    {
        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        //Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        MultiLanguageHandler _mlthandler;
        
        Initialization _Initialization;
        SortedList _slColumnIndex;
        LinkedList _llColumnMapping;

        //BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;
        private LinkedList _LimitOptionList;

        private ConfigMode _cofingMode;
        private string _sConfigRawID;
        private string _CalcCount = "0";

        private DataTable _dtAutoCalc;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        
        public AutoCalcConfigurationUC()
        {
            InitializeComponent();
        }

        // [SPC-658]
        //========================================
        //private bool _isDefaultModel = false;
        public AutoCalcConfigurationUC(bool isDefaultModel)
        {
            //this._isDefaultModel = isDefaultModel;
            InitializeComponent();

            if (isDefaultModel)
            {
                this.bLabel3.Visible = false;
                this.blblAutoInitialCalcCount.Visible = false;
                this.btxtAutoInitialCalcCount.Visible = false;
            }
        }
        //========================================

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

            this._ComUtil = new CommonUtility();
            
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._bspreadutility = new BSpreadUtility();
            this._LimitOptionList = new LinkedList();
            
            DataSet dsOption = _wsSPC.GetAutoCalculationOption(Definition.CODE_CATEGORY_AUTOCALC_OPTION);
            
            if (dsOption != null && dsOption.Tables.Count > 0)
            {
                foreach (DataRow dr in dsOption.Tables[0].Rows)
                {
                    this._LimitOptionList.Add(dr[Definition.VARIABLE_CODE], dr[Definition.VARIABLE_NAME].ToString());
                    this.bComboOption.Items.Add(dr[Definition.VARIABLE_NAME].ToString());
                }
            }

            this.InitializeDataButton();
            this.InitializeBSpread();
            
            this.blblMinSampleCnt.Text = this._mlthandler.GetVariable(Definition.LABEL.MINSAMPLECNT);
        }

        //SPC-1155 AutoCalculation chart type별로 설정 - 2013.11.26 by stella kim.
        private void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprAutoCalc, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_AUTO_CALCULATION);

            this.bsprAutoCalc.UseGeneralContextMenu = false;
            this.bsprAutoCalc.UseAutoSort = false;
            
            LinkedList llconidtion = new LinkedList();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);

            DataSet dsControlChart = this._wsSPC.CreateAutoCalculationData(llconidtion.GetSerialData());
            this.bsprAutoCalc.DataSet = dsControlChart;

            string[] option = (string[])this._LimitOptionList.GetValueList().ToArray(typeof(string));
            this._bspreadutility.SetNumberCellTypeForColumn(this.bsprAutoCalc.ActiveSheet.Columns[(int)this._slColumnIndex[_mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPC_CALCULATION_VALUE)]]);
            this._bspreadutility.SetComboBoxCellTypeForColumn(this.bsprAutoCalc.ActiveSheet.Columns[(int)this._slColumnIndex[_mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPC_CALCULATION_OPTION)]], option);
            this._bspreadutility.SetComboBoxCellTypeForColumn(this.bsprAutoCalc.ActiveSheet.Columns[(int)this._slColumnIndex[_mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPC_CALCULATION_SIDED)]], Definition.CALCULATION_SIDED);

            this._llColumnMapping = new LinkedList();
            this.SetColumnMappingInfo(dsControlChart);
        }

        private void SetColumnMappingInfo(DataSet ds)   //CODE_MST_PP의 chart name과 MODEL_AUTOCALC_MST_SPC의 chart name을 mapping.
        {
            DataTable dsChart = ds.Tables[0];
            string name = string.Empty;

            foreach (DataRow row in dsChart.Rows)
            {
                name = row["NAME"].ToString();

                switch (name)
                {
                    case Definition.AUTOCALC_CHART_TYPE.EWMA_MEAN:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.EWMA_MEAN, Definition.AUTOCALC_COLUMN.EWMA_MEAN);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.EWMA_RANGE:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.EWMA_RANGE, Definition.AUTOCALC_COLUMN.EWMA_RANGE);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.EWMA_STDDEV:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.EWMA_STDDEV, Definition.AUTOCALC_COLUMN.EWMA_STDDEV);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.MA:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.MA, Definition.AUTOCALC_COLUMN.MA);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.MR:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.MR, Definition.AUTOCALC_COLUMN.MR);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.MSD:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.MSD, Definition.AUTOCALC_COLUMN.MSD);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.RANGE:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.RANGE, Definition.AUTOCALC_COLUMN.RANGE);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.RAW:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.RAW, Definition.AUTOCALC_COLUMN.RAW);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.STDDEV:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.STDDEV, Definition.AUTOCALC_COLUMN.STDDEV);
                        break;
                    case Definition.AUTOCALC_CHART_TYPE.XBAR:
                        this._llColumnMapping.Add(Definition.AUTOCALC_CHART_TYPE.XBAR, Definition.AUTOCALC_COLUMN.XBAR);
                        break;
                }
            }
        }

        private void MakeAutoCalculateData()
        {
            foreach (DataColumn dc in this._dtAutoCalc.Columns)
            {
                string colName = dc.ColumnName;
                int index = 0;

                foreach (object obj in _llColumnMapping.GetValueList())
                {
                    string chartName = (string)obj +"_";

                    if (colName.StartsWith(chartName) && !colName.Contains("RAWID"))
                    {
                        if (colName.EndsWith("_CL_YN"))     //Auto Calculation YN
                        {
                            if (this._dtAutoCalc.Rows[0][colName].ToString() == "Y")
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CL_YN].Value = Definition.VARIABLE_TRUE;
                            else
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CL_YN].Value = Definition.VARIABLE_FALSE;
                        }
                        
                        if (colName.EndsWith("_CALC_CNT"))  //Auto Calculation Count
                        {
                            this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_CNT].Value = this._dtAutoCalc.Rows[0][colName].ToString();
                        }
                        
                        if(colName.EndsWith("_CALC_OPTION"))     //Auto Calculation Limit to Use - Sigma, %, Constant
                        {
                            if (this._dtAutoCalc.Rows[0][colName].ToString().ToString() == "P")
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_OPTION].Value = this._LimitOptionList[LimitOption.PERCENT.ToString()].ToString();
                            }
                            else if (this._dtAutoCalc.Rows[0][colName].ToString() == "C")
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_OPTION].Value = LimitOption.CONSTANT.ToString();
                            }
                            else
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_OPTION].Value = LimitOption.SIGMA.ToString();
                            }
                        }

                        if (colName.EndsWith("_CALC_VALUE"))     //Auto Calculation value
                        {
                            if(!string.IsNullOrEmpty(this._dtAutoCalc.Rows[0][colName].ToString()))
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_VALUE].Value = this._dtAutoCalc.Rows[0][colName].ToString();
                        }
                        if (colName.EndsWith("_CALC_SIDED"))     //Auto Calculation Sided
                        {
                            if (this._dtAutoCalc.Rows[0][colName].ToString() == "L")
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_SIDED].Value = AutoCalcSideOption.LOWER.ToString();
                            }
                            else if (this._dtAutoCalc.Rows[0][colName].ToString() == "U")
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_SIDED].Value = AutoCalcSideOption.UPPER.ToString();
                            }
                            else
                            {
                                this.bsprAutoCalc.ActiveSheet.Cells[index, (int)AutoCalcColumnIndex.CALC_SIDED].Value = AutoCalcSideOption.BOTH.ToString();
                            }
                        }
                    }

                    index++;
                }
            }
        }


        public void InitializeDataButton()
        {
            this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
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

        public string AUTOCALC_PERIOD
        {
            get { return this.btxtAutoCalcPeriod.Text; }
            set { this.btxtAutoCalcPeriod.Text = value; }
        }

        public string CALC_COUNT
        {
            get { return this.btxtAutoCalcCount.Text; }
            set { this.btxtAutoCalcCount.Text = value; }
        }

        //SPC-658 Initial Count
        public string INITIAL_CALC_COUNT
        {
            get { return this.btxtAutoInitialCalcCount.Text;  }
            set { this.btxtAutoInitialCalcCount.Text = value; }
                
        }


        public string MIN_SAMPLES
        {
            get { return this.btxtMinSamples.Text; }
            set { this.btxtMinSamples.Text = value; }
        }

        public string DEFAULT_PERIOD
        {
            get { return this.btxtDefaultPeriod.Text; }
            set { this.btxtDefaultPeriod.Text = value; }
        }

        public string MAX_PERIOD
        {
            get { return this.btxtMaxPeriod.Text; }
            set { this.btxtMaxPeriod.Text = value; }
        }

        public string CONTROL_LIMIT
        {
            get { return this.btxtControlLimit.Text; }
            set { this.btxtControlLimit.Text = value; }
        }

        public string CONTROL_THRESHOLD
        {
            get { return this.btxtControlThreshold.Text; }
            set { this.btxtControlThreshold.Text = value; }
        }

        //SPC-1155 by stella
        public string AUTOCALC_OPTION
        {
            get 
            {
                string sOption = string.Empty;
                if (this._LimitOptionList.GetValueList().Contains(this.bComboOption.SelectedItem))
                {
                    sOption = this._LimitOptionList.GetKey(this.bComboOption.SelectedIndex).ToString();
                }

                if (sOption == LimitOption.SIGMA.ToString())
                    return "S";
                else if (sOption == LimitOption.PERCENT.ToString())
                    return "P";
                else if (sOption == LimitOption.CONSTANT.ToString())
                    return "C";
                else
                    return "S"; 
            }
        }

        public DataTable AUTOCALC_DATASET
        {
            get
            {
                return _dtAutoCalc;
            }
            set
            {
                _dtAutoCalc = value;

                if (_dtAutoCalc != null)
                {
                    //# Data가 없을 경우 Row를 하나 생성한다.
                    if (_dtAutoCalc.Rows.Count.Equals(0))
                    {
                        DataRow newRow = _dtAutoCalc.NewRow();
                        newRow[COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);

                        _dtAutoCalc.Rows.Add(newRow);
                    }

                    //# Default 값을 입력
                    if (_dtAutoCalc.Rows[0][COLUMN.AUTOCALC_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.AUTOCALC_PERIOD] = "30";

                    if (_dtAutoCalc.Rows[0][COLUMN.MIN_SAMPLES].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MIN_SAMPLES] = "30";

                    if (_dtAutoCalc.Rows[0][COLUMN.DEFAULT_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.DEFAULT_PERIOD] = "30";

                    if (_dtAutoCalc.Rows[0][COLUMN.MAX_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MAX_PERIOD] = "90";

                    if (_dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT] = "3";

                    if (_dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT_OPTION].Equals(DBNull.Value))
                    {
                        this.bComboOption.SelectedIndex = 0;
                        _dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT_OPTION] = AUTOCALC_OPTION;
                    }

                    if (_dtAutoCalc.Rows[0][COLUMN.USE_GLOBAL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.USE_GLOBAL_YN] = "Y";

                    if (_dtAutoCalc.Rows[0][COLUMN.CONTROL_THRESHOLD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.CONTROL_THRESHOLD] = "50";

                    if (_dtAutoCalc.Rows[0][COLUMN.STD_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.STD_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.RAW_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.RAW_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.MEAN_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MEAN_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.STD_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.STD_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.RANGE_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.RANGE_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.EWMA_M_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_M_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.EWMA_R_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_R_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.EWMA_S_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_S_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.MA_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MA_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.MS_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MS_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.MR_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.MR_CL_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.ZONE_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.ZONE_YN] = "Y";

                    if (_dtAutoCalc.Rows[0][COLUMN.SHIFT_CALC_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.SHIFT_CALC_YN] = "N";

                    if (_dtAutoCalc.Rows[0][COLUMN.WITHOUT_IQR_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.WITHOUT_IQR_YN] = "N";

                    //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일

                    if (_dtAutoCalc.Rows[0][COLUMN.THRESHOLD_OFF_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][COLUMN.THRESHOLD_OFF_YN] = 'N';
                    //
                    //SPC-908 TextBox추가
                    if (_cofingMode == ConfigMode.MODIFY || _cofingMode == ConfigMode.ROLLBACK)
                    {
                        if (_dtAutoCalc.Rows[0][COLUMN.RAW_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.RAW_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.MEAN_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.MEAN_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.STD_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.STD_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.RANGE_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.RANGE_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.EWMA_MEAN_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.EWMA_MEAN_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.EWMA_RANGE_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.EWMA_RANGE_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.EWMA_STD_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.EWMA_STD_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.MA_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.MA_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.MS_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.MS_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][COLUMN.MR_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][COLUMN.MR_CALC_COUNT] = 0;
                    }
                    //SPC-908 end
                    //# Control에 DataBinding 하기

                    //AUTOCALC_PERIOD
                    this.btxtAutoCalcPeriod.DataBindings.Add("Text", _dtAutoCalc, COLUMN.AUTOCALC_PERIOD);

                    //SPC-658 Initial Calc Count
                    this.btxtAutoInitialCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.INITIAL_CALC_COUNT, true, DataSourceUpdateMode.Never);
                    
                    //CALC_
                    this.btxtAutoCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.CALC_COUNT, true, DataSourceUpdateMode.Never);
                    this._CalcCount = CALC_COUNT;

                    //MIN_SAMPLES
                    this.btxtMinSamples.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MIN_SAMPLES);

                    //DEFAULT_PERIOD
                    this.btxtDefaultPeriod.DataBindings.Add("Text", _dtAutoCalc, COLUMN.DEFAULT_PERIOD);

                    //MAX_PERIOD
                    this.btxtMaxPeriod.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MAX_PERIOD);

                    //CONTROL_LIMIT
                    this.btxtControlLimit.DataBindings.Add("Text", _dtAutoCalc, COLUMN.CONTROL_LIMIT);

                    //CONTROL_THRESHOLD
                    this.btxtControlThreshold.DataBindings.Add("Text", _dtAutoCalc, COLUMN.CONTROL_THRESHOLD);

                    #region preCode
                    ////SPC-908 by Louis
                    //if (_cofingMode == ConfigMode.MODIFY || _cofingMode == ConfigMode.ROLLBACK)
                    //{                        
                    //    //RAW_CALC_COUNT
                    //    this.btxtRawCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.RAW_CALC_COUNT);

                    //    //STD_CALC_COUNT
                    //    this.btxtSTDCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.STD_CALC_COUNT);

                    //    //MEAN_CALC_COUNT
                    //    this.btxtMeanCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MEAN_CALC_COUNT);

                    //    //RANGE_CALC_COUNT
                    //    this.btxtRangeCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.RANGE_CALC_COUNT);

                    //    //EWMA_MEAN_CALC_COUNT
                    //    this.btxtEWMACalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.EWMA_MEAN_CALC_COUNT);

                    //    //EWMA_RANGE_CALC_COUNT
                    //    this.btxtEWMARangeCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.EWMA_RANGE_CALC_COUNT);

                    //    //EWMA_STD_CALC_COUNT
                    //    this.btxtEWMASTDCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.EWMA_STD_CALC_COUNT);

                    //    //MA_CALC_COUNT
                    //    this.btxtMACalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MA_CALC_COUNT);

                    //    //MS_CALC_COUNT
                    //    this.btxtMSCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MS_CALC_COUNT);

                    //    //MR_CALC_COUNT
                    //    this.btxtMRCalcCount.DataBindings.Add("Text", _dtAutoCalc, COLUMN.MR_CALC_COUNT);
                    //}
                    ////SPC-908 end
                   
                    ////STD_YN
                    //Binding bdSTDYN = new Binding("Checked", _dtAutoCalc, COLUMN.STD_YN);
                    //bdSTDYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdSTDYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkSTDCalcYN.DataBindings.Add(bdSTDYN);

                    ////RAW_CL_YN
                    //Binding bdRawCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.RAW_CL_YN);
                    //bdRawCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdRawCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkRawCalcYN.DataBindings.Add(bdRawCLYN);

                    ////MEAN_CL_YN
                    //Binding bdMeanCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.MEAN_CL_YN);
                    //bdMeanCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMeanCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkMeanCalcYN.DataBindings.Add(bdMeanCLYN);

                    ////STD_CL_YN
                    //Binding bdSTDCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.STD_CL_YN);
                    //bdSTDCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdSTDCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkSTDCLCalcYN.DataBindings.Add(bdSTDCLYN);

                    ////RANGE_CL_YN
                    //Binding bdRangeCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.RANGE_CL_YN);
                    //bdRangeCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdRangeCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkRangeCalcYN.DataBindings.Add(bdRangeCLYN);

                    ////EWMA_M_CL_YN
                    //Binding bdEWMAMCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.EWMA_M_CL_YN);
                    //bdEWMAMCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMAMCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkEWMAMCalcYN.DataBindings.Add(bdEWMAMCLYN);

                    ////EWMA_R_CL_YN
                    //Binding bdEWMARCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.EWMA_R_CL_YN);
                    //bdEWMARCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMARCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkEWMARCalcYN.DataBindings.Add(bdEWMARCLYN);

                    ////EWMA_S_CL_YN
                    //Binding bdEWMASCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.EWMA_S_CL_YN);
                    //bdEWMASCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdEWMASCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkEWMASCalcYN.DataBindings.Add(bdEWMASCLYN);

                    ////MA_CL_YN
                    //Binding bdMACLYN = new Binding("Checked", _dtAutoCalc, COLUMN.MA_CL_YN);
                    //bdMACLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMACLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkMACalcYN.DataBindings.Add(bdMACLYN);

                    ////MS_CL_YN
                    //Binding bdMSCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.MS_CL_YN);
                    //bdMSCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMSCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkMSCalcYN.DataBindings.Add(bdMSCLYN);

                    ////MR_CL_YN
                    //Binding bdMRCLYN = new Binding("Checked", _dtAutoCalc, COLUMN.MR_CL_YN);
                    //bdMRCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdMRCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkMRCalcYN.DataBindings.Add(bdMRCLYN);

                    ////ZONE_YN
                    //Binding bdZoneYN = new Binding("Checked", _dtAutoCalc, COLUMN.ZONE_YN);
                    //bdZoneYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    //bdZoneYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkZoneCalcYN.DataBindings.Add(bdZoneYN);

                    #endregion

                    this.MakeAutoCalculateData();

                    //SHIFT_CALC_YN
                    Binding bdShiftCalcYN = new Binding("Checked", _dtAutoCalc, COLUMN.SHIFT_CALC_YN);
                    bdShiftCalcYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdShiftCalcYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCalcShift.DataBindings.Add(bdShiftCalcYN);

                    //WITHOUT_IQR_YN
                    Binding bdIQRFilterYN = new Binding("Checked", _dtAutoCalc, COLUMN.WITHOUT_IQR_YN);
                    bdIQRFilterYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdIQRFilterYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCalcIQR.DataBindings.Add(bdIQRFilterYN);

                    //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                    //THRESHOLD_OFF_YN
                    Binding bdThresholdOffYN = new Binding("Checked", _dtAutoCalc, COLUMN.THRESHOLD_OFF_YN);
                    bdThresholdOffYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdThresholdOffYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkThresholdOffYN.DataBindings.Add(bdThresholdOffYN);

                    this.InitializeLayout(this._cofingMode);
                }
            }
        }

        public void InitializeLayout(ConfigMode configMode)
        {
            //SPC-908 by Louis
            switch (configMode)
            {
                case ConfigMode.DEFAULT:

                    this.bsprAutoCalc.ActiveSheet.Columns[(int)AutoCalcColumnIndex.CALC_CNT].Visible = false;
                    foreach (DataRow dr in this._dtAutoCalc.Rows)
                    {
                        if (dr[COLUMN.USE_GLOBAL_YN].ToString() == "Y")
                            this.bCheckGlobal.Checked = true;
                        else
                            this.bCheckGlobal.Checked = false;

                        if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "S")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.SIGMA.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "P")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.PERCENT.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "C")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.CONSTANT.ToString());
                    }
                    break;
                case ConfigMode.CREATE_MAIN:
                case ConfigMode.CREATE_MAIN_FROM:
                    this.bCheckGlobal.Checked = true;
                    foreach (DataRow dr in this._dtAutoCalc.Rows)
                    {
                        if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "S")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.SIGMA.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "P")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.PERCENT.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "C")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.CONSTANT.ToString());
                    }
                    break;
                case ConfigMode.CREATE_SUB:
                case ConfigMode.MODIFY:
                    foreach (DataRow dr in this._dtAutoCalc.Rows)
                    {
                        if (dr[COLUMN.USE_GLOBAL_YN].ToString() == "Y")
                        {
                            this.bCheckGlobal.Checked = true;
                            this.bComboOption.Enabled = true;
                            this.btxtControlLimit.Enabled = true;
                        }
                        else
                        {
                            this.bCheckGlobal.Checked = false;
                            this.bComboOption.Enabled = false;
                            this.btxtControlLimit.Enabled = false;
                        }

                        if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "S")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.SIGMA.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "P")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.PERCENT.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "C")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.CONSTANT.ToString());
                    }
                    break;

                case ConfigMode.VIEW:
                    foreach (DataRow dr in this._dtAutoCalc.Rows)
                    {
                        if (dr[COLUMN.USE_GLOBAL_YN].ToString() == "Y")
                            this.bCheckGlobal.Checked = true;
                        else
                            this.bCheckGlobal.Checked = false;

                        if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "S")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.SIGMA.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "P")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.PERCENT.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "C")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.CONSTANT.ToString());
                    }

                    this.gbAutoCalcSelection.Enabled = false;
                    this.gbAutoCalcSetting.Enabled = false;
                    break;
                default:
                    foreach (DataRow dr in this._dtAutoCalc.Rows)
                    {
                        if (dr[COLUMN.USE_GLOBAL_YN].ToString() == "Y")
                            this.bCheckGlobal.Checked = true;
                        else
                            this.bCheckGlobal.Checked = false;

                        if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "S")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.SIGMA.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "P")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.PERCENT.ToString());
                        else if (dr[COLUMN.CONTROL_LIMIT_OPTION].ToString() == "C")
                            this.bComboOption.SelectedIndex = this._LimitOptionList.GetKeyList().IndexOf(LimitOption.CONSTANT.ToString());
                    }
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


        public bool InputValidation(bool isMsgDisplay)
        {
            return this.InputValidation(isMsgDisplay, false);
        }
        
        public bool InputValidation(bool isMsgDisplay, bool isEachCheck)
        {
            bool result = true;
            bool calcCount = false;

            if (isEachCheck)
            {
                
                if (AUTOCALC_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_AUTO_CALC_PERIOD", null, null);
                    result = false;
                }

                if (MIN_SAMPLES.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_MIN_SAMPLE", null, null);
                    result = false;
                }

                if (DEFAULT_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_DEFUALT_PERIOD", null, null);
                    result = false;
                }

                if (MAX_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_MAX_PERIOD", null, null);
                    result = false;
                }

                if (CONTROL_LIMIT.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CONTROL_LIMIT", null, null);
                    result = false;
                }

                if (CONTROL_THRESHOLD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_THRESHOLD", null, null);
                    result = false;
                }
            }
            else
            {
                if (AUTOCALC_PERIOD.Length.Equals(0) || MIN_SAMPLES.Length.Equals(0) || DEFAULT_PERIOD.Length.Equals(0) ||
                    MAX_PERIOD.Length.Equals(0) || CONTROL_THRESHOLD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_AUTO_CALC_OPTION", null, null);
                    result = false;
                }

                if (double.Parse(CONTROL_THRESHOLD) > 99 || double.Parse(CONTROL_THRESHOLD) < 0 )
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM", null, null);
                    result = false;
                }
                if (CONTROL_THRESHOLD.Length > 5)
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_DECIMAL", null, null);
                    result = false;
                }

                if (CONTROL_THRESHOLD == "0")
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_THRESHOLD", null, null);
                    result = false;
                }

                if (!SetAutoCalculationSpread())
                    return false;

                if (this._CalcCount.Equals(null) || this._CalcCount.Equals(""))
                {
                    if (!CALC_COUNT.Equals(null) && !CALC_COUNT.Equals(""))
                    {
                        _dtAutoCalc.Rows[0][COLUMN.INITIAL_CALC_COUNT] = CALC_COUNT;
                        calcCount = true;
                    }
                }
                else if (CALC_COUNT.Equals(null) || CALC_COUNT.Equals(""))
                {
                    if (!this._CalcCount.Equals(null) && !this._CalcCount.Equals(""))
                    {
                        _dtAutoCalc.Rows[0][COLUMN.INITIAL_CALC_COUNT] = DBNull.Value;
                        calcCount = true;
                    }
                }
                else
                {
                    if (double.Parse(CALC_COUNT) != double.Parse(this._CalcCount))
                    {
                        _dtAutoCalc.Rows[0][COLUMN.INITIAL_CALC_COUNT] = CALC_COUNT;
                        calcCount = true;

                        
                    }
                    //else
                    //{
                    //    _dtAutoCalc.Rows[0][COLUMN.INITIAL_CALC_COUNT] = CALC_COUNT;
                    //}
                }
                if (_cofingMode == ConfigMode.MODIFY || _cofingMode == ConfigMode.ROLLBACK)
                {
                    if (calcCount)
                    {
                        _dtAutoCalc.Rows[0][COLUMN.RAW_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.MEAN_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.STD_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.RANGE_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_STD_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_MEAN_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.EWMA_RANGE_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.MA_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.MS_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][COLUMN.MR_CALC_COUNT] = 0;
                    }
                }
            }

            return result;
        }


        #endregion


        #region ::: User Defined Method.

        //spc-1191 by stella : Config의 autocalc가 uncheck 된 경우에도 Set Auto Caculation item의 값은 변경 될 수 있어야하므로 따로 메소드화.
        public bool SetAutoCalculationSpread()
        {
            bool result = true;

            //변경된 DATA를 다시 _dtAutoCalc 테이블에 넣어줌.
            foreach (FarPoint.Win.Spread.Row row in this.bsprAutoCalc.ActiveSheet.Rows)
            {
                string chartName = this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.NAME].Value.ToString();

                //~~CALC_CL_YN 값 설정
                string colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CL_YN.ToString();
                if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CL_YN].Value.ToString() == Definition.VARIABLE_TRUE)
                {
                    this._dtAutoCalc.Rows[0][colName] = 'Y';
                }
                else
                {
                    this._dtAutoCalc.Rows[0][colName] = 'N';
                }

                //Global Setting Check
                if (this.bCheckGlobal.Checked)
                {
                    //~~CALC_VALUE
                    colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CALC_VALUE.ToString();

                    if (CONTROL_LIMIT.Length.Equals(0))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_AUTO_CALC_OPTION", null, null);
                        return false;
                    }
                    this._dtAutoCalc.Rows[0][colName] = CONTROL_LIMIT;

                    //~~CALC_OPTION
                    colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CALC_OPTION.ToString();

                    if (AUTOCALC_OPTION.Length.Equals(0))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_OPTION", null, null);
                        result = false;
                    }
                    this._dtAutoCalc.Rows[0][colName] = AUTOCALC_OPTION;

                }
                else
                {
                    //~~CALC_VALUE
                    colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CALC_VALUE.ToString();

                    if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_VALUE].Value != null &&
                        (!string.IsNullOrEmpty(this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_VALUE].Value.ToString())))
                    {
                        this._dtAutoCalc.Rows[0][colName] = this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_VALUE].Value;
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CALC_VALUE", null, null);
                        return false;
                    }

                    //~~CALC_OPTION
                    colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CALC_OPTION.ToString();

                    if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_OPTION].Value.ToString() == LimitOption.CONSTANT.ToString())
                    {
                        this._dtAutoCalc.Rows[0][colName] = "C";
                    }
                    else if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_OPTION].Value.ToString() == this._LimitOptionList[LimitOption.PERCENT.ToString()].ToString())
                    {
                        this._dtAutoCalc.Rows[0][colName] = "P";
                    }
                    else
                    {
                        this._dtAutoCalc.Rows[0][colName] = "S";
                    }
                }

                //~~CALC_SIDED
                colName = this._llColumnMapping[chartName].ToString() + "_" + AutoCalcColumnIndex.CALC_SIDED.ToString();
                if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_SIDED].Value.ToString() == AutoCalcSideOption.UPPER.ToString())
                {
                    this._dtAutoCalc.Rows[0][colName] = "U";
                }
                else if (this.bsprAutoCalc.ActiveSheet.Cells[row.Index, (int)AutoCalcColumnIndex.CALC_SIDED].Value.ToString() == AutoCalcSideOption.LOWER.ToString())
                {
                    this._dtAutoCalc.Rows[0][colName] = "L";
                }
                else
                {
                    this._dtAutoCalc.Rows[0][colName] = "B";
                }

            }

            //USE GLOBAL YN 
            if (this.bCheckGlobal.Checked)
            {
                this._dtAutoCalc.Rows[0][COLUMN.USE_GLOBAL_YN] = "Y";
                this._dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT_OPTION] = AUTOCALC_OPTION;
            }
            else
            {
                this._dtAutoCalc.Rows[0][COLUMN.USE_GLOBAL_YN] = "N";
                this._dtAutoCalc.Rows[0][COLUMN.CONTROL_LIMIT_OPTION] = AUTOCALC_OPTION;
            }

            return result;
        }

        private void SetConfigRow(string argColumn, BTextBox bTxtBox)
        {
            if (bTxtBox.Text.Length > 0)
            {
                try
                {
                    _dtAutoCalc.Rows[0][argColumn] = double.Parse(bTxtBox.Text.Trim());
                }
                catch
                {
                    _dtAutoCalc.Rows[0][argColumn] = DBNull.Value;
                    bTxtBox.Text = null;
                }
            }
            else
                 _dtAutoCalc.Rows[0][argColumn] = DBNull.Value;
        }

        // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
        // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
        public void SetUnitOfValue(string sUnit)
        {
            blblMinSampleCnt.Text = sUnit;
        }
        #endregion

        #region ::: EventHandler

        private void btxtSpec_Validated(object sender, EventArgs e)
        {
            BTextBox btxtMst = (BTextBox)sender;

            if (btxtMst.Name.Equals(this.btxtAutoCalcCount.Name))
            {
                this.SetConfigRow(COLUMN.CALC_COUNT, btxtMst);
            }
        }

        #endregion

        private void bCheckGlobal_CheckedChanged(object sender, EventArgs e)
        {
            if (bCheckGlobal.Checked)
            {
                this.bsprAutoCalc.ActiveSheet.Columns[(int)AutoCalcColumnIndex.CALC_VALUE].Locked = true;
                this.bsprAutoCalc.ActiveSheet.Columns[(int)AutoCalcColumnIndex.CALC_OPTION].Locked = true;

                this.btxtControlLimit.Enabled = true;
                this.bComboOption.Enabled = true;
            }
            else
            {
                this.bsprAutoCalc.ActiveSheet.Columns[(int)AutoCalcColumnIndex.CALC_VALUE].Locked = false;
                this.bsprAutoCalc.ActiveSheet.Columns[(int)AutoCalcColumnIndex.CALC_OPTION].Locked = false;

                this.btxtControlLimit.Enabled = false;
                this.bComboOption.Enabled = false;
            }
        }


    }

}
