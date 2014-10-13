namespace BISTel.eSPC.Page.ATT.Common
{
    partial class ChartDataPopup
    {
        /// <summary>
        /// Required designer ChartVariable.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartDataPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlChartRowDataPopup = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bTabControl1 = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bsprRawData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprRawData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlHistoryInfo_1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.pnlChartRowDataPopup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.bTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRawData_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.bapnlButton.SuspendLayout();
            this.bapnlHistoryInfo_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Size = new System.Drawing.Size(806, 506);
            // 
            // pnlChartRowDataPopup
            // 
            this.pnlChartRowDataPopup.Controls.Add(this.panel2);
            this.pnlChartRowDataPopup.Controls.Add(this.panel1);
            this.pnlChartRowDataPopup.Controls.Add(this.bapnlHistoryInfo_1);
            this.pnlChartRowDataPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartRowDataPopup.Location = new System.Drawing.Point(6, 27);
            this.pnlChartRowDataPopup.Name = "pnlChartRowDataPopup";
            this.pnlChartRowDataPopup.Size = new System.Drawing.Size(806, 506);
            this.pnlChartRowDataPopup.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bTabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel2.Size = new System.Drawing.Size(806, 441);
            this.panel2.TabIndex = 4;
            // 
            // bTabControl1
            // 
            this.bTabControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(161)))), ((int)(((byte)(184)))));
            this.bTabControl1.BssClass = "DataMainTab";
            this.bTabControl1.CloseImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseImage")));
            this.bTabControl1.CloseRollOverImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseRollOverImage")));
            this.bTabControl1.Controls.Add(this.tabPage1);
            this.bTabControl1.Controls.Add(this.tabPage2);
            this.bTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bTabControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTabControl1.FontSizeSet = "11";
            this.bTabControl1.IsDrawBorder = true;
            this.bTabControl1.IsFillBorder = false;
            this.bTabControl1.IsReleaseAlignmentRestrict = false;
            this.bTabControl1.IsSpace = true;
            this.bTabControl1.Key = "";
            this.bTabControl1.Location = new System.Drawing.Point(5, 5);
            this.bTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.bTabControl1.Name = "bTabControl1";
            this.bTabControl1.SelectedIndex = 0;
            this.bTabControl1.Size = new System.Drawing.Size(796, 436);
            this.bTabControl1.TabBackColor = System.Drawing.Color.White;
            this.bTabControl1.TabBackImage = null;
            this.bTabControl1.TabIndex = 6;
            this.bTabControl1.TabPageClose = false;
            this.bTabControl1.TabSelectFontColor = System.Drawing.Color.White;
            this.bTabControl1.TabSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectLeftImage")));
            this.bTabControl1.TabSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectMiddleImage")));
            this.bTabControl1.TabSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectRightImage")));
            this.bTabControl1.TabSelectRollOverFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectLeftImage")));
            this.bTabControl1.TabUnSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectMiddleImage")));
            this.bTabControl1.TabUnSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectRightImage")));
            this.bTabControl1.TabUnSelectRollOverFontColor = System.Drawing.Color.White;
            this.bTabControl1.TextLeftPosition = 5;
            this.bTabControl1.UseShadowFont = false;
            this.bTabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.bTabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bsprData);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(788, 406);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Chart Data      ";
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
            this.bsprData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprData.IsCellCopy = true;
            this.bsprData.IsEditComboListItem = false;
            this.bsprData.IsMultiLanguage = false;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(0, 0);
            this.bsprData.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprData.Name = "bsprData";
            this.bsprData.Padding = new System.Windows.Forms.Padding(5);
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(788, 406);
            this.bsprData.StyleID = null;
            this.bsprData.TabIndex = 3;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.bsprData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprData.WhenDeleteUseModify = false;
            this.bsprData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bsprData_KeyDown);
            this.bsprData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bsprData_KeyUp);
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bsprRawData);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(788, 406);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Raw Data      ";
            // 
            // bsprRawData
            // 
            this.bsprRawData.About = "3.0.2005.2005";
            this.bsprRawData.AccessibleDescription = "";
            this.bsprRawData.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRawData.AddFontColor = System.Drawing.Color.Blue;
            this.bsprRawData.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprRawData.AddtionalCodeList")));
            this.bsprRawData.AllowNewRow = true;
            this.bsprRawData.AutoClipboard = false;
            this.bsprRawData.AutoGenerateColumns = false;
            this.bsprRawData.BssClass = "";
            this.bsprRawData.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprRawData.ColFronzen = 0;
            columnInfo2.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.CheckBoxFields")));
            columnInfo2.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.ComboFields")));
            columnInfo2.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo2.DefaultValue")));
            columnInfo2.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.KeyFields")));
            columnInfo2.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.MandatoryFields")));
            columnInfo2.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo2.SaveTableInfo")));
            columnInfo2.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.UniqueFields")));
            this.bsprRawData.columnInformation = columnInfo2;
            this.bsprRawData.ComboEnable = true;
            this.bsprRawData.DataAutoHeadings = false;
            this.bsprRawData.DataSet = null;
            this.bsprRawData.DataSource_V2 = null;
            this.bsprRawData.DateToDateTimeFormat = false;
            this.bsprRawData.DefaultDeleteValue = true;
            this.bsprRawData.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRawData.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprRawData.DisplayColumnHeader = true;
            this.bsprRawData.DisplayRowHeader = true;
            this.bsprRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprRawData.EditModeReplace = true;
            this.bsprRawData.FilterVisible = false;
            this.bsprRawData.HeadHeight = 20F;
            this.bsprRawData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprRawData.IsCellCopy = true;
            this.bsprRawData.IsEditComboListItem = false;
            this.bsprRawData.IsMultiLanguage = false;
            this.bsprRawData.IsReport = false;
            this.bsprRawData.Key = "";
            this.bsprRawData.Location = new System.Drawing.Point(0, 0);
            this.bsprRawData.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprRawData.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprRawData.Name = "bsprRawData";
            this.bsprRawData.Padding = new System.Windows.Forms.Padding(5);
            this.bsprRawData.RowFronzen = 0;
            this.bsprRawData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprRawData.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprRawData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprRawData_Sheet1});
            this.bsprRawData.Size = new System.Drawing.Size(788, 406);
            this.bsprRawData.StyleID = null;
            this.bsprRawData.TabIndex = 4;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprRawData.TextTipAppearance = tipAppearance2;
            this.bsprRawData.UseCheckAll = false;
            this.bsprRawData.UseCommandIcon = false;
            this.bsprRawData.UseFilter = false;
            this.bsprRawData.UseGeneralContextMenu = true;
            this.bsprRawData.UseHeadColor = false;
            this.bsprRawData.UseMultiCheck = true;
            this.bsprRawData.UseOriginalEvent = false;
            this.bsprRawData.UseSpreadEdit = true;
            this.bsprRawData.UseStatusColor = false;
            this.bsprRawData.UseWidthMemory = true;
            this.bsprRawData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprRawData.WhenDeleteUseModify = false;
            this.bsprRawData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bsprRawData_KeyDown);
            this.bsprRawData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bsprRawData_KeyUp);
            // 
            // bsprRawData_Sheet1
            // 
            this.bsprRawData_Sheet1.Reset();
            this.bsprRawData_Sheet1.SheetName = "Sheet1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bapnlButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 471);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 35);
            this.panel1.TabIndex = 3;
            // 
            // bapnlButton
            // 
            this.bapnlButton.BLabelAutoPadding = true;
            this.bapnlButton.BssClass = "DataArrPanel";
            this.bapnlButton.Controls.Add(this.bbtnCancel);
            this.bapnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlButton.IsCondition = false;
            this.bapnlButton.Key = "";
            this.bapnlButton.Location = new System.Drawing.Point(0, 0);
            this.bapnlButton.Name = "bapnlButton";
            this.bapnlButton.Padding_Bottom = 5;
            this.bapnlButton.Padding_Left = 5;
            this.bapnlButton.Padding_Right = 5;
            this.bapnlButton.Padding_Top = 5;
            this.bapnlButton.Size = new System.Drawing.Size(806, 35);
            this.bapnlButton.Space = 5;
            this.bapnlButton.TabIndex = 2;
            this.bapnlButton.Title = "";
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
            this.bbtnCancel.Key = "FDC_POPUP_BUTTON_CLOSE";
            this.bbtnCancel.Location = new System.Drawing.Point(717, 5);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 25);
            this.bbtnCancel.TabIndex = 2;
            this.bbtnCancel.TabStop = false;
            this.bbtnCancel.Text = "Close";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bapnlHistoryInfo_1
            // 
            this.bapnlHistoryInfo_1.BLabelAutoPadding = true;
            this.bapnlHistoryInfo_1.BssClass = "DataArrPanel";
            this.bapnlHistoryInfo_1.Controls.Add(this.bbtnList);
            this.bapnlHistoryInfo_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bapnlHistoryInfo_1.IsCondition = true;
            this.bapnlHistoryInfo_1.Key = "";
            this.bapnlHistoryInfo_1.Location = new System.Drawing.Point(0, 0);
            this.bapnlHistoryInfo_1.Name = "bapnlHistoryInfo_1";
            this.bapnlHistoryInfo_1.Padding_Bottom = 5;
            this.bapnlHistoryInfo_1.Padding_Left = 5;
            this.bapnlHistoryInfo_1.Padding_Right = 5;
            this.bapnlHistoryInfo_1.Padding_Top = 5;
            this.bapnlHistoryInfo_1.Size = new System.Drawing.Size(806, 30);
            this.bapnlHistoryInfo_1.Space = 5;
            this.bapnlHistoryInfo_1.TabIndex = 0;
            this.bapnlHistoryInfo_1.Title = "";
            // 
            // bbtnList
            // 
            this.bbtnList.BssClass = "";
            this.bbtnList.ButtonHorizontalSpacing = 1;
            this.bbtnList.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnList.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnList.HorizontalMarginSpacing = 5;
            this.bbtnList.IsCondition = false;
            this.bbtnList.Location = new System.Drawing.Point(289, 0);
            this.bbtnList.MarginSpace = 5;
            this.bbtnList.Name = "bbtnList";
            this.bbtnList.Size = new System.Drawing.Size(517, 30);
            this.bbtnList.TabIndex = 7;
            this.bbtnList.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnList_ButtonClick);
            // 
            // ChartDataPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(818, 539);
            this.ContentsAreaMinHeight = 500;
            this.ContentsAreaMinWidth = 800;
            this.Controls.Add(this.pnlChartRowDataPopup);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "ChartDataPopup";
            this.Resizable = true;
            this.Controls.SetChildIndex(this.pnlContentsArea, 0);
            this.Controls.SetChildIndex(this.pnlChartRowDataPopup, 0);
            this.pnlChartRowDataPopup.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.bTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprRawData_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.bapnlHistoryInfo_1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.Panel pnlChartRowDataPopup;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlHistoryInfo_1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        protected BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        protected BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprRawData;
        protected FarPoint.Win.Spread.SheetView bsprData_Sheet1;
        protected FarPoint.Win.Spread.SheetView bsprRawData_Sheet1;
        protected BISTel.PeakPerformance.Client.BISTelControl.BTabControl bTabControl1;
    }
}