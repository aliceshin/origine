using System;
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

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class SPCModelUC2 : BasePageUCtrl
    {
        public SPCModelUCController controller = null;
        public bool AddSelectColumn = false;
        protected string itemKey = string.Empty;
        private Initialization initialization = null;
        public bool IsVisibleBButtonList = false;
        private LinkedList lastestCondition = null;

        eSPCWebService.eSPCWebService _ws = null;
        MultiLanguageHandler _lang;

        private DataSet _ds = new DataSet();

        enum iColIdx
        {
            SELECT
        }

        Dictionary<string, BISTel.eSPC.Page.Common.SPCModel> spcModels = null;
        private bool _bUseComma;

        public BISTel.eSPC.Page.Common.SPCModel[] SpcModels
        {
            get
            {
                BISTel.eSPC.Page.Common.SPCModel[] models = new BISTel.eSPC.Page.Common.SPCModel[spcModels.Count];
                spcModels.Values.CopyTo(models, 0);
                return models;
            }
        }     

        public BSpread bSpread
        {
            get { return bSpread1; }
            set { bSpread1 = value; }
        }

        public SPCModelUC2()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            try
            {
                if (llCondition[Definition.DynamicCondition_Search_key.SPCMODEL] == null
                    || ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SPCMODEL]).Rows.Count == 0)
                {
                    InitializePage();
                    return;
                }

                lastestCondition = llCondition;

                DataTable spcmodels = (DataTable)llCondition[Definition.DynamicCondition_Search_key.SPCMODEL];
                List<string> modelRawids = new List<string>();
                foreach (DataRow dr in spcmodels.Rows)
                {
                    modelRawids.Add(dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString());
                }

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_ws, "GetATTSPCModelsData", new object[] { modelRawids.ToArray() , _bUseComma});

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

                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                spcModels = new Dictionary<string, BISTel.eSPC.Page.Common.SPCModel>();

                foreach (DataRow dr in _ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows)
                {
                    BISTel.eSPC.Page.Common.SPCModel spcModel = new BISTel.eSPC.Page.Common.SPCModel
                    {
                        SPCModelRawID = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString(),
                        SPCModelName = dr[BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString(),
                    };
                    spcModels.Add(spcModel.SPCModelRawID, spcModel);
                }

                foreach (var kvp in spcModels)
                {
                    DataRow[] drs = _ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Select(BISTel.eSPC.Common.COLUMN.MODEL_RAWID + " = '" + kvp.Key + "'");
                    if (drs.Length == 0)
                        continue;

                    kvp.Value.SubModels = new List<BISTel.eSPC.Page.Common.SPCModel>();

                    foreach (DataRow dr in drs)
                    {
                        if (dr[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString().ToUpper() == "Y")
                        {
                            kvp.Value.ChartID = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                            kvp.Value.Version = dr[BISTel.eSPC.Common.COLUMN.VERSION].ToString();
                            kvp.Value.IsMainModel = true;
                            continue;
                        }

                        BISTel.eSPC.Page.Common.SPCModel spcModel = new BISTel.eSPC.Page.Common.SPCModel
                        {
                            ChartID = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString(),
                            SPCModelRawID = kvp.Value.SPCModelRawID,
                            SPCModelName = kvp.Value.SPCModelName,
                            Version = dr[BISTel.eSPC.Common.COLUMN.VERSION].ToString(),
                            IsMainModel = false,
                            ParamType = kvp.Value.ParamType
                        };
                        kvp.Value.SubModels.Add(spcModel);
                    }
                }


                if (DSUtil.GetResultInt(_ds) != 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(_ds));
                    InitializePage();
                    return;
                }

                BindingSpread();
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
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
            }
        }

        public void InitializePage(string functionName, string pageKey, SessionData session)
        {
            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.sessionData = session;

            this.InitializePage();
        }

        public void InitializePage(string functionName, string pageKey, string itemKey, SessionData session)
        {
            this.itemKey = itemKey;

            IsVisibleBButtonList = true;
            this.bButtonList1.Visible = true;

            this.InitializePage(functionName, pageKey, session);
        }

        public virtual void InitializePage()
        {
            _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._lang = MultiLanguageHandler.getInstance();
            InitializeVariables();

            InitializeSpread();

            if(IsVisibleBButtonList)
                InitializeButtonList();
        }

        private void InitializeVariables()
        {
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
            this.controller = GetController();
        }

        public virtual SPCModelUCController GetController()
        {
            return new SPCModelUCController();
        }

        private void InitializeSpread()
        {
            this.bSpread1.ClearHead();
            this.bSpread1.AddHeadComplete();
            this.bSpread1.UseSpreadEdit = false;
            this.bSpread1.Locked = true;
            this.bSpread1.UseGeneralContextMenu = false;
        }

        private void InitializeButtonList()
        {
            if(string.IsNullOrEmpty(itemKey))
            {
                throw new Exception("Item key is empty.");
                return;
            }

            if(initialization == null)
            {
                initialization = new Initialization();
                initialization.InitializePath();
            }

            this.initialization.InitializeButtonList(this.bButtonList1, ref bSpread1, this.KeyOfPage, itemKey , this.sessionData);

            this.ApplyAuthory(this.bButtonList1);
        }

        private void BindingSpread()
        {
            DataTable dtSPCModelChartList = new DataTable();

            controller.MakeColumnsForBinding(dtSPCModelChartList , _ds);

            controller.AddRowsForBinding(dtSPCModelChartList, _ds, _bUseComma);
            dtSPCModelChartList.AcceptChanges();

            bSpread1.ClearHead();
            bSpread1.UseEdit = true;
            bSpread1.UseHeadColor = true;

            MakeColumnsOfSpread(dtSPCModelChartList);
            this.bSpread1.DataSet = dtSPCModelChartList;
            AdjustColumnSize();
        }

        private void MakeColumnsOfSpread(DataTable dt)
        {
            int columnIndex = 0;
            if(AddSelectColumn)
            {
                this.bSpread1.AddHead(0, "SELECT", "_SELECT", 50, 50, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null,
                                      false, true);
                columnIndex++;
            }

            for (int i = 0; i < dt.Columns.Count; i++, columnIndex++)
            {
                string sColumn = dt.Columns[i].ColumnName.ToString();
                this.bSpread1.AddHead(columnIndex, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.ReadOnly,
                                              ColumnType.Null, null, null, null, false, true);
            }
            this.bSpread1.AddHeadComplete();
        }

        private void AdjustColumnSize()
        {
            for (int cIdx = 0; cIdx < this.bSpread1.ActiveSheet.Columns.Count; cIdx++)
            {
                this.bSpread1.ActiveSheet.Columns[cIdx].Width = this.bSpread1.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                if (this.bSpread1.ActiveSheet.Columns[cIdx].Width > 150)
                    this.bSpread1.ActiveSheet.Columns[cIdx].Width = 150;
            }

            int idxVersion = this.GetColumnIndex(BISTel.eSPC.Common.COLUMN.VERSION);
            if (idxVersion >= 0)
            {
                for (int idxRow = 0; idxRow < this.bSpread1.ActiveSheet.RowCount; idxRow++)
                {
                    if (this.bSpread1.ActiveSheet.Cells[idxRow, idxVersion].Text != null && this.bSpread1.ActiveSheet.Cells[idxRow, idxVersion].Text.Length > 0)
                    {
                        this.bSpread1.ActiveSheet.Cells[idxRow, idxVersion].Text = (1 + Convert.ToDouble(this.bSpread1.ActiveSheet.Cells[idxRow, idxVersion].Text) / 100).ToString("N2");
                    }
                }
            }
        }

        public int GetColumnIndex(string name)
        {
            for(int i=0; i<bSpread1.ActiveSheet.ColumnHeader.Columns.Count; i++)
            {
                if(bSpread1.ActiveSheet.ColumnHeader.Columns.Get(i).Label.ToUpper() == name.ToUpper())
                    return i;
            }

            return -1;
        }

        public virtual void bButtonList1_ButtonClick(string name)
        {
        }

        public void PageRefresh()
        {
            this.PageSearch(lastestCondition);
        }

        private void bSpread1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != (int)iColIdx.SELECT)
                return;

            if ((bool)this.bSpread1.GetCellValue(e.Row, (int)iColIdx.SELECT) == true)
            {
                for (int i = 0; i < this.bSpread1.ActiveSheet.RowCount; i++)
                {
                    if (e.Row == i)
                    {
                        if (e.Column == (int)iColIdx.SELECT)
                        {
                            this.bSpread1.ActiveSheet.SetActiveCell(e.Row, 1);
                            continue;
                        }
                    }
                    this.bSpread1.ActiveSheet.Cells[i, (int)iColIdx.SELECT].Value = 0;
                }
            }
        }
    }
}
