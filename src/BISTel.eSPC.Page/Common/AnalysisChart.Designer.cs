namespace BISTel.eSPC.Page.Common
{
    partial class AnalysisChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisChart));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.bTeeChart1 = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart(this.components);
            this.bButtonList1 = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bButtonList1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
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
            this.bTeeChart1.IsHighlightSeries = false;
            this.bTeeChart1.IsKeepToolTip = false;
            this.bTeeChart1.IsMoveToolTip = true;
            this.bTeeChart1.IsShowByGroup = false;
            this.bTeeChart1.IsUseAutomaticAxisOfTeeChart = false;
            this.bTeeChart1.IsUseChartEditor = true;
            this.bTeeChart1.IsVisibleLegendScrollBar = true;
            this.bTeeChart1.IsVisibleShadow = true;
            this.bTeeChart1.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.bTeeChart1.LegendVisible = true;
            this.bTeeChart1.Location = new System.Drawing.Point(0, 28);
            this.bTeeChart1.Name = "bTeeChart1";
            this.bTeeChart1.OffsetBottom = 5;
            this.bTeeChart1.OffsetLeft = 5;
            this.bTeeChart1.OffsetRight = 5;
            this.bTeeChart1.OffsetTop = 5;
            this.bTeeChart1.Size = new System.Drawing.Size(557, 340);
            this.bTeeChart1.TabIndex = 101;
            this.bTeeChart1.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.DoNotPaint;
            this.bTeeChart1.XAxisLabel = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.XAxisLabelType.LABEL;
            this.bTeeChart1.ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            // 
            // bButtonList1
            // 
            this.bButtonList1.BssClass = "";
            this.bButtonList1.ButtonHorizontalSpacing = 1;
            this.bButtonList1.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bButtonList1.Controls.Add(this.bSpread1);
            this.bButtonList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bButtonList1.HorizontalMarginSpacing = 5;
            this.bButtonList1.IsCondition = false;
            this.bButtonList1.Location = new System.Drawing.Point(0, 0);
            this.bButtonList1.MarginSpace = 5;
            this.bButtonList1.Name = "bButtonList1";
            this.bButtonList1.Size = new System.Drawing.Size(557, 28);
            this.bButtonList1.TabIndex = 102;
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
            columnInfo2.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.CheckBoxFields")));
            columnInfo2.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.ComboFields")));
            columnInfo2.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo2.DefaultValue")));
            columnInfo2.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.KeyFields")));
            columnInfo2.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.MandatoryFields")));
            columnInfo2.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo2.SaveTableInfo")));
            columnInfo2.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.UniqueFields")));
            this.bSpread1.columnInformation = columnInfo2;
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
            this.bSpread1.Location = new System.Drawing.Point(3, 12);
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(30, 10);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 0;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance2;
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
            // AnalysisChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bTeeChart1);
            this.Controls.Add(this.bButtonList1);
            this.Name = "AnalysisChart";
            this.Size = new System.Drawing.Size(557, 368);
            this.bButtonList1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart bTeeChart1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bButtonList1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
    }
}
