namespace BISTel.eSPC.Page.Report.Popup
{
    partial class SPCTimeBaseRawPopup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.SeriesTypeInfo seriesTypeInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.SeriesTypeInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCTimeBaseRawPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlChartRowDataPopup = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bTChrt_RawData = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlRawData = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlChartRowDataPopup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.bapnlRawData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
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
            this.pnlChartRowDataPopup.Controls.Add(this.bapnlRawData);
            this.pnlChartRowDataPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartRowDataPopup.Location = new System.Drawing.Point(6, 27);
            this.pnlChartRowDataPopup.Name = "pnlChartRowDataPopup";
            this.pnlChartRowDataPopup.Size = new System.Drawing.Size(806, 506);
            this.pnlChartRowDataPopup.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bTChrt_RawData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel2.Size = new System.Drawing.Size(806, 441);
            this.panel2.TabIndex = 4;
            // 
            // bTChrt_RawData
            // 
            this.bTChrt_RawData.AutoScroll = true;
            this.bTChrt_RawData.AxisDateTimeFormat = "yyyy-MM-dd";
            this.bTChrt_RawData.AxisHoriGridLineCount = 10;
            this.bTChrt_RawData.ClickAction = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.ChartClickAction.DEFAULT;
            this.bTChrt_RawData.ClickActionCustom = null;
            this.bTChrt_RawData.DefaultToolTipInfo = null;
            this.bTChrt_RawData.DivideHeightAtShowGroup = 2;
            this.bTChrt_RawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTChrt_RawData.HeaderText = "";
            this.bTChrt_RawData.IsApplyAllCSG = true;
            this.bTChrt_RawData.IsAutoValueFormatLeftAxis = false;
            this.bTChrt_RawData.IsAutoValueFormatRightAxis = false;
            this.bTChrt_RawData.IsCustomLegend = false;
            this.bTChrt_RawData.IsGradient = true;
            this.bTChrt_RawData.IsHighlightSeries = false;
            this.bTChrt_RawData.IsKeepToolTip = false;
            this.bTChrt_RawData.IsMoveToolTip = true;
            this.bTChrt_RawData.IsShowByGroup = false;
            this.bTChrt_RawData.IsUseAutomaticAxisOfTeeChart = false;
            this.bTChrt_RawData.IsUseChartEditor = true;
            this.bTChrt_RawData.IsVisibleLegendScrollBar = true;
            this.bTChrt_RawData.IsVisibleShadow = true;
            this.bTChrt_RawData.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.bTChrt_RawData.LegendVisible = true;
            this.bTChrt_RawData.Location = new System.Drawing.Point(5, 5);
            this.bTChrt_RawData.Name = "bTChrt_RawData";
            this.bTChrt_RawData.OffsetBottom = 5;
            this.bTChrt_RawData.OffsetLeft = 5;
            this.bTChrt_RawData.OffsetRight = 5;
            this.bTChrt_RawData.OffsetTop = 5;
            seriesTypeInfo1.LineWidth = 1;
            seriesTypeInfo1.PointerHeight = 2;
            seriesTypeInfo1.PointerStyle = null;
            seriesTypeInfo1.PointerWidth = 2;
            seriesTypeInfo1.StyleType = "Default";
            this.bTChrt_RawData.SeriesTypeInformation = seriesTypeInfo1;
            this.bTChrt_RawData.Size = new System.Drawing.Size(796, 436);
            this.bTChrt_RawData.TabIndex = 100;
            this.bTChrt_RawData.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.DoNotPaint;
            this.bTChrt_RawData.XAxisLabel = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.XAxisLabelType.LABEL;
            this.bTChrt_RawData.ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bbtnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 471);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 35);
            this.panel1.TabIndex = 3;
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
            this.bbtnCancel.Location = new System.Drawing.Point(720, 6);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 25);
            this.bbtnCancel.TabIndex = 3;
            this.bbtnCancel.TabStop = false;
            this.bbtnCancel.Text = "Close";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bapnlRawData
            // 
            this.bapnlRawData.BLabelAutoPadding = true;
            this.bapnlRawData.BssClass = "DataArrPanel";
            this.bapnlRawData.Controls.Add(this.bbtnList);
            this.bapnlRawData.Dock = System.Windows.Forms.DockStyle.Top;
            this.bapnlRawData.IsCondition = true;
            this.bapnlRawData.Key = "";
            this.bapnlRawData.Location = new System.Drawing.Point(0, 0);
            this.bapnlRawData.Name = "bapnlRawData";
            this.bapnlRawData.Padding_Bottom = 5;
            this.bapnlRawData.Padding_Left = 5;
            this.bapnlRawData.Padding_Right = 5;
            this.bapnlRawData.Padding_Top = 5;
            this.bapnlRawData.Size = new System.Drawing.Size(806, 30);
            this.bapnlRawData.Space = 5;
            this.bapnlRawData.TabIndex = 0;
            this.bapnlRawData.Title = "";
            // 
            // bbtnList
            // 
            this.bbtnList.BssClass = "";
            this.bbtnList.ButtonHorizontalSpacing = 1;
            this.bbtnList.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnList.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnList.HorizontalMarginSpacing = 5;
            this.bbtnList.IsCondition = false;
            this.bbtnList.Location = new System.Drawing.Point(289, 0);
            this.bbtnList.MarginSpace = 5;
            this.bbtnList.Name = "bbtnList";
            this.bbtnList.Size = new System.Drawing.Size(517, 30);
            this.bbtnList.TabIndex = 7;
            // 
            // bSpread1
            // 
            this.bSpread1.About = "3.0.2005.2005";
            this.bSpread1.AccessibleDescription = "";
            this.bSpread1.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bSpread1.AddtionalCodeList")));
            this.bSpread1.AllowNewRow = true;
            this.bSpread1.AutoClipboard = false;
            this.bSpread1.AutoGenerateColumns = false;
            this.bSpread1.BssClass = "";
            this.bSpread1.ClickPos = new System.Drawing.Point(0, 0);
            this.bSpread1.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bSpread1.columnInformation = columnInfo1;
            this.bSpread1.ComboEnable = true;
            this.bSpread1.DataAutoHeadings = false;
            this.bSpread1.DataSet = null;
            this.bSpread1.DateToDateTimeFormat = false;
            this.bSpread1.DefaultDeleteValue = true;
            this.bSpread1.DisplayColumnHeader = true;
            this.bSpread1.DisplayRowHeader = true;
            this.bSpread1.EditModeReplace = true;
            this.bSpread1.FilterVisible = false;
            this.bSpread1.HeadHeight = 20F;
            this.bSpread1.IsCellCopy = false;
            this.bSpread1.IsMultiLanguage = true;
            this.bSpread1.IsReport = false;
            this.bSpread1.Key = "";
            this.bSpread1.Location = new System.Drawing.Point(392, 264);
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(34, 10);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 16;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance1;
            this.bSpread1.UseCheckAll = false;
            this.bSpread1.UseCommandIcon = false;
            this.bSpread1.UseFilter = false;
            this.bSpread1.UseGeneralContextMenu = true;
            this.bSpread1.UseHeadColor = false;
            this.bSpread1.UseOriginalEvent = false;
            this.bSpread1.UseSpreadEdit = true;
            this.bSpread1.UseWidthMemory = true;
            this.bSpread1.Visible = false;
            this.bSpread1.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // SPCTimeBaseRawPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(818, 539);
            this.ContentsAreaMinHeight = 500;
            this.ContentsAreaMinWidth = 800;
            this.Controls.Add(this.bSpread1);
            this.Controls.Add(this.pnlChartRowDataPopup);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "SPCTimeBaseRawPopup";
            this.Resizable = true;
            this.Controls.SetChildIndex(this.pnlContentsArea, 0);
            this.Controls.SetChildIndex(this.pnlChartRowDataPopup, 0);
            this.Controls.SetChildIndex(this.bSpread1, 0);
            this.pnlChartRowDataPopup.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.bapnlRawData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
       

        #endregion

        private System.Windows.Forms.Panel pnlChartRowDataPopup;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlRawData;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
        private BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart bTChrt_RawData;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;

    }
}