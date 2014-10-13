using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Data.Server.History
{
    public class SPCModelHistoryData : DataBase
    {
        public string lastQuery = string.Empty;
        
        //spc-977 by stella : save_comment 추가
        private const string versionHistoryQuery =
            "SELECT rawid, VERSION, changed_items as \"CHANGED_ITEMS\", save_comment as \"SAVE COMMENT\", "+
            "       last_update_by, last_update_dtts, create_by, create_dtts " +
            "FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts) rnk, " +
                  "      rawid, create_by, create_dtts, last_update_by, " +
                  "      last_update_dtts, VERSION, save_comment, changed_items " +
                  " FROM model_config_msth_spc a " +
                  " WHERE rawid = :rawid AND VERSION IS NOT NULL) " +
            "WHERE rnk = 1";

        private const string modelConfigMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_MSTH_SPC a 
where Rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string modelRuleMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_RULE_MSTH_SPC a 
where model_config_rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string modelAutocalcMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_AUTOCALC_MSTH_SPC a 
where model_config_rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string ChartVWSpcQuery =
            @"SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN, history.VERSION from MODEL_MST_SPC mst, MODEL_CONFIG_MST_SPC mcms, 
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_MSTH_SPC a 
where rawid = :RAWID
and version = :VERSION
) history
where mst.RAWID = mcms.MODEL_RAWID
and history.rawid = mcms.rawid
and (mcms.rawid = :RAWID)";
            
        /// <summary>
        /// Get version history of spc model
        /// </summary>
        /// <param name="rawid">rawid of spc model</param>
        /// <returns>version history table</returns>
        public DataTable GetVersionHistory(string rawid)
        {
            LinkedList whereFieldData = new LinkedList("RAWID", rawid);
            DataSet ds = this.Query(versionHistoryQuery, whereFieldData);

            if(ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public DataSet GetSPCSpecAndRuleOfHistory(string modelConfigRawID, string[] versions)
        {
            DataSet dsReturn = new DataSet();

            DataSet temp = new DataSet();
            
            // CHART_VW_SPC
            foreach(string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(ChartVWSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.CHART_VW_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_CONFIG_MSTH_SPC
            foreach(string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelConfigMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.MODEL_CONFIG_MST_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_RULE_MSTH_SPC
            foreach(string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelRuleMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.MODEL_RULE_MST_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_AUTOCALC_MST_SPC
            foreach(string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelAutocalcMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName= TABLE.MODEL_AUTOCALC_MST_SPC;
                dsReturn.Merge(temp);
            }

            return dsReturn;
        }

        public DataSet GetSPCModelVersionData(string modelConfigRawid, string version)
        {
            DataSet dsReturn = new DataSet();


            StringBuilder sb = new StringBuilder();

            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstCondition.Add("MODEL_CONFIG_RAWID", modelConfigRawid);
                llstCondition.Add("VERSION", version);
                //#00. MODEL_MST_SPC
                string query =
                    @"SELECT * FROM MODEL_MST_SPC 
WHERE RAWID IN (SELECT MODEL_RAWID FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_MSTH_SPC a 
where Rawid = :MODEL_CONFIG_RAWID
and version = :VERSION
)
WHERE RNK = 1
)";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#01. MODEL_CONFIG_MST_SPC

                //2009-12-07 bskwon 수정
                query = @"select mcms.*, B.NAME AS PARAM_TYPE, cc.name as MANAGE_TYPE_NAME 
from (SELECT * FROM
    (
    select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_MSTH_SPC a 
    where Rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1
    ) mcms 
LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_TYPE') B 
ON mcms.PARAM_TYPE_CD = B.CODE 
LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY='SPC_MANAGE_TYPE') cc 
ON mcms.manage_type_cd = cc.code 
ORDER BY mcms.RAWID";
                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONFIG_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#02. MODEL_CONFIG_OPT_MST_SPC
                query =
                    @"SELECT A.*, 
B.NAME AS SPC_PARAM_CATEGORY, 
C.NAME AS SPC_PRIORITY 
FROM (SELECT * FROM
    (
    select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_OPT_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1) A 
LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B 
ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) 
LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C 
ON (A.SPC_PRIORITY_CD = C.CODE) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfigOPT = dsTemp.Tables[0].Copy();
                    dtConfigOPT.TableName = TABLE.MODEL_CONFIG_OPT_MST_SPC;

                    dsReturn.Tables.Add(dtConfigOPT);
                }


                //#03. MODEL_CONTEXT_MST_SPC
                query = @"SELECT mcms.*, aa.name as context_key_name 
FROM (SELECT * FROM
    ( select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONTEXT_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1 ) mcms, 
(select CODE,NAME from code_mst_pp 
where category='CONTEXT_TYPE' 
UNION 
select CODE,NAME from code_mst_pp 
where category='SPC_CONTEXT_TYPE' 
) AA 
WHERE mcms.context_key = aa.code 
ORDER BY mcms.KEY_ORDER ASC ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.MODEL_CONTEXT_MST_SPC;

                    dsReturn.Tables.Add(dtContext);
                }

                //#04. MODEL_RULE_MST_SPC
                query = @" SELECT A.*, 
B.DESCRIPTION, 
DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, 
'' AS RULE_OPTION, '' AS RULE_OPTION_DATA 
FROM (SELECT * FROM
    ( select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_RULE_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION )
    WHERE RNK = 1) A 
LEFT OUTER JOIN RULE_MST_SPC B 
ON A.SPC_RULE_NO = B.SPC_RULE_NO ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRule = dsTemp.Tables[0].Copy();
                    dtRule.TableName = TABLE.MODEL_RULE_MST_SPC;

                    dsReturn.Tables.Add(dtRule);
                }

                //#05. MODEL_RULE_OPT_MST_SPC
                query = @"SELECT b.*, c.option_name, c.description, a.spc_rule_no
  FROM (SELECT *
          FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts)
                                                                          rnk,
                       aa.*
                  FROM model_rule_msth_spc aa
                 WHERE model_config_rawid = :MODEL_CONFIG_RAWID
                   AND VERSION = :VERSION)
         WHERE rnk = 1) a
       LEFT OUTER JOIN
       (SELECT *
          FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts)
                                                                          rnk,
                       aaa.*
                  FROM model_rule_opt_msth_spc aaa
                 WHERE model_rule_rawid IN (
                          SELECT DISTINCT rawid
                                     FROM model_rule_msth_spc
                                    WHERE model_config_rawid =
                                                           :MODEL_CONFIG_RAWID
                                      AND VERSION = :VERSION)
                   AND VERSION = :VERSION)
         WHERE rnk = 1) b
       ON (a.rawid = b.model_rule_rawid AND b.VERSION = :VERSION)
       LEFT OUTER JOIN
       (SELECT a.rawid AS rule_rawid, a.spc_rule_no, b.rule_option_no,
               b.option_name, b.description
          FROM rule_mst_spc a LEFT OUTER JOIN rule_opt_mst_spc b
               ON (a.rawid = b.rule_rawid)
               ) c
       ON (    a.spc_rule_no = c.spc_rule_no
           AND b.rule_option_no = c.rule_option_no
          ) ORDER BY b.rawid ASC ";
                
                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRuleOPT = dsTemp.Tables[0].Copy();
                    dtRuleOPT.TableName = TABLE.MODEL_RULE_OPT_MST_SPC;

                    dsReturn.Tables.Add(dtRuleOPT);
                }

                //#06. MODEL_AUTOCALC_MST_SPC
                query = @"SELECT * 
