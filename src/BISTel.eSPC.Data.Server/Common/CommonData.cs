using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Common
{
    public class CommonData : DataBase
    {
        CommonUtility _ComUtil = new CommonUtility();
        
        public DataSet GetTableSchemaInfo(string sTableName)
        {
            DataSet dsReturn = new DataSet();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE, COLUMN_ID, DATA_DEFAULT ");
            sb.Append("  FROM USER_TAB_COLUMNS ");
            sb.Append(" WHERE TABLE_NAME = :TABLE_NAME ");
            sb.Append("  ORDER BY COLUMN_ID ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                llstCondition.Add("TABLE_NAME", sTableName);

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

        public DataSet QueryWeeklyStartEnd(byte[] baData)
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList llstData = new LinkedList();

                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);

                //llstCondition[Definition.CONDITION_KEY_CATEGORY].ToString()

                string sSQL = @"select sdt, edt from
                                (
                                SELECT MIN(dt) sdt, MAX(dt) edt, key_value
                                          FROM (SELECT TO_CHAR(TO_DATE('{0}','YYYYMMDD') 
                                                     + LEVEL, 'yyyy-mm-dd') dt
                                                     , TO_CHAR(TO_DATE('{0}','YYYYMMDD') 
                                                     + LEVEL, 'iw') key_value
                                                  FROM dual
                                                 CONNECT BY LEVEL <= TO_DATE('{1}','YYYYMMDD') 
                                                                   - TO_DATE('{0}','YYYYMMDD')
                                                )
                                         GROUP BY key_value
                                         )
                                         where key_value = '{2}' 
                            ";

                ds = base.Query(string.Format(sSQL, llstCondition["SDT"].ToString(), llstCondition["EDT"].ToString(), llstCondition["VAL"].ToString()), llstData);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        public DataSet QueryMonthStartEnd(byte[] baData)
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList llstData = new LinkedList();

                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);

                string sSQL = @"select sdt, edt from
                                (
                                SELECT MIN(dt) sdt, MAX(dt) edt, key_value
                                            FROM (SELECT TO_CHAR(TO_DATE('{0}','YYYYMMDD') 
                                                        + LEVEL, 'yyyy-mm-dd') dt
                                                        , TO_CHAR(TO_DATE('{0}','YYYYMMDD') 
                                                        + LEVEL, 'yyyy-mm') key_value
                                                    FROM dual
                                                    CONNECT BY LEVEL <= TO_DATE('{1}','YYYYMMDD') 
                                                                    - TO_DATE('{0}','YYYYMMDD')
                                                )
                                            GROUP BY key_value
                                            )
                                            where key_value = '{2}'  
                            ";

                ds = base.Query(string.Format(sSQL, llstCondition["SDT"].ToString(), llstCondition["EDT"].ToString(), llstCondition["VAL"].ToString()), llstData);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        public DataTable GetTableSchema(string sTableName)
        {
            DataTable dtDataTable = new DataTable(sTableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE, COLUMN_ID, DATA_DEFAULT ");
            sb.Append("  FROM USER_TAB_COLUMNS ");
            sb.Append(" WHERE TABLE_NAME = :TABLE_NAME ");
            sb.Append("  ORDER BY COLUMN_ID ");

            try
            {
                LinkedList llstCondition = new LinkedList();

                llstCondition.Add("TABLE_NAME", sTableName);

                DataSet dsSchema = base.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    return dtDataTable;
                }
                else
                {
                    DataTable dtSchema = dsSchema.Tables[0];

                    for (int i = 0; i < dtSchema.Rows.Count; i++)
                    {
                        dtDataTable.Columns.Add(dtSchema.Rows[i]["COLUMN_NAME"].ToString());

                        if (dtSchema.Rows[i]["DATA_DEFAULT"].ToString().Length > 0)
                            dtDataTable.Columns[i].DefaultValue = dtSchema.Rows[i]["DATA_DEFAULT"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dtDataTable;
        }


        #region CODE
        private const string SQL_QUERY_CODE_DATA =
       @"SELECT   * 
            FROM CODE_MST_PP
            WHERE CATEGORY = :CATEGORY ";

        public DataSet QueryCodeData(byte[] baData)
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList llstData = new LinkedList();

                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);

                string sSQL = SQL_QUERY_CODE_DATA;

                string sCategory = "";
                ArrayList alCodeList = new ArrayList();

                if (llstCondition[Definition.CONDITION_KEY_CATEGORY] != null)
                {
                    sCategory = llstCondition[Definition.CONDITION_KEY_CATEGORY].ToString();
                    llstData.Add("CATEGORY", sCategory);
                }

                if (llstCondition[Definition.CONDITION_KEY_CODE_LIST] != null)
                {
                    alCodeList = (ArrayList)llstCondition[Definition.CONDITION_KEY_CODE_LIST];

                    string sCodeList = this.MakeVariablesList(alCodeList);
                    sSQL = sSQL + string.Format("AND CODE IN ({0}) ", sCodeList);
                }

                if (llstCondition[Definition.CONDITION_KEY_USE_YN] != null)
                {
                    sSQL = sSQL + " AND USE_YN = :USE_YN ";
                    llstData.Add("USE_YN", llstCondition[Definition.CONDITION_KEY_USE_YN].ToString());
                }

                if (llstCondition[Definition.CONDITION_KEY_DEFAULT_COL] != null)
                {
                    sSQL = sSQL + " AND DEFAULT_COL = :DEFAULT_COL ";
                    llstData.Add("DEFAULT_COL", llstCondition[Definition.CONDITION_KEY_DEFAULT_COL].ToString());
                }

                sSQL = sSQL + "ORDER BY CODE_ORDER ";
                ds = base.Query(sSQL, llstData);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }



        public DataSet SaveCodeData(byte[] param)
        {
            DataSet dsResult = new DataSet();

            LinkedList llstParam = new LinkedList();
            llstParam.SetSerialData(param);

            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            DataSet dsSave = (DataSet)llstParam[Definition.CONDITION_KEY_DATASET];
            string userRawID = _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_USER_RAWID]);

            base.BeginTrans();

            if (dsSave.Tables.Contains("DELETE"))
            {
                try
                {
                    string sWhereQuery = "WHERE RAWID = :RAWID";

                    foreach (DataRow dr in dsSave.Tables["DELETE"].Rows)
                    {
                        llstWhereData.Clear();
                        llstWhereData.Add("RAWID", dr["RAWID"].ToString());

                        base.Delete(TABLE.CODE_MST_PP, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base.RollBack();
                    DSUtil.SetResult(dsResult, 0, "", ex.Message);
                }
            }

            if (dsSave.Tables.Contains("UPDATE"))
            {
                try
                {
                    string sWhereType = string.Empty;

                    string sWhereQuery = string.Empty;

                    if (dsSave.Tables["UPDATE"].Columns.Contains("RAWID"))
                    {
                        sWhereType = "RAWID";
                        sWhereQuery = "WHERE RAWID = :RAWID";
                    }
                    else if (dsSave.Tables["UPDATE"].Columns.Contains("CATEGORY") && dsSave.Tables["UPDATE"].Columns.Contains("CODE"))
                    {
                        sWhereType = "CATEGORYCODE";
                        sWhereQuery = "WHERE CATEGORY = :CATEGORY AND CODE = :CODE";
                    }

                    foreach (DataRow dr in dsSave.Tables["UPDATE"].Rows)
                    {
                        llstFieldData.Clear();

                        if (dsSave.Tables["UPDATE"].Columns.Contains("CATEGORY")) llstFieldData.Add("CATEGORY", dr["CATEGORY"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("CODE")) llstFieldData.Add("CODE", dr["CODE"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("NAME")) llstFieldData.Add("NAME", dr["NAME"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("USE_YN")) llstFieldData.Add("USE_YN", dr["USE_YN"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("DESCRIPTION")) llstFieldData.Add("DESCRIPTION", dr["DESCRIPTION"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("CODE_ORDER")) llstFieldData.Add("CODE_ORDER", dr["CODE_ORDER"].ToString());
                        if (dsSave.Tables["UPDATE"].Columns.Contains("DEFAULT_COL")) llstFieldData.Add("DEFAULT_COL", dr["DEFAULT_COL"].ToString());

                        llstFieldData.Add("LAST_UPDATE_DTTS+SYSTIMESTAMP", "");
                        llstFieldData.Add("LAST_UPDATE_BY", userRawID);

                        llstWhereData.Clear();

                        if (sWhereType.Equals("RAWID"))
                            llstWhereData.Add("RAWID", dr["RAWID"].ToString());
                        else if (sWhereType.Equals("CATEGORYCODE"))
                        {
                            llstWhereData.Add("CATEGORY", dr["CATEGORY"].ToString());
                            llstWhereData.Add("CODE", dr["CODE"].ToString());
                        }

                        base.Update(TABLE.CODE_MST_PP, llstFieldData, sWhereQuery, llstWhereData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base.RollBack();
                    DSUtil.SetResult(dsResult, 0, "", ex.Message);
                }
            }

            if (dsSave.Tables.Contains("INSERT"))
            {
                try
                {
                    foreach (DataRow dr in dsSave.Tables["INSERT"].Rows)
                    {
                        llstFieldData.Clear();
                        llstFieldData.Add("RAWID+SEQ_CODE_MST_PP.NEXTVAL", "");

                        if (dsSave.Tables["INSERT"].Columns.Contains("CATEGORY")) llstFieldData.Add("CATEGORY", dr["CATEGORY"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("CODE")) llstFieldData.Add("CODE", dr["CODE"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("NAME")) llstFieldData.Add("NAME", dr["NAME"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("USE_YN")) llstFieldData.Add("USE_YN", dr["USE_YN"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("DESCRIPTION")) llstFieldData.Add("DESCRIPTION", dr["DESCRIPTION"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("CODE_ORDER")) llstFieldData.Add("CODE_ORDER", dr["CODE_ORDER"].ToString());
                        if (dsSave.Tables["INSERT"].Columns.Contains("DEFAULT_COL")) llstFieldData.Add("DEFAULT_COL", dr["DEFAULT_COL"].ToString());

                        llstFieldData.Add("CREATE_BY", userRawID);
                        llstFieldData.Add("CREATE_DTTS+SYSTIMESTAMP", "");

                        base.Insert(TABLE.CODE_MST_PP, llstFieldData);

                        if (base.ErrorMessage.Length > 0)
                        {
                            DSUtil.SetResult(dsResult, 0, "", base.ErrorMessage);
                            base.RollBack();
                            return dsResult;
                        }
                    }
                }
                catch (Exception ex)
                {
                    base.RollBack();
                    DSUtil.SetResult(dsResult, 0, "", ex.Message);
                }
            }

            base.Commit();

            return dsResult;
        }

        #endregion

        #region : DEFAULT SETTING
        private const string SQL_QUERY_DEAULT_SETTING_VALUE =
       @"SELECT   * 
            FROM SPEC_DEFAULT_OPT_MST_FDC ";




        public DataSet QueryDefaultSettingData()
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList lklist = new LinkedList();

                ds = base.Query(SQL_QUERY_DEAULT_SETTING_VALUE, lklist);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        private const string SQL_QUERY_SPEC_SETTING_VALUE_ALL =
   @"SELECT OPTION_NAME_CD, OPTION_VALUE FROM SPEC_OPT_MST_FDC
            WHERE SPEC_RAWID = :SPEC_RAWID
            AND DATA_CATEGORY_CD = :DATA_CATEGORY_CD
            AND SPEC_TYPE_CD = :SPEC_TYPE_CD
            UNION 
            SELECT OPTION_NAME_CD, OPTION_VALUE 
            FROM SPEC_DEFAULT_OPT_MST_FDC
            WHERE OPTION_NAME_CD NOT IN 
            (
            SELECT OPTION_NAME_CD
            FROM SPEC_OPT_MST_FDC
            WHERE SPEC_RAWID = :SPEC_RAWID
            AND DATA_CATEGORY_CD = :DATA_CATEGORY_CD
            AND SPEC_TYPE_CD = :SPEC_TYPE_CD
            ) ";

        private const string SQL_QUERY_SPEC_SETTING_VALUE =
      @"SELECT   RAWID, SPEC_RAWID, DATA_CATEGORY_CD, SPEC_TYPE_CD, OPTION_NAME_CD, OPTION_VALUE, 
            OPTION_DATA_TYPE_CD, CREATE_DTTS, CREATE_BY 
            FROM SPEC_OPT_MST_FDC
            WHERE SPEC_RAWID = :SPEC_RAWID
            AND DATA_CATEGORY_CD = :DATA_CATEGORY_CD 
            AND SPEC_TYPE_CD = :SPEC_TYPE_CD ";

        public DataSet QuerySPECSettingData(byte[] baData)
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sSpecRawID = "";
                string sDataCategoryCD = "";
                string sSpecTypeCD = "";
                string sType = "";

                if (llstData[Definition.CONDITION_KEY_TYPE] != null)
                    sType = llstData[Definition.CONDITION_KEY_TYPE].ToString();

                if (llstData[Definition.CONDITION_KEY_SPEC_RAWID] != null)
                    sSpecRawID = llstData[Definition.CONDITION_KEY_SPEC_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD] != null)
                    sDataCategoryCD = llstData[Definition.CONDITION_KEY_DATA_CATEGORY_CD].ToString();

                if (llstData[Definition.CONDITION_KEY_SPEC_TYPE_CD] != null)
                    sSpecTypeCD = llstData[Definition.CONDITION_KEY_SPEC_TYPE_CD].ToString();

                LinkedList llstCondition = new LinkedList();

                llstCondition.Add("SPEC_RAWID", sSpecRawID);
                llstCondition.Add("DATA_CATEGORY_CD", sDataCategoryCD);
                llstCondition.Add("SPEC_TYPE_CD", sSpecTypeCD);

                if (sType.Equals(Definition.VARIABLE_ALL))
                    ds = base.Query(SQL_QUERY_SPEC_SETTING_VALUE_ALL, llstCondition);
                else
                    ds = base.Query(SQL_QUERY_SPEC_SETTING_VALUE, llstCondition);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }


        //public bool UpdateDefaultSettingData(byte[] baData)
        //{
        //    bool bSuccess = false;
        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(baData);

        //        SortedList slUpdateData = new SortedList();
        //        string sUserRawID = "";
        //        string sDataType = "";

        //        if (llstData[Definition.CONDITION_KEY_SORTED_LIST] != null)
        //            slUpdateData = (SortedList)llstData[Definition.CONDITION_KEY_SORTED_LIST];

        //        if (llstData[Definition.CONDITION_KEY_USER_RAWID] != null)
        //            sUserRawID = llstData[Definition.CONDITION_KEY_USER_RAWID].ToString();

        //        if (llstData[Definition.CONDITION_KEY_DATA_TYPE] != null)
        //            sDataType = llstData[Definition.CONDITION_KEY_DATA_TYPE].ToString();

        //        base.BeginTrans();

        //        int iSuccessCount = 0;
        //        LinkedList lnklstWhere = new LinkedList();
        //        LinkedList lnklstData = new LinkedList();

        //        DateTime dt = base.GetDBTimeStamp();


        //        for (int i = 0; i < slUpdateData.Count; i++)
        //        {
        //            lnklstWhere.Clear();
        //            lnklstData.Clear();

        //            string sKey = slUpdateData.GetKey(i).ToString();
        //            string sValue = slUpdateData[sKey].ToString();

        //            lnklstWhere.Add("OPTION_NAME_CD", sKey);
        //            lnklstData.Add("OPTION_VALUE", sValue);
        //            lnklstData.Add("LAST_UPDATE_DTTS", dt);
        //            lnklstData.Add("LAST_UPDATE_BY", sUserRawID);

        //            int iSuccess = 0;
        //            if (sDataType == "DEFAULT")
        //            {

        //                iSuccess = base.Update(_TABLE_SPEC_DEFAULT_OPT_MST_FDC, lnklstData, "WHERE OPTION_NAME_CD = :OPTION_NAME_CD", lnklstWhere);
        //            }
        //            else
        //            {
        //                //parameter, Step 별 다른 Option value.
        //                iSuccess = base.Update(_TABLE_SPEC_DEFAULT_OPT_MST_FDC, lnklstData, "WHERE OPTION_NAME_CD = :OPTION_NAME_CD", lnklstWhere);
        //            }

        //            if (iSuccess > 0)
        //                iSuccessCount++;
        //        }

        //        if (iSuccessCount == slUpdateData.Count)
        //        {
        //            bSuccess = true;
        //            base.Commit();
        //        }
        //        else
        //            base.RollBack();
        //    }
        //    catch (Exception ex)
        //    {
        //        base.RollBack();
        //        return bSuccess;
        //    }

        //    return bSuccess;
        //}

        //public bool UpdateSPECSettingData(ArrayList alRawID, ArrayList alValue, ArrayList alDateTime, ArrayList alUserRawID)
        //{
        //    bool bSuccess = false;
        //    try
        //    {
        //        base.BeginTrans();

        //        int iSuccessCount = 0;

        //        LinkedList llstWhere = new LinkedList();
        //        LinkedList llstData = new LinkedList();

        //        for (int i = 0; i < alRawID.Count; i++)
        //        {
        //            llstWhere.Clear();
        //            llstData.Clear();

        //            string sRawID = alRawID[i].ToString();
        //            string sValue = alValue[i].ToString();
        //            DateTime dt = (DateTime)alDateTime[i];
        //            string sUserRawID = alUserRawID[i].ToString();

        //            llstWhere.Add("RAWID", Convert.ToInt32(sRawID));
        //            llstData.Add("OPTION_VALUE", sValue);
        //            llstData.Add("LAST_UPDATE_DTTS", dt);
        //            llstData.Add("LAST_UPDATE_BY", Convert.ToInt32(sUserRawID));

        //            int iSuccess = base.Update(_TABLE_SPEC_OPT_MST_FDC, llstData, "WHERE RAWID = :RAWID", llstWhere);

        //            if (iSuccess > 0)
        //                iSuccessCount++;
        //        }

        //        if (iSuccessCount != alRawID.Count)
        //        {
        //            bSuccess = false;
        //            base.RollBack();
        //        }
        //        else
        //        {
        //            bSuccess = true;
        //            base.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        base.RollBack();
        //        return bSuccess;
        //    }

        //    return bSuccess;
        //}

        //public bool UpdateSPECSettingData(byte[] baData)
        //{
        //    bool bSuccess = false;
        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(baData);

        //        SortedList slUpdateData = new SortedList();
        //        string sUserRawID = "";
        //        string sDataType = "";

        //        if (llstData[Definition.CONDITION_KEY_SORTED_LIST] != null)
        //            slUpdateData = (SortedList)llstData[Definition.CONDITION_KEY_SORTED_LIST];

        //        if (llstData[Definition.CONDITION_KEY_USER_RAWID] != null)
        //            sUserRawID = llstData[Definition.CONDITION_KEY_USER_RAWID].ToString();

        //        if (llstData[Definition.CONDITION_KEY_DATA_TYPE] != null)
        //            sDataType = llstData[Definition.CONDITION_KEY_DATA_TYPE].ToString();

        //        base.BeginTrans();

        //        int iSuccessCount = 0;
        //        LinkedList lnklstWhere = new LinkedList();
        //        LinkedList lnklstData = new LinkedList();

        //        DateTime dt = base.GetDBTimeStamp();


        //        for (int i = 0; i < slUpdateData.Count; i++)
        //        {
        //            lnklstWhere.Clear();
        //            lnklstData.Clear();

        //            string sKey = slUpdateData.GetKey(i).ToString();
        //            string sValue = slUpdateData[sKey].ToString();

        //            lnklstWhere.Add("OPTION_NAME_CD", sKey);
        //            lnklstData.Add("OPTION_VALUE", sValue);
        //            lnklstData.Add("LAST_UPDATE_DTTS", dt);
        //            lnklstData.Add("LAST_UPDATE_BY", sUserRawID);

        //            int iSuccess = 0;

        //            //parameter, Step 별 다른 Option value.
        //            iSuccess = base.Update(_TABLE_SPEC_DEFAULT_OPT_MST_FDC, lnklstData, "WHERE OPTION_NAME_CD = :OPTION_NAME_CD", lnklstWhere);


        //            if (iSuccess > 0)
        //                iSuccessCount++;
        //        }

        //        if (iSuccessCount == slUpdateData.Count)
        //        {
        //            bSuccess = true;
        //            base.Commit();
        //        }
        //        else
        //            base.RollBack();
        //    }
        //    catch (Exception ex)
        //    {
        //        base.RollBack();
        //        return bSuccess;
        //    }

        //    return bSuccess;
        //}

        public bool InsertSPECSettingData(byte[] baData)
        {
            bool bSuccess = false;
            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                SortedList slUpdateData = new SortedList();
                string sUserRawID = "";
                string sDataType = "";

                if (llstData[Definition.CONDITION_KEY_SORTED_LIST] != null)
                    slUpdateData = (SortedList)llstData[Definition.CONDITION_KEY_SORTED_LIST];

                if (llstData[Definition.CONDITION_KEY_USER_RAWID] != null)
                    sUserRawID = llstData[Definition.CONDITION_KEY_USER_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_DATA_TYPE] != null)
                    sDataType = llstData[Definition.CONDITION_KEY_DATA_TYPE].ToString();

                base.BeginTrans();


            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;
        }

        #endregion

        #region : AUTO CALCULATION
        private const string SQL_QUERY_AUTO_CALC_VALUE =
      @"SELECT   * 
            FROM SPEC_DEFAULT_OPT_MST_FDC ";

        public DataSet QueryAutoCalculationData()
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList lklist = new LinkedList();

                ds = base.Query(SQL_QUERY_AUTO_CALC_VALUE, lklist);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        #endregion

        #region : DATA


        public bool InsertData(SortedList slData, string sTableName)
        {
            bool bSuccess = false;
            int iSuccess = 0;
            try
            {
                if (slData.Count > 0)
                {
                    LinkedList lnklst = new LinkedList();

                    for (int i = 0; i < slData.Count; i++)
                    {
                        string sFieldName = slData.GetKey(i).ToString();
                        string sData = slData.GetByIndex(i).ToString();

                        if (sFieldName == "CREATE_DTTS")
                        {
                            if (sData != "")
                            {
                                DateTime dtData = Convert.ToDateTime(sData);
                                lnklst.Add(sFieldName, dtData);
                            }
                        }
                        else if (sFieldName == "LAST_UPDATE_DTTS")
                        {
                            if (sData != "")
                            {
                                DateTime dtData = Convert.ToDateTime(sData);
                                lnklst.Add(sFieldName, dtData);
                            }
                        }
                        else if (sFieldName == "RAWID")
                        {
                            Int32 objData = Convert.ToInt32(sData);
                            lnklst.Add(sFieldName, objData);
                        }
                        else
                        {
                            lnklst.Add(sFieldName, sData);
                        }
                    }

                    base.BeginTrans();
                    iSuccess = base.Insert(sTableName, lnklst);

                    if (iSuccess > 0)
                    {
                        bSuccess = true;
                        base.Commit();
                    }
                    else
                        base.RollBack();
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;
        }

        public bool DeleteData(string sTABLE_NAME, string sWhereField, ArrayList alData)
        {
            bool bSuccess = false;
            int iSuccess = 0;
            try
            {
                base.BeginTrans();
                LinkedList llstData = new LinkedList();

                for (int i = 0; i < alData.Count; i++)
                {
                    llstData.Clear();
                    string sValue = alData[i].ToString();
                    llstData.Add(sWhereField, sValue);
                    Decimal dSuccessDelete = base.Delete(sTABLE_NAME, "WHERE " + sWhereField + " = :" + sWhereField, llstData);

                    if (dSuccessDelete > 0)
                        iSuccess++;
                }

                if (iSuccess == alData.Count)
                {
                    bSuccess = true;
                    base.Commit();
                }
                else
                    base.RollBack();
            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;

        }

        public bool DeleteData(string sTABLE_NAME, string sWhereField, LinkedList llstData)
        {
            bool bSuccess = false;
            try
            {
                base.BeginTrans();

                Decimal dSuccessDelete = base.Delete(sTABLE_NAME, sWhereField, llstData);

                if (dSuccessDelete > 0)
                    bSuccess = true;


                if (bSuccess)
                {
                    base.Commit();
                }
                else
                    base.RollBack();
            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;

        }

        public bool UpdateData(string sTABLE_NAME, LinkedList llstData, LinkedList llstWhere, string sWhereQuery)
        {
            bool bSuccess = false;
            try
            {
                base.BeginTrans();

                decimal dSuccess = base.Update(sTABLE_NAME, llstData, sWhereQuery, llstWhere);

                if (dSuccess > 0)
                {
                    bSuccess = true;
                    base.Commit();
                }
                else
                    base.RollBack();
            }
            catch (Exception ex)
            {
                base.RollBack();
            }

            return bSuccess;
        }

        public bool InsertBatchData_Single(string sTABLE_NAME, SortedList slData)
        {
            bool bSuccess = false;
            int iSuccess = 0;
            try
            {
                if (slData.Count > 0)
                {
                    LinkedList lnklst = new LinkedList();

                    ArrayList alInsertData = (ArrayList)slData.GetByIndex(0);

                    int iSuccessCount = 0;

                    base.BeginTrans();

                    for (int iItem = 0; iItem < alInsertData.Count; iItem++)
                    {
                        lnklst.Clear();
                        for (int i = 0; i < slData.Count; i++)
                        {
                            string sFieldName = slData.GetKey(i).ToString();
                            ArrayList alData = (ArrayList)slData.GetByIndex(i);

                            if (sFieldName == "CREATE_DTTS")
                            {
                                DateTime[] objData = (DateTime[])alData.ToArray(typeof(DateTime));
                                lnklst.Add(sFieldName, objData[iItem]);
                            }
                            else if (sFieldName == "LAST_UPDATE_DTTS")
                            {
                                DateTime[] objData = (DateTime[])alData.ToArray(typeof(DateTime));
                                lnklst.Add(sFieldName, objData[iItem]);
                            }
                            else if (sFieldName == "RAWID")
                            {
                                string sItem = "";
                                try
                                {
                                    Int32[] objData = (Int32[])alData.ToArray(typeof(Int32));
                                    sItem = objData[iItem].ToString();
                                }
                                catch
                                {
                                    string[] objData = (string[])alData.ToArray(typeof(string));
                                    sItem = objData[iItem];
                                }

                                if (sItem.Length > 0)
                                    lnklst.Add(sFieldName, Convert.ToInt32(sItem));
                            }
                            //else if (sFieldName == "DESCRIPTION")
                            //{
                            //    string[] objData = (string[])alData.ToArray(typeof(string));
                            //    string sItem = objData[iItem];
                            //}
                            else
                            {
                                //string sItem = "";

                                //try
                                //{
                                //    Int32[] objData = (Int32[])alData.ToArray(typeof(Int32));
                                //    sItem = objData[iItem].ToString();

                                //    string[] objData = (string[])alData.ToArray(typeof(string));
                                //    sItem = objData[iItem];
                                //}
                                //catch
                                //{

                                //}

                                //if(sItem.Length > 0)
                                //    lnklst.Add(sFieldName, sItem);

                                try
                                {
                                    double[] objData = (double[])alData.ToArray(typeof(double));
                                    double dItem = Convert.ToDouble(objData[iItem]);
                                    if (double.IsNaN(dItem))
                                    {
                                        lnklst.Add(sFieldName, "");
                                    }
                                    else
                                        lnklst.Add(sFieldName, dItem);
                                }
                                catch
                                {
                                    try
                                    {
                                        string[] objData = (string[])alData.ToArray(typeof(string));
                                        string sItem = objData[iItem];
                                        lnklst.Add(sFieldName, sItem);
                                    }
                                    catch
                                    {
                                        Int32[] objData = (Int32[])alData.ToArray(typeof(Int32));
                                        Int32 dItem = Convert.ToInt32(objData[iItem]);
                                        lnklst.Add(sFieldName, dItem);
                                    }
                                }
                            }
                        }

                        iSuccess = base.Insert(sTABLE_NAME, lnklst);

                        if (iSuccess > 0)
                            iSuccessCount++;
                    }

                    if (iSuccessCount == alInsertData.Count)
                    {
                        bSuccess = true;
                        base.Commit();
                    }
                    else
                        base.RollBack();
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                return bSuccess;
            }

            return bSuccess;
        }

        public bool InsertBatchData(string sTABLE_NAME, SortedList slData)
        {
            bool bSuccess = false;
            int iSuccess = 0;
            try
            {
                if (slData.Count > 0)
                {
                    LinkedList lnklst = new LinkedList();

                    for (int i = 0; i < slData.Count; i++)
                    {
                        string sFieldName = slData.GetKey(i).ToString();
                        ArrayList alData = (ArrayList)slData.GetByIndex(i);

                        if (sFieldName == "CREATE_DTTS")
                        {
                            DateTime[] objData = (DateTime[])alData.ToArray(typeof(DateTime));
                            lnklst.Add(sFieldName, objData);
                        }
                        else if (sFieldName == "LAST_UPDATE_DTTS")
                        {
                            DateTime[] objData = (DateTime[])alData.ToArray(typeof(DateTime));
                            lnklst.Add(sFieldName, objData);
                        }
                        //else if (sFieldName == "USER_RAWID")
                        //{
                        //    string[] objData = (string[])alData.ToArray(typeof(string));
                        //    lnklst.Add(sFieldName, objData);
                        //}
                        else if (sFieldName == "RAWID")
                        {
                            Int32[] objData = (Int32[])alData.ToArray(typeof(Int32));
                            lnklst.Add(sFieldName, objData);
                        }
                        else
                        {
                            //string[] objData = (string[])alData.ToArray(typeof(string));
                            //lnklst.Add(sFieldName, objData);

                            try
                            {
                                Int32[] objData = (Int32[])alData.ToArray(typeof(Int32));
                                lnklst.Add(sFieldName, objData);

                            }
                            catch
                            {
                                string[] objData = (string[])alData.ToArray(typeof(string));
                                lnklst.Add(sFieldName, objData);
                            }


                        }
                    }
                    base.BeginTrans();
                    iSuccess = base.InsertBatch(sTABLE_NAME, lnklst);

                    if (iSuccess > 0)
                    {
                        bSuccess = true;
                        base.Commit();
                    }
                    else
                        base.RollBack();
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


        #region EQP_MST_PP

        public DataSet QueryEQPData(byte[] baData)
        {
            DataSet ds = new DataSet();
            try
            {
                LinkedList llstData = new LinkedList();

                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);

                string sSQL = @"select * 
                            from EQP_MST_PP
                            where 1=1
                            ";

                ArrayList alCodeList = new ArrayList();
                if (llstCondition[Definition.DynamicCondition_Condition_key.EQP_ID] != null)
                {
                    sSQL += string.Format("AND EQP_ID='{0}' ", llstCondition[Definition.DynamicCondition_Condition_key.EQP_ID].ToString());
                }

                sSQL += "ORDER BY EQP_ID ";
                ds = base.Query(sSQL, llstData);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
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

        public string MakeVariablesList_Null(ArrayList alValues)
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
                            if (alValues[i].ToString().Trim() != "")
                                sValueList = "'" + alValues[0].ToString() + "'";
                            else
                                sValueList = "'" + "-1" + "'";
                        }
                        else
                        {
                            if (alValues[i].ToString().Trim() != "")
                                sValueList = sValueList + ",'" + alValues[i].ToString() + "'";
                            else
                                sValueList = "'" + "-1" + "'";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return sValueList;
        }

        public void GetPreConnection()
        {
            this.Query("select 1 from dual");
        }

        /// <summary>
        /// Update version column's value
        /// </summary>
        /// <param name="rawid">rawid of model</param>
        /// <param name="exceptTables">excepted tables, already updated</param>
        /// <returns>error message if error occurs</returns>
        //public string UpdateVersion(string rawid, string[] exceptTables)
        //{
        //    this.Open();

        //    List<string> except = new List<string>(exceptTables);

        //    string errorMessage = string.Empty;

        //    LinkedList fieldData = new LinkedList(COLUMN.VERSION+ "+version+1", "");//    fieldData.Add(COLUMN.RAWID, rawid);
        //    string whereQuery = "WHERE RAWID = :RAWID";
        //    LinkedList whereData = new LinkedList(COLUMN.RAWID, rawid);
        //    if(!except.Contains(TABLE.MODEL_CONFIG_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.MODEL_CONFIG_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    string version =
        //        this.Query("SELECT VERSION FROM " + TABLE.MODEL_CONFIG_MST_SPC + " where " + COLUMN.RAWID + " = " + rawid).Tables[0].Rows[0][
        //            COLUMN.VERSION].ToString();

        //    fieldData = new LinkedList(COLUMN.VERSION + "+" + version, "");
        //    fieldData.Add(COLUMN.MODEL_CONFIG_RAWID, rawid);
        //    whereQuery = "WHERE MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID";
        //    whereData.Clear();
        //    whereData.Add(COLUMN.MODEL_CONFIG_RAWID, rawid);
        //    if(!except.Contains(TABLE.MODEL_CONFIG_OPT_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.MODEL_CONFIG_OPT_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    if(!except.Contains(TABLE.MODEL_CONTEXT_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.MODEL_CONTEXT_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    if(!except.Contains(TABLE.MODEL_AUTOCALC_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.MODEL_AUTOCALC_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    if(!except.Contains(TABLE.MODEL_RULE_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.RULE_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    whereQuery = "WHERE MODEL_RULE_RAWID in (select rawid from " + TABLE.MODEL_RULE_MST_SPC + 
        //        " where MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID)";
        //    if(!except.Contains(TABLE.MODEL_RULE_OPT_MST_SPC))
        //    {
        //        UpdateVersionOfTable(TABLE.RULE_OPT_MST_SPC, fieldData, whereQuery, whereData, ref errorMessage);
        //    }

        //    this.Close();

        //    return errorMessage;
        //}


        public string[] GetIncreaseVersionQuery(string rawid)
        {
            List<string> resultQuery = new List<string>();
            string version =
                this.Query("SELECT " + COLUMN.VERSION + " FROM " + TABLE.MODEL_CONFIG_MST_SPC + " where " + COLUMN.RAWID + " = " + rawid).Tables[0].Rows[0][
                    COLUMN.VERSION].ToString();

            if(string.IsNullOrEmpty(version))
            {
                version = "0";
            }
            else
            {
                version = (int.Parse(version) + 1).ToString();
            }

            resultQuery.Add("UPDATE " + TABLE.MODEL_CONFIG_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " + COLUMN.RAWID + " = " +
                            rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_CONFIG_OPT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_CONTEXT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_AUTOCALC_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_RULE_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_RULE_OPT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " + 
                COLUMN.MODEL_RULE_RAWID + " in (select rawid from " + TABLE.MODEL_RULE_MST_SPC + " where MODEL_CONFIG_RAWID = " + rawid + ")");
            
            return resultQuery.ToArray();
        }

        public void MakeQueryForConfigMst(ref bool isEmpty, StringBuilder sbQuery, string tableName, string useYN, string columnName, string configRawIDColumnName, string sourceConfigRawID)
        {
            if (useYN.Equals("Y"))
            {
                if (!isEmpty)
                {
                    sbQuery.Append(", ");
                }

                sbQuery.AppendLine(columnName + " = ( SELECT " + columnName + " FROM " + tableName + " WHERE " + configRawIDColumnName + " = '" + sourceConfigRawID + "' ) ");
                isEmpty = false;
            }
        }

        public void MakeQueryForConfigMst(ref bool isEmpty, StringBuilder sbQuery, string tableName, string useYN, string columnNameSource, string columnNameTarget, string configRawIDColumnName, string sourceConfigRawID)
        {
            if (useYN.Equals("Y"))
            {
                if (!isEmpty)
                {
                    sbQuery.Append(", ");
                }

                sbQuery.AppendLine(columnNameTarget + " = ( SELECT " + columnNameSource + " FROM " + tableName + " WHERE " + configRawIDColumnName + " = '" + sourceConfigRawID + "' ) ");
                isEmpty = false;
            }
        }

        public void MakeQueryForConfigMstSpec(ref bool isEmpty, StringBuilder sbQuery, string useYN, string upperColumn, string lowerColumn, string centerColumn, string sourceConfigRawID)
        {
            if (useYN.Equals("Y"))
            {
                if (!isEmpty)
                {
                    sbQuery.Append(", ");
                }

                sbQuery.AppendLine(upperColumn + " = ( SELECT " + upperColumn + " FROM MODEL_CONFIG_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ), ");
                sbQuery.AppendLine(lowerColumn + " = ( SELECT " + lowerColumn + " FROM MODEL_CONFIG_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ), ");
                sbQuery.AppendLine(centerColumn + " = ( SELECT " + centerColumn + " FROM MODEL_CONFIG_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ) ");

                isEmpty = false;
            }
        }

        public void MakeQueryForATTConfigMstSpec(ref bool isEmpty, StringBuilder sbQuery, string useYN, string upperColumn, string lowerColumn, string centerColumn, string sourceConfigRawID)
        {
            if (useYN.Equals("Y"))
            {
                if (!isEmpty)
                {
                    sbQuery.Append(", ");
                }

                sbQuery.AppendLine(upperColumn + " = ( SELECT " + upperColumn + " FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ), ");
                sbQuery.AppendLine(lowerColumn + " = ( SELECT " + lowerColumn + " FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ), ");
                sbQuery.AppendLine(centerColumn + " = ( SELECT " + centerColumn + " FROM MODEL_CONFIG_ATT_MST_SPC WHERE RAWID = '" + sourceConfigRawID + "' ) ");

                isEmpty = false;
            }
        }

        public string[] GetIncreaseATTVersionQuery(string rawid)
        {
            List<string> resultQuery = new List<string>();
            string version =
                this.Query("SELECT " + COLUMN.VERSION + " FROM " + TABLE.MODEL_CONFIG_ATT_MST_SPC + " where " + COLUMN.RAWID + " = " + rawid).Tables[0].Rows[0][
                    COLUMN.VERSION].ToString();

            if (string.IsNullOrEmpty(version))
            {
                version = "0";
            }
            else
            {
                version = (int.Parse(version) + 1).ToString();
            }

            resultQuery.Add("UPDATE " + TABLE.MODEL_CONFIG_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " + COLUMN.RAWID + " = " +
                            rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_CONTEXT_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_AUTOCALC_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_RULE_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                            COLUMN.MODEL_CONFIG_RAWID + " = " + rawid);
            resultQuery.Add("UPDATE " + TABLE.MODEL_RULE_OPT_ATT_MST_SPC + " SET " + COLUMN.VERSION + " = " + version + " where " +
                COLUMN.MODEL_RULE_RAWID + " in (select rawid from " + TABLE.MODEL_RULE_ATT_MST_SPC + " where MODEL_CONFIG_RAWID = " + rawid + ")");

            return resultQuery.ToArray();
        }

        //SPC-678
        public DataSet GetLastTRXDataDTTs(string modelConfigRawid)
        {
            DataSet dsResult = new DataSet();
            string strSQL = string.Empty;
            try
            {
                strSQL = string.Format(@"select max(spc_end_dtts) + 1/24 as LAST_DTTS from data_trx_spc
                                    where model_config_rawid = {0} ", modelConfigRawid);

                dsResult = base.Query(strSQL);
            }
            catch
            {
            }


            return dsResult;
        }

        public DataSet GetAutoCalculationOption(string category)
        {
            DataSet dsResult = new DataSet();
            string strQuery = string.Empty;

            try
            {
                strQuery = string.Format(@"SELECT CODE, NAME FROM CODE_MST_PP WHERE CATEGORY = '{0}' AND USE_YN ='Y' ORDER BY CODE_ORDER ASC", category);
                dsResult = base.Query(strQuery);
            }
            catch(Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }

        public DataSet GetContextTypeData()
        {
            DataSet dsResult = new DataSet();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("SELECT * FROM CODE_MST_PP ");
                sb.Append(" WHERE CATEGORY = 'CONTEXT_TYPE' ");
                sb.Append(" AND USE_YN = 'Y' ");

                dsResult = base.Query(sb.ToString());
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsResult;
        }
    }
}
