using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page
{
    public partial class DummyPUC : UserControl
    {
        public DummyPUC()
        {
            InitializeComponent();

            eSPCWebService.eSPCWebService eSPCWS = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            eSPCWebService.eSPCWebService eSPCPageWS = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            LinkedList llstCondition = new LinkedList();

            eSPCWS.GetLine(llstCondition.GetSerialData());
            eSPCPageWS.GetLine(llstCondition.GetSerialData());
        }
    }
}
