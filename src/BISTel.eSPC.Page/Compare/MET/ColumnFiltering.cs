using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Compare.MET
{
    public partial class ColumnFiltering : BISTel.eSPC.Page.Compare.ColumnFiltering
    {
        public ColumnFiltering(DataTable dtData, string columnName) : base(dtData, columnName)
        {
        }
    }
}
