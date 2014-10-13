namespace BISTel.eSPC.Page.Modeling.MET.Modeling
{
    partial class SPCModelingUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCModelingUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlSPCBase = new System.Windows.Forms.Panel();
            this.btpnlSPCConfig = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bsprTempForImport = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bbtnListTop = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.bbtnCopyModel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.btxtParamAlias = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblParamAlias = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtSPCModelName = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblSPCModelName = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlSPCBase.SuspendLayout();
            this.btpnlSPCConfig.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprTempForImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
            this.bbtnListTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSPCBase
            // 
            this.pnlSPCBase.Controls.Add(this.btpnlSPCConfig);
            this.pnlSPCBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSPCBase.Location = new System.Drawing.Point(0, 0);
            this.pnlSPCBase.Name = "pnlSPCBase";
            this.pnlSPCBase.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.TabIndex = 4;
            // 
            // btpnlSPCConfig
            // 
            this.btpnlSPCConfig.BssClass = "TitlePanel";
            this.btpnlSPCConfig.Controls.Add(this.panel1);
            this.btpnlSPCConfig.Controls.Add(this.bbtnList);
            this.btpnlSPCConfig.Controls.Add(this.bbtnListTop);
            this.btpnlSPCConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btpnlSPCConfig.ImageBottom = null;
            this.btpnlSPCConfig.ImageBottomRight = null;
            this.btpnlSPCConfig.ImageClose = null;
            this.btpnlSPCConfig.ImageLeft = ((System.Drawing.Image)(resources.GetObject("btpnlSPCConfig.ImageLeft")));
            this.btpnlSPCConfig.ImageLeftBottom = null;
            this.btpnlSPCConfig.ImageLeftFill = null;
            this.btpnlSPCConfig.ImageMax = null;
            this.btpnlSPCConfig.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("btpnlSPCConfig.ImageMiddle")));
            this.btpnlSPCConfig.ImageMin = null;
            this.btpnlSPCConfig.ImagePanelMax = null;
            this.btpnlSPCConfig.ImagePanelMin = null;
            this.btpnlSPCConfig.ImagePanelNormal = null;
            this.btpnlSPCConfig.ImageRight = ((System.Drawing.Image)(resources.GetObject("btpnlSPCConfig.ImageRight")));
            this.btpnlSPCConfig.ImageRightFill = null;
            this.btpnlSPCConfig.IsPopupMax = false;
            this.btpnlSPCConfig.IsSelected = false;
            this.btpnlSPCConfig.Key = "";
            this.btpnlSPCConfig.Location = new System.Drawing.Point(0, 0);
            this.btpnlSPCConfig.MaxControlName = "";
            this.btpnlSPCConfig.MaxResizable = false;
            this.btpnlSPCConfig.MaxVisibleStateForMaxNormal = false;
            this.btpnlSPCConfig.Name = "btpnlSPCConfig";
            this.btpnlSPCConfig.Padding = new System.Windows.Forms.Padding(0, 29, 0, 0);
            this.btpnlSPCConfig.PaddingTop = 4;
            this.btpnlSPCConfig.PopMaxHeight = 0;
            this.btpnlSPCConfig.PopMaxWidth = 0;
            this.btpnlSPCConfig.RightPadding = 5;
            this.btpnlSPCConfig.ShowCloseButton = false;
            this.btpnlSPCConfig.ShowMaxNormalButton = false;
            this.btpnlSPCConfig.ShowMinButton = false;
            this.btpnlSPCConfig.Size = new System.Drawing.Size(759, 495);
            this.btpnlSPCConfig.TabIndex = 0;
            this.btpnlSPCConfig.Title = "SPC Model";
            this.btpnlSPCConfig.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(151)))), ((int)(((byte)(212)))));
            this.btpnlSPCConfig.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btpnlSPCConfig.TopPadding = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bsprTempForImport);
            this.panel1.Controls.Add(this.bsprData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 406);
            this.panel1.TabIndex = 5;
            // 
            // bsprTempForImport
            // 
            this.bsprTempForImport.About = "3.0.2005.2005";
            this.bsprTempForImport.AccessibleDescription = "";
            this.bsprTempForImport.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprTempForImport.AddFontColor = System.Drawing.Color.Blue;
            this.bsprTempForImport.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprTempForImport.AddtionalCodeList")));
            this.bsprTempForImport.AllowNewRow = true;
            this.bsprTempForImport.AutoClipboard = false;
            this.bsprTempForImport.AutoGenerateColumns = false;
            this.bsprTempForImport.BssClass = "";
            this.bsprTempForImport.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprTempForImport.ColFronzen = 0;
            columnInfo1.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.CheckBoxFields")));
            columnInfo1.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo1.ComboFields")));
            columnInfo1.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo1.DefaultValue")));
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo1.SaveTableInfo")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bsprTempForImport.columnInformation = columnInfo1;
            this.bsprTempForImport.ComboEnable = true;
            this.bsprTempForImport.DataAutoHeadings = false;
            this.bsprTempForImport.DataSet = null;
            this.bsprTempForImport.DataSource_V2 = null;
            this.bsprTempForImport.DateToDateTimeFormat = false;
            this.bsprTempForImport.DefaultDeleteValue = true;
            this.bsprTempForImport.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprTempForImport.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprTempForImport.DisplayColumnHeader = true;
            this.bsprTempForImport.DisplayRowHeader = true;
            this.bsprTempForImport.EditModeReplace = true;
            this.bsprTempForImport.FilterVisible = false;
            this.bsprTempForImport.HeadHeight = 20F;
            this.bsprTempForImport.IsCellCopy = false;
            this.bsprTempForImport.IsEditComboListItem = false;
            this.bsprTempForImport.IsMultiLanguage = true;
            this.bsprTempForImport.IsReport = false;
            this.bsprTempForImport.Key = "";
            this.bsprTempForImport.Location = new System.Drawing.Point(0, 0);
            this.bsprTempForImport.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprTempForImport.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprTempForImport.Name = "bsprTempForImport";
            this.bsprTempForImport.RowFronzen = 0;
            this.bsprTempForImport.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprTempForImport.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            this.bsprTempForImport.Size = new System.Drawing.Size(200, 100);
            this.bsprTempForImport.StyleID = null;
            this.bsprTempForImport.TabIndex = 5;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprTempForImport.TextTipAppearance = tipAppearance1;
            this.bsprTempForImport.UseCheckAll = false;
            this.bsprTempForImport.UseCommandIcon = false;
            this.bsprTempForImport.UseFilter = false;
            this.bsprTempForImport.UseGeneralContextMenu = true;
            this.bsprTempForImport.UseHeadColor = false;
            this.bsprTempForImport.UseMultiCheck = true;
            this.bsprTempForImport.UseOriginalEvent = false;
            this.bsprTempForImport.UseSpreadEdit = true;
            this.bsprTempForImport.UseStatusColor = false;
            this.bsprTempForImport.UseWidthMemory = true;
            this.bsprTempForImport.Visible = false;
            this.bsprTempForImport.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
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
            columnInfo2.CheckBoxFields = null;
            columnInfo2.ComboFields = null;
            columnInfo2.DefaultValue = null;
            columnInfo2.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.KeyFields")));
            columnInfo2.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.MandatoryFields")));
            columnInfo2.SaveTableInfo = null;
            columnInfo2.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.UniqueFields")));
            this.bsprData.columnInformation = columnInfo2;
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
            this.bsprData.IsCellCopy = false;
            this.bsprData.IsEditComboListItem = false;
            this.bsprData.IsMultiLanguage = false;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(0, 0);
            this.bsprData.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprData.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprData.Name = "bsprData";
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(759, 406);
            this.bsprData.StyleID = null;
            this.bsprData.TabIndex = 4;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprData.TextTipAppearance = tipAppearance2;
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
            this.bsprData.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.bsprData_SelectionChanged);
            this.bsprData.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.bsprData_Change);
            this.bsprData.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.bsprData_LeaveCell);
            this.bsprData.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.bsprData_CellClick);
            this.bsprData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bsprData_KeyDown);
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
            // 
            // bbtnList
            // 
            this.bbtnList.BssClass = "";
            this.bbtnList.ButtonHorizontalSpacing = 1;
            this.bbtnList.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnList.Dock = System.Windows.Forms.DockStyle.Top;
            this.bbtnList.HorizontalMarginSpacing = 5;
            this.bbtnList.IsCondition = false;
            this.bbtnList.Location = new System.Drawing.Point(0, 59);
            this.bbtnList.MarginSpace = 5;
            this.bbtnList.Name = "bbtnList";
            this.bbtnList.Size = new System.Drawing.Size(759, 30);
            this.bbtnList.TabIndex = 3;
            this.bbtnList.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnList_ButtonClick);
            // 
            // bbtnListTop
            // 
            this.bbtnListTop.BssClass = "";
            this.bbtnListTop.ButtonHorizontalSpacing = 1;
            this.bbtnListTop.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnListTop.Controls.Add(this.bbtnCopyModel);
            this.bbtnListTop.Controls.Add(this.btxtParamAlias);
            this.bbtnListTop.Controls.Add(this.blblParamAlias);
            this.bbtnListTop.Controls.Add(this.btxtSPCModelName);
            this.bbtnListTop.Controls.Add(this.blblSPCModelName);
            this.bbtnListTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.bbtnListTop.HorizontalMarginSpacing = 5;
            this.bbtnListTop.IsCondition = false;
            this.bbtnListTop.Location = new System.Drawing.Point(0, 29);
            this.bbtnListTop.MarginSpace = 5;
            this.bbtnListTop.Name = "bbtnListTop";
            this.bbtnListTop.Size = new System.Drawing.Size(759, 30);
            this.bbtnListTop.TabIndex = 4;
            // 
            // bbtnCopyModel
            // 
            this.bbtnCopyModel.AutoButtonSize = false;
            this.bbtnCopyModel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCopyModel.BackgroundImage")));
            this.bbtnCopyModel.BssClass = "ConditionButton";
            this.bbtnCopyModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCopyModel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnCopyModel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCopyModel.IsMultiLanguage = true;
            this.bbtnCopyModel.Key = "";
            this.bbtnCopyModel.Location = new System.Drawing.Point(600, 1);
            this.bbtnCopyModel.Name = "bbtnCopyModel";
            this.bbtnCopyModel.Size = new System.Drawing.Size(80, 27);
            this.bbtnCopyModel.TabIndex = 2;
            this.bbtnCopyModel.TabStop = false;
            this.bbtnCopyModel.Text = "Model Info...";
            this.bbtnCopyModel.ToolTipText = "";
            this.bbtnCopyModel.UseVisualStyleBackColor = true;
            this.bbtnCopyModel.Click += new System.EventHandler(this.bbtnCopyModel_Click_1);
            // 
            // btxtParamAlias
            // 
            this.btxtParamAlias.BackColor = System.Drawing.SystemColors.Window;
            this.btxtParamAlias.BssClass = "";
            this.btxtParamAlias.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtParamAlias.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtParamAlias.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtParamAlias.IsMultiLanguage = true;
            this.btxtParamAlias.Key = "";
            this.btxtParamAlias.Location = new System.Drawing.Point(435, 4);
            this.btxtParamAlias.Name = "btxtParamAlias";
            this.btxtParamAlias.ReadOnly = true;
            this.btxtParamAlias.Size = new System.Drawing.Size(155, 21);
            this.btxtParamAlias.TabIndex = 1;
            // 
            // blblParamAlias
            // 
            this.blblParamAlias.BssClass = "";
            this.blblParamAlias.CustomTextAlign = "";
            this.blblParamAlias.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblParamAlias.IsMultiLanguage = true;
            this.blblParamAlias.Key = "";
            this.blblParamAlias.Location = new System.Drawing.Point(362, 3);
            this.blblParamAlias.Name = "blblParamAlias";
            this.blblParamAlias.Size = new System.Drawing.Size(100, 23);
            this.blblParamAlias.TabIndex = 0;
            this.blblParamAlias.Text = "Param Alias :";
            this.blblParamAlias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtSPCModelName
            // 
            this.btxtSPCModelName.BackColor = System.Drawing.SystemColors.Window;
            this.btxtSPCModelName.BssClass = "";
            this.btxtSPCModelName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtSPCModelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtSPCModelName.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtSPCModelName.IsMultiLanguage = true;
            this.btxtSPCModelName.Key = "";
            this.btxtSPCModelName.Location = new System.Drawing.Point(100, 3);
            this.btxtSPCModelName.Name = "btxtSPCModelName";
            this.btxtSPCModelName.ReadOnly = true;
            this.btxtSPCModelName.Size = new System.Drawing.Size(258, 21);
            this.btxtSPCModelName.TabIndex = 1;
            // 
            // blblSPCModelName
            // 
            this.blblSPCModelName.BssClass = "";
            this.blblSPCModelName.CustomTextAlign = "";
            this.blblSPCModelName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblSPCModelName.IsMultiLanguage = true;
            this.blblSPCModelName.Key = "";
            this.blblSPCModelName.Location = new System.Drawing.Point(3, 3);
            this.blblSPCModelName.Name = "blblSPCModelName";
            this.blblSPCModelName.Size = new System.Drawing.Size(100, 23);
            this.blblSPCModelName.TabIndex = 0;
            this.blblSPCModelName.Text = "SPC Model Name :";
            this.blblSPCModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SPCModelingUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSPCBase);
            this.Name = "SPCModelingUC";
            this.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.ResumeLayout(false);
            this.btpnlSPCConfig.ResumeLayout(false);
            this.btpnlSPCConfig.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprTempForImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.bbtnListTop.ResumeLayout(false);
            this.bbtnListTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.Panel pnlSPCBase;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel btpnlSPCConfig;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        private FarPoint.Win.Spread.SheetView bsprData_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtSPCModelName;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblSPCModelName;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnListTop;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtParamAlias;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblParamAlias;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCopyModel;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprTempForImport;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
    }
}
