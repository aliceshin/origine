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
    public class ProcessCapabilityData : DataBase
    {

        #region ::: SELECT

        public DataSet GetModelContext(byte[] baData)
        {
        
        
            DataSet ds = new DataSet();   
            string strSQL =string.Empty;     
            string strWhere = string.Empty;    
                    
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);


                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere = string.Format("  AND am.AREA IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }
                
                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    strWhere += string.Format(" AND mcms.PARAM_TYPE_CD   = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    strWhere += string.Format(" AND mcms.MODEL_RAWID  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                strSQL = string.Format(@"
                    SELECT *
                    FROM MODEL_CONTEXT_MST_SPC
                    WHERE MODEL_CONFIG_RAWID IN (SELECT mcms.RAWID
                                                FROM MODEL_MST_SPC mms, MODEL_CONFIG_MST_SPC mcms, AREA_MST_PP am    
                                                WHERE mms.rawid=mcms.model_rawid
                                                and mms.LOCATION_RAWID =:LINE_RAWID
                                                and mms.area_rawid=am.rawid                                                
                                                {0}                                                
                                                )
                    ORDER BY MODEL_CONFIG_RAWID,KEY_ORDER  ", strWhere);

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);
                }                
                    
                                                         
                ds = base.Query(strSQL, llstCondition);
            }
            catch
            {

            }

            return ds;
        }
        public DataSet GetProcessCapabilityList(byte[] baData)
        {
            DataSet ds = new DataSet();   
            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            string strSQL =string.Empty;     
            string strWhere = string.Empty;    
            string strCol = string.Empty;
            string strGroup = string.Empty;
                    
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere = string.Format("  AND am.AREA IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }
            
                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    strWhere += string.Format(" AND mcms.PARAM_TYPE_CD   = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    strWhere += string.Format(" AND MMS.rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }



                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                    {
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM-DD') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM-DD')  ";
                        
                    }
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                    {
                        // JIRA No.SPC-594 [GSMC] No any weekly data in eSPC Ppk report - 2011.08.29 by ANDREW KO
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'IW') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'IW')  ";
                        //strCol = " ,TO_CHAR(to_date(to_char(PTS.PPK_START_DTTS, 'YYYYMMDD')), 'IW') Period ";
                        //strGroup = " ,TO_CHAR(to_date(to_char(PTS.PPK_START_DTTS, 'YYYYMMDD')), 'IW')  ";
                    }
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                    {
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM')  ";
                    }
                    else if (strType == Definition.PERIOD_PPK.NONE)
                    {
                        strCol = " ,'-' Period ";
                        strGroup = " ,'-'  ";
                    }
                }
                else
                {
                    strCol = " ,'-' Period ";
                    strGroup = " ,'-'  ";
                }


                strSQL = string.Format(@"SELECT /*+ ordered  use_nl( mcms pts ) */ distinct 
                               MCMS.PARAM_ALIAS, MCMS.PARAM_TYPE_CD
                              ,am.AREA                                                                                       
                              ,NVL(MCMS.COMPLEX_YN,'N')  AS COMPLEX_YN
                              ,MCMS.MAIN_YN
                              ,MCOMS.DEFAULT_CHART_LIST                    
                              ,MCMS.RAWID as model_config_rawid                                                                                                                                                                 
                               {0} 
                              ,PTS.LSL||'~'||PTS.USL AS SPEC
                              ,PTS.USL
                              ,PTS.LSL         
                              ,SUM(PTS.SUM) SUM                     
                              ,(ROUND(SUM(pts.SUM) / SUM(pts.COUNT),5)) AS AVG                  
                              ,SUM(PTS.SUM_SQUARED) AS SUM_SQUARED                                                             
                              ,MIN(PTS.MIN) MIN
                              ,MAX(PTS.MAX) MAX                              
                              ,SUM(PTS.LOT_COUNT) AS LOT_QTY                              
                              ,SUM(PTS.COUNT) AS SAMPLE_QTY
                              ,NVL(SUM(PTS.CPK_SUM), 0) CPK_SUM
                              ,NVL((ROUND(SUM(PTS.CPK_SUM) / SUM(PTS.CPK_COUNT),5)), 0) AS CPK_AVG
                              ,NVL(SUM(PTS.CPK_SUM_SQUARED), 0) AS CPK_SUM_SQUARED                                                             
                              ,NVL(SUM(PTS.CPK_COUNT), 0) AS CPK_SAMPLE_QTY
                              ,NVL(SUM(PTS.OOC_COUNT), 0) AS OOC_COUNT
                              ,NVL(SUM(PTS.OOS_COUNT), 0) AS OOS_COUNT
                        from AREA_MST_PP am
                             ,MODEL_MST_SPC MMS
                             ,MODEL_CONFIG_MST_SPC MCMS
                             ,MODEL_CONFIG_OPT_MST_SPC MCOMS
                             ,PPK_TRX_SPC PTS               
                        WHERE 1=1
                        {1}
                        AND mms.area_rawid=am.RAWID       
                        AND MMS.LOCATION_RAWID =:LINE_RAWID 
                        AND MCMS.MODEL_RAWID=MMS.RAWID
                        AND MCMS.RAWID=PTS.MODEL_CONFIG_RAWID
                        AND PTS.PPK_DATE >=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') 
                        AND PTS.PPK_DATE <=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')   
                        AND (
                             (PTS.PPK_START_DTTS >=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND PTS.PPK_START_DTTS<=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                             (PTS.PPK_END_DTTS>=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND PTS.PPK_END_DTTS<=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))                             
                            )                                                                                                                      
                        AND MCOMS.MODEL_CONFIG_RAWID = MCMS.RAWID  
                        GROUP BY MCMS.PARAM_ALIAS, MCMS.PARAM_TYPE_CD
                              ,am.AREA                                                                                       
                              ,NVL(MCMS.COMPLEX_YN,'N')
                              ,MCMS.MAIN_YN
                              ,MCOMS.DEFAULT_CHART_LIST
                              ,MCMS.RAWID                                                                                                                                                           
                               {2}                               
                              ,PTS.USL
                              ,PTS.LSL                                                                                                             
                              ", strCol, strWhere, strGroup);

                                
                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);
                }                
                                 

                if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                {
                    llstCondition.Add("SPC_START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                {
                    llstCondition.Add("SPC_END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                }

                DataSet dsTemp = base.Query(strSQL, llstCondition);

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

                    dsResult.Tables.Add(TABLE.PPK_TRX_SPC);
                    dsResult.Tables[TABLE.PPK_TRX_SPC].Merge(dtExcel);
                }

                dt = dsResult.Tables[0].Copy();   
                dt.TableName = TABLE.PPK_TRX_SPC;            
                ds.Tables.Add(dt);

                try{
                    dsResult = GetModelContext(baData);
                    dt = dsResult.Tables[0].Copy();
                    dt.TableName = TABLE.MODEL_CONTEXT_MST_SPC;
                    ds.Tables.Add(dt);
                }
                catch{
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public DataSet GetPpkReport(byte[] baData)
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;
            string strWhere = string.Empty;
            string strCol = string.Empty;
            

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                strSQL = @"select  mms.spc_model_name,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,  
                            mcms.main_yn,                                                
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS
                            {0}
                            ,dts.file_data
                            ,am.AREA
                    from  model_config_mst_spc mcms
                         ,model_mst_spc mms
                         ,model_config_opt_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP am  
                    where  1=1
                    {1}                    
                    and mcms.model_rawid=mms.rawid                    
                    and mcoms.model_config_rawid = mcms.rawid 
                    and mcms.rawid=dts.model_config_rawid  
                    and  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    and mms.area_rawid=am.rawid                       
                    ";


                if (llstData[Definition.DynamicCondition_Condition_key.MAIN_YN] != null)
                {
                    strWhere += string.Format("and mcms.main_yn = '{0}'", llstData[Definition.DynamicCondition_Condition_key.MAIN_YN].ToString());
                }
                
               
                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {                                    
                    strWhere += string.Format("and mcms.rawid in ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    strWhere += string.Format("and mcms.param_alias in ({0})", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }
                

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                    {
                        strCol = " ,to_char(dts.spc_start_dtts, 'YYYY-MM-DD') Period ";                        
                    }
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                    {
                        strCol = " ,TO_CHAR(to_date(to_char(spc_start_dtts, 'YYYYMMDD')), 'IW') Period ";                        
                    }
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                    {
                        strCol = " ,to_char(dts.spc_start_dtts, 'YYYY-MM') Period ";                        
                    }
                    else if (strType == Definition.PERIOD_PPK.NONE)
                    {
                        strCol = " ,'-' Period ";                        
                    }
                }
                else
                {
                    strCol = " ,'-' Period ";                    
                }

                ds = base.Query(string.Format(strSQL, strCol,strWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }



        public DataSet GetOperationID(byte[] baData)
        {
            DataSet ds = new DataSet();
            DataSet dstemp = new DataSet();
            string strSQL = string.Empty;
            string strWhere = string.Empty;
            Common.CommonData commondata = new Common.CommonData();
            int DescriptionCount = 0;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                dstemp = commondata.GetTableSchemaInfo("OPERATION_ID");

                //table이 없을 때
                if (dstemp.Tables[0].Rows.Count == 0)
                {
                    strSQL = "select '' as operation_id, '' as description  from dual ";
                    ds = base.Query(strSQL);
                }
                else
                {
                    foreach (DataRow dr in dstemp.Tables[0].Rows)
                    {
                        if (dr["COLUMN_NAME"].ToString() == Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION)
                        {
                            DescriptionCount++;
                        }
                    }                    
                    //table도 있고 description column도 있을 때
                    if (DescriptionCount > 0)
                    {
                        if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE] != null)
                        {
                            llstCondition.Add("OPERATION_TYPE", llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE].ToString());
                            strWhere = " AND OPERATION_TYPE =:OPERATION_TYPE";
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                        {
                            llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                            strWhere = " AND OPERATION_ID =:OPERATION_ID";
                        }

                        if (llstData.Contains(Definition.CONDITION_KEY_SEARCH_OPERATION_ID))
                        {
                            string sOperationID = llstData[Definition.CONDITION_KEY_SEARCH_OPERATION_ID].ToString();
                            strWhere += string.Format(" AND (OPERATION_ID like '%{0}%' or DESCRIPTION like '%{1}%')", sOperationID, sOperationID);
                        }


                        strSQL = @"SELECT OPERATION_ID,OPERATION_ID||' '||DESCRIPTION  DESCRIPTION
                            FROM OPERATION_ID
                            WHERE 1=1
                            {0}
                            ORDER BY OPERATION_ID
                            ";
                        ds = base.Query(string.Format(strSQL, strWhere), llstCondition);
                    }
                    //table은 있는데 description column 없을 때
                    else
                    {
                        if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE] != null)
                        {
                            llstCondition.Add("OPERATION_TYPE", llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE].ToString());
                            strWhere = " AND OPERATION_TYPE =:OPERATION_TYPE";
                        }

                        if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                        {
                            llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                            strWhere = " AND OPERATION_ID =:OPERATION_ID";
                        }

                        if (llstData.Contains(Definition.CONDITION_KEY_SEARCH_OPERATION_ID))
                        {
                            string sOperationID = llstData[Definition.CONDITION_KEY_SEARCH_OPERATION_ID].ToString();
                            strWhere += string.Format(" AND (OPERATION_ID like '%{0}%' or DESCRIPTION like '%{1}%')", sOperationID, sOperationID);
                        }


                        strSQL = @"SELECT OPERATION_ID,OPERATION_ID||' '||'-'  DESCRIPTION
                            FROM OPERATION_ID
                            WHERE 1=1
                            {0}
                            ORDER BY OPERATION_ID
                            ";
                        ds = base.Query(string.Format(strSQL, strWhere), llstCondition);
                    }
                }
                
                //OLD
//                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE] != null)
//                {
//                    llstCondition.Add("OPERATION_TYPE", llstData[Definition.DynamicCondition_Condition_key.OPERATION_TYPE].ToString());
//                    strWhere = " AND OPERATION_TYPE =:OPERATION_TYPE";
//                }

//                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
//                {
//                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
//                    strWhere = " AND OPERATION_ID =:OPERATION_ID";
//                }

//                if (llstData.Contains(Definition.CONDITION_KEY_SEARCH_OPERATION_ID))
//                {
//                    string sOperationID = llstData[Definition.CONDITION_KEY_SEARCH_OPERATION_ID].ToString();
//                    strWhere += string.Format(" AND (OPERATION_ID like '%{0}%' or DESCRIPTION like '%{1}%')", sOperationID, sOperationID);
//                } 

                
//                strSQL = @"SELECT OPERATION_ID,OPERATION_ID||' '||DESCRIPTION  DESCRIPTION
//                            FROM OPERATION_ID
//                            WHERE 1=1
//                            {0}
//                            ORDER BY OPERATION_ID
//                            ";
//                ds = base.Query(string.Format(strSQL, strWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }




        /// <summary>
        /// Use Display : ProcessCapabilityUC
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetDataTRXData(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;
            string sWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

//                sqlQuery = @" SELECT DTS.RAWID                                
//                                   DTS.FILE_DATA   
//                                  ,mcms.PARAM_ALIAS                                                                
//                            FROM DATA_TRX_SPC DTS,MODEL_CONFIG_MST_SPC mcms
//                            WHERE DTS.MODEL_CONFIG_RAWID  = :MODEL_CONFIG_RAWID
//                            AND  DTS.MODEL_CONFIG_RAWID=mcms.rawid                          
//                            AND (
//                                 (SPC_START_DTTS >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND SPC_START_DTTS<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
//                                 (SPC_END_DTTS>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND SPC_END_DTTS<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
//                                )  
//                            {0}                           
//                            ORDER BY DTS.SPC_START_DTTS    
//                            ";

                sqlQuery =
                    @"select  dts.rawid,
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
                    from  model_config_mst_spc mcms
                         ,model_mst_spc mms
                         ,model_config_opt_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP amp
                    where mcms.model_rawid=mms.rawid
                    and mcms.rawid = :MODEL_CONFIG_RAWID
                    and mcoms.model_config_rawid = mcms.rawid
                    and mcms.rawid=dts.model_config_rawid                      
                    and  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    {0}   
                    and mms.AREA_RAWID=AMP.RAWID     
                    order by dts.spc_start_dtts";

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

                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string sPeriod = "-";
                    if (llstData[Definition.DynamicCondition_Condition_key.PERIOD] != null)
                     sPeriod = llstData[Definition.DynamicCondition_Condition_key.PERIOD].ToString();
                    
                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                        sWhere = string.Format(" AND TO_CHAR(dts.SPC_START_DTTS, 'YYYY-MM-DD') ='{0}'  ", sPeriod);                    
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                        sWhere = string.Format(" AND to_char(SPC_START_DTTS, 'IW') ='{0}'  ", sPeriod);                                            
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                        sWhere = string.Format(" AND TO_CHAR(dts.SPC_START_DTTS, 'YYYY-MM') ='{0}'", sPeriod);                                           
                }                                         
                ds = base.Query(string.Format(sqlQuery,sWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        
        #endregion 
               
    }

    public class ATTProcessCapabilityData : DataBase
    {

        #region ::: SELECT

        public DataSet GetModelContext(byte[] baData)
        {


            DataSet ds = new DataSet();
            string strSQL = string.Empty;
            string strWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);


                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere = string.Format("  AND am.AREA IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    strWhere += string.Format(" AND mcms.MODEL_RAWID  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }

                strSQL = string.Format(@"
                    SELECT *
                    FROM MODEL_CONTEXT_ATT_MST_SPC
                    WHERE MODEL_CONFIG_RAWID IN (SELECT mcms.RAWID
                                                FROM MODEL_ATT_MST_SPC mms, MODEL_CONFIG_ATT_MST_SPC mcms, AREA_MST_PP am    
                                                WHERE mms.rawid=mcms.model_rawid
                                                and mms.LOCATION_RAWID =:LINE_RAWID
                                                and mms.area_rawid=am.rawid                                                
                                                {0}                                                
                                                )
                    ORDER BY MODEL_CONFIG_RAWID,KEY_ORDER  ", strWhere);

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);
                }


                ds = base.Query(strSQL, llstCondition);
            }
            catch
            {

            }

            return ds;
        }
        public DataSet GetProcessCapabilityList(byte[] baData)
        {
            DataSet ds = new DataSet();
            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            string strSQL = string.Empty;
            string strWhere = string.Empty;
            string strCol = string.Empty;
            string strGroup = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.AREA] != null)
                {
                    strWhere = string.Format("  AND am.AREA IN ({0})", llstData[Definition.DynamicCondition_Condition_key.AREA].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    strWhere += string.Format(" AND MMS.rawid  IN ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString());
                }



                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                    {
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM-DD') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM-DD')  ";

                    }
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                    {
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'IW') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'IW')  ";
                    }
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                    {
                        strCol = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM') Period ";
                        strGroup = " ,to_char(PTS.PPK_START_DTTS, 'YYYY-MM')  ";
                    }
                    else if (strType == Definition.PERIOD_PPK.NONE)
                    {
                        strCol = " ,'-' Period ";
                        strGroup = " ,'-'  ";
                    }
                }
                else
                {
                    strCol = " ,'-' Period ";
                    strGroup = " ,'-'  ";
                }


                strSQL = string.Format(@"SELECT /*+ ordered  use_nl( mcms pts ) */ distinct 
                               MCMS.PARAM_ALIAS ,am.AREA                                                                                       
                              ,MCMS.MAIN_YN
                              ,MCOMS.DEFAULT_CHART_LIST                    
                              ,MCMS.RAWID as model_config_rawid                                                                                                                                                                 
                               {0} 
                              ,PTS.LSL||'~'||PTS.USL AS SPEC
                              ,PTS.USL
                              ,PTS.LSL         
                              ,SUM(PTS.SUM) SUM                     
                              ,(ROUND(SUM(pts.SUM) / SUM(pts.COUNT),5)) AS AVG                  
                              ,SUM(PTS.SUM_SQUARED) AS SUM_SQUARED                                                             
                              ,MIN(PTS.MIN) MIN
                              ,MAX(PTS.MAX) MAX                              
                              ,SUM(PTS.LOT_COUNT) AS LOT_QTY                              
                              ,SUM(PTS.COUNT) AS SAMPLE_QTY
                              ,NVL(SUM(PTS.CPK_SUM), 0) CPK_SUM
                              ,NVL((ROUND(SUM(PTS.CPK_SUM) / SUM(PTS.CPK_COUNT),5)), 0) AS CPK_AVG
                              ,NVL(SUM(PTS.CPK_SUM_SQUARED), 0) AS CPK_SUM_SQUARED                                                             
                              ,NVL(SUM(PTS.CPK_COUNT), 0) AS CPK_SAMPLE_QTY
                              ,NVL(SUM(PTS.OOC_COUNT), 0) AS OOC_COUNT
                              ,NVL(SUM(PTS.OOS_COUNT), 0) AS OOS_COUNT
                        from AREA_MST_PP am
                             ,MODEL_ATT_MST_SPC MMS
                             ,MODEL_CONFIG_ATT_MST_SPC MCMS
                             ,MODEL_CONFIG_OPT_ATT_MST_SPC MCOMS      
                             ,PPK_TRX_SPC PTS
                        WHERE 1=1
                        {1}
                        AND mms.area_rawid=am.RAWID    
                        AND MMS.LOCATION_RAWID =:LINE_RAWID 
                        AND MCMS.MODEL_RAWID=MMS.RAWID
                        AND MCMS.RAWID=PTS.MODEL_CONFIG_RAWID
                        AND PTS.PPK_DATE >=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') 
                        AND PTS.PPK_DATE <=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')                             
                        AND (
                             (PTS.PPK_START_DTTS >=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND PTS.PPK_START_DTTS<=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                             (PTS.PPK_END_DTTS>=TO_TIMESTAMP(:SPC_START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND PTS.PPK_END_DTTS<=TO_TIMESTAMP(:SPC_END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))                             
                            )                                                                                                                      
                        AND MCOMS.MODEL_CONFIG_RAWID = MCMS.RAWID  
                        GROUP BY MCMS.PARAM_ALIAS
                              ,am.AREA                                                                                                                     
                              ,MCMS.MAIN_YN
                              ,MCOMS.DEFAULT_CHART_LIST
                              ,MCMS.RAWID                                                                                                                                                           
                               {2}                               
                              ,PTS.USL
                              ,PTS.LSL                                                                                                             
                              ", strCol, strWhere, strGroup);


                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);
                }


                if (llstData[Definition.CONDITION_KEY_START_DTTS] != null)
                {
                    llstCondition.Add("SPC_START_DTTS", llstData[Definition.CONDITION_KEY_START_DTTS].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_END_DTTS] != null)
                {
                    llstCondition.Add("SPC_END_DTTS", llstData[Definition.CONDITION_KEY_END_DTTS].ToString());
                }

                DataSet dsTemp = base.Query(strSQL, llstCondition);

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

                    dsResult.Tables.Add(TABLE.PPK_TRX_SPC);
                    dsResult.Tables[TABLE.PPK_TRX_SPC].Merge(dtExcel);
                }

                dt = dsResult.Tables[0].Copy();
                dt.TableName = TABLE.PPK_TRX_SPC;
                ds.Tables.Add(dt);

                try
                {
                    dsResult = GetModelContext(baData);
                    dt = dsResult.Tables[0].Copy();
                    dt.TableName = TABLE.MODEL_CONTEXT_ATT_MST_SPC;
                    ds.Tables.Add(dt);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public DataSet GetPpkReport(byte[] baData)
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;
            string strWhere = string.Empty;
            string strCol = string.Empty;


            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                strSQL = @"select  mms.spc_model_name,
                            mcms.rawid as model_config_rawid,
                            mcms.param_alias,
                            NVL (mcms.complex_yn, 'N') AS complex_yn,  
                            mcms.main_yn,                                                
                            mcoms.default_chart_list,
                            nvl(mcoms.RESTRICT_SAMPLE_DAYS,0) RESTRICT_SAMPLE_DAYS
                            {0}
                            ,dts.file_data
                            ,am.AREA
                    from  model_config_mst_spc mcms
                         ,model_mst_spc mms
                         ,model_config_opt_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP am  
                    where  1=1
                    {1}                    
                    and mcms.model_rawid=mms.rawid                    
                    and mcoms.model_config_rawid = mcms.rawid 
                    and mcms.rawid=dts.model_config_rawid  
                    and  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    and mms.area_rawid=am.rawid                       
                    ";


                if (llstData[Definition.DynamicCondition_Condition_key.MAIN_YN] != null)
                {
                    strWhere += string.Format("and mcms.main_yn = '{0}'", llstData[Definition.DynamicCondition_Condition_key.MAIN_YN].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    strWhere += string.Format("and mcms.rawid in ({0})", llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    strWhere += string.Format("and mcms.param_alias in ({0})", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                    {
                        strCol = " ,to_char(dts.spc_start_dtts, 'YYYY-MM-DD') Period ";
                    }
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                    {
                        strCol = " ,TO_CHAR(to_date(to_char(spc_start_dtts, 'YYYYMMDD')), 'IW') Period ";
                    }
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                    {
                        strCol = " ,to_char(dts.spc_start_dtts, 'YYYY-MM') Period ";
                    }
                    else if (strType == Definition.PERIOD_PPK.NONE)
                    {
                        strCol = " ,'-' Period ";
                    }
                }
                else
                {
                    strCol = " ,'-' Period ";
                }

                ds = base.Query(string.Format(strSQL, strCol, strWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        /// <summary>
        /// Use Display : ProcessCapabilityUC
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetDataTRXData(byte[] baData)
        {
            DataSet ds = new DataSet();
            string sqlQuery = string.Empty;
            string sWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                sqlQuery =
                    @"select  dts.rawid,
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
                    from  model_config_att_mst_spc mcms
                         ,model_att_mst_spc mms
                         ,model_config_opt_att_mst_spc mcoms
                         ,data_trx_spc dts 
                         ,AREA_MST_PP amp
                    where mcms.model_rawid=mms.rawid
                    and mcms.rawid = :MODEL_CONFIG_RAWID
                    and mcoms.model_config_rawid = mcms.rawid
                    and mcms.rawid=dts.model_config_rawid                      
                    and  (
                         (dts.spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR
                         (dts.spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND dts.spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3'))
                        )
                    {0}   
                    and mms.AREA_RAWID=AMP.RAWID     
                    order by dts.spc_start_dtts";

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

                if (llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK] != null)
                {
                    string sPeriod = "-";
                    if (llstData[Definition.DynamicCondition_Condition_key.PERIOD] != null)
                        sPeriod = llstData[Definition.DynamicCondition_Condition_key.PERIOD].ToString();

                    string strType = llstData[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                    if (strType == Definition.PERIOD_PPK.DAILY)
                        sWhere = string.Format(" AND TO_CHAR(dts.SPC_START_DTTS, 'YYYY-MM-DD') ='{0}'  ", sPeriod);
                    else if (strType == Definition.PERIOD_PPK.WEEKLY)
                        sWhere = string.Format(" AND to_char(SPC_START_DTTS, 'IW') ='{0}'  ", sPeriod); 
                    else if (strType == Definition.PERIOD_PPK.MONTHLY)
                        sWhere = string.Format(" AND TO_CHAR(dts.SPC_START_DTTS, 'YYYY-MM') ='{0}'", sPeriod);
                }
                ds = base.Query(string.Format(sqlQuery, sWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }



        #endregion

    }   

}
