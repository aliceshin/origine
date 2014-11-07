using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Data.Server.History
{
    public class SPCModelHistoryDataCall : DataBase
    {
        /// <summary>
        /// Get version history of spc model
        /// </summary>
        /// <param name="rawid">rawid of spc model</param>
        /// <returns>version history table</returns>
        public DataTable GetVersionHistory(string rawid, bool isATT)
        {
            //spc-977 by stella : save_comment 추가 /// KBLEE, 2014.08 refactoring 전 전역변수였음.
            string versionHistoryQuery = String.Empty;

            if (isATT)
            {
                versionHistoryQuery = 
                    "SELECT rawid, version, create_by, create_dtts, last_update_by, last_update_dtts " +
                    " FROM (SELECT rank() over (partition by version order by input_dtts) RNK, " +
                    "              rawid, create_by, create_dtts, last_update_by, last_update_dtts, version " +
                    "       FROM " + TABLE.MODEL_CONFIG_ATT_MSTH_SPC +
                    "       WHERE Rawid = :rawid " +
                    "       AND version is not null ) " +
                    " WHERE RNK = 1 " +
                    " ORDER BY version desc ";
            }
            else
            {
                versionHistoryQuery = 
                    "SELECT rawid, VERSION, changed_items as \"CHANGED_ITEMS\", save_comment as \"SAVE COMMENT\", " +
                    "       last_update_by, last_update_dtts, create_by, create_dtts " +
                    " FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts) rnk, " +
                    "              rawid, create_by, create_dtts, last_update_by, " +
                    "              last_update_dtts, VERSION, save_comment, changed_items " +
                    "       FROM " + TABLE.MODEL_CONFIG_MSTH_SPC +
                    "       WHERE rawid = :rawid AND VERSION IS NOT NULL) " +
                    " WHERE rnk = 1 ";
            }
            //spc-977 by stella : save_comment 추가, end

            LinkedList whereFieldData = new LinkedList("RAWID", rawid);
            DataSet ds = this.Query(versionHistoryQuery, whereFieldData);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        public DataSet GetSPCSpecAndRuleOfHistory(string modelConfigRawID, string[] versions, bool isATT)
        {
            DataSet dsReturn = new DataSet();
            DataSet temp = new DataSet();

            string modelMstTblName = string.Empty;
            string modelConfigMstTblName = string.Empty;
            string modelConfigMsthTblName = string.Empty;
            string modelRuleMstTblName = string.Empty;
            string modelRuleMsthTblName = string.Empty;
            string modelAutoCalcMstTblname = string.Empty;
            string modelAutoCalcMsthTblName = string.Empty;

            if (isATT)
            {
                modelMstTblName = TABLE.MODEL_ATT_MST_SPC;
                modelConfigMstTblName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_ATT_MSTH_SPC;
                modelRuleMstTblName = TABLE.MODEL_RULE_ATT_MST_SPC;
                modelRuleMsthTblName = TABLE.MODEL_RULE_ATT_MSTH_SPC;
                modelAutoCalcMstTblname = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;
                modelAutoCalcMsthTblName = TABLE.MODEL_AUTOCALC_ATT_MSTH_SPC;
            }
            else
            {
                modelMstTblName = TABLE.MODEL_MST_SPC;
                modelConfigMstTblName = TABLE.MODEL_CONFIG_MST_SPC;
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_MSTH_SPC;
                modelRuleMstTblName = TABLE.MODEL_RULE_MST_SPC;
                modelRuleMsthTblName = TABLE.MODEL_RULE_MSTH_SPC;
                modelAutoCalcMstTblname = TABLE.MODEL_AUTOCALC_MST_SPC;
                modelAutoCalcMsthTblName = TABLE.MODEL_AUTOCALC_MSTH_SPC;
            }

            //KBLEE, 2014.08 refactoring 전에 전역변수였음, start
            string modelConfigMsthSpcQuery =
                "SELECT * " +
                " FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                "       FROM " + modelConfigMsthTblName + " a " +
                "       WHERE Rawid = :RAWID " +
                "       AND version = :VERSION) " +
                " WHERE RNK = 1 ";

            string modelRuleMsthSpcQuery =
                "SELECT * " +
                " FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                "       FROM " + modelRuleMsthTblName + " a " +
                "       WHERE model_config_rawid = :RAWID " +
                "       AND version = :VERSION) " +
                " WHERE RNK = 1 ";

            string modelAutocalcMsthSpcQuery =
                "SELECT * " +
                " FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                "       FROM " + modelAutoCalcMsthTblName + " a " +
                "       WHERE model_config_rawid = :RAWID " +
                "       AND version = :VERSION) " +
                " WHERE RNK = 1 ";

            string ChartVWSpcQuery =
                "SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN, history.VERSION " +
                " FROM " + modelMstTblName + " mst, " + modelConfigMstTblName + " mcms, " +
                "      (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                "       FROM " + modelConfigMsthTblName + " a " +
                "       WHERE rawid = :RAWID " +
                "       AND version = :VERSION) history " +
                " WHERE mst.RAWID = mcms.MODEL_RAWID AND history.rawid = mcms.rawid AND (mcms.rawid = :RAWID) ";
            //KBLEE, 2014.08 refactoring 전에 전역변수였음, end

            // CHART_VW_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(ChartVWSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = TABLE.CHART_VW_SPC;
                dsReturn.Merge(temp);
            }

            // MODEL_CONFIG_MSTH_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelConfigMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = modelConfigMstTblName;
                dsReturn.Merge(temp);
            }

            // MODEL_RULE_MSTH_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelRuleMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = modelRuleMstTblName;
                dsReturn.Merge(temp);
            }

            // MODEL_AUTOCALC_MST_SPC
            foreach (string s in versions)
            {
                LinkedList llWhereFieldData = new LinkedList();
                llWhereFieldData.Add("RAWID", modelConfigRawID);
                llWhereFieldData.Add("VERSION", s);

                temp = this.Query(modelAutocalcMsthSpcQuery, llWhereFieldData);
                temp.Tables[0].TableName = modelAutoCalcMstTblname;
                dsReturn.Merge(temp);
            }

            return dsReturn;
        }

        public DataSet GetSPCModelVersionData(string modelConfigRawid, string version, bool isATT)
        {
            DataSet dsReturn = new DataSet();

            DataSet dsTemp = null;
            StringBuilder sb = new StringBuilder();
            LinkedList llstCondition = new LinkedList();

            string modelMstTblName = string.Empty;
            string modelConfigMstTblName = string.Empty;
            string modelConfigMsthTblName = string.Empty;
            string modelConfigOptMstTblname = string.Empty;
            string modelContextMstTblname = string.Empty;
            string modelRuleMstTblName = string.Empty;
            string modelRuleMsthTblName = string.Empty;
            string modelRuleOptMstTblName = string.Empty;
            string modelRuleOptMsthTblName = string.Empty;
            string modelAutoCalcMstTblname = string.Empty;
            string modelAutoCalcMsthTblName = string.Empty;
            string ruleMstTblName = string.Empty;
            string ruleOptMstTblName = string.Empty;

            if (isATT)
            {
                modelMstTblName = TABLE.MODEL_ATT_MST_SPC;
                modelConfigMstTblName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_ATT_MSTH_SPC;
                modelConfigOptMstTblname = TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC;
                modelContextMstTblname = TABLE.MODEL_CONTEXT_ATT_MST_SPC;
                modelRuleMstTblName = TABLE.MODEL_RULE_ATT_MST_SPC;
                modelRuleMsthTblName = TABLE.MODEL_RULE_ATT_MSTH_SPC;
                modelRuleOptMstTblName = TABLE.MODEL_RULE_OPT_ATT_MST_SPC;
                modelRuleOptMsthTblName = TABLE.MODEL_RULE_OPT_ATT_MSTH_SPC;
                modelAutoCalcMstTblname = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;
                modelAutoCalcMsthTblName = TABLE.MODEL_AUTOCALC_ATT_MSTH_SPC;
                ruleMstTblName = TABLE.RULE_ATT_MST_SPC;
                ruleOptMstTblName = TABLE.RULE_OPT_ATT_MST_SPC;
            }
            else
            {
                modelMstTblName = TABLE.MODEL_MST_SPC;
                modelConfigMstTblName = TABLE.MODEL_CONFIG_MST_SPC;
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_MSTH_SPC;
                modelConfigOptMstTblname = TABLE.MODEL_CONFIG_OPT_MST_SPC;
                modelContextMstTblname = TABLE.MODEL_CONTEXT_MST_SPC;
                modelRuleMstTblName = TABLE.MODEL_RULE_MST_SPC;
                modelRuleMsthTblName = TABLE.MODEL_RULE_MSTH_SPC;
                modelRuleOptMstTblName = TABLE.MODEL_RULE_OPT_MST_SPC;
                modelRuleOptMsthTblName = TABLE.MODEL_RULE_OPT_MSTH_SPC;
                modelAutoCalcMstTblname = TABLE.MODEL_AUTOCALC_MST_SPC;
                modelAutoCalcMsthTblName = TABLE.MODEL_AUTOCALC_MSTH_SPC;
                ruleMstTblName = TABLE.RULE_MST_SPC;
                ruleOptMstTblName = TABLE.RULE_OPT_MST_SPC;
            }

            try
            {
                llstCondition.Add("MODEL_CONFIG_RAWID", modelConfigRawid);
                llstCondition.Add("VERSION", version);

                //#00. MODEL_MST_SPC
                string query =
                    "SELECT * " +
                    " FROM " + modelMstTblName +
                    " WHERE RAWID IN (SELECT MODEL_RAWID " +
                    "                 FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                    "                       FROM " + modelConfigMsthTblName + " a " +
                    "                       WHERE Rawid = :MODEL_CONFIG_RAWID " +
                    "                       AND version = :VERSION) " +
                    "                 WHERE RNK = 1) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = modelMstTblName;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#01. MODEL_CONFIG_MST_SPC

                //2009-12-07 bskwon 수정
                if (isATT)
                {
                    query = 
                        "SELECT mcms.* " +
                        " FROM (SELECT * FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "       FROM MODEL_CONFIG_ATT_MSTH_SPC a " +
                        "       WHERE Rawid = :MODEL_CONFIG_RAWID " +
                        "       AND version = :VERSION) " +
                        " WHERE RNK = 1) mcms " +
                        " ORDER BY mcms.RAWID";
                }
                else
                {
                    query =
                        "SELECT mcms.*, B.NAME AS PARAM_TYPE, cc.name as MANAGE_TYPE_NAME " +
                        " FROM (SELECT * " +
                        "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "             FROM MODEL_CONFIG_MSTH_SPC a " +
                        "             WHERE Rawid = :MODEL_CONFIG_RAWID " +
                        "             AND version = :VERSION) " +
                        "       WHERE RNK = 1) mcms " +
                        "       LEFT OUTER JOIN (SELECT CODE, NAME " +
                        "                        FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_TYPE') B " +
                        "                   ON mcms.PARAM_TYPE_CD = B.CODE " +
                        "       LEFT OUTER JOIN (SELECT CODE, NAME " +
                        "                        FROM CODE_MST_PP WHERE CATEGORY='SPC_MANAGE_TYPE') cc " +
                        "                   ON mcms.manage_type_cd = cc.code " +
                        " ORDER BY mcms.RAWID";
                }

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = dsTemp.Tables[0].Copy();
                    dtConfig.TableName = modelConfigMstTblName;

                    dsReturn.Tables.Add(dtConfig);
                }


                //#02. MODEL_CONFIG_OPT_MST_SPC
                if (isATT)
                {
                    query =
                        "SELECT A.* " +
                        " FROM (SELECT * " +
                        "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "             FROM MODEL_CONFIG_OPT_ATT_MSTH_SPC a " +
                        "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                        "             AND version = :VERSION) " +
                        "       WHERE RNK = 1) A ";
                }
                else
                {
                    query =
                        "SELECT A.*, B.NAME AS SPC_PARAM_CATEGORY, C.NAME AS SPC_PRIORITY " +
                        " FROM (SELECT * " +
                        "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "             FROM MODEL_CONFIG_OPT_MSTH_SPC a " +
                        "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                        "             AND version = :VERSION) " +
                        "       WHERE RNK = 1) A " +
                        "       LEFT OUTER JOIN (SELECT CODE, NAME " + 
                        "                        FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B " +
                        "                   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) " +
                        "       LEFT OUTER JOIN (SELECT CODE, NAME " +
                        "                        FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C " +
                        "                   ON (A.SPC_PRIORITY_CD = C.CODE) ";
                }                

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfigOPT = dsTemp.Tables[0].Copy();
                    dtConfigOPT.TableName = modelConfigOptMstTblname;

                    dsReturn.Tables.Add(dtConfigOPT);
                }


                //#03. MODEL_CONTEXT_MST_SPC
                if (isATT)
                {
                    query = 
                        "SELECT mcms.* " +
                        " FROM (SELECT * " +
                        "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "             FROM MODEL_CONTEXT_ATT_MSTH_SPC a " +
                        "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                        "             AND version = :VERSION) " +
                        "       WHERE RNK = 1 ) mcms " +
                        " ORDER BY mcms.KEY_ORDER ASC ";
                }
                else
                {
                    query = 
                        "SELECT mcms.*, aa.name as context_key_name " +
                        " FROM (SELECT * " +
                        "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                        "             FROM MODEL_CONTEXT_MSTH_SPC a " +
                        "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                        "             AND version = :VERSION) " +
                        "       WHERE RNK = 1 ) mcms, " +
                        "      (SELECT CODE,NAME " +
                        "       FROM code_mst_pp " +
                        "       WHERE category='CONTEXT_TYPE' " +
                        "             UNION SELECT CODE,NAME " +
                        "             FROM code_mst_pp " +
                        "             WHERE category='SPC_CONTEXT_TYPE') AA " +
                        " WHERE mcms.context_key = aa.code " +
                        " ORDER BY mcms.KEY_ORDER ASC ";
                }

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtContext = dsTemp.Tables[0].Copy();
                    dtContext.TableName = modelContextMstTblname;

                    dsReturn.Tables.Add(dtContext);
                }

                //#04. MODEL_RULE_MST_SPC
                query = 
                    "SELECT A.*, B.DESCRIPTION, " +
                    "       DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, " +
                    "       '' AS RULE_OPTION, '' AS RULE_OPTION_DATA " +
                    " FROM (SELECT * " +
                    "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                    "             FROM " + modelRuleMsthTblName + " a " +
                    "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                    "             AND version = :VERSION ) " +
                    "       WHERE RNK = 1) A " +
                    "       LEFT OUTER JOIN " + ruleMstTblName + " B " +
                    "                   ON A.SPC_RULE_NO = B.SPC_RULE_NO ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRule = dsTemp.Tables[0].Copy();
                    dtRule.TableName = modelRuleMstTblName;

                    dsReturn.Tables.Add(dtRule);
                }

                //#05. MODEL_RULE_OPT_MST_SPC
                query = 
                    "SELECT b.*, c.option_name, c.description, a.spc_rule_no " +
                    " FROM (SELECT * " +
                    "       FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts) rnk, aa.* " +
                    "             FROM " + modelRuleMsthTblName + " aa " +
                    "             WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                    "             AND VERSION = :VERSION) " +
                    "       WHERE rnk = 1) a " +
                    "       LEFT OUTER JOIN (SELECT * " +
                    "                        FROM (SELECT RANK () OVER (PARTITION BY VERSION ORDER BY input_dtts) rnk, aaa.* " +
                    "                              FROM " + modelRuleOptMsthTblName + " aaa " +
                    "                              WHERE model_rule_rawid IN (SELECT DISTINCT rawid " +
                    "                                                         FROM " + modelRuleMsthTblName +
                    "                                                         WHERE model_config_rawid = :MODEL_CONFIG_RAWID " +
                    "                                                         AND VERSION = :VERSION) " +
                    "                              AND VERSION = :VERSION) " +
                    "                        WHERE rnk = 1) b " +
                    "                  ON (a.rawid = b.model_rule_rawid AND b.VERSION = :VERSION) " +
                    "       LEFT OUTER JOIN (SELECT a.rawid AS rule_rawid, a.spc_rule_no, " +
                    "                               b.rule_option_no, b.option_name, b.description " +
                    "                        FROM " + ruleMstTblName + " a " +
                    "                             LEFT OUTER JOIN " + ruleOptMstTblName + " b " +
                    "                                        ON (a.rawid = b.rule_rawid) ) c " +
                    "                  ON (a.spc_rule_no = c.spc_rule_no " +
                    "                      AND b.rule_option_no = c.rule_option_no) " +
                    " ORDER BY b.rawid ASC ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRuleOPT = dsTemp.Tables[0].Copy();
                    dtRuleOPT.TableName = modelRuleOptMstTblName;

                    dsReturn.Tables.Add(dtRuleOPT);
                }

                //#06. MODEL_AUTOCALC_MST_SPC
                query = 
                    "SELECT * " +
                    " FROM (SELECT * " +
                    "       FROM (SELECT rank () over(partition by version order by input_dtts) RNK, a.* " +
                    "             FROM " + modelAutoCalcMsthTblName + " a " +
                    "             WHERE model_config_rawid = :model_config_rawid " +
                    "             AND version = :version) " +
                    "       WHERE RNK = 1) ";

                dsTemp = this.Query(query, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtAutoCalc = dsTemp.Tables[0].Copy();
                    dtAutoCalc.TableName = modelAutoCalcMstTblname;

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
                if (dsTemp != null) 
                {
                    dsTemp.Dispose(); 
                    dsTemp = null;
                }

                llstCondition.Clear();
                llstCondition = null;
            }

            return dsReturn;
        }

        public string GetSPCLastestVersion(string chartID, bool isATT)
        {
            string modelConfigMstTblName = string.Empty;

            if (isATT)
            {
                modelConfigMstTblName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
            }
            else
            {
                modelConfigMstTblName = TABLE.MODEL_CONFIG_MST_SPC;
            }

            try
            {
                DataSet ds = this.Query("SELECT VERSION FROM " + modelConfigMstTblName + " WHERE rawid = '" + chartID + "'");

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

        public bool DeleteSPCModelHistory(byte[] baData, bool isATT)
        {
            string modelConfigMsthTblName = string.Empty;

            if (isATT)
            {
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_ATT_MSTH_SPC;
            }
            else
            {
                modelConfigMsthTblName = TABLE.MODEL_CONFIG_MSTH_SPC;
            }

            try
            {
                base.BeginTrans();

                LinkedList lnkCondition = new LinkedList();
                lnkCondition.SetSerialData(baData);

                String sWhereQuery = " WHERE RAWID = :MODEL_CONFIG_RAWID AND VERSION = :SPC_MODEL_VERSION ";

                base.Delete(modelConfigMsthTblName, sWhereQuery, lnkCondition);

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
