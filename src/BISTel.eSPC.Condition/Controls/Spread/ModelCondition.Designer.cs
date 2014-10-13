namespace BISTel.eSPC.Condition.Controls.Spread
{
    partial class ModelCondition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelCondition));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.bsprParam = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.st_param = new FarPoint.Win.Spread.SheetView();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.st_param)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.bsprParam);
            this.pnlBody.Size = new System.Drawing.Size(339, 151);
            // 
            // bsprParam
            // 
            this.bsprParam.About = "3.0.2005.2005";
            this.bsprParam.AccessibleDescription = "";
            this.bsprParam.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprParam.AddtionalCodeList")));
            this.bsprParam.AllowNewRow = true;
            this.bsprParam.AutoClipboard = false;
            this.bsprParam.AutoGenerateColumns = false;
            this.bsprParam.BssClass = "";
            this.bsprParam.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprParam.ColFronzen = 0;
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprParam.columnInformation = columnInfo1;
            this.bsprParam.ComboEnable = true;
            this.bsprParam.DataAutoHeadings = false;
            this.bsprParam.DataSet = null;
            this.bsprParam.DateToDateTimeFormat = false;
            this.bsprParam.DefaultDeleteValue = true;
            this.bsprParam.DisplayColumnHeader = true;
            this.bsprParam.DisplayRowHeader = true;
            this.bsprParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprParam.EditModeReplace = true;
            this.bsprParam.HeadHeight = 20F;
            this.bsprParam.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprParam.IsCellCopy = false;
            this.bsprParam.IsMultiLanguage = true;
            this.bsprParam.IsReport = false;
            this.bsprParam.Key = "";
            this.bsprParam.Location = new System.Drawing.Point(0, 0);
            this.bsprParam.Name = "bsprParam";
            this.bsprParam.RowFronzen = 0;
            this.bsprParam.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprParam.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprParam.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.st_param});
            this.bsprParam.Size = new System.Drawing.Size(339, 151);
            this.bsprParam.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprParam.TextTipAppearance = tipAppearance1;
            this.bsprParam.UseFilter = false;
            this.bsprParam.UseGeneralContextMenu = true;
            this.bsprParam.UseHeadColor = false;
            this.bsprParam.UseOriginalEvent = false;
            this.bsprParam.UseWidthMemory = true;
            this.bsprParam.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprParam.WhenDeleteUseModify = false;
            this.bsprParam.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.bsprParam_CellClick);
            this.bsprParam.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprParam_ButtonClicked);
            this.bsprParam.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.bsprParam_CellDoubleClick);
            // 
            // st_param
            // 
            this.st_param.Reset();
            this.st_param.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.st_param.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.st_param.ColumnCount = 2;
            this.st_param.RowCount = 0;
            this.st_param.ColumnHeader.Cells.Get(0, 0).Value = "SELECT";
            this.st_param.ColumnHeader.Cells.Get(0, 1).Value = "PARAMETER NAME";
            this.st_param.Columns.Get(0).CellType = checkBoxCellType1;
            this.st_param.Columns.Get(0).Label = "SELECT";
            this.st_param.Columns.Get(1).Label = "PARAMETER NAME";
            this.st_param.Columns.Get(1).Width = 164F;
            this.st_param.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.bsprParam.SetActiveViewport(0, 1, 0);
            // 
            // ParameterCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ParameterCondition";
            this.Size = new System.Drawing.Size(341, 180);
            this.Load += new System.EventHandler(this.ParameterCondition_Load);
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.st_param)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprParam;
        private FarPoint.Win.Spread.SheetView st_param;
    }
}
