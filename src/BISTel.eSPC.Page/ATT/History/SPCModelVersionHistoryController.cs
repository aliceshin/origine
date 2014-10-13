using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.History
{
    public class SPCModelVersionHistoryController
    {
        private eSPCWebService.eSPCWebService ws = null;

        public SPCModelVersionHistoryController()
        {
            ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        }

        public DataTable GetHistoryDataTable(string modelConfigRawID)
        {
            return ws.GetATTSPCModelVersionHistory(modelConfigRawID);
        }

        public string CheckLatestVersion(string sChartID)
        {
            return ws.GetATTSPCLastestVersion(sChartID);
        }

        public bool DeleteVersion(LinkedList lnkCondition)
        {
            return ws.DeleteSPCATTModelHistory(lnkCondition.GetSerialData());
        }
    }
}
