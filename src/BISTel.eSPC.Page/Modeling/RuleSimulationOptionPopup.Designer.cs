namespace BISTel.eSPC.Page.Modeling
{
    partial class RuleSimulationOptionPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleSimulationOptionPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.bbtnOk = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.gbRuleOption = new System.Windows.Forms.GroupBox();
            this.bsprRuleOpt = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprRuleOpt_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlModuleMode = new System.Windows.Forms.Panel();
            this.bchkModuleMode = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbSelectedRule = new System.Windows.Forms.GroupBox();
            this.lblRule = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlContentsArea.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.gbRuleOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt_Sheet1)).BeginInit();
            this.pnlModuleMode.SuspendLayout();
            this.gbSelectedRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnlInput);
            this.pnlContentsArea.Controls.Add(this.pnlButton);
            this.pnlContentsArea.Size = new System.Drawing.Size(506, 286);
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.bbtnOk);
            this.pnlButton.Controls.Add(this.panel4);
            this.pnlButton.Controls.Add(this.bbtnCancel);
            this.pnlButton.Controls.Add(this.panel3);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Location = new System.Drawing.Point(3, 258);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(500, 25);
            this.pnlButton.TabIndex = 1;
            // 
            // bbtnOk
            // 
            this.bbtnOk.AutoButtonSize = false;
            this.bbtnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnOk.BackgroundImage")));
            this.bbtnOk.BssClass = "";
            this.bbtnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnOk.IsMultiLanguage = true;
            this.bbtnOk.Key = "";
            this.bbtnOk.Location = new System.Drawing.Point(336, 0);
            this.bbtnOk.Name = "bbtnOk";
            this.bbtnOk.Size = new System.Drawing.Size(72, 25);
            this.bbtnOk.TabIndex = 3;
            this.bbtnOk.Text = "OK";
            this.bbtnOk.ToolTipText = "";
            this.bbtnOk.UseVisualStyleBackColor = true;
            this.bbtnOk.Click += new System.EventHandler(this.bbtnOk_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(408, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 25);
            this.panel4.TabIndex = 2;
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.AutoButtonSize = false;
            this.bbtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCancel.BackgroundImage")));
            this.bbtnCancel.BssClass = "";
            this.bbtnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCancel.IsMultiLanguage = true;
            this.bbtnCancel.Key = "";
            this.bbtnCancel.Location = new System.Drawing.Point(418, 0);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(72, 25);
            this.bbtnCancel.TabIndex = 1;
            this.bbtnCancel.Text = "Cancel";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(490, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 25);
            this.panel3.TabIndex = 0;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.gbRuleOption);
            this.pnlInput.Controls.Add(this.pnlModuleMode);
            this.pnlInput.Controls.Add(this.panel1);
            this.pnlInput.Controls.Add(this.gbSelectedRule);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInput.Location = new System.Drawing.Point(3, 3);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(500, 255);
            this.pnlInput.TabIndex = 4;
            // 
            // gbRuleOption
            // 
            this.gbRuleOption.Controls.Add(this.bsprRuleOpt);
            this.gbRuleOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRuleOption.Location = new System.Drawing.Point(0, 70);
            this.gbRuleOption.Name = "gbRuleOption";
            this.gbRuleOption.Size = new System.Drawing.Size(500, 175);
            this.gbRuleOption.TabIndex = 1;
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
            columnInfo1.CheckBoxFields = null;
            columnInfo1.ComboFields = null;
            columnInfo1.DefaultValue = null;
            columnInfo1.KeyFields = null;
            columnInfo1.MandatoryFields = null;
            columnInfo1.SaveTableInfo = null;
            columnInfo1.UniqueFields = null;
            this.bsprRuleOpt.columnInformation = columnInfo1;
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
            this.bsprRuleOpt.Size = new System.Drawing.Size(494, 155);
            this.bsprRuleOpt.StyleID = null;
            this.bsprRuleOpt.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprRuleOpt.TextTipAppearance = tipAppearance1;
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
            this.bsprRuleOpt.WhenDeleteUseModify = false;
            // 
            // bsprRuleOpt_Sheet1
            // 
            this.bsprRuleOpt_Sheet1.Reset();
            this.bsprRuleOpt_Sheet1.SheetName = "Sheet1";
            // 
            // pnlModuleMode
            // 
            this.pnlModuleMode.Controls.Add(this.bchkModuleMode);
            this.pnlModuleMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlModuleMode.Location = new System.Drawing.Point(0, 40);
            this.pnlModuleMode.Name = "pnlModuleMode";
            this.pnlModuleMode.Size = new System.Drawing.Size(500, 30);
            this.pnlModuleMode.TabIndex = 3;
            // 
            // bchkModuleMode
            // 
            this.bchkModuleMode.BssClass = "";
            this.bchkModuleMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.bchkModuleMode.IsMultiLanguage = true;
            this.bchkModuleMode.Key = "";
            this.bchkModuleMode.Location = new System.Drawing.Point(0, 0);
            this.bchkModuleMode.Name = "bchkModuleMode";
            this.bchkModuleMode.Size = new System.Drawing.Size(190, 30);
            this.bchkModuleMode.TabIndex = 0;
            this.bchkModuleMode.Text = "Validate Value for Same Module";
            this.bchkModuleMode.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 245);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 10);
            this.panel1.TabIndex = 2;
            // 
            // gbSelectedRule
            // 
            this.gbSelectedRule.Controls.Add(this.lblRule);
            this.gbSelectedRule.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSelectedRule.Location = new System.Drawing.Point(0, 0);
            this.gbSelectedRule.Name = "gbSelectedRule";
            this.gbSelectedRule.Size = new System.Drawing.Size(500, 40);
            this.gbSelectedRule.TabIndex = 0;
            this.gbSelectedRule.TabStop = false;
            this.gbSelectedRule.Text = "Rule";
            // 
            // lblRule
            // 
            this.lblRule.BssClass = "";
            this.lblRule.CustomTextAlign = "";
            this.lblRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRule.IsMultiLanguage = true;
            this.lblRule.Key = "";
            this.lblRule.Location = new System.Drawing.Point(3, 17);
            this.lblRule.Name = "lblRule";
            this.lblRule.Size = new System.Drawing.Size(494, 20);
            this.lblRule.TabIndex = 0;
            this.lblRule.Text = "This is label including Rule Number and Rule Description";
            this.lblRule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RuleSimulationOptionPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 319);
            this.ContentsAreaMinHeight = 280;
            this.ContentsAreaMinWidth = 500;
            this.Name = "RuleSimulationOptionPopup";
            this.Text = "RuleSimulationOption";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.gbRuleOption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRuleOpt_Sheet1)).EndInit();
            this.pnlModuleMode.ResumeLayout(false);
            this.gbSelectedRule.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private PeakPerformance.Client.BISTelControl.BButton bbtnOk;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.GroupBox gbSelectedRule;
        private PeakPerformance.Client.BISTelControl.BLabel lblRule;
        private System.Windows.Forms.GroupBox gbRuleOption;
        private PeakPerformance.Client.BISTelControl.BSpread bsprRuleOpt;
        private FarPoint.Win.Spread.SheetView bsprRuleOpt_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlModuleMode;
        private PeakPerformance.Client.BISTelControl.BCheckBox bchkModuleMode;
    }
}