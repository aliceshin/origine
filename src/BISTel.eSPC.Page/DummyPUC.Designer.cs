namespace BISTel.eSPC.Page
{
    partial class DummyPUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyPUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bsprEventList = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprEventList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.bsprEventList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprEventList_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // bsprEventList
            // 
            this.bsprEventList.About = "3.0.2005.2005";
            this.bsprEventList.AccessibleDescription = "";
            this.bsprEventList.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprEventList.AddtionalCodeList")));
            this.bsprEventList.AllowNewRow = true;
            this.bsprEventList.AutoClipboard = false;
            this.bsprEventList.AutoGenerateColumns = false;
            this.bsprEventList.BssClass = "";
            this.bsprEventList.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprEventList.ColFronzen = 0;
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprEventList.columnInformation = columnInfo1;
            this.bsprEventList.ComboEnable = true;
            this.bsprEventList.DataAutoHeadings = false;
            this.bsprEventList.DataSet = null;
            this.bsprEventList.DateToDateTimeFormat = false;
            this.bsprEventList.DefaultDeleteValue = true;
            this.bsprEventList.DisplayColumnHeader = true;
            this.bsprEventList.DisplayRowHeader = true;
            this.bsprEventList.EditModeReplace = true;
            this.bsprEventList.FilterVisible = false;
            this.bsprEventList.HeadHeight = 20F;
            this.bsprEventList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprEventList.IsCellCopy = true;
            this.bsprEventList.IsMultiLanguage = true;
            this.bsprEventList.IsReport = false;
            this.bsprEventList.Key = "";
            this.bsprEventList.Location = new System.Drawing.Point(51, 45);
            this.bsprEventList.Name = "bsprEventList";
            this.bsprEventList.RowFronzen = 0;
            this.bsprEventList.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprEventList.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprEventList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprEventList_Sheet1});
            this.bsprEventList.Size = new System.Drawing.Size(389, 225);
            this.bsprEventList.TabIndex = 1;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprEventList.TextTipAppearance = tipAppearance1;
            this.bsprEventList.UseFilter = false;
            this.bsprEventList.UseGeneralContextMenu = true;
            this.bsprEventList.UseHeadColor = false;
            this.bsprEventList.UseOriginalEvent = false;
            this.bsprEventList.UseSpreadEdit = true;
            this.bsprEventList.UseWidthMemory = true;
            this.bsprEventList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprEventList.WhenDeleteUseModify = false;
            // 
            // bsprEventList_Sheet1
            // 
            this.bsprEventList_Sheet1.Reset();
            this.bsprEventList_Sheet1.SheetName = "Sheet1";
            // 
            // DummyPUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bsprEventList);
            this.Name = "DummyPUC";
            this.Size = new System.Drawing.Size(744, 439);
            ((System.ComponentModel.ISupportInitialize)(this.bsprEventList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprEventList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprEventList;
        private FarPoint.Win.Spread.SheetView bsprEventList_Sheet1;
    }
}
