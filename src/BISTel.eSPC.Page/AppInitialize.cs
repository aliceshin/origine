using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.EESInterface;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page
{
    public class AppInitialize : IConnection
    {
        public void MakeSession()
        {
            eSPCWebService.eSPCWebService ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            ws.GetPreConnection();
        }

        public void GenerateDummyPage(string dllFullName, Panel basePanel)
        {
            
        }
    }
}
