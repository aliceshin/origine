using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Report
{
    public class SPCControlChartDataCall : DataBase
    {
        #region ::: SELECT

        public DataSet GetSPCControlChartData(byte[] baData, bool isATT)
        {
            DataSet ds = new DataSet();

            string strWhere = string.Empty;
            string strSQL = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (isATT)
                {
                    strSQL =
                    @"SELECT  dts.rawid,
						    mms.spc_model_name,
						    to_char(dts.spc_start_dtts,'yyyy-MM-dd') AS WORKDATE ,
						    mcms.rawid as model_config_rawid,
						    mcms.param_alias,                            
						    mcms.main_yn,                                                
						    mcoms.default_chart_list,
						    nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,       
						    nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
						    nvl(mcoms.RESTRICT_SAMPLE_HOURS,0) RESTRICT_SAMPLE_HOURS, 
						    dts.file_data ,
						    amp.area,
						    dts.toggle
				    FROM  model_config_att_mst_spc mcms
					     ,model_att_mst_spc mms
					     ,model_config_opt_att_mst_spc mcoms
					     ,data_trx_spc dts 
					     ,AREA_MST_PP amp
				    WHERE mcms.model_rawid=mms.rawid
				    {0}                             
				    AND mcoms.model_config_rawid = mcms.rawid 
				    AND mcms.rawid=dts.model_config_rawid                      
				    AND  (
					     (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
					     (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
					    )
				    AND mms.AREA_RAWID=AMP.RAWID     
				    ORDER BY dts.spc_start_dtts";
                }
                else
                {
                    strSQL =
                    @"SELECT  dts.rawid,
                            mms.spc_model_name,
                            to_char(dts.spc_start_dtts,'yyyy-MM-dd') AS WORKDATE ,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,  
                            mcms.main_yn,                                                
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,       
                            nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
                            nvl(mcoms.RESTRICT_SAMPLE_HOURS,0) RESTRICT_SAMPLE_HOURS, 
                            dts.file_data ,
                            amp.area,
                            dts.toggle,
                            dts.sample_count
                    FROM  model_config_mst_spc mcms
                         ,model_mst_spc mms
                         ,model_config_opt_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP amp
                    WHERE mcms.model_rawid=mms.rawid
                    {0}                             
                    AND mcoms.model_config_rawid = mcms.rawid 
                    AND mcms.rawid=dts.model_config_rawid                      
                    AND  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    AND mms.AREA_RAWID=AMP.RAWID     
                    ORDER BY dts.spc_start_dtts";                    
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere += string.Format(" AND amp.AREA  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    strWhere += string.Format(" AND mcms.PARAM_TYPE_CD   = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    strWhere += string.Format(" AND mcms.rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }


                ds = base.Query(string.Format(strSQL, strWhere), llstCondition);
            }
            catch
            {

            }

            return ds;
        }

        //SPC-704
        public DataSet GetSPCMulControlChartData(byte[] baData, bool isATT)
        {
            DataSet ds = new DataSet();
            string strWhere = string.Empty;
            string strSQL = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (isATT)
                {
                    strSQL =
                    @"SELECT  dts.rawid,
						    mms.spc_model_name,
						    to_char(dts.spc_start_dtts,'yyyy-MM-dd') AS WORKDATE ,
						    mcms.rawid as model_config_rawid,
						    mcms.param_alias,
						    mcms.main_yn,                                                
						    mcoms.default_chart_list,
						    nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,       
						    nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT, 
						    dts.file_data ,
						    amp.area,
						    dts.toggle,
						    dts.sample_count
				    FROM  model_config_att_mst_spc mcms
					     ,model_att_mst_spc mms
					     ,model_config_opt_att_mst_spc mcoms
					     ,data_trx_spc dts 
					     ,AREA_MST_PP amp
				    WHERE mcms.model_rawid=mms.rawid
				    {0}                             
				    AND mcoms.model_config_rawid = mcms.rawid 
				    AND mcms.rawid=dts.model_config_rawid                      
				    AND  (
					     (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
					     (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
					    )
				    AND mms.AREA_RAWID=AMP.RAWID     
				    ORDER BY dts.spc_start_dtts";
                }
                else
                {
                    strSQL =
                    @"SELECT  dts.rawid,
                            mms.spc_model_name,
                            to_char(dts.spc_start_dtts,'yyyy-MM-dd') AS WORKDATE ,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,  
                            mcms.main_yn,                                                
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,       
                            nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT, 
                            dts.file_data ,
                            amp.area,
                            dts.toggle,
                            dts.sample_count
                    FROM  model_config_mst_spc mcms
                         ,model_mst_spc mms
                         ,model_config_opt_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP amp
                    WHERE mcms.model_rawid=mms.rawid
                    {0}                             
                    AND mcoms.model_config_rawid = mcms.rawid 
                    AND mcms.rawid=dts.model_config_rawid                      
                    AND  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    AND mms.AREA_RAWID=AMP.RAWID     
                    ORDER BY dts.spc_start_dtts";
                }            
                
                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere += string.Format(" AND amp.AREA  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    strWhere += string.Format(" AND mcms.PARAM_TYPE_CD   = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    strWhere += string.Format(" AND mcms.rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }


                ds = base.Query(string.Format(strSQL, strWhere), llstCondition);

            }
            catch (Exception ex)
            {

            }


            return ds;
        }

        public DataSet GetSPCControlChartToDayData(byte[] baData, bool isATT)
        {
            DataSet ds = new DataSet();

            string strWhere = string.Empty;
            string strSQL = string.Empty;
            string strDataList = string.Empty;
            string strContextList = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();

                llstData.Add("PROPERTY", "TEMP_TRX_COL_CNT");
                strSQL = " SELECT PROPERTY_VALUE FROM CONFIG_PROPERTY_MST_PP WHERE PROPERTY_NAME = :PROPERTY ";
                DataSet dsTemp = base.Query(strSQL, llstData);

                if (dsTemp != null && dsTemp.Tables != null && dsTemp.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        int idxTemp = Convert.ToInt32(dsTemp.Tables[0].Rows[0][0].ToString());

                        if (idxTemp > 1)
                        {
                            strDataList = "dtts.data_list,";
                            strContextList = "dtts.context_list,";

                            for (int i = 1; i < idxTemp; i++)
                            {
                                strDataList += "dtts.data_list" + i.ToString() + ",";
                                strContextList += "dtts.context_list" + i.ToString() + ",";
                            }
                        }
                        else
                        {
                            strDataList = "dtts.data_list,";
                            strContextList = "dtts.context_list,";
                        }

                    }
                    catch
                    {
                        strDataList = "dtts.data_list,";
                        strContextList = "dtts.context_list,";
                    }
                }
                else
                {
                    strDataList = "dtts.data_list,";
                    strContextList = "dtts.context_list,";
                }

                llstData.Clear();
                strSQL = "";

                llstData.SetSerialData(baData);

                if (isATT)
                {
                    strSQL =
                   @"SELECT   /*+ INDEX(dtts IDX_DATA_TEMP_TRX_SPC_PK)*/
					        mms.spc_model_name,
					        to_char(dtts.spc_data_dtts,'yyyy-MM-dd') AS WORKDATE,
					        mcms.rawid as model_config_rawid,
					        mcms.param_alias,  
					        mcms.main_yn,                                                        
					        mcoms.default_chart_list,
					        nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,
					        nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
					        nvl(mcoms.RESTRICT_SAMPLE_HOURS,0) RESTRICT_SAMPLE_HOURS, 
					        to_char(dtts.spc_data_dtts, 'yyyy-mm-dd hh24:mi:ss.ddd') as TIME,      
					        {0} 
					        {1}
					        amp.area,
					        NVL(dtts.toggle_yn, 'N') toggle_yn
			        FROM  data_temp_trx_spc dtts 
				         ,model_config_att_mst_spc mcms
				         ,model_config_opt_att_mst_spc mcoms                     
				         ,model_att_mst_spc mms 
				         ,AREA_MST_PP amp
			        WHERE dtts.spc_data_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
			        AND dtts.spc_data_dtts <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
			        {2}
			        AND dtts.model_config_rawid = mcms.rawid                               
			        AND mcms.model_rawid=mms.rawid                
			        AND mcoms.model_config_rawid = mcms.rawid                 
			        AND mms.AREA_RAWID=AMP.RAWID     
			          ";
                }
                else
                {
                    strSQL =
                   @"SELECT   /*+ INDEX(dtts IDX_DATA_TEMP_TRX_SPC_PK)*/
                            mms.spc_model_name,
                            to_char(dtts.spc_data_dtts,'yyyy-MM-dd') AS WORKDATE,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            mcms.param_type_cd,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,   
                            mcms.main_yn,                                                        
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,
                            nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
                            nvl(mcoms.RESTRICT_SAMPLE_HOURS,0) RESTRICT_SAMPLE_HOURS, 
                            to_char(dtts.spc_data_dtts, 'yyyy-mm-dd hh24:mi:ss.ddd') as TIME,      
                            {0} 
                            {1}
                            amp.area,
                            NVL(dtts.toggle_yn, 'N') toggle_yn
                    FROM  data_temp_trx_spc dtts 
                         ,model_config_mst_spc mcms
                         ,model_config_opt_mst_spc mcoms                     
                         ,model_mst_spc mms 
                         ,AREA_MST_PP amp
                    WHERE dtts.spc_data_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    AND dtts.spc_data_dtts <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    {2}
                    AND dtts.model_config_rawid = mcms.rawid                               
                    AND mcms.model_rawid=mms.rawid                
                    AND mcoms.model_config_rawid = mcms.rawid                 
                    AND mms.AREA_RAWID=AMP.RAWID     
                      ";
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString().Split(',').Length > 1)
                    {
                        strWhere = string.Format(" AND dtts.model_config_rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                    }
                    else
                    {
                        strWhere = " AND dtts.model_config_rawid  = :model_config_rawid ";
                        llstCondition.Add("model_config_rawid", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                    }
                }

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }

                ds = base.Query(string.Format(strSQL, strContextList, strDataList, strWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetSPCChartViewSumTempData(byte[] baData, bool isATT)
        {
            DataSet ds = new DataSet();

            string strWhere = string.Empty;
            string strSQL = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (isATT)
                {
                    strSQL =
                   @"SELECT   
					        mms.spc_model_name,
					        to_char(dstts.spc_data_dtts,'yyyy-MM-dd') AS WORKDATE,
					        mcms.rawid as model_config_rawid,
					        mcms.param_alias, 
					        mcms.main_yn,                                                        
					        mcoms.default_chart_list,
					        nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,
					        nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
					        nvl(mcoms.RESTRICT_SAMPLE_HOURS, 0) RESTRICT_SAMPLE_HOURS,  
					        to_char(dstts.spc_data_dtts, 'yyyy-mm-dd hh24:mi:ss.ddd') as TIME,      
					        dstts.context_list, 
					        dstts.param_value data_list,
					        dstts.raw_ocap_list raw_ocap_list,
					        amp.area, '' toggle_yn, '' ocap_rawid
			        FROM  data_sum_tmp_trx_spc dstts 
				         ,model_config_att_mst_spc mcms
				         ,model_config_opt_att_mst_spc mcoms                     
				         ,model_att_mst_spc mms 
				         ,AREA_MST_PP amp
			        WHERE dstts.spc_data_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
			        AND dstts.spc_data_dtts <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
			        {0}
			        AND dstts.model_config_rawid = mcms.rawid                               
			        AND mcms.model_rawid=mms.rawid                
			        AND mcoms.model_config_rawid = mcms.rawid                 
			        AND mms.AREA_RAWID=AMP.RAWID     
			          ";
                }
                else
                {
                    strSQL =
                    @"SELECT   
                            mms.spc_model_name,
                            to_char(dstts.spc_data_dtts,'yyyy-MM-dd') AS WORKDATE,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            mcms.param_type_cd,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,   
                            mcms.main_yn,                                                        
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS,
                            nvl(mcoms.RESTRICT_SAMPLE_COUNT,0) RESTRICT_SAMPLE_COUNT,
                            nvl(mcoms.RESTRICT_SAMPLE_HOURS, 0) RESTRICT_SAMPLE_HOURS,  
                            to_char(dstts.spc_data_dtts, 'yyyy-mm-dd hh24:mi:ss.ddd') as TIME,      
                            dstts.context_list, 
                            dstts.param_value data_list,
                            dstts.raw_ocap_list raw_ocap_list,
                            amp.area, '' toggle_yn, '' ocap_rawid
                    FROM  data_sum_tmp_trx_spc dstts 
                            ,model_config_mst_spc mcms
                            ,model_config_opt_mst_spc mcoms                     
                            ,model_mst_spc mms 
                            ,AREA_MST_PP amp
                    WHERE dstts.spc_data_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    AND dstts.spc_data_dtts <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    {0}
                    AND dstts.model_config_rawid = mcms.rawid                               
                    AND mcms.model_rawid=mms.rawid                
                    AND mcoms.model_config_rawid = mcms.rawid                 
                    AND mms.AREA_RAWID=AMP.RAWID     
                        ";
                }
                
                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString().Split(',').Length > 1)
                    {
                        strWhere = string.Format(" AND dstts.model_config_rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                    }
                    else
                    {
                        strWhere = " AND dstts.model_config_rawid  = :model_config_rawid ";
                        llstCondition.Add("model_config_rawid", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                    }
                }

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }

                ds = base.Query(string.Format(strSQL, strWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetMetEQPData(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string strWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    strWhere += string.Format("and evs.eqpmodel ='{0}'", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_ID] != null)
                {
                    strWhere += string.Format("and evs.eqp_id  ='{0}'", llstData[Definition.DynamicCondition_Condition_key.EQP_ID].ToString());
                }

                strSQL = string.Format(@"SELECT distinct memp.eqp_id 
                                        FROM MET_EQP_MST_PP memp,
                                            MET_PROC_DATA_LNK_PP mpd,
                                            EQP_VW_SPC evs
                                        WHERE memp.module_id=mpd.eqp_module_id
                                        AND MPD.MAIN_MODULE_ID =evs.module_id
                                        {0} ", strWhere);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        #region

        public DataSet GetSPCModelConfigSearch(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string strMAIN_YN = string.Empty;
            string strSQLWhere = string.Empty;
            string sWhere = string.Empty;
            StringBuilder sb = null;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList whereFieldData = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                //=============================================================================================

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    sWhere += " AND mms.LOCATION_RAWID=:LINE_RAWID ";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sWhere += " AND mcms.PARAM_TYPE_CD=:PARAM_TYPE_CD ";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sWhere += string.Format(" AND mms.AREA_RAWID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sWhere += string.Format(" AND mms.EQP_MODEL in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID] != null)
                {

                    sWhere += string.Format(" AND mms.model_rawid in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sWhere += string.Format(" AND mcms.PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.MAIN_YN] != null)
                {
                    sWhere += string.Format(" AND mcms.MAIN_YN ='{0}' ", llstData[Definition.DynamicCondition_Condition_key.MAIN_YN].ToString());
                }

                strSQLWhere = @" AND mcms.rawid in (SELECT model_config_rawid FROM MODEL_CONTEXT_MST_SPC 
                                 WHERE context_value in ({0}))";

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_ID] != null)
                {
                    sWhere += string.Format(strSQLWhere, llstData[Definition.DynamicCondition_Condition_key.EQP_ID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sWhere += string.Format(strSQLWhere, llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PRODUCT_ID] != null)
                {
                    sWhere += string.Format(strSQLWhere, llstData[Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString());
                }

                strSQL = string.Format(@" SELECT distinct 
                             mcms.model_rawid
                            ,mms.spc_model_name
                            ,mcms.param_alias
                            ,mcms.MAIN_YN
                            ,mcms.COMPLEX_YN
                            ,mcms.rawid as model_config_rawid                                                                                                              
                            ,mcoms.RESTRICT_SAMPLE_DAYS
                            ,mcoms.RESTRICT_SAMPLE_COUNT
                            ,mcoms.DEFAULT_CHART_LIST                                                    
                    FROM MODEL_MST_SPC mms
                        , MODEL_CONFIG_MST_SPC mcms                        
                        , MODEL_CONFIG_OPT_MST_SPC mcoms
                    WHERE MMS.RAWID = MCMS.MODEL_RAWID                                                                    
                    AND mcms.rawid=mcoms.model_config_rawid  
                    {0}                    
                    ", sWhere);

                ds = base.Query(string.Format(strSQL, sb.ToString()), whereFieldData);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        #endregion


        #endregion
        


        public DataSet GetContextType(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string strWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DISTINCT AA.* ");
                sb.Append(" FROM  ");
                sb.Append("    ( SELECT CODE, NAME, DESCRIPTION FROM CODE_MST_PP ");
                sb.Append("    WHERE CATEGORY='CONTEXT_TYPE' ");

                if (llstData[Definition.DynamicCondition_Condition_key.USE_YN] != null)
                {
                    sb.Append("    AND USE_YN=:USE_YN     ");
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.USE_YN, llstData[Definition.DynamicCondition_Condition_key.USE_YN].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EXCLUDE] != null)
                {
                    sb.AppendFormat("    AND CODE NOT IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EXCLUDE].ToString());
                }

                sb.Append("    UNION ");
                sb.Append("    SELECT CODE, NAME, DESCRIPTION FROM CODE_MST_PP ");
                sb.Append("    WHERE CATEGORY='SPC_CONTEXT_TYPE' ");

                if (llstData[Definition.DynamicCondition_Condition_key.USE_YN] != null)
                {
                    sb.Append("    AND USE_YN=:USE_YN     ");
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EXCLUDE] != null)
                {
                    sb.AppendFormat("    AND CODE NOT IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EXCLUDE].ToString());
                }

                sb.Append("    ) AA ");
                sb.Append("WHERE RTRIM(AA.CODE) IS NOT NULL ");
                sb.Append("ORDER BY AA.NAME ");

                ds = base.Query(sb.ToString(), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public int GetTheNumberOfSubConfigOfModel(string modelConfigRawid, bool isATT)
        {
            //ATT에 따른 변화 부분, KBLEE, START
            string modelConfigMstTblName = string.Empty;

            if (isATT)
            {
                modelConfigMstTblName = TABLE.MODEL_CONFIG_ATT_MST_SPC;
            }
            else
            {
                modelConfigMstTblName = TABLE.MODEL_CONFIG_MST_SPC;
            }
            //ATT에 따른 변화 부분, KBLEE, END

            string query =
                @" SELECT count(1) 
                   FROM " + modelConfigMstTblName +
                @" WHERE model_rawid = (SELECT model_rawid 
                                        FROM " + modelConfigMstTblName + 
                 "                      WHERE rawid = :rawid) ";

            LinkedList llstCondition = new LinkedList();

            try
            {
                llstCondition.Add("rawid", modelConfigRawid);

                DataSet ds = base.Query(query, llstCondition);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString()) - 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
            }

            return -1;
        }

        public bool SaveToggleInformation(string[] rawIDs, string[] toggles, string[] modelConfigRawIDs, string[] spcDataDttses, string[] toggleYNs)
        {
            try
            {
                this.BeginTrans();

                for (int i = 0; i < rawIDs.Length; i++)
                {
                    LinkedList fieldData = new LinkedList();
                    fieldData.Add(Definition.COL_TOGGLE, toggles[i]);
                    LinkedList whereData = new LinkedList();
                    whereData.Add(":rawid", rawIDs[i]);

                    if (1 > this.Update(Definition.TableName.DATA_TRX_SPC, fieldData, " WHERE rawid = :rawid", whereData))
                    {
                        throw new Exception();
                    }
                }

                for (int i = 0; i < modelConfigRawIDs.Length; i++)
                {
                    LinkedList fieldData = new LinkedList();
                    fieldData.Add(Definition.COL_TOGGLE_YN, toggleYNs[i]);
                    LinkedList whereData = new LinkedList();
                    whereData.Add(":modelconfigrawid", modelConfigRawIDs[i]);
                    whereData.Add(":spc_data_dtts", spcDataDttses[i]);

                    if (1 > this.Update(Definition.TableName.DATA_TEMP_TRX_SPC, fieldData, " WHERE MODEL_CONFIG_RAWID = :modelconfigrawid AND to_char(spc_data_dtts, 'yyyy-mm-dd hh24:mi:ss.ddd') = :spc_data_dtts ", whereData))
                    {
                        throw new Exception();
                    }
                }

                this.Commit();

                return true;
            }
            catch
            {
                this.RollBack();
                return false;
            }
        }
        
        //SPC-678
        public string GetParamAlias(byte[] baData)
        {
            DataSet dsResult = new DataSet();
            string strReturn = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sb = new StringBuilder();

                sb.Append(" SELECT eqp_param_alias                     ");
                sb.Append("   FROM eqp_sum_param_mst_pp                ");
                sb.Append("  WHERE eqp_module_id = :EQP_MODULE_ID      ");
                sb.Append("    AND eqp_dcp_id = :EQP_DCP_ID            ");
                sb.Append("    AND sum_param_alias = :SUM_PARAM_ALIAS  ");

                if (llstData[Definition.CONDITION_KEY_EQP_MODULE_ID] != null)
                {
                    llstCondition.Add(Definition.CONDITION_KEY_EQP_MODULE_ID, llstData[Definition.CONDITION_KEY_EQP_MODULE_ID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_EQP_DCP_ID] != null)
                {
                    llstCondition.Add(Definition.CONDITION_KEY_EQP_DCP_ID, llstData[Definition.CONDITION_KEY_EQP_DCP_ID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_SUM_PARAM_ALIAS] != null)
                {
                    llstCondition.Add(Definition.CONDITION_KEY_SUM_PARAM_ALIAS, llstData[Definition.CONDITION_KEY_SUM_PARAM_ALIAS].ToString());
                }

                dsResult = base.Query(sb.ToString(), llstCondition);

                if (dsResult != null)
                {
                    strReturn = dsResult.Tables[0].Rows[0][0].ToString();
                }
            }
            catch
            {
            }

            return strReturn;
        }
    }
}
