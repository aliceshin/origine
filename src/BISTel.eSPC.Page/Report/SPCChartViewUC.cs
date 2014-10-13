using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Report
{
    public partial class SPCChartViewUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        BSpreadUtility mBSpreadUtil;

        CommonUtility mComUtil;
        ChartUtility mChartUtil;
        ChartInterface mChartVariable;
        CHART_PARENT_MODE mChartMode = CHART_PARENT_MODE.SPC_CONTROL_CHART;


        DataTableGroupBy mDTGroupBy = new DataTableGroupBy();
        DataSet mDSChart = null;
        DataSet mDSOCAPValue = null;

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

        DataTable mDTContext;
        DataTable mDTCustomContext;
        DefaultChart chartBase;
        DefaultRawChartView chartBaseRaw;
        DataSet _ds = null;

        LinkedList _llstData = new LinkedList();

        private SessionData _sdPopupSessionData = null;
        private string _sPopupApplicationName = "";
        private string _sReq = "";
        private string _sPort = "";
        private string _sFunc = "";
        private string _sPhysicalPath = "";
        private string _sBrowerVersion = "";
        private string _sClientIP = "";
        private string _sURL = "";
        LinkedList mllstContextType;
        private bool _bUseComma;

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

        #endregion


        public SPCChartViewUC()
        {
            AppDomain.CurrentDomain.AppendPrivatePath("EES/DLLS/eSPC");
            AppDomain.CurrentDomain.AppendPrivatePath("EESSub/DLLS/eSPC");

            this.InitializeComponent();
        }

        

        private bool SetRequestData()
        {
            try
            {
                if (base.Request.GetValue("MODEL_CONFIG_RAWID") != null)
                {
                    List<string> listTemp = base.Request.GetValue("MODEL_CONFIG_RAWID");
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        _llstData.Add("MODEL_CONFIG_RAWID", listTemp[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("NO MODEL_CONFIG_RAWID Info1");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("NO MODEL_CONFIG_RAWID Info");
                    return false;
                }

                if (base.Request.GetValue("CHART_TYPE") != null)
                {
                    List<string> listTemp = base.Request.GetValue("CHART_TYPE");
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        _llstData.Add("CHART_TYPE", listTemp[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("NO CHART_TYPE Info");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("NO CHART_TYPE Info");
                    return false;
                }

                if (base.Request.GetValue("OCAP_RAWID") != null)
                {
                    List<string> listTemp = base.Request.GetValue("OCAP_RAWID");
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        _llstData.Add("OCAP_RAWID", listTemp[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("NO OCAP_RAWID Info");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("NO OCAP_RAWID Info");
                    return false;
                }

                if (base.Request.GetValue("OCAP_DTTS") != null)
                {
                    List<string> listTemp = base.Request.GetValue("OCAP_DTTS");
                    if (listTemp != null && listTemp.Count > 0)
                    {
                        _llstData.Add("OCAP_DTTS", listTemp[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("NO OCAP_DTTS Info");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("NO OCAP_DTTS Info");
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        void SPCChartViewUC_CancelEvent(object sender)
        {
            BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CloseProgress(this);
            _wsSPC.CancelAsync(null);
        }

        public void InitializePage()
        {
            if (ChartVariable == null) return;

            for (int i = 0; i < this.bbtnListChart.Items.Count; i++)
            {

                if (CHART_MODE == CHART_PARENT_MODE.SPC_CONTROL_CHART)
                {
                    if ((this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT && ChartVariable.MAIN_YN != "Y"))
                        this.bbtnListChart.Items[i].Visible = false;
                    else
                        this.bbtnListChart.Items[i].Visible = true;
                }
                else
                {

                    if ((this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT) ||
                       (this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART) ||
                       (this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CONFIGURATION))
                        this.bbtnListChart.Items[i].Visible = false;

                }
            }

            this.mllstChart.Clear();
            this.mllstChart = GetDefaultChart();

            this.InitializeBSpread();
            this.InitializeChartSeriesVisibleType();
            this.InitializeChart();
        }

        private void InitializeChartSeriesVisibleType()
        {
            this.mllstChartSeriesVisibleType = new LinkedList();
            Hashtable htButtonList = this.mInitialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            for (int i = htButtonList.Count - 1; i >= 0; i--)
            {
                Hashtable htButtonAttributes = (Hashtable)htButtonList[i.ToString()];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();
                string DefaultVisible = htButtonAttributes[Definition.XML_VARIABLE_DEFAULTVALUE].ToString().ToUpper();

                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                {
                    this.mllstChartSeriesVisibleType.Add(sButtonName, true);
                }
            }
        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetCodeData(lk.GetSerialData());

            LinkedList _llstData = new LinkedList();
            DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
            this.mllstContextType = CommonPageUtil.SetContextType(dsContextType);
        }

        public void InitializeChart()
        {

            try
            {
                ArrayList arrEQPID = new ArrayList();
                ArrayList arrModuleID = new ArrayList();
                DataSet dsGroupItem = new DataSet();

                if (this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                {
                    _dataManager.RawDataTableOriginal = this.ChartVariable.dtParamData.Copy();
                    _dataManager.RawDataTable = this.ChartVariable.dtParamData;
                }

                int iHeight = 300;

                //if(this.ChartVariable.dtParamData != null && this.ChartVariable.dtParamData.Rows.Count > 0)
                //{
                //    string _sTempModelConfigRawid = this.ChartVariable.dtParamData.Rows[0][COLUMN.MODEL_CONFIG_RAWID].ToString();
                //    LinkedList _lnkTemp = new LinkedList();
                //    _lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, _sTempModelConfigRawid);
                //    dsGroupItem = this._wsSPC.GetGroupContextValue(_lnkTemp.GetSerialData());

                //    for (int i = 0; i < this.ChartVariable.dtParamData.Rows.Count; i++)
                //    {
                //        string strEQPIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString();
                //        string strModuleIDTemp = this.ChartVariable.dtParamData.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString();

                //        if (strEQPIDTemp != null && strEQPIDTemp.Length > 0 && !strEQPIDTemp.ToUpper().Equals("NULL") && !arrEQPID.Contains(strEQPIDTemp))
                //        {
                //            arrEQPID.Add(strEQPIDTemp);
                //        }
                //        if (strModuleIDTemp != null && strModuleIDTemp.Length > 0 && !strModuleIDTemp.ToUpper().Equals("NULL") && !arrModuleID.Contains(strModuleIDTemp))
                //        {
                //            arrModuleID.Add(strModuleIDTemp);
                //        }
                //    }
                //}


                this.pnlChart.Controls.Clear();

                chartBase = null;
                Steema.TeeChart.Tools.CursorTool _cursorTool = null;

                string strKey = string.Empty;
                string strValue = string.Empty;
                string strChartType = string.Empty;

                strChartType = this._llstData["CHART_TYPE"].ToString();

                switch (strChartType)
                {
                    case "RAW":
                        strKey = "RAW";
                        strValue = mllstChart["RAW"].ToString();
                        break;
                    case "MEAN":
                        strKey = "X-BAR";
                        strValue = mllstChart["X-BAR"].ToString();
                        break;
                    case "STD":
                        strKey = "STDDEV";
                        strValue = mllstChart["STDDEV"].ToString();
                        break;
                    case "RANGE":
                        strKey = "RANGE";
                        strValue = mllstChart["RANGE"].ToString();
                        break;
                    case "EWMA":
                        strKey = "EWMA_MEAN";
                        strValue = mllstChart["EWMA_MEAN"].ToString();
                        break;
                    case "MA":
                        strKey = "MA";
                        strValue = mllstChart["MA"].ToString();
                        break;
                    case "MSTD":
                        strKey = "MSD";
                        strValue = mllstChart["MSD"].ToString();
                        break;
                }

                if (strChartType == "RAW")
                {
                    chartBaseRaw = new DefaultRawChartView(_dataManager);
                    chartBaseRaw.ClearChart();
                    chartBaseRaw.Title = strValue;
                    chartBaseRaw.Name = strKey;
                    chartBaseRaw.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                    chartBaseRaw.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                    chartBaseRaw.SPCChartTitlePanel.Name = strKey;
                    chartBaseRaw.URL = this.URL;
                    chartBaseRaw.Dock = System.Windows.Forms.DockStyle.Fill;

                    //chartBaseRaw.ContextMenu = ctChart;
                    chartBaseRaw.ParentControl = this.pnlChart;
                    chartBaseRaw.StartDateTime = ChartVariable.dateTimeStart;
                    chartBaseRaw.EndDateTime = ChartVariable.dateTimeEnd;
                    chartBaseRaw.mllstContextType = this.mllstContextType;
                    chartBaseRaw.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);
                    if (!chartBaseRaw.DrawSPCChart())
                    {
                    }
                    else
                    {
                        _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                        _cursorTool.Active = true;
                        _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                        _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        _cursorTool.Pen.Color = Color.Gray;
                        _cursorTool.FollowMouse = true;
                        chartBaseRaw.SPCChart.Chart.Tools.Add(_cursorTool);
                        chartBaseRaw.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                        chartBaseRaw.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChartRaw_ClickSeries);
                        this.pnlChart.Controls.Add(chartBaseRaw);
                    }
                }
                else
                {
                    chartBase = new DefaultChart(_dataManager);
                    chartBase.ClearChart();
                    chartBase.Title = strValue;
                    chartBase.Name = strKey;
                    //chartBase.BtnVisible = false;
                    chartBase.GroupVisible = false;
                    chartBase.FilterVisible = false;
                    chartBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                    chartBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                    chartBase.SPCChartTitlePanel.Name = strKey;
                    chartBase.Height = iHeight;
                    chartBase.URL = this.URL;
                    chartBase.Dock = System.Windows.Forms.DockStyle.Fill;
                    chartBase.ContextMenu = this.bbtnListChart.ContextMenu;
                    chartBase.ParentControl = this.pnlChart;
                    chartBase.StartDateTime = ChartVariable.dateTimeStart;
                    chartBase.EndDateTime = ChartVariable.dateTimeEnd;
                    chartBase.ARREQPID = arrEQPID;
                    chartBase.ARRMODULEID = arrModuleID;
                    chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, mllstChartSeriesVisibleType);
                    if (!chartBase.DrawSPCChart())
                    {
                    }
                    else
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
                    }
                }

                
                GC.Collect();
                GC.WaitForPendingFinalizers();
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
            //this.bTitlePanel1.Title = this.mMHandler.GetVariable(Definition.TITLE_KEY_SPC_CONTROL_CHART);
            //spc-1281
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        public void InitializeDataButton()
        {
            //this.mInitialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.BUTTONLIST_KEY_CHART, this.sessionData);
        }

        public void InitializeBSpread()
        {
            this.bplInformation.Visible = true;

            DataSet _dsContext = new DataSet();
            DataSet _dsRule = new DataSet();
            LinkedList lnkList = new LinkedList();
            SPCStruct.ContextTypeInfo mContextTypeInfo = null;

            lnkList.Clear();
            lnkList.Add(Definition.DynamicCondition_Condition_key.OCAP_RAWID, this._llstData["OCAP_RAWID"].ToString());
            _dsRule = _wsSPC.GetOCAPDetails(lnkList.GetSerialData());

            if (DataUtil.IsNullOrEmptyDataSet(_dsRule)) return;

            //Initialize Context List

            string strContext_list = _dsRule.Tables[0].Rows[0][COLUMN.CONTEXT_LIST].ToString();
            string[] arrCol = strContext_list.Split('\t');
            DataTable dtContext = new DataTable();
            DataRow nRow = null;

            dtContext.Columns.Add(Definition.SpreadHeaderColKey.KEY, typeof(string));
            dtContext.Columns.Add(Definition.SpreadHeaderColKey.VALUE, typeof(string));
            
            nRow = dtContext.NewRow();
            nRow[Definition.SpreadHeaderColKey.KEY] = Definition.CHART_COLUMN.TIME;
            nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["ooc_dtts"].ToString();
            dtContext.Rows.Add(nRow);

            nRow = dtContext.NewRow();
            nRow[Definition.SpreadHeaderColKey.KEY] = "OCAP VALUE";
            nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["ooc_value"].ToString();
            dtContext.Rows.Add(nRow);

            nRow = dtContext.NewRow();
            nRow[Definition.SpreadHeaderColKey.KEY] = "RULE NO";
            nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["rule_no"].ToString();
            dtContext.Rows.Add(nRow);

            nRow = dtContext.NewRow();
            nRow[Definition.SpreadHeaderColKey.KEY] = "RULE DESCRIPTION";
            nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["rule_desc"].ToString();
            dtContext.Rows.Add(nRow);
            for (int i = 0; i < arrCol.Length; i++)
            {
                string[] arrValue = arrCol[i].ToString().Split('=');
                if (arrValue[0] == COLUMN.CONTEXT_KEY) continue;
                mContextTypeInfo = mllstContextType[arrValue[0]] as SPCStruct.ContextTypeInfo;
                if (mContextTypeInfo == null) continue;
                nRow = dtContext.NewRow();
                nRow[Definition.SpreadHeaderColKey.KEY] = mContextTypeInfo.NAME;
                nRow[Definition.SpreadHeaderColKey.VALUE] = arrValue[1];

                dtContext.Rows.Add(nRow);
            }
            _dsContext.Tables.Add(dtContext);
            this.bsprContext.ClearHead();
            this.bsprContext.AddHeadComplete();
            this.bsprContext.AutoGenerateColumns = true;

            this.bsprContext.DataSet = _dsContext;
            this.bsprContext.ActiveSheet.Columns[0].BackColor = Color.WhiteSmoke;
            this.bsprContext.ActiveSheet.Columns[1].BackColor = Color.White;
            this.bsprContext.ActiveSheet.Columns[0].Width = 100;
            this.bsprContext.ActiveSheet.Columns[1].Width = 150;

            this.bsprContext.ActiveSheet.Columns[0].DataField = Definition.SpreadHeaderColKey.KEY;
            this.bsprContext.ActiveSheet.Columns[1].DataField = Definition.SpreadHeaderColKey.VALUE;
            this.bsprContext.ActiveSheet.ColumnHeader.Columns[0].Label = "KEY";
            this.bsprContext.ActiveSheet.ColumnHeader.Columns[1].Label = "VALUE";
        }


        #region ::: Override Method

        public override void PageInit()
        {
            //System.Diagnostics.Debugger.Break();

            //this.KeyOfPage = this.GetType().FullName;
            this.mInitialization = new Initialization();
            this.mInitialization.InitializePath();
            this.mMHandler = MultiLanguageHandler.getInstance();
            this.mComUtil = new CommonUtility();
            this.mBSpreadUtil = new BSpreadUtility();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            _wsSPC.GetSPCControlChartDataCompleted += new BISTel.eSPC.Page.eSPCWebService.GetSPCControlChartDataCompletedEventHandler(_wsSPC_GetSPCControlChartDataCompleted);
            _wsSPC.GetSPCChartViewDataCompleted += new BISTel.eSPC.Page.eSPCWebService.GetSPCChartViewDataCompletedEventHandler(_wsSPC_GetSPCChartViewDataCompleted);
            BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.GetInstance().CancelEvent +=new BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CancelDegate(SPCChartViewUC_CancelEvent);

            this._dataManager = new SourceDataManager();
            this.mChartUtil = new ChartUtility();
            this.strucContextinfo = new SPCStruct.ChartContextInfo();

            this.InitializeLayout();
            this.InitializeDataButton();
            this.InitializeCode();
            this.InitializePage();

            
            bool bResult = this.SetRequestData();
            if (bResult)
            {
                this.PROC_DataBinding();
            }
        }

        //public override void PageSearch(LinkedList llstCondition)
        //{
        //    DataTable dt = null;
        //    try
        //    {

        //        this.mChartVariable = new ChartInterface();

        //        if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
        //            this.mStartTime = CommonPageUtil.StartDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
        //        }
        //        if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
        //            this.mEndTime = CommonPageUtil.EndDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
        //        }


        //        if (llstCondition[Definition.DynamicCondition_Search_key.SITE] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SITE];
        //            this.mSite = DCUtil.GetValueData(dt);
        //        }

        //        if (llstCondition[Definition.DynamicCondition_Search_key.FAB] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.FAB];
        //            this.mFab = DCUtil.GetValueData(dt);
        //        }

        //        if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
        //            this.mLineRawID = DCUtil.GetValueData(dt);
        //            this.mLine = DataUtil.GetDisplayData(dt);
        //            mChartVariable.LINE = this.mLine;
        //        }

        //        if (llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL];
        //            mChartVariable.SPC_MODEL = DataUtil.GetDisplayData(dt);
        //            mChartVariable.SPC_MODEL_RAWID = DCUtil.GetValueData(dt);
        //        }

        //        this.mParamTypeCd = mComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
        //        if (llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID] != null)
        //        {
        //            dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID];

        //            mChartVariable.PARAM_ALIAS = DataUtil.GetDisplayData(dt);
        //            mChartVariable.DEFAULT_CHART = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.DEFAULT_CHART_LIST);
        //            mChartVariable.MAIN_YN = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.MAIN_YN);
        //            mChartVariable.MODEL_CONFIG_RAWID = DCUtil.GetValueData(dt);
        //            mChartVariable.RESTRICT_SAMPLE_DAYS = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.RESTRICT_SAMPLE_DAYS);
        //            mChartVariable.AREA = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.AREA);
        //            mChartVariable.EQP_MODEL = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.EQP_MODEL);
        //            this.mAreaRawID = DataUtil.GetConditionKeyData(dt, Definition.DynamicCondition_Condition_key.AREA_RAWID);
        //            this.mArea = mChartVariable.AREA;

        //            if (string.IsNullOrEmpty(mChartVariable.MODEL_CONFIG_RAWID))
        //            {
        //                this.MsgClose();
        //                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "SPC Model"));
        //                return;
        //            }

        //            if (string.IsNullOrEmpty(mChartVariable.DEFAULT_CHART))
        //            {
        //                this.MsgClose();
        //                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            MSGHandler.DisplayMessage(MSGType.Information, "Please Select SPC Model.");
        //            return;
        //        }


        //        if (llstCondition[Definition.DynamicCondition_Search_key.CONTEXT] != null)
        //        {
        //            this.mDTContext = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CONTEXT];
        //        }

        //        if (llstCondition[Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT] != null)
        //        {
        //            this.mDTCustomContext = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT];
        //        }


        //        PROC_DataBinding();

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
        //    }
        //    finally
        //    {
        //        if (dt != null) dt.Dispose();
        //    }

        //}

        #endregion

        #region ::: User Defined Method.

        private LinkedList GetDefaultChart()
        {
            LinkedList _llstDefaultChart = new LinkedList();
            _llstDefaultChart.Clear();

            if (DataUtil.IsNullOrEmptyDataSet(mDSChart)) return _llstDefaultChart;

            if (ChartVariable == null) return _llstDefaultChart;

            foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
            {
                _llstDefaultChart.Add(dr[COLUMN.CODE].ToString(), dr[COLUMN.DESCRIPTION].ToString());
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
                BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.ShowProgress(this, this.mMHandler.GetMessage(Definition.LOADING_DATA), true);

                DateTime dtValue = DateTime.Parse(this._llstData["OCAP_DTTS"].ToString());

                this.mChartVariable = new ChartInterface();
                this.mStartTime = CommonPageUtil.StartDate(dtValue.AddDays(-1).ToString(Definition.DATETIME_FORMAT));
                this.mEndTime = CommonPageUtil.EndDate(dtValue.ToString(Definition.DATETIME_FORMAT));
                this.mChartVariable.MODEL_CONFIG_RAWID = this._llstData["MODEL_CONFIG_RAWID"].ToString();

                this.mllstChartSearchData.Clear();
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);
                this.mllstChartSearchData.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                DataSet dsTemp = _wsSPC.GetSPCCalcModelDataPopup(mllstChartSearchData.GetSerialData());
                this.mChartVariable.PARAM_ALIAS = dsTemp.Tables["MODEL_CONFIG_MST_SPC"].Rows[0]["PARAM_ALIAS"].ToString();

                this.mllstChartSearchData.Clear();
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.mStartTime);
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.mEndTime);
                this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, mChartVariable.MODEL_CONFIG_RAWID);
                if (this._llstData["CHART_TYPE"].ToString() == "RAW")
                {
                    _wsSPC.GetSPCChartViewDataAsync(mllstChartSearchData.GetSerialData());
                }
                else
                {
                    _wsSPC.GetSPCControlChartDataAsync(mllstChartSearchData.GetSerialData());
                }
            }
            catch{}
        }
        private void PROC_DataBinding1()
        {
            CreateChartDataTable _createChartDT = null;
            DataTable mDTChartData = null;

            mDTChartData = new DataTable();
            _createChartDT = new CreateChartDataTable();

            try
            {
                this.MsgShow(this.mMHandler.GetMessage(Definition.LOADING_DATA));
                
                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }
                else
                {
                    this.mllstChartSearchData.Add(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST, getCustomContext());

                    if (this._llstData["CHART_TYPE"].ToString() == "RAW")
                    {
                        mDTChartData = CommonPageUtil.CLOBnBLOBParsingRaw(_ds, mllstChartSearchData, false, false);
                    }
                    else
                    {
                        mDTChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, mllstChartSearchData, false);
                    }

                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (DataUtil.IsNullOrEmptyDataTable(mDTChartData))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }
                    else
                    {
                        mDTChartData = DataUtil.DataTableImportRow(mDTChartData.Select(null, Definition.CHART_COLUMN.TIME));
                        mChartVariable.lstDefaultChart.Clear();
                        mChartVariable.complex_yn = Definition.VARIABLE_Y;
                        mChartVariable.OPERATION_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        mChartVariable.PRODUCT_ID = mDTChartData.Rows[0][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                        mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(mChartVariable.DEFAULT_CHART);
                        //mChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(this.mMHandler.GetVariable(Definition.VARIABLE_SPC_INITIAL_DISPLAY_CHART));
                        mChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        mChartVariable.dateTimeStart = DateTime.Parse(this.mStartTime);
                        mChartVariable.dateTimeEnd = DateTime.Parse(this.mEndTime);
                        _createChartDT.COMPLEX_YN = Definition.VARIABLE_Y;
                        mChartVariable.dtParamData = _createChartDT.GetMakeDataTable(mDTChartData);
                        mChartVariable.lstRawColumn = _createChartDT.lstRawColumn;

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

        void _wsSPC_GetSPCControlChartDataCompleted(object sender, BISTel.eSPC.Page.eSPCWebService.GetSPCControlChartDataCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_OPERATION_CANCEL", null, null);
                }
                else
                {
                    this._ds = e.Result;
                    PROC_DataBinding1();
                }
            }
            catch(Exception ex)
            {
                BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CloseProgress(this);
                MSGHandler.DisplayMessage(MSGType.Information, ex.Message);
            }
        }

        void _wsSPC_GetSPCChartViewDataCompleted(object sender, BISTel.eSPC.Page.eSPCWebService.GetSPCChartViewDataCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_OPERATION_CANCEL", null, null);
                }
                else
                {
                    this._ds = e.Result;
                    PROC_DataBinding1();
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar.EESProgressBar.CloseProgress(this);
                MSGHandler.DisplayMessage(MSGType.Information, ex.Message);
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

        private BaseRawChartView FindSPCChartRaw(Control ctl)
        {
            Control parentControl = ctl.Parent;
            while (parentControl != null)
            {
                if (parentControl is BaseRawChartView)
                    return parentControl as BaseRawChartView;
                else
                    parentControl = parentControl.Parent;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCol"></param>
        /// <param name="strValue"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

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

            sb.AppendFormat("{0} : {1}\r\n", "PRODUCT ID", _sProductID);
            sb.AppendFormat("{0} : {1}\r\n", "EQP ID", _sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", "LOT_ID", _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", "OPERATION_ID", _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", "SUBSTRATE_ID", _sSubstrateID);

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
                                                sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                            }
                                        }
                                        else
                                        {
                                            sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                }
                            }
                            else
                            {
                                if (strChartName == Definition.CHART_TYPE.XBAR)
                                {
                                    if (sKey.Contains("wafer"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                    }
                                }
                                else
                                {
                                    sb.AppendFormat("{0} : {1}\r\n", mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                }
                            }
                        }
                    }
                }
                llstChartSeries = null;
            }

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            {
                string strRawid = GetOCAPRawID(_dataManager.RawDataTable, rowIndex);
                DataTable dtOCAP = null;
                mDTGroupBy = new DataTableGroupBy();
                if (!string.IsNullOrEmpty(strRawid))
                {
                    string _fieldList = "ocap_rawid,RAW_RULE";
                    string _groupby = "ocap_rawid,RAW_RULE";
                    string _sRuleNo = string.Empty;
                    if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
                    if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
                        dtOCAP = mDTGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
                        foreach (DataRow dr in dtOCAP.Rows)
                        {
                            if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
                            _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
                        }
                        sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
                    }
                }
                mDTGroupBy = null;
                if (dtOCAP != null) dtOCAP.Dispose();
            }

            return sb.ToString();
        }

        private string CreateToolTipStringRaw(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            if (chartBaseRaw.IsWaferColumn)
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

                if (chartBaseRaw.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                    _sEQPID = chartBaseRaw.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                    _sLOTID = chartBaseRaw.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                    _sOperationID = chartBaseRaw.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                    _sSubstrateID = chartBaseRaw.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                    _sProductID = chartBaseRaw.dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("TIME"))
                    _sTime = chartBaseRaw.dtDataSource.Rows[rowIndex]["TIME"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains(COLUMN.PARAM_LIST))
                    _sParamList = chartBaseRaw.dtDataSource.Rows[rowIndex][COLUMN.PARAM_LIST].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW"))
                    _sRawValue = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW_USL"))
                    _sRawUSL = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW_USL"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW_UCL"))
                    _sRawUCL = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW_UCL"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW_TARGET"))
                    _sRawTarget = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW_TARGET"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW_LCL"))
                    _sRawLCL = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW_LCL"].ToString();

                if (chartBaseRaw.dtDataSource.Columns.Contains("RAW_LSL"))
                    _sRawLSL = chartBaseRaw.dtDataSource.Rows[rowIndex]["RAW_LSL"].ToString();

                sb.AppendFormat("{0} : {1}\r\n", "PRODUCT ID", _sProductID);
                sb.AppendFormat("{0} : {1}\r\n", "EQP ID", _sEQPID);
                sb.AppendFormat("{0} : {1}\r\n", "LOT_ID", _sLOTID);
                sb.AppendFormat("{0} : {1}\r\n", "OPERATION_ID", _sOperationID);
                sb.AppendFormat("{0} : {1}\r\n", "SUBSTRATE_ID", _sSubstrateID);
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


                sb.AppendFormat("{0} : {1}\r\n", "PRODUCT ID", _sProductID);
                sb.AppendFormat("{0} : {1}\r\n", "EQP ID", _sEQPID);
                sb.AppendFormat("{0} : {1}\r\n", "LOT_ID", _sLOTID);
                sb.AppendFormat("{0} : {1}\r\n", "OPERATION_ID", _sOperationID);
                sb.AppendFormat("{0} : {1}\r\n", "SUBSTRATE_ID", _sSubstrateID);


                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString();

                    if (sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;

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
                                                sb.AppendFormat("{0} : {1}\r\n", this.mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                            }
                                        }
                                        else
                                        {
                                            sb.AppendFormat("{0} : {1}\r\n", this.mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    sb.AppendFormat("{0} : {1}\r\n", this.mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                                }
                            }
                            else
                            {
                                sb.AppendFormat("{0} : {1}\r\n", this.mMHandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                            }
                        }
                    }
                }
            }
            llstChartSeries = null;

            return sb.ToString();
        }

        private string GetOCAPRawID(DataTable dt, int rowIndex)
        {
            string strRawid = string.Empty;
            if (dt.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                strRawid = dt.Rows[rowIndex][Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();

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

        //private void ClickButtonCalc()
        //{

        //}

        //private void ClickButtonChartView()
        //{
        //    BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup chartViewPop = new BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup();
        //    chartViewPop.URL = this.URL;
        //    chartViewPop.SessionData = this.sessionData;
        //    chartViewPop.DataSetChart = mDSChart; //Chart 종류   
        //    chartViewPop.llstDefaultChart = mllstChart;
        //    chartViewPop.InitializePopup();
        //    DialogResult result = chartViewPop.ShowDialog(this);
        //    if (result == DialogResult.OK)
        //    {
        //        mllstChart.Clear();
        //        mllstChart = chartViewPop.llstResultListChart;
        //        this.InitializeChartSeriesVisibleType();
        //        this.InitializeChart(); //chart 다시 그림  

        //        chartViewPop.Dispose();
        //    }
        //}

        //private void ClickButtonConfiguration()
        //{
        //    Modeling.SPCConfigurationPopup spcConifgPopup = new Modeling.SPCConfigurationPopup();
        //    spcConifgPopup.SESSIONDATA = this.sessionData;
        //    spcConifgPopup.URL = this.URL;
        //    spcConifgPopup.PORT = this.Port;
        //    spcConifgPopup.CONFIG_MODE = ConfigMode.VIEW;
        //    spcConifgPopup.CONFIG_RAWID = ChartVariable.MODEL_CONFIG_RAWID;
        //    spcConifgPopup.MAIN_YN = ChartVariable.MAIN_YN;
        //    spcConifgPopup.InitializePopup();
        //    DialogResult result = spcConifgPopup.ShowDialog(this);
        //    if (result == DialogResult.OK)
        //    {
        //        spcConifgPopup.Dispose();
        //    }

        //}

        //private void ClickButtonShowChartData()
        //{
        //    DataTable dt = _dataManager.RawDataTable.Copy();
        //    ChartDataPopup chartDataPop = null;
        //    try
        //    {
        //        chartDataPop = new ChartDataPopup();
        //        chartDataPop.URL = this.URL;
        //        chartDataPop.SessionData = this.sessionData;
        //        chartDataPop.DataTableParam = CommonPageUtil.ExcelExport(dt, ChartVariable.lstRawColumn);
        //        chartDataPop.InitializePopup();
        //        DialogResult result = chartDataPop.ShowDialog(this);
        //        if (result == DialogResult.OK)
        //        {
        //            chartDataPop.Dispose();
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    finally
        //    {
        //        dt.Dispose();
        //    }

        //}


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

        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip
                BaseChart baseChart = FindSPCChart((Control)sender);
                if (baseChart is DefaultChart)
                {
                    string strCol = s.Title;
                    if (strCol == Definition.CHART_COLUMN.USL_LSL)
                        strCol = Definition.CHART_COLUMN.USL;
                    else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                        strCol = Definition.CHART_COLUMN.UCL;

                    string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();
                    int _iSEQIdx = Convert.ToInt32(seriesInfo.SeriesData.Rows[valueIndex][CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());

                    if (valueIndex == _iSEQIdx)
                    {
                        iValueIndex = valueIndex;
                    }
                    else
                    {
                        iValueIndex = _iSEQIdx;
                    }

                    for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                        {
                            Annotation ann = (Annotation)baseChart.SPCChart.Chart.Tools[i];
                            if(valueIndex == _iSEQIdx)
                                this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseChart.NAME));
                            else
                                this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, _iSEQIdx, baseChart.NAME));
                            break;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                iValueIndex = -1;
            }
        }

        private void SPCChartRaw_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip
                BaseRawChartView baseChart = FindSPCChartRaw((Control)sender);
                if (baseChart is DefaultRawChartView)
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
                                this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipStringRaw(strCol, strValue, valueIndex, baseChart.NAME));
                            else
                                this.mChartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipStringRaw(strCol, strValue, _iSEQIdx, baseChart.NAME));
                            break;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                iValueIndex = -1;
            }
        }



        private void bbtnListChart_ButtonClick(string name)
        {
            //try
            //{

            //    if (ChartVariable == null || ChartVariable.dtParamData == null || ChartVariable.dtParamData.Rows.Count == 0)
            //    {
            //        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
            //        return;
            //    }

            //    if (name.ToUpper() == Definition.ButtonKey.CALC)
            //        this.ClickButtonCalc();
            //    else if (name.ToUpper() == Definition.ButtonKey.SELECT_CHART_TO_VIEW)
            //        this.ClickButtonChartView();
            //    else if (name.ToUpper() == Definition.ButtonKey.DEFAULT_CHART)
            //    {
            //        mllstChart.Clear();
            //        mllstChart = GetDefaultChart();
            //        this.InitializeChartSeriesVisibleType();
            //        this.InitializeChart();
            //    }
            //    else if (name.ToUpper() == Definition.ButtonKey.CONFIGURATION)
            //        this.ClickButtonConfiguration();
            //    else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)
            //        this.ClickButtonShowChartData();
            //    else if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW)
            //    {

            //        if (mChartInfomationUI.bsprInformationData.ActiveSheet.RowCount == 0)
            //        {
            //            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
            //            return;
            //        }

            //        if (iValueIndex == -1) return;
            //        string strRawid = GetOCAPRawID(_dataManager.RawDataTable, iValueIndex);
            //        string[] arr = strRawid.Split(';');
            //        string sRawid = string.Empty;
            //        for (int i = 0; i < arr.Length; i++)
            //        {
            //            if (string.IsNullOrEmpty(arr[i])) continue;
            //            sRawid += arr[i] + ",";
            //        }
            //        if (sRawid.EndsWith(",")) sRawid = sRawid.Substring(0, sRawid.Length - 1);

            //        if (string.IsNullOrEmpty(sRawid))
            //        {
            //            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
            //            return;
            //        }

            //        BISTel.eSPC.Page.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.OCAP.OCAPDetailsPopup();
            //        popupOCAP.ChartVariable = ChartVariable;

            //        popupOCAP.ChartVariable.OPERATION_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
            //        popupOCAP.ChartVariable.PRODUCT_ID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PRODUCT_ID], 1].Text;
            //        popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(_dataManager.RawDataTable, iValueIndex);
            //        popupOCAP.ChartVariable.OCAPRawID = sRawid;
            //        popupOCAP.PopUpType = enum_PopupType.View;
            //        popupOCAP.SessionData = this.sessionData;
            //        popupOCAP.URL = this.URL;
            //        popupOCAP.InitializePopup();
            //        DialogResult result = popupOCAP.ShowDialog(this);
            //        if (result == DialogResult.OK)
            //        {
            //            popupOCAP.Dispose();
            //        }
            //    }
            //    else if (name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART)
            //    {

            //        string _sOperationID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
            //        string _sMetOperationID = string.Empty;
            //        string _sParamItem = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.PARAM_ALIAS], 1].Text;

            //        if (this.mParamTypeCd.Equals("MET"))
            //            _sMetOperationID = this.mChartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this.mChartInfomationUI.sListInformation[Definition.CHART_COLUMN.MEASURE_OPERATION_ID], 1].Text;
            //        else
            //            _sMetOperationID = _sOperationID;


            //        LinkedList lk = new LinkedList();
            //        lk.Add(Definition.DynamicCondition_Search_key.SITE, CommonPageUtil.AddArrayList(this.mSite));
            //        lk.Add(Definition.DynamicCondition_Search_key.FAB, CommonPageUtil.AddArrayList(this.mFab));
            //        lk.Add(Definition.DynamicCondition_Search_key.LINE, CommonPageUtil.AddArrayList(this.mLineRawID + ";" + this.mLine));
            //        lk.Add(Definition.DynamicCondition_Search_key.AREA, CommonPageUtil.AddArrayList(this.mAreaRawID + ";" + this.mArea));
            //        lk.Add(Definition.DynamicCondition_Search_key.PARAM_TYPE, CommonPageUtil.AddArrayList(this.mParamTypeCd));

            //        if (!string.IsNullOrEmpty(ChartVariable.EQP_MODEL))
            //            lk.Add(Definition.DynamicCondition_Search_key.EQPMODEL, CommonPageUtil.AddArrayList(ChartVariable.EQP_MODEL));
            //        lk.Add(Definition.DynamicCondition_Search_key.OPERATION, CommonPageUtil.AddArrayList(_sMetOperationID));
            //        lk.Add(Definition.DynamicCondition_Search_key.PARAM, CommonPageUtil.AddArrayList(_sParamItem));
            //        lk.Add(Definition.DynamicCondition_Search_key.SUBDATA, CommonPageUtil.AddArrayList("MEAN" + ";" + "AVG"));
            //        lk.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, CommonPageUtil.AddArrayList("EQP_ID" + ";" + "MACHINE"));
            //        lk.Add(Definition.DynamicCondition_Search_key.TARGET_TYPE, CommonPageUtil.AddArrayList("PROC;PROC"));
            //        lk.Add(Definition.DynamicCondition_Search_key.TARGET, CommonPageUtil.AddArrayList(_sOperationID));
            //        lk.Add(Definition.DynamicCondition_Search_key.CHART_LIST, CommonPageUtil.AddArrayList("RUN" + ";" + "RUN Chart"));
            //        lk.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, CommonPageUtil.AddArrayList("CUSTOM"));
            //        lk.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, CommonPageUtil.AddArrayList(ChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT_MS)));
            //        lk.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, CommonPageUtil.AddArrayList(ChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT_MS)));


            //        base.SubmitPage("DLLS/eSPC/BISTel.eSPC.Page.dll", "BISTel.eSPC.Page.Analysis.SPCAnalysisUC", "Analysis", "Analysis", false, lk);


            //    }
            //    else if (name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT)
            //    {
            //        if (ChartVariable.MAIN_YN != "Y")
            //        {
            //            MSGHandler.DisplayMessage(MSGType.Information, "SPC Main Model만 가능합니다.");
            //            return;
            //        }
            //        if (this.strucContextinfo == null)
            //            this.strucContextinfo = new SPCStruct.ChartContextInfo();

            //        this.strucContextinfo.AREA = this.ChartVariable.AREA;
            //        this.strucContextinfo.LINE = this.ChartVariable.LINE;
            //        this.strucContextinfo.MODEL_CONFIG_RAWID = this.ChartVariable.MODEL_CONFIG_RAWID;
            //        this.strucContextinfo.MAIN_YN = this.ChartVariable.MAIN_YN;
            //        this.strucContextinfo.SPC_MODEL_NAME = this.ChartVariable.SPC_MODEL;
            //        this.strucContextinfo.PARAM_ALIAS = this.ChartVariable.PARAM_ALIAS;
            //        this.strucContextinfo.SPCModelType = SPCMODEL_TYPE.SPC_CHART;

            //        DataSet _ds = GetDSContextCall();
            //        if (!DataUtil.IsNullOrEmptyDataSet(_ds))
            //            this.strucContextinfo.DTModelContext = _ds.Tables[0];

            //        if (!DataUtil.IsNullOrEmptyDataTable(this.mDTCustomContext))
            //        {
            //            this.strucContextinfo.llstCustomContext = new LinkedList();
            //            foreach (DataRow dr in this.mDTCustomContext.Rows)
            //                this.strucContextinfo.llstCustomContext.Add(dr[Definition.DynamicCondition_Condition_key.VALUEDATA], dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA]);
            //        }

            //        if (!DataUtil.IsNullOrEmptyDataTable(this.mDTContext))
            //        {
            //            this.strucContextinfo.llstContext = new LinkedList();
            //            foreach (DataRow dr in this.mDTContext.Rows)
            //                this.strucContextinfo.llstContext.Add(dr[Definition.DynamicCondition_Condition_key.VALUEDATA], dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA]);
            //        }

            //        eSPC.Condition.Report.SPCChartConditionPopup _chartConditionPop = new BISTel.eSPC.Condition.Report.SPCChartConditionPopup();
            //        _chartConditionPop.CONTEXT_INFO = strucContextinfo;
            //        _chartConditionPop.InitializePopup();
            //        _chartConditionPop.ShowDialog();
            //        DialogResult dResult = _chartConditionPop.ButtonResult;
            //        if (dResult == DialogResult.OK)
            //        {
            //            this.strucContextinfo = _chartConditionPop.CONTEXT_INFO;
            //            this.mDTContext = PROC_CreateDTContext(this.strucContextinfo.llstContext);
            //            this.mDTCustomContext = PROC_CreateDTContext(this.strucContextinfo.llstCustomContext);
            //            this.PROC_DataBinding();
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            //}
        }

        void SPCChart_ActiveDynamicCondition(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ActiveDCEventArgs csea)
        {
            //throw new NotImplementedException();
        }



        private void bTitPanelInformation_MinimizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 30;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
        }

        private void bTitPanelInformation_MaximizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 300;
            this.bplInformation.Controls[0].Dock = DockStyle.Fill;
        }

        #endregion
    }
}
