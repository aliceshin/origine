using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Common
{
    public partial class SPCModelUC : BasePageUCtrl
    {
        public SPCModelUCController controller = null;
        public bool AddSelectColumn = false;
        private string itemKey = string.Empty;
        private Initialization initialization = null;
        public bool IsVisibleBButtonList = false;

        public SPCModel[] SPCModels
        {
            get
            {
                if (controller == null)
                    return null;
                return controller.SpcModels;
            }
        }

        public BSpread bSpread
        {
            get { return bSpread1; }
            set { bSpread1 = value; }
        }

        public SPCModelUC()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            if(llCondition[Definition.DynamicCondition_Search_key.SPCMODEL] == null
                || ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SPCMODEL]).Rows.Count == 0)
            {
                InitializePage();
                return;
            }

            DataTable spcmodels = (DataTable) llCondition[Definition.DynamicCondition_Search_key.SPCMODEL];
            List<string> modelRawids = new List<string>();
            foreach(DataRow dr in spcmodels.Rows)
            {
                modelRawids.Add(dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString());
            }

            DataSet ds = this.controller.GetModelData(modelRawids.ToArray());

            if(DSUtil.GetResultInt(ds) != 0)
            {
                MSGHandler.DisplayMessage(MSGType.Error, DSUtil.GetResultMsg(ds));
                InitializePage();
                return;
            }

            BindingSpread();
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
            InitializeVariables();

            InitializeSpread();

            if(IsVisibleBButtonList)
                InitializeButtonList();
        }

        private void InitializeVariables()
        {
            this.controller = new SPCModelUCController();
        }

        private void InitializeSpread()
        {
            this.bSpread1.ClearHead();
            this.bSpread1.AddHeadComplete();
            this.bSpread1.UseSpreadEdit = false;
            this.bSpread1.Locked = true;
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

            controller.MakeColumnsForBinding(dtSPCModelChartList);

            controller.AddRowsForBinding(dtSPCModelChartList);
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
                this.bSpread1.AddHead(0, "SELECT", "SELECT", 50, 50, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null,
                                      false, true);
                columnIndex++;
            }

            for (int i = 0; i < dt.Columns.Count; i++, columnIndex++)
            {
                string sColumn = dt.Columns[i].ColumnName.ToString();
                this.bSpread1.AddHead(columnIndex, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
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
    }
}