FROM (SELECT * FROM
    (select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_AUTOCALC_MSTH_SPC a 
    where model_config_rawid = :model_config_rawid
    and version = :version
    )
    WHERE RNK = 1) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtAutoCalc = dsTemp.Tables[0].Copy();
                    dtAutoCalc.TableName = TABLE.MODEL_AUTOCALC_MST_SPC;

                    dsReturn.Tables.Add(dtAutoCalc);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstCondition.Clear();
                llstCondition = null;
            }

            return dsReturn;
        }

        public string GetSPCLastestVersion(string chartID)
        {
            try
            {
                DataSet ds = this.Query("SELECT VERSION FROM MODEL_CONFIG_MST_SPC where rawid = '" + chartID + "'");
                if(ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["VERSION"].ToString();
                }

                return string.Empty;
            }
            catch(Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return string.Empty;
        }

        public bool DeleteSPCModelHistory(byte[] baData)
        {
            try
            {
                base.BeginTrans();

                LinkedList lnkCondition = new LinkedList();
                lnkCondition.SetSerialData(baData);

                String sWhereQuery = " WHERE RAWID = :MODEL_CONFIG_RAWID AND VERSION = :SPC_MODEL_VERSION ";

                base.Delete(TABLE.MODEL_CONFIG_MSTH_SPC, sWhereQuery, lnkCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    base.RollBack();
                    return false;
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return true;
        }
    }

    public class SPCATTModelHistoryData : DataBase
    {
        public string lastQuery = string.Empty;

        private const string versionATTHistoryQuery =
            @"select rawid, version, create_by, create_dtts, last_update_by, last_update_dtts from 
(
select rank() over (partition by version order by input_dtts) RNK, rawid, create_by, create_dtts, last_update_by, last_update_dtts, version from MODEL_CONFIG_ATT_MSTH_SPC a 
where Rawid = :rawid
and version is not null
)
where RNK = 1
order by version desc";

        private const string modelConfigATTMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_ATT_MSTH_SPC a 
where Rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string modelRuleATTMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_RULE_ATT_MSTH_SPC a 
where model_config_rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string modelAutocalcATTMsthSpcQuery =
            @"SELECT * FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_AUTOCALC_ATT_MSTH_SPC a 
where model_config_rawid = :RAWID
and version = :VERSION
)
WHERE RNK = 1
";

        private const string ChartVWATTSpcQuery =
            @"SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN, history.VERSION from MODEL_ATT_MST_SPC mst, MODEL_CONFIG_ATT_MST_SPC mcms, 
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_ATT_MSTH_SPC a 
where rawid = :RAWID
and version = :VERSION
) history
where mst.RAWID = mcms.MODEL_RAWID
and history.rawid = mcms.rawid
and (mcms.rawid = :RAWID)";

        /// <summary>
        /// Get version history of spc model
        /// </summary>
        /// <param name="rawid">rawid of spc model</param>
        /// <returns>version history table</returns>
        public DataTable GetATTVersionHistory(string rawid)
        {
            LinkedList whereFieldData = new LinkedList("RAWID", rawid);
            DataSet ds = this.Query(versionATTHistoryQuery, whereFieldData);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }

        public DataSet GetATTSPCSpecAndRuleOfHistory(string modelConfigRawID, string[] versions)
        {
            DataSet dsReturn = new DataSet();

            DataSet temp = new DataSet();

            // CHART_VW_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(ChartVWATTSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.CHART_VW_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_CONFIG_MSTH_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelConfigATTMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_RULE_MSTH_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelRuleATTMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.MODEL_RULE_ATT_MST_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_AUTOCALC_MST_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelAutocalcATTMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;
                dsReturn.Merge(temp);
            }

            return dsReturn;
        }

        public DataSet GetATTSPCModelVersionData(string modelConfigRawid, string version)
        {
            DataSet dsReturn = new DataSet();


            StringBuilder sb = new StringBuilder();

            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstCondition.Add("MODEL_CONFIG_RAWID", modelConfigRawid);
                llstCondition.Add("VERSION", version);
                //#00. MODEL_MST_SPC
                string query =
                    @"SELECT * FROM MODEL_ATT_MST_SPC 
WHERE RAWID IN (SELECT MODEL_RAWID FROM
(
select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_ATT_MSTH_SPC a 
where Rawid = :MODEL_CONFIG_RAWID
and version = :VERSION
)
WHERE RNK = 1
)";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#01. MODEL_CONFIG_MST_SPC

                //2009-12-07 bskwon 수정
                query = @"select mcms.* 
from (SELECT * FROM
    (
    select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_CONFIG_ATT_MSTH_SPC a 
    where Rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1
    ) mcms 
ORDER BY mcms.RAWID";
                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONFIG_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#02. MODEL_CONFIG_OPT_MST_SPC
                query =
                    @"SELECT A.*
FROM (SELECT * FROM
    (
    select rank () over(partition by version order by input_dtts) RNK, a.* from MODEL_CONFIG_OPT_ATT_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1) A ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfigOPT = dsTemp.Tables[0].Copy();
                    dtConfigOPT.TableName = TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfigOPT);
                }


                //#03. MODEL_CONTEXT_MST_SPC
                query = @"SELECT mcms.* 
FROM (SELECT * FROM
    ( select rank () over(partition by version order by input_dtts) RNK, a.*from MODEL_CONTEXT_ATT_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION
    )
    WHERE RNK = 1 ) mcms
ORDER BY mcms.KEY_ORDER ASC ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = TABLE.MODEL_CONTEXT_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtContext);
                }

                //#04. MODEL_RULE_MST_SPC
                query = @" SELECT A.*, 
