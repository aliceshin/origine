using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using BISTel.eSPC.Page.Common.Popup;
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

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Report
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
        CHART_PARENT_MODE mChartMode = CHART_PARENT_MODE.SPC_CONTROL_CHART;


        DataTableGroupBy mDTGroupBy = new DataTableGroupBy();
        DataSet mDSChart = null;
        DataSet mDSOCAPValue = null;
        DataSet _dsOcapComment = null;

        Initialization mInitialization;

        MultiLanguageHandler mMHandler;

        LinkedList mllstSearchCondition = new LinkedList();
        LinkedList mllstChartSearchData = new LinkedList();
        LinkedList mllstChart = new LinkedList();
        LinkedList mllstChartSeriesVisibleType = null;
        LinkedList llstTmpOcapComment = null;

        public LinkedList LlstTmpOcapComment
        {
            get { return llstTmpOcapComment; }
            set { llstTmpOcapComment = value; }
        }

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
        //added by enkim 2012.10.05 SPC-910
        bool _isNormalChart = false;

        DataTable mDTContext;
        DataTable mDTCustomContext;
        DataSet _ds = null;
        DataTable dtRawData = null;
        bool isRawClick = false;
        DataTable _dtOCAPData = null;
        //bool _isURLOPen = false;

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

        bool isMETChartPage = false;
        bool isMETChartPopUp = false;
        private string _sGroupName;
        private bool _bUseComma;

        public bool ISMETCHARTPOPUP
        {
            get { return this.isMETChartPopUp; }
            set { this.isMETChartPopUp = value; }
        }

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

        public CHART_PARENT_MODE CHART_MODE
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

        //added by enkim 2012.10.05 SPC-910
        public bool isNormalChart
        {
            get { return this._isNormalChart; }
            set { this._isNormalChart = value; }
        }
        //added end SPC-910

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

        #endregion


        public SPCChartUC()
        {
            this.InitializeComponent();
        }

        //void SPCChartUC_CancelEvent(object sender)
        //{
        //    BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CloseProgress(this);
        //    _wsSPC.CancelAsync(null);
        //}

        public void InitializeCommon()
        {
            this.mComUtil = new CommonUtility();
            this.mBSpreadUtil = new BSpreadUtility();
            this.mMHandler = MultiLanguageHandler.getInstance();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            //_wsSPC.GetSPCControlChartDataCompleted += new BISTel.eSPC.Page.eSPCWebService.GetSPCControlChartDataCompletedEventHandler(_wsSPC_GetSPCControlChartDataCompleted);
            //BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.GetInstance().CancelEvent += new BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CancelDegate(SPCChartUC_CancelEvent);
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

                    if (CHART_MODE == CHART_PARENT_MODE.SPC_CONTROL_CHART)
                    {
                        if ((this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT && ChartVariable.MAIN_YN != "Y"))
                            this.bbtnListChart.Items[i].Visible = false;                       
                        //else
                        //    this.bbtnListChart.Items[i].Visible = true;

                    }
                    else
                    {

                        //if ((this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT) ||
                        //   (this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART) ||
                        //   (this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CONFIGURATION))
                        //    this.bbtnListChart.Items[i].Visible = false;

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

                //modified by enkim 2012.10.19 SPC-924
                if (!this.includingToggleData && this.mChartVariable != null && this.mChartVariable.OCAPRawID.Length > 0)
                {
                    this.FindSelectedOCAP();
                }
                //modified end SPC-924
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

                //TraceDataViewLinkData linkData = new TraceDataViewLinkData(queryXml);

                XmlElement linkData = queryXml.DocumentElement;
                XmlNode requestInfo = linkData.GetElementsByTagName("request")[0];
                XmlNode xNodeModelConfigRawID = ((XmlElement)requestInfo).GetElementsByTagName("MODEL_CONFIG_RAWID")[0];
                if (xNodeModelConfigRawID != null)
                {
                    XmlNode xNodeOCAPRawID = ((XmlElement)requestInfo).GetElementsByTagName("OCAP_RAWID")[0];
                    XmlNode xNodeOCAPDtts = ((XmlElement)requestInfo).GetElementsByTagName("OCAP_DTTS")[0];
                    string sModelConfigRawID = xNodeModelConfigRawID.InnerText;
                    string sOCAPRawID = xNodeOCAPRawID.InnerText;
                    string sOCAPDtts = xNodeOCAPDtts.InnerText;
                    //MSGHandler.DisplayMessage(MSGType.Information, sModelConfigRawID + "^" + sOCAPRawID + "^" + sOCAPDtts);

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

                    //this._isURLOPen = true;

                    this.PageSearch(lnkListConditionFromURL);

                    this._sOCAPRawIDURL = string.Empty;
                }
                else
                {
                    base.MsgClose();

                    XmlNode xNodeEQPID = ((XmlElement)requestInfo).GetElementsByTagName("EQP_ID")[0];
                    XmlNode xNodeSumParamAlias = ((XmlElement)requestInfo).GetElementsByTagName("SUM_PARAM_ALIAS")[0];
                    XmlNode xNodeEvSumParamAlias = ((XmlElement)requestInfo).GetElementsByTagName("EVENT_PARAM_ALIAS")[0];
                    XmlNode xNodeStartDate = ((XmlElement)requestInfo).GetElementsByTagName("START_DATE")[0];
                    XmlNode xNodeEndDate = ((XmlElement)requestInfo).GetElementsByTagName("END_DATE")[0];

                    string strNodeEQPID = xNodeEQPID.InnerText;
                    string strNodeSumParamAlias = "";
                    if (xNodeSumParamAlias != null)
                    {
                        strNodeSumParamAlias = xNodeSumParamAlias.InnerText;
                    }
                    string strNodeEvSumParamAlias = "";
                    if (xNodeEvSumParamAlias != null)
                    {
                        strNodeEvSumParamAlias = xNodeEvSumParamAlias.InnerText;
                    }
                    string strNodeStartDate = xNodeStartDate.InnerText;
                    string strNodeEndDate = xNodeEndDate.InnerText;

                    string strParamAliasT = "";

                    if (!string.IsNullOrEmpty(strNodeSumParamAlias))
                    {
                        strParamAliasT = strNodeSumParamAlias;
                    }
                    else
                    {
                        strParamAliasT = strNodeEvSumParamAlias;
                    }

                    LinkedList llstCondtion = new LinkedList();
                    llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
                    llstCondtion.Add(Definition.CONDITION_KEY_USE_YN, "Y");

                    DataSet dsModelLevel = this._wsSPC.GetCodeData(llstCondtion.GetSerialData());

                    string sSPCModelLevel = "";

                    if (DSUtil.CheckRowCount(dsModelLevel))
                    {
                        sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][COLUMN.CODE].ToString();
                    }

                    llstCondtion.Clear();

                    llstCondtion.Add(Definition.CONDITION_KEY_EQP_ID, strNodeEQPID);
                    DataSet dsBaseInfo = this._wsSPC.GetBaseInfoWithEQPID(llstCondtion.GetSerialData());

                    llstCondtion.Clear();

                    string sAreaRawIDT = string.Empty;
                    string sAreaT = string.Empty;
                    string sEQPModelT = string.Empty;

                    if (DSUtil.CheckRowCount(dsBaseInfo))
                    {
                        sAreaRawIDT = dsBaseInfo.Tables[0].Rows[0][COLUMN.AREA_RAWID].ToString();
                        sAreaT = dsBaseInfo.Tables[0].Rows[0][COLUMN.AREA].ToString();
                        sEQPModelT = dsBaseInfo.Tables[0].Rows[0][COLUMN.EQP_MODEL].ToString();
                    }

                    llstCondtion.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawIDT);

                    if (sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        llstCondtion.Add(Definition.CONDITION_KEY_EQP_MODEL, sEQPModelT);
                    }

                    llstCondtion.Add(Definition.CONDITION_KEY_PARAM_ALIAS, strParamAliasT);

                    DataSet dsSPCModelInfo = this._wsSPC.GetSPCModelListbyBaseInfo(llstCondtion.GetSerialData());

                    if (DSUtil.CheckRowCount(dsSPCModelInfo))
                    {
                        SelectSPCModelListPopup ssMls = new SelectSPCModelListPopup();

                        ssMls.isShowSubList = true;
                        ssMls.sSPCModelLevel = sSPCModelLevel;
                        ssMls.sAreaT = sAreaT;
                        ssMls.sEQPModelT = sEQPModelT;
                        ssMls.sAreaRawIDT = sAreaRawIDT;
                        ssMls.dsSPCModelList = dsSPCModelInfo;
                        ssMls.strParamAliasT = strParamAliasT;

                        ssMls.InitializeControl();

                        if (ssMls.ShowDialog() == DialogResult.OK)
                        {
                            string sModelConfigRawID = ssMls.CONFIG_RAWID;

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

                            DataTable dtStartValue = DCUtil.MakeDataTableForDCValue(strNodeStartDate, strNodeStartDate);
                            DataTable dtEndValue = DCUtil.MakeDataTableForDCValue(strNodeEndDate, strNodeEndDate);
                            lnkListConditionFromURL.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, dtStartValue);
                            lnkListConditionFromURL.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, dtEndValue);

                            this.PageSearch(lnkListConditionFromURL);

                            this._sOCAPRawIDURL = string.Empty;
                        }
                    }
                    else
                    {
                        //show create spc model popup
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_SPC_MODEL", null, null);
                        return;
                    }
                }
                
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
            Hashtable htButtonList = null;
            if (this.KeyOfPage == "BISTel.eSPC.Page.Report.SPCChartUC")
            {
                htButtonList = this.mInitialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            }
            else if (this.KeyOfPage == "BISTel.eSPC.Page.Report.MET.SPCChartUC")
            {
                htButtonList = this.mInitialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_MET_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            }
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
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetCodeData(lk.GetSerialData());
        }

        public void InitializeChart()
        {
            try
            {
                //2012-03-23 added by rachel -->
                //[SPC-659]
                //if문 수정 (if 문 위치 조정)

                if (this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                {
                    //2012-03-23 added by rachel -->  led가 바꾼 자리
                    //[SPC-659]
                    //for (int i = 0; i < this.pnlChart.Controls.Count; i++)
                    //{
                    //    BaseChart tempChart = (BaseChart)this.pnlChart.Controls[i];
                    //    if (tempChart != null)
                    //    {
                    //        tempChart.ReleaseData();
                    //        tempChart.ReleaseDataManager();
                    //    }
                    //}
                    ////<--

                    ArrayList arrEQPID = new ArrayList();
                    ArrayList arrModuleID = new ArrayList();
                    LinkedList llstEQPModule = new LinkedList();
                    //DataSet dsGroupItem = new DataSet();

                    //if (this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                    //{
                    _dataManager.RawDataTableOriginal = this.ChartVariable.dtParamData.Copy();
                    _dataManager.RawDataTable = this.ChartVariable.dtParamData.Copy();
                    //}

                    int iChart = mllstChart.Count;
                    int iHeight = 300;
                    if (iChart > 1)
                    {
                        iHeight = this.pnlChart.Height / 2;
                        this.pnlChart.Height = iChart * iHeight;
                    }

                    if (this.ChartVariable.dtParamData != null
                        && this.ChartVariable.dtParamData.Rows.Count > 0
                        && this.ChartVariable.dtParamData.Columns.Contains(COLUMN.MODEL_CONFIG_RAWID))
                    {
                        string _sTempModelConfigRawid = this.ChartVariable.dtParamData.Rows[0][COLUMN.MODEL_CONFIG_RAWID].ToString();
                        //LinkedList _lnkTemp = new LinkedList();
                        //_lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, _sTempModelConfigRawid);
                        //dsGroupItem = this._wsSPC.GetGroupContextValue(_lnkTemp.GetSerialData());

                        for (int i = 0; i < this.ChartVariable.dtParamData.Rows.Count; i++)
                        {
                            string strEQPIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString();
                            //string strModuleIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString();
                            string strModuleIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString();
                            string strEQPIDKey = "";
                            ArrayList arrModuleIDValue = new ArrayList();

                            if (strEQPIDTemp != null && strEQPIDTemp.Length > 0)
                            {
                                for (int j = 0; j < strEQPIDTemp.Split(';').Length; j++)
                                {
                                    if (strEQPIDTemp.Split(';')[j] != null && strEQPIDTemp.Split(';')[j].Length > 0 && !strEQPIDTemp.Split(';')[j].ToUpper().Equals("NULL"))
                                    {
                                        if (!arrEQPID.Contains(strEQPIDTemp.Split(';')[j]))
                                        {
                                            arrEQPID.Add(strEQPIDTemp.Split(';')[j]);
                                        }
                                        strEQPIDKey = strEQPIDTemp.Split(';')[j];
                                    }
                                }

                            }

                            if (strModuleIDTemp != null && strModuleIDTemp.Length > 0)
                            {
                                for (int j = 0; j < strModuleIDTemp.Split(';').Length; j++)
                                {
                                    if (strModuleIDTemp.Split(';')[j] != null && strModuleIDTemp.Split(';')[j].Length > 0 && !strModuleIDTemp.Split(';')[j].ToUpper().Equals("NULL"))
                                    {
                                        if (!arrModuleID.Contains(strModuleIDTemp.Split(';')[j]))
                                        {
                                            arrModuleID.Add(strModuleIDTemp.Split(';')[j]);
                                        }
                                        if (!arrModuleIDValue.Contains(strModuleIDTemp.Split(';')[j]))
                                        {
                                            arrModuleIDValue.Add(strModuleIDTemp.Split(';')[j]);
                                        }
                                    }
                                }

                            }

                            if (strEQPIDKey.Length > 0 && arrModuleIDValue.Count > 0)
                            {
                                if (llstEQPModule.Contains(strEQPIDKey))
                                {
                                    ArrayList arrTempModuleValue = (ArrayList)llstEQPModule[strEQPIDKey];
                                    for (int idxModuleValue = 0; idxModuleValue < arrModuleIDValue.Count; idxModuleValue++)
                                    {
                                        if (!arrTempModuleValue.Contains(arrModuleIDValue[idxModuleValue].ToString()))
                                        {
                                            arrTempModuleValue.Add(arrModuleIDValue[idxModuleValue].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    llstEQPModule.Add(strEQPIDKey, arrModuleIDValue);
                                }
                            }
                        }
                    }

                    for (int idxCtlCnt = this.pnlChart.Controls.Count - 1; idxCtlCnt >= 0; idxCtlCnt--)
                    {
                        if (this.pnlChart.Controls[idxCtlCnt].GetType() == typeof(DefaultChart))
                        {
                            ((DefaultChart)this.pnlChart.Controls[idxCtlCnt]).Dispose();
                        }
                    }

                    this.pnlChart.Controls.Clear();

                    ////2012-03-23 added by rachel -->
                    ////[SPC-659]
                    //if (chartBase != null)
                    //    chartBase.ReleaseData();
                    ////<--

                    //chartBase = null;
                    Steema.TeeChart.Tools.CursorTool _cursorTool = null;

                    for (int i = mllstChart.Count - 1; i >= 0; i--)
                    {
                        string strKey = mllstChart.GetKey(i).ToString();
                        string strValue = mllstChart.GetValue(i).ToString();
                        DefaultChart chartBase = new DefaultChart(_dataManager);
                        chartBase = new DefaultChart(_dataManager);
                        chartBase.ClearChart();
                        chartBase.Title = strValue;
                        chartBase.Name = strKey;
                        chartBase.ParamTypeCD = this.mParamTypeCd;
                        //chartBase.BtnVisible = false;

                        //if (dsGroupItem != null && dsGroupItem.Tables != null && dsGroupItem.Tables.Count > 0 && dsGroupItem.Tables[0].Rows.Count > 0)
                        //{
                        //    chartBase.GroupVisible = true;
                        //    chartBase.DSGROUP = dsGroupItem;
                        //}
                        //else
                        //{
                            chartBase.GroupVisible = false;
                        //}

                            if (this.KeyOfPage == "BISTel.eSPC.Page.Report.SPCChartUC")
                            {
                                chartBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                            }
                            else if (this.KeyOfPage == "BISTel.eSPC.Page.Report.MET.SPCChartUC")
                            {
                                chartBase.Pagekey = Definition.PAGE_KEY_SPC_MET_CONTROL_CHART_UC;
                            }
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
                        chartBase._llstEQPModule = llstEQPModule;
                        chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);
                        if (!chartBase.DrawSPCChart()) continue;
                        _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                        _cursorTool.Active = true;
                        _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                        _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        _cursorTool.Pen.Color = Color.Gray;
                        _cursorTool.FollowMouse = true;
                        _cursorTool.Series = chartBase.SPCChart.GetAllSeries()[0];
                        if (!(this.ParamTypeCD != "MET" && strKey == "RAW"))
                        {
                            _cursorTool.Change += new Steema.TeeChart.Tools.CursorChangeEventHandler(_cursorTool_Change);
                        }
                        chartBase.SPCChart.Chart.Tools.Add(_cursorTool);
                        //chartBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                        chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                        chartBase.SPCChart.ClickBackground += new MouseEventHandler(SPCChart_ClickBackground);
                        this.pnlChart.Controls.Add(chartBase);

                        if (this.mParamTypeCd != "MET" && strKey == "RAW")
                        {
                            //if (chartBase.IsWaferColumn)
                            //{
                            dtRawData = chartBase.dtDataSource.Copy();
                            if (dtRawData.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                            {
                                dtRawData.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                            }
                            if (dtRawData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
                            {
                                dtRawData.Columns.Remove(CommonChart.COLUMN_NAME_SEQ_INDEX);
                            }
                            //}
                        }

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                else
                {
                    for (int idxCtlCnt = this.pnlChart.Controls.Count - 1; idxCtlCnt >= 0; idxCtlCnt--)
                    {
                        if (this.pnlChart.Controls[idxCtlCnt].GetType() == typeof(DefaultChart))
                        {
                            ((DefaultChart)this.pnlChart.Controls[idxCtlCnt]).Dispose();
                        }
                    }

                    this.pnlChart.Controls.Clear();
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
            this.bTitlePanel1.Title = this.mMHandler.GetVariable(Definition.TITLE_KEY_SPC_CONTROL_CHART);
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        public void InitializeDataButton()
        {
            if (string.IsNullOrEmpty(this.KeyOfPage))
            {
                this.KeyOfPage = this.GetType().FullName;
            }

            if (this.KeyOfPage == "BISTel.eSPC.Page.Report.SPCChartUC")
            {
                this.isMETChartPage = false;
                this.mInitialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.BUTTONLIST_KEY_CHART, this.sessionData);
                this.FunctionName = Definition.FUNC_KEY_SPC_CONTROL_CHART;
            }
            else if (this.KeyOfPage == "BISTel.eSPC.Page.Report.MET.SPCChartUC")
            {
                this.isMETChartPage = true;
                this.mInitialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_MET_CONTROL_CHART_UC, Definition.BUTTONLIST_KEY_MET_CHART, this.sessionData);
                this.FunctionName = Definition.FUNC_KEY_SPC_MET_CONTROL_CHART;
            }

            

            if (this.ApplicationName == null || this.ApplicationName == "")
            {
                this.ApplicationName = "ESPC:ESPC_REPROT";
            }
            base.ApplyAuthory(this.bbtnListChart);
        }

        public void InitializeBSpread()
        {
            if (this.mChartInfomationUI != null)
            {
                this.mChartInfomationUI.ReleaseData();
                this.mChartInfomationUI.Dispose();
                for (int i = 0; i < this.bTitPanelInformation.Controls.Count; i++)
                {
                    if (this.bTitPanelInformation.Controls[i].GetType() == typeof(ChartInformation))
                    {
                        this.bTitPanelInformation.Controls.RemoveAt(i);
                    }
                }
            }

            this.mChartInfomationUI = new ChartInformation();
            this.mChartInfomationUI.SessionData = this.sessionData;
            this.mChartInfomationUI.ChartVariable = ChartVariable;
            this.mChartInfomationUI.dsChart = this.mDSChart.Copy();
            this.mChartInfomationUI.InitializePopup();
            this.mChartInfomationUI.Dock = DockStyle.Fill;
            //if(CHART_MODE==CHART_PARENT_MODE.SPC_CONTROL_CHART)
            //{         
            //    this.bplInformation.Visible =false;                             
            //    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "Information", mChartInfomationUI, false, new Rectangle(Screen.PrimaryScreen.WorkingArea.Width - 250, 100, 250, 500), EESConstants.InformationDock.Right, EESConstants.InformationDock.Left);

            //}else
            //{
            this.bplInformation.Visible = true;
            this.bTitPanelInformation.Controls.Add(this.mChartInfomationUI);
            //}

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

            //modified by enkim 2012.05.25 SPC-851
            this.mllstSearchCondition = llstCondition;
            //modified end SPC-851


            try
            {
                bool bChartIDExist = false;
                if (this.mChartVariable != null)
                {
                    this.mChartVariable.ReleaseChartData();
                }

                this.mChartVariable = new ChartInterface();
                BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtOCAPData);
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
                    dsTempChartID = this._wsSPC.GetSPCModelDatabyChartID(lnkTemp.GetSerialData());

                    if (dsTempChartID != null && dsTempChartID.Tables != null && dsTempChartID.Tables.Count == 4 && dsTempChartID.Tables[0].Rows.Count > 0 && dsTempChartID.Tables[1].Rows.Count > 0)
                    {
                        this.mSite = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.SITE].ToString();
                        this.mFab = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.FAB].ToString();
                        this.mLineRawID = dsTempChartID.Tables[0].Rows[0][COLUMN.LOCATION_RAWID].ToString();
                        this.mLine = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.LINE].ToString();
                        mChartVariable.LINE = this.mLine;
                        mChartVariable.SPC_MODEL = dsTempChartID.Tables[0].Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                        mChartVariable.SPC_MODEL_RAWID = dsTempChartID.Tables[0].Rows[0][COLUMN.MODEL_RAWID].ToString();
                        this.mParamTypeCd = dsTempChartID.Tables[1].Rows[0][COLUMN.PARAM_TYPE_CD].ToString();
                        mChartVariable.PARAM_ALIAS = dsTempChartID.Tables[1].Rows[0][COLUMN.PARAM_ALIAS].ToString();
                        mChartVariable.DEFAULT_CHART = dsTempChartID.Tables[2].Rows[0][COLUMN.DEFAULT_CHART_LIST].ToString();
                        mChartVariable.MAIN_YN = dsTempChartID.Tables[1].Rows[0][COLUMN.MAIN_YN].ToString();
                        mChartVariable.MODEL_CONFIG_RAWID = dsTempChartID.Tables[1].Rows[0][COLUMN.RAWID].ToString();
                        mChartVariable.RESTRICT_SAMPLE_DAYS = "";
                        mChartVariable.AREA = dsTempChartID.Tables[0].Rows[0][COLUMN.AREA].ToString();
                        mChartVariable.EQP_MODEL = dsTempChartID.Tables[0].Rows[0][COLUMN.EQP_MODEL].ToString();
                        this.mAreaRawID = dsTempChartID.Tables[0].Rows[0][COLUMN.AREA_RAWID].ToString();
                        this.mArea = mChartVariable.AREA;
                        this.mApplyRestric = false;
                    }

                    if (!base.ApplyAuthory(this.bbtnListChart, this.mSite, this.mFab, this.mLine, this.mArea))
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                        this.InitializePage();
                        return;
                    }

                    BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref dsTempChartID);
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
                        dsTempChartID = this._wsSPC.GetSPCModelDatabyChartID(lnkTemp.GetSerialData());

                        this.mParamTypeCd = dsTempChartID.Tables[1].Rows[0][COLUMN.PARAM_TYPE_CD].ToString();

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

                        BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref dsTempChartID);
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

                if (llstCondition.Contains(COLUMN.GROUP_NAME))
                {
                    dt = (DataTable)llstCondition[COLUMN.GROUP_NAME];
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
                    this.mLineRawID = ds.Tables[0].Rows[0][COLUMN.RAWID].ToString();
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


        //private void GetOCAPRawID()
        //{
        //    if (ChartVariable.dtParamData == null) return;
        //    if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
        //    {
        //        string _fieldList = Definition.CHART_COLUMN.OCAP_RAWID;
        //        string _groupby = Definition.CHART_COLUMN.OCAP_RAWID;
        //        string _rowFilter = "ocap_rawid <>''";

        //        this.mDTGroupBy = new DataTableGroupBy();
        //        DataTable dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);

        //        this.mllstChartSearchData.Clear();
        //        this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.OCAP_DATATABLE, dtOCAP);
        //        mDSOCAPValue = _wsSPC.GetOCAPDetails(this.mllstChartSearchData.GetSerialData());
        //    }
        //}

        private LinkedList GetDefaultChart()
        {
            LinkedList _llstDefaultChart = new LinkedList();
            _llstDefaultChart.Clear();

            if (DataUtil.IsNullOrEmptyDataSet(mDSChart)) return _llstDefaultChart;

            if (ChartVariable == null) return _llstDefaultChart;

            foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
            {
                string strkey = dr[COLUMN.CODE].ToString();
                if (ChartVariable.lstDefaultChart.Contains(strkey))
                {
                    _llstDefaultChart.Add(strkey, dr[COLUMN.DESCRIPTION].ToString());
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
            //sb.AppendFormat(" TIME >='{0}' AND TIME <='{1}'", this.mStartTime,this.mEndTime);         
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
                object objDataSet = ach.SendWait(_wsSPC, "GetSPCControlChartData", new object[] { mllstChartSearchData.GetSerialData() });
                EESProgressBar.CloseProgress(this);

                //_wsSPC.GetSPCControlChartDataAsync(mllstChartSearchData.GetSerialData());

                if (objDataSet != null)
                {
                    BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._ds);
                    this._ds = (DataSet)objDataSet;
                    PROC_DataBinding1();
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));

                    this.Focus();
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

                    this.Focus();
                }
                else
                {
                    if (this.mllstChartSearchData.Contains(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST))
                        this.mllstChartSearchData.Remove(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST);
                    this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST, getCustomContext());
                    if (this.ParamTypeCD == "MET")
                        mDTChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, mllstChartSearchData, false, false, includingToggleData);
                    else
                        mDTChartData = CommonPageUtil.CLOBnBLOBParsingRaw(_ds, mllstChartSearchData, false, includingToggleData);

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
                            //modified by enkim 2012.08.30 for ocap data
                            bool bRawOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST);
                            bool bMeanOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST);
                            bool bStdOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST);
                            bool bRangeOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST);
                            bool bMaOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST);
                            bool bMsOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST);
                            bool bMrOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST);
                            bool bEwmaMeanOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST);
                            bool bEwmaStdOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST);
                            bool bEwmaRangeOcap = mDTChartData.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST);

                            foreach (DataRow dr in mDTChartData.Rows)
                            {
                                string rawid = dr[Definition.CHART_COLUMN.OCAP_RAWID].ToString();

                                //2012-03-23 added by rachel -->
                                //[SPC-659]
                                string sTemp = rawid.Replace(";", "");
                                if (sTemp.Length > 0)
                                {
                                 //<--
                                    string[] ids = rawid.Split(';');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }

                                if (bRawOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString();

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

                                if (bMeanOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString();

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

                                if (bStdOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString();

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

                                if (bRangeOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString();

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

                                if (bMaOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.MA_OCAP_LIST].ToString();

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

                                if (bMsOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString();

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

                                if (bMrOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.MR_OCAP_LIST].ToString();

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

                                if (bEwmaMeanOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString();

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

                                if (bEwmaStdOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString();

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

                                if (bEwmaRangeOcap)
                                {
                                    rawid = dr[Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString();

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

                                //modified end
                            }

                            if (rawIDs.Count == 0)
                                rawIDs.Add("");

                            //LinkedList llstTmpOcapComment = new LinkedList();
                            llstTmpOcapComment = new LinkedList();
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

                            //_dsOcapComment = _wsSPC.GetOCAPCommentList(rawIDs.ToArray());
                            _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                        }

                        //added by enkim 2012.10.05 SPC-910
                        this._isNormalChart = _wsSPC.GetUseNormalValuebyChartID(this.mllstChartSearchData.GetSerialData());
                        //added end SPC-910

                        mChartVariable.lstDefaultChart.Clear();
                        //mChartVariable.complex_yn = mDTChartData.Rows[0][COLUMN.COMPLEX_YN].ToString();                                                            
                        mChartVariable.complex_yn = Definition.VARIABLE_Y;
                        mChartVariable.OPERATION_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        mChartVariable.PRODUCT_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                        mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(mChartVariable.DEFAULT_CHART);
                        //mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(Definition.INITIAL_DISPLAY_CHART);
                        //mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(this.mMHandler.GetVariable(Definition.VARIABLE_SPC_INITIAL_DISPLAY_CHART));

                        mChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        mChartVariable.dateTimeStart = DateTime.Parse(this.mStartTime);
                        mChartVariable.dateTimeEnd = DateTime.Parse(this.mEndTime);

                        //_createChartDT.COMPLEX_YN = mDTChartData.Rows[0][COLUMN.COMPLEX_YN].ToString();
                        _createChartDT.COMPLEX_YN = Definition.VARIABLE_Y;
                        mChartVariable.dtParamData = _createChartDT.GetMakeDataTable(mDTChartData);
                        //mChartVariable.dtParamData = mDTChartData;
                        mChartVariable.lstRawColumn = _createChartDT.lstRawColumn;

                        if (_dsOcapComment != null)
                            mChartVariable.dtOCAP = _dsOcapComment.Tables[0];

                        if (mDTChartData != null) mDTChartData.Dispose();
                        if (_createChartDT != null) _createChartDT = null;
                    }
                }
                //Console.WriteLine("[PARSING END]: " + DateTime.Now.ToString());
                this.InitializePage();
                //Console.WriteLine("[CHARTING END]: " + DateTime.Now.ToString());
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

        //void _wsSPC_GetSPCControlChartDataCompleted(object sender, BISTel.eSPC.Page.eSPCWebService.GetSPCControlChartDataCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Cancelled)
        //        {
        //            MSGHandler.DisplayMessage(MSGType.Information, "Operation has been cancelled.");
        //        }
        //        else
        //        {
        //            this._ds = e.Result;
        //            PROC_DataBinding1();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CloseProgress(this);
        //        MSGHandler.DisplayMessage(MSGType.Information, ex.Message);
        //    }
        //}

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

        #region OLD-LOGIC Create Tool Tip

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strCol"></param>
        ///// <param name="strValue"></param>
        ///// <param name="rowIndex"></param>
        ///// <returns></returns>
        //private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    LinkedList llstChartSeries = new LinkedList();

        //    string _sEQPID = "-";
        //    string _sLOTID = "-";
        //    string _sOperationID = "-";
        //    string _sSubstrateID = "-";
        //    string _sProductID = "-";

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
        //        _sEQPID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
        //        _sLOTID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
        //        _sOperationID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
        //        _sSubstrateID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
        //        _sProductID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();


        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
        //    //sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);

        //    if (mllstChart.Contains(strChartName))
        //    {
        //        llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
        //        for (int i = 0; i < llstChartSeries.Count; i++)
        //        {
        //            string sKey = llstChartSeries.GetKey(i).ToString();
        //            string sValue = llstChartSeries.GetValue(i).ToString();

        //            if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.LSL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.UCL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.LCL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;

        //            if (_dataManager.RawDataTable.Columns.Contains(sKey))
        //            {
        //                if (_dataManager.RawDataTable.Rows[rowIndex][sKey] != null && _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString().Length > 0)
        //                {
        //                    if (strCol.Contains("wafer"))
        //                    {
        //                        if (strChartName == Definition.CHART_TYPE.RAW)
        //                        {
        //                            if (sKey.Equals(Definition.CHART_TYPE.RAW))
        //                            {
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                if (sKey.Contains("wafer"))
        //                                {
        //                                    if (sKey.Contains(strCol + "^"))
        //                                    {
        //                                        sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (strChartName == Definition.CHART_TYPE.XBAR)
        //                        {
        //                            if (sKey.Contains("wafer"))
        //                            {
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        llstChartSeries = null;
        //    }

        //    if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
        //    {
        //        string strRawid = GetOCAPRawID(_dataManager.RawDataTable, rowIndex);
        //        DataTable dtOCAP = null;
        //        mDTGroupBy = new DataTableGroupBy();
        //        if (!string.IsNullOrEmpty(strRawid))
        //        {
        //            string _fieldList = "ocap_rawid,RAW_RULE";
        //            string _groupby = "ocap_rawid,RAW_RULE";
        //            string _sRuleNo = string.Empty;
        //            if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
        //            if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
        //            {
        //                string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
        //                dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
        //                foreach (DataRow dr in dtOCAP.Rows)
        //                {
        //                    if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
        //                    _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
        //                }
        //                sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
        //            }
        //        }
        //        mDTGroupBy = null;
        //        if (dtOCAP != null) dtOCAP.Dispose();
        //    }

        //    return sb.ToString();
        //}

        //private string CreateToolTipStringRaw(string strCol, string strValue, int rowIndex, string strChartName)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    LinkedList llstChartSeries = new LinkedList();

        //    string _sEQPID = "-";
        //    string _sLOTID = "-";
        //    string _sOperationID = "-";
        //    string _sSubstrateID = "-";
        //    string _sProductID = "-";

        //    if (this.dtRawData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
        //        _sEQPID = this.dtRawData.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

        //    if (this.dtRawData.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
        //        _sLOTID = this.dtRawData.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

        //    if (this.dtRawData.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
        //        _sOperationID = this.dtRawData.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

        //    if (this.dtRawData.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
        //        _sSubstrateID = this.dtRawData.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

        //    if (this.dtRawData.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
        //        _sProductID = this.dtRawData.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();


        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
        //    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);

        //    if (mllstChart.Contains(strChartName))
        //    {
        //        llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
        //        if (!(llstChartSeries.Contains(Definition.BLOB_FIELD_NAME.PARAM_LIST) || llstChartSeries.Contains(Definition.BLOB_FIELD_NAME.PARAM_LIST.ToUpper())))
        //        {
        //            llstChartSeries.Add(Definition.BLOB_FIELD_NAME.PARAM_LIST, Definition.BLOB_FIELD_NAME.PARAM_LIST);
        //        }
        //        for (int i = llstChartSeries.Count - 1; i >= 0; i--)
        //        {
        //            if (llstChartSeries.GetKey(i).ToString().Contains("wafer"))
        //            {
        //                llstChartSeries.Remove(llstChartSeries.GetKey(i));
        //            }
        //        }
        //        for (int i = 0; i < llstChartSeries.Count; i++)
        //        {
        //            string sKey = llstChartSeries.GetKey(i).ToString();
        //            string sValue = llstChartSeries.GetValue(i).ToString();

        //            if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.LSL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.UCL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.LCL) ||
        //                sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;

        //            if (this.dtRawData.Columns.Contains(sKey))
        //            {
        //                if (this.dtRawData.Rows[rowIndex][sKey] != null && this.dtRawData.Rows[rowIndex][sKey].ToString().Length > 0)
        //                {
        //                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtRawData.Rows[rowIndex][sKey].ToString());
        //                }
        //            }
        //        }
        //        llstChartSeries = null;
        //    }

        //    if (dtRawData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
        //    {
        //        string strRawid = GetOCAPRawID(dtRawData, rowIndex);
        //        DataTable dtOCAP = null;
        //        mDTGroupBy = new DataTableGroupBy();
        //        if (!string.IsNullOrEmpty(strRawid))
        //        {
        //            string _fieldList = "ocap_rawid,RAW_RULE";
        //            string _groupby = "ocap_rawid,RAW_RULE";
        //            string _sRuleNo = string.Empty;
        //            if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
        //            if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
        //            {
        //                string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
        //                dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
        //                foreach (DataRow dr in dtOCAP.Rows)
        //                {
        //                    if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
        //                    _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
        //                }
        //                sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
        //            }
        //        }
        //        mDTGroupBy = null;
        //        if (dtOCAP != null) dtOCAP.Dispose();
        //    }

        //    return sb.ToString();
        //}

        #endregion

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
            //sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);
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
                                                sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                            }
                                        }
                                        else
                                        {
                                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    if (sKey.Contains("wafer"))
                                    {
                                        if (sKey.Contains(strCol + "^"))
                                        {
                                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                        }
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                    }
                                }
                            }
                            else
                            {
                                if (strChartName != Definition.CHART_TYPE.XBAR)
                                {
                                    if (sKey.Contains("wafer"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                    }
                                }
                                else
                                {
                                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), dtToolTipData.Rows[rowIndex][sKey].ToString());
                                }
                            }
                        }
                    }
                }
                llstChartSeries = null;
            }

            if (!String.IsNullOrEmpty(this.mChartInfomationUI.StrComment))
            {
                string strToolTipComment = this.mChartInfomationUI.StrComment;
                if (strToolTipComment.Length > 100)
                {
                    strToolTipComment = strToolTipComment.Substring(0, 100);
                }

                sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SpreadHeaderColKey.OOC_COMMENT), strToolTipComment);
            }

            //if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            //{
            //    string strRawid = GetOCAPRawID(dtToolTipData, rowIndex);
            //    DataTable dtOCAP = null;
            //    mDTGroupBy = new DataTableGroupBy();
            //    if (!string.IsNullOrEmpty(strRawid))
            //    {
            //        string _fieldList = "ocap_rawid,RAW_RULE";
            //        string _groupby = "ocap_rawid,RAW_RULE";
            //        string _sRuleNo = string.Empty;
            //        if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
            //        if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            //        {
            //            string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
            //            dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
            //            foreach (DataRow dr in dtOCAP.Rows)
            //            {
            //                if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
            //                _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
            //            }
            //            sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
            //        }
            //    }
            //    mDTGroupBy = null;
            //    if (dtOCAP != null) dtOCAP.Dispose();
            //}

            return sb.ToString();
        }

        private string CreateToolTipStringRaw(string strCol, string strValue, int rowIndex, string strChartName, DataTable dtToolTipData)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            string _sEQPID = "-";
            string _sModuleAlias = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";
            string _sProductID = "-";
            string _sRecipeID = "-";

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                _sEQPID = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

            if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.MODULE_ALIAS))
                _sModuleAlias = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.MODULE_ALIAS].ToString();

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
            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.MODULE_ALIAS), _sModuleAlias);
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

            //added by enkim 2012.08.24 for normal value
            //modified by enkim 2012.10.05 SPC-910
            if (this._isNormalChart)
            {
                string sNormalType = "";
                string sNormalOption = "";
                string sNormalOptionValue = "";
                string sOriginalValue = "";

                if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.NORMAL_TYPE_LIST))
                    sNormalType = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.NORMAL_TYPE_LIST].ToString();

                if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.NORMAL_OPTION_LIST))
                    sNormalOption = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.NORMAL_OPTION_LIST].ToString();

                if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.NORMAL_OPTION_VALUE_LIST))
                    sNormalOptionValue = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.NORMAL_OPTION_VALUE_LIST].ToString();

                if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.ORIGINAL_PARAM_VALUE_LIST))
                    sOriginalValue = dtToolTipData.Rows[rowIndex][Definition.CHART_COLUMN.ORIGINAL_PARAM_VALUE_LIST].ToString();

                if (!string.IsNullOrEmpty(sNormalType) && !sNormalType.Equals("NULL"))
                {
                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.NORMAL_TYPE_LIST), sNormalType);
                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.NORMAL_OPTION_LIST), sNormalOption);
                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.NORMAL_OPTION_VALUE_LIST), sNormalOptionValue);
                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.ORIGINAL_PARAM_VALUE_LIST), sOriginalValue);
                }
            }

            if (!String.IsNullOrEmpty(this.mChartInfomationUI.StrComment))
            {
                string strToolTipComment = this.mChartInfomationUI.StrComment;
                if (strToolTipComment.Length > 100)
                {
                    strToolTipComment = strToolTipComment.Substring(0, 100);
                }

                sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(Definition.SpreadHeaderColKey.OOC_COMMENT), strToolTipComment);
            }
            //modified end SPC-910

            //added end

            //if (dtToolTipData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            //{
            //    string strRawid = GetOCAPRawID(dtToolTipData, rowIndex);
            //    DataTable dtOCAP = null;
            //    mDTGroupBy = new DataTableGroupBy();
            //    if (!string.IsNullOrEmpty(strRawid))
            //    {
            //        string _fieldList = "ocap_rawid,RAW_RULE";
            //        string _groupby = "ocap_rawid,RAW_RULE";
            //        string _sRuleNo = string.Empty;
            //        if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
            //        if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            //        {
            //            string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
            //            dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
            //            foreach (DataRow dr in dtOCAP.Rows)
            //            {
            //                if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
            //                _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
            //            }
            //            sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
            //        }
            //    }
            //    mDTGroupBy = null;
            //    if (dtOCAP != null) dtOCAP.Dispose();
            //}

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
                        case Definition.CHART_TYPE.RAW:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Replace("^",";");
                            break;
                        case Definition.CHART_TYPE.XBAR:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.RANGE:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.STDDEV:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.MA:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.MSD:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.MR:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.EWMA_MEAN:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.EWMA_RANGE:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Replace("^", ";");
                            break;
                        case Definition.CHART_TYPE.EWMA_STDDEV:
                            strRawid = dt.Rows[0][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Replace("^",";");
                            break;

                    }
                }
            }

            return strRawid;
        }




        //private void Chart_MouseLeave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BaseChart baseChart = FindSPCChart((Control)sender);
        //        Annotation ann;
        //        if (baseChart is DefaultChart)
        //        {
        //            for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
        //            {
        //                if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Annotation))
        //                {
        //                    ann = baseChart.SPCChart.Chart.Tools[i] as Annotation;
        //                    if (ann.Active)
        //                    {
        //                        ann.Active = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EESUtil.DebugLog(ex);
        //    }
        //    finally
        //    {

        //    }
        //}

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
            spcConifgPopup.CONFIG_MODE = ConfigMode.MODIFY;
            spcConifgPopup.CONFIG_RAWID = ChartVariable.MODEL_CONFIG_RAWID;
            spcConifgPopup.MAIN_YN = ChartVariable.MAIN_YN;
            spcConifgPopup.LINE_RAWID = this.mLineRawID;
            spcConifgPopup.AREA_RAWID = this.mAreaRawID;
            if (ChartVariable.MAIN_YN.ToUpper() == "Y")
            {
                if (0 < _wsSPC.GetTheNumberOfSubConfigOfModel(ChartVariable.MODEL_CONFIG_RAWID))
                    spcConifgPopup.HAS_SUBCONFIGS = true;
                else
                    spcConifgPopup.HAS_SUBCONFIGS = false;
            }
            if (isMETChartPage)
            {
                spcConifgPopup.MODELINGTYPE = "MET";
            }
            else
            {
                if (isMETChartPopUp)
                {
                    spcConifgPopup.MODELINGTYPE = "MET";
                }
                else
                {
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                }
            }

            spcConifgPopup.GROUP_NAME = _sGroupName;
            spcConifgPopup.InitializePopup();
            DialogResult result = spcConifgPopup.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                if (this.CHART_MODE == CHART_PARENT_MODE.SPC_CONTROL_CHART)
                {
                    if (llstSearchCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtModelMst = spcConifgPopup.SPCMODELDATA_DATASET.Tables[TABLE.MODEL_MST_SPC];

                        DataTable dtSPCModel = (DataTable)this.llstSearchCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        dtSPCModel.Rows[0][COLUMN.GROUP_NAME] = spcConifgPopup.GROUP_NAME;

                        DataTable dtSPCGroup = (DataTable)llstSearchCondition[Definition.CONDITION_KEY_GROUP_NAME];

                        dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = spcConifgPopup.GROUP_NAME;
                        dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = spcConifgPopup.GROUP_NAME;
                        dtSPCGroup.Rows[0]["CHECKED"] = "T";

                        this._sGroupName = spcConifgPopup.GROUP_NAME;

                        this.RefreshConditions(llstSearchCondition);
                    }
                }
                this.GROUP_NAME = spcConifgPopup.GROUP_NAME;
                spcConifgPopup.Dispose();
            }

        }

        private void ClickButtonShowChartData()
        {
            ChartDataPopup chartDataPop = null;
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

            if (this.mParamTypeCd != "MET")
            {
                if (this.dtRawData != null)
                {
                    chartDataPop.isVisibleRawData = true;
                    if (isFiltered)
                    {
                        DataSet filteredRaw = new DataSet();
                        foreach (DataTable table in this.filteredRawData.Tables)
                        {
                            DataTable dt = CommonPageUtil.ExcelExportRaw(table, ChartVariable.lstRawColumn);
                            dt.TableName = table.TableName;
                            filteredRaw.Tables.Add(dt);
                        }
                        chartDataPop.AddRawData(filteredRaw);
                    }
                    else
                    {
                        DataTable dtRaw = this.dtRawData.Copy();
                        chartDataPop.AddRawData(CommonPageUtil.ExcelExportRaw(dtRaw, ChartVariable.lstRawColumn));
                    }
                }
            }

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


        public void bTitlePanel1_Resize(object sender, System.EventArgs e)
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



        public void _cursorTool_Change(object sender, Steema.TeeChart.Tools.CursorChangeEventArgs e)
        {
            Steema.TeeChart.Tools.CursorTool _currentCursor = sender as Steema.TeeChart.Tools.CursorTool;
            double xValue = _currentCursor.XValue;
            double yValue = _currentCursor.YValue;
            ControlCollection parentControl = this.pnlChart.Controls;
            for (int i = 0; i < parentControl.Count; i++)
            {
                if (parentControl[i] is BaseChart)
                {
                    BaseChart baseChart = parentControl[i] as BaseChart;
                    if (!(this.ParamTypeCD != "MET" && baseChart.Name == "RAW"))
                    {
                        for (int j = 0; j < baseChart.SPCChart.Chart.Tools.Count; j++)
                        {

                            if (baseChart.SPCChart.Chart.Tools[j].GetType() == typeof(Steema.TeeChart.Tools.CursorTool))
                            {

                                Steema.TeeChart.Tools.CursorTool _CursorDesc = (Steema.TeeChart.Tools.CursorTool)baseChart.SPCChart.Chart.Tools[j];
                                if (_CursorDesc.Pen.Color == Color.Gray)
                                {
                                    if (xValue != _CursorDesc.XValue && yValue != _CursorDesc.YValue)
                                    {
                                        CursorSynchronize(_currentCursor, _CursorDesc);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        void SPCChart_ClickBackground(object sender, MouseEventArgs e)
        {
            BaseChart baseChart = FindSPCChart((Control)sender);
            RemoveToggleOn(baseChart);
            RemoveToggleOff(baseChart);
            RemoveLinkTraceChart(baseChart);
        }

        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                    if (seriesInfo == null ||
                        !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX) 
                        || s[valueIndex] == null
                        ) return;

                    #region ToolTip
                    BaseChart baseChart = FindSPCChart((Control)sender);
                    if (baseChart is DefaultChart)
                    {
                        this.clickedChart = baseChart;
                        this.clickedSeries = s;
                        this.clickedIndex = valueIndex;

                        //mChartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);
                        //iValueIndex = valueIndex;

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
                                //mChartInfomationUI.InfomationSpreadReSet(this.dtRawData.Rows[valueIndex]);
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[valueIndex], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[valueIndex]);
                                iValueIndex = valueIndex;
                            }
                            else
                            {
                                //mChartInfomationUI.InfomationSpreadReSet(this.dtRawData.Rows[_iSEQIdx]);
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
                                //mChartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);
                                mChartInfomationUI.InfomationSpreadReSet(baseChart.dtDataSourceToolTip.Rows[valueIndex], baseChart.Name);
                                this._dtOCAPData = baseChart.dtDataSourceToolTip.Clone();
                                this._dtOCAPData.ImportRow(baseChart.dtDataSourceToolTip.Rows[valueIndex]);
                                iValueIndex = valueIndex;
                            }
                            else
                            {
                                //mChartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[_iSEQIdx]);
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
            RemoveLinkTraceChart(baseChart);
            AddLinkTraceChart(baseChart);

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

        private void AddLinkTraceChart(BaseChart baseChart)
        {
            MenuItem mi = new MenuItem("Link Trace Data");
            mi.Name = "LINK_TRACE_DATA_FDC";
            mi.Click += new EventHandler(mi_Click);
            baseChart.ContextMenu.MenuItems.Add(mi);
        }

        void mi_Click(object sender, EventArgs e)
        {
            this.bbtnListChart_ButtonClick("LINK_TRACE_DATA_FDC");
        }

        private void RemoveToggleOn(BaseChart baseChart)
        {
            baseChart.ContextMenu.MenuItems.RemoveByKey("Toggle On");
        }

        private void RemoveToggleOff(BaseChart baseChart)
        {
            baseChart.ContextMenu.MenuItems.RemoveByKey("Toggle Off");
        }

        private void RemoveLinkTraceChart(BaseChart baseChart)
        {
            baseChart.ContextMenu.MenuItems.RemoveByKey("LINK_TRACE_DATA_FDC");
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
                        if (this.dtRawData != null)
                        {
                            this.dtRawData.Rows[int.Parse(seq_index)][Definition.COL_TOGGLE_YN] = value;
                        }
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

        public void bbtnListChart_ButtonClick(string name)
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
                {
                    this.ClickButtonConfiguration();
                }
                else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)
                    this.ClickButtonShowChartData();
                // JIRA SPC-613 [GF] Annotation cannot key in comment. - 2011.10.04 by ANDREW KO
                // No.25 Item on GF Communication Sheet-20110612 V1.21 - 2011.10.04 by ANDREW KO
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

                    BISTel.eSPC.Page.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.OCAP.OCAPDetailsPopup();
                    popupOCAP.BSHOWVIEWCHARTBUTTON = false;
                    popupOCAP.ChartVariable = (ChartInterface)ChartVariable.Clone();
                    popupOCAP.ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.OCAP;
                    popupOCAP.ChartVariable.OPERATION_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                    popupOCAP.ChartVariable.PRODUCT_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PRODUCT_ID], 1].Text;
                    popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(dtRawData, iValueIndex);
                    popupOCAP.ChartVariable.OCAPRawID = sRawid;
                    if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW)
                        popupOCAP.PopUpType = enum_PopupType.View;
                    // JIRA SPC-613 [GF] Annotation cannot key in comment. - 2011.10.04 by ANDREW KO
                    // No.25 Item on GF Communication Sheet-20110612 V1.21 - 2011.10.04 by ANDREW KO
                    else
                        popupOCAP.PopUpType = enum_PopupType.Modify;
                    popupOCAP.SessionData = this.sessionData;
                    popupOCAP.URL = this.URL;
                    popupOCAP.InitializePopup();

                    if (popupOCAP.DialogResult != DialogResult.Cancel)
                    {
                        popupOCAP.ShowDialog(this);

                        if (popupOCAP.DialogResult == DialogResult.OK)
                        {
                            popupOCAP.Dispose();
                        }

                        if (this.llstTmpOcapComment != null && this.llstTmpOcapComment.Count > 0)
                        {
                            byte[] baData = llstTmpOcapComment.GetSerialData();
                            _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                            if (_dsOcapComment != null)
                                ChartVariable.dtOCAP = _dsOcapComment.Tables[0];
                        }
                    }

                    if (this.llstTmpOcapComment != null && this.llstTmpOcapComment.Count > 0)
                    {
                        byte[] baData = llstTmpOcapComment.GetSerialData();
                        _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                        if (_dsOcapComment != null)
                            ChartVariable.dtOCAP = _dsOcapComment.Tables[0];
                    }
                    //}
                    //else
                    //{
                    //    string strRawid = GetOCAPRawID(_dataManager.RawDataTable, iValueIndex);
                    //    string[] arr = strRawid.Split(';');
                    //    string sRawid = string.Empty;
                    //    for (int i = 0; i < arr.Length; i++)
                    //    {
                    //        if (string.IsNullOrEmpty(arr[i])) continue;
                    //        sRawid += arr[i] + ",";
                    //    }
                    //    if (sRawid.EndsWith(",")) sRawid = sRawid.Substring(0, sRawid.Length - 1);

                    //    if (string.IsNullOrEmpty(sRawid))
                    //    {
                    //        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    //        return;
                    //    }

                    //    BISTel.eSPC.Page.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.OCAP.OCAPDetailsPopup();
                    //    popupOCAP.ChartVariable = ChartVariable;
                    //    popupOCAP.ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.OCAP;
                    //    popupOCAP.ChartVariable.OPERATION_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                    //    popupOCAP.ChartVariable.PRODUCT_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PRODUCT_ID], 1].Text;
                    //    popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(_dataManager.RawDataTable, iValueIndex);
                    //    popupOCAP.ChartVariable.OCAPRawID = sRawid;
                    //    popupOCAP.PopUpType = enum_PopupType.View;
                    //    popupOCAP.SessionData = this.sessionData;
                    //    popupOCAP.URL = this.URL;
                    //    popupOCAP.InitializePopup();
                    //    DialogResult result = popupOCAP.ShowDialog(this);
                    //    if (result == DialogResult.OK)
                    //    {
                    //        popupOCAP.Dispose();
                    //    }
                    //}
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
                    this.strucContextinfo.SPCModelType = SPCMODEL_TYPE.SPC_CHART;

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

                    eSPC.Condition.Report.SPCChartConditionPopup _chartConditionPop = new BISTel.eSPC.Condition.Report.SPCChartConditionPopup();
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
                //SPC-679 added by enkim 2012.11.27
                else if (name.ToUpper() == "LINK_TRACE_DATA_FDC")
                {
                    if (mChartInfomationUI.bsprInformationData.ActiveSheet.RowCount == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    if (iValueIndex == -1)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SERIES", null, null);
                        return;
                    }

                    if (this._dtOCAPData != null && this._dtOCAPData.Rows.Count > 0)
                    {
                        if (this.ParamTypeCD != "TRS")
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_TRACE_SUM_DATA", null, null);
                            return;
                        }

                        string strTraceEQP = "";
                        string strTraceModule = "";
                        string strTraceDCP = "";
                        string strTraceLot = "";
                        string strTraceRecipe = "";
                        DateTime dtEventTime = new DateTime();
                        string strTraceSubStrateID = "";
                        string strParamAlias = "";
                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                        {
                            strTraceEQP = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.EQP_ID].ToString();
                            if (!string.IsNullOrEmpty(strTraceEQP))
                            {
                                string[] strArrTraceEQP = strTraceEQP.Split(';');
                                strTraceEQP = strArrTraceEQP[0];
                            }
                        }

                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.MODULE_ID))
                        {
                            strTraceModule = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.MODULE_ID].ToString();
                            if (!string.IsNullOrEmpty(strTraceModule))
                            {
                                string[] strArrTraceModule = strTraceModule.Split(';');
                                strTraceModule = strArrTraceModule[0];
                            }
                        }

                        if (_dtOCAPData.Columns.Contains("DCP_ID"))
                        {
                            strTraceDCP = this._dtOCAPData.Rows[0]["DCP_ID"].ToString();
                            if (!string.IsNullOrEmpty(strTraceDCP))
                            {
                                string[] strArrTraceDCP = strTraceDCP.Split(';');
                                strTraceDCP = strArrTraceDCP[0];
                            }
                        }

                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                        {
                            strTraceLot = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.LOT_ID].ToString();
                            if (!string.IsNullOrEmpty(strTraceLot))
                            {
                                string[] strArrTraceLot = strTraceLot.Split(';');
                                strTraceLot = strArrTraceLot[0];
                            }
                        }

                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.RECIPE_ID))
                        {
                            strTraceRecipe = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.RECIPE_ID].ToString();
                            if (!string.IsNullOrEmpty(strTraceRecipe))
                            {
                                string[] strArrTracerecipe = strTraceRecipe.Split(';');
                                strTraceRecipe = strArrTracerecipe[0];
                            }
                        }

                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.TIME))
                        {
                            string strTempEventTime = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.TIME].ToString();
                            if (!string.IsNullOrEmpty(strTempEventTime))
                            {
                                string[] strArrTraceEventTime = strTempEventTime.Split(';');
                                strTempEventTime = strArrTraceEventTime[0];
                                if (!string.IsNullOrEmpty(strTempEventTime))
                                {
                                    dtEventTime = DateTime.Parse(strTempEventTime);
                                }

                            }
                        }

                        if (_dtOCAPData.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                        {
                            string strTempSubStrateID = this._dtOCAPData.Rows[0][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();
                            if (!string.IsNullOrEmpty(strTempSubStrateID))
                            {
                                string[] strArrTraceSubStrateID = strTempSubStrateID.Split(';');
                                bool isMultiSubStrate = false;

                                if (strArrTraceSubStrateID.Length > 1)
                                {
                                    for (int idxTraceSubStrate = 1; idxTraceSubStrate < strArrTraceSubStrateID.Length; idxTraceSubStrate++)
                                    {
                                        if (!string.IsNullOrEmpty(strArrTraceSubStrateID[idxTraceSubStrate]))
                                        {
                                            if (strArrTraceSubStrateID[0] != strArrTraceSubStrateID[idxTraceSubStrate])
                                            {
                                                strTraceSubStrateID = "";
                                                isMultiSubStrate = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (!isMultiSubStrate)
                                {
                                    strTraceSubStrateID = strArrTraceSubStrateID[0];
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(strTraceEQP))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DRAW_TRACE_CHART", new string[]{"EQP ID"}, null);
                            return;
                        }

                        if (string.IsNullOrEmpty(strTraceModule))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DRAW_TRACE_CHART", new string[]{"Module ID"}, null);
                            return;
                        }

                        if (string.IsNullOrEmpty(strTraceDCP))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DRAW_TRACE_CHART", new string[]{"DCP ID"}, null);
                            return;
                        }

                        if (string.IsNullOrEmpty(strTraceLot))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DRAW_TRACE_CHART", new string[]{"LOT ID"}, null);
                            return;
                        }

                        if (string.IsNullOrEmpty(strTraceRecipe))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DRAW_TRACE_CHART", new string[]{"Recipe ID"}, null);
                            return;
                        }

                        LinkedList llstGetParamAlias = new LinkedList();
                        llstGetParamAlias.Add(Definition.CONDITION_KEY_EQP_MODULE_ID, strTraceModule);
                        llstGetParamAlias.Add(Definition.CONDITION_KEY_EQP_DCP_ID, strTraceDCP);
                        llstGetParamAlias.Add(Definition.CONDITION_KEY_SUM_PARAM_ALIAS, ChartVariable.PARAM_ALIAS);
                        strParamAlias = this._wsSPC.GetParamAlias(llstGetParamAlias.GetSerialData());

                        LinkedList llstTraceCondition = new LinkedList();
                        llstTraceCondition.Add("MESSAGE_TYPE", "SHOW_CHART_FORM_CONTEXT");
                        llstTraceCondition.Add("EQP_ID", strTraceEQP);
                        llstTraceCondition.Add("MODULE_ID", strTraceModule);
                        llstTraceCondition.Add("DCP_ID", strTraceDCP);
                        llstTraceCondition.Add("LOT_ID", strTraceLot);
                        llstTraceCondition.Add("RECIPE_ID", strTraceRecipe);
                        if (!string.IsNullOrEmpty(strTraceSubStrateID))
                        {
                            llstTraceCondition.Add("SUBSTRATE_ID", strTraceSubStrateID);
                        }
                        llstTraceCondition.Add("EVENT_DTTS", dtEventTime);
                        llstTraceCondition.Add("PARAM_ALIAS", strParamAlias);

                        if (this.linkTraceDataViewEvent != null)
                        {
                            linkTraceDataViewEvent(this, null, llstTraceCondition);
                        }
                        else
                        {
                            this.SendMessage("TRACE_DATA", true, llstTraceCondition, 0);
                        }
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_INFO_CLICK_SERIES));
                        return;
                    }
                }
                //added end

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        void SPCChart_ActiveDynamicCondition(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ActiveDCEventArgs csea)
        {
            //throw new NotImplementedException();
        }



        public void bTitPanelInformation_MinimizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 40;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
            //BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
            //bTitlePanel.Dock = DockStyle.Fill;
        }

        public void bTitPanelInformation_MaximizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 300;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
            //BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
            //bTitlePanel.Dock = DockStyle.Fill;
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
                            if (baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.AVG ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.RAW ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.STDDEV ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.RANGE ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MA ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MR ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MSD)
                            {
                                sOCAPSeries = baseChart.SPCChart.Chart.Series[n];
                            }
                        }
                    }

                    bool bRawOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST);
                    bool bMeanOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST);
                    bool bStdOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST);
                    bool bRangeOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST);
                    bool bMaOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST);
                    bool bMsOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST);
                    bool bMrOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST);
                    bool bEwmaMeanOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST);
                    bool bEwmaStdOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST);
                    bool bEwmaRangeOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST);


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
                            case Definition.CHART_TYPE.RAW:
                                if (bRawOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }

                                break;

                            case Definition.CHART_TYPE.XBAR:
                                if (bMeanOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }
                                                    
                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }
                                                    
                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.STDDEV:
                                if (bStdOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.RANGE:
                                if (bRangeOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.MA:
                                if (bMaOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.MSD:
                                if (bMsOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.MR:
                                if (bMrOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.EWMA_MEAN:
                                if (bEwmaMeanOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.EWMA_RANGE:
                                if (bEwmaRangeOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

                                                }
                                            }
                                        }
                                        sArrTemp = null;
                                    }
                                }
                                break;

                            case Definition.CHART_TYPE.EWMA_STDDEV:
                                if (bEwmaStdOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Split('^');
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
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData.Add(sArrTemp[l], baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData.Add(sArrTemp[l], j.ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    if (baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID) && !String.IsNullOrEmpty(baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString()))
                                                    {
                                                        if (j.ToString() == baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString())
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                        }
                                                        else
                                                        {
                                                            lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.DTSOURCEID].ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lnkOCAPData[sArrTemp[l]] = lnkOCAPData[sArrTemp[l]].ToString() + "," + j.ToString();
                                                    }

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

                            Annotation ann = null;

                            for (int idxtool = 0; idxtool < baseChart.SPCChart.Chart.Tools.Count; idxtool++)
                            {
                                if (baseChart.SPCChart.Chart.Tools[idxtool].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                                {
                                    ann = (Annotation)baseChart.SPCChart.Chart.Tools[idxtool];
                                }
                            }

                            if (ann != null)
                            {
                                ann.Active = false;
                            }

                            arrAnnotationIdx = null;
                            SeriesInfo si = baseChart.SPCChart.GetSeriesInfo(sOCAPSeries);
                            List<SeriesInfo> lsSeries = new List<SeriesInfo>();
                            lsSeries.Add(si);

                            lsAnnotationIdx.Add(lsAnnotationIdxTemp);

                            baseChart.SPCChart.PointCrossLine.PointHighLightOn(lsSeries, lsAnnotationIdx);
                            baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].SeriesColor = Color.Green;
                            
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).LinePen.Visible = false;

                            //common lib 변경으로 인한 code 변경
                            //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                            //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;

                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.InflateMargins = true;

                            if (ann != null)
                            {
                                ann.Active = true;
                            }
                            //end

                            //baseChart.SPCChart.Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Text;// = BTeeChart.XAxisLabelType.t;
                            //baseChart.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                            //baseChart.SPCChart.ChangeXLabelColumn("TIME2");
                            //baseChart.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                            //baseChart.SPCChart.Refresh();

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
                ArrayList arrsOCAPSeries = new ArrayList();
                for (int i = 0; i < this.pnlChart.Controls.Count; i++)
                {
                    sOCAPRAWID = "";
                    arrsOCAPSeries.Clear();
                    arrIdxOCAP.Clear();
                    baseChart = (BaseChart)this.pnlChart.Controls[i];
                    baseChart.SPCChart.PointCrossLine.PointHighLightOff();
                    if (baseChart.SPCChart.Chart.Series.Count > 0)
                    {
                        for (int n = 0; n < baseChart.SPCChart.Chart.Series.Count; n++)
                        {
                            if (baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.AVG ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.RAW ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.STDDEV ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.RANGE ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MA ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MR ||
                                baseChart.SPCChart.Chart.Series[n].Title == Definition.CHART_COLUMN.MSD)
                            {
                                sOCAPSeries = baseChart.SPCChart.Chart.Series[n];
                            }
                            if (this.mParamTypeCd == "MET" && baseChart.Name == "RAW")
                            {
                                if (baseChart.lstRawColumn.Count > 0)
                                {
                                    if (baseChart.lstRawColumn.Contains(baseChart.SPCChart.Chart.Series[n].Title))
                                    {
                                        arrsOCAPSeries.Add(baseChart.SPCChart.Chart.Series[n]);
                                    }
                                }
                            }
                        }
                    }
                    bool bRawOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST);
                    bool bMeanOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST);
                    bool bStdOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST);
                    bool bRangeOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST);
                    bool bMaOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST);
                    bool bMsOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST);
                    bool bMrOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST);
                    bool bEwmaMeanOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST);
                    bool bEwmaStdOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST);
                    bool bEwmaRangeOcap = baseChart.dtDataSourceToolTip.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST);

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
                            case Definition.CHART_TYPE.RAW:
                                if (bRawOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string strParamList = "";
                                        string[] strArrParamList = null;
                                        if (baseChart.dtDataSourceToolTip.Columns.Contains("PARAM_LIST"))
                                        {
                                            strParamList = baseChart.dtDataSourceToolTip.Rows[j]["PARAM_LIST"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(strParamList))
                                        {
                                            strArrParamList = strParamList.Split(';');
                                        }
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Split(';');
                                        string sRawid = string.Empty;
                                        for (int k = 0; k < arr.Length; k++)
                                        {
                                            if (string.IsNullOrEmpty(arr[k]))
                                                continue;
                                            string[] arrTemp = arr[k].Split('^');
                                            for (int t = 0; t < arrTemp.Length; t++)
                                            {
                                                if (string.IsNullOrEmpty(arrTemp[t]))
                                                    continue;
                                                sRawid += "," + arrTemp[t];
                                                if (arrTemp[t] == this.mChartVariable.OCAPRawID)
                                                {
                                                    if (!string.IsNullOrEmpty(strParamList) && strArrParamList != null)
                                                    {
                                                        if (strArrParamList.Length >= k)
                                                        {
                                                            if (arrsOCAPSeries.Count > 0)
                                                            {
                                                                for (int idxt = arrsOCAPSeries.Count - 1; idxt >= 0; idxt--)
                                                                {
                                                                    if (((Series)arrsOCAPSeries[idxt]).Title != strArrParamList[k])
                                                                    {
                                                                        arrsOCAPSeries.RemoveAt(idxt);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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
                            case Definition.CHART_TYPE.XBAR:
                                if (bMeanOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.STDDEV:
                                if (bStdOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.RANGE:
                                if (bRangeOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.MA:
                                if (bMaOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.MSD:
                                if (bMsOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.MR:
                                if (bMrOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.EWMA_MEAN:
                                if (bEwmaMeanOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.EWMA_RANGE:
                                if (bEwmaRangeOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Split('^');
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
                            case Definition.CHART_TYPE.EWMA_STDDEV:
                                if (bEwmaStdOcap)
                                {
                                    sTempOcapRawID = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Replace(";", "").Replace(",", "").Replace("^", "").TrimEnd();
                                    if (!string.IsNullOrEmpty(sTempOcapRawID))
                                    {
                                        sTempOcapRawID = null;
                                        string[] arr = baseChart.dtDataSourceToolTip.Rows[j][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Split('^');
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
                        SeriesInfo si = null;
                        if (sOCAPSeries != null)
                        {
                            si = baseChart.SPCChart.GetSeriesInfo(sOCAPSeries);
                        }

                        List<SeriesInfo> lsSeries = new List<SeriesInfo>();
                        if (si != null)
                        {
                            lsSeries.Add(si);
                        }
                        if (lsSeries.Count == 0 && arrsOCAPSeries.Count > 0)
                        {
                            lsSeries.Add(baseChart.SPCChart.GetSeriesInfo((Series)arrsOCAPSeries[0]));
                            if (((DataSet)(((Series)arrsOCAPSeries[0]).DataSource)).Tables[0].Columns.Contains("X") && lsAnnotationIdxTemp.Count > 0)
                            {
                                for (int idxDs = 0; idxDs < ((DataSet)(((Series)arrsOCAPSeries[0]).DataSource)).Tables[0].Rows.Count; idxDs++)
                                {
                                    if (((DataSet)(((Series)arrsOCAPSeries[0]).DataSource)).Tables[0].Rows[idxDs]["X"].ToString() == lsAnnotationIdxTemp[0].ToString())
                                    {
                                        lsAnnotationIdxTemp[0] = idxDs;
                                    }
                                }
                            }
                        }
                        if (lsSeries.Count > 0)
                        {
                            lsAnnotationIdx.Add(lsAnnotationIdxTemp);
                            baseChart.SPCChart.PointCrossLine.PointHighLightOn(lsSeries, lsAnnotationIdx);
                            baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].SeriesColor = Color.Yellow;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).LinePen.Visible = false;
                            //common lib 변경으로 인한 code 변경
                            //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                            //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.HorizSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.VertSize = 4;
                            ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[0].Series).Pointer.InflateMargins = true;
                            //end
                            //baseChart.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                            //baseChart.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                            //baseChart.SPCChart.ChangeXLabelColumn("TIME2");
                            //baseChart.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                            //baseChart.SPCChart.Refresh();
                        }
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

            // check if the row is already processed
            if (rawIDs.Contains(raw.Rows[rowIndex][Definition.COL_RAW_ID].ToString()))
                return;

            // check if the row changed
            if (string.IsNullOrEmpty(raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString()) ||
                raw.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString() == original.Rows[rowIndex][Definition.COL_TOGGLE_YN].ToString())
            {
                return;
            }

            // collect changed toggle information of the rawID and add
            DataRow[] drs = raw.Select(Definition.COL_RAW_ID + " = '" + raw.Rows[rowIndex][Definition.COL_RAW_ID] + "'", Definition.CHART_COLUMN.ORDERINFILEDATA);
            int sampleCount = int.Parse(drs[0][Definition.COL_SAMPLE_COUNT].ToString());
            List<string> tempToggle = new List<string>();
            for (int i = 0; i < sampleCount; i++)
            {
                // Cause toggled data can not be included in the data table, the default value is 'Y'
                tempToggle.Add("Y");
            }

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

                tempToggle[order] = toggle;
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

                //common lib 변경으로 인한 code 변경
                //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.HorizSize = 4;
                //((Points)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.VertSize = 4;

                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.HorizSize = 4;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.VertSize = 4;
                ((BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles.ExtremeBFastPoints)baseChart.SPCChart.PointCrossLine.PointHighLightSeries[i].Series).Pointer.InflateMargins = true;
                //end
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

            //this.includingToggleData = !this.includingToggleData;
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

            if (this.ParamTypeCD == "MET" &&
                baseChart.NAME == Definition.CHART_TYPE.RAW)
            {
                if (seriesName == Definition.CHART_COLUMN.USL ||
                    seriesName == Definition.CHART_COLUMN.LSL ||
                    seriesName == Definition.CHART_COLUMN.UCL ||
                    seriesName == Definition.CHART_COLUMN.LCL ||
                    seriesName == Definition.CHART_COLUMN.TARGET)
                    return false;

            }
            else if (!((seriesName == Definition.CHART_COLUMN.RAW)
                   || (seriesName == Definition.CHART_COLUMN.STDDEV)
                   || (seriesName == Definition.CHART_COLUMN.AVG)
                   || (seriesName == Definition.CHART_COLUMN.RANGE)
                   || (seriesName == Definition.CHART_COLUMN.MA)
                   || (seriesName == Definition.CHART_COLUMN.MSD)
                   || (seriesName == Definition.CHART_COLUMN.MR)
                   || (seriesName == Definition.CHART_COLUMN.MEAN)
                   || (seriesName == Definition.CHART_COLUMN.EWMAMEAN)
                   || (seriesName == Definition.CHART_COLUMN.EWMARANGE)
                   || (seriesName == Definition.CHART_COLUMN.EWMASTDDEV)))
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
                FilterRawTable(groupingAndFilteringValue);
                panel1.Enabled = false;
            }

            LinkedList lstChartSeriesColor = null;
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                BaseChart baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.LlstChartSeriesColor = lstChartSeriesColor;

                Annotation ann = null;

                for (int idxtool = 0; idxtool < baseChart.SPCChart.Chart.Tools.Count; idxtool++)
                {
                    if (baseChart.SPCChart.Chart.Tools[idxtool].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                    {
                        ann = (Annotation)baseChart.SPCChart.Chart.Tools[idxtool];
                    }
                }

                if (ann != null)
                {
                    ann.Active = false;
                }

                baseChart.Filter(groupingAndFilteringValue);
                if (baseChart.LlstChartSeriesColor != null)
                {
                    lstChartSeriesColor = baseChart.LlstChartSeriesColor;
                }
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


        //2012-03-23 added by rachel -->
        //[SPC-659]
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
