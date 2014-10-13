namespace BISTel.eSPC.Page.Report
{
    partial class SPCChartViewUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCChartViewUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bapChart = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bcpnl_ChartBase = new System.Windows.Forms.Panel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.bplInformation = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bTitPanelInformation = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bsprContext = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprContext_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bapnl_Button = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bbtnListChart = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bTitlePanel1.SuspendLayout();
            this.bapChart.SuspendLayout();
            this.bcpnl_ChartBase.SuspendLayout();
            this.bplInformation.SuspendLayout();
            this.bTitPanelInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext_Sheet1)).BeginInit();
            this.bapnl_Button.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.AutoScroll = true;
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bapChart);
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
            this.bTitlePanel1.ImagePanelMax = null;
            this.bTitlePanel1.ImagePanelMin = null;
            this.bTitlePanel1.ImagePanelNormal = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.MaxVisibleStateForMaxNormal = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.SelectedColor = System.Drawing.Color.Blue;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.ShowMaxNormalButton = false;
            this.bTitlePanel1.ShowMinButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(906, 613);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "SPC Control Chart";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            this.bTitlePanel1.Resize += new System.EventHandler(this.bTitlePanel1_Resize);
            // 
            // bapChart
            // 
            this.bapChart.AutoScroll = true;
            this.bapChart.BLabelAutoPadding = true;
            this.bapChart.BssClass = "DataArrPanel";
            this.bapChart.Controls.Add(this.bcpnl_ChartBase);
            this.bapChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bapChart.IsCondition = true;
            this.bapChart.Key = "";
            this.bapChart.Location = new System.Drawing.Point(0, 32);
            this.bapChart.Name = "bapChart";
            this.bapChart.Padding_Bottom = 5;
            this.bapChart.Padding_Left = 5;
            this.bapChart.Padding_Right = 5;
            this.bapChart.Padding_Top = 5;
            this.bapChart.Size = new System.Drawing.Size(906, 581);
            this.bapChart.Space = 5;
            this.bapChart.TabIndex = 5;
            this.bapChart.Title = "";
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
            this.bcpnl_ChartBase.Size = new System.Drawing.Size(906, 581);
            this.bcpnl_ChartBase.TabIndex = 7;
            // 
            // pnlChart
            // 
            this.pnlChart.AutoScroll = true;
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 30);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(541, 551);
            this.pnlChart.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(541, 30);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(12, 551);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // bplInformation
            // 
            this.bplInformation.AutoScroll = true;
            this.bplInformation.BLabelAutoPadding = true;
            this.bplInformation.BssClass = "DataArrPanel";
            this.bplInformation.Controls.Add(this.bTitPanelInformation);
            this.bplInformation.Dock = System.Windows.Forms.DockStyle.Right;
            this.bplInformation.IsCondition = true;
            this.bplInformation.Key = "";
            this.bplInformation.Location = new System.Drawing.Point(553, 30);
            this.bplInformation.Name = "bplInformation";
            this.bplInformation.Padding_Bottom = 5;
            this.bplInformation.Padding_Left = 5;
            this.bplInformation.Padding_Right = 5;
            this.bplInformation.Padding_Top = 5;
            this.bplInformation.Size = new System.Drawing.Size(353, 551);
            this.bplInformation.Space = 5;
            this.bplInformation.TabIndex = 5;
            this.bplInformation.Title = "";
            this.bplInformation.Visible = false;
            // 
            // bTitPanelInformation
            // 
            this.bTitPanelInformation.BssClass = "TitlePanel";
            this.bTitPanelInformation.Controls.Add(this.bsprContext);
            this.bTitPanelInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitPanelInformation.ImageBottom = null;
            this.bTitPanelInformation.ImageBottomRight = null;
            this.bTitPanelInformation.ImageClose = null;
            this.bTitPanelInformation.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTitPanelInformation.ImageLeft")));
            this.bTitPanelInformation.ImageLeftBottom = null;
            this.bTitPanelInformation.ImageLeftFill = null;
            this.bTitPanelInformation.ImageMax = null;
            this.bTitPanelInformation.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTitPanelInformation.ImageMiddle")));
            this.bTitPanelInformation.ImageMin = null;
            this.bTitPanelInformation.ImagePanelMax = null;
            this.bTitPanelInformation.ImagePanelMin = null;
            this.bTitPanelInformation.ImagePanelNormal = null;
            this.bTitPanelInformation.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitPanelInformation.ImageRight")));
            this.bTitPanelInformation.ImageRightFill = null;
            this.bTitPanelInformation.IsPopupMax = false;
            this.bTitPanelInformation.IsSelected = false;
            this.bTitPanelInformation.Key = "";
            this.bTitPanelInformation.Location = new System.Drawing.Point(0, 0);
            this.bTitPanelInformation.MaxControlName = "";
            this.bTitPanelInformation.MaxResizable = true;
            this.bTitPanelInformation.MaxVisibleStateForMaxNormal = false;
            this.bTitPanelInformation.Name = "bTitPanelInformation";
            this.bTitPanelInformation.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.bTitPanelInformation.PaddingTop = 4;
            this.bTitPanelInformation.PopMaxHeight = 0;
            this.bTitPanelInformation.PopMaxWidth = 0;
            this.bTitPanelInformation.RightPadding = 5;
            this.bTitPanelInformation.SelectedColor = System.Drawing.Color.Blue;
            this.bTitPanelInformation.ShowCloseButton = false;
            this.bTitPanelInformation.ShowMaxNormalButton = false;
            this.bTitPanelInformation.ShowMinButton = false;
            this.bTitPanelInformation.Size = new System.Drawing.Size(353, 551);
            this.bTitPanelInformation.TabIndex = 0;
            this.bTitPanelInformation.Title = "OCAP Information";
            this.bTitPanelInformation.TitleColor = System.Drawing.Color.Black;
            this.bTitPanelInformation.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitPanelInformation.TopPadding = 5;
            this.bTitPanelInformation.MaximizedPanel += new System.EventHandler(this.bTitPanelInformation_MaximizedPanel);
            this.bTitPanelInformation.MinimizedPanel += new System.EventHandler(this.bTitPanelInformation_MinimizedPanel);
            // 
            // bsprContext
            // 
            this.bsprContext.About = "3.0.2005.2005";
            this.bsprContext.AccessibleDescription = "";
            this.bsprContext.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.AddFontColor = System.Drawing.Color.Blue;
            this.bsprContext.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprContext.AddtionalCodeList")));
            this.bsprContext.AllowNewRow = true;
            this.bsprContext.AutoClipboard = false;
            this.bsprContext.AutoGenerateColumns = false;
            this.bsprContext.BssClass = "";
            this.bsprContext.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprContext.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprContext.columnInformation = columnInfo1;
            this.bsprContext.ComboEnable = true;
            this.bsprContext.DataAutoHeadings = false;
            this.bsprContext.DataSet = null;
            this.bsprContext.DataSource_V2 = null;
            this.bsprContext.DateToDateTimeFormat = false;
            this.bsprContext.DefaultDeleteValue = true;
            this.bsprContext.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprContext.DisplayColumnHeader = true;
            this.bsprContext.DisplayRowHeader = true;
            this.bsprContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprContext.EditModeReplace = true;
            this.bsprContext.FilterVisible = false;
            this.bsprContext.HeadHeight = 20F;
            this.bsprContext.IsCellCopy = false;
            this.bsprContext.IsEditComboListItem = false;
            this.bsprContext.IsMultiLanguage = true;
            this.bsprContext.IsReport = false;
            this.bsprContext.Key = "";
            this.bsprContext.Location = new System.Drawing.Point(0, 30);
            this.bsprContext.Margin = new System.Windows.Forms.Padding(0);
            this.bsprContext.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprContext.Name = "bsprContext";
            this.bsprContext.RowFronzen = 0;
            this.bsprContext.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprContext.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprContext.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprContext_Sheet1});
            this.bsprContext.Size = new System.Drawing.Size(353, 521);
            this.bsprContext.StyleID = null;
            this.bsprContext.TabIndex = 6;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprContext.TextTipAppearance = tipAppearance1;
            this.bsprContext.UseCheckAll = false;
            this.bsprContext.UseCommandIcon = false;
            this.bsprContext.UseFilter = false;
            this.bsprContext.UseGeneralContextMenu = true;
            this.bsprContext.UseHeadColor = false;
            this.bsprContext.UseMultiCheck = true;
            this.bsprContext.UseOriginalEvent = false;
            this.bsprContext.UseSpreadEdit = true;
            this.bsprContext.UseStatusColor = false;
            this.bsprContext.UseWidthMemory = true;
            this.bsprContext.WhenDeleteUseModify = false;
            // 
            // bsprContext_Sheet1
            // 
            this.bsprContext_Sheet1.Reset();
            this.bsprContext_Sheet1.SheetName = "Sheet1";
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
            this.bapnl_Button.Size = new System.Drawing.Size(906, 30);
            this.bapnl_Button.Space = 5;
            this.bapnl_Button.TabIndex = 3;
            this.bapnl_Button.Title = "";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 30);
            this.panel1.TabIndex = 1;
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
            this.bbtnListChart.Location = new System.Drawing.Point(556, 0);
            this.bbtnListChart.MarginSpace = 5;
            this.bbtnListChart.Name = "bbtnListChart";
            this.bbtnListChart.Size = new System.Drawing.Size(350, 30);
            this.bbtnListChart.TabIndex = 0;
            this.bbtnListChart.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnListChart_ButtonClick);
            // 
            // SPCChartViewUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "SPCChartViewUC";
            this.Size = new System.Drawing.Size(906, 613);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.bapChart.ResumeLayout(false);
            this.bcpnl_ChartBase.ResumeLayout(false);
            this.bplInformation.ResumeLayout(false);
            this.bTitPanelInformation.ResumeLayout(false);
            this.bTitPanelInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext_Sheet1)).EndInit();
            this.bapnl_Button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

  
       

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapChart;
        private System.Windows.Forms.Panel bcpnl_ChartBase;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Splitter splitter1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bplInformation;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitPanelInformation;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnl_Button;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnListChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprContext;
        private FarPoint.Win.Spread.SheetView bsprContext_Sheet1;

        

    }
}