B.DESCRIPTION, 
DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, 
'' AS RULE_OPTION, '' AS RULE_OPTION_DATA 
FROM (SELECT * FROM
    ( select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_RULE_ATT_MSTH_SPC a 
    where model_config_rawid = :MODEL_CONFIG_RAWID
    and version = :VERSION )
    WHERE RNK = 1) A 
LEFT OUTER JOIN RULE_MST_SPC B 
ON A.SPC_RULE_NO = B.SPC_RULE_NO ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRule = dsTemp.Tables[0].Copy();
                    dtRule.TableName = TABLE.MODEL_RULE_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtRule);
                }

                //#05. MODEL_RULE_OPT_MST_SPC
                query = @"SELECT b.*, c.option_name, c.description, a.spc_rule_no
  FROM (SELECT *
          FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts)
                                                                          rnk,
                       aa.*
                  FROM model_rule_att_msth_spc aa
                 WHERE model_config_rawid = :MODEL_CONFIG_RAWID
                   AND VERSION = :VERSION)
         WHERE rnk = 1) a
       LEFT OUTER JOIN
       (SELECT *
          FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts)
                                                                          rnk,
                       aaa.*
                  FROM model_rule_opt_att_msth_spc aaa
                 WHERE model_rule_rawid IN (
                          SELECT DISTINCT rawid
                                     FROM model_rule_att_msth_spc
                                    WHERE model_config_rawid =
                                                           :MODEL_CONFIG_RAWID
                                      AND VERSION = :VERSION)
                   AND VERSION = :VERSION)
         WHERE rnk = 1) b
       ON (a.rawid = b.model_rule_rawid AND b.VERSION = :VERSION)
       LEFT OUTER JOIN
       (SELECT a.rawid AS rule_rawid, a.spc_rule_no, b.rule_option_no,
               b.option_name, b.description
          FROM rule_mst_spc a LEFT OUTER JOIN rule_opt_mst_spc b
               ON (a.rawid = b.rule_rawid)
               ) c
       ON (    a.spc_rule_no = c.spc_rule_no
           AND b.rule_option_no = c.rule_option_no
          ) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRuleOPT = dsTemp.Tables[0].Copy();
                    dtRuleOPT.TableName = TABLE.MODEL_RULE_OPT_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtRuleOPT);
                }

                //#06. MODEL_AUTOCALC_MST_SPC
                query = @"SELECT * 
