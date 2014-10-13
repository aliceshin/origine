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
    public class ConditionData : DataBase
    {
        Common.CommonData _commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

        public ConditionData()
        {
            this._sbQuery = new StringBuilder(4096);
        }

        public DataSet getTimeData()
        {
            DataSet ds;
            try
            {
                var query = @"SELECT * FROM CTX_TIME_MST_PP WHERE use_yn = 'Y' ORDER BY RAWID";

                ds = base.Query(query);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(ds, 0, "", base.ErrorMessage);
                }
                DSUtil.SetResult(ds, 1, "", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }


        #region :Site Condition

        public DataSet GetSite(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append("select  DISTINCT SITE from EQP_VW_SPC order by SITE");
                ds = base.Query(_sbQuery.ToString(), new LinkedList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetSiteForCondition()
        {
            DataSet ds = null;
            try
            {
                string query = "SELECT distinct value_field AS VALUEDATA, TO_CHAR(disp_field) AS DISPLAYDATA, SITE FROM ( SELECT SITE AS value_field, SITE AS disp_field, SITE FROM EQP_VW_SPC WHERE SITE is not null )  ORDER BY SITE ASC NULLS FIRST";
                ds = base.Query(query);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :FAB Condition

        public DataSet GetFab(byte[] baData)
        {
            DataSet ds = null;
            string sSite = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append("select  DISTINCT FAB from EQP_VW_SPC");
                if (!string.IsNullOrEmpty(sSite))
                {
                    _sbQuery.Append("   WHERE SITE = :SITE  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }
                _sbQuery.Append(" ORDER BY FAB  ");
                ds = base.Query(_sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetFabForCondition(byte[] baData)
        {
            DataSet ds = null;
            string sSite = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append(
                    "SELECT distinct value_field AS VALUEDATA, TO_CHAR(disp_field) AS DISPLAYDATA, FAB FROM ( SELECT FAB AS value_field, FAB AS disp_field, FAB FROM EQP_VW_SPC WHERE FAB is not null ");
                if (!string.IsNullOrEmpty(sSite))
                {
                    this._sbQuery.Append(" AND SITE = :SITE");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }
                this._sbQuery.Append(") ORDER BY FAB ASC NULLS FIRST");
                
                ds = base.Query(_sbQuery.ToString(), llstData);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion


        #region :Line Condition

        private const string SQL_QUERY_LINE =
            @"SELECT   LINE, LINE_RAWID 
                FROM EQP_VW_SPC 
                WHERE SITE = 'SITE_A'    
                ORDER BY LINE   ";

        /// <summary>
        /// Gets line information
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetLine()
        {
            DataSet ds = null;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                ds = base.Query(SQL_QUERY_LINE, whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_LINE_BYSITE =
            @"SELECT   DISTINCT LINE, LINE_RAWID  FROM EQP_VW_SPC  ";


        public DataSet GetLine(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sSite = "";
                string sFab = "";

                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append(SQL_QUERY_LINE_BYSITE);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                if (llstData[Definition.CONDITION_KEY_FAB] != null)
                    sFab = llstData[Definition.CONDITION_KEY_FAB].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append("SELECT   DISTINCT LINE, LINE_RAWID  FROM EQP_VW_SPC  WHERE 1=1 ");

                if (!string.IsNullOrEmpty(sSite))
                {
                    _sbQuery.Append("   AND SITE = :SITE  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }

                if (!string.IsNullOrEmpty(sFab))
                {
                    _sbQuery.Append("   AND  FAB = :FAB  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_FAB, sFab);
                }
                _sbQuery.Append(" ORDER BY LINE  ");
                ds = base.Query(_sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetLineForCondition(byte[] baData)
        {
            DataSet ds = null;
            string sSite = string.Empty;
            string sFab = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                if (llstData[Definition.CONDITION_KEY_FAB] != null)
                    sFab = llstData[Definition.CONDITION_KEY_FAB].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append(
                    "SELECT distinct value_field AS VALUEDATA, TO_CHAR(disp_field) AS DISPLAYDATA, LINE FROM ( SELECT LINE_RAWID AS value_field, LINE AS disp_field, LINE FROM EQP_VW_SPC WHERE LINE_RAWID is not null ");
                if (!string.IsNullOrEmpty(sSite))
                {
                    this._sbQuery.Append(" AND SITE = :SITE");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }
                if (!string.IsNullOrEmpty(sFab))
                {
                    _sbQuery.Append("   AND  FAB = :FAB  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_FAB, sFab);
                }

                this._sbQuery.Append(") ORDER BY LINE ASC NULLS FIRST");
                
                ds = base.Query(_sbQuery.ToString(), whereFieldData);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Area Condition

        private const string SQL_QUERY_AREA =
            @"SELECT DISTINCT AREA_RAWID, AREA, LINE_RAWID
                FROM EQP_VW_SPC
                WHERE 1 = 1                
                ";
//            @"SELECT DISTINCT AREA_RAWID, AREA, LINE_RAWID
//                FROM EQP_VW_SPC
//                WHERE LINE_RAWID = :LINE_RAWID
//                order by AREA
//                ";

        public DataSet GetArea(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sLine = "";

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                    sLine = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();

                StringBuilder sbQuery = new StringBuilder();
                LinkedList whereFieldData = new LinkedList();
                
                sbQuery.Append(SQL_QUERY_AREA);

                if (sLine.Trim().Length > 0)
                {
                    sbQuery.Append(" LINE_RAWID = :LINE_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLine);
                }

                sbQuery.Append(" order by AREA ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetAreaForCondition(byte[] baData)
        {
            DataSet ds = null;
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                if (llstData[Definition.CONDITION_KEY_FAB] != null)
                    sFab = llstData[Definition.CONDITION_KEY_FAB].ToString();

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                    sLine = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append(
                    "SELECT distinct value_field AS VALUEDATA, TO_CHAR(disp_field) AS DISPLAYDATA, AREA FROM ( SELECT AREA_RAWID AS value_field, AREA AS disp_field, AREA FROM EQP_VW_SPC WHERE AREA_RAWID is not null ");
                if (!string.IsNullOrEmpty(sSite))
                {
                    this._sbQuery.Append(" AND SITE = :SITE");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }
                if (!string.IsNullOrEmpty(sFab))
                {
                    _sbQuery.Append("   AND  FAB = :FAB  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_FAB, sFab);
                }
                if (!string.IsNullOrEmpty(sLine))
                {
                    _sbQuery.Append("   AND  LINE_RAWID = :LINE_RAWID  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLine);
                }

                this._sbQuery.Append(")  ORDER BY AREA ASC NULLS FIRST");
                
                ds = base.Query(_sbQuery.ToString(), whereFieldData);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }
        #endregion

        #region :EQP Model Condition

        private const string SQL_QUERY_EQPMODEL =
            @"SELECT DISTINCT PPEQP.EQP_MODEL, PPEQP.LOCATION_RAWID LINE_RAWID, PPEQP.AREA_RAWID AREA_RAWID, PPEQP.EQP_MODEL DISPLAYDATA
                FROM EQP_MST_PP PPEQP              
                WHERE PPEQP.LOCATION_RAWID = :LINE_RAWID  ";

        public DataSet GetEqpModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sLine = "";
                string sArea = "";

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                    sLine = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                    sArea = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();

                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, Convert.ToDouble(sLine));

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(SQL_QUERY_EQPMODEL);
                if (sArea != string.Empty && sArea != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, Convert.ToDouble(sArea));
                    sbQuery.Append("    AND PPEQP.AREA_RAWID = :AREA_RAWID                            ");
                }
                sbQuery.Append("  ORDER BY PPEQP.EQP_MODEL                                      ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetEqpModelForCondition(byte[] baData)
        {
            DataSet ds = null;
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;
            string sArea = string.Empty;
            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                if (llstData[Definition.CONDITION_KEY_SITE] != null)
                    sSite = llstData[Definition.CONDITION_KEY_SITE].ToString();

                if (llstData[Definition.CONDITION_KEY_FAB] != null)
                    sFab = llstData[Definition.CONDITION_KEY_FAB].ToString();

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                    sLine = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();

                if(llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                    sArea = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();

                LinkedList whereFieldData = new LinkedList();
                this._sbQuery.Remove(0, this._sbQuery.Length);
                this._sbQuery.Append(
                    "SELECT distinct value_field AS VALUEDATA, TO_CHAR(disp_field) AS DISPLAYDATA, EQPMODEL FROM ( SELECT EQPMODEL AS value_field, EQPMODEL AS disp_field, EQPMODEL FROM EQP_VW_SPC WHERE EQPMODEL is not null ");
                if (!string.IsNullOrEmpty(sSite))
                {
                    this._sbQuery.Append(" AND SITE = :SITE");
                    whereFieldData.Add(Definition.CONDITION_KEY_SITE, sSite);
                }
                if (!string.IsNullOrEmpty(sFab))
                {
                    _sbQuery.Append("   AND  FAB = :FAB  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_FAB, sFab);
                }
                if (!string.IsNullOrEmpty(sLine))
                {
                    _sbQuery.Append("   AND  LINE_RAWID = :LINE_RAWID  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLine);
                }
                if(!string.IsNullOrEmpty(sArea))
                {
                    _sbQuery.Append("   AND  AREA_RAWID = :AREA_RAWID  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, sArea);
                }


                this._sbQuery.Append(")  ORDER BY EQPMODEL ASC NULLS FIRST");
                
                ds = base.Query(_sbQuery.ToString(), whereFieldData);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :EQP Condition

        private const string SQL_QUERY_EQP =
            //            @"SELECT DISTINCT EQP_ID, RAWID, AREA_RAWID, LOCATION_RAWID LINE_RAWID, EQP_MODEL
            //                    FROM EQP_MST_PP              
            //                    WHERE LOCATION_RAWID = :LINE_RAWID    ";


@"SELECT DISTINCT EMP.EQP_ID, EMP.MODULE_NAME,EMP.RAWID, EMP.AREA_RAWID, EMP.LOCATION_RAWID LINE_RAWID, EMP.EQP_MODEL, EMP.MODULE_ID 
FROM EQP_MST_PP  EMP , MET_PROC_DATA_LNK_PP MPD,
     MET_DATA_TRX_PP MDTP     
WHERE module_type_cd='M'  
AND LOCATION_RAWID = :LINE_RAWID 
AND EMP.MODULE_ID=MPD.MAIN_MODULE_ID
AND MPD.MET_DATA_TRX_RAWID =MDTP.RAWID
";

        public DataSet GetEQP(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sLine = "";
                string sArea = "";
                string sEqpModel = "";
                string sOperation = string.Empty;
                string sProduct = string.Empty;

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                    sLine = llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString();

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                    sArea = llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString();

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                    sEqpModel = llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();

                if (llstData[Definition.DynamicCondition_Condition_key.PRODUCT] != null)
                    sProduct = llstData[Definition.DynamicCondition_Condition_key.PRODUCT].ToString();

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                    sOperation = llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();

                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, Convert.ToDouble(sLine));

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_EQP);

                if (!string.IsNullOrEmpty(sArea) && sArea != Definition.VARIABLE.ALL)
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, Convert.ToDouble(sArea));
                    sbQuery.Append("    AND EMP.AREA_RAWID = :AREA_RAWID                              ");
                }
                if (!string.IsNullOrEmpty(sEqpModel) && sEqpModel != Definition.VARIABLE.ALL)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, sEqpModel);
                    sbQuery.Append("    AND EMP.EQP_MODEL = :EQP_MODEL                          ");
                }

                if (!string.IsNullOrEmpty(sProduct) && sProduct != Definition.VARIABLE.ALL)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PRODUCT, sProduct);
                    sbQuery.Append("    AND MDTP.PRODUCT_ID=:PRODUCT_ID");
                }

                if (!string.IsNullOrEmpty(sOperation) && sOperation != Definition.VARIABLE.ALL)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, sOperation);
                    sbQuery.Append("    AND MDTP.OPERATION_ID=:OPERATION_ID");
                }

                sbQuery.Append("  AND EMP.PARENT IS NULL                                        ");
                sbQuery.Append("  ORDER BY EMP.EQP_ID                                           ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_EQP_INFO =
            @"SELECT *
                FROM EQP_MST_PP              
                WHERE RAWID = :RAWID    ";

        private DataSet GetEQPInfo(string sRawID)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.Add("RAWID", Convert.ToInt32(sRawID));

                ds = base.Query(SQL_QUERY_EQP_INFO, llstData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :DCP Condition

        private const string SQL_QUERY_DCP =
            //            @"SELECT DISTINCT DCP_ID, RAWID, DCP_STATE_CD
            //                FROM EQP_DCP_MST_PP              
            //                WHERE     ";
@"
SELECT *
  FROM eqp_dcp_mst_pp a, eqp_mst_pp b
 WHERE a.eqp_id = b.eqp_id
   AND b.PARENT IS NULL
";

        public DataSet GetDCP(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_DCP);

                string lineRawid = "";
                string areaRawid = "";
                string eqpID = "";
                ArrayList alEqpID = null;

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    lineRawid = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
                    sbQuery.Append("    AND B.LOCATION_RAWID = :LINE_RAWID      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, Convert.ToDouble(lineRawid));
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    areaRawid = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
                    sbQuery.Append("    AND B.AREA_RAWID = :AREA_RAWID      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, Convert.ToDouble(areaRawid));
                }

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                {
                    eqpID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                    sbQuery.Append("    AND B.EQP_ID = :EQP_ID      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
                }
                else if (llstData[Definition.CONDITION_KEY_EQP_ID_LIST] != null)
                {
                    alEqpID = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_ID_LIST];
                    string sEqpList = _commondata.MakeVariablesList(alEqpID);

                    string sSql = string.Format(" AND B.EQP_ID IN ({0}) ", sEqpList);
                    sbQuery.Append(sSql);
                }

                sbQuery.Append("  ORDER BY DCP_ID                                           ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        //public DataSet GetDCP(byte[] baData)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(baData);

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();
        //        sbQuery.Append(SQL_QUERY_DCP);

        //        string eqpRawID = "";
        //        ArrayList alEqpRawid = null;

        //        if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
        //        {
        //            eqpRawID = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
        //            sbQuery.Append("    EQP_RAWID = :EQP_RAWID      ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_RAWID, Convert.ToDouble(eqpRawID));
        //        }
        //        else if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
        //        {
        //            alEqpRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
        //            string sEqpList = _commondata.MakeVariablesList(alEqpRawid);

        //            string sSql = string.Format(" EQP_RAWID IN ({0}) ", sEqpList);
        //            sbQuery.Append(sSql);
        //        }
        //        sbQuery.Append("  ORDER BY DCP_ID                                           ");

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //}

        #endregion

        #region :Module Condition


        private const string SQP_QUERY_MODULE =
@"
SELECT a.*, b.dcp_id
  FROM (SELECT emp1.*, emp2.module_id AS parent_moduleid
          FROM (SELECT *
                  FROM eqp_mst_pp
                 WHERE PARENT IS NOT NULL) emp1
               INNER JOIN
               (SELECT *
                  FROM eqp_mst_pp ) emp2
               ON emp1.parent = emp2.rawid AND emp1.eqp_id = emp2.eqp_id
        UNION ALL
        SELECT eqp_mst_pp.*, NULL AS parent_id
          FROM eqp_mst_pp
         WHERE PARENT IS NULL) a,
       eqp_dcp_mst_pp b
   WHERE a.eqp_id = b.eqp_id
";

        public DataSet GetModuleByEQP(byte[] badata /*Line, Area, Model, Eqp*/)
        {
            DataSet ds = null;

            try
            {
                string sDcpRawid = string.Empty;
                string sEqpRawid = string.Empty;
                string sEqpID = string.Empty;
                string sLineRawid = string.Empty;
                string sAreaRawid = string.Empty;
                string sModuleID = string.Empty;

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQP_QUERY_MODULE);

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(badata);

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    sLineRawid = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
                    if (sLineRawid != string.Empty)
                    {
                        whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLineRawid);
                        sbQuery.Append("    AND A.LOCATION_RAWID = :LINE_RAWID    ");
                    }
                }
                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sAreaRawid = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
                    if (sAreaRawid != string.Empty)
                    {
                        whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawid);
                        sbQuery.Append("    AND A.AREA_RAWID = :AREA_RAWID    ");
                    }
                }
                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                {
                    sEqpID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                    if (sEqpID != string.Empty)
                    {
                        whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, sEqpID.Trim());
                        sbQuery.Append("    AND A.EQP_ID = :EQP_ID    ");
                    }
                }

                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                {
                    sDcpRawid = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();
                    if (sDcpRawid != string.Empty)
                    {
                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDcpRawid);
                        sbQuery.Append("    AND B.dcp_id = :dcp_id  ");
                    }
                }

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    if (sModuleID != string.Empty)
                    {
                        whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                        sbQuery.Append("    AND (a.parent_moduleid = :module_id or a.parent_moduleid is null) ");
                    }
                }


                sbQuery.Append("  ORDER BY ALIAS                              ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        //        private const string SQP_QUERY_MODULE =
        //@"
        //SELECT   emp.module_name, emp.rawid, emp.alias, emp.PARENT eqp_rawid,
        //         emp.location_rawid line_rawid, emp.area_rawid, emp.eqp_model
        //    FROM eqp_mst_pp emp, eqp_dcp_mst_pp dmp
        //   WHERE emp.PARENT = :eqp_rawid
        //      OR emp.rawid = :eqp_rawid AND emp.rawid = dmp.eqp_rawid
        //";

        //        public DataSet GetModuleByEQP(byte[] badata /*Line, Area, Model, Eqp*/)
        //        {
        //            DataSet ds = null;

        //            try
        //            {
        //                string sDcpRawid = string.Empty;
        //                string sEqpRawid = string.Empty;
        //                string sEqpID = string.Empty;
        //                string sLineRawid = string.Empty;
        //                string sAreaRawid = string.Empty;

        //                LinkedList whereFieldData = new LinkedList();
        //                StringBuilder sbQuery = new StringBuilder();
        //                sbQuery.Append(SQP_QUERY_MODULE);

        //                LinkedList llstData = new LinkedList();
        //                llstData.SetSerialData(badata);

        //                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
        //                {
        //                    sLineRawid = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
        //                    if (sLineRawid != string.Empty)
        //                    {
        //                        whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLineRawid);
        //                        sbQuery.Append("    AND LOCATION_RAWID = :LINE_RAWID    ");
        //                    }
        //                }
        //                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
        //                {
        //                    sAreaRawid = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
        //                    if (sAreaRawid != string.Empty)
        //                    {
        //                        whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawid);
        //                        sbQuery.Append("    AND AREA_RAWID = :AREA_RAWID    ");
        //                    }
        //                }
        //                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
        //                {
        //                    sEqpID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
        //                    if (sEqpID != string.Empty)
        //                    {
        //                        whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, sEqpID.Trim());
        //                        sbQuery.Append("    AND EQP_ID = :EQP_ID    ");
        //                    }
        //                }

        //                if(llstData[Definition.CONDITION_KEY_DCP_ID] != null)
        //                {
        //                    sDcpRawid = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();
        //                    if (sDcpRawid != string.Empty)
        //                    {
        //                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDcpRawid);
        //                        sbQuery.Append("AND dmp.dcp_id = :dcp_id  ");
        //                    }
        //                }

        //                sbQuery.Append(@"  GROUP BY emp.module_name,
        //                                     emp.rawid,
        //                                     emp.alias,
        //                                     emp.PARENT,
        //                                     emp.location_rawid,
        //                                     emp.area_rawid,
        //                                     emp.eqp_model                       
        //                                ");

        //                sbQuery.Append("  ORDER BY MODULE_NAME                              ");

        //                ds = base.Query(sbQuery.ToString(), whereFieldData);
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine(ex);
        //            }

        //            return ds;
        //        }


        private const string SQP_QUERY_SUB_MODULE =
            //            @"SELECT MODULE_NAME, RAWID, ALIAS, PARENT EQP_RAWID, LOCATION_RAWID LINE_RAWID, AREA_RAWID, EQP_MODEL, EQP_ID, MODULE_LEVEL
            //              FROM EQP_MST_PP
            //              WHERE PARENT = :EQP_RAWID
            //            ";
@"
SELECT   A.*, B.DCP_ID
    FROM (SELECT EMP1.*, EMP2.MODULE_ID AS PARENT_MODULEID
            FROM (SELECT *
                    FROM EQP_MST_PP
                   WHERE PARENT IS NOT NULL) EMP1
                 INNER JOIN
                 (SELECT *
                    FROM EQP_MST_PP) EMP2
                 ON EMP1.PARENT = EMP2.RAWID AND EMP1.EQP_ID = EMP2.EQP_ID
          UNION ALL
          SELECT EQP_MST_PP.*, NULL AS PARENT_ID
            FROM EQP_MST_PP
           WHERE PARENT IS NULL) A,
         EQP_DCP_MST_PP B
WHERE A.EQP_ID = B.EQP_ID
AND PARENT_MODULEID = :MODULE_ID
AND B.DCP_ID = :DCP_ID
ORDER BY ALIAS   
";

        public DataSet GetSubModuleByEQP(byte[] badata /*parent module rawid*/)
        {
            DataSet ds = null;

            try
            {
                string sEqpRawid = string.Empty;
                string sDCPID = string.Empty;

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQP_QUERY_SUB_MODULE);

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(badata);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sEqpRawid = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sEqpRawid);
                }

                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                {
                    sDCPID = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();
                    whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDCPID);
                }

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        private const string SQL_QUERY_CHAMBER =
            @"SELECT DISTINCT MODULE_NAME, RAWID, PARENT EQP_RAWID, LOCATION_RAWID LINE_RAWID, AREA_RAWID, EQP_MODEL
                FROM EQP_MST_PP              
                WHERE LOCATION_RAWID = :LINE_RAWID    ";


        public DataSet GetModule(byte[] badata /*Line, Area, Model, Eqp*/)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(badata);

                string sLine = "";
                string sAreaRawid = "";
                string sEqpModel = "";
                string sEqpRawid = "";

                //if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                //    sEqp = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();

                ArrayList alEqpRawid = null;

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_CHAMBER);

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                    sLine = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                    sAreaRawid = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                    sEqpModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();

                whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, Convert.ToDouble(sLine));

                if (sAreaRawid != string.Empty && sAreaRawid != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, Convert.ToDouble(sAreaRawid));
                    sbQuery.Append("    AND AREA_RAWID = :AREA_RAWID                          ");
                }
                if (sEqpModel != string.Empty && sEqpModel != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, sEqpModel);
                    sbQuery.Append("    AND EQP_MODEL = :EQP_MODEL                      ");
                }

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                {
                    sEqpRawid = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                    sbQuery.Append("    AND EQP_ID = :EQP_ID      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, sEqpRawid);
                }
                else if (llstData[Definition.CONDITION_KEY_EQP_ID_LIST] != null)
                {
                    alEqpRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_ID_LIST];
                    string sEqpList = _commondata.MakeVariablesList(alEqpRawid);

                    string sSql = string.Format(" AND EQP_ID IN ({0}) ", sEqpList);
                    sbQuery.Append(sSql);
                }

                if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                {
                    sEqpRawid = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
                    sbQuery.Append("    AND (PARENT = :EQP_RAWID OR RAWID = :EQP_RAWID )     ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, sEqpRawid);
                }
                else if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
                {
                    alEqpRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
                    string sEqpList = _commondata.MakeVariablesList(alEqpRawid);

                    string sSql = string.Format(" AND (PARENT IN ({0}) OR RAWID IN ({0}) ) ", sEqpList);
                    sbQuery.Append(sSql);
                }

                //sbQuery.Append("  AND MODULE_TYPE_CD = 'C'                         ");
                //sbQuery.Append("  GROUP BY MODULE_NAME, RAWID                       ");
                sbQuery.Append("  ORDER BY MODULE_NAME                              ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Product Condition




        private const string SQL_QUERY_PRODUCT =
@"
SELECT   eqp_module_id, product_id
    FROM eqp_recipe_mst_pp
";

        public DataSet GetProduct(byte[] btData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btData);

                string sModuleID = "";
                ArrayList alModuleIDList = null;

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                    alModuleIDList = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_PRODUCT);

                if (sModuleID != string.Empty && sModuleID != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                    sbQuery.Append("    WHERE eqp_module_id = :module_id                ");
                }
                else if (alModuleIDList != null && alModuleIDList.Count > 0)
                {
                    string sModuleList = _commondata.MakeVariablesList(alModuleIDList);

                    string sSql = string.Format(" WHERE eqp_module_id IN ({0}) ", sModuleList);
                    sbQuery.Append(sSql);
                }

                sbQuery.Append("  GROUP BY eqp_module_id, product_id                       ");
                sbQuery.Append("  ORDER BY product_id                                      ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion


        #region :Substrate Condition

        private const string SQL_QUERY_SUBSTRATE =
@"
SELECT   eqp_module_id, product_id, recipe_id, rawid
    FROM eqp_recipe_mst_pp
   WHERE 1=1
";

        public DataSet GetSubstrate(byte[] btData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btData);

                string sModuleID = "";
                string sProductID = "";
                string sRecipeID = "";
                string sSubstrateID = "";

                ArrayList alModuleID = null;
                bool bExistProductCondition = false;

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_SUBSTRATE);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    sbQuery.Append("    AND eqp_module_id = :module_id       ");
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                }
                else if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                {
                    alModuleID = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];
                    string sModuleList = _commondata.MakeVariablesList(alModuleID);

                    string sSql = string.Format(" AND eqp_module_id IN ({0}) ", sModuleList);
                    sbQuery.Append(sSql);
                }

                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                {
                    bExistProductCondition = true;
                    sProductID = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();
                }
                if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
                    sRecipeID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_SUBSTRATE_ID] != null)
                    sSubstrateID = llstData[Definition.CONDITION_KEY_SUBSTRATE_ID].ToString();

                if (sProductID == "-1" || (bExistProductCondition && sProductID == string.Empty))
                {
                    sbQuery.Append("    AND PRODUCT_ID IS NULL                 ");
                }
                else if (sProductID != string.Empty && sProductID != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_PRODUCT_ID, sProductID);
                    sbQuery.Append("    AND product_id = :product_id                 ");
                }

                if (sSubstrateID != string.Empty && sSubstrateID != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_SUBSTRATE_ID, sSubstrateID);
                    sbQuery.Append("    AND substrate_id = :substrate_id                 ");
                }

                sbQuery.Append("  GROUP BY eqp_module_id, product_id, recipe_id  , rawid   ");
                sbQuery.Append("  ORDER BY recipe_id                                      ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Recipe Step Condition

        private const string SQL_QUERY_RECIPE =
@"
SELECT   eqp_module_id, product_id, recipe_id, rawid
    FROM eqp_recipe_mst_pp
   WHERE 1=1
";

        public DataSet GetRecipe(byte[] btData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btData);

                string sModuleID = "";
                string sProductID = "";
                string sRecipeID = "";
                ArrayList alModuleID = null;
                bool bExistProductCondition = false;

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_RECIPE);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    sbQuery.Append("    AND eqp_module_id = :module_id       ");
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                }
                else if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                {
                    alModuleID = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];
                    string sModuleList = _commondata.MakeVariablesList(alModuleID);

                    string sSql = string.Format(" AND eqp_module_id IN ({0}) ", sModuleList);
                    sbQuery.Append(sSql);
                }

                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                {
                    bExistProductCondition = true;
                    sProductID = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();
                }
                if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
                    sRecipeID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();

                if (sProductID == "-1" || (bExistProductCondition && sProductID == string.Empty))
                {
                    sbQuery.Append("    AND PRODUCT_ID IS NULL                 ");
                }
                else if (sProductID != string.Empty && sProductID != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_PRODUCT_ID, sProductID);
                    sbQuery.Append("    AND product_id = :product_id                 ");
                }

                if (sRecipeID != string.Empty && sRecipeID != "ALL")
                {
                    whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_ID, sRecipeID);
                    sbQuery.Append("    AND recipe_id = :recipe_id                 ");
                }

                sbQuery.Append("  GROUP BY eqp_module_id, product_id, recipe_id  , rawid   ");
                sbQuery.Append("  ORDER BY recipe_id                                      ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        private const string SQL_QUERY_STEP_ALL =
@"
select * from eqp_trace_spec_mst_fdc ets
where ets.EQP_RECIPE_STEP_RAWID is NULL
";

        public DataSet GetAllStep(byte[] btdata)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string recipeRawID = "";
                string eqpRawID = "";
                bool bSpecExist = false;
                ArrayList recipeList = null;
                LinkedList whereFieldData = new LinkedList();

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(SQL_QUERY_STEP_ALL);

                if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                {
                    eqpRawID = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
                    sbQuery.Append(" and ets.eqp_rawid = :eqp_rawid ");
                    whereFieldData.Add("EQP_RAWID", Convert.ToDouble(eqpRawID));
                }

                if (llstData[Definition.CONDITION_KEY_RECIPE_RAWID] != null)
                {
                    recipeRawID = llstData[Definition.CONDITION_KEY_RECIPE_RAWID].ToString();
                    sbQuery.Append(" and (ets.eqp_recipe_rawid = :eqp_recipe_rawid or ets.eqp_recipe_rawid is NULL) ");
                    whereFieldData.Add("EQP_RECIPE_RAWID", Convert.ToDouble(recipeRawID));
                }
                else if (llstData[Definition.CONDITION_KEY_RECIPE_LIST_RAWID] != null)
                {
                    recipeList = (ArrayList)llstData[Definition.CONDITION_KEY_RECIPE_LIST_RAWID];

                    Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();
                    string sRecipeList = commondata.MakeVariablesList(recipeList);

                    string sSql = string.Format("and ets.EQP_RECIPE_RAWID IN ({0}) ", sRecipeList);
                    sbQuery.Append(sSql);
                }

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        private const string SQL_QUERY_STEP =
@"
SELECT   *
    FROM eqp_recipe_step_mst_pp
   WHERE 1 = 1
";


        //        private const string SQL_QUERY_STEP_SPEC_EXIST =
        //        @"SELECT TO_CHAR(RVT.RAWID) AS RAWID, RVT.STEP_NAME, RVT.STEP_ID, 
        //                 TO_CHAR(RVT.SPEC_RAWID) AS SPEC_RAWID, 
        //                 TO_CHAR(RVT.EQP_RECIPE_STEP_RAWID) AS EQP_RECIPE_STEP_RAWID, 
        //                 DECODE (RVT.EQP_RECIPE_STEP_RAWID, NULL, 'N', 'Y') SPEC_EXIST,
        //                RVT.EQP_RECIPE_RAWID
        //            FROM (SELECT TO_CHAR(ERSMP.RAWID) RAWID, ERSMP.STEP_NAME, ERSMP.STEP_ID,
        //                         ERSMP.RAWID SPEC_RAWID, VT.EQP_RECIPE_STEP_RAWID,
        //                         ERSMP.EQP_RECIPE_RAWID
        //                    FROM EQP_RECIPE_STEP_MST_PP ERSMP
        //                         LEFT OUTER JOIN
        //                         (SELECT ETS.EQP_RECIPE_STEP_RAWID
        //                            FROM EQP_TRACE_SPEC_MST_FDC ETS) VT
        //             ON (vt.eqp_recipe_step_rawid = ersmp.rawid)   ";

        public DataSet GetRecipeStep(byte[] btdata)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string moduleID = "";
                string productID = "";
                string recipeID = "";
                bool bSpecExist = false;
                ArrayList recipeList = null;
                LinkedList whereFieldData = new LinkedList();

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(SQL_QUERY_STEP);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    moduleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    sbQuery.Append(" AND eqp_module_id = :module_id      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, moduleID);
                }

                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                {
                    productID = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();
                    if (productID.Length > 0)
                    {
                        sbQuery.Append(" AND product_id = :product_id      ");
                        whereFieldData.Add(Definition.CONDITION_KEY_PRODUCT_ID, productID);
                    }
                    else
                    {
                        sbQuery.Append(" AND product_id is null     ");
                    }
                }

                if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
                {
                    recipeID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();
                    sbQuery.Append(" AND eqp_recipe_id = :recipe_id      ");
                    whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_ID, recipeID);
                }
                else if (llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST] != null)
                {
                    recipeList = (ArrayList)llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST];

                    Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();
                    string sRecipeList = commondata.MakeVariablesList(recipeList);

                    string sSql = string.Format("AND eqp_recipe_id IN ({0}) ", sRecipeList);
                    sbQuery.Append(sSql);
                }

                //if (bSpecExist)
                //{
                //    sbQuery.Append("    GROUP BY ERSMP.RAWID, ERSMP.STEP_NAME, ERSMP.STEP_ID, ERSMP.RAWID, ERSMP.EQP_RECIPE_RAWID, VT.EQP_RECIPE_STEP_RAWID ORDER BY ERSMP.RAWID) RVT        ");
                //}
                //else
                //{
                //    sbQuery.Append("    GROUP BY ERSMP.RAWID, ERSMP.STEP_NAME, ERSMP.STEP_ID, ERSMP.RAWID, EQP_RECIPE_RAWID ORDER BY ERSMP.RAWID) RVT       ");
                //}

                sbQuery.Append("    ORDER BY step_name  ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_STEP_BY_EQP =
        @"SELECT   STEP.STEP_ID, STEP.STEP_NAME, TO_CHAR(STEP.RAWID) RAWID
            FROM EQP_RECIPE_STEP_MST_PP STEP JOIN EQP_RECIPE_MST_PP RCP 
                 ON (RCP.RAWID = STEP.EQP_RECIPE_RAWID) AND RCP.EQP_RAWID = :EQP_RAWID {0}
            ORDER BY STEP.STEP_ID    ";

        public DataSet QueryRecipeStepByEQP(byte[] btdata)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string sModuleRawID = "";
                string sRecipeRawID = "";

                string sSQLCondition = "";

                LinkedList whereFieldData = new LinkedList();

                if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
                {
                    sModuleRawID = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();
                    whereFieldData.Add("EQP_RAWID", Convert.ToDouble(sModuleRawID));
                }


                if (llstData[Definition.CONDITION_KEY_RECIPE_RAWID] != null)
                {
                    sRecipeRawID = llstData[Definition.CONDITION_KEY_RECIPE_RAWID].ToString();
                    sSQLCondition = "AND STEP.EQP_RECIPE_RAWID = :EQP_RECIPE_RAWID";
                    whereFieldData.Add("EQP_RECIPE_RAWID", sRecipeRawID);
                }

                string sSQL = string.Format(SQL_QUERY_STEP_BY_EQP, sSQLCondition);

                ds = base.Query(sSQL, whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Parameter

        private const string SQL_QUERY_PARAM_GROUP =
            @"SELECT * FROM EQP_PG_MST_PP
              WHERE EQP_RAWID = :EQP_RAWID
              AND PARENT = :PARENT ";

        public DataSet GetParamGroup(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                this._sbQuery.Remove(0, this._sbQuery.Length);

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                _sbQuery.Append(SQL_QUERY_PARAM_GROUP);
                LinkedList whereFieldData = new LinkedList();

                string sEqpRawid = string.Empty;
                string sParent = string.Empty;

                if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                    sEqpRawid = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_PARENT] != null)
                    sParent = llstData[Definition.CONDITION_KEY_PARENT].ToString();

                if (sEqpRawid != string.Empty)
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_RAWID, sEqpRawid);

                if (sParent != string.Empty)
                    whereFieldData.Add(Definition.CONDITION_KEY_PARENT, sParent);

                ds = base.Query(_sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_PARAM_BY_GROUP =
            //@"
            //SELECT   a.*, cvt.activate_yn, DECODE (cvt.COUNT, 0, '', 'O') spec_exist
            //    FROM eqp_trace_param_mst_pp a,
            //         (SELECT   vt.rawid, COUNT (vt.eqp_trace_param_rawid) AS COUNT,
            //                   vt.activate_yn, vt.eqp_module_id as module_id
            //              FROM (SELECT etpmp.eqp_id, etpmp.rawid, etpmp.eqp_module_id,
            //                           etsmf.eqp_trace_param_rawid, etsmf.activate_yn
            //                      FROM eqp_trace_param_mst_pp etpmp LEFT OUTER JOIN eqp_trace_spec_mst_fdc etsmf
            //                           ON etsmf.eqp_trace_param_rawid = etpmp.rawid
            //                         AND etsmf.activate_yn = 'Y'
            //                     WHERE etpmp.eqp_module_id = :module_id) vt
            //          GROUP BY vt.rawid, vt.activate_yn, vt.eqp_module_id) cvt
            //   WHERE module_id = :module_id
            //     AND eqp_dcp_id = :dcp_id
            //     AND cvt.rawid = a.rawid
            //     AND (a.display_yn IS NULL OR a.display_yn = 'Y')
            //ORDER BY a.param_alias
            //";

@"SELECT   a.*, cvt.activate_yn, DECODE (cvt.COUNT, 0, '', 'O') spec_exist
    FROM eqp_trace_param_mst_pp a,
         (SELECT   vt.rawid, COUNT (vt.eqp_param_alias) AS COUNT,
                   vt.activate_yn, vt.eqp_module_id as module_id
              FROM (SELECT etpmp.eqp_id, etpmp.rawid, etpmp.eqp_module_id,
                           ETSMF.EQP_PARAM_ALIAS, etsmf.activate_yn
                      FROM eqp_trace_param_mst_pp etpmp LEFT OUTER JOIN eqp_trace_spec_mst_fdc etsmf                           
                        ON ETPMP.EQP_MODULE_ID = ETSMF.EQP_MODULE_ID
                           AND ETPMP.PARAM_ALIAS = ETSMF.EQP_PARAM_ALIAS                       
                           AND etsmf.activate_yn = 'Y'
                     WHERE etpmp.eqp_module_id = :module_id
                       and ETPMP.EQP_DCP_ID = :dcp_id
                   ) vt
          GROUP BY vt.rawid, vt.activate_yn, vt.eqp_module_id) cvt
   WHERE module_id = :module_id
     AND eqp_dcp_id = :dcp_id
     AND cvt.rawid = a.rawid
     AND (a.display_yn IS NULL OR a.display_yn = 'Y')
ORDER BY a.param_alias";



        public DataSet GetParamByGroup(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                this._sbQuery.Remove(0, this._sbQuery.Length);

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                _sbQuery.Append(SQL_QUERY_PARAM_BY_GROUP);
                LinkedList whereFieldData = new LinkedList();

                string sEqpPGRawid = string.Empty;
                string sModuleID = string.Empty;
                string sDcpID = string.Empty;

                if (llstData[Definition.CONDITION_KEY_EQP_PG_RAWID] != null)
                    sEqpPGRawid = llstData[Definition.CONDITION_KEY_EQP_PG_RAWID].ToString();
                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                    sDcpID = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();

                //if (sEqpPGRawid != string.Empty)
                //{
                //    if (sEqpPGRawid == "-1")
                //    {
                //        _sbQuery.Append("   and (etpmp.eqp_pg_rawid = :eqp_pg_rawid or etpmp.eqp_pg_rawid is null)  ");
                //    }
                //    else
                //    {
                //        _sbQuery.Append("   and etpmp.eqp_pg_rawid = :eqp_pg_rawid  ");
                //    }

                //    whereFieldData.Add(Definition.CONDITION_KEY_EQP_PG_RAWID, sEqpPGRawid);
                //}

                if (sDcpID != string.Empty)
                {
                    //_sbQuery.Append("   and edmp.rawid = :dcp_rawid  ");
                    whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDcpID);
                }

                if (sModuleID != string.Empty)
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);

                //_sbQuery.Append("   and (etpmp.display_yn is null or etpmp.display_yn = 'Y') ");
                //_sbQuery.Append("   and etpmp.data_type_cd in ('FLT', 'INT') ");
                //_sbQuery.Append("   order by etpmp.param_name  ");

                ds = base.Query(_sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_PARAM_SPEC =
            @"SELECT * FROM EQP_TRACE_SPEC_MST_FDC
            ";


        public DataSet GetParamSpec(byte[] btData)
        {
            DataSet ds = null;

            try
            {
                this._sbQuery.Remove(0, this._sbQuery.Length);

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btData);

                _sbQuery.Append(SQL_QUERY_PARAM_SPEC);

                LinkedList whereFieldData = new LinkedList();
                ArrayList alParamRawIDList = new ArrayList();

                if (llstData[Definition.CONDITION_KEY_PARAM_RAWID_LIST] != null)
                {
                    alParamRawIDList = (ArrayList)llstData[Definition.CONDITION_KEY_PARAM_RAWID_LIST];

                    string sParamRawIDList = _commondata.MakeVariablesList(alParamRawIDList);
                    string sSql = string.Format("WHERE EQP_TRACE_PARAM_RAWID IN ({0}) ", sParamRawIDList);
                    _sbQuery.Append(sSql);
                }

                ds = base.Query(_sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetParameter(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                ParameterType type = 0;
                string sModuleRawid = "";
                string sDCPID = "";
                string sDCPRawID = "";
                string sEQPID = "";
                string sEQPModuleID = "";
                bool bSpecExist = false;

                ArrayList alParamRawIDList = new ArrayList();

                if (llstData[Definition.CONDITION_KEY_TYPE] != null)
                    type = (ParameterType)(llstData[Definition.CONDITION_KEY_TYPE]);

                if (llstData[Definition.CONDITION_KEY_EQP_MODULE_ID] != null)
                    sEQPModuleID = llstData[Definition.CONDITION_KEY_EQP_MODULE_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
                    sModuleRawid = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_SPEC_EXIST] != null)
                    bSpecExist = (Boolean)llstData[Definition.CONDITION_KEY_SPEC_EXIST];

                if (llstData[Definition.CONDITION_KEY_PARAM_RAWID_LIST] != null)
                    alParamRawIDList = (ArrayList)llstData[Definition.CONDITION_KEY_PARAM_RAWID_LIST];

                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                    sDCPID = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_DCP_RAWID] != null)
                    sDCPRawID = llstData[Definition.CONDITION_KEY_DCP_RAWID].ToString();


                switch (type)
                {
                    case ParameterType.EVENT_SUMMARY:
                        ds = GetEventParam(baData, bSpecExist);
                        break;
                    case ParameterType.TRACE_SUMMARY:
                        ds = GetSummaryParam(baData, bSpecExist);
                        break;
                    case ParameterType.TRACE:
                        ds = GetTraceParam(baData, alParamRawIDList, bSpecExist);
                        break;
                    case ParameterType.EVENT:
                        ds = GetEvent(baData, alParamRawIDList);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }



        private const string SQL_QUERY_TRACE_PARAM_SPEC_EXIST_1 =
           @"SELECT DISTINCT TO_CHAR(ETPMP.RAWID) RAWID, ETPMP.PARAM_NAME, ETPMP.PARAM_ALIAS, ETPMP.UNIT,
                   DECODE (SVT.RAWID, NULL, '', 'O') SPEC_EXIST,
                   DECODE (ETPMP.VIRTUAL_YN, 'Y', 'Y', 'N') VIRTUAL_YN,
                   DECODE (SVT.ACTIVATE_YN, 'Y', 'Y', 'N') ACTIVATE_YN, TO_CHAR(ETPMP.PARAM_NAME) PARAM_KEY,
                   emvt.module_name, emvt.rawid MODULE_RAWID, emvt.eqp_id 
              FROM EQP_TRACE_PARAM_MST_PP ETPMP
                   LEFT OUTER JOIN
                   (SELECT RAWID, EQP_TRACE_PARAM_RAWID, ACTIVATE_YN
                      FROM EQP_TRACE_SPEC_MST_FDC
                    ";

        private const string SQL_QUERY_TRACE_PARAM_SPEC_EXIST_2 =
            //WHERE EQP_RAWID = :EQP_RAWID
                    @" AND ACTIVATE_YN = 'Y'
                   ) SVT 
                    ON ETPMP.RAWID = SVT.EQP_TRACE_PARAM_RAWID
                   LEFT OUTER JOIN
                   (SELECT RAWID, EQP_DCP_RAWID FROM EQP_TRID_MST_PP) ETVT
                   ON ETVT.RAWID = ETPMP.EQP_TRID_RAWID
                   LEFT OUTER JOIN
                   (SELECT RAWID FROM EQP_DCP_MST_PP
                   ";
        //WHERE RAWID = :DCP_RAWID ) EDVT
        //ON EDVT.RAWID = ETVT.EQP_DCP_RAWID
        //LEFT OUTER JOIN
        // (SELECT rawid, module_name
        //    FROM eqp_mst_pp
        //   WHERE rawid = :eqp_rawid) emvt
        // ON emvt.rawid = etpmp.eqp_rawid

        //WHERE ETPMP.EQP_RAWID = :EQP_RAWID
        //AND EDVT.RAWID = :DCP_RAWID        ";

        private const string SQL_QUERY_TRACE_PARAM_SPEC_EXIST_BYEQP_1 =
            @"SELECT DISTINCT TO_CHAR(ETPMP.RAWID) RAWID, ETPMP.PARAM_NAME, ETPMP.UNIT, ETPMP.PARAM_TYPE_CD, ETPMP.PARAM_ALIAS,
                   DECODE (svt.eqp_module_id, NULL, '', 'O') SPEC_EXIST, ETPMP.DATA_TYPE_CD,
                   DECODE (ETPMP.VIRTUAL_YN, 'Y', 'Y', 'N') VIRTUAL_YN, TO_CHAR(ETPMP.PARAM_NAME) PARAM_KEY,
                   emvt.module_name, emvt.module_id, emvt.eqp_id, emvt.parent EQP_RAWID  
              FROM EQP_TRACE_PARAM_MST_PP ETPMP
                   LEFT OUTER JOIN
                   (SELECT eqp_module_id, eqp_param_alias
                      FROM EQP_TRACE_SPEC_MST_FDC
                    ";
        private const string SQL_QUERY_TRACE_PARAM_SPEC_EXIST_BYEQP_2 =
            //WHERE EQP_RAWID = :EQP_RAWID
                     @"AND ACTIVATE_YN = 'Y'
                   ) SVT 
                    ON etpmp.PARAM_ALIAS = svt.eqp_param_alias
                    ";
        //WHERE ETPMP.EQP_RAWID = :EQP_RAWID       

        private DataSet GetTraceParamSpecExist(byte[] baData, ArrayList alParamRawIDList)
        {
            DataSet ds = null;

            try
            {
                string sModuleRawid = "";
                string sDCPRawid = "";
                ArrayList alModuleRawid = null;

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sbQuery = new StringBuilder();
                LinkedList whereFieldData = new LinkedList();

                if (llstData[Definition.CONDITION_KEY_DCP_RAWID] != null
                   && llstData[Definition.CONDITION_KEY_DCP_RAWID].ToString() != string.Empty)
                {
                    sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_1);
                    sDCPRawid = llstData[Definition.CONDITION_KEY_DCP_RAWID].ToString();
                    if (sDCPRawid != string.Empty)
                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_RAWID, sDCPRawid);

                    if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null || llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                    {
                        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
                        {
                            sModuleRawid = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();
                        }
                        else if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
                        {
                            sModuleRawid = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
                        }

                        whereFieldData.Add(Definition.CONDITION_KEY_EQP_RAWID, sModuleRawid);
                        sbQuery.Append(" eqp_module_id = : module_id ");
                        sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_2);
                        sbQuery.Append(" WHERE RAWID = :DCP_RAWID ) EDVT    ON EDVT.RAWID = ETVT.EQP_DCP_RAWID  ");
                        sbQuery.Append(" LEFT OUTER JOIN   (SELECT rawid, eqp_id, module_name, parent  FROM eqp_mst_pp WHERE rawid = :eqp_rawid) emvt  ON emvt.rawid = etpmp.eqp_rawid ");
                        sbQuery.Append(" WHERE ETPMP.EQP_RAWID = :EQP_RAWID ");
                        sbQuery.Append(" AND EDVT.RAWID = :DCP_RAWID ");
                    }
                    else if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null || llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
                    {
                        string sModuleList = string.Empty;
                        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
                        {
                            alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST];
                            sModuleList = _commondata.MakeVariablesList(alModuleRawid);
                        }
                        else if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
                        {
                            alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
                            sModuleList = _commondata.MakeVariablesList(alModuleRawid);
                        }

                        string sSql = string.Format(" IN ({0}) ", sModuleList);

                        sbQuery.Append(" WHERE EQP_RAWID " + sSql);
                        sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_2);
                        sbQuery.Append(" WHERE RAWID = :DCP_RAWID ) EDVT    ON EDVT.RAWID = ETVT.EQP_DCP_RAWID  ");
                        sbQuery.Append(" LEFT OUTER JOIN   (SELECT rawid, eqp_id, module_name, parent  FROM eqp_mst_pp ");
                        sbQuery.Append(" WHERE rawid " + sSql + ") emvt  ON emvt.rawid = etpmp.eqp_rawid ");
                        sbQuery.Append(" WHERE ETPMP.EQP_RAWID " + sSql);
                        sbQuery.Append(" AND EDVT.RAWID = :DCP_RAWID ");
                    }
                }
                else
                {
                    sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_BYEQP_1);

                    if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                    {
                        string sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();

                        whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);

                        sbQuery.Append(" WHERE eqp_module_id = :module_id ");
                        sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_BYEQP_2);
                        //sbQuery.Append("WHERE ETPMP.EQP_RAWID = :EQP_RAWID ");
                        sbQuery.Append(" LEFT OUTER JOIN   (SELECT module_id, eqp_id, module_name, parent  FROM eqp_mst_pp WHERE module_id = :module_id) emvt  ON etpmp.EQP_MODULE_ID = emvt.module_id ");
                        sbQuery.Append(" WHERE etpmp.eqp_module_id = :module_id ");
                    }
                    else if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null || llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
                    {
                        string sModuleList = string.Empty;
                        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
                        {
                            alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST];
                            sModuleList = _commondata.MakeVariablesList(alModuleRawid);
                        }
                        else if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
                        {
                            alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
                            sModuleList = _commondata.MakeVariablesList(alModuleRawid);
                        }

                        string sSql = string.Format(" IN ({0}) ", sModuleList);

                        sbQuery.Append(" WHERE EQP_RAWID " + sSql);
                        sbQuery.Append(SQL_QUERY_TRACE_PARAM_SPEC_EXIST_BYEQP_2);
                        //sbQuery.Append(" WHERE ETPMP.EQP_RAWID " + sSql);
                        sbQuery.Append(" LEFT OUTER JOIN   (SELECT rawid, eqp_id, module_name, parent  FROM eqp_mst_pp ");
                        sbQuery.Append(" WHERE rawid " + sSql + ") emvt  ON emvt.rawid = etpmp.eqp_rawid ");
                        sbQuery.Append(" WHERE ETPMP.EQP_RAWID " + sSql);
                    }
                }

                sbQuery.Append(" AND (ETPMP.DISPLAY_YN IS NULL OR ETPMP.DISPLAY_YN = 'Y') ");
                sbQuery.Append("  AND ETPMP.DATA_TYPE_CD IN ('FLT', 'INT') ORDER BY ETPMP.PARAM_NAME           ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        private const string SQL_QUERY_TRACE_PARAM =
            @"SELECT DISTINCT TO_CHAR(ETPMP.RAWID) RAWID, ETPMP.PARAM_NAME, ETPMP.PARAM_ALIAS, ETPMP.UNIT, ETPMP.DATA_TYPE_CD,
                    DECODE(ETPMP.VIRTUAL_YN, 'Y', 'Y', 'N') VIRTUAL_YN, TO_CHAR(ETPMP.PARAM_NAME) PARAM_KEY, ETPMP.EQP_DCP_ID, ETPMP.DISPLAY_YN
              FROM EQP_TRACE_PARAM_MST_PP ETPMP 
                   LEFT OUTER JOIN
                       (SELECT RAWID, EQP_DCP_ID, EQP_ID, EQP_MODULE_ID FROM EQP_TRID_MST_PP) ETVT
                   ON ETVT.EQP_DCP_ID = ETPMP.EQP_DCP_ID AND ETVT.EQP_MODULE_ID = ETPMP.EQP_MODULE_ID
                   LEFT OUTER JOIN
                       (SELECT RAWID, DCP_ID, EQP_ID FROM EQP_DCP_MST_PP
                       WHERE DCP_ID = :DCP_RAWID ) EDVT
                   ON EDVT.DCP_ID = ETVT.EQP_DCP_ID
                   AND EDVT.EQP_ID = ETVT.EQP_ID
              WHERE  ETPMP.EQP_DCP_ID = :DCP_RAWID
              AND (ETPMP.DISPLAY_YN IS NULL OR ETPMP.DISPLAY_YN = 'Y')
              ";

        private const string SQL_QUERY_TRACE_PARAM_BYEQP =
            @"SELECT DISTINCT TO_CHAR(ETPMP.RAWID) RAWID, ETPMP.PARAM_NAME, ETPMP.PARAM_ALIAS, ETPMP.UNIT, ETPMP.DATA_TYPE_CD,
                    DECODE(ETPMP.VIRTUAL_YN, 'Y', 'Y', 'N') VIRTUAL_YN, ETPMP.DISPLAY_YN
              FROM EQP_TRACE_PARAM_MST_PP ETPMP 
              WHERE  1=1
              AND (ETPMP.DISPLAY_YN IS NULL OR ETPMP.DISPLAY_YN = 'Y')
             ";

        private DataSet GetTraceParam(byte[] baData, ArrayList alParamRawIDList)
        {
            DataSet ds = null;

            try
            {
                string sEQPModuleID = "";
                string sDCPRawid = "";
                ArrayList alEQPModuleIDList = null;

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sbQuery = new StringBuilder();
                LinkedList whereFieldData = new LinkedList();


                if (llstData[Definition.CONDITION_KEY_DCP_RAWID] != null
                        && llstData[Definition.CONDITION_KEY_DCP_RAWID].ToString() != string.Empty)
                    sbQuery.Append(SQL_QUERY_TRACE_PARAM);
                else
                    sbQuery.Append(SQL_QUERY_TRACE_PARAM_BYEQP);

                if (llstData[Definition.CONDITION_KEY_EQP_MODULE_ID] != null)
                {
                    sEQPModuleID = llstData[Definition.CONDITION_KEY_EQP_MODULE_ID].ToString();
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODULE_ID, sEQPModuleID);
                    sbQuery.Append(" AND ETPMP.EQP_MODULE_ID = :EQP_MODULE_ID ");
                }
                else if (llstData[Definition.CONDITION_KEY_EQP_MODULE_ID_LIST] != null)
                {
                    alEQPModuleIDList = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_MODULE_ID_LIST];
                    string sEQPModuleIDList = _commondata.MakeVariablesList(alEQPModuleIDList);

                    string sSql = string.Format(" AND ETPMP.EQP_MODULE_ID IN ({0}) ", sEQPModuleIDList);
                    sbQuery.Append(sSql);
                }

                if (alParamRawIDList.Count > 0)
                {
                    CommonData commonData = new CommonData();
                    string sParamRawIDList = commonData.MakeVariablesList(alParamRawIDList);

                    sbQuery.Append(string.Format(" AND ETPMP.PARAM_ALIAS IN ({0}) ", sParamRawIDList));
                }

                if (llstData[Definition.CONDITION_KEY_DCP_RAWID] != null)
                {
                    sDCPRawid = llstData[Definition.CONDITION_KEY_DCP_RAWID].ToString();
                    if (sDCPRawid != string.Empty)
                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_RAWID, sDCPRawid);
                }

                //sbQuery.Append("  AND ETPMP.DATA_TYPE_CD IN ('FLT', 'INT')  ");
                sbQuery.Append("    ORDER BY ETPMP.PARAM_ALIAS           ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private DataSet GetTraceParam(byte[] baData, ArrayList alParamRawIDList, bool bSpecExist)
        {
            DataSet ds = null;

            try
            {
                if (bSpecExist)
                {
                    ds = GetTraceParamSpecExist(baData, alParamRawIDList);
                }
                else
                {
                    ds = GetTraceParam(baData, alParamRawIDList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_SUMMARY_PARAM =
@"
SELECT   a.eqp_module_id, a.eqp_dcp_id, a.eqp_param_alias AS param_alias
    FROM eqp_sum_param_mst_pp a
   WHERE 1=1
";

        private DataSet GetSummaryParam(byte[] baData, bool bSpecExist)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_SUMMARY_PARAM);

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null || llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    string sModuleID = string.Empty;
                    if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                    {
                        sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    }
                    else if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    {
                        sModuleID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                    }

                    if (sModuleID != string.Empty && sModuleID != "MAIN")
                    {
                        sbQuery.Append(string.Format(" AND a.eqp_module_id = :module_id  ", sModuleID));
                        whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                    }
                }
                else if (llstData[Definition.CONDITION_KEY_EQP_ID_LIST] != null || llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                {
                    ArrayList alModuleIDList = null;
                    if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                    {
                        alModuleIDList = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];
                    }
                    else if (llstData[Definition.CONDITION_KEY_EQP_ID_LIST] != null)
                    {
                        alModuleIDList = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_ID_LIST];
                    }

                    if (alModuleIDList != null)
                    {
                        string sModuleIDList = _commondata.MakeVariablesList(alModuleIDList);
                        sbQuery.Append(string.Format(" AND a.eqp_module_id IN ({0}) ", sModuleIDList));
                    }
                }

                //if (llstData[Definition.CONDITION_KEY_RECIPE_RAWID] != null)
                //{
                //    string sRecipeRawid = llstData[Definition.CONDITION_KEY_RECIPE_RAWID].ToString();
                //    if (sRecipeRawid != string.Empty)
                //    {
                //        sbQuery.Append(" AND ES.EQP_RECIPE_RAWID = :RECIPE_RAWID ");
                //        whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_RAWID, sRecipeRawid);
                //    }
                //}
                //else if (llstData[Definition.CONDITION_KEY_RECIPE_RAWID_LIST] != null)
                //{
                //    ArrayList alRecipeRawid = (ArrayList)llstData[Definition.CONDITION_KEY_RECIPE_RAWID_LIST];
                //    string sRecipeRawidList = _commondata.MakeVariablesList(alRecipeRawid);

                //    sbQuery.Append(string.Format(" AND ES.EQP_RECIPE_RAWID IN ({0}) ", sRecipeRawidList));
                //}

                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                {
                    string sDcpID = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();
                    if (sDcpID != string.Empty)
                    {
                        sbQuery.Append(" AND a.eqp_dcp_id = :dcp_id    ");
                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDcpID);
                    }
                }

                //sbQuery.Append("  AND ES.DATA_TYPE_CD IN ('FLT', 'INT')");
                sbQuery.Append("    group by a.eqp_module_id, a.eqp_dcp_id, a.eqp_param_alias           ");
                sbQuery.Append("    ORDER BY a.eqp_param_alias           ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        private const string SQL_QUERY_EVENT =
            @"SELECT TO_CHAR(RAWID) AS RAWID, EVENT_NAME AS PARAM_NAME
              FROM eqp_event_mst_pp a, eqp_event_param_mst_pp b
 WHERE a.eqp_module_id = b.eqp_module_id
   AND a.eqp_dcp_id = b.eqp_dcp_id
   AND a.event_id = b.eqp_event_id ";

        private DataSet GetEvent(byte[] baData, ArrayList alParamRawIDList)
        {
            DataSet ds = null;

            try
            {
                string sModuleid = "";

                ArrayList alModuleid = null;

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sbQuery = new StringBuilder();
                LinkedList whereFieldData = new LinkedList();

                sbQuery.Append(SQL_QUERY_EVENT);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sModuleid = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleid);
                    sbQuery.Append(" and eqp_module_id = :module_id ");
                }
                else if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                {
                    alModuleid = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];
                    string sModuleList = _commondata.MakeVariablesList(alModuleid);

                    string sSql = string.Format(" and eqp_module_id IN ({0}) ", sModuleList);
                    sbQuery.Append(sSql);
                }

                if (alParamRawIDList.Count > 0)
                {
                    CommonData commonData = new CommonData();
                    string sParamRawIDList = commonData.MakeVariablesList(alParamRawIDList);

                    sbQuery.Append(string.Format(" AND param_alias IN ({0}) ", sParamRawIDList));
                }

                sbQuery.Append("  ORDER BY EVENT_NAME           ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        //        private const string SQL_QUERY_EVENT_PARAM =
        //            @"SELECT TO_CHAR(EP.RAWID) AS RAWID, EP.PARAM_NAME,
        //            DECODE (EP.VIRTUAL_YN, 'Y', 'Y', 'N') VIRTUAL_YN 
        //                    FROM EQP_EVENT_PARAM_MST_PP EP
        //                    JOIN EQP_EVENT_MST_PP EM
        //                    ON EM.RAWID = EP.EQP_EVENT_RAWID
        //                    WHERE  ";

        //        private DataSet GetEventParam(byte[] baData, bool bSpecExist)
        //        {
        //            DataSet ds = null;

        //            try
        //            {
        //                string sModuleRawid = "";
        //                ArrayList alModuleRawid = null;

        //                LinkedList llstData = new LinkedList();
        //                llstData.SetSerialData(baData);

        //                LinkedList whereFieldData = new LinkedList();
        //                StringBuilder sbQuery = new StringBuilder();
        //                sbQuery.Append(SQL_QUERY_EVENT_PARAM);

        //                if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null || llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
        //                {
        //                    if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
        //                    {
        //                        sModuleRawid = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();
        //                    }
        //                    else if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
        //                    {
        //                        sModuleRawid = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
        //                    }

        //                    if (sModuleRawid != string.Empty && sModuleRawid != "MAIN")
        //                    {
        //                        sbQuery.Append(string.Format(" EM.EQP_RAWID = :EQP_RAWID ", sModuleRawid));
        //                        whereFieldData.Add(Definition.CONDITION_KEY_EQP_RAWID, Convert.ToDouble(sModuleRawid));
        //                    }
        //                }
        //                else if(llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null || llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
        //                {
        //                    if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
        //                    {
        //                        alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST];
        //                    }
        //                    else if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
        //                    {
        //                        alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
        //                    }

        //                    if (alModuleRawid != null)
        //                    {
        //                        sModuleRawid = _commondata.MakeVariablesList(alModuleRawid);
        //                        sbQuery.Append(string.Format(" EM.EQP_RAWID IN ({0}) ", sModuleRawid));
        //                    }
        //                }

        //                sbQuery.Append("  AND EP.DATA_TYPE_CD IN ('FLT', 'INT')     ORDER BY EP.PARAM_NAME           ");
        //                ds = base.Query(sbQuery.ToString(), whereFieldData);
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine(ex);
        //            }

        //            return ds;
        //        }

        private const string SQL_QUERY_EVENT_PARAM =
            //@"
            //SELECT a.eqp_module_id, a.eqp_dcp_id, a.param_alias
            //  FROM eqp_event_param_mst_pp a RIGHT OUTER JOIN eqp_event_mst_pp b
            //       ON a.eqp_event_id = b.event_id
            //     AND a.eqp_module_id = b.eqp_module_id
            //     AND a.eqp_dcp_id = b.eqp_dcp_id
            // WHERE a.param_alias IS NOT NULL
            //";

        @"SELECT DISTINCT A.EQP_MODULE_ID, A.EQP_DCP_ID, A.SUM_GROUP_NAME AS PARAM_ALIAS 
          FROM EQP_EVENT_PARAM_MST_PP A
         WHERE A.SUM_GROUP_NAME IS NOT NULL ";

        private DataSet GetEventParam(byte[] baData, bool bSpecExist)
        {
            DataSet ds = null;

            try
            {
                string sModuleID = string.Empty;
                string sDcpID = string.Empty;
                ArrayList alModuleID = null;

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_EVENT_PARAM);

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                {
                    sModuleID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();
                    if (sModuleID != string.Empty && sModuleID != "MAIN")
                    {
                        sbQuery.Append(string.Format(" AND a.eqp_module_id = :module_id     ", sModuleID));
                        whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, sModuleID);
                    }
                }
                else if (llstData[Definition.CONDITION_KEY_MODULE_ID_LIST] != null)
                {
                    alModuleID = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_ID_LIST];
                    string sModuleIDList = _commondata.MakeVariablesList(alModuleID);

                    sbQuery.Append(string.Format(" AND a.eqp_module_id IN ({0})     ", sModuleIDList));
                }

                if (llstData[Definition.CONDITION_KEY_DCP_ID] != null)
                {
                    sDcpID = llstData[Definition.CONDITION_KEY_DCP_ID].ToString();
                    if (sDcpID != string.Empty)
                    {
                        sbQuery.Append("    AND a.eqp_dcp_id = :dcp_id   ");
                        whereFieldData.Add(Definition.CONDITION_KEY_DCP_ID, sDcpID);
                    }
                }

                //sbQuery.Append("    GROUP BY a.eqp_module_id, a.eqp_dcp_id, a.param_alias           ");
                //sbQuery.Append("    ORDER BY a.param_alias           ");
                sbQuery.Append("    ORDER BY A.SUM_GROUP_NAME           ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_MULTIVARIATE_MODEL =
            @"SELECT * FROM EQP_MVA_MST_FDC
                WHERE 1=1   ";

        public DataSet GetMultivariateModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                string sProductID = "";
                string sRecipeID = "";
                string sRecipeRawID = "";
                string sStepID = "";
                string sStepRawID = "";
                string sModuleRawid = "";
                ArrayList alModuleRawid = null;

                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(SQL_QUERY_MULTIVARIATE_MODEL);

                if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
                {
                    sModuleRawid = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();
                    if (sModuleRawid != string.Empty && sModuleRawid != "MAIN")
                    {
                        sbQuery.Append(string.Format(" AND EQP_MODULE_ID = :EQP_MODULE_ID ", sModuleRawid));
                        whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODULE_ID, sModuleRawid);
                    }
                }
                else if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
                {
                    alModuleRawid = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST];
                    sModuleRawid = _commondata.MakeVariablesList(alModuleRawid);

                    sbQuery.Append(string.Format(" AND EQP_RAWID IN ({0}) ", sModuleRawid));
                }

                if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
                {
                    sRecipeID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();
                    if (sRecipeID != string.Empty && sRecipeID != "ALL")
                    {
                        sbQuery.Append(string.Format(" AND EQP_RECIPE_ID = :RECIPE_ID ", sRecipeID));
                        whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_ID, sRecipeID);
                    }
                }

                if (llstData[Definition.CONDITION_KEY_RECIPE_RAWID] != null)
                {
                    sRecipeRawID = llstData[Definition.CONDITION_KEY_RECIPE_RAWID].ToString();
                    if (sRecipeRawID.Length > 0)
                    {
                        sbQuery.Append(string.Format(" AND EQP_RECIPE_RAWID = :RECIPE_RAWID ", sRecipeRawID));
                        whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_RAWID, Convert.ToDouble(sRecipeRawID));
                    }
                }

                if (llstData[Definition.CONDITION_KEY_STEP_ID] != null)
                {
                    sStepID = llstData[Definition.CONDITION_KEY_STEP_ID].ToString();
                    if (sStepID != string.Empty && sStepID != "ALL")
                    {
                        sbQuery.Append(string.Format(" AND EQP_STEP_ID = :STEP_ID ", sStepID));
                        whereFieldData.Add(Definition.CONDITION_KEY_STEP_ID, sStepID);
                    }
                }

                if (llstData[Definition.CONDITION_KEY_STEP_RAWID] != null)
                {
                    sStepRawID = llstData[Definition.CONDITION_KEY_STEP_RAWID].ToString();
                    if (sStepRawID.Length > 0)
                    {
                        sbQuery.Append(string.Format(" AND EQP_RECIPE_STEP_RAWID = :STEP_RAWID ", sStepRawID));
                        whereFieldData.Add(Definition.CONDITION_KEY_STEP_RAWID, Convert.ToDouble(sStepRawID));
                    }
                }

                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                {
                    sProductID = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();
                    if (sProductID != string.Empty && sProductID != "ALL")
                    {
                        sbQuery.Append(string.Format(" AND PRODUCT_ID = :PRODUCT_ID ", sProductID));
                        whereFieldData.Add(Definition.CONDITION_KEY_PRODUCT_ID, sProductID);
                    }
                }

                sbQuery.Append("  ORDER BY MVA_NAME           ");
                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Lot List Condition

        private const string SQL_QUERY_LOT_LIST =
            @"SELECT LOT_ID            
                FROM EQP_TRACE_TRX_FDC
                WHERE START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')
                AND END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')
                GROUP BY LOT_ID
                ";

        public DataSet GetLotIDList(byte[] btdata)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string sStartDate = string.Empty;
                string sEndDate = string.Empty;

                DateTime dtStart = (DateTime)llstData[Definition.CONDITION_KEY_START_DATE];
                DateTime dtEnd = (DateTime)llstData[Definition.CONDITION_KEY_END_DATE];

                sStartDate = dtStart.ToString(Definition.DATETIME_FORMAT);
                sEndDate = dtEnd.ToString(Definition.DATETIME_FORMAT);

                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, sStartDate);
                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, sEndDate);

                ds = base.Query(SQL_QUERY_LOT_LIST, whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        //        private const string SQL_QUERY_TRACE_SUBSTRATE_HISTORY =
        //@"
        //SELECT ETT.RAWID, ETT.EQP_RAWID, ETT.LOT_ID, ETT.RECIPE_ID, ETT.PPID, TO_CHAR(ETT.START_DTTS, 'YYYY-MM-DD HH24:MI:SS') START_DTTS,
        //            TO_CHAR(ETT.END_DTTS, 'YYYY-MM-DD HH24:MI:SS') END_DTTS, ETT.TRACE_WARNING_COUNT, ETT.TRACE_FAULT_COUNT, ETT.MSPC_FAULT_COUNT,
        //            ETT.SAMPLE_COUNT, EMP.MODULE_NAME, EMP.EQP_ID, ETT.PRODUCT_ID, ETT.RECIPE_ID, ETT.START_DTTS REAL_START_DTTS, ETT.END_dTTS REAL_END_DTTS,
        //            ETST.SUBSTRATE_ID, ETST.STEP_ID
        //FROM EQP_TRACE_TRX_PP ETT, EQP_TRACE_SLOT_TRX_PP ETST, EQP_MST_PP EMP
        //WHERE EMP.RAWID = ETT.EQP_RAWID
        //AND ETT.RAWID = ETST.EQP_TRACE_RAWID
        //";

        //public DataSet GetTraceSubstrateHistory(byte[] btdata)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(btdata);

        //        string start = "";
        //        string end = "";

        //        DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
        //        DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];
        //        DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
        //        DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];
        //        DataTable dtRecipeID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_RECIPE];

        //        DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        start = dateFrom.ToString(Definition.DATETIME_FORMAT);
        //        DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        end = dateTo.ToString(Definition.DATETIME_FORMAT);

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();

        //        sbQuery.Append(SQL_QUERY_TRACE_SUBSTRATE_HISTORY);

        //        CommonUtility util = new CommonUtility();

        //        if (dtModuleID != null)
        //        {
        //            string sModuleList = util.ConvertDataColumnIntoStringList(dtModuleID, Definition.CONDITION_SEARCH_KEY_VALUEDATA);
        //            if (sModuleList != string.Empty)
        //            {
        //                string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sModuleList);
        //                sbQuery.Append(sSql);
        //            }
        //        }
        //        else if (dtEqpID != null)
        //        {
        //            string sEqpList = util.ConvertDataColumnIntoStringList(dtEqpID, Definition.CONDITION_SEARCH_KEY_VALUEDATA);
        //            if (sEqpList != string.Empty)
        //            {
        //                string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sEqpList);
        //                sbQuery.Append(sSql);
        //            }
        //        }

        //        if (dtRecipeID != null)
        //        {
        //            string sRecipeList = util.ConvertDataColumnIntoStringList(dtRecipeID, Definition.CONDITION_SEARCH_KEY_DISPLAYDATA, "'", ",");
        //            if (sRecipeList != null)
        //            {
        //                string sSql = string.Format(" AND ETT.RECIPE_ID IN ({0}) ", sRecipeList);
        //                sbQuery.Append(sSql);
        //            }
        //        }

        //        sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //        sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //        whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //        whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);
        //        sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //}

        //        private const string SQL_QUERY_TRACE_LOTHISTORY =
        //         @"SELECT ETT.RAWID, ETT.EQP_RAWID, ETT.LOT_ID, ETT.RECIPE_ID, ETT.PPID, TO_CHAR(ETT.START_DTTS, 'YYYY-MM-DD HH24:MI:SS') START_DTTS,
        //            TO_CHAR(ETT.END_DTTS, 'YYYY-MM-DD HH24:MI:SS') END_DTTS, ETT.TRACE_WARNING_COUNT, ETT.TRACE_FAULT_COUNT, ETT.MSPC_FAULT_COUNT,
        //            ETT.SAMPLE_COUNT, EMP.MODULE_NAME, EMP.EQP_ID, ETT.PRODUCT_ID, ETT.RECIPE_ID, ETT.START_DTTS REAL_START_DTTS, ETT.END_dTTS REAL_END_DTTS
        //            FROM EQP_TRACE_TRX_PP ETT 
        //                JOIN EQP_MST_PP EMP
        //                ON EMP.RAWID = ETT.EQP_RAWID   ";

        //        public DataSet GetTraceLotHistory(byte[] btdata)
        //        {
        //            DataSet ds = null;

        //            try
        //            {
        //                LinkedList llstData = new LinkedList();
        //                llstData.SetSerialData(btdata);

        //                string start = "";
        //                string end = "";
        //                ArrayList alModule = new ArrayList();
        //                ArrayList alEqp = new ArrayList();
        //                ArrayList alRecipe = new ArrayList();

        //                DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
        //                DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];
        //                DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
        //                DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];
        //                DataTable dtRecipeID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_RECIPE];

        //                if (dtEqpID != null)
        //                {
        //                    for (int i = 0; i < dtEqpID.Rows.Count; i++)
        //                    {
        //                        string rawid = dtEqpID.Rows[i][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //                        alEqp.Add(rawid);
        //                    }
        //                }

        //                if (dtModuleID != null)
        //                {
        //                    for (int i = 0; i < dtModuleID.Rows.Count; i++)
        //                    {
        //                        string rawid = dtModuleID.Rows[i][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //                        alModule.Add(rawid);
        //                    }
        //                }

        //                if (dtRecipeID != null)
        //                {
        //                    for (int i = 0; i < dtRecipeID.Rows.Count; i++)
        //                    {
        //                        string sRecipeID = dtRecipeID.Rows[i][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
        //                        alRecipe.Add(sRecipeID);
        //                    }
        //                }

        //                DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //                start = dateFrom.ToString(Definition.DATETIME_FORMAT);
        //                DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //                end = dateTo.ToString(Definition.DATETIME_FORMAT);

        //                LinkedList whereFieldData = new LinkedList();
        //                StringBuilder sbQuery = new StringBuilder();
        //                sbQuery.Append(SQL_QUERY_TRACE_LOTHISTORY);

        //                if (alModule != null && alModule.Count > 0)
        //                {
        //                    string sModuleList = _commondata.MakeVariablesList(alModule);
        //                    string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sModuleList);
        //                    sbQuery.Append(sSql);
        //                }
        //                else if (alEqp != null && alEqp.Count > 0)
        //                {
        //                    string sEqpList = _commondata.MakeVariablesList(alEqp);
        //                    string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sEqpList);
        //                    sbQuery.Append(sSql);
        //                }

        //                if (alRecipe != null && alRecipe.Count > 0)
        //                {
        //                    string sRecipeList = _commondata.MakeVariablesList(alRecipe);
        //                    string sSql = string.Format(" AND ETT.RECIPE_ID IN ({0}) ", sRecipeList);
        //                    sbQuery.Append(sSql);
        //                }

        //                sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //                sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);
        //                sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //                ds = base.Query(sbQuery.ToString(), whereFieldData);

        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine(ex);
        //            }

        //            return ds;
        //        }


        //        private const string SQL_QUERY_TRACE_SUBSTRATE_HISTORY =
        //@"
        //SELECT ETT.RAWID, ETT.EQP_RAWID, ETT.LOT_ID, ETT.RECIPE_ID, ETT.PPID, TO_CHAR(ETT.START_DTTS, 'YYYY-MM-DD HH24:MI:SS') START_DTTS,
        //            TO_CHAR(ETT.END_DTTS, 'YYYY-MM-DD HH24:MI:SS') END_DTTS, ETT.TRACE_WARNING_COUNT, ETT.TRACE_FAULT_COUNT, ETT.MSPC_FAULT_COUNT,
        //            ETT.SAMPLE_COUNT, EMP.MODULE_NAME, EMP.EQP_ID, ETT.PRODUCT_ID, ETT.RECIPE_ID, ETT.START_DTTS REAL_START_DTTS, ETT.END_dTTS REAL_END_DTTS,
        //            ETST.SUBSTRATE_ID, ETST.STEP_ID
        //FROM EQP_TRACE_TRX_PP ETT, EQP_TRACE_SLOT_TRX_PP ETST, EQP_MST_PP EMP
        //WHERE EMP.RAWID = ETT.EQP_RAWID
        //AND ETT.RAWID = ETST.EQP_TRACE_RAWID
        //";


        //public DataSet GetTraceLotHistory(byte[] btdata)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(btdata);

        //        string eqp = "";
        //        string cmbr = "";
        //        string start = "";
        //        string end = "";
        //        string sRecipeID = "";
        //        ArrayList alModule = null;
        //        ArrayList alEqp = null;

        //        if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
        //            eqp = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST] != null)
        //            alEqp = (ArrayList)llstData[Definition.CONDITION_KEY_EQP_RAWID_LIST];
        //        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
        //            cmbr = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST] != null)
        //            alModule = (ArrayList)llstData[Definition.CONDITION_KEY_MODULE_RAWID_LIST];
        //        if (llstData[Definition.CONDITION_KEY_START_DATE] != null)
        //            start = llstData[Definition.CONDITION_KEY_START_DATE].ToString();
        //        if (llstData[Definition.CONDITION_KEY_END_DATE] != null)
        //            end = llstData[Definition.CONDITION_KEY_END_DATE].ToString();
        //        if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
        //            sRecipeID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();
        //        sbQuery.Append(SQL_QUERY_TRACE_LOTHISTORY);

        //        if ((cmbr != string.Empty && cmbr != "ALL") || (alModule != null && alModule.Count > 0))
        //        {
        //            if (alModule == null)
        //            {
        //                whereFieldData.Add(Definition.CONDITION_KEY_MODULE_RAWID, Convert.ToDouble(cmbr));
        //                sbQuery.Append("    AND EMP.RAWID = :MODULE_RAWID                 ");
        //            }
        //            else
        //            {
        //                string sModuleList = _commondata.MakeVariablesList(alModule);
        //                string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sModuleList);
        //                sbQuery.Append(sSql);
        //            }
        //        }
        //        else if ((eqp != string.Empty && eqp != "ALL") || (alEqp != null && alEqp.Count > 0))
        //        {
        //            if (alEqp == null)
        //            {
        //                whereFieldData.Add(Definition.CONDITION_KEY_EQP_RAWID, Convert.ToDouble(eqp));
        //                sbQuery.Append("    AND EMP.RAWID = :EQP_RAWID                ");
        //            }
        //            else
        //            {
        //                string sEqpList = _commondata.MakeVariablesList(alEqp);
        //                string sSql = string.Format(" AND EMP.RAWID IN ({0}) ", sEqpList);
        //                sbQuery.Append(sSql);
        //            }
        //        }

        //        if (sRecipeID != string.Empty)
        //        {
        //            whereFieldData.Add(Definition.CONDITION_KEY_RECIPE_ID, sRecipeID);
        //            sbQuery.Append(" AND ETT.RECIPE_ID = :RECIPE_ID  ");
        //        }


        //        sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //        sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //        whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //        whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);
        //        sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //} 

        //        private const string SQL_QUERY_TRACE_SUBSTRATE =
        //@"
        //SELECT ETT.RAWID, ETT.EQP_RAWID, ETT.LOT_ID, ETT.RECIPE_ID, ETT.PPID, TO_CHAR(ETT.START_DTTS, 'YYYY-MM-DD HH24:MI:SS') START_DTTS,
        //            TO_CHAR(ETT.END_DTTS, 'YYYY-MM-DD HH24:MI:SS') END_DTTS, ETT.TRACE_WARNING_COUNT, ETT.TRACE_FAULT_COUNT, ETT.MSPC_FAULT_COUNT,
        //            ETT.SAMPLE_COUNT, EMP.MODULE_NAME, EMP.EQP_ID, ETT.PRODUCT_ID, ETT.START_DTTS REAL_START_DTTS, ETT.END_dTTS REAL_END_DTTS,
        //            ETST.SUBSTRATE_ID, ETST.STEP_ID
        //FROM EQP_TRACE_TRX_PP ETT, EQP_TRACE_SLOT_TRX_PP ETST, EQP_MST_PP EMP
        //WHERE EMP.RAWID = ETT.EQP_RAWID
        //AND ETT.RAWID = ETST.EQP_TRACE_RAWID
        //";

        //public DataSet GetTraceSubstrateHistoryByLot(byte[] btdata)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(btdata);

        //        string lot = "";
        //        string start = "";
        //        string end = "";
        //        string lineRawid = "";
        //        string areaRawid = "";
        //        string eqpModel = "";
        //        string eqpRawid = "";
        //        string eqpID = "";
        //        string moduleRawid = "";

        //        DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
        //        DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];

        //        DataTable dtLot = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LOT];
        //        DataTable dtLine = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LINE];
        //        DataTable dtArea = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_AREA];
        //        DataTable dtEqpModel = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
        //        DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
        //        DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];

        //        if (dtLot != null)
        //            lot = dtLot.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
        //        if (dtLine != null)
        //            lineRawid = dtLine.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtArea != null)
        //            areaRawid = dtArea.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtEqpModel != null)
        //            eqpModel = dtEqpModel.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtEqpID != null)
        //        {
        //            eqpRawid = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //            eqpID = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
        //        }
        //        if (dtModuleID != null)
        //            moduleRawid = dtModuleID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();

        //        DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        start = dateFrom.ToString(Definition.DATETIME_FORMAT);
        //        DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        end = dateTo.ToString(Definition.DATETIME_FORMAT);

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();

        //        sbQuery.Append(SQL_QUERY_TRACE_SUBSTRATE);

        //        if (eqpID != string.Empty && moduleRawid == string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_ID = :EQP_ID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
        //        }
        //        if (eqpRawid != string.Empty && moduleRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND ETT.EQP_RAWID = :MODULE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_MODULE_RAWID, moduleRawid);
        //        }
        //        if (lineRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.LOCATION_RAWID = :LINE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
        //        }
        //        if (areaRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.AREA_RAWID = :AREA_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
        //        }
        //        if (eqpModel != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_MODEL = :EQP_MODEL ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
        //        }

        //        sbQuery.Append("    AND ETT.LOT_ID = :LOT_ID                 ");
        //        sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //        sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //        sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //        whereFieldData.Add(Definition.CONDITION_KEY_LOT_ID, lot);
        //        whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //        whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //}





        //        private const string SQL_QUERY_TRACE_LOT =
        //        @"SELECT ETT.RAWID, ETT.EQP_RAWID, ETT.LOT_ID, ETT.RECIPE_ID, ETT.PPID, TO_CHAR(ETT.START_DTTS, 'YYYY-MM-DD HH24:MI:SS') START_DTTS,
        //            TO_CHAR(ETT.END_DTTS, 'YYYY-MM-DD HH24:MI:SS') END_DTTS, ETT.TRACE_WARNING_COUNT, ETT.TRACE_FAULT_COUNT, ETT.MSPC_FAULT_COUNT,
        //            ETT.SAMPLE_COUNT, EMP.MODULE_NAME, EMP.EQP_ID, ETT.PRODUCT_ID, ETT.START_DTTS REAL_START_DTTS, ETT.END_dTTS REAL_END_DTTS
        //            FROM EQP_TRACE_TRX_PP ETT 
        //                JOIN EQP_MST_PP EMP
        //                ON EMP.RAWID = ETT.EQP_RAWID   ";
        //        //WHERE EMP.MODULE_ID != -1       ";

        //public DataSet GetTraceLotHistoryByLot(byte[] btdata)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(btdata);

        //        string lot = "";
        //        string start = "";
        //        string end = "";
        //        string lineRawid = "";
        //        string areaRawid = "";
        //        string eqpModel = "";
        //        string eqpRawid = "";
        //        string eqpID = "";
        //        string moduleRawid = "";

        //        DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
        //        DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];

        //        DataTable dtLot = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LOT];
        //        DataTable dtLine = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LINE];
        //        DataTable dtArea = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_AREA];
        //        DataTable dtEqpModel = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
        //        DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
        //        DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];

        //        if(dtLot != null)
        //            lot = dtLot.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
        //        if (dtLine != null)
        //            lineRawid = dtLine.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtArea != null)
        //            areaRawid = dtArea.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtEqpModel != null)
        //            eqpModel = dtEqpModel.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //        if (dtEqpID != null)
        //        {
        //            eqpRawid = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
        //            eqpID = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
        //        }
        //        if (dtModuleID != null)
        //            moduleRawid = dtModuleID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();

        //        DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        start = dateFrom.ToString(Definition.DATETIME_FORMAT);
        //        DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
        //        end = dateTo.ToString(Definition.DATETIME_FORMAT);

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();

        //        sbQuery.Append(SQL_QUERY_TRACE_LOT);

        //        if (eqpID != string.Empty && moduleRawid == string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_ID = :EQP_ID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
        //        }
        //        if (eqpRawid != string.Empty && moduleRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND ETT.EQP_RAWID = :MODULE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_MODULE_RAWID, moduleRawid);
        //        }
        //        if (lineRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.LOCATION_RAWID = :LINE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
        //        }
        //        if (areaRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.AREA_RAWID = :AREA_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
        //        }
        //        if (eqpModel != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_MODEL = :EQP_MODEL ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
        //        }

        //        sbQuery.Append("    AND ETT.LOT_ID = :LOT_ID                 ");
        //        sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //        sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //        sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //        whereFieldData.Add(Definition.CONDITION_KEY_LOT_ID, lot);
        //        whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //        whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //}






        //public DataSet GetTraceLotHistoryByLot(byte[] btdata)
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        LinkedList llstData = new LinkedList();
        //        llstData.SetSerialData(btdata);

        //        string lot = "";
        //        string start = "";
        //        string end = "";
        //        string line = "";
        //        string area = "";
        //        string eqpModel = "";
        //        string eqpRawid = "";
        //        string eqpID = "";
        //        string moduleRawid = "";

        //        if (llstData[Definition.CONDITION_KEY_LOT_ID] != null)
        //            lot = llstData[Definition.CONDITION_KEY_LOT_ID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_START_DATE] != null)
        //            start = llstData[Definition.CONDITION_KEY_START_DATE].ToString();
        //        if (llstData[Definition.CONDITION_KEY_END_DATE] != null)
        //            end = llstData[Definition.CONDITION_KEY_END_DATE].ToString();
        //        if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
        //            line = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
        //            area = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
        //            eqpModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
        //        if (llstData[Definition.CONDITION_KEY_EQP_RAWID] != null)
        //            eqpRawid = llstData[Definition.CONDITION_KEY_EQP_RAWID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
        //            eqpID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
        //        if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
        //            moduleRawid = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();

        //        LinkedList whereFieldData = new LinkedList();
        //        StringBuilder sbQuery = new StringBuilder();

        //        sbQuery.Append(SQL_QUERY_TRACE_LOT);

        //        if (eqpID != string.Empty && moduleRawid == string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_ID = :EQP_ID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
        //        }
        //        if (eqpRawid != string.Empty && moduleRawid != string.Empty)
        //        {
        //            sbQuery.Append("    AND ETT.EQP_RAWID = :MODULE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_MODULE_RAWID, moduleRawid);
        //        }
        //        if (line != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.LOCATION_RAWID = :LINE_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, line);
        //        }
        //        if (area != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.AREA_RAWID = :AREA_RAWID ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, area);
        //        }
        //        if (eqpModel != string.Empty)
        //        {
        //            sbQuery.Append("    AND EMP.EQP_MODEL = :EQP_MODEL ");
        //            whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
        //        }

        //        sbQuery.Append("    AND ETT.LOT_ID = :LOT_ID                 ");
        //        sbQuery.Append("    AND ETT.START_DTTS >= TO_TIMESTAMP ( :START_DATE, 'YYYY-MM-DD HH24:MI:SS')  ");
        //        sbQuery.Append("    AND ETT.END_DTTS <= TO_DATE ( :END_DATE, 'YYYY-MM-DD HH24:MI:SS')      ");
        //        sbQuery.Append("  ORDER BY ETT.START_DTTS DESC          ");

        //        whereFieldData.Add(Definition.CONDITION_KEY_LOT_ID, lot);
        //        whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
        //        whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

        //        ds = base.Query(sbQuery.ToString(), whereFieldData);

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    return ds;
        //}





        public DataSet GetTraceLotHistory(byte[] btdata)
        {
            StringBuilder sbQuery = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();

            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string start = "";
                string end = "";
                ArrayList alModule = new ArrayList();
                ArrayList alEqp = new ArrayList();
                ArrayList alRecipe = new ArrayList();

                DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
                DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];
                DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
                DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];
                DataTable dtRecipeID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_RECIPE];

                if (dtEqpID != null)
                {
                    for (int i = 0; i < dtEqpID.Rows.Count; i++)
                    {
                        string rawid = dtEqpID.Rows[i][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                        alEqp.Add(rawid);
                    }
                }

                if (dtModuleID != null)
                {
                    for (int i = 0; i < dtModuleID.Rows.Count; i++)
                    {
                        string rawid = dtModuleID.Rows[i][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                        alModule.Add(rawid);
                    }
                }

                if (dtRecipeID != null)
                {
                    for (int i = 0; i < dtRecipeID.Rows.Count; i++)
                    {
                        string sRecipeID = dtRecipeID.Rows[i][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                        alRecipe.Add(sRecipeID);
                    }
                }

                DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                start = dateFrom.ToString(Definition.DATETIME_FORMAT);
                DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                end = dateTo.ToString(Definition.DATETIME_FORMAT);

                LinkedList whereFieldData = new LinkedList();

                if (alModule != null && alModule.Count > 0)
                {
                    string sModuleList = _commondata.MakeVariablesList(alModule);
                    sbWhere.AppendFormat("   AND A.EQP_MODULE_ID IN ({0}) ", sModuleList);
                }
                else if (alEqp != null && alEqp.Count > 0)
                {
                    string sEqpList = _commondata.MakeVariablesList(alEqp);
                    sbWhere.AppendFormat("   AND B.EQP_ID IN ({0}) ", sEqpList);
                }

                if (alRecipe != null && alRecipe.Count > 0)
                {
                    string sRecipeList = _commondata.MakeVariablesList(alRecipe);
                    sbWhere.AppendFormat("   AND A.RECIPE_ID IN ({0}) ", sRecipeList);
                }

                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

                sbWhere.Append("   AND START_DTTS BETWEEN TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbWhere.Append("                      AND TO_TIMESTAMP (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");


                //QUERY
                sbQuery.Append("SELECT * FROM ( ");
                sbQuery.Append("SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, LOT_ID, PRODUCT_ID, RECIPE_ID, STATUS_CD, ");
                sbQuery.Append("       MIN(START_DTTS) AS START_DTTS, MAX(END_DTTS) AS END_DTTS, ");
                sbQuery.Append("       SUM(SAMPLE_COUNT) AS SAMPLE_COUNT, ");
                sbQuery.Append("       SUM(ALARM_COUNT) AS ALARM_COUNT, ");
                sbQuery.Append("       SUM(TRACE_FAULT_COUNT) AS TRACE_FAULT_COUNT, ");
                sbQuery.Append("       SUM(TRACE_WARNING_COUNT) AS TRACE_WARNING_COUNT, ");
                sbQuery.Append("       SUM(MSPC_FAULT_COUNT) AS MSPC_FAULT_COUNT ");
                sbQuery.Append("  FROM EQP_TRACE_TRX_FDC A INNER JOIN EQP_MST_PP B ");
                sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
                sbQuery.Append(" WHERE 1 = 1 ");
                sbQuery.Append("   AND STATUS_CD = 'R' ");
                sbQuery.Append(sbWhere.ToString());
                sbQuery.Append(" GROUP BY B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS, LOT_ID, PRODUCT_ID, RECIPE_ID, STATUS_CD ");
                sbQuery.Append(" UNION ALL ");
                sbQuery.Append(" SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, LOT_ID, PRODUCT_ID, RECIPE_ID, STATUS_CD, ");
                sbQuery.Append("       START_DTTS, END_DTTS, ");
                sbQuery.Append("       SAMPLE_COUNT, ");
                sbQuery.Append("       ALARM_COUNT, ");
                sbQuery.Append("       TRACE_FAULT_COUNT, ");
                sbQuery.Append("       TRACE_WARNING_COUNT, ");
                sbQuery.Append("       MSPC_FAULT_COUNT ");
                sbQuery.Append("  FROM EQP_TRACE_TRX_FDC A INNER JOIN EQP_MST_PP B ");
                sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
                sbQuery.Append(" WHERE 1 = 1 ");
                sbQuery.Append("   AND STATUS_CD = 'I' ");
                sbQuery.Append(sbWhere.ToString());
                sbQuery.Append(") ");
                sbQuery.Append("ORDER BY START_DTTS DESC ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }



        public DataSet GetTraceSubstrateHistory(byte[] btdata)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, ");
            sbQuery.Append("       A.EQP_DCP_ID, A.STATUS_CD, A.START_DTTS, A.END_DTTS, ");
            sbQuery.Append("       A.TRACE_FAULT_COUNT, A.SAMPLE_COUNT, A.ALARM_COUNT, A.MSPC_FAULT_COUNT, A.TRACE_WARNING_COUNT, ");
            sbQuery.Append("       A.DATA_QUALITY_INDEX, A.OPERATION_ID, A.LOT_ID, A.LOT_TYPE_CD, ");
            sbQuery.Append("       A.CASSETTE_SLOT, A.SUBSTRATE_ID, A.PRODUCT_ID, A.RECIPE_ID, A.STEP_ID, ");
            sbQuery.Append("       A.RSD_01, A.RSD_02, A.RSD_03, A.RSD_04, A.RSD_05, A.RSD_06, A.RSD_07, A.RSD_08, A.RSD_09, A.RSD_10, ");
            sbQuery.Append("       A.RSD_11, A.RSD_12, A.RSD_13, A.RSD_14, A.RSD_15, A.RSD_16, A.RSD_17, A.RSD_18, A.RSD_19, A.RSD_20 ");
            sbQuery.Append("  FROM EQP_TRACE_TRX_FDC A INNER JOIN EQP_MST_PP B ");
            sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
            sbQuery.Append(" WHERE 1 = 1 ");
            sbQuery.Append("   AND SUBSTRATE_ID <> 'N/A' ");
            sbQuery.Append("   AND SUBSTRATE_ID IS NOT NULL ");
            //sbQuery.Append("   AND EQP_RAWID = :EQP_RAWID ");
            //sbQuery.Append("   AND RECIPE_ID = :RECIPE_ID ");
            //sbQuery.Append("   AND SUBSTRATE_ID = :SUBSTRATE_ID ");
            //sbQuery.Append("   AND CASSETTE_SLOT = :CASSETTE_SLOT ");
            //sbQuery.Append("   AND LOT_ID = :LOT_ID ");
            //sbQuery.Append("   AND B.LOCATION_RAWID = :LINE_RAWID ");
            //sbQuery.Append("   AND B.AREA_RAWID = :AREA_RAWID ");
            //sbQuery.Append("   AND B.EQP_MODEL = :EQP_MODEL ");
            //sbQuery.Append("   AND B.EQP_ID = :EQP_ID ");   
            //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append(" ORDER BY START_DTTS DESC ");

            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string start = "";
                string end = "";

                DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
                DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];
                DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
                DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];
                DataTable dtRecipeID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_RECIPE];

                DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                start = dateFrom.ToString(Definition.DATETIME_FORMAT);
                DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                end = dateTo.ToString(Definition.DATETIME_FORMAT);

                LinkedList whereFieldData = new LinkedList();

                CommonUtility util = new CommonUtility();

                if (dtModuleID != null)
                {
                    string sModuleList = util.ConvertDataColumnIntoStringList(dtModuleID, Definition.CONDITION_SEARCH_KEY_VALUEDATA, "'", ",");
                    if (sModuleList != string.Empty)
                    {
                        sbQuery.AppendFormat(" AND A.EQP_MODULE_ID IN ({0}) ", sModuleList);
                    }
                }
                else if (dtEqpID != null)
                {
                    string sEqpList = util.ConvertDataColumnIntoStringList(dtEqpID, Definition.CONDITION_SEARCH_KEY_VALUEDATA, "'", ",");
                    if (sEqpList != string.Empty)
                    {
                        sbQuery.AppendFormat(" AND B.EQP_ID IN ({0}) ", sEqpList);
                    }
                }

                if (dtRecipeID != null)
                {
                    string sRecipeList = util.ConvertDataColumnIntoStringList(dtRecipeID, Definition.CONDITION_SEARCH_KEY_DISPLAYDATA, "'", ",");
                    if (sRecipeList != null)
                    {
                        sbQuery.AppendFormat("   AND A.RECIPE_ID IN ({0}) ", sRecipeList);
                    }
                }

                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

                //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("   AND A.START_DTTS BETWEEN TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("                        AND TO_TIMESTAMP (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");

                sbQuery.Append(" ORDER BY A.START_DTTS DESC ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetTraceLotHistoryByLot(byte[] btdata)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ( ");
            sbQuery.Append("SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, LOT_ID, PRODUCT_ID, RECIPE_ID, STATUS_CD, ");
            sbQuery.Append("       MIN(START_DTTS) AS START_DTTS, MAX(END_DTTS) AS END_DTTS, ");
            sbQuery.Append("       SUM(SAMPLE_COUNT) AS SAMPLE_COUNT, ");
            sbQuery.Append("       SUM(ALARM_COUNT) AS ALARM_COUNT, ");
            sbQuery.Append("       SUM(TRACE_FAULT_COUNT) AS TRACE_FAULT_COUNT, ");
            sbQuery.Append("       SUM(TRACE_WARNING_COUNT) AS TRACE_WARNING_COUNT, ");
            sbQuery.Append("       SUM(MSPC_FAULT_COUNT) AS MSPC_FAULT_COUNT ");
            sbQuery.Append("  FROM EQP_TRACE_TRX_FDC A INNER JOIN EQP_MST_PP B ");
            sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
            sbQuery.Append(" WHERE 1 = 1 ");
            //sbQuery.Append("   AND EQP_RAWID = :MODULE_RAWID ");
            //sbQuery.Append("   AND RECIPE_ID = :RECIPE_ID ");
            //sbQuery.Append("   AND LOT_ID = :LOT_ID ");
            //sbQuery.Append("   AND B.LOCATION_RAWID = :LINE_RAWID ");
            //sbQuery.Append("   AND B.AREA_RAWID = :AREA_RAWID ");
            //sbQuery.Append("   AND B.EQP_MODEL = :EQP_MODEL ");
            //sbQuery.Append("   AND B.EQP_ID = :EQP_ID ");   
            //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append(" GROUP BY B.EQP_ID, B.MODULE_NAME, EQP_RAWID, LOT_ID, PPID, PRODUCT_ID, RECIPE_ID, STATUS_CD ");
            //sbQuery.Append(" ORDER BY START_DTTS DESC ");

            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string lot = "";
                string substrate = "";
                string start = "";
                string end = "";
                string lineRawid = "";
                string areaRawid = "";
                string eqpModel = "";
                string eqpRawid = "";
                string eqpID = "";
                string moduleID = "";

                DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
                DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];

                DataTable dtLot = null;
                DataTable dtSubstrate = null;
                if (llstData[Definition.CONDITION_SEARCH_KEY_LOT] != null)
                    dtLot = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LOT];
                if (llstData[Definition.CONDITION_SEARCH_KEY_SUBSTRATE] != null)
                    dtSubstrate = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_SUBSTRATE];
                DataTable dtLine = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LINE];
                DataTable dtArea = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_AREA];
                DataTable dtEqpModel = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
                DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];

                if (dtLot != null)
                    lot = dtLot.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                if (dtSubstrate != null)
                    substrate = dtSubstrate.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                if (dtLine != null)
                    lineRawid = dtLine.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtArea != null)
                    areaRawid = dtArea.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtEqpModel != null)
                    eqpModel = dtEqpModel.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtEqpID != null)
                {
                    eqpRawid = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                    eqpID = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                }
                if (dtModuleID != null)
                    moduleID = dtModuleID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();

                DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                start = dateFrom.ToString(Definition.DATETIME_FORMAT);
                DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                end = dateTo.ToString(Definition.DATETIME_FORMAT);

                LinkedList whereFieldData = new LinkedList();

                if (eqpID != string.Empty && moduleID == string.Empty)
                {
                    sbQuery.Append("   AND B.EQP_ID = :EQP_ID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
                }
                if (eqpID != string.Empty && moduleID != string.Empty)
                {
                    sbQuery.Append("   AND A.EQP_MODULE_ID = :MODULE_ID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, moduleID);
                }
                if (lineRawid != string.Empty)
                {
                    sbQuery.Append("   AND B.LOCATION_RAWID = :LINE_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
                }
                if (areaRawid != string.Empty)
                {
                    sbQuery.Append("   AND B.AREA_RAWID = :AREA_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
                }
                if (eqpModel != string.Empty)
                {
                    sbQuery.Append("   AND B.EQP_MODEL = :EQP_MODEL ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
                }

                if (lot != string.Empty)
                {
                    lot = "'%" + lot + "%'   ";
                    sbQuery.Append("   AND LOT_ID LIKE " + lot);
                }
                if (substrate != string.Empty)
                {
                    substrate = "'%" + substrate + "%'   ";
                    sbQuery.Append("   AND SUBSTRATE_ID LIKE " + substrate);
                }

                //sbQuery.Append("   AND LOT_ID LIKE :LOT_ID ");
                //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("   AND A.START_DTTS BETWEEN TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("                        AND TO_TIMESTAMP (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");

                sbQuery.Append(" GROUP BY B.EQP_ID,  B.MODULE_ID, B.MODULE_NAME, B.ALIAS, LOT_ID, PRODUCT_ID, RECIPE_ID, STATUS_CD ");
                sbQuery.Append(" ORDER BY START_DTTS DESC ");
                sbQuery.Append(" ) ");

                //whereFieldData.Add(Definition.CONDITION_KEY_LOT_ID, lot);
                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

                ds = base.Query(sbQuery.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        public DataSet GetTraceSubstrateHistoryByLot(byte[] btdata)
        {
            StringBuilder sbQuery = new StringBuilder();
            //sbQuery.Append("SELECT B.EQP_ID, B.MODULE_NAME, A.* ");
            sbQuery.Append("SELECT B.EQP_ID, B.MODULE_ID, B.MODULE_NAME, B.ALIAS AS MODULE_ALIAS, ");
            sbQuery.Append("       A.EQP_DCP_ID, A.STATUS_CD, A.START_DTTS, A.END_DTTS, ");
            sbQuery.Append("       A.TRACE_FAULT_COUNT, A.SAMPLE_COUNT, A.ALARM_COUNT, A.MSPC_FAULT_COUNT, A.TRACE_WARNING_COUNT, ");
            sbQuery.Append("       A.DATA_QUALITY_INDEX, A.OPERATION_ID, A.LOT_ID, A.LOT_TYPE_CD, ");
            sbQuery.Append("       A.CASSETTE_SLOT, A.SUBSTRATE_ID, A.PRODUCT_ID, A.RECIPE_ID, A.STEP_ID, ");
            sbQuery.Append("       A.RSD_01, A.RSD_02, A.RSD_03, A.RSD_04, A.RSD_05, A.RSD_06, A.RSD_07, A.RSD_08, A.RSD_09, A.RSD_10, ");
            sbQuery.Append("       A.RSD_11, A.RSD_12, A.RSD_13, A.RSD_14, A.RSD_15, A.RSD_16, A.RSD_17, A.RSD_18, A.RSD_19, A.RSD_20 ");
            sbQuery.Append("  FROM EQP_TRACE_TRX_FDC A INNER JOIN EQP_MST_PP B ");
            sbQuery.Append("       ON (A.EQP_MODULE_ID = B.MODULE_ID) ");
            sbQuery.Append(" WHERE 1 = 1 ");
            sbQuery.Append("   AND SUBSTRATE_ID <> 'N/A' ");
            sbQuery.Append("   AND SUBSTRATE_ID IS NOT NULL ");
            //sbQuery.Append("   AND EQP_RAWID = :EQP_RAWID ");
            //sbQuery.Append("   AND RECIPE_ID = :RECIPE_ID ");
            //sbQuery.Append("   AND SUBSTRATE_ID = :SUBSTRATE_ID ");
            //sbQuery.Append("   AND CASSETTE_SLOT = :CASSETTE_SLOT ");
            //sbQuery.Append("   AND LOT_ID = :LOT_ID ");
            //sbQuery.Append("   AND B.LOCATION_RAWID = :LINE_RAWID ");
            //sbQuery.Append("   AND B.AREA_RAWID = :AREA_RAWID ");
            //sbQuery.Append("   AND B.EQP_MODEL = :EQP_MODEL ");
            //sbQuery.Append("   AND B.EQP_ID = :EQP_ID ");   
            //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
            //sbQuery.Append(" ORDER BY START_DTTS DESC ");

            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string lot = "";
                string substrate = "";
                string start = "";
                string end = "";
                string lineRawid = "";
                string areaRawid = "";
                string eqpModel = "";
                string eqpRawid = "";
                string eqpID = "";
                string moduleID = "";

                DataTable dtFrom = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_FROMDATE];
                DataTable dtTo = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_TODATE];

                DataTable dtLot = null;
                DataTable dtSubstrate = null;
                if (llstData[Definition.CONDITION_SEARCH_KEY_LOT] != null)
                    dtLot = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LOT];
                if (llstData[Definition.CONDITION_SEARCH_KEY_SUBSTRATE] != null)
                    dtSubstrate = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_SUBSTRATE];
                DataTable dtLine = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_LINE];
                DataTable dtArea = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_AREA];
                DataTable dtEqpModel = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                DataTable dtEqpID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_EQP];
                DataTable dtModuleID = (DataTable)llstData[Definition.CONDITION_SEARCH_KEY_MODULE];

                if (dtLot != null)
                    lot = dtLot.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                if (dtSubstrate != null)
                    substrate = dtSubstrate.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                if (dtLine != null)
                    lineRawid = dtLine.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtArea != null)
                    areaRawid = dtArea.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtEqpModel != null)
                    eqpModel = dtEqpModel.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                if (dtEqpID != null)
                {
                    eqpRawid = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                    eqpID = dtEqpID.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                }
                if (dtModuleID != null)
                    moduleID = dtModuleID.Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();

                DateTime dateFrom = (DateTime)dtFrom.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                start = dateFrom.ToString(Definition.DATETIME_FORMAT);
                DateTime dateTo = (DateTime)dtTo.Rows[0][Definition.CONDITION_SEARCH_KEY_DATETIME_VALUEDATA];
                end = dateTo.ToString(Definition.DATETIME_FORMAT);

                LinkedList whereFieldData = new LinkedList();

                if (eqpID != string.Empty && moduleID == string.Empty)
                {
                    sbQuery.Append("   AND B.EQP_ID = :EQP_ID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, eqpID);
                }
                if (eqpID != string.Empty && moduleID != string.Empty)
                {
                    sbQuery.Append("   AND A.EQP_MODULE_ID = :MODULE_ID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_MODULE_ID, moduleID);
                }
                if (lineRawid != string.Empty)
                {
                    sbQuery.Append("   AND B.LOCATION_RAWID = :LINE_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
                }
                if (areaRawid != string.Empty)
                {
                    sbQuery.Append("   AND B.AREA_RAWID = :AREA_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
                }
                if (eqpModel != string.Empty)
                {
                    sbQuery.Append("   AND B.EQP_MODEL = :EQP_MODEL ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
                }

                if (lot != string.Empty)
                {
                    lot = "'%" + lot + "%'   ";
                    sbQuery.Append("   AND LOT_ID LIKE " + lot);
                }
                if (substrate != string.Empty)
                {
                    substrate = "'%" + substrate + "%'   ";
                    sbQuery.Append("   AND SUBSTRATE_ID LIKE " + substrate);
                }
                //sbQuery.Append("   AND LOT_ID LIKE :LOT_ID ");
                //sbQuery.Append("   AND START_DTTS >= TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                //sbQuery.Append("   AND END_DTTS <= TO_DATE (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("   AND A.START_DTTS BETWEEN TO_TIMESTAMP (:START_DATE, 'YYYY-MM-DD HH24:MI:SS') ");
                sbQuery.Append("                        AND TO_TIMESTAMP (:END_DATE, 'YYYY-MM-DD HH24:MI:SS') ");

                sbQuery.Append(" ORDER BY A.START_DTTS DESC ");

                //whereFieldData.Add(Definition.CONDITION_KEY_LOT_ID, lot);
                whereFieldData.Add(Definition.CONDITION_KEY_START_DATE, start);
                whereFieldData.Add(Definition.CONDITION_KEY_END_DATE, end);

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }


        #endregion


        #region :Interlock Condition

        public DataSet GetMVA(string eqprawid)
        {
            DataSet ds = null;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add("EQP_RAWID", Convert.ToDouble(eqprawid));

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("  SELECT                                                ");
                sbQuery.Append("        MST.MVA_NAME MVA_NAME                           ");
                sbQuery.Append("      , MST.RAWID RAWID                                 ");
                sbQuery.Append("      , RCP.RECIPE_NAME RECIPE_NAME                     ");
                sbQuery.Append("      , STEP.STEP_ID STEP_ID                            ");
                sbQuery.Append("  FROM EQP_MVA_MST_FDC MST                              ");
                sbQuery.Append("  JOIN EQP_MVA_RCP_STEP_MST_FDC MVASTEP                 ");
                sbQuery.Append("       ON (MVASTEP.EQP_MVA_RAWID = MST.RAWID)           ");
                sbQuery.Append("  JOIN EQP_RECIPE_STEP_MST_PP STEP                      ");
                sbQuery.Append("       ON (STEP.RAWID = MVASTEP.EQP_RECIPE_STEP_RAWID)  ");
                sbQuery.Append("  JOIN EQP_RECIPE_MST_PP RCP                            ");
                sbQuery.Append("       ON (RCP.RAWID = STEP.EQP_RECIPE_RAWID)           ");
                sbQuery.Append("  WHERE EQP_RAWID = :EQP_RAWID                          ");
                sbQuery.Append("  ORDER BY MVA_NAME                                     ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region :Code Query

        public string BoolToString(bool value)
        {
            if (value) return "Y";
            else return "N";
        }

        public string GetCodeByName(string category, string name)
        {
            DataSet ds = null;
            string data = string.Empty;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add("CATEGORY", category);
                whereFieldData.Add("NAME", name);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("  SELECT CODE                       ");
                sbQuery.Append("  FROM CODE_MST_PP                  ");
                sbQuery.Append("  WHERE	CATEGORY = :CATEGORY        ");
                sbQuery.Append("    AND NAME = :NAME                ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data = Convert.ToString(ds.Tables[0].Rows[0]["CODE"]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return data;
        }

        public string GetNameByCode(string category, string code)
        {
            DataSet ds = null;
            string data = string.Empty;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add("CATEGORY", category);
                whereFieldData.Add("CODE", code);

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("  SELECT NAME                       ");
                sbQuery.Append("  FROM CODE_MST_PP                  ");
                sbQuery.Append("  WHERE	CATEGORY = :CATEGORY        ");
                sbQuery.Append("    AND CODE = :CODE                ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data = Convert.ToString(ds.Tables[0].Rows[0]["NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return data;
        }

        #endregion

        #region :User Query

        public string GetUserRawID(string user)
        {
            DataSet ds = null;
            string data = string.Empty;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add("USERID", user.ToUpper());

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("  SELECT RAWID                      ");
                sbQuery.Append("  FROM USER_MST_PP                  ");
                sbQuery.Append("  WHERE	UPPER(USER_ID) = :USERID    ");

                ds = base.Query(sbQuery.ToString(), whereFieldData);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data = Convert.ToString(ds.Tables[0].Rows[0]["RAWID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return data;
        }


        #endregion

        #region :Insert User Change History

        public int InsertUpdateHistory(string userRawID, string type, string app, string eqp, string name, string value)
        {
            int rawid = 0;

            try
            {
                this.BeginTrans();

                decimal seq = base.GetSequence("SEQ_USER_UPDATE_TRX_SEC");
                rawid = Convert.ToInt32(seq);

                LinkedList paramData = new LinkedList();

                paramData.Add("RAWID", Convert.ToDouble(rawid));

                paramData.Add("USER_RAWID", Convert.ToDouble(userRawID));
                paramData.Add("EQP_RAWID", Convert.ToDouble(eqp));
                paramData.Add("UPDATE_DTTS", DateTime.Now);
                paramData.Add("UPDATE_TYPE", Convert.ToString(type));

                paramData.Add("APP_NAME", Convert.ToString(app));
                paramData.Add("ITEM_NAME", Convert.ToString(name));
                paramData.Add("ITEM_VALUE", Convert.ToString(value));

                paramData.Add("CREATE_DTTS", DateTime.Now);
                paramData.Add("CREATE_BY", Convert.ToDouble(userRawID));

                this.Insert("USER_UPDATE_TRX_SEC", paramData);

                if (this.ErrorMessage.Length > 0)
                {
                    this.RollBack();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                this.RollBack(ex);
                return 0;
            }

            this.Commit();

            return rawid;
        }


        #endregion

        #region Recipe Info

        private const string SQL_QUERY_RECIPE_STEP_INFOS =
        @"SELECT EMP.MODULE_ID, EMP.EQP_ID, EMP.MODULE_NAME, EMP.ALIAS AS MODULE_ALIAS, 
                 EMP.PARENT, RVT.RECIPE_ID, SVT.STEP_NAME, SVT.STEP_ID
            FROM EQP_MST_PP EMP
                 LEFT OUTER JOIN
                 (SELECT * FROM EQP_RECIPE_MST_PP ERMP) RVT 
                 ON RVT.EQP_MODULE_ID = EMP.MODULE_ID
                 LEFT OUTER JOIN
                 (SELECT * FROM EQP_RECIPE_STEP_MST_PP ERSMP) SVT
                 ON (SVT.EQP_MODULE_ID = RVT.EQP_MODULE_ID AND SVT.EQP_RECIPE_ID = RVT.RECIPE_ID)
           WHERE EMP.LOCATION_RAWID = :LOCATION_RAWID                  
                 AND EMP.EQP_ID = :EQP_ID
                 {0}
            ORDER BY EMP.EQP_ID, EMP.MODULE_NAME, RVT.RECIPE_ID, SVT.STEP_NAME ";

        public DataSet QueryRecipeStepInfos(byte[] btdata)
        {
            DataSet ds = new DataSet();

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btdata);

                string sLineRawID = "";
                string sAreaRawID = "";
                string sEQPID = "";

                LinkedList whereFieldData = new LinkedList();
                string sSubCondition = "";

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    sLineRawID = llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString();
                    whereFieldData.Add("LOCATION_RAWID", sLineRawID);
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sAreaRawID = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
                    sSubCondition = "AND EMP.AREA_RAWID = :AREA_RAWID ";
                    whereFieldData.Add("AREA_RAWID", sAreaRawID);
                }

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                {
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                    whereFieldData.Add("EQP_ID", sEQPID);
                }

                string sSQL = string.Format(SQL_QUERY_RECIPE_STEP_INFOS, sSubCondition);

                ds = base.Query(sSQL, whereFieldData);

                if (DSUtil.GetResultSucceed(ds) == 0)
                    ds = new DataSet();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion

        #region Favorite Condition

        private const string SQL_QUERY_FAVORITE_CONDITION =
            @"SELECT * FROM CUSTOM_CDT_MST_FDC
                WHERE USER_RAWID = :USER_RAWID ";

        public DataSet GetFavoriteCondition(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sUserRawID = "";

                if (llstData[Definition.CONDITION_KEY_USER_RAWID] != null)
                    sUserRawID = llstData[Definition.CONDITION_KEY_USER_RAWID].ToString();

                LinkedList whereFieldData = new LinkedList();
                if (sUserRawID.Length > 0)
                {
                    whereFieldData.Add("USER_RAWID", sUserRawID);
                    ds = base.Query(SQL_QUERY_FAVORITE_CONDITION, whereFieldData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_FAVORITE_PERIOD =
           @"SELECT TVT.*, FVT.NAME CONDITION_TYPE, UVT.USER_NAME CREATE_BY FROM 
                (
                 SELECT EMP.EQP_ID, 'EQ' CONDITION_CD, EMP.MODULE_ID CONDITION_RAWID, EMP.MODULE_NAME CONDITION_VALUE, VT.USER_RAWID, 
                       DECODE(VT.PERIOD, '', 
                       (SELECT CONDITION_VALUE FROM CUSTOM_CDT_MST_FDC
                       WHERE USER_RAWID = :USER_RAWID
                       AND CONDITION_CD = 'DP'), VT.PERIOD) PERIOD,
                       VT.CREATE_DTTS, 
                       VT.CREATE_BY CREATE_BY_RAWID,
                       VT.LAST_UPDATE_DTTS,
                       VT.LAST_UPDATE_BY,
                       DECODE(VT.PERIOD, '', 'N', 'Y') EXIST
                       FROM EQP_MST_PP EMP
                       LEFT OUTER JOIN
                       (SELECT * FROM CUSTOM_PERIOD_MST_FDC CPMF
                       WHERE CPMF.USER_RAWID = :USER_RAWID) VT
                       ON EMP.RAWID = VT.CONDITION_RAWID  
                 ) TVT
                 LEFT OUTER JOIN
                (SELECT CODE, NAME
                   FROM CODE_MST_PP
                  WHERE CATEGORY = 'FDC_CUSTOM_CONDITION'
                  and code = 'EQ') FVT
                 ON FVT.CODE = TVT.CONDITION_CD
                 LEFT OUTER JOIN
                 (SELECT RAWID, USER_ID, USER_NAME
                 FROM USER_MST_PP
                 WHERE RAWID = :USER_RAWID) UVT
                 ON UVT.RAWID = TVT.CREATE_BY_RAWID
                WHERE TVT.EQP_ID = :EQP_ID ";

        private const string SQL_QUERY_FAVORITE_PERIOD_WITHOUT_USER =
            @"SELECT TVT.*, FVT.NAME CONDITION_TYPE, UVT.USER_NAME CREATE_BY FROM 
                (
                 SELECT EMP.EQP_ID, 'EQ' CONDITION_CD, EMP.MODULE_ID CONDITION_RAWID, EMP.MODULE_NAME CONDITION_VALUE, VT.USER_RAWID, 
                       DECODE(VT.PERIOD, '', 
                       (SELECT CONDITION_VALUE FROM CUSTOM_CDT_MST_FDC
                       WHERE USER_RAWID = :USER_RAWID
                       AND CONDITION_CD = 'DP'), VT.PERIOD) PERIOD,
                       VT.CREATE_DTTS, 
                       VT.CREATE_BY CREATE_BY_RAWID,
                       VT.LAST_UPDATE_DTTS,
                       VT.LAST_UPDATE_BY,
                       DECODE(VT.PERIOD, '', 'N', 'Y') EXIST
                       FROM EQP_MST_PP EMP
                       LEFT OUTER JOIN
                       (SELECT * FROM CUSTOM_PERIOD_MST_FDC CPMF
                       WHERE CPMF.USER_RAWID = :USER_RAWID) VT
                       ON EMP.RAWID = VT.CONDITION_RAWID  
                 ) TVT
                 LEFT OUTER JOIN
                (SELECT CODE, NAME
                   FROM CODE_MST_PP
                  WHERE CATEGORY = 'FDC_CUSTOM_CONDITION'
                  and code = 'EQ') FVT
                 ON FVT.CODE = TVT.CONDITION_CD
                 LEFT OUTER JOIN
                 (SELECT RAWID, USER_ID, USER_NAME
                 FROM USER_MST_PP
                 WHERE RAWID = :USER_RAWID) UVT
                 ON UVT.RAWID = TVT.CREATE_BY_RAWID
                WHERE TVT.EQP_ID = :EQP_ID ";

        public DataSet GetFavoritePeriod(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sUserRawID = "";
                string sEQPID = "";
                string sModuleRawID = "";
                string sModuleName = "";

                if (llstData[Definition.CONDITION_KEY_USER_RAWID] != null)
                    sUserRawID = llstData[Definition.CONDITION_KEY_USER_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_MODULE_RAWID] != null)
                    sModuleRawID = llstData[Definition.CONDITION_KEY_MODULE_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_MODULE_NAME] != null)
                    sModuleName = llstData[Definition.CONDITION_KEY_MODULE_NAME].ToString();

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                LinkedList whereFieldData = new LinkedList();

                //임시로 넣음.
                if (sUserRawID.Length == 0)
                    sUserRawID = "1";

                if (sUserRawID.Length > 0)
                {
                    whereFieldData.Add("USER_RAWID", sUserRawID);
                    whereFieldData.Add("EQP_ID", sEQPID);

                    string sSQL = SQL_QUERY_FAVORITE_PERIOD;
                    if (sModuleRawID.Length > 0)
                    {
                        sSQL = sSQL + "AND TVT.CONDITION_RAWID = :MODULE_RAWID ";

                        whereFieldData.Add("MODULE_RAWID", sModuleRawID);
                    }

                    sSQL = sSQL + "ORDER BY TVT.CONDITION_VALUE";

                    ds = base.Query(sSQL, whereFieldData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }




        #endregion

        #region :Spec Model Group

        //        private const string SQL_QUERY_SPEC_MODEL_GROUP_LIST =
        //           @"SELECT * FROM SPEC_MODEL_GROUP_MST_PP
        //               WHERE EQP_RECIPE_RAWID  = :EQP_RECIPE_RAWID
        //               ORDER BY SPEC_MODEL_GROUP_NAME ";

        private const string SQL_QUERY_SPEC_MODEL_GROUP_LIST =
            //          @" SELECT TO_CHAR(SMGMF.RAWID) RAWID, SMGMF.EQP_RECIPE_ID,
            //                SMGMF.SPEC_MODEL_GROUP_NAME, SMGMF.CREATE_DTTS, SMGMF.CREATE_BY,
            //                ERMP.EQP_RAWID, ERMP.EQP_MODULE_ID, ERMP.PRODUCT_ID
            //                FROM SPEC_MODEL_GROUP_MST_FDC SMGMF
            //                LEFT OUTER JOIN EQP_RECIPE_MST_PP ERMP
            //                ON SMGMF.EQP_RECIPE_ID = ERMP.RECIPE_ID
            //                WHERE SMGMF.EQP_MODULE_ID = :EQP_MODULE_ID ";

        @"SELECT TO_CHAR(SMGMF.RAWID) RAWID, SMGMF.EQP_RECIPE_ID,
               SMGMF.SPEC_MODEL_GROUP_NAME, SMGMF.CREATE_DTTS, SMGMF.CREATE_BY,
               SMGMF.EQP_MODULE_ID, SMGMF.PRODUCT_ID
          FROM SPEC_MODEL_GROUP_MST_FDC SMGMF       
         WHERE 1 = 1
           AND SMGMF.EQP_MODULE_ID = :EQP_MODULE_ID ";

        public DataSet QuerySpecModelGroupList(byte[] btData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(btData);

                string sEQPRawID = "";
                string sRecipeRawID = "";
                string sSpecModelGroupName = "";
                string sProductID = "";
                string sSQL = SQL_QUERY_SPEC_MODEL_GROUP_LIST;

                if (llstData[Definition.CONDITION_KEY_MODULE_ID] != null)
                    sEQPRawID = llstData[Definition.CONDITION_KEY_MODULE_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
                    sRecipeRawID = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_SPEC_MODEL_GROUP_NAME] != null)
                    sSpecModelGroupName = llstData[Definition.CONDITION_KEY_SPEC_MODEL_GROUP_NAME].ToString();

                if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                    sProductID = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();

                LinkedList llstCondition = new LinkedList();
                llstCondition.Add("EQP_MODULE_ID", sEQPRawID);

                if (sRecipeRawID.Length > 0)
                {
                    llstCondition.Add("EQP_RECIPE_ID", sRecipeRawID);
                    sSQL = sSQL + " AND SMGMF.EQP_RECIPE_ID  = :EQP_RECIPE_ID ";
                }

                if (sSpecModelGroupName.Length > 0)
                {
                    llstCondition.Add("SPEC_MODEL_GROUP_NAME", sSpecModelGroupName);
                    sSQL = sSQL + " AND SMGMF.SPEC_MODEL_GROUP_NAME  = :SPEC_MODEL_GROUP_NAME ";
                }

                if (sProductID.Length > 0)
                {
                    llstCondition.Add("PRODUCT_ID", sProductID);
                    sSQL = sSQL + " AND SMGMF.PRODUCT_ID  = :PRODUCT_ID ";
                }
                else
                {
                    sSQL = sSQL + " AND SMGMF.PRODUCT_ID  IS NULL ";
                }

                sSQL = sSQL + " ORDER BY SPEC_MODEL_GROUP_NAME ";

                ds = base.Query(sSQL, llstCondition);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }
        #endregion


        #region :SPC MODEL
        public DataSet GetSPCModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT  /*+ index (mms IDX_MODEL_MST_SPC_UK) */ distinct mms.*, mcms.chart_mode_cd, mcms.chart_mode ");
                sb.Append("  FROM MODEL_MST_SPC  mms , (SELECT model_rawid, NVL (chart_mode_cd, 'NOT DEFINED') chart_mode_cd, NVL (NAME, 'NOT DEFINED') chart_mode, param_type_cd ");
                sb.Append("FROM model_config_mst_spc A LEFT OUTER JOIN code_mst_pp B ON A.chart_mode_cd = B.code and B.CATEGORY = 'SPC_CHART_MODE'where A.main_yn = 'Y') mcms ");
                sb.Append(" WHERE mms.RAWID = mcms.MODEL_RAWID ");
                sb.Append(" AND mms.LOCATION_RAWID = :LINE_RAWID ");

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    whereFieldData.Add("LINE_RAWID", llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID in ({0}) ", llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sb.AppendFormat(" AND mcms.PARAM_TYPE_CD not in ('{0}')", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData["FILTER"] != null)
                {
                    if (llstData["FILTER"].ToString().Trim().Length > 0)
                    {
                        sb.AppendFormat(" AND upper(mms.spc_model_name) like '%{0}%'", llstData["FILTER"].ToString().ToUpper());
                    }
                }

                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                {
                    string sEQPModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
                    if (sEQPModel.Length > 0 && sEQPModel.Replace("'", "").Length > 0)
                    {
                        sb.AppendFormat(" AND mms.EQP_MODEL in ({0}) ", sEQPModel);
                    }
                }
                sb.Append(" ORDER BY mms.SPC_MODEL_NAME ASC ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        public DataSet GetMETSPCModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT  /*+ index (mms IDX_MODEL_MST_SPC_UK) */ distinct mms.*, mcms.chart_mode_cd, mcms.chart_mode ");
                sb.Append("  FROM MODEL_MST_SPC  mms , (SELECT model_rawid, NVL (chart_mode_cd, 'NOT DEFINED') chart_mode_cd, NVL (NAME, 'NOT DEFINED') chart_mode, param_type_cd ");
                sb.Append("FROM model_config_mst_spc A LEFT OUTER JOIN code_mst_pp B ON A.chart_mode_cd = B.code and B.CATEGORY = 'SPC_CHART_MODE'where A.main_yn = 'Y') mcms ");
                sb.Append(" WHERE mms.RAWID = mcms.MODEL_RAWID ");
                sb.Append(" AND mms.LOCATION_RAWID = :LINE_RAWID ");

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    whereFieldData.Add("LINE_RAWID", llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID in ({0}) ", llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sb.AppendFormat(" AND mcms.PARAM_TYPE_CD = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData["FILTER"] != null)
                {
                    if (llstData["FILTER"].ToString().Trim().Length > 0)
                    {
                        sb.AppendFormat(" AND upper(mms.spc_model_name) like '%{0}%'", llstData["FILTER"].ToString().ToUpper());
                    }
                }

                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                {
                    string sEQPModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
                    if (sEQPModel.Length > 0 && sEQPModel.Replace("'", "").Length > 0)
                    {
                        sb.AppendFormat(" AND mms.EQP_MODEL in ({0}) ", sEQPModel);
                    }
                }
                sb.Append(" ORDER BY mms.SPC_MODEL_NAME ASC ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        public DataSet GetSPCModelOfLines(string[] lineRawid, string[] areaRawid, string eqpModelName, string paramTypeCD, string filterValue)
        {
            DataSet ds = null;

            try
            {
                if(lineRawid.Length == 0 || areaRawid.Length == 0
                    || lineRawid.Length != areaRawid.Length)
                    return null;

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT  /*+ index (mms IDX_MODEL_MST_SPC_UK) */ distinct mms.*, mcms.chart_mode_cd, mcms.chart_mode ");
                sb.Append("  FROM MODEL_MST_SPC  mms , (SELECT model_rawid, NVL (chart_mode_cd, 'NOT DEFINED') chart_mode_cd, NVL (NAME, 'NOT DEFINED') chart_mode FROM model_config_mst_spc A LEFT OUTER JOIN code_mst_pp B ON A.chart_mode_cd = B.code and B.CATEGORY = 'SPC_CHART_MODE'where A.main_yn = 'Y') mcms ");
                sb.Append(" WHERE mms.RAWID = mcms.MODEL_RAWID AND (");
                
                for(int i=0; i<lineRawid.Length; i++)
                {
                    if(i != 0)
                        sb.Append("OR ");
                    sb.Append("(mms.LOCATION_RAWID = '" + lineRawid[i] + "' and mms.AREA_RAWID = '" + areaRawid[i] + "')");
                }
                sb.Append(")");

                //if (!string.IsNullOrEmpty(paramTypeCD))
                //{
                //    sb.AppendFormat(" AND mcms.PARAM_TYPE_CD = '{0}'", paramTypeCD);
                //}

                if (!string.IsNullOrEmpty(filterValue))
                {
                    sb.AppendFormat(" AND upper(mms.spc_model_name) like '%{0}%'", filterValue.ToUpper());
                }

                if (!string.IsNullOrEmpty(eqpModelName))
                {
                    sb.AppendFormat(" AND mms.EQP_MODEL in ({0}) ", eqpModelName);
                }
                sb.Append(" ORDER BY mms.SPC_MODEL_NAME ASC ");
                ds = base.Query(sb.ToString());
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        #endregion

        #region GetSPCModelContext
        public DataSet GetSPCModelContext(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sMODEL_RAWID = "";

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                    sMODEL_RAWID = llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString();

                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add("MODEL_RAWID", sMODEL_RAWID);

                string sql = @"select mcms.rawid, mcms.param_alias, mcms.main_yn,mcms2.CONTEXT_KEY,mcms2.CONTEXT_VALUE
                                from MODEL_CONFIG_MST_SPC mcms,
                                       MODEL_CONTEXT_MST_SPC mcms2
                                where  mcms.model_rawid =:MODEL_RAWID
                                and mcms.rawid = mcms2.model_config_rawid
                                order by mcms.param_alias asc ,mcms.main_yn desc,mcms.rawid,mcms2.key_order asc";

                ds = base.Query(sql, whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }
        #endregion




        public DataSet GetParamName(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sb = new StringBuilder();

                sb.Append(" select distinct param_alias ");
                sb.Append(" from model_mst_spc mms, model_config_mst_spc mcms, ");
                sb.Append(
                    @" (SELECT   aa.model_config_rawid,
                             MAX (aa.product_id) AS product_id,
                             MAX (aa.eqp_id) AS eqp_id,
                             MAX (aa.operation_id) AS operation_id
                        FROM (SELECT DISTINCT model_config_rawid,
                                              CASE
                                                 WHEN context_key =
                                                        'PRODUCT_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS product_id,
                                              CASE
                                                 WHEN context_key =
                                                           'EQP_ID'
                                                  OR context_key =
                                                              'MEASURE_EQP_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS eqp_id,
                                              CASE
                                                 WHEN context_key =
                                                        'MEASURE_OPERATION_ID'
                                                  OR context_key =
                                                                'OPERATION_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS operation_id
                                         FROM model_context_mst_spc) aa
                    GROUP BY aa.model_config_rawid) mcon");
                sb.Append(" where mms.rawid = mcms.model_rawid ");
                sb.Append(" and mcms.rawid = mcon.model_config_rawid ");

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    sb.Append(" and mms.location_rawid =:LINE_RAWID ");
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sb.Append(" and mcms.PARAM_TYPE_CD=:PARAM_TYPE_CD ");
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sb.AppendFormat(" AND mms.EQP_MODEL IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sb.AppendFormat(" AND mcon.OPERATION_ID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sb.AppendFormat(" AND mcms.PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }

                if(llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.RAWID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID]);
                }

                sb.Append("  order by  param_alias  ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        public string[] GetParamListHavingSPCModel(string locationRawID, string areaRawID, string eqpModel, string paramTypeCD)
        {
            List<string> paramAlias = new List<string>();

            StringBuilder query = new StringBuilder();
            query.Append("select distinct mc.PARAM_ALIAS from model_mst_spc m join model_config_mst_spc mc on m.RAWID = mc.MODEL_RAWID where 1=1 ");
            
            try
            {
                DataSet ds = null;

                LinkedList where = new LinkedList();
                if(locationRawID != null)
                {
                    query.Append(" and m.LOCATION_RAWID = :LOCATION_RAWID ");
                    where.Add("LOCATION_RAWID", locationRawID);
                }
                if(areaRawID != null)
                {
                    query.Append(" and m.AREA_RAWID = :AREA_RAWID ");
                    where.Add("AREA_RAWID", areaRawID);
                }
                if(eqpModel != null && eqpModel.Trim().Length > 0)
                {
                    query.Append(" and m.EQP_MODEL = :EQP_MODEL ");
                    where.Add("EQP_MODEL", eqpModel);
                }
                if(paramTypeCD != null)
                {
                    query.Append(" and mc.PARAM_TYPE_CD = :PARAM_TYPE_CD");
                    where.Add("PARAM_TYPE_CD", paramTypeCD);
                }

                ds = this.Query(query.ToString(), where);
                if(ds == null || ds.Tables.Count == 0)
                    return null;

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    paramAlias.Add(dr["PARAM_ALIAS"].ToString());
                }
            }
            catch(Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return paramAlias.ToArray();
        }


        #region :: SPC OPERATION

        public DataSet GetSPCOperation(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sb = new StringBuilder();
                sb.Append(" select distinct operation_id,operation_desc as description  ");
                sb.Append(" from CHART_VW_SPC ");
                sb.Append(" where LINE_RAWID =:LINE_RAWID ");
                sb.Append(" and PARAM_TYPE_CD=:PARAM_TYPE_CD ");

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND AREA_RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sb.AppendFormat(" AND EQP_MODEL IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sb.AppendFormat(" AND PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }

                sb.Append("  order by  operation_id  ");

                ds = base.Query(sb.ToString(), whereFieldData);

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        #endregion


        #region SPC Product

        public DataSet GetSPCProduct(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList whereFieldData = new LinkedList();
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sb = new StringBuilder();
                sb.Append(" select distinct product_id  ");
                sb.Append(" from CHART_VW_SPC ");
                sb.Append(" where LINE_RAWID =:LINE_RAWID ");
                sb.Append(" and PARAM_TYPE_CD=:PARAM_TYPE_CD ");

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND AREA_RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sb.AppendFormat(" AND EQP_MODEL IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sb.AppendFormat(" AND PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_ID] != null)
                {
                    sb.AppendFormat(" AND EQP_ID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_ID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sb.AppendFormat(" AND operation_id in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                }

                sb.Append("  order by  product_id  ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }



        public DataSet GetSPCEQP(byte[] baData)
        {
            DataSet ds = null;

            try
            {

                LinkedList whereFieldData = new LinkedList();
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sb = new StringBuilder();
                sb.Append(" select distinct eqp_id  ");
                sb.Append(" from CHART_VW_SPC ");
                sb.Append(" where LINE_RAWID =:LINE_RAWID ");
                sb.Append(" and PARAM_TYPE_CD=:PARAM_TYPE_CD ");

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND AREA_RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sb.AppendFormat(" AND EQP_MODEL IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sb.AppendFormat(" AND PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }


                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sb.AppendFormat(" AND operation_id in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                }

                sb.Append("  order by  eqp_id  ");
                ds = base.Query(sb.ToString(), whereFieldData);


            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        public string GetSPCParamType(byte[] baData)
        {
            string sParamType = string.Empty;
            DataSet ds = null;

            try
            {

                LinkedList whereFieldData = new LinkedList();
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                StringBuilder sb = new StringBuilder();
                sb.Append(" select distinct param_type_cd  ");
                sb.Append(" from model_config_mst_spc ");
                sb.Append(" where model_rawid in ({0}) ");

                ds = base.Query(string.Format(sb.ToString(), llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID].ToString()), whereFieldData);
                sParamType = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return sParamType;
        }

        #endregion



        #region SPC Control Chart Context

        public DataSet GetSPCContextList(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                bool useComma = false;

                if (llstData[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstData[Definition.VARIABLE_USE_COMMA];

                string sWhere = "";

                LinkedList whereFieldData = new LinkedList();
                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    sWhere += " AND mms.LOCATION_RAWID=:LINE_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
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
                    sWhere += " AND mcms.model_rawid=:SPC_MODEL_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID, llstData[Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID].ToString());

                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sWhere += " AND mcms.PARAM_TYPE_CD=:PARAM_TYPE_CD";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());

                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sWhere += " AND mcms.PARAM_ALIAS=:PARAM_ALIAS";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());

                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    sWhere += " AND mcms.rawid=:MODEL_CONFIG_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.CONTEXT_KEY] != null)
                {
                    sWhere += string.Format(@" and mctms.model_config_rawid in (select model_config_rawid from MODEL_CONTEXT_MST_SPC 
                                 where context_key='{0}' and context_value='{1}')"
                                 , llstData[Definition.DynamicCondition_Condition_key.CONTEXT_KEY].ToString()
                                 , llstData[Definition.DynamicCondition_Condition_key.CONTEXT_VALUE].ToString()
                                 );
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT      ");
                sb.Append("mcms.model_rawid,    ");
                sb.Append("mms.spc_model_name,  ");
                sb.Append("mms.area_rawid,       ");
                sb.Append("amp.area,            ");
                sb.Append("mms.eqp_model,       ");
                sb.Append("mcms.param_alias,        ");
                sb.Append("mcms.MAIN_YN,             ");
                sb.Append("mctms.model_config_rawid, ");
                sb.Append("DECODE (NVL (cc.name, ''), '', mctms.context_key, cc.name) AS context_key,      ");
                if (useComma)
                {
                    sb.Append("REPLACE(mctms.context_value, ';', ',') CONTEXT_VALUE ,   ");
                }
                else
                {
                    sb.Append("mctms.context_value,   ");
                }
                sb.Append("NVL (mctms.key_order, 0) key_order,     ");
                sb.Append("mcoms.RESTRICT_SAMPLE_DAYS,       ");
                sb.Append("mcoms.DEFAULT_CHART_LIST       ");
                sb.Append("FROM MODEL_MST_SPC mms,       ");
                sb.Append(" MODEL_CONFIG_MST_SPC mcms,       ");
                sb.Append(" MODEL_CONTEXT_MST_SPC mctms,        ");
                sb.Append(" MODEL_CONFIG_OPT_MST_SPC mcoms,      ");
                sb.Append(" area_mst_pp amp,        ");
                sb.Append(" (SELECT CODE, NAME        ");
                sb.Append("  FROM code_mst_pp            ");
                sb.Append("  WHERE category = 'CONTEXT_TYPE'         ");
                sb.Append("  UNION       ");
                sb.Append("  SELECT CODE, NAME      ");
                sb.Append("   FROM code_mst_pp        ");
                sb.Append("   WHERE category = 'SPC_CONTEXT_TYPE') cc          ");
                sb.Append(" WHERE     MMS.RAWID = MCMS.MODEL_RAWID          ");

                sb.Append("and mcms.rawid=mctms.model_config_rawid   ");
                sb.Append("and mcms.rawid=mcoms.model_config_rawid    ");
                sb.Append("and mms.area_rawid=amp.rawid    ");
                sb.Append("and mctms.context_key = cc.code(+)   ");
                sb.Append(string.Format("{0} ", sWhere));
                sb.Append(" order by mms.spc_model_name, ");
                sb.Append(" nvl(mctms.key_order,0), ");
                sb.Append(" decode(nvl(cc.name,''),'',mctms.context_key,cc.name), ");
                sb.Append(" context_value ");

//                string sql = string.Format(@"select distinct 
//                                                     mcms.model_rawid
//                                                    ,mms.spc_model_name
//                                                    ,mms.area_rawid
//                                                    ,amp.area
//                                                    ,mms.eqp_model
//                                                    ,mcms.param_alias
//                                                    ,mcms.MAIN_YN
//                                                    ,mctms.model_config_rawid                                                    
//                                                    ,decode(nvl(cc.name,''),'',mctms.context_key,cc.name) as context_key
//                                                    ,mctms.context_value 
//                                                    ,nvl(mctms.key_order,0) key_order 
//                                                    ,mcoms.RESTRICT_SAMPLE_DAYS
//                                                    ,mcoms.DEFAULT_CHART_LIST                                                    
//                                            from MODEL_MST_SPC mms
//                                                , MODEL_CONFIG_MST_SPC mcms
//                                                , MODEL_CONTEXT_MST_SPC mctms
//                                                , MODEL_CONFIG_OPT_MST_SPC mcoms
//                                                , area_mst_pp amp
//                                                , (select CODE,NAME from code_mst_pp 
//                                                    where category='CONTEXT_TYPE'    
//                                                    UNION 
//                                                    select CODE,NAME from code_mst_pp 
//                                                    where category='SPC_CONTEXT_TYPE'    
//                                                    ) cc
//                                            where MMS.RAWID = MCMS.MODEL_RAWID
//                                            and mcms.rawid=mctms.model_config_rawid                                                
//                                            and mcms.rawid=mcoms.model_config_rawid
//                                            and mms.area_rawid=amp.rawid
//                                            and mctms.context_key = cc.code(+)
//                                            {0}                                                                               
//                                            order by mms.spc_model_name
//                                                    , nvl(mctms.key_order,0)
//                                                    , decode(nvl(cc.name,''),'',mctms.context_key,cc.name)
//                                                    , mctms.context_value", sWhere);

                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        #endregion

        #region TOSHIBA

        private const string SQL_QUERY_BASE_WITH_EQP_ID =
                @"SELECT A.LOCATION_RAWID, A.AREA_RAWID, B.AREA, A.EQP_MODEL
                    FROM EQP_MST_PP A, AREA_MST_PP B
                   WHERE A.AREA_RAWID = B.RAWID AND A.EQP_ID = :EQP_ID AND A.PARENT IS NULL  ";

        public DataSet GetBaseInfoWithEQPID(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sEQPID = "";

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                LinkedList whereFieldData = new LinkedList();
                whereFieldData.Add(Definition.CONDITION_KEY_EQP_ID, sEQPID);

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(SQL_QUERY_BASE_WITH_EQP_ID);

                ds = base.Query(sbQuery.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        private const string SQL_QUERY_SPCMODEL_WITH_BASEINFO =
                @"SELECT A.RAWID, A.SPC_MODEL_NAME
                    FROM MODEL_MST_SPC A, MODEL_CONFIG_MST_SPC B
                    WHERE 1=1
                    {0}
                    AND B.MAIN_YN = 'Y'
                    AND A.RAWID = B.MODEL_RAWID  ";

        public DataSet GetSPCModelListbyBaseInfo(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sArea = string.Empty;
                string sEQPModel = string.Empty;
                string sParamAlias = string.Empty;

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sArea = llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString();
                }

                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                {
                    sEQPModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
                }

                if (llstData[Definition.CONDITION_KEY_PARAM_ALIAS] != null)
                {
                    sParamAlias = llstData[Definition.CONDITION_KEY_PARAM_ALIAS].ToString();
                }

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbSubQuery = new StringBuilder();

                sbQuery.Append(SQL_QUERY_SPCMODEL_WITH_BASEINFO);

                if (!string.IsNullOrEmpty(sArea))
                {
                    sbSubQuery.Append(" AND A.AREA_RAWID = :AREA_RAWID ");
                    whereFieldData.Add(Definition.CONDITION_KEY_AREA_RAWID, sArea);
                }

                if (!string.IsNullOrEmpty(sEQPModel))
                {
                    sbSubQuery.Append(" AND A.EQP_MODEL = :EQP_MODEL ");
                    whereFieldData.Add(Definition.CONDITION_KEY_EQP_MODEL, sEQPModel);
                }

                if (!string.IsNullOrEmpty(sParamAlias))
                {
                    sbSubQuery.Append(" AND B.PARAM_ALIAS = :PARAM_ALIAS ");
                    whereFieldData.Add(Definition.CONDITION_KEY_PARAM_ALIAS, sParamAlias);
                }

                ds = base.Query(string.Format(sbQuery.ToString(), sbSubQuery.ToString()), whereFieldData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return ds;
        }

        #endregion
    }

    public class ATTConditionData : DataBase
    {
        Common.CommonData _commondata = new Common.CommonData();

        public ATTConditionData()
        {
            this._sbQuery = new StringBuilder(4096);
        }


        #region :SPC MODEL
        public DataSet GetATTSPCModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT mms.*, mcms.chart_mode_cd, mcms.chart_mode                              ");
                sb.Append("           FROM model_att_mst_spc mms,                                                  ");
                sb.Append("                (SELECT model_rawid,                                                    ");
                sb.Append("                        NVL (chart_mode_cd, 'NOT DEFINED') chart_mode_cd,               ");
                sb.Append("                        NVL (NAME, 'NOT DEFINED') chart_mode                            ");
                sb.Append("                   FROM model_config_att_mst_spc a LEFT OUTER JOIN code_mst_pp b        ");
                sb.Append("                        ON a.chart_mode_cd = b.code                                     ");
                sb.Append("                      AND b.CATEGORY = 'SPC_CHART_MODE'                                 ");
                sb.Append("                  WHERE a.main_yn = 'Y') mcms                                           ");
                sb.Append("          WHERE mms.rawid = mcms.model_rawid                                            ");
                sb.Append("            AND mms.location_rawid = :line_rawid                                        ");

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    whereFieldData.Add("LINE_RAWID", llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID in ({0}) ", llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD] != null)
                {
                    sb.AppendFormat(" AND mcms.PARAM_TYPE_CD = '{0}'", llstData[Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD].ToString());
                }

                if (llstData["FILTER"] != null)
                {
                    if (llstData["FILTER"].ToString().Trim().Length > 0)
                    {
                        sb.AppendFormat(" AND upper(mms.spc_model_name) like '%{0}%'", llstData["FILTER"].ToString().ToUpper());
                    }
                }

                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                {
                    string sEQPModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
                    if (sEQPModel.Length > 0 && sEQPModel.Replace("'", "").Length > 0)
                    {
                        sb.AppendFormat(" AND mms.EQP_MODEL in ({0}) ", sEQPModel);
                    }
                }
                sb.Append(" ORDER BY mms.SPC_MODEL_NAME ASC ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        #endregion

        public DataSet GetParamName(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();
                StringBuilder sb = new StringBuilder();

                sb.Append(" select distinct param_alias ");
                sb.Append(" from model_att_mst_spc mms, model_config_att_mst_spc mcms, ");
                sb.Append(
                    @" (SELECT   aa.model_config_rawid,
                             MAX (aa.product_id) AS product_id,
                             MAX (aa.eqp_id) AS eqp_id,
                             MAX (aa.operation_id) AS operation_id
                        FROM (SELECT DISTINCT model_config_rawid,
                                              CASE
                                                 WHEN context_key =
                                                        'PRODUCT_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS product_id,
                                              CASE
                                                 WHEN context_key =
                                                           'EQP_ID'
                                                  OR context_key =
                                                              'MEASURE_EQP_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS eqp_id,
                                              CASE
                                                 WHEN context_key =
                                                        'MEASURE_OPERATION_ID'
                                                  OR context_key =
                                                                'OPERATION_ID'
                                                    THEN context_value
                                                 ELSE '*'
                                              END AS operation_id
                                         FROM model_context_att_mst_spc) aa
                    GROUP BY aa.model_config_rawid) mcon");
                sb.Append(" where mms.rawid = mcms.model_rawid ");
                sb.Append(" and mcms.rawid = mcon.model_config_rawid ");

                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    sb.Append(" and mms.location_rawid =:LINE_RAWID ");
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL] != null)
                {
                    sb.AppendFormat(" AND mms.EQP_MODEL IN ({0}) ", llstData[Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID] != null)
                {
                    sb.AppendFormat(" AND mcon.OPERATION_ID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sb.AppendFormat(" AND mcms.PARAM_ALIAS in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.RAWID in ({0}) ", llstData[Definition.DynamicCondition_Condition_key.MODEL_RAWID]);
                }

                sb.Append("  order by  param_alias  ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        public string[] GetParamListHavingSPCModel(string locationRawID, string areaRawID, string eqpModel, string paramTypeCD)
        {
            List<string> paramAlias = new List<string>();

            StringBuilder query = new StringBuilder();
            query.Append("select distinct mc.PARAM_ALIAS from model_att_mst_spc m join model_config_att_mst_spc mc on m.RAWID = mc.MODEL_RAWID where 1=1 ");

            try
            {
                DataSet ds = null;

                LinkedList where = new LinkedList();
                if (locationRawID != null)
                {
                    query.Append(" and m.LOCATION_RAWID = :LOCATION_RAWID ");
                    where.Add("LOCATION_RAWID", locationRawID);
                }
                if (areaRawID != null)
                {
                    query.Append(" and m.AREA_RAWID = :AREA_RAWID ");
                    where.Add("AREA_RAWID", areaRawID);
                }
                if (eqpModel != null && eqpModel.Trim().Length > 0)
                {
                    query.Append(" and m.EQP_MODEL = :EQP_MODEL ");
                    where.Add("EQP_MODEL", eqpModel);
                }

                ds = this.Query(query.ToString(), where);
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    paramAlias.Add(dr["PARAM_ALIAS"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return paramAlias.ToArray();
        }



        #region SPC Control Chart Context

        public DataSet GetSPCContextList(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                bool useComma = false;

                if (llstData[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstData[Definition.VARIABLE_USE_COMMA];

                string sWhere = "";

                LinkedList whereFieldData = new LinkedList();
                if (llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID] != null)
                {
                    sWhere += " AND mms.LOCATION_RAWID=:LINE_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, llstData[Definition.DynamicCondition_Condition_key.LINE_RAWID].ToString());
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
                    sWhere += " AND mcms.model_rawid=:SPC_MODEL_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID, llstData[Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID].ToString());

                }


                if (llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] != null)
                {
                    sWhere += " AND mcms.PARAM_ALIAS=:PARAM_ALIAS";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, llstData[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString());

                }

                if (llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID] != null)
                {
                    sWhere += " AND mcms.rawid=:MODEL_CONFIG_RAWID";
                    whereFieldData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, llstData[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString());
                }

                if (llstData[Definition.DynamicCondition_Condition_key.CONTEXT_KEY] != null)
                {
                    sWhere += string.Format(@" and mctms.model_config_rawid in (select model_config_rawid from MODEL_CONTEXT_ATT_MST_SPC 
                                 where context_key='{0}' and context_value='{1}')"
                                 , llstData[Definition.DynamicCondition_Condition_key.CONTEXT_KEY].ToString()
                                 , llstData[Definition.DynamicCondition_Condition_key.CONTEXT_VALUE].ToString()
                                 );
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT DISTINCT      ");
                sb.Append("mcms.model_rawid,    ");
                sb.Append("mms.spc_model_name,  ");
                sb.Append("mms.area_rawid,       ");
                sb.Append("amp.area,            ");
                sb.Append("mms.eqp_model,       ");
                sb.Append("mcms.param_alias,        ");
                sb.Append("mcms.MAIN_YN,             ");
                sb.Append("mctms.model_config_rawid, ");
                sb.Append("DECODE (NVL (cc.name, ''), '', mctms.context_key, cc.name) AS context_key,      ");
                if (useComma)
                {
                    sb.Append("REPLACE(mctms.context_value, ';', ',') CONTEXT_VALUE ,   ");
                }
                else
                {
                    sb.Append("mctms.context_value,   ");
                }
                sb.Append("NVL (mctms.key_order, 0) key_order,     ");
                sb.Append("mcoms.RESTRICT_SAMPLE_DAYS,       ");
                sb.Append("mcoms.DEFAULT_CHART_LIST       ");
                sb.Append("FROM MODEL_MST_SPC mms,       ");
                sb.Append("MODEL_CONFIG_MST_SPC mcms,       ");
                sb.Append("MODEL_CONTEXT_MST_SPC mctms,        ");
                sb.Append("MODEL_CONFIG_OPT_MST_SPC mcoms,      ");
                sb.Append("area_mst_pp amp,        ");
                sb.Append("(SELECT CODE, NAME        ");
                sb.Append(" FROM code_mst_pp            ");
                sb.Append(" WHERE category = 'CONTEXT_TYPE'         ");
                sb.Append(" UNION       ");
                sb.Append(" SELECT CODE, NAME      ");
                sb.Append("   FROM code_mst_pp        ");
                sb.Append("   WHERE category = 'SPC_CONTEXT_TYPE') cc          ");
                sb.Append(" WHERE     MMS.RAWID = MCMS.MODEL_RAWID          ");

                sb.Append("and mcms.rawid=mctms.model_config_rawid   ");
                sb.Append("and mcms.rawid=mcoms.model_config_rawid    ");
                sb.Append("and mms.area_rawid=amp.rawid    ");
                sb.Append("and mctms.context_key = cc.code(+)   ");
                sb.Append(string.Format("{0} ", sWhere));
                sb.Append(" order by mms.spc_model_name, ");
                sb.Append(" nvl(mctms.key_order,0), ");
                sb.Append(" decode(nvl(cc.name,''),'',mctms.context_key,cc.name), ");
                sb.Append(" context_value ");

//                string sql = string.Format(@"select distinct 
//                                                     mcms.model_rawid
//                                                    ,mms.spc_model_name
//                                                    ,mms.area_rawid
//                                                    ,amp.area
//                                                    ,mms.eqp_model
//                                                    ,mcms.param_alias
//                                                    ,mcms.MAIN_YN
//                                                    ,mctms.model_config_rawid                                                    
//                                                    ,decode(nvl(cc.name,''),'',mctms.context_key,cc.name) as context_key
//                                                    ,mctms.context_value 
//                                                    ,nvl(mctms.key_order,0) key_order 
//                                                    ,mcoms.RESTRICT_SAMPLE_DAYS
//                                                    ,mcoms.DEFAULT_CHART_LIST                                                    
//                                            from MODEL_ATT_MST_SPC mms
//                                                , MODEL_CONFIG_ATT_MST_SPC mcms
//                                                , MODEL_CONTEXT_ATT_MST_SPC mctms
//                                                , MODEL_CONFIG_OPT_ATT_MST_SPC mcoms
//                                                , area_mst_pp amp
//                                                , (select CODE,NAME from code_mst_pp 
//                                                    where category='CONTEXT_TYPE'    
//                                                    UNION 
//                                                    select CODE,NAME from code_mst_pp 
//                                                    where category='SPC_CONTEXT_TYPE'    
//                                                    ) cc
//                                            where MMS.RAWID = MCMS.MODEL_RAWID
//                                            and mcms.rawid=mctms.model_config_rawid                                                
//                                            and mcms.rawid=mcoms.model_config_rawid
//                                            and mms.area_rawid=amp.rawid
//                                            and mctms.context_key = cc.code(+)
//                                            {0}                                                                               
//                                            order by mms.spc_model_name
//                                                    , nvl(mctms.key_order,0)
//                                                    , decode(nvl(cc.name,''),'',mctms.context_key,cc.name)
//                                                    , mctms.context_value", sWhere);

                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }

        #endregion

        public DataSet GetSPCModel(byte[] baData)
        {
            DataSet ds = null;

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                LinkedList whereFieldData = new LinkedList();

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT  /*+ index (mms IDX_MODEL_MST_SPC_UK) */ distinct mms.*, mcms.chart_mode_cd, mcms.chart_mode ");
                sb.Append("  FROM MODEL_ATT_MST_SPC  mms , (SELECT model_rawid, NVL (chart_mode_cd, 'NOT DEFINED') chart_mode_cd, ");
                sb.Append(" NVL (NAME, 'NOT DEFINED') chart_mode FROM model_config_att_mst_spc A LEFT OUTER JOIN code_mst_pp B ON A.chart_mode_cd = B.code and B.CATEGORY = 'SPC_CHART_MODE'where A.main_yn = 'Y') mcms ");
                sb.Append(" WHERE mms.RAWID = mcms.MODEL_RAWID ");
                sb.Append(" AND mms.LOCATION_RAWID = :LINE_RAWID ");

                if (llstData[Definition.CONDITION_KEY_LINE_RAWID] != null)
                {
                    whereFieldData.Add("LINE_RAWID", llstData[Definition.CONDITION_KEY_LINE_RAWID].ToString());
                }

                if (llstData[Definition.CONDITION_KEY_AREA_RAWID] != null)
                {
                    sb.AppendFormat(" AND mms.AREA_RAWID in ({0}) ", llstData[Definition.CONDITION_KEY_AREA_RAWID].ToString());
                }

                if (llstData["FILTER"] != null)
                {
                    if (llstData["FILTER"].ToString().Trim().Length > 0)
                    {
                        sb.AppendFormat(" AND upper(mms.spc_model_name) like '%{0}%'", llstData["FILTER"].ToString().ToUpper());
                    }
                }

                if (llstData[Definition.CONDITION_KEY_EQP_MODEL] != null)
                {
                    string sEQPModel = llstData[Definition.CONDITION_KEY_EQP_MODEL].ToString();
                    if (sEQPModel.Length > 0 && sEQPModel.Replace("'", "").Length > 0)
                    {
                        sb.AppendFormat(" AND mms.EQP_MODEL in ({0}) ", sEQPModel);
                    }
                }
                sb.Append(" ORDER BY mms.SPC_MODEL_NAME ASC ");
                ds = base.Query(sb.ToString(), whereFieldData);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return ds;
        }
    }

}
