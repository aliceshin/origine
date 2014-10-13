using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Web.Services;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.DataHandler;


using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Common;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class ChartViewPopup : BasePopupFrm
    {
        #region : Field

        CommonUtility _comUtil;
        SessionData _SessionData;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;

        DataSet _dsOcapComment = null;

        ChartInterface _ChartVariable = new ChartInterface();
        Initialization mInitialization;
        string mParamTypeCd = string.Empty;

        BISTel.eSPC.Page.ATT.Report.SPCChartUC spcControlChart;

        MultiLanguageHandler _lang;

        public delegate void LinkTraceDataViewEventHandlerPopup(object sender, EventArgs e, LinkedList llstTraceLinkData);
        public event LinkTraceDataViewEventHandlerPopup linkTraceDataViewEventPopup;

        //SPC-1292, KBLEE, START
        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sGroupName = string.Empty;
        //SPC-1292, KBLEE, END

        //SPC-1292, KBLEE, START
        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }

        public string AREA_RAWID
        {
            get { return _sAreaRawID; }
            set { _sAreaRawID = value; }
        }

        public string LINE_RAWID
        {
            get { return _sLineRawID; }
            set { _sLineRawID = value; }
        }
        //SPC-1292, KBLEE, END

        #endregion

        #region : Constructor

        public ChartViewPopup()
        {
            InitializeComponent();

            this.mInitialization = new Initialization();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();

            
        }

        void spcControlChart_linkTraceDataViewEvent(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            if (linkTraceDataViewEventPopup != null)
            {
                linkTraceDataViewEventPopup(this, null, llstTraceLinkData);
            }
        }

        void ChartViewPopup_CancelEvent(object sender)
        {
        }

        #endregion

        #region : Initialization

        public void InitializePopup()
        {
            this._lang = MultiLanguageHandler.getInstance();

            this.InitializeLayout();
            this.InitializeCondition();
        }

        public void InitializeCondition()
        {

            CreateChartDataTable _createChartDT = new CreateChartDataTable();

            try
            {
                double dSearchPeriod = double.NaN;

                this.bapSearch.Visible = true;
                if (this.ChartVariable.CHART_PARENT_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.MODELING)
                {
                    ArrayList arrSearchCondition = this.mInitialization.GetSearchPeriod(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC);
                    if (arrSearchCondition != null && arrSearchCondition.Count == 2)
                    {
                        string sSearchType = arrSearchCondition[0].ToString();
                        string sSearchValue = arrSearchCondition[1].ToString();

                        bool bResult = double.TryParse(sSearchValue, out dSearchPeriod);

                        if (bResult)
                        {
                            dSearchPeriod = 0 - dSearchPeriod;
                            if (sSearchType.ToUpper() == "DAY")
                            {
                                this.bDtStart.Value = ChartVariable.dateTimeEnd.AddDays(dSearchPeriod);
                                this.bDtEnd.Value = ChartVariable.dateTimeEnd;
                            }
                            else
                            {
                                this.bDtStart.Value = ChartVariable.dateTimeStart;
                                this.bDtEnd.Value = ChartVariable.dateTimeEnd;
                            }
                        }
                        else
                        {
                            this.bDtStart.Value = ChartVariable.dateTimeStart;
                            this.bDtEnd.Value = ChartVariable.dateTimeEnd;
                        }
                    }
                    else
                    {
                        this.bDtStart.Value = ChartVariable.dateTimeStart;
                        this.bDtEnd.Value = ChartVariable.dateTimeEnd;
                    }
                }
                else
                {
                    this.bDtStart.Value = ChartVariable.dateTimeStart;
                    this.bDtEnd.Value = ChartVariable.dateTimeEnd;
                }

                ChartVariable.dateTimeStart = this.bDtStart.Value;
                ChartVariable.dateTimeEnd = this.bDtEnd.Value;

                if (ChartVariable.CHART_PARENT_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.MODELING)
                {
                    if (string.IsNullOrEmpty(ChartVariable.MODEL_CONFIG_RAWID))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_SELECT_MODEL", null, null);
                        return;
                    }
                    CallDataTrxDataFirst(dSearchPeriod);
                    this.MsgClose();
                    return;
                }
                else
                {
                    if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_EQP_ID] != null)
                        ChartVariable.EQP_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_EQP_ID, Definition.CONDITION_KEY_EQP_ID);

                    if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                        ChartVariable.PRODUCT_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_PRODUCT_ID, Definition.CONDITION_KEY_PRODUCT_ID);

                    if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_LOT_ID] != null)
                        ChartVariable.LOT_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_LOT_ID, Definition.CONDITION_KEY_LOT_ID);

                    if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_OPERATION_ID] != null)
                        ChartVariable.OPERATION_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_OPERATION_ID, Definition.CONDITION_KEY_OPERATION_ID);

                    if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_SUBSTRATE_ID] != null)
                        ChartVariable.SUBSTRATE_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_SUBSTRATE_ID, Definition.CONDITION_KEY_SUBSTRATE_ID);

                    _createChartDT.COMPLEX_YN = Definition.VARIABLE_Y;
                    if (ChartVariable.CHART_PARENT_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.PPK_REPORT)
                    {
                        if (DataUtil.IsNullOrEmptyDataTable(ChartVariable.dtParamData))
                        {
                            this.MsgClose();
                            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                            this.Close();
                            return;
                        }
                        if (ChartVariable.lstRawColumn.Count == 0)
                            ChartVariable.lstRawColumn = _createChartDT.CallRefCol(ChartVariable.dtParamData);
                    }
                    else
                    {
                        if (DataUtil.IsNullOrEmptyDataTable(ChartVariable.dtResource))
                        {
                            this.MsgClose();
                            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                            return;
                        }
                        ChartVariable.dtParamData = _createChartDT.GetMakeDataTable(ChartVariable.dtResource);
                        ChartVariable.lstRawColumn = _createChartDT.lstRawColumn;
                        _createChartDT = null;
                    }

                    ChartVariable.lstDefaultChart.Clear();
                    ChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(ChartVariable.DEFAULT_CHART);

                    this.InitializeSPCChart();

                }

            }
            catch (Exception ex)
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {
                this.MsgClose();
                _createChartDT = null;
            }

        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_CHART);
        }

        public void InitializeSPCChart()
        {
            this.spcControlChart = new BISTel.eSPC.Page.ATT.Report.SPCChartUC();
            LinkedList lnkTemp = new LinkedList();
            lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, ChartVariable.MODEL_CONFIG_RAWID);
            spcControlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            spcControlChart.URL = this.URL;
            spcControlChart.ChartVariable = ChartVariable;
            spcControlChart.sessionData = this.SessionData;
            spcControlChart.Height = this.pnlChart.Height;
            spcControlChart.CHART_MODE = ChartVariable.CHART_PARENT_MODE;
            spcControlChart.ParamTypeCD = this.mParamTypeCd;
            spcControlChart.GROUP_NAME = this._sGroupName;
            spcControlChart.LINE_RAWID = this._sLineRawID;
            spcControlChart.AREA_RAWID = this._sAreaRawID;
            spcControlChart.InitializeCommon();
            spcControlChart.InitializePage();
            this.pnlChart.Controls.Add(spcControlChart);
            spcControlChart.linkTraceDataViewEvent += new Report.SPCChartUC.LinkTraceDataViewEventHandler(spcControlChart_linkTraceDataViewEvent);

        }

        #endregion



        #region : Popup Logic

        private void CallDataTrxData()
        {
            DataSet _ds = null;
            DataTable _dtChartData = new DataTable();
            CreateChartDataTable _createChartDT = new CreateChartDataTable();
            try
            {
                LinkedList _llstSearchCondition = new LinkedList();
                _llstSearchCondition.Add(Definition.CONDITION_KEY_START_DTTS, CommonPageUtil.StartDate(this.bDtStart.Value.ToString(Definition.DATETIME_FORMAT)));
                _llstSearchCondition.Add(Definition.CONDITION_KEY_END_DTTS, CommonPageUtil.EndDate(this.bDtEnd.Value.ToString(Definition.DATETIME_FORMAT)));
                _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, ChartVariable.MODEL_CONFIG_RAWID);
                if (!string.IsNullOrEmpty(ChartVariable.CONTEXT_LIST))
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_CONTEXT_KEY_LIST, ChartVariable.CONTEXT_LIST);

                //Cancle기능
                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetATTSPCControlChartData", new object[] { _llstSearchCondition.GetSerialData() });

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

                ChartVariable.dateTimeStart = this.bDtStart.Value;
                ChartVariable.dateTimeEnd = this.bDtEnd.Value;

                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    this.pnlChart.Controls.Clear();
                }
                else
                {
                    this.MsgShow("Drawing Chart... Can't Cancel!!!");

                    _dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearchCondition, false);

                    if (!DataUtil.IsNullOrEmptyDataTable(_dtChartData))
                    {
                        _dtChartData = DataUtil.DataTableImportRow(_dtChartData.Select(null, Definition.CHART_COLUMN.TIME));
                        ChartVariable.DEFAULT_CHART = _dtChartData.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST].ToString();
                        ChartVariable.MAIN_YN = _dtChartData.Rows[0][BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
                        ChartVariable.complex_yn = "Y";
                        ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        ChartVariable.dtResource = _dtChartData;
                        _createChartDT.COMPLEX_YN = ChartVariable.complex_yn;
                        ChartVariable.dtParamData = _createChartDT.GetMakeDataTable(_dtChartData);
                        ChartVariable.lstRawColumn = _createChartDT.lstRawColumn;
                        ChartVariable.lstDefaultChart.Clear();
                        ChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(ChartVariable.DEFAULT_CHART);

                        List<string> rawIDs = new List<string>();
                        if(_dtChartData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                        {
                            bool bPOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                            bool bPNOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                            bool bCOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                            bool bUOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

                            foreach (DataRow dr in _dtChartData.Rows)
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
                            if(rawIDs.Count == 0)
                                rawIDs.Add("");

                            LinkedList llstTmpOcapComment = new LinkedList();
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, rawIDs.ToArray());
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (this.bDtStart.Value.AddHours(-12)).ToString(Definition.DATETIME_FORMAT_MS));
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (this.bDtEnd.Value.AddHours(12)).ToString(Definition.DATETIME_FORMAT_MS));

                            byte[] baData = llstTmpOcapComment.GetSerialData();

                            _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                            ChartVariable.dtOCAP = _dsOcapComment.Tables[0]; 
                        }
                    }
                    this.pnlChart.Controls.Clear();
                    this.InitializeSPCChart();
                    this.MsgClose();
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
                if (_createChartDT != null) _createChartDT = null;
                if (_dtChartData != null) _dtChartData.Dispose();
                if (_ds != null) _ds.Dispose();
            }
        }
        
        #endregion

        #region :: Button Event & Method

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            //SPC-1292, KBLEE, START
            if (this.spcControlChart != null)
                this.GROUP_NAME = this.spcControlChart.GROUP_NAME;
            //SPC-1292, KBLEE, END
        }

        private void bbtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if(bDtStart.Value > bDtEnd.Value)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD", null, null);
                    return;
                }

                CallDataTrxData();
                this.MsgClose();
            }
            catch (Exception ex)
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ReleaseChartData();
            base.OnFormClosed(e);
        }

        private void ReleaseChartData()
        {
            if (pnlChart.Controls.Count > 0)
            {
                if (this.spcControlChart != null)
                {
                    spcControlChart.linkTraceDataViewEvent -= new Report.SPCChartUC.LinkTraceDataViewEventHandler(spcControlChart_linkTraceDataViewEvent); 
                    spcControlChart.GetDirtyStatus();
                }
            }

            if (this._dsOcapComment != null)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._dsOcapComment);
            }

            if (this._ChartVariable != null)
            {
                this._ChartVariable.ReleaseChartData();
                this._ChartVariable = null;
            }
        }

        #endregion

        private void CallDataTrxDataFirst(double dSearchPeriod)
        {
            DataSet _ds = null;
            DataTable _dtChartData = new DataTable();
            CreateChartDataTable _createChartDT = new CreateChartDataTable();
            try
            {
                LinkedList _llstSearchCondition = new LinkedList();
                _llstSearchCondition.Add(Definition.CONDITION_KEY_START_DTTS, CommonPageUtil.StartDate(this.bDtStart.Value.ToString(Definition.DATETIME_FORMAT)));
                _llstSearchCondition.Add(Definition.CONDITION_KEY_END_DTTS, CommonPageUtil.EndDate(this.bDtEnd.Value.ToString(Definition.DATETIME_FORMAT)));
                _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, ChartVariable.MODEL_CONFIG_RAWID);
                if (!string.IsNullOrEmpty(ChartVariable.CONTEXT_LIST))
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_CONTEXT_KEY_LIST, ChartVariable.CONTEXT_LIST);

                //Cancle기능 추가
                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetATTSPCControlChartData", new object[] { _llstSearchCondition.GetSerialData() });

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

                ChartVariable.dateTimeStart = this.bDtStart.Value;
                ChartVariable.dateTimeEnd = this.bDtEnd.Value;

                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    DataSet DTTSTemp = _wsSPC.GetATTLastTRXDataDTTs(ChartVariable.MODEL_CONFIG_RAWID);

                    string temp = "";
                    foreach (DataRow dr in DTTSTemp.Tables[0].Rows)
                    {
                        temp = dr[0].ToString();
                    }

                    if (temp.Length != 0)
                    {
                        foreach (DataRow dr in DTTSTemp.Tables[0].Rows)
                        {
                            DateTime dt = (DateTime)dr[0];
                            this.bDtEnd.Value = dt;

                            if (!double.IsNaN(dSearchPeriod))
                            {
                                this.bDtStart.Value = dt.AddDays(dSearchPeriod);
                            }
                            else
                            {
                                this.bDtStart.Value = dt.AddDays(-7);
                            }
                        }
                        this.CallDataTrxData();
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    
                }
                else
                {
                    this.MsgShow("Drawing Chart... Can't Cancel!!!");

                    _dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearchCondition, false);

                    if (!DataUtil.IsNullOrEmptyDataTable(_dtChartData))
                    {
                        _dtChartData = DataUtil.DataTableImportRow(_dtChartData.Select(null, Definition.CHART_COLUMN.TIME));
                        ChartVariable.DEFAULT_CHART = _dtChartData.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST].ToString();
                        ChartVariable.MAIN_YN = _dtChartData.Rows[0][BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
                        ChartVariable.complex_yn = "Y";
                        ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        ChartVariable.dtResource = _dtChartData;
                        _createChartDT.COMPLEX_YN = ChartVariable.complex_yn;
                        ChartVariable.dtParamData = _createChartDT.GetMakeDataTable(_dtChartData);
                        ChartVariable.lstRawColumn = _createChartDT.lstRawColumn;
                        ChartVariable.lstDefaultChart.Clear();
                        ChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(ChartVariable.DEFAULT_CHART);

                        List<string> rawIDs = new List<string>();
                        if (_dtChartData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                        {
                            bool bPOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                            bool bPNOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                            bool bCOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                            bool bUOcap = _dtChartData.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

                            foreach (DataRow dr in _dtChartData.Rows)
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
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (this.bDtStart.Value.AddHours(-12)).ToString(Definition.DATETIME_FORMAT_MS));
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (this.bDtEnd.Value.AddHours(12)).ToString(Definition.DATETIME_FORMAT_MS));

                            byte[] baData = llstTmpOcapComment.GetSerialData();
                            _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                            ChartVariable.dtOCAP = _dsOcapComment.Tables[0];
                        }
                    }
                    this.pnlChart.Controls.Clear();
                    this.InitializeSPCChart();
                    this.MsgClose();
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
                if (_createChartDT != null) _createChartDT = null;
                if (_dtChartData != null) _dtChartData.Dispose();
                if (_ds != null) _ds.Dispose();
            }
        }

        #region : Public

        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public string ParamTypeCD
        {
            get { return this.mParamTypeCd; }
            set { this.mParamTypeCd = value; }
        }

        #endregion
    }
}