FROM (SELECT * FROM
    (select rank () over(partition by version order by input_dtts) RNK, a.*  from MODEL_AUTOCALC_ATT_MSTH_SPC a 
    where model_config_rawid = :model_config_rawid
    and version = :version
    )
    WHERE RNK = 1) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtAutoCalc = dsTemp.Tables[0].Copy();
                    dtAutoCalc.TableName = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtAutoCalc);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstCondition.Clear();
                llstCondition = null;
            }

            return dsReturn;
        }

        public string GetATTSPCLastestVersion(string chartID)
        {
            try
            {
                DataSet ds = this.Query("SELECT VERSION FROM MODEL_CONFIG_ATT_MST_SPC where rawid = '" + chartID + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["VERSION"].ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return string.Empty;
        }

        public bool DeleteSPCATTModelHistory(byte[] baData)
        {
            try
            {
                base.BeginTrans();

                LinkedList lnkCondition = new LinkedList();
                lnkCondition.SetSerialData(baData);

                String sWhereQuery = " WHERE RAWID = :MODEL_CONFIG_RAWID AND VERSION = :SPC_MODEL_VERSION ";

                base.Delete(TABLE.MODEL_CONFIG_ATT_MSTH_SPC, sWhereQuery, lnkCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    base.RollBack();
                    return false;
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return true;
        }
    }
}
