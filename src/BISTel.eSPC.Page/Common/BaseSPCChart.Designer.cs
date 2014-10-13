namespace BISTel.eSPC.Page.Common
{
    partial class BaseSPCChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseSPCChart));
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bTeeChart1 = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart(this.components);
            this.pnlPCA = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.bTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bTeeChart1);
            this.bTitlePanel1.Controls.Add(this.pnlPCA);
            this.bTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitlePanel1.ImageBottom = null;
            this.bTitlePanel1.ImageBottomRight = null;
            this.bTitlePanel1.ImageClose = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageClose")));
            this.bTitlePanel1.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageLeft")));
            this.bTitlePanel1.ImageLeftBottom = null;
            this.bTitlePanel1.ImageLeftFill = null;
            this.bTitlePanel1.ImageMax = null;
            this.bTitlePanel1.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageMiddle")));
            this.bTitlePanel1.ImageMin = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(448, 344);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "Title";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // bTeeChart1
            // 
            this.bTeeChart1.AutoScroll = true;
            this.bTeeChart1.AxisDateTimeFormat = "yyyy-MM-dd";
            this.bTeeChart1.AxisHoriGridLineCount = 10;
            this.bTeeChart1.ClickAction = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.ChartClickAction.DEFAULT;
            this.bTeeChart1.ClickActionCustom = null;
            this.bTeeChart1.DefaultToolTipInfo = null;
            this.bTeeChart1.DivideHeightAtShowGroup = 2;
            this.bTeeChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTeeChart1.HeaderText = "";
            this.bTeeChart1.IsApplyAllCSG = true;
            this.bTeeChart1.IsCustomLegend = false;
            this.bTeeChart1.IsGradient = true;
            this.bTeeChart1.IsKeepToolTip = false;
            this.bTeeChart1.IsMoveToolTip = true;
            this.bTeeChart1.IsShowByGroup = false;
            this.bTeeChart1.IsUseAutomaticAxisOfTeeChart = false;
            this.bTeeChart1.IsUseChartEditor = true;
            this.bTeeChart1.IsVisibleLegendScrollBar = false;
            this.bTeeChart1.IsVisibleShadow = true;
            this.bTeeChart1.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.bTeeChart1.LegendVisible = true;
            this.bTeeChart1.Location = new System.Drawing.Point(0, 67);
            this.bTeeChart1.Name = "bTeeChart1";
            this.bTeeChart1.OffsetBottom = 5;
            this.bTeeChart1.OffsetLeft = 5;
            this.bTeeChart1.OffsetRight = 5;
            this.bTeeChart1.OffsetTop = 5;
            this.bTeeChart1.Size = new System.Drawing.Size(448, 277);
            this.bTeeChart1.TabIndex = 99;
            this.bTeeChart1.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.DoNotPaint;
            this.bTeeChart1.XAxisLabel = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.XAxisLabelType.LABEL;
            this.bTeeChart1.ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            // 
            // pnlPCA
            // 
            this.pnlPCA.BottomImage = null;
            this.pnlPCA.BottomLeftImage = null;
            this.pnlPCA.BssClass = "ConditionPanel";
            this.pnlPCA.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPCA.DownImage = null;
            this.pnlPCA.Key = "";
            this.pnlPCA.LeftImage = null;
            this.pnlPCA.LeftTopImage = null;
            this.pnlPCA.Location = new System.Drawing.Point(0, 32);
            this.pnlPCA.Name = "pnlPCA";
            this.pnlPCA.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlPCA.RightBottomImage = null;
            this.pnlPCA.RightImage = null;
            this.pnlPCA.Size = new System.Drawing.Size(448, 35);
            this.pnlPCA.TabIndex = 98;
            this.pnlPCA.TopImage = null;
            this.pnlPCA.TopRightImage = null;
            this.pnlPCA.UpImage = null;
            // 
            // BaseSPCChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "BaseSPCChart";
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

       
    
        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart bTeeChart1;
        protected BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel pnlPCA;
    }
}
