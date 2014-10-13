using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Compare.MET
{
    public partial class SpreadFilteringPopup : BISTel.eSPC.Page.Compare.SpreadFilteringPopup
    {
        public SpreadFilteringPopup(DataTable dataTable, string[] columnNames) : base(dataTable, columnNames) { }
    }
}
