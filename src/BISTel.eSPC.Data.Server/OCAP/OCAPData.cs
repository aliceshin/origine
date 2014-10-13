using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.OCAP
{
    public class OCAPData : DataBase
    {


        #region ::: SELECT
        public DataSet GetOCAPCommentList(string[] OOCRawID)
        {
            string query = "SELECT RAWID, OOC_PROBLEM, OOC_CAUSE, OOC_SOLUTION, OOC_COMMENT, NVL(FALSE_ALARM_YN, 'N') as FALSE_ALARM_YN FROM OOC_TRX_SPC WHERE RAWID in ({0})";

            DataSet ds = new DataSet();
            try
            {
                StringBuilder rawIds = new StringBuilder();
                for(int i=0; i < OOCRawID.Length; i += 1000)
                {
                    for(int j=0; j<1000 && i+j < OOCRawID.Length; j++)
                    {
                        if (rawIds.Length == 0)
                        {
                            rawIds.Append("'" + OOCRawID[i + j] + "'");
                        }
                        else
                        {
                            rawIds.Append(", '" + OOCRawID[i + j] + "'");
                        }
                    }

                    DataSet temp = base.Query(string.Format(query, rawIds.ToString()));
                    ds.Merge(temp);
                    rawIds = new StringBuilder();
                }

                return ds;
            }
            catch
            {
            }
            return null;
        }

        public DataSet GetOCAPCommentList_New(byte[] baData)
        {
            string query = "SELECT RAWID, OOC_PROBLEM, OOC_CAUSE, OOC_SOLUTION, OOC_COMMENT, NVL(FALSE_ALARM_YN, 'N') as FALSE_ALARM_YN FROM OOC_TRX_SPC WHERE RAWID in ({0})";
            query += " AND OOC_DTTS>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')";
            query += " AND OOC_DTTS<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')";

            LinkedList llstData = new LinkedList();
            LinkedList llstCondition = new LinkedList();
            llstData.SetSerialData(baData);

            string[] OOCRawID = null;

            if (llstData[Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID] != null)
            {
                OOCRawID = (string[])llstData[Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID];
            }

            if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
            {
                llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
            }

            if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
            {
                llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
            }

            DataSet ds = new DataSet();
            try
            {
                StringBuilder rawIds = new StringBuilder();
                for (int i = 0; i < OOCRawID.Length; i += 1000)
                {
                    for (int j = 0; j < 1000 && i + j < OOCRawID.Length; j++)
                    {
                        if (rawIds.Length == 0)
                        {
                            rawIds.Append("'" + OOCRawID[i + j] + "'");
                        }
                        else
                        {
                            rawIds.Append(", '" + OOCRawID[i + j] + "'");
                        }
                    }

                    DataSet temp = base.Query(string.Format(query, rawIds.ToString()), llstCondition);
                    ds.Merge(temp);
                    rawIds = new StringBuilder();
                }

                return ds;
            }
            catch
            {
            }
            return null;
        }

        private const string SQL_GET_FAULT_REPORT_SYSTEM = @"SELECT *
                                                              FROM dispatcher_mst_dcp
                                                             WHERE dictionary_rawid = (SELECT rawid
                                                                                         FROM dictionary_mst_dcp
                                                                                        WHERE e_field_name = 'FAULT_REPORT')
                                                               AND application_name = 'COM'";
        public DataSet GetOCAPListData(byte[] baData)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            //string strWhere1 = string.Empty;


            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                string sOCAP_OOC = llstData[Definition.CONDITION_SEARCH_KEY_OCAP_OOC].ToString();

                // JIRA SPC-617 OCAP List performance over 100 sec - 2011.10.11 by ANDREW KO
                if (sOCAP_OOC == "OCAP")
                {
                    bool isComApplied = false;
                    ds = base.Query(SQL_GET_FAULT_REPORT_SYSTEM, llstCondition);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        isComApplied = true;
                    }

                    if (llstData.Contains(Definition.DynamicCondition_Search_key.CHART_ID) && llstData[Definition.DynamicCondition_Search_key.CHART_ID] != null)
                    {
                        if (isComApplied)
                        {
                            sqlQuery = @"SELECT /*+ ORDERED  USE_NL(mms, ots, ortc ) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX)  INDEX(ortc IDX_OCAP_RESULT_TRX_COM_PK ) */
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' as ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid, 
                                                   mms.spc_model_name, mcms.param_alias,
                                                   NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                              FROM area_mst_pp amp,
                                                   model_mst_spc mms,
                                                   model_config_mst_spc mcms,
                                                   model_config_opt_mst_spc mcoms
                                             WHERE mcms.rawid = :RAWID
                                               AND mms.area_rawid = amp.rawid
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots,
                                           ocap_result_trx_com ortc
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.rawid = ortc.ooc_rawid
                                       AND ots.model_config_rawid = ortc.application_model_rawid
                                       AND ortc.application_name = 'SPC'
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ortc.ooc_dtts >=
                                                 TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ortc.ooc_dtts <=
                                                 TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ";
                        }
                        else 
                        {
                            sqlQuery = @"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX)  INDEX(octs IDX_OCAP_TRX_SPC_UK ) */
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid, 
                                                   mms.spc_model_name, mcms.param_alias,
                                                   NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                              FROM area_mst_pp amp,
                                                   model_mst_spc mms,
                                                   model_config_mst_spc mcms,
                                                   model_config_opt_mst_spc mcoms
                                             WHERE mcms.rawid = :RAWID
                                               AND mms.area_rawid = amp.rawid
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots,
                                           ocap_trx_spc octs
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.rawid = octs.ocap_rawid
                                       AND ots.model_config_rawid = octs.model_config_rawid
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts >=
                                                 TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts <=
                                                 TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ";
                        }

                        llstCondition.Add("RAWID", llstData[Definition.DynamicCondition_Search_key.CHART_ID].ToString());

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        ds = base.Query(sqlQuery, llstCondition);
                    }
                    else
                    {
                        if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                        {
                            strWhere2 += string.Format(" amp.area IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                        {
                            strWhere3 += string.Format(" AND mms.location_rawid IN ({0})", llstData[Definition.CONDITION_KEY_LINE_RAWID]);
                        }

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                        {
                            string[] strArrTemp = llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString().Split(',');
                            if (strArrTemp.Length > 1000)
                            {
                                int itemp = 0;
                                string strTemp = "";
                                ArrayList arrsModelRawID = new ArrayList();
                                for (int i = 0; i < strArrTemp.Length; i++)
                                {
                                    strTemp += "," + strArrTemp[i];
                                    itemp++;
                                    if (itemp == 990)
                                    {
                                        arrsModelRawID.Add(strTemp.Substring(1));
                                        itemp = 0;
                                        strTemp = "";
                                    }
                                }
                                if (strTemp.Length > 0)
                                {
                                    arrsModelRawID.Add(strTemp.Substring(1));
                                }

                                for (int k = 0; k < arrsModelRawID.Count; k++)
                                {
                                    strWhere = string.Format(" mms.rawid IN  ({0})", arrsModelRawID[k].ToString());

                                    if (isComApplied)
                                    {
                                        sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, ortc ) 
                                                                   INDEX(ots IDX_OOC_TRX_SPC_IDX)  INDEX(ortc IDX_OCAP_RESULT_TRX_COM_PK ) */
                                                                   'false' AS V_SELECT,
                                                                   mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                                                   ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                                                   ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                                                   ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                                                   mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' as ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                                              FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                                               index(mms IDX_MODEL_MST_SPC_UK)  
                                                                               index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                                               index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                                           mcms.rawid AS mcms_rawid, 
                                                                           mms.spc_model_name, mcms.param_alias,
                                                                           NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                                           amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                                                      FROM area_mst_pp amp,
                                                                           model_mst_spc mms,
                                                                           model_config_mst_spc mcms,
                                                                           model_config_opt_mst_spc mcoms
                                                                     WHERE {1}
                                                                       AND mms.area_rawid = amp.rawid
                                                                       {2}
                                                                       AND {0}
                                                                       AND mcms.model_rawid = mms.rawid
                                                                       AND mcoms.model_config_rawid = mcms.rawid) mms,
                                                                   ooc_trx_spc ots,
                                                                   ocap_result_trx_com ortc
                                                             WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                                               AND ots.rawid = ortc.ooc_rawid
                                                               AND ots.model_config_rawid = ortc.application_model_rawid
                                                               AND ortc.application_name = 'SPC'
                                                               AND ots.ooc_dtts >=
                                                                         TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ots.ooc_dtts <=
                                                                         TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ortc.ooc_dtts >=
                                                                         TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ortc.ooc_dtts <=
                                                                         TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                             ", strWhere, strWhere2, strWhere3);
                                    }
                                    else
                                    {
                                        sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                                                   INDEX(ots IDX_OOC_TRX_SPC_IDX)  INDEX(octs IDX_OCAP_TRX_SPC_UK ) */ 
                                                                   'false' AS V_SELECT,
                                                                   mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                                                   ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                                                   octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                                                   ots.ooc_solution,  nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area,
                                                                   mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                                              FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                                               index(mms IDX_MODEL_MST_SPC_UK)  
                                                                               index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                                               index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                                           mcms.rawid AS mcms_rawid, 
                                                                           mms.spc_model_name, mcms.param_alias,
                                                                           NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                                           amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                                                      FROM area_mst_pp amp,
                                                                           model_mst_spc mms,
                                                                           model_config_mst_spc mcms,
                                                                           model_config_opt_mst_spc mcoms
                                                                     WHERE {1}
                                                                       AND mms.area_rawid = amp.rawid
                                                                       {2}
                                                                       AND {0}
                                                                       AND mcms.model_rawid = mms.rawid
                                                                       AND mcoms.model_config_rawid = mcms.rawid
                                                                       ) mms,
                                                                   ooc_trx_spc ots,
                                                                   ocap_trx_spc octs
                                                             WHERE ots.model_config_rawid = mms.mcms_rawid
                                                               AND ots.rawid = octs.ocap_rawid
                                                               AND ots.model_config_rawid = octs.model_config_rawid
                                                               AND ots.ooc_dtts >=
                                                                         TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ots.ooc_dtts <=
                                                                         TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND octs.ocap_dtts >=
                                                                         TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND octs.ocap_dtts <=
                                                                         TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                             ", strWhere, strWhere2, strWhere3);
                                    }
                                    

                                    DataSet dsTemp = base.Query(sqlQuery, llstCondition);

                                    ds.Merge(dsTemp);
                                }
                            }
                            else
                            {
                                strWhere = string.Format(" mms.rawid IN  ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                                if (isComApplied)
                                {
                                    sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, ortc ) 
                                                                   INDEX(ots IDX_OOC_TRX_SPC_IDX)  INDEX(ortc IDX_OCAP_RESULT_TRX_COM_PK ) */
                                                                   'false' AS V_SELECT,
                                                                   mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                                                   ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                                                   ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                                                   ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                                                   mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' as ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                                              FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                                               index(mms IDX_MODEL_MST_SPC_UK)  
                                                                               index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                                               index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                                           mcms.rawid AS mcms_rawid, 
                                                                           mms.spc_model_name, mcms.param_alias,
                                                                           NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                                           amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                                                      FROM area_mst_pp amp,
                                                                           model_mst_spc mms,
                                                                           model_config_mst_spc mcms,
                                                                           model_config_opt_mst_spc mcoms
                                                                     WHERE {1}
                                                                       AND mms.area_rawid = amp.rawid
                                                                       {2}
                                                                       AND {0}
                                                                       AND mcms.model_rawid = mms.rawid
                                                                       AND mcoms.model_config_rawid = mcms.rawid) mms,
                                                                   ooc_trx_spc ots,
                                                                   ocap_result_trx_com ortc
                                                             WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                                               AND ots.rawid = ortc.ooc_rawid
                                                               AND ots.model_config_rawid = ortc.application_model_rawid
                                                               AND ortc.application_name = 'SPC'
                                                               AND ots.ooc_dtts >=
                                                                         TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ots.ooc_dtts <=
                                                                         TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ortc.ooc_dtts >=
                                                                         TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                               AND ortc.ooc_dtts <=
                                                                         TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                             ", strWhere, strWhere2, strWhere3);
                                }
                                else
                                {
                                    sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                                               INDEX(ots IDX_OOC_TRX_SPC_IDX)  IDEX(octs IDX_OCAP_TRX_SPC_UK ) */ 
                                                               'false' AS V_SELECT,
                                                               mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                                               ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                                               octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                                               ots.ooc_solution,  nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area, 
                                                               mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                                          FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                                           index(mms IDX_MODEL_MST_SPC_UK)  
                                                                           index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                                           index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                                       mcms.rawid AS mcms_rawid, 
                                                                       mms.spc_model_name, mcms.param_alias,
                                                                       NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                                       amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                                                  FROM area_mst_pp amp,
                                                                       model_mst_spc mms,
                                                                       model_config_mst_spc mcms,
                                                                       model_config_opt_mst_spc mcoms
                                                                 WHERE {1}
                                                                   AND mms.area_rawid = amp.rawid
                                                                   {2}
                                                                   AND {0}
                                                                   AND mcms.model_rawid = mms.rawid
                                                                   AND mcoms.model_config_rawid = mcms.rawid
                                                                   ) mms,
                                                               ooc_trx_spc ots,
                                                               ocap_trx_spc octs
                                                         WHERE ots.model_config_rawid = mms.mcms_rawid
                                                           AND ots.rawid = octs.ocap_rawid
                                                           AND ots.model_config_rawid = octs.model_config_rawid
                                                           AND ots.ooc_dtts >=
                                                                     TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                           AND ots.ooc_dtts <=
                                                                     TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                                           AND octs.ocap_dtts >=
                                                                     TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                           AND octs.ocap_dtts <=
                                                                     TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                                         ", strWhere, strWhere2, strWhere3);
                                }
                                ds = base.Query(sqlQuery, llstCondition);
                            }
                        }



                    }
                }
                else
                {
                    if (llstData.Contains(Definition.DynamicCondition_Search_key.CHART_ID) && llstData[Definition.DynamicCondition_Search_key.CHART_ID] != null)
                    {
                        sqlQuery = @"SELECT /*+ ORDERED  USE_NL(mms, ots) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area, 
                                           mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias,
                                                   NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                              FROM area_mst_pp amp,
                                                   model_mst_spc mms,
                                                   model_config_mst_spc mcms,
                                                   model_config_opt_mst_spc mcoms
                                             WHERE mcms.rawid = :RAWID
                                               AND mms.area_rawid = amp.rawid
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ";


                        llstCondition.Add("RAWID", llstData[Definition.DynamicCondition_Search_key.CHART_ID].ToString());

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        ds = base.Query(sqlQuery, llstCondition);
                    }
                    else
                    {
                        if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                        {
                            strWhere2 += string.Format(" amp.area IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                        {
                            strWhere3 += string.Format(" AND mms.location_rawid IN ({0})", llstData[Definition.CONDITION_KEY_LINE_RAWID]);
                        }

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                        {
                            string[] strArrTemp = llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString().Split(',');
                            if (strArrTemp.Length > 1000)
                            {
                                int itemp = 0;
                                string strTemp = "";
                                ArrayList arrsModelRawID = new ArrayList();
                                for (int i = 0; i < strArrTemp.Length; i++)
                                {
                                    strTemp += "," + strArrTemp[i];
                                    itemp++;
                                    if (itemp == 990)
                                    {
                                        arrsModelRawID.Add(strTemp.Substring(1));
                                        itemp = 0;
                                        strTemp = "";
                                    }
                                }
                                if (strTemp.Length > 0)
                                {
                                    arrsModelRawID.Add(strTemp.Substring(1));
                                }

                                for (int k = 0; k < arrsModelRawID.Count; k++)
                                {
                                    strWhere = string.Format(" mms.rawid IN  ({0})", arrsModelRawID[k].ToString());

                                    sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area, 
                                           mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias,
                                                   NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                              FROM area_mst_pp amp,
                                                   model_mst_spc mms,
                                                   model_config_mst_spc mcms,
                                                   model_config_opt_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);

                                    DataSet dsTemp = base.Query(sqlQuery, llstCondition);

                                    ds.Merge(dsTemp);
                                }
                            }
                            else
                            {
                                strWhere = string.Format(" mms.rawid IN  ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                                sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots)
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.complex_yn, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area, 
                                           mms.default_chart_list, mms.param_type_cd, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.module_alias, mms.location_rawid, mms.area_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias,
                                                   NVL (mcms.complex_yn, 'N') AS complex_yn, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mcms.param_type_cd, mms.location_rawid, mms.area_rawid
                                              FROM area_mst_pp amp,
                                                   model_mst_spc mms,
                                                   model_config_mst_spc mcms,
                                                   model_config_opt_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);
                                ds = base.Query(sqlQuery, llstCondition);
                            }
                        }
                    }
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtExcel = new DataTable();
                    for (int idx = 0; idx < ds.Tables[0].Columns.Count; idx++)
                    {
                        string sCol = ds.Tables[0].Columns[idx].ColumnName.ToString().ToUpper();
                        if (sCol == "OOC_DTTS")
                        {
                            dtExcel.Columns.Add(sCol, ds.Tables[0].Columns[idx].DataType);
                        }
                        else
                        {
                            dtExcel.Columns.Add(sCol, typeof(string));
                        }
                    }

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dtExcel.ImportRow(dr);
                    }

                    dsResult.Tables.Add("OOC_LIST");
                    dsResult.Tables["OOC_LIST"].Merge(dtExcel);
                }
            }
            catch
            {
            }
            return dsResult;
        }


        public DataSet GetOCAPData(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                sqlQuery = @"SELECT *
                            FROM OOC_TRX_SPC
                            WHERE MODEL_CONFIG_RAWID=:MODEL_CONFIG_RAWID
                            AND OOC_DTTS>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                            AND OOC_DTTS<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')  
                            order by OOC_DTTS
                            ";

                if (llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID] != null)
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]);
                }

                if (llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID] != null)
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]);
                }

                if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                }


                ds = base.Query(sqlQuery, llstCondition);
            }
            catch
            {

            }

            return ds;
        }



        public DataSet GetOCAPDetails(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;
            string ocap_rawid = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID] != null)
                {
                    ocap_rawid = llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();
                }
                else if (llstData[Definition.DynamicCondition_Condition_key.OCAP_DATATABLE] != null)
                {
                    DataTable dtOCAPRawID = (DataTable)llstData[Definition.DynamicCondition_Condition_key.OCAP_DATATABLE];
                    foreach (DataRow dr in dtOCAPRawID.Rows)
                    {
                        string strRawid = dr["ocap_rawid"].ToString();
                        string str = strRawid.Replace(";", "");
                        if (str == ";" || string.IsNullOrEmpty(str)) continue;
                        strRawid = strRawid.Replace(";", ",");

                        if (!string.IsNullOrEmpty(ocap_rawid)) ocap_rawid += ",";
                        if (strRawid.LastIndexOf(",") > -1)
                        {
                            ocap_rawid += strRawid.Substring(0, strRawid.Length - 1);
                        }
                        else
                            ocap_rawid += strRawid;
                    }
                }

                sqlQuery = string.Format(@"select 
                        ots.rawid as OCAP_RAWID,
                        ots.status,
                        ots.ooc_dtts,    
                        ots.rule_no,
                        r.description as rule_desc,
                        ots.ooc_value, 
                        ots.ooc_problem, 
                        ots.ooc_cause, 
                        ots.ooc_solution,
                        ots.context_list,
                        ots.ooc_comment, NVL(ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id
                    from  ooc_trx_spc ots,
                            rule_mst_spc r
                    where ots.rawid in ({0})
                    and ots.rule_no=r.spc_rule_no order by ots.rawid ", ocap_rawid);
                ds = base.Query(sqlQuery, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet FindAnnotationOCAP(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                SortedList llOCAPRAWID = new SortedList();
                string sOCAPRAWID = "";
                llstData.SetSerialData(baData);

                sqlQuery = @"SELECT RAWID
                            FROM OOC_TRX_SPC
                            WHERE RAWID IN ({0})
                            AND MODEL_CONFIG_RAWID=:MODEL_CONFIG_RAWID
                            AND OOC_DTTS>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                            AND OOC_DTTS<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                            AND (OOC_CAUSE IS NOT NULL OR OOC_PROBLEM IS NOT NULL OR OOC_SOLUTION IS NOT NULL OR OOC_COMMENT IS NOT NULL)                        
                            ";

                if (llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID] != null)
                {
                    llOCAPRAWID = (SortedList)llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID];
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }

                if (llOCAPRAWID.Count > 1000)
                {
                    int iTemp = 0;
                    ArrayList arrsOCAPRawID = new ArrayList();

                    for (int i = 0; i < llOCAPRAWID.Count; i++)
                    {
                        sOCAPRAWID += "," + llOCAPRAWID.GetKey(i).ToString();
                        iTemp++;
                        if (iTemp == 990)
                        {
                            arrsOCAPRawID.Add(sOCAPRAWID);
                            iTemp = 0;
                            sOCAPRAWID = "";
                        }
                    }
                    if (sOCAPRAWID.Length > 0)
                    {
                        arrsOCAPRawID.Add(sOCAPRAWID);
                    }

                    for (int j = 0; j < arrsOCAPRawID.Count; j++)
                    {
                        DataSet dsTemp = base.Query(string.Format(sqlQuery, arrsOCAPRawID[j].ToString().Substring(1)), llstCondition);
                        ds.Merge(dsTemp);
                    }
                }
                else
                {
                    for (int i = 0; i < llOCAPRAWID.Count; i++)
                    {
                        sOCAPRAWID += "," + llOCAPRAWID.GetKey(i).ToString();
                    }
                    sOCAPRAWID = sOCAPRAWID.Substring(1);
                    ds = base.Query(string.Format(sqlQuery, sOCAPRAWID), llstCondition);
                }

            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        #endregion

        //SPC-703
        public DataSet GetOOCCommentList(string OCAPRawid)
        {
            DataSet dsResult = new DataSet();
            DataSet dsTemp = new DataSet();
            string strSQL = string.Empty;
            string LocationRawid = "";
            string AreaRawid = "";
            try
            {
                strSQL = string.Format(@"select location_rawid, area_rawid from model_mst_spc 
                                where rawid in (select model_rawid from model_config_mst_spc where rawid = {0})", OCAPRawid);

                dsTemp = base.Query(strSQL);

                if (dsTemp != null)
                {
                    foreach (DataRow dr in dsTemp.Tables[0].Rows)
                    {
                        LocationRawid = dr[0].ToString();
                        AreaRawid = dr[1].ToString();
                    }
                }
                else
                {
                    return dsTemp;
                }


                //OOC_CAUSE_MST_SPC Data
                strSQL = string.Format(@"select * from OOC_CAUSE_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_CAUSE_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_COMMENT_MST_SPC Data
                strSQL = string.Format(@"select * from OOC_COMMENT_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_COMMENT_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_PROBLEM_MST_SPC  Data
                strSQL = string.Format(@"select * from OOC_PROBLEM_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_PROBLEM_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_SOLUTION_MST_SPC  Data
                strSQL = string.Format(@"select * from OOC_SOLUTION_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_SOLUTION_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

            }
            catch
            {
            }

            return dsResult;
        }

        public DataSet getGroupInfoByChartId(string chartId)
        {
            DataSet ds = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.Add(COLUMN.CHART_ID, chartId);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *                                   ");
                sb.Append("FROM MODEL_MST_SPC                         ");
                sb.Append("WHERE RAWID = (SELECT MODEL_RAWID          ");
                sb.Append("                FROM MODEL_CONFIG_MST_SPC  ");
                sb.Append("               WHERE RAWID = :CHART_ID)    ");

                ds = this.Query(sb.ToString(), llparam);

                if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    string groupRawid = ds.Tables[0].Rows[0][COLUMN.GROUP_RAWID].ToString();
                    string locationRawid = ds.Tables[0].Rows[0][COLUMN.LOCATION_RAWID].ToString();
                    string areaRawid = ds.Tables[0].Rows[0][COLUMN.AREA_RAWID].ToString();
                    string eqpModel = string.Empty;

                    if(ds.Tables[0].Rows[0][COLUMN.EQP_MODEL]!=null)
                        eqpModel = ds.Tables[0].Rows[0][COLUMN.EQP_MODEL].ToString();

                    if (string.IsNullOrEmpty(groupRawid))
                    {
                        sb.Remove(0, sb.Length - 1);
                        sb.Append("SELECT * FROM MODEL_GROUP_MST_SPC ");
                        sb.Append("WHERE RAWID IS NULL ");

                        ds = this.Query(sb.ToString());

                        DataRow dr = ds.Tables[0].NewRow();
                        dr[COLUMN.LOCATION_RAWID] = locationRawid;
                        dr[COLUMN.AREA_RAWID] = areaRawid;
                        dr[COLUMN.EQP_MODEL] = eqpModel;
                        dr[COLUMN.GROUP_NAME] = Definition.VARIABLE_UNASSIGNED_MODEL;
                        ds.Tables[0].Rows.Add(dr);

                    }
                    else
                    {
                        sb.Remove(0, sb.Length - 1);
                        sb.Append("SELECT * FROM MODEL_GROUP_MST_SPC ");
                        sb.Append(string.Format("WHERE RAWID = {0} ", groupRawid));

                        ds = this.Query(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ds;
        }

        #region ::: UPDATE

        public bool UpdateOCAPDate(byte[] baData)
        {
            bool bSuccess = false;

            try
            {

                LinkedList llstSave = new LinkedList();
                LinkedList llstWhere = new LinkedList();
                CommonUtility _comUtil = new CommonUtility();
                LinkedList llstData = new LinkedList();
                string strRawID = string.Empty;
                llstData.SetSerialData(baData);

                DataSet dsSave = (DataSet)llstData[Definition.DynamicCondition_Condition_key.OCAP_DATASET];

                if (dsSave.Tables.Contains("MODIFY"))
                {
                    int iSuccess = 0;
                    base.BeginTrans();
                    foreach (DataRow dr in dsSave.Tables["MODIFY"].Rows)
                    {
                        if (llstData[Definition.DynamicCondition_Condition_key.USE_YN].ToString() == Definition.VARIABLE_Y)
                        {
                            ArrayList arrOCAPRawID = (ArrayList)llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID];
                            for (int i = 0; i < arrOCAPRawID.Count; i++)
                            {
                                strRawID = arrOCAPRawID[i].ToString();

                                string strWhere = "WHERE RAWID = :OOC_TRX_SPC_RAWID";

                                llstWhere.Clear();
                                llstSave.Clear();

                                llstWhere.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, strRawID);
                                llstSave.Add(Definition.CONDITION_KEY_OOC_CAUSE, dr[Definition.CONDITION_KEY_OOC_CAUSE].ToString());
                                llstSave.Add(Definition.CONDITION_KEY_OOC_PROBLEM, dr[Definition.CONDITION_KEY_OOC_PROBLEM].ToString());
                                llstSave.Add(Definition.CONDITION_KEY_OOC_SOLUTION, dr[Definition.CONDITION_KEY_OOC_SOLUTION].ToString());
                                llstSave.Add(Definition.CONDITION_KEY_OOC_COMMENT, dr[Definition.CONDITION_KEY_OOC_COMMENT].ToString());
                                llstSave.Add(Definition.CONDITION_KEY_FALSE_ALARM_YN, dr[Definition.CONDITION_KEY_FALSE_ALARM_YN].ToString());
                                llstSave.Add("LAST_UPDATE_DTTS+SYSTIMESTAMP", "");
                                llstSave.Add("LAST_UPDATE_BY", _comUtil.NVL(llstData[Definition.DynamicCondition_Condition_key.USER_ID]));

                                iSuccess = base.Update(Definition.TableName.OOC_TRX_SPC, llstSave, strWhere, llstWhere);
                            }
                        }
                        else
                        {
                            strRawID = dr[Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();

                            string strWhere = "WHERE RAWID = :OOC_TRX_SPC_RAWID";

                            //JIRA SPC-737 MODIFIED BY ENKIM 2012.01.31
                            llstWhere.Clear();
                            llstSave.Clear();
                            //MODIFIED END

                            llstWhere.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, strRawID);
                            llstSave.Add(Definition.CONDITION_KEY_OOC_CAUSE, dr[Definition.CONDITION_KEY_OOC_CAUSE].ToString());
                            llstSave.Add(Definition.CONDITION_KEY_OOC_PROBLEM, dr[Definition.CONDITION_KEY_OOC_PROBLEM].ToString());
                            llstSave.Add(Definition.CONDITION_KEY_OOC_SOLUTION, dr[Definition.CONDITION_KEY_OOC_SOLUTION].ToString());
                            llstSave.Add(Definition.CONDITION_KEY_OOC_COMMENT, dr[Definition.CONDITION_KEY_OOC_COMMENT].ToString());
                            llstSave.Add(Definition.CONDITION_KEY_FALSE_ALARM_YN, dr[Definition.CONDITION_KEY_FALSE_ALARM_YN].ToString());
                            llstSave.Add("LAST_UPDATE_DTTS+SYSTIMESTAMP", "");
                            llstSave.Add("LAST_UPDATE_BY", _comUtil.NVL(llstData[Definition.DynamicCondition_Condition_key.USER_ID]));

                            iSuccess = base.Update(Definition.TableName.OOC_TRX_SPC, llstSave, strWhere, llstWhere);
                        }
                    }

                    if (iSuccess > 0) bSuccess = true;
                    base.Commit();
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;
        }


        #endregion


        #region ::: INSERT

        #endregion
    }

    public class ATTOCAPData : DataBase
    {


        #region ::: SELECT

        public DataSet GetOCAPListData(byte[] baData)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            string strWhere2 = string.Empty;
            string strWhere3 = string.Empty;
            //string strWhere1 = string.Empty;


            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                string sOCAP_OOC = llstData[Definition.CONDITION_SEARCH_KEY_OCAP_OOC].ToString();

                // JIRA SPC-617 OCAP List performance over 100 sec - 2011.10.11 by ANDREW KO
                if (sOCAP_OOC == "OCAP")
                {
                    if (llstData.Contains(Definition.DynamicCondition_Search_key.CHART_ID) && llstData[Definition.DynamicCondition_Search_key.CHART_ID] != null)
                    {
                        sqlQuery = @"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX)  IDEX(octs IDX_OCAP_TRX_SPC_UK ) */
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid, 
                                                   mms.spc_model_name, mcms.param_alias, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE mcms.rawid = :RAWID
                                               AND mms.area_rawid = amp.rawid
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots,
                                           ocap_trx_spc octs
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.rawid = octs.ocap_rawid
                                       AND ots.model_config_rawid = octs.model_config_rawid
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts >=
                                                 TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts <=
                                                 TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ";


                        llstCondition.Add("RAWID", llstData[Definition.DynamicCondition_Search_key.CHART_ID].ToString());

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        ds = base.Query(sqlQuery, llstCondition);
                    }
                    else
                    {
                        //if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                        //{
                        //    strWhere1 += string.Format(" mcms.param_type_cd = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                        //}


                        if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                        {
                            strWhere2 += string.Format(" amp.area IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                        {
                            strWhere3 += string.Format(" AND mms.location_rawid IN ({0})", llstData[Definition.CONDITION_KEY_LINE_RAWID]);
                        }

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                        {
                            string[] strArrTemp = llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString().Split(',');
                            if (strArrTemp.Length > 1000)
                            {
                                int itemp = 0;
                                string strTemp = "";
                                ArrayList arrsModelRawID = new ArrayList();
                                for (int i = 0; i < strArrTemp.Length; i++)
                                {
                                    strTemp += "," + strArrTemp[i];
                                    itemp++;
                                    if (itemp == 990)
                                    {
                                        arrsModelRawID.Add(strTemp.Substring(1));
                                        itemp = 0;
                                        strTemp = "";
                                    }
                                }
                                if (strTemp.Length > 0)
                                {
                                    arrsModelRawID.Add(strTemp.Substring(1));
                                }

                                for (int k = 0; k < arrsModelRawID.Count; k++)
                                {
                                    strWhere = string.Format(" mms.rawid IN  ({0})", arrsModelRawID[k].ToString());

                                    sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX)  IDEX(octs IDX_OCAP_TRX_SPC_UK ) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution,  nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid, 
                                                   mms.spc_model_name, mcms.param_alias, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid
                                               ) mms,
                                           ooc_trx_spc ots,
                                           ocap_trx_spc octs
                                     WHERE ots.model_config_rawid = mms.mcms_rawid
                                       AND ots.rawid = octs.ocap_rawid
                                       AND ots.model_config_rawid = octs.model_config_rawid
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts >=
                                                 TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts <=
                                                 TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);

                                    DataSet dsTemp = base.Query(sqlQuery, llstCondition);

                                    ds.Merge(dsTemp);
                                }
                            }
                            else
                            {
                                strWhere = string.Format(" mms.rawid IN  ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                                sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots, octs ) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX)  IDEX(octs IDX_OCAP_TRX_SPC_UK ) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           octs.ocap_system status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution,  nvl(octs.rule_list, ots.rule_no) rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, octs.ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid, 
                                                   mms.spc_model_name, mcms.param_alias, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid
                                               ) mms,
                                           ooc_trx_spc ots,
                                           ocap_trx_spc octs
                                     WHERE ots.model_config_rawid = mms.mcms_rawid
                                       AND ots.rawid = octs.ocap_rawid
                                       AND ots.model_config_rawid = octs.model_config_rawid
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts >=
                                                 TO_TIMESTAMP (:START_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND octs.ocap_dtts <=
                                                 TO_TIMESTAMP (:END_DTTS, 'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);
                                ds = base.Query(sqlQuery, llstCondition);
                            }
                        }



                    }
                }
                else
                {
                    if (llstData.Contains(Definition.DynamicCondition_Search_key.CHART_ID) && llstData[Definition.DynamicCondition_Search_key.CHART_ID] != null)
                    {
                        sqlQuery = @"SELECT /*+ ORDERED  USE_NL(mms, ots) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE mcms.rawid = :RAWID
                                               AND mms.area_rawid = amp.rawid
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ";


                        llstCondition.Add("RAWID", llstData[Definition.DynamicCondition_Search_key.CHART_ID].ToString());

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        ds = base.Query(sqlQuery, llstCondition);
                    }
                    else
                    {
                        //if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                        //{
                        //    strWhere1 += string.Format(" mcms.param_type_cd = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                        //}


                        if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                        {
                            strWhere2 += string.Format(" amp.area IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                        {
                            strWhere3 += string.Format(" AND mms.location_rawid IN ({0})", llstData[Definition.CONDITION_KEY_LINE_RAWID]);
                        }

                        if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                        {
                            llstCondition.Add("START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                        }

                        if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                        {
                            llstCondition.Add("END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                        {
                            string[] strArrTemp = llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString().Split(',');
                            if (strArrTemp.Length > 1000)
                            {
                                int itemp = 0;
                                string strTemp = "";
                                ArrayList arrsModelRawID = new ArrayList();
                                for (int i = 0; i < strArrTemp.Length; i++)
                                {
                                    strTemp += "," + strArrTemp[i];
                                    itemp++;
                                    if (itemp == 990)
                                    {
                                        arrsModelRawID.Add(strTemp.Substring(1));
                                        itemp = 0;
                                        strTemp = "";
                                    }
                                }
                                if (strTemp.Length > 0)
                                {
                                    arrsModelRawID.Add(strTemp.Substring(1));
                                }

                                for (int k = 0; k < arrsModelRawID.Count; k++)
                                {
                                    strWhere = string.Format(" mms.rawid IN  ({0})", arrsModelRawID[k].ToString());

                                    sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots) 
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */ 
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias,mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);

                                    DataSet dsTemp = base.Query(sqlQuery, llstCondition);

                                    ds.Merge(dsTemp);
                                }
                            }
                            else
                            {
                                strWhere = string.Format(" mms.rawid IN  ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                                sqlQuery = string.Format(@"SELECT /*+ ORDERED  USE_NL(mms, ots)
                                           INDEX(ots IDX_OOC_TRX_SPC_IDX) */
                                           'false' AS V_SELECT,
                                           mms.spc_model_name, mms.param_alias, mms.main_yn,
                                           ots.rawid AS ocap_rawid, ots.ooc_dtts, ots.model_config_rawid,
                                           ots.status, ots.context_list, ots.ooc_problem, ots.ooc_cause,
                                           ots.ooc_solution, ots.rule_no, ots.ooc_value, mms.area,
                                           mms.default_chart_list, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id, '' ocap_rawid_list, NVL (ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, mms.location_rawid
                                      FROM (SELECT /*+ ORDERED   USE_NL( amp, mms, mcms, mcoms ) 
                                                       index(mms IDX_MODEL_MST_SPC_UK)  
                                                       index(mcms IDX_MODEL_MC_MST_PP_SPC_UK_01) 
                                                       index(mcoms IDX_MODEL_CO_MST_SPC_001 ) */
                                                   mcms.rawid AS mcms_rawid,
                                                   mms.spc_model_name, mcms.param_alias, mcms.main_yn,
                                                   amp.area, mcoms.default_chart_list, mms.location_rawid
                                              FROM area_mst_pp amp,
                                                   model_att_mst_spc mms,
                                                   model_config_att_mst_spc mcms,
                                                   model_config_opt_att_mst_spc mcoms
                                             WHERE {1}
                                               AND mms.area_rawid = amp.rawid
                                               {2}
                                               AND {0}
                                               AND mcms.model_rawid = mms.rawid
                                               AND mcoms.model_config_rawid = mcms.rawid) mms,
                                           ooc_trx_spc ots
                                     WHERE ots.model_config_rawid = mms.mcms_rawid                                       
                                       AND ots.ooc_dtts >=
                                                 TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                       AND ots.ooc_dtts <=
                                                 TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                     ", strWhere, strWhere2, strWhere3);
                                ds = base.Query(sqlQuery, llstCondition);
                            }
                        }
                    }
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtExcel = new DataTable();
                    for (int idx = 0; idx < ds.Tables[0].Columns.Count; idx++)
                    {
                        string sCol = ds.Tables[0].Columns[idx].ColumnName.ToString().ToUpper();
                        if (sCol == "OOC_DTTS")
                        {
                            dtExcel.Columns.Add(sCol, ds.Tables[0].Columns[idx].DataType);
                        }
                        else
                        {
                            dtExcel.Columns.Add(sCol, typeof(string));
                        }
                    }

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dtExcel.ImportRow(dr);
                    }

                    dsResult.Tables.Add("OOC_LIST");
                    dsResult.Tables["OOC_LIST"].Merge(dtExcel);
                }
            }
            catch
            {
            }
            return dsResult;
        }


        public DataSet GetOCAPDetails(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;
            string ocap_rawid = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID] != null)
                {
                    ocap_rawid = llstData[Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();
                }
                else if (llstData[Definition.DynamicCondition_Condition_key.OCAP_DATATABLE] != null)
                {
                    DataTable dtOCAPRawID = (DataTable)llstData[Definition.DynamicCondition_Condition_key.OCAP_DATATABLE];
                    foreach (DataRow dr in dtOCAPRawID.Rows)
                    {
                        string strRawid = dr["ocap_rawid"].ToString();
                        string str = strRawid.Replace(";", "");
                        if (str == ";" || string.IsNullOrEmpty(str)) continue;
                        strRawid = strRawid.Replace(";", ",");

                        if (!string.IsNullOrEmpty(ocap_rawid)) ocap_rawid += ",";
                        if (strRawid.LastIndexOf(",") > -1)
                        {
                            ocap_rawid += strRawid.Substring(0, strRawid.Length - 1);
                        }
                        else
                            ocap_rawid += strRawid;
                    }
                }

                sqlQuery = string.Format(@"select 
                        ots.rawid as OCAP_RAWID,
                        ots.status,
                        ots.ooc_dtts,    
                        ots.rule_no,
                        r.description as rule_desc,
                        ots.ooc_value, 
                        ots.ooc_problem, 
                        ots.ooc_cause, 
                        ots.ooc_solution,
                        ots.context_list,
                        ots.ooc_comment, NVL(ots.false_alarm_yn, 'N') as FALSE_ALARM_YN, ots.main_module_id, ots.module_id, ots.eqp_id, ots.lot_id, ots.substrate_id, ots.cassette_slot, ots.operation_id, ots.product_id, ots.recipe_id, ots.step_id
                    from  ooc_trx_spc ots,
                            rule_att_mst_spc r
                    where ots.rawid in ({0})
                    and ots.rule_no=r.spc_rule_no order by ots.rawid ", ocap_rawid);
                ds = base.Query(sqlQuery, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        #endregion

        //SPC-703
        public DataSet GetOOCCommentList(string OCAPRawid)
        {
            DataSet dsResult = new DataSet();
            DataSet dsTemp = new DataSet();
            string strSQL = string.Empty;
            string LocationRawid = "";
            string AreaRawid = "";
            try
            {
                strSQL = string.Format(@"select location_rawid, area_rawid from model_att_mst_spc 
                                where rawid in (select model_rawid from model_config_att_mst_spc where rawid = {0})", OCAPRawid);

                dsTemp = base.Query(strSQL);

                if (dsTemp != null)
                {
                    foreach (DataRow dr in dsTemp.Tables[0].Rows)
                    {
                        LocationRawid = dr[0].ToString();
                        AreaRawid = dr[1].ToString();
                    }
                }
                else
                {
                    return dsTemp;
                }


                //OOC_CAUSE_MST_SPC Data
                strSQL = string.Format(@"select * from OOC_CAUSE_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_CAUSE_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_COMMENT_MST_SPC Data
                strSQL = string.Format(@"select * from OOC_COMMENT_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_COMMENT_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_PROBLEM_MST_SPC  Data
                strSQL = string.Format(@"select * from OOC_PROBLEM_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_PROBLEM_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

                //OOC_SOLUTION_MST_SPC  Data
                strSQL = string.Format(@"select * from OOC_SOLUTION_MST_SPC where location_rawid = {0} and area_rawid = {1} ", LocationRawid, AreaRawid);
                dsTemp = base.Query(strSQL);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.OOC_SOLUTION_MST_SPC;

                    dsResult.Tables.Add(dtContext);
                }

            }
            catch
            {
            }

            return dsResult;
        }

        //SPC-1292, KBLEE
        public DataSet GetGroupInfoByChartId(string chartId)
        {
            DataSet ds = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.Add(COLUMN.CHART_ID, chartId);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT *                                      ");
                sb.Append("FROM MODEL_ATT_MST_SPC                        ");
                sb.Append("WHERE RAWID = (SELECT MODEL_RAWID             ");
                sb.Append("                FROM MODEL_CONFIG_ATT_MST_SPC ");
                sb.Append("               WHERE RAWID = :CHART_ID)       ");

                ds = this.Query(sb.ToString(), llparam);

                if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                {
                    string groupRawid = ds.Tables[0].Rows[0][COLUMN.GROUP_RAWID].ToString();
                    string locationRawid = ds.Tables[0].Rows[0][COLUMN.LOCATION_RAWID].ToString();
                    string areaRawid = ds.Tables[0].Rows[0][COLUMN.AREA_RAWID].ToString();
                    string eqpModel = string.Empty;

                    if (ds.Tables[0].Rows[0][COLUMN.EQP_MODEL] != null)
                        eqpModel = ds.Tables[0].Rows[0][COLUMN.EQP_MODEL].ToString();

                    if (string.IsNullOrEmpty(groupRawid))
                    {
                        sb.Remove(0, sb.Length - 1);
                        sb.Append("SELECT * FROM MODEL_GROUP_ATT_MST_SPC ");
                        sb.Append("WHERE RAWID IS NULL ");

                        ds = this.Query(sb.ToString());

                        DataRow dr = ds.Tables[0].NewRow();
                        dr[COLUMN.LOCATION_RAWID] = locationRawid;
                        dr[COLUMN.AREA_RAWID] = areaRawid;
                        dr[COLUMN.EQP_MODEL] = eqpModel;
                        dr[COLUMN.GROUP_NAME] = Definition.VARIABLE_UNASSIGNED_MODEL;
                        ds.Tables[0].Rows.Add(dr);

                    }
                    else
                    {
                        sb.Remove(0, sb.Length - 1);
                        sb.Append("SELECT * FROM MODEL_GROUP_ATT_MST_SPC ");
                        sb.Append(string.Format("WHERE RAWID = {0} ", groupRawid));

                        ds = this.Query(sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return ds;
        }

        #region ::: UPDATE
        #endregion


        #region ::: INSERT
        #endregion
    }

}
