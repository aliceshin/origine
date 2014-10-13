using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;
using BISTel.PeakPerformance.Client.DataAsyncHandler;

using FarPoint.Win.Spread;

namespace BISTel.eSPC.Page.ATT.Compare
{
    public partial class SPCModelListUC : BasePageUCtrl
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private Initialization _initialization = null;
        private SPCModelListController controller = null;
        private string itemKey = string.Empty;

        MultiLanguageHandler _lang;

        SortedList alSelect;
        private bool _bUseComma;

        public SPCModelListUC()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            InitializePage();

            DataTable dtSPCModelList = this.GetSPCModelList(llCondition);

            

            if(dtSPCModelList == null)
            {
                this.MsgClose();
                this.InitializePage();
                return;
            }
            EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

            if(dtSPCModelList.Rows.Count > 0)
            {
                AddHeader(dtSPCModelList);

                this.bsprData.DataSet = dtSPCModelList;

                this.bsprData.ActiveSheet.Columns[0].Locked = false;
            }
            EESProgressBar.CloseProgress(this);
        }

        public void InitializePage(string functionName, string pageKey, string itemKey, SessionData session)
        {

            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.itemKey = itemKey;
            this.sessionData = session;


            this.InitializePage();
        }

        public void InitializePage()
        {
            this._lang = MultiLanguageHandler.getInstance();

            InitializeVariables();

            InitializeBspr();

            InitializeDataButton();

            InitializeController();
        }

        private void InitializeVariables()
        {
            if(this._ws == null)
                this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            
            if(this._initialization == null)
            {
                this._initialization = new Initialization();
                this._initialization.InitializePath();
            }

            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        private void InitializeBspr()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.SetFilterVisible(false);
        }

        private void InitializeDataButton()
        {
            this._initialization.InitializeButtonList(this.bbtnList, ref bsprData, this.KeyOfPage, itemKey, this.sessionData);

            this.ApplyAuthory(this.bbtnList);
        }

        private void InitializeController()
        {
            Control parent = this.Parent;
            while(!(parent is SPCModelCompareUC))
            {
                parent = parent.Parent;
            }

            controller = new SPCModelListController(this._ws, parent);
        }

        private void AddHeader(DataTable dtSPCModelList)
        {
            this.bsprData.AddHead(0, "SELECT", "SELECT", 50, 50, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false,
                                      true);
            this.controller.columnIndex.Add("SELECT", 0);
            for(int i=1; i<=dtSPCModelList.Columns.Count; i++)
            {
                this.bsprData.AddHead(i, dtSPCModelList.Columns[i - 1].ColumnName, dtSPCModelList.Columns[i - 1].ColumnName, 150, 300, null, null,
                                      null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
                this.controller.columnIndex.Add(dtSPCModelList.Columns[i - 1].ColumnName, i);
            }
            this.bsprData.AddHeadComplete();
        }

        private void bbtnList_ButtonClick(string name)
        {
            if(this.bsprData.ActiveSheet.Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_PARAMETER", null, null);
                return;
            }

            if(name == "FILTER")
            {
                Filter();   
            }
            else if(name == "SPC_SHOW_EXCLUDED")
            {
                ShowExcluded();
            }
            else if(name == "SPC_MODEL_COMPARE")
            {
                Compare();
            }
        }

        private DataTable GetSPCModelList(LinkedList llCondition)
        {
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLineRawID = string.Empty;
            string sLine = string.Empty;
            string sAreaRawID = string.Empty;
            string sArea = string.Empty;
            string sEqpModel = string.Empty;
            string sParamAlias = string.Empty;
            string sParamTypeCd = string.Empty;
            string sSPC_MODEL_LEVEL = string.Empty;

            DataTable dtresult = null;
            DataSet dsData = null;

            try
            {
                LinkedList llstCondition = new LinkedList();
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_ATT_MODEL_LEVEL");
                llstCondition.Add(Definition.CONDITION_KEY_USE_YN, "Y");
                //llstCondition.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

                DataSet ds = _ws.GetATTCodeData(llstCondition.GetSerialData());

                if (ds != null && ds.Tables[0].Rows[0]["CODE"].ToString().ToUpper() == "AREA")
                {
                    sSPC_MODEL_LEVEL = "AREA";
                }
                else
                {
                    sSPC_MODEL_LEVEL = "EQP MODEL";
                }

                if (llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
                {
                    sSite =
                       ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                if (llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
                {
                    sFab =
                       ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                if (llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
                {
                    sLineRawID =
                        ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                    sLine = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                }

                if (llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
                {
                    sAreaRawID =
                        ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                    sArea = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }

                if (llCondition[Definition.DynamicCondition_Search_key.EQPMODEL] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows.Count > 0)
                {
                    sEqpModel =
                        ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }

                if (llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows.Count > 0)
                {
                    sParamAlias =
                        ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }

                if (llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE] != null
                    && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows.Count > 0)
                {
                    sParamTypeCd =
                        ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }

                if (string.IsNullOrEmpty(sLineRawID) || (sSPC_MODEL_LEVEL == "AREA" && string.IsNullOrEmpty(sAreaRawID)) || (sSPC_MODEL_LEVEL == "EQP MODEL" && string.IsNullOrEmpty(sEqpModel)) ||
                    string.IsNullOrEmpty(sParamAlias) || string.IsNullOrEmpty(sParamTypeCd))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_PARAM", null, null);
                    return null;
                }

                if (!this.ApplyAuthory(this.bbtnList, sSite, sFab, sLine, sAreaRawID))
                {
                    return null;
                }

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_ws, "GetATTSPCModelList", new object[] { sLineRawID, sAreaRawID, sEqpModel, sParamAlias, sParamTypeCd, _bUseComma });

                EESProgressBar.CloseProgress(this);

                if (objDataSet != null)
                {
                    dsData = (DataSet)objDataSet;
                    dtresult = MergeWithContextTable(dsData.Tables[BISTel.eSPC.Common.TABLE.CHART_VW_SPC], dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC]);
                    this.controller.dtSPCModelList = dtresult;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
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
            }
            return dtresult;
        }

        private static DataTable MergeWithContextTable(DataTable dt, DataTable contextTable)
        {
            DataTable dtReturn = new DataTable();

            foreach (DataColumn dc in dt.Columns)
            {
                dtReturn.Columns.Add(dc.ColumnName, dc.DataType);
            }
            foreach (DataRow dr in contextTable.Rows)
            {
                string columnName = dr["CONTEXT_KEY_NAME"].ToString();
                if (!dtReturn.Columns.Contains(columnName))
                    dtReturn.Columns.Add(columnName);
            }

            dtReturn.Merge(dt, false, MissingSchemaAction.Ignore);

            foreach (DataRow dr in dtReturn.Rows)
            {
                foreach (DataRow drContext in contextTable.Select("MODEL_CONFIG_RAWID = '" + dr["CHART_ID"] + "'"))
                {
                    dr[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext["CONTEXT_VALUE"].ToString();
                }
            }

            return dtReturn;
        }

        private void Filter()
        {
            this.controller.Filter(this.bsprData);
        }

        private void ShowExcluded()
        {
            foreach(Row r in bsprData.ActiveSheet.Rows)
            {
                r.Visible = true;
            }
        }

        private void Compare()
        {
            this.MsgShow(COMMON_MSG.Query_Data);

            ArrayList checkedRows = this.bsprData.GetCheckedList(this.controller.columnIndex["SELECT"]);

            if(checkedRows.Count < 1)
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Information, "Please select chart IDs");
                return;
            }

            this.controller.Compare(this.bsprData);

            this.MsgClose();
        }

        private void bsprData_MouseDown(object sender, MouseEventArgs e)
        {
            alSelect = this.bsprData.GetSelectedRows();
        }

        private void bsprData_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (alSelect.Count > 0)
            {
                if (!(bool)this.bsprData.GetCellValue(e.Row, 0))
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "False");
                    }
                }
                else
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "True");
                    }
                }
            }
            alSelect.Clear();
        }
    }
}
