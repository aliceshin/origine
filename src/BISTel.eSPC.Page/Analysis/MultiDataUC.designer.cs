namespace BISTel.eSPC.Page.Analysis
{
    partial class MultiDataUC
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiDataUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bButtonList1 = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bSpread1);
            this.bTitlePanel1.Controls.Add(this.bButtonList1);
            this.bTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitlePanel1.ImageBottom = null;
            this.bTitlePanel1.ImageBottomRight = null;
            this.bTitlePanel1.ImageClose = null;
            this.bTitlePanel1.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageLeft")));
            this.bTitlePanel1.ImageLeftBottom = null;
            this.bTitlePanel1.ImageLeftFill = null;
            this.bTitlePanel1.ImageMax = null;
            this.bTitlePanel1.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageMiddle")));
            this.bTitlePanel1.ImageMin = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(931, 685);
            this.bTitlePanel1.TabIndex = 10;
            this.bTitlePanel1.Title = "Multi Data";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // bSpread1
            // 
            this.bSpread1.About = "3.0.2005.2005";
            this.bSpread1.AccessibleDescription = "bSpread1, Sheet1, Row 0, Column 0, ";
            this.bSpread1.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bSpread1.AddtionalCodeList")));
            this.bSpread1.AllowNewRow = true;
            this.bSpread1.AutoClipboard = false;
            this.bSpread1.AutoGenerateColumns = false;
            this.bSpread1.BssClass = "";
            this.bSpread1.ClickPos = new System.Drawing.Point(0, 0);
            this.bSpread1.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bSpread1.columnInformation = columnInfo1;
            this.bSpread1.ComboEnable = true;
            this.bSpread1.DataAutoHeadings = false;
            this.bSpread1.DataSet = null;
            this.bSpread1.DateToDateTimeFormat = false;
            this.bSpread1.DefaultDeleteValue = true;
            this.bSpread1.DisplayColumnHeader = true;
            this.bSpread1.DisplayRowHeader = true;
            this.bSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSpread1.EditModeReplace = true;
            this.bSpread1.FilterVisible = false;
            this.bSpread1.HeadHeight = 20F;
            this.bSpread1.IsCellCopy = false;
            this.bSpread1.IsMultiLanguage = true;
            this.bSpread1.IsReport = false;
            this.bSpread1.Key = "";
            this.bSpread1.Location = new System.Drawing.Point(0, 62);
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(931, 623);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 4;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance1;
            this.bSpread1.UseCheckAll = false;
            this.bSpread1.UseCommandIcon = false;
            this.bSpread1.UseFilter = false;
            this.bSpread1.UseGeneralContextMenu = true;
            this.bSpread1.UseHeadColor = false;
            this.bSpread1.UseOriginalEvent = false;
            this.bSpread1.UseSpreadEdit = true;
            this.bSpread1.UseWidthMemory = true;
            this.bSpread1.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.bSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.bSpread1_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(211)))), ((int)(((byte)(225))))), FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(241)))), ((int)(((byte)(245))))), false, false, false, true, true);
            this.bSpread1_Sheet1.AutoGenerateColumns = false;
            this.bSpread1_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bSpread1_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bSpread1_Sheet1.ColumnHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bSpread1_Sheet1.ColumnHeader.Rows.Default.Height = 18F;
            this.bSpread1_Sheet1.ColumnHeader.Rows.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bSpread1_Sheet1.ColumnHeader.Rows.Get(0).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bSpread1_Sheet1.ColumnHeader.Rows.Get(0).Height = 20F;
            this.bSpread1_Sheet1.ColumnHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bSpread1_Sheet1.DataAutoHeadings = false;
            this.bSpread1_Sheet1.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.bSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.bSpread1_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bSpread1_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bSpread1_Sheet1.RowHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bSpread1_Sheet1.RowHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bSpread1_Sheet1.Rows.Default.Height = 17F;
            this.bSpread1_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bSpread1_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bSpread1_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.bSpread1_Sheet1.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.bSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // bButtonList1
            // 
            this.bButtonList1.BssClass = "";
            this.bButtonList1.ButtonHorizontalSpacing = 1;
            this.bButtonList1.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bButtonList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bButtonList1.HorizontalMarginSpacing = 5;
            this.bButtonList1.IsCondition = false;
            this.bButtonList1.Location = new System.Drawing.Point(0, 32);
            this.bButtonList1.MarginSpace = 5;
            this.bButtonList1.Name = "bButtonList1";
            this.bButtonList1.Size = new System.Drawing.Size(931, 30);
            this.bButtonList1.TabIndex = 3;
            this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bButtonList1_ButtonClick);
            // 
            // MultiDataUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "MultiDataUC";
            this.Size = new System.Drawing.Size(931, 685);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        
        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bButtonList1;
    }
}
