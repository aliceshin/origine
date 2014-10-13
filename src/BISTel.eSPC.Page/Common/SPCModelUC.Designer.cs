namespace BISTel.eSPC.Page.Common
{
    partial class SPCModelUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCModelUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo3 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance3 = new FarPoint.Win.Spread.TipAppearance();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bButtonList1 = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bTitlePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BackColor = System.Drawing.Color.White;
            this.bTitlePanel1.BssClass = "";
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
            this.bTitlePanel1.ImagePanelMax = null;
            this.bTitlePanel1.ImagePanelMin = null;
            this.bTitlePanel1.ImagePanelNormal = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.MaxVisibleStateForMaxNormal = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.ShowMaxNormalButton = false;
            this.bTitlePanel1.ShowMinButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(448, 344);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "SPC Model";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // bSpread1
            // 
            this.bSpread1.About = "3.0.2005.2005";
            this.bSpread1.AccessibleDescription = "";
            this.bSpread1.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.AddFontColor = System.Drawing.Color.Blue;
            this.bSpread1.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bSpread1.AddtionalCodeList")));
            this.bSpread1.AllowNewRow = true;
            this.bSpread1.AutoClipboard = false;
            this.bSpread1.AutoGenerateColumns = false;
            this.bSpread1.BssClass = "";
            this.bSpread1.ClickPos = new System.Drawing.Point(0, 0);
            this.bSpread1.ColFronzen = 0;
            columnInfo3.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo3.CheckBoxFields")));
            columnInfo3.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo3.ComboFields")));
            columnInfo3.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo3.DefaultValue")));
            columnInfo3.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.KeyFields")));
            columnInfo3.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.MandatoryFields")));
            columnInfo3.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo3.SaveTableInfo")));
            columnInfo3.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo3.UniqueFields")));
            this.bSpread1.columnInformation = columnInfo3;
            this.bSpread1.ComboEnable = true;
            this.bSpread1.DataAutoHeadings = false;
            this.bSpread1.DataSet = null;
            this.bSpread1.DataSource_V2 = null;
            this.bSpread1.DateToDateTimeFormat = false;
            this.bSpread1.DefaultDeleteValue = true;
            this.bSpread1.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.DeleteFontColor = System.Drawing.Color.Gray;
            this.bSpread1.DisplayColumnHeader = true;
            this.bSpread1.DisplayRowHeader = true;
            this.bSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSpread1.EditModeReplace = true;
            this.bSpread1.FilterVisible = false;
            this.bSpread1.HeadHeight = 20F;
            this.bSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bSpread1.IsCellCopy = false;
            this.bSpread1.IsMultiLanguage = true;
            this.bSpread1.IsReport = false;
            this.bSpread1.Key = "";
            this.bSpread1.Location = new System.Drawing.Point(0, 62);
            this.bSpread1.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.ModifyFontColor = System.Drawing.Color.Red;
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(448, 282);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 6;
            tipAppearance3.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance3.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance3;
            this.bSpread1.UseCheckAll = false;
            this.bSpread1.UseCommandIcon = false;
            this.bSpread1.UseFilter = false;
            this.bSpread1.UseGeneralContextMenu = true;
            this.bSpread1.UseHeadColor = false;
            this.bSpread1.UseMultiCheck = true;
            this.bSpread1.UseOriginalEvent = false;
            this.bSpread1.UseSpreadEdit = true;
            this.bSpread1.UseStatusColor = false;
            this.bSpread1.UseWidthMemory = true;
            this.bSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bSpread1.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
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
            this.bButtonList1.Size = new System.Drawing.Size(448, 30);
            this.bButtonList1.TabIndex = 7;
            this.bButtonList1.Visible = false;
            this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bButtonList1_ButtonClick);
            // 
            // SPCModelUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "SPCModelUC";
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
        public BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        public PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private PeakPerformance.Client.BISTelControl.BButtonList bButtonList1;
    }
}
