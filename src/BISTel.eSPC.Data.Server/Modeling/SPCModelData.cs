using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using BISTel.eSPC.Data.Server.Common;
using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Modeling
{
    public class SPCModelData : DataBase
    {
        CommonUtility _ComUtil = new CommonUtility();
        Common.CommonData _commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

        public DataSet GetSPCModelData(byte[] param)
        {
            DataSet dsReturn = new DataSet();


            StringBuilder sb = new StringBuilder();

            LinkedList llstParam = new LinkedList();

            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);

                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                //#00. MODEL_MST_SPC
                sb.Append("SELECT * FROM MODEL_MST_SPC ");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND RAWID = :MODEL_RAWID ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                //sb.Remove(0, sb.Length);
                //sb.Append("SELECT A.*, B.NAME AS PARAM_TYPE ");
                //sb.Append("  FROM MODEL_CONFIG_MST_SPC A ");
                //sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_TYPE') B ");
                //sb.Append("       ON A.PARAM_TYPE_CD = B.CODE ");
                //sb.Append(" WHERE 1 = 1 ");

                //2009-12-07 bskwon 수정
                sb.Remove(0, sb.Length);
                //sb.Append("select mcms.*, B.NAME AS PARAM_TYPE, cc.name as MANAGE_TYPE_NAME ");
                sb.Append("select  ");
                sb.Append(" mcms.RAWID,                                                 ");
                sb.Append(" mcms.MODEL_RAWID,                                           ");
                sb.Append(" mcms.PARAM_TYPE_CD,                                         ");
                sb.Append(" mcms.PARAM_ALIAS,                                           ");
                if (useComma)
                {
                    sb.Append(" replace(mcms.PARAM_LIST,';',',') as PARAM_LIST,          ");
                }
                else
                {
                    sb.Append(" mcms.PARAM_LIST,                                            ");
                }
                sb.Append(" mcms.COMPLEX_YN,                                            ");
                sb.Append(" mcms.REF_PARAM,                                             ");
                if (useComma)
                {
                    sb.Append(" replace(mcms.REF_PARAM_LIST,';',',') as REF_PARAM_LIST,  ");
                }
                else
                {
                    sb.Append(" mcms.REF_PARAM_LIST,                                    ");
                }
                sb.Append(" mcms.MAIN_YN,                                               ");
                sb.Append(" mcms.AUTO_TYPE_CD,                                          ");
                sb.Append(" mcms.AUTO_SUB_YN,                                           ");
                sb.Append(" mcms.USE_EXTERNAL_SPEC_YN,                                  ");
                sb.Append(" mcms.INTERLOCK_YN,                                          ");
                sb.Append(" TRUNC(mcms.UPPER_SPEC, 16) UPPER_SPEC,                      ");
                sb.Append(" TRUNC(mcms.LOWER_SPEC,16) LOWER_SPEC,                       ");
                sb.Append(" TRUNC(mcms.UPPER_CONTROL,16) UPPER_CONTROL,                 ");
                sb.Append(" TRUNC(mcms.LOWER_CONTROL,16) LOWER_CONTROL,                 ");
                sb.Append(" mcms.SAMPLE_COUNT,                                          ");
                sb.Append(" mcms.EWMA_LAMBDA,                                           ");
                sb.Append(" mcms.MOVING_COUNT,                                          ");
                sb.Append(" TRUNC(mcms.CENTER_LINE,16) CENTER_LINE,                     ");
                sb.Append(" mcms.DESCRIPTION,                                           ");
                sb.Append(" mcms.CREATE_DTTS,                                           ");
                sb.Append(" mcms.CREATE_BY,                                             ");
                sb.Append(" mcms.LAST_UPDATE_DTTS,                                      ");
                sb.Append(" mcms.LAST_UPDATE_BY,                                        ");
                sb.Append(" mcms.AUTOCALC_YN,                                           ");
                sb.Append(" TRUNC(mcms.TARGET,16) TARGET,                               ");
                sb.Append(" mcms.MANAGE_TYPE_CD,                                        ");
                sb.Append(" TRUNC(mcms.STD,16) STD,                                     ");
                sb.Append(" TRUNC(mcms.RAW_UCL,16) RAW_UCL,                             ");
                sb.Append(" TRUNC(mcms.RAW_LCL,16) RAW_LCL,                             ");
                sb.Append(" TRUNC(mcms.RAW_CENTER_LINE,16) RAW_CENTER_LINE,             ");
                sb.Append(" TRUNC (mcms.STD_UCL, 16) STD_UCL,              ");
                sb.Append(" TRUNC (mcms.STD_LCL, 16) STD_LCL,              ");
                sb.Append(" TRUNC(mcms.STD_CENTER_LINE,16) STD_CENTER_LINE,             ");
                sb.Append(" TRUNC(mcms.RANGE_UCL,16) RANGE_UCL,                         ");
                sb.Append(" TRUNC(mcms.RANGE_LCL,16) RANGE_LCL,                         ");
                sb.Append(" TRUNC(mcms.RANGE_CENTER_LINE,16) RANGE_CENTER_LINE,         ");
                sb.Append(" TRUNC(mcms.EWMA_M_UCL,16) EWMA_M_UCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_M_LCL,16) EWMA_M_LCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_M_CENTER_LINE,16) EWMA_M_CENTER_LINE,       ");
                sb.Append(" TRUNC(mcms.EWMA_R_UCL,16) EWMA_R_UCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_R_LCL,16) EWMA_R_LCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_R_CENTER_LINE,16) EWMA_R_CENTER_LINE,       ");
                sb.Append(" TRUNC(mcms.EWMA_S_UCL,16) EWMA_S_UCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_S_LCL,16) EWMA_S_LCL,                       ");
                sb.Append(" TRUNC(mcms.EWMA_S_CENTER_LINE,16) EWMA_S_CENTER_LINE,       ");
                sb.Append(" TRUNC(mcms.MA_UCL,16) MA_UCL,                               ");
                sb.Append(" TRUNC(mcms.MA_LCL,16) MA_LCL,                               ");
                sb.Append(" TRUNC(mcms.MA_CENTER_LINE,16) MA_CENTER_LINE,               ");
                sb.Append(" TRUNC(mcms.MR_UCL,16) MR_UCL,                               ");
                sb.Append(" TRUNC(mcms.MR_LCL,16) MR_LCL,                               ");
                sb.Append(" TRUNC(mcms.MR_CENTER_LINE,16) MR_CENTER_LINE,               ");
                sb.Append(" TRUNC(mcms.MS_UCL,16) MS_UCL,                               ");
                sb.Append(" TRUNC(mcms.MS_LCL,16) MS_LCL,                               ");
                sb.Append(" TRUNC(mcms.MS_CENTER_LINE,16) MS_CENTER_LINE,               ");
                sb.Append(" TRUNC(mcms.ZONE_A_UCL,16) ZONE_A_UCL,                       ");
                sb.Append(" TRUNC(mcms.ZONE_A_LCL,16) ZONE_A_LCL,                       ");
                sb.Append(" TRUNC(mcms.ZONE_B_UCL,16) ZONE_B_UCL,                       ");
                sb.Append(" TRUNC(mcms.ZONE_B_LCL,16)  ZONE_B_LCL,                      ");
                sb.Append(" TRUNC(mcms.ZONE_C_UCL,16) ZONE_C_UCL,                       ");
                sb.Append(" TRUNC(mcms.ZONE_C_LCL,16) ZONE_C_LCL,                       ");
                sb.Append(" mcms.ACTIVATION_YN,                                         ");
                sb.Append(" mcms.SUB_INTERLOCK_YN,                                      ");
                sb.Append(" mcms.INHERIT_MAIN_YN,                                       ");
                sb.Append(" mcms.OFFSET_YN,                                             ");
                sb.Append(" TRUNC(mcms.SPEC_USL_OFFSET,16) SPEC_USL_OFFSET,             ");
                sb.Append(" TRUNC(mcms.SPEC_LSL_OFFSET,16) SPEC_LSL_OFFSET,             ");
                sb.Append(" TRUNC(mcms.MEAN_UCL_OFFSET,16) MEAN_UCL_OFFSET,             ");
                sb.Append(" TRUNC(mcms.MEAN_LCL_OFFSET,16) MEAN_LCL_OFFSET,             ");
                sb.Append(" TRUNC(mcms.RAW_UCL_OFFSET,16) RAW_UCL_OFFSET,               ");
                sb.Append(" TRUNC(mcms.RAW_LCL_OFFSET,16) RAW_LCL_OFFSET,               ");
                sb.Append(" TRUNC(mcms.STD_UCL_OFFSET,16) STD_UCL_OFFSET,               ");
                sb.Append(" TRUNC(mcms.STD_LCL_OFFSET,16) STD_LCL_OFFSET,               ");
                sb.Append(" TRUNC(mcms.RANGE_UCL_OFFSET,16) RANGE_UCL_OFFSET,           ");
                sb.Append(" TRUNC(mcms.RANGE_LCL_OFFSET,16) RANGE_LCL_OFFSET,           ");
                sb.Append(" TRUNC(mcms.EWMA_M_UCL_OFFSET,16) EWMA_M_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.EWMA_M_LCL_OFFSET,16) EWMA_M_LCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.EWMA_R_UCL_OFFSET,16) EWMA_R_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.EWMA_R_LCL_OFFSET,16) EWMA_R_LCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.EWMA_S_UCL_OFFSET,16) EWMA_S_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.EWMA_S_LCL_OFFSET,16) EWMA_S_LCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.MA_UCL_OFFSET,16) MA_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.MA_LCL_OFFSET,16) MA_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.MR_UCL_OFFSET,16) MR_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.MR_LCL_OFFSET,16) MR_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.MS_UCL_OFFSET,16) MS_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.MS_LCL_OFFSET,16) MS_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(mcms.ZONE_A_UCL_OFFSET,16) ZONE_A_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.ZONE_A_LCL_OFFSET,16) ZONE_A_LCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.ZONE_B_UCL_OFFSET,16) ZONE_B_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.ZONE_B_LCL_OFFSET,16) ZONE_B_LCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.ZONE_C_UCL_OFFSET,16) ZONE_C_UCL_OFFSET,         ");
                sb.Append(" TRUNC(mcms.ZONE_C_LCL_OFFSET,16) ZONE_C_LCL_OFFSET,         ");
                sb.Append(" mcms.MISSINGDATA_YN,                                        ");
                sb.Append(" TRUNC(mcms.UPPER_HALT,16) UPPER_HALT,                       ");
                sb.Append(" TRUNC(mcms.LOWER_HALT,16) LOWER_HALT,                       ");
                sb.Append(" TRUNC(mcms.UPPER_FILTER,16) UPPER_FILTER,                   ");
                sb.Append(" TRUNC(mcms.LOWER_FILTER,16) LOWER_FILTER,                   ");
                sb.Append(" mcms.SUB_AUTOCALC_YN,                                       ");
                sb.Append(" TRUNC(mcms.UPPER_TECHNICAL_LIMIT,16) UPPER_TECHNICAL_LIMIT, ");
                sb.Append(" TRUNC(mcms.LOWER_TECHNICAL_LIMIT,16) LOWER_TECHNICAL_LIMIT, ");
                sb.Append(" mcms.USE_NORM_YN,                                           ");
                sb.Append(" mcms.CHART_MODE_CD,                                         ");
                sb.Append(" mcms.VERSION,                                               ");
                sb.Append(" mcms.VALIDATION_SAME_MODULE_YN,                             ");
                sb.Append(" mcms.SPC_DATA_LEVEL,                                        ");
                sb.Append(" mcms.CHART_DESCRIPTION,                                     ");
                sb.Append(" mcms.SAVE_COMMENT,                                     ");
                sb.Append(" B.NAME AS PARAM_TYPE, cc.name as MANAGE_TYPE_NAME ");
                sb.Append("from MODEL_CONFIG_MST_SPC mcms ");
                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_TYPE') B ");
                sb.Append("       ON mcms.PARAM_TYPE_CD = B.CODE ");

                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY='SPC_MANAGE_TYPE') cc");
                sb.Append("       ON mcms.manage_type_cd = cc.code");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_RAWID = :MODEL_RAWID ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.main_yn desc, mcms.RAWID");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.*, ");
                sb.Append("       B.NAME AS SPC_PARAM_CATEGORY, ");
                sb.Append("       C.NAME AS SPC_PRIORITY ");
                sb.Append("  FROM MODEL_CONFIG_OPT_MST_SPC A ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
                sb.Append("   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
                sb.Append("   ON (A.SPC_PRIORITY_CD = C.CODE) ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("  AND A.model_config_rawid IN (SELECT rawid FROM model_config_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                //sb.Append(" SELECT A.*, "); 
                //sb.Append("        NVL(B.CODE, A.CONTEXT_KEY) AS CONTEXT_NAME "); //dev define columns
                //sb.Append("   FROM MODEL_CONTEXT_MST_SPC A ");
                //sb.Append("        LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'CONTEXT_TYPE') B ");
                //sb.Append("          ON (A.CONTEXT_KEY = B.CODE) ");
                //sb.Append(" WHERE 1 = 1 ");
                //sb.Append(" SELECT * ");
                //sb.Append("   FROM MODEL_CONTEXT_MST_SPC ");
                //sb.Append(" WHERE 1 = 1 ");

                sb.Append("SELECT mcms.rawid, mcms.model_config_rawid, mcms.context_key,  ");

                if (useComma)
                    sb.Append(" replace(mcms.context_value, ';', ',') as context_value, ");
                else
                    sb.Append(" mcms.context_value, ");

                sb.Append(" mcms.exclude_list, mcms.key_order, mcms.create_dtts, mcms.create_by, mcms.last_update_dtts, ");
                sb.Append(" mcms.last_update_by, mcms.group_yn, mcms.version, aa.name AS context_key_name ");
                sb.Append(" FROM MODEL_CONTEXT_MST_SPC mcms, ");
                sb.Append("    (select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='CONTEXT_TYPE'");
                //sb.Append("    and use_yn='Y'");
                sb.Append("    UNION ");
                sb.Append("    select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='SPC_CONTEXT_TYPE'");
                //sb.Append("    and use_yn='Y'");
                sb.Append("    ) AA");
                sb.Append(" WHERE mcms.context_key = aa.code");


                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.KEY_ORDER ASC ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT A.*, ");
                sb.Append("        B.DESCRIPTION, "); //add columns 
                sb.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, "); //dev define columns
                sb.Append("        '' AS RULE_OPTION, '' AS RULE_OPTION_DATA "); //dev define columns
                sb.Append("   FROM MODEL_RULE_MST_SPC A ");
                sb.Append("        LEFT OUTER JOIN RULE_MST_SPC B ");
                sb.Append("          ON A.SPC_RULE_NO = B.SPC_RULE_NO ");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                //sb.Append(" SELECT B.*, ");
                //sb.Append("        C.OPTION_NAME, C.DESCRIPTION, "); //add columns
                //sb.Append("        A.SPC_RULE_NO "); //add columns
                //sb.Append("   FROM MODEL_RULE_MST_SPC A ");
                //sb.Append("        LEFT OUTER JOIN MODEL_RULE_OPT_MST_SPC B ");
                //sb.Append("          ON (A.RAWID = B.MODEL_RULE_RAWID) ");
                //sb.Append("        LEFT OUTER JOIN ");
                //sb.Append("             (SELECT A.RAWID AS RULE_RAWID, A.SPC_RULE_NO, ");
                //sb.Append("                     B.RULE_OPTION_NO, B.OPTION_NAME, B.DESCRIPTION ");
                //sb.Append("                FROM RULE_MST_SPC A ");
                //sb.Append("                     LEFT OUTER JOIN RULE_OPT_MST_SPC B ");
                //sb.Append("                       ON (A.RAWID = B.RULE_RAWID) ");
                //sb.Append("             ) C ");
                //sb.Append("          ON (A.SPC_RULE_NO = C.SPC_RULE_NO AND B.RULE_OPTION_NO = C.RULE_OPTION_NO) ");
                //sb.Append(" WHERE 1 = 1 ");

                //if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                //{
                //    sb.Append("   AND A.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                //}

                //if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                //{
                //    sb.Append("   AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                //}

                sb.Append(" SELECT A.RAWID, A.MODEL_RULE_RAWID,  A.RULE_OPTION_NO, A.CREATE_DTTS, A.CREATE_BY,      ");
                sb.Append(" A.LAST_UPDATE_DTTS, A.LAST_UPDATE_BY,                                                   ");

                if (useComma)
                {
                    sb.Append(" REPLACE( A.RULE_OPTION_VALUE, ';', ',') AS RULE_OPTION_VALUE,                       ");
                }
                else
                {
                    sb.Append(" A.RULE_OPTION_VALUE,                                                                ");
                }

                sb.Append(" A.VERSION, A.SPC_RULE_NO, B.OPTION_NAME, B.DESCRIPTION                                  ");
                sb.Append("   FROM (SELECT MRO.*, MRM.SPC_RULE_NO                                                   ");
                sb.Append("           FROM MODEL_RULE_OPT_MST_SPC MRO,                                              ");
                sb.Append("                (SELECT MRM.RAWID, MRM.SPC_RULE_NO                                       ");
                sb.Append("                   FROM MODEL_RULE_MST_SPC MRM, MODEL_CONFIG_MST_SPC MCM                 ");
                sb.Append("                  WHERE 1 = 1                                                            ");
                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND MCM.MODEL_RAWID = :MODEL_RAWID ");
                }
                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND MRM.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }
                sb.Append("                    AND MRM.MODEL_CONFIG_RAWID = MCM.RAWID) MRM                          ");
                sb.Append("          WHERE MRO.MODEL_RULE_RAWID(+) = MRM.RAWID) A,                                  ");
                sb.Append("        (SELECT A.RAWID AS RULE_RAWID, A.SPC_RULE_NO, B.RULE_OPTION_NO,                  ");
                sb.Append("                B.OPTION_NAME, B.DESCRIPTION                                             ");
                sb.Append("           FROM RULE_MST_SPC A, RULE_OPT_MST_SPC B                                       ");
                sb.Append("          WHERE A.RAWID = B.RULE_RAWID(+)) B                                             ");
                sb.Append("  WHERE A.SPC_RULE_NO = B.SPC_RULE_NO(+) AND A.RULE_OPTION_NO = B.RULE_OPTION_NO(+)      ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                //SPC-1129 BY STELLA
                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, AUTOCALC_PERIOD, MIN_SAMPLES,  ");
                sb.Append("       DEFAULT_PERIOD, MAX_PERIOD, CONTROL_LIMIT, CONTROL_THRESHOLD,  ");
                sb.Append("       CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, ");
                sb.Append("       CALC_COUNT, STD_YN, RAW_CL_YN, MEAN_CL_YN, STD_CL_YN, RANGE_CL_YN, ");
                sb.Append("       EWMA_M_CL_YN, EWMA_R_CL_YN, EWMA_S_CL_YN, MA_CL_YN, NVL(MR_CL_YN,'N') AS MR_CL_YN, ");
                sb.Append("       MS_CL_YN, ZONE_YN, SHIFT_CALC_YN, WITHOUT_IQR_YN, VERSION, ");
                sb.Append("       THRESHOLD_OFF_YN, INITIAL_CALC_COUNT, NVL(RAW_CALC_CNT, 0) AS RAW_CALC_CNT, NVL(MEAN_CALC_CNT,0) AS MEAN_CALC_CNT, ");
                sb.Append("       NVL(STD_CALC_CNT,0) AS STD_CALC_CNT, NVL(RANGE_CALC_CNT,0) AS RANGE_CALC_CNT, NVL(EWMA_M_CALC_CNT,0) AS EWMA_M_CALC_CNT, NVL(EWMA_R_CALC_CNT,0) AS EWMA_R_CALC_CNT, ");
                sb.Append("       NVL(EWMA_S_CALC_CNT,0) AS EWMA_S_CALC_CNT, NVL(MA_CALC_CNT,0) AS MA_CALC_CNT, NVL(MR_CALC_CNT,0) AS MR_CALC_CNT, NVL(MS_CALC_CNT,0) AS MS_CALC_CNT, RAW_CALC_VALUE, ");
                sb.Append("       MEAN_CALC_VALUE, STD_CALC_VALUE, RANGE_CALC_VALUE, EWMA_M_CALC_VALUE, ");
                sb.Append("       EWMA_R_CALC_VALUE, EWMA_S_CALC_VALUE, MA_CALC_VALUE, MR_CALC_VALUE, ");
                sb.Append("       MS_CALC_VALUE, RAW_CALC_OPTION, MEAN_CALC_OPTION, STD_CALC_OPTION, ");
                sb.Append("       RANGE_CALC_OPTION, EWMA_M_CALC_OPTION, EWMA_R_CALC_OPTION, EWMA_S_CALC_OPTION, ");
                sb.Append("       MA_CALC_OPTION,MR_CALC_OPTION, MS_CALC_OPTION, RAW_CALC_SIDED, ");
                sb.Append("       MEAN_CALC_SIDED, STD_CALC_SIDED, RANGE_CALC_SIDED, EWMA_M_CALC_SIDED, ");
                sb.Append("       EWMA_R_CALC_SIDED, EWMA_S_CALC_SIDED, MA_CALC_SIDED, MR_CALC_SIDED, ");
                sb.Append("       MS_CALC_SIDED, NVL(USE_GLOBAL_YN, 'Y') AS USE_GLOBAL_YN, CONTROL_LIMIT_OPTION ");
                sb.Append("FROM MODEL_AUTOCALC_MST_SPC ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("  AND model_config_rawid IN (SELECT rawid FROM model_config_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                ////#03. MODEL_FILTER_MST_SPC
                //sb.Remove(0, sb.Length);

                //sb.Append(" SELECT mfms.*, aa.name as filter_key_name");
                //sb.Append(" FROM MODEL_FILTER_MST_SPC mfms, ");
                //sb.Append("    (select CODE,NAME from code_mst_pp ");
                //sb.Append("    where category='CONTEXT_TYPE'");
                //sb.Append("    UNION ");
                //sb.Append("    select CODE,NAME from code_mst_pp ");
                //sb.Append("    where category='SPC_CONTEXT_TYPE'");
                //sb.Append("    ) AA");
                //sb.Append(" WHERE mfms.filter_key = aa.code");


                //if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                //{
                //    sb.Append("   AND mfms.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                //}

                //if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                //{
                //    sb.Append("   AND mfms.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                //}

                //sb.Append(" ORDER BY mfms.RAWID ASC ");

                //dsTemp = this.Query(sb.ToString(), llstCondition);

                //if (base.ErrorMessage.Length > 0)
                //{
                //    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                //    return dsReturn;
                //}
                //else
                //{
                //    DataTable dtFilter = dsTemp.Tables[0].Copy();
                //    dtFilter.TableName = TABLE.MODEL_FILTER_MST_SPC;

                //    dsReturn.Tables.Add(dtFilter);
                //}
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCModelDatabyChartID(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();
            DataSet dsTemp = null;
            string groupRawid = string.Empty;

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                //#00. MODEL_MST_SPC
                sb.Append("SELECT B.RAWID LOCATION_RAWID, B.SITE, B.LINE, B.FAB, C.RAWID AREA_RAWID, C.AREA, A.RAWID MODEL_RAWID, A.SPC_MODEL_NAME, A.EQP_MODEL, A.GROUP_RAWID ");
                sb.Append("FROM MODEL_MST_SPC A, LOCATION_MST_PP B, AREA_MST_PP C ");
                sb.Append(" WHERE 1 = 1 ");
                sb.Append(" AND A.LOCATION_RAWID = B.RAWID ");
                sb.Append(" AND A.AREA_RAWID = C.RAWID ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND A.RAWID = (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                    if (dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        groupRawid = dsTemp.Tables[0].Rows[0][COLUMN.GROUP_RAWID].ToString();
                    }
                }


                //#01. MODEL_CONFIG_MST_SPC
                //2009-12-07 bskwon 수정
                sb.Remove(0, sb.Length);
                sb.Append("select mcms.*, B.NAME AS PARAM_TYPE, cc.name as MANAGE_TYPE_NAME ");
                sb.Append("from MODEL_CONFIG_MST_SPC mcms ");
                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_TYPE') B ");
                sb.Append("       ON mcms.PARAM_TYPE_CD = B.CODE ");

                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY='SPC_MANAGE_TYPE') cc");
                sb.Append("       ON mcms.manage_type_cd = cc.code");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.RAWID");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.*, ");
                sb.Append("       B.NAME AS SPC_PARAM_CATEGORY, ");
                sb.Append("       C.NAME AS SPC_PRIORITY ");
                sb.Append("  FROM MODEL_CONFIG_OPT_MST_SPC A ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
                sb.Append("   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
                sb.Append("   ON (A.SPC_PRIORITY_CD = C.CODE) ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                //#04. MODEL_GROUP_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append("SELECT * FROM MODEL_GROUP_MST_SPC ");

                if (String.IsNullOrEmpty(groupRawid))
                {
                    sb.Append(string.Format("WHERE RAWID IS NULL "));
                }
                else
                {
                    sb.Append(string.Format("WHERE RAWID = {0} ", groupRawid));
                }

                dsTemp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtGroup = dsTemp.Tables[0].Copy();
                    dtGroup.TableName = TABLE.MODEL_GROUP_MST_SPC;

                    if (String.IsNullOrEmpty(groupRawid))
                    {
                        DataRow dr = dtGroup.NewRow();
                        dr[COLUMN.GROUP_NAME] = Definition.VARIABLE_UNASSIGNED_MODEL;
                        dtGroup.Rows.Add(dr);
                    }

                    dsReturn.Tables.Add(dtGroup);
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        //private const string SQL_SEARCH_CALC_MODEL = " SELECT *                                                                  "
        //                                            + "   FROM MODEL_MST_SPC                                                      "
        //                                            + "  WHERE LOCATION_RAWID = :LINE                                             "
        //                                            + "    AND AREA_RAWID = :AREA                                                 "
        //                                            + "    AND NVL (EQP_MODEL, '-') = :EQP_MODEL                                  "
        //                                            + "    AND RAWID IN (                                                         "
        //                                            + "           SELECT DISTINCT MODEL_RAWID                                     "
        //                                            + "             FROM MODEL_CONFIG_MST_SPC                                     "
        //                                            + "            WHERE RAWID IN (                                               "
        //                                            + "                     SELECT A.MODEL_CONFIG_RAWID                           "
        //                                            + "                       FROM (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'EQP_ID' {0} ) A,        "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'MODULE_ID' {1} ) B,     "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'RECIPE_ID' {2} ) C,     "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'STEP_ID' {3} ) D        "
        //                                            + "                      WHERE A.MODEL_CONFIG_RAWID = B.MODEL_CONFIG_RAWID    "
        //                                            + "                        AND A.MODEL_CONFIG_RAWID = C.MODEL_CONFIG_RAWID    "
        //                                            + "                        AND A.MODEL_CONFIG_RAWID = D.MODEL_CONFIG_RAWID)   "
        //                                            + "              AND MAIN_YN = 'N')                                           ";



        //private const string SQL_SEARCH_CALC_MODEL_CONFIG = "           SELECT *                                                  "
        //                                            + "             FROM MODEL_CONFIG_MST_SPC                                     "
        //                                            + "            WHERE RAWID IN (                                               "
        //                                            + "                     SELECT A.MODEL_CONFIG_RAWID                           "
        //                                            + "                       FROM (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'EQP_ID' {0} ) A,        "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'MODULE_ID' {1} ) B,     "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'RECIPE_ID' {2} ) C,     "
        //                                            + "                            (SELECT MODEL_CONFIG_RAWID                     "
        //                                            + "                               FROM MODEL_CONTEXT_MST_SPC                  "
        //                                            + "                              WHERE CONTEXT_KEY = 'STEP_ID' {3} ) D        "
        //                                            + "                      WHERE A.MODEL_CONFIG_RAWID = B.MODEL_CONFIG_RAWID    "
        //                                            + "                        AND A.MODEL_CONFIG_RAWID = C.MODEL_CONFIG_RAWID    "
        //                                            + "                        AND A.MODEL_CONFIG_RAWID = D.MODEL_CONFIG_RAWID)   "
        //                                            + "                   AND MODEL_RAWID IN (                                    "
        //                                            + "                              SELECT RAWID                                 "
        //                                            + "                                FROM MODEL_MST_SPC                         "
        //                                            + "                               WHERE LOCATION_RAWID = :LINE                "
        //                                            + "                                     AND AREA_RAWID = :AREA                "
        //                                            + "                                     AND NVL(EQP_MODEL, '-') = :EQP_MODEL) "
        //                                            + "                   AND MAIN_YN = 'N'                                       ";

        //private const string SQL_SEARCH_CALC_MODEL_CONTEXT = " SELECT *                                                            "
        //                                            + "            FROM MODEL_CONTEXT_MST_SPC                                      "
        //                                            + "           WHERE MODEL_CONFIG_RAWID IN (                                    "
        //                                            + "                    SELECT A.MODEL_CONFIG_RAWID                             "
        //                                            + "                      FROM (SELECT MODEL_CONFIG_RAWID                       "
        //                                            + "                              FROM MODEL_CONTEXT_MST_SPC                    "
        //                                            + "                             WHERE CONTEXT_KEY = 'EQP_ID' {0}) A,               "
        //                                            + "                           (SELECT MODEL_CONFIG_RAWID                       "
        //                                            + "                              FROM MODEL_CONTEXT_MST_SPC                    "
        //                                            + "                             WHERE CONTEXT_KEY = 'MODULE_ID' {1}) B,            "
        //                                            + "                           (SELECT MODEL_CONFIG_RAWID                       "
        //                                            + "                              FROM MODEL_CONTEXT_MST_SPC                    "
        //                                            + "                             WHERE CONTEXT_KEY = 'RECIPE_ID' {2}) C,            "
        //                                            + "                           (SELECT MODEL_CONFIG_RAWID                       "
        //                                            + "                              FROM MODEL_CONTEXT_MST_SPC                    "
        //                                            + "                             WHERE CONTEXT_KEY = 'STEP_ID' {3}) D               "
        //                                            + "                     WHERE A.MODEL_CONFIG_RAWID = B.MODEL_CONFIG_RAWID      "
        //                                            + "                       AND A.MODEL_CONFIG_RAWID = C.MODEL_CONFIG_RAWID      "
        //                                            + "                       AND A.MODEL_CONFIG_RAWID = D.MODEL_CONFIG_RAWID)     "
        //                                            + "             AND MODEL_CONFIG_RAWID IN (                                    "
        //                                            + "                    SELECT RAWID                                            "
        //                                            + "                      FROM MODEL_CONFIG_MST_SPC                             "
        //                                            + "                     WHERE MODEL_RAWID IN (                                 "
        //                                            + "                              SELECT RAWID                                  "
        //                                            + "                                FROM MODEL_MST_SPC                          "
        //                                            + "                               WHERE LOCATION_RAWID = :LINE                 "
        //                                            + "                                     AND AREA_RAWID = :AREA                 "
        //                                            + "                                     AND NVL(EQP_MODEL, '-') = :EQP_MODEL)  "
        //                                            + "                       AND MAIN_YN = 'N')                                   "
        //                                            + "                       ORDER BY CONTEXT_KEY, CONTEXT_VALUE                  ";

        private const string SQL_SEARCH_CALC_MODEL = " SELECT *                                                                   "
                                                    + "   FROM MODEL_MST_SPC                                                      "
                                                    + "  WHERE RAWID = :RAWID                                ";

        public DataSet GetSPCCalcModelData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            string sSQL = "";

            try
            {
                llstParam.SetSerialData(param);

                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_RAWID))
                {
                    llstWhereData.Add(COLUMN.RAWID, llstParam[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                //#00. MODEL_MST_SPC
                sSQL = SQL_SEARCH_CALC_MODEL;

                dsTemp = this.Query(sSQL, llstWhereData);

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

                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT                                                 ");
                sb.Append(" RAWID,                                                 ");
                sb.Append(" MODEL_RAWID,                                           ");
                sb.Append(" PARAM_TYPE_CD,                                         ");
                sb.Append(" PARAM_ALIAS,                                           ");
                if (useComma)
                {
                    sb.Append(" REPLACE(PARAM_LIST,';', ',') PARAM_LIST,           ");
                }
                else
                    sb.Append(" PARAM_LIST,                                            ");

                sb.Append(" COMPLEX_YN,                                            ");
                sb.Append(" REF_PARAM,                                             ");

                if (useComma)
                {
                    sb.Append(" REPLACE(REF_PARAM_LIST,';', ',') REF_PARAM_LIST,   ");
                }
                sb.Append(" REF_PARAM_LIST,                                        ");

                sb.Append(" MAIN_YN,                                               ");
                sb.Append(" AUTO_TYPE_CD,                                          ");
                sb.Append(" AUTO_SUB_YN,                                           ");
                sb.Append(" USE_EXTERNAL_SPEC_YN,                                  ");
                sb.Append(" INTERLOCK_YN,                                          ");
                sb.Append(" TRUNC(UPPER_SPEC, 16) UPPER_SPEC,                      ");
                sb.Append(" TRUNC(LOWER_SPEC,16) LOWER_SPEC,                       ");
                sb.Append(" TRUNC(UPPER_CONTROL,16) UPPER_CONTROL,                 ");
                sb.Append(" TRUNC(LOWER_CONTROL,16) LOWER_CONTROL,                 ");
                sb.Append(" SAMPLE_COUNT,                                          ");
                sb.Append(" EWMA_LAMBDA,                                           ");
                sb.Append(" MOVING_COUNT,                                          ");
                sb.Append(" TRUNC(CENTER_LINE,16) CENTER_LINE,                     ");
                sb.Append(" DESCRIPTION,                                           ");
                sb.Append(" CREATE_DTTS,                                           ");
                sb.Append(" CREATE_BY,                                             ");
                sb.Append(" LAST_UPDATE_DTTS,                                      ");
                sb.Append(" LAST_UPDATE_BY,                                        ");
                sb.Append(" AUTOCALC_YN,                                           ");
                sb.Append(" TRUNC(TARGET,16) TARGET,                               ");
                sb.Append(" MANAGE_TYPE_CD,                                        ");
                sb.Append(" TRUNC(STD,16) STD,                                     ");
                sb.Append(" TRUNC(RAW_UCL,16) RAW_UCL,                             ");
                sb.Append(" TRUNC(RAW_LCL,16) RAW_LCL,                             ");
                sb.Append(" TRUNC(RAW_CENTER_LINE,16) RAW_CENTER_LINE,             ");
                sb.Append(" TRUNC (STD_UCL, 16) STD_UCL,                           ");
                sb.Append(" TRUNC (STD_LCL, 16) STD_LCL,                           ");
                sb.Append(" TRUNC(STD_CENTER_LINE,16) STD_CENTER_LINE,             ");
                sb.Append(" TRUNC(RANGE_UCL,16) RANGE_UCL,                         ");
                sb.Append(" TRUNC(RANGE_LCL,16) RANGE_LCL,                         ");
                sb.Append(" TRUNC(RANGE_CENTER_LINE,16) RANGE_CENTER_LINE,         ");
                sb.Append(" TRUNC(EWMA_M_UCL,16) EWMA_M_UCL,                       ");
                sb.Append(" TRUNC(EWMA_M_LCL,16) EWMA_M_LCL,                       ");
                sb.Append(" TRUNC(EWMA_M_CENTER_LINE,16) EWMA_M_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_R_UCL,16) EWMA_R_UCL,                       ");
                sb.Append(" TRUNC(EWMA_R_LCL,16) EWMA_R_LCL,                       ");
                sb.Append(" TRUNC(EWMA_R_CENTER_LINE,16) EWMA_R_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_S_UCL,16) EWMA_S_UCL,                       ");
                sb.Append(" TRUNC(EWMA_S_LCL,16) EWMA_S_LCL,                       ");
                sb.Append(" TRUNC(EWMA_S_CENTER_LINE,16) EWMA_S_CENTER_LINE,       ");
                sb.Append(" TRUNC(MA_UCL,16) MA_UCL,                               ");
                sb.Append(" TRUNC(MA_LCL,16) MA_LCL,                               ");
                sb.Append(" TRUNC(MA_CENTER_LINE,16) MA_CENTER_LINE,               ");
                sb.Append(" TRUNC(MR_UCL,16) MR_UCL,                               ");
                sb.Append(" TRUNC(MR_LCL,16) MR_LCL,                               ");
                sb.Append(" TRUNC(MR_CENTER_LINE,16) MR_CENTER_LINE,               ");
                sb.Append(" TRUNC(MS_UCL,16) MS_UCL,                               ");
                sb.Append(" TRUNC(MS_LCL,16) MS_LCL,                               ");
                sb.Append(" TRUNC(MS_CENTER_LINE,16) MS_CENTER_LINE,               ");
                sb.Append(" TRUNC(ZONE_A_UCL,16) ZONE_A_UCL,                       ");
                sb.Append(" TRUNC(ZONE_A_LCL,16) ZONE_A_LCL,                       ");
                sb.Append(" TRUNC(ZONE_B_UCL,16) ZONE_B_UCL,                       ");
                sb.Append(" TRUNC(ZONE_B_LCL,16)  ZONE_B_LCL,                      ");
                sb.Append(" TRUNC(ZONE_C_UCL,16) ZONE_C_UCL,                       ");
                sb.Append(" TRUNC(ZONE_C_LCL,16) ZONE_C_LCL,                       ");
                sb.Append(" ACTIVATION_YN,                                         ");
                sb.Append(" SUB_INTERLOCK_YN,                                      ");
                sb.Append(" INHERIT_MAIN_YN,                                       ");
                sb.Append(" OFFSET_YN,                                             ");
                sb.Append(" TRUNC(SPEC_USL_OFFSET,16) SPEC_USL_OFFSET,             ");
                sb.Append(" TRUNC(SPEC_LSL_OFFSET,16) SPEC_LSL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_UCL_OFFSET,16) MEAN_UCL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_LCL_OFFSET,16) MEAN_LCL_OFFSET,             ");
                sb.Append(" TRUNC(RAW_UCL_OFFSET,16) RAW_UCL_OFFSET,               ");
                sb.Append(" TRUNC(RAW_LCL_OFFSET,16) RAW_LCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_UCL_OFFSET,16) STD_UCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_LCL_OFFSET,16) STD_LCL_OFFSET,               ");
                sb.Append(" TRUNC(RANGE_UCL_OFFSET,16) RANGE_UCL_OFFSET,           ");
                sb.Append(" TRUNC(RANGE_LCL_OFFSET,16) RANGE_LCL_OFFSET,           ");
                sb.Append(" TRUNC(EWMA_M_UCL_OFFSET,16) EWMA_M_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_M_LCL_OFFSET,16) EWMA_M_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_UCL_OFFSET,16) EWMA_R_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_LCL_OFFSET,16) EWMA_R_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_UCL_OFFSET,16) EWMA_S_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_LCL_OFFSET,16) EWMA_S_LCL_OFFSET,         ");
                sb.Append(" TRUNC(MA_UCL_OFFSET,16) MA_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MA_LCL_OFFSET,16) MA_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_UCL_OFFSET,16) MR_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_LCL_OFFSET,16) MR_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_UCL_OFFSET,16) MS_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_LCL_OFFSET,16) MS_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(ZONE_A_UCL_OFFSET,16) ZONE_A_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_A_LCL_OFFSET,16) ZONE_A_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_UCL_OFFSET,16) ZONE_B_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_LCL_OFFSET,16) ZONE_B_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_UCL_OFFSET,16) ZONE_C_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_LCL_OFFSET,16) ZONE_C_LCL_OFFSET,         ");
                sb.Append(" MISSINGDATA_YN,                                        ");
                sb.Append(" TRUNC(UPPER_HALT,16) UPPER_HALT,                       ");
                sb.Append(" TRUNC(LOWER_HALT,16) LOWER_HALT,                       ");
                sb.Append(" TRUNC(UPPER_FILTER,16) UPPER_FILTER,                   ");
                sb.Append(" TRUNC(LOWER_FILTER,16) LOWER_FILTER,                   ");
                sb.Append(" SUB_AUTOCALC_YN,                                       ");
                sb.Append(" TRUNC(UPPER_TECHNICAL_LIMIT,16) UPPER_TECHNICAL_LIMIT, ");
                sb.Append(" TRUNC(LOWER_TECHNICAL_LIMIT,16) LOWER_TECHNICAL_LIMIT, ");
                sb.Append(" USE_NORM_YN,                                           ");
                sb.Append(" CHART_MODE_CD,                                         ");
                sb.Append(" VERSION,                                               ");
                sb.Append(" VALIDATION_SAME_MODULE_YN,                             ");
                sb.Append(" SPC_DATA_LEVEL,                                        ");
                sb.Append(" CHART_DESCRIPTION,                                     ");
                sb.Append(" SAVE_COMMENT,                                          ");
                sb.Append(" CHANGED_ITEMS                                          ");
                sb.Append(" FROM MODEL_CONFIG_MST_SPC                              ");
                sb.Append(" WHERE MODEL_RAWID = :RAWID  ORDER BY MAIN_YN DESC      ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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


                //#02. MODEL_CONTEXT_MST_SPC

                sb.Remove(0, sb.Length);

                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" KEY_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, GROUP_YN, VERSION ");
                sb.Append("  FROM MODEL_CONTEXT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID IN ( SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :RAWID) ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }
        //SPC-812 Calculation
        private const string SQL_SEARCH_CALC_MODEL_SAVE = " SELECT *                                                                   "
                                                    + "   FROM MODEL_MST_SPC                                                      "
                                                    + "  WHERE RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC WHERE RAWID = :RAWID ) ";

        //SPC-812 Calculation
        private const string SQL_SEARCH_CALC_MODEL_CONFIG_SAVE = "           SELECT *                                                  "
                                                    + "             FROM MODEL_CONFIG_MST_SPC                                     "
                                                    + "            WHERE RAWID = :RAWID                                     ";
        //SPC-812 Calculation
        private const string SQL_SEARCH_CALC_MODEL_CONTEXT_SAVE = " SELECT *                                                            "
                                                    + "            FROM MODEL_CONTEXT_MST_SPC                                      "
                                                    + "           WHERE MODEL_CONFIG_RAWID = :RAWID ";


        //SPC-812 SPC Calculation.
        public DataSet GetSPCCalcModelDataSave(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            string sSQL = "";

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_RAWID))
                {
                    llstWhereData.Add(COLUMN.RAWID, llstParam[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                //#00. MODEL_MST_SPC
                sSQL = SQL_SEARCH_CALC_MODEL_SAVE;

                dsTemp = this.Query(sSQL, llstWhereData);

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

                //sSQL = string.Format(SQL_SEARCH_CALC_MODEL_CONFIG, strSubQueryEQP, strSubQueryModule, strSubQueryRecipe, strSubQueryStep);

                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT                                                 ");
                sb.Append(" RAWID,                                                 ");
                sb.Append(" MODEL_RAWID,                                           ");
                sb.Append(" PARAM_TYPE_CD,                                         ");
                sb.Append(" PARAM_ALIAS,                                           ");
                if (useComma)
                {
                    sb.Append(" REPLACE(PARAM_LIST,';', ',') PARAM_LIST,           ");
                }
                else
                    sb.Append(" PARAM_LIST,                                            ");

                sb.Append(" COMPLEX_YN,                                            ");
                sb.Append(" REF_PARAM,                                             ");

                if (useComma)
                {
                    sb.Append(" REPLACE(REF_PARAM_LIST,';', ',') REF_PARAM_LIST,   ");
                }
                sb.Append(" REF_PARAM_LIST,                                        ");

                sb.Append(" MAIN_YN,                                               ");
                sb.Append(" AUTO_TYPE_CD,                                          ");
                sb.Append(" AUTO_SUB_YN,                                           ");
                sb.Append(" USE_EXTERNAL_SPEC_YN,                                  ");
                sb.Append(" INTERLOCK_YN,                                          ");
                sb.Append(" TRUNC(UPPER_SPEC, 16) UPPER_SPEC,                      ");
                sb.Append(" TRUNC(LOWER_SPEC,16) LOWER_SPEC,                       ");
                sb.Append(" TRUNC(UPPER_CONTROL,16) UPPER_CONTROL,                 ");
                sb.Append(" TRUNC(LOWER_CONTROL,16) LOWER_CONTROL,                 ");
                sb.Append(" SAMPLE_COUNT,                                          ");
                sb.Append(" EWMA_LAMBDA,                                           ");
                sb.Append(" MOVING_COUNT,                                          ");
                sb.Append(" TRUNC(CENTER_LINE,16) CENTER_LINE,                     ");
                sb.Append(" DESCRIPTION,                                           ");
                sb.Append(" CREATE_DTTS,                                           ");
                sb.Append(" CREATE_BY,                                             ");
                sb.Append(" LAST_UPDATE_DTTS,                                      ");
                sb.Append(" LAST_UPDATE_BY,                                        ");
                sb.Append(" AUTOCALC_YN,                                           ");
                sb.Append(" TRUNC(TARGET,16) TARGET,                               ");
                sb.Append(" MANAGE_TYPE_CD,                                        ");
                sb.Append(" TRUNC(STD,16) STD,                                     ");
                sb.Append(" TRUNC(RAW_UCL,16) RAW_UCL,                             ");
                sb.Append(" TRUNC(RAW_LCL,16) RAW_LCL,                             ");
                sb.Append(" TRUNC(RAW_CENTER_LINE,16) RAW_CENTER_LINE,             ");
                sb.Append(" TRUNC (STD_UCL, 16) STD_UCL,                           ");
                sb.Append(" TRUNC (STD_LCL, 16) STD_LCL,                           ");
                sb.Append(" TRUNC(STD_CENTER_LINE,16) STD_CENTER_LINE,             ");
                sb.Append(" TRUNC(RANGE_UCL,16) RANGE_UCL,                         ");
                sb.Append(" TRUNC(RANGE_LCL,16) RANGE_LCL,                         ");
                sb.Append(" TRUNC(RANGE_CENTER_LINE,16) RANGE_CENTER_LINE,         ");
                sb.Append(" TRUNC(EWMA_M_UCL,16) EWMA_M_UCL,                       ");
                sb.Append(" TRUNC(EWMA_M_LCL,16) EWMA_M_LCL,                       ");
                sb.Append(" TRUNC(EWMA_M_CENTER_LINE,16) EWMA_M_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_R_UCL,16) EWMA_R_UCL,                       ");
                sb.Append(" TRUNC(EWMA_R_LCL,16) EWMA_R_LCL,                       ");
                sb.Append(" TRUNC(EWMA_R_CENTER_LINE,16) EWMA_R_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_S_UCL,16) EWMA_S_UCL,                       ");
                sb.Append(" TRUNC(EWMA_S_LCL,16) EWMA_S_LCL,                       ");
                sb.Append(" TRUNC(EWMA_S_CENTER_LINE,16) EWMA_S_CENTER_LINE,       ");
                sb.Append(" TRUNC(MA_UCL,16) MA_UCL,                               ");
                sb.Append(" TRUNC(MA_LCL,16) MA_LCL,                               ");
                sb.Append(" TRUNC(MA_CENTER_LINE,16) MA_CENTER_LINE,               ");
                sb.Append(" TRUNC(MR_UCL,16) MR_UCL,                               ");
                sb.Append(" TRUNC(MR_LCL,16) MR_LCL,                               ");
                sb.Append(" TRUNC(MR_CENTER_LINE,16) MR_CENTER_LINE,               ");
                sb.Append(" TRUNC(MS_UCL,16) MS_UCL,                               ");
                sb.Append(" TRUNC(MS_LCL,16) MS_LCL,                               ");
                sb.Append(" TRUNC(MS_CENTER_LINE,16) MS_CENTER_LINE,               ");
                sb.Append(" TRUNC(ZONE_A_UCL,16) ZONE_A_UCL,                       ");
                sb.Append(" TRUNC(ZONE_A_LCL,16) ZONE_A_LCL,                       ");
                sb.Append(" TRUNC(ZONE_B_UCL,16) ZONE_B_UCL,                       ");
                sb.Append(" TRUNC(ZONE_B_LCL,16)  ZONE_B_LCL,                      ");
                sb.Append(" TRUNC(ZONE_C_UCL,16) ZONE_C_UCL,                       ");
                sb.Append(" TRUNC(ZONE_C_LCL,16) ZONE_C_LCL,                       ");
                sb.Append(" ACTIVATION_YN,                                         ");
                sb.Append(" SUB_INTERLOCK_YN,                                      ");
                sb.Append(" INHERIT_MAIN_YN,                                       ");
                sb.Append(" OFFSET_YN,                                             ");
                sb.Append(" TRUNC(SPEC_USL_OFFSET,16) SPEC_USL_OFFSET,             ");
                sb.Append(" TRUNC(SPEC_LSL_OFFSET,16) SPEC_LSL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_UCL_OFFSET,16) MEAN_UCL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_LCL_OFFSET,16) MEAN_LCL_OFFSET,             ");
                sb.Append(" TRUNC(RAW_UCL_OFFSET,16) RAW_UCL_OFFSET,               ");
                sb.Append(" TRUNC(RAW_LCL_OFFSET,16) RAW_LCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_UCL_OFFSET,16) STD_UCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_LCL_OFFSET,16) STD_LCL_OFFSET,               ");
                sb.Append(" TRUNC(RANGE_UCL_OFFSET,16) RANGE_UCL_OFFSET,           ");
                sb.Append(" TRUNC(RANGE_LCL_OFFSET,16) RANGE_LCL_OFFSET,           ");
                sb.Append(" TRUNC(EWMA_M_UCL_OFFSET,16) EWMA_M_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_M_LCL_OFFSET,16) EWMA_M_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_UCL_OFFSET,16) EWMA_R_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_LCL_OFFSET,16) EWMA_R_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_UCL_OFFSET,16) EWMA_S_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_LCL_OFFSET,16) EWMA_S_LCL_OFFSET,         ");
                sb.Append(" TRUNC(MA_UCL_OFFSET,16) MA_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MA_LCL_OFFSET,16) MA_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_UCL_OFFSET,16) MR_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_LCL_OFFSET,16) MR_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_UCL_OFFSET,16) MS_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_LCL_OFFSET,16) MS_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(ZONE_A_UCL_OFFSET,16) ZONE_A_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_A_LCL_OFFSET,16) ZONE_A_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_UCL_OFFSET,16) ZONE_B_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_LCL_OFFSET,16) ZONE_B_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_UCL_OFFSET,16) ZONE_C_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_LCL_OFFSET,16) ZONE_C_LCL_OFFSET,         ");
                sb.Append(" MISSINGDATA_YN,                                        ");
                sb.Append(" TRUNC(UPPER_HALT,16) UPPER_HALT,                       ");
                sb.Append(" TRUNC(LOWER_HALT,16) LOWER_HALT,                       ");
                sb.Append(" TRUNC(UPPER_FILTER,16) UPPER_FILTER,                   ");
                sb.Append(" TRUNC(LOWER_FILTER,16) LOWER_FILTER,                   ");
                sb.Append(" SUB_AUTOCALC_YN,                                       ");
                sb.Append(" TRUNC(UPPER_TECHNICAL_LIMIT,16) UPPER_TECHNICAL_LIMIT, ");
                sb.Append(" TRUNC(LOWER_TECHNICAL_LIMIT,16) LOWER_TECHNICAL_LIMIT, ");
                sb.Append(" USE_NORM_YN,                                           ");
                sb.Append(" CHART_MODE_CD,                                         ");
                sb.Append(" VERSION,                                               ");
                sb.Append(" VALIDATION_SAME_MODULE_YN,                             ");
                sb.Append(" SPC_DATA_LEVEL,                                        ");
                sb.Append(" CHART_DESCRIPTION,                                     ");
                sb.Append(" SAVE_COMMENT,                                          ");
                sb.Append(" CHANGED_ITEMS                                          ");
                sb.Append(" FROM MODEL_CONFIG_MST_SPC                              ");
                sb.Append(" WHERE RAWID = :RAWID                                   ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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


                //#02. MODEL_CONTEXT_MST_SPC

                //sSQL = string.Format(SQL_SEARCH_CALC_MODEL_CONTEXT, strSubQueryEQP, strSubQueryModule, strSubQueryRecipe, strSubQueryStep);
                sb.Remove(0, sb.Length);

                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" KEY_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, GROUP_YN, VERSION ");
                sb.Append("  FROM MODEL_CONTEXT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :RAWID                                ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        private const string SQL_SEARCH_CALC_MODEL_POPUP = " SELECT *                                                                 "
                                                        + "   FROM MODEL_MST_SPC                                                      "
                                                        + "  WHERE RAWID IN (                                                         "
                                                        + "           SELECT DISTINCT MODEL_RAWID                                     "
                                                        + "             FROM MODEL_CONFIG_MST_SPC                                     "
                                                        + "            WHERE RAWID = :CONFIGRAWID)                                    ";

        //SPC-704 MultiCalc Popup
        private const string SQL_SEARCH_MULTI_CALC_MODEL_POPUP = " SELECT *                                                                 "
                                                        + "   FROM MODEL_MST_SPC                                                      "
                                                        + "  WHERE RAWID IN (                                                         "
                                                        + "           SELECT DISTINCT MODEL_RAWID                                     "
                                                        + "             FROM MODEL_CONFIG_MST_SPC                                     "
                                                        + "            WHERE RAWID IN(                                     ";


        //SPC-704 MultiCalculation
        public DataSet GetSPCMulCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID))
                {
                    llstWhereData.Add("CONFIGRAWID", llstParam[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                StringBuilder sb = new StringBuilder();

                sb.Append(SQL_SEARCH_MULTI_CALC_MODEL_POPUP);
                sb.Append(llstWhereData["CONFIGRAWID"]).Append("  ))");

                //#00. MODEL_MST_SPC


                // dsTemp = this.Query(SQL_SEARCH_MULTI_CALC_MODEL_POPUP, llstWhereData);
                dsTemp = this.Query(sb.ToString());

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

                StringBuilder sb2 = new StringBuilder();

                sb2.Append(" SELECT                                                 ");
                sb2.Append(" RAWID,                                                 ");
                sb2.Append(" MODEL_RAWID,                                           ");
                sb2.Append(" PARAM_TYPE_CD,                                         ");
                sb2.Append(" PARAM_ALIAS,                                           ");
                if (useComma)
                {
                    sb2.Append(" REPLACE(PARAM_LIST,';', ',') PARAM_LIST,           ");
                }
                else
                    sb2.Append(" PARAM_LIST,                                            ");

                sb2.Append(" COMPLEX_YN,                                            ");
                sb2.Append(" REF_PARAM,                                             ");

                if (useComma)
                {
                    sb2.Append(" REPLACE(REF_PARAM_LIST,';', ',') REF_PARAM_LIST,   ");
                }
                sb2.Append(" REF_PARAM_LIST,                                        ");

                sb2.Append(" MAIN_YN,                                               ");
                sb2.Append(" AUTO_TYPE_CD,                                          ");
                sb2.Append(" AUTO_SUB_YN,                                           ");
                sb2.Append(" USE_EXTERNAL_SPEC_YN,                                  ");
                sb2.Append(" INTERLOCK_YN,                                          ");
                sb2.Append(" TRUNC(UPPER_SPEC, 16) UPPER_SPEC,                      ");
                sb2.Append(" TRUNC(LOWER_SPEC,16) LOWER_SPEC,                       ");
                sb2.Append(" TRUNC(UPPER_CONTROL,16) UPPER_CONTROL,                 ");
                sb2.Append(" TRUNC(LOWER_CONTROL,16) LOWER_CONTROL,                 ");
                sb2.Append(" SAMPLE_COUNT,                                          ");
                sb2.Append(" EWMA_LAMBDA,                                           ");
                sb2.Append(" MOVING_COUNT,                                          ");
                sb2.Append(" TRUNC(CENTER_LINE,16) CENTER_LINE,                     ");
                sb2.Append(" DESCRIPTION,                                           ");
                sb2.Append(" CREATE_DTTS,                                           ");
                sb2.Append(" CREATE_BY,                                             ");
                sb2.Append(" LAST_UPDATE_DTTS,                                      ");
                sb2.Append(" LAST_UPDATE_BY,                                        ");
                sb2.Append(" AUTOCALC_YN,                                           ");
                sb2.Append(" TRUNC(TARGET,16) TARGET,                               ");
                sb2.Append(" MANAGE_TYPE_CD,                                        ");
                sb2.Append(" TRUNC(STD,16) STD,                                     ");
                sb2.Append(" TRUNC(RAW_UCL,16) RAW_UCL,                             ");
                sb2.Append(" TRUNC(RAW_LCL,16) RAW_LCL,                             ");
                sb2.Append(" TRUNC(RAW_CENTER_LINE,16) RAW_CENTER_LINE,             ");
                sb2.Append(" TRUNC (STD_UCL, 16) STD_UCL,                           ");
                sb2.Append(" TRUNC (STD_LCL, 16) STD_LCL,                           ");
                sb2.Append(" TRUNC(STD_CENTER_LINE,16) STD_CENTER_LINE,             ");
                sb2.Append(" TRUNC(RANGE_UCL,16) RANGE_UCL,                         ");
                sb2.Append(" TRUNC(RANGE_LCL,16) RANGE_LCL,                         ");
                sb2.Append(" TRUNC(RANGE_CENTER_LINE,16) RANGE_CENTER_LINE,         ");
                sb2.Append(" TRUNC(EWMA_M_UCL,16) EWMA_M_UCL,                       ");
                sb2.Append(" TRUNC(EWMA_M_LCL,16) EWMA_M_LCL,                       ");
                sb2.Append(" TRUNC(EWMA_M_CENTER_LINE,16) EWMA_M_CENTER_LINE,       ");
                sb2.Append(" TRUNC(EWMA_R_UCL,16) EWMA_R_UCL,                       ");
                sb2.Append(" TRUNC(EWMA_R_LCL,16) EWMA_R_LCL,                       ");
                sb2.Append(" TRUNC(EWMA_R_CENTER_LINE,16) EWMA_R_CENTER_LINE,       ");
                sb2.Append(" TRUNC(EWMA_S_UCL,16) EWMA_S_UCL,                       ");
                sb2.Append(" TRUNC(EWMA_S_LCL,16) EWMA_S_LCL,                       ");
                sb2.Append(" TRUNC(EWMA_S_CENTER_LINE,16) EWMA_S_CENTER_LINE,       ");
                sb2.Append(" TRUNC(MA_UCL,16) MA_UCL,                               ");
                sb2.Append(" TRUNC(MA_LCL,16) MA_LCL,                               ");
                sb2.Append(" TRUNC(MA_CENTER_LINE,16) MA_CENTER_LINE,               ");
                sb2.Append(" TRUNC(MR_UCL,16) MR_UCL,                               ");
                sb2.Append(" TRUNC(MR_LCL,16) MR_LCL,                               ");
                sb2.Append(" TRUNC(MR_CENTER_LINE,16) MR_CENTER_LINE,               ");
                sb2.Append(" TRUNC(MS_UCL,16) MS_UCL,                               ");
                sb2.Append(" TRUNC(MS_LCL,16) MS_LCL,                               ");
                sb2.Append(" TRUNC(MS_CENTER_LINE,16) MS_CENTER_LINE,               ");
                sb2.Append(" TRUNC(ZONE_A_UCL,16) ZONE_A_UCL,                       ");
                sb2.Append(" TRUNC(ZONE_A_LCL,16) ZONE_A_LCL,                       ");
                sb2.Append(" TRUNC(ZONE_B_UCL,16) ZONE_B_UCL,                       ");
                sb2.Append(" TRUNC(ZONE_B_LCL,16)  ZONE_B_LCL,                      ");
                sb2.Append(" TRUNC(ZONE_C_UCL,16) ZONE_C_UCL,                       ");
                sb2.Append(" TRUNC(ZONE_C_LCL,16) ZONE_C_LCL,                       ");
                sb2.Append(" ACTIVATION_YN,                                         ");
                sb2.Append(" SUB_INTERLOCK_YN,                                      ");
                sb2.Append(" INHERIT_MAIN_YN,                                       ");
                sb2.Append(" OFFSET_YN,                                             ");
                sb2.Append(" TRUNC(SPEC_USL_OFFSET,16) SPEC_USL_OFFSET,             ");
                sb2.Append(" TRUNC(SPEC_LSL_OFFSET,16) SPEC_LSL_OFFSET,             ");
                sb2.Append(" TRUNC(MEAN_UCL_OFFSET,16) MEAN_UCL_OFFSET,             ");
                sb2.Append(" TRUNC(MEAN_LCL_OFFSET,16) MEAN_LCL_OFFSET,             ");
                sb2.Append(" TRUNC(RAW_UCL_OFFSET,16) RAW_UCL_OFFSET,               ");
                sb2.Append(" TRUNC(RAW_LCL_OFFSET,16) RAW_LCL_OFFSET,               ");
                sb2.Append(" TRUNC(STD_UCL_OFFSET,16) STD_UCL_OFFSET,               ");
                sb2.Append(" TRUNC(STD_LCL_OFFSET,16) STD_LCL_OFFSET,               ");
                sb2.Append(" TRUNC(RANGE_UCL_OFFSET,16) RANGE_UCL_OFFSET,           ");
                sb2.Append(" TRUNC(RANGE_LCL_OFFSET,16) RANGE_LCL_OFFSET,           ");
                sb2.Append(" TRUNC(EWMA_M_UCL_OFFSET,16) EWMA_M_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(EWMA_M_LCL_OFFSET,16) EWMA_M_LCL_OFFSET,         ");
                sb2.Append(" TRUNC(EWMA_R_UCL_OFFSET,16) EWMA_R_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(EWMA_R_LCL_OFFSET,16) EWMA_R_LCL_OFFSET,         ");
                sb2.Append(" TRUNC(EWMA_S_UCL_OFFSET,16) EWMA_S_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(EWMA_S_LCL_OFFSET,16) EWMA_S_LCL_OFFSET,         ");
                sb2.Append(" TRUNC(MA_UCL_OFFSET,16) MA_UCL_OFFSET,                 ");
                sb2.Append(" TRUNC(MA_LCL_OFFSET,16) MA_LCL_OFFSET,                 ");
                sb2.Append(" TRUNC(MR_UCL_OFFSET,16) MR_UCL_OFFSET,                 ");
                sb2.Append(" TRUNC(MR_LCL_OFFSET,16) MR_LCL_OFFSET,                 ");
                sb2.Append(" TRUNC(MS_UCL_OFFSET,16) MS_UCL_OFFSET,                 ");
                sb2.Append(" TRUNC(MS_LCL_OFFSET,16) MS_LCL_OFFSET,                 ");
                sb2.Append(" TRUNC(ZONE_A_UCL_OFFSET,16) ZONE_A_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(ZONE_A_LCL_OFFSET,16) ZONE_A_LCL_OFFSET,         ");
                sb2.Append(" TRUNC(ZONE_B_UCL_OFFSET,16) ZONE_B_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(ZONE_B_LCL_OFFSET,16) ZONE_B_LCL_OFFSET,         ");
                sb2.Append(" TRUNC(ZONE_C_UCL_OFFSET,16) ZONE_C_UCL_OFFSET,         ");
                sb2.Append(" TRUNC(ZONE_C_LCL_OFFSET,16) ZONE_C_LCL_OFFSET,         ");
                sb2.Append(" MISSINGDATA_YN,                                        ");
                sb2.Append(" TRUNC(UPPER_HALT,16) UPPER_HALT,                       ");
                sb2.Append(" TRUNC(LOWER_HALT,16) LOWER_HALT,                       ");
                sb2.Append(" TRUNC(UPPER_FILTER,16) UPPER_FILTER,                   ");
                sb2.Append(" TRUNC(LOWER_FILTER,16) LOWER_FILTER,                   ");
                sb2.Append(" SUB_AUTOCALC_YN,                                       ");
                sb2.Append(" TRUNC(UPPER_TECHNICAL_LIMIT,16) UPPER_TECHNICAL_LIMIT, ");
                sb2.Append(" TRUNC(LOWER_TECHNICAL_LIMIT,16) LOWER_TECHNICAL_LIMIT, ");
                sb2.Append(" USE_NORM_YN,                                           ");
                sb2.Append(" CHART_MODE_CD,                                         ");
                sb2.Append(" VERSION,                                               ");
                sb2.Append(" VALIDATION_SAME_MODULE_YN,                             ");
                sb2.Append(" SPC_DATA_LEVEL,                                        ");
                sb2.Append(" CHART_DESCRIPTION,                                     ");
                sb2.Append(" SAVE_COMMENT,                                          ");
                sb2.Append(" CHANGED_ITEMS                                          ");
                sb2.Append(" FROM MODEL_CONFIG_MST_SPC                              ");
                sb2.Append("            WHERE RAWID IN(                             ");
                sb2.Append(llstWhereData["CONFIGRAWID"]).Append(")");

                //#01. MODEL_CONFIG_MST_SPC

                dsTemp = this.Query(sb2.ToString());

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


                //#02. MODEL_CONTEXT_MST_SPC

                StringBuilder sb3 = new StringBuilder();

                sb3.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb3.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb3.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb3.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb3.Append(" KEY_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, GROUP_YN, VERSION ");
                sb3.Append("            FROM MODEL_CONTEXT_MST_SPC                             ");
                sb3.Append("           WHERE MODEL_CONFIG_RAWID IN(                            ");
                sb3.Append(llstWhereData["CONFIGRAWID"]).Append(")");

                dsTemp = this.Query(sb3.ToString());

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID))
                {
                    llstWhereData.Add("CONFIGRAWID", llstParam[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                //#00. MODEL_MST_SPC

                dsTemp = this.Query(SQL_SEARCH_CALC_MODEL_POPUP, llstWhereData);

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

                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT                                                 ");
                sb.Append(" RAWID,                                                 ");
                sb.Append(" MODEL_RAWID,                                           ");
                sb.Append(" PARAM_TYPE_CD,                                         ");
                sb.Append(" PARAM_ALIAS,                                           ");
                if (useComma)
                {
                    sb.Append(" REPLACE(PARAM_LIST,';', ',') PARAM_LIST,           ");
                }
                else
                    sb.Append(" PARAM_LIST,                                            ");

                sb.Append(" COMPLEX_YN,                                            ");
                sb.Append(" REF_PARAM,                                             ");

                if (useComma)
                {
                    sb.Append(" REPLACE(REF_PARAM_LIST,';', ',') REF_PARAM_LIST,   ");
                }
                sb.Append(" REF_PARAM_LIST,                                        ");

                sb.Append(" MAIN_YN,                                               ");
                sb.Append(" AUTO_TYPE_CD,                                          ");
                sb.Append(" AUTO_SUB_YN,                                           ");
                sb.Append(" USE_EXTERNAL_SPEC_YN,                                  ");
                sb.Append(" INTERLOCK_YN,                                          ");
                sb.Append(" TRUNC(UPPER_SPEC, 16) UPPER_SPEC,                      ");
                sb.Append(" TRUNC(LOWER_SPEC,16) LOWER_SPEC,                       ");
                sb.Append(" TRUNC(UPPER_CONTROL,16) UPPER_CONTROL,                 ");
                sb.Append(" TRUNC(LOWER_CONTROL,16) LOWER_CONTROL,                 ");
                sb.Append(" SAMPLE_COUNT,                                          ");
                sb.Append(" EWMA_LAMBDA,                                           ");
                sb.Append(" MOVING_COUNT,                                          ");
                sb.Append(" TRUNC(CENTER_LINE,16) CENTER_LINE,                     ");
                sb.Append(" DESCRIPTION,                                           ");
                sb.Append(" CREATE_DTTS,                                           ");
                sb.Append(" CREATE_BY,                                             ");
                sb.Append(" LAST_UPDATE_DTTS,                                      ");
                sb.Append(" LAST_UPDATE_BY,                                        ");
                sb.Append(" AUTOCALC_YN,                                           ");
                sb.Append(" TRUNC(TARGET,16) TARGET,                               ");
                sb.Append(" MANAGE_TYPE_CD,                                        ");
                sb.Append(" TRUNC(STD,16) STD,                                     ");
                sb.Append(" TRUNC(RAW_UCL,16) RAW_UCL,                             ");
                sb.Append(" TRUNC(RAW_LCL,16) RAW_LCL,                             ");
                sb.Append(" TRUNC(RAW_CENTER_LINE,16) RAW_CENTER_LINE,             ");
                sb.Append(" TRUNC (STD_UCL, 16) STD_UCL,                           ");
                sb.Append(" TRUNC (STD_LCL, 16) STD_LCL,                           ");
                sb.Append(" TRUNC(STD_CENTER_LINE,16) STD_CENTER_LINE,             ");
                sb.Append(" TRUNC(RANGE_UCL,16) RANGE_UCL,                         ");
                sb.Append(" TRUNC(RANGE_LCL,16) RANGE_LCL,                         ");
                sb.Append(" TRUNC(RANGE_CENTER_LINE,16) RANGE_CENTER_LINE,         ");
                sb.Append(" TRUNC(EWMA_M_UCL,16) EWMA_M_UCL,                       ");
                sb.Append(" TRUNC(EWMA_M_LCL,16) EWMA_M_LCL,                       ");
                sb.Append(" TRUNC(EWMA_M_CENTER_LINE,16) EWMA_M_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_R_UCL,16) EWMA_R_UCL,                       ");
                sb.Append(" TRUNC(EWMA_R_LCL,16) EWMA_R_LCL,                       ");
                sb.Append(" TRUNC(EWMA_R_CENTER_LINE,16) EWMA_R_CENTER_LINE,       ");
                sb.Append(" TRUNC(EWMA_S_UCL,16) EWMA_S_UCL,                       ");
                sb.Append(" TRUNC(EWMA_S_LCL,16) EWMA_S_LCL,                       ");
                sb.Append(" TRUNC(EWMA_S_CENTER_LINE,16) EWMA_S_CENTER_LINE,       ");
                sb.Append(" TRUNC(MA_UCL,16) MA_UCL,                               ");
                sb.Append(" TRUNC(MA_LCL,16) MA_LCL,                               ");
                sb.Append(" TRUNC(MA_CENTER_LINE,16) MA_CENTER_LINE,               ");
                sb.Append(" TRUNC(MR_UCL,16) MR_UCL,                               ");
                sb.Append(" TRUNC(MR_LCL,16) MR_LCL,                               ");
                sb.Append(" TRUNC(MR_CENTER_LINE,16) MR_CENTER_LINE,               ");
                sb.Append(" TRUNC(MS_UCL,16) MS_UCL,                               ");
                sb.Append(" TRUNC(MS_LCL,16) MS_LCL,                               ");
                sb.Append(" TRUNC(MS_CENTER_LINE,16) MS_CENTER_LINE,               ");
                sb.Append(" TRUNC(ZONE_A_UCL,16) ZONE_A_UCL,                       ");
                sb.Append(" TRUNC(ZONE_A_LCL,16) ZONE_A_LCL,                       ");
                sb.Append(" TRUNC(ZONE_B_UCL,16) ZONE_B_UCL,                       ");
                sb.Append(" TRUNC(ZONE_B_LCL,16)  ZONE_B_LCL,                      ");
                sb.Append(" TRUNC(ZONE_C_UCL,16) ZONE_C_UCL,                       ");
                sb.Append(" TRUNC(ZONE_C_LCL,16) ZONE_C_LCL,                       ");
                sb.Append(" ACTIVATION_YN,                                         ");
                sb.Append(" SUB_INTERLOCK_YN,                                      ");
                sb.Append(" INHERIT_MAIN_YN,                                       ");
                sb.Append(" OFFSET_YN,                                             ");
                sb.Append(" TRUNC(SPEC_USL_OFFSET,16) SPEC_USL_OFFSET,             ");
                sb.Append(" TRUNC(SPEC_LSL_OFFSET,16) SPEC_LSL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_UCL_OFFSET,16) MEAN_UCL_OFFSET,             ");
                sb.Append(" TRUNC(MEAN_LCL_OFFSET,16) MEAN_LCL_OFFSET,             ");
                sb.Append(" TRUNC(RAW_UCL_OFFSET,16) RAW_UCL_OFFSET,               ");
                sb.Append(" TRUNC(RAW_LCL_OFFSET,16) RAW_LCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_UCL_OFFSET,16) STD_UCL_OFFSET,               ");
                sb.Append(" TRUNC(STD_LCL_OFFSET,16) STD_LCL_OFFSET,               ");
                sb.Append(" TRUNC(RANGE_UCL_OFFSET,16) RANGE_UCL_OFFSET,           ");
                sb.Append(" TRUNC(RANGE_LCL_OFFSET,16) RANGE_LCL_OFFSET,           ");
                sb.Append(" TRUNC(EWMA_M_UCL_OFFSET,16) EWMA_M_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_M_LCL_OFFSET,16) EWMA_M_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_UCL_OFFSET,16) EWMA_R_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_R_LCL_OFFSET,16) EWMA_R_LCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_UCL_OFFSET,16) EWMA_S_UCL_OFFSET,         ");
                sb.Append(" TRUNC(EWMA_S_LCL_OFFSET,16) EWMA_S_LCL_OFFSET,         ");
                sb.Append(" TRUNC(MA_UCL_OFFSET,16) MA_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MA_LCL_OFFSET,16) MA_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_UCL_OFFSET,16) MR_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MR_LCL_OFFSET,16) MR_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_UCL_OFFSET,16) MS_UCL_OFFSET,                 ");
                sb.Append(" TRUNC(MS_LCL_OFFSET,16) MS_LCL_OFFSET,                 ");
                sb.Append(" TRUNC(ZONE_A_UCL_OFFSET,16) ZONE_A_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_A_LCL_OFFSET,16) ZONE_A_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_UCL_OFFSET,16) ZONE_B_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_B_LCL_OFFSET,16) ZONE_B_LCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_UCL_OFFSET,16) ZONE_C_UCL_OFFSET,         ");
                sb.Append(" TRUNC(ZONE_C_LCL_OFFSET,16) ZONE_C_LCL_OFFSET,         ");
                sb.Append(" MISSINGDATA_YN,                                        ");
                sb.Append(" TRUNC(UPPER_HALT,16) UPPER_HALT,                       ");
                sb.Append(" TRUNC(LOWER_HALT,16) LOWER_HALT,                       ");
                sb.Append(" TRUNC(UPPER_FILTER,16) UPPER_FILTER,                   ");
                sb.Append(" TRUNC(LOWER_FILTER,16) LOWER_FILTER,                   ");
                sb.Append(" SUB_AUTOCALC_YN,                                       ");
                sb.Append(" TRUNC(UPPER_TECHNICAL_LIMIT,16) UPPER_TECHNICAL_LIMIT, ");
                sb.Append(" TRUNC(LOWER_TECHNICAL_LIMIT,16) LOWER_TECHNICAL_LIMIT, ");
                sb.Append(" USE_NORM_YN,                                           ");
                sb.Append(" CHART_MODE_CD,                                         ");
                sb.Append(" VERSION,                                               ");
                sb.Append(" VALIDATION_SAME_MODULE_YN,                             ");
                sb.Append(" SPC_DATA_LEVEL,                                        ");
                sb.Append(" CHART_DESCRIPTION,                                     ");
                sb.Append(" SAVE_COMMENT,                                          ");
                sb.Append(" CHANGED_ITEMS                                          ");
                sb.Append(" FROM MODEL_CONFIG_MST_SPC                              ");
                sb.Append(" WHERE RAWID = :CONFIGRAWID                             ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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


                //#02. MODEL_CONTEXT_MST_SPC

                sb.Remove(0, sb.Length);

                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" KEY_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, GROUP_YN, VERSION ");
                sb.Append("  FROM MODEL_CONTEXT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :CONFIGRAWID                          ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        //public DataSet SaveSPCModelData(byte[] param)
        //{
        //    DataSet dsReturn = new DataSet();

        //    LinkedList llstParam = new LinkedList();
        //    llstParam.SetSerialData(param);

        //    string sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();
        //    ConfigMode configMode = (ConfigMode)llstParam[Definition.CONDITION_KEY_CONFIG_MODE];

        //    DataTable dtModel = (DataTable)llstParam[TABLE.MODEL_MST_SPC];
        //    DataTable dtConfig = (DataTable)llstParam[TABLE.MODEL_CONFIG_MST_SPC];
        //    DataTable dtConfigOpt = (DataTable)llstParam[TABLE.MODEL_CONFIG_OPT_MST_SPC];
        //    DataTable dtContext = (DataTable)llstParam[TABLE.MODEL_CONTEXT_MST_SPC];
        //    DataTable dtRule = (DataTable)llstParam[TABLE.MODEL_RULE_MST_SPC];
        //    DataTable dtRuleOpt = (DataTable)llstParam[TABLE.MODEL_RULE_OPT_MST_SPC];
        //    DataTable dtAutoCalc = (DataTable)llstParam[TABLE.MODEL_AUTOCALC_MST_SPC];

        //    try
        //    {

        //        switch (configMode)
        //        {
        //            case ConfigMode.CREATE_MAIN:
        //            case ConfigMode.SAVE_AS:
        //                dsReturn = this.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc);
        //                break;
        //            case ConfigMode.CREATE_SUB:
        //                dsReturn = this.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc);
        //                break;

        //            case ConfigMode.MODIFY:
        //                List<string> lstChangedMasterColList = (List<string>)llstParam["CHANGED_MASTER_COL_LIST"];
        //                dsReturn = this.ModifySPCModel(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, lstChangedMasterColList);
        //                break;

        //            case ConfigMode.DEFAULT:
        //                dsReturn = this.SaveDefaultConfig(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
        //    }
        //    finally
        //    {
        //        //resource 해제
        //        if (dtModel != null) { dtModel.Dispose(); dtModel = null; }
        //        if (dtConfig != null) { dtConfig.Dispose(); dtConfig = null; }
        //        if (dtConfigOpt != null) { dtConfigOpt.Dispose(); dtConfigOpt = null; }
        //        if (dtContext != null) { dtContext.Dispose(); dtContext = null; }
        //        if (dtAutoCalc != null) { dtAutoCalc.Dispose(); dtAutoCalc = null; }
        //        if (dtRule != null) { dtRule.Dispose(); dtRule = null; }
        //        if (dtRuleOpt != null) { dtRuleOpt.Dispose(); dtRuleOpt = null; }

        //        llstParam.Clear(); llstParam = null;
        //    }

        //    return dsReturn;
        //}




        //public DataSet CreateSPCModel(ConfigMode configMode, string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, DataTable dtFilter)
        //{
        //    DataSet dsResult = new DataSet();

        //    LinkedList llstFieldData = new LinkedList();

        //    try
        //    {
        //        base.BeginTrans();

        //        //#01. MODEL_MST_SPC
        //        foreach (DataRow drModel in dtModel.Rows)
        //        {
        //            decimal modelRawID = 0;

        //            if (configMode.Equals(ConfigMode.CREATE_MAIN) || configMode.Equals(ConfigMode.SAVE_AS))
        //            {
        //                modelRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_MST_SPC);

        //                //llstFieldData.Add(COLUMN., drModel[COLUMN.]);

        //                llstFieldData.Add(COLUMN.RAWID, modelRawID);

        //                llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
        //                llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
        //                llstFieldData.Add(COLUMN.LOCATION_RAWID, drModel[COLUMN.LOCATION_RAWID]);
        //                llstFieldData.Add(COLUMN.AREA_RAWID, drModel[COLUMN.AREA_RAWID]);
        //                llstFieldData.Add(COLUMN.EQP_MODEL, drModel[COLUMN.EQP_MODEL]);

        //                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                base.Insert(TABLE.MODEL_MST_SPC, llstFieldData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    base.RollBack();
        //                    return dsResult;
        //                }

        //                drModel[COLUMN.RAWID] = modelRawID;
        //            }
        //            else if (configMode.Equals(ConfigMode.CREATE_SUB))
        //            {
        //                modelRawID = decimal.Parse(drModel[COLUMN.RAWID].ToString());
        //            }

        //            //#02. MODEL_CONFIG_MST_SPC
        //            foreach (DataRow drConfig in dtConfig.Rows)
        //            {
        //                decimal configRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_CONFIG_MST_SPC);

        //                llstFieldData.Clear();
        //                llstFieldData.Add(COLUMN.RAWID, configRawID);
        //                llstFieldData.Add(COLUMN.MODEL_RAWID, modelRawID);

        //                llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
        //                llstFieldData.Add(COLUMN.PARAM_TYPE_CD, drConfig[COLUMN.PARAM_TYPE_CD]);
        //                llstFieldData.Add(COLUMN.COMPLEX_YN, drConfig[COLUMN.COMPLEX_YN]);
        //                llstFieldData.Add(COLUMN.PARAM_LIST, drConfig[COLUMN.PARAM_LIST]);
        //                llstFieldData.Add(COLUMN.REF_PARAM, drConfig[COLUMN.REF_PARAM]);
        //                llstFieldData.Add(COLUMN.REF_PARAM_LIST, drConfig[COLUMN.REF_PARAM_LIST]);
        //                llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
        //                llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
        //                llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
        //                llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
        //                llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
        //                llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
        //                llstFieldData.Add(COLUMN.USE_EXTERNAL_SPEC_YN, drConfig[COLUMN.USE_EXTERNAL_SPEC_YN]);
        //                llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
        //                llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
        //                llstFieldData.Add(COLUMN.UPPER_SPEC, drConfig[COLUMN.UPPER_SPEC]);
        //                llstFieldData.Add(COLUMN.LOWER_SPEC, drConfig[COLUMN.LOWER_SPEC]);
        //                llstFieldData.Add(COLUMN.TARGET, drConfig[COLUMN.TARGET]);
        //                llstFieldData.Add(COLUMN.UPPER_CONTROL, drConfig[COLUMN.UPPER_CONTROL]);
        //                llstFieldData.Add(COLUMN.LOWER_CONTROL, drConfig[COLUMN.LOWER_CONTROL]);
        //                llstFieldData.Add(COLUMN.CENTER_LINE, drConfig[COLUMN.CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.RAW_LCL, drConfig[COLUMN.RAW_LCL]);
        //                llstFieldData.Add(COLUMN.RAW_UCL, drConfig[COLUMN.RAW_UCL]);
        //                llstFieldData.Add(COLUMN.RAW_CENTER_LINE, drConfig[COLUMN.RAW_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.STD_LCL, drConfig[COLUMN.STD_LCL]);
        //                llstFieldData.Add(COLUMN.STD_UCL, drConfig[COLUMN.STD_UCL]);
        //                llstFieldData.Add(COLUMN.STD_CENTER_LINE, drConfig[COLUMN.STD_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.RANGE_LCL, drConfig[COLUMN.RANGE_LCL]);
        //                llstFieldData.Add(COLUMN.RANGE_UCL, drConfig[COLUMN.RANGE_UCL]);
        //                llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, drConfig[COLUMN.RANGE_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_M_LCL, drConfig[COLUMN.EWMA_M_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_M_UCL, drConfig[COLUMN.EWMA_M_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, drConfig[COLUMN.EWMA_M_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_R_LCL, drConfig[COLUMN.EWMA_R_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_R_UCL, drConfig[COLUMN.EWMA_R_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, drConfig[COLUMN.EWMA_R_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_S_LCL, drConfig[COLUMN.EWMA_S_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_S_UCL, drConfig[COLUMN.EWMA_S_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, drConfig[COLUMN.EWMA_S_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.MA_LCL, drConfig[COLUMN.MA_LCL]);
        //                llstFieldData.Add(COLUMN.MA_UCL, drConfig[COLUMN.MA_UCL]);
        //                llstFieldData.Add(COLUMN.MA_CENTER_LINE, drConfig[COLUMN.MA_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.MS_LCL, drConfig[COLUMN.MS_LCL]);
        //                llstFieldData.Add(COLUMN.MS_UCL, drConfig[COLUMN.MS_UCL]);
        //                llstFieldData.Add(COLUMN.MS_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.ZONE_A_LCL, drConfig[COLUMN.ZONE_A_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_A_UCL, drConfig[COLUMN.ZONE_A_UCL]);

        //                llstFieldData.Add(COLUMN.ZONE_B_LCL, drConfig[COLUMN.ZONE_B_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_B_UCL, drConfig[COLUMN.ZONE_B_UCL]);

        //                llstFieldData.Add(COLUMN.ZONE_C_LCL, drConfig[COLUMN.ZONE_C_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_C_UCL, drConfig[COLUMN.ZONE_C_UCL]);


        //                llstFieldData.Add(COLUMN.SAMPLE_COUNT, drConfig[COLUMN.SAMPLE_COUNT]);
        //                llstFieldData.Add(COLUMN.EWMA_LAMBDA, drConfig[COLUMN.EWMA_LAMBDA]);
        //                llstFieldData.Add(COLUMN.MOVING_COUNT, drConfig[COLUMN.MOVING_COUNT]);

        //                llstFieldData.Add(COLUMN.STD, drConfig[COLUMN.STD]);
        //                llstFieldData.Add(COLUMN.DESCRIPTION, drConfig[COLUMN.DESCRIPTION]);


        //                if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
        //                {
        //                    llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, drConfig[COLUMN.MANAGE_TYPE_CD]);
        //                }
        //                else
        //                {
        //                    llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, "");
        //                }


        //                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                base.Insert(TABLE.MODEL_CONFIG_MST_SPC, llstFieldData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    base.RollBack();
        //                    return dsResult;
        //                }

        //                //#03. MODEL_CONFIG_OPT_MST_SPC
        //                foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
        //                    llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
        //                    llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
        //                    llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
        //                    llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
        //                    llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_CONFIG_OPT_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        base.RollBack();
        //                        return dsResult;
        //                    }
        //                }


        //                //#04. MODEL_CONTEXT_MST_SPC
        //                foreach (DataRow drContext in dtContext.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);
        //                    llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
        //                    llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
        //                    llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_CONTEXT_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        base.RollBack();
        //                        return dsResult;
        //                    }
        //                }

        //                //#04_01. MODEL_FILTER_MST_SPC
        //                foreach (DataRow drFilter in dtFilter.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_FILTER_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.FILTER_KEY, drFilter[COLUMN.FILTER_KEY]);
        //                    llstFieldData.Add(COLUMN.FILTER_VALUE, drFilter[COLUMN.FILTER_VALUE]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_FILTER_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        base.RollBack();
        //                        return dsResult;
        //                    }
        //                }

        //                //#05. MODEL_RULE_MST_SPC
        //                foreach (DataRow drRule in dtRule.Rows)
        //                {
        //                    decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_MST_SPC);

        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(COLUMN.RAWID, ruleRawID);
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
        //                    llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
        //                    llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        base.RollBack();
        //                        return dsResult;
        //                    }

        //                    //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
        //                    string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
        //                    DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

        //                    if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

        //                    //#06. MODEL_RULE_OPT_MST_SPC
        //                    foreach (DataRow drRuleOpt in drRuleOpts)
        //                    {
        //                        llstFieldData.Clear();
        //                        llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC), "");
        //                        llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

        //                        llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);
        //                        llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

        //                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                        base.Insert(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

        //                        if (base.ErrorMessage.Length > 0)
        //                        {
        //                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                            base.RollBack();
        //                            return dsResult;
        //                        }
        //                    }
        //                }

        //                //#06. MODEL_AUTOCALC_MST_SPC
        //                foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_AUTOCALC_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
        //                    llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);
        //                    llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
        //                    llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
        //                    llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
        //                    llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
        //                    llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

        //                    llstFieldData.Add(COLUMN.STD_YN, drAutoCalc[COLUMN.STD_YN]);
        //                    llstFieldData.Add(COLUMN.RAW_CL_YN, drAutoCalc[COLUMN.RAW_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MEAN_CL_YN, drAutoCalc[COLUMN.MEAN_CL_YN]);
        //                    llstFieldData.Add(COLUMN.STD_CL_YN, drAutoCalc[COLUMN.STD_CL_YN]);
        //                    llstFieldData.Add(COLUMN.RANGE_CL_YN, drAutoCalc[COLUMN.RANGE_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_M_CL_YN, drAutoCalc[COLUMN.EWMA_M_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_R_CL_YN, drAutoCalc[COLUMN.EWMA_R_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_S_CL_YN, drAutoCalc[COLUMN.EWMA_S_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MA_CL_YN, drAutoCalc[COLUMN.MA_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MS_CL_YN, drAutoCalc[COLUMN.MS_CL_YN]);
        //                    llstFieldData.Add(COLUMN.ZONE_YN, drAutoCalc[COLUMN.ZONE_YN]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_AUTOCALC_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        base.RollBack();
        //                        return dsResult;
        //                    }
        //                }
        //            }

        //        }

        //        base.Commit();

        //        //새로만들었을 경우 MODEL의 Rawid를 넘겨주기위해 model table을 return해준다.
        //        dsResult.Tables.Add(dtModel.Copy());
        //    }
        //    catch (Exception ex)
        //    {
        //        base.RollBack();
        //        DSUtil.SetResult(dsResult, 0, "", ex.Message);

        //        BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
        //    }
        //    finally
        //    {
        //        //Resource 해제
        //        llstFieldData.Clear();
        //        llstFieldData = null;
        //    }

        //    return dsResult;
        //}

        public DataSet CreateSPCModel(ConfigMode configMode, string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, ref string ref_Config_RawID, string groupRawid, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();

            try
            {
                base.BeginTrans();

                //#01. MODEL_MST_SPC
                foreach (DataRow drModel in dtModel.Rows)
                {
                    decimal modelRawID = 0;

                    if (configMode.Equals(ConfigMode.CREATE_MAIN) || configMode.Equals(ConfigMode.SAVE_AS) || configMode.Equals(ConfigMode.CREATE_MAIN_FROM))
                    {
                        modelRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_MST_SPC);

                        //llstFieldData.Add(COLUMN., drModel[COLUMN.]);

                        llstFieldData.Add(COLUMN.RAWID, modelRawID);

                        llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
                        llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, drModel[COLUMN.LOCATION_RAWID]);
                        llstFieldData.Add(COLUMN.AREA_RAWID, drModel[COLUMN.AREA_RAWID]);
                        llstFieldData.Add(COLUMN.EQP_MODEL, drModel[COLUMN.EQP_MODEL]);

                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        if (groupRawid != null)
                        {
                            llstFieldData.Add(COLUMN.GROUP_RAWID, groupRawid);
                        }

                        base.Insert(TABLE.MODEL_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        drModel[COLUMN.RAWID] = modelRawID;

                        if (groupRawid != null)
                        {
                            drModel[COLUMN.GROUP_RAWID] = groupRawid;
                        }
                        else
                        {
                            drModel[COLUMN.GROUP_RAWID] = DBNull.Value;
                        }
                    }
                    else if (configMode.Equals(ConfigMode.CREATE_SUB))
                    {
                        modelRawID = decimal.Parse(drModel[COLUMN.RAWID].ToString());
                    }

                    //#02. MODEL_CONFIG_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        decimal configRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_CONFIG_MST_SPC);
                        ref_Config_RawID = configRawID.ToString();

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, configRawID);
                        llstFieldData.Add(COLUMN.MODEL_RAWID, modelRawID);

                        //SPC-676 by Louis
                        llstFieldData.Add(COLUMN.CHART_DESCRIPTON, drConfig[COLUMN.CHART_DESCRIPTON]);

                        llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
                        llstFieldData.Add(COLUMN.PARAM_TYPE_CD, drConfig[COLUMN.PARAM_TYPE_CD]);
                        llstFieldData.Add(COLUMN.COMPLEX_YN, drConfig[COLUMN.COMPLEX_YN]);

                        if (useComma)
                        {
                            string sValue = string.Empty;

                            if (drConfig[COLUMN.PARAM_LIST] != null)
                            {
                                sValue = drConfig[COLUMN.PARAM_LIST].ToString();
                                sValue = sValue.Replace(",", ";");
                            }
                            drConfig[COLUMN.REF_PARAM_LIST] = sValue;

                            if (drConfig[COLUMN.REF_PARAM_LIST] != null)
                            {
                                sValue = drConfig[COLUMN.REF_PARAM_LIST].ToString();
                                sValue = sValue.Replace(",", ";");
                            }
                            drConfig[COLUMN.REF_PARAM_LIST] = sValue;

                        }

                        llstFieldData.Add(COLUMN.PARAM_LIST, drConfig[COLUMN.PARAM_LIST]);
                        llstFieldData.Add(COLUMN.REF_PARAM, drConfig[COLUMN.REF_PARAM]);
                        llstFieldData.Add(COLUMN.REF_PARAM_LIST, drConfig[COLUMN.REF_PARAM_LIST]);
                        llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
                        llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
                        llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
                        llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.SUB_AUTOCALC_YN, drConfig[COLUMN.SUB_AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.USE_EXTERNAL_SPEC_YN, drConfig[COLUMN.USE_EXTERNAL_SPEC_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.UPPER_SPEC, drConfig[COLUMN.UPPER_SPEC]);
                        llstFieldData.Add(COLUMN.LOWER_SPEC, drConfig[COLUMN.LOWER_SPEC]);
                        llstFieldData.Add(COLUMN.TARGET, drConfig[COLUMN.TARGET]);
                        llstFieldData.Add(COLUMN.UPPER_CONTROL, drConfig[COLUMN.UPPER_CONTROL]);
                        llstFieldData.Add(COLUMN.LOWER_CONTROL, drConfig[COLUMN.LOWER_CONTROL]);
                        llstFieldData.Add(COLUMN.CENTER_LINE, drConfig[COLUMN.CENTER_LINE]);

                        llstFieldData.Add(COLUMN.RAW_LCL, drConfig[COLUMN.RAW_LCL]);
                        llstFieldData.Add(COLUMN.RAW_UCL, drConfig[COLUMN.RAW_UCL]);
                        llstFieldData.Add(COLUMN.RAW_CENTER_LINE, drConfig[COLUMN.RAW_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.STD_LCL, drConfig[COLUMN.STD_LCL]);
                        llstFieldData.Add(COLUMN.STD_UCL, drConfig[COLUMN.STD_UCL]);
                        llstFieldData.Add(COLUMN.STD_CENTER_LINE, drConfig[COLUMN.STD_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.RANGE_LCL, drConfig[COLUMN.RANGE_LCL]);
                        llstFieldData.Add(COLUMN.RANGE_UCL, drConfig[COLUMN.RANGE_UCL]);
                        llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, drConfig[COLUMN.RANGE_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_M_LCL, drConfig[COLUMN.EWMA_M_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_UCL, drConfig[COLUMN.EWMA_M_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, drConfig[COLUMN.EWMA_M_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_R_LCL, drConfig[COLUMN.EWMA_R_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_UCL, drConfig[COLUMN.EWMA_R_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, drConfig[COLUMN.EWMA_R_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_S_LCL, drConfig[COLUMN.EWMA_S_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_UCL, drConfig[COLUMN.EWMA_S_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, drConfig[COLUMN.EWMA_S_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MA_LCL, drConfig[COLUMN.MA_LCL]);
                        llstFieldData.Add(COLUMN.MA_UCL, drConfig[COLUMN.MA_UCL]);
                        llstFieldData.Add(COLUMN.MA_CENTER_LINE, drConfig[COLUMN.MA_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MS_LCL, drConfig[COLUMN.MS_LCL]);
                        llstFieldData.Add(COLUMN.MS_UCL, drConfig[COLUMN.MS_UCL]);
                        llstFieldData.Add(COLUMN.MS_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MR_LCL, drConfig[COLUMN.MR_LCL]);
                        llstFieldData.Add(COLUMN.MR_UCL, drConfig[COLUMN.MR_UCL]);
                        llstFieldData.Add(COLUMN.MR_CENTER_LINE, drConfig[COLUMN.MR_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.LOWER_TECHNICAL_LIMIT, drConfig[COLUMN.LOWER_TECHNICAL_LIMIT]);
                        llstFieldData.Add(COLUMN.UPPER_TECHNICAL_LIMIT, drConfig[COLUMN.UPPER_TECHNICAL_LIMIT]);

                        llstFieldData.Add(COLUMN.ZONE_A_LCL, drConfig[COLUMN.ZONE_A_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_A_UCL, drConfig[COLUMN.ZONE_A_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_B_LCL, drConfig[COLUMN.ZONE_B_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_B_UCL, drConfig[COLUMN.ZONE_B_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_C_LCL, drConfig[COLUMN.ZONE_C_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_C_UCL, drConfig[COLUMN.ZONE_C_UCL]);

                        //Chris : 2010-06-18

                        llstFieldData.Add(COLUMN.SPEC_USL_OFFSET, drConfig[COLUMN.SPEC_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.SPEC_LSL_OFFSET, drConfig[COLUMN.SPEC_LSL_OFFSET]);
                        //llstFieldData.Add(COLUMN.SPEC_OFFSET_YN, drConfig[COLUMN.SPEC_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RAW_UCL_OFFSET, drConfig[COLUMN.RAW_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RAW_LCL_OFFSET, drConfig[COLUMN.RAW_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RAW_OFFSET_YN, drConfig[COLUMN.RAW_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.STD_UCL_OFFSET, drConfig[COLUMN.STD_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.STD_LCL_OFFSET, drConfig[COLUMN.STD_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.STD_OFFSET_YN, drConfig[COLUMN.STD_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MEAN_UCL_OFFSET, drConfig[COLUMN.MEAN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MEAN_LCL_OFFSET, drConfig[COLUMN.MEAN_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MEAN_OFFSET_YN, drConfig[COLUMN.MEAN_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RANGE_UCL_OFFSET, drConfig[COLUMN.RANGE_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RANGE_LCL_OFFSET, drConfig[COLUMN.RANGE_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RANGE_OFFSET_YN, drConfig[COLUMN.RANGE_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_M_UCL_OFFSET, drConfig[COLUMN.EWMA_M_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_M_LCL_OFFSET, drConfig[COLUMN.EWMA_M_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_M_OFFSET_YN, drConfig[COLUMN.EWMA_M_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_S_UCL_OFFSET, drConfig[COLUMN.EWMA_S_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_S_LCL_OFFSET, drConfig[COLUMN.EWMA_S_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_S_OFFSET_YN, drConfig[COLUMN.EWMA_S_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_R_UCL_OFFSET, drConfig[COLUMN.EWMA_R_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_R_LCL_OFFSET, drConfig[COLUMN.EWMA_R_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_R_OFFSET_YN, drConfig[COLUMN.EWMA_R_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MA_UCL_OFFSET, drConfig[COLUMN.MA_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MA_LCL_OFFSET, drConfig[COLUMN.MA_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MA_OFFSET_YN, drConfig[COLUMN.MA_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MS_UCL_OFFSET, drConfig[COLUMN.MS_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MS_LCL_OFFSET, drConfig[COLUMN.MS_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MS_OFFSET_YN, drConfig[COLUMN.MS_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MR_UCL_OFFSET, drConfig[COLUMN.MR_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MR_LCL_OFFSET, drConfig[COLUMN.MR_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.ZONE_A_UCL_OFFSET, drConfig[COLUMN.ZONE_A_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_A_LCL_OFFSET, drConfig[COLUMN.ZONE_A_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_A_OFFSET_YN, drConfig[COLUMN.ZONE_A_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_B_UCL_OFFSET, drConfig[COLUMN.ZONE_B_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_B_LCL_OFFSET, drConfig[COLUMN.ZONE_B_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_B_OFFSET_YN, drConfig[COLUMN.ZONE_B_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_C_UCL_OFFSET, drConfig[COLUMN.ZONE_C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_C_LCL_OFFSET, drConfig[COLUMN.ZONE_C_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_C_OFFSET_YN, drConfig[COLUMN.ZONE_C_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.UPPER_FILTER, drConfig[COLUMN.UPPER_FILTER]);
                        llstFieldData.Add(COLUMN.LOWER_FILTER, drConfig[COLUMN.LOWER_FILTER]);

                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);

                        //---------------------------------------------------------------------------------


                        llstFieldData.Add(COLUMN.SAMPLE_COUNT, drConfig[COLUMN.SAMPLE_COUNT]);
                        llstFieldData.Add(COLUMN.EWMA_LAMBDA, drConfig[COLUMN.EWMA_LAMBDA]);
                        llstFieldData.Add(COLUMN.MOVING_COUNT, drConfig[COLUMN.MOVING_COUNT]);

                        llstFieldData.Add(COLUMN.STD, drConfig[COLUMN.STD]);
                        llstFieldData.Add(COLUMN.DESCRIPTION, drConfig[COLUMN.DESCRIPTION]);

                        llstFieldData.Add(COLUMN.USE_NORM_YN, drConfig[COLUMN.USE_NORM_YN]);

                        if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
                        {
                            llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, drConfig[COLUMN.MANAGE_TYPE_CD]);
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, "");
                        }

                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);

                        llstFieldData.Add(COLUMN.VALIDATION_SAME_MODULE_YN, drConfig[COLUMN.VALIDATION_SAME_MODULE_YN]);

                        if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
                        {
                            llstFieldData.Add(COLUMN.SPC_DATA_LEVEL, "");
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.SPC_DATA_LEVEL, drConfig[COLUMN.SPC_DATA_LEVEL]);
                        }

                        llstFieldData.Add(COLUMN.SAVE_COMMENT, "");
                        llstFieldData.Add(COLUMN.CHANGED_ITEMS, "");
                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        base.Insert(TABLE.MODEL_CONFIG_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //#03. MODEL_CONFIG_OPT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

                            //SPC-721 By Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONFIG_OPT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }


                        //#04. MODEL_CONTEXT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            if (useComma)   //spc1281 use commaa == ture 인경우 dataset에 저장하기 전 ,를 ;로 변경.
                            {
                                string contextValues = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = contextValues.Replace(",", ";");

                                string excludeList = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = excludeList.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONTEXT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#05. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string ruleOPTValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = ruleOPTValue.Replace(",", ";");
                                }
                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#06. MODEL_AUTOCALC_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_AUTOCALC_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

                            llstFieldData.Add(COLUMN.STD_YN, drAutoCalc[COLUMN.STD_YN]);
                            llstFieldData.Add(COLUMN.RAW_CL_YN, drAutoCalc[COLUMN.RAW_CL_YN]);
                            llstFieldData.Add(COLUMN.MEAN_CL_YN, drAutoCalc[COLUMN.MEAN_CL_YN]);
                            llstFieldData.Add(COLUMN.STD_CL_YN, drAutoCalc[COLUMN.STD_CL_YN]);
                            llstFieldData.Add(COLUMN.RANGE_CL_YN, drAutoCalc[COLUMN.RANGE_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_M_CL_YN, drAutoCalc[COLUMN.EWMA_M_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_R_CL_YN, drAutoCalc[COLUMN.EWMA_R_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_S_CL_YN, drAutoCalc[COLUMN.EWMA_S_CL_YN]);
                            llstFieldData.Add(COLUMN.MA_CL_YN, drAutoCalc[COLUMN.MA_CL_YN]);
                            llstFieldData.Add(COLUMN.MS_CL_YN, drAutoCalc[COLUMN.MS_CL_YN]);
                            llstFieldData.Add(COLUMN.MR_CL_YN, drAutoCalc[COLUMN.MR_CL_YN]);
                            llstFieldData.Add(COLUMN.ZONE_YN, drAutoCalc[COLUMN.ZONE_YN]);
                            llstFieldData.Add(COLUMN.SHIFT_CALC_YN, drAutoCalc[COLUMN.SHIFT_CALC_YN]);
                            llstFieldData.Add(COLUMN.WITHOUT_IQR_YN, drAutoCalc[COLUMN.WITHOUT_IQR_YN]);

                            //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);

                            //SPC-658 Initial Calc Count
                            llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            //SPC-1155 ~~_CALC_VALUE, ~~_CALC_OPTION, ~~_CALC_SIDED Column추가로 인한 수정
                            llstFieldData.Add(COLUMN.RAW_CALC_VALUE, drAutoCalc[COLUMN.RAW_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RAW_CALC_OPTION, drAutoCalc[COLUMN.RAW_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RAW_CALC_SIDED, drAutoCalc[COLUMN.RAW_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_VALUE, drAutoCalc[COLUMN.RANGE_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_OPTION, drAutoCalc[COLUMN.RANGE_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_SIDED, drAutoCalc[COLUMN.RANGE_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.STD_CALC_VALUE, drAutoCalc[COLUMN.STD_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.STD_CALC_OPTION, drAutoCalc[COLUMN.STD_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.STD_CALC_SIDED, drAutoCalc[COLUMN.STD_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_VALUE, drAutoCalc[COLUMN.EWMA_M_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_OPTION, drAutoCalc[COLUMN.EWMA_M_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_SIDED, drAutoCalc[COLUMN.EWMA_M_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_VALUE, drAutoCalc[COLUMN.EWMA_R_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_OPTION, drAutoCalc[COLUMN.EWMA_R_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_SIDED, drAutoCalc[COLUMN.EWMA_R_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_VALUE, drAutoCalc[COLUMN.EWMA_S_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_OPTION, drAutoCalc[COLUMN.EWMA_S_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_SIDED, drAutoCalc[COLUMN.EWMA_S_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_VALUE, drAutoCalc[COLUMN.MEAN_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_OPTION, drAutoCalc[COLUMN.MEAN_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_SIDED, drAutoCalc[COLUMN.MEAN_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MR_CALC_VALUE, drAutoCalc[COLUMN.MR_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MR_CALC_OPTION, drAutoCalc[COLUMN.MR_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MR_CALC_SIDED, drAutoCalc[COLUMN.MR_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MS_CALC_VALUE, drAutoCalc[COLUMN.MS_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MS_CALC_OPTION, drAutoCalc[COLUMN.MS_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MS_CALC_SIDED, drAutoCalc[COLUMN.MS_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MA_CALC_VALUE, drAutoCalc[COLUMN.MA_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MA_CALC_OPTION, drAutoCalc[COLUMN.MA_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MA_CALC_SIDED, drAutoCalc[COLUMN.MA_CALC_SIDED]);

                            //SPC-1129 BY STELLA
                            llstFieldData.Add(COLUMN.USE_GLOBAL_YN, drAutoCalc[COLUMN.USE_GLOBAL_YN]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT_OPTION, drAutoCalc[COLUMN.CONTROL_LIMIT_OPTION]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_AUTOCALC_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }
                }

                base.Commit();

                //새로만들었을 경우 MODEL의 Rawid를 넘겨주기위해 model table을 return해준다.
                dsResult.Tables.Add(dtModel.Copy());
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear();
                llstFieldData = null;
            }

            return dsResult;
        }



        //public DataSet ModifySPCModel(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, DataTable dtFilter, List<string> lstChangedMasterColList)
        //{
        //    DataSet dsResult = new DataSet();

        //    LinkedList llstFieldData = new LinkedList();
        //    LinkedList llstWhereData = new LinkedList();
        //    string sWhereQuery = string.Empty;

        //    try
        //    {
        //        //base.BeginTrans();


        //        //#01. MODEL_MST_SPC
        //        foreach (DataRow drModel in dtModel.Rows)
        //        {
        //            llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
        //            llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
        //            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
        //            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

        //            sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
        //            llstWhereData.Add(COLUMN.RAWID, drModel[COLUMN.RAWID]);

        //            base.Update(TABLE.MODEL_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

        //            if (base.ErrorMessage.Length > 0)
        //            {
        //                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                //base.RollBack();
        //                return dsResult;
        //            }

        //            //#02. MODEL_CONFIG_MST_SPC
        //            foreach (DataRow drConfig in dtConfig.Rows)
        //            {
        //                llstFieldData.Clear();
        //                llstWhereData.Clear();

        //                string configRawID = drConfig[COLUMN.RAWID].ToString();
        //                //llstFieldData.Add(COLUMN.RAWID, configRawID);
        //                //llstFieldData.Add(COLUMN.MODEL_RAWID, modelRawID);

        //                llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
        //                llstFieldData.Add(COLUMN.PARAM_TYPE_CD, drConfig[COLUMN.PARAM_TYPE_CD]);
        //                llstFieldData.Add(COLUMN.COMPLEX_YN, drConfig[COLUMN.COMPLEX_YN]);
        //                llstFieldData.Add(COLUMN.PARAM_LIST, drConfig[COLUMN.PARAM_LIST]);
        //                llstFieldData.Add(COLUMN.REF_PARAM, drConfig[COLUMN.REF_PARAM]);
        //                llstFieldData.Add(COLUMN.REF_PARAM_LIST, drConfig[COLUMN.REF_PARAM_LIST]);
        //                llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
        //                llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
        //                llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
        //                llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
        //                llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
        //                llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
        //                llstFieldData.Add(COLUMN.USE_EXTERNAL_SPEC_YN, drConfig[COLUMN.USE_EXTERNAL_SPEC_YN]);
        //                llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
        //                llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
        //                llstFieldData.Add(COLUMN.UPPER_SPEC, drConfig[COLUMN.UPPER_SPEC]);
        //                llstFieldData.Add(COLUMN.LOWER_SPEC, drConfig[COLUMN.LOWER_SPEC]);
        //                llstFieldData.Add(COLUMN.TARGET, drConfig[COLUMN.TARGET]);
        //                llstFieldData.Add(COLUMN.UPPER_CONTROL, drConfig[COLUMN.UPPER_CONTROL]);
        //                llstFieldData.Add(COLUMN.LOWER_CONTROL, drConfig[COLUMN.LOWER_CONTROL]);
        //                llstFieldData.Add(COLUMN.SAMPLE_COUNT, drConfig[COLUMN.SAMPLE_COUNT]);
        //                llstFieldData.Add(COLUMN.EWMA_LAMBDA, drConfig[COLUMN.EWMA_LAMBDA]);
        //                llstFieldData.Add(COLUMN.MOVING_COUNT, drConfig[COLUMN.MOVING_COUNT]);
        //                llstFieldData.Add(COLUMN.CENTER_LINE, drConfig[COLUMN.CENTER_LINE]);
        //                llstFieldData.Add(COLUMN.STD, drConfig[COLUMN.STD]);
        //                llstFieldData.Add(COLUMN.DESCRIPTION, drConfig[COLUMN.DESCRIPTION]);

        //                llstFieldData.Add(COLUMN.RAW_LCL, drConfig[COLUMN.RAW_LCL]);
        //                llstFieldData.Add(COLUMN.RAW_UCL, drConfig[COLUMN.RAW_UCL]);
        //                llstFieldData.Add(COLUMN.RAW_CENTER_LINE, drConfig[COLUMN.RAW_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.STD_LCL, drConfig[COLUMN.STD_LCL]);
        //                llstFieldData.Add(COLUMN.STD_UCL, drConfig[COLUMN.STD_UCL]);
        //                llstFieldData.Add(COLUMN.STD_CENTER_LINE, drConfig[COLUMN.STD_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.RANGE_LCL, drConfig[COLUMN.RANGE_LCL]);
        //                llstFieldData.Add(COLUMN.RANGE_UCL, drConfig[COLUMN.RANGE_UCL]);
        //                llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, drConfig[COLUMN.RANGE_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_M_LCL, drConfig[COLUMN.EWMA_M_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_M_UCL, drConfig[COLUMN.EWMA_M_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, drConfig[COLUMN.EWMA_M_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_R_LCL, drConfig[COLUMN.EWMA_R_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_R_UCL, drConfig[COLUMN.EWMA_R_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, drConfig[COLUMN.EWMA_R_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.EWMA_S_LCL, drConfig[COLUMN.EWMA_S_LCL]);
        //                llstFieldData.Add(COLUMN.EWMA_S_UCL, drConfig[COLUMN.EWMA_S_UCL]);
        //                llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, drConfig[COLUMN.EWMA_S_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.MA_LCL, drConfig[COLUMN.MA_LCL]);
        //                llstFieldData.Add(COLUMN.MA_UCL, drConfig[COLUMN.MA_UCL]);
        //                llstFieldData.Add(COLUMN.MA_CENTER_LINE, drConfig[COLUMN.MA_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.MS_LCL, drConfig[COLUMN.MS_LCL]);
        //                llstFieldData.Add(COLUMN.MS_UCL, drConfig[COLUMN.MS_UCL]);
        //                llstFieldData.Add(COLUMN.MS_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

        //                llstFieldData.Add(COLUMN.ZONE_A_LCL, drConfig[COLUMN.ZONE_A_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_A_UCL, drConfig[COLUMN.ZONE_A_UCL]);

        //                llstFieldData.Add(COLUMN.ZONE_B_LCL, drConfig[COLUMN.ZONE_B_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_B_UCL, drConfig[COLUMN.ZONE_B_UCL]);

        //                llstFieldData.Add(COLUMN.ZONE_C_LCL, drConfig[COLUMN.ZONE_C_LCL]);
        //                llstFieldData.Add(COLUMN.ZONE_C_UCL, drConfig[COLUMN.ZONE_C_UCL]);


        //                if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
        //                {
        //                    llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, drConfig[COLUMN.MANAGE_TYPE_CD]);
        //                }else{
        //                    llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, "");
        //                }


        //                llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
        //                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

        //                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
        //                llstWhereData.Add(COLUMN.RAWID, drConfig[COLUMN.RAWID]);

        //                base.Update(TABLE.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    //base.RollBack();
        //                    return dsResult;
        //                }

        //                //#02-1. MODEL_CONFIG_HST_SPC
        //                //       MODEL_CONFIG_MST에서 MASTER VALUE가 변경되었을 경우 변경된 내용만 MODEL_CONFIG_HST_SPC에 INSERT한다.
        //                if (lstChangedMasterColList != null && lstChangedMasterColList.Count > 0)
        //                {
        //                    llstFieldData.Clear();

        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_HST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    foreach (string colList in lstChangedMasterColList)
        //                    {
        //                        llstFieldData.Add(colList, drConfig[colList]);
        //                    }

        //                    llstFieldData.Add(COLUMN.MODIFIED_TYPE_CD, "MAN");
        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_CONFIG_HST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }
        //                }


        //                //#03. MODEL_CONFIG_OPT_MST_SPC
        //                foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstWhereData.Clear();
        //                    //llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
        //                    //llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
        //                    llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
        //                    llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
        //                    llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
        //                    llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
        //                    llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

        //                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

        //                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
        //                    llstWhereData.Add(COLUMN.RAWID, drConfigOpt[COLUMN.RAWID]);

        //                    base.Update(TABLE.MODEL_CONFIG_OPT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }
        //                }


        //                //#04. MODEL_CONTEXT_MST_SPC DATA 삭제
        //                llstWhereData.Clear();

        //                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
        //                llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    //base.RollBack();
        //                    return dsResult;
        //                }

        //                //#05. MODEL_CONTEXT_MST_SPC
        //                foreach (DataRow drContext in dtContext.Rows)
        //                {
        //                    llstFieldData.Clear();

        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);
        //                    llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);
        //                    llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
        //                    llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
        //                    llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_CONTEXT_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }
        //                }

        //                //#04_01. MODEL_FILTER_MST_SPC DATA 삭제
        //                llstWhereData.Clear();

        //                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
        //                llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                base.Delete(TABLE.MODEL_FILTER_MST_SPC, sWhereQuery, llstWhereData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    //base.RollBack();
        //                    return dsResult;
        //                }

        //                //#05_01. MODEL_FILTER_MST_SPC
        //                foreach (DataRow drFilter in dtFilter.Rows)
        //                {
        //                    llstFieldData.Clear();

        //                    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_FILTER_MST_SPC), "");
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);
        //                    llstFieldData.Add(COLUMN.FILTER_KEY, drFilter[COLUMN.FILTER_KEY]);
        //                    llstFieldData.Add(COLUMN.FILTER_VALUE, drFilter[COLUMN.FILTER_VALUE]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_FILTER_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }
        //                }

        //                //#06. MODEL_RULE_MST_SPC, MODEL_RULE_OPT_MST_SPC DATA 삭제
        //                llstWhereData.Clear();
        //                sWhereQuery = string.Format("WHERE {0} IN (SELECT RAWID FROM {1} WHERE {2} = :{2}) ", COLUMN.MODEL_RULE_RAWID,
        //                                                                                                      TABLE.MODEL_RULE_MST_SPC,
        //                                                                                                      COLUMN.MODEL_CONFIG_RAWID);
        //                llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    //base.RollBack();
        //                    return dsResult;
        //                }

        //                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);

        //                base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

        //                if (base.ErrorMessage.Length > 0)
        //                {
        //                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                    //base.RollBack();
        //                    return dsResult;
        //                }

        //                //#07. MODEL_RULE_MST_SPC
        //                foreach (DataRow drRule in dtRule.Rows)
        //                {
        //                    decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_MST_SPC);

        //                    llstFieldData.Clear();
        //                    llstFieldData.Add(COLUMN.RAWID, ruleRawID);
        //                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
        //                    llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
        //                    llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);

        //                    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                    base.Insert(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }

        //                    //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
        //                    string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
        //                    DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

        //                    if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

        //                    //#06. MODEL_RULE_OPT_MST_SPC
        //                    foreach (DataRow drRuleOpt in drRuleOpts)
        //                    {
        //                        llstFieldData.Clear();
        //                        llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC), "");
        //                        llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

        //                        llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);
        //                        llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

        //                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
        //                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

        //                        base.Insert(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

        //                        if (base.ErrorMessage.Length > 0)
        //                        {
        //                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                            //base.RollBack();
        //                            return dsResult;
        //                        }
        //                    }
        //                }

        //                //#08. MODEL_AUTOCALC_MST_SPC
        //                foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
        //                {
        //                    llstFieldData.Clear();
        //                    llstWhereData.Clear();
        //                    //llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
        //                    //llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

        //                    llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
        //                    llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);
        //                    llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
        //                    llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
        //                    llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
        //                    llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
        //                    llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

        //                    llstFieldData.Add(COLUMN.STD_YN, drAutoCalc[COLUMN.STD_YN]);
        //                    llstFieldData.Add(COLUMN.RAW_CL_YN, drAutoCalc[COLUMN.RAW_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MEAN_CL_YN, drAutoCalc[COLUMN.MEAN_CL_YN]);
        //                    llstFieldData.Add(COLUMN.STD_CL_YN, drAutoCalc[COLUMN.STD_CL_YN]);
        //                    llstFieldData.Add(COLUMN.RANGE_CL_YN, drAutoCalc[COLUMN.RANGE_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_M_CL_YN, drAutoCalc[COLUMN.EWMA_M_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_R_CL_YN, drAutoCalc[COLUMN.EWMA_R_CL_YN]);
        //                    llstFieldData.Add(COLUMN.EWMA_S_CL_YN, drAutoCalc[COLUMN.EWMA_S_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MA_CL_YN, drAutoCalc[COLUMN.MA_CL_YN]);
        //                    llstFieldData.Add(COLUMN.MS_CL_YN, drAutoCalc[COLUMN.MS_CL_YN]);
        //                    llstFieldData.Add(COLUMN.ZONE_YN, drAutoCalc[COLUMN.ZONE_YN]);

        //                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
        //                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

        //                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
        //                    llstWhereData.Add(COLUMN.RAWID, drAutoCalc[COLUMN.RAWID]);

        //                    base.Update(TABLE.MODEL_AUTOCALC_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

        //                    if (base.ErrorMessage.Length > 0)
        //                    {
        //                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
        //                        //base.RollBack();
        //                        return dsResult;
        //                    }
        //                }
        //            }

        //        }

        //        //base.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        //base.RollBack();
        //        DSUtil.SetResult(dsResult, 0, "", ex.Message);
        //        BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
        //    }
        //    finally
        //    {
        //        //Resource 해제
        //        llstFieldData.Clear(); llstFieldData = null;
        //        llstWhereData.Clear(); llstWhereData = null;
        //        sWhereQuery = null;
        //    }

        //    return dsResult;
        //}

        public DataSet ModifySPCModel(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, List<string> lstChangedMasterColList, string comment, string changedItems, string groupRawid, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                //base.BeginTrans();


                //#01. MODEL_MST_SPC
                foreach (DataRow drModel in dtModel.Rows)
                {
                    llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
                    llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    if (groupRawid != null)
                    {
                        llstFieldData.Add(COLUMN.GROUP_RAWID, groupRawid);
                    }
                    else
                    {
                        llstFieldData.Add(COLUMN.GROUP_RAWID, "");
                    }

                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                    llstWhereData.Add(COLUMN.RAWID, drModel[COLUMN.RAWID]);

                    base.Update(TABLE.MODEL_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        //base.RollBack();
                        return dsResult;
                    }

                    //#02. MODEL_CONFIG_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        llstFieldData.Clear();
                        llstWhereData.Clear();

                        string configRawID = drConfig[COLUMN.RAWID].ToString();
                        //llstFieldData.Add(COLUMN.RAWID, configRawID);
                        //llstFieldData.Add(COLUMN.MODEL_RAWID, modelRawID);

                        //SPC-676 by Louis
                        llstFieldData.Add(COLUMN.CHART_DESCRIPTON, drConfig[COLUMN.CHART_DESCRIPTON].ToString());

                        llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
                        llstFieldData.Add(COLUMN.PARAM_TYPE_CD, drConfig[COLUMN.PARAM_TYPE_CD]);
                        llstFieldData.Add(COLUMN.COMPLEX_YN, drConfig[COLUMN.COMPLEX_YN]);

                        if (useComma)
                        {
                            string sValue = string.Empty;

                            if (drConfig[COLUMN.PARAM_LIST] != null)
                            {
                                sValue = drConfig[COLUMN.PARAM_LIST].ToString();
                                sValue = sValue.Replace(",", ";");
                            }
                            drConfig[COLUMN.REF_PARAM_LIST] = sValue;

                            if (drConfig[COLUMN.REF_PARAM_LIST] != null)
                            {
                                sValue = drConfig[COLUMN.REF_PARAM_LIST].ToString();
                                sValue = sValue.Replace(",", ";");
                            }
                            drConfig[COLUMN.REF_PARAM_LIST] = sValue;

                        }

                        llstFieldData.Add(COLUMN.PARAM_LIST, drConfig[COLUMN.PARAM_LIST]);
                        llstFieldData.Add(COLUMN.REF_PARAM, drConfig[COLUMN.REF_PARAM]);
                        llstFieldData.Add(COLUMN.REF_PARAM_LIST, drConfig[COLUMN.REF_PARAM_LIST]);
                        llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
                        llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
                        llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
                        llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.SUB_AUTOCALC_YN, drConfig[COLUMN.SUB_AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.USE_EXTERNAL_SPEC_YN, drConfig[COLUMN.USE_EXTERNAL_SPEC_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.UPPER_SPEC, drConfig[COLUMN.UPPER_SPEC]);
                        llstFieldData.Add(COLUMN.LOWER_SPEC, drConfig[COLUMN.LOWER_SPEC]);
                        llstFieldData.Add(COLUMN.TARGET, drConfig[COLUMN.TARGET]);
                        llstFieldData.Add(COLUMN.UPPER_CONTROL, drConfig[COLUMN.UPPER_CONTROL]);
                        llstFieldData.Add(COLUMN.LOWER_CONTROL, drConfig[COLUMN.LOWER_CONTROL]);
                        llstFieldData.Add(COLUMN.SAMPLE_COUNT, drConfig[COLUMN.SAMPLE_COUNT]);
                        llstFieldData.Add(COLUMN.EWMA_LAMBDA, drConfig[COLUMN.EWMA_LAMBDA]);
                        llstFieldData.Add(COLUMN.MOVING_COUNT, drConfig[COLUMN.MOVING_COUNT]);
                        llstFieldData.Add(COLUMN.CENTER_LINE, drConfig[COLUMN.CENTER_LINE]);
                        llstFieldData.Add(COLUMN.STD, drConfig[COLUMN.STD]);
                        llstFieldData.Add(COLUMN.DESCRIPTION, drConfig[COLUMN.DESCRIPTION]);

                        llstFieldData.Add(COLUMN.RAW_LCL, drConfig[COLUMN.RAW_LCL]);
                        llstFieldData.Add(COLUMN.RAW_UCL, drConfig[COLUMN.RAW_UCL]);
                        llstFieldData.Add(COLUMN.RAW_CENTER_LINE, drConfig[COLUMN.RAW_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.STD_LCL, drConfig[COLUMN.STD_LCL]);
                        llstFieldData.Add(COLUMN.STD_UCL, drConfig[COLUMN.STD_UCL]);
                        llstFieldData.Add(COLUMN.STD_CENTER_LINE, drConfig[COLUMN.STD_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.RANGE_LCL, drConfig[COLUMN.RANGE_LCL]);
                        llstFieldData.Add(COLUMN.RANGE_UCL, drConfig[COLUMN.RANGE_UCL]);
                        llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, drConfig[COLUMN.RANGE_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_M_LCL, drConfig[COLUMN.EWMA_M_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_UCL, drConfig[COLUMN.EWMA_M_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, drConfig[COLUMN.EWMA_M_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_R_LCL, drConfig[COLUMN.EWMA_R_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_UCL, drConfig[COLUMN.EWMA_R_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, drConfig[COLUMN.EWMA_R_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_S_LCL, drConfig[COLUMN.EWMA_S_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_UCL, drConfig[COLUMN.EWMA_S_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, drConfig[COLUMN.EWMA_S_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MA_LCL, drConfig[COLUMN.MA_LCL]);
                        llstFieldData.Add(COLUMN.MA_UCL, drConfig[COLUMN.MA_UCL]);
                        llstFieldData.Add(COLUMN.MA_CENTER_LINE, drConfig[COLUMN.MA_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MS_LCL, drConfig[COLUMN.MS_LCL]);
                        llstFieldData.Add(COLUMN.MS_UCL, drConfig[COLUMN.MS_UCL]);
                        llstFieldData.Add(COLUMN.MS_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MR_LCL, drConfig[COLUMN.MR_LCL]);
                        llstFieldData.Add(COLUMN.MR_UCL, drConfig[COLUMN.MR_UCL]);
                        llstFieldData.Add(COLUMN.MR_CENTER_LINE, drConfig[COLUMN.MR_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.LOWER_TECHNICAL_LIMIT, drConfig[COLUMN.LOWER_TECHNICAL_LIMIT]);
                        llstFieldData.Add(COLUMN.UPPER_TECHNICAL_LIMIT, drConfig[COLUMN.UPPER_TECHNICAL_LIMIT]);

                        llstFieldData.Add(COLUMN.ZONE_A_LCL, drConfig[COLUMN.ZONE_A_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_A_UCL, drConfig[COLUMN.ZONE_A_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_B_LCL, drConfig[COLUMN.ZONE_B_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_B_UCL, drConfig[COLUMN.ZONE_B_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_C_LCL, drConfig[COLUMN.ZONE_C_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_C_UCL, drConfig[COLUMN.ZONE_C_UCL]);



                        //Chris : 2010-06-18

                        llstFieldData.Add(COLUMN.SPEC_USL_OFFSET, drConfig[COLUMN.SPEC_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.SPEC_LSL_OFFSET, drConfig[COLUMN.SPEC_LSL_OFFSET]);
                        //llstFieldData.Add(COLUMN.SPEC_OFFSET_YN, drConfig[COLUMN.SPEC_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RAW_UCL_OFFSET, drConfig[COLUMN.RAW_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RAW_LCL_OFFSET, drConfig[COLUMN.RAW_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RAW_OFFSET_YN, drConfig[COLUMN.RAW_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.STD_UCL_OFFSET, drConfig[COLUMN.STD_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.STD_LCL_OFFSET, drConfig[COLUMN.STD_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.STD_OFFSET_YN, drConfig[COLUMN.STD_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MEAN_UCL_OFFSET, drConfig[COLUMN.MEAN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MEAN_LCL_OFFSET, drConfig[COLUMN.MEAN_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MEAN_OFFSET_YN, drConfig[COLUMN.MEAN_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RANGE_UCL_OFFSET, drConfig[COLUMN.RANGE_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RANGE_LCL_OFFSET, drConfig[COLUMN.RANGE_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RANGE_OFFSET_YN, drConfig[COLUMN.RANGE_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_M_UCL_OFFSET, drConfig[COLUMN.EWMA_M_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_M_LCL_OFFSET, drConfig[COLUMN.EWMA_M_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_M_OFFSET_YN, drConfig[COLUMN.EWMA_M_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_S_UCL_OFFSET, drConfig[COLUMN.EWMA_S_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_S_LCL_OFFSET, drConfig[COLUMN.EWMA_S_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_S_OFFSET_YN, drConfig[COLUMN.EWMA_S_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_R_UCL_OFFSET, drConfig[COLUMN.EWMA_R_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_R_LCL_OFFSET, drConfig[COLUMN.EWMA_R_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_R_OFFSET_YN, drConfig[COLUMN.EWMA_R_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MA_UCL_OFFSET, drConfig[COLUMN.MA_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MA_LCL_OFFSET, drConfig[COLUMN.MA_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MA_OFFSET_YN, drConfig[COLUMN.MA_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MS_UCL_OFFSET, drConfig[COLUMN.MS_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MS_LCL_OFFSET, drConfig[COLUMN.MS_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MS_OFFSET_YN, drConfig[COLUMN.MS_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MR_UCL_OFFSET, drConfig[COLUMN.MR_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MR_LCL_OFFSET, drConfig[COLUMN.MR_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.ZONE_A_UCL_OFFSET, drConfig[COLUMN.ZONE_A_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_A_LCL_OFFSET, drConfig[COLUMN.ZONE_A_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_A_OFFSET_YN, drConfig[COLUMN.ZONE_A_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_B_UCL_OFFSET, drConfig[COLUMN.ZONE_B_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_B_LCL_OFFSET, drConfig[COLUMN.ZONE_B_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_B_OFFSET_YN, drConfig[COLUMN.ZONE_B_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_C_UCL_OFFSET, drConfig[COLUMN.ZONE_C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_C_LCL_OFFSET, drConfig[COLUMN.ZONE_C_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_C_OFFSET_YN, drConfig[COLUMN.ZONE_C_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.UPPER_FILTER, drConfig[COLUMN.UPPER_FILTER]);
                        llstFieldData.Add(COLUMN.LOWER_FILTER, drConfig[COLUMN.LOWER_FILTER]);

                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);

                        llstFieldData.Add(COLUMN.USE_NORM_YN, drConfig[COLUMN.USE_NORM_YN]);

                        if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
                        {
                            llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, drConfig[COLUMN.MANAGE_TYPE_CD]);
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.MANAGE_TYPE_CD, "");
                        }

                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);

                        llstFieldData.Add(COLUMN.VALIDATION_SAME_MODULE_YN, drConfig[COLUMN.VALIDATION_SAME_MODULE_YN]);

                        if (drConfig[COLUMN.PARAM_TYPE_CD].Equals("MET"))
                        {
                            llstFieldData.Add(COLUMN.SPC_DATA_LEVEL, "");
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.SPC_DATA_LEVEL, drConfig[COLUMN.SPC_DATA_LEVEL]);
                        }

                        llstFieldData.Add(COLUMN.SAVE_COMMENT, comment);
                        llstFieldData.Add(COLUMN.CHANGED_ITEMS, changedItems);
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                        llstWhereData.Add(COLUMN.RAWID, drConfig[COLUMN.RAWID]);

                        base.Update(TABLE.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#02-1. MODEL_CONFIG_HST_SPC
                        //       MODEL_CONFIG_MST에서 MASTER VALUE가 변경되었을 경우 변경된 내용만 MODEL_CONFIG_HST_SPC에 INSERT한다.
                        if (lstChangedMasterColList != null && lstChangedMasterColList.Count > 0)
                        {
                            llstFieldData.Clear();

                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_HST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            foreach (string colList in lstChangedMasterColList)
                            {
                                llstFieldData.Add(colList, drConfig[colList]);
                            }

                            llstFieldData.Add(COLUMN.MODIFIED_TYPE_CD, "MAN");
                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONFIG_HST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }


                        //#03. MODEL_CONFIG_OPT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstWhereData.Clear();
                            //llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
                            //llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);
                            //SPC-712 by Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                            sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                            llstWhereData.Add(COLUMN.RAWID, drConfigOpt[COLUMN.RAWID]);

                            base.Update(TABLE.MODEL_CONFIG_OPT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }


                        //#04. MODEL_CONTEXT_MST_SPC DATA 삭제
                        llstWhereData.Clear();

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
                        llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                        base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#05. MODEL_CONTEXT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();

                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);
                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            if (useComma)   //spc1281 use commaa == ture 인경우 dataset에 저장하기 전 ,를 ;로 변경.
                            {
                                string contextValues = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = contextValues.Replace(",", ";");

                                string excludeList = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = excludeList.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONTEXT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }


                        //#06. MODEL_RULE_MST_SPC, MODEL_RULE_OPT_MST_SPC DATA 삭제
                        llstWhereData.Clear();
                        sWhereQuery = string.Format("WHERE {0} IN (SELECT RAWID FROM {1} WHERE {2} = :{2}) ", COLUMN.MODEL_RULE_RAWID,
                                                                                                              TABLE.MODEL_RULE_MST_SPC,
                                                                                                              COLUMN.MODEL_CONFIG_RAWID);
                        llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                        base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);

                        base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#07. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string ruleOPTValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = ruleOPTValue.Replace(",", ";");
                                }
                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    //base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#08. MODEL_AUTOCALC_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstWhereData.Clear();
                            //llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
                            //llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            //SPC-658 Initial Calc Count
                            llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.INITIAL_CALC_COUNT]);

                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

                            llstFieldData.Add(COLUMN.STD_YN, drAutoCalc[COLUMN.STD_YN]);
                            llstFieldData.Add(COLUMN.RAW_CL_YN, drAutoCalc[COLUMN.RAW_CL_YN]);
                            llstFieldData.Add(COLUMN.MEAN_CL_YN, drAutoCalc[COLUMN.MEAN_CL_YN]);
                            llstFieldData.Add(COLUMN.STD_CL_YN, drAutoCalc[COLUMN.STD_CL_YN]);
                            llstFieldData.Add(COLUMN.RANGE_CL_YN, drAutoCalc[COLUMN.RANGE_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_M_CL_YN, drAutoCalc[COLUMN.EWMA_M_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_R_CL_YN, drAutoCalc[COLUMN.EWMA_R_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_S_CL_YN, drAutoCalc[COLUMN.EWMA_S_CL_YN]);
                            llstFieldData.Add(COLUMN.MA_CL_YN, drAutoCalc[COLUMN.MA_CL_YN]);
                            llstFieldData.Add(COLUMN.MS_CL_YN, drAutoCalc[COLUMN.MS_CL_YN]);
                            llstFieldData.Add(COLUMN.MR_CL_YN, drAutoCalc[COLUMN.MR_CL_YN]);
                            llstFieldData.Add(COLUMN.ZONE_YN, drAutoCalc[COLUMN.ZONE_YN]);
                            llstFieldData.Add(COLUMN.SHIFT_CALC_YN, drAutoCalc[COLUMN.SHIFT_CALC_YN]);
                            llstFieldData.Add(COLUMN.WITHOUT_IQR_YN, drAutoCalc[COLUMN.WITHOUT_IQR_YN]);
                            //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);
                            //SPC-908 Calc Count 추가
                            llstFieldData.Add(COLUMN.RAW_CALC_COUNT, drAutoCalc[COLUMN.RAW_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.STD_CALC_COUNT, drAutoCalc[COLUMN.STD_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_COUNT, drAutoCalc[COLUMN.MEAN_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_COUNT, drAutoCalc[COLUMN.RANGE_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.EWMA_RANGE_CALC_COUNT, drAutoCalc[COLUMN.EWMA_RANGE_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.EWMA_STD_CALC_COUNT, drAutoCalc[COLUMN.EWMA_STD_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.EWMA_MEAN_CALC_COUNT, drAutoCalc[COLUMN.EWMA_MEAN_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.MA_CALC_COUNT, drAutoCalc[COLUMN.MA_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.MS_CALC_COUNT, drAutoCalc[COLUMN.MS_CALC_COUNT]);
                            llstFieldData.Add(COLUMN.MR_CALC_COUNT, drAutoCalc[COLUMN.MR_CALC_COUNT]);

                            //SPC-1155 ~~_CALC_VALUE, ~~_CALC_OPTION, ~~_CALC_SIDED Column추가로 인한 수정
                            llstFieldData.Add(COLUMN.RAW_CALC_VALUE, drAutoCalc[COLUMN.RAW_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RAW_CALC_OPTION, drAutoCalc[COLUMN.RAW_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RAW_CALC_SIDED, drAutoCalc[COLUMN.RAW_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_VALUE, drAutoCalc[COLUMN.RANGE_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_OPTION, drAutoCalc[COLUMN.RANGE_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_SIDED, drAutoCalc[COLUMN.RANGE_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.STD_CALC_VALUE, drAutoCalc[COLUMN.STD_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.STD_CALC_OPTION, drAutoCalc[COLUMN.STD_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.STD_CALC_SIDED, drAutoCalc[COLUMN.STD_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_VALUE, drAutoCalc[COLUMN.EWMA_M_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_OPTION, drAutoCalc[COLUMN.EWMA_M_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_SIDED, drAutoCalc[COLUMN.EWMA_M_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_VALUE, drAutoCalc[COLUMN.EWMA_R_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_OPTION, drAutoCalc[COLUMN.EWMA_R_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_SIDED, drAutoCalc[COLUMN.EWMA_R_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_VALUE, drAutoCalc[COLUMN.EWMA_S_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_OPTION, drAutoCalc[COLUMN.EWMA_S_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_SIDED, drAutoCalc[COLUMN.EWMA_S_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_VALUE, drAutoCalc[COLUMN.MEAN_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_OPTION, drAutoCalc[COLUMN.MEAN_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_SIDED, drAutoCalc[COLUMN.MEAN_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MR_CALC_VALUE, drAutoCalc[COLUMN.MR_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MR_CALC_OPTION, drAutoCalc[COLUMN.MR_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MR_CALC_SIDED, drAutoCalc[COLUMN.MR_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MS_CALC_VALUE, drAutoCalc[COLUMN.MS_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MS_CALC_OPTION, drAutoCalc[COLUMN.MS_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MS_CALC_SIDED, drAutoCalc[COLUMN.MS_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MA_CALC_VALUE, drAutoCalc[COLUMN.MA_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MA_CALC_OPTION, drAutoCalc[COLUMN.MA_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MA_CALC_SIDED, drAutoCalc[COLUMN.MA_CALC_SIDED]);

                            //SPC-1129 BY STELLA
                            llstFieldData.Add(COLUMN.USE_GLOBAL_YN, drAutoCalc[COLUMN.USE_GLOBAL_YN]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT_OPTION, drAutoCalc[COLUMN.CONTROL_LIMIT_OPTION]);

                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                            sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                            llstWhereData.Add(COLUMN.RAWID, drAutoCalc[COLUMN.RAWID]);

                            base.Update(TABLE.MODEL_AUTOCALC_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }
                    }

                }

                //base.Commit();
            }
            catch (Exception ex)
            {
                //base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }


        public DataSet DeleteSPCModel(byte[] param)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                base.BeginTrans();

                llstWhereData.Add(COLUMN.MODEL_RAWID, _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));

                //MODEL_RULE_OPT_MST_SPC
                sWhereQuery = "WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID IN ( SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID))";
                base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_RULE_MST_SPC
                sWhereQuery = "WHERE MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID)";

                base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_CONTEXT_MST_SPC
                base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                ////MODEL_FILTER_MST_SPC
                //base.Delete(TABLE.MODEL_FILTER_MST_SPC, sWhereQuery, llstWhereData);

                //if (base.ErrorMessage.Length > 0)
                //{
                //    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                //    base.RollBack();
                //    return dsResult;
                //}

                //MODEL_CONFIG_OPT_MST_SPC
                base.Delete(TABLE.MODEL_CONFIG_OPT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_AUTOCALC_MST_SPC
                base.Delete(TABLE.MODEL_AUTOCALC_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }


                //MODEL_CONFIG_MST_SPC
                sWhereQuery = "WHERE MODEL_RAWID = :MODEL_RAWID";

                base.Delete(TABLE.MODEL_CONFIG_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_MST_SPC
                sWhereQuery = "WHERE RAWID = :MODEL_RAWID";

                base.Delete(TABLE.MODEL_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        public DataSet DeleteSPCModelConfig(byte[] param)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                base.BeginTrans();

                //ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];
                //string strWhereCondition = _commondata.MakeVariablesList(arrTemp);

                //llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));

                //SPC-843 by Louis
                if (llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID] != null)
                {
                    ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];
                    if (arrTemp.Count > 300)
                    {
                        int itemp = 0;
                        string strTemp = "";
                        ArrayList arrsModelRawID = new ArrayList();
                        for (int i = 0; i < arrTemp.Count; i++)
                        {
                            if (itemp == 0)
                            {
                                strTemp = "'" + arrTemp[0].ToString() + "'";
                            }
                            else
                            {
                                strTemp = strTemp + ",'" + arrTemp[i].ToString() + "'";
                            }

                            //strTemp += "," + arrTemp[i];
                            itemp++;
                            if (itemp == 300)
                            {
                                // arrsModelRawID.Add(strTemp.Substring(1));
                                arrsModelRawID.Add(strTemp);
                                itemp = 0;
                                strTemp = "";
                            }
                        }
                        if (strTemp.Length > 0)
                        {
                            arrsModelRawID.Add(strTemp);
                        }

                        for (int k = 0; k < arrsModelRawID.Count; k++)
                        {

                            string strWhereCondition = arrsModelRawID[k].ToString();
                            //MODEL_RULE_OPT_MST_SPC
                            sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                            base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_RULE_MST_SPC
                            sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                            base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_CONTEXT_MST_SPC
                            base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            ////MODEL_FILTER_MST_SPC
                            //base.Delete(TABLE.MODEL_FILTER_MST_SPC, sWhereQuery, llstWhereData);

                            //if (base.ErrorMessage.Length > 0)
                            //{
                            //    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //    base.RollBack();
                            //    return dsResult;
                            //}

                            //MODEL_CONFIG_OPT_MST_SPC
                            base.Delete(TABLE.MODEL_CONFIG_OPT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_AUTOCALC_MST_SPC
                            base.Delete(TABLE.MODEL_AUTOCALC_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_CONFIG_MST_SPC
                            sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                            base.Delete(TABLE.MODEL_CONFIG_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }
                    else
                    {
                        //ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];
                        string strWhereCondition = _commondata.MakeVariablesList(arrTemp);

                        sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                        base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_RULE_MST_SPC
                        sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                        base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_CONTEXT_MST_SPC
                        base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        ////MODEL_FILTER_MST_SPC
                        //base.Delete(TABLE.MODEL_FILTER_MST_SPC, sWhereQuery, llstWhereData);

                        //if (base.ErrorMessage.Length > 0)
                        //{
                        //    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        //    base.RollBack();
                        //    return dsResult;
                        //}

                        //MODEL_CONFIG_OPT_MST_SPC
                        base.Delete(TABLE.MODEL_CONFIG_OPT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_AUTOCALC_MST_SPC
                        base.Delete(TABLE.MODEL_AUTOCALC_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_CONFIG_MST_SPC
                        sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                        base.Delete(TABLE.MODEL_CONFIG_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                    }
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }


            return dsResult;
        }

        public bool DeleteSPCModelConfig(string strSubconfig)
        {
            bool _bResult = true;

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                string[] strArrTemp = strSubconfig.Split(';');
                ArrayList arrTemp = new ArrayList();
                for (int i = 0; i < strArrTemp.Length; i++)
                {
                    if (strArrTemp[i].Trim().Length > 0)
                    {
                        arrTemp.Add(strArrTemp[i].Trim());
                    }

                }
                string strWhereCondition = _commondata.MakeVariablesList(arrTemp);

                //MODEL_RULE_OPT_MST_SPC
                sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_RULE_MST_SPC
                sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_CONTEXT_MST_SPC
                base.Delete(TABLE.MODEL_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                ////MODEL_FILTER_MST_SPC
                //base.Delete(TABLE.MODEL_FILTER_MST_SPC, sWhereQuery, llstWhereData);

                //if (base.ErrorMessage.Length > 0)
                //{
                //    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                //    base.RollBack();
                //    return dsResult;
                //}

                //MODEL_CONFIG_OPT_MST_SPC
                base.Delete(TABLE.MODEL_CONFIG_OPT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_AUTOCALC_MST_SPC
                base.Delete(TABLE.MODEL_AUTOCALC_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_CONFIG_MST_SPC
                sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                base.Delete(TABLE.MODEL_CONFIG_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return _bResult;
        }


        public DataSet GetSPCRuleMasterData(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();

                //#01. MODEL_RULE_MST_SPC
                sb.Append(" SELECT A.*, SPC_RULE_NO || '. ' || DESCRIPTION AS SPC_RULE FROM RULE_MST_SPC A ORDER BY A.SPC_RULE_NO ");

                DataSet dsTemp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRule = dsTemp.Tables[0].Copy();
                    dtRule.TableName = "RULE_MST_SPC";

                    dsReturn.Tables.Add(dtRule);
                }

                //#02. MODEL_RULE_OPT_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT B.*, A.SPC_RULE_NO, '' AS RULE_OPTION_VALUE ");
                sb.Append("   FROM RULE_MST_SPC A ");
                sb.Append("        INNER JOIN RULE_OPT_MST_SPC B ");
                sb.Append("           ON (A.RAWID = B.RULE_RAWID) ");

                dsTemp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRuleOPT = dsTemp.Tables[0].Copy();
                    dtRuleOPT.TableName = "RULE_OPT_MST_SPC";

                    dsReturn.Tables.Add(dtRuleOPT);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }


        #region :DEFAULT

        public DataSet GetSPCDefaultModelData(byte[] param)
        {
            //CONFIGURATION 화면을 공통적으로 사용하기 위해
            //MODEL_* TABLE 을 기준으로 COLUMN의 NAME을 맞추도록 한다. 

            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();

            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);

                string sLineRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_LINE_RAWID]);
                string sAreaRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_AREA_RAWID]);
                string sEQPModel = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_EQP_MODEL]);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                llstCondition.Add("LINE_RAWID", sLineRawID);
                llstCondition.Add("AREA_RAWID", sAreaRawID);
                llstCondition.Add("EQP_MODEL", sEQPModel);


                //#00. MODEL_MST_SPC
                Common.CommonData comData = new Common.CommonData();
                DataTable dtModel = comData.GetTableSchema(TABLE.MODEL_MST_SPC);
                dtModel.TableName = TABLE.MODEL_MST_SPC;

                DataRow newRow = dtModel.NewRow();
                newRow[COLUMN.LOCATION_RAWID] = sLineRawID;
                newRow[COLUMN.AREA_RAWID] = sAreaRawID;
                newRow[COLUMN.EQP_MODEL] = sEQPModel;
                dtModel.Rows.Add(newRow);

                dsReturn.Tables.Add(dtModel);


                //#01. MODEL_CONFIG_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append("SELECT '' AS MODEL_RAWID, '' AS PARAM_TYPE_CD, '' AS PARAM_ALIAS, '' AS COMPLEX_YN, '' AS PARAM_LIST, ");
                sb.Append("       '' AS REF_PARAM, '' AS REF_PARAM_LIST, '' AS MAIN_YN, '' MANAGE_TYPE_NAME, 'Y' AS ACTIVATION_YN, 'Y' AS SUB_INTERLOCK_YN, 'Y' AS INHERIT_MAIN_YN, 'Y' AS SUB_AUTOCALC_YN, 'N' AS USE_NORM_YN, 'N' AS VALIDATION_SAME_MODULE_YN, 'L' AS SPC_DATA_LEVEL, ");
                sb.Append("     '' AS CHART_DESCRIPTION,   A.* ");
                sb.Append("  FROM DEF_CONFIG_MST_SPC A ");
                sb.Append(" WHERE 1 = 1 ");
                sb.Append("   AND LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.RAWID, A.DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, ");
                sb.Append("       A.SPC_PARAM_CATEGORY_CD, A.SPC_PRIORITY_CD, A.AUTO_CPK_YN, A.DEFAULT_CHART_LIST, ");
                sb.Append("       A.RESTRICT_SAMPLE_COUNT, A.RESTRICT_SAMPLE_DAYS, ");
                sb.Append("       B.NAME AS SPC_PARAM_CATEGORY, C.NAME AS SPC_PRIORITY , A.RESTRICT_SAMPLE_HOURS"); //<- DEV DEFINE COLUMN
                sb.Append("  FROM DEF_CONFIG_OPT_MST_SPC A ");
                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
                sb.Append("         ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
                sb.Append("       LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
                sb.Append("         ON (A.SPC_PRIORITY_CD = C.CODE) ");
                sb.Append(" WHERE 1 = 1 ");

                sb.Append("   AND A.DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                //sb.Append(" SELECT A.RAWID, A.DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, ");
                //sb.Append("        A.CONTEXT_KEY, A.CONTEXT_VALUE, A.KEY_ORDER, A.EXCLUDE_LIST, ");
                //sb.Append("        NVL(B.CODE, A.CONTEXT_KEY) AS CONTEXT_NAME "); //DEV DEFINE COLUMNS
                //sb.Append("   FROM DEF_CONTEXT_MST_SPC A ");
                //sb.Append("        LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'CONTEXT_TYPE') B ");
                //sb.Append("          ON (A.CONTEXT_KEY = B.CODE) ");
                //sb.Append(" WHERE 1 = 1 ");


                sb.Append(" SELECT DCMS.RAWID, DCMS.DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, DCMS.CONTEXT_KEY,  ");

                if (useComma)
                {
                    sb.Append(" REPLACE(DCMS.CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,  ");
                }
                else
                {
                    sb.Append(" DCMS.CONTEXT_VALUE,  ");
                }

                sb.Append(" DCMS.KEY_ORDER, ");

                if (useComma)
                {
                    sb.Append(" REPLACE(DCMS.EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,  ");
                }
                else
                {
                    sb.Append(" DCMS.EXCLUDE_LIST,  ");
                }

                sb.Append(" DCMS.GROUP_YN, aa.name as context_key_name ");
                sb.Append(" FROM DEF_CONTEXT_MST_SPC DCMS, ");
                sb.Append("    (select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='CONTEXT_TYPE'");
                sb.Append("    UNION ");
                sb.Append("    select CODE,NAME from code_mst_pp ");
                sb.Append("    where category='SPC_CONTEXT_TYPE'");
                sb.Append("    ) AA");
                sb.Append(" WHERE DCMS.context_key = aa.code");

                //sb.Append(" SELECT RAWID, DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, ");
                //sb.Append("        CONTEXT_KEY, CONTEXT_VALUE, KEY_ORDER, EXCLUDE_LIST ");
                //sb.Append("   FROM DEF_CONTEXT_MST_SPC  DCMS, ");

                //sb.Append(" WHERE 1 = 1 ");

                sb.Append("   AND DCMS.DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                sb.Append(" ORDER BY DCMS.KEY_ORDER ASC ");


                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT A.RAWID, A.DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, A.SPC_RULE_NO, A.OCAP_LIST, A.USE_MAIN_SPEC_YN, ");
                sb.Append("        B.DESCRIPTION, ");//추가 COLUMNS
                sb.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, "); //DEV DEFINE COLUMNS
                sb.Append("        '' AS RULE_OPTION, '' AS RULE_OPTION_DATA, A.RULE_ORDER "); //DEV DEFINE COLUMNS
                sb.Append("   FROM DEF_RULE_MST_SPC A ");
                sb.Append("        LEFT OUTER JOIN RULE_MST_SPC B ");
                sb.Append("          ON A.SPC_RULE_NO = B.SPC_RULE_NO ");
                sb.Append(" WHERE 1 = 1 ");

                sb.Append("   AND A.DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT B.RAWID, B.DEF_RULE_RAWID AS MODEL_RULE_RAWID, B.RULE_OPTION_NO,  ");

                if (useComma)
                {
                    sb.Append(" REPLACE(B.RULE_OPTION_VALUE, ';', ',') AS RULE_OPTION_VALUE,  ");
                }
                else
                {
                    sb.Append(" B.RULE_OPTION_VALUE,  ");
                }

                sb.Append("        A.SPC_RULE_NO, "); //추가 COLUMNS
                sb.Append("        C.OPTION_NAME, C.DESCRIPTION  "); //추가 COLUMNS
                sb.Append("   FROM DEF_RULE_MST_SPC A ");
                sb.Append("        LEFT OUTER JOIN DEF_RULE_OPT_MST_SPC B ");
                sb.Append("          ON (A.RAWID = B.DEF_RULE_RAWID) ");
                sb.Append("        LEFT OUTER JOIN ");
                sb.Append("             (SELECT A.RAWID AS RULE_RAWID, A.SPC_RULE_NO, ");
                sb.Append("                     B.RULE_OPTION_NO, B.OPTION_NAME, B.DESCRIPTION ");
                sb.Append("                FROM RULE_MST_SPC A ");
                sb.Append("                     LEFT OUTER JOIN RULE_OPT_MST_SPC B ");
                sb.Append("                       ON (A.RAWID = B.RULE_RAWID) ");
                sb.Append("             ) C ");
                sb.Append("          ON (A.SPC_RULE_NO = C.SPC_RULE_NO AND B.RULE_OPTION_NO = C.RULE_OPTION_NO) ");
                sb.Append(" WHERE 1 = 1 ");

                sb.Append("   AND A.DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                //TODO: select column에 새로 추가된 column도 같이 가져와야 함.
                //#06. MODEL_AUTOCALC_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.RAWID, A.DEF_CONFIG_RAWID AS MODEL_CONFIG_RAWID, ");
                sb.Append("       A.AUTOCALC_PERIOD, A.CALC_COUNT, A.MIN_SAMPLES, A.DEFAULT_PERIOD, A.MAX_PERIOD, A.CONTROL_LIMIT, A.CONTROL_THRESHOLD, ");
                sb.Append("       A.STD_YN, A.RAW_CL_YN, A.MEAN_CL_YN, A.STD_CL_YN, A.RANGE_CL_YN, A.EWMA_M_CL_YN, A.EWMA_R_CL_YN, ");
                sb.Append("       A.EWMA_S_CL_YN, A.MA_CL_YN, NVL(A.MR_CL_YN,'N') AS MR_CL_YN, A.MS_CL_YN, A.ZONE_YN, A.SHIFT_CALC_YN, NVL(A.WITHOUT_IQR_YN,'N') as WITHOUT_IQR_YN,");
                sb.Append("       NVL(a.THRESHOLD_OFF_YN , 'N') as THRESHOLD_OFF_YN, A.CALC_COUNT as INITIAL_CALC_COUNT, ");
                //SPC-1155 COLUMN 추가됨
                sb.Append("       A.RAW_CALC_VALUE, A.MEAN_CALC_VALUE, A.STD_CALC_VALUE, A.RANGE_CALC_VALUE, A.EWMA_M_CALC_VALUE, ");
                sb.Append("       A.EWMA_R_CALC_VALUE, A.EWMA_S_CALC_VALUE, A.MA_CALC_VALUE, A.MR_CALC_VALUE, A.MS_CALC_VALUE, ");
                sb.Append("       A.RAW_CALC_OPTION, A.MEAN_CALC_OPTION, A.STD_CALC_OPTION, A.RANGE_CALC_OPTION, A.EWMA_M_CALC_OPTION, ");
                sb.Append("       A.EWMA_R_CALC_OPTION, A.EWMA_S_CALC_OPTION, A.MA_CALC_OPTION, A.MR_CALC_OPTION, A.MS_CALC_OPTION, ");
                sb.Append("       A.RAW_CALC_SIDED, A.MEAN_CALC_SIDED, A.STD_CALC_SIDED, A.RANGE_CALC_SIDED, A.EWMA_M_CALC_SIDED, ");
                sb.Append("       A.EWMA_R_CALC_SIDED, A.EWMA_S_CALC_SIDED, A.MA_CALC_SIDED, A.MR_CALC_SIDED, A.MS_CALC_SIDED, ");
                sb.Append("       NVL(A.USE_GLOBAL_YN, 'Y') AS USE_GLOBAL_YN, A.CONTROL_LIMIT_OPTION ");

                sb.Append("  FROM DEF_AUTOCALC_MST_SPC A ");
                sb.Append(" WHERE 1 = 1 ");
                sb.Append("   AND DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dAutoCalc = dsTemp.Tables[0].Copy();
                    dAutoCalc.TableName = TABLE.MODEL_AUTOCALC_MST_SPC;

                    dsReturn.Tables.Add(dAutoCalc);
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
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        public DataSet SaveDefaultConfig(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;

            try
            {
                base.BeginTrans();


                foreach (DataRow drModel in dtModel.Rows)
                {
                    string sLineRawID = drModel[COLUMN.LOCATION_RAWID].ToString();
                    string sAreaRawID = drModel[COLUMN.AREA_RAWID].ToString();
                    string sEQPModel = drModel[COLUMN.EQP_MODEL].ToString();
                    string sTempEQPModel = "";

                    llstWhereData.Clear();
                    llstWhereData.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                    llstWhereData.Add(COLUMN.AREA_RAWID, sAreaRawID);
                    if (sEQPModel.Length == 0)
                    {
                        sTempEQPModel = "-";
                        llstWhereData.Add(COLUMN.EQP_MODEL, sTempEQPModel);
                    }
                    else
                    {
                        llstWhereData.Add(COLUMN.EQP_MODEL, sEQPModel);
                    }


                    //#00. 먼저 삭제함

                    //#00-1. DEF_RULE_OPT_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE DEF_RULE_RAWID IN (SELECT RAWID FROM DEF_RULE_MST_SPC WHERE DEF_CONFIG_RAWID IN (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2}))",
                                                COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_RULE_OPT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-2. DEF_RULE_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE DEF_CONFIG_RAWID IN (SELECT RAWID FROM DEF_CONFIG_MST_SPC WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2})", COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_RULE_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-3. DEF_CONTEXT_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_CONTEXT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-4. DEF_CONFIG_OPT_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_CONFIG_OPT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-5. DEF_AUTOCALC_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_AUTOCALC_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //00-6. DEF_CONFIG_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2}", COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_CONFIG_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }


                    //#01. DEFAULT DATA INSERT

                    //#01-1. DEF_CONFIG_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        decimal configRawID = base.GetSequence(SEQUENCE.SEQ_DEF_CONFIG_MST_SPC);

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, configRawID);
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                        llstFieldData.Add(COLUMN.AREA_RAWID, sAreaRawID);
                        llstFieldData.Add(COLUMN.EQP_MODEL, sEQPModel);

                        llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.USE_EXTERNAL_SPEC_YN, drConfig[COLUMN.USE_EXTERNAL_SPEC_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.UPPER_SPEC, drConfig[COLUMN.UPPER_SPEC]);
                        llstFieldData.Add(COLUMN.LOWER_SPEC, drConfig[COLUMN.LOWER_SPEC]);
                        llstFieldData.Add(COLUMN.TARGET, drConfig[COLUMN.TARGET]);
                        llstFieldData.Add(COLUMN.UPPER_CONTROL, drConfig[COLUMN.UPPER_CONTROL]);
                        llstFieldData.Add(COLUMN.LOWER_CONTROL, drConfig[COLUMN.LOWER_CONTROL]);
                        llstFieldData.Add(COLUMN.CENTER_LINE, drConfig[COLUMN.CENTER_LINE]);
                        llstFieldData.Add(COLUMN.SAMPLE_COUNT, drConfig[COLUMN.SAMPLE_COUNT]);
                        llstFieldData.Add(COLUMN.EWMA_LAMBDA, drConfig[COLUMN.EWMA_LAMBDA]);
                        llstFieldData.Add(COLUMN.MOVING_COUNT, drConfig[COLUMN.MOVING_COUNT]);
                        llstFieldData.Add(COLUMN.STD, drConfig[COLUMN.STD]);
                        llstFieldData.Add(COLUMN.DESCRIPTION, drConfig[COLUMN.DESCRIPTION]);

                        llstFieldData.Add(COLUMN.RAW_LCL, drConfig[COLUMN.RAW_LCL]);
                        llstFieldData.Add(COLUMN.RAW_UCL, drConfig[COLUMN.RAW_UCL]);
                        llstFieldData.Add(COLUMN.RAW_CENTER_LINE, drConfig[COLUMN.RAW_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.STD_LCL, drConfig[COLUMN.STD_LCL]);
                        llstFieldData.Add(COLUMN.STD_UCL, drConfig[COLUMN.STD_UCL]);
                        llstFieldData.Add(COLUMN.STD_CENTER_LINE, drConfig[COLUMN.STD_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.RANGE_LCL, drConfig[COLUMN.RANGE_LCL]);
                        llstFieldData.Add(COLUMN.RANGE_UCL, drConfig[COLUMN.RANGE_UCL]);
                        llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, drConfig[COLUMN.RANGE_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_M_LCL, drConfig[COLUMN.EWMA_M_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_UCL, drConfig[COLUMN.EWMA_M_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, drConfig[COLUMN.EWMA_M_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_R_LCL, drConfig[COLUMN.EWMA_R_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_UCL, drConfig[COLUMN.EWMA_R_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, drConfig[COLUMN.EWMA_R_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.EWMA_S_LCL, drConfig[COLUMN.EWMA_S_LCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_UCL, drConfig[COLUMN.EWMA_S_UCL]);
                        llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, drConfig[COLUMN.EWMA_S_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MA_LCL, drConfig[COLUMN.MA_LCL]);
                        llstFieldData.Add(COLUMN.MA_UCL, drConfig[COLUMN.MA_UCL]);
                        llstFieldData.Add(COLUMN.MA_CENTER_LINE, drConfig[COLUMN.MA_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MS_LCL, drConfig[COLUMN.MS_LCL]);
                        llstFieldData.Add(COLUMN.MS_UCL, drConfig[COLUMN.MS_UCL]);
                        llstFieldData.Add(COLUMN.MS_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.MR_LCL, drConfig[COLUMN.MS_LCL]);
                        llstFieldData.Add(COLUMN.MR_UCL, drConfig[COLUMN.MS_UCL]);
                        llstFieldData.Add(COLUMN.MR_CENTER_LINE, drConfig[COLUMN.MS_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.LOWER_TECHNICAL_LIMIT, drConfig[COLUMN.LOWER_TECHNICAL_LIMIT]);
                        llstFieldData.Add(COLUMN.UPPER_TECHNICAL_LIMIT, drConfig[COLUMN.UPPER_TECHNICAL_LIMIT]);

                        llstFieldData.Add(COLUMN.ZONE_A_LCL, drConfig[COLUMN.ZONE_A_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_A_UCL, drConfig[COLUMN.ZONE_A_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_B_LCL, drConfig[COLUMN.ZONE_B_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_B_UCL, drConfig[COLUMN.ZONE_B_UCL]);

                        llstFieldData.Add(COLUMN.ZONE_C_LCL, drConfig[COLUMN.ZONE_C_LCL]);
                        llstFieldData.Add(COLUMN.ZONE_C_UCL, drConfig[COLUMN.ZONE_C_UCL]);

                        //Chris : 2010-06-18

                        llstFieldData.Add(COLUMN.SPEC_USL_OFFSET, drConfig[COLUMN.SPEC_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.SPEC_LSL_OFFSET, drConfig[COLUMN.SPEC_LSL_OFFSET]);
                        //llstFieldData.Add(COLUMN.SPEC_OFFSET_YN, drConfig[COLUMN.SPEC_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RAW_UCL_OFFSET, drConfig[COLUMN.RAW_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RAW_LCL_OFFSET, drConfig[COLUMN.RAW_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RAW_OFFSET_YN, drConfig[COLUMN.RAW_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.STD_UCL_OFFSET, drConfig[COLUMN.STD_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.STD_LCL_OFFSET, drConfig[COLUMN.STD_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.STD_OFFSET_YN, drConfig[COLUMN.STD_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MEAN_UCL_OFFSET, drConfig[COLUMN.MEAN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MEAN_LCL_OFFSET, drConfig[COLUMN.MEAN_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MEAN_OFFSET_YN, drConfig[COLUMN.MEAN_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.RANGE_UCL_OFFSET, drConfig[COLUMN.RANGE_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.RANGE_LCL_OFFSET, drConfig[COLUMN.RANGE_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.RANGE_OFFSET_YN, drConfig[COLUMN.RANGE_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_M_UCL_OFFSET, drConfig[COLUMN.EWMA_M_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_M_LCL_OFFSET, drConfig[COLUMN.EWMA_M_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_M_OFFSET_YN, drConfig[COLUMN.EWMA_M_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_S_UCL_OFFSET, drConfig[COLUMN.EWMA_S_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_S_LCL_OFFSET, drConfig[COLUMN.EWMA_S_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_S_OFFSET_YN, drConfig[COLUMN.EWMA_S_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.EWMA_R_UCL_OFFSET, drConfig[COLUMN.EWMA_R_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.EWMA_R_LCL_OFFSET, drConfig[COLUMN.EWMA_R_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.EWMA_R_OFFSET_YN, drConfig[COLUMN.EWMA_R_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MA_UCL_OFFSET, drConfig[COLUMN.MA_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MA_LCL_OFFSET, drConfig[COLUMN.MA_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MA_OFFSET_YN, drConfig[COLUMN.MA_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MS_UCL_OFFSET, drConfig[COLUMN.MS_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MS_LCL_OFFSET, drConfig[COLUMN.MS_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.MS_OFFSET_YN, drConfig[COLUMN.MS_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.MR_UCL_OFFSET, drConfig[COLUMN.MR_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.MR_LCL_OFFSET, drConfig[COLUMN.MR_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.ZONE_A_UCL_OFFSET, drConfig[COLUMN.ZONE_A_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_A_LCL_OFFSET, drConfig[COLUMN.ZONE_A_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_A_OFFSET_YN, drConfig[COLUMN.ZONE_A_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_B_UCL_OFFSET, drConfig[COLUMN.ZONE_B_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_B_LCL_OFFSET, drConfig[COLUMN.ZONE_B_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_B_OFFSET_YN, drConfig[COLUMN.ZONE_B_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.ZONE_C_UCL_OFFSET, drConfig[COLUMN.ZONE_C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.ZONE_C_LCL_OFFSET, drConfig[COLUMN.ZONE_C_LCL_OFFSET]);
                        //llstFieldData.Add(COLUMN.ZONE_C_OFFSET_YN, drConfig[COLUMN.ZONE_C_OFFSET_YN]);

                        llstFieldData.Add(COLUMN.UPPER_FILTER, drConfig[COLUMN.UPPER_FILTER]);
                        llstFieldData.Add(COLUMN.LOWER_FILTER, drConfig[COLUMN.LOWER_FILTER]);

                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);

                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);

                        //---------------------------------------------------------------------------------

                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        base.Insert(TABLE.DEF_CONFIG_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //#01-2. DEF_CONFIG_OPT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_CONFIG_OPT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

                            //SPC-712 By Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_CONFIG_OPT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#01-2. DEF_CONTEXT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_CONTEXT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            //spc-1281
                            if (useComma)
                            {
                                string sValue = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = sValue.Replace(",", ";");

                                sValue = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = sValue.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_CONTEXT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#01-3. DEF_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_DEF_RULE_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_RULE_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#01-4. DEF_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_RULE_OPT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.DEF_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string sValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = sValue.Replace(",", ";");
                                }

                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.DEF_RULE_OPT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#01-4. DEF_AUTOCALC_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_AUTOCALC_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            //SPC-658 Initial Calc Count
                            //  llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.INITIAL_CALC_COUNT]);

                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

                            llstFieldData.Add(COLUMN.STD_YN, drAutoCalc[COLUMN.STD_YN]);
                            llstFieldData.Add(COLUMN.RAW_CL_YN, drAutoCalc[COLUMN.RAW_CL_YN]);
                            llstFieldData.Add(COLUMN.MEAN_CL_YN, drAutoCalc[COLUMN.MEAN_CL_YN]);
                            llstFieldData.Add(COLUMN.STD_CL_YN, drAutoCalc[COLUMN.STD_CL_YN]);
                            llstFieldData.Add(COLUMN.RANGE_CL_YN, drAutoCalc[COLUMN.RANGE_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_M_CL_YN, drAutoCalc[COLUMN.EWMA_M_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_R_CL_YN, drAutoCalc[COLUMN.EWMA_R_CL_YN]);
                            llstFieldData.Add(COLUMN.EWMA_S_CL_YN, drAutoCalc[COLUMN.EWMA_S_CL_YN]);
                            llstFieldData.Add(COLUMN.MA_CL_YN, drAutoCalc[COLUMN.MA_CL_YN]);
                            llstFieldData.Add(COLUMN.MS_CL_YN, drAutoCalc[COLUMN.MS_CL_YN]);
                            llstFieldData.Add(COLUMN.MR_CL_YN, drAutoCalc[COLUMN.MR_CL_YN]);
                            llstFieldData.Add(COLUMN.ZONE_YN, drAutoCalc[COLUMN.ZONE_YN]);
                            llstFieldData.Add(COLUMN.SHIFT_CALC_YN, drAutoCalc[COLUMN.SHIFT_CALC_YN]);
                            llstFieldData.Add(COLUMN.WITHOUT_IQR_YN, drAutoCalc[COLUMN.WITHOUT_IQR_YN]);

                            //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);

                            //SPC-1155 ~~_CALC_VALUE, ~~_CALC_OPTION, ~~_CALC_SIDED Column추가로 인한 수정
                            llstFieldData.Add(COLUMN.RAW_CALC_VALUE, drAutoCalc[COLUMN.RAW_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RAW_CALC_OPTION, drAutoCalc[COLUMN.RAW_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RAW_CALC_SIDED, drAutoCalc[COLUMN.RAW_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_VALUE, drAutoCalc[COLUMN.RANGE_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_OPTION, drAutoCalc[COLUMN.RANGE_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.RANGE_CALC_SIDED, drAutoCalc[COLUMN.RANGE_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.STD_CALC_VALUE, drAutoCalc[COLUMN.STD_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.STD_CALC_OPTION, drAutoCalc[COLUMN.STD_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.STD_CALC_SIDED, drAutoCalc[COLUMN.STD_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_VALUE, drAutoCalc[COLUMN.EWMA_M_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_OPTION, drAutoCalc[COLUMN.EWMA_M_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_M_CALC_SIDED, drAutoCalc[COLUMN.EWMA_M_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_VALUE, drAutoCalc[COLUMN.EWMA_R_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_OPTION, drAutoCalc[COLUMN.EWMA_R_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_R_CALC_SIDED, drAutoCalc[COLUMN.EWMA_R_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_VALUE, drAutoCalc[COLUMN.EWMA_S_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_OPTION, drAutoCalc[COLUMN.EWMA_S_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.EWMA_S_CALC_SIDED, drAutoCalc[COLUMN.EWMA_S_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_VALUE, drAutoCalc[COLUMN.MEAN_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_OPTION, drAutoCalc[COLUMN.MEAN_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MEAN_CALC_SIDED, drAutoCalc[COLUMN.MEAN_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MR_CALC_VALUE, drAutoCalc[COLUMN.MR_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MR_CALC_OPTION, drAutoCalc[COLUMN.MR_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MR_CALC_SIDED, drAutoCalc[COLUMN.MR_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MS_CALC_VALUE, drAutoCalc[COLUMN.MS_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MS_CALC_OPTION, drAutoCalc[COLUMN.MS_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MS_CALC_SIDED, drAutoCalc[COLUMN.MS_CALC_SIDED]);
                            llstFieldData.Add(COLUMN.MA_CALC_VALUE, drAutoCalc[COLUMN.MA_CALC_VALUE]);
                            llstFieldData.Add(COLUMN.MA_CALC_OPTION, drAutoCalc[COLUMN.MA_CALC_OPTION]);
                            llstFieldData.Add(COLUMN.MA_CALC_SIDED, drAutoCalc[COLUMN.MA_CALC_SIDED]);

                            //SPC-1129 BY STELLA
                            llstFieldData.Add(COLUMN.USE_GLOBAL_YN, drAutoCalc[COLUMN.USE_GLOBAL_YN]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT_OPTION, drAutoCalc[COLUMN.CONTROL_LIMIT_OPTION]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_AUTOCALC_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }

                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        #endregion






        public DataSet QueryModelConfigMstSPC(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            //sb.Append("select * from MODEL_CONFIG_MST_SPC ");
            //sb.Append(" where 1 = 1 ");

            //2009-12-07 bskwon 수정
            sb.Append("select mcms.*,cc.name as MANAGE_TYPE_NAME ");
            sb.Append("from MODEL_CONFIG_MST_SPC mcms,");
            sb.Append("    (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY='SPC_MANAGE_TYPE') cc");
            sb.Append("WHERE mcms.manage_type_cd = cc.code(+)   ");

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   and rawid = :MODEL_CONFIG_RAWID ");
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   and model_rawid = :MODEL_RAWID ");
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "MODEL_CONFIG_MST_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        public DataSet QueryModelConfigOPTMstSPC(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT A.*, B.NAME AS SPC_PARAM_CATEGORY, C.NAME AS SPC_PRIORITY ");
            sb.Append("  FROM MODEL_CONFIG_OPT_MST_SPC A ");
            sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
            sb.Append("   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
            sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
            sb.Append("   ON (A.SPC_PRIORITY_CD = C.CODE) ");
            sb.Append("WHERE 1 = 1 ");

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("  AND A.model_config_rawid IN (SELECT rawid FROM model_config_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND A.model_config_rawid = :MODEL_CONFIG_RAWID ");
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "MODEL_CONFIG_OPT_MST_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        public DataSet QueryModelContextMstSPC(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            //sb.Append("SELECT * FROM MODEL_CONTEXT_MST_SPC ");
            //sb.Append(" WHERE 1 = 1 ");

            //2009-12-07 bskwon 수정
            sb.Append(" SELECT mcms.*, aa.name as context_key_name");
            sb.Append(" FROM MODEL_CONTEXT_MST_SPC mcms, ");
            sb.Append("    (select CODE,NAME from code_mst_pp ");
            sb.Append("    where category='CONTEXT_TYPE'");
            sb.Append("    UNION ");
            sb.Append("    select CODE,NAME from code_mst_pp ");
            sb.Append("    where category='SPC_CONTEXT_TYPE'");
            sb.Append("    ) AA");
            sb.Append(" WHERE mcms.context_key = aa.code");


            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND model_config_rawid IN (SELECT rawid FROM model_config_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND model_config_rawid = :MODEL_CONFIG_RAWID ");
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "MODEL_CONTEXT_MST_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }


        public DataSet QueryModelRuleMstSPC(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM MODEL_RULE_MST_SPC ");
            sb.Append(" WHERE 1 = 1 ");

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND model_config_rawid IN (SELECT rawid FROM model_config_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND model_config_rawid = :MODEL_CONFIG_RAWID ");
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "MODEL_RULE_MST_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }


        public DataSet QueryModelRuleOPTMstSPC(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT B.* ");
            sb.Append("  FROM MODEL_RULE_MST_SPC A ");
            sb.Append("       LEFT OUTER JOIN MODEL_RULE_OPT_MST_SPC B ");
            sb.Append("         ON (A.RAWID = B.MODEL_RULE_RAWID) ");
            sb.Append(" WHERE 1 = 1 ");

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID = :MODEL_CONIFG_RAWID ");
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "MODEL_RULE_OPT_MST_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }


        public DataSet GetSPCParamList(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            LinkedList llstParam = new LinkedList();
            llstParam.SetSerialData(param);

            string sLineRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_LINE_RAWID]);
            string sAreaRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_AREA_RAWID]);
            string sEQPModel = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_EQP_MODEL]);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT DISTINCT PARAM_TYPE_CD, PARAM_TYPE, LOCATION_RAWID, AREA_RAWID, PARAM_ALIAS, DATA_TYPE_CD, DATA_TYPE, UNIT ");
            sb.Append("  FROM PARAM_VW_SPC ");
            sb.Append(" WHERE 1 = 1 ");
            sb.Append("   AND PARAM_TYPE_CD = :PARAM_TYPE_CD ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                llstCondition.Add("PARAM_TYPE_CD", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_PARAM_TYPE_CD]));

                if (sLineRawID.Length > 0)
                {
                    sb.Append("   AND LOCATION_RAWID = :LINE_RAWID ");
                    llstCondition.Add("LINE_RAWID", sLineRawID);
                }
                if (sAreaRawID.Length > 0)
                {
                    sb.Append("   AND AREA_RAWID = :AREA_RAWID ");
                    llstCondition.Add("AREA_RAWID", sAreaRawID);
                }

                if (sEQPModel.Length > 0)
                {
                    sb.Append("   AND EQP_MODEL = :EQP_MODEL ");
                    llstCondition.Add("EQP_MODEL", sEQPModel);
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        public DataSet GetSPCContextKeyTableAndColumns()
        {
            DataSet dsReturn = new DataSet();

            string strQuery = " SELECT CONTEXT_KEY, TABLE_NAME, COLUMN_NAME, DISPLAY_COLUMNS, WHERE_CONDITION FROM CONTEXT_LINK_MST_SPC ";

            try
            {
                dsReturn = base.Query(strQuery);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        public DataSet GetSPCContextValueList(string sTableName, string sColumnName, string sDisplayColumns, string sWhereCondition, string sWhereValue)
        {
            DataSet dsReturn = new DataSet();

            string strQuery = "";
            if (sDisplayColumns.Trim().Length == 0)
            {
                strQuery = string.Format(" SELECT DISTINCT {0} FROM {1} ", sColumnName, sTableName);
            }
            else
            {
                strQuery = string.Format(" SELECT DISTINCT {0}, {2} FROM {1} ", sColumnName, sTableName, sDisplayColumns);
            }

            if (sWhereCondition.Trim().Length > 0)
            {
                string[] strArrWhereCondition = sWhereCondition.Split(',');
                string[] strArrWhereValue = sWhereValue.Split(',');
                for (int i = 0; i < strArrWhereCondition.Length; i++)
                {
                    if (i == 0)
                        strQuery += string.Format(" WHERE {0} = '{1}' ", strArrWhereCondition[i].Trim(), strArrWhereValue[i].Trim());
                    else
                        strQuery += string.Format(" AND {0} = '{1}' ", strArrWhereCondition[i].Trim(), strArrWhereValue[i].Trim());
                }
            }

            if (sDisplayColumns.Trim().Length == 0)
            {
                strQuery += string.Format(" ORDER BY {0} ", sColumnName);
            }
            else
            {
                strQuery += string.Format(" ORDER BY {1}, {0} ", sColumnName, sDisplayColumns);
            }

            try
            {
                dsReturn = base.Query(strQuery);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        private const string SQL_GET_DUPLICATE_SPC_MODEL_RAWID = " SELECT   DISTINCT A.RAWID, A.SPC_MODEL_NAME " //, A.LOCATION_RAWID, A.AREA_RAWID, A.EQP_MODEL, B.PARAM_ALIAS, "
            //+ "          C.CONTEXT_KEY, C.CONTEXT_VALUE, C.KEY_ORDER                          "
                                                            + "     FROM MODEL_MST_SPC A, MODEL_CONFIG_MST_SPC B, MODEL_CONTEXT_MST_SPC C     "
                                                            + "    WHERE A.RAWID = B.MODEL_RAWID                                              "
                                                            + "      AND B.RAWID = C.MODEL_CONFIG_RAWID                                       "
                                                            + "      AND B.MAIN_YN = 'Y'                                                      "
                                                            + "      AND A.LOCATION_RAWID = :LOCATIONRAWID                                    "
                                                            + "      AND NVL (A.AREA_RAWID, -1) = :AREARAWID                                  "
                                                            + "      AND NVL (A.EQP_MODEL, '-') = :EQPMODEL                                   "
                                                            + "      AND B.PARAM_ALIAS = :PARAMALIAS                                          "
                                                            + "      {0}                                                                      "
            //+ " ORDER BY A.RAWID, C.KEY_ORDER                                                 ";
                                                            + " ORDER BY A.RAWID                                                              ";

        private const string SQL_GET_DUPLICATE_SPC_MODEL = " SELECT   C.RAWID, C.CONTEXT_KEY, C.CONTEXT_VALUE, C.KEY_ORDER                    "
                                                            + "     FROM MODEL_MST_SPC A, MODEL_CONFIG_MST_SPC B, MODEL_CONTEXT_MST_SPC C     "
                                                            + "    WHERE A.RAWID = B.MODEL_RAWID                                              "
                                                            + "      AND B.RAWID = C.MODEL_CONFIG_RAWID                                       "
                                                            + "      AND B.MAIN_YN = 'Y'                                                      "
                                                            + "      AND B.ACTIVATION_YN = 'Y'                                                "
                                                            + "      AND A.RAWID = :RAWID                                                     "
                                                            + "      ORDER BY C.KEY_ORDER                                                     ";

        private const string SQL_GET_DUPLICATE_SPC_NAME = " SELECT   DISTINCT RAWID, SPC_MODEL_NAME                                          "
                                                           + "     FROM MODEL_MST_SPC                                                        "
                                                           + "    WHERE LOCATION_RAWID = :LOCATIONRAWID                                      "
                                                           + "      AND NVL (AREA_RAWID, -1) = :AREARAWID                                    "
                                                           + "      AND NVL (EQP_MODEL, '-') = :EQPMODEL                                     "
                                                           + "      AND SPC_MODEL_NAME = :SPCMODELNAME                                       ";


        public string CheckDuplicateSPCModel(byte[] param)
        {

            bool bResult = true;
            string strResult = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                string strLocationRawID = "";
                string strAreaRawID = "";
                string strEQPModel = "";
                string strParamAlias = "";
                string strSubWhere = "";
                string strModelRawID = "";
                string strModelName = "";
                int idx = 0;

                DataTable dtModel = (DataTable)llstParam[TABLE.MODEL_MST_SPC];
                DataTable dtConfig = (DataTable)llstParam[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = (DataTable)llstParam[TABLE.MODEL_CONTEXT_MST_SPC];

                foreach (DataRow drModel in dtModel.Rows)
                {
                    strModelName = drModel[COLUMN.SPC_MODEL_NAME].ToString();
                    strModelRawID = drModel[COLUMN.RAWID].ToString();
                    strLocationRawID = drModel[COLUMN.LOCATION_RAWID].ToString();
                    strAreaRawID = drModel[COLUMN.AREA_RAWID].ToString();
                    strEQPModel = drModel[COLUMN.EQP_MODEL].ToString();

                    if (strAreaRawID == null || strAreaRawID == "")
                        strAreaRawID = "-1";

                    if (strEQPModel == null || strEQPModel == "")
                        strEQPModel = "-";
                }

                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    strParamAlias = drConfig[COLUMN.PARAM_ALIAS].ToString();
                }

                foreach (DataRow drContext in dtContext.Rows)
                {
                    string strTempContextKey = drContext[COLUMN.CONTEXT_KEY].ToString();
                    string strTempContextValue = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    string strTempKeyOrder = drContext[COLUMN.KEY_ORDER].ToString();

                    if (idx == 0)
                    {
                        strSubWhere += string.Format(" AND ((C.CONTEXT_KEY = '{0}' AND C.CONTEXT_VALUE = '{1}' AND C.KEY_ORDER = {2} ", strTempContextKey, strTempContextValue, strTempKeyOrder);
                    }
                    else
                    {
                        strSubWhere += string.Format(" ) OR (C.CONTEXT_KEY = '{0}' AND C.CONTEXT_VALUE = '{1}' AND C.KEY_ORDER = {2} ", strTempContextKey, strTempContextValue, strTempKeyOrder);
                    }
                    idx++;
                }

                if (strSubWhere.Length > 0)
                    strSubWhere += " )) ";

                LinkedList lnkListWhere = new LinkedList();
                lnkListWhere.Add("LOCATIONRAWID", strLocationRawID);
                lnkListWhere.Add("AREARAWID", strAreaRawID);
                lnkListWhere.Add("EQPMODEL", strEQPModel);
                lnkListWhere.Add("SPCMODELNAME", strModelName);

                string strQuery = SQL_GET_DUPLICATE_SPC_NAME;

                if (strModelRawID != null && strModelRawID.Length > 0)
                {
                    strQuery += "      AND RAWID <> :RAWID                                                      ";
                    lnkListWhere.Add("RAWID", strModelRawID);
                }

                DataSet dsTempReturn = base.Query(strQuery, lnkListWhere);
                if (dsTempReturn != null && dsTempReturn.Tables.Count > 0)
                {
                    if (dsTempReturn.Tables[0].Rows.Count == 0)
                    {
                        bResult = false;
                    }
                    else
                    {
                        for (int i = 0; i < dsTempReturn.Tables[0].Rows.Count; i++)
                        {
                            string strTempSPCModelName = dsTempReturn.Tables[0].Rows[i][COLUMN.SPC_MODEL_NAME].ToString();
                            if (strTempSPCModelName.Length > 0)
                                return strTempSPCModelName;
                        }
                    }

                }

                lnkListWhere.Clear();
                lnkListWhere.Add("LOCATIONRAWID", strLocationRawID);
                lnkListWhere.Add("AREARAWID", strAreaRawID);
                lnkListWhere.Add("EQPMODEL", strEQPModel);
                lnkListWhere.Add("PARAMALIAS", strParamAlias);

                if (strModelRawID != null && strModelRawID.Length > 0)
                {
                    strSubWhere += "      AND A.RAWID <> :RAWID ";
                    lnkListWhere.Add("RAWID", strModelRawID);
                }

                DataSet dsReturn = base.Query(string.Format(SQL_GET_DUPLICATE_SPC_MODEL_RAWID, strSubWhere), lnkListWhere);

                if (dsReturn != null && dsReturn.Tables.Count > 0)
                {
                    if (dsReturn.Tables[0].Rows.Count == 0)
                    {
                        bResult = false;
                    }
                    else
                    {
                        for (int i = 0; i < dsReturn.Tables[0].Rows.Count; i++)
                        {
                            string strTempRawid = dsReturn.Tables[0].Rows[i]["RAWID"].ToString();
                            string strTempSPCModelName = dsReturn.Tables[0].Rows[i]["SPC_MODEL_NAME"].ToString();

                            lnkListWhere.Clear();
                            lnkListWhere.Add("RAWID", strTempRawid);

                            DataSet dsTemp = base.Query(SQL_GET_DUPLICATE_SPC_MODEL, lnkListWhere);

                            if (dsTemp != null && dsTemp.Tables.Count > 0)
                            {
                                if (dsTemp.Tables[0].Rows.Count == idx)
                                {
                                    for (int k = 0; k < dsTemp.Tables[0].Rows.Count; k++)
                                    {
                                        if (dsTemp.Tables[0].Rows[k][COLUMN.CONTEXT_KEY].ToString() == dtContext.Rows[k][COLUMN.CONTEXT_KEY].ToString()
                                            && dsTemp.Tables[0].Rows[k][COLUMN.CONTEXT_VALUE].ToString() == dtContext.Rows[k][COLUMN.CONTEXT_VALUE].ToString()
                                            && dsTemp.Tables[0].Rows[k][COLUMN.KEY_ORDER].ToString() == dtContext.Rows[k][COLUMN.KEY_ORDER].ToString())
                                        {
                                            bResult = true;
                                            if (k == dsTemp.Tables[0].Rows.Count - 1)
                                            {
                                                strResult = strTempSPCModelName;
                                                return strResult;
                                            }
                                        }
                                        else
                                        {
                                            bResult = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    bResult = false;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                bResult = true;
            }

            return strResult;
        }

        public bool SaveInterlockYN(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            ArrayList arrActivation = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sUserID = "";
            string sComment = string.Empty;
            string sChangedItems = string.Empty;

            base.BeginTrans();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.RAWID))
                {
                    arrRawID = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.RAWID];
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.INTERLOCK_YN))
                {
                    arrInterlock = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.INTERLOCK_YN];
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.ACTIVATION_YN))
                {
                    arrActivation = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.ACTIVATION_YN];
                }

                if (llstParam.Contains(COLUMN.SAVE_COMMENT))
                {
                    sComment = llstParam[COLUMN.SAVE_COMMENT].ToString();
                }

                if (llstParam.Contains(COLUMN.CHANGED_ITEMS))
                {
                    sChangedItems = llstParam[COLUMN.CHANGED_ITEMS].ToString();
                }

                for (int i = 0; i < arrRawID.Count; i++)
                {
                    llstFieldData.Clear();
                    if (Convert.ToBoolean(arrInterlock[i].ToString()))
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_Y);
                    }
                    else
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_N);
                    }

                    if (Convert.ToBoolean(arrActivation[i].ToString()))
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.ACTIVATION_YN, Definition.VARIABLE_Y);
                    }
                    else
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.ACTIVATION_YN, Definition.VARIABLE_N);
                    }
                    //spc-977
                    llstFieldData.Add(COLUMN.SAVE_COMMENT, sComment);
                    llstFieldData.Add(COLUMN.CHANGED_ITEMS, sChangedItems);

                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    llstWhereData.Clear();
                    llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, arrRawID[i].ToString());

                    base.Update(Definition.TableName.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                bReturn = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();

            return bReturn;
        }

        public bool SaveSPCModelSpecData(byte[] param)
        {
            bool bReturn = true;

            ArrayList arrTempSpecRawID = new ArrayList();
            ArrayList arrTempSpecMainYN = new ArrayList();

            //SPC-676 by Louis
            ArrayList arrTempChartDescription = new ArrayList();

            ArrayList arrTempUPPER_SPEC = new ArrayList();
            ArrayList arrTempLOWER_SPEC = new ArrayList();
            ArrayList arrTempUPPER_CONTROL = new ArrayList();
            ArrayList arrTempLOWER_CONTROL = new ArrayList();
            ArrayList arrTempRAW_UCL = new ArrayList();
            ArrayList arrTempRAW_LCL = new ArrayList();
            ArrayList arrTempSTD_UCL = new ArrayList();
            ArrayList arrTempSTD_LCL = new ArrayList();
            ArrayList arrTempRANGE_UCL = new ArrayList();
            ArrayList arrTempRANGE_LCL = new ArrayList();
            ArrayList arrTempEWMA_MEAN_UCL = new ArrayList();
            ArrayList arrTempEWMA_MEAN_LCL = new ArrayList();
            ArrayList arrTempEWMA_RANGE_UCL = new ArrayList();
            ArrayList arrTempEWMA_RANGE_LCL = new ArrayList();
            ArrayList arrTempEWMA_STD_UCL = new ArrayList();
            ArrayList arrTempEWMA_STD_LCL = new ArrayList();
            ArrayList arrTempMA_UCL = new ArrayList();
            ArrayList arrTempMA_LCL = new ArrayList();
            ArrayList arrTempMS_UCL = new ArrayList();
            ArrayList arrTempMS_LCL = new ArrayList();
            ArrayList arrTempMR_UCL = new ArrayList();
            ArrayList arrTempMR_LCL = new ArrayList();
            ArrayList arrTempUPPER_TECHNICAL_LIMIT = new ArrayList();
            ArrayList arrTempLOWER_TECHNICAL_LIMIT = new ArrayList();
            ArrayList arrTempTARGET = new ArrayList();
            ArrayList arrTempCENTER_LINE = new ArrayList();
            ArrayList arrTempRAW_CENTER_LINE = new ArrayList();
            ArrayList arrTempSTD_CENTER_LINE = new ArrayList();
            ArrayList arrTempRANGE_CENTER_LINE = new ArrayList();
            ArrayList arrTempEWMA_MEAN_CENTER_LINE = new ArrayList();
            ArrayList arrTempEWMA_RANGE_CENTER_LINE = new ArrayList();
            ArrayList arrTempEWMA_STD_CENTER_LINE = new ArrayList();
            ArrayList arrTempMA_CENTER_LINE = new ArrayList();
            ArrayList arrTempMS_CENTER_LINE = new ArrayList();
            ArrayList arrTempMR_CENTER_LINE = new ArrayList();
            ArrayList arrTempSTD = new ArrayList();
            ArrayList arrTempZONE_A_UCL = new ArrayList();
            ArrayList arrTempZONE_A_LCL = new ArrayList();
            ArrayList arrTempZONE_B_UCL = new ArrayList();
            ArrayList arrTempZONE_B_LCL = new ArrayList();
            ArrayList arrTempZONE_C_UCL = new ArrayList();
            ArrayList arrTempZONE_C_LCL = new ArrayList();

            string sWhereQuery = "WHERE RAWID = :RAWID";

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sUserID = "";
            string comment = "";    //spc-977 by stella

            base.BeginTrans();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();
                comment = llstParam[COLUMN.SAVE_COMMENT].ToString();     //spc-977 by stella

                arrTempChartDescription = (ArrayList)llstParam[COLUMN.CHART_DESCRIPTON];
                arrTempSpecRawID = (ArrayList)llstParam[COLUMN.RAWID];
                arrTempSpecMainYN = (ArrayList)llstParam[COLUMN.MAIN_YN];
                arrTempUPPER_SPEC = (ArrayList)llstParam[COLUMN.UPPER_SPEC];
                arrTempLOWER_SPEC = (ArrayList)llstParam[COLUMN.LOWER_SPEC];
                arrTempUPPER_CONTROL = (ArrayList)llstParam[COLUMN.UPPER_CONTROL];
                arrTempLOWER_CONTROL = (ArrayList)llstParam[COLUMN.LOWER_CONTROL];
                arrTempRAW_UCL = (ArrayList)llstParam[COLUMN.RAW_UCL];
                arrTempRAW_LCL = (ArrayList)llstParam[COLUMN.RAW_LCL];
                arrTempSTD_UCL = (ArrayList)llstParam[COLUMN.STD_UCL];
                arrTempSTD_LCL = (ArrayList)llstParam[COLUMN.STD_LCL];
                arrTempRANGE_UCL = (ArrayList)llstParam[COLUMN.RANGE_UCL];
                arrTempRANGE_LCL = (ArrayList)llstParam[COLUMN.RANGE_LCL];
                arrTempEWMA_MEAN_UCL = (ArrayList)llstParam[COLUMN.EWMA_MEAN_UCL];
                arrTempEWMA_MEAN_LCL = (ArrayList)llstParam[COLUMN.EWMA_MEAN_LCL];
                arrTempEWMA_RANGE_UCL = (ArrayList)llstParam[COLUMN.EWMA_RANGE_UCL];
                arrTempEWMA_RANGE_LCL = (ArrayList)llstParam[COLUMN.EWMA_RANGE_LCL];
                arrTempEWMA_STD_UCL = (ArrayList)llstParam[COLUMN.EWMA_STD_UCL];
                arrTempEWMA_STD_LCL = (ArrayList)llstParam[COLUMN.EWMA_STD_LCL];
                arrTempMA_UCL = (ArrayList)llstParam[COLUMN.MA_UCL];
                arrTempMA_LCL = (ArrayList)llstParam[COLUMN.MA_LCL];
                arrTempMS_UCL = (ArrayList)llstParam[COLUMN.MS_UCL];
                arrTempMS_LCL = (ArrayList)llstParam[COLUMN.MS_LCL];
                arrTempMR_UCL = (ArrayList)llstParam[COLUMN.MR_UCL];
                arrTempMR_LCL = (ArrayList)llstParam[COLUMN.MR_LCL];
                arrTempUPPER_TECHNICAL_LIMIT = (ArrayList)llstParam[COLUMN.UPPER_TECHNICAL_LIMIT];
                arrTempLOWER_TECHNICAL_LIMIT = (ArrayList)llstParam[COLUMN.LOWER_TECHNICAL_LIMIT];
                arrTempTARGET = (ArrayList)llstParam[COLUMN.TARGET];
                arrTempCENTER_LINE = (ArrayList)llstParam[COLUMN.CENTER_LINE];
                arrTempRAW_CENTER_LINE = (ArrayList)llstParam[COLUMN.RAW_CENTER_LINE];
                arrTempSTD_CENTER_LINE = (ArrayList)llstParam[COLUMN.STD_CENTER_LINE];
                arrTempRANGE_CENTER_LINE = (ArrayList)llstParam[COLUMN.RANGE_CENTER_LINE];
                arrTempEWMA_MEAN_CENTER_LINE = (ArrayList)llstParam[COLUMN.EWMA_MEAN_CENTER_LINE];
                arrTempEWMA_RANGE_CENTER_LINE = (ArrayList)llstParam[COLUMN.EWMA_RANGE_CENTER_LINE];
                arrTempEWMA_STD_CENTER_LINE = (ArrayList)llstParam[COLUMN.EWMA_STD_CENTER_LINE];
                arrTempMA_CENTER_LINE = (ArrayList)llstParam[COLUMN.MA_CENTER_LINE];
                arrTempMS_CENTER_LINE = (ArrayList)llstParam[COLUMN.MS_CENTER_LINE];
                arrTempMR_CENTER_LINE = (ArrayList)llstParam[COLUMN.MR_CENTER_LINE];
                arrTempSTD = (ArrayList)llstParam[COLUMN.STD];
                arrTempZONE_A_UCL = (ArrayList)llstParam[COLUMN.ZONE_A_UCL];
                arrTempZONE_A_LCL = (ArrayList)llstParam[COLUMN.ZONE_A_LCL];
                arrTempZONE_B_UCL = (ArrayList)llstParam[COLUMN.ZONE_B_UCL];
                arrTempZONE_B_LCL = (ArrayList)llstParam[COLUMN.ZONE_B_LCL];
                arrTempZONE_C_UCL = (ArrayList)llstParam[COLUMN.ZONE_C_UCL];
                arrTempZONE_C_LCL = (ArrayList)llstParam[COLUMN.ZONE_C_LCL];


                for (int i = 0; i < arrTempSpecRawID.Count; i++)
                {
                    llstFieldData.Clear();

                    llstFieldData.Add(COLUMN.RAWID, arrTempSpecRawID[i].ToString());
                    llstFieldData.Add(COLUMN.MAIN_YN, arrTempSpecMainYN[i].ToString());

                    //SPC-676 by Louis
                    llstFieldData.Add(COLUMN.CHART_DESCRIPTON, arrTempChartDescription[i].ToString());

                    llstFieldData.Add(COLUMN.UPPER_SPEC, arrTempUPPER_SPEC[i].ToString());
                    llstFieldData.Add(COLUMN.LOWER_SPEC, arrTempLOWER_SPEC[i].ToString());
                    llstFieldData.Add(COLUMN.UPPER_CONTROL, arrTempUPPER_CONTROL[i].ToString());
                    llstFieldData.Add(COLUMN.LOWER_CONTROL, arrTempLOWER_CONTROL[i].ToString());
                    llstFieldData.Add(COLUMN.RAW_UCL, arrTempRAW_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.RAW_LCL, arrTempRAW_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.STD_UCL, arrTempSTD_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.STD_LCL, arrTempSTD_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.RANGE_UCL, arrTempRANGE_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.RANGE_LCL, arrTempRANGE_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_M_UCL, arrTempEWMA_MEAN_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_M_LCL, arrTempEWMA_MEAN_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_R_UCL, arrTempEWMA_RANGE_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_R_LCL, arrTempEWMA_RANGE_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_S_UCL, arrTempEWMA_STD_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_S_LCL, arrTempEWMA_STD_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.MA_UCL, arrTempMA_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.MA_LCL, arrTempMA_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.MS_UCL, arrTempMS_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.MS_LCL, arrTempMS_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.MR_UCL, arrTempMR_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.MR_LCL, arrTempMR_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.UPPER_TECHNICAL_LIMIT, arrTempUPPER_TECHNICAL_LIMIT[i].ToString());
                    llstFieldData.Add(COLUMN.LOWER_TECHNICAL_LIMIT, arrTempLOWER_TECHNICAL_LIMIT[i].ToString());
                    llstFieldData.Add(COLUMN.TARGET, arrTempTARGET[i].ToString());
                    llstFieldData.Add(COLUMN.CENTER_LINE, arrTempCENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.RAW_CENTER_LINE, arrTempRAW_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.STD_CENTER_LINE, arrTempSTD_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, arrTempRANGE_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, arrTempEWMA_MEAN_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, arrTempEWMA_RANGE_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, arrTempEWMA_STD_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.MA_CENTER_LINE, arrTempMA_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.MS_CENTER_LINE, arrTempMS_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.MR_CENTER_LINE, arrTempMR_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.STD, arrTempSTD[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_A_UCL, arrTempZONE_A_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_A_LCL, arrTempZONE_A_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_B_UCL, arrTempZONE_B_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_B_LCL, arrTempZONE_B_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_C_UCL, arrTempZONE_C_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.ZONE_C_LCL, arrTempZONE_C_LCL[i].ToString());

                    llstFieldData.Add(COLUMN.SAVE_COMMENT, comment);         //spc-977 by stella
                    llstFieldData.Add(COLUMN.CHANGED_ITEMS, llstParam[COLUMN.CHANGED_ITEMS].ToString());
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    llstWhereData.Clear();
                    llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, arrTempSpecRawID[i].ToString());

                    base.Update(Definition.TableName.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    //SPC-977
                    //foreach(string query in _commondata.GetIncreaseVersionQuery(arrTempSpecRawID[i].ToString()))
                    //{
                    //    this.Query(query);
                    //}

                    if (base.ErrorMessage.Length > 0)
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                bReturn = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();

            return bReturn;
        }

        public bool SaveSpecData(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            base.BeginTrans();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.RAWID))
                {
                    arrRawID = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.RAWID];
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.INTERLOCK_YN))
                {
                    arrInterlock = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.INTERLOCK_YN];
                }

                for (int i = 0; i < arrRawID.Count; i++)
                {
                    llstFieldData.Clear();
                    if (Convert.ToBoolean(arrInterlock[i].ToString()))
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_Y);
                    }
                    else
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_N);
                    }

                    llstWhereData.Clear();
                    llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, arrRawID[i].ToString());

                    base.Update(Definition.TableName.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        bReturn = false;
                    }

                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                bReturn = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();

            return bReturn;
        }
        public bool SaveSpecMultiCalcData(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sChartType = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                llstFieldData.Clear();

                //spc-977 by stella
                string comment = llstParam[COLUMN.SAVE_COMMENT].ToString();
                string changedItems = llstParam[COLUMN.CHANGED_ITEMS].ToString();

                if (llstParam.Contains(COLUMN.UPPER_SPEC))
                {
                    llstFieldData.Add(COLUMN.UPPER_SPEC, llstParam[COLUMN.UPPER_SPEC].ToString());
                }

                if (llstParam.Contains(COLUMN.LOWER_SPEC))
                {
                    llstFieldData.Add(COLUMN.LOWER_SPEC, llstParam[COLUMN.LOWER_SPEC].ToString());
                }

                if (llstParam.Contains("CHART_TYPE"))
                {
                    sChartType = llstParam["CHART_TYPE"].ToString();
                }

                switch (sChartType)
                {
                    case "RAW":
                        if (llstParam[COLUMN.RAW_UCL].ToString() != "" || llstParam[COLUMN.RAW_LCL].ToString() != "")   //정상적인 값이 있는 경우만 저장하도록 함.
                        {
                            if (llstParam.Contains(COLUMN.RAW_UCL) && llstParam[COLUMN.RAW_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.RAW_UCL, llstParam[COLUMN.RAW_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.RAW_LCL) && llstParam[COLUMN.RAW_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.RAW_LCL, llstParam[COLUMN.RAW_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.RAW_UCL].ToString() != "" && llstParam[COLUMN.RAW_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.RAW_UCL) && llstParam.Contains(COLUMN.RAW_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.RAW_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.RAW_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.RAW_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "X-BAR":
                        if (llstParam[COLUMN.UPPER_CONTROL].ToString() != "" || llstParam[COLUMN.LOWER_CONTROL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.UPPER_CONTROL) && llstParam[COLUMN.UPPER_CONTROL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.UPPER_CONTROL, llstParam[COLUMN.UPPER_CONTROL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.LOWER_CONTROL) && llstParam[COLUMN.LOWER_CONTROL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.LOWER_CONTROL, llstParam[COLUMN.LOWER_CONTROL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.UPPER_CONTROL].ToString() != "" && llstParam[COLUMN.LOWER_CONTROL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.UPPER_CONTROL) && llstParam.Contains(COLUMN.LOWER_CONTROL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.UPPER_CONTROL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.LOWER_CONTROL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "STDDEV":
                        if (llstParam[COLUMN.STD_UCL].ToString() != "" || llstParam[COLUMN.STD_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.STD_UCL) && llstParam[COLUMN.STD_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.STD_UCL, llstParam[COLUMN.STD_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.STD_LCL) && llstParam[COLUMN.STD_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.STD_LCL, llstParam[COLUMN.STD_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.STD_UCL].ToString() != "" && llstParam[COLUMN.STD_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.STD_UCL) && llstParam.Contains(COLUMN.STD_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.STD_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.STD_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.STD_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "RANGE":
                        if (llstParam[COLUMN.RANGE_UCL].ToString() != "" || llstParam[COLUMN.RANGE_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.RANGE_UCL) && llstParam[COLUMN.RANGE_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.RANGE_UCL, llstParam[COLUMN.RANGE_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.RANGE_LCL) && llstParam[COLUMN.RANGE_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.RANGE_LCL, llstParam[COLUMN.RANGE_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.RANGE_UCL].ToString() != "" && llstParam[COLUMN.RANGE_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.RANGE_UCL) && llstParam.Contains(COLUMN.RANGE_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.RANGE_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.RANGE_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "EWMA_RANGE":
                        if (llstParam[COLUMN.EWMA_R_UCL].ToString() != "" || llstParam[COLUMN.EWMA_R_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.EWMA_R_UCL) && llstParam[COLUMN.EWMA_R_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_R_UCL, llstParam[COLUMN.EWMA_R_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.EWMA_R_LCL) && llstParam[COLUMN.EWMA_R_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_R_LCL, llstParam[COLUMN.EWMA_R_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.EWMA_R_UCL].ToString() != "" && llstParam[COLUMN.EWMA_R_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.EWMA_R_UCL) && llstParam.Contains(COLUMN.EWMA_R_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.EWMA_R_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.EWMA_R_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "EWMA_STDDEV":
                        if (llstParam[COLUMN.EWMA_S_UCL].ToString() != "" || llstParam[COLUMN.EWMA_S_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.EWMA_S_UCL) && llstParam[COLUMN.EWMA_S_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_S_UCL, llstParam[COLUMN.EWMA_S_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.EWMA_S_LCL) && llstParam[COLUMN.EWMA_S_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_S_LCL, llstParam[COLUMN.EWMA_S_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.EWMA_S_UCL].ToString() != "" && llstParam[COLUMN.EWMA_S_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.EWMA_S_UCL) && llstParam.Contains(COLUMN.EWMA_S_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.EWMA_S_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.EWMA_S_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "EWMA_MEAN":
                        if (llstParam[COLUMN.EWMA_M_UCL].ToString() != "" || llstParam[COLUMN.EWMA_M_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.EWMA_M_UCL) && llstParam[COLUMN.EWMA_M_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_M_UCL, llstParam[COLUMN.EWMA_M_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.EWMA_M_LCL) && llstParam[COLUMN.EWMA_M_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.EWMA_M_LCL, llstParam[COLUMN.EWMA_M_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.EWMA_M_UCL].ToString() != "" && llstParam[COLUMN.EWMA_M_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.EWMA_M_UCL) && llstParam.Contains(COLUMN.EWMA_M_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.EWMA_M_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.EWMA_M_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "MA":
                        if (llstParam[COLUMN.MA_UCL].ToString() != "" || llstParam[COLUMN.MA_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.MA_UCL) && llstParam[COLUMN.MA_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MA_UCL, llstParam[COLUMN.MA_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.MA_LCL) && llstParam[COLUMN.MA_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MA_LCL, llstParam[COLUMN.MA_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.MA_UCL].ToString() != "" && llstParam[COLUMN.MA_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.MA_UCL) && llstParam.Contains(COLUMN.MA_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.MA_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.MA_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.MA_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "MSD":
                        if (llstParam[COLUMN.MS_UCL].ToString() != "" || llstParam[COLUMN.MS_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.MS_UCL) && llstParam[COLUMN.MS_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MS_UCL, llstParam[COLUMN.MS_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.MS_LCL) && llstParam[COLUMN.MS_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MS_LCL, llstParam[COLUMN.MS_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.MS_UCL].ToString() != "" && llstParam[COLUMN.MS_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.MS_UCL) && llstParam.Contains(COLUMN.MS_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.MS_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.MS_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.MS_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                    case "MR":
                        if (llstParam[COLUMN.MR_UCL].ToString() != "" || llstParam[COLUMN.MR_LCL].ToString() != "")
                        {
                            if (llstParam.Contains(COLUMN.MR_UCL) && llstParam[COLUMN.MR_UCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MR_UCL, llstParam[COLUMN.MR_UCL].ToString());
                            }
                            if (llstParam.Contains(COLUMN.MR_LCL) && llstParam[COLUMN.MR_LCL].ToString() != "")
                            {
                                llstFieldData.Add(COLUMN.MR_LCL, llstParam[COLUMN.MR_LCL].ToString());
                            }
                        }
                        else if (llstParam[COLUMN.MR_UCL].ToString() != "" && llstParam[COLUMN.MR_LCL].ToString() != "")   //LCL, UCL 값이 모두 있을 경우(NaN이 아닌) center 저장.
                        {
                            if (llstParam.Contains(COLUMN.MR_UCL) && llstParam.Contains(COLUMN.MR_LCL))
                            {
                                double dUCL = Convert.ToDouble(llstParam[COLUMN.MR_UCL].ToString());
                                double dLCL = Convert.ToDouble(llstParam[COLUMN.MR_LCL].ToString());
                                double dTarget = (dUCL + dLCL) / 2;
                                llstFieldData.Add(COLUMN.MR_CENTER_LINE, dTarget);
                            }
                        }
                        break;
                }

                if (llstParam.Contains(COLUMN.UPPER_SPEC) && llstParam.Contains(COLUMN.LOWER_SPEC))
                {

                    if (llstParam[COLUMN.UPPER_SPEC].ToString() != null && llstParam[COLUMN.UPPER_SPEC].ToString() != ""
                        && llstParam[COLUMN.LOWER_SPEC].ToString() != null && llstParam[COLUMN.LOWER_SPEC].ToString() != "")
                    {
                        double dUSL = Convert.ToDouble(llstParam[COLUMN.UPPER_SPEC].ToString());
                        double dLSL = Convert.ToDouble(llstParam[COLUMN.LOWER_SPEC].ToString());

                        double dTargetSL = (dUSL + dLSL) / 2;
                        llstFieldData.Add(COLUMN.TARGET, dTargetSL.ToString());
                    }
                    else
                    {
                        llstFieldData.Add(COLUMN.TARGET, "");
                    }
                }

                llstFieldData.Add(COLUMN.SAVE_COMMENT, comment);
                llstFieldData.Add(COLUMN.CHANGED_ITEMS, changedItems);   //spc-977

                if (llstParam.Contains(Definition.CONDITION_KEY_USER_ID))
                {
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, llstParam[Definition.CONDITION_KEY_USER_ID].ToString());
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                }

                llstWhereData.Clear();
                string rawid = llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
                llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, rawid);

                base.Update(Definition.TableName.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                foreach (string s in _commondata.GetIncreaseVersionQuery(rawid))
                {
                    this.Query(s);
                }

                if (base.ErrorMessage.Length > 0)
                {
                    bReturn = false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return bReturn;

        }

        public bool SaveSpecCalcData(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sChartType = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                llstFieldData.Clear();

                string comment = llstParam[COLUMN.SAVE_COMMENT].ToString();
                string changedItems = llstParam[COLUMN.CHANGED_ITEMS].ToString();

                if (llstParam.Contains(COLUMN.UPPER_SPEC))
                {
                    llstFieldData.Add(COLUMN.UPPER_SPEC, llstParam[COLUMN.UPPER_SPEC].ToString());
                }

                if (llstParam.Contains(COLUMN.LOWER_SPEC))
                {
                    llstFieldData.Add(COLUMN.LOWER_SPEC, llstParam[COLUMN.LOWER_SPEC].ToString());
                }

                if (llstParam.Contains("CHART_TYPE"))
                {
                    sChartType = llstParam["CHART_TYPE"].ToString();
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.UCL))
                {
                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.RAW:
                            llstFieldData.Add(COLUMN.RAW_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.XBAR:
                            llstFieldData.Add(COLUMN.UPPER_CONTROL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.RANGE:
                            llstFieldData.Add(COLUMN.RANGE_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.STDDEV:
                            llstFieldData.Add(COLUMN.STD_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_MEAN:
                            llstFieldData.Add(COLUMN.EWMA_M_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_RANGE:
                            llstFieldData.Add(COLUMN.EWMA_R_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_STDDEV:
                            llstFieldData.Add(COLUMN.EWMA_S_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MA:
                            llstFieldData.Add(COLUMN.MA_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MSD:
                            llstFieldData.Add(COLUMN.MS_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MR:
                            llstFieldData.Add(COLUMN.MR_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                    }
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.LCL))
                {
                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.RAW:
                            llstFieldData.Add(COLUMN.RAW_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.XBAR:
                            llstFieldData.Add(COLUMN.LOWER_CONTROL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.RANGE:
                            llstFieldData.Add(COLUMN.RANGE_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.STDDEV:
                            llstFieldData.Add(COLUMN.STD_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_MEAN:
                            llstFieldData.Add(COLUMN.EWMA_M_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_RANGE:
                            llstFieldData.Add(COLUMN.EWMA_R_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_STDDEV:
                            llstFieldData.Add(COLUMN.EWMA_S_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MA:
                            llstFieldData.Add(COLUMN.MA_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MSD:
                            llstFieldData.Add(COLUMN.MS_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.MR:
                            llstFieldData.Add(COLUMN.MR_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                    }
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.UCL) && llstParam.Contains(Definition.CHART_COLUMN.LCL))
                {
                    double dUCL = Convert.ToDouble(llstParam[Definition.CHART_COLUMN.UCL].ToString());
                    double dLCL = Convert.ToDouble(llstParam[Definition.CHART_COLUMN.LCL].ToString());
                    double dTarget = (dUCL + dLCL) / 2;

                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.RAW:
                            llstFieldData.Add(COLUMN.RAW_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.XBAR:
                            llstFieldData.Add(COLUMN.CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.RANGE:
                            llstFieldData.Add(COLUMN.RANGE_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.STDDEV:
                            llstFieldData.Add(COLUMN.STD_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_MEAN:
                            llstFieldData.Add(COLUMN.EWMA_M_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_RANGE:
                            llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.EWMA_STDDEV:
                            llstFieldData.Add(COLUMN.EWMA_S_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.MA:
                            llstFieldData.Add(COLUMN.MA_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.MSD:
                            llstFieldData.Add(COLUMN.MS_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.MR:
                            llstFieldData.Add(COLUMN.MR_CENTER_LINE, dTarget.ToString());
                            break;
                    }

                }

                if (llstParam.Contains(COLUMN.UPPER_SPEC) && llstParam.Contains(COLUMN.LOWER_SPEC))
                {
                    double dTargetSL;

                    //spc-1219
                    if (llstParam[COLUMN.TARGET].ToString() != null && llstParam[COLUMN.TARGET].ToString() != "")
                    {
                        dTargetSL = Convert.ToDouble(llstParam[Definition.CHART_COLUMN.TARGET].ToString());
                        llstFieldData.Add(COLUMN.TARGET, dTargetSL.ToString());
                    }
                    else if (llstParam[COLUMN.UPPER_SPEC].ToString() != null && llstParam[COLUMN.UPPER_SPEC].ToString() != ""
                        && llstParam[COLUMN.LOWER_SPEC].ToString() != null && llstParam[COLUMN.LOWER_SPEC].ToString() != "")
                    {
                        double dUSL = Convert.ToDouble(llstParam[COLUMN.UPPER_SPEC].ToString());
                        double dLSL = Convert.ToDouble(llstParam[COLUMN.LOWER_SPEC].ToString());

                        dTargetSL = (dUSL + dLSL) / 2;
                        llstFieldData.Add(COLUMN.TARGET, dTargetSL.ToString());
                    }
                    else
                    {
                        llstFieldData.Add(COLUMN.TARGET, "");
                    }
                }

                llstFieldData.Add(COLUMN.SAVE_COMMENT, comment);
                llstFieldData.Add(COLUMN.CHANGED_ITEMS, changedItems);   //spc-977

                if (llstParam.Contains(Definition.CONDITION_KEY_USER_ID))
                {
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, llstParam[Definition.CONDITION_KEY_USER_ID].ToString());
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                }

                llstWhereData.Clear();
                string rawid = llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
                llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, rawid);

                base.Update(Definition.TableName.MODEL_CONFIG_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                foreach (string s in _commondata.GetIncreaseVersionQuery(rawid))
                {
                    this.Query(s);
                }

                if (base.ErrorMessage.Length > 0)
                {
                    bReturn = false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return bReturn;
        }

        private const string SQL_UPDATE_MODEL_CONFIG_MST_SPC_FOR_SUBCONFIG = " UPDATE MODEL_CONFIG_MST_SPC                                                     "
                                                                           + "    SET (UPPER_SPEC, LOWER_SPEC, TARGET, UPPER_CONTROL, LOWER_CONTROL,           "
                                                                           + "         EWMA_LAMBDA, MOVING_COUNT, CENTER_LINE, LAST_UPDATE_DTTS,               "
                                                                           + "         LAST_UPDATE_BY, STD, RAW_UCL, RAW_LCL, RAW_CENTER_LINE, STD_UCL,        "
                                                                           + "         STD_LCL, STD_CENTER_LINE, RANGE_UCL, RANGE_LCL, RANGE_CENTER_LINE,      "
                                                                           + "         EWMA_M_UCL, EWMA_M_LCL, EWMA_M_CENTER_LINE, EWMA_R_UCL, EWMA_R_LCL,     "
                                                                           + "         EWMA_R_CENTER_LINE, EWMA_S_UCL, EWMA_S_LCL, EWMA_S_CENTER_LINE,         "
                                                                           + "         MA_UCL, MA_LCL, MA_CENTER_LINE, MS_UCL, MS_LCL, MS_CENTER_LINE, MR_UCL, MR_LCL, MR_CENTER_LINE,         "
                                                                           + "         UPPER_TECHNICAL_LIMIT, LOWER_TECHNICAL_LIMIT,                           "
                                                                           + "         ZONE_A_UCL, ZONE_A_LCL, ZONE_B_UCL, ZONE_B_LCL, ZONE_C_UCL,             "
                                                                           + "         ZONE_C_LCL,spec_usl_offset, spec_lsl_offset,                            "
                                                                           + "         mean_ucl_offset, mean_lcl_offset, raw_ucl_offset,                       "
                                                                           + "         raw_lcl_offset, std_ucl_offset, std_lcl_offset,                         "
                                                                           + "         range_ucl_offset, range_lcl_offset,                                     "
                                                                           + "         ewma_m_ucl_offset, ewma_m_lcl_offset,                                   "
                                                                           + "         ewma_r_ucl_offset, ewma_r_lcl_offset,                                   "
                                                                           + "         ewma_s_ucl_offset, ewma_s_lcl_offset, ma_ucl_offset,                    "
                                                                           + "         ma_lcl_offset, mr_ucl_offset, mr_lcl_offset,                            "
                                                                           + "         ms_ucl_offset, ms_lcl_offset,                                           "
                                                                           + "         zone_a_ucl_offset, zone_a_lcl_offset,                                   "
                                                                           + "         zone_b_ucl_offset, zone_b_lcl_offset,                                   "
                                                                           + "         zone_c_ucl_offset, zone_c_lcl_offset,                                   "
                                                                           + "         upper_filter, lower_filter, offset_yn,                                   "
                                                                           + "         save_comment, changed_items) =                                          "
                                                                           + "           (SELECT UPPER_SPEC, LOWER_SPEC, TARGET, UPPER_CONTROL,                "
                                                                           + "                   LOWER_CONTROL, EWMA_LAMBDA, MOVING_COUNT, CENTER_LINE,        "
                                                                           + "                   LAST_UPDATE_DTTS, LAST_UPDATE_BY, STD, RAW_UCL, RAW_LCL,      "
                                                                           + "                   RAW_CENTER_LINE, STD_UCL, STD_LCL, STD_CENTER_LINE,           "
                                                                           + "                   RANGE_UCL, RANGE_LCL, RANGE_CENTER_LINE, EWMA_M_UCL,          "
                                                                           + "                   EWMA_M_LCL, EWMA_M_CENTER_LINE, EWMA_R_UCL, EWMA_R_LCL,       "
                                                                           + "                   EWMA_R_CENTER_LINE, EWMA_S_UCL, EWMA_S_LCL,                   "
                                                                           + "                   EWMA_S_CENTER_LINE, MA_UCL, MA_LCL, MA_CENTER_LINE, MS_UCL,   "
                                                                           + "                   MS_LCL, MS_CENTER_LINE, MR_UCL, MR_LCL, MR_CENTER_LINE, UPPER_TECHNICAL_LIMIT, LOWER_TECHNICAL_LIMIT, ZONE_A_UCL, ZONE_A_LCL, ZONE_B_UCL,   "
                                                                           + "                   ZONE_B_LCL, ZONE_C_UCL, ZONE_C_LCL,                           "
                                                                           + "                   spec_usl_offset, spec_lsl_offset,                             "
                                                                           + "                   mean_ucl_offset, mean_lcl_offset,                             "
                                                                           + "                   raw_ucl_offset, raw_lcl_offset,                               "
                                                                           + "                   std_ucl_offset, std_lcl_offset,                               "
                                                                           + "                   range_ucl_offset, range_lcl_offset,                           "
                                                                           + "                   ewma_m_ucl_offset, ewma_m_lcl_offset,                         "
                                                                           + "                   ewma_r_ucl_offset, ewma_r_lcl_offset,                         "
                                                                           + "                   ewma_s_ucl_offset, ewma_s_lcl_offset,                         "
                                                                           + "                   ma_ucl_offset, ma_lcl_offset, mr_ucl_offset,                  "
                                                                           + "                   mr_lcl_offset, ms_ucl_offset, ms_lcl_offset,                  "
                                                                           + "                   zone_a_ucl_offset, zone_a_lcl_offset,                         "
                                                                           + "                   zone_b_ucl_offset, zone_b_lcl_offset,                         "
                                                                           + "                   zone_c_ucl_offset, zone_c_lcl_offset,                         "
                                                                           + "                   upper_filter, lower_filter, offset_yn,                         "
                                                                           + "                   save_comment, changed_items                                   "
                                                                           + "              FROM MODEL_CONFIG_MST_SPC                                          "
                                                                           + "             WHERE RAWID = :RAWID AND MAIN_YN = :MAIN_MAIN)                      "
                                                                           + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                                        "
                                                                           + "                         FROM MODEL_CONFIG_MST_SPC                               "
                                                                           + "                        WHERE RAWID = :RAWID AND MAIN_YN = :MAIN_MAIN) AND MAIN_YN = :MAIN_SUB AND INHERIT_MAIN_YN = :INHERIT_YN ";

        private const string SQL_DELETE_MODEL_RULE_OPT_MST_SPC_FOR_SUBCONFIG = " DELETE MODEL_RULE_OPT_MST_SPC                                            "
                                                                            + "  WHERE MODEL_RULE_RAWID IN (                                             "
                                                                            + "           SELECT RAWID                                                   "
                                                                            + "             FROM MODEL_RULE_MST_SPC                                      "
                                                                            + "            WHERE MODEL_CONFIG_RAWID IN (                                 "
                                                                            + "                     SELECT RAWID                                         "
                                                                            + "                       FROM MODEL_CONFIG_MST_SPC                          "
                                                                            + "                      WHERE MODEL_RAWID = (SELECT MODEL_RAWID             "
                                                                            + "                                             FROM MODEL_CONFIG_MST_SPC    "
                                                                            + "                                            WHERE RAWID = :RAWID)         "
                                                                            + "                        AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y'))     ";


        private const string SQL_DELETE_MODEL_RULE_MST_SPC_FOR_SUBCONFIG = " DELETE      MODEL_RULE_MST_SPC                                   "
                                                                        + "       WHERE MODEL_CONFIG_RAWID IN (                               "
                                                                        + "                SELECT RAWID                                       "
                                                                        + "                  FROM MODEL_CONFIG_MST_SPC                        "
                                                                        + "                 WHERE MODEL_RAWID = (SELECT MODEL_RAWID           "
                                                                        + "                                        FROM MODEL_CONFIG_MST_SPC  "
                                                                        + "                                       WHERE RAWID = :RAWID)       "
                                                                        + "                   AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y')    ";


        public readonly string SQL_SELECT_SUB_CONFIG_RAWID = " SELECT RAWID                                              "
                                                            + "   FROM MODEL_CONFIG_MST_SPC                               "
                                                            + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                  "
                                                            + "                         FROM MODEL_CONFIG_MST_SPC         "
                                                            + "                        WHERE RAWID = :RAWID) AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y' ";





        public DataSet ModifySPCSubModel(string sConfigRawID, DataTable dtRule, DataTable dtRuleOpt, string sUserID, ref string sConfigRawid)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;
            string[] sArrModelConfigRawID = null;

            try
            {
                DateTime dt = base.GetDBTimeStamp();

                //#01. MODEL_CONFIG_MST_SPC
                llstWhereData.Clear();
                llstWhereData.Add("RAWID", sConfigRawID);
                llstWhereData.Add("MAIN_MAIN", "Y");
                llstWhereData.Add("MAIN_SUB", "N");
                llstWhereData.Add("INHERIT_YN", "Y");

                dsResult = base.Query(SQL_UPDATE_MODEL_CONFIG_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //#02. MODEL_RULE_MST_SPC, MODEL_RULE_OPT_MST_SPC DATA 삭제하고 Main config rawid 기준으로 Insert 한다.
                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_OPT_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //sub config의 model_config_rawid를 구한다.

                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_SELECT_SUB_CONFIG_RAWID, llstWhereData);

                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    ArrayList arrTemp = new ArrayList();
                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                    {
                        arrTemp.Add(dsResult.Tables[0].Rows[i][COLUMN.RAWID].ToString());
                    }

                    sArrModelConfigRawID = (string[])arrTemp.ToArray(typeof(string));
                }

                if (sArrModelConfigRawID != null && sArrModelConfigRawID.Length > 0)
                {
                    ArrayList arrRawID = new ArrayList();
                    ArrayList arrModelConfigRawID = new ArrayList();
                    ArrayList arrSPCRuleNo = new ArrayList();
                    ArrayList arrOCAPList = new ArrayList();
                    ArrayList arrUseMainSpec = new ArrayList();
                    ArrayList arrRuleOrder = new ArrayList();
                    ArrayList arrCreateBy = new ArrayList();
                    ArrayList arrCreateDtts = new ArrayList();

                    ArrayList arrRuleOptRawID = new ArrayList();
                    ArrayList arrRuleRawID = new ArrayList();
                    ArrayList arrRuleOptNo = new ArrayList();
                    ArrayList arrRuleOptValue = new ArrayList();
                    ArrayList arrRuleOptCreateBy = new ArrayList();
                    ArrayList arrRuleOptCreateDtts = new ArrayList();

                    decimal kTemp = 0;
                    decimal jTemp = 0;
                    decimal ruleRawID = 0;
                    decimal ruleOptRawID = 0;

                    for (int i = 0; i < sArrModelConfigRawID.Length; i++)
                    {
                        //#07. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            if (kTemp == 0)
                            {
                                ruleRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_RULE_MST_SPC, 500);
                            }


                            arrRawID.Add(ruleRawID.ToString());
                            arrModelConfigRawID.Add(sArrModelConfigRawID[i]);
                            arrSPCRuleNo.Add(drRule[COLUMN.SPC_RULE_NO].ToString());
                            arrOCAPList.Add(drRule[COLUMN.OCAP_LIST].ToString());
                            arrUseMainSpec.Add(drRule[COLUMN.USE_MAIN_SPEC_YN].ToString());
                            arrRuleOrder.Add(drRule[COLUMN.RULE_ORDER].ToString());
                            arrCreateBy.Add(sUserID);
                            arrCreateDtts.Add(dt);



                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID),
                                                                    COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                if (jTemp == 0)
                                {
                                    ruleOptRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC, 500);
                                }

                                arrRuleOptRawID.Add(ruleOptRawID.ToString());
                                arrRuleRawID.Add(ruleRawID.ToString());
                                arrRuleOptNo.Add(drRuleOpt[COLUMN.RULE_OPTION_NO].ToString());
                                arrRuleOptValue.Add(drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString());
                                arrRuleOptCreateBy.Add(sUserID);
                                arrRuleOptCreateDtts.Add(dt);

                                ruleOptRawID++;
                                jTemp++;
                                if (jTemp == 500)
                                    jTemp = 0;

                            }

                            ruleRawID++;
                            kTemp++;
                            if (kTemp == 500)
                                kTemp = 0;
                        }
                        sConfigRawid += ";" + sArrModelConfigRawID[i];
                    }

                    llstFieldData.Clear();

                    if (arrRawID.Count > 0)
                    {
                        llstFieldData.Add(COLUMN.RAWID, (string[])arrRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, (string[])arrModelConfigRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.SPC_RULE_NO, (string[])arrSPCRuleNo.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.OCAP_LIST, (string[])arrOCAPList.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, (string[])arrUseMainSpec.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_ORDER, (string[])arrRuleOrder.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrCreateBy.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrCreateDtts.ToArray(typeof(DateTime)));

                        base.InsertBatch(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, (string[])arrRuleOptRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, (string[])arrRuleRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_OPTION_NO, (string[])arrRuleOptNo.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, (string[])arrRuleOptValue.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrRuleOptCreateBy.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrRuleOptCreateDtts.ToArray(typeof(DateTime)));

                        base.InsertBatch(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }


        private const string SQL_DELETE_SUB_CONTEXT_KEY = @"DELETE FROM MODEL_CONTEXT_MST_SPC
                                                            WHERE MODEL_CONFIG_RAWID IN 
                                                            ( 
                                                            SELECT RAWID FROM MODEL_CONFIG_MST_SPC
                                                            WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC
                                                            WHERE RAWID = :RAWID)
                                                            AND MAIN_YN = 'N'
                                                            )
                                                            AND CONTEXT_KEY NOT IN (SELECT CONTEXT_KEY FROM MODEL_CONTEXT_MST_SPC
                                                            WHERE MODEL_CONFIG_RAWID = :RAWID)";


        private const string SQL_UPDATE_SUB_CONTEXT_KEY_ORDER = @"UPDATE MODEL_CONTEXT_MST_SPC A
                                                                    SET A.KEY_ORDER = (SELECT KEY_ORDER FROM MODEL_CONTEXT_MST_SPC B
                                                                    WHERE MODEL_CONFIG_RAWID = :RAWID AND A.CONTEXT_KEY = B.CONTEXT_KEY)
                                                                    WHERE A.MODEL_CONFIG_RAWID IN 
                                                                    ( 
                                                                    SELECT RAWID FROM MODEL_CONFIG_MST_SPC
                                                                    WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC
                                                                    WHERE RAWID = :RAWID)
                                                                    AND MAIN_YN = 'N'
                                                                    )";

        private const string SQL_UPDATE_SUB_CONTEXT_VALUE_GROUP = @"UPDATE MODEL_CONTEXT_MST_SPC A
                                                                    SET (A.CONTEXT_VALUE, A.EXCLUDE_LIST) = (SELECT CONTEXT_VALUE, EXCLUDE_LIST FROM MODEL_CONTEXT_MST_SPC B
                                                                    WHERE MODEL_CONFIG_RAWID = :RAWID AND A.CONTEXT_KEY = B.CONTEXT_KEY AND B.GROUP_YN = 'Y'
                                                                    )
                                                                    WHERE A.MODEL_CONFIG_RAWID IN 
                                                                    ( 
                                                                    SELECT RAWID FROM MODEL_CONFIG_MST_SPC
                                                                    WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC
                                                                    WHERE RAWID = :RAWID)
                                                                    AND MAIN_YN = 'N'
                                                                    )
                                                                    AND A.CONTEXT_KEY in ({0})";

        private const string SQL_SELECT_SUB_CONFIG_RAWID_FOR_CONTEXT_SAVE = " SELECT RAWID                                              "
                                                            + "   FROM MODEL_CONFIG_MST_SPC                               "
                                                            + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                  "
                                                            + "                         FROM MODEL_CONFIG_MST_SPC         "
                                                            + "                        WHERE RAWID = :RAWID) AND MAIN_YN = 'N' ";


        public DataSet ModifySPCSubModelContext(string sConfigRawID, DataTable dtContext, string sUserID, bool isOnlyMainGroup, string groupRawid)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;
            bool bGroupAdded = false;

            try
            {

                DateTime dt = base.GetDBTimeStamp();

                //delete 한 경우는 sub의 context도 지우고 key_order도 맞춰준다.
                llstWhereData.Clear();
                llstWhereData.Add(COLUMN.RAWID, sConfigRawID);
                dsResult = base.Query(SQL_DELETE_SUB_CONTEXT_KEY, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                dsResult = base.Query(SQL_UPDATE_SUB_CONTEXT_KEY_ORDER, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //isonlymain = false 이고 Group이 추가된 경우는 전 sub model에 insert 한다.
                //isonlymain = false 이고 기존 Grouping된 Data가 Modify된 경우는 전 sub model을 Modify 한다.
                if (!isOnlyMainGroup)
                {
                    DataRow[] drArrTempModify = dtContext.Select("_MODIFY = 'True' and group_yn = 'True'");

                    if (drArrTempModify.Length > 0)
                    {
                        string strConTextKey = "";

                        for (int i = 0; i < drArrTempModify.Length; i++)
                        {
                            strConTextKey += ",'" + drArrTempModify[i][COLUMN.CONTEXT_KEY].ToString() + "'";
                        }

                        strConTextKey = strConTextKey.Substring(1);

                        dsResult = base.Query(string.Format(SQL_UPDATE_SUB_CONTEXT_VALUE_GROUP, strConTextKey), llstWhereData);
                    }


                    DataRow[] drArrTemp = dtContext.Select("_INSERT = 'True' and group_yn = 'True'");
                    if (drArrTemp.Length > 0)
                    {
                        DataSet dsTemp = base.Query(SQL_SELECT_SUB_CONFIG_RAWID_FOR_CONTEXT_SAVE, llstWhereData);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            llstFieldData.Clear();
                            ArrayList arrTempRawID = new ArrayList();
                            ArrayList arrTempModelConfigRawID = new ArrayList();
                            ArrayList arrTempContextKey = new ArrayList();
                            ArrayList arrTempContextValue = new ArrayList();
                            ArrayList arrTempExcludeList = new ArrayList();
                            ArrayList arrTempKeyOrder = new ArrayList();
                            ArrayList arrTempGroupYN = new ArrayList();
                            ArrayList arrTempCreateDtts = new ArrayList();
                            ArrayList arrTempCreateBy = new ArrayList();

                            decimal ddxRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_CONTEXT_MST_SPC, dsTemp.Tables[0].Rows.Count * drArrTemp.Length);

                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < drArrTemp.Length; j++)
                                {
                                    arrTempRawID.Add(ddxRawID.ToString());
                                    arrTempModelConfigRawID.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                                    arrTempContextKey.Add(drArrTemp[j][COLUMN.CONTEXT_KEY]);
                                    arrTempContextValue.Add(drArrTemp[j][COLUMN.CONTEXT_VALUE]);
                                    arrTempExcludeList.Add(drArrTemp[j][COLUMN.EXCLUDE_LIST].ToString());
                                    arrTempKeyOrder.Add(drArrTemp[j][COLUMN.KEY_ORDER].ToString());
                                    arrTempGroupYN.Add(Definition.VARIABLE_Y);
                                    arrTempCreateBy.Add(sUserID);
                                    arrTempCreateDtts.Add(dt);

                                    ddxRawID++;
                                }
                            }

                            if (arrTempRawID.Count > 0)
                            {
                                llstFieldData.Clear();

                                llstFieldData.Add(COLUMN.RAWID, (string[])arrTempRawID.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, (string[])arrTempModelConfigRawID.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CONTEXT_KEY, (string[])arrTempContextKey.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CONTEXT_VALUE, (string[])arrTempContextValue.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.EXCLUDE_LIST, (string[])arrTempExcludeList.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.KEY_ORDER, (string[])arrTempKeyOrder.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.GROUP_YN, (string[])arrTempGroupYN.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrTempCreateBy.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrTempCreateDtts.ToArray(typeof(DateTime)));

                                base.InsertBatch(TABLE.MODEL_CONTEXT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    return dsResult;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }


        private const string SQL_SELECT_GROUP_CONTEXT_VALUE = " SELECT CONTEXT_KEY, CONTEXT_VALUE                         "
                                                            + "   FROM MODEL_CONTEXT_MST_SPC                              "
                                                            + "  WHERE MODEL_CONFIG_RAWID = (SELECT RAWID                 "
                                                            + "                         FROM MODEL_CONFIG_MST_SPC         "
                                                            + "                        WHERE MODEL_RAWID = (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) "
                                                            + "                        AND MAIN_YN = 'Y') AND GROUP_YN = 'Y' ";

        public DataSet GetGroupContextValue(byte[] param)
        {
            DataSet dsReturn = new DataSet();


            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                dsReturn = base.Query(SQL_SELECT_GROUP_CONTEXT_VALUE, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

                if (dsReturn.Tables.Count > 0)
                    dsReturn.Tables[0].TableName = "GROUP_SPC";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }


        //Chris :: 2010-06-15  MCMS.RAW_UCL, MCMS.RAW_LCL 추가

        //이전
        /*private const string SQL_SEARCH_BY_AREA = "SELECT  AMP.AREA, MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, '' EQP_ID_CONTEXT, '' MODULE_ID_CONTEXT, '' RECIPE_ID_CONTEXT, '' STEP_ID_CONTEXT, " +
            " MCMS.PARAM_ALIAS, MCMS.PARAM_TYPE_CD, MCMS.MAIN_YN, MCMS.INTERLOCK_YN,  " +
            " MCMS.UPPER_SPEC, MCMS.LOWER_SPEC, MCMS.UPPER_CONTROL, MCMS.LOWER_CONTROL, MCMS.RAW_UCL, MCMS.RAW_LCL, MCMS.SAMPLE_COUNT, MCMS.EWMA_LAMBDA, MCMS.MOVING_COUNT, " +
            " MCMS.AUTOCALC_YN, MCMS.TARGET, MCOMS.DEFAULT_CHART_LIST, MCOMS.MODEL_CONFIG_RAWID   " +
            " FROM    MODEL_MST_SPC MMS " +
            " INNER JOIN MODEL_CONFIG_MST_SPC MCMS ON MMS.RAWID = MCMS.MODEL_RAWID " +
            " INNER JOIN AREA_MST_PP AMP ON MMS.AREA_RAWID = AMP.RAWID " +
            " INNER JOIN MODEL_CONFIG_OPT_MST_SPC MCOMS ON MCMS.RAWID =MCOMS.MODEL_CONFIG_RAWID " +
            " WHERE   MMS.AREA_RAWID = :AREA ";// +
        //" ORDER BY AMP.AREA, MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, MCMS.PARAM_ALIAS ";*/

        //SPC Rule, Ocap List 추가
        private const string SQL_SEARCH_BY_AREA = @"SELECT MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, '' EQP_ID_CONTEXT,
                                                   '' MODULE_ID_CONTEXT, '' RECIPE_ID_CONTEXT, '' STEP_ID_CONTEXT,
                                                   MCMS.PARAM_ALIAS, MCMS.MAIN_YN, MCMS.UPPER_SPEC, MCMS.LOWER_SPEC,
                                                   MCMS.UPPER_CONTROL, MCMS.LOWER_CONTROL, MCMS.RAW_UCL, MCMS.RAW_LCL,
                                                   MCMS.STD_UCL, MCMS.STD_LCL, MCMS.MS_UCL, MCMS.MS_LCL, MCMS.AUTOCALC_YN,
                                                   MRMS.SPC_RULE_NO, MRMS.OCAP_LIST, MCMS.INTERLOCK_YN,
                                                   MCMS.ACTIVATION_YN, MCMS.CREATE_BY, MCMS.CREATE_DTTS,
                                                   MCMS.LAST_UPDATE_BY, MCMS.LAST_UPDATE_DTTS, MCOMS.MODEL_CONFIG_RAWID
                                              FROM MODEL_MST_SPC MMS,
                                                   MODEL_CONFIG_MST_SPC MCMS,
                                                   MODEL_CONFIG_OPT_MST_SPC MCOMS,
                                                   (SELECT   MODEL_CONFIG_RAWID, WM_CONCAT (SPC_RULE_NO) SPC_RULE_NO,
                                                             WM_CONCAT (OCAP_LIST) OCAP_LIST
                                                        FROM MODEL_RULE_MST_SPC
                                                       WHERE MODEL_CONFIG_RAWID IN (
                                                                            SELECT RAWID
                                                                              FROM MODEL_CONFIG_MST_SPC
                                                                             WHERE MODEL_RAWID IN (
                                                                                                   SELECT RAWID
                                                                                                     FROM MODEL_MST_SPC
                                                                                                    WHERE AREA_RAWID =
                                                                                                                     :AREA))
                                                    GROUP BY MODEL_CONFIG_RAWID) MRMS
                                             WHERE MMS.RAWID = MCMS.MODEL_RAWID
                                               AND MCMS.RAWID = MCOMS.MODEL_CONFIG_RAWID
                                               AND MCMS.RAWID = MRMS.MODEL_CONFIG_RAWID(+)
                                               AND MMS.AREA_RAWID = :AREA ";

        private const string SQL_SEARCH_BY_AREA_CONTEXT_KEY_VALUE = " SELECT  MCMS2.MODEL_CONFIG_RAWID, MCMS2.CONTEXT_KEY, MCMS2.CONTEXT_VALUE " +
            " FROM  MODEL_MST_SPC MMS " +
            "       INNER JOIN MODEL_CONFIG_MST_SPC MCMS ON MMS.RAWID = MCMS.MODEL_RAWID " +
            "       INNER JOIN MODEL_CONTEXT_MST_SPC MCMS2 ON MCMS.RAWID = MCMS2.MODEL_CONFIG_RAWID " +
            " WHERE   MMS.AREA_RAWID = :AREA";

        public DataSet SearchByArea(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sbQuery = new StringBuilder();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                string sAreaRawID = llstParam[Definition.DynamicCondition_Condition_key.AREA].ToString();

                sbQuery.Append(SQL_SEARCH_BY_AREA);

                LinkedList llstWhereData = new LinkedList();
                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.AREA))
                {
                    llstWhereData.Add("AREA", sAreaRawID);
                }
                else
                {
                    return null;
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_SPC_MODEL_NAME))
                {
                    string sSPCModelName = llstParam[Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();
                    sbQuery.Append(" AND MMS.SPC_MODEL_NAME IN ( " + sSPCModelName + " ) ");
                }

                sbQuery.Append(" ORDER BY MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, MCMS.PARAM_ALIAS ");

                DataSet dsTemp = base.Query(sbQuery.ToString(), llstWhereData);

                if (dsTemp != null && dsTemp.Tables.Count > 0)
                {
                    DataTable dtExcel = new DataTable();
                    for (int idx = 0; idx < dsTemp.Tables[0].Columns.Count; idx++)
                    {
                        string sCol = dsTemp.Tables[0].Columns[idx].ColumnName.ToString().ToUpper();
                        dtExcel.Columns.Add(sCol, typeof(string));
                    }

                    foreach (DataRow dr in dsTemp.Tables[0].Rows)
                    {
                        dtExcel.ImportRow(dr);
                    }

                    dsReturn.Tables.Add("MODEL_INFO");
                    dsReturn.Tables["MODEL_INFO"].Merge(dtExcel);
                }

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return null;
                }

                DataSet dsContext = base.Query(SQL_SEARCH_BY_AREA_CONTEXT_KEY_VALUE, llstWhereData);

                dsReturn.Tables.Add("CONTEXT_TABLE");
                dsReturn.Tables["CONTEXT_TABLE"].Merge(dsContext.Tables[0]);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return null;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        private const string SQL_SEARCH_BY_AREA_SPC_MODEL_NAME =
@"SELECT   spc_model_name
    FROM model_mst_spc
   WHERE area_rawid = :area
GROUP BY spc_model_name
ORDER BY spc_model_name";

        public DataSet SearchByAreaSPCModelName(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                string sAreaRawID = llstParam[Definition.DynamicCondition_Condition_key.AREA].ToString();

                LinkedList llstWhereData = new LinkedList();
                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.AREA))
                {
                    llstWhereData.Add("AREA", sAreaRawID);
                }
                else
                {
                    return null;
                }

                dsReturn = base.Query(SQL_SEARCH_BY_AREA_SPC_MODEL_NAME, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return null;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        ///// <summary>
        ///// SPC Model 별 Search기능 추가
        ///// Chris
        ///// </summary>
        //private const string SQL_SEARCH_BY_AREA_AND_SPCMODELNAME = "SELECT  AMP.AREA, MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, '' EQP_ID_CONTEXT, '' MODULE_ID_CONTEXT, '' RECIPE_ID_CONTEXT, '' STEP_ID_CONTEXT, " +
        //    " MCMS.PARAM_ALIAS, MCMS.PARAM_TYPE_CD, MCMS.MAIN_YN, MCMS.INTERLOCK_YN,  " +
        //    " MCMS.UPPER_SPEC, MCMS.LOWER_SPEC, MCMS.UPPER_CONTROL, MCMS.LOWER_CONTROL, MCMS.RAW_UCL, MCMS.RAW_LCL, MCMS.SAMPLE_COUNT, MCMS.EWMA_LAMBDA, MCMS.MOVING_COUNT, " +
        //    " MCMS.AUTOCALC_YN, MCMS.TARGET, MCOMS.DEFAULT_CHART_LIST, MCOMS.MODEL_CONFIG_RAWID   " +
        //    " FROM    MODEL_MST_SPC MMS " +
        //    " INNER JOIN MODEL_CONFIG_MST_SPC MCMS ON MMS.RAWID = MCMS.MODEL_RAWID " +
        //    " INNER JOIN AREA_MST_PP AMP ON MMS.AREA_RAWID = AMP.RAWID " +
        //    " INNER JOIN MODEL_CONFIG_OPT_MST_SPC MCOMS ON MCMS.RAWID =MCOMS.MODEL_CONFIG_RAWID " +
        //    " WHERE   MMS.AREA_RAWID = :AREA ";// +
        ////" ORDER BY AMP.AREA, MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, MCMS.PARAM_ALIAS ";

        //private const string SQL_SEARCH_BY_AREA_AND_SPCMODELNAME_CONTEXT_KEY_VALUE = " SELECT  MCMS2.MODEL_CONFIG_RAWID, MCMS2.CONTEXT_KEY, MCMS2.CONTEXT_VALUE " +
        //    " FROM  MODEL_MST_SPC MMS " +
        //    "       INNER JOIN MODEL_CONFIG_MST_SPC MCMS ON MMS.RAWID = MCMS.MODEL_RAWID " +
        //    "       INNER JOIN MODEL_CONTEXT_MST_SPC MCMS2 ON MCMS.RAWID = MCMS2.MODEL_CONFIG_RAWID " +
        //    " WHERE   MMS.AREA_RAWID = :AREA";

        //public DataSet SearchByAreaAndSPCModelName(byte[] param)
        //{
        //    DataSet dsReturn = new DataSet();

        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        LinkedList llstParam = new LinkedList();
        //        llstParam.SetSerialData(param);

        //        string sAreaRawID = llstParam[Definition.DynamicCondition_Condition_key.AREA].ToString();
        //        string sSPCModelName = llstParam[Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();

        //        LinkedList llstWhereData = new LinkedList();
        //        if (llstParam.Contains(Definition.DynamicCondition_Condition_key.AREA))
        //        {
        //            sb.Append(SQL_SEARCH_BY_AREA_AND_SPCMODELNAME);
        //            llstWhereData.Add("AREA", sAreaRawID);
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        if (llstParam.Contains(Definition.CONDITION_KEY_SPC_MODEL_NAME))
        //        {
        //            sb.Append(" AND MMS.SPC_MODEL_NAME IN ( " + sSPCModelName + " )");
        //        }

        //        sb.Append(" ORDER BY AMP.AREA, MMS.EQP_MODEL, MMS.SPC_MODEL_NAME, MCMS.PARAM_ALIAS ");

        //        dsReturn = base.Query(sb.ToString(), llstWhereData);

        //        if (base.ErrorMessage.Length > 0)
        //        {
        //            DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
        //            return null;
        //        }

        //        sb = new StringBuilder();

        //        if (llstParam.Contains(Definition.CONDITION_KEY_SPC_MODEL_NAME))
        //        {
        //            sb.Append(SQL_SEARCH_BY_AREA_AND_SPCMODELNAME_CONTEXT_KEY_VALUE);
        //            sb.Append(" AND MMS.SPC_MODEL_NAME IN ( " + sSPCModelName + " ) ");
        //        }


        //        DataSet dsContext = base.Query(sb.ToString(), llstWhereData);

        //        dsReturn.Tables.Add("CONTEXT_TABLE");
        //        dsReturn.Tables["CONTEXT_TABLE"].Merge(dsContext.Tables[0]);

        //        if (base.ErrorMessage.Length > 0)
        //        {
        //            DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
        //    }

        //    return dsReturn;
        //}

        ////<--여기까지SPC Model 별 Search기능 추가

        private const string SQL_SEARCH_CONTEXT_KEY = " SELECT DISTINCT CONTEXT_KEY, CONTEXT_VALUE                                 "
                                                    + "            FROM MODEL_CONTEXT_MST_SPC                                      "
                                                    + "           WHERE MODEL_CONFIG_RAWID IN (                                    "
                                                    + "                    SELECT A.MODEL_CONFIG_RAWID                             "
                                                    + "                      FROM (SELECT MODEL_CONFIG_RAWID                       "
                                                    + "                              FROM MODEL_CONTEXT_MST_SPC                    "
                                                    + "                             WHERE CONTEXT_KEY = 'EQP_ID' {0}) A,               "
                                                    + "                           (SELECT MODEL_CONFIG_RAWID                       "
                                                    + "                              FROM MODEL_CONTEXT_MST_SPC                    "
                                                    + "                             WHERE CONTEXT_KEY = 'MODULE_ID' {1}) B,            "
                                                    + "                           (SELECT MODEL_CONFIG_RAWID                       "
                                                    + "                              FROM MODEL_CONTEXT_MST_SPC                    "
                                                    + "                             WHERE CONTEXT_KEY = 'RECIPE_ID' {2}) C,            "
                                                    + "                           (SELECT MODEL_CONFIG_RAWID                       "
                                                    + "                              FROM MODEL_CONTEXT_MST_SPC                    "
                                                    + "                             WHERE CONTEXT_KEY = 'STEP_ID' {3}) D               "
                                                    + "                     WHERE A.MODEL_CONFIG_RAWID = B.MODEL_CONFIG_RAWID      "
                                                    + "                       AND A.MODEL_CONFIG_RAWID = C.MODEL_CONFIG_RAWID      "
                                                    + "                       AND A.MODEL_CONFIG_RAWID = D.MODEL_CONFIG_RAWID)     "
                                                    + "             AND MODEL_CONFIG_RAWID IN (                                    "
                                                    + "                    SELECT RAWID                                            "
                                                    + "                      FROM MODEL_CONFIG_MST_SPC                             "
                                                    + "                     WHERE MODEL_RAWID IN (                                 "
                                                    + "                              SELECT RAWID                                  "
                                                    + "                                FROM MODEL_MST_SPC                          "
                                                    + "                               WHERE LOCATION_RAWID = :LINE                 "
                                                    + "                                     AND AREA_RAWID = :AREA                 "
                                                    + "                                     AND NVL(EQP_MODEL, '-') = :EQP_MODEL)  "
                                                    + "                       AND MAIN_YN = 'N') {4}                               "
                                                    + "                       ORDER BY CONTEXT_KEY, CONTEXT_VALUE                  ";

        public DataSet GetSPCCalcContextKey(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            string sSQL = "";
            string strSubQueryEQP = "";
            string strSubQueryModule = "";
            string strSubQueryRecipe = "";
            string strSubQueryStep = "";
            string strSubQueryEnd = "";

            try
            {
                LinkedList llstWhereData = new LinkedList();
                LinkedList llstParam = new LinkedList();

                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.LINE_RAWID))
                {
                    llstWhereData.Add("LINE", llstParam[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.AREA_RAWID))
                {
                    llstWhereData.Add("AREA", llstParam[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.EQP_MODEL))
                {
                    if (llstParam[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString() == "")
                    {
                        llstWhereData.Add("EQP_MODEL", "-");
                    }
                    else
                    {
                        llstWhereData.Add("EQP_MODEL", llstParam[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                    }
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.EQP_ID))
                {
                    if (llstParam[Definition.DynamicCondition_Condition_key.EQP_ID].ToString().Length > 0)
                    {
                        strSubQueryEQP = string.Format(" AND CONTEXT_VALUE LIKE '%{0}%' ", llstParam[Definition.DynamicCondition_Condition_key.EQP_ID].ToString());
                    }
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODULE_ID))
                {
                    if (llstParam[Definition.DynamicCondition_Condition_key.MODULE_ID].ToString().Length > 0)
                    {
                        strSubQueryModule = string.Format(" AND CONTEXT_VALUE LIKE '%{0}%' ", llstParam[Definition.DynamicCondition_Condition_key.MODULE_ID].ToString());
                    }
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.RECIPE_ID))
                {
                    if (llstParam[Definition.DynamicCondition_Condition_key.RECIPE_ID].ToString().Length > 0)
                    {
                        strSubQueryRecipe = string.Format(" AND CONTEXT_VALUE LIKE '%{0}%' ", llstParam[Definition.DynamicCondition_Condition_key.RECIPE_ID].ToString());
                    }
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.STEP_ID))
                {
                    if (llstParam[Definition.DynamicCondition_Condition_key.STEP_ID].ToString().Length > 0)
                    {
                        strSubQueryStep = string.Format(" AND CONTEXT_VALUE LIKE '%{0}%' ", llstParam[Definition.DynamicCondition_Condition_key.STEP_ID].ToString());
                    }
                }


                sSQL = string.Format(SQL_SEARCH_CONTEXT_KEY, strSubQueryEQP, strSubQueryModule, strSubQueryRecipe, strSubQueryStep, strSubQueryEnd);
                dsReturn = base.Query(sSQL, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            return dsReturn;
        }

        private const string SQL_COPY_MODEL_FOR_MODEL_CONFIG_MST_SPC = " UPDATE MODEL_CONFIG_MST_SPC SET ";

        //private const string SQL_COPY_MODEL_FOR_MODEL_CONFIG_MST_SPC_RULE = " UPDATE MODEL_CONFIG_MST_SPC " +
        //    " SET (UPPER_SPEC, LOWER_SPEC, TARGET, UPPER_CONTROL, LOWER_CONTROL, EWMA_LAMBDA, MOVING_COUNT, CENTER_LINE, LAST_UPDATE_DTTS, " +
        //    "       LAST_UPDATE_BY, STD, RAW_UCL, RAW_LCL, RAW_CENTER_LINE, STD_UCL, STD_LCL, STD_CENTER_LINE, RANGE_UCL, RANGE_LCL, RANGE_CENTER_LINE, " +
        //    "       EWMA_M_UCL, EWMA_M_LCL, EWMA_M_CENTER_LINE, EWMA_R_UCL, EWMA_R_LCL, EWMA_R_CENTER_LINE, EWMA_S_UCL, EWMA_S_LCL, EWMA_S_CENTER_LINE, " +
        //    "       MA_UCL, MA_LCL, MA_CENTER_LINE, MS_UCL, MS_LCL, MS_CENTER_LINE, ZONE_A_UCL, ZONE_A_LCL, ZONE_B_UCL, ZONE_B_LCL, ZONE_C_UCL, ZONE_C_LCL) = " +
        //    " ( SELECT UPPER_SPEC, LOWER_SPEC, TARGET, UPPER_CONTROL, LOWER_CONTROL, EWMA_LAMBDA, MOVING_COUNT, CENTER_LINE, SYSTIMESTAMP, " +
        //    "       :USER_ID, STD, RAW_UCL, RAW_LCL, RAW_CENTER_LINE, STD_UCL, STD_LCL, STD_CENTER_LINE, RANGE_UCL, RANGE_LCL, RANGE_CENTER_LINE, " +
        //    "       EWMA_M_UCL, EWMA_M_LCL, EWMA_M_CENTER_LINE, EWMA_R_UCL, EWMA_R_LCL, EWMA_R_CENTER_LINE, EWMA_S_UCL, EWMA_S_LCL, EWMA_S_CENTER_LINE, " +
        //    "       MA_UCL, MA_LCL, MA_CENTER_LINE, MS_UCL, MS_LCL, MS_CENTER_LINE, ZONE_A_UCL, ZONE_A_LCL, ZONE_B_UCL, ZONE_B_LCL, ZONE_C_UCL, ZONE_C_LCL " +
        //    "   FROM MODEL_CONFIG_MST_SPC " +
        //    "   WHERE RAWID = :SOURCE_MODEL_CONFIG_RAWID) " +
        //    " WHERE RAWID = :TARGET_MODEL_CONFIG_RAWID ";

        private const string SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_MST_SPC = " INSERT INTO MODEL_RULE_MST_SPC (RAWID, MODEL_CONFIG_RAWID, SPC_RULE_NO, OCAP_LIST, USE_MAIN_SPEC_YN, RULE_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY) " +
            " SELECT  SEQ_MODEL_RULE_MST_SPC.NEXTVAL, :TARGET_MODEL_CONFIG_RAWID, SPC_RULE_NO, OCAP_LIST, USE_MAIN_SPEC_YN, RULE_ORDER, SYSTIMESTAMP, :USER_ID, SYSTIMESTAMP, :USER_ID " +
            " FROM    MODEL_RULE_MST_SPC " +
            " WHERE   MODEL_CONFIG_RAWID = :SOURCE_MODEL_CONFIG_RAWID ";

        private const string SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_OPT_MST_SPC = " INSERT INTO MODEL_RULE_OPT_MST_SPC (RAWID, MODEL_RULE_RAWID, RULE_OPTION_NO, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, RULE_OPTION_VALUE) " +
            " SELECT  SEQ_MODEL_RULE_OPT_MST_SPC.NEXTVAL, (SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID = :TARGET_MODEL_CONFIG_RAWID AND SPC_RULE_NO = MR.SPC_RULE_NO ), " +
            " MO.RULE_OPTION_NO, SYSTIMESTAMP, :USER_ID, SYSTIMESTAMP, :USER_ID, MO.RULE_OPTION_VALUE " +
            " FROM    MODEL_RULE_MST_SPC MR INNER JOIN MODEL_RULE_OPT_MST_SPC MO " +
            " ON MR.RAWID = MO.MODEL_RULE_RAWID " +
            " WHERE   MR.MODEL_CONFIG_RAWID = :SOURCE_MODEL_CONFIG_RAWID ";

        private const string SQL_COPY_MODEL_FOR_MODEL_CONFIG_OPT_MST_SPC = " UPDATE MODEL_CONFIG_OPT_MST_SPC SET ";

        private const string SQL_COPY_MODEL_FOR_MODEL_AUTOCALC_MST_SPC = " UPDATE MODEL_AUTOCALC_MST_SPC SET ";


        public DataSet CopyModelInfo(LinkedList llstParam)
        {
            DataSet dsResult = null;

            try
            {
                LinkedList llstFieldData = new LinkedList();

                string sourceConfigRawID = llstParam[Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID].ToString();
                string targetConfigRawID = llstParam[Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID].ToString();
                string userID = llstParam[Definition.DynamicCondition_Condition_key.USER_ID].ToString();

                StringBuilder sbQuery = new StringBuilder();

                #region MODEL_CONFIG_MST_SPC
                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_CONFIG_MST_SPC);
                bool isEmpty = true;

                string tableName = Definition.TableName.MODEL_CONFIG_MST_SPC;

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_INTERLOCK].ToString(), COLUMN.INTERLOCK_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT].ToString(), COLUMN.USE_EXTERNAL_SPEC_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION].ToString(), COLUMN.AUTOCALC_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART].ToString(), COLUMN.AUTO_SUB_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_ACTIVE].ToString(), COLUMN.ACTIVATION_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_MODE].ToString(), COLUMN.CHART_MODE_CD, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT].ToString(), COLUMN.SAMPLE_COUNT, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE].ToString(), COLUMN.MANAGE_TYPE_CD, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_SETTING].ToString(), COLUMN.AUTO_TYPE_CD, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK].ToString(), COLUMN.SUB_INTERLOCK_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION].ToString(), COLUMN.SUB_AUTOCALC_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE].ToString(), COLUMN.USE_NORM_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE].ToString(), COLUMN.VALIDATION_SAME_MODULE_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN].ToString(), COLUMN.INHERIT_MAIN_YN, COLUMN.RAWID, sourceConfigRawID);

                //SPC-676 by Louis
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION].ToString(), COLUMN.CHART_DESCRIPTON, COLUMN.RAWID, sourceConfigRawID);

                string sUseOffset = "N";

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString(), COLUMN.UPPER_SPEC, COLUMN.LOWER_SPEC, COLUMN.TARGET, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString(), COLUMN.SPEC_USL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString(), COLUMN.SPEC_LSL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }


                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_RAW].ToString(), COLUMN.RAW_UCL, COLUMN.RAW_LCL, COLUMN.RAW_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RAW].ToString(), COLUMN.RAW_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RAW].ToString(), COLUMN.RAW_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RAW].ToString(), COLUMN.UPPER_FILTER, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RAW].ToString(), COLUMN.LOWER_FILTER, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_MEAN].ToString(), COLUMN.UPPER_CONTROL, COLUMN.LOWER_CONTROL, COLUMN.CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MEAN].ToString(), COLUMN.MEAN_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MEAN].ToString(), COLUMN.MEAN_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_STD].ToString(), COLUMN.STD_UCL, COLUMN.STD_LCL, COLUMN.STD_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_STD].ToString(), COLUMN.STD_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_STD].ToString(), COLUMN.STD_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_RANGE].ToString(), COLUMN.RANGE_UCL, COLUMN.RANGE_LCL, COLUMN.RANGE_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RANGE].ToString(), COLUMN.RANGE_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_RANGE].ToString(), COLUMN.RANGE_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_EWMA_MEAN].ToString(), COLUMN.EWMA_M_UCL, COLUMN.EWMA_M_LCL, COLUMN.EWMA_M_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_MEAN].ToString(), COLUMN.EWMA_M_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_MEAN].ToString(), COLUMN.EWMA_M_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_EWMA_STD].ToString(), COLUMN.EWMA_S_UCL, COLUMN.EWMA_S_LCL, COLUMN.EWMA_S_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_STD].ToString(), COLUMN.EWMA_S_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_STD].ToString(), COLUMN.EWMA_S_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_EWMA_RANGE].ToString(), COLUMN.EWMA_R_UCL, COLUMN.EWMA_R_LCL, COLUMN.EWMA_R_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_RANGE].ToString(), COLUMN.EWMA_R_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_EWMA_RANGE].ToString(), COLUMN.EWMA_R_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_MA].ToString(), COLUMN.MA_UCL, COLUMN.MA_LCL, COLUMN.MA_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MA].ToString(), COLUMN.MA_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MA].ToString(), COLUMN.MA_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_MS].ToString(), COLUMN.MS_UCL, COLUMN.MS_LCL, COLUMN.MS_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MS].ToString(), COLUMN.MS_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MS].ToString(), COLUMN.MS_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_MR].ToString(), COLUMN.MR_UCL, COLUMN.MR_LCL, COLUMN.MR_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MR].ToString(), COLUMN.MR_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MR].ToString(), COLUMN.MR_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT].ToString(), COLUMN.UPPER_TECHNICAL_LIMIT, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT].ToString(), COLUMN.LOWER_TECHNICAL_LIMIT, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MOVING_OPTIONS].ToString(), COLUMN.EWMA_LAMBDA, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MOVING_OPTIONS].ToString(), COLUMN.MOVING_COUNT, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_MEAN_OPTIONS].ToString(), COLUMN.STD, COLUMN.RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_A].ToString(), COLUMN.ZONE_A_UCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_A].ToString(), COLUMN.ZONE_A_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_A].ToString(), COLUMN.ZONE_A_LCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_A].ToString(), COLUMN.ZONE_A_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_B].ToString(), COLUMN.ZONE_B_UCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_B].ToString(), COLUMN.ZONE_B_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_B].ToString(), COLUMN.ZONE_B_LCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_B].ToString(), COLUMN.ZONE_B_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_C].ToString(), COLUMN.ZONE_C_UCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_C].ToString(), COLUMN.ZONE_C_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_C].ToString(), COLUMN.ZONE_C_LCL, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_ZONE_C].ToString(), COLUMN.ZONE_C_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                if (sUseOffset == "Y")
                {
                    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, sUseOffset, COLUMN.OFFSET_YN, COLUMN.RAWID, sourceConfigRawID);
                }

                if (isEmpty == false)
                {
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");
                    //spc-977 by stella
                    sbQuery.AppendLine(", " + COLUMN.CHANGED_ITEMS + string.Format("= '{0}' ", llstParam[COLUMN.CHANGED_ITEMS].ToString()));
                    sbQuery.AppendLine(", " + COLUMN.SAVE_COMMENT + string.Format("= '{0}' ", llstParam[COLUMN.SAVE_COMMENT].ToString()));
                    sbQuery.AppendLine(" WHERE RAWID = '" + targetConfigRawID + "' ");

                    //dsResult = base.Query(sbQuery.ToString());

                    //if (base.ErrorMessage.Length > 0)
                    //{
                    //    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    //    return dsResult;
                    //}
                }
                else
                {
                    sbQuery.AppendLine(COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");
                    //spc-977 by stella
                    sbQuery.AppendLine(", " + COLUMN.CHANGED_ITEMS + string.Format("= '{0}' ", llstParam[COLUMN.CHANGED_ITEMS].ToString()));
                    sbQuery.AppendLine(", " + COLUMN.SAVE_COMMENT + string.Format("= '{0}' ", llstParam[COLUMN.SAVE_COMMENT].ToString()));
                    sbQuery.AppendLine(" WHERE RAWID = '" + targetConfigRawID + "' ");
                }

                dsResult = base.Query(sbQuery.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                #endregion

                #region MODEL_RULE_MST_SPC & MODEL_RULE_OPT_MST_SPC
                string ruleSelection = llstParam[Definition.COPY_MODEL.RULE_RULE_SELECTION].ToString();

                if (ruleSelection.Trim().ToUpper() == "Y")
                {
                    // DELETE MODEL_RULE_OPT_MST_SPC
                    llstFieldData.Clear();
                    string sWhereQuery = string.Format("WHERE {0} IN (SELECT RAWID FROM {1} WHERE {2} = :{2}) ",
                        COLUMN.MODEL_RULE_RAWID, TABLE.MODEL_RULE_MST_SPC, COLUMN.MODEL_CONFIG_RAWID);

                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, targetConfigRawID);

                    base.Delete(TABLE.MODEL_RULE_OPT_MST_SPC, sWhereQuery, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // DELETE MODEL_RULE_MST_SPC
                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
                    base.Delete(TABLE.MODEL_RULE_MST_SPC, sWhereQuery, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // INSERT MODEL_RULE_MST_SPC
                    llstFieldData.Clear();
                    llstFieldData.Add("TARGET_MODEL_CONFIG_RAWID", targetConfigRawID);
                    llstFieldData.Add("USER_ID", userID);
                    llstFieldData.Add("SOURCE_MODEL_CONFIG_RAWID", sourceConfigRawID);

                    dsResult = base.Query(SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_MST_SPC, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // INSERT MODULE_RULE_OPT_MST_SPC
                    dsResult = base.Query(SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_OPT_MST_SPC, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_CONFIG_OPT_MST_SPC
                isEmpty = true;
                sbQuery.Length = 0;
                tableName = Definition.TableName.MODEL_CONFIG_OPT_MST_SPC;

                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_CONFIG_OPT_MST_SPC);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY].ToString(), COLUMN.SPC_PARAM_CATEGORY_CD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_CALCULATE_PPK].ToString(), COLUMN.AUTO_CPK_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_PRIORITY].ToString(), COLUMN.SPC_PRIORITY_CD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS].ToString(), COLUMN.RESTRICT_SAMPLE_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DAYS].ToString(), COLUMN.RESTRICT_SAMPLE_DAYS, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW].ToString(), COLUMN.DEFAULT_CHART_LIST, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                //SPC-712 By Louis
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DAYS].ToString(), COLUMN.RESTRICT_SAMPLE_HOURS, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                if (isEmpty == false)
                {
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");

                    sbQuery.AppendLine(" WHERE MODEL_CONFIG_RAWID = '" + targetConfigRawID + "' ");

                    dsResult = base.Query(sbQuery.ToString());

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_AUTOCALC_MST_SPC
                isEmpty = true;
                sbQuery.Length = 0;
                tableName = Definition.TableName.MODEL_AUTOCALC_MST_SPC;

                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_AUTOCALC_MST_SPC);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD].ToString(), COLUMN.AUTOCALC_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT].ToString(), COLUMN.CALC_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE].ToString(), COLUMN.MIN_SAMPLES, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD].ToString(), COLUMN.DEFAULT_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE].ToString(), COLUMN.MAX_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD].ToString(), COLUMN.CONTROL_THRESHOLD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT].ToString(), COLUMN.RAW_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT].ToString(), COLUMN.MEAN_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT].ToString(), COLUMN.STD_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT].ToString(), COLUMN.RANGE_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT].ToString(), COLUMN.EWMA_M_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT].ToString(), COLUMN.EWMA_S_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT].ToString(), COLUMN.EWMA_R_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT].ToString(), COLUMN.MA_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT].ToString(), COLUMN.MS_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT].ToString(), COLUMN.MR_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION].ToString(), COLUMN.SHIFT_CALC_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER].ToString(), COLUMN.WITHOUT_IQR_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN].ToString(), COLUMN.THRESHOLD_OFF_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //SPC-658 Initial Clac Count
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT].ToString(), COLUMN.CALC_COUNT, COLUMN.INITIAL_CALC_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                //SPC-1155 by Stella
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.USE_GLOBAL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                //if (llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString().Equals("Y"))
                //{
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.RAW_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MEAN_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.STD_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.RANGE_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.EWMA_M_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.EWMA_R_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.EWMA_S_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MA_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MS_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MR_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.RAW_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.MEAN_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.STD_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.RANGE_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.EWMA_M_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.EWMA_R_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.EWMA_S_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.MA_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.MS_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN].ToString(), COLUMN.CONTROL_LIMIT_OPTION, COLUMN.MR_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //}
                //else
                //{
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT].ToString(), COLUMN.RAW_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT].ToString(), COLUMN.MEAN_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT].ToString(), COLUMN.STD_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT].ToString(), COLUMN.RANGE_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT].ToString(), COLUMN.EWMA_M_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT].ToString(), COLUMN.EWMA_R_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT].ToString(), COLUMN.EWMA_S_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT].ToString(), COLUMN.MA_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT].ToString(), COLUMN.MS_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT].ToString(), COLUMN.MR_CALC_VALUE, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT].ToString(), COLUMN.RAW_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT].ToString(), COLUMN.MEAN_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT].ToString(), COLUMN.STD_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT].ToString(), COLUMN.RANGE_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT].ToString(), COLUMN.EWMA_M_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT].ToString(), COLUMN.EWMA_R_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT].ToString(), COLUMN.EWMA_S_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT].ToString(), COLUMN.MA_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT].ToString(), COLUMN.MS_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT].ToString(), COLUMN.MR_CALC_OPTION, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //}
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT].ToString(), COLUMN.RAW_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT].ToString(), COLUMN.MEAN_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT].ToString(), COLUMN.STD_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT].ToString(), COLUMN.RANGE_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT].ToString(), COLUMN.EWMA_M_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT].ToString(), COLUMN.EWMA_R_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT].ToString(), COLUMN.EWMA_S_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT].ToString(), COLUMN.MA_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT].ToString(), COLUMN.MS_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT].ToString(), COLUMN.MR_CALC_SIDED, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                if (isEmpty == false)
                {
                    if (llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT].ToString().Equals("Y"))
                    {
                        sbQuery.AppendLine(", " + COLUMN.RAW_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.MEAN_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.RANGE_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.STD_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.EWMA_MEAN_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.EWMA_STD_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.EWMA_RANGE_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.MA_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.MS_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.MR_CALC_COUNT + " = 0 ");
                    }

                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");

                    sbQuery.AppendLine(" WHERE MODEL_CONFIG_RAWID = '" + targetConfigRawID + "' ");

                    dsResult = base.Query(sbQuery.ToString());

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_CONTEXT_MST_SPC

                //SPC-1218, KBLEE, START
                if (llstParam[Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION].ToString().Equals("Y"))
                {
                    string queryGetSourceContext =
                        @"SELECT context_key, context_value 
                            FROM MODEL_CONTEXT_MST_SPC 
                           WHERE model_config_rawid = " + sourceConfigRawID;

                    DataSet sourceResult = base.Query(queryGetSourceContext);
                    int rowCount = sourceResult.Tables[0].Rows.Count;

                    for (int i = 0; i < rowCount; i++)
                    {
                        DataRow dr = sourceResult.Tables[0].Rows[i];
                        string contextKey = dr[COLUMN.CONTEXT_KEY].ToString();
                        string contextValue = dr[COLUMN.CONTEXT_VALUE].ToString();

                        string queryUpdateContextInform =
                            @"UPDATE MODEL_CONTEXT_MST_SPC
                                 SET context_value = '" + contextValue + @"' 
                               WHERE context_key = '" + contextKey + @"' 
                               AND model_config_rawid = " + targetConfigRawID;

                        dsResult = base.Query(queryUpdateContextInform);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }
                    }
                }
                //SPC-1218, KBLEE, END

                #endregion

                foreach (string s in _commondata.GetIncreaseVersionQuery(targetConfigRawID))
                {
                    this.Query(s);
                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        public DataSet ModifySPCSubModelForCopy(LinkedList llstParam, ref string subConfigRawIDs)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;
            string[] sArrModelConfigRawID = null;

            try
            {
                string targetConfigRawID = llstParam[Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID].ToString();
                string userID = llstParam[Definition.DynamicCondition_Condition_key.USER_ID].ToString();

                //#01. MODEL_CONFIG_MST_SPC
                llstWhereData.Clear();
                llstWhereData.Add("RAWID", targetConfigRawID);
                llstWhereData.Add("MAIN_MAIN", "Y");
                llstWhereData.Add("MAIN_SUB", "N");
                llstWhereData.Add("INHERIT_YN", "Y");

                dsResult = base.Query(SQL_UPDATE_MODEL_CONFIG_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //#02. MODEL_RULE_MST_SPC, MODEL_RULE_OPT_MST_SPC DATA 삭제하고 Main config rawid 기준으로 Insert 한다.
                llstWhereData.Clear();

                llstWhereData.Add("RAWID", targetConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_OPT_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                llstWhereData.Clear();

                llstWhereData.Add("RAWID", targetConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //sub config의 model_config_rawid를 구한다.
                llstWhereData.Clear();
                llstWhereData.Add("RAWID", targetConfigRawID);

                dsResult = base.Query(SQL_SELECT_SUB_CONFIG_RAWID, llstWhereData);

                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    ArrayList arrTemp = new ArrayList();
                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                    {
                        arrTemp.Add(dsResult.Tables[0].Rows[i][COLUMN.RAWID].ToString());
                    }

                    sArrModelConfigRawID = (string[])arrTemp.ToArray(typeof(string));
                }

                if (sArrModelConfigRawID != null && sArrModelConfigRawID.Length > 0)
                {
                    DataSet dsRuleMst = this.GetSPCModelRuleMstInfo(targetConfigRawID);
                    DataSet dsRuleOptMst = this.GetSPCModelRuleOptMstInfo(targetConfigRawID);

                    for (int i = 0; i < sArrModelConfigRawID.Length; i++)
                    {
                        //#07. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dsRuleMst.Tables[0].Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, sArrModelConfigRawID[i]);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, userID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_RULE_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dsRuleOptMst.Tables[0].Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);
                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, userID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.MODEL_RULE_OPT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    return dsResult;
                                }
                            }
                        }

                        subConfigRawIDs += ";" + sArrModelConfigRawID[i];
                    }
                }
            }
            catch (Exception ex)
            {
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;

                sWhereQuery = null;
            }

            return dsResult;
        }

        public DataSet GetSPCModelRuleMstInfo(string targetConfigRawID)
        {
            StringBuilder sbQuery = new StringBuilder();

            //#04. MODEL_RULE_MST_SPC
            sbQuery.Length = 0;

            sbQuery.Append(" SELECT A.*, ");
            sbQuery.Append("        B.DESCRIPTION, "); //add columns 
            sbQuery.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, "); //dev define columns
            sbQuery.Append("        '' AS RULE_OPTION, '' AS RULE_OPTION_DATA "); //dev define columns
            sbQuery.Append("   FROM MODEL_RULE_MST_SPC A ");
            sbQuery.Append("        LEFT OUTER JOIN RULE_MST_SPC B ");
            sbQuery.Append("          ON A.SPC_RULE_NO = B.SPC_RULE_NO ");
            sbQuery.Append(" WHERE 1 = 1 ");
            sbQuery.Append("   AND A.MODEL_CONFIG_RAWID = " + targetConfigRawID + " ");

            DataSet dsResult = this.Query(sbQuery.ToString());

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }

        public DataSet GetSPCModelRuleOptMstInfo(string targetConfigRawID)
        {
            StringBuilder sbQuery = new StringBuilder();

            //#05. MODEL_RULE_OPT_MST_SPC
            sbQuery.Length = 0;

            sbQuery.Append(" SELECT B.*, ");
            sbQuery.Append("        C.OPTION_NAME, C.DESCRIPTION, "); //add columns
            sbQuery.Append("        A.SPC_RULE_NO "); //add columns
            sbQuery.Append("   FROM MODEL_RULE_MST_SPC A ");
            sbQuery.Append("        LEFT OUTER JOIN MODEL_RULE_OPT_MST_SPC B ");
            sbQuery.Append("          ON (A.RAWID = B.MODEL_RULE_RAWID) ");
            sbQuery.Append("        LEFT OUTER JOIN ");
            sbQuery.Append("             (SELECT A.RAWID AS RULE_RAWID, A.SPC_RULE_NO, ");
            sbQuery.Append("                     B.RULE_OPTION_NO, B.OPTION_NAME, B.DESCRIPTION ");
            sbQuery.Append("                FROM RULE_MST_SPC A ");
            sbQuery.Append("                     LEFT OUTER JOIN RULE_OPT_MST_SPC B ");
            sbQuery.Append("                       ON (A.RAWID = B.RULE_RAWID) ");
            sbQuery.Append("             ) C ");
            sbQuery.Append("          ON (A.SPC_RULE_NO = C.SPC_RULE_NO AND B.RULE_OPTION_NO = C.RULE_OPTION_NO) ");
            sbQuery.Append(" WHERE 1 = 1 ");
            sbQuery.Append("   AND A.MODEL_CONFIG_RAWID = " + targetConfigRawID + " ");

            DataSet dsResult = this.Query(sbQuery.ToString());

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }

        //Chris
        private const string SQL_MODEL_HISTORY_CONTEXT =
@"SELECT MCMS2.MODEL_CONFIG_RAWID, MCMS2.CONTEXT_KEY, MCMS2.CONTEXT_VALUE
  FROM MODEL_MST_SPC MMS INNER JOIN MODEL_CONFIG_MST_SPC MCMS
       ON MMS.RAWID = MCMS.MODEL_RAWID
       INNER JOIN MODEL_CONTEXT_MST_SPC MCMS2
       ON MCMS.RAWID = MCMS2.MODEL_CONFIG_RAWID
     AND MCMS2.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ";

        public DataSet GetSPCModelContextInfo(byte[] baData)
        {
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(SQL_MODEL_HISTORY_CONTEXT);

            DataSet dsResult = this.Query(sbQuery.ToString(), llstData);

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }

        //Chris
        private const string SQL_MODEL_CONFIG_HISTORY =
@"SELECT *
  FROM MODEL_CONFIG_HST_SPC
 WHERE MODIFIED_TYPE_CD = 'AC' AND MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ORDER BY CREATE_DTTS";

        public DataSet GetSPCModelHistoryInfo(byte[] baData)
        {
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(SQL_MODEL_CONFIG_HISTORY);

            //DataSet dsResult = this.Query(sbQuery.ToString(), llstData);
            DataSet dsResult = new DataSet();

            DataSet dsTemp = this.Query(sbQuery.ToString(), llstData);

            if (dsTemp != null && dsTemp.Tables.Count > 0)
            {
                DataTable dtExcel = new DataTable();
                for (int idx = 0; idx < dsTemp.Tables[0].Columns.Count; idx++)
                {
                    string sCol = dsTemp.Tables[0].Columns[idx].ColumnName.ToString().ToUpper();
                    dtExcel.Columns.Add(sCol, typeof(string));
                }

                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    dtExcel.ImportRow(dr);
                }

                dsResult.Tables.Add("AC_HISTORY");
                dsResult.Tables["AC_HISTORY"].Merge(dtExcel);
            }

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }

        //added by enkim 2012.05.18 SPC-739

        private const string strQuery_MCMS = @"UPDATE MODEL_CONFIG_MST_SPC A
                                        SET VERSION = ( SELECT NVL(VERSION, 0)+1 
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE RAWID = A.RAWID )
                                        WHERE MODEL_RAWID IN (SELECT MODEL_RAWID
                                                                FROM MODEL_CONFIG_MST_SPC
                                                               WHERE RAWID = :MODEL_CONFIG_RAWID)
                                        AND MAIN_YN = 'N'
                                        AND INHERIT_MAIN_YN = 'Y'";

        private const string strQuery_MCOMS = @"UPDATE MODEL_CONFIG_OPT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_MCTXMS = @"UPDATE MODEL_CONTEXT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_MAMS = @"UPDATE MODEL_AUTOCALC_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_MRMS = @"UPDATE MODEL_RULE_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_MROMS = @"UPDATE MODEL_RULE_OPT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_RULE_MST_SPC
                                                        WHERE RAWID = A.MODEL_RULE_RAWID )
                                        WHERE MODEL_RULE_RAWID IN (
                                                    SELECT RAWID 
                                                    FROM MODEL_RULE_MST_SPC
                                                    WHERE MODEL_CONFIG_RAWID IN ( 
                                                        SELECT RAWID
                                                        FROM MODEL_CONFIG_MST_SPC
                                                        WHERE MODEL_RAWID IN (
                                                            SELECT MODEL_RAWID
                                                            FROM MODEL_CONFIG_MST_SPC
                                                            WHERE RAWID = :MODEL_CONFIG_RAWID )
                                                        AND MAIN_YN = 'N'
                                                        AND INHERIT_MAIN_YN = 'Y'
                                                                    )
                                                    ) ";

        public bool IncreaseVersionOfSubConfigs(string sMainConfigRawID)
        {
            bool bResult = true;

            try
            {
                LinkedList llstCondition = new LinkedList();
                llstCondition.Add("MODEL_CONFIG_RAWID", sMainConfigRawID);

                base.Query(strQuery_MCMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_MCOMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_MCTXMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_MAMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_MRMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_MROMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                bResult = false;
            }

            return bResult;
        }

        //added end SPC-739

        //added by enkim 2012.10.05 SPC-910
        public bool GetUseNormalValuebyChartID(byte[] param)
        {
            bool bResult = false;
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                    sb.Append("SELECT NVL(USE_NORM_YN, 'N') USE_NORMAL FROM MODEL_CONFIG_MST_SPC ");
                    sb.Append(" WHERE MODEL_RAWID = (SELECT MODEL_RAWID FROM MODEL_CONFIG_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) AND MAIN_YN = 'Y' ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    bResult = false;
                }
                else
                {
                    if (dsTemp != null && dsTemp.Tables != null && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        if (dsTemp.Tables[0].Rows[0]["USE_NORMAL"].ToString().Equals(Definition.YES))
                            bResult = true;
                        else
                            bResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return bResult;
        }
        //added end SPC-910

        //SPC-855 By Louis
        public DataSet GetTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            try
            {
                if (targetList.Length != 0)
                {
                    int itemp = 0;
                    string strTemp = "";
                    ArrayList arrsModelRawID = new ArrayList();
                    if (targetList.Length > 1000)
                    {
                        for (int i = 0; i < targetList.Length; i++)
                        {
                            strTemp += "," + targetList[i];
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
                            strWhere = string.Format(" Rawid in ({0})", arrsModelRawID[k].ToString());

                            sqlQuery = string.Format(@"SELECT RAWID, UPPER_SPEC, LOWER_SPEC, UPPER_CONTROL, LOWER_CONTROL, RAW_UCL, RAW_LCL
                                                    FROM MODEL_CONFIG_MST_SPC WHERE {0}", strWhere);

                            DataSet dsTemp = base.Query(sqlQuery);

                            dsResult.Merge(dsTemp);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < targetList.Length; i++)
                        {
                            strTemp += "," + targetList[i];
                            itemp++;
                        }
                        if (strTemp.Length > 0)
                        {
                            arrsModelRawID.Add(strTemp.Substring(1));
                        }
                        for (int k = 0; k < arrsModelRawID.Count; k++)
                        {
                            strWhere = string.Format(" Rawid in ({0})", arrsModelRawID[k].ToString());

                            sqlQuery = string.Format(@"SELECT RAWID, UPPER_SPEC, LOWER_SPEC, UPPER_CONTROL, LOWER_CONTROL, RAW_UCL, RAW_LCL
                                                    FROM MODEL_CONFIG_MST_SPC WHERE {0}", strWhere);

                            DataSet dsTemp = base.Query(sqlQuery);

                            dsResult.Merge(dsTemp);
                        }
                    }

                }

            }
            catch
            {
            }

            return dsResult;
        }

        public DataSet GetSourceConfigSpecData(string rawid)
        {
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            try
            {
                strWhere = string.Format(" Rawid in ({0})", rawid);

                sqlQuery = string.Format(@"SELECT RAWID, UPPER_SPEC, LOWER_SPEC, UPPER_CONTROL, LOWER_CONTROL, RAW_UCL, RAW_LCL
                                                    FROM MODEL_CONFIG_MST_SPC WHERE {0}", strWhere);

                dsResult = base.Query(sqlQuery);

            }
            catch
            {
            }
            return dsResult;
        }

        private const string SQL_CREATE_AUTO_CALC_DATA_SHEET = "SELECT NAME, NULL AS CALCULATION_YN, NULL AS CALCULATION_COUNT, " +
                                                                " NULL AS CALCULATION_OPTION, NULL AS CALCULATION_VALUE, NULL AS CALCUATION_SIDED " +
                                                                " FROM CODE_MST_PP " +
                                                                " WHERE CATEGORY = :CATEGORY " +
                                                                " ORDER BY CODE_ORDER ";

        public DataSet CreateAutoCalculationDataSheet(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstData = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            string sSQL = SQL_CREATE_AUTO_CALC_DATA_SHEET;
            string sCategory = "";

            try
            {
                llstCondition.SetSerialData(baData);

                if (llstCondition[Definition.CONDITION_KEY_CATEGORY] != null)
                {
                    sCategory = llstCondition[Definition.CONDITION_KEY_CATEGORY].ToString();
                    llstData.Add("CATEGORY", sCategory);
                }

                dsReturn = this.Query(sSQL, llstData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                llstData.Clear();
                llstData = null;
                llstCondition.Clear();
                llstCondition = null;
            }

            return dsReturn;
        }

        //spc-977 rawid를 받아 version을 update해줌.
        public bool SetIncreaseVersion(byte[] param)
        {
            bool bReturn = true;
            LinkedList llRawid = new LinkedList();
            llRawid.SetSerialData(param);

            for (int i = 0; i < llRawid.Count; i++)
            {
                foreach (string query in _commondata.GetIncreaseVersionQuery(llRawid[i].ToString()))
                {
                    this.Query(query);
                }
            }

            if (base.ErrorMessage.Length > 0)
            {
                bReturn = false;
            }

            return bReturn;
        }

        public DataSet GetSPCModelGroupList(bool isMET)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            string level = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (isMET)
                sb.Append("SELECT * FROM CODE_MST_PP WHERE CATEGORY='SPC_MET_MODEL_LEVEL'");
            else
                sb.Append("SELECT * FROM CODE_MST_PP WHERE CATEGORY='SPC_MODEL_LEVEL'");

            ds = this.Query(sb.ToString());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[Definition.CONDITION_KEY_CODE].ToString() == Definition.CONDITION_KEY_AREA && dr[Definition.CONDITION_KEY_USE_YN].ToString() == "Y")
                    {
                        level = Definition.CONDITION_KEY_AREA;
                        break;
                    }
                    else
                    {
                        level = Definition.CONDITION_KEY_EQP_MODEL;
                    }
                }
            }

            if (level == Definition.CONDITION_KEY_AREA)
            {
                sb.Remove(0, sb.Length);
                sb.Append("SELECT LINE_RAWID AS LOCATION_RAWID, AREA_RAWID, AREA AS GROUP_LEVEL, EQPMODEL ");
                sb.Append("FROM EQP_VW_SPC GROUP BY LINE_RAWID, AREA_RAWID, AREA, EQPMODEL ");
                sb.Append("ORDER BY GROUP_LEVEL ASC ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "SPC_MODEL_LEVEL";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT DISTINCT A.RAWID, A.LOCATION_RAWID, A.AREA_RAWID, B.AREA AS GROUP_LEVEL, A.GROUP_NAME      ");
                sb.Append("FROM MODEL_GROUP_MST_SPC A, EQP_VW_SPC B                                   ");
                sb.Append("WHERE A.LOCATION_RAWID = B.LINE_RAWID AND A.AREA_RAWID = B.AREA_RAWID      ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_GROUP_SPC";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT   b.rawid, b.location_rawid, b.area_rawid,  ");
                sb.Append("b.spc_model_name, NVL (d.group_name, 'UNASSIGNED MODEL') AS GROUP_NAME  ");
                sb.Append("FROM model_mst_spc b,  ");
                sb.Append("model_config_mst_spc c,  ");
                sb.Append("model_group_mst_spc d  ");
                sb.Append("WHERE 1=1  ");
                sb.Append("AND c.model_rawid = b.rawid  ");
                sb.Append("AND c.main_yn = 'Y'  ");


                if (isMET)
                {
                    sb.Append("AND C.PARAM_TYPE_CD = 'MET'                                                                                         ");
                }
                else
                {
                    sb.Append("AND C.PARAM_TYPE_CD != 'MET'                                                                                       ");
                }

                sb.Append("AND b.location_rawid = d.location_rawid(+)  ");
                sb.Append("AND b.area_rawid = d.area_rawid(+)  ");
                sb.Append("AND b.group_rawid = d.rawid(+)  ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_MST_SPC";

                dsResult.Tables.Add(ds.Tables[0].Copy());
            }
            else
            {
                sb.Remove(0, sb.Length);
                sb.Append("SELECT LINE_RAWID AS LOCATION_RAWID, AREA_RAWID, EQPMODEL AS GROUP_LEVEL, EQPMODEL ");
                sb.Append("FROM EQP_VW_SPC GROUP BY LINE_RAWID, AREA_RAWID, EQPMODEL ");
                sb.Append("ORDER BY GROUP_LEVEL ASC ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "SPC_MODEL_LEVEL";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT RAWID, LOCATION_RAWID, AREA_RAWID, EQP_MODEL AS GROUP_LEVEL, GROUP_NAME FROM MODEL_GROUP_MST_SPC ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_GROUP_SPC";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT   b.rawid, b.location_rawid, b.area_rawid, b.eqp_model,     ");
                sb.Append("b.spc_model_name, NVL (d.group_name, 'UNASSIGNED MODEL') AS GROUP_NAME                                        ");
                sb.Append("FROM model_mst_spc b, ");
                sb.Append("model_config_mst_spc c, ");
                sb.Append("model_group_mst_spc d ");
                sb.Append("WHERE 1=1 ");
                sb.Append("AND c.model_rawid = b.rawid  ");
                sb.Append("AND c.main_yn = 'Y'  ");

                if (isMET)
                {
                    sb.Append("AND c.PARAM_TYPE_CD = 'MET'                                                                                         ");
                }
                else
                {
                    sb.Append("AND c.PARAM_TYPE_CD != 'MET'                                                                                       ");
                }

                sb.Append("AND b.location_rawid = d.location_rawid(+) ");
                sb.Append("AND b.area_rawid = d.area_rawid(+) ");
                sb.Append("AND b.eqp_model = d.eqp_model(+) ");
                sb.Append("AND b.group_rawid = d.rawid(+) ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_MST_SPC";

                dsResult.Tables.Add(ds.Tables[0].Copy());


            }
            return dsResult;
        }

        public DataSet GetSPCModelListbyGroup(byte[] param, bool isMET)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llWhereField = new LinkedList();
            llWhereField.Add(COLUMN.LINE_RAWID, llParam[COLUMN.LOCATION_RAWID].ToString());
            llWhereField.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID].ToString());

            if (llParam.Contains(Definition.CONDITION_KEY_EQP_MODEL))
            {
                llWhereField.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL].ToString());
            }

            llWhereField.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, "MET");

            try
            {
                #region original
                //StringBuilder sb = new StringBuilder();                                                                             
                //sb.Append("SELECT A.RAWID, A.SPC_MODEL_NAME, A.GROUP_RAWID                                                          ");
                //sb.Append("FROM    (  SELECT A.RAWID, A.LOCATION_RAWID, A.AREA_RAWID, A.EQP_MODEL, A.SPC_MODEL_NAME, A.GROUP_RAWID  ");
                //sb.Append("             FROM MODEL_MST_SPC A, MODEL_CONFIG_MST_SPC B                                                ");
                //sb.Append("            WHERE     A.RAWID = B.MODEL_RAWID                                                            ");
                //sb.Append("                  AND A.LOCATION_RAWID = :LOCATION_RAWID                                                 ");
                //sb.Append("                  AND A.AREA_RAWID = :AREA_RAWID                                                         ");

                //if (isMET)
                //{
                //    sb.Append("                  AND B.PARAM_TYPE_CD = 'MET'                                                            ");
                //}
                //else
                //{
                //    sb.Append("                  AND B.PARAM_TYPE_CD != 'MET'                                                           ");
                //}

                //sb.Append("         GROUP BY A.RAWID, A.LOCATION_RAWID, A.AREA_RAWID, A.EQP_MODEL, A.SPC_MODEL_NAME, A.GROUP_RAWID  ");
                //sb.Append("         ORDER BY A.SPC_MODEL_NAME ASC) A                                                                ");
                //sb.Append("     LEFT OUTER JOIN                                                                                     ");
                //sb.Append("        MODEL_GROUP_MST_SPC C                                                                            ");
                //sb.Append("     ON A.LOCATION_RAWID = C.LOCATION_RAWID                                                              ");
                //sb.Append("        AND A.AREA_RAWID = C.AREA_RAWID                                                                  ");

                //if (llParam.Contains(Definition.CONDITION_KEY_EQP_MODEL))
                //{
                //    sb.Append("        AND A.EQP_MODEL = C.EQP_MODEL                                                                ");
                //}

                //if (llParam[COLUMN.GROUP_NAME] != null)
                //{
                //    llWhereField.Add(COLUMN.GROUP_NAME, llParam[COLUMN.GROUP_NAME].ToString());
                //    sb.Append("        AND GROUP_NAME =: GROUP_NAME AND A.GROUP_RAWID = C.RAWID                                     ");
                //}
                //else
                //{
                //    sb.Append("        AND A.GROUP_RAWID IS NULL                                                                    ");
                //}

                //sb.Append("        AND A.GROUP_RAWID IS NULL                                                                        ");
                //sb.Append("GROUP BY A.RAWID, A.SPC_MODEL_NAME, A.GROUP_RAWID                                                        ");
                //sb.Append("ORDER BY A.SPC_MODEL_NAME ASC                                                                            ");

                //ds = base.Query(sb.ToString(), llWhereField);
                #endregion
                ConditionData conditionData = new ConditionData();
                DataRow[] dr;

                if (!isMET)
                {
                    ds = conditionData.GetSPCModel(llWhereField.GetSerialData());
                }
                else if (isMET)
                {
                    ds = conditionData.GetMETSPCModel(llWhereField.GetSerialData());
                }

                if (llParam[COLUMN.GROUP_RAWID] != null)
                {
                    dr = ds.Tables[0].Select(string.Format("GROUP_RAWID = {0}", llParam[COLUMN.GROUP_RAWID].ToString()));
                }
                else
                {
                    dr = ds.Tables[0].Select("GROUP_RAWID IS NULL");
                }

                dsResult.Tables.Add(ds.Tables[0].Clone());

                foreach (DataRow row in dr)
                {
                    dsResult.Tables[0].ImportRow(row);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        public bool SaveSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool bResult = false;
            LinkedList llParam = new LinkedList();
            LinkedList llstFieldData = new LinkedList();

            llParam.SetSerialData(param);
            base.BeginTrans();

            try
            {

                string sUserId = llParam[COLUMN.USER_ID].ToString();

                if (dsSave.Tables.Count > 0)
                {
                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                    {
                        LinkedList ll = new LinkedList();

                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                        {
                            DataSet dsMapping = new DataSet();

                            StringBuilder sbQuery = new StringBuilder();
                            sbQuery.Append("SELECT A.* FROM MODEL_MST_SPC A, MODEL_GROUP_MST_SPC B ");
                            sbQuery.Append("WHERE  A.GROUP_RAWID = B.RAWID ");
                            sbQuery.Append("AND A.GROUP_RAWID =: GROUP_RAWID ");

                            ll.Clear();
                            ll.Add(COLUMN.GROUP_RAWID, row[COLUMN.RAWID].ToString());

                            dsMapping = base.Query(sbQuery.ToString(), ll);

                            if (dsMapping.Tables[0].Rows.Count > 0)
                            {
                                string sQuery = "UPDATE MODEL_MST_SPC SET GROUP_RAWID = NULL WHERE GROUP_RAWID =: GROUP_RAWID";
                                base.Query(sQuery, ll);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    //DSUtil.SetResult(ds, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return false;
                                }
                            }

                            //group 삭제
                            string sWhereQuery = "WHERE RAWID = :GROUP_RAWID";
                            base.Delete(TABLE.MODEL_GROUP_MST_SPC, sWhereQuery, ll);
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_UPDATE))
                    {
                        LinkedList ll = new LinkedList();
                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_UPDATE].Rows)
                        {
                            string sWhereQuery = "WHERE RAWID = :RAWID";
                            ll.Clear();
                            ll.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.GROUP_NAME, row[COLUMN.GROUP_NAME].ToString().ToUpper());
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                            base.Update(TABLE.MODEL_GROUP_MST_SPC, llstFieldData, sWhereQuery, ll);
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                    {
                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                        {
                            decimal groupRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_GROUP_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, groupRawID);
                            llstFieldData.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LOCATION_RAWID].ToString());
                            llstFieldData.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID].ToString());

                            if (llParam[COLUMN.EQP_MODEL] != null)
                                llstFieldData.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL].ToString());

                            llstFieldData.Add(COLUMN.GROUP_NAME, row[COLUMN.GROUP_NAME].ToString().ToUpper());
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");
                            llstFieldData.Add(COLUMN.CREATE_BY, sUserId);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                            base.Insert(TABLE.MODEL_GROUP_MST_SPC, llstFieldData);
                        }
                    }

                    if (base.ErrorMessage.Length > 0)
                    {
                        //DSUtil.SetResult(ds, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return false;
                    }

                    Commit();
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return bResult;
        }

        public bool SaveSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llstFieldData = new LinkedList();
            StringBuilder sbQuery = new StringBuilder();

            string sWhereQuery = "WHERE RAWID = :RAWID";
            string sUserId = llParam[Definition.CONDITION_KEY_USER_ID].ToString();
            string sGroup_Rawid = llParam[COLUMN.GROUP_RAWID].ToString();

            base.BeginTrans();

            try
            {
                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                {
                    foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                    {
                        llParam.Clear();
                        llParam.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.GROUP_RAWID, sGroup_Rawid);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                        base.Update(TABLE.MODEL_MST_SPC, llstFieldData, sWhereQuery, llParam);
                    }
                }

                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                {
                    foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                    {
                        llParam.Clear();
                        llParam.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.GROUP_RAWID, DBNull.Value);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                        base.Update(TABLE.MODEL_MST_SPC, llstFieldData, sWhereQuery, llParam);
                    }
                }

                if (base.ErrorMessage.Length > 0)
                {
                    base.RollBack();
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();
            bResult = true;

            return bResult;
        }

        public DataSet GetGroupList(byte[] param)
        {
            DataSet dsResult = null;
            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llWhereField = new LinkedList();
            llWhereField.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LINE_RAWID].ToString());
            llWhereField.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID].ToString());

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM MODEL_GROUP_MST_SPC ");
                sb.Append("WHERE LOCATION_RAWID =: LOCATION_RAWID ");
                sb.Append("AND AREA_RAWID =: AREA_RAWID ");

                if (llParam.Contains(COLUMN.EQP_MODEL))
                {
                    llWhereField.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL].ToString().Replace("'", string.Empty));
                    sb.Append("AND EQP_MODEL =: EQP_MODEL ");
                }
                sb.Append("ORDER BY GROUP_NAME ASC");

                dsResult = base.Query(sb.ToString(), llWhereField);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        public DataSet GetGroupNameByChartId(string chartId)
        {
            DataSet dsResult = new DataSet();
            LinkedList ll = new LinkedList();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("SELECT A.*                                                                ");
                sb.Append("FROM MODEL_GROUP_MST_SPC A                                                ");
                sb.Append("WHERE RAWID = (SELECT C.GROUP_RAWID                                       ");
                sb.Append("                  FROM MODEL_CONFIG_MST_SPC B, MODEL_MST_SPC C            ");
                sb.Append("                 WHERE B.MODEL_RAWID = C.RAWID AND B.RAWID = :CHART_ID)   ");

                ll.Add(COLUMN.CHART_ID, chartId);

                dsResult = base.Query(sb.ToString(), ll);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;

        }

        public bool CheckDuplicateGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = true;
            DataSet ds = new DataSet();

            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llWhereField = new LinkedList();
            llWhereField.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LOCATION_RAWID]);
            llWhereField.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID]);

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM MODEL_GROUP_MST_SPC        ");
            sb.Append("WHERE LOCATION_RAWID =: LOCATION_RAWID   ");
            sb.Append("AND AREA_RAWID =: AREA_RAWID             ");

            //if (llParam[COLUMN.SPC_DATA_LEVEL].ToString() == Definition.CONDITION_KEY_EQP_MODEL)
            //{
            //    sb.Append("AND EQP_MODEL =: EQP_MODEL             ");
            //    llWhereField.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL]);
            //}

            ds = base.Query(sb.ToString(), llWhereField);

            if (ds.Tables.Count > 0)
            {
                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                {
                    foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                    {
                        DataRow[] drDel = ds.Tables[0].Select(string.Format("RAWID = '{0}' ", dr[COLUMN.RAWID].ToString()));
                        ds.Tables[0].Rows.Remove(drDel[0]);
                    }
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_UPDATE))
                    {
                        foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_UPDATE].Rows)
                        {
                            DataRow[] drUpdate = ds.Tables[0].Select(string.Format("RAWID = '{0}' ", dr[COLUMN.RAWID].ToString()));

                            if (drUpdate.Length > 0)
                            {
                                drUpdate[0][COLUMN.GROUP_NAME] = dr[COLUMN.GROUP_NAME].ToString().ToUpper();
                            }
                            else if (row[COLUMN.GROUP_NAME].ToString() == dr[COLUMN.GROUP_NAME].ToString() && dr["_DELETE"].ToString() == Definition.VARIABLE_FALSE)
                            {
                                return false;
                            }
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                    {
                        foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                        {
                            if (row[COLUMN.GROUP_NAME].ToString() == dr[COLUMN.GROUP_NAME].ToString().ToUpper())
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return bResult;
        }

        public string CheckDuplicateMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string strResult = string.Empty;

            LinkedList llParam = new LinkedList();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM MODEL_MST_SPC ");
            sb.Append("WHERE RAWID =: RAWID  ");

            if (isEmptyGroupRawid)
                sb.Append("AND GROUP_RAWID IS NULL ");
            else
                sb.Append("AND GROUP_RAWID =: GROUP_RAWID ");

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["_SELECT"].ToString() == Definition.VARIABLE_TRUE)
                {
                    llParam.Clear();
                    llParam.Add(COLUMN.RAWID, dr[COLUMN.RAWID].ToString());

                    if (!isEmptyGroupRawid)
                        llParam.Add(COLUMN.GROUP_RAWID, dr[COLUMN.GROUP_RAWID].ToString());

                    DataSet ds = base.Query(sb.ToString(), llParam);

                    if (base.ErrorMessage.Length > 0 || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return dr[Definition.CONDITION_KEY_MODEL_NAME].ToString();
                    }
                }
            }

            return strResult;
        }

        public int CheckCountSPCModel(byte[] param)
        {
            int cntModel = -1;

            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(param);


            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) FROM MODEL_MST_SPC ");
            sb.Append("WHERE LOCATION_RAWID = :LOCATION_RAWID ");
            sb.Append("AND AREA_RAWID = :AREA_RAWID ");
            sb.Append("AND GROUP_RAWID = :GROUP_RAWID ");

            DataSet ds = base.Query(sb.ToString(), llparam);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cntModel = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
            }

            return cntModel;
        }

        //SPC-929, KBLEE, START
        public DataSet GetEqpSummaryTrxFileData(byte[] baData)
        {
            DataSet dsReturn = new DataSet();

            LinkedList llstCondition = new LinkedList();
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, A.* ");
            sbQuery.Append("  FROM EQP_SUMMARY_TRX_PP A ");
            sbQuery.Append("       INNER JOIN EQP_MST_PP B ");
            sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
            sbQuery.Append(" WHERE 1 = 1 ");

            if (llstData[Definition.CONDITION_KEY_RAWID] != null)
            {
                //01. RAWID로 조회하는 경우

                object obRawID = llstData[Definition.CONDITION_KEY_RAWID];

                if (obRawID is ArrayList)
                {
                    ArrayList alRawIDList = (ArrayList)llstData[Definition.CONDITION_KEY_RAWID];

                    sbQuery.AppendFormat(" AND A.RAWID IN ({0}) ", _ComUtil.ConvertArrayIntoStringList(alRawIDList, ","));
                }
                else
                {
                    sbQuery.Append(" AND A.RAWID = :RAWID ");
                    llstCondition.Add("RAWID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_RAWID]));
                }
            }
            else
            {
                //02. CONTEXT 정보로 조회하는 경우

                //EQP
                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sbQuery.Append(" AND A.EQP_MODULE_ID = :EQP_MODULE_ID ");
                    llstCondition.Add("EQP_MODULE_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_MODULE_ID]));
                }

                //DCP
                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                {
                    sbQuery.Append(" AND A.EQP_DCP_ID = :EQP_DCP_ID ");
                    llstCondition.Add("EQP_DCP_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_DCP_ID]));
                }

                //PARAMETER_ALIAS
                if (llstData[Definition.CONDITION_KEY_PARAM_ALIAS] != null)
                {
                    sbQuery.Append(" AND A.PARAMETER_ALIAS = :PARAM_ALIAS ");
                    llstCondition.Add("PARAM_ALIAS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_PARAM_ALIAS]));
                }

                //MESSAGE_TYPE_CD
                if (llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD] != null)
                {
                    sbQuery.Append(" AND A.MESSAGE_TYPE_CD = :MESSAGE_TYPE_CD ");
                    llstCondition.Add("MESSAGE_TYPE_CD", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD]));
                }

                if (llstData[Definition.CONDITION_KEY_START_DTTS] != null && llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                {
                    object obStartDtts = llstData[Definition.CONDITION_KEY_START_DTTS];
                    object obEndDtts = llstData[Definition.CONDITION_KEY_END_DTTS];

                    if (obStartDtts is DateTime && obEndDtts is DateTime)
                    {
                        llstCondition.Add("START_DTTS", (DateTime)llstData[Definition.CONDITION_KEY_START_DTTS]);
                        llstCondition.Add("END_DTTS", (DateTime)llstData[Definition.CONDITION_KEY_END_DTTS]);

                        sbQuery.Append(" AND ");
                        sbQuery.Append(" ( ");
                        sbQuery.Append(" a.start_dtts between :START_DTTS and :END_DTTS ");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" a.end_dtts between :START_DTTS and :END_DTTS ");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" :START_DTTS BETWEEN a.start_dtts AND a.end_dtts ");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" :END_DTTS BETWEEN a.start_dtts AND a.end_dtts ");
                        sbQuery.Append(" ) ");
                    }
                    else
                    {
                        llstCondition.Add("START_DTTS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_START_DTTS]));
                        llstCondition.Add("END_DTTS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_END_DTTS]));

                        sbQuery.Append(" AND ");
                        sbQuery.Append(" ( ");
                        sbQuery.Append(" a.start_dtts between (TO_TIMESTAMP(:START_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3')) and (TO_TIMESTAMP(:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3'))");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" a.end_dtts between (TO_TIMESTAMP(:START_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3')) and (TO_TIMESTAMP(:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3'))");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" TO_TIMESTAMP(:START_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3') BETWEEN a.start_dtts AND a.end_dtts ");
                        sbQuery.Append(" or ");
                        sbQuery.Append(" TO_TIMESTAMP(:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3') BETWEEN a.start_dtts AND a.end_dtts ");
                        sbQuery.Append(" ) ");
                    }
                }

                //END DATE
                /*if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                {
                    object obEndDtts = llstData[Definition.CONDITION_KEY_END_DTTS];

                    if (obEndDtts is DateTime)
                    {
                        DateTime dtEnd = (DateTime)llstData[Definition.CONDITION_KEY_END_DTTS];
                        llstCondition.Add("END_DTTS", dtEnd.ToString(Definition.DATETIME_FORMAT_MS));
                    }
                    else
                    {
                        llstCondition.Add("END_DTTS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_END_DTTS]));
                    }

                    sbQuery.Append(" AND A.START_DTTS <= TO_TIMESTAMP(:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3') ");
                }*/
            }

            sbQuery.Append(" ORDER BY A.START_DTTS  ");

            try
            {
                dsReturn = base.Query(sbQuery.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                DSUtil.SetResult(dsReturn, 0, "", ex.Message);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public DataSet GetEqpSummaryTempTrxCLOBData(byte[] baData)
        {
            DataSet dsReturn = new DataSet();

            LinkedList llstCondition = new LinkedList();
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT b.eqp_id, b.module_id, b.alias AS module_alias, ");
            sbQuery.Append("       a.sum_dtts AS TIME, a.lot_id AS lotid, a.substrate_id AS substrateid, ");
            sbQuery.Append("       a.recipe_id AS recipe, a.operation_id AS operation, ");
            sbQuery.Append("       a.product_id AS product, a.cassette_slot AS slot, a.step_id AS step, a.context_key, ");
            sbQuery.Append("       a.file_data, ");
            sbQuery.Append("       a.rsd_01, a.rsd_02, a.rsd_03, a.rsd_04, a.rsd_05, a.rsd_06, a.rsd_07, a.rsd_08, a.rsd_09, a.rsd_10, ");
            sbQuery.Append("       a.rsd_11, a.rsd_12, a.rsd_13, a.rsd_14, a.rsd_15, a.rsd_16, a.rsd_17, a.rsd_18, a.rsd_19, a.rsd_20 ");
            sbQuery.Append("  FROM eqp_summary_tmp_trx_pp a INNER JOIN eqp_mst_pp b ");
            sbQuery.Append("       ON (a.eqp_module_id = b.module_id) ");
            sbQuery.Append(" WHERE 1 = 1 ");

            //EQP
            if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
            {
                sbQuery.Append(" AND A.EQP_MODULE_ID = :EQP_MODULE_ID ");
                llstCondition.Add("EQP_MODULE_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_MODULE_ID]));
            }

            //DCP
            if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
            {
                sbQuery.Append(" AND A.EQP_DCP_ID = :EQP_DCP_ID ");
                llstCondition.Add("EQP_DCP_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_DCP_ID]));
            }

            //RECIPE
            if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
            {
                if (llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString() != "DEFAULT_ALL")
                {
                    sbQuery.Append(" AND A.RECIPE_ID = :RECIPE_ID ");
                    llstCondition.Add("RECIPE_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_RECIPE_ID]));
                }
            }

            //RECIPE LIST
            if (llstData.Contains(Definition.CONDITION_KEY_RECIPE_ID_LIST))
            {
                object obj = llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST];

                if (obj is string)
                {
                    sbQuery.Append(" AND A.RECIPE_ID = :RECIPE_ID ");
                    llstCondition.Add("RECIPE_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST]));
                }
                else if (obj is IEnumerable)
                {
                    sbQuery.AppendFormat(" AND A.RECIPE_ID IN ({0}) ", _ComUtil.ConvertArrayIntoStringList((IEnumerable)obj, "'", ","));
                }
            }

            //STEP
            if (llstData[Definition.CONDITION_KEY_STEP_ID] != null)
            {
                if (llstData[Definition.CONDITION_KEY_STEP_ID].ToString() != "DEFAULT_ALL")
                {
                    if (llstData[Definition.CONDITION_KEY_STEP_ID].ToString().Length > 0)
                    {
                        sbQuery.Append(" AND A.STEP_ID= :STEP_ID");
                        llstCondition.Add("STEP_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_STEP_ID]));
                    }
                }
            }

            //STEP LIST
            if (llstData.Contains(Definition.CONDITION_KEY_STEP_ID_LIST))
            {
                object obj = llstData[Definition.CONDITION_KEY_STEP_ID_LIST];

                if (obj is string)
                {
                    sbQuery.Append(" AND A.STEP_ID= :STEP_ID");
                    llstCondition.Add("STEP_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_STEP_ID_LIST]));
                }
                else if (obj is IEnumerable)
                {
                    sbQuery.AppendFormat(" AND A.STEP_ID IN ({0}) ", _ComUtil.ConvertArrayIntoStringList((IEnumerable)obj, "'", ","));
                }
            }


            //PRODUCT
            if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null && llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString().ToUpper() != "DEFAULT_ALL")
            {
                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString().Length > 0)
                {
                    sbQuery.Append(" AND A.PRODUCT_ID= :PRODUCT_ID ");
                    llstCondition.Add("PRODUCT_ID", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_PRODUCT_ID]));
                }
                //else
                //{
                //    sbQuery.Append(" AND A.PRODUCT_ID IS NULL ");
                //}
            }

            //DCP
            if (llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD] != null)
            {
                sbQuery.Append(" AND A.MESSAGE_TYPE_CD= :MESSAGE_TYPE_CD");
                llstCondition.Add("MESSAGE_TYPE_CD", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD]));
            }

            //Additional Data Filter
            //if (llstData[Definition.CONDITION_KEY_DATA_FILTER] != null)
            //{
            //    sbQuery.Append(llstData[Definition.CONDITION_KEY_DATA_FILTER].ToString());
            //}

            if (llstData[Definition.CONDITION_KEY_START_DTTS] != null && llstData[Definition.CONDITION_KEY_END_DTTS] != null)
            {
                object obStartDtts = llstData[Definition.CONDITION_KEY_START_DTTS];
                object obEndDtts = llstData[Definition.CONDITION_KEY_END_DTTS];

                if (obStartDtts is DateTime)
                {
                    DateTime dtStart = (DateTime)llstData[Definition.CONDITION_KEY_START_DTTS];
                    llstCondition.Add("START_DTTS", dtStart);

                    sbQuery.Append(" AND A.SUM_DTTS >= :START_DTTS ");
                }
                else
                {
                    llstCondition.Add("START_DTTS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_START_DTTS]));

                    sbQuery.Append(" AND A.SUM_DTTS >= TO_TIMESTAMP(:START_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3') ");
                }

                if (obEndDtts is DateTime)
                {
                    DateTime dtEnd = (DateTime)llstData[Definition.CONDITION_KEY_END_DTTS];
                    llstCondition.Add("END_DTTS", dtEnd);

                    sbQuery.Append(" AND A.SUM_DTTS <= :END_DTTS ");
                }
                else
                {
                    llstCondition.Add("END_DTTS", _ComUtil.NVL(llstData[Definition.CONDITION_KEY_END_DTTS]));

                    sbQuery.Append(" AND A.SUM_DTTS <= TO_TIMESTAMP(:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3') ");
                }

                //sbQuery.Append(" AND A.SUM_DTTS BETWEEN (TO_TIMESTAMP (:START_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3')) AND (TO_TIMESTAMP (:END_DTTS, 'YYYY-MM-DD HH24:MI:SS.FF3'))");
            }

            sbQuery.Append(" ORDER BY A.SUM_DTTS  ");

            try
            {
                dsReturn = base.Query(sbQuery.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                DSUtil.SetResult(dsReturn, 0, "", ex.Message);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public DataSet GetEqpModuleInfoByParamAlias(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct EQPM.EQP_ID, EQPM.alias Chamber, EQPM.module_id EQP_MODULE_ID ");
            sb.Append(" FROM EQP_VW_SPC EQPM, EQP_SUM_PARAM_MST_PP EQPSPM                            ");
            sb.Append(" WHERE 1=1                                                                    ");
            sb.Append(" AND EQPM.eqp_id = EQPSPM.eqp_id                                              ");
            sb.Append(" AND EQPM.module_id = EQPSPM.eqp_module_id                                    ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.CONDITION_KEY_PARAM_ALIAS].ToString().Length > 0)
                {
                    sb.Append(" AND EQPSPM.SUM_PARAM_ALIAS = :PARAM_ALIAS ");
                    llstCondition.Add(Definition.CONDITION_KEY_PARAM_ALIAS, llparam[Definition.CONDITION_KEY_PARAM_ALIAS]);
                }
                if (llparam[Definition.CONDITION_KEY_LINE_RAWID].ToString().Length > 0)
                {
                    sb.Append(" AND EQPM.LINE_RAWID = :LINE_RAWID ");
                    llstCondition.Add(Definition.CONDITION_KEY_LINE_RAWID, llparam[Definition.CONDITION_KEY_LINE_RAWID]);
                }
                if (llparam[Definition.CONDITION_KEY_AREA_RAWID].ToString().Length > 0)
                {
                    sb.Append(" AND EQPM.AREA_RAWID = :AREA_RAWID ");
                    llstCondition.Add(Definition.CONDITION_KEY_AREA_RAWID, llparam[Definition.CONDITION_KEY_AREA_RAWID]);
                }

                sb.Append(" ORDER BY EQPM.eqp_id ");

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public DataSet GetRecipeStepByEqpModuleId(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();

            if (llparam.Contains(Definition.SUMMARY_DATA.DATA_EXIST_MODE))
            {
                string dataExistMode = llparam[Definition.SUMMARY_DATA.DATA_EXIST_MODE].ToString();

                if (dataExistMode.Equals(Definition.SUMMARY_DATA.ALL_EXIST))
                {
                    sb.Append("SELECT distinct eqp_recipe_id recipe, step_id step ");
                }
                else if (dataExistMode.Equals(Definition.SUMMARY_DATA.ONLY_RECIPE_EXIST))
                {
                    sb.Append("SELECT distinct eqp_recipe_id recipe, '*' step ");
                }
                else if (dataExistMode.Equals(Definition.SUMMARY_DATA.ONLY_STEP_EXIST))
                {
                    sb.Append("SELECT distinct '*' recipe, step_id step ");
                }
            }
            else
            {
                sb.Append("SELECT distinct '*' recipe, '*' step ");
            }

            sb.Append(" FROM EQP_RECIPE_STEP_MST_PP                       ");
            sb.Append(" WHERE 1=1                                         ");
            sb.Append(" AND eqp_recipe_id <> 'DEFAULT_ALL'                ");
            sb.Append(" AND step_id <> 'DEFAULT_ALL'                      ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.COL_EQP_MODULE_ID].ToString().Length > 0)
                {
                    sb.Append(" AND EQP_MODULE_ID = :EQP_MODULE_ID ");
                    llstCondition.Add(Definition.COL_EQP_MODULE_ID, llparam[Definition.COL_EQP_MODULE_ID]);
                }

                if (llparam.Contains(Definition.SUMMARY_DATA.DATA_EXIST_MODE))
                {
                    string dataExistMode = llparam[Definition.SUMMARY_DATA.DATA_EXIST_MODE].ToString();

                    if (dataExistMode.Equals(Definition.SUMMARY_DATA.ALL_EXIST))
                    {
                        sb.Append(" ORDER BY eqp_recipe_id, step_id ");
                    }
                    else if (dataExistMode.Equals(Definition.SUMMARY_DATA.ONLY_RECIPE_EXIST))
                    {
                        sb.Append(" ORDER BY eqp_recipe_id ");
                    }
                    else if (dataExistMode.Equals(Definition.SUMMARY_DATA.ONLY_STEP_EXIST))
                    {
                        sb.Append(" ORDER BY step_id ");
                    }
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }

                return dsReturn;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public DataSet GetConfigInfoByRawId(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append(" FROM MODEL_CONFIG_MST_SPC    ");
            sb.Append(" WHERE 1=1                                                                    ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.COL_RAW_ID].ToString().Length > 0)
                {
                    sb.Append(" AND rawid = :RAWID ");
                    llstCondition.Add(Definition.COL_RAW_ID, llparam[Definition.COL_RAW_ID]);
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public DataSet GetModuleNameByModuleId(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT MODULE_NAME ");
            sb.Append(" FROM EQP_VW_SPC   ");
            sb.Append(" WHERE 1=1         ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.COL_MODULE_ID].ToString().Length > 0)
                {
                    sb.Append(" AND module_id = :MODULE_ID ");
                    llstCondition.Add(Definition.COL_MODULE_ID, llparam[Definition.COL_MODULE_ID]);
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        public DataSet GetRestrictionFilter(string userRawid)
        {
            DataSet dsResult = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("select * from user_mst_pp where rawid = {0} ", userRawid));

                DataSet dsUserInfo = base.Query(sb.ToString());

                if (dsUserInfo != null && dsUserInfo.Tables.Count > 0 && dsUserInfo.Tables[0].Rows.Count > 0)
                {
                    //user group에 설정된 Data restriction filter 조건 가져옴.

                    sb.Remove(0, sb.Length);
                    sb.Append("SELECT A.RAWID, ZZ.GROUP_RAWID, A.USER_ID, ZZ.GROUP_ID   ");
                    sb.Append("FROM USER_MST_PP A, LOCATION_MST_PP E, AREA_MST_PP F,    ");
                    sb.Append("     (SELECT USER_RAWID, GROUP_ID, D.RAWID AS GROUP_RAWID    ");
                    sb.Append("      FROM USER_GROUP_LINK_MST_PP B, USER_COND_GROUPSET_MST_PP C, ");
                    sb.Append("       USER_GROUP_MST_PP D                                       ");
                    sb.Append("      WHERE B.END_DTTS > SYSTIMESTAMP                            ");
                    sb.Append("         AND C.USER_COND_RAWID = B.RAWID                         ");
                    sb.Append("         AND C.GROUP_RAWID = D.RAWID                             ");
                    sb.Append("         AND C.END_DTTS > SYSTIMESTAMP ) ZZ                      ");
                    sb.Append(" WHERE ZZ.USER_RAWID(+) = A.RAWID AND A.LOCATION_RAWID = E.RAWID(+)  ");
                    sb.Append("     AND A.AREA_RAWID = F.RAWID(+)                                   ");
                    sb.Append(string.Format("     AND A.RAWID = {0}                             ", userRawid));
                    sb.Append("     ORDER BY A.USER_ID                                          ");

                    DataSet dsUserGroup = base.Query(sb.ToString());

                    if (dsUserGroup != null && dsUserGroup.Tables.Count > 0 && dsUserGroup.Tables[0].Rows.Count > 0)
                    {
                        ArrayList arrGroup = new ArrayList();

                        foreach (DataRow dr in dsUserGroup.Tables[0].Rows)
                        {
                            arrGroup.Add(dr[Definition.DynamicCondition_Condition_key.GROUP_RAWID].ToString());
                        }

                        sb.Remove(0, sb.Length);
                        sb.Append("SELECT RAWID, USER_GROUP_RAWID, DATA_FILTER, OPTION_FILTER, EXCLUDE_YN, ");
                        sb.Append("DESCRIPTION, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, NVL(RESTRICT_TYPE_CD, 'DT') AS RESTRICT_TYPE_CD ");
                        sb.Append("FROM DATA_RESTRICT_MST_FDC WHERE USER_GROUP_RAWID IN( ");

                        for (int i = 0; i < arrGroup.Count; i++)
                        {
                            if (i == arrGroup.Count - 1)
                            {
                                sb.Append(arrGroup[i]);
                            }
                            else
                            {
                                sb.Append(arrGroup[i] + ",");
                            }
                        }

                        sb.Append(" )");

                        dsResult = base.Query(sb.ToString());

                        if (dsResult != null && dsResult.Tables.Count > 0)
                        {
                            DataSet dsModule;
                            ArrayList arrModuleName = new ArrayList();
                            ArrayList arrModuleId = new ArrayList();
                            ArrayList arrAlias = new ArrayList();

                            foreach (DataRow dr in dsResult.Tables[0].Rows)
                            {
                                string[] sFilter = dr[Definition.DATA_FILTER].ToString().Split('=');
                                if (sFilter[0] == Definition.CONDITION_KEY_EQP_ID)
                                {
                                    foreach (string sEqp in sFilter[1].Split(','))
                                    {
                                        string sQuery = string.Format("SELECT EQP_ID, MODULE_NAME, MODULE_ID, ALIAS FROM EQP_VW_SPC WHERE EQP_ID='{0}'", sEqp);
                                        dsModule = base.Query(sQuery);

                                        if (dsModule != null && dsModule.Tables.Count > 0)
                                        {
                                            foreach (DataRow drModule in dsModule.Tables[0].Rows)
                                            {
                                                if (!arrModuleName.Contains(drModule[Definition.CONDITION_KEY_MODULE_NAME].ToString()))
                                                {
                                                    arrModuleName.Add(drModule[Definition.CONDITION_KEY_MODULE_NAME].ToString());
                                                }
                                                if (!arrModuleId.Contains(drModule[Definition.CONDITION_KEY_MODULE_ID].ToString()))
                                                {
                                                    arrModuleId.Add(drModule[Definition.CONDITION_KEY_MODULE_ID].ToString());
                                                }
                                                if (!arrAlias.Contains(drModule[Definition.CONDITION_KEY_ALIAS].ToString()))
                                                {
                                                    arrAlias.Add(drModule[Definition.CONDITION_KEY_ALIAS].ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (arrModuleName.Count > 0)
                            {
                                DataRow dr = dsResult.Tables[0].NewRow();
                                string data = "MODULE_NAME=";

                                for (int i = 0; i < arrModuleName.Count; i++)
                                {
                                    data += arrModuleName[i].ToString() + ",";
                                }

                                data = data.Remove(data.Length - 1, 1);
                                dr[Definition.DATA_FILTER] = data;
                                dr[Definition.SpreadHeaderColKey.EXCLUDE_YN] = "N";
                                dr[Definition.CHART_COLUMN.RESTRICT_TYPE_CD] = "EQ";
                                dsResult.Tables[0].Rows.Add(dr);
                            }

                            if (arrModuleId.Count > 0)
                            {
                                DataRow dr = dsResult.Tables[0].NewRow();
                                string data = "MODULE_ID=";

                                for (int i = 0; i < arrModuleId.Count; i++)
                                {
                                    data += arrModuleId[i].ToString() + ",";
                                }

                                data = data.Remove(data.Length - 1, 1);
                                dr[Definition.DATA_FILTER] = data;
                                dr[Definition.SpreadHeaderColKey.EXCLUDE_YN] = "N";
                                dr[Definition.CHART_COLUMN.RESTRICT_TYPE_CD] = "EQ";
                                dsResult.Tables[0].Rows.Add(dr);
                            }

                            if (arrAlias.Count > 0)
                            {
                                DataRow dr = dsResult.Tables[0].NewRow();
                                string data = "MODULE_ALIAS=";

                                for (int i = 0; i < arrAlias.Count; i++)
                                {
                                    data += arrAlias[i].ToString() + ",";
                                }

                                data = data.Remove(data.Length - 1, 1);
                                dr[Definition.DATA_FILTER] = data;
                                dr[Definition.SpreadHeaderColKey.EXCLUDE_YN] = "N";
                                dr[Definition.CHART_COLUMN.RESTRICT_TYPE_CD] = "EQ";
                                dsResult.Tables[0].Rows.Add(dr);
                            }

                            dsResult.AcceptChanges();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        //SPC-930, KBLEE, START
        public DataSet GetRuleListByChartType(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT spc_rule_no, description ");
            sb.Append(" FROM RULE_MST_SPC              ");
            sb.Append(" WHERE 1=1                      ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.CHART_TYPE_STRING].ToString().Length > 0)
                {
                    sb.Append(" AND source_type_cd = :CHART_TYPE ");
                    llstCondition.Add(Definition.CHART_TYPE_STRING, llparam[Definition.CHART_TYPE_STRING]);
                }

                sb.Append(" ORDER BY spc_rule_no ");

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-930, KBLEE, END

        //SPC-930, KBLEE, START
        public DataSet GetRuleOptionByRuleNo(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llparam = new LinkedList();
            llparam.SetSerialData(baData);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT MRO.*                                                ");
            sb.Append("  FROM MODEL_RULE_MST_SPC MR, MODEL_RULE_OPT_MST_SPC MRO    ");
            sb.Append(" WHERE 1 = 1                                                ");
            sb.Append("   AND MR.rawid = MRO.model_rule_rawid                      ");
            sb.Append("   AND MR.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID          ");
            sb.Append("   AND MR.spc_rule_no = :SPC_RULE_NO                        ");
            sb.Append("   AND MRO.rule_option_no =                                 ");
            sb.Append("               (SELECT RO.rule_option_no                    ");
            sb.Append("                  FROM RULE_MST_SPC R, RULE_OPT_MST_SPC RO  ");
            sb.Append("                 WHERE 1 = 1                                ");
            sb.Append("                   AND R.rawid = RO.rule_rawid              ");
            sb.Append("                   AND R.spc_rule_no = :SPC_RULE_NO         ");
            sb.Append("                   AND RO.option_name = :OPTION_NAME )      ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                if (llparam[Definition.COL_MODEL_CONFIG_RAWID].ToString().Length > 0)
                {
                    llstCondition.Add(Definition.COL_MODEL_CONFIG_RAWID, llparam[Definition.COL_MODEL_CONFIG_RAWID]);
                }
                if (llparam[Definition.SPC_RULE_NO].ToString().Length > 0)
                {
                    llstCondition.Add(Definition.SPC_RULE_NO, llparam[Definition.SPC_RULE_NO]);
                }
                if (llparam[Definition.OPTION_NAME].ToString().Length > 0)
                {
                    llstCondition.Add(Definition.OPTION_NAME, llparam[Definition.OPTION_NAME]);
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }
        //SPC-930, KBLEE, END
    }

    public class SPCATTModelData : DataBase
    {
        CommonUtility _ComUtil = new CommonUtility();
        Common.CommonData _commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

        #region ::ATT Model Data Selection

        public DataSet GetATTSPCModelData(byte[] param)
        {
            DataSet dsReturn = new DataSet();


            StringBuilder sb = new StringBuilder();

            LinkedList llstParam = new LinkedList();

            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    llstCondition.Add("MODEL_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                //#00. MODEL_MST_SPC
                sb.Append("SELECT * FROM MODEL_ATT_MST_SPC ");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND RAWID = :MODEL_RAWID ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                //2009-12-07 bskwon 수정
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT mcms.rawid, mcms.model_rawid, mcms.param_alias,            ");
                sb.Append("        mcms.main_yn, mcms.auto_type_cd, mcms.auto_sub_yn,         ");
                sb.Append("        mcms.interlock_yn, mcms.sub_autocalc_yn,                   ");
                sb.Append("        mcms.activation_yn, mcms.sub_interlock_yn,                 ");
                sb.Append("        mcms.inherit_main_yn, mcms.offset_yn,                      ");
                sb.Append("        mcms.create_dtts, mcms.create_by,                          ");
                sb.Append("        mcms.last_update_dtts, mcms.last_update_by,                ");
                sb.Append("        mcms.autocalc_yn, mcms.chart_mode_cd,                      ");
                sb.Append("        mcms.VERSION,    mcms.validation_same_module_yn,           ");
                sb.Append("        mcms.chart_description,                                    ");
                sb.Append("        TRUNC (mcms.pn_usl, 16) PN_USL,                      ");
                sb.Append("        TRUNC (mcms.pn_lsl, 16) pn_LSL,                      ");
                sb.Append("        TRUNC (mcms.pn_target, 16) pn_target,                      ");
                sb.Append("        TRUNC (mcms.pn_ucl, 16) pn_ucl,                            ");
                sb.Append("        TRUNC (mcms.pn_lcl, 16) pn_lcl,                            ");
                sb.Append("        TRUNC (mcms.pn_center_line, 16) pn_center_line,            ");
                sb.Append("        TRUNC (mcms.c_usl, 16) c_USL,                        ");
                sb.Append("        TRUNC (mcms.c_lsl, 16) c_LSL,                        ");
                sb.Append("        TRUNC (mcms.c_target, 16) c_target,                        ");
                sb.Append("        TRUNC (mcms.c_ucl, 16) c_ucl,                              ");
                sb.Append("        TRUNC (mcms.c_lcl, 16) c_lcl,                              ");
                sb.Append("        TRUNC (mcms.c_center_line, 16) c_center_line,              ");
                sb.Append("        TRUNC (mcms.u_ucl, 16) u_ucl,                              ");
                sb.Append("        TRUNC (mcms.u_lcl, 16) u_lcl,                              ");
                sb.Append("        TRUNC (mcms.u_center_line, 16) u_center_line,              ");
                sb.Append("        TRUNC (mcms.p_ucl, 16) p_ucl,                              ");
                sb.Append("        TRUNC (mcms.p_lcl, 16) p_lcl,                              ");
                sb.Append("        TRUNC (mcms.p_center_line, 16) p_center_line,              ");
                sb.Append("        TRUNC (mcms.pn_usl_offset, 16) pn_usl_offset,              ");
                sb.Append("        TRUNC (mcms.pn_lsl_offset, 16) pn_lsl_offset,              ");
                sb.Append("        TRUNC (mcms.c_usl_offset, 16) c_usl_offset,                ");
                sb.Append("        TRUNC (mcms.c_lsl_offset, 16) c_lsl_offset,                ");
                sb.Append("        TRUNC (mcms.pn_ucl_offset, 16) pn_ucl_offset,              ");
                sb.Append("        TRUNC (mcms.pn_lcl_offset, 16) pn_lcl_offset,              ");
                sb.Append("        TRUNC (mcms.p_ucl_offset, 16) p_ucl_offset,                ");
                sb.Append("        TRUNC (mcms.p_lcl_offset, 16) p_lcl_offset,                ");
                sb.Append("        TRUNC (mcms.c_ucl_offset, 16) c_ucl_offset,                ");
                sb.Append("        TRUNC (mcms.c_lcl_offset, 16) c_lcl_offset,                ");
                sb.Append("        TRUNC (mcms.u_ucl_offset, 16) u_ucl_offset,                ");
                sb.Append("        TRUNC (mcms.u_lcl_offset, 16) u_lcl_offset                 ");
                sb.Append("   FROM model_config_att_mst_spc mcms                              ");
                sb.Append("  WHERE 1 = 1													  ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_RAWID = :MODEL_RAWID ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.RAWID");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.*, ");
                sb.Append("       B.NAME AS SPC_PARAM_CATEGORY, ");
                sb.Append("       C.NAME AS SPC_PRIORITY ");
                sb.Append("  FROM MODEL_CONFIG_OPT_ATT_MST_SPC A ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
                sb.Append("   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
                sb.Append("   ON (A.SPC_PRIORITY_CD = C.CODE) ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("  AND A.model_config_rawid IN (SELECT rawid FROM model_config_att_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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


                //#03. MODEL_CONTEXT_ATT_MST_SPC
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


                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.KEY_ORDER ASC ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT A.*, ");
                sb.Append("        B.DESCRIPTION, "); //add columns 
                sb.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC, "); //dev define columns
                sb.Append("        '' AS RULE_OPTION, '' AS RULE_OPTION_DATA "); //dev define columns
                sb.Append("   FROM MODEL_RULE_ATT_MST_SPC A ");
                sb.Append("        LEFT OUTER JOIN RULE_MST_SPC B ");
                sb.Append("          ON A.SPC_RULE_NO = B.SPC_RULE_NO ");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);

                sb.Append(" SELECT A.RAWID, A.MODEL_RULE_RAWID,  A.RULE_OPTION_NO, A.CREATE_DTTS, A.CREATE_BY,      ");
                sb.Append(" A.LAST_UPDATE_DTTS, A.LAST_UPDATE_BY,                                                   ");

                if (useComma)
                {
                    sb.Append(" REPLACE( A.RULE_OPTION_VALUE, ';', ',') AS RULE_OPTION_VALUE,                       ");
                }
                else
                {
                    sb.Append(" A.RULE_OPTION_VALUE,                                                                ");
                }

                sb.Append(" A.VERSION, A.SPC_RULE_NO, B.OPTION_NAME, B.DESCRIPTION                                  ");
                sb.Append("   FROM (SELECT MRO.*, MRM.SPC_RULE_NO                                                   ");
                sb.Append("           FROM MODEL_RULE_OPT_ATT_MST_SPC MRO,                                              ");
                sb.Append("                (SELECT MRM.RAWID, MRM.SPC_RULE_NO                                       ");
                sb.Append("                   FROM MODEL_RULE_ATT_MST_SPC MRM, MODEL_CONFIG_ATT_MST_SPC MCM                 ");
                sb.Append("                  WHERE 1 = 1                                                            ");
                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("   AND MCM.MODEL_RAWID = :MODEL_RAWID ");
                }
                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND MRM.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }
                sb.Append("                    AND MRM.MODEL_CONFIG_RAWID = MCM.RAWID) MRM                          ");
                sb.Append("          WHERE MRO.MODEL_RULE_RAWID(+) = MRM.RAWID) A,                                  ");
                sb.Append("        (SELECT A.RAWID AS RULE_RAWID, A.SPC_RULE_NO, B.RULE_OPTION_NO,                  ");
                sb.Append("                B.OPTION_NAME, B.DESCRIPTION                                             ");
                sb.Append("           FROM RULE_MST_SPC A, RULE_OPT_MST_SPC B                                       ");
                sb.Append("          WHERE A.RAWID = B.RULE_RAWID(+)) B                                             ");
                sb.Append("  WHERE A.SPC_RULE_NO = B.SPC_RULE_NO(+) AND A.RULE_OPTION_NO = B.RULE_OPTION_NO(+)      ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT * ");
                sb.Append("  FROM MODEL_AUTOCALC_ATT_MST_SPC ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_RAWID))
                {
                    sb.Append("  AND model_config_rawid IN (SELECT rawid FROM model_config_ATT_mst_spc WHERE model_rawid = :MODEL_RAWID) ");
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        #endregion

        #region ::ATT Default Model Selection

        public DataSet GetSPCDefaultModelData(byte[] param)
        {
            //CONFIGURATION 화면을 공통적으로 사용하기 위해
            //MODEL_* TABLE 을 기준으로 COLUMN의 NAME을 맞추도록 한다. 

            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();

            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                string sLineRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_LINE_RAWID]);
                string sAreaRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_AREA_RAWID]);
                string sEQPModel = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_EQP_MODEL]);

                llstCondition.Add("LINE_RAWID", sLineRawID);
                llstCondition.Add("AREA_RAWID", sAreaRawID);
                llstCondition.Add("EQP_MODEL", sEQPModel);


                //#00. MODEL_MST_SPC
                Common.CommonData comData = new Common.CommonData();
                DataTable dtModel = comData.GetTableSchema(TABLE.MODEL_ATT_MST_SPC);
                dtModel.TableName = TABLE.MODEL_ATT_MST_SPC;

                DataRow newRow = dtModel.NewRow();
                newRow[COLUMN.LOCATION_RAWID] = sLineRawID;
                newRow[COLUMN.AREA_RAWID] = sAreaRawID;
                newRow[COLUMN.EQP_MODEL] = sEQPModel;
                dtModel.Rows.Add(newRow);

                dsReturn.Tables.Add(dtModel);


                //#01. MODEL_CONFIG_MST_SPC
                sb.Remove(0, sb.Length);


                sb.Append(" SELECT '' AS model_rawid,'' AS param_alias,                                 ");
                sb.Append("        '' AS param_list, '' AS main_yn,                                     ");
                sb.Append("        'Y' AS activation_yn, 'Y' AS sub_interlock_yn,                       ");
                sb.Append("        'Y' AS inherit_main_yn, 'Y' AS sub_autocalc_yn,                      ");
                sb.Append("        'N' AS validation_same_module_yn,'' AS chart_description, a.*        ");
                sb.Append("   FROM def_config_att_mst_spc a                                             ");
                sb.Append("  WHERE 1 = 1                                                                ");
                sb.Append("    AND location_rawid = :line_rawid                                         ");
                sb.Append("    AND area_rawid = :area_rawid                                             ");
                sb.Append("    AND NVL (eqp_model, '^') = NVL (:eqp_model, '^')                         ");


                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT a.rawid, a.def_config_rawid AS model_config_rawid,                     ");
                sb.Append("       a.spc_param_category_cd,a.spc_priority_cd,a.auto_cpk_yn,               ");
                sb.Append("       a.default_chart_list, a.restrict_sample_count,                         ");
                sb.Append("       a.restrict_sample_days, b.NAME AS spc_param_category,                  ");
                sb.Append("       c.NAME AS spc_priority, a.restrict_sample_hours                        ");
                sb.Append("  FROM def_config_opt_att_mst_spc a                                           ");
                sb.Append("       LEFT OUTER JOIN                                                        ");
                sb.Append("       (SELECT code, NAME                                                     ");
                sb.Append("          FROM code_mst_pp                                                    ");
                sb.Append("         WHERE CATEGORY = 'SPC_PARAM_CATEGORY') b                             ");
                sb.Append("       ON (a.spc_param_category_cd = b.code)                                  ");
                sb.Append("       LEFT OUTER JOIN                                                        ");
                sb.Append("       (SELECT code, NAME                                                     ");
                sb.Append("          FROM code_mst_pp                                                    ");
                sb.Append("         WHERE CATEGORY = 'SPC_PRIOTIRY') c ON (a.spc_priority_cd = c.code)   ");
                sb.Append(" WHERE 1 = 1                                                                  ");
                sb.Append("   AND a.def_config_rawid =                                                   ");
                sb.Append("          (SELECT rawid                                                       ");
                sb.Append("             FROM def_config_att_mst_spc                                      ");
                sb.Append("            WHERE location_rawid = :line_rawid                                ");
                sb.Append("              AND area_rawid = :area_rawid                                    ");
                sb.Append("              AND NVL (eqp_model, '^') = NVL (:eqp_model, '^'))               ");


                dsTemp = this.Query(sb.ToString(), llstCondition);

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

                //#03. MODEL_CONTEXT_ATT_MST_SPC
                sb.Remove(0, sb.Length);

                sb.Append("SELECT dcms.rawid,                                   ");
                sb.Append("       dcms.def_config_rawid AS model_config_rawid, dcms.context_key,  ");

                if (useComma)
                {
                    sb.Append("  replace(dcms.context_value, ';', ',') as context_value,          ");

                }
                else
                {
                    sb.Append("       dcms.context_value,                           ");
                }
                sb.Append("       dcms.key_order,                               ");

                if (useComma)
                {
                    sb.Append("  replace(dcms.exclude_list, ';', ',') as exclude_list,          ");
                }
                else
                {
                    sb.Append("       dcms.exclude_list,                            ");
                }

                sb.Append("       dcms.group_yn, aa.NAME AS context_key_name    ");
                sb.Append("  FROM def_context_att_mst_spc dcms,                 ");
                sb.Append("       (SELECT code, NAME                            ");
                sb.Append("          FROM code_mst_pp                           ");
                sb.Append("         WHERE CATEGORY = 'CONTEXT_TYPE'             ");
                sb.Append("        UNION                                        ");
                sb.Append("        SELECT code, NAME                            ");
                sb.Append("          FROM code_mst_pp                           ");
                sb.Append("         WHERE CATEGORY = 'SPC_CONTEXT_TYPE') aa     ");
                sb.Append(" WHERE dcms.context_key = aa.code                    ");
                sb.Append("   AND DCMS.DEF_CONFIG_RAWID = (SELECT RAWID FROM DEF_CONFIG_ATT_MST_SPC WHERE LOCATION_RAWID = :LINE_RAWID AND AREA_RAWID = :AREA_RAWID AND NVL(EQP_MODEL, '^') = NVL(:EQP_MODEL,'^') ) ");

                sb.Append(" ORDER BY DCMS.KEY_ORDER ASC ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);

                sb.Append("SELECT a.rawid, a.def_config_rawid AS model_config_rawid, a.spc_rule_no,                ");
                sb.Append("       a.ocap_list, a.use_main_spec_yn, b.description,                                  ");
                sb.Append("       DECODE (a.use_main_spec_yn,'Y', 'True','N', 'False','True') AS use_main_spec,    ");
                sb.Append("       '' AS rule_option, '' AS rule_option_data, a.rule_order                          ");
                sb.Append("  FROM def_rule_att_mst_spc a LEFT OUTER JOIN rule_mst_spc b                            ");
                sb.Append("       ON a.spc_rule_no = b.spc_rule_no                                                 ");
                sb.Append(" WHERE 1 = 1                                                                            ");
                sb.Append("   AND a.def_config_rawid =                                                             ");
                sb.Append("          (SELECT rawid                                                                 ");
                sb.Append("             FROM def_config_att_mst_spc                                                ");
                sb.Append("            WHERE location_rawid = :line_rawid                                          ");
                sb.Append("              AND area_rawid = :area_rawid                                              ");
                sb.Append("              AND NVL (eqp_model, '^') = NVL (:eqp_model, '^'))                         ");


                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);

                sb.Append("SELECT b.rawid, b.def_rule_rawid AS model_rule_rawid, b.rule_option_no,    ");

                if (useComma)
                {
                    sb.Append("  replace(b.rule_option_value, ';', ',') as rule_option_value,          ");
                }
                else
                {
                    sb.Append("       b.rule_option_value,   ");
                }

                sb.Append("       a.spc_rule_no, c.option_name, c.description    ");
                sb.Append("  FROM def_rule_att_mst_spc a LEFT OUTER JOIN def_rule_opt_att_mst_spc b   ");
                sb.Append("       ON (a.rawid = b.def_rule_rawid)                                     ");
                sb.Append("       LEFT OUTER JOIN                                                     ");
                sb.Append("       (SELECT a.rawid AS rule_rawid, a.spc_rule_no, b.rule_option_no,     ");
                sb.Append("               b.option_name, b.description                                ");
                sb.Append("          FROM rule_mst_spc a LEFT OUTER JOIN rule_opt_mst_spc b           ");
                sb.Append("               ON (a.rawid = b.rule_rawid)                                 ");
                sb.Append("               ) c                                                         ");
                sb.Append("       ON (    a.spc_rule_no = c.spc_rule_no                               ");
                sb.Append("           AND b.rule_option_no = c.rule_option_no                         ");
                sb.Append("          )                                                                ");
                sb.Append(" WHERE 1 = 1                                                               ");
                sb.Append("   AND a.def_config_rawid =                                                ");
                sb.Append("          (SELECT rawid                                                    ");
                sb.Append("             FROM def_config_att_mst_spc                                   ");
                sb.Append("            WHERE location_rawid = :line_rawid                             ");
                sb.Append("              AND area_rawid = :area_rawid                                 ");
                sb.Append("              AND NVL (eqp_model, '^') = NVL (:eqp_model, '^'))            ");


                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT a.rawid, a.def_config_rawid AS model_config_rawid,         ");
                sb.Append("       a.autocalc_period,a.calc_count, a.min_samples,             ");
                sb.Append("       a.default_period, a.max_period, a.control_limit,           ");
                sb.Append("       a.control_threshold, A.P_CL_YN, A.PN_CL_YN,                ");
                sb.Append("       A.C_CL_YN, A.U_CL_YN, a.shift_calc_yn,                     ");
                sb.Append("       NVL (a.threshold_off_yn, 'N') AS threshold_off_yn,         ");
                sb.Append("       a.calc_count AS initial_calc_count                                   ");
                sb.Append("  FROM def_autocalc_att_mst_spc a                                 ");
                sb.Append(" WHERE 1 = 1                                                      ");
                sb.Append("   AND def_config_rawid =                                         ");
                sb.Append("          (SELECT rawid                                           ");
                sb.Append("             FROM def_config_att_mst_spc                          ");
                sb.Append("            WHERE location_rawid = :line_rawid                    ");
                sb.Append("              AND area_rawid = :area_rawid                        ");
                sb.Append("              AND NVL (eqp_model, '^') = NVL (:eqp_model, '^'))   ");

                dsTemp = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dAutoCalc = dsTemp.Tables[0].Copy();
                    dAutoCalc.TableName = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;

                    dsReturn.Tables.Add(dAutoCalc);
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
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        #endregion

        public DataSet GetSPCParamList(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            LinkedList llstParam = new LinkedList();
            llstParam.SetSerialData(param);

            string sLineRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_LINE_RAWID]);
            string sAreaRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_AREA_RAWID]);
            string sEQPModel = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_EQP_MODEL]);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT DISTINCT PARAM_TYPE_CD, PARAM_TYPE, LOCATION_RAWID, AREA_RAWID, PARAM_ALIAS, DATA_TYPE_CD, DATA_TYPE, UNIT ");
            sb.Append("  FROM PARAM_VW_SPC ");
            sb.Append(" WHERE 1 = 1 ");
            sb.Append("   AND PARAM_TYPE_CD = :PARAM_TYPE_CD ");
            //sb.Append("   AND PARAM_TYPE_CD = :PARAM_TYPE_CD ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                llstCondition.Add("PARAM_TYPE_CD", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_PARAM_TYPE_CD]));

                if (sLineRawID.Length > 0)
                {
                    sb.Append("   AND LOCATION_RAWID = :LINE_RAWID ");
                    llstCondition.Add("LINE_RAWID", sLineRawID);
                }
                if (sAreaRawID.Length > 0)
                {
                    sb.Append("   AND AREA_RAWID = :AREA_RAWID ");
                    llstCondition.Add("AREA_RAWID", sAreaRawID);
                }

                if (sEQPModel.Length > 0)
                {
                    sb.Append("   AND EQP_MODEL = :EQP_MODEL ");
                    llstCondition.Add("EQP_MODEL", sEQPModel);
                }

                dsReturn = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        #region ::CheckDuplicateATTSPCModel

        private const string SQL_GET_DUPLICATE_SPC_ATT_MODEL_RAWID = " SELECT   DISTINCT A.RAWID, A.SPC_MODEL_NAME "
                                                            + "     FROM MODEL_ATT_MST_SPC A, MODEL_CONFIG_ATT_MST_SPC B, MODEL_CONTEXT_ATT_MST_SPC C     "
                                                            + "    WHERE A.RAWID = B.MODEL_RAWID                                              "
                                                            + "      AND B.RAWID = C.MODEL_CONFIG_RAWID                                       "
                                                            + "      AND B.MAIN_YN = 'Y'                                                      "
                                                            + "      AND A.LOCATION_RAWID = :LOCATIONRAWID                                    "
                                                            + "      AND NVL (A.AREA_RAWID, -1) = :AREARAWID                                  "
                                                            + "      AND NVL (A.EQP_MODEL, '-') = :EQPMODEL                                   "
                                                            + "      AND B.PARAM_ALIAS = :PARAMALIAS                                          "
                                                            + "      {0}                                                                      "
                                                            + " ORDER BY A.RAWID                                                              ";

        private const string SQL_GET_DUPLICATE_SPC_ATT_MODEL = " SELECT   C.RAWID, C.CONTEXT_KEY, C.CONTEXT_VALUE, C.KEY_ORDER                    "
                                                            + "     FROM MODEL_ATT_MST_SPC A, MODEL_CONFIG_ATT_MST_SPC B, MODEL_CONTEXT_ATT_MST_SPC C     "
                                                            + "    WHERE A.RAWID = B.MODEL_RAWID                                              "
                                                            + "      AND B.RAWID = C.MODEL_CONFIG_RAWID                                       "
                                                            + "      AND B.MAIN_YN = 'Y'                                                      "
                                                            + "      AND B.ACTIVATION_YN = 'Y'                                                "
                                                            + "      AND A.RAWID = :RAWID                                                     "
                                                            + "      ORDER BY C.KEY_ORDER                                                     ";

        private const string SQL_GET_DUPLICATE_SPC_ATT_NAME = " SELECT   DISTINCT RAWID, SPC_MODEL_NAME                                          "
                                                           + "     FROM MODEL_ATT_MST_SPC                                                        "
                                                           + "    WHERE LOCATION_RAWID = :LOCATIONRAWID                                      "
                                                           + "      AND NVL (AREA_RAWID, -1) = :AREARAWID                                    "
                                                           + "      AND NVL (EQP_MODEL, '-') = :EQPMODEL                                     "
                                                           + "      AND SPC_MODEL_NAME = :SPCMODELNAME                                       ";


        public string CheckDuplicateSPCModel(byte[] param)
        {

            bool bResult = true;
            string strResult = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                string strLocationRawID = "";
                string strAreaRawID = "";
                string strEQPModel = "";
                string strParamAlias = "";
                string strSubWhere = "";
                string strModelRawID = "";
                string strModelName = "";
                int idx = 0;

                DataTable dtModel = (DataTable)llstParam[TABLE.MODEL_ATT_MST_SPC];
                DataTable dtConfig = (DataTable)llstParam[TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtContext = (DataTable)llstParam[TABLE.MODEL_CONTEXT_ATT_MST_SPC];

                foreach (DataRow drModel in dtModel.Rows)
                {
                    strModelName = drModel[COLUMN.SPC_MODEL_NAME].ToString();
                    strModelRawID = drModel[COLUMN.RAWID].ToString();
                    strLocationRawID = drModel[COLUMN.LOCATION_RAWID].ToString();
                    strAreaRawID = drModel[COLUMN.AREA_RAWID].ToString();
                    strEQPModel = drModel[COLUMN.EQP_MODEL].ToString();

                    if (strAreaRawID == null || strAreaRawID == "")
                        strAreaRawID = "-1";

                    if (strEQPModel == null || strEQPModel == "")
                        strEQPModel = "-";
                }

                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    strParamAlias = drConfig[COLUMN.PARAM_ALIAS].ToString();
                }

                foreach (DataRow drContext in dtContext.Rows)
                {
                    string strTempContextKey = drContext[COLUMN.CONTEXT_KEY].ToString();
                    string strTempContextValue = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    string strTempKeyOrder = drContext[COLUMN.KEY_ORDER].ToString();

                    if (idx == 0)
                    {
                        strSubWhere += string.Format(" AND ((C.CONTEXT_KEY = '{0}' AND C.CONTEXT_VALUE = '{1}' AND C.KEY_ORDER = {2} ", strTempContextKey, strTempContextValue, strTempKeyOrder);
                    }
                    else
                    {
                        strSubWhere += string.Format(" ) OR (C.CONTEXT_KEY = '{0}' AND C.CONTEXT_VALUE = '{1}' AND C.KEY_ORDER = {2} ", strTempContextKey, strTempContextValue, strTempKeyOrder);
                    }
                    idx++;
                }

                if (strSubWhere.Length > 0)
                    strSubWhere += " )) ";

                LinkedList lnkListWhere = new LinkedList();
                lnkListWhere.Add("LOCATIONRAWID", strLocationRawID);
                lnkListWhere.Add("AREARAWID", strAreaRawID);
                lnkListWhere.Add("EQPMODEL", strEQPModel);
                lnkListWhere.Add("SPCMODELNAME", strModelName);

                string strQuery = SQL_GET_DUPLICATE_SPC_ATT_NAME;

                if (strModelRawID != null && strModelRawID.Length > 0)
                {
                    strQuery += "      AND RAWID <> :RAWID                                                      ";
                    lnkListWhere.Add("RAWID", strModelRawID);
                }

                DataSet dsTempReturn = base.Query(strQuery, lnkListWhere);
                if (dsTempReturn != null && dsTempReturn.Tables.Count > 0)
                {
                    if (dsTempReturn.Tables[0].Rows.Count == 0)
                    {
                        bResult = false;
                    }
                    else
                    {
                        for (int i = 0; i < dsTempReturn.Tables[0].Rows.Count; i++)
                        {
                            string strTempSPCModelName = dsTempReturn.Tables[0].Rows[i][COLUMN.SPC_MODEL_NAME].ToString();
                            if (strTempSPCModelName.Length > 0)
                                return strTempSPCModelName;
                        }
                    }

                }

                lnkListWhere.Clear();
                lnkListWhere.Add("LOCATIONRAWID", strLocationRawID);
                lnkListWhere.Add("AREARAWID", strAreaRawID);
                lnkListWhere.Add("EQPMODEL", strEQPModel);
                lnkListWhere.Add("PARAMALIAS", strParamAlias);

                if (strModelRawID != null && strModelRawID.Length > 0)
                {
                    strSubWhere += "      AND A.RAWID <> :RAWID ";
                    lnkListWhere.Add("RAWID", strModelRawID);
                }

                DataSet dsReturn = base.Query(string.Format(SQL_GET_DUPLICATE_SPC_ATT_MODEL_RAWID, strSubWhere), lnkListWhere);

                if (dsReturn != null && dsReturn.Tables.Count > 0)
                {
                    if (dsReturn.Tables[0].Rows.Count == 0)
                    {
                        bResult = false;
                    }
                    else
                    {
                        for (int i = 0; i < dsReturn.Tables[0].Rows.Count; i++)
                        {
                            string strTempRawid = dsReturn.Tables[0].Rows[i]["RAWID"].ToString();
                            string strTempSPCModelName = dsReturn.Tables[0].Rows[i]["SPC_MODEL_NAME"].ToString();

                            lnkListWhere.Clear();
                            lnkListWhere.Add("RAWID", strTempRawid);

                            DataSet dsTemp = base.Query(SQL_GET_DUPLICATE_SPC_ATT_MODEL, lnkListWhere);

                            if (dsTemp != null && dsTemp.Tables.Count > 0)
                            {
                                if (dsTemp.Tables[0].Rows.Count == idx)
                                {
                                    for (int k = 0; k < dsTemp.Tables[0].Rows.Count; k++)
                                    {
                                        if (dsTemp.Tables[0].Rows[k][COLUMN.CONTEXT_KEY].ToString() == dtContext.Rows[k][COLUMN.CONTEXT_KEY].ToString()
                                            && dsTemp.Tables[0].Rows[k][COLUMN.CONTEXT_VALUE].ToString() == dtContext.Rows[k][COLUMN.CONTEXT_VALUE].ToString()
                                            && dsTemp.Tables[0].Rows[k][COLUMN.KEY_ORDER].ToString() == dtContext.Rows[k][COLUMN.KEY_ORDER].ToString())
                                        {
                                            bResult = true;
                                            if (k == dsTemp.Tables[0].Rows.Count - 1)
                                            {
                                                strResult = strTempSPCModelName;
                                                return strResult;
                                            }
                                        }
                                        else
                                        {
                                            bResult = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    bResult = false;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                bResult = true;
            }

            return strResult;
        }

        #endregion

        #region ::SaveCreate ATT Model

        public DataSet CreateSPCModel(BISTel.eSPC.Common.ConfigMode configMode, string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, ref string ref_Config_RawID, string groupRawId, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();

            try
            {
                base.BeginTrans();

                //#01. MODEL_ATT_MST_SPC
                foreach (DataRow drModel in dtModel.Rows)
                {
                    decimal modelRawID = 0;

                    if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.SAVE_AS))
                    {
                        modelRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_ATT_MST_SPC);

                        //llstFieldData.Add(COLUMN., drModel[COLUMN.]);

                        llstFieldData.Add(COLUMN.RAWID, modelRawID);

                        llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
                        llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, drModel[COLUMN.LOCATION_RAWID]);
                        llstFieldData.Add(COLUMN.AREA_RAWID, drModel[COLUMN.AREA_RAWID]);
                        llstFieldData.Add(COLUMN.EQP_MODEL, drModel[COLUMN.EQP_MODEL]);

                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        //SPC-1292, KBLEE, START
                        if (groupRawId != null)
                        {
                            llstFieldData.Add(COLUMN.GROUP_RAWID, groupRawId);
                        }
                        //SPC-1292, KBLEE, END

                        base.Insert(TABLE.MODEL_ATT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        drModel[COLUMN.RAWID] = modelRawID;
                    }
                    else if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_SUB))
                    {
                        modelRawID = decimal.Parse(drModel[COLUMN.RAWID].ToString());
                    }

                    //#02. MODEL_CONFIG_ATT_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        decimal configRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_CONFIG_ATT_MST_SPC);
                        ref_Config_RawID = configRawID.ToString();
                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, configRawID);
                        llstFieldData.Add(COLUMN.MODEL_RAWID, modelRawID);

                        //SPC-676 by Louis
                        llstFieldData.Add(COLUMN.CHART_DESCRIPTON, drConfig[COLUMN.CHART_DESCRIPTON]);

                        llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
                        llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
                        llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
                        llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.SUB_AUTOCALC_YN, drConfig[COLUMN.SUB_AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);

                        llstFieldData.Add(COLUMN.PN_UPPERSPEC, drConfig[COLUMN.PN_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.PN_LOWERSPEC, drConfig[COLUMN.PN_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.PN_TARGET, drConfig[COLUMN.PN_TARGET]);

                        llstFieldData.Add(COLUMN.PN_UCL, drConfig[COLUMN.PN_UCL]);
                        llstFieldData.Add(COLUMN.PN_LCL, drConfig[COLUMN.PN_LCL]);
                        llstFieldData.Add(COLUMN.PN_CENTER_LINE, drConfig[COLUMN.PN_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.P_UCL, drConfig[COLUMN.P_UCL]);
                        llstFieldData.Add(COLUMN.P_LCL, drConfig[COLUMN.P_LCL]);
                        llstFieldData.Add(COLUMN.P_CENTER_LINE, drConfig[COLUMN.P_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.C_UPPERSPEC, drConfig[COLUMN.C_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.C_LOWERSPEC, drConfig[COLUMN.C_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.C_TARGET, drConfig[COLUMN.C_TARGET]);

                        llstFieldData.Add(COLUMN.C_UCL, drConfig[COLUMN.C_UCL]);
                        llstFieldData.Add(COLUMN.C_LCL, drConfig[COLUMN.C_LCL]);
                        llstFieldData.Add(COLUMN.C_CENTER_LINE, drConfig[COLUMN.C_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.U_UCL, drConfig[COLUMN.U_UCL]);
                        llstFieldData.Add(COLUMN.U_LCL, drConfig[COLUMN.U_LCL]);
                        llstFieldData.Add(COLUMN.U_CENTER_LINE, drConfig[COLUMN.U_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.PN_UCL_OFFSET, drConfig[COLUMN.PN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LCL_OFFSET, drConfig[COLUMN.PN_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.PN_USL_OFFSET, drConfig[COLUMN.PN_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LSL_OFFSET, drConfig[COLUMN.PN_LSL_OFFSET]);


                        llstFieldData.Add(COLUMN.P_UCL_OFFSET, drConfig[COLUMN.P_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.P_LCL_OFFSET, drConfig[COLUMN.P_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.C_USL_OFFSET, drConfig[COLUMN.C_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LSL_OFFSET, drConfig[COLUMN.C_LSL_OFFSET]);

                        llstFieldData.Add(COLUMN.C_UCL_OFFSET, drConfig[COLUMN.C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LCL_OFFSET, drConfig[COLUMN.C_LCL_OFFSET]);


                        llstFieldData.Add(COLUMN.U_UCL_OFFSET, drConfig[COLUMN.U_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.U_LCL_OFFSET, drConfig[COLUMN.U_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);

                        //---------------------------------------------------------------------------------


                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);

                        llstFieldData.Add(COLUMN.VALIDATION_SAME_MODULE_YN, drConfig[COLUMN.VALIDATION_SAME_MODULE_YN]);



                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        base.Insert(TABLE.MODEL_CONFIG_ATT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //#03. MODEL_CONFIG_OPT_ATT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_ATT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

                            //SPC-721 By Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }


                        //#04. MODEL_CONTEXT_ATT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_ATT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            if (useComma)   //spc1281 use commaa == ture 인경우 dataset에 저장하기 전 ,를 ;로 변경.
                            {
                                string contextValues = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = contextValues.Replace(",", ";");

                                string excludeList = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = excludeList.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONTEXT_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#05. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_ATT_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_RULE_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_ATT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string ruleOPTValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = ruleOPTValue.Replace(",", ";");
                                }
                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#06. MODEL_AUTOCALC_ATT_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_AUTOCALC_ATT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

                            llstFieldData.Add(COLUMN.PN_CL_YN, drAutoCalc[COLUMN.PN_CL_YN]);
                            llstFieldData.Add(COLUMN.P_CL_YN, drAutoCalc[COLUMN.P_CL_YN]);
                            llstFieldData.Add(COLUMN.C_CL_YN, drAutoCalc[COLUMN.C_CL_YN]);
                            llstFieldData.Add(COLUMN.U_CL_YN, drAutoCalc[COLUMN.U_CL_YN]);
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);
                            llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);
                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }
                }

                base.Commit();

                //새로만들었을 경우 MODEL의 Rawid를 넘겨주기위해 model table을 return해준다.
                dsResult.Tables.Add(dtModel.Copy());
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear();
                llstFieldData = null;
            }

            return dsResult;
        }

        #endregion

        #region ::Save Modify ATT Model

        public DataSet ModifySPCModel(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, List<string> lstChangedMasterColList, string groupRawid, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                //base.BeginTrans();


                //#01. MODEL_MST_SPC
                foreach (DataRow drModel in dtModel.Rows)
                {
                    llstFieldData.Add(COLUMN.SPC_MODEL_NAME, drModel[COLUMN.SPC_MODEL_NAME]);
                    llstFieldData.Add(COLUMN.DESCRIPTION, drModel[COLUMN.DESCRIPTION]);
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    //SPC-1292, KBLEE, START
                    if (groupRawid != null)
                    {
                        llstFieldData.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID, groupRawid);
                    }
                    else
                    {
                        llstFieldData.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID, "");
                    }
                    //SPC-1292, KBLEE, END

                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                    llstWhereData.Add(COLUMN.RAWID, drModel[COLUMN.RAWID]);

                    base.Update(TABLE.MODEL_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        //base.RollBack();
                        return dsResult;
                    }

                    //#02. MODEL_CONFIG_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        llstFieldData.Clear();
                        llstWhereData.Clear();

                        string configRawID = drConfig[COLUMN.RAWID].ToString();

                        llstFieldData.Add(COLUMN.CHART_DESCRIPTON, drConfig[COLUMN.CHART_DESCRIPTON]);
                        llstFieldData.Add(COLUMN.PARAM_ALIAS, drConfig[COLUMN.PARAM_ALIAS]);
                        llstFieldData.Add(COLUMN.MAIN_YN, drConfig[COLUMN.MAIN_YN]);
                        llstFieldData.Add(COLUMN.AUTO_TYPE_CD, drConfig[COLUMN.AUTO_TYPE_CD]);
                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.ACTIVATION_YN, drConfig[COLUMN.ACTIVATION_YN]);
                        llstFieldData.Add(COLUMN.INHERIT_MAIN_YN, drConfig[COLUMN.INHERIT_MAIN_YN]);
                        llstFieldData.Add(COLUMN.SUB_INTERLOCK_YN, drConfig[COLUMN.SUB_INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.SUB_AUTOCALC_YN, drConfig[COLUMN.SUB_AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.PN_UPPERSPEC, drConfig[COLUMN.PN_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.PN_LOWERSPEC, drConfig[COLUMN.PN_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.PN_TARGET, drConfig[COLUMN.PN_TARGET]);
                        llstFieldData.Add(COLUMN.PN_UCL, drConfig[COLUMN.PN_UCL]);
                        llstFieldData.Add(COLUMN.PN_LCL, drConfig[COLUMN.PN_LCL]);
                        llstFieldData.Add(COLUMN.PN_CENTER_LINE, drConfig[COLUMN.PN_CENTER_LINE]);
                        llstFieldData.Add(COLUMN.P_UCL, drConfig[COLUMN.P_UCL]);
                        llstFieldData.Add(COLUMN.P_LCL, drConfig[COLUMN.P_LCL]);
                        llstFieldData.Add(COLUMN.P_CENTER_LINE, drConfig[COLUMN.P_CENTER_LINE]);
                        llstFieldData.Add(COLUMN.C_UPPERSPEC, drConfig[COLUMN.C_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.C_LOWERSPEC, drConfig[COLUMN.C_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.C_TARGET, drConfig[COLUMN.C_TARGET]);
                        llstFieldData.Add(COLUMN.C_UCL, drConfig[COLUMN.C_UCL]);
                        llstFieldData.Add(COLUMN.C_LCL, drConfig[COLUMN.C_LCL]);
                        llstFieldData.Add(COLUMN.C_CENTER_LINE, drConfig[COLUMN.C_CENTER_LINE]);
                        llstFieldData.Add(COLUMN.U_UCL, drConfig[COLUMN.U_UCL]);
                        llstFieldData.Add(COLUMN.U_LCL, drConfig[COLUMN.U_LCL]);
                        llstFieldData.Add(COLUMN.U_CENTER_LINE, drConfig[COLUMN.U_CENTER_LINE]);
                        llstFieldData.Add(COLUMN.PN_UCL_OFFSET, drConfig[COLUMN.PN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LCL_OFFSET, drConfig[COLUMN.PN_LCL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_USL_OFFSET, drConfig[COLUMN.PN_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LSL_OFFSET, drConfig[COLUMN.PN_LSL_OFFSET]);
                        llstFieldData.Add(COLUMN.P_UCL_OFFSET, drConfig[COLUMN.P_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.P_LCL_OFFSET, drConfig[COLUMN.P_LCL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_USL_OFFSET, drConfig[COLUMN.C_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LSL_OFFSET, drConfig[COLUMN.C_LSL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_UCL_OFFSET, drConfig[COLUMN.C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LCL_OFFSET, drConfig[COLUMN.C_LCL_OFFSET]);
                        llstFieldData.Add(COLUMN.U_UCL_OFFSET, drConfig[COLUMN.U_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.U_LCL_OFFSET, drConfig[COLUMN.U_LCL_OFFSET]);
                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);
                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);
                        llstFieldData.Add(COLUMN.VALIDATION_SAME_MODULE_YN, drConfig[COLUMN.VALIDATION_SAME_MODULE_YN]);
                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                        llstWhereData.Add(COLUMN.RAWID, drConfig[COLUMN.RAWID]);

                        base.Update(TABLE.MODEL_CONFIG_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#02-1. MODEL_CONFIG_HST_SPC
                        //       MODEL_CONFIG_MST에서 MASTER VALUE가 변경되었을 경우 변경된 내용만 MODEL_CONFIG_HST_SPC에 INSERT한다.
                        //if (lstChangedMasterColList != null && lstChangedMasterColList.Count > 0)
                        //{
                        //    llstFieldData.Clear();

                        //    llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_HST_SPC), "");
                        //    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                        //    foreach (string colList in lstChangedMasterColList)
                        //    {
                        //        llstFieldData.Add(colList, drConfig[colList]);
                        //    }

                        //    llstFieldData.Add(COLUMN.MODIFIED_TYPE_CD, "MAN");
                        //    llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        //    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        //    base.Insert(TABLE.MODEL_CONFIG_HST_SPC, llstFieldData);

                        //    if (base.ErrorMessage.Length > 0)
                        //    {
                        //        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        //        //base.RollBack();
                        //        return dsResult;
                        //    }
                        //}


                        //#03. MODEL_CONFIG_OPT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstWhereData.Clear();
                            //llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONFIG_OPT_MST_SPC), "");
                            //llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);
                            //SPC-712 by Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                            sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                            llstWhereData.Add(COLUMN.RAWID, drConfigOpt[COLUMN.RAWID]);

                            base.Update(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }


                        //#04. MODEL_CONTEXT_MST_SPC DATA 삭제
                        llstWhereData.Clear();

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
                        llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                        base.Delete(TABLE.MODEL_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#05. MODEL_CONTEXT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();

                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_CONTEXT_ATT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);
                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            if (useComma)   //spc1281 use commaa == ture 인경우 dataset에 저장하기 전 ,를 ;로 변경.
                            {
                                string contextValues = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = contextValues.Replace(",", ";");

                                string excludeList = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = excludeList.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_CONTEXT_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }


                        //#06. MODEL_RULE_MST_SPC, MODEL_RULE_OPT_MST_SPC DATA 삭제
                        llstWhereData.Clear();
                        sWhereQuery = string.Format("WHERE {0} IN (SELECT RAWID FROM {1} WHERE {2} = :{2}) ", COLUMN.MODEL_RULE_RAWID,
                                                                                                              TABLE.MODEL_RULE_ATT_MST_SPC,
                                                                                                              COLUMN.MODEL_CONFIG_RAWID);
                        llstWhereData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                        base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);

                        base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            //base.RollBack();
                            return dsResult;
                        }

                        //#07. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_RULE_ATT_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.MODEL_RULE_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_MODEL_RULE_OPT_ATT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string ruleOPTValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = ruleOPTValue.Replace(",", ";");
                                }
                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    //base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#08. MODEL_AUTOCALC_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstWhereData.Clear();

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);
                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);
                            llstFieldData.Add(COLUMN.PN_CL_YN, drAutoCalc[COLUMN.PN_CL_YN]);
                            llstFieldData.Add(COLUMN.P_CL_YN, drAutoCalc[COLUMN.P_CL_YN]);
                            llstFieldData.Add(COLUMN.C_CL_YN, drAutoCalc[COLUMN.C_CL_YN]);
                            llstFieldData.Add(COLUMN.U_CL_YN, drAutoCalc[COLUMN.U_CL_YN]);
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);
                            llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");



                            sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);
                            llstWhereData.Add(COLUMN.RAWID, drAutoCalc[COLUMN.RAWID]);

                            base.Update(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                //base.RollBack();
                                return dsResult;
                            }
                        }
                    }

                }

                //base.Commit();
            }
            catch (Exception ex)
            {
                //base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        #endregion

        #region ::Save Default ATT Model

        public DataSet SaveDefaultConfig(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, bool useComma)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;

            try
            {
                base.BeginTrans();


                foreach (DataRow drModel in dtModel.Rows)
                {
                    string sLineRawID = drModel[COLUMN.LOCATION_RAWID].ToString();
                    string sAreaRawID = drModel[COLUMN.AREA_RAWID].ToString();
                    string sEQPModel = drModel[COLUMN.EQP_MODEL].ToString();
                    string sTempEQPModel = "";

                    llstWhereData.Clear();
                    llstWhereData.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                    llstWhereData.Add(COLUMN.AREA_RAWID, sAreaRawID);
                    if (sEQPModel.Length == 0)
                    {
                        sTempEQPModel = "-";
                        llstWhereData.Add(COLUMN.EQP_MODEL, sTempEQPModel);
                    }
                    else
                    {
                        llstWhereData.Add(COLUMN.EQP_MODEL, sEQPModel);
                    }


                    //#00. 먼저 삭제함

                    //#00-1. DEF_RULE_OPT_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE DEF_RULE_RAWID IN (SELECT RAWID FROM DEF_RULE_ATT_MST_SPC WHERE DEF_CONFIG_RAWID IN (SELECT RAWID FROM DEF_CONFIG_ATT_MST_SPC WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2}))",
                                                COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-2. DEF_RULE_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE DEF_CONFIG_RAWID IN (SELECT RAWID FROM DEF_CONFIG_ATT_MST_SPC WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2})", COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-3. DEF_CONTEXT_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-4. DEF_CONFIG_OPT_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_CONFIG_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //#00-5. DEF_AUTOCALC_MST_SPC DATA 삭제
                    base.Delete(TABLE.DEF_AUTOCALC_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }

                    //00-6. DEF_CONFIG_MST_SPC DATA 삭제
                    sWhereQuery = string.Format("WHERE {0} = :{0} AND {1} = :{1} AND NVL({2}, '-') = :{2}", COLUMN.LOCATION_RAWID, COLUMN.AREA_RAWID, COLUMN.EQP_MODEL);

                    base.Delete(TABLE.DEF_CONFIG_ATT_MST_SPC, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return dsResult;
                    }


                    //#01. DEFAULT DATA INSERT

                    //#01-1. DEF_CONFIG_MST_SPC
                    foreach (DataRow drConfig in dtConfig.Rows)
                    {
                        decimal configRawID = base.GetSequence(SEQUENCE.SEQ_DEF_CONFIG_MST_SPC);

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, configRawID);
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                        llstFieldData.Add(COLUMN.AREA_RAWID, sAreaRawID);
                        llstFieldData.Add(COLUMN.EQP_MODEL, sEQPModel);

                        llstFieldData.Add(COLUMN.AUTO_SUB_YN, drConfig[COLUMN.AUTO_SUB_YN]);
                        llstFieldData.Add(COLUMN.INTERLOCK_YN, drConfig[COLUMN.INTERLOCK_YN]);
                        llstFieldData.Add(COLUMN.AUTOCALC_YN, drConfig[COLUMN.AUTOCALC_YN]);

                        llstFieldData.Add(COLUMN.PN_UPPERSPEC, drConfig[COLUMN.PN_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.PN_LOWERSPEC, drConfig[COLUMN.PN_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.PN_TARGET, drConfig[COLUMN.PN_TARGET]);

                        llstFieldData.Add(COLUMN.C_UPPERSPEC, drConfig[COLUMN.C_UPPERSPEC]);
                        llstFieldData.Add(COLUMN.C_LOWERSPEC, drConfig[COLUMN.C_LOWERSPEC]);
                        llstFieldData.Add(COLUMN.C_TARGET, drConfig[COLUMN.C_TARGET]);


                        llstFieldData.Add(COLUMN.PN_LCL, drConfig[COLUMN.PN_LCL]);
                        llstFieldData.Add(COLUMN.PN_UCL, drConfig[COLUMN.PN_UCL]);
                        llstFieldData.Add(COLUMN.PN_CENTER_LINE, drConfig[COLUMN.PN_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.C_LCL, drConfig[COLUMN.C_LCL]);
                        llstFieldData.Add(COLUMN.C_UCL, drConfig[COLUMN.C_UCL]);
                        llstFieldData.Add(COLUMN.C_CENTER_LINE, drConfig[COLUMN.C_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.P_LCL, drConfig[COLUMN.P_LCL]);
                        llstFieldData.Add(COLUMN.P_UCL, drConfig[COLUMN.P_UCL]);
                        llstFieldData.Add(COLUMN.P_CENTER_LINE, drConfig[COLUMN.P_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.U_LCL, drConfig[COLUMN.U_LCL]);
                        llstFieldData.Add(COLUMN.U_UCL, drConfig[COLUMN.U_UCL]);
                        llstFieldData.Add(COLUMN.U_CENTER_LINE, drConfig[COLUMN.U_CENTER_LINE]);

                        llstFieldData.Add(COLUMN.PN_USL_OFFSET, drConfig[COLUMN.PN_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LSL_OFFSET, drConfig[COLUMN.PN_LSL_OFFSET]);

                        llstFieldData.Add(COLUMN.PN_UCL_OFFSET, drConfig[COLUMN.PN_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.PN_LCL_OFFSET, drConfig[COLUMN.PN_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.C_USL_OFFSET, drConfig[COLUMN.C_USL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LSL_OFFSET, drConfig[COLUMN.C_LSL_OFFSET]);

                        llstFieldData.Add(COLUMN.C_UCL_OFFSET, drConfig[COLUMN.C_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.C_LCL_OFFSET, drConfig[COLUMN.C_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.P_UCL_OFFSET, drConfig[COLUMN.P_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.P_LCL_OFFSET, drConfig[COLUMN.P_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.U_UCL_OFFSET, drConfig[COLUMN.U_UCL_OFFSET]);
                        llstFieldData.Add(COLUMN.U_LCL_OFFSET, drConfig[COLUMN.U_LCL_OFFSET]);

                        llstFieldData.Add(COLUMN.OFFSET_YN, drConfig[COLUMN.OFFSET_YN]);

                        llstFieldData.Add(COLUMN.CHART_MODE_CD, drConfig[COLUMN.CHART_MODE_CD]);

                        //---------------------------------------------------------------------------------

                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        base.Insert(TABLE.DEF_CONFIG_ATT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //#01-2. DEF_CONFIG_OPT_MST_SPC
                        foreach (DataRow drConfigOpt in dtConfigOpt.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_CONFIG_OPT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_PARAM_CATEGORY_CD, drConfigOpt[COLUMN.SPC_PARAM_CATEGORY_CD]);
                            llstFieldData.Add(COLUMN.SPC_PRIORITY_CD, drConfigOpt[COLUMN.SPC_PRIORITY_CD]);
                            llstFieldData.Add(COLUMN.AUTO_CPK_YN, drConfigOpt[COLUMN.AUTO_CPK_YN]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_COUNT, drConfigOpt[COLUMN.RESTRICT_SAMPLE_COUNT]);
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_DAYS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_DAYS]);
                            llstFieldData.Add(COLUMN.DEFAULT_CHART_LIST, drConfigOpt[COLUMN.DEFAULT_CHART_LIST]);

                            //SPC-712 By Louis
                            llstFieldData.Add(COLUMN.RESTRICT_SAMPLE_HOURS, drConfigOpt[COLUMN.RESTRICT_SAMPLE_HOURS]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_CONFIG_OPT_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#01-2. DEF_CONTEXT_MST_SPC
                        foreach (DataRow drContext in dtContext.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_CONTEXT_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.CONTEXT_KEY, drContext[COLUMN.CONTEXT_KEY]);

                            if (useComma)
                            {
                                string sValue = drContext[COLUMN.CONTEXT_VALUE].ToString();
                                drContext[COLUMN.CONTEXT_VALUE] = sValue.Replace(",", ";");

                                sValue = drContext[COLUMN.EXCLUDE_LIST].ToString();
                                drContext[COLUMN.EXCLUDE_LIST] = sValue.Replace(",", ";");
                            }

                            llstFieldData.Add(COLUMN.CONTEXT_VALUE, drContext[COLUMN.CONTEXT_VALUE]);
                            llstFieldData.Add(COLUMN.EXCLUDE_LIST, drContext[COLUMN.EXCLUDE_LIST]);
                            llstFieldData.Add(COLUMN.KEY_ORDER, drContext[COLUMN.KEY_ORDER]);
                            if (drContext[COLUMN.GROUP_YN] != null && drContext[COLUMN.GROUP_YN].ToString().Length > 0)
                                llstFieldData.Add(COLUMN.GROUP_YN, ((Convert.ToBoolean(drContext[COLUMN.GROUP_YN])) ? Definition.VARIABLE_Y : Definition.VARIABLE_N));
                            else
                                llstFieldData.Add(COLUMN.GROUP_YN, Definition.VARIABLE_N);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_CONTEXT_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }

                        //#01-3. DEF_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            decimal ruleRawID = base.GetSequence(SEQUENCE.SEQ_DEF_RULE_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, ruleRawID);
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.SPC_RULE_NO, drRule[COLUMN.SPC_RULE_NO]);
                            llstFieldData.Add(COLUMN.OCAP_LIST, drRule[COLUMN.OCAP_LIST]);
                            llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, drRule[COLUMN.USE_MAIN_SPEC_YN]);
                            llstFieldData.Add(COLUMN.RULE_ORDER, drRule[COLUMN.RULE_ORDER]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_RULE_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //UI에서 가상으로 만든 RuleMst의 RawID로 Option을 찾는다
                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID), COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#01-4. DEF_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                llstFieldData.Clear();
                                llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_RULE_OPT_MST_SPC), "");
                                llstFieldData.Add(COLUMN.DEF_RULE_RAWID, ruleRawID);

                                llstFieldData.Add(COLUMN.RULE_OPTION_NO, drRuleOpt[COLUMN.RULE_OPTION_NO]);

                                if (useComma)
                                {
                                    string sValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                                    drRuleOpt[COLUMN.RULE_OPTION_VALUE] = sValue.Replace(",", ";");
                                }

                                llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, drRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                                llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                                llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                                base.Insert(TABLE.DEF_RULE_OPT_ATT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return dsResult;
                                }
                            }
                        }

                        //#01-4. DEF_AUTOCALC_MST_SPC
                        foreach (DataRow drAutoCalc in dtAutoCalc.Rows)
                        {
                            llstFieldData.Clear();
                            llstFieldData.Add(string.Format("{0}+{1}.NEXTVAL", COLUMN.RAWID, SEQUENCE.SEQ_DEF_AUTOCALC_MST_SPC), "");
                            llstFieldData.Add(COLUMN.DEF_CONFIG_RAWID, configRawID);

                            llstFieldData.Add(COLUMN.AUTOCALC_PERIOD, drAutoCalc[COLUMN.AUTOCALC_PERIOD]);
                            llstFieldData.Add(COLUMN.CALC_COUNT, drAutoCalc[COLUMN.CALC_COUNT]);

                            //SPC-658 Initial Calc Count
                            //llstFieldData.Add(COLUMN.INITIAL_CALC_COUNT, drAutoCalc[COLUMN.INITIAL_CALC_COUNT]);

                            llstFieldData.Add(COLUMN.MIN_SAMPLES, drAutoCalc[COLUMN.MIN_SAMPLES]);
                            llstFieldData.Add(COLUMN.DEFAULT_PERIOD, drAutoCalc[COLUMN.DEFAULT_PERIOD]);
                            llstFieldData.Add(COLUMN.MAX_PERIOD, drAutoCalc[COLUMN.MAX_PERIOD]);
                            llstFieldData.Add(COLUMN.CONTROL_LIMIT, drAutoCalc[COLUMN.CONTROL_LIMIT]);
                            llstFieldData.Add(COLUMN.CONTROL_THRESHOLD, drAutoCalc[COLUMN.CONTROL_THRESHOLD]);

                            llstFieldData.Add(COLUMN.PN_CL_YN, drAutoCalc[COLUMN.PN_CL_YN]);
                            llstFieldData.Add(COLUMN.P_CL_YN, drAutoCalc[COLUMN.P_CL_YN]);
                            llstFieldData.Add(COLUMN.C_CL_YN, drAutoCalc[COLUMN.C_CL_YN]);
                            llstFieldData.Add(COLUMN.U_CL_YN, drAutoCalc[COLUMN.U_CL_YN]);
                            llstFieldData.Add(COLUMN.THRESHOLD_OFF_YN, drAutoCalc[COLUMN.THRESHOLD_OFF_YN]);

                            llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.DEF_AUTOCALC_ATT_MST_SPC, llstFieldData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }

                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        #endregion

        #region ::Delete SPC ATTModel

        public DataSet DeleteSPCModel(byte[] param)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                base.BeginTrans();

                llstWhereData.Add(COLUMN.MODEL_RAWID, _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));

                //MODEL_RULE_OPT_MST_SPC
                sWhereQuery = "WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_ATT_MST_SPC WHERE MODEL_CONFIG_RAWID IN ( SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID))";
                base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_RULE_MST_SPC
                sWhereQuery = "WHERE MODEL_CONFIG_RAWID IN (SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE MODEL_RAWID = :MODEL_RAWID)";

                base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_CONTEXT_MST_SPC
                base.Delete(TABLE.MODEL_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_CONFIG_OPT_ATT_MST_SPC
                base.Delete(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_AUTOCALC_MST_SPC
                base.Delete(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }


                //MODEL_CONFIG_MST_SPC
                sWhereQuery = "WHERE MODEL_RAWID = :MODEL_RAWID";

                base.Delete(TABLE.MODEL_CONFIG_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                //MODEL_MST_SPC
                sWhereQuery = "WHERE RAWID = :MODEL_RAWID";

                base.Delete(TABLE.MODEL_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    base.RollBack();
                    return dsResult;
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        #endregion


        #region ::SaveATTSPCModelSpecData

        public bool SaveSPCModelSpecData(byte[] param)
        {
            bool bReturn = true;

            ArrayList arrTempSpecRawID = new ArrayList();
            ArrayList arrTempSpecMainYN = new ArrayList();

            //SPC-676 by Louis
            ArrayList arrTempChartDescription = new ArrayList();
            ArrayList arrTempPN_UPPERSPEC = new ArrayList();
            ArrayList arrTempPN_LOWERSPEC = new ArrayList();
            ArrayList arrTempC_UPPERSPEC = new ArrayList();
            ArrayList arrTempC_LOWERSPEC = new ArrayList();
            ArrayList arrTempPN_UCL = new ArrayList();
            ArrayList arrTempPN_LCL = new ArrayList();
            ArrayList arrTempP_UCL = new ArrayList();
            ArrayList arrTempP_LCL = new ArrayList();
            ArrayList arrTempC_UCL = new ArrayList();
            ArrayList arrTempC_LCL = new ArrayList();
            ArrayList arrTempU_UCL = new ArrayList();
            ArrayList arrTempU_LCL = new ArrayList();
            ArrayList arrTempPN_TARGET = new ArrayList();
            ArrayList arrTempC_TARGET = new ArrayList();
            ArrayList arrTempPN_CENTER_LINE = new ArrayList();
            ArrayList arrTempP_CENTER_LINE = new ArrayList();
            ArrayList arrTempC_CENTER_LINE = new ArrayList();
            ArrayList arrTempU_CENTER_LINE = new ArrayList();

            string sWhereQuery = "WHERE RAWID = :RAWID";

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sUserID = "";

            base.BeginTrans();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();

                arrTempChartDescription = (ArrayList)llstParam[COLUMN.CHART_DESCRIPTON];
                arrTempSpecRawID = (ArrayList)llstParam[COLUMN.RAWID];
                arrTempSpecMainYN = (ArrayList)llstParam[COLUMN.MAIN_YN];
                arrTempPN_UPPERSPEC = (ArrayList)llstParam[COLUMN.PN_UPPERSPEC];
                arrTempPN_LOWERSPEC = (ArrayList)llstParam[COLUMN.PN_LOWERSPEC];
                arrTempC_UPPERSPEC = (ArrayList)llstParam[COLUMN.C_UPPERSPEC];
                arrTempC_LOWERSPEC = (ArrayList)llstParam[COLUMN.C_LOWERSPEC];
                arrTempPN_UCL = (ArrayList)llstParam[COLUMN.PN_UCL];
                arrTempPN_LCL = (ArrayList)llstParam[COLUMN.PN_LCL];
                arrTempP_UCL = (ArrayList)llstParam[COLUMN.P_UCL];
                arrTempP_LCL = (ArrayList)llstParam[COLUMN.P_LCL];
                arrTempC_UCL = (ArrayList)llstParam[COLUMN.C_UCL];
                arrTempC_LCL = (ArrayList)llstParam[COLUMN.C_LCL];
                arrTempU_UCL = (ArrayList)llstParam[COLUMN.U_UCL];
                arrTempU_LCL = (ArrayList)llstParam[COLUMN.U_LCL];
                arrTempPN_TARGET = (ArrayList)llstParam[COLUMN.PN_TARGET];
                arrTempC_TARGET = (ArrayList)llstParam[COLUMN.C_TARGET];
                arrTempPN_CENTER_LINE = (ArrayList)llstParam[COLUMN.PN_CENTER_LINE];
                arrTempP_CENTER_LINE = (ArrayList)llstParam[COLUMN.P_CENTER_LINE];
                arrTempC_CENTER_LINE = (ArrayList)llstParam[COLUMN.C_CENTER_LINE];
                arrTempU_CENTER_LINE = (ArrayList)llstParam[COLUMN.U_CENTER_LINE];


                for (int i = 0; i < arrTempSpecRawID.Count; i++)
                {
                    llstFieldData.Clear();

                    llstFieldData.Add(COLUMN.RAWID, arrTempSpecRawID[i].ToString());
                    llstFieldData.Add(COLUMN.MAIN_YN, arrTempSpecMainYN[i].ToString());

                    //SPC-676 by Louis
                    llstFieldData.Add(COLUMN.CHART_DESCRIPTON, arrTempChartDescription[i].ToString());

                    llstFieldData.Add(COLUMN.PN_UPPERSPEC, arrTempPN_UPPERSPEC[i].ToString());
                    llstFieldData.Add(COLUMN.PN_LOWERSPEC, arrTempPN_LOWERSPEC[i].ToString());
                    llstFieldData.Add(COLUMN.C_UPPERSPEC, arrTempC_UPPERSPEC[i].ToString());
                    llstFieldData.Add(COLUMN.C_LOWERSPEC, arrTempC_LOWERSPEC[i].ToString());
                    llstFieldData.Add(COLUMN.PN_UCL, arrTempPN_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.PN_LCL, arrTempPN_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.P_UCL, arrTempP_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.P_LCL, arrTempP_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.C_UCL, arrTempC_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.C_LCL, arrTempC_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.U_UCL, arrTempU_UCL[i].ToString());
                    llstFieldData.Add(COLUMN.U_LCL, arrTempU_LCL[i].ToString());
                    llstFieldData.Add(COLUMN.PN_TARGET, arrTempPN_TARGET[i].ToString());
                    llstFieldData.Add(COLUMN.C_TARGET, arrTempC_TARGET[i].ToString());
                    llstFieldData.Add(COLUMN.PN_CENTER_LINE, arrTempPN_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.P_CENTER_LINE, arrTempP_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.C_CENTER_LINE, arrTempC_CENTER_LINE[i].ToString());
                    llstFieldData.Add(COLUMN.U_CENTER_LINE, arrTempU_CENTER_LINE[i].ToString());

                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    llstWhereData.Clear();
                    llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, arrTempSpecRawID[i].ToString());

                    base.Update(Definition.TableName.MODEL_CONFIG_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);
                    foreach (string query in _commondata.GetIncreaseATTVersionQuery(arrTempSpecRawID[i].ToString()))
                    {
                        this.Query(query);
                    }

                    if (base.ErrorMessage.Length > 0)
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                bReturn = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();

            return bReturn;
        }

        #endregion

        public bool SaveInterlockYN(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            ArrayList arrActivation = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sUserID = "";

            base.BeginTrans();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.RAWID))
                {
                    arrRawID = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.RAWID];
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.INTERLOCK_YN))
                {
                    arrInterlock = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.INTERLOCK_YN];
                }

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.ACTIVATION_YN))
                {
                    arrActivation = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.ACTIVATION_YN];
                }

                for (int i = 0; i < arrRawID.Count; i++)
                {
                    llstFieldData.Clear();
                    if (Convert.ToBoolean(arrInterlock[i].ToString()))
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_Y);
                    }
                    else
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, Definition.VARIABLE_N);
                    }

                    if (Convert.ToBoolean(arrActivation[i].ToString()))
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.ACTIVATION_YN, Definition.VARIABLE_Y);
                    }
                    else
                    {
                        llstFieldData.Add(Definition.DynamicCondition_Condition_key.ACTIVATION_YN, Definition.VARIABLE_N);
                    }
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                    llstWhereData.Clear();
                    llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, arrRawID[i].ToString());

                    base.Update(Definition.TableName.MODEL_CONFIG_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                bReturn = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();

            return bReturn;
        }

        private const string SQL_COPY_MODEL_FOR_MODEL_CONFIG_ATT_MST_SPC = " UPDATE MODEL_CONFIG_ATT_MST_SPC SET ";


        private const string SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_ATT_MST_SPC = " INSERT INTO MODEL_RULE_ATT_MST_SPC (RAWID, MODEL_CONFIG_RAWID, SPC_RULE_NO, OCAP_LIST, USE_MAIN_SPEC_YN, RULE_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY) " +
            " SELECT  SEQ_MODEL_RULE_MST_SPC.NEXTVAL, :TARGET_MODEL_CONFIG_RAWID, SPC_RULE_NO, OCAP_LIST, USE_MAIN_SPEC_YN, RULE_ORDER, SYSTIMESTAMP, :USER_ID, SYSTIMESTAMP, :USER_ID " +
            " FROM    MODEL_RULE_ATT_MST_SPC " +
            " WHERE   MODEL_CONFIG_RAWID = :SOURCE_MODEL_CONFIG_RAWID ";

        private const string SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_OPT_ATT_MST_SPC = " INSERT INTO MODEL_RULE_OPT_ATT_MST_SPC (RAWID, MODEL_RULE_RAWID, RULE_OPTION_NO, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, RULE_OPTION_VALUE) " +
            " SELECT  SEQ_MODEL_RULE_OPT_MST_SPC.NEXTVAL, (SELECT RAWID FROM MODEL_RULE_MST_SPC WHERE MODEL_CONFIG_RAWID = :TARGET_MODEL_CONFIG_RAWID AND SPC_RULE_NO = MR.SPC_RULE_NO ), " +
            " MO.RULE_OPTION_NO, SYSTIMESTAMP, :USER_ID, SYSTIMESTAMP, :USER_ID, MO.RULE_OPTION_VALUE " +
            " FROM    MODEL_RULE_ATT_MST_SPC MR INNER JOIN MODEL_RULE_OPT_ATT_MST_SPC MO " +
            " ON MR.RAWID = MO.MODEL_RULE_RAWID " +
            " WHERE   MR.MODEL_CONFIG_RAWID = :SOURCE_MODEL_CONFIG_RAWID ";

        private const string SQL_COPY_MODEL_FOR_MODEL_CONFIG_OPT_ATT_MST_SPC = " UPDATE MODEL_CONFIG_OPT_ATT_MST_SPC SET ";

        private const string SQL_COPY_MODEL_FOR_MODEL_AUTOCALC_ATT_MST_SPC = " UPDATE MODEL_AUTOCALC_ATT_MST_SPC SET ";

        #region ::ATTCopyModelInfo

        public DataSet CopyModelInfo(LinkedList llstParam)
        {
            DataSet dsResult = null;

            try
            {
                LinkedList llstFieldData = new LinkedList();

                string sourceConfigRawID = llstParam[Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID].ToString();
                string targetConfigRawID = llstParam[Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID].ToString();
                string userID = llstParam[Definition.DynamicCondition_Condition_key.USER_ID].ToString();

                StringBuilder sbQuery = new StringBuilder();

                #region MODEL_CONFIG_MST_SPC
                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_CONFIG_ATT_MST_SPC);
                bool isEmpty = true;

                string tableName = Definition.TableName.MODEL_CONFIG_ATT_MST_SPC;

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_INTERLOCK].ToString(), COLUMN.INTERLOCK_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION].ToString(), COLUMN.AUTOCALC_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART].ToString(), COLUMN.AUTO_SUB_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_ACTIVE].ToString(), COLUMN.ACTIVATION_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT].ToString(), COLUMN.SAMPLE_COUNT, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_AUTO_SETTING].ToString(), COLUMN.AUTO_TYPE_CD, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK].ToString(), COLUMN.SUB_INTERLOCK_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION].ToString(), COLUMN.SUB_AUTOCALC_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN].ToString(), COLUMN.INHERIT_MAIN_YN, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_MODE].ToString(), COLUMN.CHART_MODE_CD, COLUMN.RAWID, sourceConfigRawID);

                //SPC-676 by Louis
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION].ToString(), COLUMN.CHART_DESCRIPTON, COLUMN.RAWID, sourceConfigRawID);

                string sUseOffset = "N";

                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT].ToString(), COLUMN.PN_UPPERSPEC, COLUMN.PN_LOWERSPEC, COLUMN.PN_TARGET, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT].ToString(), COLUMN.PN_USL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT].ToString(), COLUMN.PN_LSL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }


                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_C_SPEC_LIMIT].ToString(), COLUMN.C_UPPERSPEC, COLUMN.C_LOWERSPEC, COLUMN.C_TARGET, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_C_SPEC_LIMIT].ToString(), COLUMN.C_USL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_C_SPEC_LIMIT].ToString(), COLUMN.C_LSL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                if (llstParam[Definition.COPY_MODEL.RULE_C_SPEC_LIMIT].ToString() == "Y")
                {
                    sUseOffset = "Y";
                }

                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_PN_CONTROL].ToString(), COLUMN.PN_UCL, COLUMN.PN_LCL, COLUMN.PN_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_PN_CONTROL].ToString(), COLUMN.PN_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_PN_CONTROL].ToString(), COLUMN.PN_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);

                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_C_CONTROL].ToString(), COLUMN.C_UCL, COLUMN.C_LCL, COLUMN.C_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_C_CONTROL].ToString(), COLUMN.C_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_C_CONTROL].ToString(), COLUMN.C_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);


                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_P_CONTROL].ToString(), COLUMN.P_UCL, COLUMN.P_LCL, COLUMN.P_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_P_CONTROL].ToString(), COLUMN.P_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_P_CONTROL].ToString(), COLUMN.P_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);



                _commondata.MakeQueryForATTConfigMstSpec(ref isEmpty, sbQuery, llstParam[Definition.COPY_MODEL.RULE_U_CONTROL].ToString(), COLUMN.U_UCL, COLUMN.U_LCL, COLUMN.U_CENTER_LINE, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_U_CONTROL].ToString(), COLUMN.U_UCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.RULE_U_CONTROL].ToString(), COLUMN.U_LCL_OFFSET, COLUMN.RAWID, sourceConfigRawID);




                if (sUseOffset == "Y")
                {
                    _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, sUseOffset, COLUMN.OFFSET_YN, COLUMN.RAWID, sourceConfigRawID);
                }

                if (isEmpty == false)
                {
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");
                    sbQuery.AppendLine(" WHERE RAWID = '" + targetConfigRawID + "' ");

                    dsResult = base.Query(sbQuery.ToString());

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_RULE_MST_SPC & MODEL_RULE_OPT_MST_SPC
                string ruleSelection = llstParam[Definition.COPY_MODEL.RULE_RULE_SELECTION].ToString();

                if (ruleSelection.Trim().ToUpper() == "Y")
                {
                    // DELETE MODEL_RULE_OPT_MST_SPC
                    llstFieldData.Clear();
                    string sWhereQuery = string.Format("WHERE {0} IN (SELECT RAWID FROM {1} WHERE {2} = :{2}) ",
                        COLUMN.MODEL_RULE_RAWID, TABLE.MODEL_RULE_ATT_MST_SPC, COLUMN.MODEL_CONFIG_RAWID);

                    llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, targetConfigRawID);

                    base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // DELETE MODEL_RULE_MST_SPC
                    sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.MODEL_CONFIG_RAWID);
                    base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // INSERT MODEL_RULE_MST_SPC
                    llstFieldData.Clear();
                    llstFieldData.Add("TARGET_MODEL_CONFIG_RAWID", targetConfigRawID);
                    llstFieldData.Add("USER_ID", userID);
                    llstFieldData.Add("SOURCE_MODEL_CONFIG_RAWID", sourceConfigRawID);

                    dsResult = base.Query(SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_ATT_MST_SPC, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }

                    // INSERT MODULE_RULE_OPT_MST_SPC
                    dsResult = base.Query(SQL_COPY_MODEL_FOR_INSERT_MODEL_RULE_OPT_ATT_MST_SPC, llstFieldData);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_CONFIG_OPT_MST_SPC
                isEmpty = true;
                sbQuery.Length = 0;
                tableName = TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC;

                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_CONFIG_OPT_ATT_MST_SPC);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY].ToString(), COLUMN.SPC_PARAM_CATEGORY_CD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_CALCULATE_PPK].ToString(), COLUMN.AUTO_CPK_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_PRIORITY].ToString(), COLUMN.SPC_PRIORITY_CD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS].ToString(), COLUMN.RESTRICT_SAMPLE_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DAYS].ToString(), COLUMN.RESTRICT_SAMPLE_DAYS, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW].ToString(), COLUMN.DEFAULT_CHART_LIST, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                //SPC-712 By Louis
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.OPTION_DAYS].ToString(), COLUMN.RESTRICT_SAMPLE_HOURS, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                if (isEmpty == false)
                {
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");

                    sbQuery.AppendLine(" WHERE MODEL_CONFIG_RAWID = '" + targetConfigRawID + "' ");

                    dsResult = base.Query(sbQuery.ToString());

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion

                #region MODEL_AUTOCALC_MST_SPC
                isEmpty = true;
                sbQuery.Length = 0;
                tableName = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;

                sbQuery.AppendLine(SQL_COPY_MODEL_FOR_MODEL_AUTOCALC_ATT_MST_SPC);

                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD].ToString(), COLUMN.AUTOCALC_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT].ToString(), COLUMN.CALC_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE].ToString(), COLUMN.MIN_SAMPLES, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD].ToString(), COLUMN.DEFAULT_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE].ToString(), COLUMN.MAX_PERIOD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE].ToString(), COLUMN.CONTROL_LIMIT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD].ToString(), COLUMN.CONTROL_THRESHOLD, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_PN_CONTROL_LIMIT].ToString(), COLUMN.PN_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_P_CONTROL_LIMIT].ToString(), COLUMN.P_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_C_CONTROL_LIMIT].ToString(), COLUMN.C_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_U_CONTROL_LIMIT].ToString(), COLUMN.U_CL_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION].ToString(), COLUMN.SHIFT_CALC_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                _commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN].ToString(), COLUMN.THRESHOLD_OFF_YN, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);
                //_commondata.MakeQueryForConfigMst(ref isEmpty, sbQuery, tableName, llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_INITIAL_COUNT].ToString(), COLUMN.CALC_COUNT, COLUMN.INITIAL_CALC_COUNT, COLUMN.MODEL_CONFIG_RAWID, sourceConfigRawID);

                if (isEmpty == false)
                {
                    if (llstParam[Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT].ToString().Equals("Y"))
                    {
                        sbQuery.AppendLine(", " + COLUMN.PN_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.P_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.C_CALC_COUNT + " = 0 ");
                        sbQuery.AppendLine(", " + COLUMN.U_CALC_COUNT + " = 0 ");
                    }

                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_BY + " = ( select user_id from USER_MST_PP where rawid = '" + userID + "' )");
                    sbQuery.AppendLine(", " + COLUMN.LAST_UPDATE_DTTS + " = SYSTIMESTAMP ");

                    sbQuery.AppendLine(" WHERE MODEL_CONFIG_RAWID = '" + targetConfigRawID + "' ");

                    dsResult = base.Query(sbQuery.ToString());

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
                #endregion


                #region MODEL_CONTEXT_MST_SPC

                //SPC-1218, KBLEE, START
                if (llstParam[Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION].ToString().Equals("Y"))
                {
                    string queryGetSourceContext =
                        @"SELECT context_key, context_value 
                            FROM MODEL_CONTEXT_ATT_MST_SPC 
                           WHERE 1=1
                            AND model_config_rawid = " + sourceConfigRawID;

                    DataSet sourceResult = base.Query(queryGetSourceContext);
                    int rowCount = sourceResult.Tables[0].Rows.Count;

                    for (int i = 0; i < rowCount; i++)
                    {
                        DataRow dr = sourceResult.Tables[0].Rows[i];
                        string contextKey = dr[COLUMN.CONTEXT_KEY].ToString();
                        string contextValue = dr[COLUMN.CONTEXT_VALUE].ToString();

                        string queryUpdateContextInform =
                            @"UPDATE MODEL_CONTEXT_ATT_MST_SPC
                                 SET context_value = '" + contextValue + @"' 
                               WHERE 1=1
                                AND context_key = '" + contextKey + @"' 
                                AND model_config_rawid = " + targetConfigRawID;

                        dsResult = base.Query(queryUpdateContextInform);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }
                    }
                }
                //SPC-1218, KBLEE, END

                #endregion


                foreach (string s in _commondata.GetIncreaseATTVersionQuery(targetConfigRawID))
                {
                    this.Query(s);
                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                        return dsResult;
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }


        #endregion

        public DataSet GetSPCModelDatabyChartID(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                //#00. MODEL_MST_SPC
                sb.Append("SELECT B.RAWID LOCATION_RAWID, B.SITE, B.LINE, B.FAB, C.RAWID AREA_RAWID, C.AREA, A.RAWID MODEL_RAWID, A.SPC_MODEL_NAME, A.EQP_MODEL FROM MODEL_ATT_MST_SPC A, LOCATION_MST_PP B, AREA_MST_PP C ");
                sb.Append(" WHERE 1 = 1 ");
                sb.Append(" AND A.LOCATION_RAWID = B.RAWID ");
                sb.Append(" AND A.AREA_RAWID = C.RAWID ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND A.RAWID = (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = :MODEL_CONFIG_RAWID) ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("select mcms.* ");
                sb.Append("from MODEL_CONFIG_ATT_MST_SPC mcms ");
                sb.Append(" WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("   AND mcms.RAWID = :MODEL_CONFIG_RAWID ");
                }

                sb.Append(" ORDER BY mcms.RAWID");

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.*, ");
                sb.Append("       B.NAME AS SPC_PARAM_CATEGORY, ");
                sb.Append("       C.NAME AS SPC_PRIORITY ");
                sb.Append("  FROM MODEL_CONFIG_OPT_ATT_MST_SPC A ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PARAM_CATEGORY') B ");
                sb.Append("   ON (A.SPC_PARAM_CATEGORY_CD = B.CODE) ");
                sb.Append("  LEFT OUTER JOIN (SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = 'SPC_PRIOTIRY') C ");
                sb.Append("   ON (A.SPC_PRIORITY_CD = C.CODE) ");
                sb.Append("WHERE 1 = 1 ");

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    sb.Append("  AND A.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                }

                dsTemp = this.Query(sb.ToString(), llstCondition);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }


        private const string SQL_SEARCH_CALC_ATT_MODEL = " SELECT *                                                                   "
                                                    + "   FROM MODEL_ATT_MST_SPC                                                      "
                                                    + "  WHERE RAWID = :RAWID                                ";


        private const string SQL_SEARCH_CALC_ATT_MODEL_CONFIG = "           SELECT *                                                  "
                                                    + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                    + "            WHERE MODEL_RAWID = :RAWID  ORDER BY MAIN_YN DESC              ";


        public DataSet GetATTSPCCalcModelData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            string sSQL = "";

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_RAWID))
                {
                    llstWhereData.Add(COLUMN.RAWID, llstParam[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                sSQL = SQL_SEARCH_CALC_ATT_MODEL;

                dsTemp = this.Query(sSQL, llstWhereData);

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


                sSQL = SQL_SEARCH_CALC_ATT_MODEL_CONFIG;

                dsTemp = this.Query(sSQL, llstWhereData);

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

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" GROUP_YN, KEY_ORDER, VERSION, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY ");
                sb.Append("  FROM MODEL_CONTEXT_ATT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID IN ( SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE MODEL_RAWID = :RAWID ) ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }


        private const string SQL_SEARCH_CALC_ATT_MODEL_POPUP = " SELECT *                                                                 "
                                                        + "   FROM MODEL_ATT_MST_SPC                                                      "
                                                        + "  WHERE RAWID IN (                                                         "
                                                        + "           SELECT DISTINCT MODEL_RAWID                                     "
                                                        + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                        + "            WHERE RAWID = :CONFIGRAWID)                                    ";

        private const string SQL_SEARCH_CALC_ATT_MODEL_CONFIG_POPUP = "           SELECT *                                            "
                                                    + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                    + "            WHERE RAWID = :CONFIGRAWID                                     ";

        private const string SQL_SEARCH_CALC_ATT_MODEL_CONTEXT_POPUP = " SELECT *                                                      "
                                                    + "            FROM MODEL_CONTEXT_ATT_MST_SPC                                      "
                                                    + "           WHERE MODEL_CONFIG_RAWID = :CONFIGRAWID                          ";

        public DataSet GetATTSPCCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID))
                {
                    llstWhereData.Add("CONFIGRAWID", llstParam[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                //#00. MODEL_ATT_MST_SPC

                dsTemp = this.Query(SQL_SEARCH_CALC_ATT_MODEL_POPUP, llstWhereData);

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


                //#01. MODEL_CONFIG_ATT_MST_SPC

                dsTemp = this.Query(SQL_SEARCH_CALC_ATT_MODEL_CONFIG_POPUP, llstWhereData);

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


                //#02. MODEL_CONTEXT_ATT_MST_SPC

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" KEY_ORDER, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY, GROUP_YN, VERSION ");
                sb.Append("  FROM MODEL_CONTEXT_ATT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :CONFIGRAWID ");

                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        private const string SQL_ATT_MODEL_CONFIG_HISTORY =
@"SELECT *
  FROM MODEL_CONFIG_ATT_HST_SPC
 WHERE MODIFIED_TYPE_CD = 'AC' AND MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ORDER BY CREATE_DTTS";

        public DataSet GetATTSPCModelHistoryInfo(byte[] baData)
        {
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(SQL_ATT_MODEL_CONFIG_HISTORY);

            //DataSet dsResult = this.Query(sbQuery.ToString(), llstData);
            DataSet dsResult = new DataSet();

            DataSet dsTemp = this.Query(sbQuery.ToString(), llstData);

            if (dsTemp != null && dsTemp.Tables.Count > 0)
            {
                DataTable dtExcel = new DataTable();
                for (int idx = 0; idx < dsTemp.Tables[0].Columns.Count; idx++)
                {
                    string sCol = dsTemp.Tables[0].Columns[idx].ColumnName.ToString().ToUpper();
                    dtExcel.Columns.Add(sCol, typeof(string));
                }

                foreach (DataRow dr in dsTemp.Tables[0].Rows)
                {
                    dtExcel.ImportRow(dr);
                }

                dsResult.Tables.Add("AC_HISTORY");
                dsResult.Tables["AC_HISTORY"].Merge(dtExcel);
            }

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }


        private const string SQL_ATT_MODEL_HISTORY_CONTEXT =
@"SELECT MCMS2.MODEL_CONFIG_RAWID, MCMS2.CONTEXT_KEY, MCMS2.CONTEXT_VALUE
  FROM MODEL_ATT_MST_SPC MMS INNER JOIN MODEL_CONFIG_ATT_MST_SPC MCMS
       ON MMS.RAWID = MCMS.MODEL_RAWID
       INNER JOIN MODEL_CONTEXT_ATT_MST_SPC MCMS2
       ON MCMS.RAWID = MCMS2.MODEL_CONFIG_RAWID
     AND MCMS2.MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ";

        public DataSet GetATTSPCModelContextInfo(byte[] baData)
        {
            LinkedList llstData = new LinkedList();
            llstData.SetSerialData(baData);

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(SQL_ATT_MODEL_HISTORY_CONTEXT);

            DataSet dsResult = this.Query(sbQuery.ToString(), llstData);

            if (base.ErrorMessage.Length > 0)
            {
                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                return dsResult;
            }

            return dsResult;
        }


        public bool SaveATTSpecCalcData(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sChartType = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                llstFieldData.Clear();

                if (llstParam.Contains("CHART_TYPE"))
                {
                    sChartType = llstParam["CHART_TYPE"].ToString();
                }

                if (sChartType == "PN")
                {
                    if (llstParam.Contains(COLUMN.UPPER_SPEC))
                    {
                        llstFieldData.Add(COLUMN.PN_UPPERSPEC, llstParam[COLUMN.UPPER_SPEC].ToString());
                    }

                    if (llstParam.Contains(COLUMN.LOWER_SPEC))
                    {
                        llstFieldData.Add(COLUMN.PN_LOWERSPEC, llstParam[COLUMN.LOWER_SPEC].ToString());
                    }
                }
                else if (sChartType == "C")
                {
                    if (llstParam.Contains(COLUMN.UPPER_SPEC))
                    {
                        llstFieldData.Add(COLUMN.C_UPPERSPEC, llstParam[COLUMN.UPPER_SPEC].ToString());
                    }

                    if (llstParam.Contains(COLUMN.LOWER_SPEC))
                    {
                        llstFieldData.Add(COLUMN.C_LOWERSPEC, llstParam[COLUMN.LOWER_SPEC].ToString());
                    }
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.UCL))
                {
                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.P:
                            llstFieldData.Add(COLUMN.P_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.PN:
                            llstFieldData.Add(COLUMN.PN_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.C:
                            llstFieldData.Add(COLUMN.C_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                        case Definition.CHART_TYPE.U:
                            llstFieldData.Add(COLUMN.U_UCL, llstParam[Definition.CHART_COLUMN.UCL].ToString());
                            break;
                    }
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.LCL))
                {
                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.P:
                            llstFieldData.Add(COLUMN.P_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.PN:
                            llstFieldData.Add(COLUMN.PN_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.U:
                            llstFieldData.Add(COLUMN.U_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                        case Definition.CHART_TYPE.C:
                            llstFieldData.Add(COLUMN.C_LCL, llstParam[Definition.CHART_COLUMN.LCL].ToString());
                            break;
                    }
                }

                if (llstParam.Contains(Definition.CHART_COLUMN.UCL) && llstParam.Contains(Definition.CHART_COLUMN.LCL))
                {
                    double dUCL = Convert.ToDouble(llstParam[Definition.CHART_COLUMN.UCL].ToString());
                    double dLCL = Convert.ToDouble(llstParam[Definition.CHART_COLUMN.LCL].ToString());
                    double dTarget = (dUCL + dLCL) / 2;
                    switch (sChartType)
                    {
                        case Definition.CHART_TYPE.P:
                            llstFieldData.Add(COLUMN.P_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.PN:
                            llstFieldData.Add(COLUMN.PN_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.U:
                            llstFieldData.Add(COLUMN.U_CENTER_LINE, dTarget.ToString());
                            break;
                        case Definition.CHART_TYPE.C:
                            llstFieldData.Add(COLUMN.C_CENTER_LINE, dTarget.ToString());
                            break;
                    }

                }

                if (llstParam.Contains(COLUMN.UPPER_SPEC) && llstParam.Contains(COLUMN.LOWER_SPEC))
                {
                    if (sChartType == "PN")
                    {

                        if (llstParam[COLUMN.UPPER_SPEC].ToString() != null && llstParam[COLUMN.UPPER_SPEC].ToString() != ""
                            && llstParam[COLUMN.LOWER_SPEC].ToString() != null && llstParam[COLUMN.LOWER_SPEC].ToString() != "")
                        {
                            double dUSL = Convert.ToDouble(llstParam[COLUMN.UPPER_SPEC].ToString());
                            double dLSL = Convert.ToDouble(llstParam[COLUMN.LOWER_SPEC].ToString());

                            double dTargetSL = (dUSL + dLSL) / 2;
                            llstFieldData.Add(COLUMN.PN_TARGET, dTargetSL.ToString());
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.PN_TARGET, "");
                        }
                    }
                    else if (sChartType == "C")
                    {
                        if (llstParam[COLUMN.UPPER_SPEC].ToString() != null && llstParam[COLUMN.UPPER_SPEC].ToString() != ""
                            && llstParam[COLUMN.LOWER_SPEC].ToString() != null && llstParam[COLUMN.LOWER_SPEC].ToString() != "")
                        {
                            double dUSL = Convert.ToDouble(llstParam[COLUMN.UPPER_SPEC].ToString());
                            double dLSL = Convert.ToDouble(llstParam[COLUMN.LOWER_SPEC].ToString());

                            double dTargetSL = (dUSL + dLSL) / 2;
                            llstFieldData.Add(COLUMN.C_TARGET, dTargetSL.ToString());
                        }
                        else
                        {
                            llstFieldData.Add(COLUMN.C_TARGET, "");
                        }
                    }
                }

                if (llstParam.Contains(Definition.CONDITION_KEY_USER_ID))
                {
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, llstParam[Definition.CONDITION_KEY_USER_ID].ToString());
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                }

                llstWhereData.Clear();
                string rawid = llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
                llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, rawid);

                base.Update(Definition.TableName.MODEL_CONFIG_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                foreach (string s in _commondata.GetIncreaseATTVersionQuery(rawid))
                {
                    this.Query(s);
                }

                if (base.ErrorMessage.Length > 0)
                {
                    bReturn = false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return bReturn;
        }

        private const string SQL_UPDATE_MODEL_CONFIG_ATT_MST_SPC_FOR_SUBCONFIG = " UPDATE MODEL_CONFIG_ATT_MST_SPC                                                     "
                                                                           + "    SET (P_UCL, P_LCL, P_CENTER_LINE, PN_USL, PN_LSL, PN_TARGET, PN_UCL, PN_LCL, PN_CENTER_LINE,            "
                                                                           + "         U_UCL, U_LCL, U_CENTER_LINE, C_USL, C_LSL, C_TARGET, C_UCL, C_LCL, C_CENTER_LINE,  LAST_UPDATE_DTTS,               "
                                                                           + "         LAST_UPDATE_BY,        "
                                                                           + "         P_UCL_OFFSET, P_LCL_OFFSET, PN_USL_OFFSET, PN_LSL_OFFSET, PN_UCL_OFFSET, PN_LCL_OFFSET, "
                                                                           + "         U_UCL_OFFSET, U_LCL_OFFSET, C_USL_OFFSET, C_LSL_OFFSET, C_UCL_OFFSET, C_LCL_OFFSET, "
                                                                           + "         OFFSET_YN) =                                "
                                                                           + "           (SELECT P_UCL, P_LCL, P_CENTER_LINE, PN_USL, PN_LSL, PN_TARGET, PN_UCL, PN_LCL, PN_CENTER_LINE,            "
                                                                           + "         U_UCL, U_LCL, U_CENTER_LINE, C_USL, C_LSL, C_TARGET, C_UCL, C_LCL, C_CENTER_LINE,  LAST_UPDATE_DTTS,               "
                                                                           + "         LAST_UPDATE_BY,        "
                                                                           + "         P_UCL_OFFSET, P_LCL_OFFSET, PN_USL_OFFSET, PN_LSL_OFFSET, PN_UCL_OFFSET, PN_LCL_OFFSET, "
                                                                           + "         U_UCL_OFFSET, U_LCL_OFFSET, C_USL_OFFSET, C_LSL_OFFSET, C_UCL_OFFSET, C_LCL_OFFSET, "
                                                                           + "         OFFSET_YN                         "
                                                                           + "              FROM MODEL_CONFIG_ATT_MST_SPC                                          "
                                                                           + "             WHERE RAWID = :RAWID AND MAIN_YN = :MAIN_MAIN)                      "
                                                                           + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                                        "
                                                                           + "                         FROM MODEL_CONFIG_ATT_MST_SPC                               "
                                                                           + "                        WHERE RAWID = :RAWID AND MAIN_YN = :MAIN_MAIN) AND MAIN_YN = :MAIN_SUB AND INHERIT_MAIN_YN = :INHERIT_YN ";

        private const string SQL_DELETE_MODEL_RULE_OPT_ATT_MST_SPC_FOR_SUBCONFIG = " DELETE MODEL_RULE_OPT_ATT_MST_SPC                                            "
                                                                            + "  WHERE MODEL_RULE_RAWID IN (                                             "
                                                                            + "           SELECT RAWID                                                   "
                                                                            + "             FROM MODEL_RULE_MST_SPC                                      "
                                                                            + "            WHERE MODEL_CONFIG_RAWID IN (                                 "
                                                                            + "                     SELECT RAWID                                         "
                                                                            + "                       FROM MODEL_CONFIG_MST_SPC                          "
                                                                            + "                      WHERE MODEL_RAWID = (SELECT MODEL_RAWID             "
                                                                            + "                                             FROM MODEL_CONFIG_MST_SPC    "
                                                                            + "                                            WHERE RAWID = :RAWID)         "
                                                                            + "                        AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y'))     ";


        private const string SQL_DELETE_MODEL_RULE_ATT_MST_SPC_FOR_SUBCONFIG = " DELETE      MODEL_RULE_ATT_MST_SPC                                   "
                                                                        + "       WHERE MODEL_CONFIG_RAWID IN (                               "
                                                                        + "                SELECT RAWID                                       "
                                                                        + "                  FROM MODEL_CONFIG_ATT_MST_SPC                        "
                                                                        + "                 WHERE MODEL_RAWID = (SELECT MODEL_RAWID           "
                                                                        + "                                        FROM MODEL_CONFIG_ATT_MST_SPC  "
                                                                        + "                                       WHERE RAWID = :RAWID)       "
                                                                        + "                   AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y')    ";


        public readonly string SQL_SELECT_ATT_SUB_CONFIG_RAWID = " SELECT RAWID                                              "
                                                            + "   FROM MODEL_CONFIG_ATT_MST_SPC                               "
                                                            + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                  "
                                                            + "                         FROM MODEL_CONFIG_ATT_MST_SPC         "
                                                            + "                        WHERE RAWID = :RAWID) AND MAIN_YN = 'N' AND INHERIT_MAIN_YN = 'Y' ";

        public DataSet ModifyATTSPCSubModel(string sConfigRawID, DataTable dtRule, DataTable dtRuleOpt, string sUserID, ref string sConfigRawid)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;
            string[] sArrModelConfigRawID = null;

            try
            {
                DateTime dt = base.GetDBTimeStamp();

                //#01. MODEL_CONFIG_ATT_MST_SPC
                llstWhereData.Clear();
                llstWhereData.Add("RAWID", sConfigRawID);
                llstWhereData.Add("MAIN_MAIN", "Y");
                llstWhereData.Add("MAIN_SUB", "N");
                llstWhereData.Add("INHERIT_YN", "Y");

                dsResult = base.Query(SQL_UPDATE_MODEL_CONFIG_ATT_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //#02. MODEL_RULE_ATT_MST_SPC, MODEL_RULE_OPT_ATT_MST_SPC DATA 삭제하고 Main config rawid 기준으로 Insert 한다.
                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_OPT_ATT_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_DELETE_MODEL_RULE_ATT_MST_SPC_FOR_SUBCONFIG, llstWhereData);

                dsResult = new DataSet();

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                //sub config의 model_config_rawid를 구한다.

                llstWhereData.Clear();

                llstWhereData.Add("RAWID", sConfigRawID);

                dsResult = base.Query(SQL_SELECT_ATT_SUB_CONFIG_RAWID, llstWhereData);

                if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                {
                    ArrayList arrTemp = new ArrayList();
                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                    {
                        arrTemp.Add(dsResult.Tables[0].Rows[i][COLUMN.RAWID].ToString());
                    }

                    sArrModelConfigRawID = (string[])arrTemp.ToArray(typeof(string));
                }

                if (sArrModelConfigRawID != null && sArrModelConfigRawID.Length > 0)
                {
                    ArrayList arrRawID = new ArrayList();
                    ArrayList arrModelConfigRawID = new ArrayList();
                    ArrayList arrSPCRuleNo = new ArrayList();
                    ArrayList arrOCAPList = new ArrayList();
                    ArrayList arrUseMainSpec = new ArrayList();
                    ArrayList arrRuleOrder = new ArrayList();
                    ArrayList arrCreateBy = new ArrayList();
                    ArrayList arrCreateDtts = new ArrayList();

                    ArrayList arrRuleOptRawID = new ArrayList();
                    ArrayList arrRuleRawID = new ArrayList();
                    ArrayList arrRuleOptNo = new ArrayList();
                    ArrayList arrRuleOptValue = new ArrayList();
                    ArrayList arrRuleOptCreateBy = new ArrayList();
                    ArrayList arrRuleOptCreateDtts = new ArrayList();

                    decimal kTemp = 0;
                    decimal jTemp = 0;
                    decimal ruleRawID = 0;
                    decimal ruleOptRawID = 0;

                    for (int i = 0; i < sArrModelConfigRawID.Length; i++)
                    {
                        //#07. MODEL_RULE_MST_SPC
                        foreach (DataRow drRule in dtRule.Rows)
                        {
                            if (kTemp == 0)
                            {
                                ruleRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_RULE_ATT_MST_SPC, 500);
                            }


                            arrRawID.Add(ruleRawID.ToString());
                            arrModelConfigRawID.Add(sArrModelConfigRawID[i]);
                            arrSPCRuleNo.Add(drRule[COLUMN.SPC_RULE_NO].ToString());
                            arrOCAPList.Add(drRule[COLUMN.OCAP_LIST].ToString());
                            arrUseMainSpec.Add(drRule[COLUMN.USE_MAIN_SPEC_YN].ToString());
                            arrRuleOrder.Add(drRule[COLUMN.RULE_ORDER].ToString());
                            arrCreateBy.Add(sUserID);
                            arrCreateDtts.Add(dt);



                            string sVirtualRawID = drRule[COLUMN.RAWID].ToString();
                            DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0}='{1}'", COLUMN.MODEL_RULE_RAWID, sVirtualRawID),
                                                                    COLUMN.RULE_OPTION_NO);

                            if (drRuleOpts == null || drRuleOpts.Length == 0) continue;

                            //#06. MODEL_RULE_OPT_MST_SPC
                            foreach (DataRow drRuleOpt in drRuleOpts)
                            {
                                if (jTemp == 0)
                                {
                                    ruleOptRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_RULE_OPT_ATT_MST_SPC, 500);
                                }

                                arrRuleOptRawID.Add(ruleOptRawID.ToString());
                                arrRuleRawID.Add(ruleRawID.ToString());
                                arrRuleOptNo.Add(drRuleOpt[COLUMN.RULE_OPTION_NO].ToString());
                                arrRuleOptValue.Add(drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString());
                                arrRuleOptCreateBy.Add(sUserID);
                                arrRuleOptCreateDtts.Add(dt);

                                ruleOptRawID++;
                                jTemp++;
                                if (jTemp == 500)
                                    jTemp = 0;

                            }

                            ruleRawID++;
                            kTemp++;
                            if (kTemp == 500)
                                kTemp = 0;
                        }
                        sConfigRawid += ";" + sArrModelConfigRawID[i];
                    }

                    llstFieldData.Clear();

                    if (arrRawID.Count > 0)
                    {
                        llstFieldData.Add(COLUMN.RAWID, (string[])arrRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, (string[])arrModelConfigRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.SPC_RULE_NO, (string[])arrSPCRuleNo.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.OCAP_LIST, (string[])arrOCAPList.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.USE_MAIN_SPEC_YN, (string[])arrUseMainSpec.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_ORDER, (string[])arrRuleOrder.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrCreateBy.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrCreateDtts.ToArray(typeof(DateTime)));

                        base.InsertBatch(TABLE.MODEL_RULE_ATT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.RAWID, (string[])arrRuleOptRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.MODEL_RULE_RAWID, (string[])arrRuleRawID.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_OPTION_NO, (string[])arrRuleOptNo.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.RULE_OPTION_VALUE, (string[])arrRuleOptValue.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrRuleOptCreateBy.ToArray(typeof(string)));
                        llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrRuleOptCreateDtts.ToArray(typeof(DateTime)));

                        base.InsertBatch(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            return dsResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        public DataSet GetATTTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            try
            {
                if (targetList.Length != 0)
                {
                    int itemp = 0;
                    string strTemp = "";
                    ArrayList arrsModelRawID = new ArrayList();
                    if (targetList.Length > 1000)
                    {
                        for (int i = 0; i < targetList.Length; i++)
                        {
                            strTemp += "," + targetList[i];
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
                            strWhere = string.Format(" Rawid in ({0})", arrsModelRawID[k].ToString());

                            sqlQuery = string.Format(@"SELECT RAWID, PN_USL, PN_LSL, PN_UCL, PN_LCL, C_USL, C_LSL, C_UCL, C_LCL
                                                    FROM MODEL_CONFIG_ATT_MST_SPC WHERE {0}", strWhere);

                            DataSet dsTemp = base.Query(sqlQuery);

                            dsResult.Merge(dsTemp);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < targetList.Length; i++)
                        {
                            strTemp += "," + targetList[i];
                            itemp++;
                        }
                        if (strTemp.Length > 0)
                        {
                            arrsModelRawID.Add(strTemp.Substring(1));
                        }
                        for (int k = 0; k < arrsModelRawID.Count; k++)
                        {
                            strWhere = string.Format(" Rawid in ({0})", arrsModelRawID[k].ToString());

                            sqlQuery = string.Format(@"SELECT RAWID, PN_USL, PN_LSL, PN_UCL, PN_LCL, C_USL, C_LSL, C_UCL, C_LCL
                                                    FROM MODEL_CONFIG_ATT_MST_SPC WHERE {0}", strWhere);

                            DataSet dsTemp = base.Query(sqlQuery);

                            dsResult.Merge(dsTemp);
                        }
                    }

                }

            }
            catch
            {
            }

            return dsResult;
        }

        public DataSet GetATTSourceConfigSpecData(string rawid)
        {
            DataSet dsResult = new DataSet();
            string sqlQuery = string.Empty;
            string strWhere = string.Empty;
            try
            {
                strWhere = string.Format(" Rawid in ({0})", rawid);

                sqlQuery = string.Format(@"SELECT RAWID, PN_USL, PN_LSL, PN_UCL, PN_LCL, C_USL, C_LSL, C_UCL, C_LCL
                                                    FROM MODEL_CONFIG_ATT_MST_SPC WHERE {0}", strWhere);

                dsResult = base.Query(sqlQuery);

            }
            catch
            {
            }
            return dsResult;
        }

        //SPC-704 MultiCalc Popup
        private const string SQL_SEARCH_MULTI_CALC_ATT_MODEL_CONFIG_POPUP = "           SELECT *                                            "
                                                    + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                    + "            WHERE RAWID IN(                                      ";


        //SPC-704 MultiCalc Popup
        private const string SQL_SEARCH_MULTI_CALC_ATT_MODEL_POPUP = " SELECT *                                                                 "
                                                        + "   FROM MODEL_ATT_MST_SPC                                                      "
                                                        + "  WHERE RAWID IN (                                                         "
                                                        + "           SELECT DISTINCT MODEL_RAWID                                     "
                                                        + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                        + "            WHERE RAWID IN(                                     ";


        //SPC-704 MultiCalculation
        public DataSet GetATTSPCMulCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            try
            {
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID))
                {
                    llstWhereData.Add("CONFIGRAWID", llstParam[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                StringBuilder sb = new StringBuilder();

                sb.Append(SQL_SEARCH_MULTI_CALC_ATT_MODEL_POPUP);
                sb.Append(llstWhereData["CONFIGRAWID"]).Append("  ))");

                //#00. MODEL_MST_SPC


                // dsTemp = this.Query(SQL_SEARCH_MULTI_CALC_ATT_MODEL_POPUP, llstWhereData);
                dsTemp = this.Query(sb.ToString());

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

                StringBuilder sb2 = new StringBuilder();

                sb2.Append(SQL_SEARCH_MULTI_CALC_ATT_MODEL_CONFIG_POPUP);
                sb2.Append(llstWhereData["CONFIGRAWID"]).Append(")");

                //#01. MODEL_CONFIG_MST_SPC

                dsTemp = this.Query(sb2.ToString());

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


                //#02. MODEL_CONTEXT_MST_SPC

                StringBuilder sb3 = new StringBuilder();

                sb3.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb3.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb3.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb3.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb3.Append(" GROUP_YN, KEY_ORDER, VERSION, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY ");
                sb3.Append("  FROM MODEL_CONTEXT_ATT_MST_SPC                                      ");
                sb3.Append(" MODEL_CONFIG_RAWID IN( ");
                sb3.Append(llstWhereData["CONFIGRAWID"]).Append(")");

                dsTemp = this.Query(sb3.ToString());

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //resource 해제
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCATTRuleMasterData(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                StringBuilder sb = new StringBuilder();

                //#01. MODEL_RULE_MST_SPC
                sb.Append(" SELECT A.*, SPC_RULE_NO || '. ' || DESCRIPTION AS SPC_RULE FROM RULE_ATT_MST_SPC A ORDER BY A.SPC_RULE_NO ");

                DataSet dsTemp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRule = dsTemp.Tables[0].Copy();
                    dtRule.TableName = "RULE_ATT_MST_SPC";

                    dsReturn.Tables.Add(dtRule);
                }

                //#02. MODEL_RULE_OPT_MST_SPC
                sb.Remove(0, sb.Length);
                sb.Append(" SELECT B.*, A.SPC_RULE_NO, '' AS RULE_OPTION_VALUE ");
                sb.Append("   FROM RULE_ATT_MST_SPC A ");
                sb.Append("        INNER JOIN RULE_OPT_ATT_MST_SPC B ");
                sb.Append("           ON (A.RAWID = B.RULE_RAWID) ");

                dsTemp = this.Query(sb.ToString());

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
                else
                {
                    DataTable dtRuleOPT = dsTemp.Tables[0].Copy();
                    dtRuleOPT.TableName = "RULE_OPT_ATT_MST_SPC";

                    dsReturn.Tables.Add(dtRuleOPT);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        private const string strQuery_ATT_MCMS = @"UPDATE MODEL_CONFIG_ATT_MST_SPC A
                                        SET VERSION = ( SELECT NVL(VERSION, 0)+1 
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE RAWID = A.RAWID )
                                        WHERE MODEL_RAWID IN (SELECT MODEL_RAWID
                                                                FROM MODEL_CONFIG_ATT_MST_SPC
                                                               WHERE RAWID = :MODEL_CONFIG_RAWID)
                                        AND MAIN_YN = 'N'
                                        AND INHERIT_MAIN_YN = 'Y'";

        private const string strQuery_ATT_MCOMS = @"UPDATE MODEL_CONFIG_OPT_ATT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_ATT_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_ATT_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_ATT_MCTXMS = @"UPDATE MODEL_CONTEXT_ATT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_ATT_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_ATT_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_ATT_MAMS = @"UPDATE MODEL_AUTOCALC_ATT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_ATT_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_ATT_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_ATT_MRMS = @"UPDATE MODEL_RULE_ATT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE RAWID = A.MODEL_CONFIG_RAWID )
                                        WHERE MODEL_CONFIG_RAWID IN (
                                            SELECT RAWID
                                            FROM MODEL_CONFIG_ATT_MST_SPC
                                            WHERE MODEL_RAWID IN (
                                                SELECT MODEL_RAWID
                                                FROM MODEL_CONFIG_ATT_MST_SPC
                                                WHERE RAWID = :MODEL_CONFIG_RAWID )
                                            AND MAIN_YN = 'N'
                                            AND INHERIT_MAIN_YN = 'Y'
                                                        )";

        private const string strQuery_ATT_MROMS = @"UPDATE MODEL_RULE_OPT_ATT_MST_SPC A
                                        SET VERSION = ( SELECT VERSION 
                                                        FROM MODEL_RULE_ATT_MST_SPC
                                                        WHERE RAWID = A.MODEL_RULE_RAWID )
                                        WHERE MODEL_RULE_RAWID IN (
                                                    SELECT RAWID 
                                                    FROM MODEL_RULE_ATT_MST_SPC
                                                    WHERE MODEL_CONFIG_RAWID IN ( 
                                                        SELECT RAWID
                                                        FROM MODEL_CONFIG_ATT_MST_SPC
                                                        WHERE MODEL_RAWID IN (
                                                            SELECT MODEL_RAWID
                                                            FROM MODEL_CONFIG_ATT_MST_SPC
                                                            WHERE RAWID = :MODEL_CONFIG_RAWID )
                                                        AND MAIN_YN = 'N'
                                                        AND INHERIT_MAIN_YN = 'Y'
                                                                    )
                                                    ) ";

        public bool IncreaseVersionOfSubConfigs(string sMainConfigRawID)
        {
            bool bResult = true;

            try
            {
                LinkedList llstCondition = new LinkedList();
                llstCondition.Add("MODEL_CONFIG_RAWID", sMainConfigRawID);

                base.Query(strQuery_ATT_MCMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_ATT_MCOMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_ATT_MCTXMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_ATT_MAMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_ATT_MRMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                base.Query(strQuery_ATT_MROMS, llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                bResult = false;
            }

            return bResult;
        }

        private const string SQL_DELETE_ATT_SUB_CONTEXT_KEY = @"DELETE FROM MODEL_CONTEXT_ATT_MST_SPC
                                                            WHERE MODEL_CONFIG_RAWID IN 
                                                            ( 
                                                            SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                            WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                            WHERE RAWID = :RAWID)
                                                            AND MAIN_YN = 'N'
                                                            )
                                                            AND CONTEXT_KEY NOT IN (SELECT CONTEXT_KEY FROM MODEL_CONTEXT_ATT_MST_SPC
                                                            WHERE MODEL_CONFIG_RAWID = :RAWID)";


        private const string SQL_UPDATE_ATT_SUB_CONTEXT_KEY_ORDER = @"UPDATE MODEL_CONTEXT_ATT_MST_SPC A
                                                                    SET A.KEY_ORDER = (SELECT KEY_ORDER FROM MODEL_CONTEXT_ATT_MST_SPC B
                                                                    WHERE MODEL_CONFIG_RAWID = :RAWID AND A.CONTEXT_KEY = B.CONTEXT_KEY)
                                                                    WHERE A.MODEL_CONFIG_RAWID IN 
                                                                    ( 
                                                                    SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                                    WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                                    WHERE RAWID = :RAWID)
                                                                    AND MAIN_YN = 'N'
                                                                    )";

        private const string SQL_UPDATE_ATT_SUB_CONTEXT_VALUE_GROUP = @"UPDATE MODEL_CONTEXT_ATT_MST_SPC A
                                                                    SET (A.CONTEXT_VALUE, A.EXCLUDE_LIST) = (SELECT CONTEXT_VALUE, EXCLUDE_LIST FROM MODEL_CONTEXT_ATT_MST_SPC B
                                                                    WHERE MODEL_CONFIG_RAWID = :RAWID AND A.CONTEXT_KEY = B.CONTEXT_KEY AND B.GROUP_YN = 'Y'
                                                                    )
                                                                    WHERE A.MODEL_CONFIG_RAWID IN 
                                                                    ( 
                                                                    SELECT RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                                    WHERE MODEL_RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC
                                                                    WHERE RAWID = :RAWID)
                                                                    AND MAIN_YN = 'N'
                                                                    )
                                                                    AND A.CONTEXT_KEY in ({0})";

        private const string SQL_SELECT_ATT_SUB_CONFIG_RAWID_FOR_CONTEXT_SAVE = " SELECT RAWID                                              "
                                                            + "   FROM MODEL_CONFIG_ATT_MST_SPC                               "
                                                            + "  WHERE MODEL_RAWID = (SELECT MODEL_RAWID                  "
                                                            + "                         FROM MODEL_CONFIG_ATT_MST_SPC         "
                                                            + "                        WHERE RAWID = :RAWID) AND MAIN_YN = 'N' ";




        public DataSet ModifyATTSPCSubModelContext(string sConfigRawID, DataTable dtContext, string sUserID, bool isOnlyMainGroup)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {

                DateTime dt = base.GetDBTimeStamp();

                //delete 한 경우는 sub의 context도 지우고 key_order도 맞춰준다.
                llstWhereData.Clear();
                llstWhereData.Add(COLUMN.RAWID, sConfigRawID);
                dsResult = base.Query(SQL_DELETE_ATT_SUB_CONTEXT_KEY, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }

                dsResult = base.Query(SQL_UPDATE_ATT_SUB_CONTEXT_KEY_ORDER, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                    return dsResult;
                }
                //isonlymain = false 이고 Group이 추가된 경우는 전 sub model에 insert 한다.
                //isonlymain = false 이고 기존 Grouping된 Data가 Modify된 경우는 전 sub model을 Modify 한다.
                if (!isOnlyMainGroup)
                {
                    DataRow[] drArrTempModify = dtContext.Select("_MODIFY = 'True' and group_yn = 'True'");

                    if (drArrTempModify.Length > 0)
                    {
                        string strConTextKey = "";

                        for (int i = 0; i < drArrTempModify.Length; i++)
                        {
                            strConTextKey += ",'" + drArrTempModify[i][COLUMN.CONTEXT_KEY].ToString() + "'";
                        }

                        strConTextKey = strConTextKey.Substring(1);

                        dsResult = base.Query(string.Format(SQL_UPDATE_ATT_SUB_CONTEXT_VALUE_GROUP, strConTextKey), llstWhereData);
                    }


                    DataRow[] drArrTemp = dtContext.Select("_INSERT = 'True' and group_yn = 'True'");
                    if (drArrTemp.Length > 0)
                    {
                        DataSet dsTemp = base.Query(SQL_SELECT_ATT_SUB_CONFIG_RAWID_FOR_CONTEXT_SAVE, llstWhereData);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            llstFieldData.Clear();
                            ArrayList arrTempRawID = new ArrayList();
                            ArrayList arrTempModelConfigRawID = new ArrayList();
                            ArrayList arrTempContextKey = new ArrayList();
                            ArrayList arrTempContextValue = new ArrayList();
                            ArrayList arrTempExcludeList = new ArrayList();
                            ArrayList arrTempKeyOrder = new ArrayList();
                            ArrayList arrTempGroupYN = new ArrayList();
                            ArrayList arrTempCreateDtts = new ArrayList();
                            ArrayList arrTempCreateBy = new ArrayList();

                            decimal ddxRawID = base.GetSequenceCount(SEQUENCE.SEQ_MODEL_CONTEXT_ATT_MST_SPC, dsTemp.Tables[0].Rows.Count * drArrTemp.Length);

                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < drArrTemp.Length; j++)
                                {
                                    arrTempRawID.Add(ddxRawID.ToString());
                                    arrTempModelConfigRawID.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                                    arrTempContextKey.Add(drArrTemp[j][COLUMN.CONTEXT_KEY]);
                                    arrTempContextValue.Add(drArrTemp[j][COLUMN.CONTEXT_VALUE]);
                                    arrTempExcludeList.Add(drArrTemp[j][COLUMN.EXCLUDE_LIST].ToString());
                                    arrTempKeyOrder.Add(drArrTemp[j][COLUMN.KEY_ORDER].ToString());
                                    arrTempGroupYN.Add(Definition.VARIABLE_Y);
                                    arrTempCreateBy.Add(sUserID);
                                    arrTempCreateDtts.Add(dt);

                                    ddxRawID++;
                                }
                            }

                            if (arrTempRawID.Count > 0)
                            {
                                llstFieldData.Clear();

                                llstFieldData.Add(COLUMN.RAWID, (string[])arrTempRawID.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.MODEL_CONFIG_RAWID, (string[])arrTempModelConfigRawID.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CONTEXT_KEY, (string[])arrTempContextKey.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CONTEXT_VALUE, (string[])arrTempContextValue.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.EXCLUDE_LIST, (string[])arrTempExcludeList.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.KEY_ORDER, (string[])arrTempKeyOrder.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.GROUP_YN, (string[])arrTempGroupYN.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CREATE_BY, (string[])arrTempCreateBy.ToArray(typeof(string)));
                                llstFieldData.Add(COLUMN.CREATE_DTTS, (DateTime[])arrTempCreateDtts.ToArray(typeof(DateTime)));

                                base.InsertBatch(TABLE.MODEL_CONTEXT_ATT_MST_SPC, llstFieldData);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                    return dsResult;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Resource 해제
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return dsResult;
        }

        public DataSet DeleteATTSPCModelConfig(byte[] param)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                base.BeginTrans();

                if (llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID] != null)
                {
                    ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];
                    if (arrTemp.Count > 300)
                    {
                        int itemp = 0;
                        string strTemp = "";
                        ArrayList arrsModelRawID = new ArrayList();
                        for (int i = 0; i < arrTemp.Count; i++)
                        {
                            if (itemp == 0)
                            {
                                strTemp = "'" + arrTemp[0].ToString() + "'";
                            }
                            else
                            {
                                strTemp = strTemp + ",'" + arrTemp[i].ToString() + "'";
                            }

                            //strTemp += "," + arrTemp[i];
                            itemp++;
                            if (itemp == 300)
                            {
                                // arrsModelRawID.Add(strTemp.Substring(1));
                                arrsModelRawID.Add(strTemp);
                                itemp = 0;
                                strTemp = "";
                            }
                        }
                        if (strTemp.Length > 0)
                        {
                            arrsModelRawID.Add(strTemp);
                        }

                        for (int k = 0; k < arrsModelRawID.Count; k++)
                        {

                            string strWhereCondition = arrsModelRawID[k].ToString();
                            //MODEL_RULE_OPT_MST_SPC
                            sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_ATT_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                            base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_RULE_MST_SPC
                            sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                            base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_CONTEXT_MST_SPC
                            base.Delete(TABLE.MODEL_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            base.Delete(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_AUTOCALC_MST_SPC
                            base.Delete(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }

                            //MODEL_CONFIG_MST_SPC
                            sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                            base.Delete(TABLE.MODEL_CONFIG_ATT_MST_SPC, sWhereQuery, llstWhereData);

                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                        }
                    }
                    else
                    {
                        //ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];
                        string strWhereCondition = _commondata.MakeVariablesList(arrTemp);

                        sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_ATT_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                        base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_RULE_MST_SPC
                        sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                        base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_CONTEXT_MST_SPC
                        base.Delete(TABLE.MODEL_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        base.Delete(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_AUTOCALC_MST_SPC
                        base.Delete(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                        //MODEL_CONFIG_MST_SPC
                        sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                        base.Delete(TABLE.MODEL_CONFIG_ATT_MST_SPC, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }

                    }
                }

                base.Commit();
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }


            return dsResult;
        }

        public bool DeleteATTSPCModelConfig(string strSubconfig)
        {
            bool _bResult = true;

            LinkedList llstWhereData = new LinkedList();
            string sWhereQuery = string.Empty;

            try
            {
                string[] strArrTemp = strSubconfig.Split(';');
                ArrayList arrTemp = new ArrayList();
                for (int i = 0; i < strArrTemp.Length; i++)
                {
                    if (strArrTemp[i].Trim().Length > 0)
                    {
                        arrTemp.Add(strArrTemp[i].Trim());
                    }

                }
                string strWhereCondition = _commondata.MakeVariablesList(arrTemp);

                //MODEL_RULE_OPT_ATT_MST_SPC
                sWhereQuery = string.Format("WHERE MODEL_RULE_RAWID IN ( SELECT RAWID FROM MODEL_RULE_ATT_MST_SPC WHERE MODEL_CONFIG_RAWID IN ({0}))", strWhereCondition);
                base.Delete(TABLE.MODEL_RULE_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_RULE_ATT_MST_SPC
                sWhereQuery = string.Format("WHERE MODEL_CONFIG_RAWID IN ({0})", strWhereCondition);

                base.Delete(TABLE.MODEL_RULE_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_CONTEXT_ATT_MST_SPC
                base.Delete(TABLE.MODEL_CONTEXT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_CONFIG_OPT_ATT_MST_SPC
                base.Delete(TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_AUTOCALC_ATT_MST_SPC
                base.Delete(TABLE.MODEL_AUTOCALC_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

                //MODEL_CONFIG_ATT_MST_SPC
                sWhereQuery = string.Format("WHERE RAWID IN ({0})", strWhereCondition);

                base.Delete(TABLE.MODEL_CONFIG_ATT_MST_SPC, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                //Resource 해제
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return _bResult;
        }

        public bool SaveATTSpecMultiCalcData(byte[] param)
        {
            bool bReturn = true;
            ArrayList arrRawID = new ArrayList();
            ArrayList arrInterlock = new ArrayList();
            string sWhereQuery = "WHERE RAWID = :RAWID";
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            string sChartType = "";

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                llstFieldData.Clear();

                if (llstParam.Contains("CHART_TYPE"))
                {
                    sChartType = llstParam["CHART_TYPE"].ToString();
                }

                switch (sChartType)
                {
                    case "P":
                        if (llstParam.Contains(COLUMN.P_UCL))
                        {
                            llstFieldData.Add(COLUMN.P_UCL, llstParam[COLUMN.P_UCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.P_LCL))
                        {
                            llstFieldData.Add(COLUMN.P_LCL, llstParam[COLUMN.P_LCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.P_UCL) && llstParam.Contains(COLUMN.P_LCL))
                        {
                            double dUCL = Convert.ToDouble(llstParam[COLUMN.P_UCL].ToString());
                            double dLCL = Convert.ToDouble(llstParam[COLUMN.P_LCL].ToString());
                            double dTarget = (dUCL + dLCL) / 2;
                            llstFieldData.Add(COLUMN.P_CENTER_LINE, dTarget);
                        }
                        break;
                    case "PN":
                        if (llstParam.Contains(COLUMN.PN_UCL))
                        {
                            llstFieldData.Add(COLUMN.PN_UCL, llstParam[COLUMN.PN_UCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.PN_LCL))
                        {
                            llstFieldData.Add(COLUMN.PN_LCL, llstParam[COLUMN.PN_LCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.PN_UCL) && llstParam.Contains(COLUMN.PN_LCL))
                        {
                            double dUCL = Convert.ToDouble(llstParam[COLUMN.PN_UCL].ToString());
                            double dLCL = Convert.ToDouble(llstParam[COLUMN.PN_LCL].ToString());
                            double dTarget = (dUCL + dLCL) / 2;
                            llstFieldData.Add(COLUMN.PN_CENTER_LINE, dTarget);
                        }
                        break;
                    case "C":
                        if (llstParam.Contains(COLUMN.C_UCL))
                        {
                            llstFieldData.Add(COLUMN.C_UCL, llstParam[COLUMN.C_UCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.C_LCL))
                        {
                            llstFieldData.Add(COLUMN.C_LCL, llstParam[COLUMN.C_LCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.C_UCL) && llstParam.Contains(COLUMN.C_LCL))
                        {
                            double dUCL = Convert.ToDouble(llstParam[COLUMN.C_UCL].ToString());
                            double dLCL = Convert.ToDouble(llstParam[COLUMN.C_LCL].ToString());
                            double dTarget = (dUCL + dLCL) / 2;
                            llstFieldData.Add(COLUMN.C_CENTER_LINE, dTarget);
                        }
                        break;
                    case "U":
                        if (llstParam.Contains(COLUMN.U_UCL))
                        {
                            llstFieldData.Add(COLUMN.U_UCL, llstParam[COLUMN.U_UCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.U_LCL))
                        {
                            llstFieldData.Add(COLUMN.U_LCL, llstParam[COLUMN.U_LCL].ToString());
                        }
                        if (llstParam.Contains(COLUMN.U_UCL) && llstParam.Contains(COLUMN.U_LCL))
                        {
                            double dUCL = Convert.ToDouble(llstParam[COLUMN.U_UCL].ToString());
                            double dLCL = Convert.ToDouble(llstParam[COLUMN.U_LCL].ToString());
                            double dTarget = (dUCL + dLCL) / 2;
                            llstFieldData.Add(COLUMN.EWMA_R_CENTER_LINE, dTarget);
                        }
                        break;
                }


                if (llstParam.Contains(Definition.CONDITION_KEY_USER_ID))
                {
                    llstFieldData.Add(COLUMN.LAST_UPDATE_BY, llstParam[Definition.CONDITION_KEY_USER_ID].ToString());
                    llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                }

                llstWhereData.Clear();
                string rawid = llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();
                llstWhereData.Add(Definition.DynamicCondition_Condition_key.RAWID, rawid);

                base.Update(Definition.TableName.MODEL_CONFIG_ATT_MST_SPC, llstFieldData, sWhereQuery, llstWhereData);

                foreach (string s in _commondata.GetIncreaseATTVersionQuery(rawid))
                {
                    this.Query(s);
                }

                if (base.ErrorMessage.Length > 0)
                {
                    bReturn = false;
                }

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }

            return bReturn;

        }

        //SPC-1292, KBLEE
        public DataSet GetATTGroupNameByChartId(string chartId)
        {
            DataSet dsResult = new DataSet();
            LinkedList ll = new LinkedList();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("SELECT A.*                                                                ");
                sb.Append("FROM MODEL_GROUP_ATT_MST_SPC A                                            ");
                sb.Append("WHERE RAWID = (SELECT C.GROUP_RAWID                                       ");
                sb.Append("                  FROM MODEL_CONFIG_ATT_MST_SPC B, MODEL_ATT_MST_SPC C    ");
                sb.Append("                 WHERE B.MODEL_RAWID = C.RAWID AND B.RAWID = :CHART_ID)   ");

                ll.Add(COLUMN.CHART_ID, chartId);

                dsResult = base.Query(sb.ToString(), ll);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;

        }

        private const string SQL_SEARCH_ATT_CALC_MODEL_SAVE = " SELECT *                                                                   "
                                                    + "   FROM MODEL_ATT_MST_SPC                                                      "
                                                    + "  WHERE RAWID IN (SELECT MODEL_RAWID FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = :RAWID ) ";

        //SPC-812 Calculation
        private const string SQL_SEARCH_ATT_CALC_MODEL_CONFIG_SAVE = "           SELECT *                                                  "
                                                    + "             FROM MODEL_CONFIG_ATT_MST_SPC                                     "
                                                    + "            WHERE RAWID = :RAWID                                     ";
        //SPC-812 Calculation
        private const string SQL_SEARCH_ATT_CALC_MODEL_CONTEXT_SAVE = " SELECT *                                                            "
                                                    + "            FROM MODEL_CONTEXT_ATT_MST_SPC                                      "
                                                    + "           WHERE MODEL_CONFIG_RAWID = :RAWID ";


        //SPC-812 SPC Calculation.
        public DataSet GetATTSPCCalcModelDataSave(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            LinkedList llstParam = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            DataSet dsTemp = null;

            string sSQL = "";

            try
            {
                llstParam.SetSerialData(param);

                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MODEL_RAWID))
                {
                    llstWhereData.Add(COLUMN.RAWID, llstParam[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                //#00. MODEL_MST_SPC
                sSQL = SQL_SEARCH_ATT_CALC_MODEL_SAVE;

                dsTemp = this.Query(sSQL, llstWhereData);

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

                //sSQL = string.Format(SQL_SEARCH_CALC_MODEL_CONFIG, strSubQueryEQP, strSubQueryModule, strSubQueryRecipe, strSubQueryStep);
                sSQL = SQL_SEARCH_ATT_CALC_MODEL_CONFIG_SAVE;

                dsTemp = this.Query(sSQL, llstWhereData);

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


                //#02. MODEL_CONTEXT_MST_SPC

                //sSQL = string.Format(SQL_SEARCH_CALC_MODEL_CONTEXT, strSubQueryEQP, strSubQueryModule, strSubQueryRecipe, strSubQueryStep);
                sSQL = SQL_SEARCH_ATT_CALC_MODEL_CONTEXT_SAVE;
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT RAWID, MODEL_CONFIG_RAWID, CONTEXT_KEY,                    ");

                if (useComma)
                {
                    sb.Append(" REPLACE (CONTEXT_VALUE, ';', ',') AS CONTEXT_VALUE,           ");
                    sb.Append(" REPLACE (EXCLUDE_LIST, ';', ',') AS EXCLUDE_LIST,             ");
                }
                else
                {
                    sb.Append(" CONTEXT_VALUE, EXCLUDE_LIST                                  ");
                }
                sb.Append(" GROUP_YN, KEY_ORDER, VERSION, CREATE_DTTS, CREATE_BY, LAST_UPDATE_DTTS, LAST_UPDATE_BY ");
                sb.Append("  FROM MODEL_CONTEXT_ATT_MST_SPC                                      ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :RAWID                                ");
                dsTemp = this.Query(sb.ToString(), llstWhereData);

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
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (dsTemp != null) { dsTemp.Dispose(); dsTemp = null; }
                llstParam.Clear(); llstParam = null;
                llstWhereData.Clear(); llstWhereData = null;
            }

            return dsReturn;
        }

        public DataSet GetATTGroupList(byte[] param)
        {
            DataSet dsResult = null;
            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llWhereField = new LinkedList();
            llWhereField.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LINE_RAWID].ToString());
            llWhereField.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID].ToString());

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * FROM MODEL_GROUP_ATT_MST_SPC ");
                sb.Append("WHERE LOCATION_RAWID =: LOCATION_RAWID ");
                sb.Append("AND AREA_RAWID =: AREA_RAWID ");

                if (llParam.Contains(COLUMN.EQP_MODEL))
                {
                    llWhereField.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL].ToString().Replace("'", string.Empty));
                    sb.Append("AND EQP_MODEL =: EQP_MODEL ");
                }
                sb.Append("ORDER BY GROUP_NAME ASC");

                dsResult = base.Query(sb.ToString(), llWhereField);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        public DataSet GetATTSPCModelGroupList()
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            string level = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM CODE_MST_PP WHERE CATEGORY='SPC_ATT_MODEL_LEVEL'");

            ds = this.Query(sb.ToString());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[Definition.CONDITION_KEY_CODE].ToString() == Definition.CONDITION_KEY_AREA && dr[Definition.CONDITION_KEY_USE_YN].ToString() == "Y")
                    {
                        level = Definition.CONDITION_KEY_AREA;
                        break;
                    }
                    else
                    {
                        level = Definition.CONDITION_KEY_EQP_MODEL;
                    }
                }
            }

            if (level == Definition.CONDITION_KEY_AREA)
            {
                sb.Remove(0, sb.Length);
                sb.Append("SELECT LINE_RAWID AS LOCATION_RAWID, AREA_RAWID, AREA AS GROUP_LEVEL, EQPMODEL ");
                sb.Append("FROM EQP_VW_SPC GROUP BY LINE_RAWID, AREA_RAWID, AREA, EQPMODEL ");
                sb.Append("ORDER BY GROUP_LEVEL ASC ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "SPC_MODEL_LEVEL";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT A.RAWID, A.LOCATION_RAWID, A.AREA_RAWID, B.AREA AS GROUP_LEVEL, A.GROUP_NAME      ");
                sb.Append("FROM MODEL_GROUP_ATT_MST_SPC A, EQP_VW_SPC B                                   ");
                sb.Append("WHERE A.LOCATION_RAWID = B.LINE_RAWID AND A.AREA_RAWID = B.AREA_RAWID      ");
                sb.Append("GROUP BY A.RAWID,A.LOCATION_RAWID,A.AREA_RAWID,B.AREA, A.GROUP_NAME                     ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_GROUP_SPC";
                dsResult.Tables.Add(ds.Tables[0].Copy());

                sb.Remove(0, sb.Length);
                sb.Append("SELECT B.RAWID, B.LOCATION_RAWID, B.AREA_RAWID, A.AREA AS GROUP_LEVEL, B.SPC_MODEL_NAME , B.GROUP_NAME                      ");
                sb.Append("FROM (  SELECT A.AREA, B.LOCATION_RAWID, B.AREA_RAWID                                                                       ");
                sb.Append("              FROM EQP_VW_SPC A, MODEL_MST_SPC B                                                                            ");
                sb.Append("             WHERE A.LINE_RAWID = B.LOCATION_RAWID                                                                          ");
                sb.Append("                   AND A.AREA_RAWID = B.AREA_RAWID                                                                          ");
                sb.Append("          GROUP BY AREA, B.LOCATION_RAWID, B.AREA_RAWID                                                                     ");
                sb.Append("          ORDER BY A.AREA ASC) A,                                                                                           ");
                sb.Append("         (  SELECT B.RAWID,                                                                                                 ");
                sb.Append("                   B.LOCATION_RAWID,                                                                                        ");
                sb.Append("                   B.AREA_RAWID,                                                                                            ");
                sb.Append("                   B.GROUP_RAWID,                                                                                           ");
                sb.Append("                   B.SPC_MODEL_NAME,                                                                                        ");
                sb.Append("                   C.GROUP_NAME                                                                                             ");
                sb.Append("              FROM MODEL_ATT_MST_SPC B, MODEL_GROUP_ATT_MST_SPC C                                                                   ");
                sb.Append("             WHERE B.LOCATION_RAWID = C.LOCATION_RAWID                                                                      ");
                sb.Append("                   AND B.AREA_RAWID = C.AREA_RAWID                                                                          ");
                sb.Append("                   AND B.GROUP_RAWID = C.RAWID                                                                              ");
                sb.Append("                   AND B.GROUP_RAWID IS NOT NULL                                                                            ");
                sb.Append("          GROUP BY B.RAWID,                                                                                                 ");
                sb.Append("                   B.LOCATION_RAWID,                                                                                        ");
                sb.Append("                   B.AREA_RAWID,                                                                                            ");
                sb.Append("                   B.GROUP_RAWID,                                                                                           ");
                sb.Append("                   B.SPC_MODEL_NAME,                                                                                        ");
                sb.Append("                   C.GROUP_NAME) B , MODEL_CONFIG_ATT_MST_SPC C                                                                 ");
                sb.Append("WHERE A.LOCATION_RAWID = B.LOCATION_RAWID                                                                                   ");
                sb.Append("AND A.AREA_RAWID = B.AREA_RAWID AND C.MODEL_RAWID =  B.RAWID                                                                ");

                sb.Append("GROUP BY B.RAWID, B.LOCATION_RAWID, B.AREA_RAWID, A.AREA, B.SPC_MODEL_NAME, B.GROUP_NAME                                    ");
                sb.Append("UNION ALL                                                                                                                   ");
                sb.Append("SELECT B.RAWID, B.LOCATION_RAWID, B.AREA_RAWID, A.AREA AS GROUP_LEVEL, B.SPC_MODEL_NAME , 'UNASSIGNED MODEL' AS GROUP_NAME  ");
                sb.Append("FROM EQP_VW_SPC  A, MODEL_ATT_MST_SPC B, MODEL_CONFIG_ATT_MST_SPC C                                                                 ");
                sb.Append("WHERE A.LINE_RAWID = B.LOCATION_RAWID AND A.AREA_RAWID = B.AREA_RAWID                                                       ");
                sb.Append("AND B.GROUP_RAWID IS NULL  AND C.MODEL_RAWID = B.RAWID                                                                      ");

                sb.Append("GROUP BY B.RAWID, B.LOCATION_RAWID, B.AREA_RAWID, A.AREA, B.SPC_MODEL_NAME                                                  ");

                ds = this.Query(sb.ToString());
                ds.Tables[0].TableName = "MODEL_MST_SPC";

                dsResult.Tables.Add(ds.Tables[0].Copy());
            }

            return dsResult;
        }

        public bool CheckDuplicateATTGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = true;
            DataSet ds = new DataSet();

            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llWhereField = new LinkedList();
            llWhereField.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LOCATION_RAWID]);
            llWhereField.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID]);

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM MODEL_GROUP_ATT_MST_SPC        ");
            sb.Append("WHERE LOCATION_RAWID =: LOCATION_RAWID   ");
            sb.Append("AND AREA_RAWID =: AREA_RAWID             ");

            //if (llParam[COLUMN.SPC_DATA_LEVEL].ToString() == Definition.CONDITION_KEY_EQP_MODEL)
            //{
            //    sb.Append("AND EQP_MODEL =: EQP_MODEL             ");
            //    llWhereField.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL]);
            //}

            ds = base.Query(sb.ToString(), llWhereField);

            if (ds.Tables.Count > 0)
            {
                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                {
                    foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                    {
                        DataRow[] drDel = ds.Tables[0].Select(string.Format("RAWID = '{0}' ", dr[COLUMN.RAWID].ToString()));
                        ds.Tables[0].Rows.Remove(drDel[0]);
                    }
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_UPDATE))
                    {
                        foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_UPDATE].Rows)
                        {
                            DataRow[] drUpdate = ds.Tables[0].Select(string.Format("RAWID = '{0}' ", dr[COLUMN.RAWID].ToString()));

                            if (drUpdate.Length > 0)
                            {
                                drUpdate[0][COLUMN.GROUP_NAME] = dr[COLUMN.GROUP_NAME].ToString().ToUpper();
                            }
                            else if (row[COLUMN.GROUP_NAME].ToString() == dr[COLUMN.GROUP_NAME].ToString() && dr["_DELETE"].ToString() == Definition.VARIABLE_FALSE)
                            {
                                return false;
                            }
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                    {
                        foreach (DataRow dr in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                        {
                            if (row[COLUMN.GROUP_NAME].ToString() == dr[COLUMN.GROUP_NAME].ToString().ToUpper())
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return bResult;
        }

        public string CheckDuplicateATTMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string strResult = string.Empty;

            LinkedList llParam = new LinkedList();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM MODEL_ATT_MST_SPC ");
            sb.Append("WHERE RAWID =: RAWID  ");

            if (isEmptyGroupRawid)
                sb.Append("AND GROUP_RAWID IS NULL ");
            else
                sb.Append("AND GROUP_RAWID =: GROUP_RAWID ");

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["_SELECT"].ToString() == Definition.VARIABLE_TRUE)
                {
                    llParam.Clear();
                    llParam.Add(COLUMN.RAWID, dr[COLUMN.RAWID].ToString());

                    if (!isEmptyGroupRawid)
                        llParam.Add(COLUMN.GROUP_RAWID, dr[COLUMN.GROUP_RAWID].ToString());

                    DataSet ds = base.Query(sb.ToString(), llParam);

                    if (base.ErrorMessage.Length > 0 || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        return dr[Definition.CONDITION_KEY_MODEL_NAME].ToString();
                    }
                }
            }

            return strResult;
        }

        public bool SaveATTSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            LinkedList llParam = new LinkedList();
            llParam.SetSerialData(param);

            LinkedList llstFieldData = new LinkedList();
            StringBuilder sbQuery = new StringBuilder();

            string sWhereQuery = "WHERE RAWID = :RAWID";
            string sUserId = llParam[Definition.CONDITION_KEY_USER_ID].ToString();
            string sGroup_Rawid = llParam[COLUMN.GROUP_RAWID].ToString();

            base.BeginTrans();

            try
            {
                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                {
                    foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                    {
                        llParam.Clear();
                        llParam.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.GROUP_RAWID, sGroup_Rawid);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                        base.Update(TABLE.MODEL_ATT_MST_SPC, llstFieldData, sWhereQuery, llParam);
                    }
                }

                if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                {
                    foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                    {
                        llParam.Clear();
                        llParam.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                        llstFieldData.Clear();
                        llstFieldData.Add(COLUMN.GROUP_RAWID, DBNull.Value);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                        base.Update(TABLE.MODEL_ATT_MST_SPC, llstFieldData, sWhereQuery, llParam);
                    }
                }

                if (base.ErrorMessage.Length > 0)
                {
                    base.RollBack();
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            base.Commit();
            bResult = true;

            return bResult;
        }

        public bool SaveATTSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool bResult = false;
            LinkedList llParam = new LinkedList();
            LinkedList llstFieldData = new LinkedList();

            llParam.SetSerialData(param);
            base.BeginTrans();

            try
            {

                string sUserId = llParam[COLUMN.USER_ID].ToString();

                if (dsSave.Tables.Count > 0)
                {
                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_DELETE))
                    {
                        LinkedList ll = new LinkedList();

                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_DELETE].Rows)
                        {
                            DataSet dsMapping = new DataSet();

                            StringBuilder sbQuery = new StringBuilder();
                            sbQuery.Append("SELECT A.* FROM MODEL_ATT_MST_SPC A, MODEL_GROUP_ATT_MST_SPC B ");
                            sbQuery.Append("WHERE  A.GROUP_RAWID = B.RAWID ");
                            sbQuery.Append("AND A.GROUP_RAWID =: GROUP_RAWID ");

                            ll.Clear();
                            ll.Add(COLUMN.GROUP_RAWID, row[COLUMN.RAWID].ToString());

                            dsMapping = base.Query(sbQuery.ToString(), ll);

                            if (dsMapping.Tables[0].Rows.Count > 0)
                            {
                                string sQuery = "UPDATE MODEL_ATT_MST_SPC SET GROUP_RAWID = NULL WHERE GROUP_RAWID =: GROUP_RAWID";
                                base.Query(sQuery, ll);

                                if (base.ErrorMessage.Length > 0)
                                {
                                    //DSUtil.SetResult(ds, 0, "", base.ErrorMessage);
                                    base.RollBack();
                                    return false;
                                }
                            }

                            //group 삭제
                            string sWhereQuery = "WHERE RAWID = :GROUP_RAWID";
                            base.Delete(TABLE.MODEL_GROUP_ATT_MST_SPC, sWhereQuery, ll);
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_UPDATE))
                    {
                        LinkedList ll = new LinkedList();
                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_UPDATE].Rows)
                        {
                            string sWhereQuery = "WHERE RAWID = :RAWID";
                            ll.Clear();
                            ll.Add(COLUMN.RAWID, row[COLUMN.RAWID].ToString());

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.GROUP_NAME, row[COLUMN.GROUP_NAME].ToString().ToUpper());
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                            base.Update(TABLE.MODEL_GROUP_ATT_MST_SPC, llstFieldData, sWhereQuery, ll);
                        }
                    }

                    if (dsSave.Tables.Contains(TABLE.DATA_SAVE_INSERT))
                    {
                        foreach (DataRow row in dsSave.Tables[TABLE.DATA_SAVE_INSERT].Rows)
                        {
                            decimal groupRawID = base.GetSequence(SEQUENCE.SEQ_MODEL_GROUP_MST_SPC);

                            llstFieldData.Clear();
                            llstFieldData.Add(COLUMN.RAWID, groupRawID);
                            llstFieldData.Add(COLUMN.LOCATION_RAWID, llParam[COLUMN.LOCATION_RAWID].ToString());
                            llstFieldData.Add(COLUMN.AREA_RAWID, llParam[COLUMN.AREA_RAWID].ToString());

                            if (llParam[COLUMN.EQP_MODEL] != null)
                                llstFieldData.Add(COLUMN.EQP_MODEL, llParam[COLUMN.EQP_MODEL].ToString());

                            llstFieldData.Add(COLUMN.GROUP_NAME, row[COLUMN.GROUP_NAME].ToString().ToUpper());
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");
                            llstFieldData.Add(COLUMN.CREATE_BY, sUserId);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserId);

                            base.Insert(TABLE.MODEL_GROUP_ATT_MST_SPC, llstFieldData);
                        }
                    }

                    if (base.ErrorMessage.Length > 0)
                    {
                        //DSUtil.SetResult(ds, 0, "", base.ErrorMessage);
                        base.RollBack();
                        return false;
                    }

                    Commit();
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return bResult;
        }

    }

}
