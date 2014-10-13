using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Data.Server.Compare
{
    public class SPCModelCompareData : DataBase
    {
        public DataSet GetSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder query = new StringBuilder();

            query.Append(
                "SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN from MODEL_MST_SPC mst, MODEL_CONFIG_MST_SPC mcms where mst.RAWID = mcms.MODEL_RAWID ");

            LinkedList llCondition = new LinkedList();

            if(!string.IsNullOrEmpty(lineRawID))
            {
                query.Append(" and mst.location_rawid = :LINE_RAWID ");
                llCondition.Add("LINE_RAWID", lineRawID);
            }
            if(!string.IsNullOrEmpty(areaRawID))
            {
                query.Append(" and mst.AREA_RAWID = :AREA_RAWID");
                llCondition.Add("AREA_RAWID", areaRawID);
            }
            if(!string.IsNullOrEmpty(eqpModel))
            {
                query.Append(" and mst.EQP_MODEL = :EQP_MODEL");
                llCondition.Add("EQP_MODEL", eqpModel);
            }
            if(!string.IsNullOrEmpty(paramAlias))
            {
                query.Append(" and mcms.PARAM_ALIAS = :PARAM_ALIAS");
                llCondition.Add("PARAM_ALIAS", paramAlias);
            }
            if(!string.IsNullOrEmpty(paramTypeCd))
            {
                query.Append(" and mcms.PARAM_TYPE_CD = :PARAM_TYPE_CD");
                llCondition.Add("PARAM_TYPE_CD", paramTypeCd);
            }

            query.Append(" ORDER BY mst.SPC_MODEL_NAME, mcms.RAWID");

            dsReturn = this.Query(query.ToString(), llCondition);

            dsReturn.Tables[0].TableName = TABLE.CHART_VW_SPC;

            List<string> modelConfigRawid = new List<string>();
            foreach(DataRow dr in dsReturn.Tables[0].Rows)
            {
                modelConfigRawid.Add(dr["CHART_ID"].ToString());
            }

            DataSet dsTemp = new DataSet();
            for(int i=0; i < modelConfigRawid.Count; i+= 1000)
            {
                query = new StringBuilder();

                query.Append("SELECT mcms.rawid, mcms.model_config_rawid, mcms.context_key,  ");

                if (useComma)
                    query.Append(" replace(mcms.context_value, ';', ',') as context_value, ");
                else
                    query.Append(" mcms.context_value, ");

                query.Append(" mcms.exclude_list, mcms.key_order, mcms.create_dtts, mcms.create_by, mcms.last_update_dtts, ");
                query.Append(" mcms.last_update_by, mcms.group_yn, mcms.version, aa.name AS context_key_name ");
                query.Append(" FROM MODEL_CONTEXT_MST_SPC mcms, ");
                query.Append("    (select CODE,NAME from code_mst_pp where category='CONTEXT_TYPE'");
                query.Append("    UNION select CODE,NAME from code_mst_pp where category='SPC_CONTEXT_TYPE') AA");
                query.Append(" WHERE mcms.context_key = aa.code");

                string[] array = null;
                
                if(modelConfigRawid.Count >= i + 1000)
                {
                    array = new string[1000];
                    modelConfigRawid.CopyTo(i, array, 0, 1000);
                }
                else
                {
                    array = new string[modelConfigRawid.Count - i];
                    modelConfigRawid.CopyTo(i, array, 0, modelConfigRawid.Count - i);
                }

                query.Append("   AND mcms.MODEL_CONFIG_RAWID IN ('" + string.Join("', '", array) + "')");

                query.Append(" ORDER BY mcms.KEY_ORDER ASC ");

                DataTable dtTemp = this.Query(query.ToString(), llCondition).Tables[0];
                dtTemp.TableName = TABLE.MODEL_CONTEXT_MST_SPC;

                dsTemp.Merge(dtTemp);
            }

            if(dsTemp.Tables.Count> 0)
            {
                DataView dvTemp = dsTemp.Tables[TABLE.MODEL_CONTEXT_MST_SPC].DefaultView;
                dvTemp.Sort = "KEY_ORDER ASC";

                dsReturn.Merge(dvTemp.ToTable(TABLE.MODEL_CONTEXT_MST_SPC));
            }

            return dsReturn;
        }

        public DataSet GetSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder query = new StringBuilder();

            // 
            query.Append(
                "SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN, mcms.CHART_DESCRIPTION from MODEL_MST_SPC mst, MODEL_CONFIG_MST_SPC mcms where mst.RAWID = mcms.MODEL_RAWID ");
            query.Append(" and (mcms.rawid = '" + string.Join("' or mcms.rawid ='", modelConfigRawIDs) + "')");

            DataSet temp = this.Query(query.ToString());
            temp.Tables[0].TableName= TABLE.CHART_VW_SPC;
            dsReturn.Merge(temp);

            // MODEL_CONFIG_MST_SPC
            query = new StringBuilder();
            query.Append("select mcms.*, NVL(OFFSET_YN, 'N') as OFFSET_YN ");
            query.Append("from MODEL_CONFIG_MST_SPC mcms ");
            query.Append(" WHERE 1 = 1 ");
            query.Append(" and (mcms.RAWID = '" + string.Join("' or mcms.RAWID = '", modelConfigRawIDs) + "')");
            query.Append(" ORDER BY mcms.RAWID");

            temp = this.Query(query.ToString());
            temp.Tables[0].TableName= TABLE.MODEL_CONFIG_MST_SPC;
            dsReturn.Merge(temp);

            // MODEL_RULE_MST_SPC
            query = new StringBuilder();
            query.Append(" SELECT A.*, ");
            query.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC "); //dev define columns
            query.Append("   FROM MODEL_RULE_MST_SPC A ");
            query.Append(" WHERE 1 = 1 ");
            query.Append(" and (A.MODEL_CONFIG_RAWID = '" + string.Join("' or A.MODEL_CONFIG_RAWID = '", modelConfigRawIDs) + "')");

            temp = this.Query(query.ToString());
            temp.Tables[0].TableName= TABLE.MODEL_RULE_MST_SPC;
            dsReturn.Merge(temp);

            // MODEL_AUTOCALC_MST_SPC
            query = new StringBuilder();
            query.Append("SELECT RAWID, MODEL_CONFIG_RAWID, AUTOCALC_PERIOD, MIN_SAMPLES, DEFAULT_PERIOD, MAX_PERIOD, CONTROL_LIMIT, CONTROL_THRESHOLD, CALC_COUNT,INITIAL_CALC_COUNT, ");
            query.Append(" STD_CL_YN, RAW_CL_YN, MEAN_CL_YN, RANGE_CL_YN, EWMA_M_CL_YN, EWMA_R_CL_YN, EWMA_S_CL_YN, MA_CL_YN, MS_CL_YN, MR_CL_YN, ");
            query.Append(" STD_CALC_VALUE, RAW_CALC_VALUE, MEAN_CALC_VALUE, RANGE_CALC_VALUE, EWMA_M_CALC_VALUE, EWMA_R_CALC_VALUE, EWMA_S_CALC_VALUE, MA_CALC_VALUE, MS_CALC_VALUE, MR_CALC_VALUE, ");
            query.Append(" STD_CALC_OPTION, RAW_CALC_OPTION, MEAN_CALC_OPTION, RANGE_CALC_OPTION, EWMA_M_CALC_OPTION, EWMA_R_CALC_OPTION, EWMA_S_CALC_OPTION, MA_CALC_OPTION, MS_CALC_OPTION, MR_CALC_OPTION, ");
            query.Append(" STD_CALC_SIDED, RAW_CALC_SIDED, MEAN_CALC_SIDED, RANGE_CALC_SIDED, EWMA_M_CALC_SIDED, EWMA_R_CALC_SIDED, EWMA_S_CALC_SIDED, MA_CALC_SIDED, MS_CALC_SIDED, MR_CALC_SIDED, ");
            query.Append(" ZONE_YN, NVL(SHIFT_CALC_YN, 'N') as SHIFT_CALC_YN, NVL(WITHOUT_IQR_YN, 'N') as WITHOUT_IQR_YN, ");
            query.Append(" NVL(THRESHOLD_OFF_YN, 'N') as THRESHOLD_OFF_YN , NVL(USE_GLOBAL_YN, 'Y') AS USE_GLOBAL_YN, CONTROL_LIMIT_OPTION ");
            query.Append("  FROM MODEL_AUTOCALC_MST_SPC ");
            query.Append("WHERE 1 = 1 ");
            query.Append(" and (MODEL_CONFIG_RAWID = '" + string.Join("' or MODEL_CONFIG_RAWID = '", modelConfigRawIDs) + "')");

            temp = this.Query(query.ToString());
            temp.Tables[0].TableName= TABLE.MODEL_AUTOCALC_MST_SPC;
            dsReturn.Merge(temp);

            return dsReturn;
        }
    }

    public class SPCATTModelCompareData : DataBase
    {
        public DataSet GetSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder query = new StringBuilder();

            query.Append(
                "SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN from MODEL_ATT_MST_SPC mst, MODEL_CONFIG_ATT_MST_SPC mcms where mst.RAWID = mcms.MODEL_RAWID ");

            LinkedList llCondition = new LinkedList();

            if (!string.IsNullOrEmpty(lineRawID))
            {
                query.Append(" and mst.location_rawid = :LINE_RAWID ");
                llCondition.Add("LINE_RAWID", lineRawID);
            }
            if (!string.IsNullOrEmpty(areaRawID))
            {
                query.Append(" and mst.AREA_RAWID = :AREA_RAWID");
                llCondition.Add("AREA_RAWID", areaRawID);
            }
            if (!string.IsNullOrEmpty(eqpModel))
            {
                query.Append(" and mst.EQP_MODEL = :EQP_MODEL");
                llCondition.Add("EQP_MODEL", eqpModel);
            }
            if (!string.IsNullOrEmpty(paramAlias))
            {
                query.Append(" and mcms.PARAM_ALIAS = :PARAM_ALIAS");
                llCondition.Add("PARAM_ALIAS", paramAlias);
            }
            //if(!string.IsNullOrEmpty(paramTypeCd))
            //{
            //    query.Append(" and mcms.PARAM_TYPE_CD = :PARAM_TYPE_CD");
            //    llCondition.Add("PARAM_TYPE_CD", paramTypeCd);
            //}

            query.Append(" ORDER BY mst.SPC_MODEL_NAME, mcms.RAWID");

            dsReturn = this.Query(query.ToString(), llCondition);

            dsReturn.Tables[0].TableName = TABLE.CHART_VW_SPC;

            List<string> modelConfigRawid = new List<string>();
            foreach (DataRow dr in dsReturn.Tables[0].Rows)
            {
                modelConfigRawid.Add(dr["CHART_ID"].ToString());
            }

            DataSet dsTemp = new DataSet();
            for (int i = 0; i < modelConfigRawid.Count; i += 1000)
            {
                query = new StringBuilder();

                query.Append("SELECT mcms.rawid, mcms.model_config_rawid, mcms.context_key,  ");
                
                if(useComma)
                    query.Append(" replace(mcms.context_value, ';', ',') as context_value, ");
                else
                    query.Append(" mcms.context_value, ");

                query.Append(" mcms.exclude_list, mcms.group_yn, mcms.key_order, mcms.version,  ");
                query.Append(" mcms.create_dtts, mcms.create_by, mcms.last_update_dtts, mcms.last_update_by, aa.name AS context_key_name ");
                query.Append(" FROM MODEL_CONTEXT_ATT_MST_SPC mcms, ");
                query.Append("    (select CODE,NAME from code_mst_pp where category='CONTEXT_TYPE'");
                query.Append("    UNION select CODE,NAME from code_mst_pp where category='SPC_CONTEXT_TYPE') AA");
                query.Append(" WHERE mcms.context_key = aa.code");

                string[] array = null;

                if (modelConfigRawid.Count >= i + 1000)
                {
                    array = new string[1000];
                    modelConfigRawid.CopyTo(i, array, 0, 1000);
                }
                else
                {
                    array = new string[modelConfigRawid.Count - i];
                    modelConfigRawid.CopyTo(i, array, 0, modelConfigRawid.Count - i);
                }

                query.Append("   AND mcms.MODEL_CONFIG_RAWID IN ('" + string.Join("', '", array) + "')");

                query.Append(" ORDER BY mcms.KEY_ORDER ASC ");

                DataTable dtTemp = this.Query(query.ToString(), llCondition).Tables[0];
                dtTemp.TableName = TABLE.MODEL_CONTEXT_ATT_MST_SPC;

                dsTemp.Merge(dtTemp);
            }

            if (dsTemp.Tables.Count > 0)
            {
                DataView dvTemp = dsTemp.Tables[TABLE.MODEL_CONTEXT_ATT_MST_SPC].DefaultView;
                dvTemp.Sort = "KEY_ORDER ASC";

                dsReturn.Merge(dvTemp.ToTable(TABLE.MODEL_CONTEXT_ATT_MST_SPC));
            }

            return dsReturn;
        }

        public DataSet GetSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder query = new StringBuilder();

            // 
            query.Append(
                "SELECT mcms.rawid as CHART_ID, mst.SPC_MODEL_NAME, mcms.MAIN_YN, mcms.CHART_DESCRIPTION from MODEL_ATT_MST_SPC mst, MODEL_CONFIG_ATT_MST_SPC mcms where mst.RAWID = mcms.MODEL_RAWID ");
            query.Append(" and (mcms.rawid = '" + string.Join("' or mcms.rawid ='", modelConfigRawIDs) + "')");

            DataSet temp = this.Query(query.ToString());
            temp.Tables[0].TableName = TABLE.CHART_VW_SPC;
            dsReturn.Merge(temp);

            // MODEL_CONFIG_MST_SPC
            query = new StringBuilder();
            query.Append("select mcms.*, NVL(OFFSET_YN, 'N') as OFFSET_YN ");
            query.Append("from MODEL_CONFIG_ATT_MST_SPC mcms ");
            query.Append(" WHERE 1 = 1 ");
            query.Append(" and (mcms.RAWID = '" + string.Join("' or mcms.RAWID = '", modelConfigRawIDs) + "')");
            query.Append(" ORDER BY mcms.RAWID");

            temp = this.Query(query.ToString());
            temp.Tables[0].TableName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
            dsReturn.Merge(temp);

            // MODEL_RULE_MST_SPC
            query = new StringBuilder();
            query.Append(" SELECT A.*, ");
            query.Append("        DECODE(A.USE_MAIN_SPEC_YN,'Y','True','N','False','True') AS USE_MAIN_SPEC "); //dev define columns
            query.Append("   FROM MODEL_RULE_ATT_MST_SPC A ");
            query.Append(" WHERE 1 = 1 ");
            query.Append(" and (A.MODEL_CONFIG_RAWID = '" + string.Join("' or A.MODEL_CONFIG_RAWID = '", modelConfigRawIDs) + "')");

            temp = this.Query(query.ToString());
            temp.Tables[0].TableName = TABLE.MODEL_RULE_ATT_MST_SPC;
            dsReturn.Merge(temp);

            // MODEL_AUTOCALC_MST_SPC
            query = new StringBuilder();
            query.Append("SELECT rawid, model_config_rawid, autocalc_period,             ");
            query.Append("       min_samples, default_period, max_period,                ");
            query.Append("       control_limit, control_threshold,                       ");
            query.Append("       calc_count, initial_calc_count,                         ");
            query.Append("       pn_cl_yn, p_cl_yn, c_cl_yn, u_cl_yn,                    ");
            query.Append("       NVL(SHIFT_CALC_YN, 'N') as SHIFT_CALC_YN,               ");
            query.Append("       NVL (threshold_off_yn, 'N') AS threshold_off_yn         ");
            query.Append("  FROM model_autocalc_att_mst_spc                              ");
            query.Append(" WHERE 1 = 1                                                   ");
            query.Append(" and (MODEL_CONFIG_RAWID = '" + string.Join("' or MODEL_CONFIG_RAWID = '", modelConfigRawIDs) + "')");
            temp = this.Query(query.ToString());
            temp.Tables[0].TableName = TABLE.MODEL_AUTOCALC_ATT_MST_SPC;
            dsReturn.Merge(temp);

            return dsReturn;
        }
    }
}
