using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.UserAuthorityHandler;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.Additional;

namespace BISTel.eSPC.Common.ATT
{
    public enum ButtonListType
    {
        Spread = 0
        , Chart = 1
    }
    public class Initialization : BISTel.eSPC.Common.Initialization
    {
    }

}
