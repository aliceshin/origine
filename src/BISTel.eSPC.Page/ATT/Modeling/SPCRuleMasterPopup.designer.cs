namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class SPCRuleMasterPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCRuleMasterPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlBottom = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.gbRuleOption = new System.Windows.Forms.GroupBox();
            this.bsprRuleOpt = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprRuleOpt_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.gbRule = new System.Windows.Forms.GroupBox();
            this.bcboRule = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.gbOCAP = new System.Windows.Forms.GroupBox();
            this.bsprOCAP = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprOCAP_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlContentsArea.SuspendLayout();
            this.bapnlBottom.SuspendLayout();
            this.gbRuleOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt_Sheet1)).BeginInit();
            this.gbRule.SuspendLayout();
            this.gbOCAP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprOCAP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprOCAP_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.gbOCAP);
            this.pnlContentsArea.Controls.Add(this.gbRuleOption);
            this.pnlContentsArea.Controls.Add(this.gbRule);
            this.pnlContentsArea.Controls.Add(this.bapnlBottom);
            this.pnlContentsArea.Size = new System.Drawing.Size(506, 456);
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
            this.bbtnCancel.Key = "SPC_BUTTON_CANCEL";
            this.bbtnCancel.Location = new System.Drawing.Point(412, 6);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 25);
            this.bbtnCancel.TabIndex = 1;
            this.bbtnCancel.TabStop = false;
            this.bbtnCancel.Text = "CANCEL";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bbtnOK
            // 
            this.bbtnOK.AutoButtonSize = false;
            this.bbtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnOK.BackgroundImage")));
            this.bbtnOK.BssClass = "ConditionButton";
            this.bbtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnOK.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnOK.IsMultiLanguage = true;
            this.bbtnOK.Key = "SPC_BUTTON_OK";
            this.bbtnOK.Location = new System.Drawing.Point(327, 6);
            this.bbtnOK.Name = "bbtnOK";
            this.bbtnOK.Size = new System.Drawing.Size(80, 25);
            this.bbtnOK.TabIndex = 0;
            this.bbtnOK.TabStop = false;
            this.bbtnOK.Text = "OK";
            this.bbtnOK.ToolTipText = "";
            this.bbtnOK.UseVisualStyleBackColor = true;
            this.bbtnOK.Click += new System.EventHandler(this.bbtnOK_Click);
            // 
            // bapnlBottom
            // 
            this.bapnlBottom.BLabelAutoPadding = true;
            this.bapnlBottom.BssClass = "DataArrPanel";
            this.bapnlBottom.Controls.Add(this.bbtnOK);
            this.bapnlBottom.Controls.Add(this.bbtnCancel);
            this.bapnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlBottom.IsCondition = false;
            this.bapnlBottom.Key = "";
            this.bapnlBottom.Location = new System.Drawing.Point(3, 416);
            this.bapnlBottom.Name = "bapnlBottom";
            this.bapnlBottom.Padding_Bottom = 5;
            this.bapnlBottom.Padding_Left = 5;
            this.bapnlBottom.Padding_Right = 5;
            this.bapnlBottom.Padding_Top = 5;
            this.bapnlBottom.Size = new System.Drawing.Size(500, 37);
            this.bapnlBottom.Space = 5;
            this.bapnlBottom.TabIndex = 4;
            this.bapnlBottom.Title = "";
            // 
            // gbRuleOption
            // 
            this.gbRuleOption.Controls.Add(this.bsprRuleOpt);
            this.gbRuleOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbRuleOption.Location = new System.Drawing.Point(3, 44);
            this.gbRuleOption.Name = "gbRuleOption";
            this.gbRuleOption.Size = new System.Drawing.Size(500, 158);
            this.gbRuleOption.TabIndex = 6;
            this.gbRuleOption.TabStop = false;
            this.gbRuleOption.Text = "Rule Option";
            // 
            // bsprRuleOpt
            // 
            this.bsprRuleOpt.About = "3.0.2005.2005";
            this.bsprRuleOpt.AccessibleDescription = "";
            this.bsprRuleOpt.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRuleOpt.AddFontColor = System.Drawing.Color.Blue;
            this.bsprRuleOpt.AddtionalCodeList = null;
            this.bsprRuleOpt.AllowNewRow = true;
            this.bsprRuleOpt.AutoClipboard = false;
            this.bsprRuleOpt.AutoGenerateColumns = false;
            this.bsprRuleOpt.BssClass = "";
            this.bsprRuleOpt.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprRuleOpt.ColFronzen = 0;
            columnInfo2.CheckBoxFields = null;
            columnInfo2.ComboFields = null;
            columnInfo2.DefaultValue = null;
            columnInfo2.KeyFields = null;
            columnInfo2.MandatoryFields = null;
            columnInfo2.SaveTableInfo = null;
            columnInfo2.UniqueFields = null;
            this.bsprRuleOpt.columnInformation = columnInfo2;
            this.bsprRuleOpt.ComboEnable = true;
            this.bsprRuleOpt.DataAutoHeadings = false;
            this.bsprRuleOpt.DataSet = null;
            this.bsprRuleOpt.DataSource_V2 = null;
            this.bsprRuleOpt.DateToDateTimeFormat = false;
            this.bsprRuleOpt.DefaultDeleteValue = true;
            this.bsprRuleOpt.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRuleOpt.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprRuleOpt.DisplayColumnHeader = true;
            this.bsprRuleOpt.DisplayRowHeader = true;
            this.bsprRuleOpt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprRuleOpt.EditModeReplace = true;
            this.bsprRuleOpt.FilterVisible = false;
            this.bsprRuleOpt.HeadHeight = 20F;
            this.bsprRuleOpt.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprRuleOpt.IsCellCopy = false;
            this.bsprRuleOpt.IsEditComboListItem = false;
            this.bsprRuleOpt.IsMultiLanguage = true;
            this.bsprRuleOpt.IsReport = false;
            this.bsprRuleOpt.Key = "";
            this.bsprRuleOpt.Location = new System.Drawing.Point(3, 17);
            this.bsprRuleOpt.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRuleOpt.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprRuleOpt.Name = "bsprRuleOpt";
            this.bsprRuleOpt.RowFronzen = 0;
            this.bsprRuleOpt.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprRuleOpt.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprRuleOpt.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprRuleOpt_Sheet1});
            this.bsprRuleOpt.Size = new System.Drawing.Size(494, 138);
            this.bsprRuleOpt.StyleID = null;
            this.bsprRuleOpt.TabIndex = 4;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprRuleOpt.TextTipAppearance = tipAppearance2;
            this.bsprRuleOpt.UseCheckAll = false;
            this.bsprRuleOpt.UseCommandIcon = false;
            this.bsprRuleOpt.UseFilter = false;
            this.bsprRuleOpt.UseGeneralContextMenu = true;
            this.bsprRuleOpt.UseHeadColor = false;
            this.bsprRuleOpt.UseMultiCheck = true;
            this.bsprRuleOpt.UseOriginalEvent = false;
            this.bsprRuleOpt.UseSpreadEdit = true;
            this.bsprRuleOpt.UseStatusColor = false;
            this.bsprRuleOpt.UseWidthMemory = true;
            this.bsprRuleOpt.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprRuleOpt.WhenDeleteUseModify = false;
            // 
            // bsprRuleOpt_Sheet1
            // 
            this.bsprRuleOpt_Sheet1.Reset();
            this.bsprRuleOpt_Sheet1.SheetName = "Sheet1";
            // 
            // gbRule
            // 
            this.gbRule.Controls.Add(this.bcboRule);
            this.gbRule.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbRule.Location = new System.Drawing.Point(3, 3);
            this.gbRule.Name = "gbRule";
            this.gbRule.Size = new System.Drawing.Size(500, 41);
            this.gbRule.TabIndex = 5;
            this.gbRule.TabStop = false;
            this.gbRule.Text = "Rule Selection";
            // 
            // bcboRule
            // 
            this.bcboRule.AutoDropDownWidth = true;
            this.bcboRule.BssClass = "";
            this.bcboRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboRule.DropDownWidth = 600;
            this.bcboRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboRule.FormattingEnabled = true;
            this.bcboRule.IsMultiLanguage = true;
            this.bcboRule.Key = "";
            this.bcboRule.Location = new System.Drawing.Point(3, 17);
            this.bcboRule.MaxDropDownItems = 10;
            this.bcboRule.Name = "bcboRule";
            this.bcboRule.Size = new System.Drawing.Size(494, 21);
            this.bcboRule.TabIndex = 5;
            this.bcboRule.TabStop = false;
            this.bcboRule.SelectedIndexChanged += new System.EventHandler(this.bcboRule_SelectedIndexChanged);
            // 
            // gbOCAP
            // 
            this.gbOCAP.Controls.Add(this.bsprOCAP);
            this.gbOCAP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOCAP.Location = new System.Drawing.Point(3, 202);
            this.gbOCAP.Name = "gbOCAP";
            this.gbOCAP.Size = new System.Drawing.Size(500, 214);
            this.gbOCAP.TabIndex = 7;
            this.gbOCAP.TabStop = false;
            this.gbOCAP.Text = "OCAP";
            // 
            // bsprOCAP
            // 
            this.bsprOCAP.About = "3.0.2005.2005";
            this.bsprOCAP.AccessibleDescription = "";
            this.bsprOCAP.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprOCAP.AddFontColor = System.Drawing.Color.Blue;
            this.bsprOCAP.AddtionalCodeList = null;
            this.bsprOCAP.AllowNewRow = true;
            this.bsprOCAP.AutoClipboard = false;
            this.bsprOCAP.AutoGenerateColumns = false;
            this.bsprOCAP.BssClass = "";
            this.bsprOCAP.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprOCAP.ColFronzen = 0;
            columnInfo1.CheckBoxFields = null;
            columnInfo1.ComboFields = null;
            columnInfo1.DefaultValue = null;
            columnInfo1.KeyFields = null;
            columnInfo1.MandatoryFields = null;
            columnInfo1.SaveTableInfo = null;
            columnInfo1.UniqueFields = null;
            this.bsprOCAP.columnInformation = columnInfo1;
            this.bsprOCAP.ComboEnable = true;
            this.bsprOCAP.DataAutoHeadings = false;
            this.bsprOCAP.DataSet = null;
            this.bsprOCAP.DataSource_V2 = null;
            this.bsprOCAP.DateToDateTimeFormat = false;
            this.bsprOCAP.DefaultDeleteValue = true;
            this.bsprOCAP.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprOCAP.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprOCAP.DisplayColumnHeader = true;
            this.bsprOCAP.DisplayRowHeader = true;
            this.bsprOCAP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprOCAP.EditModeReplace = true;
            this.bsprOCAP.FilterVisible = false;
            this.bsprOCAP.HeadHeight = 20F;
            this.bsprOCAP.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprOCAP.IsCellCopy = false;
            this.bsprOCAP.IsEditComboListItem = false;
            this.bsprOCAP.IsMultiLanguage = true;
            this.bsprOCAP.IsReport = false;
            this.bsprOCAP.Key = "";
            this.bsprOCAP.Location = new System.Drawing.Point(3, 17);
            this.bsprOCAP.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprOCAP.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprOCAP.Name = "bsprOCAP";
            this.bsprOCAP.RowFronzen = 0;
            this.bsprOCAP.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprOCAP.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprOCAP.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprOCAP_Sheet1});
            this.bsprOCAP.Size = new System.Drawing.Size(494, 194);
            this.bsprOCAP.StyleID = null;
            this.bsprOCAP.TabIndex = 4;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprOCAP.TextTipAppearance = tipAppearance1;
            this.bsprOCAP.UseCheckAll = false;
            this.bsprOCAP.UseCommandIcon = false;
            this.bsprOCAP.UseFilter = false;
            this.bsprOCAP.UseGeneralContextMenu = true;
            this.bsprOCAP.UseHeadColor = false;
            this.bsprOCAP.UseMultiCheck = true;
            this.bsprOCAP.UseOriginalEvent = false;
            this.bsprOCAP.UseSpreadEdit = true;
            this.bsprOCAP.UseStatusColor = false;
            this.bsprOCAP.UseWidthMemory = true;
            this.bsprOCAP.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprOCAP.WhenDeleteUseModify = false;
            this.bsprOCAP.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprOCAP_ButtonClicked);
            // 
            // bsprOCAP_Sheet1
            // 
            this.bsprOCAP_Sheet1.Reset();
            this.bsprOCAP_Sheet1.SheetName = "Sheet1";
            // 
            // SPCRuleMasterPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 489);
            this.ContentsAreaMinHeight = 450;
            this.ContentsAreaMinWidth = 500;
            this.Name = "SPCRuleMasterPopup";
            this.Text = "SPC Rule";
            this.Title = "SPC Rule";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapnlBottom.ResumeLayout(false);
            this.gbRuleOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt_Sheet1)).EndInit();
            this.gbRule.ResumeLayout(false);
            this.gbOCAP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprOCAP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprOCAP_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlBottom;
        private System.Windows.Forms.GroupBox gbRuleOption;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprRuleOpt;
        private FarPoint.Win.Spread.SheetView bsprRuleOpt_Sheet1;
        private System.Windows.Forms.GroupBox gbRule;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboRule;
        private System.Windows.Forms.GroupBox gbOCAP;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprOCAP;
        private FarPoint.Win.Spread.SheetView bsprOCAP_Sheet1;
    }
}