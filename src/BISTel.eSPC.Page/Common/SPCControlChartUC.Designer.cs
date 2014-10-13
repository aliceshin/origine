namespace BISTel.eSPC.Page.Common
{
    partial class SPCControlChartUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCControlChartUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.bbtnListChart = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bapnl_Button = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bCheckCombo1 = new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo();
            this.bcpnl_ChartBase = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bShowRawData = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bapnl_Button.SuspendLayout();
            this.bArrangePanel1.SuspendLayout();
            this.bcpnl_ChartBase.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
            this.SuspendLayout();
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
            this.bbtnListChart.Location = new System.Drawing.Point(534, 0);
            this.bbtnListChart.MarginSpace = 5;
            this.bbtnListChart.Name = "bbtnListChart";
            this.bbtnListChart.Size = new System.Drawing.Size(191, 33);
            this.bbtnListChart.TabIndex = 0;
            this.bbtnListChart.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnListChart_ButtonClick);
            // 
            // bapnl_Button
            // 
            this.bapnl_Button.BackColor = System.Drawing.Color.White;
            this.bapnl_Button.BLabelAutoPadding = true;
            this.bapnl_Button.BssClass = "TitleArrPanel";
            this.bapnl_Button.Controls.Add(this.bArrangePanel1);
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
            this.bapnl_Button.Size = new System.Drawing.Size(725, 33);
            this.bapnl_Button.Space = 5;
            this.bapnl_Button.TabIndex = 3;
            this.bapnl_Button.Title = "";
            // 
            // bArrangePanel1
            // 
            this.bArrangePanel1.BLabelAutoPadding = true;
            this.bArrangePanel1.BssClass = "TitlePanel";
            this.bArrangePanel1.Controls.Add(this.bShowRawData);
            this.bArrangePanel1.Controls.Add(this.bLabel1);
            this.bArrangePanel1.Controls.Add(this.bCheckCombo1);
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bArrangePanel1.IsCondition = true;
            this.bArrangePanel1.Key = "";
            this.bArrangePanel1.Location = new System.Drawing.Point(0, 0);
            this.bArrangePanel1.Name = "bArrangePanel1";
            this.bArrangePanel1.Padding_Bottom = 5;
            this.bArrangePanel1.Padding_Left = 5;
            this.bArrangePanel1.Padding_Right = 5;
            this.bArrangePanel1.Padding_Top = 5;
            this.bArrangePanel1.Size = new System.Drawing.Size(534, 33);
            this.bArrangePanel1.Space = 5;
            this.bArrangePanel1.TabIndex = 1;
            this.bArrangePanel1.Title = "";
            // 
            // bCheckCombo1
            // 
            this.bCheckCombo1.BackColor = System.Drawing.Color.White;
            this.bCheckCombo1.BssClass = "";
            this.bCheckCombo1.CausesValidation = false;
            this.bCheckCombo1.DataSource = null;
            this.bCheckCombo1.DisplayMember = "";
            this.bCheckCombo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bCheckCombo1.IsIncludeAll = false;
            this.bCheckCombo1.Key = "";
            this.bCheckCombo1.Location = new System.Drawing.Point(5, 6);
            this.bCheckCombo1.Name = "bCheckCombo1";
            this.bCheckCombo1.Size = new System.Drawing.Size(286, 21);
            this.bCheckCombo1.TabIndex = 0;
            this.bCheckCombo1.ValueMember = "";
            this.bCheckCombo1.TextChanged += new System.EventHandler(this.bCheckCombo1_TextChanged);
            this.bCheckCombo1.CheckCombo_Select += new System.EventHandler(this.bCheckCombo1_CheckCombo_Select);
            // 
            // bcpnl_ChartBase
            // 
            this.bcpnl_ChartBase.BackColor = System.Drawing.Color.White;
            this.bcpnl_ChartBase.Controls.Add(this.splitContainer1);
            this.bcpnl_ChartBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcpnl_ChartBase.Location = new System.Drawing.Point(0, 0);
            this.bcpnl_ChartBase.Name = "bcpnl_ChartBase";
            this.bcpnl_ChartBase.Size = new System.Drawing.Size(991, 546);
            this.bcpnl_ChartBase.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlChart);
            this.splitContainer1.Panel1.Controls.Add(this.bapnl_Button);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bsprData);
            this.splitContainer1.Size = new System.Drawing.Size(991, 546);
            this.splitContainer1.SplitterDistance = 725;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 4;
            // 
            // pnlChart
            // 
            this.pnlChart.AutoScroll = true;
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 33);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(725, 513);
            this.pnlChart.TabIndex = 4;
            // 
            // bsprData
            // 
            this.bsprData.About = "3.0.2005.2005";
            this.bsprData.AccessibleDescription = "";
            this.bsprData.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprData.AddtionalCodeList")));
            this.bsprData.AllowNewRow = true;
            this.bsprData.AutoClipboard = false;
            this.bsprData.AutoGenerateColumns = false;
            this.bsprData.BssClass = "";
            this.bsprData.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprData.ColFronzen = 0;
            columnInfo2.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.CheckBoxFields")));
            columnInfo2.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.ComboFields")));
            columnInfo2.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo2.DefaultValue")));
            columnInfo2.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.KeyFields")));
            columnInfo2.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.MandatoryFields")));
            columnInfo2.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo2.SaveTableInfo")));
            columnInfo2.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.UniqueFields")));
            this.bsprData.columnInformation = columnInfo2;
            this.bsprData.ComboEnable = true;
            this.bsprData.DataAutoHeadings = false;
            this.bsprData.DataSet = null;
            this.bsprData.DateToDateTimeFormat = false;
            this.bsprData.DefaultDeleteValue = true;
            this.bsprData.DisplayColumnHeader = true;
            this.bsprData.DisplayRowHeader = true;
            this.bsprData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprData.EditModeReplace = true;
            this.bsprData.FilterVisible = false;
            this.bsprData.HeadHeight = 20F;
            this.bsprData.IsCellCopy = false;
            this.bsprData.IsMultiLanguage = true;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(0, 0);
            this.bsprData.Name = "bsprData";
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(263, 546);
            this.bsprData.TabIndex = 0;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprData.TextTipAppearance = tipAppearance2;
            this.bsprData.UseCheckAll = false;
            this.bsprData.UseCommandIcon = false;
            this.bsprData.UseFilter = false;
            this.bsprData.UseGeneralContextMenu = true;
            this.bsprData.UseHeadColor = false;
            this.bsprData.UseOriginalEvent = false;
            this.bsprData.UseSpreadEdit = true;
            this.bsprData.UseWidthMemory = true;
            this.bsprData.WhenDeleteUseModify = false;
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
            // 
            // bLabel1
            // 
            this.bLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.ForeColor = System.Drawing.Color.Black;
            this.bLabel1.Image = global::BISTel.eSPC.Page.Properties.Resources.bullet_01;
            this.bLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(296, 5);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(10, 23);
            this.bLabel1.TabIndex = 29;
            // 
            // bShowRawData
            // 
            this.bShowRawData.BssClass = "";
            this.bShowRawData.Enabled = false;
            this.bShowRawData.IsMultiLanguage = true;
            this.bShowRawData.Key = "";
            this.bShowRawData.Location = new System.Drawing.Point(311, 4);
            this.bShowRawData.Name = "bShowRawData";
            this.bShowRawData.Size = new System.Drawing.Size(162, 24);
            this.bShowRawData.TabIndex = 30;
            this.bShowRawData.Text = "Show raw data on/off flag";
            this.bShowRawData.UseVisualStyleBackColor = true;
            // 
            // SPCControlChartUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bcpnl_ChartBase);
            this.Name = "SPCControlChartUC";
            this.Size = new System.Drawing.Size(991, 546);
            this.bapnl_Button.ResumeLayout(false);
            this.bArrangePanel1.ResumeLayout(false);
            this.bcpnl_ChartBase.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

     
        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnListChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnl_Button;
        private System.Windows.Forms.Panel bcpnl_ChartBase;
        private System.Windows.Forms.SplitContainer splitContainer1;

        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bArrangePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        private FarPoint.Win.Spread.SheetView bsprData_Sheet1;
        private System.Windows.Forms.Panel pnlChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo bCheckCombo1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bShowRawData;

    }
}
