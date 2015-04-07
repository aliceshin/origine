using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Modeling;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace BISTel.eSPC.Page.Compare
{
    internal class SPCModelCompareResultController : ICompareResultController
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private DataSet dsData = null;
        internal Dictionary<string, int> rowIndex = new Dictionary<string, int>();
        private static Dictionary<string, string> specColumnName = new Dictionary<string, string>();
        private static Dictionary<string, string> autocalcColumnName = new Dictionary<string, string>();

        private bool isMET = false;

        private LinkedList _LimitOptionList;
        ArrayList _arrColConfig;
        ArrayList _arrColContext;
        ArrayList _arrColOption;
        ArrayList _arrColAutoCalc;
        ArrayList _arrColRule;
        ArrayList _arrColLimit;

        int a;

        internal SPCModelCompareResultController()
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
            ColumnMappingByItems();
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

                //SPC-658 Initial Calc Count
                autocalcColumnName.Add("Auto Calculation Initial Count", "INITIAL_CALC_COUNT");

                autocalcColumnName.Add("Calculate STD", "STD_CL_YN");
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
            dsData = this._ws.GetSPCSpecAndRule((string[])llCondition["CHART_ID"]);

            DataTable dtData = MakeDataTableForSpread(dsData);
            return dtData;
        }

        private DataTable MakeDataTableForSpread(DataSet dsData)
        {
            DataTable comparedTable = new DataTable();

            AddColumns(comparedTable, dsData.Tables[TABLE.CHART_VW_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[TABLE.CHART_VW_SPC]);

            AddSpec(comparedTable, dsData.Tables[TABLE.MODEL_CONFIG_MST_SPC]);

            AddRuleList(comparedTable, dsData.Tables[TABLE.MODEL_RULE_MST_SPC]);

            AddAutocalc(comparedTable, dsData.Tables[TABLE.MODEL_AUTOCALC_MST_SPC]);

            return comparedTable;
        }

        private static void AddColumns(DataTable target, DataTable original)
        {
            target.Columns.Add("CHART ID");
            target.Columns.Add(" ");

            foreach(DataRow dr in original.Rows)
            {
                target.Columns.Add(dr["CHART_ID"].ToString());
            }
        }

        private void AddChartInformation(DataTable target, DataTable original)
        {
            DataRow drModelName = target.NewRow();
            DataRow drMain = target.NewRow();
            DataRow drChartDesc = target.NewRow();

            drModelName["CHART ID"] = "SPC MODEL NAME";
            drMain["CHART ID"] = "Main YN";
            drChartDesc["CHART ID"] = "Chart Description";

            rowIndex.Add("SPC MODEL NAME", rowIndex.Count);
            rowIndex.Add("Main YN", rowIndex.Count);
            rowIndex.Add("chart Description", rowIndex.Count);

            FillDataRow(drModelName, original, "CHART_ID", "SPC_MODEL_NAME");
            FillDataRow(drMain, original, "CHART_ID", "MAIN_YN");
            FillDataRow(drChartDesc, original, "CHART_ID", "CHART_DESCRIPTION");

            target.Rows.Add(drModelName);
            target.Rows.Add(drMain);
            target.Rows.Add(drChartDesc);
        }

        private void AddSpec(DataTable target, DataTable original)
        {
            foreach(KeyValuePair<string, string> kvp in specColumnName)
            {
                DataRow dr = target.NewRow();
                dr["CHART ID"] = "RULE";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, "RAWID", kvp.Value);
                target.Rows.Add(dr);
            }
        }

        private void AddRuleList(DataTable target, DataTable original)
        {
            Dictionary<string, List<int>> rules = new Dictionary<string, List<int>>();
            foreach(DataRow dr in original.Rows)
            {
                string chartId = dr["MODEL_CONFIG_RAWID"].ToString();
                if(!rules.ContainsKey(chartId))
                {
                    rules.Add(chartId, new List<int>());
                }
                rules[chartId].Add(int.Parse(dr["SPC_RULE_NO"].ToString()));
            }

            DataRow newRow = target.NewRow();
            newRow["CHART ID"] = "RULE";
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
                dr["CHART ID"] = "Auto Calculation";
                dr[" "] = kvp.Key;

                rowIndex.Add(kvp.Key, rowIndex.Count);

                FillDataRow(dr, original, "MODEL_CONFIG_RAWID", kvp.Value);
                target.Rows.Add(dr);
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
                        case "S" :
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
                        case "U" :
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
        public string PasteModel(LinkedList pasteModelList)
        {
            DataSet dsResult = _ws.CopyModelInfo(pasteModelList.GetSerialData());

            if (dsResult == null)
                return Definition.MSG_KEY_NO_PASTE_ITEM;
            if (DSUtil.GetResultSucceed(dsResult) == 0)
                return "FAIL";

            return "SUCCESS";
        }

        //spc-977 by stella
        private void ColumnMappingByItems()
        {
            _arrColConfig = new ArrayList();
            _arrColContext = new ArrayList();
            _arrColOption = new ArrayList();
            _arrColAutoCalc = new ArrayList();
            _arrColRule = new ArrayList();
            _arrColLimit = new ArrayList();

            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_INTERLOCK);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_MODE);

            _arrColContext.Add(Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION); //SPC-1218, KBLEE

            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_RAW);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MEAN);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_STD);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_RANGE);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_MEAN);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_STD);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_RANGE);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MA);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MR);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MOVING_OPTIONS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MEAN_OPTIONS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_A);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_B);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_C);
            _arrColRule.Add(Definition.COPY_MODEL.RULE_RULE_SELECTION);

            _arrColOption.Add(Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_CALCULATE_PPK);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_PRIORITY);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_DAYS);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT); ;
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN);

            //spc-1155 by stella
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN);

            _arrColAutoCalc.Add(COLUMN.SAVE_COMMENT);
        }

        //spc-977 by stella
        private string CompareCopyModel(LinkedList llstConfigurationInfo)
        {
            string key = string.Empty;
            string changedItems = string.Empty;

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColConfig.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Config,";
                    break;
                }
            }

            //SPC-1218, KBLEE, START
            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColContext.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Context,";
                    break;
                }
            }
            //SPC-1218, KBLEE, END

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColOption.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Option,";
                    break;
                }
            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColAutoCalc.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "AutoCalc,";
                    break;
                }

            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColRule.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Rule,";
                    break;
                }

            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColLimit.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Limit,";
                    break;
                }
            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (key == Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "ETC,";
                    break;
                }
            }

            if (changedItems.Length > 0)
                changedItems = changedItems.Substring(0, changedItems.Length - 1);

            return changedItems;
        }

        //SPC-629 by Louis you
        public LinkedList Paste(SPCCopySpec popup, string targetChartID, string mainYN, string userRawID)
        {
            bool hasSubConfigs = false;
            if(this._ws.GetTheNumberOfSubConfigOfModel(targetChartID) > 0)
                hasSubConfigs = true;

            LinkedList llstConfigurationInfo = new LinkedList();
            llstConfigurationInfo.Add(Definition.DynamicCondition_Condition_key.USER_ID, userRawID);
            llstConfigurationInfo.Add(Definition.CONDITION_KEY_MAIN_YN, mainYN);
            llstConfigurationInfo.Add(Definition.CONDITION_KEY_HAS_SUBCONFIG, hasSubConfigs);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID, popup.CONFIG_RAWID);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID, targetChartID);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INTERLOCK, popup.CONTEXT_INTERLOCK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT, popup.CONTEXT_USE_EXTERNAL_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION, popup.CONTEXT_AUTO_CALCULATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART, popup.CONTEXT_AUTO_GENERATE_SUB_CHART);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE, popup.CONTEXT_ACTIVE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT, popup.CONTEXT_SAMPLE_COUNT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE, popup.CONTEXT_MANAGE_TYPE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING, popup.CONTEXT_AUTO_SETTING);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK,
                popup.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION, 
                popup.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE, popup.CONTEXT_USE_NORMALIZATION_VALUE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN, popup.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MODE, popup.CONTEXT_MODE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION, popup.CONTEXT_CHART_DESCRIPTION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION, popup.CONTEXT_CONTEXT_INFORMATION); //SPC-1218, KBLEE


            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT, popup.RULE_MASTER_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RAW, popup.RULE_RAW);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MEAN, popup.RULE_MEAN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_STD, popup.RULE_STD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RANGE, popup.RULE_RANGE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_MEAN, popup.RULE_EWMA_MEAN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_STD, popup.RULE_EWMA_STD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_RANGE, popup.RULE_EWMA_RANGE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MA, popup.RULE_MA);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MS, popup.RULE_MS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MR, popup.RULE_MR);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT, popup.RULE_TECHNICAL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MOVING_OPTIONS, popup.RULE_MOVING_OPTIONS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MEAN_OPTIONS, popup.RULE_MEAN_OPTIONS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_A, popup.RULE_ZONE_A);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_B, popup.RULE_ZONE_B);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_C, popup.RULE_ZONE_C);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RULE_SELECTION, popup.RULE_RULE_SELECTION);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY, popup.OPTION_PARAMETER_CATEGORY);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_CALCULATE_PPK, popup.OPTION_CALCULATE_PPK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_PRIORITY, popup.OPTION_PRIORITY);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS, popup.OPTION_SAMPLE_COUNTS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_DAYS, popup.OPTION_DAYS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW, popup.OPTION_DEFAULT_CHART_TO_SHOW);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD, popup.AUTO_AUTO_CALCULATION_PERIOD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT, popup.AUTO_AUTO_CALCULATION_COUNT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE, popup.AUTO_MINIMUM_SAMPLES_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD, popup.AUTO_DEFAULT_PERIOD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE, popup.AUTO_MAXIMUM_PERIOD_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE, popup.AUTO_CONTROL_LIMIT_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD, popup.AUTO_CONTROL_LIMIT_THREASHOLD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION, 
                popup.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER, popup.AUTO_CALCULATION_WITHOUT_IQR_FILTER);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT, popup.AUTO_RAW_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT, popup.AUTO_MEAN_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT, popup.AUTO_STD_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT, popup.AUTO_RANGE_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT, popup.AUTO_EWMA_MEAN_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT, popup.AUTO_EWMA_STD_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT, popup.AUTO_EWMA_RANGE_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT, popup.AUTO_MA_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT, popup.AUTO_MS_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT, popup.AUTO_MR_CONTROL_LIMIT);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN, popup.AUTO_THRESHOLD_FUNTION);

            //SPC-1129 BY STELLA
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN, popup.USE_GLOBAL_YN);
            //SPC-977 BY STELA
            llstConfigurationInfo.Add(COLUMN.CHANGED_ITEMS, this.CompareCopyModel(llstConfigurationInfo));
            return llstConfigurationInfo;

            //DataSet dsResult = _ws.CopyModelInfo(llstConfigurationInfo.GetSerialData());

            //if(dsResult == null)
            //    return "There is no item to paste";
            //if(DSUtil.GetResultSucceed(dsResult) == 0)
            //    return "FAIL";

            //return "SUCCESS";
        }

        public DialogResult Modify(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId, string mainYN, string groupName)
        {
            SPCConfigurationPopup spcConfigPopup = new SPCConfigurationPopup();
            spcConfigPopup.SESSIONDATA = sessionData;
            spcConfigPopup.URL = URL;
            spcConfigPopup.PORT = Port;
            spcConfigPopup.CONFIG_MODE = ConfigMode.MODIFY;
            spcConfigPopup.LINE_RAWID = line;
            spcConfigPopup.AREA_RAWID = area;
            spcConfigPopup.EQP_MODEL = eqpModel;
            spcConfigPopup.CONFIG_RAWID = chartId;
            spcConfigPopup.MAIN_YN = mainYN;
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

            spcConfigPopup.InitializePopup();

            return spcConfigPopup.ShowDialog();
        }

        

        public int GetRowIndex(string name)
        {
            if(this.rowIndex.ContainsKey(name))
                return this.rowIndex[name];

            return -1;
        }

        public DialogResult ViewModel(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId, string mainYN, string version, string groupName)
        {
            throw new NotImplementedException();
        }

        //SPC-855
        public DataSet GetSourseConfigSpecData(string SourceRawid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = _ws.GetSourseConfigSpecData(SourceRawid);
            }
            catch
            { 
            }
            return dsResult;
        }
        public DataSet GetTargetConfigSpecData(string[] targetRawid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = _ws.GetTargetConfigSpecData(targetRawid);
            }
            catch
            {
            }
            return dsResult;
        }

        public LinkedList GetGroupNameByChartID(string chartId)
        {
            LinkedList llGroupName = new LinkedList();
            DataSet dsGroupName = new DataSet();
            dsGroupName = _ws.GetGroupNameByChartId(chartId);

            if (dsGroupName.Tables.Count > 0 && dsGroupName.Tables[0].Rows.Count > 0)
            {
                llGroupName.Add(COLUMN.GROUP_NAME, dsGroupName.Tables[0].Rows[0][COLUMN.GROUP_NAME].ToString());
            }

            return llGroupName;
        }

    }
}