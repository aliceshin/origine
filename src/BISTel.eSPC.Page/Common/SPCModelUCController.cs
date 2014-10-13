using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Common
{
    public class SPCModelUCController
    {
        internal eSPCWebService.eSPCWebService _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        internal DataSet modelData = null;
        internal Dictionary<string, SPCModel> spcModels = null;

        //internal SPCModel[] SpcModels
        //{
        //    get 
        //    { 
        //        SPCModel[] models = new SPCModel[spcModels.Count];
        //        spcModels.Values.CopyTo(models, 0);
        //        return models;
        //    }
        //}

        internal DataSet GetModelData(string[] modelRawids, bool useComma)
        {
            modelData = _ws.GetSPCModelsData(modelRawids, useComma);

            spcModels = new Dictionary<string, SPCModel>();

            foreach(DataRow dr in modelData.Tables[TABLE.MODEL_MST_SPC].Rows)
            {
                SPCModel spcModel = new SPCModel
                                        {
                                            SPCModelRawID = dr[COLUMN.RAWID].ToString(),
                                            SPCModelName = dr[COLUMN.SPC_MODEL_NAME].ToString(),
                                        };
                spcModels.Add(spcModel.SPCModelRawID, spcModel);
            }

            foreach(var kvp in spcModels)
            {
                DataRow[] drs = modelData.Tables[TABLE.MODEL_CONFIG_MST_SPC].Select(COLUMN.MODEL_RAWID + " = '" + kvp.Key + "'");
                if(drs.Length == 0)
                    continue;

                kvp.Value.ParamType = drs[0][COLUMN.PARAM_TYPE_CD].ToString();
                
                kvp.Value.SubModels = new List<SPCModel>();

                foreach(DataRow dr in drs)
                {
                    if(dr[COLUMN.MAIN_YN].ToString().ToUpper() == "Y")
                    {
                        kvp.Value.ChartID = dr[COLUMN.RAWID].ToString();
                        kvp.Value.Version = dr[COLUMN.VERSION].ToString();
                        kvp.Value.IsMainModel = true;
                        continue;
                    }

                    SPCModel spcModel = new SPCModel
                                            {
                                                ChartID = dr[COLUMN.RAWID].ToString(),
                                                SPCModelRawID = kvp.Value.SPCModelRawID,
                                                SPCModelName = kvp.Value.SPCModelName,
                                                Version = dr[COLUMN.VERSION].ToString(),
                                                IsMainModel = false,
                                                ParamType = kvp.Value.ParamType
                                            };
                    kvp.Value.SubModels.Add(spcModel);
                }
            }

            return modelData;
        }

        public virtual void MakeColumnsForBinding(DataTable dt, DataSet _ds)
        {
            DataTable dtConfig = _ds.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            DataTable dtContext = _ds.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

            //#01. SPC Model Chart List를 위한 Datatable 생성
            foreach(string s in GetSPCModelBasicColumnNames())
            {
                dt.Columns.Add(s);
            }

            //CONTEXT COLUMN 생성
            DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);

            if (drConfigs != null && drConfigs.Length > 0)
            {
                DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                foreach (DataRow drMainContext in drMainContexts)
                {
                    dt.Columns.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                }
            }

            //MODEL 정보 COLUMN 생성
            dt.Columns.Add("RULE_LIST");
        }

        public virtual string[] GetSPCModelBasicColumnNames()
        {
            return new string[]
            {
                COLUMN.CHART_ID,
                COLUMN.MAIN_YN,
                "MODE"
            }
            ;
        }

        public virtual void AddRowsForBinding(DataTable dt , DataSet ds, bool useComma)
        {
            Dictionary<string, string> modeCodeData = new Dictionary<string, string>();
            LoadChartCodeMaster(modeCodeData);

            DataTable dtConfig = ds.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            DataTable dtContext = ds.Tables[TABLE.MODEL_CONTEXT_MST_SPC];
            DataTable dtRuleMst = ds.Tables[TABLE.MODEL_RULE_MST_SPC];

            foreach (DataRow drConfig in dtConfig.Rows)
            {
                DataRow drChartList = dt.NewRow();

                AddRowDataFromConfigRow(drConfig, drChartList, modeCodeData);

                DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                foreach (DataRow drContext in drContexts)
                {
                    if (!dt.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                        dt.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                    drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();

                    //RULE LIST
                    DataRow[] drRuleMsts = dtRuleMst.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    if (drRuleMsts != null && drRuleMsts.Length > 0)
                    {
                        for (int i = 0; i < drRuleMsts.Length; i++)
                        {
                            if (i == 0)
                                drChartList["RULE_LIST"] = string.Format("Rule{0}", drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                            else
                            {
                                if(useComma)
                                    drChartList["RULE_LIST"] = string.Format("{0},Rule{1}", drChartList["RULE_LIST"], drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                                else
                                    drChartList["RULE_LIST"] = string.Format("{0};Rule{1}", drChartList["RULE_LIST"], drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                            }
                        }
                    }
                }

                dt.Rows.Add(drChartList);
            }
        }
        
        public virtual void AddRowDataFromConfigRow(DataRow configRow, DataRow newRow, Dictionary<string, string> codeData)
        {
            newRow[COLUMN.CHART_ID] = configRow[COLUMN.RAWID].ToString();
            newRow[COLUMN.MAIN_YN] = configRow[COLUMN.MAIN_YN].ToString();
            string modeValue = configRow[COLUMN.CHART_MODE_CD].ToString();
            if (codeData.ContainsKey(modeValue))
            {
                modeValue = codeData[modeValue];
            }
            newRow["MODE"] = modeValue;
        }

        public void LoadChartCodeMaster(Dictionary<string, string> modeCodeData)
        {
            // Loading chart code master data 
            LinkedList llCondition = new LinkedList();
            llCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
            DataSet _dsChartMode = this._ws.GetCodeData(llCondition.GetSerialData());

            if(_dsChartMode != null && _dsChartMode.Tables.Count > 0)
            {
                foreach (DataRow dr in _dsChartMode.Tables[0].Rows)
                {
                    modeCodeData.Add(dr[COLUMN.CODE].ToString(), dr[COLUMN.NAME].ToString());
                }
            }
        }
    }
}
