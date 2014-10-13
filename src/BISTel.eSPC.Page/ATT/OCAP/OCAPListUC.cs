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
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Common;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.ATT.OCAP
{
    public partial class OCAPListUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstSearchCondition = new LinkedList();
        LinkedList _llstDTSelectCondition = new LinkedList();
        DataSet _dsOCAPList = null;
        DataSet _dsOcapComment = null;

        BISTel.eSPC.Common.ATT.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil = null;


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

        //SPC-1292, KBLEE, START
        private string _lineRawid = string.Empty;
        private string _areaRawid = string.Empty;
        private string _eqpModel = string.Empty;

        private LinkedList _llGroup = null;
        //SPC-1292, KBLEE, END

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

            this._llGroup = new LinkedList(); //SPC-1292, KBLEE

            if (llstCondition.Contains(Definition.DynamicCondition_Search_key.CHART_ID))
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CHART_ID];
                this._llstSearchCondition.Add(Definition.DynamicCondition_Search_key.CHART_ID, dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString());

                string sChartID = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                LinkedList lnkTemp = new LinkedList();
                DataSet dsTempChartID = new DataSet();
                lnkTemp.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sChartID);
                dsTempChartID = this._wsSPC.GetATTSPCModelDatabyChartID(lnkTemp.GetSerialData());

                //SPC-1292, KBLEE, START
                this._llGroup.Clear();

                if (dsTempChartID.Tables.Contains(BISTel.eSPC.Common.TABLE.MODEL_GROUP_ATT_MST_SPC) && dsTempChartID.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows.Count > 0 && dsTempChartID.Tables[BISTel.eSPC.Common.TABLE.MODEL_GROUP_ATT_MST_SPC].Rows.Count > 0)
                {
                    this._llGroup.Add(dsTempChartID.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString(), dsTempChartID.Tables[BISTel.eSPC.Common.TABLE.MODEL_GROUP_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString());
                }
                //SPC-1292, KBLEE, END

                if (dsTempChartID != null && dsTempChartID.Tables != null && dsTempChartID.Tables.Count == 3 && dsTempChartID.Tables[0].Rows.Count > 0 && dsTempChartID.Tables[1].Rows.Count > 0)
                {
                    string site = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.SITE].ToString();
                    string fab = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.FAB].ToString();
                    string line = dsTempChartID.Tables[0].Rows[0][Definition.DynamicCondition_Condition_key.LINE].ToString();
                    string area = dsTempChartID.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.AREA].ToString();

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

                string strModelRawID = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL], Definition.DynamicCondition_Condition_key.VALUEDATA);
                if (!string.IsNullOrEmpty(strModelRawID))
                    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, strModelRawID);

                string area = string.Empty;
                if (strArea != null)
                {
                    string[] areas = strArea.Split(',');
                    area = areas[0].Trim('\'');
                }

                //SPC-1292, KBLEE, START
                this._llGroup.Clear(); 

                DataTable dtSPCModel = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL];

                foreach (DataRow dr in dtSPCModel.Rows)
                {
                    this._llGroup.Add(dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString(), dr[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString());
                }
                //SPC-1292, KBLEE, END

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
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL_OR_CHART_ID",null, null);
                base.ApplyAuthory(this.bbtnList, null, null, null, null);
                return;
            }

            DateTime dtFrom = new DateTime();
            DateTime dtTo = new DateTime();
            if (llstCondition[Definition.DynamicCondition_Search_key.FROMDATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.FROMDATE];
                dtFrom = (DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA];
                sStartTime = dtFrom.ToString(Definition.DATETIME_FORMAT);

            }
            if (llstCondition[Definition.DynamicCondition_Search_key.TODATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.TODATE];
                dtTo = ((DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA]);
                sEndTime = dtTo.ToString(Definition.DATETIME_FORMAT);

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
            this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_ATT_OCAP_LIST_UC, Definition.BUTTONLIST_KEY_ATT_OCAP_LIST, this.sessionData);
            this.FunctionName = Definition.FUNC_KEY_ATT_OCAP_LIST;
            this.ApplyAuthory(this.bbtnList);
        }

        public void InitializeBSpread()
        {
            this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_ATT_OCAP_LIST_UC, false, Definition.PAGE_KEY_OCAP_LIST_UC_HEADER_KEY);
            this._Initialization.SetCheckColumnHeader(this.bsprData, 0);
            this.bsprData.IsCellCopy = true;
        }


        #endregion

        #region ::: User Defined Method.

        private void PROC_DataBinding()
        {
            this._bRemovedDuplicatedRow = false;
            ParseCLOB pclob = null;
            try
            {
                _dsOCAPList = new DataSet();

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetATTOCAPList", new object[] { _llstSearchCondition.GetSerialData() });

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
                    return;
                }

                if (DataUtil.IsNullOrEmptyDataSet(_dsOCAPList))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
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

                if (this._sFilterModuleID.Length > 0)
                {
                    if (strTempWhere.Length > 0)
                        strTempWhere += string.Format(" AND MODULE_ID = '{0}'", this._sFilterModuleID);
                    else
                        strTempWhere += string.Format(" MODULE_ID = '{0}'", this._sFilterModuleID);
                }

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
                            this.bsprData.ActiveSheet.SetText(selections[0].Row + i, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT, "True");
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
                            this.bsprData.ActiveSheet.SetText(selections[0].Row + i, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT, "False");
                        }
                    }
                }
                else if (name.ToUpper() == Definition.ButtonKey.VIEW || name.ToUpper() == Definition.ButtonKey.OCAP_MODIFY)
                {
                    ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT);
                    if (alCheckRowIndex.Count == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                        return;
                    }

                    if (name.ToUpper() == Definition.ButtonKey.VIEW)
                    {
                        this.ClickButtonOCAP(BISTel.eSPC.Common.enum_PopupType.View, alCheckRowIndex);
                    }
                    else if (name.ToUpper() == Definition.ButtonKey.OCAP_MODIFY)
                    {
                        this.ClickButtonOCAP(BISTel.eSPC.Common.enum_PopupType.Modify, alCheckRowIndex);
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
                            string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPLIST.MODEL_CONFIG_RAWID].Text;
                            if (arrDuplicatedRow.Contains(strModelConfigRawID))
                            {
                                this.bsprData.ActiveSheet.Rows[i].Visible = false;
                                if (this.bsprData.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT].Text == "True")
                                {
                                    this.bsprData.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT].Text = "False";
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
                    ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT);
                    if (alCheckRowIndex.Count < 1 || alCheckRowIndex.Count > 1)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                        return;
                    }

                    _ChartVariable = new ChartInterface();
                    int iRowIndex = (int)alCheckRowIndex[0];
                    if (iRowIndex < 0) return;


                    string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.MODEL_CONFIG_RAWID].Text;
                    string strTime = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.TIME].Text;
                    string strocaprawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.OCAP_RAWID].Text;

                    //SPC-1292, KBLEE, START
                    if (this._llstSearchCondition.Contains(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD))
                    {
                        this._llstSearchCondition.Remove(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD);
                        this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PARAM_TYPE_CD].Text);
                    }
                    else
                    {
                        this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PARAM_TYPE_CD].Text);
                    }
                    //SPC-1292, KBLEE, END

                    DataRow drCurrent = this._dsOCAPList.Tables[0].Rows[iRowIndex];

                    _llstPopup = CommonPageUtil.GetOCAPParameter(this._dsOCAPList.Tables[0], iRowIndex);
                    dtResource = GetChartData(_llstPopup, strModelConfigRawID, iRowIndex);

                    List<string> rawIDs = new List<string>();
                    if (dtResource.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        bool bPOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                        bool bPNOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                        bool bCOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                        bool bUOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

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

                    if (lineName.ContainsKey(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.LINE].Text))
                    {
                        _ChartVariable.LINE = lineName[this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.LINE].Text];
                    }
                    _ChartVariable.AREA = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.AREA].Text;
                    _ChartVariable.SPC_MODEL = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_MODEL_NAME].Text;
                    _ChartVariable.PARAM_ALIAS = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PARAM_ALIAS].Text;
                    _ChartVariable.OPERATION_ID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.OPERATION_ID].Text;
                    _ChartVariable.PRODUCT_ID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PRODUCT_ID].Text;
                    _ChartVariable.llstInfoCondition = _llstPopup;
                    _ChartVariable.DEFAULT_CHART = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.DEFAULT_CHART_LIST].Text;
                    _ChartVariable.complex_yn = Definition.VARIABLE_Y;
                    _ChartVariable.MODEL_CONFIG_RAWID = strModelConfigRawID;
                    _ChartVariable.MAIN_YN = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.MAIN_YN].Text;
                    _ChartVariable.dtResource = dtResource;
                    _ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.OCAP;
                    _ChartVariable.dateTimeStart = DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                    _ChartVariable.dateTimeEnd = DateTime.Parse(this._llstSearchCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                    _ChartVariable.llstDTSelectCondition = this._llstDTSelectCondition;
                    _ChartVariable.OCAPRawID = strocaprawid;

                    this._lineRawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.LINE].Text;
                    this._areaRawid = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.AREA_RAWID].Text;

                    if (name.ToUpper() == Definition.ButtonKey.VIEW_CHART)
                    {
                        this.ClickButtonChartView();
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

        private void ClickButtonOCAP(BISTel.eSPC.Common.enum_PopupType _popupType, ArrayList arrCheckedRowIndex)
        {

            DataSet dsTemp = this._bSpreadUtil.GetSelectedDataSet_V2(this.bsprData, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT);

            OCAPDetailsPopup popupOCAP = new OCAPDetailsPopup();
            popupOCAP.PopUpType = _popupType;
            popupOCAP.SessionData = this.sessionData;
            popupOCAP.DSSelectedOCAP = dsTemp;
            popupOCAP.llstCondition = this._llstSearchCondition;
            popupOCAP.ISMulti = true;
            popupOCAP.URL = this.URL;
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

        private void ClickButtonChartView()
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
            chartViewPop.URL = this.URL;
            //SPC-1292, KBLEE, START
            chartViewPop.LINE_RAWID = this._lineRawid;
            chartViewPop.AREA_RAWID = this._areaRawid;
            chartViewPop.GROUP_NAME = this._llGroup[_ChartVariable.SPC_MODEL].ToString();
            //SPC-1292, KBLEE, END
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.ParamTypeCD = "ATT";
            if (_dsOcapComment != null && _dsOcapComment.Tables.Count > 0)
                chartViewPop.ChartVariable.dtOCAP = _dsOcapComment.Tables[0];
            chartViewPop.InitializePopup();

            chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
            chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

            DialogResult result = chartViewPop.ShowDialog(this);
            this._llGroup[_ChartVariable.SPC_MODEL] = chartViewPop.GROUP_NAME; //SPC-1292, KBLEE
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


                _llstDTSelectCondition.Clear();
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.OPERATION_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.OPERATION_ID].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.PRODUCT_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PRODUCT_ID].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.PARAM_ALIAS, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.PARAM_ALIAS].Text));
                _llstDTSelectCondition.Add(Definition.CHART_COLUMN.EQP_ID, CommonPageUtil.GetConCatString(this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_OCAPLIST.EQP_ID].Text));

                _llstSearch.Add(Definition.CONDITION_KEY_START_DTTS, _ComUtil.NVL(this._llstSearchCondition[Definition.CONDITION_KEY_START_DTTS]));
                _llstSearch.Add(Definition.CONDITION_KEY_END_DTTS, _ComUtil.NVL(this._llstSearchCondition[Definition.CONDITION_KEY_END_DTTS]));
                _llstSearch.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, strModelConfigRawID);

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetATTSPCControlChartData", new object[] { _llstSearch.GetSerialData() });

                EESProgressBar.CloseProgress(this);

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
                    _dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearch, false, false, false);
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
                    if (e.Column == (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT)
                    {
                        FarPoint.Win.Spread.CellType.ICellType ct = this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].CellType;
                        if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                        {
                            this._Initialization.SetCheckColumnHeaderStatus(this.bsprData, (int)BISTel.eSPC.Common.enum_OCAPLIST.SPC_V_SELECT);
                        }
                    }
                }
            }
        }

    }
}
