using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.Common
{
    public class SPCModelUCController
    {
        internal eSPCWebService.eSPCWebService _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        internal DataSet modelData = null;
        internal Dictionary<string, BISTel.eSPC.Page.Common.SPCModel> spcModels = null;

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

            spcModels = new Dictionary<string, BISTel.eSPC.Page.Common.SPCModel>();

            foreach(DataRow dr in modelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows)
            {
                BISTel.eSPC.Page.Common.SPCModel spcModel = new BISTel.eSPC.Page.Common.SPCModel
                                        {
                                            SPCModelRawID = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString(),
                                            SPCModelName = dr[BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString(),
                                        };
                spcModels.Add(spcModel.SPCModelRawID, spcModel);
            }

            foreach(var kvp in spcModels)
            {
                DataRow[] drs = modelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Select(BISTel.eSPC.Common.COLUMN.MODEL_RAWID + " = '" + kvp.Key + "'");
                if(drs.Length == 0)
                    continue;

                kvp.Value.ParamType = drs[0][BISTel.eSPC.Common.COLUMN.PARAM_TYPE_CD].ToString();
                
                kvp.Value.SubModels = new List<BISTel.eSPC.Page.Common.SPCModel>();

                foreach(DataRow dr in drs)
                {
                    if(dr[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString().ToUpper() == "Y")
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

            return modelData;
        }

        public virtual void MakeColumnsForBinding(DataTable dt, DataSet _ds)
        {
            DataTable dtConfig = _ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
            DataTable dtContext = _ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];

            //#01. SPC Model Chart List를 위한 Datatable 생성
            foreach(string s in GetSPCModelBasicColumnNames())
            {
                dt.Columns.Add(s);
            }

            //CONTEXT COLUMN 생성
            DataRow[] drConfigs = dtConfig.Select(BISTel.eSPC.Common.COLUMN.MAIN_YN + " = 'Y'", BISTel.eSPC.Common.COLUMN.RAWID);

            if (drConfigs != null && drConfigs.Length > 0)
            {
                DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][BISTel.eSPC.Common.COLUMN.RAWID]), BISTel.eSPC.Common.COLUMN.KEY_ORDER);

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
                BISTel.eSPC.Common.COLUMN.CHART_ID,
                BISTel.eSPC.Common.COLUMN.MAIN_YN,
                "MODE"
            }
            ;
        }

        public virtual void AddRowsForBinding(DataTable dt , DataSet ds, bool useComma)
        {
            Dictionary<string, string> modeCodeData = new Dictionary<string, string>();
            LoadChartCodeMaster(modeCodeData);

            DataTable dtConfig = ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
            DataTable dtContext = ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];
            DataTable dtRuleMst = ds.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC];

            foreach (DataRow drConfig in dtConfig.Rows)
            {
                DataRow drChartList = dt.NewRow();

                AddRowDataFromConfigRow(drConfig, drChartList, modeCodeData);

                DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfig[BISTel.eSPC.Common.COLUMN.RAWID]));

                foreach (DataRow drContext in drContexts)
                {
                    if (!dt.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                        dt.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                    drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();

                    //RULE LIST
                    DataRow[] drRuleMsts = dtRuleMst.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfig[BISTel.eSPC.Common.COLUMN.RAWID]));

                    if (drRuleMsts != null && drRuleMsts.Length > 0)
                    {

                        for (int i = 0; i < drRuleMsts.Length; i++)
                        {
                            if (i == 0)
                                drChartList["RULE_LIST"] = string.Format("Rule{0}", drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                            else
                            {
                                if (useComma)
                                    drChartList["RULE_LIST"] = string.Format("{0},Rule{1}", drChartList["RULE_LIST"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                                else
                                    drChartList["RULE_LIST"] = string.Format("{0};Rule{1}", drChartList["RULE_LIST"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                            }
                        }
                    }
                }

                dt.Rows.Add(drChartList);
            }
        }
        
        public virtual void AddRowDataFromConfigRow(DataRow configRow, DataRow newRow, Dictionary<string, string> codeData)
        {
            newRow[BISTel.eSPC.Common.COLUMN.CHART_ID] = configRow[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
            newRow[BISTel.eSPC.Common.COLUMN.MAIN_YN] = configRow[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
            string modeValue = configRow[BISTel.eSPC.Common.COLUMN.CHART_MODE_CD].ToString();
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
                    modeCodeData.Add(dr[BISTel.eSPC.Common.COLUMN.CODE].ToString(), dr[BISTel.eSPC.Common.COLUMN.NAME].ToString());
                }
            }
        }
    }
}
