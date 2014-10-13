using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Compare;
using BISTel.eSPC.Page.Modeling;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using System.Reflection;
using System.ComponentModel;

namespace BISTel.eSPC.Page.History
{
    public class SPCModelVersionCompareResultController : ICompareResultController
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private DataSet dsData = null;
        internal Dictionary<string, int> rowIndex = new Dictionary<string, int>();
        private static Dictionary<string, string> specColumnName = new Dictionary<string, string>();
        private static Dictionary<string, string> autocalcColumnName = new Dictionary<string, string>();
        private string lastestVersion = string.Empty;

        private bool isMET = false;

        private LinkedList _LimitOptionList;

        internal SPCModelVersionCompareResultController()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            Initialize();
        }

        private void Initialize()
        {
            this._LimitOptionList = new LinkedList();

            DataSet dsOption = _ws.GetAutoCalculationOption(Definition.CODE_CATEGORY_AUTOCALC_OPTION);

            if (dsOption != null && dsOption.Tables.Count > 0)
            {
                foreach (DataRow dr in dsOption.Tables[0].Rows)
                {
                    this._LimitOptionList.Add(dr[Definition.VARIABLE_CODE], dr[Definition.VARIABLE_NAME].ToString());
                }
            }

            InitializeSpec();
        }

        private void InitializeSpec()
        {
            if(specColumnName.Count == 0)
            {
                specColumnName.Add("USL", "UPPER_SPEC");
                specColumnName.Add("LSL", "LOWER_SPEC");
                specColumnName.Add("MEAN UCL", "UPPER_CONTROL");
                specColumnName.Add("MEAN LCL", "LOWER_CONTROL");
                specColumnName.Add("RAW UCL", "RAW_UCL");
                specColumnName.Add("RAW LCL", "RAW_LCL");
                specColumnName.Add("STD UCL", "STD_UCL");
                specColumnName.Add("STD LCL", "STD_LCL");
                specColumnName.Add("RANGE UCL", "RANGE_UCL");
                specColumnName.Add("RANGE LCL", "RANGE_LCL");
                specColumnName.Add("EWMA MEAN UCL", "EWMA_M_UCL");
                specColumnName.Add("EWMA MEAN LCL", "EWMA_M_LCL");
                specColumnName.Add("EWMA RANGE UCL", "EWMA_R_UCL");
                specColumnName.Add("EWMA RANGE LCL", "EWMA_R_LCL");
                specColumnName.Add("EWMA STD UCL", "EWMA_S_UCL");
                specColumnName.Add("EWMA STD LCL", "EWMA_S_LCL");
                specColumnName.Add("MA UCL", "MA_UCL");
                specColumnName.Add("MA LCL", "MA_LCL");
                specColumnName.Add("MS UCL", "MS_UCL");
                specColumnName.Add("MS LCL", "MS_LCL");
                specColumnName.Add("MR UCL", "MR_UCL");
                specColumnName.Add("MR LCL", "MR_LCL");
                specColumnName.Add("ZONE A UCL", "ZONE_A_UCL");
                specColumnName.Add("ZONE A LCL", "ZONE_A_LCL");
                specColumnName.Add("ZONE B UCL", "ZONE_B_UCL");
                specColumnName.Add("ZONE B LCL", "ZONE_B_LCL");
                specColumnName.Add("ZONE C UCL", "ZONE_C_UCL");
                specColumnName.Add("ZONE C LCL", "ZONE_C_LCL");
                specColumnName.Add("OFFSET USE YN", "OFFSET_YN");
                specColumnName.Add("USL OFFSET", "SPEC_USL_OFFSET");
                specColumnName.Add("LSL OFFSET", "SPEC_LSL_OFFSET");
                specColumnName.Add("MEAN UCL OFFSET", "MEAN_UCL_OFFSET");
                specColumnName.Add("MEAN LCL OFFSET", "MEAN_LCL_OFFSET");
                specColumnName.Add("RAW UCL OFFSET", "RAW_UCL_OFFSET");
                specColumnName.Add("RAW LCL OFFSET", "RAW_LCL_OFFSET");
                specColumnName.Add("STD UCL OFFSET", "STD_UCL_OFFSET");
                specColumnName.Add("STD LCL OFFSET", "STD_LCL_OFFSET");
                specColumnName.Add("RANGE UCL OFFSET", "RANGE_UCL_OFFSET");
                specColumnName.Add("RANGE LCL OFFSET", "RANGE_LCL_OFFSET");
                specColumnName.Add("EWMA MEAN UCL OFFSET", "EWMA_M_UCL_OFFSET");
                specColumnName.Add("EWMA MEAN LCL OFFSET", "EWMA_M_LCL_OFFSET");
                specColumnName.Add("EWMA RANGE UCL OFFSET", "EWMA_R_UCL_OFFSET");
                specColumnName.Add("EWMA RANGE LCL OFFSET", "EWMA_R_LCL_OFFSET");
                specColumnName.Add("EWMA STD UCL OFFSET", "EWMA_S_UCL_OFFSET");
                specColumnName.Add("EWMA STD LCL OFFSET", "EWMA_S_LCL_OFFSET");
                specColumnName.Add("MA UCL OFFSET", "MA_UCL_OFFSET");
                specColumnName.Add("MA LCL OFFSET", "MA_LCL_OFFSET");
                specColumnName.Add("MS UCL OFFSET", "MS_UCL_OFFSET");
                specColumnName.Add("MS LCL OFFSET", "MS_LCL_OFFSET");
                specColumnName.Add("MR UCL OFFSET", "MR_UCL_OFFSET");
                specColumnName.Add("MR LCL OFFSET", "MR_LCL_OFFSET");
                specColumnName.Add("ZONE A UCL OFFSET", "ZONE_A_UCL_OFFSET");
                specColumnName.Add("ZONE A LCL OFFSET", "ZONE_A_LCL_OFFSET");
                specColumnName.Add("ZONE B UCL OFFSET", "ZONE_B_UCL_OFFSET");
                specColumnName.Add("ZONE B LCL OFFSET", "ZONE_B_LCL_OFFSET");
                specColumnName.Add("ZONE C UCL OFFSET", "ZONE_C_UCL_OFFSET");
                specColumnName.Add("ZONE C LCL OFFSET", "ZONE_C_LCL_OFFSET");
                specColumnName.Add("FILTER UPPER LIMIT", "UPPER_FILTER");
                specColumnName.Add("FILTER LOWER LIMIT", "LOWER_FILTER");
                specColumnName.Add("UPPER TECHNICAL LIMIT", "UPPER_TECHNICAL_LIMIT");
                specColumnName.Add("LOWER TECHNICAL LIMIT", "LOWER_TECHNICAL_LIMIT");
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

                autocalcColumnName.Add("Global Option", "CONTROL_LIMIT_OPTION");    //SPC-1129 BY STELLA
                autocalcColumnName.Add("Use Global", "USE_GLOBAL_YN");              //SPC-1129 BY STELLA

                autocalcColumnName.Add("Calculate STD", "STD_YN");
                autocalcColumnName.Add("Calculate RAW", "RAW_CL_YN");
                autocalcColumnName.Add("Calculate MEAN", "MEAN_CL_YN");
                autocalcColumnName.Add("Calculate RANGE", "RANGE_CL_YN");
                autocalcColumnName.Add("Calculate EWMA MEAN", "EWMA_M_CL_YN");
                autocalcColumnName.Add("Calculate EWMA RANGE", "EWMA_R_CL_YN");
                autocalcColumnName.Add("Calculate EWMA STD", "EWMA_S_CL_YN");
                autocalcColumnName.Add("Calculate MA", "MA_CL_YN");
                autocalcColumnName.Add("Calculate MS", "MS_CL_YN");
                autocalcColumnName.Add("Calculate MR", "MR_CL_YN");
                autocalcColumnName.Add("Calculate ZONE", "ZONE_YN");
                autocalcColumnName.Add("SHIFT CALCULATION", "SHIFT_CALC_YN");
                autocalcColumnName.Add("WITHOUT IQR FILTER", "WITHOUT_IQR_YN");
                //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                autocalcColumnName.Add("THRESHOLD OFF YN", "THRESHOLD_OFF_YN");
                //SPC-658 Initial Calc Count
                autocalcColumnName.Add("Auto Calculation Initial Count", "INITIAL_CALC_COUNT");

                //SPC-1155 BY STELLA
                autocalcColumnName.Add("Calculate STD Value", "STD_CALC_VALUE");
                autocalcColumnName.Add("Calculate RAW Value", "RAW_CALC_VALUE");
                autocalcColumnName.Add("Calculate MEAN Value", "MEAN_CALC_VALUE");
                autocalcColumnName.Add("Calculate RANGE Value", "RANGE_CALC_VALUE");
                autocalcColumnName.Add("Calculate EWMA MEAN Value", "EWMA_M_CALC_VALUE");
                autocalcColumnName.Add("Calculate EWMA RANGE Value", "EWMA_R_CALC_VALUE");
                autocalcColumnName.Add("Calculate EWMA STD Value", "EWMA_S_CALC_VALUE");
                autocalcColumnName.Add("Calculate MA Value", "MA_CALC_VALUE");
                autocalcColumnName.Add("Calculate MS Value", "MS_CALC_VALUE");
                autocalcColumnName.Add("Calculate MR Value", "MR_CALC_VALUE");

                autocalcColumnName.Add("Calculate STD Option", "STD_CALC_OPTION");
                autocalcColumnName.Add("Calculate RAW Option", "RAW_CALC_OPTION");
                autocalcColumnName.Add("Calculate MEAN Option", "MEAN_CALC_OPTION");
                autocalcColumnName.Add("Calculate RANGE Option", "RANGE_CALC_OPTION");
                autocalcColumnName.Add("Calculate EWMA MEAN Option", "EWMA_M_CALC_OPTION");
                autocalcColumnName.Add("Calculate EWMA RANGE Option", "EWMA_R_CALC_OPTION");
                autocalcColumnName.Add("Calculate EWMA STD Option", "EWMA_S_CALC_OPTION");
                autocalcColumnName.Add("Calculate MA Option", "MA_CALC_OPTION");
                autocalcColumnName.Add("Calculate MS Option", "MS_CALC_OPTION");
                autocalcColumnName.Add("Calculate MR Option", "MR_CALC_OPTION");

                autocalcColumnName.Add("Calculate STD Side", "STD_CALC_SIDED");
                autocalcColumnName.Add("Calculate RAW Side", "RAW_CALC_SIDED");
                autocalcColumnName.Add("Calculate MEAN Side", "MEAN_CALC_SIDED");
                autocalcColumnName.Add("Calculate RANGE Side", "RANGE_CALC_SIDED");
                autocalcColumnName.Add("Calculate EWMA MEAN Side", "EWMA_M_CALC_SIDED");
                autocalcColumnName.Add("Calculate EWMA RANGE Side", "EWMA_R_CALC_SIDED");
                autocalcColumnName.Add("Calculate EWMA STD Side", "EWMA_S_CALC_SIDED");
                autocalcColumnName.Add("Calculate MA Side", "MA_CALC_SIDED");
                autocalcColumnName.Add("Calculate MS Side", "MS_CALC_SIDED");
                autocalcColumnName.Add("Calculate MR Side", "MR_CALC_SIDED");
            }
        }

        public void ISMET(bool isMet)
        {
            this.isMET = isMet;
        }

        public DataTable GetComparedDataTable(LinkedList llCondition)
        {
            dsData = this._ws.GetSPCSpecAndRuleOfHistory((string)llCondition["CHART_ID"], (string[])llCondition["VERSIONS"]);
            lastestVersion = this._ws.GetSPCLastestVersion((string) llCondition["CHART_ID"]);
            
            DataTable dtData = MakeDataTableForSpread(dsData);
            return dtData;
        }

        private DataTable MakeDataTableForSpread(DataSet dsData)
        {
            DataTable comparedTable = new DataTable();

            AddColumns(comparedTable, dsData.Tables[TABLE.MODEL_CONFIG_MST_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[TABLE.CHART_VW_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[TABLE.MODEL_CONFIG_MST_SPC]);

            AddSpec(comparedTable, dsData.Tables[TABLE.MODEL_CONFIG_MST_SPC]);

            AddRuleList(comparedTable, dsData.Tables[TABLE.MODEL_RULE_MST_SPC]);

            AddAutocalc(comparedTable, dsData.Tables[TABLE.MODEL_AUTOCALC_MST_SPC]);

            return comparedTable;
        }

        private static void AddColumns(DataTable target, DataTable original)
        {
            target.Columns.Add(COLUMN.VERSION);
            target.Columns.Add(" ");

            foreach(DataRow dr in original.Rows)
            {
                target.Columns.Add(dr[COLUMN.VERSION].ToString());
            }
        }

        private void AddChartInformation(DataTable target, DataTable original)
        {
            if (original.TableName == TABLE.CHART_VW_SPC)
            {
                DataRow drModelName = target.NewRow();
                DataRow drMain = target.NewRow();

                drModelName[COLUMN.VERSION] = "SPC MODEL NAME";
                drMain[COLUMN.VERSION] = "Main YN";

                rowIndex.Add("SPC MODEL NAME", rowIndex.Count);
                rowIndex.Add("Main YN", rowIndex.Count);

                FillDataRow(drModelName, original, COLUMN.VERSION, "SPC_MODEL_NAME");
                FillDataRow(drMain, original, COLUMN.VERSION, "MAIN_YN");

                target.Rows.Add(drModelName);
                target.Rows.Add(drMain);
            }
            else
            {
                DataRow drChartDesc = target.NewRow();

                drChartDesc[COLUMN.VERSION] = "Chart Description";

                rowIndex.Add("Chart Description", rowIndex.Count);

                FillDataRow(drChartDesc, original, COLUMN.VERSION, "CHART_DESCRIPTION");

                target.Rows.Add(drChartDesc);
            }
        }

        private void AddSpec(DataTable target, DataTable original)
        {
            foreach(KeyValuePair<string, string> kvp in specColumnName)
            {
                DataRow dr = target.NewRow();
                dr[COLUMN.VERSION] = "RULE";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, COLUMN.VERSION, kvp.Value);
                target.Rows.Add(dr);
            }
        }

        private void AddRuleList(DataTable target, DataTable original)
        {
            Dictionary<string, List<int>> rules = new Dictionary<string, List<int>>();
            foreach(DataRow dr in original.Rows)
            {
                string version = dr[COLUMN.VERSION].ToString();
                if(!rules.ContainsKey(version))
                {
                    rules.Add(version, new List<int>());
                }
                rules[version].Add(int.Parse(dr["SPC_RULE_NO"].ToString()));
            }

            DataRow newRow = target.NewRow();
            newRow[COLUMN.VERSION] = "RULE";
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
                dr[COLUMN.VERSION] = "Auto Calculation";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, COLUMN.VERSION, kvp.Value);
                target.Rows.Add(dr);
            }

            for (int idxColumn = 2; idxColumn < target.Columns.Count; idxColumn++)
            {
                target.Columns[idxColumn].ColumnName = (1 + Convert.ToDouble(target.Columns[idxColumn].ColumnName) / 100).ToString("N2"); 
            }
        }

        private void FillDataRow(DataRow target, DataTable original, string chartIDColumnName, string addedValueColumnName)
        {
            //spc-1155 by stella 
            foreach (DataRow dr in original.Rows)
            {
                string chartID = dr[chartIDColumnName].ToString();

                if (addedValueColumnName.EndsWith("CALC_OPTION"))
                {
                    switch (dr[addedValueColumnName].ToString())
                    {
                        case "S":
                            target[chartID] = LimitOption.SIGMA.ToString();
                            break;
                        case "P":
                            target[chartID] = this._LimitOptionList[LimitOption.PERCENT.ToString()].ToString();
                            break;
                        case "C":
                            target[chartID] = LimitOption.CONSTANT.ToString();
                            break;
                        default:
                            break;
                    }
                }
                else if (addedValueColumnName.EndsWith("CALC_SIDED"))
                {
                    switch (dr[addedValueColumnName].ToString())
                    {
                        case "U":
                            target[chartID] = AutoCalcSideOption.UPPER.ToString();
                            break;
                        case "L":
                            target[chartID] = AutoCalcSideOption.LOWER.ToString();
                            break;
                        case "B":
                            target[chartID] = AutoCalcSideOption.BOTH.ToString();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    target[chartID] = dr[addedValueColumnName].ToString();
                }
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
        public DataSet GetTargetConfigSpecData(string[] targetRawid)
        {
            throw new NotImplementedException();
        }
        public DataSet GetSourseConfigSpecData(string sourceRawid)
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
            spcConfigPopup.CONFIG_MODE = ConfigMode.ROLLBACK;
            spcConfigPopup.LINE_RAWID = line;
            spcConfigPopup.AREA_RAWID = area;
            spcConfigPopup.EQP_MODEL = eqpModel;
            spcConfigPopup.CONFIG_RAWID = chartId;
            spcConfigPopup.MAIN_YN = mainYN;
            spcConfigPopup.VERSION = version;
            spcConfigPopup.GROUP_NAME = groupName;
            if (this.isMET)
            {
                spcConfigPopup.MODELINGTYPE = "MET";
            }
            else
            {
                spcConfigPopup.MODELINGTYPE = "TRACE";
            }

            if(_ws.GetTheNumberOfSubConfigOfModel(chartId) > 0)
                spcConfigPopup.HAS_SUBCONFIGS = true;
            else
                spcConfigPopup.HAS_SUBCONFIGS = false;

            if(version.Equals(lastestVersion))
                spcConfigPopup.SaveButtonVisible = false;
            
            spcConfigPopup.InitializePopup();

            return spcConfigPopup.ShowDialog();
        }

        public LinkedList GetGroupNameByChartID(string chartId)
        {
            throw new NotImplementedException();
        }
    }
}
