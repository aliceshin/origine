using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;


namespace BISTel.eSPC.Data.Server
{
    public class InterfaceData : DataBase
    {
        private const string SQL_QUERY_FRAMEWORK_LIST = "SELECT * FROM FRAMEWORK_MST_PP";

        public DataSet QueryFrameWorkList()
        {
            DataSet ds = new DataSet();

            try
            {
                ds = base.Query(SQL_QUERY_FRAMEWORK_LIST);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }

            }
            catch
            {

            }

            return ds;
        }

        private const string SQL_QUERY_VIEW_TABLE_LIST = "SELECT * FROM CONFIG_PROPERTY_MST_PP WHERE PROPERTY_NAME = :PROPERTY_NAME";

        public DataSet QueryViewTableList()
        {
            DataSet ds = new DataSet();

            try
            {
                LinkedList lklist = new LinkedList();
                lklist.Add("PROPERTY_NAME", "TABLE");


                //ds = base.Query(SQL_QUERY_VIEW_TABLE_LIST, lklist);
                ds = base.Query(SQL_QUERY_VIEW_TABLE_LIST, lklist);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        private const string SQL_QUERY_VIEW_EQP_LIST = "SELECT * FROM {0} ";


        public DataSet QueryViewEQPList(string sViewTableName)
        {
            DataSet ds = new DataSet();

            try
            {
                string sSQL = string.Format(SQL_QUERY_VIEW_EQP_LIST, sViewTableName);

                ds = base.Query(sSQL);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        private const string SQL_QUERY_CONFIG_LIST = "SELECT * FROM CONFIG_MST_PP";

        public DataSet QueryConfigList()
        {
            DataSet ds = new DataSet();

            try
            {
                //LinkedList lklist = new LinkedList();
                //lklist.Add("FRAMEWORK_ID", sFrameworkID);
                //lklist.Add("MEMBER_ID", sMemberID);

                //ds = base.Query(SQL_QUERY_CONFIG_LIST, lklist);

                ds = base.Query(SQL_QUERY_CONFIG_LIST);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        private const string SQL_QUERY_CONFIG_PROPERTY_LIST = " SELECT * FROM CONFIG_PROPERTY_MST_PP ";

        public DataSet QueryConfigPropertyList()
        {
            DataSet ds = new DataSet();

            try
            {
                //LinkedList lklist = new LinkedList();
                //lklist.Add("CONFIG_RAWID", CONFIG_RAWID);

                //ds = base.Query(SQL_QUERY_CONFIG_PROPERTY_LIST, lklist);

                ds = base.Query(SQL_QUERY_CONFIG_PROPERTY_LIST);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }

        private const string SQL_QUERY_SUBJECT_NAME =
                "SELECT * FROM CONFIG_PROPERTY_MST_PP "
                + "WHERE CONFIG_RAWID IN ( "
                + "SELECT MAX(RAWID) FROM CONFIG_MST_PP "
                + "WHERE ITEM_TYPE = 'CONNECTIVITY' "
                + "AND ITEM_NAME = 'CLIENT_TIBRV') "
                + "AND PROPERTY_NAME IN ('SUBJECT') "
                ;

        public DataSet QuerySubjectName()
        {
            DataSet ds = new DataSet();

            try
            {
                ds = base.Query(SQL_QUERY_SUBJECT_NAME);

                if (DSUtil.GetResultSucceed(ds) == 0)
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {

            }

            return ds;
        }



        #region :Get Target Config

        public DataSet QueryTargetConfig(byte[] param)
        {
            string sql_query_target_config =
            @"SELECT TM.TARGET_NAME, TC.ITEM_TYPE, TC.ITEM_NAME 
              FROM TARGET_MST_PP TM
              JOIN TARGET_CONFIG_MST_PP TC
                ON (TM.RAWID = TC.TARGET_RAWID AND TM.TARGET_NAME = :TARGET_NAME )";


            DataSet dsReturn = new DataSet();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                LinkedList llstCondition = new LinkedList();
                llstCondition.Add("TARGET_NAME", llstParam[Definition.CONDITION_KEY_TARGET_NAME].ToString());

                dsReturn = base.Query(sql_query_target_config, llstCondition);

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

        #endregion

    }
}
