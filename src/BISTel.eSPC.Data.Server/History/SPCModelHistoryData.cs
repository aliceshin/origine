using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BISTel.eSPC.Data.Server.History
{
    public class SPCModelHistoryData
    {
        SPCModelHistoryDataCall _dataCall = new SPCModelHistoryDataCall();
        bool _isATT = false;

        /// <summary>
        /// Get version history of spc model
        /// </summary>
        /// <param name="rawid">rawid of spc model</param>
        /// <returns>version history table</returns>
        public DataTable GetVersionHistory(string rawid)
        {
            DataTable result = _dataCall.GetVersionHistory(rawid, _isATT);

            return result;
        }

        public DataSet GetSPCSpecAndRuleOfHistory(string modelConfigRawID, string[] versions)
        {
            DataSet dsReturn = _dataCall.GetSPCSpecAndRuleOfHistory(modelConfigRawID, versions, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCModelVersionData(string modelConfigRawid, string version)
        {
            DataSet dsReturn = _dataCall.GetSPCModelVersionData(modelConfigRawid, version, _isATT);

            return dsReturn;
        }

        public string GetSPCLastestVersion(string chartID)
        {
            string result = _dataCall.GetSPCLastestVersion(chartID, _isATT);

            return result;
        }

        public bool DeleteSPCModelHistory(byte[] baData)
        {
            bool result = _dataCall.DeleteSPCModelHistory(baData, _isATT);

            return result;
        }
    }

    public class SPCATTModelHistoryData
    {
        SPCModelHistoryDataCall _dataCall = new SPCModelHistoryDataCall();
        bool _isATT = true;

        /// <summary>
        /// Get version history of spc model
        /// </summary>
        /// <param name="rawid">rawid of spc model</param>
        /// <returns>version history table</returns>
        public DataTable GetATTVersionHistory(string rawid)
        {
            DataTable result = _dataCall.GetVersionHistory(rawid, _isATT);

            return result;
        }

        public DataSet GetATTSPCSpecAndRuleOfHistory(string modelConfigRawID, string[] versions)
        {
            DataSet dsReturn = _dataCall.GetSPCSpecAndRuleOfHistory(modelConfigRawID, versions, _isATT);

            return dsReturn;
        }

        public DataSet GetATTSPCModelVersionData(string modelConfigRawid, string version)
        {
            DataSet dsReturn = _dataCall.GetSPCModelVersionData(modelConfigRawid, version, _isATT);

            return dsReturn;
        }

        public string GetATTSPCLastestVersion(string chartID)
        {
            string result = _dataCall.GetSPCLastestVersion(chartID, _isATT);

            return result;
        }

        public bool DeleteSPCATTModelHistory(byte[] baData)
        {
            bool result = _dataCall.DeleteSPCModelHistory(baData, _isATT);

            return result;
        }
    }
}
