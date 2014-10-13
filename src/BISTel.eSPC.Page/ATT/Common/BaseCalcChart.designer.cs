namespace BISTel.eSPC.Page.ATT.Common
{
    partial class BaseCalcChart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseCalcChart));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.SeriesTypeInfo seriesTypeInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.SeriesTypeInfo();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bTeeChart1 = new BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart(this.components);
            this.bButtonList1 = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bckbPointMarking = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.bButtonList1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bSpread1);
            this.bTitlePanel1.Controls.Add(this.bTeeChart1);
            this.bTitlePanel1.Controls.Add(this.bButtonList1);
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
            this.bTitlePanel1.ImagePanelMax = null;
            this.bTitlePanel1.ImagePanelMin = null;
            this.bTitlePanel1.ImagePanelNormal = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.Margin = new System.Windows.Forms.Padding(0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.MaxVisibleStateForMaxNormal = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 33, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.ShowMaxNormalButton = false;
            this.bTitlePanel1.ShowMinButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(448, 344);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "Title";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            this.bTitlePanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bTitlePanel1_MouseClick);
            this.bTitlePanel1.MinimizedPanel += new System.EventHandler(this.bTitlePanel1_MinimizedPanel);
            this.bTitlePanel1.MaximizedPanel += new System.EventHandler(this.bTitlePanel1_MaximizedPanel);
            this.bTitlePanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bTitlePanel1_MouseUp);
            this.bTitlePanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bTitlePanel1_MouseMove);
            // 
            // bSpread1
            // 
            this.bSpread1.About = "3.0.2005.2005";
            this.bSpread1.AccessibleDescription = "";
            this.bSpread1.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.AddFontColor = System.Drawing.Color.Blue;
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
            this.bSpread1.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.DeleteFontColor = System.Drawing.Color.Gray;
            this.bSpread1.DisplayColumnHeader = true;
            this.bSpread1.DisplayRowHeader = true;
            this.bSpread1.EditModeReplace = true;
            this.bSpread1.FilterVisible = false;
            this.bSpread1.HeadHeight = 20F;
            this.bSpread1.IsCellCopy = false;
            this.bSpread1.IsMultiLanguage = true;
            this.bSpread1.IsReport = false;
            this.bSpread1.Key = "";
            this.bSpread1.Location = new System.Drawing.Point(3, 333);
            this.bSpread1.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.ModifyFontColor = System.Drawing.Color.Red;
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(30, 11);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance1;
            this.bSpread1.UseCheckAll = false;
            this.bSpread1.UseCommandIcon = false;
            this.bSpread1.UseFilter = false;
            this.bSpread1.UseGeneralContextMenu = true;
            this.bSpread1.UseHeadColor = false;
            this.bSpread1.UseMultiCheck = true;
            this.bSpread1.UseOriginalEvent = false;
            this.bSpread1.UseSpreadEdit = true;
            this.bSpread1.UseStatusColor = false;
            this.bSpread1.UseWidthMemory = true;
            this.bSpread1.Visible = false;
            this.bSpread1.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
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
            this.bTeeChart1.HighlightSeriesTransparency = 80;
            this.bTeeChart1.IsApplyAllCSG = true;
            this.bTeeChart1.IsAutoValueFormatLeftAxis = false;
            this.bTeeChart1.IsAutoValueFormatRightAxis = false;
            this.bTeeChart1.IsCustomLegend = false;
            this.bTeeChart1.IsGradient = true;
            this.bTeeChart1.IsHighlightSeries = false;
            this.bTeeChart1.IsKeepToolTip = false;
            this.bTeeChart1.IsMoveToolTip = true;
            this.bTeeChart1.IsShapeSeriesEventDisable = false;
            this.bTeeChart1.IsShowByGroup = false;
            this.bTeeChart1.IsShowByGroupSetAxisBottom = false;
            this.bTeeChart1.IsShowByGroupZoom = false;
            this.bTeeChart1.IsUndoZoom = false;
            this.bTeeChart1.IsUseAutomaticAxisOfTeeChart = false;
            this.bTeeChart1.IsUseChartEditor = true;
            this.bTeeChart1.IsVisibleLegendScrollBar = true;
            this.bTeeChart1.IsVisibleShadow = true;
            this.bTeeChart1.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.bTeeChart1.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            this.bTeeChart1.LegendTextStyle = Steema.TeeChart.LegendTextStyles.LeftValue;
            this.bTeeChart1.LegendVisible = true;
            this.bTeeChart1.Location = new System.Drawing.Point(0, 61);
            this.bTeeChart1.Name = "bTeeChart1";
            this.bTeeChart1.OffsetBottom = 5;
            this.bTeeChart1.OffsetLeft = 5;
            this.bTeeChart1.OffsetRight = 5;
            this.bTeeChart1.OffsetTop = 5;
            seriesTypeInfo1.LineWidth = 1;
            seriesTypeInfo1.PointerHeight = 2;
            seriesTypeInfo1.PointerStyle = null;
            seriesTypeInfo1.PointerWidth = 2;
            seriesTypeInfo1.StyleType = "Default";
            this.bTeeChart1.SeriesTypeInformation = seriesTypeInfo1;
            this.bTeeChart1.Size = new System.Drawing.Size(448, 283);
            this.bTeeChart1.TabIndex = 99;
            this.bTeeChart1.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.DoNotPaint;
            this.bTeeChart1.XAxisLabel = BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart.XAxisLabelType.LABEL;
            this.bTeeChart1.ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            this.bTeeChart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bTeeChart1_MouseUp);
            this.bTeeChart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bTeeChart1_MouseMove);
            this.bTeeChart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bTeeChart1_MouseDown);
            // 
            // bButtonList1
            // 
            this.bButtonList1.BssClass = "";
            this.bButtonList1.ButtonHorizontalSpacing = 1;
            this.bButtonList1.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bButtonList1.Controls.Add(this.bckbPointMarking);
            this.bButtonList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bButtonList1.HorizontalMarginSpacing = 5;
            this.bButtonList1.IsCondition = false;
            this.bButtonList1.Location = new System.Drawing.Point(0, 33);
            this.bButtonList1.MarginSpace = 5;
            this.bButtonList1.Name = "bButtonList1";
            this.bButtonList1.Size = new System.Drawing.Size(448, 28);
            this.bButtonList1.TabIndex = 100;
            this.bButtonList1.Visible = false;
            // 
            // bckbPointMarking
            // 
            this.bckbPointMarking.BssClass = "";
            this.bckbPointMarking.Dock = System.Windows.Forms.DockStyle.Left;
            this.bckbPointMarking.IsMultiLanguage = true;
            this.bckbPointMarking.Key = "";
            this.bckbPointMarking.Location = new System.Drawing.Point(0, 0);
            this.bckbPointMarking.Name = "bckbPointMarking";
            this.bckbPointMarking.Size = new System.Drawing.Size(104, 28);
            this.bckbPointMarking.TabIndex = 0;
            this.bckbPointMarking.Text = "Point Marking";
            this.bckbPointMarking.UseVisualStyleBackColor = true;
            this.bckbPointMarking.CheckedChanged += new System.EventHandler(this.bckbPointMarking_CheckedChanged);
            // 
            // BaseCalcChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "BaseCalcChart";
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.bButtonList1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


     
       
    
        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.BTeeChart bTeeChart1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bButtonList1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bckbPointMarking;
    }
}
