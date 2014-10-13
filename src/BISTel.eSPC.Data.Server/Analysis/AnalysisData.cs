using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Analysis
{
    public class AnalysisData : DataBase
    {

        #region ::: SELECT

        public DataSet GetParamList(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string strTagetType = string.Empty;
            string strOperation = string.Empty;
            string strParamType = string.Empty;
            StringBuilder sb = null;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();
                if (llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE] != null)
                    strTagetType = llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE].ToString();

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    llstCondition.Add("PARAM_TYPE_CD", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    llstCondition.Add("PARAM_ALIAS", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS]);
                }

                strSQL = @"select distinct PARAM_ALIAS,PARAM_LIST
                            from CHART_VW_SPC
                            where param_type_cd =:PARAM_TYPE_CD                            
                            AND OPERATION_ID =:OPERATION_ID    
                            AND PARAM_ALIAS =:PARAM_ALIAS                        
                            ";
                ds = base.Query(strSQL, llstCondition);

            }
            catch
            {

            }

            return ds;
        }




        public DataSet GetAnalysisChartData(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            StringBuilder sb = null;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                strSQL = @"select distinct param_list 
                            from model_config_mst_spc
                            where PARAM_ALIAS=:PARAM_ALIAS";

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                    llstCondition.Add("PARAM_ALIAS", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS]);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public DataSet GetAnalysisLine(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            StringBuilder sb = null;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                strSQL = @"select * from LOCATION_MST_PP where LINE=:LINE";

                if (llstData[Definition.DynamicCondition_Condition_key.LINE] != null)
                    llstCondition.Add("LINE", llstData[Definition.DynamicCondition_Condition_key.LINE]);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public DataSet GetAnalysisArea(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            StringBuilder sb = null;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                strSQL = @"select * from AREA_MST_PP where AREA=:AREA";

                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                    llstCondition.Add("AREA", llstData[Definition.DynamicCondition_Condition_key.AREA]);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }




        public DataSet GetAnalysisTargetMachine(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string sWhere = string.Empty;

            StringBuilder sb = null;
            string sTargetType = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                if (llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE] != null)
                    sTargetType = llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE].ToString();

                if (sTargetType == "MET")
                {
                    strSQL = @" select distinct eqp_id, lot_id, product_id
                        from MET_DATA_TRX_PP
                        where operation_id=:OPERATION_ID                        
                       -- and lot_id =:LOT_ID
                        --and PRODUCT_ID=:PRODUCT_ID
                        and measure_trx_dtts >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                        order by eqp_id
                        ";
                    //if (llstData[Definition.DynamicCondition_Condition_key.LOT_ID] != null)
                    //    llstCondition.Add("LOT_ID", llstData[Definition.DynamicCondition_Condition_key.LOT_ID]);

                }
                else
                {

                    strSQL = @" select  distinct emp.eqp_id, aa.lot_id, aa.product_id
                            from  EQP_MST_PP emp,
                                    (select eqp_module_id , product_id, lot_id
                                            from EQP_RUN_INFO_TRX_PP
                                            where operation_id=:OPERATION_ID
                                            --and PRODUCT_ID=:PRODUCT_ID
                                            --{0}
                                            and CREATE_DTTS >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                                      ) aa
                            where emp.module_id =aa.eqp_module_id
                            order by emp.eqp_id ";

                    //if (llstData[Definition.DynamicCondition_Condition_key.LOT_ID] != null)
                    //{
                    //    sWhere = string.Format(" and lot_id  like '%{0}%'", llstData[Definition.DynamicCondition_Condition_key.LOT_ID].ToString());
                    //}                                                  
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);

                //if (llstData[Definition.DynamicCondition_Condition_key.PRODUCT_ID] != null)
                //    llstCondition.Add("PRODUCT_ID", llstData[Definition.DynamicCondition_Condition_key.PRODUCT_ID]);

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }
                ds = base.Query(string.Format(strSQL, sWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }









        #endregion



        #region MultiData 조회
        public DataSet GetIsNullMultiOperation(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            StringBuilder sb = null;
            string sWhere = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                if (llstData[Definition.DynamicCondition_Condition_key.FAB] != null)
                {
                    sWhere += " AND FAB =:FAB";
                    llstCondition.Add("FAB", llstData[Definition.DynamicCondition_Condition_key.FAB]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sWhere += " AND OPERATION_ID =:OPERATION_ID";
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);
                }

                strSQL = string.Format(@" SELECT DISTINCT po.OPERATION_ID , oi.description
                                          FROM PROBEDATA_OPERATION  po, operation_id oi
                                          WHERE po.operation_id=oi.operation_id(+)
                                          {0}", sWhere);
                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet GetMultiData(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            StringBuilder sb = null;
            string sWhere = string.Empty;
            string sCol = string.Empty;
            string sGroup = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                sb = new StringBuilder();

                if (llstData[Definition.DynamicCondition_Condition_key.FAB] != null)
                {
                    llstCondition.Add("FAB", llstData[Definition.DynamicCondition_Condition_key.FAB]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM] != null)
                {
                    sWhere = string.Format(" AND ITEM IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.COLUMN_LIST] != null)
                {
                    sCol = llstData[Definition.DynamicCondition_Condition_key.COLUMN_LIST].ToString();
                }

                if (llstData[Definition.DynamicCondition_Condition_key.GROUP] != null)
                {
                    sGroup = llstData[Definition.DynamicCondition_Condition_key.GROUP].ToString();
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);

                    strSQL = string.Format(@" SELECT /*+ INDEX (PROBEDATA_SUMMARY IDX_PROBEDATA_SUMMARY) */ 
                             ITEM
                            ,OPERATION_ID
                            ,{0}
                            ,SUM(SAMPLE_COUNT) AS SAMPLE
                            ,SUM(FAIL_COUNT) AS FAIL_COUNT        
                            ,SUM(SUM)  SUM
                            ,SUM(SUM2) SUM2
                            ,ROUND(SUM(SUM)/SUM(SAMPLE_COUNT),5) AVG   
                            ,ROUND(SUM(FAIL_COUNT)/SUM(SAMPLE_COUNT)*100,5) FAILRATE     
                            ,MEDIAN(MEDIAN) AS MEDIAN
                            ,MIN(MIN) AS MIN
                            ,MAX(MAX) AS MAX                            
                    FROM PROBEDATA_SUMMARY 
                    WHERE FAB=:FAB
                    AND EVENT_DTTS >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    AND EVENT_DTTS <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')                                           
                    AND OPERATION_ID=:OPERATION_ID
                    {1}
                    GROUP BY  ITEM
                         ,OPERATION_ID
                         ,{2}
                         ", sCol, sWhere, sGroup);
                }
                else if (llstData[Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID] != null)
                {
                    llstCondition.Add("MEASURE_OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID]);

                    strSQL = string.Format(@" SELECT /*+ INDEX (PROBEDATA_SUMMARY IDX_PROBEDATA_SUMMARY) */ 
                             ITEM
                            ,OPERATION_ID
                            ,{0}
                            ,SUM(SAMPLE_COUNT) AS SAMPLE
                            ,SUM(FAIL_COUNT) AS FAIL_COUNT        
                            ,SUM(SUM)  SUM
                            ,SUM(SUM2) SUM2
                            ,ROUND(SUM(SUM)/SUM(SAMPLE_COUNT),5) AVG   
                            ,ROUND(SUM(FAIL_COUNT)/SUM(SAMPLE_COUNT)*100,5) FAILRATE     
                            ,MEDIAN(MEDIAN) AS MEDIAN
                            ,MIN(MIN) AS MIN
                            ,MAX(MAX) AS MAX                            
                    FROM PROBEDATA_SUMMARY 
                    WHERE FAB=:FAB
                    AND EVENT_DTTS >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    AND EVENT_DTTS <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')                                           
                    AND MEASURE_OPERATION_ID=:MEASURE_OPERATION_ID
                    {1}
                    GROUP BY  ITEM
                         ,OPERATION_ID
                         ,{2}
                         ", sCol, sWhere, sGroup);
                }
                else
                { }

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public string GetMOCVDName(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string strRtn = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.SUBSTRATE_ID] != null)
                {
                    llstCondition.Add("SUBSTRATE_ID", llstData[Definition.DynamicCondition_Condition_key.SUBSTRATE_ID]);
                }

                strSQL = @"select * 
                            from SUBSTRATE_MOCVD_MES
                            where SUBSTRATE_ID =:SUBSTRATE_ID";

                ds = base.Query(strSQL, llstCondition);

                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    strRtn = ds.Tables[0].Rows[0][Definition.CHART_COLUMN.MOCVDNAME].ToString();
                }
            }
            catch (Exception ex)
            {

            }

            return strRtn;
        }

        #endregion


        #region : Link Multi Data Page to Analysis


        public DataSet GetChartCode()
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;

            try
            {
                strSQL = string.Format(@"SELECT CODE, NAME  
										   FROM CODE_MST_PP 
										  WHERE CATEGORY = 'SPC_ANALYSIS_CHART'
											AND CODE IN ('CA', 'RUN')"); //현재는 Run Chart와 상관분석 차트만 지원. 향후 Chart 종류가 추가되면 이부분은 수정.

                ds = base.Query(strSQL, null);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetTargetOperation(byte[] baData)
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;
            string sWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sWhere = llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();
                }

                strSQL = string.Format(@"SELECT proc_data_key, met_data_key 
										   FROM proc_met_data_map_mst_pp 
										  WHERE met_data_key='OPERATION_ID={0};'", sWhere);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        #endregion
    }
}
