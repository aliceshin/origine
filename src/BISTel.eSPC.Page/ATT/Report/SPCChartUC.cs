using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using BISTel.eSPC.Page.ATT.Common.Popup;
using BISTel.eSPC.Page.Report.Popup;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;

using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.ATT.Report
{
    public partial class SPCChartUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        BSpreadUtility mBSpreadUtil;

        CommonUtility mComUtil;
        ChartUtility mChartUtil;
        ChartInterface mChartVariable;
        ChartInformation mChartInfomationUI;
        BISTel.eSPC.Common.CHART_PARENT_MODE mChartMode = BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART;


        BISTel.eSPC.Common.ATT.DataTableGroupBy mDTGroupBy = new eSPC.Common.ATT.DataTableGroupBy();
        DataSet mDSChart = null;
        DataSet mDSOCAPValue = null;
        DataSet _dsOcapComment = null;

        Initialization mInitialization;

        MultiLanguageHandler mMHandler;

        LinkedList mllstSearchCondition = new LinkedList();
        LinkedList mllstChartSearchData = new LinkedList();
        LinkedList mllstChart = new LinkedList();
        LinkedList mllstChartSeriesVisibleType = null;

        SourceDataManager _dataManager = null;
        SPCStruct.ChartContextInfo strucContextinfo;

        int iValueIndex = -1;

        string mSite = string.Empty;
        string mFab = string.Empty;
        string mLineRawID = string.Empty;
        string mLine = string.Empty;
        string mAreaRawID = string.Empty;
        string mArea = string.Empty;
        string mStartTime = string.Empty;
        string mEndTime = string.Empty;
        string mParamTypeCd = string.Empty;
        string mEQPModel = string.Empty;

        string _sOCAPRawIDURL = string.Empty;

        bool mApplyRestric = false;

        DataTable mDTContext;
        DataTable mDTCustomContext;
        DefaultChart chartBase;
        DataSet _ds = null;
        DataTable dtRawData = null;
        bool isRawClick = false;
        DataTable _dtOCAPData = null;

        private BaseChart clickedChart = null;
        private Series clickedSeries = null;
        private int clickedIndex = -1;

        private bool includingToggleData = false;

        private SeriesGroupingAndFilteringPopup filteringPopup = null;
        private bool isFiltered = false;
        private DataSet filteredRawData = null;

        public delegate void LinkTraceDataViewEventHandler(object sender, EventArgs e, LinkedList llstTraceLinkData);
        public event LinkTraceDataViewEventHandler linkTraceDataViewEvent;

        GrouperAndFilter _grouperAndFilter = new GrouperAndFilter();
        private bool _bUseComma;
        private string _sGroupName; //SPC-1292, KBLEE

        #endregion


        #region ::: Properties

        public LinkedList llstSearchCondition
        {
            set
            {
                this.mllstSearchCondition = value;
            }
            get
            {
                return this.mllstSearchCondition;
            }
        }

        public BISTel.eSPC.Common.CHART_PARENT_MODE CHART_MODE
        {
            get { return this.mChartMode; }
            set { this.mChartMode = value; }
        }

        public ChartInterface ChartVariable
        {
            get { return this.mChartVariable; }
            set { this.mChartVariable = value; }
        }

        public string ParamTypeCD
        {
            get { return this.mParamTypeCd; }
            set { this.mParamTypeCd = value; }
        }

        //SPC-1292, KBLEE, START
        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }

        public string AREA_RAWID
        {
            get { return mAreaRawID; }
            set { mAreaRawID = value; }
        }

        public string LINE_RAWID
        {
            get { return mLineRawID; }
            set { mLineRawID = value; }
        }
        //SPC-1292, KBLEE, END

        #endregion


        public SPCChartUC()
        {
            this.InitializeComponent();
        }

        public void InitializeCommon()
        {
            this.mComUtil = new CommonUtility();
            this.mBSpreadUtil = new BSpreadUtility();
            this.mMHandler = MultiLanguageHandler.getInstance();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this.mInitialization = new Initialization();
            this._dataManager = new SourceDataManager();
            this.mInitialization.InitializePath();
            this.mChartUtil = new ChartUtility();
            this.strucContextinfo = new SPCStruct.ChartContextInfo();

            this.InitializeLayout();
            this.InitializeDataButton();
            this.InitializeCode();
        }


        public void InitializePage()
        {
            if (ChartVariable == null)
                this.ProcessLink();
            else
            {
                for (int i = 0; i < this.bbtnListChart.Items.Count; i++)
                {
                    if (CHART_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART)
                    {
                        if ((this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT && ChartVariable.MAIN_YN != "Y"))
                        {
                            this.bbtnListChart.Items[i].Visible = false;
                        }
                    }
                }


                this.iValueIndex = -1;
                this.mllstChart.Clear();
                this.mllstChart = GetDefaultChart();

                this.InitializeBSpread();
                this.InitializeChartSeriesVisibleType();
                this.InitializeChart();
                if (this.includingToggleData)
                    this.RefreshTogglePoint();

                if (!this.includingToggleData && this.mChartVariable != null && this.mChartVariable.OCAPRawID.Length > 0)
                {
                    this.FindSelectedOCAP();
                }
            }
        }

        /// <summary>
        /// If this page opened by URL link, it read condition of parameters of the requested URL and display chart
        /// Ex : http://192.168.1.94/ees/EES.application?func=TRACE_DATA&queryxml=<ees><page_info><dll>eFDC/BISTel.eFDC.Page.dll</dll><class>BISTel.eFDC.Page.Report.TraceDataViewPUC</class><page>TraceDataViewPUC</page><pageTitle>TraceData</pageTitle><page_type>link</page_type></page_info><request><EQP_ID><value>vFAB_EQ_02</value></EQP_ID><MODULE_ID><value>vFAB_SITE/vFAB_FAB/vFAB_LINE/AREA1:vFAB_EQ_02</value></MODULE_ID><DCP_ID><value>vFAB_EQ_02_DCP</value></DCP_ID><RECIPE_ID><value>2798</value></RECIPE_ID><STEP_ID><value></value></STEP_ID><PARAMETER_ID><value>DATA_QUALITY_1</value></PARAMETER_ID><LOT_ID><value>LOT000000020000</value></LOT_ID><TRACE_DATE><value>2009-09-10 10:59:00</value></TRACE_DATE></request></ees>
        /// </summary>
        private void ProcessLink()
        {
            try
            {
                if (!GlobalDefinition.PassData.ContainsKey("queryxml"))
                {
                    return;
                }

                // Parse queryxml
                XmlDocument queryXml = new XmlDocument();
                string tempXml = GlobalDefinition.PassData["queryxml"];
                GlobalDefinition.PassData.Clear();
                queryXml.LoadXml(tempXml);

                XmlElement linkData = queryXml.DocumentElement;
                XmlNode requestInfo = linkData.GetElementsByTagName("request")[0];
                XmlNode xNodeModelConfigRawID = ((XmlElement)requestInfo).GetElementsByTagName("MODEL_CONFIG_RAWID")[0];
                XmlNode xNodeOCAPRawID = ((XmlElement)requestInfo).GetElementsByTagName("OCAP_RAWID")[0];
                XmlNode xNodeOCAPDtts = ((XmlElement)requestInfo).GetElementsByTagName("OCAP_DTTS")[0];
                string sModelConfigRawID = xNodeModelConfigRawID.InnerText;
                string sOCAPRawID = xNodeOCAPRawID.InnerText;
                string sOCAPDtts = xNodeOCAPDtts.InnerText;

                LinkedList lnkListConditionFromURL = new LinkedList();

                if (sModelConfigRawID.Length > 0)
                {
                    DataTable dt = new DataTable();

                    dt.Columns.Add(Definition.DynamicCondition_Condition_key.VALUEDATA, typeof(string));
                    dt.Columns.Add(Definition.DynamicCondition_Condition_key.DISPLAYDATA, typeof(string));

                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr[Definition.DynamicCondition_Condition_key.VALUEDATA] = Definition.DynamicCondition_Search_key.CHART_ID;
                    dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA] = sModelConfigRawID.Trim();
                    dt.Rows.Add(dr);

                    lnkListConditionFromURL.Add(Definition.DynamicCondition_Search_key.CHART_ID, dt);
                }

                if (sOCAPRawID.Length > 0)
                {
                    this._sOCAPRawIDURL = sOCAPRawID;
                }

                if (sOCAPDtts.Length > 0)
                {

                    String sStartTime = CommonPageUtil.CalcStartDate(sOCAPDtts);
                    String sEndTime = CommonPageUtil.CalcEndDate(sOCAPDtts);

                    DataTable dtStartValue = DCUtil.MakeDataTableForDCValue(sStartTime, sStartTime);
                    DataTable dtEndValue = DCUtil.MakeDataTableForDCValue(sEndTime, sEndTime);
                    lnkListConditionFromURL.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, dtStartValue);
                    lnkListConditionFromURL.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, dtEndValue);
                }

                this.PageSearch(lnkListConditionFromURL);

                this._sOCAPRawIDURL = string.Empty;
                
            }
            catch
            {
            }
        }

        private void InitializeChartSeriesVisibleType()
        {
            this.mllstChartSeriesVisibleType = new LinkedList();
            this.panel1.Enabled = true;
            this.panel1.Controls.Clear();
            Hashtable htButtonList = this.mInitialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            BCheckBox bchk = null;
            for (int i = htButtonList.Count - 1; i >= 0; i--)
            {
                Hashtable htButtonAttributes = (Hashtable)htButtonList[i.ToString()];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();
                string DefaultVisible = htButtonAttributes[Definition.XML_VARIABLE_DEFAULTVALUE].ToString().ToUpper();

                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                {
                    bchk = new BCheckBox();
                    bchk.Text = this.mMHandler.GetVariable(sButtonName);
                    bchk.Name = sButtonName;
                    bchk.Click += new EventHandler(bchk_Click);
                    bchk.Dock = DockStyle.Left;
                    bchk.TabIndex = i;
                    if (DefaultVisible == Definition.VARIABLE_TRUE.ToUpper())
                        bchk.Checked = true;
                    else
                        bchk.Checked = false;

                    if (sButtonName == Definition.CHART_SERIES.POINT)
                    {
                        if (ChartVariable.complex_yn != "Y")
                            bchk.Visible = false;
                        else
                            bchk.Visible = true;
                    }
                    this.mllstChartSeriesVisibleType.Add(bchk.Text, bchk.Checked);
                    this.panel1.Controls.Add(bchk);
                }
            }
        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_ATT_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetATTCodeData(lk.GetSerialData());
        }

        public void InitializeChart()
        {
            try
            {
                if (this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                {
                    ArrayList arrEQPID = new ArrayList();
                    ArrayList arrModuleID = new ArrayList();
                    //DataSet dsGroupItem = new DataSet();

                    _dataManager.RawDataTableOriginal = this.ChartVariable.dtParamData.Copy();
                    _dataManager.RawDataTable = this.ChartVariable.dtParamData.Copy();

                    int iChart = mllstChart.Count;
                    int iHeight = 300;
                    if (iChart > 1)
                    {
                        iHeight = this.pnlChart.Height / 2;
                        this.pnlChart.Height = iChart * iHeight;
                    }

                    if (this.ChartVariable.dtParamData != null
                        && this.ChartVariable.dtParamData.Rows.Count > 0
                        && this.ChartVariable.dtParamData.Columns.Contains(BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID))
                    {
                        string _sTempModelConfigRawid = this.ChartVariable.dtParamData.Rows[0][BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID].ToString();
                        //LinkedList _lnkTemp = new LinkedList();
                        //_lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, _sTempModelConfigRawid);
                        //dsGroupItem = this._wsSPC.GetGroupContextValue(_lnkTemp.GetSerialData());

                        for (int i = 0; i < this.ChartVariable.dtParamData.Rows.Count; i++)
                        {
                            string strEQPIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString();
                            string strModuleIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString();

                            if (strEQPIDTemp != null && strEQPIDTemp.Length > 0)
                            {
                                for (int j = 0; j < strEQPIDTemp.Split(';').Length; j++)
                                {
                                    if (strEQPIDTemp.Split(';')[j] != null && strEQPIDTemp.Split(';')[j].Length > 0 && !strEQPIDTemp.Split(';')[j].ToUpper().Equals("NULL") && !arrEQPID.Contains(strEQPIDTemp.Split(';')[j]))
                                    {
                                        arrEQPID.Add(strEQPIDTemp.Split(';')[j]);
                                    }
                                }

                            }

                            if (strModuleIDTemp != null && strModuleIDTemp.Length > 0)
                            {
                                for (int j = 0; j < strModuleIDTemp.Split(';').Length; j++)
                                {
                                    if (strModuleIDTemp.Split(';')[j] != null && strModuleIDTemp.Split(';')[j].Length > 0 && !strModuleIDTemp.Split(';')[j].ToUpper().Equals("NULL") && !arrModuleID.Contains(strModuleIDTemp.Split(';')[j]))
                                    {
                                        arrModuleID.Add(strModuleIDTemp.Split(';')[j]);
                                    }
                                }

                            }
                        }
                    }

                    this.pnlChart.Controls.Clear();

                    if (chartBase != null)
                        chartBase.ReleaseData();

                    chartBase = null;
                    Steema.TeeChart.Tools.CursorTool _cursorTool = null;

                    for (int i = mllstChart.Count - 1; i >= 0; i--)
                    {
                        string strKey = mllstChart.GetKey(i).ToString();
                        string strValue = mllstChart.GetValue(i).ToString();
                        chartBase = new DefaultChart(_dataManager);
                        chartBase.ClearChart();
                        chartBase.Title = strValue;
                        chartBase.Name = strKey;
                        chartBase.ParamTypeCD = this.mParamTypeCd;

                        //if (dsGroupItem != null && dsGroupItem.Tables != null && dsGroupItem.Tables.Count > 0 && dsGroupItem.Tables[0].Rows.Count > 0)
                        //{
                        //    chartBase.GroupVisible = true;
                        //    chartBase.DSGROUP = dsGroupItem;
                        //}
                        //else
                        //{
                            chartBase.GroupVisible = false;
                        //}
                        chartBase.Pagekey = Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC;
                        chartBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                        chartBase.SPCChartTitlePanel.Name = strKey;
                        chartBase.Height = iHeight;
                        chartBase.URL = this.URL;

                        if (iChart == 1)
                            chartBase.Dock = System.Windows.Forms.DockStyle.Fill;
                        else
                            chartBase.Dock = System.Windows.Forms.DockStyle.Top;

                        chartBase.ContextMenu = this.bbtnListChart.ContextMenu;
                        chartBase.ParentControl = this.pnlChart;
                        chartBase.StartDateTime = ChartVariable.dateTimeStart;
                        chartBase.EndDateTime = ChartVariable.dateTimeEnd;
                        chartBase.ARREQPID = arrEQPID;
                        chartBase.ARRMODULEID = arrModuleID;
                        chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);
                        if (!chartBase.DrawSPCChart()) continue;
                        _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                        _cursorTool.Active = true;
                        _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                        _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        _cursorTool.Pen.Color = Color.Gray;
                        _cursorTool.FollowMouse = true;
                        _cursorTool.Series = chartBase.SPCChart.GetAllSeries()[0];
                        chartBase.SPCChart.Chart.Tools.Add(_cursorTool);
                        chartBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                        chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                        chartBase.SPCChart.ClickBackground += new MouseEventHandler(SPCChart_ClickBackground);
                        this.pnlChart.Controls.Add(chartBase);

                        dtRawData = chartBase.dtDataSource.Copy();
                        if (dtRawData.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                        {
                            dtRawData.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                        }
                        if (dtRawData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
                        {
                            dtRawData.Columns.Remove(CommonChart.COLUMN_NAME_SEQ_INDEX);
                        }

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
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

        public void InitializeLayout()
        {
            this.bTitlePanel1.Title = this.mMHandler.GetVariable(Definition.TITLE_KEY_SPC_ATT_CONTROL_CHART);
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        public void InitializeDataButton()
        {
            this.mInitialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC, Definition.BUTTONLIST_KEY_ATT_CHART, this.sessionData);
            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_CONTROL_CHART;
            base.ApplyAuthory(this.bbtnListChart);
        }

        public void InitializeBSpread()
        {
            this.mChartInfomationUI = new ChartInformation();
            this.mChartInfomationUI.SessionData = this.sessionData;
            this.mChartInfomationUI.ChartVariable = ChartVariable;
            this.mChartInfomationUI.dsChart = this.mDSChart;
            this.mChartInfomationUI.InitializePopup();
            this.mChartInfomationUI.Dock = DockStyle.Fill;
            this.bplInformation.Visible = true;
            for (int i = 0; i < this.bTitPanelInformation.Controls.Count; i++)
            {
                if (this.bTitPanelInformation.Controls[i].GetType() == typeof(ChartInformation))
                    this.bTitPanelInformation.Controls.RemoveAt(i);
            }
            this.bTitPanelInformation.Controls.Add(this.mChartInfomationUI);

        }


        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;
            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.
            this.InitializeCommon();
            this.InitializePage();
        }

        public override void PageSearch(LinkedList llstCondition)
        {
            DataTable dt = null;
            string sChartID = "";
            filteringPopup = null;
            isFiltered = false;
            this.includingToggleData = false;
            this.mllstSearchCondition = llstCondition;

            try
            {
                bool bChartIDExist = false;

                this.mChartVariable = new ChartInterface();
                this._dtOCAPData = null;

                if (llstCondition[Definition.DynamicCondition_Search_key.CHART_ID] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CHART_ID];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString().Length > 0)
                        {
                            bChartIDExist = true;
                            sChartID = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                        }
                    }
                }

                if (bChartIDExist)
                {
                    LinkedList lnkTemp = new LinkedList();
                    DataSet dsTempChartID = new DataSet();
                    lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sChartID);
                    dsTempChartID = this._wsSPC.GetATTSPCModelDatabyChartID(lnkTemp.GetSerialData());

                    if (dsTempChartID != null && dsTempChartID.Tables != null && dsTempChartID.Tables.Count == 3 && dsTempChartID.Tables[0].Rows.Count > 0 && dsTempChartID.Tables[1].Rows.Count > 0)
                    {
                        this.mSite = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.SITE].ToString();
                        this.mFab = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.FAB].ToString();
                        this.mLineRawID = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.LOCATION_RAWID].ToString();
                        this.mLine = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.LINE].ToString();
                        mChartVariable.LINE = this.mLine;
                        mChartVariable.SPC_MODEL = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                        mChartVariable.SPC_MODEL_RAWID = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.MODEL_RAWID].ToString();
                        mChartVariable.PARAM_ALIAS = dsTempChartID.Tables[1].Rows[0][BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();
                        mChartVariable.DEFAULT_CHART = dsTempChartID.Tables[2].Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST].ToString();
                        mChartVariable.MAIN_YN = dsTempChartID.Tables[1].Rows[0][BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
                        mChartVariable.MODEL_CONFIG_RAWID = dsTempChartID.Tables[1].Rows[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                        mChartVariable.RESTRICT_SAMPLE_DAYS = "";
                        mChartVariable.AREA = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.AREA].ToString();
                        mChartVariable.EQP_MODEL = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.EQP_MODEL].ToString();
                        this.mAreaRawID = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.AREA_RAWID].ToString();
                        this.mArea = mChartVariable.AREA;
                        this.mApplyRestric = false;
                    }

                    if (!base.ApplyAuthory(this.bbtnListChart, this.mSite, this.mFab, this.mLine, this.mArea))
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                        this.InitializePage();
                        return;
                    }
                }
                else
                {
                    if (llstCondition[Definition.DynamicCondition_Search_key.SITE] != null)
                    {
                        dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SITE];
                        this.mSite = DCUtil.GetValueData(dt);
                    }
                    else
                        this.mSite = null;

                    if (llstCondition[Definition.DynamicCondition_Search_key.FAB] != null)
                    {
                        dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.FAB];
                        this.mFab = DCUtil.GetValueData(dt);
                    }
                    else
                        this.mFab = null;

                    if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
                    {
                        dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
                        this.mLineRawID = DCUtil.GetValueData(dt);
                        this.mLine = DataUtil.GetDisplayData(dt);
                        mChartVariable.LINE = this.mLine;
                    }
                    else
                        this.mLine = null;

                    if (llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL] != null)
                    {
                        dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL];
                        mChartVariable.SPC_MODEL = DataUtil.GetDisplayData(dt);
                        mChartVariable.SPC_MODEL_RAWID = DCUtil.GetValueData(dt);
                    }

                    if (!base.ApplyAuthory(this.bbtnListChart, this.mSite, this.mFab, this.mLine, DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.DISPLAYDATA)))
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                        this.InitializePage();
                        return;
                    }

                    this.mParamTypeCd = mComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
                    if (llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID] != null)
                    {
                        dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID];

                        if (string.IsNullOrEmpty(DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.VALUEDATA)))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL_OR_CHART_ID", null, null);
                            return;
                        }

                        mChartVariable.PARAM_ALIAS = DataUtil.GetDisplayData(dt);
                        mChartVariable.DEFAULT_CHART = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.DEFAULT_CHART_LIST);
                        mChartVariable.MAIN_YN = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.MAIN_YN);
                        mChartVariable.MODEL_CONFIG_RAWID = DCUtil.GetValueData(dt);
                        mChartVariable.RESTRICT_SAMPLE_DAYS = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.RESTRICT_SAMPLE_DAYS);
                        mChartVariable.AREA = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.AREA);
                        mChartVariable.EQP_MODEL = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.EQP_MODEL);
                        this.mAreaRawID = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.AREA_RAWID);
                        this.mArea = mChartVariable.AREA;

                        LinkedList lnkTemp = new LinkedList();
                        DataSet dsTempChartID = new DataSet();
                        lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);
                        dsTempChartID = this._wsSPC.GetATTSPCModelDatabyChartID(lnkTemp.GetSerialData());

                       // this.mParamTypeCd = dsTempChartID.Tables[1].Rows[0][BISTel.eSPC.Common.COLUMN.PARAM_TYPE_CD].ToString();

                        if (string.IsNullOrEmpty(mChartVariable.MODEL_CONFIG_RAWID))
                        {
                            this.MsgClose();
                            MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "SPC Model"));
                            return;
                        }

                        if (string.IsNullOrEmpty(mChartVariable.DEFAULT_CHART))
                        {
                            this.MsgClose();
                            MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                            return;
                        }
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                        return;
                    }


                    if (llstCondition[Definition.DynamicCondition_Search_key.CONTEXT] != null)
                    {
                        this.mDTContext = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CONTEXT];
                    }

                    if (llstCondition[Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT] != null)
                    {
                        this.mDTCustomContext = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT];
                    }
                }

                if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                    this.mStartTime = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                }
                if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                    this.mEndTime = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                }
                if (llstCondition[Definition.DynamicCondition_Search_key.RESTRICT_CONDITION] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.RESTRICT_CONDITION];
                    if (dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString().ToUpper() == "TRUE")
                        this.mApplyRestric = true;
                    else
                        this.mApplyRestric = false;
                }
                if (DateTime.Parse(this.mStartTime) > DateTime.Parse(this.mEndTime))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD", null, null);
                    return;
                }

                if (this._sOCAPRawIDURL.Length > 0)
                {
                    this.mChartVariable.OCAPRawID = this._sOCAPRawIDURL;
                }

                if (llstCondition.Contains(BISTel.eSPC.Common.COLUMN.GROUP_NAME))
                {
                    dt = (DataTable)llstCondition[BISTel.eSPC.Common.COLUMN.GROUP_NAME];
                    this._sGroupName = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                }

                PROC_DataBinding();

            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }

        }

        #endregion

        #region ::: User Defined Method.

        private void GetLine()
        {
            DataSet ds = null;
            try
            {
                mllstChartSearchData.Clear();
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.LINE, ChartVariable.LINE);
                ds = _wsSPC.GetAnalysisLine(mllstChartSearchData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this.mSite = ds.Tables[0].Rows[0]["SITE"].ToString();
                    this.mFab = ds.Tables[0].Rows[0]["FAB"].ToString();
                    this.mLineRawID = ds.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                }
            }
            catch
            {
            }
            finally
            {
                if (ds != null) ds.Dispose();

            }
        }


        private void GetArea()
        {
            DataSet ds = null;
            try
            {
                this.mllstChartSearchData.Clear();
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.AREA, ChartVariable.AREA);
                ds = _wsSPC.GetAnalysisArea(mllstChartSearchData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this.mAreaRawID = ds.Tables[0].Rows[0]["RAWID"].ToString();
                }

            }
            catch
            {
            }
            finally
            {
                if (ds != null) ds.Dispose();

            }
        }


        private LinkedList GetDefaultChart()
        {
            LinkedList _llstDefaultChart = new LinkedList();
            _llstDefaultChart.Clear();

            if (DataUtil.IsNullOrEmptyDataSet(mDSChart)) return _llstDefaultChart;

            if (ChartVariable == null) return _llstDefaultChart;

            foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
            {
                string strkey = dr[BISTel.eSPC.Common.COLUMN.CODE].ToString();
                if (ChartVariable.lstDefaultChart.Contains(strkey))
                {
                    _llstDefaultChart.Add(strkey, dr[BISTel.eSPC.Common.COLUMN.DESCRIPTION].ToString());
                }
            }
            return _llstDefaultChart;
        }




        /// <summary>
        /// SPC Model이 Main이고 Custom Context인경우
        /// </summary>
        private string getCustomContext()
        {
            StringBuilder sb = new StringBuilder();
            if (this.mChartVariable.MAIN_YN.Equals("Y") && !DataUtil.IsNullOrEmptyDataTable(this.mDTCustomContext))
            {
                sb.Append(" 1=1");
                for (int i = 0; i < this.mDTCustomContext.Rows.Count; i++)
                {
                    string sKey = this.mDTCustomContext.Rows[i][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                    string sValue = this.mDTCustomContext.Rows[i][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();

                    if (sValue == Definition.VARIABLE.STAR) continue;

                    if (sValue.IndexOf(Definition.VARIABLE.STAR) > -1) continue;

                    if (sValue.IndexOf(";") > -1)
                        sb.AppendFormat(" AND {0} in ({1}) ", sKey, "'" + sValue.Replace(";", "','") + "'");
                    else
                        sb.AppendFormat(" AND {0}='{1}'", sKey, sValue);
                }
            }
            return sb.ToString();
        }


        private DataSet GetDSContextCall()
        {
            DataSet _dsContext = null;
            try
            {
                this.mllstChartSearchData.Clear();
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.mLineRawID);
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.mAreaRawID);
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.mParamTypeCd);
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this.ChartVariable.PARAM_ALIAS);
                mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID, this.ChartVariable.SPC_MODEL_RAWID);
                mllstChartSearchData.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                _dsContext = this._wsSPC.GetSPCContextList(mllstChartSearchData.GetSerialData());
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {

            }

            return _dsContext;
        }


        private DataTable PROC_CreateDTContext(LinkedList _llstContext)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.VALUEDATA, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.DISPLAYDATA, typeof(string));

            DataRow dr = null;
            for (int i = 0; i < _llstContext.Count; i++)
            {
                dr = dt.NewRow();
                dr[Definition.DynamicCondition_Condition_key.VALUEDATA] = _llstContext.GetKey(i).ToString();
                dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA] = _llstContext.GetValue(i).ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void PROC_DataBinding()
        {
            try
            {
                EESProgressBar.ShowProgress(this, this.mMHandler.GetMessage(Definition.LOADING_DATA), true);

                if (this.mStartTime.Length == 0 && this.mChartVariable != null)
                    this.mStartTime = CommonPageUtil.StartDate(this.mChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT));

                if (this.mEndTime.Length == 0 && this.mChartVariable != null)
                    this.mEndTime = CommonPageUtil.EndDate(this.mChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT));

                this.mllstChartSearchData.Clear();
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.mStartTime);
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.mEndTime);
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);
                object objDataSet = ach.SendWait(_wsSPC, "GetATTSPCControlChartData", new object[] { mllstChartSearchData.GetSerialData() });
                EESProgressBar.CloseProgress(this);

                if (objDataSet != null)
                {
                    this._ds = (DataSet)objDataSet;
                    PROC_DataBinding1();
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
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
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
            }
        }
        private void PROC_DataBinding1()
        {
            CreateChartDataTable _createChartDT = null;
            DataTable mDTChartData = null;

            mDTChartData = new DataTable();
            _createChartDT = new CreateChartDataTable();

            try
            {
                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                }
                else
                {
                    if (this.mllstChartSearchData.Contains(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST))
                    {
                        this.mllstChartSearchData.Remove(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST);
                    }
                    this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST, getCustomContext());
                    
                    mDTChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, mllstChartSearchData, false, false, includingToggleData);

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (DataUtil.IsNullOrEmptyDataTable(mDTChartData))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    }
                    else
                    {
                        mDTChartData = DataUtil.DataTableImportRow(mDTChartData.Select(null, Definition.CHART_COLUMN.TIME));

                        if (this.mApplyRestric == true)
                        {
                            int sampleDays = 0;
                            int.TryParse(mDTChartData.Rows[0][Definition.CHART_COLUMN.RESTRICT_SAMPLE_DAYS].ToString(), out sampleDays);
                            int sampleCount = 0;
                            int.TryParse(mDTChartData.Rows[0][Definition.CHART_COLUMN.RESTRICT_SAMPLE_COUNT].ToString(), out sampleCount);
                            int sampleHours = 0;
                            int.TryParse(mDTChartData.Rows[0][Definition.CHART_COLUMN.RESTRICT_SAMPLE_HOURS].ToString(), out sampleHours);
                            DateTime sampleDateTime = DateTime.Parse(mDTChartData.Rows[mDTChartData.Rows.Count - 1][Definition.CHART_COLUMN.TIME].ToString())
                                .AddDays(sampleDays * -1);
                            sampleDateTime = sampleDateTime.AddHours(sampleHours * -1);

                            for (int i = mDTChartData.Rows.Count - 1, count = 0; i >= 0; i--)
                            {
                                if (sampleCount != 0
                                    && sampleCount == count)
                                {
                                    mDTChartData.Rows[i].Delete();
                                    continue;
                                }

                                if (sampleDays != 0 || sampleHours != 0)
                                {
                                    DateTime dateTime = DateTime.Parse(mDTChartData.Rows[i][Definition.CHART_COLUMN.TIME].ToString());
                                    if (sampleDateTime > dateTime)
                                    {
                                        mDTChartData.Rows[i].Delete();
                                        continue;
                                    }
                                }

                                count++;
                            }

                            mDTChartData.AcceptChanges();

                            if (DataUtil.IsNullOrEmptyDataTable(mDTChartData))
                            {
                                this.MsgClose();
                                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                            }
                        }

                        List<string> rawIDs = new List<string>();
                        if (mDTChartData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                        {
                            bool bPOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                            bool bPNOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                            bool bCOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                            bool bUOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

                            foreach (DataRow dr in mDTChartData.Rows)
                            {
                                string rawid = dr[Definition.CHART_COLUMN.OCAP_RAWID].ToString();

                                string sTemp = rawid.Replace(";", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split(';');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }

                                if (bPOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.P_OCAP_LIST].ToString();

                                    sTemp = rawid.Replace("^", "").Replace(";", "");
                                    if (sTemp.Length > 0)
                                    {
                                        string[] ids = rawid.Replace(";", "^").Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                                continue;
                                            if (!rawIDs.Contains(id))
                                                rawIDs.Add(id);
                                        }
                                    }
                                }

                                if (bPNOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.PN_OCAP_LIST].ToString();

                                    sTemp = rawid.Replace("^", "");
                                    if (sTemp.Length > 0)
                                    {
                                        string[] ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                                continue;
                                            if (!rawIDs.Contains(id))
                                                rawIDs.Add(id);
                                        }
                                    }
                                }

                                if (bCOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.C_OCAP_LIST].ToString();

                                    sTemp = rawid.Replace("^", "");
                                    if (sTemp.Length > 0)
                                    {
                                        string[] ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                                continue;
                                            if (!rawIDs.Contains(id))
                                                rawIDs.Add(id);
                                        }
                                    }
                                }

                                if (bUOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.U_OCAP_LIST].ToString();

                                    sTemp = rawid.Replace("^", "");
                                    if (sTemp.Length > 0)
                                    {
                                        string[] ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                                continue;
                                            if (!rawIDs.Contains(id))
                                                rawIDs.Add(id);
                                        }
                                    }
                                }
                            }

                            if (rawIDs.Count == 0)
                                rawIDs.Add("");

                            LinkedList llstTmpOcapComment = new LinkedList();
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, rawIDs.ToArray());

                            if (this.mllstSearchCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
                            {
                                DataTable dt = (DataTable)this.mllstSearchCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                                llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (DateTime.Parse(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString()).AddHours(-12)).ToString(Definition.DATETIME_FORMAT_MS));
                            }
                            else if (this.mStartTime != null && this.mStartTime.Length > 0)
                            {
                                llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (DateTime.Parse(this.mStartTime).AddHours(-12)).ToString(Definition.DATETIME_FORMAT_MS));
                            }
                            if (this.mllstSearchCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
                            {
                                DataTable dt = (DataTable)this.mllstSearchCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                                llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (DateTime.Parse(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString()).AddHours(12)).ToString(Definition.DATETIME_FORMAT_MS));
                            }
                            else if (this.mEndTime!= null && this.mEndTime.Length > 0)
                            {
                                llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (DateTime.Parse(this.mEndTime).AddHours(12)).ToString(Definition.DATETIME_FORMAT_MS));
                            }

                            byte[] baData = llstTmpOcapComment.GetSerialData();

                            _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                        }

                        mChartVariable.lstDefaultChart.Clear();
                        mChartVariable.complex_yn = Definition.VARIABLE_Y;
                        mChartVariable.OPERATION_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        mChartVariable.PRODUCT_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                        mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(mChartVariable.DEFAULT_CHART);
                        mChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        mChartVariable.dateTimeStart = DateTime.Parse(this.mStartTime);
                        mChartVariable.dateTimeEnd = DateTime.Parse(this.mEndTime);
                        _createChartDT.COMPLEX_YN = Definition.VARIABLE_Y;
                        mChartVariable.dtParamData = _createChartDT.GetMakeDataTable(mDTChartData);
                        mChartVariable.lstRawColumn = _createChartDT.lstRawColumn;

                        if (_dsOcapComment != null)
                            mChartVariable.dtOCAP = _dsOcapComment.Tables[0];

                        if (mDTChartData != null) mDTChartData.Dispose();
                        if (_createChartDT != null) _createChartDT = null;
                    }
                }
                this.InitializePage();
            }
            catch (Exception ex)
            {
                this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.MsgClose();
            }
        }


        #endregion

        #region ::: EventHandler

        #region ::: Event Methods
        private BaseChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;
            while (parentControl != null)
            {
                if (parentControl is BaseChart)
                    return parentControl as BaseChart;
                else
                    parentControl = parentControl.Parent;
            }

            return null;
        }

        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName, DataTable dtToolTipData)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            string _sEQPID = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";
            string _sProductID = "-";
            string _sRecipeID = "-";

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
            {
                _sEQPID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();
                if (_sEQPID.Length > 0 && _sEQPID.Split(';').Length > 1)
                {
                    _sEQPID = _sEQPID.Split(';')[0];
                }
            }


            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
            {
                _sLOTID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();
                if (_sLOTID.Length > 0 && _sLOTID.Split(';').Length > 1)
                {
                    _sLOTID = _sLOTID.Split(';')[0];
                }
            }

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
            {
                _sOperationID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                if (_sOperationID.Length > 0 && _sOperationID.Split(';').Length > 1)
                {
                    _sOperationID = _sOperationID.Split(';')[0];
                }
            }

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
            {
                _sSubstrateID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();
                if (_sSubstrateID.Length > 0 && _sSubstrateID.Split(';').Length > 1)
                {
                    _sSubstrateID = _sSubstrateID.Split(';')[0];
                }
            }

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
            {
                _sProductID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                if (_sProductID.Length > 0 && _sProductID.Split(';').Length > 1)
                {
                    _sProductID = _sProductID.Split(';')[0];
                }
            }

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.RECIPE_ID))
            {
                _sRecipeID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.RECIPE_ID].ToString();
                if (_sRecipeID.Length > 0 && _sRecipeID.Split(';').Length > 1)
                {
                    _sRecipeID = _sRecipeID.Split(';')[0];
                }
            }


            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.RECIPE_ID), _sRecipeID);

            if (mllstChart.Contains(strChartName))
            {
                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString();

                    if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LSL) ||
                        sValue.Equals(Definition.CHART_COLUMN.UCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.TARGET) ||
                        sValue.Equals(Definition.COL_TOGGLE_YN) ||
                        sValue.Equals(Definition.CHART_COLUMN.DTSOURCEID)) continue;

                    if (dtToolTipData.Columns.Contains(sKey))
                    {
                        if (dtToolTipData.Rows[rowIndex][sKey] != null && dtToolTipData.Rows[rowIndex][sKey].ToString().Length > 0)
                        {
                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                        }
                    }
                }
                llstChartSeries = null;
            }

            return sb.ToString();
        }

        private string CreateToolTipStringRaw(string strCol, string strValue, int rowIndex, string strChartName, DataTable dtToolTipData)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            string _sEQPID = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";
            string _sProductID = "-";
            string _sRecipeID = "-";

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                _sEQPID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                _sLOTID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                _sOperationID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                _sSubstrateID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                _sProductID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.RECIPE_ID))
                _sRecipeID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.RECIPE_ID].ToString();

            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.RECIPE_ID), _sRecipeID);

            

            if (mllstChart.Contains(strChartName))
            {
                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                if (!(llstChartSeries.Contains(Definition.BLOB_FIELD_NAME.PARAM_LIST) || llstChartSeries.Contains(Definition.BLOB_FIELD_NAME.PARAM_LIST.ToUpper())))
                {
                    llstChartSeries.Add(Definition.BLOB_FIELD_NAME.PARAM_LIST, Definition.BLOB_FIELD_NAME.PARAM_LIST);
                }
                for (int i = llstChartSeries.Count - 1; i >= 0; i--)
                {
                    if (llstChartSeries.GetKey(i).ToString().Contains("wafer"))
                    {
                        llstChartSeries.Remove(llstChartSeries.GetKey(i));
                    }
                }
                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString();

                    if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LSL) ||
                        sValue.Equals(Definition.CHART_COLUMN.UCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.TARGET) ||
                        sValue.Equals(Definition.CHART_COLUMN.DTSOURCEID)) continue;

                    if (dtToolTipData.Columns.Contains(sKey))
                    {
                        if (dtToolTipData.Rows[rowIndex][sKey] != null && dtToolTipData.Rows[rowIndex][sKey].ToString().Length > 0)
                        {
                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                        }
                    }
                }
                llstChartSeries = null;
            }

            return sb.ToString();
        }

        private string GetOCAPRawID(DataTable dt, int rowIndex)
        {
            string strRawid = string.Empty;
            if (dt.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                strRawid = dt.Rows[0][Definition.CHART_COLUMN.OCAP_RAWID].ToString();

            if (string.IsNullOrEmpty(strRawid))
            {
                if (this.clickedChart != null)
                {
                    switch (this.clickedChart.Name)
                    {
                        case Definition.CHART_TYPE.P:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Replace("^",";");
                            break;
                        case Definition.CHART_TYPE.PN:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.C:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.U:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Replace("^", ";");
                            break;

                    }
                }
            }

            return strRawid;
        }




        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                BaseChart baseChart = FindSPCChart((Control)sender);
                Annotation ann;
                if (baseChart is DefaultChart)
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
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
            finally
            {

            }
        }

        private void ClickButtonCalc()
        {
            Common.ChartCalculationPopup chartCalcPopup = new ChartCalculationPopup();
            chartCalcPopup.URL = this.URL;
            chartCalcPopup.SessionData = this.sessionData;
            chartCalcPopup.SModelConfigRawID = ChartVariable.MODEL_CONFIG_RAWID;
            chartCalcPopup.InitializePopup();
            chartCalcPopup.ShowDialog(this);
        }

        private void ClickButtonChartView()
        {
            if (IsToggleInformationChanged())
            {
                if (DialogResult.Yes != MSGHandler.DialogQuestionResult("SPC_INFO_NOT_SAVE_TOGGLE_INFO",
                                                                        new string[] { "" }, MessageBoxButtons.YesNo))
                    return;
            }

            BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup chartViewPop = new BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup();
            chartViewPop.URL = this.URL;
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.DataSetChart = mDSChart; //Chart 종류   
            chartViewPop.llstDefaultChart = mllstChart;
            chartViewPop.InitializePopup();
            DialogResult result = chartViewPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                mllstChart.Clear();
                mllstChart = chartViewPop.llstResultListChart;
                this.InitializeChartSeriesVisibleType();
                this.InitializeChart(); //chart 다시 그림  

                chartViewPop.Dispose();
            }
        }

        private void ClickButtonConfiguration()
        {
            Modeling.SPCConfigurationPopup spcConifgPopup = new Modeling.SPCConfigurationPopup();
            spcConifgPopup.SESSIONDATA = this.sessionData;
            spcConifgPopup.URL = this.URL;
            spcConifgPopup.PORT = this.Port;
            spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.MODIFY;
            spcConifgPopup.CONFIG_RAWID = ChartVariable.MODEL_CONFIG_RAWID;
            spcConifgPopup.MAIN_YN = ChartVariable.MAIN_YN;
            spcConifgPopup.LINE_RAWID = this.mLineRawID; //SPC-1292, KBLEE
            spcConifgPopup.AREA_RAWID = this.mAreaRawID; //SPC-1292, KBLEE

            if (ChartVariable.MAIN_YN.ToUpper() == "Y")
            {
                if (0 < _wsSPC.GetTheNumberOfSubConfigOfModel(ChartVariable.MODEL_CONFIG_RAWID))
                    spcConifgPopup.HAS_SUBCONFIGS = true;
                else
                    spcConifgPopup.HAS_SUBCONFIGS = false;
            }

            spcConifgPopup.GROUP_NAME = _sGroupName;
            spcConifgPopup.InitializePopup();
            DialogResult result = spcConifgPopup.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                //SPC-1292, KBLEE, START : Group 처리
                if (this.CHART_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART)
                {
                    if (llstSearchCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtModelMst = spcConifgPopup.SPCMODELDATA_DATASET.Tables[BISTel.eSPC.Common.TABLE.MODEL_MST_SPC];

                        DataTable dtSPCModel = (DataTable)this.llstSearchCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME] = spcConifgPopup.GROUP_NAME;

                        DataTable dtSPCGroup = (DataTable)llstSearchCondition[Definition.CONDITION_KEY_GROUP_NAME];

                        dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = spcConifgPopup.GROUP_NAME;
                        dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = spcConifgPopup.GROUP_NAME;
                        dtSPCGroup.Rows[0]["CHECKED"] = "T";

                        this._sGroupName = spcConifgPopup.GROUP_NAME;

                        this.RefreshConditions(llstSearchCondition);
                    }
                }

                this.GROUP_NAME = spcConifgPopup.GROUP_NAME;
                //SPC-1292, KBLEE, END : Group 처리

                spcConifgPopup.Dispose();
            }

        }

        private void ClickButtonShowChartData()
        {
            BISTel.eSPC.Page.ATT.Common.ChartDataPopup chartDataPop = null;
            if (isFiltered)
            {
                chartDataPop = new ChartFilteredDataPopup { URL = this.URL, SessionData = this.sessionData };
                DataSet filteredParam = new DataSet();
                foreach (DataTable table in _dataManager.FilteredRawDataSet.Tables)
                {
                    DataTable dt = CommonPageUtil.ExcelExport(table.Copy(), ChartVariable.lstRawColumn);
                    dt.TableName = table.TableName;
                    filteredParam.Tables.Add(dt);
                }
                chartDataPop.AddParamData(filteredParam);
            }
            else
            {
                chartDataPop = new ChartDataPopup { URL = this.URL, SessionData = this.sessionData };
                DataTable dt = _dataManager.RawDataTable.Copy();
                chartDataPop.AddParamData(CommonPageUtil.ExcelExport(dt, ChartVariable.lstRawColumn));
            }

            //if (this.mParamTypeCd != "MET")
            //{
            //    if (this.dtRawData != null)
            //    {
            //        chartDataPop.isVisibleRawData = true;
            //        if (isFiltered)
            //        {
            //            DataSet filteredRaw = new DataSet();
            //            foreach (DataTable table in this.filteredRawData.Tables)
            //            {
            //                DataTable dt = CommonPageUtil.ExcelExportRaw(table, ChartVariable.lstRawColumn);
            //                dt.TableName = table.TableName;
            //                filteredRaw.Tables.Add(dt);
            //            }
            //            chartDataPop.AddRawData(filteredRaw);
            //        }
            //        else
            //        {
            //            DataTable dtRaw = this.dtRawData.Copy();
            //            chartDataPop.AddRawData(CommonPageUtil.ExcelExportRaw(dtRaw, ChartVariable.lstRawColumn));
            //        }
            //    }
            //}

            chartDataPop.InitializePopup();
            DialogResult result = chartDataPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                chartDataPop.Dispose();
            }
        }


        private void CursorSynchronize(Steema.TeeChart.Tools.CursorTool SRC, Steema.TeeChart.Tools.CursorTool DEST)
        {
            DEST.XValue = SRC.XValue;
            DEST.YValue = SRC.YValue;
        }

        #endregion



        private void SPCChart_Resize(object sender, System.EventArgs e)
        {
            BaseChart baseChart = null;
            int iCount = this.pnlChart.Controls.Count;
            int iHeight = 0;
            if (iCount == 1)
            {
                iHeight = this.pnlChart.Height;
            }
            else if (iCount > 1)
            {
                iHeight = this.pnlChart.Height / 2;
            }


            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.Height = iHeight;
            }
        }


        private void bTitlePanel1_Resize(object sender, System.EventArgs e)
        {

            BTitlePanel _bTitlePanel = sender as BTitlePanel;

            try
            {
                SPCChart_Resize(this, null);
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }

        private void bchk_Click(object sender, System.EventArgs e)
        {
            BCheckBox chk = (BCheckBox)sender;
            if (chk == null) return;
            BaseChart baseChart = null;
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];
                this.mllstChartSeriesVisibleType.Remove(chk.Text);
                this.mllstChartSeriesVisibleType.Add(chk.Text, chk.Checked);
                baseChart.llstChartSeriesVisibleType = mllstChartSeriesVisibleType;
                baseChart.ChangeSeriesStyle(chk.Text, chk.Checked);
            }
        }



        void SPCChart_ClickBackground(object sender, MouseEventArgs e)
        {
            BaseChart baseChart = FindSPCChart((Control)sender);
            RemoveToggleOn(baseChart);
            RemoveToggleOff(baseChart);
        }

        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                    if (seriesInfo == null ||
                        !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX) ||
                        s.IsNull(valueIndex)) return;

                    #region ToolTip
                    BaseChart baseChart = FindSPCChart((Control)sender);
                    if (baseChart is DefaultChart)
                    {
                        this.clickedChart = baseChart;
                        this.clickedSeries = s;
                        this.clickedIndex = valueIndex;

                        RefreshToggleOnOffInContextMenu(baseChart, s, valueIndex);

                        string strCol = s.Title;
                        if (strCol == Definition.CHART_COLUMN.USL_LSL)
                            strCol = Definition.CHART_COLUMN.USL;
                        else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                            strCol = Definition.CHART_COLUMN.UCL;

                        string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();
                        int _iSEQIdx = Convert.ToInt32(seriesInfo.SeriesData.Rows[valueIndex][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());

                        if (this.mParamTypeCd != "MET" && baseChart.Name == "RAW")
                        {
                            this.isRawClick = true;
                            if (valueIndex == _iSEQIdx)
                            {
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[valueIndex], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[valueIndex]);
                                iValueIndex = valueIndex;
                            }
                            else
                            {
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[_iSEQIdx], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[_iSEQIdx]);
                                iValueIndex = _iSEQIdx;
                            }
                        }
                        else
                        {
                            this.isRawClick = false;
                            if (valueIndex == _iSEQIdx)
                            {
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[valueIndex], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[valueIndex]);
                                iValueIndex = valueIndex;
                            }
                            else
                            {
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[_iSEQIdx], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[_iSEQIdx]);
                                iValueIndex = _iSEQIdx;
                            }
                        }

                        Annotation ann = null;

                        for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                        {
                            if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                            {
                                ann = (Annotation)baseChart.SPCChart.Chart.Tools[i];
                            }
                        }

                        if (ann != null)
                        {
                            if (isRawClick)
                            {
                                if (valueIndex == _iSEQIdx)
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipStringRaw(strCol, strValue, valueIndex, baseChart.NAME, baseChart.dtDataSourceToolTip));
                                else
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipStringRaw(strCol, strValue, _iSEQIdx, baseChart.NAME, baseChart.dtDataSourceToolTip));
                            }
                            else
                            {
                                if (valueIndex == _iSEQIdx)
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseChart.NAME, baseChart.dtDataSourceToolTip));
                                else
                                    this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, _iSEQIdx, baseChart.NAME, baseChart.dtDataSourceToolTip));
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                iValueIndex = -1;
            }
        }

        private void RefreshToggleOnOffInContextMenu(BaseChart baseChart, Series s, int valueIndex)
        {
            RemoveToggleOn(baseChart);
            RemoveToggleOff(baseChart);

            if (!IsToggleFlagNeeded(baseChart, s))
                return;

            string toggleInfo = GetToggleInformation(baseChart, s, valueIndex);

            if (toggleInfo == "Y")
            {
                AddToggleOn(baseChart);
            }
            else if (toggleInfo == "N")
            {
                AddToggleOff(baseChart);
            }
        }

        private string GetToggleInformation(BaseChart baseChart, int seqIndex)
        {
            DataRow[] dr = baseChart.dtDataSource.Select("$SEQ_INDEX$ = '" + seqIndex + "'");
            if (dr.Length > 0)
            {
                DataRow[] rawDr = this._dataManager.RawDataTable.Select(Definition.CHART_COLUMN.DTSOURCEID + " = '" + dr[0][Definition.CHART_COLUMN.DTSOURCEID] + "'");

                switch (rawDr[0][Definition.CHART_COLUMN.TABLENAME].ToString())
                {
                    case Definition.TableName.USERNAME_DATA_SHORT:
                    //case Definition.TableName.USERNAME_TEMPDATA_SHORT:
                        return rawDr[0][Definition.COL_TOGGLE_YN].ToString();
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetToggleInformation(BaseChart baseChart, Series s, int valueIndexOfSeries)
        {
            string seq_index = ((DataSet)s.DataSource).Tables[0].Rows[valueIndexOfSeries]["X"].ToString();

            return GetToggleInformation(baseChart, int.Parse(seq_index));
        }

        private void AddToggleOn(BaseChart baseChart)
        {
            MenuItem mi = new MenuItem("Toggle On");
            mi.Name = "Toggle On";
            mi.Click += new EventHandler(ToggleOn);
            baseChart.ContextMenu.MenuItems.Add(mi);
        }

        private void AddToggleOff(BaseChart baseChart)
        {
            MenuItem mi = new MenuItem("Toggle Off");
            mi.Name = "Toggle Off";
            mi.Click += new EventHandler(ToggleOff);
            baseChart.ContextMenu.MenuItems.Add(mi);
        }

        private void RemoveToggleOn(BaseChart baseChart)
        {
            baseChart.ContextMenu.MenuItems.RemoveByKey("Toggle On");
        }

        private void RemoveToggleOff(BaseChart baseChart)
        {
            baseChart.ContextMenu.MenuItems.RemoveByKey("Toggle Off");
        }

        private void ToggleOn(System.Object sender, System.EventArgs e)
        {
            ChangeToggleValue("N");
        }

        private void ToggleOff(System.Object sender, System.EventArgs e)
        {
            ChangeToggleValue("Y");
        }

        private void ChangeToggleValue(string value)
        {
            string seq_index = ((DataSet)clickedSeries.DataSource).Tables[0].Rows[clickedIndex]["X"].ToString();
            DataRow[] dr = clickedChart.dtDataSource.Select("$SEQ_INDEX$ = '" + seq_index + "'");
            if (dr.Length > 0)
            {
                DataRow[] rawDr = this._dataManager.RawDataTable.Select(Definition.CHART_COLUMN.DTSOURCEID + " = '" + dr[0][Definition.CHART_COLUMN.DTSOURCEID] + "'");

                switch (rawDr[0][Definition.CHART_COLUMN.TABLENAME].ToString())
                {
                    case Definition.TableName.USERNAME_DATA_SHORT:
                    //case Definition.TableName.USERNAME_TEMPDATA_SHORT:
                        rawDr[0][Definition.COL_TOGGLE_YN] = value;
                        this.dtRawData.Rows[int.Parse(seq_index)][Definition.COL_TOGGLE_YN] = value;
                        break;
                }

                UpdateToggleValuesOfCharts();

                RefreshTogglePoint();
            }
        }

        private void UpdateToggleValuesOfCharts()
        {
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                BaseChart baseChart = (BaseChart)this.pnlChart.Controls[i];

                for (int j = 1; j < baseChart.dtDataSource.Rows.Count; j++)
                {
                    string toggleYN = GetToggleInformation(baseChart, j);
                    baseChart.dtDataSource.Rows[j][Definition.COL_TOGGLE_YN] = toggleYN;
                }
            }
        }

        private void RefreshTogglePoint()
        {
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                BaseChart baseChart = (BaseChart)this.pnlChart.Controls[i];

                baseChart.SPCChart.PointCrossLine.PointHighLightOff();

                List<SeriesInfo> siList = new List<SeriesInfo>();
                List<List<int>> indexLists = new List<List<int>>();
                for (int j = 0; j < baseChart.SPCChart.Chart.Series.Count; j++)
                {
                    if (!IsToggleFlagNeeded(baseChart, baseChart.SPCChart.Chart.Series[j]))
                        continue;

                    SeriesInfo si = baseChart.SPCChart.GetSeriesInfo(baseChart.SPCChart.Chart.Series[j]);
                    siList.Add(si);
                    List<int> indexList = new List<int>();
                    indexLists.Add(indexList);
                    for (int k = 0; k < baseChart.SPCChart.Chart.Series[j].Count; k++)
                    {
                        string toggleInfo = this.GetToggleInformation(baseChart, baseChart.SPCChart.Chart.Series[j], k);
                        if (toggleInfo == "Y")
                        {
                            indexList.Add(k);
                        }
                    }
                }

                HighlightOnPoint(baseChart, siList, indexLists, Color.Gold);
            }
        }

        private void bbtnListChart_ButtonClick(string name)
        {
            try
            {
                if (ChartVariable == null || ChartVariable.dtParamData == null || ChartVariable.dtParamData.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                if (name.ToUpper() == Definition.ButtonKey.CALC)
                    this.ClickButtonCalc();
                else if (name.ToUpper() == Definition.ButtonKey.SELECT_CHART_TO_VIEW)
                    this.ClickButtonChartView();
                else if (name.ToUpper() == Definition.ButtonKey.DEFAULT_CHART)
                {
                    mllstChart.Clear();
                    mllstChart = GetDefaultChart();
                    this.InitializeChartSeriesVisibleType();
                    this.InitializeChart();
                }
                else if (name.ToUpper() == Definition.ButtonKey.CONFIGURATION)
                    this.ClickButtonConfiguration();
                else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)
                    this.ClickButtonShowChartData();
                else if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW || name.ToUpper() == Definition.ButtonKey.OCAP_MODIFY)
                {

                    if (mChartInfomationUI.bsprInformationData.ActiveSheet.RowCount == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    if (iValueIndex == -1)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_CLICK_SERIES));
                        return;
                    }
                    //if (this.isRawClick)
                    //{
                    
                    string strRawid = GetOCAPRawID(this._dtOCAPData, iValueIndex);
                    string[] arr = strRawid.Split(';');
                    string sRawid = string.Empty;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i])) continue;
                        sRawid += arr[i] + ",";
                    }
                    if (sRawid.EndsWith(",")) sRawid = sRawid.Substring(0, sRawid.Length - 1);

                    if (string.IsNullOrEmpty(sRawid))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    BISTel.eSPC.Page.ATT.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.ATT.OCAP.OCAPDetailsPopup();
                    popupOCAP.BSHOWVIEWCHARTBUTTON = false;
                    popupOCAP.ChartVariable = (ChartInterface)ChartVariable.Clone();
                    popupOCAP.ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.OCAP;
                    popupOCAP.ChartVariable.OPERATION_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                    popupOCAP.ChartVariable.PRODUCT_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PRODUCT_ID], 1].Text;
                    popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(dtRawData, iValueIndex);
                    popupOCAP.ChartVariable.OCAPRawID = sRawid;
                    if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW)
                        popupOCAP.PopUpType = BISTel.eSPC.Common.enum_PopupType.View;
                    else
                        popupOCAP.PopUpType = BISTel.eSPC.Common.enum_PopupType.Modify;
                    popupOCAP.SessionData = this.sessionData;
                    popupOCAP.URL = this.URL;
                    popupOCAP.InitializePopup();
                    DialogResult result = popupOCAP.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        popupOCAP.Dispose();
                    }
                }
                else if (name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART)
                {

                    string _sOperationID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                    string _sMetOperationID = string.Empty;
                    string _sParamItem = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PARAM_ALIAS], 1].Text;

                    if (this.mParamTypeCd.Equals("MET"))
                        _sMetOperationID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.MEASURE_OPERATION_ID], 1].Text;
                    else
                        _sMetOperationID = _sOperationID;


                    LinkedList lk = new LinkedList();
                    lk.Add(Definition.DynamicCondition_Search_key.SITE, CommonPageUtil.AddArrayList(this.mSite));
                    lk.Add(Definition.DynamicCondition_Search_key.FAB, CommonPageUtil.AddArrayList(this.mFab));
                    lk.Add(Definition.DynamicCondition_Search_key.LINE, CommonPageUtil.AddArrayList(this.mLineRawID + ";" + this.mLine));
                    lk.Add(Definition.DynamicCondition_Search_key.AREA, CommonPageUtil.AddArrayList(this.mAreaRawID + ";" + this.mArea));
                    lk.Add(Definition.DynamicCondition_Search_key.PARAM_TYPE, CommonPageUtil.AddArrayList(this.mParamTypeCd));

                    if (!string.IsNullOrEmpty(ChartVariable.EQP_MODEL))
                        lk.Add(Definition.DynamicCondition_Search_key.EQPMODEL, CommonPageUtil.AddArrayList(ChartVariable.EQP_MODEL));
                    lk.Add(Definition.DynamicCondition_Search_key.OPERATION, CommonPageUtil.AddArrayList(_sMetOperationID));
                    lk.Add(Definition.DynamicCondition_Search_key.PARAM, CommonPageUtil.AddArrayList(_sParamItem));
                    lk.Add(Definition.DynamicCondition_Search_key.SUBDATA, CommonPageUtil.AddArrayList("MEAN" + ";" + "AVG"));
                    lk.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, CommonPageUtil.AddArrayList("EQP_ID" + ";" + "MACHINE"));
                    lk.Add(Definition.DynamicCondition_Search_key.TARGET_TYPE, CommonPageUtil.AddArrayList("PROC;PROC"));
                    lk.Add(Definition.DynamicCondition_Search_key.TARGET, CommonPageUtil.AddArrayList(_sOperationID));
                    lk.Add(Definition.DynamicCondition_Search_key.CHART_LIST, CommonPageUtil.AddArrayList("RUN" + ";" + "RUN Chart"));
                    lk.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, CommonPageUtil.AddArrayList("CUSTOM"));
                    lk.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, CommonPageUtil.AddArrayList(ChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT_MS)));
                    lk.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, CommonPageUtil.AddArrayList(ChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT_MS)));


                    base.SubmitPage("DLLS/eSPC/BISTel.eSPC.Page.dll", "BISTel.eSPC.Page.Analysis.SPCAnalysisUC", "Analysis", "Analysis", false, lk);


                }
                else if (name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT)
                {
                    if (ChartVariable.MAIN_YN != "Y")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MAIN", null, null);
                        return;
                    }
                    if (this.strucContextinfo == null)
                        this.strucContextinfo = new SPCStruct.ChartContextInfo();

                    this.strucContextinfo.AREA = this.ChartVariable.AREA;
                    this.strucContextinfo.LINE = this.ChartVariable.LINE;
                    this.strucContextinfo.MODEL_CONFIG_RAWID = this.ChartVariable.MODEL_CONFIG_RAWID;
                    this.strucContextinfo.MAIN_YN = this.ChartVariable.MAIN_YN;
                    this.strucContextinfo.SPC_MODEL_NAME = this.ChartVariable.SPC_MODEL;
                    this.strucContextinfo.PARAM_ALIAS = this.ChartVariable.PARAM_ALIAS;
                    this.strucContextinfo.SPCModelType = BISTel.eSPC.Common.SPCMODEL_TYPE.SPC_CHART;

                    DataSet _ds = GetDSContextCall();
                    if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                        this.strucContextinfo.DTModelContext = _ds.Tables[0];

                    if (!DataUtil.IsNullOrEmptyDataTable(this.mDTCustomContext))
                    {
                        this.strucContextinfo.llstCustomContext = new LinkedList();
                        foreach (DataRow dr in this.mDTCustomContext.Rows)
                            this.strucContextinfo.llstCustomContext.Add(dr[Definition.DynamicCondition_Condition_key.VALUEDATA], dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA]);
                    }

                    if (!DataUtil.IsNullOrEmptyDataTable(this.mDTContext))
                    {
                        this.strucContextinfo.llstContext = new LinkedList();
                        foreach (DataRow dr in this.mDTContext.Rows)
                            this.strucContextinfo.llstContext.Add(dr[Definition.DynamicCondition_Condition_key.VALUEDATA], dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA]);
                    }

                    

                    BISTel.eSPC.Condition.ATT.Report.SPCChartConditionPopup _chartConditionPop = new BISTel.eSPC.Condition.ATT.Report.SPCChartConditionPopup();
                    _chartConditionPop.CONTEXT_INFO = strucContextinfo;
                    _chartConditionPop.InitializePopup();
                    _chartConditionPop.ShowDialog();
                    DialogResult dResult = _chartConditionPop.ButtonResult;
                    if (dResult == DialogResult.OK)
                    {
                        this.strucContextinfo = _chartConditionPop.CONTEXT_INFO;
                        this.mDTContext = PROC_CreateDTContext(this.strucContextinfo.llstContext);
                        this.mDTCustomContext = PROC_CreateDTContext(this.strucContextinfo.llstCustomContext);
                        this.PROC_DataBinding();
                    }
                }
                else if (name.ToUpper() == "SPC_FIND_ANNOTATION")
                {
                    this.FindAnnotationOCAP();
                }
                else if (name.ToUpper() == "SAVE")
                {
                    this.SaveToggleInformation();
                }
                else if (name.ToUpper() == "SPC_SHOW_EXCLUDED")
                {
                    this.ChangeToggleVisible();
                }
                else if (name.ToUpper() == "FILTER")
                {
                    this.Filter();
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        void SPCChart_ActiveDynamicCondition(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ActiveDCEventArgs csea)
        {
        }



        private void bTitPanelInformation_MinimizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 40;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
        }

        private void bTitPanelInformation_MaximizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 300;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
        }

        private void FindAnnotationOCAP()
        {
            try
            {
                if (IsToggleInformationChanged())
                {
                    if (DialogResult.Yes !=
                        MSGHandler.DialogQuestionResult("SPC_INFO_NOT_SAVE_TOGGLE_INFO",
                                                        new string[] { "" }, MessageBoxButtons.YesNo))
                        return;
                }

                this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();

                bool isExistAnnotatedOCAP = false;
                base.MsgShow("Finding Annotated OCAP Data");
                BaseChart baseChart = null;
                SortedList lnkOCAPData = new SortedList();
                string sOCAPRAWID = "";
                Series sOCAPSeries = null;

                for (int i = 0; i < this.pnlChart.Controls.Count; i++)
                {
                    sOCAPRAWID = "";
                    lnkOCAPData.Clear();
                    baseChart = (BaseChart)this.pnlChart.Controls[i];

                    baseChart.SPCChart.PointCrossLine.PointHighLightOff();

                    if (baseChart.SPCChart.Chart.Series.Count > 0)
                    {
                        for (int n = 0; n < baseChart.SPCChart.Chart.Series.Count; n++)
                        {
                            if (baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.P ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.PN ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.C ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.U
                                )
                            {
                                sOCAPSeries = baseChart.SPCChart.Chart.Series[n];
                            }
                        }
                    }

                    bool bPOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                    bool bPNOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                    bool bCOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                    bool bUOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);


                    for (int j = 0; j < baseChart.dtDataSourceToolTip.Rows.Count; j++)
                    {
                        string sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").Replace(",", "").TrimEnd();

                        if (!string.IsNullOrEmpty(sTempOcapRawID))
                        {
                            sTempOcapRawID = null;
                            string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Split(';');
                            string sRawid = string.Empty;
                            for (int k = 0; k < arr.Length; k++)
                            {
                                if (string.IsNullOrEmpty(arr[k])) continue;
                                sRawid += "," + arr[k];
                            }

                            sOCAPRAWID += sRawid;
                            string[] sArrTemp = sRawid.Split(',');
                            for (int l = 0; l < sArrTemp.Length; l++)
                            {
                                if (sArrTemp[l].Length > 0)
                                {
                                    if (!lnkOCAPData.Contains(sArrTemp[l]))
                                    {
                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                    }
                                    else
                                    {
                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                    }
                                }

                            }
                            sArrTemp = null;
                        }

                        switch (baseChart.Name)
                        {
                            case Definition.CHART_TYPE.P:
                                if (bPOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0)
                                            {
                                                if (!lnkOCAPData.Contains(sArrTemp[l]))
                                                {
                                                    lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                }
                                                else
                                                {
                                                    lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }

                                break;

                            case Definition.CHART_TYPE.PN:
                                if (bPNOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0)
                                            {
                                                if (!lnkOCAPData.Contains(sArrTemp[l]))
                                                {
                                                    lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                }
                                                else
                                                {
                                                    lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.C:
                                if (bCOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0)
                                            {
                                                if (!lnkOCAPData.Contains(sArrTemp[l]))
                                                {
                                                    lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                }
                                                else
                                                {
                                                    lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.U:
                                if (bUOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0)
                                            {
                                                if (!lnkOCAPData.Contains(sArrTemp[l]))
                                                {
                                                    lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                }
                                                else
                                                {
                                                    lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;
                        }
                    }

                    if (lnkOCAPData.Count > 0)
                    {
                        this.mllstChartSearchData.Clear();
                        if (this.mStartTime.Length == 0)
                            this.mStartTime = CommonPageUtil.StartDate(this.mChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT));

                        if (this.mEndTime.Length == 0)
                            this.mEndTime = CommonPageUtil.EndDate(this.mChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT));


                        this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.mStartTime);
                        this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.mEndTime);
                        this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);
                        this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.OCAP_RAWID, lnkOCAPData);
                        DataSet dsTemp = _wsSPC.FindAnnotationOCAP(mllstChartSearchData.GetSerialData());

                        if (dsTemp != null && dsTemp.Tables != null && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            isExistAnnotatedOCAP = true;
                            ArrayList arrAnnotationIdx = new ArrayList();
                            List<int> lsAnnotationIdxTemp = new List<int>();
                            List<List<int>> lsAnnotationIdx = new List<List<int>>();
                            for (int m = 0; m < dsTemp.Tables[0].Rows.Count; m++)
                            {
                                if (lnkOCAPData.Contains(dsTemp.Tables[0].Rows[m][0].ToString()))
                                {
                                    string stridxTemp = lnkOCAPData[dsTemp.Tables[0].Rows[m][0].ToString()].ToString();
                                    string[] strArridxTemp = stridxTemp.Split(',');
                                    for (int k = 0; k < strArridxTemp.Length; k++)
                                    {
                                        int idxTemp = Convert.ToInt32(strArridxTemp[k]);
                                        if (!arrAnnotationIdx.Contains(idxTemp))
                                        {
                                            arrAnnotationIdx.Add(idxTemp);
                                            lsAnnotationIdxTemp.Add(idxTemp);
                                        }
                                    }
                                    stridxTemp = null;
                                    strArridxTemp = null;
                                }
                            }
                            arrAnnotationIdx = null;
                            SeriesInfo si = baseChart.SPCChart.GetSeriesInfo(sOCAPSeries);
                            List<SeriesInfo> lsSeries = new List<SeriesInfo>();
                            lsSeries.Add(si);

                            lsAnnotationIdx.Add(lsAnnotationIdxTemp);

                            baseChart.SPCChart.PointCrossLine.PointHighLightOn(lsSeries, lsAnnotationIdx);
                            baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].SeriesColor = Color.Green;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).LinePen.Visible = false;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.InflateMargins = true;
                        }
                    }
                }
                if (!isExistAnnotatedOCAP)
                {
                    base.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_ANNOTATE_OCAP", null, null);
                }
            }
            catch
            {
                base.MsgClose();
            }
            finally
            {
                base.MsgClose();
            }
        }

        private void FindSelectedOCAP()
        {
            try
            {
                base.MsgShow("FIND Selected OCAP DATA");
                BaseChart baseChart = null;
                ArrayList arrIdxOCAP = new ArrayList();
                string sOCAPRAWID = "";
                Series sOCAPSeries = null;

                for (int i = 0; i < this.pnlChart.Controls.Count; i++)
                {
                    sOCAPRAWID = "";
                    arrIdxOCAP.Clear();
                    baseChart = (BaseChart)this.pnlChart.Controls[i];

                    baseChart.SPCChart.PointCrossLine.PointHighLightOff();

                    if (baseChart.SPCChart.Chart.Series.Count > 0)
                    {
                        for (int n = 0; n < baseChart.SPCChart.Chart.Series.Count; n++)
                        {
                            if (baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.P ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.PN ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.C ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.U
                                )
                            {
                                sOCAPSeries = baseChart.SPCChart.Chart.Series[n];
                            }
                        }
                    }

                    bool bPOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                    bool bPNOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                    bool bCOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                    bool bUOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);


                    for (int j = 0; j < baseChart.dtDataSourceToolTip.Rows.Count; j++)
                    {
                        string sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").Replace(",", "").TrimEnd();

                        if (!string.IsNullOrEmpty(sTempOcapRawID))
                        {
                            sTempOcapRawID = null;
                            string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Split(';');
                            string sRawid = string.Empty;
                            for (int k = 0; k < arr.Length; k++)
                            {
                                if (string.IsNullOrEmpty(arr[k])) continue;
                                sRawid += "," + arr[k];
                            }

                            sOCAPRAWID += sRawid;
                            string[] sArrTemp = sRawid.Split(',');
                            for (int l = 0; l < sArrTemp.Length; l++)
                            {
                                if (sArrTemp[l].Length > 0 && sArrTemp[l] == this.mChartVariable.OCAPRawID)
                                {
                                    if (!arrIdxOCAP.Contains(j))
                                        arrIdxOCAP.Add(j);
                                }
                            }
                            sArrTemp = null;
                        }

                        switch (baseChart.Name)
                        {
                            case Definition.CHART_TYPE.P:
                                if (bPOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0 && sArrTemp[l] == this.mChartVariable.OCAPRawID)
                                            {
                                                if (!arrIdxOCAP.Contains(j))
                                                    arrIdxOCAP.Add(j);
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                
                                break;

                            case Definition.CHART_TYPE.PN:
                                if (bPNOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0 && sArrTemp[l] == this.mChartVariable.OCAPRawID)
                                            {
                                                if (!arrIdxOCAP.Contains(j))
                                                    arrIdxOCAP.Add(j);
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.C:
                                if (bCOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0 && sArrTemp[l] == this.mChartVariable.OCAPRawID)
                                            {
                                                if (!arrIdxOCAP.Contains(j))
                                                    arrIdxOCAP.Add(j);
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.U:
                                if (bUOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Split('^');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k])) continue;
                                            sRawid += "," + arr[k];
                                        }

                                        sOCAPRAWID += sRawid;
                                        string[] sArrTemp = sRawid.Split(',');
                                        for (int l = 0; l < sArrTemp.Length; l++)
                                        {
                                            if (sArrTemp[l].Length > 0 && sArrTemp[l] == this.mChartVariable.OCAPRawID)
                                            {
                                                if (!arrIdxOCAP.Contains(j))
                                                    arrIdxOCAP.Add(j);
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;
                        }
                    }

                    if (arrIdxOCAP.Count > 0)
                    {
                        List<int> lsAnnotationIdxTemp = new List<int>();
                        List<List<int>> lsAnnotationIdx = new List<List<int>>();
                        for (int m = 0; m < arrIdxOCAP.Count; m++)
                        {
                            int idxTemp = Convert.ToInt32(arrIdxOCAP[m].ToString());
                            lsAnnotationIdxTemp.Add(idxTemp);
                        }
                        SeriesInfo si = baseChart.SPCChart.GetSeriesInfo(sOCAPSeries);
                        List<SeriesInfo> lsSeries = new List<SeriesInfo>();
                        lsSeries.Add(si);

                        lsAnnotationIdx.Add(lsAnnotationIdxTemp);

                        baseChart.SPCChart.PointCrossLine.PointHighLightOn(lsSeries, lsAnnotationIdx);
                        baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].SeriesColor = Color.Yellow;
                        ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).LinePen.Visible = false;
                        ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                        ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;
                        ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.InflateMargins = true;
                    }
                }
            }
            catch
            {
                base.MsgClose();
            }
            finally
            {
                base.MsgClose();
            }
        }

        private void SaveToggleInformation()
        {
            if (!IsToggleInformationChanged())
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_SAVE_DATA", null, null);
                return;
            }

            List<string> rawIDs = new List<string>();
            List<string> toggles = new List<string>();
            List<string> modelConfigRawids = new List<string>();
            List<string> spcDataDttses = new List<string>();
            List<string> toggleYNs = new List<string>();

            for (int i = 0; i < this._dataManager.RawDataTable.Rows.Count; i++)
            {
                DataRow dr = this._dataManager.RawDataTable.Rows[i];

                switch (dr[Definition.CHART_COLUMN.TABLENAME].ToString())
                {
                    case Definition.TableName.USERNAME_DATA_SHORT:
                        AddToggleInformation(i, rawIDs, toggles);
                        break;
                    //case Definition.TableName.USERNAME_TEMPDATA_SHORT:
                    //    AddToggleYNInformation(i, modelConfigRawids, spcDataDttses, toggleYNs);
                    //    break;
                }
            }

            MsgShow(COMMON_MSG.Save_Data);
            if (_wsSPC.SaveToggleInformation(rawIDs.ToArray(), toggles.ToArray(), modelConfigRawids.ToArray(), spcDataDttses.ToArray(), toggleYNs.ToArray()))
            {
                MsgClose();
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                this.includingToggleData = false;
                PROC_DataBinding();
            }
            else
            {
                MsgClose();
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
            }
        }

        private void AddToggleInformation(int rowIndex, List<string> rawIDs, List<string> toggles)
        {
            DataTable raw = this._dataManager.RawDataTable;
            DataTable original = this._dataManager.RawDataTableOriginal;

            if (rawIDs.Contains(raw.Rows[rowIndex][Definition.COL_RAW_ID].ToString()))
                return;

            if (string.IsNullOrEmpty(raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString()) ||
                raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString() == original.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString())
            {
                return;
            }

            DataRow[] drs = raw.Select(Definition.COL_RAW_ID + " = '" + raw.Rows[rowIndex][Definition.COL_RAW_ID] + "'", Definition.CHART_COLUMN.ORDERINFILEDATA);
            //int sampleCount = int.Parse(drs[0][Definition.COL_SAMPLE_COUNT].ToString());
            List<string> tempToggle = new List<string>();
            //for (int i = 0; i < sampleCount; i++)
            //{
            //    tempToggle.Add("Y");
            //}

            for (int i = 0; i < drs.Length; i++)
            {
                int order = int.Parse(drs[i][Definition.CHART_COLUMN.ORDERINFILEDATA].ToString());
                string toggle = string.Empty;
                if (string.IsNullOrEmpty(drs[i][Definition.COL_TOGGLE_YN].ToString()))
                {
                    toggle = "N";
                }
                else
                {
                    toggle = drs[i][Definition.COL_TOGGLE_YN].ToString();
                }

                tempToggle.Add(toggle);
            }

            rawIDs.Add(raw.Rows[rowIndex][Definition.COL_RAW_ID].ToString());
            toggles.Add(string.Join(";", tempToggle.ToArray()) + ";");
        }

        private void AddToggleYNInformation(int rowIndex, List<string> modelConfigRawIDs, List<string> spcDataDttses, List<string> toggleYNs)
        {
            DataTable raw = this._dataManager.RawDataTable;
            DataTable original = this._dataManager.RawDataTableOriginal;

            // check if the row changed
            if (string.IsNullOrEmpty(raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString()) ||
                raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString() == original.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString())
            {
                return;
            }

            modelConfigRawIDs.Add(raw.Rows[rowIndex][Definition.COL_MODEL_CONFIG_RAWID].ToString());
            spcDataDttses.Add(raw.Rows[rowIndex][Definition.CHART_COLUMN.TIME].ToString());
            toggleYNs.Add(raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString());
        }

        private void HighlightOnPoint(BaseChart baseChart, List<SeriesInfo> siList, List<List<int>> indexLists, Color color)
        {
            baseChart.SPCChart.PointCrossLine.PointHighLightOn(siList, indexLists);
            for (int i = 0; i < baseChart.SPCChart.PointCrossLine.PointHighLightSeries.Count; i++)
            {
                baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].SeriesColor = color;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).LinePen.Visible = false;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.HorizSize = 4;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.VertSize = 4;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.InflateMargins = true;
            }
        }

        private void HighlightOffPoint(BaseChart baseChart)
        {
            baseChart.SPCChart.PointCrossLine.PointHighLightOff();
        }

        private void ChangeToggleVisible()
        {
            if (IsToggleInformationChanged())
            {
                if (DialogResult.Yes != MSGHandler.DialogQuestionResult("SPC_INFO_NOT_SAVE_TOGGLE_INFO",
                                                                        new string[] { "" }, MessageBoxButtons.YesNo))
                    return;
            }

            this.includingToggleData = true;
            this.PROC_DataBinding();
        }

        private bool IsToggleInformationChanged()
        {
            for (int i = 0; i < this._dataManager.RawDataTable.Rows.Count; i++)
            {
                if (this._dataManager.RawDataTable.Rows[i][Definition.COL_TOGGLE_YN]
                    != this._dataManager.RawDataTableOriginal.Rows[i][Definition.COL_TOGGLE_YN])
                    return true;
            }

            return false;
        }

        private bool IsToggleFlagNeeded(BaseChart baseChart, Series s)
        {
            string seriesName = s.Title;

            if (seriesName == Definition.CHART_COLUMN.USL ||
                seriesName == Definition.CHART_COLUMN.LSL ||
                seriesName == Definition.CHART_COLUMN.UCL ||
                seriesName == Definition.CHART_COLUMN.LCL ||
                seriesName == Definition.CHART_COLUMN.TARGET)
                return false;

            return true;
        }

        private void Filter()
        {
            if (filteringPopup == null)
                InitializeFilteringPopup();

            if (filteringPopup.ContextNames.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CONTEXT_GROUP_FILTER", null, null);
                return;
            }

            if (DialogResult.OK != filteringPopup.ShowDialog())
                return;

            var groupingAndFilteringValue = filteringPopup.GetGroupingAndFilteringValues;
            if (groupingAndFilteringValue.Count == 0)
            {
                isFiltered = false;
                filteredRawData = null;
                panel1.Enabled = true;
            }
            else
            {
                isFiltered = true;
                //FilterRawTable(groupingAndFilteringValue);
                panel1.Enabled = false;
            }

            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                BaseChart baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.Filter(groupingAndFilteringValue);
            }

            this._dataManager.Filter(groupingAndFilteringValue);
        }

        private void FilterRawTable(Dictionary<string, string[]> groupingAndFilteringCondition)
        {
            if(this.dtRawData != null)
                this.filteredRawData = _grouperAndFilter.GroupingAndFiltering(this.dtRawData, groupingAndFilteringCondition);
        }

        private void InitializeFilteringPopup()
        {
            List<string> contexts = new List<string>();
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                contexts.Add(Definition.CHART_COLUMN.EQP_ID);
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.RECIPE_ID))
                contexts.Add(Definition.CHART_COLUMN.RECIPE_ID);
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                contexts.Add(Definition.CHART_COLUMN.PRODUCT_ID);
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                contexts.Add(Definition.CHART_COLUMN.OPERATION_ID);
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MODULE_ID))
                contexts.Add(Definition.CHART_COLUMN.MODULE_ID);
            if (this._dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.STEP_ID))
                contexts.Add(Definition.CHART_COLUMN.STEP_ID);

            filteringPopup = new SeriesGroupingAndFilteringPopup(this._dataManager.RawDataTable, contexts.ToArray());
        }

        #endregion


        public override bool GetDirtyStatus()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtOCAPData);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this.dtRawData);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this.mDTContext);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this.mDTCustomContext);

            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._ds);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._dsOcapComment);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this.mDSChart);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this.mDSOCAPValue);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this.filteredRawData);

            if (this.linkTraceDataViewEvent != null)
                this.linkTraceDataViewEvent = null;

            if(mChartVariable!=null)
                this.mChartVariable.ReleaseChartData();

            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                BaseChart tempChart = (BaseChart)this.pnlChart.Controls[i];
                if (tempChart != null)
                {
                    tempChart.ReleaseData();
                    tempChart.ReleaseDataManager();
                }
            }

            _dataManager.ReleaseData();

            return false;
        }


    }
}
