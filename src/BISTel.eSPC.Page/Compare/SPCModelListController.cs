using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Compare
{
    internal class SPCModelListController
    {
        internal eSPCWebService.eSPCWebService _ws = null;
        private List<string> notContextColumnNames = new List<string>();
        internal DataTable dtSPCModelList = null;
        internal Dictionary<string, int> columnIndex = new Dictionary<string, int>();
        internal Control parent = null;

        internal SPCModelListController(eSPCWebService.eSPCWebService ws, Control parent)
        {
            this._ws = ws;

            this.parent = parent;

            notContextColumnNames.AddRange(new string[] {"SELECT", "CHART_ID", "SPC_MODEL_NAME", "MAIN_YN"});
        }

        internal DataTable GetSPCModelList(string sLineRawID, string sAreaRawID, string sEqpModel, string sParamAlias, string sParamTypeCd, bool useComma)
        {
            if(_ws == null)
                _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            DataSet ds = _ws.GetSPCModelList(sLineRawID, sAreaRawID, sEqpModel, sParamAlias, sParamTypeCd, useComma);

            dtSPCModelList = MergeWithContextTable(ds.Tables[TABLE.CHART_VW_SPC], ds.Tables[TABLE.MODEL_CONTEXT_MST_SPC]);

            return dtSPCModelList;
        }

        private static DataTable MergeWithContextTable(DataTable dt, DataTable contextTable)
        {
            DataTable dtReturn = new DataTable();

            foreach(DataColumn dc in dt.Columns)
            {
                dtReturn.Columns.Add(dc.ColumnName, dc.DataType);
            }
            foreach(DataRow dr in contextTable.Rows)
            {
                string columnName = dr["CONTEXT_KEY_NAME"].ToString();
                if(!dtReturn.Columns.Contains(columnName))
                    dtReturn.Columns.Add(columnName);
            }

            dtReturn.Merge(dt, false, MissingSchemaAction.Ignore);

            foreach(DataRow dr in dtReturn.Rows)
            {
                foreach(DataRow drContext in contextTable.Select("MODEL_CONFIG_RAWID = '" + dr["CHART_ID"] + "'"))
                {
                    dr[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext["CONTEXT_VALUE"].ToString();
                }
            }

            return dtReturn;
        }

        internal void Filter(BSpread spread)
        {
            string conditionQuery = string.Empty;

            List<string> filteringColumns = new List<string>();
            foreach(DataColumn dc in this.dtSPCModelList.Columns)
            {
                if(!notContextColumnNames.Contains(dc.ColumnName.ToUpper()))
                {
                    filteringColumns.Add(dc.ColumnName);
                }
            }
            SpreadFilteringPopup filteringPopup = new SpreadFilteringPopup(this.dtSPCModelList, filteringColumns.ToArray());

            if(DialogResult.Cancel == filteringPopup.ShowDialog())
            {
                return;
            }

            conditionQuery = filteringPopup.ConditionQuery;

            List<string> checkedCartID = new List<string>(GetCheckedChartID(conditionQuery));

            for (int i = 0; i < spread.ActiveSheet.Rows.Count; i++)
            {
                if(checkedCartID.Contains(spread.ActiveSheet.Cells[i, columnIndex["CHART_ID"]].Text))
                {
                    spread.ActiveSheet.Rows[i].Visible = true;
                    continue;
                }

                if(spread.ActiveSheet.Cells[i, columnIndex["SELECT"]].Text == "True")
                {
                    spread.ActiveSheet.Cells[i, columnIndex["SELECT"]].Text = null;
                }

                spread.ActiveSheet.Rows[i].Visible = false;
            }
        }

        private string[] GetCheckedChartID(string conditionQuery)
        {
            List<string> checkedChartID = new List<string>();

            foreach(DataRow dr in this.dtSPCModelList.Select(conditionQuery))
            {
                checkedChartID.Add(dr["CHART_ID"].ToString());
            }

            return checkedChartID.ToArray();
        }

        internal void Compare(BSpread spread)
        {
            ArrayList checkedRows = spread.GetCheckedList(columnIndex["SELECT"]);

            if(!(parent is SPCModelCompareUC) || parent == null)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, "Error");
                return;
            }

            ArrayList chartIDs = new ArrayList();
            foreach(int rowIndex in checkedRows)
            {
                chartIDs.Add(spread.ActiveSheet.GetText(rowIndex, columnIndex["CHART_ID"]));
            }

            LinkedList llCondition = new LinkedList("CHART_ID", chartIDs.ToArray(typeof (string)));
            ((SPCModelCompareUC) parent).Compare(llCondition);
        }
    }
}
