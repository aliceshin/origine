using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BISTel.eSPC.Data.Server.Tool
{
    public class SPCDataExport
    {
        SPCDataExportDataCall _dataCall = new SPCDataExportDataCall();
        bool _isATT = false;

        public DataSet GetSPCModelsData(string[] modelRawids, bool useComma)
        {
            DataSet result = _dataCall.GetSPCModelsData(modelRawids, useComma, _isATT);

            return result;
        }
    }

    public class SPCATTDataExport
    {
        SPCDataExportDataCall _dataCall = new SPCDataExportDataCall();
        bool _isATT = true;

        public DataSet GetATTSPCModelsData(string[] modelRawids, bool useComma)
        {
            DataSet result = _dataCall.GetSPCModelsData(modelRawids, useComma, _isATT);

            return result;
        }
    }
}
