using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Report
{
    public class ProcessCapabilityData : DataBase
    {
        ProcessCapabilityDataCall dataCall = new ProcessCapabilityDataCall();
        bool isATT = false;


        #region ::: SELECT

        public DataSet GetModelContext(byte[] baData)
        {
            DataSet ds = dataCall.GetModelContext(baData, isATT);
            
            return ds;
        }
        
        public DataSet GetProcessCapabilityList(byte[] baData)
        {
            DataSet ds = dataCall.GetProcessCapabilityList(baData, isATT);
            
            return ds;
        }
        
        public DataSet GetPpkReport(byte[] baData)
        {
            DataSet ds = dataCall.GetPpkReport(baData, isATT);
            
            return ds;
        }
        
        public DataSet GetOperationID(byte[] baData)
        {
            DataSet ds = dataCall.GetOperationID(baData);
            
            return ds;
        }
        
        /// <summary>
        /// Use Display : ProcessCapabilityUC
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetDataTRXData(byte[] baData)
        {
            DataSet ds = dataCall.GetDataTRXData(baData, isATT);
            
            return ds;
        }
        
        #endregion 
               
    }

    public class ATTProcessCapabilityData : DataBase
    {
        ProcessCapabilityDataCall dataCall = new ProcessCapabilityDataCall();
        bool isATT = true;


        #region ::: SELECT

        public DataSet GetModelContext(byte[] baData)
        {
            DataSet ds = dataCall.GetModelContext(baData, isATT);
            
            return ds;
        }

        public DataSet GetProcessCapabilityList(byte[] baData)
        {
            DataSet ds = dataCall.GetProcessCapabilityList(baData, isATT);
            
            return ds;
        }

        public DataSet GetPpkReport(byte[] baData)
        {
            DataSet ds = dataCall.GetPpkReport(baData, isATT);
            
            return ds;
        }

        /// <summary>
        /// Use Display : ProcessCapabilityUC
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetDataTRXData(byte[] baData)
        {
            DataSet ds = dataCall.GetDataTRXData(baData, isATT);
            
            return ds;
        }
        
        #endregion

    }   

}
