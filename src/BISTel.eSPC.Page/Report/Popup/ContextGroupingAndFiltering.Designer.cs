namespace BISTel.eSPC.Page.Report
{
    partial class ContextGroupingAndFiltering
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextGroupingAndFiltering));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bsprContextValue = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprContextValue_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bchkbGroup = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bsprContextValue);
            this.groupBox1.Controls.Add(this.bchkbGroup);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 185);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Context Name";
            // 
            // bsprContextValue
            // 
            this.bsprContextValue.About = "3.0.2005.2005";
            this.bsprContextValue.AccessibleDescription = "";
            this.bsprContextValue.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContextValue.AddFontColor = System.Drawing.Color.Blue;
            this.bsprContextValue.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprContextValue.AddtionalCodeList")));
            this.bsprContextValue.AllowNewRow = true;
            this.bsprContextValue.AutoClipboard = false;
            this.bsprContextValue.AutoGenerateColumns = false;
            this.bsprContextValue.BssClass = "";
            this.bsprContextValue.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprContextValue.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprContextValue.columnInformation = columnInfo1;
            this.bsprContextValue.ComboEnable = true;
            this.bsprContextValue.DataAutoHeadings = false;
            this.bsprContextValue.DataSet = null;
            this.bsprContextValue.DataSource_V2 = null;
            this.bsprContextValue.DateToDateTimeFormat = false;
            this.bsprContextValue.DefaultDeleteValue = true;
            this.bsprContextValue.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContextValue.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprContextValue.DisplayColumnHeader = true;
            this.bsprContextValue.DisplayRowHeader = true;
            this.bsprContextValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprContextValue.EditModeReplace = true;
            this.bsprContextValue.FilterVisible = false;
            this.bsprContextValue.HeadHeight = 20F;
            this.bsprContextValue.IsCellCopy = false;
            this.bsprContextValue.IsEditComboListItem = false;
            this.bsprContextValue.IsMultiLanguage = true;
            this.bsprContextValue.IsReport = false;
            this.bsprContextValue.Key = "";
            this.bsprContextValue.Location = new System.Drawing.Point(3, 39);
            this.bsprContextValue.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContextValue.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprContextValue.Name = "bsprContextValue";
            this.bsprContextValue.RowFronzen = 0;
            this.bsprContextValue.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprContextValue.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprContextValue.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprContextValue_Sheet1});
            this.bsprContextValue.Size = new System.Drawing.Size(197, 143);
            this.bsprContextValue.StyleID = null;
            this.bsprContextValue.TabIndex = 3;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprContextValue.TextTipAppearance = tipAppearance1;
            this.bsprContextValue.UseCheckAll = false;
            this.bsprContextValue.UseCommandIcon = false;
            this.bsprContextValue.UseFilter = false;
            this.bsprContextValue.UseGeneralContextMenu = true;
            this.bsprContextValue.UseHeadColor = false;
            this.bsprContextValue.UseMultiCheck = true;
            this.bsprContextValue.UseOriginalEvent = false;
            this.bsprContextValue.UseSpreadEdit = true;
            this.bsprContextValue.UseStatusColor = false;
            this.bsprContextValue.UseWidthMemory = true;
            this.bsprContextValue.WhenDeleteUseModify = false;
            this.bsprContextValue.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprContextValue_ButtonClicked);
            this.bsprContextValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bsprContextValue_MouseDown);
            // 
            // bsprContextValue_Sheet1
            // 
            this.bsprContextValue_Sheet1.Reset();
            this.bsprContextValue_Sheet1.SheetName = "Sheet1";
            // 
            // bchkbGroup
            // 
            this.bchkbGroup.BssClass = "";
            this.bchkbGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.bchkbGroup.IsMultiLanguage = true;
            this.bchkbGroup.Key = "";
            this.bchkbGroup.Location = new System.Drawing.Point(3, 17);
            this.bchkbGroup.Name = "bchkbGroup";
            this.bchkbGroup.Size = new System.Drawing.Size(197, 22);
            this.bchkbGroup.TabIndex = 2;
            this.bchkbGroup.Text = "Grouping";
            this.bchkbGroup.UseVisualStyleBackColor = true;
            this.bchkbGroup.CheckedChanged += new System.EventHandler(this.bchkbGroup_CheckedChanged);
            // 
            // ContextGroupingAndFiltering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Name = "ContextGroupingAndFiltering";
            this.Size = new System.Drawing.Size(203, 185);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkbGroup;
        private PeakPerformance.Client.BISTelControl.BSpread bsprContextValue;
        private FarPoint.Win.Spread.SheetView bsprContextValue_Sheet1;
    }
}
