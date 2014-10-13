namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class ContextConfigurationUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextConfigurationUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pnlSPCBase = new System.Windows.Forms.Panel();
            this.gbContext = new System.Windows.Forms.GroupBox();
            this.bsprContext = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprContext_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bbtnListContext = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.gbModel = new System.Windows.Forms.GroupBox();
            this.tlpModel5 = new System.Windows.Forms.TableLayoutPanel();
            this.bchkValSameModule = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkInheritYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.tlpModel4 = new System.Windows.Forms.TableLayoutPanel();
            this.bchkCreateSubAutoCalc = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkCreateSubInterlock = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.tlpModel2 = new System.Windows.Forms.TableLayoutPanel();
            this.bcboAutoType = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.blblAutoSettings = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blbMode = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcboMode = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.tlpModel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btxtSampleCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblSampleCount = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bchkMainYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkInterlockYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkAutoCalcYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkActive = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkAutoSubYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.gbParam = new System.Windows.Forms.GroupBox();
            this.bLine1 = new BISTel.PeakPerformance.Client.BISTelControl.BLine();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tlpParam = new System.Windows.Forms.TableLayoutPanel();
            this.blblParamAlias = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlParamAlias = new System.Windows.Forms.Panel();
            this.btxtParamAlias = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bbtnParamAlias = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.pnlSPCBase.SuspendLayout();
            this.gbContext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext_Sheet1)).BeginInit();
            this.gbModel.SuspendLayout();
            this.tlpModel5.SuspendLayout();
            this.tlpModel4.SuspendLayout();
            this.tlpModel2.SuspendLayout();
            this.tlpModel3.SuspendLayout();
            this.gbParam.SuspendLayout();
            this.tlpParam.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlParamAlias.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSPCBase
            // 
            this.pnlSPCBase.Controls.Add(this.gbContext);
            this.pnlSPCBase.Controls.Add(this.gbModel);
            this.pnlSPCBase.Controls.Add(this.gbParam);
            this.pnlSPCBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSPCBase.Location = new System.Drawing.Point(0, 0);
            this.pnlSPCBase.Name = "pnlSPCBase";
            this.pnlSPCBase.Size = new System.Drawing.Size(885, 458);
            this.pnlSPCBase.TabIndex = 4;
            // 
            // gbContext
            // 
            this.gbContext.Controls.Add(this.bsprContext);
            this.gbContext.Controls.Add(this.bbtnListContext);
            this.gbContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbContext.Location = new System.Drawing.Point(0, 177);
            this.gbContext.Name = "gbContext";
            this.gbContext.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.gbContext.Size = new System.Drawing.Size(885, 281);
            this.gbContext.TabIndex = 3;
            this.gbContext.TabStop = false;
            this.gbContext.Text = "Context";
            // 
            // bsprContext
            // 
            this.bsprContext.About = "3.0.2005.2005";
            this.bsprContext.AccessibleDescription = "";
            this.bsprContext.AddFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.AddFontColor = System.Drawing.Color.Blue;
            this.bsprContext.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprContext.AddtionalCodeList")));
            this.bsprContext.AllowNewRow = true;
            this.bsprContext.AutoClipboard = false;
            this.bsprContext.AutoGenerateColumns = false;
            this.bsprContext.BssClass = "";
            this.bsprContext.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprContext.ColFronzen = 0;
            columnInfo1.CheckBoxFields = null;
            columnInfo1.ComboFields = null;
            columnInfo1.DefaultValue = null;
            columnInfo1.KeyFields = null;
            columnInfo1.MandatoryFields = null;
            columnInfo1.SaveTableInfo = null;
            columnInfo1.UniqueFields = null;
            this.bsprContext.columnInformation = columnInfo1;
            this.bsprContext.ComboEnable = true;
            this.bsprContext.DataAutoHeadings = false;
            this.bsprContext.DataSet = null;
            this.bsprContext.DataSource_V2 = null;
            this.bsprContext.DateToDateTimeFormat = false;
            this.bsprContext.DefaultDeleteValue = true;
            this.bsprContext.DeleteFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.DeleteFontColor = System.Drawing.Color.Gray;
            this.bsprContext.DisplayColumnHeader = true;
            this.bsprContext.DisplayRowHeader = true;
            this.bsprContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprContext.EditModeReplace = true;
            this.bsprContext.FilterVisible = false;
            this.bsprContext.HeadHeight = 20F;
            this.bsprContext.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprContext.IsCellCopy = false;
            this.bsprContext.IsEditComboListItem = false;
            this.bsprContext.IsMultiLanguage = true;
            this.bsprContext.IsReport = false;
            this.bsprContext.Key = "";
            this.bsprContext.Location = new System.Drawing.Point(3, 42);
            this.bsprContext.ModifyFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext.ModifyFontColor = System.Drawing.Color.Red;
            this.bsprContext.Name = "bsprContext";
            this.bsprContext.RowFronzen = 0;
            this.bsprContext.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprContext.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprContext.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprContext_Sheet1});
            this.bsprContext.Size = new System.Drawing.Size(879, 236);
            this.bsprContext.StyleID = null;
            this.bsprContext.TabIndex = 4;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprContext.TextTipAppearance = tipAppearance1;
            this.bsprContext.UseCheckAll = false;
            this.bsprContext.UseCommandIcon = false;
            this.bsprContext.UseFilter = false;
            this.bsprContext.UseGeneralContextMenu = true;
            this.bsprContext.UseHeadColor = false;
            this.bsprContext.UseMultiCheck = true;
            this.bsprContext.UseOriginalEvent = false;
            this.bsprContext.UseSpreadEdit = true;
            this.bsprContext.UseStatusColor = false;
            this.bsprContext.UseWidthMemory = true;
            this.bsprContext.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bsprContext.WhenDeleteUseModify = false;
            this.bsprContext.AddRowEvent += new BISTel.PeakPerformance.Client.BISTelControl.BSpread.AddRowDelegate(this.bsprContext_AddRowEvent);
            this.bsprContext.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.bsprContext_Change);
            this.bsprContext.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.bsprContext_LeaveCell);
            this.bsprContext.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprContext_ButtonClicked);
            this.bsprContext.ComboSelChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.bsprContext_ComboSelChange);
            this.bsprContext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bsprContext_KeyDown);
            // 
            // bsprContext_Sheet1
            // 
            this.bsprContext_Sheet1.Reset();
            this.bsprContext_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.bsprContext_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.bsprContext_Sheet1.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(211)))), ((int)(((byte)(225))))), FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.Empty, System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30))))), System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(241)))), ((int)(((byte)(245))))), false, false, false, true, true);
            this.bsprContext_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bsprContext_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bsprContext_Sheet1.ColumnHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprContext_Sheet1.ColumnHeader.Rows.Default.Height = 18F;
            this.bsprContext_Sheet1.ColumnHeader.Rows.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprContext_Sheet1.ColumnHeader.Rows.Get(0).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bsprContext_Sheet1.ColumnHeader.Rows.Get(0).Height = 20F;
            this.bsprContext_Sheet1.ColumnHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprContext_Sheet1.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.bsprContext_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.bsprContext_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprContext_Sheet1.RowHeader.DefaultStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.bsprContext_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(123)))), ((int)(((byte)(168)))));
            this.bsprContext_Sheet1.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.bsprContext_Sheet1.RowHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprContext_Sheet1.RowHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Raised, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))), System.Drawing.SystemColors.ControlLightLight, System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(151)))), ((int)(((byte)(193))))));
            this.bsprContext_Sheet1.Rows.Default.Height = 17F;
            this.bsprContext_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(216)))), ((int)(((byte)(237)))));
            this.bsprContext_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(145)))), ((int)(((byte)(30)))));
            this.bsprContext_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.bsprContext_Sheet1.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.bsprContext_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // bbtnListContext
            // 
            this.bbtnListContext.BssClass = "";
            this.bbtnListContext.ButtonHorizontalSpacing = 1;
            this.bbtnListContext.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnListContext.Dock = System.Windows.Forms.DockStyle.Top;
            this.bbtnListContext.HorizontalMarginSpacing = 5;
            this.bbtnListContext.IsCondition = false;
            this.bbtnListContext.Location = new System.Drawing.Point(3, 14);
            this.bbtnListContext.MarginSpace = 5;
            this.bbtnListContext.Name = "bbtnListContext";
            this.bbtnListContext.Size = new System.Drawing.Size(879, 28);
            this.bbtnListContext.TabIndex = 3;
            this.bbtnListContext.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(this.bbtnListContext_ButtonClick);
            // 
            // gbModel
            // 
            this.gbModel.Controls.Add(this.tlpModel5);
            this.gbModel.Controls.Add(this.tlpModel4);
            this.gbModel.Controls.Add(this.tlpModel2);
            this.gbModel.Controls.Add(this.tlpModel3);
            this.gbModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbModel.Location = new System.Drawing.Point(0, 58);
            this.gbModel.Name = "gbModel";
            this.gbModel.Size = new System.Drawing.Size(885, 119);
            this.gbModel.TabIndex = 2;
            this.gbModel.TabStop = false;
            this.gbModel.Text = "Model Selection";
            // 
            // tlpModel5
            // 
            this.tlpModel5.ColumnCount = 2;
            this.tlpModel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 268F));
            this.tlpModel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModel5.Controls.Add(this.bchkValSameModule, 0, 0);
            this.tlpModel5.Controls.Add(this.bchkInheritYN, 0, 0);
            this.tlpModel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpModel5.Location = new System.Drawing.Point(3, 89);
            this.tlpModel5.Name = "tlpModel5";
            this.tlpModel5.RowCount = 1;
            this.tlpModel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpModel5.Size = new System.Drawing.Size(879, 24);
            this.tlpModel5.TabIndex = 11;
            // 
            // bchkValSameModule
            // 
            this.bchkValSameModule.BssClass = "";
            this.bchkValSameModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkValSameModule.IsMultiLanguage = true;
            this.bchkValSameModule.Key = "";
            this.bchkValSameModule.Location = new System.Drawing.Point(271, 3);
            this.bchkValSameModule.Name = "bchkValSameModule";
            this.bchkValSameModule.Size = new System.Drawing.Size(605, 19);
            this.bchkValSameModule.TabIndex = 3;
            this.bchkValSameModule.Text = "Validate Value for Same Module";
            this.bchkValSameModule.UseVisualStyleBackColor = true;
            // 
            // bchkInheritYN
            // 
            this.bchkInheritYN.BssClass = "";
            this.bchkInheritYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkInheritYN.IsMultiLanguage = true;
            this.bchkInheritYN.Key = "";
            this.bchkInheritYN.Location = new System.Drawing.Point(3, 3);
            this.bchkInheritYN.Name = "bchkInheritYN";
            this.bchkInheritYN.Size = new System.Drawing.Size(262, 19);
            this.bchkInheritYN.TabIndex = 3;
            this.bchkInheritYN.Text = "Inherit the SPEC/RULE of Main Config";
            this.bchkInheritYN.UseVisualStyleBackColor = true;
            // 
            // tlpModel4
            // 
            this.tlpModel4.ColumnCount = 3;
            this.tlpModel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpModel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpModel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpModel4.Controls.Add(this.bchkCreateSubAutoCalc, 0, 0);
            this.tlpModel4.Controls.Add(this.bchkCreateSubInterlock, 0, 0);
            this.tlpModel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpModel4.Location = new System.Drawing.Point(3, 65);
            this.tlpModel4.Name = "tlpModel4";
            this.tlpModel4.RowCount = 2;
            this.tlpModel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpModel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpModel4.Size = new System.Drawing.Size(879, 24);
            this.tlpModel4.TabIndex = 10;
            // 
            // bchkCreateSubAutoCalc
            // 
            this.bchkCreateSubAutoCalc.BssClass = "";
            this.bchkCreateSubAutoCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkCreateSubAutoCalc.IsMultiLanguage = true;
            this.bchkCreateSubAutoCalc.Key = "";
            this.bchkCreateSubAutoCalc.Location = new System.Drawing.Point(310, 3);
            this.bchkCreateSubAutoCalc.Name = "bchkCreateSubAutoCalc";
            this.bchkCreateSubAutoCalc.Size = new System.Drawing.Size(345, 19);
            this.bchkCreateSubAutoCalc.TabIndex = 3;
            this.bchkCreateSubAutoCalc.Text = "Generate New Sub Charts with Auto Calculation(Y)";
            this.bchkCreateSubAutoCalc.UseVisualStyleBackColor = true;
            // 
            // bchkCreateSubInterlock
            // 
            this.bchkCreateSubInterlock.BssClass = "";
            this.bchkCreateSubInterlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkCreateSubInterlock.IsMultiLanguage = true;
            this.bchkCreateSubInterlock.Key = "";
            this.bchkCreateSubInterlock.Location = new System.Drawing.Point(3, 3);
            this.bchkCreateSubInterlock.Name = "bchkCreateSubInterlock";
            this.bchkCreateSubInterlock.Size = new System.Drawing.Size(301, 19);
            this.bchkCreateSubInterlock.TabIndex = 2;
            this.bchkCreateSubInterlock.Text = "Generate New Sub Charts with Interlock(Y)";
            this.bchkCreateSubInterlock.UseVisualStyleBackColor = true;
            // 
            // tlpModel2
            // 
            this.tlpModel2.ColumnCount = 5;
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpModel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpModel2.Controls.Add(this.bcboAutoType, 1, 0);
            this.tlpModel2.Controls.Add(this.blblAutoSettings, 0, 0);
            this.tlpModel2.Controls.Add(this.blbMode, 2, 0);
            this.tlpModel2.Controls.Add(this.bcboMode, 3, 0);
            this.tlpModel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpModel2.Location = new System.Drawing.Point(3, 41);
            this.tlpModel2.Name = "tlpModel2";
            this.tlpModel2.RowCount = 1;
            this.tlpModel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpModel2.Size = new System.Drawing.Size(879, 24);
            this.tlpModel2.TabIndex = 9;
            // 
            // bcboAutoType
            // 
            this.bcboAutoType.AutoDropDownWidth = true;
            this.bcboAutoType.BssClass = "";
            this.bcboAutoType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcboAutoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboAutoType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboAutoType.FormattingEnabled = true;
            this.bcboAutoType.IsMultiLanguage = true;
            this.bcboAutoType.Key = "";
            this.bcboAutoType.Location = new System.Drawing.Point(103, 3);
            this.bcboAutoType.Name = "bcboAutoType";
            this.bcboAutoType.Size = new System.Drawing.Size(144, 21);
            this.bcboAutoType.TabIndex = 5;
            // 
            // blblAutoSettings
            // 
            this.blblAutoSettings.BssClass = "";
            this.blblAutoSettings.CustomTextAlign = "";
            this.blblAutoSettings.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblAutoSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.blblAutoSettings.IsMultiLanguage = true;
            this.blblAutoSettings.Key = "";
            this.blblAutoSettings.Location = new System.Drawing.Point(3, 0);
            this.blblAutoSettings.Name = "blblAutoSettings";
            this.blblAutoSettings.Size = new System.Drawing.Size(93, 24);
            this.blblAutoSettings.TabIndex = 4;
            this.blblAutoSettings.Text = "Auto Settings";
            this.blblAutoSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blbMode
            // 
            this.blbMode.BssClass = "";
            this.blbMode.CustomTextAlign = "";
            this.blbMode.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blbMode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.blbMode.IsMultiLanguage = true;
            this.blbMode.Key = "";
            this.blbMode.Location = new System.Drawing.Point(253, 0);
            this.blbMode.Name = "blbMode";
            this.blbMode.Size = new System.Drawing.Size(52, 24);
            this.blbMode.TabIndex = 6;
            this.blbMode.Text = "Mode";
            this.blbMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcboMode
            // 
            this.bcboMode.AutoDropDownWidth = true;
            this.bcboMode.BssClass = "";
            this.bcboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboMode.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboMode.FormattingEnabled = true;
            this.bcboMode.IsMultiLanguage = true;
            this.bcboMode.Key = "";
            this.bcboMode.Location = new System.Drawing.Point(312, 3);
            this.bcboMode.Name = "bcboMode";
            this.bcboMode.Size = new System.Drawing.Size(143, 21);
            this.bcboMode.TabIndex = 7;
            // 
            // tlpModel3
            // 
            this.tlpModel3.ColumnCount = 7;
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tlpModel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModel3.Controls.Add(this.btxtSampleCount, 6, 0);
            this.tlpModel3.Controls.Add(this.blblSampleCount, 5, 0);
            this.tlpModel3.Controls.Add(this.bchkMainYN, 0, 0);
            this.tlpModel3.Controls.Add(this.bchkInterlockYN, 1, 0);
            this.tlpModel3.Controls.Add(this.bchkAutoCalcYN, 2, 0);
            this.tlpModel3.Controls.Add(this.bchkActive, 4, 0);
            this.tlpModel3.Controls.Add(this.bchkAutoSubYN, 3, 0);
            this.tlpModel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpModel3.Location = new System.Drawing.Point(3, 17);
            this.tlpModel3.Name = "tlpModel3";
            this.tlpModel3.RowCount = 1;
            this.tlpModel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpModel3.Size = new System.Drawing.Size(879, 24);
            this.tlpModel3.TabIndex = 8;
            // 
            // btxtSampleCount
            // 
            this.btxtSampleCount.BssClass = "";
            this.btxtSampleCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtSampleCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtSampleCount.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.btxtSampleCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.Number;
            this.btxtSampleCount.IsMultiLanguage = true;
            this.btxtSampleCount.Key = "";
            this.btxtSampleCount.Location = new System.Drawing.Point(748, 3);
            this.btxtSampleCount.Name = "btxtSampleCount";
            this.btxtSampleCount.Size = new System.Drawing.Size(128, 21);
            this.btxtSampleCount.TabIndex = 8;
            // 
            // blblSampleCount
            // 
            this.blblSampleCount.BssClass = "";
            this.blblSampleCount.CustomTextAlign = "";
            this.blblSampleCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblSampleCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblSampleCount.IsMultiLanguage = true;
            this.blblSampleCount.Key = "";
            this.blblSampleCount.Location = new System.Drawing.Point(637, 0);
            this.blblSampleCount.Name = "blblSampleCount";
            this.blblSampleCount.Size = new System.Drawing.Size(105, 25);
            this.blblSampleCount.TabIndex = 7;
            this.blblSampleCount.Text = "Sample Count";
            this.blblSampleCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bchkMainYN
            // 
            this.bchkMainYN.BssClass = "";
            this.bchkMainYN.Dock = System.Windows.Forms.DockStyle.Left;
            this.bchkMainYN.IsMultiLanguage = true;
            this.bchkMainYN.Key = "";
            this.bchkMainYN.Location = new System.Drawing.Point(3, 3);
            this.bchkMainYN.Name = "bchkMainYN";
            this.bchkMainYN.Size = new System.Drawing.Size(83, 19);
            this.bchkMainYN.TabIndex = 0;
            this.bchkMainYN.Text = "Main";
            this.bchkMainYN.UseVisualStyleBackColor = true;
            // 
            // bchkInterlockYN
            // 
            this.bchkInterlockYN.BssClass = "";
            this.bchkInterlockYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkInterlockYN.IsMultiLanguage = true;
            this.bchkInterlockYN.Key = "";
            this.bchkInterlockYN.Location = new System.Drawing.Point(93, 3);
            this.bchkInterlockYN.Name = "bchkInterlockYN";
            this.bchkInterlockYN.Size = new System.Drawing.Size(84, 19);
            this.bchkInterlockYN.TabIndex = 2;
            this.bchkInterlockYN.Text = "Interlock";
            this.bchkInterlockYN.UseVisualStyleBackColor = true;
            // 
            // bchkAutoCalcYN
            // 
            this.bchkAutoCalcYN.BssClass = "";
            this.bchkAutoCalcYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkAutoCalcYN.IsMultiLanguage = true;
            this.bchkAutoCalcYN.Key = "";
            this.bchkAutoCalcYN.Location = new System.Drawing.Point(183, 3);
            this.bchkAutoCalcYN.Name = "bchkAutoCalcYN";
            this.bchkAutoCalcYN.Size = new System.Drawing.Size(146, 19);
            this.bchkAutoCalcYN.TabIndex = 3;
            this.bchkAutoCalcYN.Text = "Auto Calculation";
            this.bchkAutoCalcYN.UseVisualStyleBackColor = true;
            // 
            // bchkActive
            // 
            this.bchkActive.BssClass = "";
            this.bchkActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkActive.IsMultiLanguage = false;
            this.bchkActive.Key = "";
            this.bchkActive.Location = new System.Drawing.Point(485, 3);
            this.bchkActive.Name = "bchkActive";
            this.bchkActive.Size = new System.Drawing.Size(146, 19);
            this.bchkActive.TabIndex = 6;
            this.bchkActive.Text = "Active";
            this.bchkActive.UseVisualStyleBackColor = true;
            // 
            // bchkAutoSubYN
            // 
            this.bchkAutoSubYN.BssClass = "";
            this.bchkAutoSubYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkAutoSubYN.IsMultiLanguage = true;
            this.bchkAutoSubYN.Key = "";
            this.bchkAutoSubYN.Location = new System.Drawing.Point(335, 3);
            this.bchkAutoSubYN.Name = "bchkAutoSubYN";
            this.bchkAutoSubYN.Size = new System.Drawing.Size(144, 19);
            this.bchkAutoSubYN.TabIndex = 1;
            this.bchkAutoSubYN.Text = "Auto Generate Sub Charts";
            this.bchkAutoSubYN.UseVisualStyleBackColor = true;
            // 
            // gbParam
            // 
            this.gbParam.Controls.Add(this.bLine1);
            this.gbParam.Controls.Add(this.panel4);
            this.gbParam.Controls.Add(this.tlpParam);
            this.gbParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbParam.Location = new System.Drawing.Point(0, 0);
            this.gbParam.Name = "gbParam";
            this.gbParam.Size = new System.Drawing.Size(885, 58);
            this.gbParam.TabIndex = 1;
            this.gbParam.TabStop = false;
            this.gbParam.Text = "Attributes Selection";
            // 
            // bLine1
            // 
            this.bLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bLine1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.bLine1.Location = new System.Drawing.Point(3, 50);
            this.bLine1.Name = "bLine1";
            this.bLine1.Size = new System.Drawing.Size(879, 2);
            this.bLine1.TabIndex = 0;
            this.bLine1.Text = "bLine1";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 47);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(879, 3);
            this.panel4.TabIndex = 15;
            // 
            // tlpParam
            // 
            this.tlpParam.ColumnCount = 4;
            this.tlpParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tlpParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.tlpParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlpParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpParam.Controls.Add(this.blblParamAlias, 0, 0);
            this.tlpParam.Controls.Add(this.panel2, 1, 0);
            this.tlpParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpParam.Location = new System.Drawing.Point(3, 17);
            this.tlpParam.Name = "tlpParam";
            this.tlpParam.RowCount = 1;
            this.tlpParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpParam.Size = new System.Drawing.Size(879, 30);
            this.tlpParam.TabIndex = 11;
            // 
            // blblParamAlias
            // 
            this.blblParamAlias.BssClass = "";
            this.blblParamAlias.CustomTextAlign = "";
            this.blblParamAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblParamAlias.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.blblParamAlias.IsMultiLanguage = true;
            this.blblParamAlias.Key = "";
            this.blblParamAlias.Location = new System.Drawing.Point(3, 0);
            this.blblParamAlias.Name = "blblParamAlias";
            this.blblParamAlias.Size = new System.Drawing.Size(184, 30);
            this.blblParamAlias.TabIndex = 0;
            this.blblParamAlias.Text = "Attributes Name";
            this.blblParamAlias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.tlpParam.SetColumnSpan(this.panel2, 3);
            this.panel2.Controls.Add(this.pnlParamAlias);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(190, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(689, 30);
            this.panel2.TabIndex = 15;
            // 
            // pnlParamAlias
            // 
            this.pnlParamAlias.Controls.Add(this.btxtParamAlias);
            this.pnlParamAlias.Controls.Add(this.bbtnParamAlias);
            this.pnlParamAlias.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParamAlias.Location = new System.Drawing.Point(3, 3);
            this.pnlParamAlias.Name = "pnlParamAlias";
            this.pnlParamAlias.Size = new System.Drawing.Size(683, 19);
            this.pnlParamAlias.TabIndex = 11;
            // 
            // btxtParamAlias
            // 
            this.btxtParamAlias.BssClass = "";
            this.btxtParamAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtParamAlias.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtParamAlias.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtParamAlias.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtParamAlias.IsMultiLanguage = true;
            this.btxtParamAlias.Key = "";
            this.btxtParamAlias.Location = new System.Drawing.Point(0, 0);
            this.btxtParamAlias.Name = "btxtParamAlias";
            this.btxtParamAlias.Size = new System.Drawing.Size(655, 21);
            this.btxtParamAlias.TabIndex = 2;
            // 
            // bbtnParamAlias
            // 
            this.bbtnParamAlias.AutoButtonSize = false;
            this.bbtnParamAlias.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnParamAlias.BackgroundImage")));
            this.bbtnParamAlias.BssClass = "";
            this.bbtnParamAlias.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnParamAlias.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnParamAlias.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnParamAlias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnParamAlias.IsMultiLanguage = true;
            this.bbtnParamAlias.Key = "";
            this.bbtnParamAlias.Location = new System.Drawing.Point(655, 0);
            this.bbtnParamAlias.Name = "bbtnParamAlias";
            this.bbtnParamAlias.Size = new System.Drawing.Size(28, 19);
            this.bbtnParamAlias.TabIndex = 4;
            this.bbtnParamAlias.ToolTipText = "";
            this.bbtnParamAlias.UseVisualStyleBackColor = true;
            this.bbtnParamAlias.Click += new System.EventHandler(this.bbtnParamAlias_Click);
            // 
            // ContextConfigurationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlSPCBase);
            this.Name = "ContextConfigurationUC";
            this.Size = new System.Drawing.Size(885, 458);
            this.pnlSPCBase.ResumeLayout(false);
            this.gbContext.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprContext_Sheet1)).EndInit();
            this.gbModel.ResumeLayout(false);
            this.tlpModel5.ResumeLayout(false);
            this.tlpModel4.ResumeLayout(false);
            this.tlpModel2.ResumeLayout(false);
            this.tlpModel3.ResumeLayout(false);
            this.tlpModel3.PerformLayout();
            this.gbParam.ResumeLayout(false);
            this.tlpParam.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlParamAlias.ResumeLayout(false);
            this.pnlParamAlias.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.Panel pnlSPCBase;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprContext;
        private FarPoint.Win.Spread.SheetView bsprContext_Sheet1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnListContext;
        private System.Windows.Forms.GroupBox gbContext;
        private System.Windows.Forms.GroupBox gbParam;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnParamAlias;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtParamAlias;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblParamAlias;
        private System.Windows.Forms.TableLayoutPanel tlpParam;
        private System.Windows.Forms.Panel pnlParamAlias;
        private System.Windows.Forms.Panel panel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BLine bLine1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox gbModel;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblAutoSettings;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboAutoType;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkMainYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkAutoCalcYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkAutoSubYN;
        private System.Windows.Forms.TableLayoutPanel tlpModel3;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkInterlockYN;
        private System.Windows.Forms.TableLayoutPanel tlpModel2;
        private System.Windows.Forms.TableLayoutPanel tlpModel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkInheritYN;
        private System.Windows.Forms.TableLayoutPanel tlpModel5;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkCreateSubAutoCalc;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blbMode;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboMode;
        private PeakPerformance.Client.BISTelControl.BCheckBox bchkValSameModule;
        private PeakPerformance.Client.BISTelControl.BCheckBox bchkCreateSubInterlock;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtSampleCount;
        private PeakPerformance.Client.BISTelControl.BLabel blblSampleCount;
        private PeakPerformance.Client.BISTelControl.BCheckBox bchkActive;
    }
}
