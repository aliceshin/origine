namespace BISTel.eSPC.Condition.Analysis
{
    partial class MultiDataCondition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiDataCondition));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.bSpread1 = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlHidden = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.bTPnlSortingKey = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bTPnlPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bTPnlStepMapping = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.pnlHidden.SuspendLayout();
            this.panel2.SuspendLayout();
            this.bTPnlStepMapping.SuspendLayout();
            this.SuspendLayout();
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
            columnInfo1.CheckBoxFields = null;
            columnInfo1.ComboFields = null;
            columnInfo1.DefaultValue = null;
            columnInfo1.KeyFields = null;
            columnInfo1.MandatoryFields = null;
            columnInfo1.SaveTableInfo = null;
            columnInfo1.UniqueFields = null;
            this.bSpread1.columnInformation = columnInfo1;
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
            this.bSpread1.IsCellCopy = false;
            this.bSpread1.IsEditComboListItem = false;
            this.bSpread1.IsMultiLanguage = true;
            this.bSpread1.IsReport = false;
            this.bSpread1.Key = "";
            this.bSpread1.Location = new System.Drawing.Point(0, 56);
            this.bSpread1.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bSpread1.ModifyFontColor = System.Drawing.Color.Red;
            this.bSpread1.Name = "bSpread1";
            this.bSpread1.RowFronzen = 0;
            this.bSpread1.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bSpread1.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bSpread1.Size = new System.Drawing.Size(229, 110);
            this.bSpread1.StyleID = null;
            this.bSpread1.TabIndex = 0;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bSpread1.TextTipAppearance = tipAppearance1;
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
            this.bSpread1.WhenDeleteUseModify = false;
            this.bSpread1.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bSpread1_ButtonClicked);
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // pnlHidden
            // 
            this.pnlHidden.Controls.Add(this.panel2);
            this.pnlHidden.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHidden.Location = new System.Drawing.Point(0, 32);
            this.pnlHidden.Name = "pnlHidden";
            this.pnlHidden.Size = new System.Drawing.Size(229, 24);
            this.pnlHidden.TabIndex = 51;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnPlus);
            this.panel2.Controls.Add(this.btnMinus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(143, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(86, 24);
            this.panel2.TabIndex = 26;
            // 
            // btnPlus
            // 
            this.btnPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlus.BackgroundImage")));
            this.btnPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlus.Location = new System.Drawing.Point(61, 5);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(23, 18);
            this.btnPlus.TabIndex = 45;
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMinus.BackgroundImage")));
            this.btnMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinus.Location = new System.Drawing.Point(36, 5);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(23, 18);
            this.btnMinus.TabIndex = 44;
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Visible = false;
            // 
            // bTPnlSortingKey
            // 
            this.bTPnlSortingKey.BssClass = "TitlePanel";
            this.bTPnlSortingKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.bTPnlSortingKey.ImageBottom = null;
            this.bTPnlSortingKey.ImageBottomRight = null;
            this.bTPnlSortingKey.ImageClose = null;
            this.bTPnlSortingKey.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTPnlSortingKey.ImageLeft")));
            this.bTPnlSortingKey.ImageLeftBottom = null;
            this.bTPnlSortingKey.ImageLeftFill = null;
            this.bTPnlSortingKey.ImageMax = null;
            this.bTPnlSortingKey.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTPnlSortingKey.ImageMiddle")));
            this.bTPnlSortingKey.ImageMin = null;
            this.bTPnlSortingKey.ImagePanelMax = null;
            this.bTPnlSortingKey.ImagePanelMin = null;
            this.bTPnlSortingKey.ImagePanelNormal = null;
            this.bTPnlSortingKey.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTPnlSortingKey.ImageRight")));
            this.bTPnlSortingKey.ImageRightFill = null;
            this.bTPnlSortingKey.IsPopupMax = false;
            this.bTPnlSortingKey.IsSelected = false;
            this.bTPnlSortingKey.Key = "";
            this.bTPnlSortingKey.Location = new System.Drawing.Point(0, 138);
            this.bTPnlSortingKey.MaxControlName = "";
            this.bTPnlSortingKey.MaxResizable = false;
            this.bTPnlSortingKey.MaxVisibleStateForMaxNormal = false;
            this.bTPnlSortingKey.Name = "bTPnlSortingKey";
            this.bTPnlSortingKey.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTPnlSortingKey.PaddingTop = 4;
            this.bTPnlSortingKey.PopMaxHeight = 0;
            this.bTPnlSortingKey.PopMaxWidth = 0;
            this.bTPnlSortingKey.RightPadding = 5;
            this.bTPnlSortingKey.SelectedColor = System.Drawing.Color.Blue;
            this.bTPnlSortingKey.ShowCloseButton = false;
            this.bTPnlSortingKey.ShowMaxNormalButton = false;
            this.bTPnlSortingKey.ShowMinButton = false;
            this.bTPnlSortingKey.Size = new System.Drawing.Size(229, 139);
            this.bTPnlSortingKey.TabIndex = 24;
            this.bTPnlSortingKey.Title = "SortingKey";
            this.bTPnlSortingKey.TitleColor = System.Drawing.Color.Black;
            this.bTPnlSortingKey.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTPnlSortingKey.TopPadding = 5;
            // 
            // bTPnlPeriod
            // 
            this.bTPnlPeriod.BssClass = "TitlePanel";
            this.bTPnlPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.bTPnlPeriod.ImageBottom = null;
            this.bTPnlPeriod.ImageBottomRight = null;
            this.bTPnlPeriod.ImageClose = null;
            this.bTPnlPeriod.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTPnlPeriod.ImageLeft")));
            this.bTPnlPeriod.ImageLeftBottom = null;
            this.bTPnlPeriod.ImageLeftFill = null;
            this.bTPnlPeriod.ImageMax = null;
            this.bTPnlPeriod.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTPnlPeriod.ImageMiddle")));
            this.bTPnlPeriod.ImageMin = null;
            this.bTPnlPeriod.ImagePanelMax = null;
            this.bTPnlPeriod.ImagePanelMin = null;
            this.bTPnlPeriod.ImagePanelNormal = null;
            this.bTPnlPeriod.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTPnlPeriod.ImageRight")));
            this.bTPnlPeriod.ImageRightFill = null;
            this.bTPnlPeriod.IsPopupMax = false;
            this.bTPnlPeriod.IsSelected = false;
            this.bTPnlPeriod.Key = "";
            this.bTPnlPeriod.Location = new System.Drawing.Point(0, 0);
            this.bTPnlPeriod.MaxControlName = "";
            this.bTPnlPeriod.MaxResizable = false;
            this.bTPnlPeriod.MaxVisibleStateForMaxNormal = false;
            this.bTPnlPeriod.Name = "bTPnlPeriod";
            this.bTPnlPeriod.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTPnlPeriod.PaddingTop = 4;
            this.bTPnlPeriod.PopMaxHeight = 0;
            this.bTPnlPeriod.PopMaxWidth = 0;
            this.bTPnlPeriod.RightPadding = 5;
            this.bTPnlPeriod.SelectedColor = System.Drawing.Color.Blue;
            this.bTPnlPeriod.ShowCloseButton = false;
            this.bTPnlPeriod.ShowMaxNormalButton = false;
            this.bTPnlPeriod.ShowMinButton = false;
            this.bTPnlPeriod.Size = new System.Drawing.Size(229, 138);
            this.bTPnlPeriod.TabIndex = 25;
            this.bTPnlPeriod.Title = "Period";
            this.bTPnlPeriod.TitleColor = System.Drawing.Color.Black;
            this.bTPnlPeriod.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTPnlPeriod.TopPadding = 5;
            // 
            // bTPnlStepMapping
            // 
            this.bTPnlStepMapping.BssClass = "TitlePanel";
            this.bTPnlStepMapping.Controls.Add(this.bSpread1);
            this.bTPnlStepMapping.Controls.Add(this.pnlHidden);
            this.bTPnlStepMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.bTPnlStepMapping.ImageBottom = null;
            this.bTPnlStepMapping.ImageBottomRight = null;
            this.bTPnlStepMapping.ImageClose = null;
            this.bTPnlStepMapping.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTPnlStepMapping.ImageLeft")));
            this.bTPnlStepMapping.ImageLeftBottom = null;
            this.bTPnlStepMapping.ImageLeftFill = null;
            this.bTPnlStepMapping.ImageMax = null;
            this.bTPnlStepMapping.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTPnlStepMapping.ImageMiddle")));
            this.bTPnlStepMapping.ImageMin = null;
            this.bTPnlStepMapping.ImagePanelMax = null;
            this.bTPnlStepMapping.ImagePanelMin = null;
            this.bTPnlStepMapping.ImagePanelNormal = null;
            this.bTPnlStepMapping.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTPnlStepMapping.ImageRight")));
            this.bTPnlStepMapping.ImageRightFill = null;
            this.bTPnlStepMapping.IsPopupMax = false;
            this.bTPnlStepMapping.IsSelected = false;
            this.bTPnlStepMapping.Key = "";
            this.bTPnlStepMapping.Location = new System.Drawing.Point(0, 277);
            this.bTPnlStepMapping.MaxControlName = "";
            this.bTPnlStepMapping.MaxResizable = false;
            this.bTPnlStepMapping.MaxVisibleStateForMaxNormal = false;
            this.bTPnlStepMapping.Name = "bTPnlStepMapping";
            this.bTPnlStepMapping.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTPnlStepMapping.PaddingTop = 4;
            this.bTPnlStepMapping.PopMaxHeight = 0;
            this.bTPnlStepMapping.PopMaxWidth = 0;
            this.bTPnlStepMapping.RightPadding = 5;
            this.bTPnlStepMapping.SelectedColor = System.Drawing.Color.Blue;
            this.bTPnlStepMapping.ShowCloseButton = false;
            this.bTPnlStepMapping.ShowMaxNormalButton = false;
            this.bTPnlStepMapping.ShowMinButton = false;
            this.bTPnlStepMapping.Size = new System.Drawing.Size(229, 166);
            this.bTPnlStepMapping.TabIndex = 26;
            this.bTPnlStepMapping.Title = "Step별 Data Mapping";
            this.bTPnlStepMapping.TitleColor = System.Drawing.Color.Black;
            this.bTPnlStepMapping.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTPnlStepMapping.TopPadding = 5;
            // 
            // MultiDataCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.bTPnlStepMapping);
            this.Controls.Add(this.bTPnlSortingKey);
            this.Controls.Add(this.bTPnlPeriod);
            this.Name = "MultiDataCondition";
            this.Size = new System.Drawing.Size(229, 462);
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.pnlHidden.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.bTPnlStepMapping.ResumeLayout(false);
            this.bTPnlStepMapping.PerformLayout();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Panel pnlHidden;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTPnlSortingKey;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTPnlPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTPnlStepMapping;
    }
}
