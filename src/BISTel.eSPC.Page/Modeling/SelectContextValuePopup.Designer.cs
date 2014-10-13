namespace BISTel.eSPC.Page.Modeling
{
    partial class SelectContextValuePopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectContextValuePopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlBottom = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bsprContextValue = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprContextValue_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlContentsArea.SuspendLayout();
            this.bapnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.bsprContextValue);
            this.pnlContentsArea.Controls.Add(this.bapnlBottom);
            this.pnlContentsArea.Size = new System.Drawing.Size(556, 326);
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
            this.bbtnCancel.Location = new System.Drawing.Point(462, 6);
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
            this.bbtnOK.Location = new System.Drawing.Point(377, 6);
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
            this.bapnlBottom.Location = new System.Drawing.Point(3, 286);
            this.bapnlBottom.Name = "bapnlBottom";
            this.bapnlBottom.Padding_Bottom = 5;
            this.bapnlBottom.Padding_Left = 5;
            this.bapnlBottom.Padding_Right = 5;
            this.bapnlBottom.Padding_Top = 5;
            this.bapnlBottom.Size = new System.Drawing.Size(550, 37);
            this.bapnlBottom.Space = 5;
            this.bapnlBottom.TabIndex = 4;
            this.bapnlBottom.Title = "";
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
            this.bsprContextValue.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprContextValue.IsCellCopy = false;
            this.bsprContextValue.IsEditComboListItem = false;
            this.bsprContextValue.IsMultiLanguage = false;
            this.bsprContextValue.IsReport = false;
            this.bsprContextValue.Key = "";
            this.bsprContextValue.Location = new System.Drawing.Point(3, 3);
            this.bsprContextValue.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContextValue.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprContextValue.Name = "bsprContextValue";
            this.bsprContextValue.RowFronzen = 0;
            this.bsprContextValue.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprContextValue.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprContextValue.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprContextValue_Sheet1});
            this.bsprContextValue.Size = new System.Drawing.Size(550, 283);
            this.bsprContextValue.StyleID = null;
            this.bsprContextValue.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprContextValue.TextTipAppearance = tipAppearance1;
            this.bsprContextValue.UseAutoSort = false;
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
            this.bsprContextValue.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprContextValue.WhenDeleteUseModify = false;
            this.bsprContextValue.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprContextValue_ButtonClicked);
            // 
            // bsprContextValue_Sheet1
            // 
            this.bsprContextValue_Sheet1.Reset();
            this.bsprContextValue_Sheet1.SheetName = "Sheet1";
            // 
            // SelectContextValuePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 359);
            this.ContentsAreaMinHeight = 320;
            this.ContentsAreaMinWidth = 550;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "SelectContextValuePopup";
            this.Resizable = true;
            this.Text = "Set Context Value";
            this.Title = "Set Context Value";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContextValue_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlBottom;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprParam;
        private FarPoint.Win.Spread.SheetView bsprContextValue_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprContextValue;
    }
}