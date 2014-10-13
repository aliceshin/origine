using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Data.Server.Tool
{
    public class SPCDataExport :DataBase
    {
        public DataSet GetSPCModelsData(string[] modelRawids, bool useComma)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                DataSet temp = new DataSet();

                StringBuilder sb = new StringBuilder();

                //#00. MODEL_MST_SPC
                sb.Append("SELECT rawid, spc_model_name FROM MODEL_MST_SPC ");
                sb.Append(" WHERE 1 = 1 ");

                for(int i=0; i<modelRawids.Length; i++)
                {
                    if(i==0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(") order by rawid");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#01. MODEL_CONFIG_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(@"SELECT rawid, model_rawid, MAIN_YN, CHART_MODE_CD, PARAM_TYPE_CD, VERSION
    FROM model_config_mst_spc WHERE 1 = 1 ");

                for(int i=0; i<modelRawids.Length; i++)
                {
                    if(i==0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(")");

                sb.Append(" ORDER BY model_rawid, RAWID");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONFIG_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#02. MODEL_CONTEXT_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append("SELECT mcms.rawid, mcms.model_config_rawid, mcms.context_key,  ");

                if (useComma)
                    sb.Append(" replace(mcms.context_value, ';', ',') as context_value, ");
                else
                    sb.Append(" mcms.context_value, ");

                sb.Append(" mcms.exclude_list, mcms.key_order, mcms.create_dtts, mcms.create_by, mcms.last_update_dtts, ");
                sb.Append(" mcms.last_update_by, mcms.group_yn, mcms.version, aa.name AS context_key_name ");

                sb.Append(" FROM model_context_mst_spc mcms, ");
                sb.Append("   (SELECT code, NAME FROM code_mst_pp ");
                sb.Append("     WHERE CATEGORY = 'CONTEXT_TYPE'   ");
                sb.Append("     UNION                                      ");
                sb.Append("    SELECT code, NAME FROM code_mst_pp               ");
                sb.Append("     WHERE CATEGORY = 'SPC_CONTEXT_TYPE') aa   ");
                sb.Append("  WHERE mcms.context_key = aa.code                  ");
                sb.Append("  AND mcms.model_config_rawid IN (SELECT rawid    ");
                sb.Append("  FROM model_config_mst_spc            ");
                sb.Append("  WHERE 1=1                          ");

                for(int i=0; i<modelRawids.Length; i++)
                {
                    if(i==0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(")");
                sb.Append(") ORDER BY mcms.key_order ASC");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONTEXT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#03. MODEL_RULE_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(@"SELECT a.*, b.description,
       DECODE (a.use_main_spec_yn,
               'Y', 'True',
               'N', 'False',
               'True'
              ) AS use_main_spec,
       '' AS rule_option, '' AS rule_option_data
  FROM model_rule_mst_spc a LEFT OUTER JOIN rule_mst_spc b
       ON a.spc_rule_no = b.spc_rule_no
 WHERE 1 = 1 AND a.model_config_rawid IN (SELECT rawid
                                            FROM model_config_mst_spc
                                           WHERE 1=1 ");

                for(int i=0; i<modelRawids.Length; i++)
                {
                    if(i==0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(") )");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_RULE_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }
            }
            catch(Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
    }

    public class SPCATTDataExport : DataBase
    {
        public DataSet GetATTSPCModelsData(string[] modelRawids, bool useComma)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                DataSet temp = new DataSet();

                StringBuilder sb = new StringBuilder();

                //#00. MODEL_MST_SPC
                sb.Append("SELECT rawid, spc_model_name FROM MODEL_ATT_MST_SPC ");
                sb.Append(" WHERE 1 = 1 ");

                for (int i = 0; i < modelRawids.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(") order by rawid");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#01. MODEL_CONFIG_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(@"SELECT rawid, model_rawid, MAIN_YN, CHART_MODE_CD,  VERSION
    FROM model_config_att_mst_spc WHERE 1 = 1 ");

                for (int i = 0; i < modelRawids.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(")");

                sb.Append(" ORDER BY model_rawid, RAWID");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONFIG_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#02. MODEL_CONTEXT_MST_SPC
                sb.Remove(0, sb.Length);

                sb.Append("SELECT mcms.rawid, mcms.model_config_rawid, mcms.context_key,  ");

                if (useComma)
                    sb.Append(" replace(mcms.context_value, ';', ',') as context_value, ");
                else
                    sb.Append(" mcms.context_value, ");

                sb.Append(" mcms.exclude_list, mcms.key_order, mcms.create_dtts, mcms.create_by, mcms.last_update_dtts, ");
                sb.Append(" mcms.last_update_by, mcms.group_yn, mcms.version, aa.name AS context_key_name ");
                sb.Append(" FROM MODEL_CONTEXT_ATT_MST_SPC mcms, ");
                sb.Append("    (select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='CONTEXT_TYPE'");
                sb.Append("    UNION ");
                sb.Append("    select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='SPC_CONTEXT_TYPE'");
                sb.Append("    ) AA");
                sb.Append(" WHERE mcms.context_key = aa.code");
                sb.Append("   AND mcms.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE 1=1 ");

                for (int i = 0; i < modelRawids.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(")");
                sb.Append(") ORDER BY mcms.key_order ASC");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_CONTEXT_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }

                //#03. MODEL_RULE_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(@"SELECT a.*, b.description,
       DECODE (a.use_main_spec_yn,
               'Y', 'True',
               'N', 'False',
               'True'
              ) AS use_main_spec,
       '' AS rule_option, '' AS rule_option_data
  FROM model_rule_att_mst_spc a LEFT OUTER JOIN rule_att_mst_spc b
       ON a.spc_rule_no = b.spc_rule_no
 WHERE 1 = 1 AND a.model_config_rawid IN (SELECT rawid
                                            FROM model_config_att_mst_spc
                                           WHERE 1=1 ");

                for (int i = 0; i < modelRawids.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(" and ((");
                    }
                    else
                    {
                        sb.Append(" or (");
                    }
                    sb.Append("MODEL_RAWID = '" + modelRawids[i] + "')");
                }
                sb.Append(") )");

                temp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtConfig = temp.Tables[0].Copy();
                    dtConfig.TableName = TABLE.MODEL_RULE_ATT_MST_SPC;

                    dsReturn.Tables.Add(dtConfig);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
    }
}
