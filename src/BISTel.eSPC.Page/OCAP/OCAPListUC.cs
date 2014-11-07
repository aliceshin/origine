using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BISTel.eSPC.Page.Common.DataQuery;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;

using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.OCAP
{
    public partial class OCAPListUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;
        //private Common.DataQuery.eSPCWebServiceAsync _wsAsync;

        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstSearchCondition = new LinkedList();
        LinkedList _llstDTSelectCondition = new LinkedList();
        DataSet _dsOCAPList = null;
        DataSet _dsOcapComment = null;

        BISTel.eSPC.Common.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.CommonUtility _ComUtil = null;


        string _line = string.Empty;

        string sStartTime = String.Empty;
        string sEndTime = String.Empty;

        string _sFilterEQPID = string.Empty;
        string _sFilterModuleID = string.Empty;
        string _sFilterLot = string.Empty;
        string _sFilterSubStrate = string.Empty;
        string _sFilterRecipe = string.Empty;

        ChartInterface _ChartVariable = null;
        bool _bRemovedDuplicatedRow = false;

        private Dictionary<string, string> lineName = new Dictionary<string, string>();

        private string _lineRawid = string.Empty;
        private string _areaRawid = string.Empty;
        private string _eqpModel = string.Empty;

        private LinkedList _llGroup = null;

        #endregion

        #region ::: Properties

        public LinkedList llstSearchCondition
        {
            set
            {
                this._llstSearchCondition = value;
            }
            get
            {
                return this._llstSearchCondition;
            }
        }

        #endregion

        #region ::: Constructor
        public OCAPListUC()
        {
            InitializeComponent();
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }


        public override void PageSearch(LinkedList llstCondition)
        {
            this._llstSearchCondition.Clear();
            this._sFilterEQPID = string.Empty;
            this._sFilterModuleID = string.Empty;
            this._sFilterLot = string.Empty;
            this._sFilterSubStrate = string.Empty;
            this._sFilterRecipe = string.Empty;
            DataTable dt = null;

            this._llGroup = new LinkedList();

            if (llstCondition.Contains(Definition.DynamicCondition_Search_key.CHART_ID))
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CHART_ID];
                this._llstSearchCondition.Add(Definition.DynamicCondition_Search_key.CHART_ID, dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString());

                string sChartID = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                LinkedList lnkTemp = new LinkedList();
                DataSet dsTempChartID = new DataSet();
                lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sChartID);
                dsTempChartID = this._wsSPC.GetSPCModelDatabyChartID(lnkTemp.GetSerialData());

                this._llGroup.Clear();

                if (dsTempChartID.Tables.Contains(TABLE.MODEL_GROUP_MST_SPC) && dsTempChartID.Tables[TABLE.MODEL_MST_SPC].Rows.Count > 0 && dsTempChartID.Tables[TABLE.MODEL_GROUP_MST_SPC].Rows.Count > 0)
                {
                    this._llGroup.Add(dsTempChartID.Tables[TABLE.MODEL_MST_SPC].Rows[0][COLUMN.SPC_MODEL_NAME].ToString(), dsTempChartID.Tables[TABLE.MODEL_GROUP_MST_SPC].Rows[0][COLUMN.GROUP_NAME].ToString());
                }

                if (dsTempChartID != null && dsTempChartID.Tables != null && dsTempChartID.Tables.Count == 4 && dsTempChartID.Tables[0].Rows.Count > 0 && dsTempChartID.Tables[1].Rows.Count > 0)
                {
                    string site = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.SITE].ToString();
                    string fab = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.FAB].ToString();
                    string line = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.LINE].ToString();
                    string area = dsTempChartID.Tables[0].Rows[0][COLUMN.AREA].ToString();

                    lineName.Clear();
                    foreach (DataRow dr in dsTempChartID.Tables[0].Rows)
                    {
                        string value = dr[COLUMN.LOCATION_RAWID].ToString();
                        string name = dr[Definition.CONDITION_KEY_LINE].ToString();

                        if (!lineName.ContainsKey(value))
                            lineName.Add(value, name);
                    }

                    if (!base.ApplyAuthory(this.bbtnList, site, fab, line, area))
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                        this.InitializePage();
                        return;
                    }
                }
            }
            else if (llstCondition.Contains(Definition.DynamicCondition_Search_key.SPCMODEL))
            {
                DataTable dtSite = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_SITE];
                string site = "";
                if (dtSite != null)
                    site = dtSite.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();

                DataTable dtFab = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_FAB];
                string fab = "";
                if (dtFab != null)
                    fab = dtFab.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();

                if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.AREA))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SELECT_CONDITION_DATA));
                    return;
                }

                if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
                    lineName.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string value = dr[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                        string name = dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();

                        if (!lineName.ContainsKey(value))
                            lineName.Add(value, name);
                    }
                    _line = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE],
                                                        Definition.DynamicCondition_Condition_key.VALUEDATA);
                    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, _line);
                }

                string strArea = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA], Definition.DynamicCondition_Condition_key.AREA);
                if (!string.IsNullOrEmpty(strArea))
                    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, strArea);

                _areaRawid = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA],
                                                        Definition.DynamicCondition_Condition_key.VALUEDATA);
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, _areaRawid);


                string strEQPModel = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL], Definition.DynamicCondition_Condition_key.VALUEDATA);
                if (!string.IsNullOrEmpty(strEQPModel))
                    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, strEQPModel);

                //string strParamType = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
                //if (!string.IsNullOrEmpty(strParamType))
                //    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, strParamType);

                //foreach()
                string strModelRawID = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL], Definition.DynamicCondition_Condition_key.VALUEDATA);
                if (!string.IsNullOrEmpty(strModelRawID))
                    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, strModelRawID);

                //if (string.IsNullOrEmpty(strParamType))
                //{
                //    strParamType = _wsSPC.GetSPCParamType(this._llstSearchCondition.GetSerialData());
                //    if (!string.IsNullOrEmpty(strParamType))
                //        this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, strParamType);
                //}

                string area = string.Empty;
                if (strArea != null)
                {
                    string[] areas = strArea.Split(',');
                    area = areas[0].Trim('\'');

                    areas = _areaRawid.Split(',');
                    _areaRawid = areas[0].Trim('\'');
                }

                this._llGroup.Clear();

                DataTable dtSPCModel = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL];

                foreach (DataRow dr in dtSPCModel.Rows)
                {
                    this._llGroup.Add(dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString(), dr[COLUMN.GROUP_NAME].ToString());
                }

                string tempLine = string.Empty;
                foreach (var kvp in lineName)
                {
                    tempLine = kvp.Value;
                    break;
                }

                if (!base.ApplyAuthory(this.bbtnList, site, fab, tempLine, area))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "GENERAL_NOT_ENOUGHT_SUFFICIENT", null, null, true);
                    this.InitializePage();
                    return;
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL_OR_CHART_ID", null, null);
                base.ApplyAuthory(this.bbtnList, null, null, null, null);
                return;
            }

            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();
            if (llstCondition[Definition.DynamicCondition_Search_key.FROMDATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.FROMDATE];
                dtFrom = (DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA];
                //SPC-841 by Louis
                sStartTime = dtFrom.ToString(Definition.DATETIME_FORMAT);
                //sStartTime = CommonPageUtil.StartDate(dtFrom.ToString(Definition.DATETIME_FORMAT));
                //////////////////////

            }
            if (llstCondition[Definition.DynamicCondition_Search_key.TODATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.TODATE];
                //SPC-946 by Louis
                //dtTo = ((DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA]).AddDays(1);

                dtTo = ((DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA]);

                //SPC-841 by Louis
                sEndTime = dtTo.ToString(Definition.DATETIME_FORMAT);
                //sEndTime = CommonPageUtil.EndDate(dtTo.ToString(Definition.DATETIME_FORMAT));
                //////////////////////

            }
            if (llstCondition[Definition.DynamicCondition_Search_key.FROMDATE] != null
                && llstCondition[Definition.DynamicCondition_Search_key.TODATE] != null)
            {
                if (dtFrom > dtTo)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD", null, null);
                    return;
                }
            }

            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartTime);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndTime);

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_EQP] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_EQP];
                this._sFilterEQPID = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
            }

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_MODULE] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_MODULE];
                this._sFilterModuleID = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
            }

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_LOT] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_LOT];
                this._sFilterLot = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
            }

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_SUBSTRATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_SUBSTRATE];
                this._sFilterSubStrate = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
            }

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_RECIPE] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_RECIPE];
                this._sFilterRecipe = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
            }

            if (llstCondition[Definition.CONDITION_SEARCH_KEY_OCAP_OOC] != null)
            {
                dt = (DataTable)llstCondition[Definition.CONDITION_SEARCH_KEY_OCAP_OOC];
                string sOCAP_OOC = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();

                if (!string.IsNullOrEmpty(sOCAP_OOC))
                    this._llstSearchCondition.Add(Definition.CONDITION_SEARCH_KEY_OCAP_OOC, sOCAP_OOC);
            }

            //this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, "N");

            PROC_DataBinding();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._lang = MultiLanguageHandler.getInstance();

            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }

        public void InitialUserInfo()
        {
        }

        public void InitializeLayout()
        {
            //this.bsprData.ActiveSheet.FrozenColumnCount = 0;
            //this.bsprData.ActiveSheet.FrozenRowCount = 0;
            this.bsprData.ColFronzen = 0;
            this.bsprData.RowFronzen = 0;
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_OCAP_LIST_UC, Definition.BUTTONLIST_KEY_OCAP_LIST, this.sessionData);
            this.FunctionName = Definition.FUNC_KEY_OCAP_LIST;
            this.ApplyAuthory(this.bbtnList);
        }

        public void InitializeBSpread()
        {

            this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_OCAP_LIST_UC, false, Definition.PAGE_KEY_OCAP_LIST_UC_HEADER_KEY);
            //this.bsprData.UseHeadColor = false;
            //this.bsprData.UseAutoSort = false;
            //this.bsprData.UseFilter = false;
            //this.bsprData.UseEdit = false;
            this._Initialization.SetCheckColumnHeader(this.bsprData, 0);
            this.bsprData.IsCellCopy = true;
            
            //SPC-753 by Louisyou
            //this.bsprData.UseGeneralContextMenu = false;
        }


        #endregion

        #region ::: User Defined Method.

        private void PROC_DataBinding()
        {
            ////초기화
            //this._iSelectedReportRowIdx = -1;
            this._bRemovedDuplicatedRow = false;
            ParseCLOB pclob = null;
            try
            {
                _dsOCAPList = new DataSet();

                // SPC-648 Searching data in SPC Chart Model and OCAP/OOC List UI, Make Cancel Button in Progress Bar - 2011.10.28 by ANDREW KO
                // modified by enkim 2012.06.04 SPC-847

                //if (_wsAsync==null) _wsAsync = new eSPCWebServiceAsync();

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetOCAPList", new object[] { _llstSearchCondition.GetSerialData() });

                //_dsOCAPList = _wsAsync.GetOCAPListAsync(_llstSearchCondition.GetSerialData());

                EESProgressBar.CloseProgress(this);

                this.bsprData.ActiveSheet.RowCount = 0;
                this.bsprData.DataSource = null;
                
                //this.bsprData.ActiveSheet.FrozenColumnCount = 1;
                this.bsprData.ColFronzen = 1;

                if (objDataSet != null)
                {
                    _dsOCAPList = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    this.Focus();

                    return;
                }

                if (DataUtil.IsNullOrEmptyDataSet(_dsOCAPList))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    this.Focus();

                    return;
                }

                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                pclob = new ParseCLOB();

                LinkedList _llstData = new LinkedList();
                _llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
                DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
                LinkedList mllstContextType = CommonPageUtil.SetContextType(dsContextType);

                _dsOCAPList = pclob.DecompressOCAPDataTRXData(_dsOCAPList.Tables[0], mllstContextType, false);

                string strTempWhere = "";
                if (this._sFilterEQPID.Length > 0)
                {
                    strTempWhere += string.Format(" EQP_ID = '{0}'", this._sFilterEQPID);
                }

                //modified by enkim 2012.09.18 SPC-630
                if (this._sFilterModuleID.Length > 0)
                {
                    if (strTempWhere.Length > 0)
                        strTempWhere += string.Format(" AND MODULE_ID = '{0}'", this._sFilterModuleID);
                    else
                        strTempWhere += string.Format(" MODULE_ID = '{0}'", this._sFilterModuleID);
                }
                //modified end

                if (this._sFilterLot.Length > 0)
                {
                    if (strTempWhere.Length > 0)
                        strTempWhere += string.Format(" AND LOT_ID = '{0}'", this._sFilterLot);
                    else
                        strTempWhere += string.Format(" LOT_ID = '{0}'", this._sFilterLot);
                }

                if (this._sFilterSubStrate.Length > 0)
                {
                    if (strTempWhere.Length > 0)
                        strTempWhere += string.Format(" AND SUBSTRATE_ID = '{0}'", this._sFilterSubStrate);
                    else
                        strTempWhere += string.Format(" SUBSTRATE_ID = '{0}'", this._sFilterSubStrate);
                }

                if (this._sFilterRecipe.Length > 0)
                {
                    if (strTempWhere.Length > 0)
                        strTempWhere += string.Format(" AND RECIPE_ID = '{0}'", this._sFilterRecipe);
                    else
                        strTempWhere += string.Format(" RECIPE_ID = '{0}'", this._sFilterRecipe);
                }

                if (strTempWhere.Length > 0)
                {
                    DataRow[] drs = _dsOCAPList.Tables[0].Select(strTempWhere);

                    DataSet _dsOCAPList_Filter = _dsOCAPList.Clone();
                    foreach (DataRow drTemp in drs)
                    {
                        _dsOCAPList_Filter.Tables[0].ImportRow(drTemp);
                    }

                    if (DataUtil.IsNullOrEmptyDataSet(_dsOCAPList_Filter))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    this.bsprData.DataSource = _dsOCAPList_Filter;
                }
                else
                {
                    this.bsprData.DataSource = _dsOCAPList;
                }

                //if (_dsOCAPList.Tables[0].Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                //{
                //    List<string> rawIDs = new List<string>();
                //    foreach (DataRow dr in _dsOCAPList.Tables[0].Rows)
                //    {
                //        string rawid = dr[Definition.CHART_COLUMN.OCAP_RAWID].ToString();
                //        string[] ids = rawid.Split(';');
                //        foreach (string id in ids)
                //        {
                //            if (string.IsNullOrEmpty(id))
                //                continue;
                //            if (!rawIDs.Contains(id))
                //                rawIDs.Add(id);
                //        }
                //    }

                //    if (rawIDs.Count == 0)
                //        rawIDs.Add("");

                //    _dsOcapComment = _wsSPC.GetOCAPCommentList(rawIDs.ToArray());
                //}


                //modified end SPC-847
            }
            catch (Exception ex)
            {
                //modified by enkim 2012.06.04 SPC-847

                //LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                //MsgClose();

                //if (ex.Message.Equals("Cancel"))
                //{
                //    MSGHandler.DisplayMessage(MSGType.Information, "Canceled", null, null, true);
                //}
                //else if (ex.Message.Equals("Request timed out"))
                //{
                //    MSGHandler.DisplayMessage(MSGType.Warning, "Request timed out", null, null, true);
                //}
                //else
                //{
                //    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                //    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                //}
                //WebAsyncCall.FormActivate(this);

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

                //modified end SPC-847
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
                this._dsOCAPList.Dispose();
            }
        }
        #endregion

        #region ::: EventHandler


        private void bbtnList_ButtonClick(string name)
        {
            LinkedList _llstPopup = null;
            DataTable dtResource = null;
            try
            {
                if (!(this.bsprData.ActiveSheet.RowCount > 0))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_SEARCH_DATA));
                    return;
                }


                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    //SPC-748 By Louis 
                    //this.bsprData.Export(true);
                    BSpreadUtility bsprUtil = new BSpreadUtility();
                    bsprUtil.Export(this.bsprData, true);
                }
                else if (name.ToUpper().Equals("SELECT"))
                {
                    if (this.bsprData.ActiveSheet.SelectionCount > 0)
                    {
                        FarPoint.Win.Spread.Model.CellRange[] selections = this.bsprData.ActiveSheet.GetSelections();

                        for (int i = 0; i < selections[0].RowCount; i++)
                        {
                            this.bsprData.ActiveSheet.SetText(selections[0].Row + i, (int)enum_OCAPLIST.SPC_V_SELECT, "True");
                        }
                    }
                }
                else if (name.ToUpper().Equals("UNSELECT"))
                {
                    if (this.bsprData.ActiveSheet.SelectionCount > 0)
                    {
                        FarPoint.Win.Spread.Model.CellRange[] selections = this.bsprData.ActiveSheet.GetSelections();

                        for (int i = 0; i < selections[0].RowCount; i++)
                        {
                            this.bsprData.ActiveSheet.SetText(selections[0].Row + i, (int)enum_OCAPLIST.SPC_V_SELECT, "False");
                        }
                    }
                }
                else if (name.ToUpper() == Definition.ButtonKey.VIEW || name.ToUpper() == Definition.ButtonKey.OCAP_MODIFY)
                {
                    ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)enum_OCAPLIST.SPC_V_SELECT);
                    if (alCheckRowIndex.Count == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                        return;
                    }

                    if (name.ToUpper() == Definition.ButtonKey.VIEW)
                    {
                        this.ClickButtonOCAP(enum_PopupType.View, alCheckRowIndex);
                    }
                    else if (name.ToUpper() == Definition.ButtonKey.OCAP_MODIFY)
                    {
                        this.ClickButtonOCAP(enum_PopupType.Modify, alCheckRowIndex);
                    }

                }
                else if (name.ToUpper() == "SPC_REMOVE_DUPLICATE")
                {
                    if (this._bRemovedDuplicatedRow)
                    {
                        this.bsprData.ActiveSheet.Rows[0, this.bsprData.ActiveSheet.RowCount - 1].Visible = true;
                        this._bRemovedDuplicatedRow = false;
                    }
                    else
                    {
                        ArrayList arrDuplicatedRow = new ArrayList();
                        for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                        {
                            string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[i, (int)enum_OCAPLIST.MODEL_CONFIG_RAWID].Text;
                            if (arrDuplicatedRow.Contains(strModelConfigRawID))
                            {
                                this.bsprData.ActiveSheet.Rows[i].Visible = false;
                                if (this.bsprData.ActiveSheet.Cells[i, (int)enum_OCAPLIST.SPC_V_SELECT].Text == "True")
                                {
                                    this.bsprData.ActiveSheet.Cells[i, (int)enum_OCAPLIST.SPC_V_SELECT].Text = "False";
                                }
                            }
                            else
                            {
                                arrDuplicatedRow.Add(strModelConfigRawID);
                            }
                        }
                        this._bRemovedDuplicatedRow = true;
                    }
                }
                else
                {
                    ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)enum_OCAPLIST.SPC_V_SELECT);
                    if (alCheckRowIndex.Count < 1 || alCheckRowIndex.Count > 1)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                        return;
                    }

                    _ChartVariable = new ChartInterface();
                    int iRowIndex = (int)alCheckRowIndex[0];
                    if (iRowIndex < 0) return;


                    string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.MODEL_CONFIG_RAWID].Text;
                    string strTime = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.TIME].Text;
                    string strocaprawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.OCAP_RAWID].Text;

                    if (this._llstSearchCondition.Contains(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD))
                    {
                        this._llstSearchCondition.Remove(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD);
                        this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PARAM_TYPE_CD].Text);
                    }
                    else
                    {
                        this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PARAM_TYPE_CD].Text);
                    }


                    DataRow drCurrent = this._dsOCAPList.Tables[0].Rows[iRowIndex];

                    _llstPopup = CommonPageUtil.GetOCAPParameter(this._dsOCAPList.Tables[0], iRowIndex);
                    _llstPopup[Definition.CONDITION_KEY_TIME] = strTime;
                    dtResource = GetChartData(_llstPopup, strModelConfigRawID, iRowIndex);

                    List<string> rawIDs = new List<string>();
                    LinkedList llstTmpOcapComment = new LinkedList();
                    if (dtResource.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        bool bRawOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST);
                        bool bMeanOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST);
                        bool bStdOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST);
                        bool bRangeOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST);
                        bool bMaOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST);
                        bool bMsOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST);
                        bool bMrOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST);
                        bool bEwmaMeanOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST);
                        bool bEwmaStdOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST);
                        bool bEwmaRangeOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST);

                        foreach (DataRow dr in dtResource.Rows)
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
                        }

                        if (rawIDs.Count == 0)
                            rawIDs.Add("");

                        
                        llstTmpOcapComment.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, rawIDs.ToArray());

                        if (this._llstSearchCondition[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                        {
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString())).ToString(Definition.DATETIME_FORMAT_MS));
                        }
                        if (this._llstSearchCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                        {
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString())).ToString(Definition.DATETIME_FORMAT_MS));
                        }

                        byte[] baData = llstTmpOcapComment.GetSerialData();

                        _dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);
                    }

                    if (DataUtil.IsNullOrEmptyDataTable(dtResource))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    if (lineName.ContainsKey(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.LINE].Text))
                    {
                        _ChartVariable.LINE = lineName[this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.LINE].Text];
                    }
                    _ChartVariable.AREA = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.AREA].Text;
                    _ChartVariable.SPC_MODEL = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.SPC_MODEL_NAME].Text;
                    _ChartVariable.PARAM_ALIAS = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PARAM_ALIAS].Text;
                    _ChartVariable.OPERATION_ID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.OPERATION_ID].Text;
                    _ChartVariable.PRODUCT_ID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PRODUCT_ID].Text;
                    _ChartVariable.llstInfoCondition = _llstPopup;
                    _ChartVariable.DEFAULT_CHART = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.DEFAULT_CHART_LIST].Text;
                    //_ChartVariable.DEFAULT_CHART = this._lang.GetVariable(Definition.VARIABLE_SPC_INITIAL_DISPLAY_CHART);
                    _ChartVariable.complex_yn = Definition.VARIABLE_Y;
                    _ChartVariable.MODEL_CONFIG_RAWID = strModelConfigRawID;
                    _ChartVariable.MAIN_YN = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.MAIN_YN].Text;
                    _ChartVariable.dtResource = dtResource;
                    //_ChartVariable.dtParamData = dtResource;
                    _ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.OCAP;
                    //modified by enkim 2010.08.06 Gemini P3-2843
                    //발생일 기준으로 앞뒤 7일을 조회한다.
                    //_ChartVariable.dateTimeStart = DateTime.Parse(strTime).AddDays(-7);
                    //if (DateTime.Parse(strTime).AddDays(7) > DateTime.Parse(CommonPageUtil.ToDayStart()))
                    //    _ChartVariable.dateTimeEnd = DateTime.Parse(CommonPageUtil.ToDayEnd());
                    //else
                    //    _ChartVariable.dateTimeEnd = DateTime.Parse(strTime).AddDays(7);

                    if (name != Definition.ButtonKey.VIEW_CHART)
                    {
                        _ChartVariable.dateTimeStart = DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                        _ChartVariable.dateTimeEnd = DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                    }
                    
                    //modified end
                    _ChartVariable.llstDTSelectCondition = this._llstDTSelectCondition;
                    _ChartVariable.OCAPRawID = strocaprawid;
                    this._lineRawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.LINE].Text;
                    this._areaRawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.AREA_RAWID].Text;

                    if (name.ToUpper() == Definition.ButtonKey.VIEW_CHART)
                    {
                        //this._eqpModel = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.].Text;
                        this.ClickButtonChartView(llstTmpOcapComment);
                    }
                }

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {
                if (dtResource != null) dtResource.Dispose();

                _llstPopup = null;
                _ChartVariable = null;
            }
        }

        private void ClickButtonOCAP(enum_PopupType _popupType, ArrayList arrCheckedRowIndex)
        {
            //SPC-1300, KBLEE, START
            ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)enum_OCAPLIST.SPC_V_SELECT);
            int iRowIndex = (int)alCheckRowIndex[0];
            //SPC-1300, KBLEE, END

            DataSet dsTemp = this._bSpreadUtil.GetSelectedDataSet_V2(this.bsprData, (int)enum_OCAPLIST.SPC_V_SELECT);
            //string ocap_rawid = this.bsprData.ActiveSheet.Cells[0, (int)enum_OCAPLIST.OCAP_RAWID].Text;

            OCAPDetailsPopup popupOCAP = new OCAPDetailsPopup();
            //popupOCAP.ChartVariable = _ChartVariable;            
            //popupOCAP.ChartVariable.OCAPRawID = ocap_rawid;            
            popupOCAP.PopUpType = _popupType;
            popupOCAP.SessionData = this.sessionData;
            popupOCAP.DSSelectedOCAP = dsTemp;
            popupOCAP.llstCondition = this._llstSearchCondition;
            popupOCAP.ISMulti = true;
            popupOCAP.URL = this.URL;
            popupOCAP.LINE = lineName[this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.LINE].Text]; //SPC-1300, KBLEE
            //popupOCAP.LINE_RAWID = this._lineRawid;
            //popupOCAP.AREA_RAWID = this._areaRawid;
            //popupOCAP.GROUP_NAME = this._llGroup[this._ChartVariable.SPC_MODEL].ToString();
            popupOCAP.InitializePopup();

            popupOCAP.linkTraceDataViewEventOcap -= new OCAPDetailsPopup.LinkTraceDataViewEventHandlerOcap(chartViewPop_linkTraceDataViewEventPopup);
            popupOCAP.linkTraceDataViewEventOcap += new OCAPDetailsPopup.LinkTraceDataViewEventHandlerOcap(chartViewPop_linkTraceDataViewEventPopup);

            if (this._dsOcapComment != null && this._dsOcapComment.Tables.Count > 0)
                popupOCAP.dtComment = this._dsOcapComment.Tables[0];

            DialogResult result = popupOCAP.ShowDialog(this);
            if (result == DialogResult.OK)
            {
            }
        }

        private void ClickButtonChartView(LinkedList lstTmpOcap)
        {

            if (string.IsNullOrEmpty(_ChartVariable.DEFAULT_CHART))
            {
                MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                return;
            }

            if (_ChartVariable.dtResource == null)
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                return;
            }

            ChartViewPopup chartViewPop = new ChartViewPopup();
            chartViewPop.ChartVariable = _ChartVariable;
            chartViewPop.LlstTmpOcapComment_Popup = lstTmpOcap;
            chartViewPop.URL = this.URL;
            chartViewPop.LINE_RAWID = this._lineRawid;
            chartViewPop.AREA_RAWID = this._areaRawid;
            chartViewPop.GROUP_NAME = this._llGroup[_ChartVariable.SPC_MODEL].ToString();
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.ParamTypeCD = this.llstSearchCondition[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString();
            if(_dsOcapComment != null && _dsOcapComment.Tables.Count > 0)
                chartViewPop.ChartVariable.dtOCAP = _dsOcapComment.Tables[0];
            chartViewPop.InitializePopup();

            chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
            chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

            DialogResult result = chartViewPop.ShowDialog(this);

            this._llGroup[_ChartVariable.SPC_MODEL] = chartViewPop.GROUP_NAME;
            if (result == DialogResult.OK)
            {

            }

        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            this.SendMessage("TRACE_DATA", true, llstTraceLinkData, 0);
        }


        /// <summary>
        /// Create Chart DataSet
        /// </summary>
        /// <param name="_llstPopup"></param>
        /// <param name="strModelConfigRawID"></param>
        /// <returns></returns>
        private DataTable GetChartData(LinkedList _llstPopup, string strModelConfigRawID, int iRowIndex)
        {

            DataSet _ds = null;
            DataTable _dtChartData = new DataTable();
            LinkedList _llstSearch = new LinkedList();
            try
            {

                _llstDTSelectCondition = new LinkedList();
                _llstDTSelectCondition.Clear();
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.OPERATION_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.OPERATION_ID].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.PRODUCT_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PRODUCT_ID].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.PARAM_ALIAS, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.PARAM_ALIAS].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.EQP_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_OCAPLIST.EQP_ID].Text));

                DateTime dtStart = DateTime.Parse(_llstPopup[Definition.CONDITION_KEY_TIME].ToString());
                dtStart = dtStart.AddHours(-12.0);

                DateTime dtEnd = DateTime.Parse(_llstPopup[Definition.CONDITION_KEY_TIME].ToString());
                dtEnd = dtEnd.AddHours(12.0);

                _llstSearch.Add(Definition.CONDITION_KEY_START_DTTS, dtStart.ToString("yyyy-MM-dd HH:mm:ss"));
                _llstSearch.Add(Definition.CONDITION_KEY_END_DTTS, dtEnd.ToString("yyyy-MM-dd HH:mm:ss"));

                _ChartVariable.dateTimeStart = dtStart;
                _ChartVariable.dateTimeEnd = dtEnd;

                //_llstSearch.Add(Definition.CONDITION_KEY_START_DTTS, _ComUtil.NVL(this._llstSearchCondition[Definition.CONDITION_KEY_START_DTTS]));
                //_llstSearch.Add(Definition.CONDITION_KEY_END_DTTS, _ComUtil.NVL(this._llstSearchCondition[Definition.CONDITION_KEY_END_DTTS]));
                _llstSearch.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, strModelConfigRawID);

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetSPCControlChartData", new object[] { _llstSearch.GetSerialData() });

                EESProgressBar.CloseProgress(this);

                //_ds = _wsSPC.GetSPCControlChartData(_llstSearch.GetSerialData());
                if (objDataSet != null)
                {
                    _ds = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return _dtChartData;
                }

                if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    this.MsgShow("Drawing Chart... Can't Cancel!!!");
                    _llstSearch.Clear();
                    if (this._llstSearchCondition.Contains(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD))
                    {
                        if (this._llstSearchCondition[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString() == "MET")
                            _dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearch, false);
                        else
                            _dtChartData = CommonPageUtil.CLOBnBLOBParsingRaw(_ds, _llstSearch, false, false);
                    }
                    else
                    {
                        _dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearch, false);
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
            }
            finally
            {
                if (_ds != null)
                {
                    _ds.Dispose();
                }
            }
            return _dtChartData;
        }


        #endregion

        private void bsprData_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.bsprData.ActiveSheet.RowCount > 0)
            {
                if (e.ColumnHeader)
                {
                    if (e.Column == (int)enum_OCAPLIST.SPC_V_SELECT)
                    {
                        FarPoint.Win.Spread.CellType.ICellType ct = this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].CellType;
                        if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                        {
                            this._Initialization.SetCheckColumnHeaderStatus(this.bsprData, (int)enum_OCAPLIST.SPC_V_SELECT);
                        }
                    }
                }
            }
        }

    }
}
