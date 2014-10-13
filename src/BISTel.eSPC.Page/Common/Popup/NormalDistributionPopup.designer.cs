namespace BISTel.eSPC.Page.Common
{
    partial class NormalDistributionPopup
    {
        /// <summary>
        /// Required designer ChartVariable.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NormalDistributionPopup));
            this.pnlChartRowDataPopup = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ndcUC = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.NormalDistributionChartUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bTabControl1 = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.hstUC = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.HistogramChartUC();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.bpcUC = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.BoxPlotChartUC();
            this.pnlChartRowDataPopup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.bapnlButton.SuspendLayout();
            this.bTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Size = new System.Drawing.Size(806, 506);
            // 
            // pnlChartRowDataPopup
            // 
            this.pnlChartRowDataPopup.Controls.Add(this.panel2);
            this.pnlChartRowDataPopup.Controls.Add(this.panel1);
            this.pnlChartRowDataPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartRowDataPopup.Location = new System.Drawing.Point(6, 27);
            this.pnlChartRowDataPopup.Name = "pnlChartRowDataPopup";
            this.pnlChartRowDataPopup.Size = new System.Drawing.Size(806, 506);
            this.pnlChartRowDataPopup.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bTabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel2.Size = new System.Drawing.Size(806, 471);
            this.panel2.TabIndex = 4;
            // 
            // ndcUC
            // 
            this.ndcUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ndcUC.Location = new System.Drawing.Point(3, 3);
            this.ndcUC.LowerSpeclimit = 0F;
            this.ndcUC.Mean = 0F;
            this.ndcUC.Name = "ndcUC";
            this.ndcUC.SampleCount = 0;
            this.ndcUC.Size = new System.Drawing.Size(782, 430);
            this.ndcUC.StandardDeviation = 0F;
            this.ndcUC.TabIndex = 0;
            this.ndcUC.Target = 0F;
            this.ndcUC.UpperSpecLimit = 0F;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bapnlButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 471);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 35);
            this.panel1.TabIndex = 3;
            // 
            // bapnlButton
            // 
            this.bapnlButton.BLabelAutoPadding = true;
            this.bapnlButton.BssClass = "DataArrPanel";
            this.bapnlButton.Controls.Add(this.bbtnCancel);
            this.bapnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlButton.IsCondition = false;
            this.bapnlButton.Key = "";
            this.bapnlButton.Location = new System.Drawing.Point(0, 0);
            this.bapnlButton.Name = "bapnlButton";
            this.bapnlButton.Padding_Bottom = 5;
            this.bapnlButton.Padding_Left = 5;
            this.bapnlButton.Padding_Right = 5;
            this.bapnlButton.Padding_Top = 5;
            this.bapnlButton.Size = new System.Drawing.Size(806, 35);
            this.bapnlButton.Space = 5;
            this.bapnlButton.TabIndex = 2;
            this.bapnlButton.Title = "";
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.AutoButtonSize = false;
            this.bbtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCancel.BackgroundImage")));
            this.bbtnCancel.BssClass = "ConditionButton";
            this.bbtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCancel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCancel.IsMultiLanguage = true;
            this.bbtnCancel.Key = "FDC_POPUP_BUTTON_CLOSE";
            this.bbtnCancel.Location = new System.Drawing.Point(717, 5);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 25);
            this.bbtnCancel.TabIndex = 2;
            this.bbtnCancel.TabStop = false;
            this.bbtnCancel.Text = "Close";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bTabControl1
            // 
            this.bTabControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(161)))), ((int)(((byte)(184)))));
            this.bTabControl1.BssClass = "";
            this.bTabControl1.CloseImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseImage")));
            this.bTabControl1.CloseRollOverImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseRollOverImage")));
            this.bTabControl1.Controls.Add(this.tabPage1);
            this.bTabControl1.Controls.Add(this.tabPage2);
            this.bTabControl1.Controls.Add(this.tabPage3);
            this.bTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bTabControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTabControl1.FontSizeSet = "11";
            this.bTabControl1.IsDrawBorder = true;
            this.bTabControl1.IsFillBorder = false;
            this.bTabControl1.IsReleaseAlignmentRestrict = false;
            this.bTabControl1.IsSpace = true;
            this.bTabControl1.Key = "";
            this.bTabControl1.Location = new System.Drawing.Point(5, 5);
            this.bTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.bTabControl1.Name = "bTabControl1";
            this.bTabControl1.SelectedIndex = 0;
            this.bTabControl1.Size = new System.Drawing.Size(796, 466);
            this.bTabControl1.TabBackColor = System.Drawing.Color.Transparent;
            this.bTabControl1.TabBackImage = null;
            this.bTabControl1.TabIndex = 1;
            this.bTabControl1.TabPageClose = false;
            this.bTabControl1.TabSelectFontColor = System.Drawing.Color.White;
            this.bTabControl1.TabSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectLeftImage")));
            this.bTabControl1.TabSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectMiddleImage")));
            this.bTabControl1.TabSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectRightImage")));
            this.bTabControl1.TabSelectRollOverFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectLeftImage")));
            this.bTabControl1.TabUnSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectMiddleImage")));
            this.bTabControl1.TabUnSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectRightImage")));
            this.bTabControl1.TabUnSelectRollOverFontColor = System.Drawing.Color.White;
            this.bTabControl1.TextLeftPosition = 5;
            this.bTabControl1.UseShadowFont = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ndcUC);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(788, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Normal Distribution      ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.hstUC);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(788, 436);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Histogram      ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // hstUC
            // 
            this.hstUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hstUC.IsNumberTypeOfYAxis = false;
            this.hstUC.Location = new System.Drawing.Point(3, 3);
            this.hstUC.LowerSpeclimit = 0F;
            this.hstUC.Mean = 0F;
            this.hstUC.Name = "hstUC";
            this.hstUC.SampleCount = 0;
            this.hstUC.Size = new System.Drawing.Size(782, 430);
            this.hstUC.StandardDeviation = 0F;
            this.hstUC.TabIndex = 1;
            this.hstUC.Target = 0F;
            this.hstUC.UpperSpecLimit = 0F;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.bpcUC);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(788, 436);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "BoxPlot      ";
            // 
            // bpcUC
            // 
            this.bpcUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bpcUC.Location = new System.Drawing.Point(0, 0);
            this.bpcUC.Name = "bpcUC";
            this.bpcUC.OffsetMax = 15D;
            this.bpcUC.OffsetMin = 15D;
            this.bpcUC.Size = new System.Drawing.Size(788, 436);
            this.bpcUC.TabIndex = 1;
            // 
            // NormalDistributionPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(818, 539);
            this.ContentsAreaMinHeight = 500;
            this.ContentsAreaMinWidth = 800;
            this.Controls.Add(this.pnlChartRowDataPopup);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "NormalDistributionPopup";
            this.Resizable = true;
            this.Controls.SetChildIndex(this.pnlContentsArea, 0);
            this.Controls.SetChildIndex(this.pnlChartRowDataPopup, 0);
            this.pnlChartRowDataPopup.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.bTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.Panel pnlChartRowDataPopup;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.NormalDistributionChartUC ndcUC;
        private PeakPerformance.Client.BISTelControl.BTabControl bTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.HistogramChartUC hstUC;
        private System.Windows.Forms.TabPage tabPage3;
        public PeakPerformance.Client.BISTelControl.BTeeCharts.BStatChart.BoxPlotChartUC bpcUC;
    }
}