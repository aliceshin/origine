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
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using FarPoint.Excel;
using FarPoint.Win.Spread;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

using System.CodeDom;
using System.Globalization;


namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCCalculationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();

        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        private string _sLineRawid;
        private string _sLine;
        private string _sAreaRawid;
        private string _sArea;
        private string _sEQPModel;
        //private string _sEQPID;
        //private string _sModuleID;
        //private string _sRecipeID;
        //private string _sStepID;
        private string _sSPCModelRawid;
        private string _sSPCModelName;


        private DataSet _dsSPCModeData = new DataSet();
        private DataTable _dtTableOriginal = new DataTable();

        ChartInterface mChartVariable;
        SourceDataManager _dataManager = null;
        DataSet mDSChart = null;
        ChartUtility mChartUtil;
        LinkedList mllstChartSeriesVisibleType = null;

        private string mStartTime = string.Empty;
        private string mEndTime = string.Empty;

        //SPC-812
        public bool _sCalculation = true;

        DefaultRawChart chartBase;
        DefaultCalcChart chartCalcBase;

        int iValueIndex = -1;

        private string _sModelConfigRawID;
        private string _sModelConfigRawIDforSave;
        private string _sMainYNforSave;
        private string _sParamAlias;
        LinkedList mllstContextType;
        private string _sUSL = "";
        private string _sLSL = "";

        ContextMenu ctChart;
        ContextMenu ctSpread;
        ArrayList arrOutlierList = new ArrayList();

        private string _sChartType = string.Empty;
        LinkedList _mllstChart = new LinkedList();

        private List<int> lstTempForSetOulierPreview = new List<int>();

        private int _selectedRawID = -1;

        MultiLanguageHandler _lang;
        private string _sTarget;
        private bool _bUseComma;
       
        enum iColIdx
        {
            SELECT,
            RAWID,
            PARAM_ALIAS,
            MAIN_YN
        }

        enum OutlierLimitType
        {
            CONSTANT,
            VALUE,
            SIGMA,
            CONTROL
        }
        #endregion


        #region ::: Properties

        public ChartInterface ChartVariable
        {
            get { return this.mChartVariable; }
            set { this.mChartVariable = value; }
        }

        public SessionData SessionData
        {
            get { return this.sessionData; }
            set { this.sessionData = value; }
        }

        public string SModelConfigRawID
        {
            get { return this._sModelConfigRawID; }
            set { this._sModelConfigRawID = value; }
        }

        public string SParamAlias
        {
            get { return this._sParamAlias; }
            set { this._sParamAlias = value; }
        }


        #endregion


        #region ::: Constructor

        public SPCCalculationUC()
        {
            InitializeComponent();
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;
            //this.KeyOfPage = this.GetType().FullName;
            if (this.GetType().FullName == "BISTel.eSPC.Page.Modeling.MET.Modeling.SPCCalculationUC")
            {
                this.KeyOfPage = "BISTel.eSPC.Page.Modeling.MET.Modeling.SPCModelingUC";
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Page.Modeling.SPCCalculationUC")
            {
                this.KeyOfPage = "BISTel.eSPC.Page.Modeling.SPCModelingUC";
            }
            this.InitializePage();
        }


        public override void PageSearch(LinkedList llCondition)
        {
            //초기화
            this.InitializePageData();

            DataTable dt = new DataTable();

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            {
                dt = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_LINE];
                this._sLine = dt.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                this._sLineRawid = DCUtil.GetValueData(dt);
            }

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                dt = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_AREA];
                this._sAreaRawid = DCUtil.GetValueData(dt);
                this._sArea = dt.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_AREA", null, null);
                return;
            }

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
            {
                dt = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                this._sEQPModel = dt.Rows[0][DCUtil.VALUE_FIELD].ToString();

                if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                    return;
                }
                else
                {
                    DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                    this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                    this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                }
            }
            else
            {
                this._sEQPModel = "";

                if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                    return;
                }
                else
                {
                    DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                    this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                    this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                }
            }

            //if (llCondition.Contains(Definition.DynamicCondition_Condition_key.EQP_ID))
            //{
            //    dt = (DataTable)llCondition[Definition.DynamicCondition_Condition_key.EQP_ID];
            //    this._sEQPID = dt.Rows[0][DCUtil.VALUE_FIELD].ToString();
            //}
            //else
            //{
            //    MSGHandler.DisplayMessage(MSGType.Information, "Please Select EQP ID.");
            //    return;
            //}
            //if (llCondition.Contains(Definition.DynamicCondition_Condition_key.MODULE_ID))
            //{
            //    dt = (DataTable)llCondition[Definition.DynamicCondition_Condition_key.MODULE_ID];
            //    this._sModuleID = dt.Rows[0][DCUtil.VALUE_FIELD].ToString();
            //}
            //else
            //{
            //    MSGHandler.DisplayMessage(MSGType.Information, "Please Select MODULE ID.");
            //    return;
            //}
            //if (llCondition.Contains(Definition.DynamicCondition_Condition_key.RECIPE_ID))
            //{
            //    dt = (DataTable)llCondition[Definition.DynamicCondition_Condition_key.RECIPE_ID];
            //    this._sRecipeID = dt.Rows[0][DCUtil.VALUE_FIELD].ToString();
            //}
            //else
            //{
            //    MSGHandler.DisplayMessage(MSGType.Information, "Please Select RECIPE ID.");
            //    return;
            //}
            //if (llCondition.Contains(Definition.DynamicCondition_Condition_key.STEP_ID))
            //{
            //    dt = (DataTable)llCondition[Definition.DynamicCondition_Condition_key.STEP_ID];
            //    this._sStepID = dt.Rows[0][DCUtil.VALUE_FIELD].ToString();
            //}
            //else
            //{
            //    MSGHandler.DisplayMessage(MSGType.Information, "Please Select STEP ID.");
            //    return;
            //}

            //SPC-812
            this.ConfigListDataBinding(true);
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePopup()
        {
            this.InitializePage();
            this.ConfigListDataBindingPopup();
        }


        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.mChartVariable = new ChartInterface();
            this._dataManager = new SourceDataManager();
            this.mChartUtil = new ChartUtility();

            this._lang = MultiLanguageHandler.getInstance();

            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }

        public void InitializeLayout()
        {
            //spc-1281
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);

            this.bDtStart.Value = DateTime.Now.AddDays(-7).Subtract(TimeSpan.FromHours(1));
            this.bDtEnd.Value = DateTime.Now.Subtract(TimeSpan.FromHours(-1));
            ctChart = new ContextMenu();
            ctChart.MenuItems.Add("Set Outlier", new EventHandler(ChartMenuClick));
            ctChart.MenuItems.Add("UnSet Outlier", new EventHandler(ChartMenuClick));

            ctSpread = new ContextMenu();
            ctSpread.MenuItems.Add("Set Outlier", new EventHandler(SpreadMenuClick));
            ctSpread.MenuItems.Add("UnSet Outlier", new EventHandler(SpreadMenuClick));

            this.bcboOutlierType.SelectedIndex = 0;
        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetCodeData(lk.GetSerialData());

            if (!DataUtil.IsNullOrEmptyDataSet(this.mDSChart) && this.bcboChartType.Items.Count == 0)
            {
                _ComUtil.SetBComboBoxData(this.bcboChartType, this.mDSChart, COLUMN.CODE, COLUMN.CODE, "", true);

                foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
                {
                    if (!this._mllstChart.Contains(dr[COLUMN.CODE]))
                    {
                        this._mllstChart.Add(dr[COLUMN.CODE], dr[COLUMN.DESCRIPTION]);
                    }
                }
            }

            LinkedList _llstData = new LinkedList();
            DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
            this.mllstContextType = CommonPageUtil.SetContextType(dsContextType);
        }

        private void InitializeChartSeriesVisibleType()
        {
            this.mllstChartSeriesVisibleType = new LinkedList();
            Hashtable htButtonList = this._Initialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            for (int i = htButtonList.Count - 1; i >= 0; i--)
            {
                Hashtable htButtonAttributes = (Hashtable)htButtonList[i.ToString()];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();
                string DefaultVisible = htButtonAttributes[Definition.XML_VARIABLE_DEFAULTVALUE].ToString().ToUpper();

                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                {
                    this.mllstChartSeriesVisibleType.Add(this._mlthandler.GetVariable(sButtonName), true);
                }
            }

        }


        public void InitializeDataButton()
        {
            this.FunctionName = Definition.FUNC_KEY_SPC_CALCULATION;
        }



        public void InitializeBSpread()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.AutoGenerateColumns = true;
            this.bsprData.UseAutoSort = false;

            this.bsprChartData.ClearHead();
            this.bsprChartData.AddHeadComplete();
            this.bsprChartData.UseSpreadEdit = false;
            this.bsprChartData.AutoGenerateColumns = true;
            this.bsprChartData.UseAutoSort = false;
            this.bsprChartData.Locked = true;

            //SPC-761 By Louis - ContextMenu 삭제
            this.bsprChartData.UseGeneralContextMenu = false;
            //this.bsprChartData.ActiveSheet.
        }

        public void InitializeBTextBox()
        {
            this._sUSL = "";
            this._sLSL = "";
            this._sTarget = "";
            this.btxtSigma.Clear();
            this.btxtAvg.Clear();
            this.btxtLCL.Clear();
            this.btxtLSL.Clear();
            this.btxtSTD.Clear();
            this.btxtUCL.Clear();
            this.btxtUSL.Clear();
            this.btxtTarget.Clear();

            this.ntxtOutlierAverage.AllowDecimalSeperator = true;
        }

        public void InitializePageData()
        {
            this._sLineRawid = null;
            this._sAreaRawid = null;
            this._sEQPModel = null;

            this.InitializeBSpread();
            this.InitializeBTextBox();
            this.pnlChart.Controls.Clear();
            this._dtTableOriginal = new DataTable();
        }

        public void InitializeChart()
        {
            try
            {
                if (this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                {
                    _dataManager.RawDataTableOriginal = this.ChartVariable.dtParamData.Copy();
                    _dataManager.RawDataTable = this.ChartVariable.dtParamData.Copy();
                }

                this.pnlChart.Controls.Clear();

                chartBase = null;
                chartCalcBase = null;

                Steema.TeeChart.Tools.CursorTool _cursorTool = null;

                string strKey = this._sChartType;
                string strValue = this._mllstChart[strKey].ToString();

                if (this._sChartType == Definition.CHART_TYPE.RAW)
                {
                    chartBase = new DefaultRawChart(_dataManager);
                    chartBase.ClearChart();
                    chartBase.Title = strValue;
                    chartBase.Name = strKey;
                    chartBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                    chartBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                    chartBase.SPCChartTitlePanel.Name = strKey;
                    chartBase.SPCChartTitlePanel.MaxResizable = false;
                    chartBase.URL = this.URL;
                    chartBase.Dock = System.Windows.Forms.DockStyle.Fill;

                    chartBase.ContextMenu = ctChart;
                    this.bsprChartData.ContextMenu = ctSpread;
                    chartBase.ParentControl = this.pnlChart;
                    chartBase.StartDateTime = ChartVariable.dateTimeStart;
                    chartBase.EndDateTime = ChartVariable.dateTimeEnd;
                    chartBase.mllstContextType = this.mllstContextType;
                    chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);

                    bool result = false;
                    if(bchkSampling.Checked)
                    {
                        //result = chartBase.DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text));
                        result = chartBase.DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text), int.Parse(btxtWSampleCnt.Text));
                    }
                    else
                    {
                        result = chartBase.DrawSPCChart();
                    }

                    if (result)
                    {
                        _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                        _cursorTool.Active = true;
                        _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                        _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        _cursorTool.Pen.Color = Color.Gray;
                        _cursorTool.FollowMouse = true;
                        chartBase.SPCChart.Chart.Tools.Add(_cursorTool);

                        chartBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                        chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                        this.pnlChart.Controls.Add(chartBase);

                        if (chartBase.IsWaferColumn)
                        {
                            DataTable dt = chartBase.dtDataSource.Copy();
                            if (dt.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                            {
                                dt.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                            }
                            if (dt.Columns.Contains("TIME"))
                            {
                                dt.Columns.Remove("TIME");
                            }
                            if (dt.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
                            {
                                dt.Columns.Remove(CommonChart.COLUMN_NAME_SEQ_INDEX);
                            }

                            this.bsprChartData.DataSource = dt;
                        }
                        else
                        {
                            this.bsprChartData.DataSource = CommonPageUtil.ExcelExport(_dataManager.RawDataTableOriginal, ChartVariable.lstRawColumn);
                        }

                        //Column Size 조절
                        for (int cIdx = 0; cIdx < this.bsprChartData.ActiveSheet.Columns.Count; cIdx++)
                        {
                            this.bsprChartData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = ((DataSet)this.bsprChartData.DataSource).Tables[0].Columns[cIdx].ToString();
                        }

                        int rowCount = 0;

                        rowCount = chartBase.dtDataSource.Rows.Count;

                        DataTable dtDataDisplay = new DataTable();
                        dtDataDisplay.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX);
                        dtDataDisplay.Columns.Add(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
                        dtDataDisplay.Columns.Add(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);

                        for (int i = 0; i < rowCount; i++)
                        {
                            if (i == 0 || i == rowCount - 1)
                            {
                                DataRow dr = dtDataDisplay.NewRow();
                                dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = chartBase.dtDataSource.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX];
                                dr[Definition.CHART_COLUMN.PREVIEWUPPERLIMIT] = 0;
                                dr[Definition.CHART_COLUMN.PREVIEWLOWERLIMIT] = 0;
                                dtDataDisplay.Rows.Add(dr);
                            }
                        }

                        SeriesInfo siUpperLimit = new SeriesInfo(typeof(ExtremeBFastLine), Definition.CHART_COLUMN.PREVIEWUPPERLIMIT,
                                                     dtDataDisplay, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
                        SeriesInfo siLowerLimit = new SeriesInfo(typeof(ExtremeBFastLine), Definition.CHART_COLUMN.PREVIEWLOWERLIMIT,
                                                                 dtDataDisplay, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);

                        ChartUtility util = new ChartUtility();
                        siUpperLimit.ShowLegend = false;
                        siLowerLimit.ShowLegend = false;
                        siUpperLimit.SeriesColor = util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.SPECPREVIEW);
                        siLowerLimit.SeriesColor = util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.SPECPREVIEW);

                        Series su = chartBase.SPCChart.AddSeries(siUpperLimit);
                        su.Visible = false;
                        ((ExtremeBFastLine)su).LinePen.Width = 2;
                        ((ExtremeBFastLine)su).Pointer.Visible = false;

                        Series sl = chartBase.SPCChart.AddSeries(siLowerLimit);
                        sl.Visible = false;
                        ((ExtremeBFastLine)sl).LinePen.Width = 2;
                        ((ExtremeBFastLine)sl).Pointer.Visible = false;

                        chartBase.SPCChart.Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                else
                {
                    chartCalcBase = new DefaultCalcChart(_dataManager);
                    chartCalcBase.ClearChart();
                    chartCalcBase.Title = strValue;
                    chartCalcBase.Name = strKey;
                    chartCalcBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                    chartCalcBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                    chartCalcBase.SPCChartTitlePanel.Name = strKey;
                    chartCalcBase.SPCChartTitlePanel.MaxResizable = false;
                    chartCalcBase.URL = this.URL;
                    chartCalcBase.Dock = System.Windows.Forms.DockStyle.Fill;

                    chartCalcBase.ContextMenu = ctChart;
                    this.bsprChartData.ContextMenu = ctSpread;
                    chartCalcBase.ParentControl = this.pnlChart;
                    chartCalcBase.StartDateTime = ChartVariable.dateTimeStart;
                    chartCalcBase.EndDateTime = ChartVariable.dateTimeEnd;
                    //chartCalcBase.mllstContextType = this.mllstContextType;
                    chartCalcBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);
                    
                    bool result = false;
                    if(bchkSampling.Checked)
                    {
                        result = chartCalcBase.DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text));
                    }
                    else
                    {
                        result = chartCalcBase.DrawSPCChart();
                    }

                    if (result)
                    {
                        _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                        _cursorTool.Active = true;
                        _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                        _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        _cursorTool.Pen.Color = Color.Gray;
                        _cursorTool.FollowMouse = true;
                        chartCalcBase.SPCChart.Chart.Tools.Add(_cursorTool);

                        chartCalcBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                        chartCalcBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                        this.pnlChart.Controls.Add(chartCalcBase);

                        if (chartCalcBase.IsWaferColumn)
                        {
                            DataTable dt = chartCalcBase.dtDataSource.Copy();
                            if (dt.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                            {
                                dt.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                            }
                            if (dt.Columns.Contains("TIME"))
                            {
                                dt.Columns.Remove("TIME");
                            }
                            if (dt.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
                            {
                                dt.Columns.Remove(CommonChart.COLUMN_NAME_SEQ_INDEX);
                            }

                            this.bsprChartData.DataSource = dt;
                        }
                        else
                        {
                            this.bsprChartData.DataSource = CommonPageUtil.ExcelExport(_dataManager.RawDataTable, ChartVariable.lstRawColumn);
                        }

                        //Column Size 조절
                        for (int cIdx = 0; cIdx < this.bsprChartData.ActiveSheet.Columns.Count; cIdx++)
                        {
                            this.bsprChartData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = ((DataSet)this.bsprChartData.DataSource).Tables[0].Columns[cIdx].ToString();
                        }

                        int rowCount = 0;

                        rowCount = chartCalcBase.dtDataSource.Rows.Count;

                        DataTable dtDataDisplay = new DataTable();
                        dtDataDisplay.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX);
                        dtDataDisplay.Columns.Add(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
                        dtDataDisplay.Columns.Add(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);

                        for (int i = 0; i < rowCount; i++)
                        {
                            if (i == 0 || i == rowCount - 1)
                            {
                                DataRow dr = dtDataDisplay.NewRow();
                                dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = chartCalcBase.dtDataSource.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX];
                                dr[Definition.CHART_COLUMN.PREVIEWUPPERLIMIT] = 0;
                                dr[Definition.CHART_COLUMN.PREVIEWLOWERLIMIT] = 0;
                                dtDataDisplay.Rows.Add(dr);
                            }
                        }

                        SeriesInfo siUpperLimit = new SeriesInfo(typeof(ExtremeBFastLine), Definition.CHART_COLUMN.PREVIEWUPPERLIMIT,
                                                     dtDataDisplay, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
                        SeriesInfo siLowerLimit = new SeriesInfo(typeof(ExtremeBFastLine), Definition.CHART_COLUMN.PREVIEWLOWERLIMIT,
                                                                 dtDataDisplay, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);
                        
                        ChartUtility util = new ChartUtility();
                        siUpperLimit.ShowLegend = false;
                        siLowerLimit.ShowLegend = false;
                        siUpperLimit.SeriesColor = util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.SPECPREVIEW);
                        siLowerLimit.SeriesColor = util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.SPECPREVIEW);

                        Series su = chartCalcBase.SPCChart.AddSeries(siUpperLimit);
                        su.Visible = false;
                        ((ExtremeBFastLine)su).LinePen.Width = 2;
                        ((ExtremeBFastLine)su).Pointer.Visible = false;

                        Series sl = chartCalcBase.SPCChart.AddSeries(siLowerLimit);
                        sl.Visible = false;
                        ((ExtremeBFastLine)sl).LinePen.Width = 2;
                        ((ExtremeBFastLine)sl).Pointer.Visible = false;

                        chartCalcBase.SPCChart.Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
            }
        }
        #endregion

        protected void ChartMenuClick(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;

            try
            {
                if (this.bsprChartData.ActiveSheet.Rows.Count <= 0)
                    return;

                bool isPointMarking = false;
                if((this.chartCalcBase != null && this.chartCalcBase.IsPointMarking)
                    || (this.chartBase != null && this.chartBase.IsPointMarking))
                {
                    isPointMarking = true;
                }
                if (this.iValueIndex < 0 && isPointMarking == false)
                    return;
                

                if (menuItem.Text == "Set Outlier")
                {
                    if(this.chartCalcBase != null && this.chartCalcBase.IsPointMarking)
                    {
                        SetOutlier(this.chartCalcBase.GetSelectedShape());
                    }
                    else if(this.chartBase != null && this.chartBase.IsPointMarking)
                    {
                        SetOutlier(this.chartBase.GetSelectedShape());
                    }
                    else
                    {
                        SetOutlier(iValueIndex, true);
                    }
                }
                else if((menuItem.Text == "UnSet Outlier"))
                {
                    if(this.chartCalcBase != null && this.chartCalcBase.IsPointMarking)
                    {
                        UnsetOutlier(this.chartCalcBase.GetSelectedShape());
                    }
                    else if(this.chartBase != null && this.chartBase.IsPointMarking)
                    {
                        UnsetOutlier(this.chartBase.GetSelectedShape());
                    }
                    else
                    {
                        UnsetOutlier(iValueIndex, true);
                    }
                }
            }
            catch(Exception ex)
            {
                LogHandler.LogWriteException(Definition.APPLICATION_NAME, ex.Message, ex);
            }
        }

        protected void SpreadMenuClick(System.Object sender, System.EventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;

            try
            {
                if (this.bsprChartData.ActiveSheet.RowCount <= 0)
                    return;

                FarPoint.Win.Spread.Model.CellRange range = this.bsprChartData.ActiveSheet.GetSelection(0);
                if (menuItem.Text == "Set Outlier")
                {
                    if (range.RowCount == -1)
                    {
                        for (int i = 0; i < bsprChartData.ActiveSheet.RowCount; i++)
                        {
                            SetOutlier(i, true);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < range.RowCount; i++)
                        {
                            SetOutlier(range.Row + i, true);
                        }
                    }
                }
                else if (menuItem.Text == "UnSet Outlier")
                {
                    if (range.RowCount == -1)
                    {
                        for (int i = 0; i < bsprChartData.ActiveSheet.RowCount; i++)
                        {
                            UnsetOutlier(i, true);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < range.RowCount; i++)
                        {
                            UnsetOutlier(range.Row + i, true);
                        }
                    }
                }
            }
            catch
            {
            }
        }


        #region ::: User Defined Method.

        private void ConfigListDataBinding(bool main)
        {
            try
            {
                //this.MsgShow(COMMON_MSG.Query_Data);

                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);
                object objDataSet = null;
                

                

                //_ds = _wsSPC.GetSPCControlChartData(_llstSearch.GetSerialData());
               

                //SPC-750 Calculate Spec/Control Limit
                if (main)
                {
                    _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this._sLineRawid);
                    _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this._sAreaRawid);
                    _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this._sEQPModel);
                    _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, this._sSPCModelRawid);
                    //_dsSPCModeData = _wsSPC.GetSPCCalcModelData(_llstSearchCondition.GetSerialData());
                    objDataSet = ach.SendWait(_wsSPC, "GetSPCCalcModelData", new object[] { _llstSearchCondition.GetSerialData() });
                }
                else
                {
                    _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, this.bsprData.ActiveSheet.Cells[0, 1].Value);
                    //_dsSPCModeData = _wsSPC.GetSPCCalcModelDataSave(_llstSearchCondition.GetSerialData());
                    objDataSet = ach.SendWait(_wsSPC, "GetSPCCalcModelDataSave", new object[] { _llstSearchCondition.GetSerialData() });
                }
                //_llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this._sEQPID);
                //_llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, this._sModuleID);
                //_llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.RECIPE_ID, this._sRecipeID);
                //_llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.STEP_ID, this._sStepID);

                EESProgressBar.CloseProgress(this);
                if (objDataSet != null)
                {
                    _dsSPCModeData = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }


                

                if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
                {
                    //this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);


                DataTable dtMaster = _dsSPCModeData.Tables[TABLE.MODEL_MST_SPC];
                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(COLUMN.RAWID);
                dtSPCModelChartList.Columns.Add(COLUMN.SPC_MODEL_NAME);
                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);

                if (!main)
                {
                    foreach (DataRow drContext in dtContext.Rows)
                    {
                        dtSPCModelChartList.Columns.Add(drContext[COLUMN.CONTEXT_KEY].ToString());
                    }
                }
                else
                {

                    //CONTEXT COLUMN 생성
                    DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);

                    if (drConfigs != null && drConfigs.Length > 0)
                    {
                        DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                        foreach (DataRow drMainContext in drMainContexts)
                        {
                            dtSPCModelChartList.Columns.Add(drMainContext["CONTEXT_KEY"].ToString());
                        }
                    }
                }

                //dtSPCModelChartList.Columns.Add("EQP_ID");
                //dtSPCModelChartList.Columns.Add("MODULE_ID");
                //dtSPCModelChartList.Columns.Add("RECIPE_ID");
                //dtSPCModelChartList.Columns.Add("STEP_ID");
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.TARGET);
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_CONTROL);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_CONTROL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.STD_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.STD_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RANGE_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RANGE_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_MEAN_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_MEAN_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_RANGE_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_RANGE_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_STD_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_STD_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MA_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MA_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MS_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MS_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MR_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MR_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_TECHNICAL_LIMIT);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_TECHNICAL_LIMIT);
                dtSPCModelChartList.Columns.Add(COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.ACTIVATION_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_DTTS);
                dtSPCModelChartList.Columns.Add(COLUMN.LAST_UPDATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.LAST_UPDATE_DTTS);

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[COLUMN.RAWID] = drConfig[COLUMN.RAWID].ToString();
                    drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();

                    DataRow[] drMaster = dtMaster.Select(string.Format("{0} = '{1}'", COLUMN.RAWID, drConfig[COLUMN.MODEL_RAWID]));
                    drChartList[COLUMN.SPC_MODEL_NAME] = drMaster[0][COLUMN.SPC_MODEL_NAME].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    foreach (DataRow drContext in drContexts)
                    {
                        //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                        drChartList[drContext[COLUMN.CONTEXT_KEY].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    }

                    //MODEL 정보                
                    drChartList[COLUMN.UPPER_SPEC] = drConfig[COLUMN.UPPER_SPEC].ToString();
                    drChartList[COLUMN.LOWER_SPEC] = drConfig[COLUMN.LOWER_SPEC].ToString();
                    drChartList[COLUMN.TARGET] = drConfig[COLUMN.TARGET].ToString();
                    drChartList[COLUMN.UPPER_CONTROL] = drConfig[COLUMN.UPPER_CONTROL].ToString();
                    drChartList[COLUMN.LOWER_CONTROL] = drConfig[COLUMN.LOWER_CONTROL].ToString();
                    drChartList[COLUMN.RAW_UCL] = drConfig[COLUMN.RAW_UCL].ToString();
                    drChartList[COLUMN.RAW_LCL] = drConfig[COLUMN.RAW_LCL].ToString();
                    
                    drChartList[COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[COLUMN.INTERLOCK_YN], "N", true);
                    drChartList[COLUMN.ACTIVATION_YN] = _ComUtil.NVL(drConfig[COLUMN.ACTIVATION_YN], "N", true);
                    drChartList[COLUMN.CREATE_BY] = drConfig[COLUMN.CREATE_BY].ToString();
                    drChartList[COLUMN.CREATE_DTTS] = drConfig[COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();
                    drChartList[COLUMN.LAST_UPDATE_BY] = drConfig[COLUMN.LAST_UPDATE_BY].ToString();
                    drChartList[COLUMN.LAST_UPDATE_DTTS] = drConfig[COLUMN.LAST_UPDATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.LAST_UPDATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();

                    drChartList[COLUMN.STD_UCL] = drConfig[COLUMN.STD_UCL].ToString();
                    drChartList[COLUMN.STD_LCL] = drConfig[COLUMN.STD_LCL].ToString();
                    drChartList[COLUMN.RANGE_UCL] = drConfig[COLUMN.RANGE_UCL].ToString();
                    drChartList[COLUMN.RANGE_LCL] = drConfig[COLUMN.RANGE_LCL].ToString();
                    drChartList[COLUMN.EWMA_MEAN_UCL] = drConfig[COLUMN.EWMA_M_UCL].ToString();
                    drChartList[COLUMN.EWMA_MEAN_LCL] = drConfig[COLUMN.EWMA_M_LCL].ToString();
                    drChartList[COLUMN.EWMA_RANGE_UCL] = drConfig[COLUMN.EWMA_R_UCL].ToString();
                    drChartList[COLUMN.EWMA_RANGE_LCL] = drConfig[COLUMN.EWMA_R_LCL].ToString();
                    drChartList[COLUMN.EWMA_STD_UCL] = drConfig[COLUMN.EWMA_S_UCL].ToString();
                    drChartList[COLUMN.EWMA_STD_LCL] = drConfig[COLUMN.EWMA_S_LCL].ToString();
                    drChartList[COLUMN.MA_UCL] = drConfig[COLUMN.MA_UCL].ToString();
                    drChartList[COLUMN.MA_LCL] = drConfig[COLUMN.MA_LCL].ToString();
                    drChartList[COLUMN.MS_UCL] = drConfig[COLUMN.MS_UCL].ToString();
                    drChartList[COLUMN.MS_LCL] = drConfig[COLUMN.MS_LCL].ToString();
                    drChartList[COLUMN.MR_UCL] = drConfig[COLUMN.MR_UCL].ToString();
                    drChartList[COLUMN.MR_LCL] = drConfig[COLUMN.MR_LCL].ToString();
                    drChartList[COLUMN.UPPER_TECHNICAL_LIMIT] = drConfig[COLUMN.UPPER_TECHNICAL_LIMIT].ToString();
                    drChartList[COLUMN.LOWER_TECHNICAL_LIMIT] = drConfig[COLUMN.LOWER_TECHNICAL_LIMIT].ToString();

                    dtSPCModelChartList.Rows.Add(drChartList);
                }

                dtSPCModelChartList.AcceptChanges();

                this.bsprData.ClearHead();
                this.bsprData.Locked = true;
                this.bsprData.AutoGenerateColumns = false;
                for (int i = 0; i < dtSPCModelChartList.Columns.Count; i++)
                {
                    string sColumn = dtSPCModelChartList.Columns[i].ColumnName.ToString();
                    //this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text = sColumn;
                    if (i == (int) iColIdx.SELECT)
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
                    }
                    else if(sColumn == COLUMN.UPPER_CONTROL)
                    {
                        string headerName = MSGHandler.GetVariable("SPC_MEAN_UCL");
                        this.bsprData.AddHead(i, headerName, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                    else if(sColumn == COLUMN.LOWER_CONTROL)
                    {
                        string headerName = MSGHandler.GetVariable("SPC_MEAN_LCL");
                        this.bsprData.AddHead(i, headerName, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                    else
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                }
                this.bsprData.AddHeadComplete();

                this.bsprData.ActiveSheet.Columns[(int)iColIdx.SELECT].Locked = false;
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.RAWID].Visible = false;

                this.bsprData.DataSet = dtSPCModelChartList;
                this._dtTableOriginal = dtSPCModelChartList.Copy();

                for(int i=0; i<dtSPCModelChartList.Rows.Count; i++)
                {
                    this.bsprData.ActiveSheet.Cells[i, (int) iColIdx.SELECT].Locked = false;
                }

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprData.ActiveSheet.Columns[cIdx].Width = this.bsprData.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprData.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprData.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprData.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprData.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }
            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);

                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                //this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
                //this.MsgClose();
            }
        }

        private void ConfigListDataBindingPopup()
        {
            try
            {
                this.MsgShow(COMMON_MSG.Query_Data);

                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, this._sModelConfigRawID);
                _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                _dsSPCModeData = _wsSPC.GetSPCCalcModelDataPopup(_llstSearchCondition.GetSerialData());

                if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                DataTable dtMaster = _dsSPCModeData.Tables[TABLE.MODEL_MST_SPC];
                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(COLUMN.RAWID);
                dtSPCModelChartList.Columns.Add(COLUMN.SPC_MODEL_NAME);
                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);
                foreach (DataRow drContext in dtContext.Rows)
                {
                    dtSPCModelChartList.Columns.Add(drContext[COLUMN.CONTEXT_KEY].ToString());
                }
                
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.TARGET);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_DTTS);

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[COLUMN.RAWID] = drConfig[COLUMN.RAWID].ToString();
                    drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();

                    DataRow[] drMaster = dtMaster.Select(string.Format("{0} = '{1}'", COLUMN.RAWID, drConfig[COLUMN.MODEL_RAWID]));
                    drChartList[COLUMN.SPC_MODEL_NAME] = drMaster[0][COLUMN.SPC_MODEL_NAME].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    foreach (DataRow drContext in drContexts)
                    {
                        //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                        drChartList[drContext[COLUMN.CONTEXT_KEY].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    }

                    //MODEL 정보                
                    drChartList[COLUMN.UPPER_SPEC] = drConfig[COLUMN.UPPER_SPEC].ToString();
                    drChartList[COLUMN.LOWER_SPEC] = drConfig[COLUMN.LOWER_SPEC].ToString();
                    drChartList[COLUMN.TARGET] = drConfig[COLUMN.TARGET].ToString();
                    drChartList[COLUMN.RAW_UCL] = drConfig[COLUMN.RAW_UCL].ToString();
                    drChartList[COLUMN.RAW_LCL] = drConfig[COLUMN.RAW_LCL].ToString();
                    drChartList[COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[COLUMN.INTERLOCK_YN], "N", true);

                    drChartList[COLUMN.CREATE_BY] = drConfig[COLUMN.CREATE_BY].ToString();
                    drChartList[COLUMN.CREATE_DTTS] = drConfig[COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();

                    dtSPCModelChartList.Rows.Add(drChartList);
                }

                dtSPCModelChartList.AcceptChanges();

                this.bsprData.DataSet = dtSPCModelChartList;
                this._dtTableOriginal = dtSPCModelChartList.Copy();

                this.bsprData.Locked = true;
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.SELECT].Locked = false;
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.RAWID].Visible = false;

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = dtSPCModelChartList.Columns[cIdx].ToString();
                    this.bsprData.ActiveSheet.Columns[cIdx].Width = this.bsprData.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprData.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprData.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprData.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprData.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }

                if (this.bsprData.ActiveSheet.Rows.Count > 0)
                {
                    this.bsprData.ActiveSheet.Cells[0, 0].Value = true;
                }
            }
            catch (Exception ex)
            {
                this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                this.MsgClose();
            }
        }

        #endregion

        /// <summary>
        /// Check if a series is a data series
        /// </summary>
        /// <param name="title">Title of series</param>
        /// <returns></returns>
        private bool IsDataSeriesOfCurrentChartType(string title)
        {
            if (chartBase != null)
            {
                if (title == "RAW")
                {
                    return true;
                }
                return false;
            }
            else if (chartCalcBase != null)
            {
                if ((this._sChartType == Definition.CHART_TYPE.MSD && title == Definition.BLOB_FIELD_NAME.MSD)
                    || (this._sChartType == Definition.CHART_TYPE.XBAR && title == Definition.CHART_COLUMN.AVG)
                    || (this._sChartType == Definition.CHART_TYPE.STDDEV && title == Definition.CHART_COLUMN.STDDEV)
                    || (this._sChartType == Definition.CHART_TYPE.RANGE && title == Definition.CHART_COLUMN.RANGE)
                    || (this._sChartType == Definition.CHART_TYPE.EWMA_MEAN && title == Definition.CHART_COLUMN.AVG)
                    || (this._sChartType == Definition.CHART_TYPE.EWMA_RANGE && title == Definition.CHART_COLUMN.RANGE)
                    || (this._sChartType == Definition.CHART_TYPE.EWMA_STDDEV && title == Definition.CHART_COLUMN.STDDEV)
                    || (this._sChartType == Definition.CHART_TYPE.MA && title == Definition.CHART_COLUMN.MA)
                    || (this._sChartType == Definition.CHART_TYPE.MSD && title == Definition.CHART_COLUMN.MSD)
                    || (this._sChartType == Definition.CHART_TYPE.MR && title == Definition.CHART_COLUMN.MR))
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        /// <summary>
        /// Set outlier a point
        /// </summary>
        /// <param name="rowIndex">Row index of bsprChartData or Series</param>
        /// <returns>if set, returns true, if already be set, returns false</returns>
        private bool SetOutlier(int rowIndex, bool isApplySpread)
        {
            SeriesCollection seriesCollection = GetSeriesCollection();
            if(seriesCollection == null)
            {
                Exception ex = new Exception("seriesCollection is null");
                LogHandler.LogWriteException(Definition.APPLICATION_NAME, ex.Message, ex);
                throw ex;
            }

            DataTable dtDataSource = null;
            if(chartBase != null)
                dtDataSource = chartBase.dtDataSource;
            else if(chartCalcBase != null)
                dtDataSource = _dataManager.RawDataTableOriginal;
            else
            {
                Exception ex = new Exception("Can not assign dtDataSource");
                LogHandler.LogWriteException(Definition.APPLICATION_NAME, ex.Message, ex);
                throw ex;
            }

            bool result = false;
            foreach(Series s in seriesCollection)
            {
                if(!IsDataSeriesOfCurrentChartType(s.Title))
                    continue;

                s[rowIndex].Color = Color.Red;
                if (isApplySpread)
                {
                    this.bsprChartData.ActiveSheet.Rows[rowIndex].BackColor = Color.LightGreen;

                    string sOutlier = GetStringForSavingOutlier(dtDataSource, rowIndex);
                    if (!arrOutlierList.Contains(sOutlier))
                    {
                        arrOutlierList.Add(sOutlier);
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Set outlier points in the shape
        /// </summary>
        /// <param name="shape">shape containing to be set outlier points</param>
        /// <returns>if set, returns true, if already be set, returns false</returns>
        private bool SetOutlier(Steema.TeeChart.Styles.Shape shape)
        {
            List<Series> series = GetAllSeriesOfChart();
            if(series == null) return false;

            if(shape.X0 > shape.X1)
            {
                double temp = shape.X1;
                shape.X1 = shape.X0;
                shape.X0 = temp;
            }
            if(shape.Y1 > shape.Y0)
            {
                double temp = shape.Y0;
                shape.Y0 = shape.Y1;
                shape.Y1 = temp;
            }

            bool result = false;
            foreach(Series s in series)
            {
                if(IsDataSeriesOfCurrentChartType(s.Title))
                {
                    DataTable dtSource = ((DataSet)s.DataSource).Tables[0];
                    DataRow[] dtRows = dtSource.Select(string.Format("X > {0} and X < {1} and Y > {2} and Y < {3}", shape.X0, shape.X1, shape.Y1, shape.Y0));
                    for (int i = 0; i < dtRows.Length; i++)
                    {
                        SetOutlier(int.Parse(dtRows[i]["X"].ToString()), true);
                    }
                    result = true;
                }
            }

            return result;
        }

        private List<Series> GetAllSeriesOfChart()
        {
            if(this.chartCalcBase != null)
            {
                return this.chartCalcBase.SPCChart.GetAllSeries();
            }
            else if(this.chartBase != null)
            {
                return this.chartBase.SPCChart.GetAllSeries();
            }
            else
                return null;
        }

        /// <summary>
        /// Unset outlier a point
        /// </summary>
        /// <param name="rowIndex">Row index of bsprChartData or Series</param>
        private void UnsetOutlier(int rowIndex, bool isApplySpread)
        {    
            SeriesCollection seriesCollection = GetSeriesCollection();
            if(seriesCollection == null)
                return;

            DataTable dtDataSource = null;
            if(chartBase != null)
                dtDataSource = chartBase.dtDataSource;
            else if(chartCalcBase != null)
                dtDataSource = _dataManager.RawDataTableOriginal;
            else
                return;

            foreach(Series s in seriesCollection)
            {
                if (!IsDataSeriesOfCurrentChartType(s.Title))
                    continue;

                s[rowIndex].Color = Color.Black;
                if (isApplySpread)
                {
                    this.bsprChartData.ActiveSheet.Rows[rowIndex].ResetBackColor();

                    string sOutlier = GetStringForSavingOutlier(dtDataSource, rowIndex);
                    if (arrOutlierList.Contains(sOutlier))
                    {
                        arrOutlierList.Remove(sOutlier);
                    }
                }
            }
        }

        // <summary>
        /// Unset outlier points in the shape
        /// </summary>
        /// <param name="shape">shape containing to be unset outlier points</param>
        /// <returns>if unset, returns true, if already be unset, returns false</returns>
        private bool UnsetOutlier(Steema.TeeChart.Styles.Shape shape)
        {
            List<Series> series = GetAllSeriesOfChart();
            if(series == null) return false;

            if(shape.X0 > shape.X1)
            {
                double temp = shape.X1;
                shape.X1 = shape.X0;
                shape.X0 = temp;
            }
            if(shape.Y1 > shape.Y0)
            {
                double temp = shape.Y0;
                shape.Y0 = shape.Y1;
                shape.Y1 = temp;
            }

            bool result = false;
            foreach(Series s in series)
            {
                if(IsDataSeriesOfCurrentChartType(s.Title))
                {
                    DataTable dtSource = ((DataSet)s.DataSource).Tables[0];
                    DataRow[] dtRows = dtSource.Select(string.Format("X > {0} and X < {1} and Y > {2} and Y < {3}", shape.X0, shape.X1, shape.Y1, shape.Y0));
                    for (int i = 0; i < dtRows.Length; i++)
                    {
                        UnsetOutlier(int.Parse(dtRows[i]["X"].ToString()), true);
                    }
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Check the row is outlier
        /// </summary>
        /// <param name="rowIndex">index of row</param>
        /// <returns>if it is outlier it returns true, if not false</returns>
        private bool IsOutlier(int rowIndex)
        {
            SeriesCollection seriesCollection = GetSeriesCollection();
            if(seriesCollection == null)
                throw new Exception();

            DataTable dtDataSource = null;
            if(chartBase != null)
                dtDataSource = chartBase.dtDataSource;
            else if(chartCalcBase != null)
                dtDataSource = _dataManager.RawDataTableOriginal;
            else
                throw new Exception();

            foreach(Series s in seriesCollection)
            {
                if (!IsDataSeriesOfCurrentChartType(s.Title))
                    continue;

                string sOutlier = GetStringForSavingOutlier(dtDataSource, rowIndex);
                if (arrOutlierList.Contains(sOutlier))
                {
                    return true;
                }
                return false;
            }

            throw new Exception();
        }

        /// <summary>
        /// Get string value for saving outlier to outlier list
        /// </summary>
        /// <param name="dt">DataTable having lot ID and substarate ID</param>
        /// <param name="rowIndex">Target row index to make string</param>
        /// <returns>return string value for saving outlier</returns>
        private string GetStringForSavingOutlier(DataTable dt, int rowIndex)
        {
            string sLOTID = "";
            string sSubstrateID = "";

            if (dt.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                sLOTID = dt.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

            if (dt.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                sSubstrateID = dt.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

            return sLOTID + "^" + sSubstrateID;
        }

        private SeriesCollection GetSeriesCollection()
        {
            if (chartBase != null)
            {
                return chartBase.SPCChart.Chart.Series;
            }
            else if(chartCalcBase != null)
            {
                return chartCalcBase.SPCChart.Chart.Series;
            }
            else
                return null;
        }

        public void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)iColIdx.SELECT)
                return;

            if ((bool)this.bsprData.GetCellValue(e.Row, (int)iColIdx.SELECT) == true)
            {
                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                {
                    if (e.Row == i)
                    {
                        if (e.Column != (int)iColIdx.SELECT)
                            this.bsprData.ActiveSheet.Cells[i, (int)iColIdx.SELECT].Value = 1;

                        int.TryParse(this.bsprData.GetCellText(e.Row, (int) iColIdx.RAWID), out _selectedRawID);
                        continue;
                    }
                    this.bsprData.ActiveSheet.Cells[i, (int)iColIdx.SELECT].Value = 0;
                }
            }
        }

        public void bbtnSearch_Click(object sender, EventArgs e)
        {
            this.InitializeBTextBox();
            this._dataManager = new SourceDataManager();

            if (bsprData.ActiveSheet.RowCount <= 0) return;

            int iRowIndex = -1;

            ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                return;
            }
            else if (alCheckRowIndex.Count > 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                return;
            }
            else
            {
                iRowIndex = (int)alCheckRowIndex[0];
            }

            if(bchkSampling.Checked)
            {
                if(string.IsNullOrEmpty(btxtSampleCnt.Text) ||
                    string.IsNullOrEmpty(btxtSamplePeriod.Text) ||
                    (bcboChartType.Text == "RAW" && string.IsNullOrEmpty(btxtWSampleCnt.Text)))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SAMPLE_CONDITION", null, null);
                    return;
                }

                //spc-1188 by stella : period, sample count의 Max값 먼저 체크후 0 체크.
                if (Convert.ToDouble(btxtSamplePeriod.Text) > 100)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_SAMPLE_PERIOD", null, null);
                    return;
                }

                if (Convert.ToDouble(btxtSampleCnt.Text) > 10000 || (bcboChartType.Text == "RAW" && Convert.ToDouble(btxtWSampleCnt.Text) > 10000))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_CNT", null, null);
                    return;
                }

                if (int.Parse(btxtSampleCnt.Text) == 0 ||
                    int.Parse(btxtSamplePeriod.Text) == 0 ||
                    (bcboChartType.Text == "RAW" && int.Parse(btxtWSampleCnt.Text) == 0))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_CONDITION", null, null);
                    return;
                }
                
            }

            if(this.bDtStart.Value > this.bDtEnd.Value)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD",null, null);
                return;
            }

            this.mChartVariable = new ChartInterface();
            this.mStartTime = CommonPageUtil.CalcStartDate(this.bDtStart.Value.ToString(Definition.DATETIME_FORMAT));
            this.mEndTime = CommonPageUtil.CalcEndDate(this.bDtEnd.Value.ToString(Definition.DATETIME_FORMAT));
            this.mChartVariable.MODEL_CONFIG_RAWID = this.bsprData.ActiveSheet.Cells[iRowIndex, 1].Text;
            this.mChartVariable.MAIN_YN = this.bsprData.ActiveSheet.Cells[iRowIndex, 4].Text;
            for (int i = 0; i < this.bsprData.ActiveSheet.ColumnCount; i++)
            {
                if (this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text == "UPPER_SPEC")
                {
                    this._sUSL = this.bsprData.ActiveSheet.Cells[iRowIndex, i].Text;
                }
                else if (this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text == "LOWER_SPEC")
                {
                    this._sLSL = this.bsprData.ActiveSheet.Cells[iRowIndex, i].Text;
                }
                else if (this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text == "TARGET")
                {
                    this._sTarget = this.bsprData.ActiveSheet.Cells[iRowIndex, i].Text;
                }
            }

            this.bsprChartData.ClearHead();
            this.bsprChartData.AddHeadComplete();
            this.bsprChartData.UseSpreadEdit = false;
            this.bsprChartData.AutoGenerateColumns = true;
            this.bsprChartData.UseAutoSort = false;

            this.PROC_DataBinding();
        }

        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (chartBase != null)
                {
                    BaseRawChart baseChart = FindSPCChart((Control)sender);
                    Annotation ann;
                    if (baseChart is DefaultRawChart)
                    {
                        for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                        {
                            if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Annotation))
                            {
                                ann = baseChart.SPCChart.Chart.Tools[i] as Annotation;
                                if (ann.Active)
                                {
                                    ann.Active = false;
                                }
                            }
                        }
                    }
                }
                else if (chartCalcBase != null)
                {
                    BaseCalcChart baseCalcChart = FindSPCCalcChart((Control)sender);
                    Annotation ann;
                    if (baseCalcChart is DefaultCalcChart)
                    {
                        for (int i = 0; i < baseCalcChart.SPCChart.Chart.Tools.Count; i++)
                        {
                            if (baseCalcChart.SPCChart.Chart.Tools[i].GetType() == typeof(Annotation))
                            {
                                ann = baseCalcChart.SPCChart.Chart.Tools[i] as Annotation;
                                if (ann.Active)
                                {
                                    ann.Active = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
            finally
            {

            }
        }

        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip
                if (chartBase != null)
                {
                    BaseRawChart baseChart = FindSPCChart((Control)sender);
                    if (baseChart is DefaultRawChart)
                    {
                        //mChartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);
                        iValueIndex = valueIndex;

                        string strCol = s.Title;
                        if (strCol == Definition.CHART_COLUMN.USL_LSL)
                            strCol = Definition.CHART_COLUMN.USL;
                        else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                            strCol = Definition.CHART_COLUMN.UCL;

                        string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();
                        int _iSEQIdx = Convert.ToInt32(seriesInfo.SeriesData.Rows[valueIndex][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());

                        for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                        {
                            if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                            {
                                Annotation ann = (Annotation)baseChart.SPCChart.Chart.Tools[i];
                                if (valueIndex == _iSEQIdx)
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseChart.NAME));
                                else
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, _iSEQIdx, baseChart.NAME));
                                break;
                            }
                        }
                    }
                }
                else if (chartCalcBase != null)
                {
                    BaseCalcChart baseCalcChart = FindSPCCalcChart((Control)sender);
                    if (baseCalcChart is DefaultCalcChart)
                    {
                        //mChartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);
                        iValueIndex = valueIndex;

                        string strCol = s.Title;
                        if (strCol == Definition.CHART_COLUMN.USL_LSL)
                            strCol = Definition.CHART_COLUMN.USL;
                        else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                            strCol = Definition.CHART_COLUMN.UCL;

                        string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();
                        int _iSEQIdx = Convert.ToInt32(seriesInfo.SeriesData.Rows[valueIndex][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());

                        for (int i = 0; i < baseCalcChart.SPCChart.Chart.Tools.Count; i++)
                        {
                            if (baseCalcChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                            {
                                Annotation ann = (Annotation)baseCalcChart.SPCChart.Chart.Tools[i];
                                if (valueIndex == _iSEQIdx)
                                    this.mChartUtil.ShowAnnotate(baseCalcChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseCalcChart.NAME));
                                else
                                    this.mChartUtil.ShowAnnotate(baseCalcChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, _iSEQIdx, baseCalcChart.NAME));
                                break;
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                //iValueIndex = -1;
            }
        }

        private void PROC_DataBinding()
        {
            DataSet _ds = null;
            CreateChartDataTable _createChartDT = null;
            DataTable mDTChartData = null;
            _sModelConfigRawIDforSave = "";
            _sMainYNforSave = "";
            try
            {

                mDTChartData = new DataTable();
                _createChartDT = new CreateChartDataTable();

                //this.MsgShow(COMMON_MSG.Query_Data);

                LinkedList lnkChartSearchData = new LinkedList();

                lnkChartSearchData.Clear();
                lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.mStartTime);
                lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.mEndTime);
                lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetSPCControlChartData", new object[] { lnkChartSearchData.GetSerialData() });

                EESProgressBar.CloseProgress(this);

                if (objDataSet != null)
                {
                    _ds = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                //_ds = _wsSPC.GetSPCControlChartData(lnkChartSearchData.GetSerialData());

                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                }
                else
                {
                    EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                    lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST, "");
                    mDTChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, lnkChartSearchData, false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (DataUtil.IsNullOrEmptyDataTable(mDTChartData))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    }
                    else
                    {
                        this._sChartType = this.bcboChartType.Text;
                        _sModelConfigRawIDforSave = mChartVariable.MODEL_CONFIG_RAWID;
                        _sMainYNforSave = mChartVariable.MAIN_YN;
                        mDTChartData = DataUtil.DataTableImportRow(mDTChartData.Select(null, Definition.CHART_COLUMN.TIME));
                        mChartVariable.lstDefaultChart.Clear();
                        mChartVariable.complex_yn = Definition.VARIABLE_Y;
                        mChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        mChartVariable.dateTimeStart = DateTime.Parse(this.mStartTime);
                        mChartVariable.dateTimeEnd = DateTime.Parse(this.mEndTime);
                        _createChartDT.COMPLEX_YN = Definition.VARIABLE_Y;
                        if(this._sChartType == Definition.CHART_TYPE.RAW)
                            mChartVariable.dtParamData = _createChartDT.GetMakeDataTablePopup(mDTChartData);
                        else
                            mChartVariable.dtParamData = _createChartDT.GetMakeDataTable(mDTChartData);

                        mChartVariable.lstRawColumn = _createChartDT.lstRawColumn;
                        if (mDTChartData != null) mDTChartData.Dispose();
                        if (_createChartDT != null) _createChartDT = null;
                    }
                }
                this.InitializeCode();
                this.InitializeChartSeriesVisibleType();
                this.InitializeChart();
                if (arrOutlierList.Count > 0)
                {
                    if (chartBase != null)
                    {
                        Series RawSeries = null;
                        foreach (Series s in chartBase.SPCChart.Chart.Series)
                        {
                            if (s.Title == "RAW")
                            {
                                RawSeries = s;
                            }
                        }

                        if (this.bsprChartData != null && this.bsprChartData.ActiveSheet.RowCount > 0 && RawSeries != null)
                        {
                            for (int i = 0; i < chartBase.dtDataSource.Rows.Count; i++)
                            {
                                string sLOTID = "";
                                string sSubstrateID = "";
                                string sOutlier = "";

                                if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                                    sLOTID = chartBase.dtDataSource.Rows[i][Definition.CHART_COLUMN.LOT_ID].ToString();

                                if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                                    sSubstrateID = chartBase.dtDataSource.Rows[i][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

                                sOutlier = sLOTID + "^" + sSubstrateID;

                                if (arrOutlierList.Contains(sOutlier))
                                {
                                    this.bsprChartData.ActiveSheet.Rows[i].BackColor = Color.LightGreen;
                                    RawSeries[i].Color = Color.Red;
                                }
                            }
                        }
                    }
                    else if (chartCalcBase != null)
                    {
                        Series sSeries = null; 
                        foreach (Series s in chartCalcBase.SPCChart.Chart.Series)
                        {
                            if (s.Title != Definition.CHART_COLUMN.UCL && s.Title != Definition.CHART_COLUMN.LCL && s.Title != Definition.CHART_COLUMN.USL && s.Title != Definition.CHART_COLUMN.LSL && s.Title != Definition.CHART_COLUMN.TARGET && s.Title != Definition.CHART_COLUMN.MIN && s.Title != Definition.CHART_COLUMN.MAX && s.Title != Definition.CHART_COLUMN.PREVIEWUPPERLIMIT && s.Title != Definition.CHART_COLUMN.PREVIEWLOWERLIMIT)
                            {
                                sSeries = s;
                            }
                        }

                        if (this.bsprChartData != null && this.bsprChartData.ActiveSheet.RowCount > 0 && sSeries != null)
                        {
                            for (int i = 0; i < chartCalcBase.dtDataSource.Rows.Count; i++)
                            {
                                string sLOTID = "";
                                string sSubstrateID = "";
                                string sOutlier = "";

                                if (_dataManager.RawDataTableOriginal.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                                    sLOTID = _dataManager.RawDataTableOriginal.Rows[i][Definition.CHART_COLUMN.LOT_ID].ToString();

                                if (_dataManager.RawDataTableOriginal.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                                    sSubstrateID = _dataManager.RawDataTableOriginal.Rows[i][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

                                sOutlier = sLOTID + "^" + sSubstrateID;

                                if (arrOutlierList.Contains(sOutlier))
                                {
                                    this.bsprChartData.ActiveSheet.Rows[i].BackColor = Color.LightGreen;
                                    sSeries[i].Color = Color.Red;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);
                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                //this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.MsgClose();
            }
        }

        private BaseRawChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;
            while (parentControl != null)
            {
                if (parentControl is BaseRawChart)
                    return parentControl as BaseRawChart;
                else
                    parentControl = parentControl.Parent;
            }

            return null;
        }

        private BaseCalcChart FindSPCCalcChart(Control ctl)
        {
            Control parentControl = ctl.Parent;
            while (parentControl != null)
            {
                if (parentControl is BaseCalcChart)
                    return parentControl as BaseCalcChart;
                else
                    parentControl = parentControl.Parent;
            }

            return null;
        }

        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            if (chartBase != null)
            {
                if (chartBase.IsWaferColumn)
                {
                    string _sEQPID = "-";
                    string _sLOTID = "-";
                    string _sOperationID = "-";
                    string _sSubstrateID = "-";
                    string _sProductID = "-";
                    string _sTime = "-";
                    string _sParamList = "-";
                    string _sRawValue = "-";
                    string _sRawUSL = "-";
                    string _sRawUCL = "-";
                    string _sRawTarget = "-";
                    string _sRawLCL = "-";
                    string _sRawLSL = "-";

                    if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                        _sEQPID = chartBase.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

                    if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                        _sLOTID = chartBase.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

                    if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                        _sOperationID = chartBase.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

                    if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                        _sSubstrateID = chartBase.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

                    if (chartBase.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                        _sProductID = chartBase.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("TIME"))
                        _sTime = chartBase.dtDataSource.Rows[rowIndex]["TIME"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains(COLUMN.PARAM_LIST))
                        _sParamList = chartBase.dtDataSource.Rows[rowIndex][COLUMN.PARAM_LIST].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW"))
                        _sRawValue = chartBase.dtDataSource.Rows[rowIndex]["RAW"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW_USL"))
                        _sRawUSL = chartBase.dtDataSource.Rows[rowIndex]["RAW_USL"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW_UCL"))
                        _sRawUCL = chartBase.dtDataSource.Rows[rowIndex]["RAW_UCL"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW_TARGET"))
                        _sRawTarget = chartBase.dtDataSource.Rows[rowIndex]["RAW_TARGET"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW_LCL"))
                        _sRawLCL = chartBase.dtDataSource.Rows[rowIndex]["RAW_LCL"].ToString();

                    if (chartBase.dtDataSource.Columns.Contains("RAW_LSL"))
                        _sRawLSL = chartBase.dtDataSource.Rows[rowIndex]["RAW_LSL"].ToString();


                    sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
                    sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
                    sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
                    sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
                    sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);
                    sb.AppendFormat("{0} : {1}\r\n", "TIME", _sTime);
                    sb.AppendFormat("{0} : {1}\r\n", "PARAM LIST", _sParamList);
                    sb.AppendFormat("{0} : {1}\r\n", "VALUE", _sRawValue);
                    sb.AppendFormat("{0} : {1}\r\n", "USL", _sRawUSL);
                    sb.AppendFormat("{0} : {1}\r\n", "UCL", _sRawUCL);
                    sb.AppendFormat("{0} : {1}\r\n", "TARGET", _sRawTarget);
                    sb.AppendFormat("{0} : {1}\r\n", "LCL", _sRawLCL);
                    sb.AppendFormat("{0} : {1}\r\n", "LSL", _sRawLSL);
                }
                else
                {
                    string _sEQPID = "-";
                    string _sLOTID = "-";
                    string _sOperationID = "-";
                    string _sSubstrateID = "-";
                    string _sProductID = "-";

                    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                        _sEQPID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

                    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                        _sLOTID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

                    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                        _sOperationID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

                    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                        _sSubstrateID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

                    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                        _sProductID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();


                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);


                    llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                    for (int i = 0; i < llstChartSeries.Count; i++)
                    {
                        string sKey = llstChartSeries.GetKey(i).ToString();
                        string sValue = llstChartSeries.GetValue(i).ToString();

                        if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
                            sValue.Equals(Definition.CHART_COLUMN.LSL) ||
                            sValue.Equals(Definition.CHART_COLUMN.UCL) ||
                            sValue.Equals(Definition.CHART_COLUMN.LCL) ||
                            sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;

                        if (_dataManager.RawDataTable.Columns.Contains(sKey))
                        {
                            if (_dataManager.RawDataTable.Rows[rowIndex][sKey] != null && _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString().Length > 0)
                            {
                                if (strCol.Contains("wafer"))
                                {
                                    if (strChartName == Definition.CHART_TYPE.RAW)
                                    {
                                        if (sKey.Equals(Definition.CHART_TYPE.RAW))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (sKey.Contains("wafer"))
                                            {
                                                if (sKey.Contains(strCol + "^"))
                                                {
                                                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                                }
                                            }
                                            else
                                            {
                                                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                    }
                                }
                                else
                                {
                                    sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                }
                            }
                        }
                    }
                }
            }
            else if (chartCalcBase != null)
            {
                string _sEQPID = "-";
                string _sLOTID = "-";
                string _sOperationID = "-";
                string _sSubstrateID = "-";
                string _sProductID = "-";

                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                {
                    _sEQPID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();
                    if (_sEQPID.Length > 0 && _sEQPID.Split(';').Length > 1)
                    {
                        _sEQPID = _sEQPID.Split(';')[0];
                    }
                }

                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                {
                    _sLOTID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();
                    if (_sLOTID.Length > 0 && _sLOTID.Split(';').Length > 1)
                    {
                        _sLOTID = _sLOTID.Split(';')[0];
                    }
                }

                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                {
                    _sOperationID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                    if (_sOperationID.Length > 0 && _sOperationID.Split(';').Length > 1)
                    {
                        _sOperationID = _sOperationID.Split(';')[0];
                    }
                }

                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                {
                    _sSubstrateID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();
                    if (_sSubstrateID.Length > 0 && _sSubstrateID.Split(';').Length > 1)
                    {
                        _sSubstrateID = _sSubstrateID.Split(';')[0];
                    }
                }


                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                {
                    _sProductID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                    if (_sProductID.Length > 0 && _sProductID.Split(';').Length > 1)
                    {
                        _sProductID = _sProductID.Split(';')[0];
                    }
                }


                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
                sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);

                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString();

                    if (sValue.Equals(Definition.CHART_COLUMN.MIN) ||
                        sValue.Equals(Definition.CHART_COLUMN.MAX) ||
                        sValue.Contains("wafer")
                        ) continue;

                    if (_dataManager.RawDataTable.Columns.Contains(sKey))
                    {
                        if (_dataManager.RawDataTable.Rows[rowIndex][sKey] != null && _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString().Length > 0)
                        {
                            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                        }
                    }
                }

                    
            }
            llstChartSeries = null;

            return sb.ToString();
        }

        public void bbtnCalculation_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_SAMPLE_DATA_USE_BTN", null, null);
                    return;
                }

                if (this.btxtSigma.Text.Trim().Length == 0)
                {
                    if (bcboOptionList.Text == "SIGMA")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SIGMA", null, null);
                        return;
                    }
                    else if (bcboOptionList.Text == "%")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_PERCENT", null, null);
                        return;
                    }
                    else if (bcboOptionList.Text == "CONSTANT")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CONSTANT", null, null);
                        return;
                    }
                }

                if (bcboOptionList.Text.Equals("SIGMA"))
                {
                    double dsigma = 0;

                    bool isParseSigma = double.TryParse(this.btxtSigma.Text.Trim(), out dsigma);
                    if (!isParseSigma)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"sigma"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false);

                    if (dsValueList.Length == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CALC_CONTROL_LIMIT", null, null);
                        return;
                    }

                    double avg = StStat.mean(dsValueList);
                    double std = StStat.std(dsValueList);

                    double dUCL = avg + (std*dsigma);
                    double dLCL = avg - (std*dsigma);

                    this.btxtSTD.Text = std.ToString();
                    this.btxtAvg.Text = avg.ToString();
                    this.btxtLCL.Text = dLCL.ToString();
                    this.btxtUCL.Text = dUCL.ToString();
                    this.btxtUSL.Text = this._sUSL;
                    this.btxtLSL.Text = this._sLSL;
                    this.btxtTarget.Text = this._sTarget;
                }
                else if (bcboOptionList.Text.Equals("CONSTANT"))
                {
                    double dconstant = 0;

                    bool isParseConstant = double.TryParse(this.btxtSigma.Text.Trim(), out dconstant);
                    if (!isParseConstant)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"constant"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false);

                    if (dsValueList.Length == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CALC_CONTROL_LIMIT", null, null);
                        return;
                    }

                    double avg = StStat.mean(dsValueList);
                    double std = StStat.std(dsValueList);

                    double dUCL = avg + dconstant;
                    double dLCL = avg - dconstant;

                    this.btxtSTD.Text = std.ToString();
                    this.btxtAvg.Text = avg.ToString();
                    this.btxtLCL.Text = dLCL.ToString();
                    this.btxtUCL.Text = dUCL.ToString();
                    this.btxtUSL.Text = this._sUSL;
                    this.btxtLSL.Text = this._sLSL;
                    this.btxtTarget.Text = this._sTarget;
                }
                else if (bcboOptionList.Text.Equals("%"))
                {
                    double dPercentage = 0;

                    bool isParsePercent = double.TryParse(this.btxtSigma.Text.Trim(), out dPercentage);
                    if (!isParsePercent)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"Percent"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false);

                    if (dsValueList.Length == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CALC_CONTROL_LIMIT", null, null);
                        return;
                    }

                    double avg = StStat.mean(dsValueList);
                    double std = StStat.std(dsValueList);

                    double dUCL = avg + avg * dPercentage / 100;
                    double dLCL = avg - avg * dPercentage / 100;

                    this.btxtSTD.Text = std.ToString();
                    this.btxtAvg.Text = avg.ToString();
                    this.btxtLCL.Text = dLCL.ToString();
                    this.btxtUCL.Text = dUCL.ToString();
                    this.btxtUSL.Text = this._sUSL;
                    this.btxtLSL.Text = this._sLSL;
                    this.btxtTarget.Text = this._sTarget;
                }

                double dUCLTemp = Convert.ToDouble(btxtUCL.Text);
                double dLCLTemp = Convert.ToDouble(btxtLCL.Text);

                
                

                if (this._sChartType == Definition.CHART_TYPE.RAW || this._sChartType == Definition.CHART_TYPE.XBAR)
                {
                    
                    //if ((btxtUSL.Text != null && btxtLSL.Text != null) && (btxtUSL.Text != "" && btxtLSL.Text != ""))
                    //{
                    //    double dUSLTemp = Convert.ToDouble(btxtUSL.Text);
                    //    double dLSLTemp = Convert.ToDouble(btxtLSL.Text); 
                    //    if (dUCLTemp > dUSLTemp)
                    //    {
                    //        MSGHandler.DisplayMessage(MSGType.Information, "Calculated UCL value is bigger than USL value.");
                    //    }
                    //    else if (dLCLTemp < dLSLTemp)
                    //    {
                    //        MSGHandler.DisplayMessage(MSGType.Information, "Calculated LCL value is smaller than LSL value.");
                    //    }
                    //}
                    if (btxtUSL.Text != null && btxtUSL.Text != "")
                    {
                        double dUSLTemp = Convert.ToDouble(btxtUSL.Text);
                        if (dUCLTemp > dUSLTemp)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CALC_VALUE_BIGGER", new string[]{"UCL", "USL"}, null);
                        }
                    }
                    if (btxtLSL.Text != null && btxtLSL.Text != "")
                    {
                        double dLSLTemp = Convert.ToDouble(btxtLSL.Text);
                        if (dLCLTemp < dLSLTemp)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CALC_VALUE_SMALLER", new string[]{"LCL", "LSL"}, null);
                        }
                    }
                }    
            }
            catch
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ERROR_CALC", null, null);
            }

        }

        private string GetBLOBFieldNameFromChartType(string chartType)
        {
            switch(chartType)
            {
                case Definition.CHART_TYPE.RAW :
                    return Definition.BLOB_FIELD_NAME.RAW;
                case Definition.CHART_TYPE.XBAR :
                    return Definition.BLOB_FIELD_NAME.MEAN;
                case Definition.CHART_TYPE.STDDEV :
                    return Definition.BLOB_FIELD_NAME.STDDEV;
                case Definition.CHART_TYPE.RANGE :
                    return Definition.BLOB_FIELD_NAME.RANGE;
                case Definition.CHART_TYPE.EWMA_MEAN :
                    return Definition.BLOB_FIELD_NAME.EWMAMEAN;
                case Definition.CHART_TYPE.EWMA_RANGE :
                    return Definition.BLOB_FIELD_NAME.EWMARANGE;
                case Definition.CHART_TYPE.EWMA_STDDEV :
                    return Definition.BLOB_FIELD_NAME.EWMASTDDEV;
                case Definition.CHART_TYPE.MA :
                    return Definition.BLOB_FIELD_NAME.MA;
                case Definition.CHART_TYPE.MSD :
                    return Definition.BLOB_FIELD_NAME.MSD;
                case Definition.CHART_TYPE.MR:
                    return Definition.BLOB_FIELD_NAME.MR;
                default :
                    return string.Empty;
            }
        }

        public void bbtnSave_Click(object sender, EventArgs e)
        {
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_SEARCH_SAMPLE_DATA));
                return;
            }

            LinkedList lnkListSaveData = new LinkedList();

            if (this.btxtUCL.Text.Trim().Length > 0)
            {
                double dUCL = 0;

                bool isParse = double.TryParse(this.btxtUCL.Text.Trim(), out dUCL);
                if (!isParse)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"UCL"}, null);
                    return;
                }
                lnkListSaveData.Add(Definition.CHART_COLUMN.UCL, dUCL.ToString());
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE", new string[]{"UCL"}, null);
                return;
            }

            if (this.btxtLCL.Text.Trim().Length > 0)
            {
                double dLCL= 0;

                bool isParse = double.TryParse(this.btxtLCL.Text.Trim(), out dLCL);
                if (!isParse)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"LCL"}, null);
                    return;
                }
                lnkListSaveData.Add(Definition.CHART_COLUMN.LCL, dLCL.ToString());
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE", new string[]{"LCL"}, null);
                return;
            }

            if (this.btxtUSL.Text.Trim().Length > 0)
            {
                double dUSL = 0;

                bool isParse = double.TryParse(this.btxtUSL.Text.Trim(), out dUSL);
                if (!isParse)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"USL"}, null);
                    return;
                }
                lnkListSaveData.Add(COLUMN.UPPER_SPEC, dUSL.ToString());
            }
            else
            {
                lnkListSaveData.Add(COLUMN.UPPER_SPEC, "");
                //MSGHandler.DisplayMessage(MSGType.Information, "Please input USL value.");
                //return;
            }

            if (this.btxtLSL.Text.Trim().Length > 0)
            {
                double dLSL = 0;

                bool isParse = double.TryParse(this.btxtLSL.Text.Trim(), out dLSL);
                if (!isParse)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"LSL"}, null);
                    return;
                }
                lnkListSaveData.Add(COLUMN.LOWER_SPEC, dLSL.ToString());
            }
            else
            {
                lnkListSaveData.Add(COLUMN.LOWER_SPEC, "");
                //MSGHandler.DisplayMessage(MSGType.Information, "Please input LSL value.");
                //return;
            }

            //spc-1219
            if (this.btxtTarget.Text.Trim().Length > 0)
            {
                double dTarget = 0;

                bool isParse = double.TryParse(this.btxtTarget.Text.Trim(), out dTarget);
                if (!isParse)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"Target"}, null);
                    return;
                }
                lnkListSaveData.Add(COLUMN.TARGET, dTarget.ToString());
            }
            else
            {
                lnkListSaveData.Add(COLUMN.TARGET, "");
                //MSGHandler.DisplayMessage(MSGType.Information, "Please input LSL value.");
                //return;
            }

            if (this._sModelConfigRawIDforSave.Length > 0)
            {
                lnkListSaveData.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, this._sModelConfigRawIDforSave);
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_SAMPLE_DATA", null, null);
                return;
            }

            if (this._sMainYNforSave.Length > 0)
            {
                lnkListSaveData.Add(COLUMN.MAIN_YN, this._sMainYNforSave);
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_SAMPLE_DATA", null, null);
                return;
            }

            if (lnkListSaveData.Count == 0)
                return;

            double dUCLTemp = Convert.ToDouble(btxtUCL.Text);
            double dLCLTemp = Convert.ToDouble(btxtLCL.Text);
            //double dUSLTemp = Convert.ToDouble(btxtUSL.Text);
            //double dLSLTemp = Convert.ToDouble(btxtLSL.Text);

            //modified by enkim 2012.10.18 SPC-923
            if (this._sChartType == Definition.CHART_TYPE.RAW || this._sChartType == Definition.CHART_TYPE.XBAR)
            {
                
                if (btxtUSL.Text != null && btxtUSL.Text != "")
                {
                    double dUSLTemp = Convert.ToDouble(btxtUSL.Text);
                    if (dUCLTemp > dUSLTemp)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_BIGGER", new string[]{"UCL", "USL"}, null);
                        return;
                    }
                }

                if (btxtLSL.Text != null && btxtLSL.Text != "")
                {
                    double dLSLTemp = Convert.ToDouble(btxtLSL.Text);
                    
                    if (dLSLTemp > dLCLTemp)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_SMALLER", new string[]{"LCL", "LSL"}, null);
                        return;
                    }
                }

                //if (dUCLTemp > dUSLTemp)
                //{
                //    MSGHandler.DisplayMessage(MSGType.Information, "You can not save the calculation result.\nCalculated UCL value is bigger than USL value.");
                //    return;
                //}
                //else if (dLCLTemp < dLSLTemp)
                //{
                //    MSGHandler.DisplayMessage(MSGType.Information, "You can not save the calculation result.\nCalculated LCL value is smaller than LSL value.");
                //    return;
                //}
            }

            if (dUCLTemp < dLCLTemp)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_SMALLER", new string[]{"UCL", "LCL"}, null);
                return;
            }

            if (btxtUSL.Text != null && btxtUSL.Text != "" && btxtLSL.Text != null && btxtLSL.Text != "")
            {
                double dUSLTemp = Convert.ToDouble(btxtUSL.Text);
                double dLSLTemp = Convert.ToDouble(btxtLSL.Text);
                if (dLSLTemp > dUSLTemp)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_SMALLER", new string[]{"USL", "LSL"}, null);
                    return;
                }
            }

            if (btxtUSL.Text != null && btxtUSL.Text != "" || btxtLSL.Text != null && btxtLSL.Text != "")
            {
                double dUSLTemp = 0;
                double dLSLTemp = 0;
                double dTarget = Convert.ToDouble(btxtTarget.Text);
                
                if (btxtUSL.Text != null && btxtUSL.Text != "")
                {
                    dUSLTemp = Convert.ToDouble(btxtUSL.Text);
                }
                else if (btxtLSL.Text != null && btxtLSL.Text != "")
                {
                    dLSLTemp = Convert.ToDouble(btxtLSL.Text);
                }

                if (dTarget > dUSLTemp || dTarget < dLSLTemp)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SVAE_CALC_TARGET", null, null);
                    return;
                }
            }

            //modified end SPC-923

            this.MsgShow(COMMON_MSG.Save_Data);

            lnkListSaveData.Add("CHART_TYPE", this._sChartType);
            lnkListSaveData.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);
            lnkListSaveData.Add(COLUMN.SAVE_COMMENT, "Manual Calculation");
            lnkListSaveData.Add(COLUMN.CHANGED_ITEMS, "Limit");

            bool bSuccess = this._wsSPC.SaveSPCCalcModelData(lnkListSaveData.GetSerialData());

            this.MsgClose();

            if (bSuccess)
            {
                this._sUSL = lnkListSaveData[COLUMN.UPPER_SPEC].ToString();
                this._sLSL = lnkListSaveData[COLUMN.LOWER_SPEC].ToString();
                this._sTarget = lnkListSaveData[COLUMN.TARGET].ToString();
                //SPC-812 
                if (_sCalculation)
                {
                    ConfigListDataBinding(true);
                }
                else
                {
                    ConfigListDataBinding(false);
                }
                //
                SelectRow(int.Parse(this.mChartVariable.MODEL_CONFIG_RAWID));
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
            }
        }

        /// <summary>
        /// Select a row of bsprData
        /// </summary>
        /// <param name="rawID">Raw ID of selected row</param>
        private void SelectRow(int rawID)
        {
            if(-1 == rawID)
                return;

            int temp;
            for(int i=0; i<bsprData.ActiveSheet.Rows.Count; i++)
            {
                if(!int.TryParse(this.bsprData.GetCellText(i, (int) iColIdx.RAWID), out temp))
                    continue;

                if(temp == rawID)
                {
                    this.bsprData.SetCellValue(i, (int)iColIdx.SELECT, true);
                    FarPoint.Win.Spread.EditorNotifyEventArgs e =
                        new EditorNotifyEventArgs(this.bsprData.GetRootWorkbook(), this.bsprData.EditingControl, i,
                                                  (int) iColIdx.SELECT);
                    this.bsprData_ButtonClicked(this.bsprData, e);
                }
            }
        }

        public void bbtnDisplayResult_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            try
            {
                double dUCL = 0;
                double dLCL = 0;

                bool IsUpperLimitParsed = double.TryParse(this.btxtUCL.Text.Trim(), out dUCL);
                bool IsLowerLimitParsed = double.TryParse(this.btxtLCL.Text.Trim(), out dLCL);

                ChartUtility util = new ChartUtility();
                ShowPreviewSeries(dUCL, dLCL, IsUpperLimitParsed, IsLowerLimitParsed, util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.SPECPREVIEW));

                int numOOC = 0;
                string dataValueColumnName = GetDataValueColumnName();
                int total = ((DataSet) bsprChartData.ActiveSheet.DataSource).Tables[0].Rows.Count;
                for(int i=0; i < total; i++)
                {
                    double value;
                    if (
                        !double.TryParse(((DataSet) bsprChartData.ActiveSheet.DataSource).Tables[0].Rows[i][dataValueColumnName].ToString(), out value))
                        continue;

                    if(IsUpperLimitParsed && value > dUCL)
                    {
                        numOOC++;
                        continue;
                    }
                    if(IsLowerLimitParsed && value < dLCL)
                    {
                        numOOC++;
                        continue;
                    }
                }
                this.blbNumOOC.Text = numOOC + " / " + total + " (" + (Convert.ToDouble(numOOC) / Convert.ToDouble(total) * 100).ToString("0.00") + "%)";
            }
            catch
            {
            }
        }

        public void bbtnDisplayResult_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            HidePreviewSeries();

            this.blbNumOOC.Text = "";
        }

        public void bbtnApplyOutlier_Click(object sender, EventArgs e)
        {
            // check it can apply
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            double rangeValue = 0;
            if(this.rbtnAverage.Checked)
            {
                if(this.ntxtOutlierAverage.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_INPUT_OUTLIER_VALUE));
                    return;
                }

                if(!double.TryParse(this.ntxtOutlierAverage.Text, out rangeValue))
                {
                    return;
                }
            }

            string dataValueColumnName = GetDataValueColumnName();
            DataTable dtChartData = ((DataSet) bsprChartData.DataSource).Tables[0];
            if(!dtChartData.Columns.Contains(dataValueColumnName))
            {
                return;
            }
            
            ClearOutlierOfCurrentChart();

            // check all data and set outlier if it is
            OutlierLimitType type = GetOutlierLimitType();

            ApplyOutlier(type, dataValueColumnName, rangeValue, false, Color.Empty, true);
        }

        /// <summary>
        /// Apply outlier setting
        /// </summary>
        /// <param name="type">outlier setting type</param>
        /// <param name="dataValueColumnName">data colum name to use</param>
        /// <param name="rangeValue">boundary to decide outlier</param>
        /// <param name="previewLine">whether it draws preview line or not</param>
        /// <param name="previewLineColor">color of preview line</param>
        private void ApplyOutlier(OutlierLimitType type, string dataValueColumnName, double rangeValue, bool previewLine, Color previewLineColor, bool isApplySpread)
        {
            double upperLimit = 0;
            double lowerLimit = 0;
            bool IsUpperParsed = false;
            bool IsLowerParsed = false;

            if(type == OutlierLimitType.VALUE
                    || type == OutlierLimitType.SIGMA
                    || type == OutlierLimitType.CONSTANT)
            {
                IsUpperParsed = TryGetOutlierUpperLimitUsingAverage(type, rangeValue, out upperLimit);
                IsLowerParsed = TryGetOutlierLowerLimitUsingAverage(type, rangeValue, out lowerLimit);

                if(previewLine)
                {
                    ShowPreviewSeries(upperLimit, lowerLimit, IsUpperParsed, IsLowerParsed, previewLineColor);
                }
            }

            for(int i=0; i < bsprChartData.ActiveSheet.Rows.Count; i++)
            {
                double value;
                if(!double.TryParse(((DataSet)bsprChartData.ActiveSheet.DataSource).Tables[0].Rows[i][dataValueColumnName].ToString(), out value))
                    continue;
                
                if(type == OutlierLimitType.CONTROL)
                {
                    IsUpperParsed = TryGetUpperControlValue(i, out upperLimit);
                    IsLowerParsed = TryGetLowerControlValue(i, out lowerLimit);
                }

                if(IsUpperParsed
                    && value > upperLimit)
                {
                    SetOutlier(i, isApplySpread);
                }
                if(IsLowerParsed
                    && value < lowerLimit)
                {
                    SetOutlier(i, isApplySpread);
                }
            }
        }

        public void bbtnPreview_MouseDown(object sender, MouseEventArgs e)
        {
            // check it can previews
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                return;
            }
            double rangeValue = 0;
            if(this.rbtnAverage.Checked)
            {
                if(this.ntxtOutlierAverage.Text.Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_INPUT_OUTLIER_VALUE));
                    return;
                }

                if(!double.TryParse(this.ntxtOutlierAverage.Text, out rangeValue))
                {
                    return;
                }
            }

            string dataValueColumnName = GetDataValueColumnName();
            DataTable dtChartData = ((DataSet) bsprChartData.DataSource).Tables[0];
            if(!dtChartData.Columns.Contains(dataValueColumnName))
            {
                return;
            }

            // copy original outlier to preview
            for (int i = 0; i < dtChartData.Rows.Count; i++)
            {
                if (this.bsprChartData.ActiveSheet.Rows[i].BackColor == Color.LightGreen)
                {
                    lstTempForSetOulierPreview.Add(i);
                    UnsetOutlier(i, false);
                }
            }

            // check all data and set outlier if it is
            OutlierLimitType type = GetOutlierLimitType();

            ChartUtility util = new ChartUtility();
            ApplyOutlier(type, dataValueColumnName, rangeValue, true, util.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.OUTLIERPREVIEW), false);
        }

        private OutlierLimitType GetOutlierLimitType()
        {
            if(this.rbtnAverage.Checked)
            {
                string text = this.bcboOutlierType.SelectedItem.ToString().ToUpper();
                if (text == "CONSTANT")
                    return OutlierLimitType.CONSTANT;
                else if (text == "VALUE")
                    return OutlierLimitType.VALUE;
                else if (text == "SIGMA")
                {
                    return OutlierLimitType.SIGMA;
                }
            }
            else if(this.rbtnControlLimit.Checked)
            {
                return OutlierLimitType.CONTROL;
            }

            throw new Exception();
        }

        public void bbtnPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            ClearOutlierOfCurrentChart();

            foreach(int i in lstTempForSetOulierPreview)
            {
                SetOutlier(i, true);
            }
            lstTempForSetOulierPreview.Clear();

            HidePreviewSeries();
        }

        private void ClearOutlierOfCurrentChart()
        {
            arrOutlierList.Clear();
            DataTable dtChartData = ((DataSet) bsprChartData.DataSource).Tables[0];
            for (int i = 0; i < dtChartData.Rows.Count; i++)
            {
                UnsetOutlier(i, true);
            }
        }

        /// <summary>
        /// Show Preview Series
        /// </summary>
        /// <param name="upperLimit">upper limit value</param>
        /// <param name="lowerLimit">lower limit value</param>
        /// <param name="isShowUpperLimit">whether it shows upper limit or not</param>
        /// <param name="isShowLowerLimit">whether it shows lower limit or not</param>
        /// <param name="previewLineColor">color of previewLine</param>
        private void ShowPreviewSeries(double upperLimit, double lowerLimit, bool isShowUpperLimit, bool isShowLowerLimit, Color previewLineColor)
        {
            BTeeChart chart = null;
            if (chartCalcBase != null)
            {
                chart = chartCalcBase.SPCChart;
            }
            else if (chartBase != null)
            {
                chart = chartBase.SPCChart;
            }
            else
                return;

            for (int i = chart.Chart.Series.Count - 1; i >= 0; i--)
            {
                if ((chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT) && isShowUpperLimit)
                    || (chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT) && isShowLowerLimit))
                {
                    for (int j = 0; j < chart.Chart.Series[i].XValues.Count; j++)
                    {
                        if (chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT) && isShowUpperLimit)
                        {
                            chart.Chart.Series[i].Color = previewLineColor;
                            chart.Chart.Series[i][j].Y = upperLimit;
                        }
                        else if (chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT) && isShowLowerLimit)
                        {
                            chart.Chart.Series[i].Color = previewLineColor;
                            chart.Chart.Series[i][j].Y = lowerLimit;
                        }
                    }
                    chart.Chart.Series[i].Visible = true;
                }
            }

            //int rowCount = 0;
            //BTeeChart spcChart = null;
            //DataTable spcDataTable = null;
            //if(chartCalcBase != null)
            //{
            //    rowCount = chartCalcBase.dtDataSource.Rows.Count;
            //    spcChart = chartCalcBase.SPCChart;
            //    spcDataTable = chartCalcBase.dtDataSource;
            //}
            //else if(chartBase != null)
            //{
            //    rowCount = chartBase.dtDataSource.Rows.Count;
            //    spcChart = chartBase.SPCChart;
            //    spcDataTable = chartBase.dtDataSource;
            //}
            //else
            //    return;

            //DataTable dtDataSource = new DataTable();
            //dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX);
            //dtDataSource.Columns.Add(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
            //dtDataSource.Columns.Add(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);
            //for (int i = 0; i < rowCount; i++)
            //{
            //    DataRow dr = dtDataSource.NewRow();
            //    dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = spcDataTable.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX];
            //    dr[Definition.CHART_COLUMN.PREVIEWUPPERLIMIT] = upperLimit;
            //    dr[Definition.CHART_COLUMN.PREVIEWLOWERLIMIT] = lowerLimit;
            //    dtDataSource.Rows.Add(dr);
            //}

            //SeriesInfo siUpperLimit = new SeriesInfo(typeof (Line), Definition.CHART_COLUMN.PREVIEWUPPERLIMIT,
            //                                         dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWUPPERLIMIT);
            //SeriesInfo siLowerLimit = new SeriesInfo(typeof (Line), Definition.CHART_COLUMN.PREVIEWUPPERLIMIT,
            //                                         dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, Definition.CHART_COLUMN.PREVIEWLOWERLIMIT);
            
            //siUpperLimit.ShowLegend = false;
            //siLowerLimit.ShowLegend = false;
            //siUpperLimit.SeriesColor = previewLineColor;
            //siLowerLimit.SeriesColor = previewLineColor;
            
            //if(isShowUpperLimit)
            //{
            //    Series s = spcChart.AddSeries(siUpperLimit);
            //    s.Visible = true;
            //    ((Line)s).LinePen.Width = 2;
            //    ((Line)s).Pointer.Visible = false;
            //}
            //if(isShowLowerLimit)
            //{
            //   Series s = spcChart.AddSeries(siLowerLimit);
            //   s.Visible = true;
            //   ((Line)s).LinePen.Width = 2;
            //   ((Line)s).Pointer.Visible = false;
            //}
            //spcChart.Chart.Axes.Bottom.Labels.Style =Steema.TeeChart.AxisLabelStyle.Text;
            //if (!spcChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
            //{
            //    spcChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
            //}
        }

        /// <summary>
        /// Hide preview series
        /// </summary>
        private void HidePreviewSeries()
        {
            BTeeChart chart = null;
            if(chartCalcBase != null)
            {
                chart = chartCalcBase.SPCChart;
            }
            else if(chartBase != null)
            {
                chart = chartBase.SPCChart;
            }
            else
                return;

            for (int i = chart.Chart.Series.Count - 1; i >= 0; i--)
            {
                if(chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWUPPERLIMIT)
                    || chart.Chart.Series[i].Title.Equals(Definition.CHART_COLUMN.PREVIEWLOWERLIMIT))
                {
                    chart.Chart.Series[i].Visible = false;
                }
            }
            chart.Chart.Invalidate();
        }

        /// <summary>
        /// Get outlier upper limit value from chart data using average
        /// </summary>
        /// <param name="type">Outlier limit type</param>
        /// <param name="value">range value to calculate, if type is control it is not used</param>
        /// <param name="outlierUpperLimit">Upper limit value</param>
        /// <returns>Whether it gets limit value</returns>
        private bool TryGetOutlierUpperLimitUsingAverage(OutlierLimitType type, double value, out double outlierUpperLimit)
        {
            try
            {
                double[] dsValueList = GetValueListOfChartData(true);
                double avg = StStat.mean(dsValueList);
                double std = StStat.std(dsValueList);

                if (type == OutlierLimitType.CONSTANT)
                {
                    outlierUpperLimit = avg + value;
                    return true;
                }
                else if (type == OutlierLimitType.VALUE)
                {
                    outlierUpperLimit = avg + System.Math.Abs(avg) * value * 0.01;
                    return true;
                }
                else if (type == OutlierLimitType.SIGMA)
                {
                    outlierUpperLimit = avg + (std * value);
                    return true;
                }

            } catch(Exception ex)
            {
                throw ex;
            }

            throw new Exception();
        }

        /// <summary>
        /// Get values of chart data
        /// </summary>
        /// <param name="containOutlier">if it is true, the result contains outliers</param>
        /// <returns>value list of chart data</returns>
        private double[] GetValueListOfChartData(bool containOutlier)
        {
            ArrayList arrOutlierData = new ArrayList();

            if(containOutlier == false)
            {
                for (int i = 0; i < this.bsprChartData.ActiveSheet.Rows.Count; i++)
                {
                    if (this.bsprChartData.ActiveSheet.Rows[i].BackColor == Color.LightGreen)
                    {
                        arrOutlierData.Add(i);
                    }
                }
            }

            ArrayList arrData = new ArrayList();
            DataSet dsTemp = (DataSet)this.bsprChartData.DataSource;
            string BLOBFieldName = GetBLOBFieldNameFromChartType(this._sChartType);
            if(!string.IsNullOrEmpty(BLOBFieldName))
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    if (arrOutlierData.Contains(i))
                            continue;
                    if (dsTemp.Tables[0].Rows[i][BLOBFieldName].ToString().Length > 0)
                    {
                        double dOutPut = 0;
                        bool isParse = double.TryParse(dsTemp.Tables[0].Rows[i][BLOBFieldName].ToString(), out dOutPut);

                        if (isParse)
                            arrData.Add(dOutPut);
                    }
                }
            }

            return (double[])arrData.ToArray(typeof(double));
        }

        /// <summary>
        /// Get outlier lower limit value from chart data using average
        /// </summary>
        /// <param name="type">Outlier limit type</param>
        /// <param name="value">Limit value</param>
        /// <param name="outlierLowerLimit">Lower limit value to assign</param>
        /// <returns>Whether it gets limit value</returns>
        private bool TryGetOutlierLowerLimitUsingAverage(OutlierLimitType type, double value, out double outlierLowerLimit)
        {
            try
            {
                double[] dsValueList = GetValueListOfChartData(true);
                double avg = StStat.mean(dsValueList);
                double std = StStat.std(dsValueList);

                if (type == OutlierLimitType.CONSTANT)
                {
                    outlierLowerLimit = avg - value;
                    return true;
                }
                else if (type == OutlierLimitType.VALUE)
                {
                    outlierLowerLimit = avg - System.Math.Abs(avg) * value * 0.01;
                    return true;
                }
                else if (type == OutlierLimitType.SIGMA)
                {
                    outlierLowerLimit = avg - (std * value);
                    return true;
                }

            } catch(Exception ex)
            {
                throw ex;
            }

            throw new Exception();
        }


        /// <summary>
        /// Get target value used by the chart
        /// </summary>
        /// <returns>It returns true if success to get value</returns>
        private bool TryGetTargetValue(int rowIndex, out double target)
        {
            if(bsprChartData == null || bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                string exceptionMsg = "Chart Data is null or have not rows.";
                Exception ex = new Exception(exceptionMsg);
                LogHandler.LogWriteException("SPC", "SPCCalculationUC", "GetTargetValue", exceptionMsg, ex);
                throw ex;
            }

            string targetValue = GetColumnValue(GetAverageColumnName(), rowIndex);

            return double.TryParse(targetValue, out target);
        }

        /// <summary>
        /// Get average value used by the chart
        /// </summary>
        /// <returns>It returns true if success to get value</returns>
        private bool TryGetAverageValue(int rowIndex, out double average)
        {
            if(bsprChartData == null || bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                string exceptionMsg = "Chart Data is null or have not rows.";
                Exception ex = new Exception(exceptionMsg);
                LogHandler.LogWriteException("SPC", "SPCCalculationUC", "GetTargetValue", exceptionMsg, ex);
                throw ex;
            }

            string averageValue = GetColumnValue(GetAverageColumnName(), rowIndex);

            return double.TryParse(averageValue, out average);
        }

        /// <summary>
        /// Get upper control value used by the chart
        /// </summary>
        /// <returns>It returns true if success to get value</returns>
        private bool TryGetUpperControlValue(int rowIndex, out double upperControl)
        {
            if(bsprChartData == null || bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                string exceptionMsg = "Chart Data is null or have not rows.";
                Exception ex = new Exception(exceptionMsg);
                LogHandler.LogWriteException("SPC", "SPCCalculationUC", "GetTargetValue", exceptionMsg, ex);
                throw ex;
            }

            string upperControlValue = GetColumnValue(GetUpperControlCoulmnName(), rowIndex);

            return double.TryParse(upperControlValue, out upperControl);
        }

        /// <summary>
        /// Get lower control value used by the chart
        /// </summary>
        /// <returns>It returns true if success to get value</returns>
        private bool TryGetLowerControlValue(int rowIndex, out double upperControl)
        {
            if(bsprChartData == null || bsprChartData.ActiveSheet.Rows.Count == 0)
            {
                string exceptionMsg = "Chart Data is null or have not rows.";
                Exception ex = new Exception(exceptionMsg);
                LogHandler.LogWriteException("SPC", "SPCCalculationUC", "GetTargetValue", exceptionMsg, ex);
                throw ex;
            }

            string upperControlValue = GetColumnValue(GetLowerControlCoulmnName(), rowIndex);

            return double.TryParse(upperControlValue, out upperControl);
        }

        /// <summary>
        /// Get value of dtChartData
        /// </summary>
        /// <param name="columnName">Column Name</param>
        /// <param name="rowIndex">Index of row</param>
        /// <returns>string value</returns>
        private string GetColumnValue(string columnName, int rowIndex)
        {
            int indexColumn = -1;
            DataTable dtChartData = ((DataSet)bsprChartData.ActiveSheet.DataSource).Tables[0];
            for(int i=0; i<bsprChartData.ActiveSheet.Columns.Count; i++)
            {
                if(dtChartData.Columns[i].ColumnName.ToUpper() == columnName.ToUpper())
                {
                    indexColumn = i;
                    break;
                }
            }
            return dtChartData.Rows[rowIndex][indexColumn].ToString();
        }

        /// <summary>
        /// Get name of data value column used by the chart
        /// </summary>
        /// <returns>if found it returns name of data value column, else empty value</returns>
        private string GetDataValueColumnName()
        {
            try
            {
                LinkedList llstChartSeries = GetChartSeries();

                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string value = llstChartSeries.GetValue(i).ToString();
                    if (value == Definition.BLOB_FIELD_NAME.MSD
                        || value == Definition.CHART_COLUMN.AVG
                        || value == Definition.CHART_COLUMN.STDDEV
                        || value == Definition.CHART_COLUMN.RANGE
                        || value == Definition.CHART_COLUMN.AVG
                        || value == Definition.CHART_COLUMN.RANGE
                        || value == Definition.CHART_COLUMN.STDDEV
                        || value == Definition.CHART_COLUMN.MA
                        || value == Definition.CHART_COLUMN.MSD
                        || value == Definition.CHART_COLUMN.MR
                        || value == Definition.CHART_COLUMN.RAW)
                    {
                        return llstChartSeries.GetKey(i).ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get name of average column used by the chart
        /// </summary>
        /// <returns>if found it returns name of target column, else empty value</returns>
        private string GetAverageColumnName()
        {
            try
            {
                LinkedList llstChartSeries = GetChartSeries();

                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    if (llstChartSeries.GetValue(i).ToString() == Definition.CHART_COLUMN.AVG)
                        return llstChartSeries.GetKey(i).ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Get name of upper control column used by the chart
        /// </summary>
        /// <returns>if found it returns name of target column, else empty value</returns>
        private string GetUpperControlCoulmnName()
        {
            try
            {
                LinkedList llstChartSeries = GetChartSeries();

                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    if (llstChartSeries.GetValue(i).ToString() == Definition.CHART_COLUMN.UCL)
                        return llstChartSeries.GetKey(i).ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get name of lower control column used by the chart
        /// </summary>
        /// <returns>if found it returns name of target column, else empty value</returns>
        private string GetLowerControlCoulmnName()
        {
            try
            {
                LinkedList llstChartSeries = GetChartSeries();

                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    if (llstChartSeries.GetValue(i).ToString() == Definition.CHART_COLUMN.LCL)
                        return llstChartSeries.GetKey(i).ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Get name of target column used by the chart
        /// </summary>
        /// <returns>if found it returns name of target column, else empty value</returns>
        private string GetTargetColumnName()
        {
            try
            {
                LinkedList llstChartSeries = GetChartSeries();

                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    if (llstChartSeries.GetValue(i).ToString() == Definition.CHART_COLUMN.TARGET)
                        return llstChartSeries.GetKey(i).ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        private LinkedList GetChartSeries()
        {
            string strChartName = string.Empty;

            if(chartBase != null)
                strChartName = chartBase.Name;
            else if(chartCalcBase != null)
                strChartName = chartCalcBase.Name;
            else
            {
                string exceptionMsg = "Chart is null";
                Exception ex = new Exception(exceptionMsg);
                LogHandler.LogWriteException("SPC", "SPCCalculationUC", "GetTargetColumnName", exceptionMsg, ex);
                throw ex;
            }

            return CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
        }

        public void SetOutlier_CheckedChanged(object sender, EventArgs e)
        {
            if(sender == this.rbtnAverage)
            {
                if(rbtnAverage.Checked)
                {
                    this.rbtnControlLimit.Checked = false;
                    this.bcboOutlierType.Enabled = true;
                    this.ntxtOutlierAverage.Enabled = true;
                }
                else
                {
                    this.bcboOutlierType.Enabled = false;
                    this.ntxtOutlierAverage.Enabled = false;
                }
            }
            else if(sender == rbtnControlLimit)
            {
                if(this.rbtnControlLimit.Checked)
                {
                    this.rbtnAverage.Checked = false;
                }
            }
        }

        public void bcboOutlierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BComboBox box = (BComboBox) sender;
            if(box.Text == "SIGMA")
            {
                this.blblOutlierSymbol.Text = "sigma";
            }
            else if(box.Text == "VALUE")
            {
                this.blblOutlierSymbol.Text = "%";
            }
            else if(box.Text == "CONSTANT")
            {
                this.blblOutlierSymbol.Text = "";
            }
        }

        public void bchkSampling_CheckStateChanged(object sender, System.EventArgs e)
        {
            if(bchkSampling.Checked)
            {
                if(this.bcboChartType.Text == "RAW")
                    this.btxtWSampleCnt.Enabled = true;

                this.btxtSampleCnt.Enabled = true;
                this.btxtSamplePeriod.Enabled = true;
            }
            else
            {
                this.btxtWSampleCnt.Enabled = false;
                this.btxtSampleCnt.Enabled = false;
                this.btxtSamplePeriod.Enabled = false;
                this.btxtWSampleCnt.Text = "";
                this.btxtSampleCnt.Text = "";
                this.btxtSamplePeriod.Text = "";
            }
        }

        public void bcboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bchkSampling.Checked)
            {
                if (bcboChartType.Text == "RAW")
                    this.btxtWSampleCnt.Enabled = true;
                else
                {
                    this.btxtWSampleCnt.Enabled = false;
                    this.btxtWSampleCnt.Text = "";
                }
            }
        }

        public void bsprChartData_CellClick(object sender, CellClickEventArgs e)
        {
            
        }

        private void btxt_Validated(object sender, EventArgs e)
        {
            if (btxtTarget.Text.Length == 0 && btxtUSL.Text.Length > 0 && btxtLSL.Text.Length > 0)
            {
                double iUpper = string.IsNullOrEmpty(this.btxtUSL.Text) ? 0 : double.Parse(this.btxtUSL.Text);
                double iLower = string.IsNullOrEmpty(this.btxtLSL.Text) ? 0 : double.Parse(this.btxtLSL.Text);

                this.btxtTarget.Text = ((iUpper + iLower) / 2).ToString();
            }
        }
    }
}