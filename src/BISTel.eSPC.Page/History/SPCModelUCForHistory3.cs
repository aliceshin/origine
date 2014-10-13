using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.History
{
    public partial class SPCModelUCForHistory3 : SPCModelUC2
    {
        public VersionHistoryButtonClickEventHandler VersionHistoryButtonClicked;

        public SPCModelUCForHistory3()
        {
            this.AddSelectColumn = true;
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public void InitializePage(string functionName, string pageKey, string itemKey, SessionData session)
        {
            this.itemKey = itemKey;

            IsVisibleBButtonList = true;
            this.bButtonList1.Visible = true;

            this.InitializePage(functionName, pageKey, session);
        }

        public override SPCModelUCController GetController()
        {
            return new SPCModelUCControllerForHistory();
        }

        public override void bButtonList1_ButtonClick(string name)
        {
            if(name == Definition.ButtonKey.VERSION_HISTORY)
            {
                VersionHistoryClicked();
            }
        }

        public void VersionHistoryClicked()
        {
            if (!(this.bSpread != null && this.bSpread.ActiveSheet.RowCount > 0))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH_SPC_MODEL_LIST", null, null);
                return;
            }

            //int selectColumnIndex = GetColumnIndex("SELECT");
            //if(selectColumnIndex == -1)
            //    throw new Exception("There is no 'select' column.");

            ArrayList selectedList = bSpread.GetCheckedList(0);
            if(selectedList.Count != 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ONE_MODEL", null, null);
                return;
            }

            string modelID = this.bSpread.GetCellText((int)selectedList[0], GetColumnIndex(COLUMN.CHART_ID));

            if(VersionHistoryButtonClicked != null)
            {
                var args = new VersionHistoryButtonClickedEventArgs {ModelID = modelID};
                VersionHistoryButtonClicked(this, args);
            }
        }
    }

    public delegate void VersionHistoryButtonClickEventHandler(object sender,VersionHistoryButtonClickedEventArgs args);

    public class VersionHistoryButtonClickedEventArgs : EventArgs
    {
        public string ModelID = string.Empty;
    }
}
