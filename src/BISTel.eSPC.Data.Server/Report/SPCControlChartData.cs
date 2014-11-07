using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Report
{
    public class SPCControlChartData : DataBase
    {
        SPCControlChartDataCall dataCall = new SPCControlChartDataCall();
        bool isATT = false;


        #region ::: SELECT


        public DataSet GetSPCControlChartData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCControlChartData(baData, isATT);
            
            return ds;
        }

        //SPC-704
        public DataSet GetSPCMulControlChartData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCMulControlChartData(baData, isATT);
            
            return ds;
        }

        public DataSet GetSPCControlChartToDayData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCControlChartToDayData(baData, isATT);

            return ds;
        }

        public DataSet GetSPCChartViewSumTempData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCChartViewSumTempData(baData, isATT);
            
            return ds;
        }

        public DataSet GetMetEQPData(byte[] baData)
        {
            DataSet ds = dataCall.GetMetEQPData(baData);

            return ds;
        }

        public DataSet GetSPCModelConfigSearch(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCModelConfigSearch(baData);
            
            return ds;
        }


        #endregion
        

        public DataSet GetContextType(byte[] baData)
        {
            DataSet ds = dataCall.GetContextType(baData);

            return ds;
        }

        public int GetTheNumberOfSubConfigOfModel(string modelConfigRawid)
        {
            int iResult = dataCall.GetTheNumberOfSubConfigOfModel(modelConfigRawid, false);

            return iResult;
        }

        public bool SaveToggleInformation(string[] rawIDs, string[] toggles, string[] modelConfigRawIDs, string[] spcDataDttses, string[] toggleYNs)
        {
            bool bResult = dataCall.SaveToggleInformation(rawIDs, toggles, modelConfigRawIDs, spcDataDttses, toggleYNs);

            return bResult;
        }        

        //SPC-678
        public string GetParamAlias(byte[] baData)
        {
            string strResult = dataCall.GetParamAlias(baData);
            
            return strResult;
        }
    }

    public class SPCATTControlChartData : DataBase
    {
        SPCControlChartDataCall dataCall = new SPCControlChartDataCall();
        bool isATT = true;


        #region ::: SELECT

        public DataSet GetSPCControlChartData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCControlChartData(baData, isATT);

            return ds;
        }

        //SPC-704
        public DataSet GetSPCMulControlChartData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCMulControlChartData(baData, isATT);
            
            return ds;
        }

        public DataSet GetSPCControlChartToDayData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCControlChartToDayData(baData, isATT);

            return ds;
        }

        public DataSet GetSPCChartViewSumTempData(byte[] baData)
        {
            DataSet ds = dataCall.GetSPCChartViewSumTempData(baData, isATT);

            return ds;
        }
        
        #endregion


        public int GetTheNumberOfATTSubConfigOfModel(string modelConfigRawid)
        {
            int iResult = dataCall.GetTheNumberOfSubConfigOfModel(modelConfigRawid, isATT);

            return iResult;
        }
    }
}
