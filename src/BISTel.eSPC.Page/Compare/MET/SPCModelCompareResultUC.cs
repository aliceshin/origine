using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.History.MET;
using BISTel.eSPC.Page.Modeling;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;

namespace BISTel.eSPC.Page.Compare.MET
{
    public enum CompareType
    {
        Model,
        Version
    }

    public partial class SPCModelCompareResultUC : BISTel.eSPC.Page.Compare.SPCModelCompareResultUC
    {
    } 
}
