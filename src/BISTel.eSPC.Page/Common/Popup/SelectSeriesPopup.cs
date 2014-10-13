using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class SelectSeriesPopup : BasePopupFrm
    {
        BaseChart Chart;

        CommonUtility _ComUtil;

        public SelectSeriesPopup(BaseChart ChartParent)
        {
            InitializeComponent();
            this._ComUtil = new CommonUtility();

            Chart = ChartParent;
            InitializeLayout();
        }

        public void InitializeLayout()
        {
            try
            {
                Image bulletImg = this._ComUtil.GetImage(Definition.BUTTON_KEY_BULLET);
                this.blblbullet01.Image = bulletImg;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            bcb_Kind.Items.Clear();
            bcb_Kind.Items.Add("Series Shift Hori");
            bcb_Kind.Items.Add("Series Shift Vert");
            bcb_Kind.Items.Add("Same Start Point");
            bcb_Kind.Items.Add("Clear Shift");
        }

        public void bcb_Kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.bcb_Kind.SelectedIndex == 0)
            {
                this.Chart.Click_SERIES_SHIFT_HORI_Button();
                this.Close();
            }
            if (this.bcb_Kind.SelectedIndex == 1)
            {
                this.Chart.Click_SERIES_SHIFT_VERT_Button();
                this.Close();
            }
            if (this.bcb_Kind.SelectedIndex == 2)
            {
                this.Chart.Click_SAME_START_POINT_Button();
                this.Close();
            }
            if (this.bcb_Kind.SelectedIndex == 3)
            {
                this.Chart.Click_CLEAR_SHIFT_Button();
                this.Close();
            }
        }
    }
}
