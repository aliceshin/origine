namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class SelectParamPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectParamPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlBottom = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bsprParam = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprParam_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlContentsArea.SuspendLayout();
            this.bapnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.bsprParam);
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
            // bsprParam
            // 
            this.bsprParam.About = "3.0.2005.2005";
            this.bsprParam.AccessibleDescription = "";
            this.bsprParam.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprParam.AddFontColor = System.Drawing.Color.Blue;
            this.bsprParam.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprParam.AddtionalCodeList")));
            this.bsprParam.AllowNewRow = true;
            this.bsprParam.AutoClipboard = false;
            this.bsprParam.AutoGenerateColumns = false;
            this.bsprParam.BssClass = "";
            this.bsprParam.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprParam.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprParam.columnInformation = columnInfo1;
            this.bsprParam.ComboEnable = true;
            this.bsprParam.DataAutoHeadings = false;
            this.bsprParam.DataSet = null;
            this.bsprParam.DataSource_V2 = null;
            this.bsprParam.DateToDateTimeFormat = false;
            this.bsprParam.DefaultDeleteValue = true;
            this.bsprParam.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprParam.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprParam.DisplayColumnHeader = true;
            this.bsprParam.DisplayRowHeader = true;
            this.bsprParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprParam.EditModeReplace = true;
            this.bsprParam.FilterVisible = false;
            this.bsprParam.HeadHeight = 20F;
            this.bsprParam.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprParam.IsCellCopy = false;
            this.bsprParam.IsEditComboListItem = false;
            this.bsprParam.IsMultiLanguage = true;
            this.bsprParam.IsReport = false;
            this.bsprParam.Key = "";
            this.bsprParam.Location = new System.Drawing.Point(3, 3);
            this.bsprParam.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprParam.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprParam.Name = "bsprParam";
            this.bsprParam.RowFronzen = 0;
            this.bsprParam.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprParam.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprParam.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprParam_Sheet1});
            this.bsprParam.Size = new System.Drawing.Size(500, 413);
            this.bsprParam.StyleID = null;
            this.bsprParam.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprParam.TextTipAppearance = tipAppearance1;
            this.bsprParam.UseCheckAll = false;
            this.bsprParam.UseCommandIcon = false;
            this.bsprParam.UseFilter = false;
            this.bsprParam.UseGeneralContextMenu = true;
            this.bsprParam.UseHeadColor = false;
            this.bsprParam.UseMultiCheck = true;
            this.bsprParam.UseOriginalEvent = false;
            this.bsprParam.UseSpreadEdit = true;
            this.bsprParam.UseStatusColor = false;
            this.bsprParam.UseWidthMemory = true;
            this.bsprParam.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprParam.WhenDeleteUseModify = false;
            this.bsprParam.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.bsprParam_CellDoubleClick);
            this.bsprParam.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprParam_ButtonClicked);
            // 
            // bsprParam_Sheet1
            // 
            this.bsprParam_Sheet1.Reset();
            this.bsprParam_Sheet1.SheetName = "Sheet1";
            // 
            // SelectParamPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 489);
            this.ContentsAreaMinHeight = 450;
            this.ContentsAreaMinWidth = 500;
            this.Name = "SelectParamPopup";
            this.Text = "Parameter";
            this.Title = "Parameter";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprParam_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlBottom;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprParam;
        private FarPoint.Win.Spread.SheetView bsprParam_Sheet1;
    }
}