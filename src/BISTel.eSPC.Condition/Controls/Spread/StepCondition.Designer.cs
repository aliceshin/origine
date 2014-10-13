namespace BISTel.eSPC.Condition.Controls.Spread
{
    partial class StepCondition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StepCondition));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.bsprStep = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.st_step = new FarPoint.Win.Spread.SheetView();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.st_step)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.bsprStep);
            this.pnlBody.Size = new System.Drawing.Size(339, 130);
            // 
            // bsprStep
            // 
            this.bsprStep.About = "3.0.2005.2005";
            this.bsprStep.AccessibleDescription = "";
            this.bsprStep.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprStep.AddtionalCodeList")));
            this.bsprStep.AllowNewRow = true;
            this.bsprStep.AutoClipboard = false;
            this.bsprStep.AutoGenerateColumns = false;
            this.bsprStep.BssClass = "";
            this.bsprStep.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprStep.ColFronzen = 0;
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprStep.columnInformation = columnInfo1;
            this.bsprStep.ComboEnable = true;
            this.bsprStep.DataAutoHeadings = false;
            this.bsprStep.DataSet = null;
            this.bsprStep.DateToDateTimeFormat = false;
            this.bsprStep.DefaultDeleteValue = true;
            this.bsprStep.DisplayColumnHeader = true;
            this.bsprStep.DisplayRowHeader = true;
            this.bsprStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprStep.EditModeReplace = true;
            this.bsprStep.HeadHeight = 20F;
            this.bsprStep.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprStep.IsCellCopy = false;
            this.bsprStep.IsMultiLanguage = true;
            this.bsprStep.IsReport = false;
            this.bsprStep.Key = "";
            this.bsprStep.Location = new System.Drawing.Point(0, 0);
            this.bsprStep.Name = "bsprStep";
            this.bsprStep.RowFronzen = 0;
            this.bsprStep.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprStep.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprStep.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.st_step});
            this.bsprStep.Size = new System.Drawing.Size(339, 130);
            this.bsprStep.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprStep.TextTipAppearance = tipAppearance1;
            this.bsprStep.UseFilter = false;
            this.bsprStep.UseGeneralContextMenu = true;
            this.bsprStep.UseHeadColor = false;
            this.bsprStep.UseOriginalEvent = false;
            this.bsprStep.UseWidthMemory = true;
            this.bsprStep.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprStep.WhenDeleteUseModify = false;
            this.bsprStep.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.bsprStep_CellClick);
            this.bsprStep.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprStep_ButtonClicked);
            // 
            // st_step
            // 
            this.st_step.Reset();
            this.st_step.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.st_step.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.st_step.ColumnCount = 2;
            this.st_step.RowCount = 0;
            this.st_step.ColumnHeader.Cells.Get(0, 0).Value = "SELECT";
            this.st_step.ColumnHeader.Cells.Get(0, 1).Value = "STEP NAME";
            this.st_step.Columns.Get(0).CellType = checkBoxCellType1;
            this.st_step.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.st_step.Columns.Get(0).Label = "SELECT";
            this.st_step.Columns.Get(0).Width = 55F;
            this.st_step.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.st_step.Columns.Get(1).Label = "STEP NAME";
            this.st_step.Columns.Get(1).Width = 101F;
            this.st_step.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.bsprStep.SetActiveViewport(0, 1, 0);
            // 
            // StepCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StepCondition";
            this.Size = new System.Drawing.Size(341, 159);
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.st_step)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprStep;
        private FarPoint.Win.Spread.SheetView st_step;
    }
}
