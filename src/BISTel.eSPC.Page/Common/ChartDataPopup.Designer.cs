namespace BISTel.eSPC.Page.Common
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
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo3 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance3 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlChartRowDataPopup = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlHistoryInfo_1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.pnlChartRowDataPopup.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
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
            this.panel2.Controls.Add(this.bsprData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.panel2.Size = new System.Drawing.Size(806, 441);
            this.panel2.TabIndex = 4;
            // 
            // bsprData
            // 
            this.bsprData.About = "3.0.2005.2005";
            this.bsprData.AccessibleDescription = "";
            this.bsprData.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprData.AddtionalCodeList")));
            this.bsprData.AllowNewRow = true;
            this.bsprData.AutoClipboard = false;
            this.bsprData.AutoGenerateColumns = false;
            this.bsprData.BssClass = "";
            this.bsprData.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprData.ColFronzen = 0;
            columnInfo3.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.KeyFields")));
            columnInfo3.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.MandatoryFields")));
            columnInfo3.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.UniqueFields")));
            this.bsprData.columnInformation = columnInfo3;
            this.bsprData.ComboEnable = true;
            this.bsprData.DataAutoHeadings = false;
            this.bsprData.DataSet = null;
            this.bsprData.DateToDateTimeFormat = false;
            this.bsprData.DefaultDeleteValue = true;
            this.bsprData.DisplayColumnHeader = true;
            this.bsprData.DisplayRowHeader = true;
            this.bsprData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprData.EditModeReplace = true;
            this.bsprData.FilterVisible = false;
            this.bsprData.HeadHeight = 20F;
            this.bsprData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprData.IsCellCopy = false;
            this.bsprData.IsMultiLanguage = false;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(5, 5);
            this.bsprData.Name = "bsprData";
            this.bsprData.Padding = new System.Windows.Forms.Padding(5);
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(796, 436);
            this.bsprData.TabIndex = 3;
            tipAppearance3.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance3.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprData.TextTipAppearance = tipAppearance3;
            this.bsprData.UseFilter = false;
            this.bsprData.UseGeneralContextMenu = true;
            this.bsprData.UseHeadColor = false;
            this.bsprData.UseOriginalEvent = false;
            this.bsprData.UseSpreadEdit = true;
            this.bsprData.UseWidthMemory = true;
            this.bsprData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprData.WhenDeleteUseModify = false;
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
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
            this.bbtnList.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bbtnList_ButtonClick);
            // 
            // ChartRowDataPopup
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
            this.Name = "ChartRowDataPopup";
            this.Resizable = true;
            this.Controls.SetChildIndex(this.pnlContentsArea, 0);
            this.Controls.SetChildIndex(this.pnlChartRowDataPopup, 0);
            this.pnlChartRowDataPopup.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.bapnlHistoryInfo_1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private System.Windows.Forms.Panel pnlChartRowDataPopup;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlHistoryInfo_1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        private FarPoint.Win.Spread.SheetView bsprData_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
    }
}