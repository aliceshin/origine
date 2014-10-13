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

namespace BISTel.eSPC.Page.ATT.History
{
    public partial class SPCModelVersionHistory : BasePageUCtrl
    {
        public VersionCompareButtonClickEventHandler VersionCompareButtonClicked;

        private string itemKey = string.Empty;
        private Initialization initialization = null;
        private LinkedList lastestCondition = null;

        private SPCModelVersionHistoryController controller = null;

        SortedList alSelect;

        public SPCModelVersionHistory()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public void InitializePage()
        {
            InitializeVariables();

            InitializeSpread();

            InitializeButtonList();
        }

        public void InitializePage(string functionName, string pageKey, string itemKey, SessionData session)
        {
            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.sessionData = session;
            this.itemKey = itemKey;

            InitializePage();
        }

        private void InitializeVariables()
        {
            controller = new SPCModelVersionHistoryController();
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

        public override void PageSearch(LinkedList llCondition)
        {
            InitializePage();

            if(!llCondition.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
            {
                return;
            }

            lastestCondition = llCondition;

            string modelConfigRawID = llCondition[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
            
            DataTable historyTable = controller.GetHistoryDataTable(modelConfigRawID);

            BindHistoryDataTable(historyTable);
        }

        private void BindHistoryDataTable(DataTable dt)
        {
            InitializeSpread();
            InitializeColumns(dt);

            bSpread1.DataSet = dt;
            this.bSpread1.ActiveSheet.Columns[0].Locked = false;
            AdjustColumnSize();
        }

        private void InitializeColumns(DataTable dt)
        {
            bSpread1.ClearHead();
            
            this.bSpread1.AddHead(0, "SELECT", "SELECT", 50, 50, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false,
                                      true);

            for(int i=1; i<=dt.Columns.Count; i++)
            {
                bool visible = true;
                if(dt.Columns[i - 1].ColumnName.ToUpper() == "RAWID")
                    visible = false;

                this.bSpread1.AddHead(i, dt.Columns[i - 1].ColumnName, dt.Columns[i - 1].ColumnName, 120, 300, null, null,
                                      null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, visible);
            }

            bSpread1.AddHeadComplete();
        }

        private void AdjustColumnSize()
        {
            for (int cIdx = 0; cIdx < this.bSpread1.ActiveSheet.Columns.Count; cIdx++)
            {
                this.bSpread1.ActiveSheet.Columns[cIdx].Width = this.bSpread1.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                if (this.bSpread1.ActiveSheet.Columns[cIdx].Width > 120)
                    this.bSpread1.ActiveSheet.Columns[cIdx].Width = 120;
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

        private void bButtonList1_ButtonClick(string name)
        {
            if(name.ToUpper() == Definition.ButtonKey.VERSION_COMPARE)
            {
                VersionCompareClicked();
            }
            else if (name.ToUpper() == Definition.ButtonKey.VERSION_DELETE)
            {
                VersionDeleteClicked();
            }
        }

        private void VersionCompareClicked()
        {
            if (!(this.bSpread1 != null && this.bSpread1.ActiveSheet.RowCount > 0))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_HISTORY", null, null);
                return;
            }

            //int selectColumnIndex = GetColumnIndex("SELECT");
            //if(selectColumnIndex == -1)
            //    throw new Exception("There is no 'select' column.");

            ArrayList selectedList = bSpread1.GetCheckedList(0);
            if(selectedList.Count < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_COMPARE_MODEL", null, null);
                return;
            }

            string modelID = lastestCondition[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
            
            List<string> version = new List<string>();
            int versionIndex = GetColumnIndex(BISTel.eSPC.Common.COLUMN.VERSION);
            foreach(int i in selectedList)
            {
                version.Add(Convert.ToInt32((Convert.ToDouble(this.bSpread1.ActiveSheet.GetText(i, versionIndex)) - 1)*100).ToString());
            }

            if(VersionCompareButtonClicked != null)
            {
                var args = new VersionCompareButtonClickedEventArgs {ModelID = modelID, Versions = version.ToArray()};
                VersionCompareButtonClicked(this, args);
            }
        }

        private void VersionDeleteClicked()
        {
            if (!(this.bSpread1 != null && this.bSpread1.ActiveSheet.RowCount > 0))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_HISTORY", null, null);
                return;
            }

            //int selectColumnIndex = GetColumnIndex("SELECT");
            //if (selectColumnIndex == -1)
            //    throw new Exception("There is no 'select' column.");

            ArrayList selectedList = bSpread1.GetCheckedList(0);
            if (selectedList.Count < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_VERSION", null, null);
                return;
            }
            if (selectedList.Count > 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ONE_VERSION", null, null);
                return;
            }

            string modelID = lastestCondition[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();

            List<string> version = new List<string>();
            int versionIndex = GetColumnIndex(BISTel.eSPC.Common.COLUMN.VERSION);
            foreach (int i in selectedList)
            {
                version.Add(Convert.ToInt32((Convert.ToDouble(this.bSpread1.ActiveSheet.GetText(i, versionIndex)) - 1) * 100).ToString());
            }

            if (version[0] == controller.CheckLatestVersion(modelID))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CANT_DEL_LAST_VERSION", null, null);
                return;
            }

            LinkedList lnkCondition = new LinkedList();
            lnkCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, modelID);
            lnkCondition.Add(Definition.CONDITION_KEY_SPC_MODEL_VERSION, version[0]);

            if (controller.DeleteVersion(lnkCondition))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SUCCESS_DEL_VERSION", null, null);
                this.PageSearch(lnkCondition);
                return;
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_FAIL_DEL_VERSION", null, null);
                this.PageSearch(lnkCondition);
                return;
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

        public void PageRefresh()
        {
            this.PageSearch(lastestCondition);
        }

        private void bSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (alSelect.Count > 0)
            {
                if (!(bool)this.bSpread1.GetCellValue(e.Row, 0))
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bSpread1.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "False");
                    }
                }
                else
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bSpread1.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "True");
                    }
                }
            }
            alSelect.Clear();
        }
        private void bSpread1_MouseDown(object sender, MouseEventArgs e)
        {
            alSelect = this.bSpread1.GetSelectedRows();
        }
    }

    public delegate void VersionCompareButtonClickEventHandler(object sender, VersionCompareButtonClickedEventArgs args);
    public delegate void VersionDeleteButtonClickEventHandler(object sender, VersionDeleteButtonClickedEventArgs args);

    public class VersionCompareButtonClickedEventArgs : EventArgs
    {
        public string ModelID = string.Empty;
        public string[] Versions = new string[0];
    }

    public class VersionDeleteButtonClickedEventArgs : EventArgs
    {
        public string ModelID = string.Empty;
        public string[] Versions = new string[0];
    }
}
