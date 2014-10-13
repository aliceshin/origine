using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Compare;
using BISTel.eSPC.Page.ATT.Modeling;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.History
{
    public class SPCModelVersionCompareResultController : ICompareResultController
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private DataSet dsData = null;
        internal Dictionary<string, int> rowIndex = new Dictionary<string, int>();
        private static Dictionary<string, string> specColumnName = new Dictionary<string, string>();
        private static Dictionary<string, string> autocalcColumnName = new Dictionary<string, string>();
        private string lastestVersion = string.Empty;

        internal SPCModelVersionCompareResultController()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            Initialize();
        }

        private void Initialize()
        {
            InitializeSpec();
        }

        private void InitializeSpec()
        {
            if(specColumnName.Count == 0)
            {
                specColumnName.Add("PN USL", "PN_USL");
                specColumnName.Add("PN LSL", "PN_LSL");
                specColumnName.Add("C USL", "C_USL");
                specColumnName.Add("C LSL", "C_LSL");
                specColumnName.Add("PN UCL", "PN_UCL");
                specColumnName.Add("PN LCL", "PN_LCL");
                specColumnName.Add("C UCL", "C_UCL");
                specColumnName.Add("C LCL", "C_LCL");
                specColumnName.Add("P UCL", "P_UCL");
                specColumnName.Add("P LCL", "P_LCL");
                specColumnName.Add("U UCL", "U_UCL");
                specColumnName.Add("U LCL", "U_LCL");
                specColumnName.Add("OFFSET USE YN", "OFFSET_YN");
                specColumnName.Add("PN USL OFFSET", "PN_USL_OFFSET");
                specColumnName.Add("PN LSL OFFSET", "PN_LSL_OFFSET");
                specColumnName.Add("C USL OFFSET", "C_USL_OFFSET");
                specColumnName.Add("C LSL OFFSET", "C_LSL_OFFSET");
                specColumnName.Add("PN UCL OFFSET", "PN_UCL_OFFSET");
                specColumnName.Add("PN LCL OFFSET", "PN_LCL_OFFSET");
                specColumnName.Add("C UCL OFFSET", "C_UCL_OFFSET");
                specColumnName.Add("C LCL OFFSET", "C_LCL_OFFSET");
                specColumnName.Add("P UCL OFFSET", "P_UCL_OFFSET");
                specColumnName.Add("P LCL OFFSET", "P_LCL_OFFSET");
                specColumnName.Add("U UCL OFFSET", "U_UCL_OFFSET");
                specColumnName.Add("U LCL OFFSET", "U_LCL_OFFSET");
            }

            if(autocalcColumnName.Count == 0)
            {
                autocalcColumnName.Add("Auto Calculation Period", "AUTOCALC_PERIOD");
                autocalcColumnName.Add("Minimum samples", "MIN_SAMPLES");
                autocalcColumnName.Add("Default Period", "DEFAULT_PERIOD");
                autocalcColumnName.Add("Maximum Period", "MAX_PERIOD");
                autocalcColumnName.Add("SIGMA", "CONTROL_LIMIT");
                autocalcColumnName.Add("THRESHOLD", "CONTROL_THRESHOLD");
                autocalcColumnName.Add("Auto Calculation Count", "CALC_COUNT");
                autocalcColumnName.Add("Auto Calculation Initial Count", "INITIAL_CALC_COUNT");
                autocalcColumnName.Add("Calculate PN", "PN_CL_YN");
                autocalcColumnName.Add("Calculate C", "C_CL_YN");
                autocalcColumnName.Add("Calculate P", "P_CL_YN");
                autocalcColumnName.Add("Calculate U", "U_CL_YN");
                autocalcColumnName.Add("SHIFT CALCULATION", "SHIFT_CALC_YN");
                autocalcColumnName.Add("THRESHOLD OFF YN", "THRESHOLD_OFF_YN");
                
            }
        }

        public DataTable GetComparedDataTable(LinkedList llCondition)
        {
            dsData = this._ws.GetATTSPCSpecAndRuleOfHistory((string)llCondition["CHART_ID"], (string[])llCondition["VERSIONS"]);
            lastestVersion = this._ws.GetATTSPCLastestVersion((string) llCondition["CHART_ID"]);
            
            DataTable dtData = MakeDataTableForSpread(dsData);
            return dtData;
        }

        private DataTable MakeDataTableForSpread(DataSet dsData)
        {
            DataTable comparedTable = new DataTable();

            AddColumns(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.CHART_VW_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC]);

            AddSpec(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC]);

            AddRuleList(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC]);

            AddAutocalc(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC]);

            return comparedTable;
        }

        private static void AddColumns(DataTable target, DataTable original)
        {
            target.Columns.Add(BISTel.eSPC.Common.COLUMN.VERSION);
            target.Columns.Add(" ");

            foreach(DataRow dr in original.Rows)
            {
                target.Columns.Add(dr[BISTel.eSPC.Common.COLUMN.VERSION].ToString());
            }
        }

        private void AddChartInformation(DataTable target, DataTable original)
        {
            if (original.TableName == BISTel.eSPC.Common.TABLE.CHART_VW_SPC)
            {
                DataRow drModelName = target.NewRow();
                DataRow drMain = target.NewRow();

                drModelName[BISTel.eSPC.Common.COLUMN.VERSION] = "SPC MODEL NAME";
                drMain[BISTel.eSPC.Common.COLUMN.VERSION] = "Main YN";

                rowIndex.Add("SPC MODEL NAME", rowIndex.Count);
                rowIndex.Add("Main YN", rowIndex.Count);

                FillDataRow(drModelName, original, BISTel.eSPC.Common.COLUMN.VERSION, "SPC_MODEL_NAME");
                FillDataRow(drMain, original, BISTel.eSPC.Common.COLUMN.VERSION, "MAIN_YN");

                target.Rows.Add(drModelName);
                target.Rows.Add(drMain);
            }
            else
            {
                DataRow drChartDesc = target.NewRow();

                drChartDesc[BISTel.eSPC.Common.COLUMN.VERSION] = "Chart Description";

                rowIndex.Add("Chart Description", rowIndex.Count);

                FillDataRow(drChartDesc, original, BISTel.eSPC.Common.COLUMN.VERSION, "CHART_DESCRIPTION");

                target.Rows.Add(drChartDesc);
            }
        }

        private void AddSpec(DataTable target, DataTable original)
        {
            foreach(KeyValuePair<string, string> kvp in specColumnName)
            {
                DataRow dr = target.NewRow();
                dr[BISTel.eSPC.Common.COLUMN.VERSION] = "RULE";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, BISTel.eSPC.Common.COLUMN.VERSION, kvp.Value);
                target.Rows.Add(dr);
            }
        }

        private void AddRuleList(DataTable target, DataTable original)
        {
            Dictionary<string, List<int>> rules = new Dictionary<string, List<int>>();
            foreach(DataRow dr in original.Rows)
            {
                string version = dr[BISTel.eSPC.Common.COLUMN.VERSION].ToString();
                if(!rules.ContainsKey(version))
                {
                    rules.Add(version, new List<int>());
                }
                rules[version].Add(int.Parse(dr["SPC_RULE_NO"].ToString()));
            }

            DataRow newRow = target.NewRow();
            newRow[BISTel.eSPC.Common.COLUMN.VERSION] = "RULE";
            newRow[" "] = "RULE LIST";

            rowIndex.Add("RULE LIST", rowIndex.Count);

            foreach(var kvp in rules)
            {
                kvp.Value.Sort();
                StringBuilder sb = new StringBuilder();
                foreach(int value in kvp.Value)
                {
                    if(sb.Length != 0)
                        sb.Append(",");
                    sb.Append(value.ToString());
                }
                newRow[kvp.Key] = sb.ToString();
            }

            target.Rows.Add(newRow);
        }

        private void AddAutocalc(DataTable target, DataTable original)
        {
            foreach(KeyValuePair<string, string> kvp in autocalcColumnName)
            {
                DataRow dr = target.NewRow();
                dr[BISTel.eSPC.Common.COLUMN.VERSION] = "Auto Calculation";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, BISTel.eSPC.Common.COLUMN.VERSION, kvp.Value);
                target.Rows.Add(dr);
            }

            for (int idxColumn = 2; idxColumn < target.Columns.Count; idxColumn++)
            {
                target.Columns[idxColumn].ColumnName = (1 + Convert.ToDouble(target.Columns[idxColumn].ColumnName) / 100).ToString("N2"); 
            }
        }

        private void FillDataRow(DataRow target, DataTable original, string chartIDColumnName, string addedValueColumnName)
        {
            foreach(DataRow dr in original.Rows)
            {
                string chartID = dr[chartIDColumnName].ToString();

                target[chartID] = dr[addedValueColumnName].ToString();
            }
        }

        //SPC-629
        public LinkedList Paste(SPCCopySpec popup, string targetChartID, string mainYN, string userRawID)
        {
            throw new NotImplementedException();
        }

        public DialogResult Modify(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId, string mainYN, string groupName)
        {
            throw new NotImplementedException();
        }

        //public void ViewChart(SessionData sessionData, string URL, string line, string area, string spcModelName, string paramAlias, string chartId, string paramType)
        //{
        //    throw new NotImplementedException();
        //}

        //SPC-629
        public string PasteModel(LinkedList pasteModelList)
        {
            throw new NotImplementedException();
        }

        //SPC-855
        public DataSet GetATTTargetConfigSpecData(string[] targetRawid)
        {
            throw new NotImplementedException();
        }
        public DataSet GetATTSourseConfigSpecData(string sourceRawid)
        {
            throw new NotImplementedException();
        }

        public int GetRowIndex(string name)
        {
            if(this.rowIndex.ContainsKey(name))
                return this.rowIndex[name];

            return -1;
        }

        public DialogResult ViewModel(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId, string mainYN, string version, string groupName)
        {
            SPCConfigurationPopup spcConfigPopup = new SPCConfigurationPopup();
            spcConfigPopup.SESSIONDATA = sessionData;
            spcConfigPopup.URL = URL;
            spcConfigPopup.PORT = Port;
            spcConfigPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.ROLLBACK;
            spcConfigPopup.LINE_RAWID = line;
            spcConfigPopup.AREA_RAWID = area;
            spcConfigPopup.EQP_MODEL = eqpModel;
            spcConfigPopup.CONFIG_RAWID = chartId;
            spcConfigPopup.MAIN_YN = mainYN;
            spcConfigPopup.VERSION = version;
            spcConfigPopup.GROUP_NAME = groupName; //SPC-1292, KBLEE

            if(_ws.GetTheNumberOfSubConfigOfModel(chartId) > 0)
                spcConfigPopup.HAS_SUBCONFIGS = true;
            else
                spcConfigPopup.HAS_SUBCONFIGS = false;

            if(version.Equals(lastestVersion))
                spcConfigPopup.SaveButtonVisible = false;
            
            spcConfigPopup.InitializePopup();

            return spcConfigPopup.ShowDialog();
        }

        //SPC-1292, KBLEE
        public LinkedList GetGroupNameByChartID(string chartId)
        {
            throw new NotImplementedException();
        }
    }
}
