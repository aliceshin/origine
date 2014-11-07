using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.OCAP
{
    public class OCAPData : DataBase
    {
        private OCAPDataCall dataCall = new OCAPDataCall();

        #region ::: SELECT


        public DataSet GetOCAPCommentList(string[] OOCRawID)
        {
            DataSet dsResult = dataCall.GetOCAPCommentList(OOCRawID);

            return dsResult;
        }

        public DataSet GetOCAPCommentList_New(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPCommentList_New(baData);

            return dsResult;
        }

        public DataSet GetOCAPListData(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPListData(baData, false);

            return dsResult;
        }

        public DataSet GetOCAPData(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPData(baData);

            return dsResult;
        }

        public DataSet GetOCAPDetails(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPDetails(baData, false);

            return dsResult;
        }

        public DataSet FindAnnotationOCAP(byte[] baData)
        {
            DataSet dsResult = dataCall.FindAnnotationOCAP(baData);

            return dsResult;
        }


        #endregion

        //SPC-703
        public DataSet GetOOCCommentList(string OCAPRawid)
        {
            DataSet dsResult = dataCall.GetOOCCommentList(OCAPRawid, false);

            return dsResult;
        }

        public DataSet getGroupInfoByChartId(string chartId)
        {
            DataSet ds = dataCall.getGroupInfoByChartId(chartId, false);

            return ds;
        }


        #region ::: UPDATE


        public bool UpdateOCAPDate(byte[] baData)
        {
            bool bSuccess = dataCall.UpdateOCAPDate(baData);

            return bSuccess;
        }


        #endregion


        #region ::: INSERT

        #endregion
    }

    public class ATTOCAPData : DataBase
    {
        private OCAPDataCall dataCall = new OCAPDataCall();


        #region ::: SELECT


        public DataSet GetOCAPListData(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPListData(baData, true);

            return dsResult;
        }

        public DataSet GetOCAPDetails(byte[] baData)
        {
            DataSet dsResult = dataCall.GetOCAPDetails(baData, true);

            return dsResult;
        }


        #endregion

        //SPC-703
        public DataSet GetOOCCommentList(string OCAPRawid)
        {
            DataSet dsResult = dataCall.GetOOCCommentList(OCAPRawid, true);

            return dsResult;
        }

        //SPC-1292, KBLEE
        public DataSet GetGroupInfoByChartId(string chartId)
        {
            DataSet ds = dataCall.getGroupInfoByChartId(chartId, true);

            return ds;
        }

        #region ::: UPDATE
        #endregion


        #region ::: INSERT
        #endregion
    }

}
