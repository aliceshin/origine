namespace BISTel.eSPC.Page.ATT.Common
{
    partial class ChartInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartInformation));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // bsprData
            // 
            this.bsprData.About = "3.0.2005.2005";
            this.bsprData.AccessibleDescription = "";
            this.bsprData.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData.AddFontColor = System.Drawing.Color.Blue;
            this.bsprData.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprData.AddtionalCodeList")));
            this.bsprData.AllowNewRow = true;
            this.bsprData.AutoClipboard = false;
            this.bsprData.AutoGenerateColumns = false;
            this.bsprData.BssClass = "";
            this.bsprData.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprData.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprData.columnInformation = columnInfo1;
            this.bsprData.ComboEnable = true;
            this.bsprData.DataAutoHeadings = false;
            this.bsprData.DataSet = null;
            this.bsprData.DataSource_V2 = null;
            this.bsprData.DateToDateTimeFormat = false;
            this.bsprData.DefaultDeleteValue = true;
            this.bsprData.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprData.DisplayColumnHeader = true;
            this.bsprData.DisplayRowHeader = true;
            this.bsprData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprData.EditModeReplace = true;
            this.bsprData.FilterVisible = false;
            this.bsprData.HeadHeight = 20F;
            this.bsprData.IsCellCopy = false;
            this.bsprData.IsEditComboListItem = false;
            this.bsprData.IsMultiLanguage = true;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(0, 0);
            this.bsprData.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprData.Name = "bsprData";
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(337, 634);
            this.bsprData.StyleID = null;
            this.bsprData.TabIndex = 1;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprData.TextTipAppearance = tipAppearance1;
            this.bsprData.UseCheckAll = false;
            this.bsprData.UseCommandIcon = false;
            this.bsprData.UseFilter = false;
            this.bsprData.UseGeneralContextMenu = true;
            this.bsprData.UseHeadColor = false;
            this.bsprData.UseMultiCheck = true;
            this.bsprData.UseOriginalEvent = false;
            this.bsprData.UseSpreadEdit = true;
            this.bsprData.UseStatusColor = false;
            this.bsprData.UseWidthMemory = true;
            this.bsprData.WhenDeleteUseModify = false;
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.bsprData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.bsprData_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(211)))), ((int)(((byte)(225))))), FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(241)))), ((int)(((byte)(245))))), false, false, false, true, true);
            this.bsprData_Sheet1.AutoGenerateColumns = false;
            this.bsprData_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprData_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bsprData_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bsprData_Sheet1.ColumnHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprData_Sheet1.ColumnHeader.Rows.Default.Height = 18F;
            this.bsprData_Sheet1.ColumnHeader.Rows.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprData_Sheet1.ColumnHeader.Rows.Get(0).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bsprData_Sheet1.ColumnHeader.Rows.Get(0).Height = 20F;
            this.bsprData_Sheet1.ColumnHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprData_Sheet1.DataAutoHeadings = false;
            this.bsprData_Sheet1.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.bsprData_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.bsprData_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprData_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bsprData_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bsprData_Sheet1.RowHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprData_Sheet1.RowHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprData_Sheet1.Rows.Default.Height = 17F;
            this.bsprData_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprData_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bsprData_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.bsprData_Sheet1.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.bsprData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ChartInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bsprData);
            this.Name = "ChartInformation";
            this.Size = new System.Drawing.Size(337, 634);
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        private FarPoint.Win.Spread.SheetView bsprData_Sheet1;
    }
}