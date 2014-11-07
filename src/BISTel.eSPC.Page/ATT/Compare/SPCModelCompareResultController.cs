using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Modeling;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.Compare
{
    internal class SPCModelCompareResultController : ICompareResultController
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private DataSet dsData = null;
        internal Dictionary<string, int> rowIndex = new Dictionary<string, int>();
        private static Dictionary<string, string> specColumnName = new Dictionary<string, string>();
        private static Dictionary<string, string> autocalcColumnName = new Dictionary<string, string>();

        internal SPCModelCompareResultController()
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
            dsData = this._ws.GetATTSPCSpecAndRule((string[])llCondition["CHART_ID"]);

            DataTable dtData = MakeDataTableForSpread(dsData);
            return dtData;
        }

        private DataTable MakeDataTableForSpread(DataSet dsData)
        {
            DataTable comparedTable = new DataTable();

            AddColumns(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.CHART_VW_SPC]);

            AddChartInformation(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.CHART_VW_SPC]);

            AddSpec(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC]);

            AddRuleList(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC]);

            AddAutocalc(comparedTable, dsData.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC]);

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
            foreach(DataRow dr in original.Rows)
            {
                string chartID = dr[chartIDColumnName].ToString();

                target[chartID] = dr[addedValueColumnName].ToString();
            }
        }
        //SPC-629
        public string PasteModel(LinkedList pasteModelList)
        {
            DataSet dsResult = _ws.ATTCopyModelInfo(pasteModelList.GetSerialData());

            if (dsResult == null)
            {
                return Definition.MSG_KEY_NO_PASTE_ITEM;
            }
            if (DSUtil.GetResultSucceed(dsResult) == 0)
            {
                return "FAIL";
            }

            return "SUCCESS";
        }

        //SPC-629 by Louis you
        public LinkedList Paste(SPCCopySpec popup, string targetChartID, string mainYN, string userRawID)
        {
            bool hasSubConfigs = false;

            if (this._ws.GetTheNumberOfATTSubConfigOfModel(targetChartID) > 0) //SPC-1306, KBLEE
            {
                hasSubConfigs = true;
            }

            LinkedList llstConfigurationInfo = new LinkedList();
            llstConfigurationInfo.Add(Definition.DynamicCondition_Condition_key.USER_ID, userRawID);
            llstConfigurationInfo.Add(Definition.CONDITION_KEY_MAIN_YN, mainYN);
            llstConfigurationInfo.Add(Definition.CONDITION_KEY_HAS_SUBCONFIG, hasSubConfigs);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID, popup.CONFIG_RAWID);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID, targetChartID);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INTERLOCK, popup.CONTEXT_INTERLOCK);
            //llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT, popup.CONTEXT_USE_EXTERNAL_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION, popup.CONTEXT_AUTO_CALCULATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART, popup.CONTEXT_AUTO_GENERATE_SUB_CHART);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE, popup.CONTEXT_ACTIVE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT, popup.CONTEXT_SAMPLE_COUNT);
            //llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE, popup.CONTEXT_MANAGE_TYPE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING, popup.CONTEXT_AUTO_SETTING);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK,
                popup.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION, 
                popup.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
            //llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE, popup.CONTEXT_USE_NORMALIZATION_VALUE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN, popup.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MODE, popup.CONTEXT_MODE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION, popup.CONTEXT_CHART_DESCRIPTION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION, popup.CONTEXT_CONTEXT_INFORMATION); //SPC-1218, KBLEE

            //SPC-1218, KBLEE, START
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT, popup.RULE_PN_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_C_SPEC_LIMIT, popup.RULE_C_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_PN_CONTROL, popup.RULE_PN_CONTROL);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_C_CONTROL, popup.RULE_C_CONTROL);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_P_CONTROL, popup.RULE_P_CONTROL);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_U_CONTROL, popup.RULE_U_CONTROL);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RULE_SELECTION, popup.RULE_RULE_SELECTION);
            //SPC-1218, KBLEE, END

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
            //SPC-1218, KBLEE, START
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_PN_CONTROL_LIMIT, popup.AUTO_PN_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_P_CONTROL_LIMIT, popup.AUTO_P_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_C_CONTROL_LIMIT, popup.AUTO_C_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_U_CONTROL_LIMIT, popup.AUTO_U_CONTROL_LIMIT);
            //SPC-1218, KBLEE, END
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION, 
                popup.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER, popup.AUTO_CALCULATION_WITHOUT_IQR_FILTER);


            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN, popup.AUTO_THRESHOLD_FUNTION);
            //llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_INITIAL_COUNT, popup.auto_);

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
            spcConfigPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.MODIFY;
            spcConfigPopup.LINE_RAWID = line;
            spcConfigPopup.AREA_RAWID = area;
            spcConfigPopup.EQP_MODEL = eqpModel;
            spcConfigPopup.CONFIG_RAWID = chartId;
            spcConfigPopup.MAIN_YN = mainYN;
            spcConfigPopup.GROUP_NAME = groupName; //SPC-1292, KBLEE

            if(_ws.GetTheNumberOfATTSubConfigOfModel(chartId) > 0)
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
        public DataSet GetATTSourseConfigSpecData(string SourceRawid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = _ws.GetATTSourseConfigSpecData(SourceRawid);
            }
            catch
            { 
            }
            return dsResult;
        }
        public DataSet GetATTTargetConfigSpecData(string[] targetRawid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = _ws.GetATTTargetConfigSpecData(targetRawid);
            }
            catch
            {
            }
            return dsResult;
        }

        //SPC-1292, KBLEE
        public LinkedList GetGroupNameByChartID(string chartId)
        {
            LinkedList llGroupName = new LinkedList();
            DataSet dsGroupName = new DataSet();
            dsGroupName = _ws.GetATTGroupNameByChartId(chartId);

            if (dsGroupName.Tables.Count > 0 && dsGroupName.Tables[0].Rows.Count > 0)
            {
                llGroupName.Add(BISTel.eSPC.Common.COLUMN.GROUP_NAME, dsGroupName.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString());
            }

            return llGroupName;
        }
    }
}