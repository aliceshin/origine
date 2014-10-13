namespace BISTel.eSPC.Page.Report
{
    partial class SPCControlChartModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCControlChartModel));
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bapChart = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bTitlePanel1.SuspendLayout();
            this.bArrangePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bapChart);
            this.bTitlePanel1.Controls.Add(this.bArrangePanel1);
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
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(759, 664);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "SPC Control Chart";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // bapChart
            // 
            this.bapChart.BLabelAutoPadding = true;
            this.bapChart.BssClass = "DataArrPanel";
            this.bapChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bapChart.IsCondition = true;
            this.bapChart.Key = "";
            this.bapChart.Location = new System.Drawing.Point(0, 69);
            this.bapChart.Name = "bapChart";
            this.bapChart.Padding_Bottom = 5;
            this.bapChart.Padding_Left = 5;
            this.bapChart.Padding_Right = 5;
            this.bapChart.Padding_Top = 5;
            this.bapChart.Size = new System.Drawing.Size(759, 595);
            this.bapChart.Space = 5;
            this.bapChart.TabIndex = 5;
            this.bapChart.Title = "";
            // 
            // bArrangePanel1
            // 
            this.bArrangePanel1.BLabelAutoPadding = true;
            this.bArrangePanel1.BssClass = "DataArrPanel";
            this.bArrangePanel1.Controls.Add(this.bbtnList);
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bArrangePanel1.IsCondition = true;
            this.bArrangePanel1.Key = "";
            this.bArrangePanel1.Location = new System.Drawing.Point(0, 35);
            this.bArrangePanel1.Name = "bArrangePanel1";
            this.bArrangePanel1.Padding_Bottom = 5;
            this.bArrangePanel1.Padding_Left = 5;
            this.bArrangePanel1.Padding_Right = 5;
            this.bArrangePanel1.Padding_Top = 5;
            this.bArrangePanel1.Size = new System.Drawing.Size(759, 34);
            this.bArrangePanel1.Space = 5;
            this.bArrangePanel1.TabIndex = 4;
            this.bArrangePanel1.Title = "";
            // 
            // bbtnList
            // 
            this.bbtnList.BssClass = "";
            this.bbtnList.ButtonHorizontalSpacing = 1;
            this.bbtnList.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnList.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnList.HorizontalMarginSpacing = 5;
            this.bbtnList.IsCondition = false;
            this.bbtnList.Location = new System.Drawing.Point(388, 0);
            this.bbtnList.MarginSpace = 5;
            this.bbtnList.Name = "bbtnList";
            this.bbtnList.Size = new System.Drawing.Size(371, 34);
            this.bbtnList.TabIndex = 0;
            // 
            // SPCControlChartUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "SPCControlChartUC";
            this.Size = new System.Drawing.Size(759, 664);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.bArrangePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bArrangePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapChart;



    }
}
