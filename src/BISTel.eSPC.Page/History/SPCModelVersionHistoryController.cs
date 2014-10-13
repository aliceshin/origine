using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.History
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
            return ws.GetSPCModelVersionHistory(modelConfigRawID);
        }

        public string CheckLatestVersion(string sChartID)
        {
            return ws.GetSPCLastestVersion(sChartID);
        }

        public bool DeleteVersion(LinkedList lnkCondition)
        {
            return ws.DeleteSPCModelHistory(lnkCondition.GetSerialData());
        }
    }
}
