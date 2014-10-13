namespace BISTel.eSPC.Page.Modeling
{
    partial class OptionalConfigurationUC
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionalConfigurationUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlSPCBase = new System.Windows.Forms.Panel();
            this.gbDefaultChart = new System.Windows.Forms.GroupBox();
            this.bsprDefaultChart = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprDefaultChart_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.gbOptionSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.blblPriority = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblRestrict = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcboParamCategory = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bcboPriority = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bLabel4 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtRestrictSample = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblParamCategory = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bchkAutoCpkYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.btxtRestrictHours = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtRestrictDays = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel5 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlSPCBase.SuspendLayout();
            this.gbDefaultChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprDefaultChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprDefaultChart_Sheet1)).BeginInit();
            this.gbOptionSelection.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSPCBase
            // 
            this.pnlSPCBase.Controls.Add(this.gbDefaultChart);
            this.pnlSPCBase.Controls.Add(this.gbOptionSelection);
            this.pnlSPCBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSPCBase.Location = new System.Drawing.Point(0, 0);
            this.pnlSPCBase.Name = "pnlSPCBase";
            this.pnlSPCBase.Size = new System.Drawing.Size(885, 457);
            this.pnlSPCBase.TabIndex = 4;
            // 
            // gbDefaultChart
            // 
            this.gbDefaultChart.Controls.Add(this.bsprDefaultChart);
            this.gbDefaultChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDefaultChart.Location = new System.Drawing.Point(0, 121);
            this.gbDefaultChart.Name = "gbDefaultChart";
            this.gbDefaultChart.Size = new System.Drawing.Size(885, 336);
            this.gbDefaultChart.TabIndex = 3;
            this.gbDefaultChart.TabStop = false;
            this.gbDefaultChart.Text = "Default charts to show";
            // 
            // bsprDefaultChart
            // 
            this.bsprDefaultChart.About = "3.0.2005.2005";
            this.bsprDefaultChart.AccessibleDescription = "";
            this.bsprDefaultChart.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprDefaultChart.AddFontColor = System.Drawing.Color.Blue;
            this.bsprDefaultChart.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprDefaultChart.AddtionalCodeList")));
            this.bsprDefaultChart.AllowNewRow = true;
            this.bsprDefaultChart.AutoClipboard = false;
            this.bsprDefaultChart.AutoGenerateColumns = false;
            this.bsprDefaultChart.BssClass = "";
            this.bsprDefaultChart.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprDefaultChart.ColFronzen = 0;
            columnInfo1.CheckBoxFields = null;
            columnInfo1.ComboFields = null;
            columnInfo1.DefaultValue = null;
            columnInfo1.KeyFields = null;
            columnInfo1.MandatoryFields = null;
            columnInfo1.SaveTableInfo = null;
            columnInfo1.UniqueFields = null;
            this.bsprDefaultChart.columnInformation = columnInfo1;
            this.bsprDefaultChart.ComboEnable = true;
            this.bsprDefaultChart.DataAutoHeadings = false;
            this.bsprDefaultChart.DataSet = null;
            this.bsprDefaultChart.DataSource_V2 = null;
            this.bsprDefaultChart.DateToDateTimeFormat = false;
            this.bsprDefaultChart.DefaultDeleteValue = true;
            this.bsprDefaultChart.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprDefaultChart.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprDefaultChart.DisplayColumnHeader = true;
            this.bsprDefaultChart.DisplayRowHeader = true;
            this.bsprDefaultChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprDefaultChart.EditModeReplace = true;
            this.bsprDefaultChart.FilterVisible = false;
            this.bsprDefaultChart.HeadHeight = 20F;
            this.bsprDefaultChart.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprDefaultChart.IsCellCopy = false;
            this.bsprDefaultChart.IsEditComboListItem = false;
            this.bsprDefaultChart.IsMultiLanguage = true;
            this.bsprDefaultChart.IsReport = false;
            this.bsprDefaultChart.Key = "";
            this.bsprDefaultChart.Location = new System.Drawing.Point(3, 17);
            this.bsprDefaultChart.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprDefaultChart.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprDefaultChart.Name = "bsprDefaultChart";
            this.bsprDefaultChart.RowFronzen = 0;
            this.bsprDefaultChart.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprDefaultChart.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprDefaultChart.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprDefaultChart_Sheet1});
            this.bsprDefaultChart.Size = new System.Drawing.Size(879, 316);
            this.bsprDefaultChart.StyleID = null;
            this.bsprDefaultChart.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprDefaultChart.TextTipAppearance = tipAppearance1;
            this.bsprDefaultChart.UseCheckAll = false;
            this.bsprDefaultChart.UseCommandIcon = false;
            this.bsprDefaultChart.UseFilter = false;
            this.bsprDefaultChart.UseGeneralContextMenu = true;
            this.bsprDefaultChart.UseHeadColor = false;
            this.bsprDefaultChart.UseMultiCheck = true;
            this.bsprDefaultChart.UseOriginalEvent = false;
            this.bsprDefaultChart.UseSpreadEdit = true;
            this.bsprDefaultChart.UseStatusColor = false;
            this.bsprDefaultChart.UseWidthMemory = true;
            this.bsprDefaultChart.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprDefaultChart.WhenDeleteUseModify = false;
            // 
            // bsprDefaultChart_Sheet1
            // 
            this.bsprDefaultChart_Sheet1.Reset();
            this.bsprDefaultChart_Sheet1.SheetName = "Sheet1";
            // 
            // gbOptionSelection
            // 
            this.gbOptionSelection.Controls.Add(this.tableLayoutPanel1);
            this.gbOptionSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbOptionSelection.Location = new System.Drawing.Point(0, 0);
            this.gbOptionSelection.Name = "gbOptionSelection";
            this.gbOptionSelection.Size = new System.Drawing.Size(885, 121);
            this.gbOptionSelection.TabIndex = 2;
            this.gbOptionSelection.TabStop = false;
            this.gbOptionSelection.Text = "Option Selection";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.blblPriority, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.blblRestrict, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.bcboParamCategory, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.bcboPriority, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bLabel4, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btxtRestrictSample, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.blblParamCategory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bchkAutoCpkYN, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btxtRestrictHours, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.bLabel1, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.btxtRestrictDays, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.bLabel5, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(879, 100);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // blblPriority
            // 
            this.blblPriority.BssClass = "";
            this.blblPriority.CustomTextAlign = "";
            this.blblPriority.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblPriority.IsMultiLanguage = true;
            this.blblPriority.Key = "";
            this.blblPriority.Location = new System.Drawing.Point(3, 25);
            this.blblPriority.Name = "blblPriority";
            this.blblPriority.Size = new System.Drawing.Size(110, 21);
            this.blblPriority.TabIndex = 1;
            this.blblPriority.Text = "Priority";
            this.blblPriority.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblRestrict
            // 
            this.blblRestrict.BssClass = "";
            this.blblRestrict.CustomTextAlign = "";
            this.blblRestrict.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblRestrict.IsMultiLanguage = true;
            this.blblRestrict.Key = "";
            this.blblRestrict.Location = new System.Drawing.Point(3, 50);
            this.blblRestrict.Name = "blblRestrict";
            this.blblRestrict.Size = new System.Drawing.Size(168, 21);
            this.blblRestrict.TabIndex = 2;
            this.blblRestrict.Text = "Restrict default samples to";
            this.blblRestrict.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcboParamCategory
            // 
            this.bcboParamCategory.AutoDropDownWidth = true;
            this.bcboParamCategory.BssClass = "";
            this.bcboParamCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboParamCategory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboParamCategory.FormattingEnabled = true;
            this.bcboParamCategory.IsMultiLanguage = true;
            this.bcboParamCategory.Key = "";
            this.bcboParamCategory.Location = new System.Drawing.Point(178, 3);
            this.bcboParamCategory.Name = "bcboParamCategory";
            this.bcboParamCategory.Size = new System.Drawing.Size(109, 21);
            this.bcboParamCategory.TabIndex = 4;
            // 
            // bcboPriority
            // 
            this.bcboPriority.AutoDropDownWidth = true;
            this.bcboPriority.BssClass = "";
            this.bcboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboPriority.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboPriority.FormattingEnabled = true;
            this.bcboPriority.IsMultiLanguage = true;
            this.bcboPriority.Key = "";
            this.bcboPriority.Location = new System.Drawing.Point(178, 28);
            this.bcboPriority.Name = "bcboPriority";
            this.bcboPriority.Size = new System.Drawing.Size(109, 21);
            this.bcboPriority.TabIndex = 5;
            // 
            // bLabel4
            // 
            this.bLabel4.BssClass = "";
            this.bLabel4.CustomTextAlign = "";
            this.bLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel4.IsMultiLanguage = true;
            this.bLabel4.Key = "";
            this.bLabel4.Location = new System.Drawing.Point(295, 50);
            this.bLabel4.Name = "bLabel4";
            this.bLabel4.Size = new System.Drawing.Size(110, 21);
            this.bLabel4.TabIndex = 3;
            this.bLabel4.Text = "sample counts or";
            this.bLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtRestrictSample
            // 
            this.btxtRestrictSample.BssClass = "";
            this.btxtRestrictSample.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtRestrictSample.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.btxtRestrictSample.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.Number;
            this.btxtRestrictSample.IsMultiLanguage = true;
            this.btxtRestrictSample.Key = "";
            this.btxtRestrictSample.Location = new System.Drawing.Point(178, 53);
            this.btxtRestrictSample.MaxLength = 5;
            this.btxtRestrictSample.Name = "btxtRestrictSample";
            this.btxtRestrictSample.Size = new System.Drawing.Size(109, 21);
            this.btxtRestrictSample.TabIndex = 6;
            // 
            // blblParamCategory
            // 
            this.blblParamCategory.BssClass = "";
            this.blblParamCategory.CustomTextAlign = "";
            this.blblParamCategory.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblParamCategory.IsMultiLanguage = true;
            this.blblParamCategory.Key = "";
            this.blblParamCategory.Location = new System.Drawing.Point(3, 0);
            this.blblParamCategory.Name = "blblParamCategory";
            this.blblParamCategory.Size = new System.Drawing.Size(133, 21);
            this.blblParamCategory.TabIndex = 0;
            this.blblParamCategory.Text = "Parameter Category";
            this.blblParamCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bchkAutoCpkYN
            // 
            this.bchkAutoCpkYN.BssClass = "";
            this.bchkAutoCpkYN.IsMultiLanguage = true;
            this.bchkAutoCpkYN.Key = "";
            this.bchkAutoCpkYN.Location = new System.Drawing.Point(412, 3);
            this.bchkAutoCpkYN.Name = "bchkAutoCpkYN";
            this.bchkAutoCpkYN.Size = new System.Drawing.Size(110, 18);
            this.bchkAutoCpkYN.TabIndex = 9;
            this.bchkAutoCpkYN.Text = "Calculate Ppk";
            this.bchkAutoCpkYN.UseVisualStyleBackColor = true;
            // 
            // btxtRestrictHours
            // 
            this.btxtRestrictHours.BssClass = "";
            this.btxtRestrictHours.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtRestrictHours.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.btxtRestrictHours.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.Number;
            this.btxtRestrictHours.IsMultiLanguage = true;
            this.btxtRestrictHours.Key = "";
            this.btxtRestrictHours.Location = new System.Drawing.Point(412, 78);
            this.btxtRestrictHours.MaxLength = 3;
            this.btxtRestrictHours.Name = "btxtRestrictHours";
            this.btxtRestrictHours.Size = new System.Drawing.Size(109, 21);
            this.btxtRestrictHours.TabIndex = 10;
            // 
            // bLabel1
            // 
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(529, 75);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(117, 21);
            this.bLabel1.TabIndex = 11;
            this.bLabel1.Text = "hours";
            this.bLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtRestrictDays
            // 
            this.btxtRestrictDays.BssClass = "";
            this.btxtRestrictDays.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtRestrictDays.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.btxtRestrictDays.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.Number;
            this.btxtRestrictDays.IsMultiLanguage = true;
            this.btxtRestrictDays.Key = "";
            this.btxtRestrictDays.Location = new System.Drawing.Point(178, 78);
            this.btxtRestrictDays.MaxLength = 2;
            this.btxtRestrictDays.Name = "btxtRestrictDays";
            this.btxtRestrictDays.Size = new System.Drawing.Size(109, 21);
            this.btxtRestrictDays.TabIndex = 8;
            // 
            // bLabel5
            // 
            this.bLabel5.BssClass = "";
            this.bLabel5.CustomTextAlign = "";
            this.bLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel5.IsMultiLanguage = true;
            this.bLabel5.Key = "";
            this.bLabel5.Location = new System.Drawing.Point(295, 75);
            this.bLabel5.Name = "bLabel5";
            this.bLabel5.Size = new System.Drawing.Size(110, 21);
            this.bLabel5.TabIndex = 7;
            this.bLabel5.Text = "days";
            this.bLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OptionalConfigurationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlSPCBase);
            this.Name = "OptionalConfigurationUC";
            this.Size = new System.Drawing.Size(885, 457);
            this.pnlSPCBase.ResumeLayout(false);
            this.gbDefaultChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprDefaultChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprDefaultChart_Sheet1)).EndInit();
            this.gbOptionSelection.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.Panel pnlSPCBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblParamCategory;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblPriority;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblRestrict;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboParamCategory;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboPriority;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel5;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtRestrictSample;
        private System.Windows.Forms.GroupBox gbOptionSelection;
        private System.Windows.Forms.GroupBox gbDefaultChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtRestrictDays;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkAutoCpkYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprDefaultChart;
        private FarPoint.Win.Spread.SheetView bsprDefaultChart_Sheet1;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtRestrictHours;
        private PeakPerformance.Client.BISTelControl.BLabel bLabel1;
    }
}
