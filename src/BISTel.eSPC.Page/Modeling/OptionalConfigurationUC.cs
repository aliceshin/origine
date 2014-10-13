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
    public partial class OptionalConfigurationUC : BasePageUCtrl
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
        //private SortedList _slSpcModelingIndex = new SortedList();

        private int _ColIdx_SELECT;

        private int _ColIdx_CODE;
        private int _ColIdx_NAME;

        private ConfigMode _cofingMode;
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
        }

        private void InitializeCode()
        {
            LinkedList llconidtion = new LinkedList();

            //PARAM TYPE
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PARAM_CATEGORY);

            DataSet _dsParamCategory = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboParamCategory, _dsParamCategory, COLUMN.NAME, COLUMN.CODE, "", true);


            //AUTO TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PRIORITY);

            DataSet _dsPriority = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboPriority, _dsPriority, COLUMN.NAME, COLUMN.CODE, "", true);
        }


        public void InitializeDataButton()
        {
            //this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_MODELING_UC, "", this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
            //this.ApplyAuthory(this.bbtnList);
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
            //this._ColIdx_EQPID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_EQP_ID)];
            //this._ColIdx_ModuleID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_ID)];
            //this._ColIdx_OperationID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_OPERATION_ID)];
            //this._ColIdx_ProductID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PRODUCT_ID)];

            //this._Initialization.SetCheckColumnHeader(this.bsprData, 0);

            //OCAP LIST Binding
            LinkedList llconidtion = new LinkedList();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);

            DataSet dsControlChart = this._wsSPC.GetCodeData(llconidtion.GetSerialData());

            this.bsprDefaultChart.DataSet = dsControlChart;

            //Default로 모든 Chart를 선택
            for (int i = 0; i < bsprDefaultChart.ActiveSheet.RowCount; i++)
            {
                //spc-1141 by stella 노란색 색상표시 제거.
                this.bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, true);
            }
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
                        _sDefaultChartList = _ComUtil.ConvertDataColumnIntoStringList(dsSelectChartList.Tables[0], COLUMN.CODE, ";");
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
                            //spc-1141 by stella 노란색 색상표시 제거.
                            bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, true);
                        }
                        else
                        {
                            bsprDefaultChart.SetCellValue(i, _ColIdx_SELECT, false);
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
                    _dtConfigOPT.Rows[0][COLUMN.DEFAULT_CHART_LIST] = this.DEFAULT_CHART_LIST;

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
                        newRow[COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);

                        _dtConfigOPT.Rows.Add(newRow);
                    }

                    //# Control에 DataBinding 하기

                    //SPC_PARAM_CATEGORY_CD
                    this.bcboParamCategory.DataBindings.Add("SelectedValue", _dtConfigOPT, COLUMN.SPC_PARAM_CATEGORY_CD);

                    //SPC_PRIORITY_CD
                    this.bcboPriority.DataBindings.Add("SelectedValue", _dtConfigOPT, COLUMN.SPC_PRIORITY_CD);

                    //AUTO_CPK_YN
                    Binding bdAutoCPKYN = new Binding("Checked", _dtConfigOPT, COLUMN.AUTO_CPK_YN);
                    bdAutoCPKYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoCPKYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCpkYN.DataBindings.Add(bdAutoCPKYN);

                    //RESTRICT_SAMPLE_COUNT
                    this.btxtRestrictSample.DataBindings.Add("Text", _dtConfigOPT, COLUMN.RESTRICT_SAMPLE_COUNT);

                    //RESTRICT_SAMPLE_DAYS
                    this.btxtRestrictDays.DataBindings.Add("Text", _dtConfigOPT, COLUMN.RESTRICT_SAMPLE_DAYS);

                    //DEFAULT_CHART_LIST
                    this.DEFAULT_CHART_LIST = _dtConfigOPT.Rows[0][COLUMN.DEFAULT_CHART_LIST].ToString();

                    //RESTRINT_SAMPLE_HOURS
                    this.btxtRestrictHours.DataBindings.Add("Text", _dtConfigOPT, COLUMN.RESTRICT_SAMPLE_HOURS);
                }
            }
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

            double dSampleCnt = 0;
            double dDay = 0;
            double dHour = 0;

            if (this.btxtRestrictSample.Text != null && this.btxtRestrictSample.Text.Length < 6)    //MAX : 10000
            {
                if (this.btxtRestrictSample.Text.Length > 0)
                {
                    dSampleCnt = double.Parse(this.btxtRestrictSample.Text);

                    if (dSampleCnt > 10000)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_SAMPLE_CNT", null, null);
                        return false;
                    }
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_SAMPLE_CNT", null, null);
                return false;
            }

            if (this.btxtRestrictDays.Text != null && this.btxtRestrictDays.Text.Length < 3)    //MAX : 30
            {
                if (this.btxtRestrictDays.Text.Length > 0)
                {
                    dDay = double.Parse(this.btxtRestrictDays.Text);

                    if (dDay > 30)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_DAY", null, null);
                        return false;
                    }
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_DAY", null, null);
                return false;
            }

            if (this.btxtRestrictHours.Text != null && this.btxtRestrictHours.Text.Length < 4)  //MAX : 100
            {
                if (this.btxtRestrictHours.Text.Length > 0)
                {
                    dHour = double.Parse(this.btxtRestrictHours.Text);

                    if (dHour > 100)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_HOUR", null, null);
                        return false;
                    }
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_HOUR", null, null);
                return false;
            }

            return result;
        }
        #endregion

        #region ::: EventHandler

        #endregion

    }

}
