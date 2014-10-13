using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Compare.MET
{
    internal class SPCModelListController : BISTel.eSPC.Page.Compare.SPCModelListController
    {
        internal SPCModelListController(eSPCWebService.eSPCWebService ws, Control parent) : base(ws, parent) { }
    }
}
