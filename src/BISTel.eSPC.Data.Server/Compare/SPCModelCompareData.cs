using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BISTel.eSPC.Data.Server.Compare
{
    public class SPCModelCompareData
    {
        SPCModelCompareDataCall _dataCall = new SPCModelCompareDataCall();
        bool _isATT = false;

        public DataSet GetSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            DataSet dsReturn = _dataCall.GetSPCModelList(lineRawID, areaRawID, eqpModel, paramAlias, paramTypeCd, useComma, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            DataSet dsReturn = _dataCall.GetSPCSpecAndRule(modelConfigRawIDs, _isATT);

            return dsReturn;
        }
    }

    public class SPCATTModelCompareData
    {
        SPCModelCompareDataCall _dataCall = new SPCModelCompareDataCall();
        bool _isATT = true;

        public DataSet GetSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            DataSet dsReturn = _dataCall.GetSPCModelList(lineRawID, areaRawID, eqpModel, paramAlias, paramTypeCd, useComma, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            DataSet dsReturn = _dataCall.GetSPCSpecAndRule(modelConfigRawIDs, _isATT);

            return dsReturn;
        }
    }
}
