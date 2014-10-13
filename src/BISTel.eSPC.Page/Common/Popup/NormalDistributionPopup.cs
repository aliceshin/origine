using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using Microsoft.Win32;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class NormalDistributionPopup : BasePopupFrm
    {

        #region : Constructor
        public NormalDistributionPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Initialization

        #region :: Button

        #region ::: Event

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

        #endregion
    }
}
