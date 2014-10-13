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
    public class AnalysisChartData : DataBase
    {

        #region Select


        /// <summary>
        /// 공정 맵핑
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetAnalysisTargetMachine(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            //string sWhere = string.Empty;

            //StringBuilder sb = null;
            string sTargetType = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);
                //sb = new StringBuilder();

                if (llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE] != null)
                    sTargetType = llstData[Definition.DynamicCondition_Condition_key.TARGET_TYPE].ToString();

                if (sTargetType == "MET")
                {
                    strSQL = @" select distinct eqp_id, lot_id, product_id 
                        from MET_DATA_TRX_PP 
                        where operation_id=:OPERATION_ID 
                        and measure_trx_dtts >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2  
                        and measure_trx_dtts <= TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                        order by eqp_id 
                        ";

                    //                    strSQL = @" select   distinct emp.eqp_id, aa.lot_id, aa.product_id,AA.RECIPE_ID
                    //                            from  EQP_MST_PP emp,
                    //                                    (select eqp_module_id , product_id, lot_id,RECIPE_ID
                    //                                            from EQP_RUN_INFO_TRX_PP
                    //                                            where operation_id=:OPERATION_ID                                            
                    //                                            and CREATE_DTTS >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                    //                                            and CREATE_DTTS <= TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                    //                                      ) aa
                    //                            where emp.module_id =aa.eqp_module_id
                    //                            order by emp.eqp_id
                    //                        ";
                }
                else
                {
                    strSQL = @" select  distinct emp.eqp_id, aa.lot_id, aa.product_id,AA.RECIPE_ID
                            from  EQP_MST_PP emp,
                                    (select eqp_module_id , product_id, lot_id,RECIPE_ID
                                            from EQP_RUN_INFO_TRX_PP
                                            where operation_id=:OPERATION_ID                                            
                                            and CREATE_DTTS >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                                            and CREATE_DTTS <= TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')
                                      ) aa
                            where emp.module_id =aa.eqp_module_id
                            order by emp.eqp_id ";
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);

                if (llstData[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    llstCondition.Add("START_DTTS", llstData[Definition.DynamicCondition_Condition_key.START_DTTS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    llstCondition.Add("END_DTTS", llstData[Definition.DynamicCondition_Condition_key.END_DTTS].ToString());
                }
                //ds = base.Query(string.Format(strSQL, sWhere), llstCondition);
                ds = base.Query(strSQL, llstCondition);
            }
            catch
            {

            }

            return ds;
        }


        public DataSet getLocationInfo(byte[] baData)
        {
            DataSet ds = new DataSet();

            string strSQL = string.Empty;
            string sWhere = string.Empty;

            string sTargetType = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                strSQL = @" select distinct  * from LOCATION_MST_PP where RAWID=:LINE_RAWID ";

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }
                ds = base.Query(string.Format(strSQL, sWhere), llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet GetAnalysisGroup(byte[] baData)
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
                    strSQL = @" select distinct eqp_id, lot_id, product_id,recipe_id
                        from MET_DATA_TRX_PP
                        where operation_id=:OPERATION_ID                                               
                        and measure_trx_dtts >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                        order by eqp_id
                        ";
                }
                else
                {

                    strSQL = @" select  distinct emp.eqp_id, aa.lot_id, aa.product_id, aa.recipe_id
                            from  EQP_MST_PP emp,
                                    (select eqp_module_id , product_id, lot_id,decode(nvl(recipe_id,''),'NULL','',nvl(recipe_id,'')) recipe_id
                                            from EQP_RUN_INFO_TRX_PP
                                            where operation_id=:OPERATION_ID                                            
                                            and CREATE_DTTS >= TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')-2 
                                      ) aa
                            where emp.module_id =aa.eqp_module_id
                            and emp.parent is null
                            order by emp.eqp_id ";
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);

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


        public DataSet getAnalysisChartConfigData(byte[] baData)
        {
            DataSet ds = new DataSet();
            LinkedList llstData = new LinkedList();
            LinkedList llstCondition = new LinkedList();
            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT AA.* ");
            sb.Append(" FROM ANALYSISCHAT_CONFIG_SPC  aa ");
            sb.Append(" WHERE RAWID IN (");
            sb.Append("            select RAWID ");
            sb.Append("            from ANALYSISCHAT_CONFIG_SPC");
            sb.Append("            where LOCATION_RAWID=:LINE_RAWID");
            sb.Append("            and USER_ID=:USER_ID");
            sb.Append("            union ");
            sb.Append("            select RAWID");
            sb.Append("            from ANALYSISCHAT_CONFIG_SPC");
            sb.Append("            where PUBLIC_YN='Y'");
            sb.Append("            )  ");
            sb.Append("  ORDER BY USER_ID,CHART_ALIAS  ");

            try
            {
                llstData.SetSerialData(baData);
                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    llstCondition.Add("LINE_RAWID", llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.USER_ID] != null)
                {
                    llstCondition.Add("USER_ID", llstData[Definition.DynamicCondition_Condition_key.USER_ID]);
                }

                ds = base.Query(sb.ToString(), llstCondition);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }



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

                strSQL = string.Format(@" SELECT DISTINCT po.OPERATION_ID , po.operation_type, oi.description
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

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    llstCondition.Add("OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID]);
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID] != null)
                {
                    llstCondition.Add("MEASURE_OPERATION_ID", llstData[Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID]);
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

                strSQL = string.Format(@" SELECT /*+ INDEX (PROBEDATA_SUMMARY IDX_PROBEDATA_SUMMARY) */ 
                             ITEM AS PARAM_ALIAS                            
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
                    AND (OPERATION_ID=:OPERATION_ID OR MEASURE_OPERATION_ID=:OPERATION_ID)
                    {1}
                    GROUP BY  ITEM                         
                         ,{2}
                         ", sCol, sWhere, sGroup);

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }

            return ds;
        }



        public DataSet getSPECfromModelConfigInfo(byte[] baData)
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;
            string sWhere = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.RAWID] != null)
                {
                    sWhere = string.Format(" where RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.RAWID].ToString());
                }

                strSQL = string.Format(@" select param_alias, upper_spec as USL, lower_spec as LSL 
                                            from MODEL_CONFIG_MST_SPC
                                           {0}", sWhere);

                ds = base.Query(strSQL, null);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }



        public DataSet getSpecialLotGroup(byte[] baData)
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                LinkedList llstCondition = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.USER_ID] != null)
                {
                    llstCondition.Add("USER_ID", llstData[Definition.DynamicCondition_Condition_key.USER_ID].ToString());
                }

                strSQL = string.Format(@" select LOT_ID, LOT_GROUP_NAME 
                                            from SPECIAL_LOT_SPC 
                                           where USER_ID=:USER_ID ");

                if (llstData.Contains("LOT_GROUP_NAME"))
                {
                    //llstCondition.Add("LOT_GROUP_NAME", llstData["LOT_GROUP_NAME"].ToString());
                    //strSQL += string.Format(" and LOT_GROUP_NAME=:LOT_GROUP_NAME ");

                    string[] slNames = llstData["LOT_GROUP_NAME"].ToString().Split(',');
                    ArrayList alNames = ConvertStringarrayToArrayList(slNames);
                    string sName = MakeVariablesList(alNames);

                    strSQL += string.Format(" AND LOT_GROUP_NAME IN ({0}) ", sName);
                }

                ds = base.Query(strSQL, llstCondition);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }



        public DataSet getProducts()
        {
            DataSet ds = new DataSet();
            string strSQL = string.Empty;

            try
            {
                strSQL = string.Format(@" select PRODUCT_ID  
                                            from PRODUCT 
                                           order by PRODUCT_ID asc ");

                ds = base.Query(strSQL, null);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }


        #endregion

        public string MakeVariablesList(ArrayList alValues)
        {
            string sValueList = "";

            try
            {
                if (alValues.Count > 0)
                {

                    for (int i = 0; i < alValues.Count; i++)
                    {
                        if (i == 0)
                        {
                            sValueList = "'" + alValues[0].ToString() + "'";
                        }
                        else
                        {
                            sValueList = sValueList + ",'" + alValues[i].ToString() + "'";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return sValueList;
        }

        public ArrayList ConvertStringarrayToArrayList(string[] arString)
        {
            if (arString == null) return null;

            ArrayList al = new ArrayList();
            try
            {
                for (int i = 0; i < arString.Length; i++)
                {
                    al.Add(arString[i].ToString());
                }
            }
            catch (Exception ex)
            {
                return al;
            }
            return al;
        }

        #region Insert

        public DataSet InsertAnalysisChartConfig(byte[] baData)
        {
            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstData = new LinkedList();
            StringBuilder sb = new StringBuilder();
            decimal dRawID = 0;
            string sUserID = string.Empty;

            try
            {

                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.USER_ID] != null)
                {
                    sUserID = llstData[Definition.DynamicCondition_Condition_key.USER_ID].ToString();
                }

                if (llstData[Definition.CONDITION_KEY_DATA] != null)
                {
                    dt = (DataTable)llstData[Definition.CONDITION_KEY_DATA];
                }

                DataRow drDel = null;

                base.BeginTrans();

                foreach (DataRow dr in dt.Rows)
                {
                    llstFieldData.Clear();
                    sb = new StringBuilder();
                    switch (dr.RowState)
                    {
                        case DataRowState.Added:
                            dRawID = base.GetSequence(SEQUENCE.SEQ_ANALYSISCHAT_CONFIG_SPC);
                            llstFieldData.Add(COLUMN.RAWID, dRawID);
                            llstFieldData.Add(COLUMN.LOCATION_RAWID, dr[COLUMN.LOCATION_RAWID]);
                            llstFieldData.Add(COLUMN.USER_ID, dr[COLUMN.USER_ID]);
                            llstFieldData.Add(COLUMN.CHART_CODE, dr[COLUMN.CHART_CODE]);
                            llstFieldData.Add(COLUMN.CHART_NAME, dr[COLUMN.CHART_NAME]);
                            llstFieldData.Add(COLUMN.CHART_ALIAS, dr[COLUMN.CHART_ALIAS]);
                            llstFieldData.Add(COLUMN.CHART_LIST, dr[COLUMN.CHART_LIST]);
                            llstFieldData.Add(COLUMN.FROM_NOW, dr[COLUMN.FROM_NOW]);
                            llstFieldData.Add(COLUMN.PUBLIC_YN, dr[COLUMN.PUBLIC_YN]);
                            llstFieldData.Add(COLUMN.USE_YN, dr[COLUMN.USE_YN]);
                            llstFieldData.Add(COLUMN.SEARCH_YN, dr[COLUMN.SEARCH_YN]);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                            base.Insert(TABLE.ANALYSISCHAT_CONFIG_SPC, llstFieldData);
                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                            dr[COLUMN.RAWID] = dRawID;
                            break;
                        case DataRowState.Deleted:
                            drDel = dr;
                            drDel.RejectChanges();
                            sb.AppendFormat("Where RAWID ={0}", drDel[COLUMN.RAWID].ToString());
                            base.Delete(TABLE.ANALYSISCHAT_CONFIG_SPC, sb.ToString(), new LinkedList());
                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                            break;
                        case DataRowState.Modified:
                            dRawID = decimal.Parse(dr[COLUMN.RAWID].ToString());
                            llstFieldData.Add(COLUMN.LOCATION_RAWID, dr[COLUMN.LOCATION_RAWID]);
                            llstFieldData.Add(COLUMN.CHART_CODE, dr[COLUMN.CHART_CODE]);
                            llstFieldData.Add(COLUMN.CHART_NAME, dr[COLUMN.CHART_NAME]);
                            llstFieldData.Add(COLUMN.CHART_ALIAS, dr[COLUMN.CHART_ALIAS]);
                            llstFieldData.Add(COLUMN.CHART_LIST, dr[COLUMN.CHART_LIST]);
                            llstFieldData.Add(COLUMN.FROM_NOW, dr[COLUMN.FROM_NOW]);
                            llstFieldData.Add(COLUMN.PUBLIC_YN, dr[COLUMN.PUBLIC_YN]);
                            llstFieldData.Add(COLUMN.USE_YN, dr[COLUMN.USE_YN]);
                            llstFieldData.Add(COLUMN.SEARCH_YN, dr[COLUMN.SEARCH_YN]);
                            llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                            llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                            sb.AppendFormat("Where RAWID ={0}", dRawID);
                            base.Update(TABLE.ANALYSISCHAT_CONFIG_SPC, llstFieldData, sb.ToString(), new LinkedList());
                            if (base.ErrorMessage.Length > 0)
                            {
                                DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                                base.RollBack();
                                return dsResult;
                            }
                            break;
                    }
                }
                base.Commit();

                dt.AcceptChanges();
                dsResult.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (llstFieldData != null)
                    llstFieldData = null;
            }

            return dsResult;
        }


        public bool InsertAnalysisChartConfiguration(byte[] baData)
        {
            bool bSuccess = false;

            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstData = new LinkedList();
            StringBuilder sb = new StringBuilder();
            decimal dRawID = 0;
            string sUserID = string.Empty;

            try
            {

                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.USER_ID] != null)
                {
                    sUserID = llstData[Definition.DynamicCondition_Condition_key.USER_ID].ToString();
                }

                if (llstData[Definition.CONDITION_KEY_DATA] != null)
                {
                    dt = (DataTable)llstData[Definition.CONDITION_KEY_DATA];
                }

                DataRow drDel = null;

                base.BeginTrans();

                foreach (DataRow dr in dt.Rows)
                {
                    llstFieldData.Clear();
                    sb = new StringBuilder();
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        drDel = dr;
                        drDel.RejectChanges();
                        sb.AppendFormat("Where RAWID ={0}", drDel[COLUMN.RAWID].ToString());
                        base.Delete(TABLE.ANALYSISCHAT_CONFIG_SPC, sb.ToString(), new LinkedList());
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                    }
                    else if (dr.RowState == DataRowState.Added)
                    {
                        dRawID = base.GetSequence(SEQUENCE.SEQ_ANALYSISCHAT_CONFIG_SPC);
                        llstFieldData.Add(COLUMN.RAWID, dRawID);
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, dr[COLUMN.LOCATION_RAWID]);
                        llstFieldData.Add(COLUMN.USER_ID, dr[COLUMN.USER_ID]);
                        llstFieldData.Add(COLUMN.CHART_CODE, dr[COLUMN.CHART_CODE]);
                        llstFieldData.Add(COLUMN.CHART_NAME, dr[COLUMN.CHART_NAME]);
                        llstFieldData.Add(COLUMN.CHART_ALIAS, dr[COLUMN.CHART_ALIAS]);
                        llstFieldData.Add(COLUMN.CHART_LIST, dr[COLUMN.CHART_LIST]);
                        llstFieldData.Add(COLUMN.FROM_NOW, dr[COLUMN.FROM_NOW]);
                        llstFieldData.Add(COLUMN.PUBLIC_YN, dr[COLUMN.PUBLIC_YN]);
                        llstFieldData.Add(COLUMN.USE_YN, dr[COLUMN.USE_YN]);
                        llstFieldData.Add(COLUMN.SEARCH_YN, dr[COLUMN.SEARCH_YN]);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");

                        base.Insert(TABLE.ANALYSISCHAT_CONFIG_SPC, llstFieldData);
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                        dr[COLUMN.RAWID] = dRawID;
                    }
                    else if (dr.RowState == DataRowState.Modified || dr["_MODIFY"].ToString().ToUpper() == "TRUE")
                    {
                        dRawID = decimal.Parse(dr[COLUMN.RAWID].ToString());
                        llstFieldData.Add(COLUMN.LOCATION_RAWID, dr[COLUMN.LOCATION_RAWID]);
                        llstFieldData.Add(COLUMN.CHART_CODE, dr[COLUMN.CHART_CODE]);
                        llstFieldData.Add(COLUMN.CHART_NAME, dr[COLUMN.CHART_NAME]);
                        llstFieldData.Add(COLUMN.CHART_ALIAS, dr[COLUMN.CHART_ALIAS]);
                        llstFieldData.Add(COLUMN.CHART_LIST, dr[COLUMN.CHART_LIST]);
                        llstFieldData.Add(COLUMN.FROM_NOW, dr[COLUMN.FROM_NOW]);
                        llstFieldData.Add(COLUMN.PUBLIC_YN, dr[COLUMN.PUBLIC_YN]);
                        llstFieldData.Add(COLUMN.USE_YN, dr[COLUMN.USE_YN]);
                        llstFieldData.Add(COLUMN.SEARCH_YN, dr[COLUMN.SEARCH_YN]);
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                        sb.AppendFormat("Where RAWID ={0}", dRawID);
                        base.Update(TABLE.ANALYSISCHAT_CONFIG_SPC, llstFieldData, sb.ToString(), new LinkedList());
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                    }
                }
                base.Commit();
                bSuccess = true;
            }
            catch (Exception ex)
            {
                bSuccess = false;
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (llstFieldData != null)
                    llstFieldData = null;
            }

            return bSuccess;
        }


        public bool InsertSpecialLotGroup(byte[] baData)
        {
            bool bSuccess = false;

            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstData = new LinkedList();
            StringBuilder sb = new StringBuilder();
            string sUserID = string.Empty;

            try
            {

                llstData.SetSerialData(baData);

                if (llstData[Definition.DynamicCondition_Condition_key.USER_ID] != null)
                {
                    sUserID = llstData[Definition.DynamicCondition_Condition_key.USER_ID].ToString();
                }

                if (llstData[Definition.CONDITION_KEY_DATA] != null)
                {
                    dt = (DataTable)llstData[Definition.CONDITION_KEY_DATA];
                }

                DataRow drDel = null;

                base.BeginTrans();

                foreach (DataRow dr in dt.Rows)
                {
                    llstFieldData.Clear();
                    sb = new StringBuilder();

                    if (dr.RowState == DataRowState.Deleted || dr["_DELETE"].ToString().ToUpper() == "TRUE")
                    {
                        drDel = dr;
                        drDel.RejectChanges();
                        sb.AppendFormat("Where LOT_GROUP_NAME ='{0}'", drDel[COLUMN.LOT_GROUP_NAME].ToString());
                        sb.AppendFormat(" And USER_ID ='{0}'", sUserID);
                        base.Delete(TABLE.SPECIAL_LOT_SPC, sb.ToString(), new LinkedList());
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                    }
                    else if (dr["_INSERT"].ToString().ToUpper() == "TRUE")
                    {
                        llstFieldData.Add(COLUMN.LOT_ID, dr[COLUMN.LOT_ID]);
                        llstFieldData.Add(COLUMN.LOT_GROUP_NAME, dr[COLUMN.LOT_GROUP_NAME]);
                        llstFieldData.Add(COLUMN.USER_ID, sUserID);
                        llstFieldData.Add(COLUMN.CREATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.CREATE_DTTS), "");
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                        base.Insert(TABLE.SPECIAL_LOT_SPC, llstFieldData);
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                    }
                    else if (dr["_MODIFY"].ToString().ToUpper() == "TRUE")
                    {
                        llstFieldData.Add(COLUMN.LOT_ID, dr[COLUMN.LOT_ID]);
                        llstFieldData.Add(COLUMN.LOT_GROUP_NAME, dr[COLUMN.LOT_GROUP_NAME]);
                        llstFieldData.Add(COLUMN.USER_ID, sUserID);
                        llstFieldData.Add(COLUMN.LAST_UPDATE_BY, sUserID);
                        llstFieldData.Add(string.Format("{0}+SYSTIMESTAMP", COLUMN.LAST_UPDATE_DTTS), "");

                        sb.AppendFormat("Where LOT_GROUP_NAME ='{0}'", dr[COLUMN.LOT_GROUP_NAME].ToString());
                        sb.AppendFormat(" And USER_ID ='{0}'", sUserID);
                        base.Update(TABLE.SPECIAL_LOT_SPC, llstFieldData, sb.ToString(), new LinkedList());
                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            bSuccess = false;
                            return bSuccess;
                        }
                    }
                    else
                    {
                    }
                }
                base.Commit();
                bSuccess = true;
            }
            catch (Exception ex)
            {
                bSuccess = false;
                base.RollBack();
                DSUtil.SetResult(dsResult, 0, "", ex.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                if (llstFieldData != null)
                    llstFieldData = null;
            }

            return bSuccess;
        }

        #endregion

    }
}
