namespace BISTel.eSPC.Page.Common
{
    partial class SPCChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCChart));
            this.bcpnl_ChartBase = new System.Windows.Forms.Panel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.bplInformation = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bapnl_Button = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnListChart = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bcpnl_ChartBase.SuspendLayout();
            this.bplInformation.SuspendLayout();
            this.bapnl_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // bcpnl_ChartBase
            // 
            this.bcpnl_ChartBase.BackColor = System.Drawing.Color.White;
            this.bcpnl_ChartBase.Controls.Add(this.pnlChart);
            this.bcpnl_ChartBase.Controls.Add(this.splitter1);
            this.bcpnl_ChartBase.Controls.Add(this.bplInformation);
            this.bcpnl_ChartBase.Controls.Add(this.bapnl_Button);
            this.bcpnl_ChartBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcpnl_ChartBase.Location = new System.Drawing.Point(0, 0);
            this.bcpnl_ChartBase.Name = "bcpnl_ChartBase";
            this.bcpnl_ChartBase.Size = new System.Drawing.Size(1190, 599);
            this.bcpnl_ChartBase.TabIndex = 6;
            // 
            // pnlChart
            // 
            this.pnlChart.AutoScroll = true;
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 33);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(877, 566);
            this.pnlChart.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(877, 33);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 566);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // bplInformation
            // 
            this.bplInformation.AutoScroll = true;
            this.bplInformation.BLabelAutoPadding = true;
            this.bplInformation.BssClass = "DataArrPanel";
            this.bplInformation.Controls.Add(this.bTitlePanel1);
            this.bplInformation.Dock = System.Windows.Forms.DockStyle.Right;
            this.bplInformation.IsCondition = true;
            this.bplInformation.Key = "";
            this.bplInformation.Location = new System.Drawing.Point(887, 33);
            this.bplInformation.Name = "bplInformation";
            this.bplInformation.Padding_Bottom = 5;
            this.bplInformation.Padding_Left = 5;
            this.bplInformation.Padding_Right = 5;
            this.bplInformation.Padding_Top = 5;
            this.bplInformation.Size = new System.Drawing.Size(303, 566);
            this.bplInformation.Space = 5;
            this.bplInformation.TabIndex = 5;
            this.bplInformation.Title = "";
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitlePanel1.ImageBottom = null;
            this.bTitlePanel1.ImageBottomRight = null;
            this.bTitlePanel1.ImageClose = null;
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
            this.bTitlePanel1.MaxResizable = true;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(303, 566);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "Information";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            this.bTitlePanel1.MinimizedPanel += new System.EventHandler(this.bTitPanelInformation_MinimizedPanel);
            this.bTitlePanel1.MaximizedPanel += new System.EventHandler(this.bTitPanelInformation_MaximizedPanel);

            // 
            // bapnl_Button
            // 
            this.bapnl_Button.BackColor = System.Drawing.Color.White;
            this.bapnl_Button.BLabelAutoPadding = true;
            this.bapnl_Button.BssClass = "TitleArrPanel";
            this.bapnl_Button.Controls.Add(this.panel1);
            this.bapnl_Button.Controls.Add(this.bbtnListChart);
            this.bapnl_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.bapnl_Button.IsCondition = true;
            this.bapnl_Button.Key = "";
            this.bapnl_Button.Location = new System.Drawing.Point(0, 0);
            this.bapnl_Button.Name = "bapnl_Button";
            this.bapnl_Button.Padding_Bottom = 5;
            this.bapnl_Button.Padding_Left = 5;
            this.bapnl_Button.Padding_Right = 5;
            this.bapnl_Button.Padding_Top = 5;
            this.bapnl_Button.Size = new System.Drawing.Size(1190, 33);
            this.bapnl_Button.Space = 5;
            this.bapnl_Button.TabIndex = 3;
            this.bapnl_Button.Title = "";
            // 
            // bbtnListChart
            // 
            this.bbtnListChart.BackColor = System.Drawing.Color.White;
            this.bbtnListChart.BssClass = "";
            this.bbtnListChart.ButtonHorizontalSpacing = 1;
            this.bbtnListChart.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnListChart.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnListChart.HorizontalMarginSpacing = 5;
            this.bbtnListChart.IsCondition = false;
            this.bbtnListChart.Location = new System.Drawing.Point(890, 0);
            this.bbtnListChart.MarginSpace = 5;
            this.bbtnListChart.Name = "bbtnListChart";
            this.bbtnListChart.Size = new System.Drawing.Size(300, 33);
            this.bbtnListChart.TabIndex = 0;
            this.bbtnListChart.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnListChart_ButtonClick);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(890, 33);
            this.panel1.TabIndex = 1;
            // 
            // SPCChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bcpnl_ChartBase);
            this.Name = "SPCChart";
            this.Size = new System.Drawing.Size(1190, 599);
            this.Resize += new System.EventHandler(this.SPCChart_Resize);
            this.bcpnl_ChartBase.ResumeLayout(false);
            this.bplInformation.ResumeLayout(false);
            this.bapnl_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bcpnl_ChartBase;
        private System.Windows.Forms.Panel pnlChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnl_Button;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnListChart;
        private System.Windows.Forms.Splitter splitter1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bplInformation;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private System.Windows.Forms.Panel panel1;

    }
}
