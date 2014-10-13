namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class SPCConfigurationUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCConfigurationUC));
            this.pnlSPCBase = new System.Windows.Forms.Panel();
            this.bTabControl = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.contextConfigurationUC = new BISTel.eSPC.Page.ATT.Modeling.ContextConfigurationUC();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ruleConfigurationUC = new BISTel.eSPC.Page.ATT.Modeling.RuleConfigurationUC();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.optionalConfigurationUC = new BISTel.eSPC.Page.ATT.Modeling.OptionalConfigurationUC();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            // [SPC-658]
            this.autoCalcConfigurationUC = new BISTel.eSPC.Page.ATT.Modeling.AutoCalcConfigurationUC(this._isDefaultModel);
            this.gbSPCModelName = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.blblSPCModelName = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtSPCModelName = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.btxtSPCModelDesc = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblSPCModelDesc = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.blblChartDesc = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtChartDesc = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblSPCGroup = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bComboGroupList = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.pnlSPCBase.SuspendLayout();
            this.bTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.gbSPCModelName.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSPCBase
            // 
            this.pnlSPCBase.Controls.Add(this.bTabControl);
            this.pnlSPCBase.Controls.Add(this.gbSPCModelName);
            this.pnlSPCBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSPCBase.Location = new System.Drawing.Point(0, 0);
            this.pnlSPCBase.Name = "pnlSPCBase";
            this.pnlSPCBase.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.TabIndex = 4;
            // 
            // bTabControl
            // 
            this.bTabControl.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(161)))), ((int)(((byte)(184)))));
            this.bTabControl.BssClass = "DataMainTab";
            this.bTabControl.CloseImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.CloseImage")));
            this.bTabControl.CloseRollOverImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.CloseRollOverImage")));
            this.bTabControl.Controls.Add(this.tabPage1);
            this.bTabControl.Controls.Add(this.tabPage2);
            this.bTabControl.Controls.Add(this.tabPage5);
            this.bTabControl.Controls.Add(this.tabPage6);
            this.bTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bTabControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTabControl.FontSizeSet = "11";
            this.bTabControl.IsDrawBorder = true;
            this.bTabControl.IsFillBorder = false;
            this.bTabControl.IsReleaseAlignmentRestrict = false;
            this.bTabControl.IsSpace = true;
            this.bTabControl.Key = "";
            this.bTabControl.Location = new System.Drawing.Point(0, 72);
            this.bTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.bTabControl.Name = "bTabControl";
            this.bTabControl.SelectedIndex = 0;
            this.bTabControl.Size = new System.Drawing.Size(759, 423);
            this.bTabControl.TabBackColor = System.Drawing.Color.White;
            this.bTabControl.TabBackImage = null;
            this.bTabControl.TabIndex = 0;
            this.bTabControl.TabPageClose = false;
            this.bTabControl.TabSelectFontColor = System.Drawing.Color.White;
            this.bTabControl.TabSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabSelectLeftImage")));
            this.bTabControl.TabSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabSelectMiddleImage")));
            this.bTabControl.TabSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabSelectRightImage")));
            this.bTabControl.TabSelectRollOverFontColor = System.Drawing.Color.Black;
            this.bTabControl.TabUnSelectFontColor = System.Drawing.Color.Black;
            this.bTabControl.TabUnSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabUnSelectLeftImage")));
            this.bTabControl.TabUnSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabUnSelectMiddleImage")));
            this.bTabControl.TabUnSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl.TabUnSelectRightImage")));
            this.bTabControl.TabUnSelectRollOverFontColor = System.Drawing.Color.White;
            this.bTabControl.TextLeftPosition = 5;
            this.bTabControl.UseShadowFont = false;
            this.bTabControl.SelectedIndexChanged += new System.EventHandler(this.bTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.contextConfigurationUC);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(751, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Context      ";
            // 
            // contextConfigurationUC
            // 
            this.contextConfigurationUC.ACTIVATION_YN = "N";
            this.contextConfigurationUC.ApplicationName = "";
            this.contextConfigurationUC.AREA_RAWID = null;
            this.contextConfigurationUC.AUTO_SUB_YN = "N";
            this.contextConfigurationUC.AUTO_TYPE_CD = "";
            this.contextConfigurationUC.AUTOCALC_YN = "N";
            this.contextConfigurationUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.contextConfigurationUC.BackColorHtml = "#DCDCDC";
            this.contextConfigurationUC.BssClass = null;
            this.contextConfigurationUC.CHART_MODE_CD = "";
            //this.contextConfigurationUC.COMPLEX_YN = "N";
            //this.contextConfigurationUC.CONFIG_DATASET = null;
            this.contextConfigurationUC.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_MAIN;
            this.contextConfigurationUC.CONFIG_RAWID = null;
            this.contextConfigurationUC.CONTEXT_DATASET = null;
            this.contextConfigurationUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contextConfigurationUC.DonotCloseProgress = false;
            this.contextConfigurationUC.EQP_MODEL = null;
            this.contextConfigurationUC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.contextConfigurationUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.contextConfigurationUC.ForeColorHtml = "#000000";
            this.contextConfigurationUC.FunctionName = "";
            this.contextConfigurationUC.INTERLOCK_YN = "N";
            this.contextConfigurationUC.LINE_RAWID = null;
            //this.contextConfigurationUC.llstContextType = ((BISTel.PeakPerformance.Client.CommonLibrary.LinkedList)(resources.GetObject("contextConfigurationUC.llstContextType")));
            this.contextConfigurationUC.Location = new System.Drawing.Point(3, 3);
            this.contextConfigurationUC.MAIN_YN = "N";
            //this.contextConfigurationUC.MANAGE_TYPE_CD = "";
            this.contextConfigurationUC.Name = "contextConfigurationUC";
            this.contextConfigurationUC.PARAM_ALIAS = "";
            //this.contextConfigurationUC.PARAM_LIST = "";
           // this.contextConfigurationUC.PARAM_TYPE_CD = "";
            //this.contextConfigurationUC.REF_PARAM = "";
           // this.contextConfigurationUC.REF_PARAM_LIST = "";
            this.contextConfigurationUC.SAMPLE_COUNT = "";
            this.contextConfigurationUC.sessionData = null;
            this.contextConfigurationUC.Size = new System.Drawing.Size(745, 387);
           // this.contextConfigurationUC.SPC_DATA_LEVEL = "";
            this.contextConfigurationUC.TabIndex = 0;
            this.contextConfigurationUC.Title = null;
            this.contextConfigurationUC.URL = "";
            //this.contextConfigurationUC.USE_EXTERNAL_SPEC_YN = "N";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.ruleConfigurationUC);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(751, 419);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rule      ";
            // 
            // ruleConfigurationUC
            // 
            this.ruleConfigurationUC.ApplicationName = "";
            this.ruleConfigurationUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ruleConfigurationUC.BackColorHtml = "#DCDCDC";
            this.ruleConfigurationUC.BssClass = null;
            this.ruleConfigurationUC.CENTER_EWMAMEAN = "";
            this.ruleConfigurationUC.CENTER_LINE = "";
            this.ruleConfigurationUC.CENTER_RANGE = "";
            this.ruleConfigurationUC.CENTER_RAW = "";
            this.ruleConfigurationUC.CENTER_STD = "";
            this.ruleConfigurationUC.CONFIG_DATASET = null;
            this.ruleConfigurationUC.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_MAIN;
            this.ruleConfigurationUC.CONFIG_RAWID = null;
            this.ruleConfigurationUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleConfigurationUC.DonotCloseProgress = false;
            this.ruleConfigurationUC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ruleConfigurationUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ruleConfigurationUC.ForeColorHtml = "#000000";
            this.ruleConfigurationUC.FunctionName = "";
            this.ruleConfigurationUC.LCL_EWMAMEAN = "";
            this.ruleConfigurationUC.LCL_RANGE = "";
            this.ruleConfigurationUC.LCL_RAW = "";
            this.ruleConfigurationUC.LCL_STD = "";
            this.ruleConfigurationUC.Location = new System.Drawing.Point(3, 3);
            this.ruleConfigurationUC.LOWER_CONTROL = "";
            this.ruleConfigurationUC.LOWER_SPEC = "";
            this.ruleConfigurationUC.Name = "ruleConfigurationUC";
            this.ruleConfigurationUC.RULE_DATASET = null;
            this.ruleConfigurationUC.RULE_OPT_DATASET = null;
            this.ruleConfigurationUC.sessionData = null;
            this.ruleConfigurationUC.Size = new System.Drawing.Size(745, 414);
            this.ruleConfigurationUC.TabIndex = 0;
            this.ruleConfigurationUC.TARGET = "";
            this.ruleConfigurationUC.Title = null;
            
            this.ruleConfigurationUC.UCL_RANGE = "";
            this.ruleConfigurationUC.UCL_RAW = "";
            this.ruleConfigurationUC.UCL_STD = "";
            this.ruleConfigurationUC.UPPER_CONTROL = "";
            this.ruleConfigurationUC.UPPER_SPEC = "";
            this.ruleConfigurationUC.URL = "";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Controls.Add(this.optionalConfigurationUC);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(751, 419);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Option      ";
            // 
            // optionalConfigurationUC
            // 
            this.optionalConfigurationUC.ApplicationName = "";
            this.optionalConfigurationUC.AUTO_CPK_YN = "N";
            this.optionalConfigurationUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.optionalConfigurationUC.BackColorHtml = "#DCDCDC";
            this.optionalConfigurationUC.BssClass = null;
            this.optionalConfigurationUC.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_MAIN;
            this.optionalConfigurationUC.CONFIG_OPT_DATASET = null;
            this.optionalConfigurationUC.CONFIG_RAWID = null;
            this.optionalConfigurationUC.DAYS = "";
            this.optionalConfigurationUC.DEFAULT_CHART_LIST = null;
            this.optionalConfigurationUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionalConfigurationUC.DonotCloseProgress = false;
            this.optionalConfigurationUC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.optionalConfigurationUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.optionalConfigurationUC.ForeColorHtml = "#000000";
            this.optionalConfigurationUC.FunctionName = "";
            this.optionalConfigurationUC.Location = new System.Drawing.Point(3, 3);
            this.optionalConfigurationUC.Name = "optionalConfigurationUC";
            this.optionalConfigurationUC.SAMPLE_COUNT = "";
            this.optionalConfigurationUC.sessionData = null;
            this.optionalConfigurationUC.Size = new System.Drawing.Size(745, 415);
            this.optionalConfigurationUC.SPC_PARAM_CATEGORY_CD = "";
            this.optionalConfigurationUC.SPC_PRIORITY_CD = "";
            this.optionalConfigurationUC.TabIndex = 0;
            this.optionalConfigurationUC.Title = null;
            this.optionalConfigurationUC.URL = "";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.White;
            this.tabPage6.Controls.Add(this.autoCalcConfigurationUC);
            this.tabPage6.Location = new System.Drawing.Point(4, 26);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(751, 419);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Auto Calculation      ";
            // 
            // autoCalcConfigurationUC
            // 
            this.autoCalcConfigurationUC.ApplicationName = "";
            this.autoCalcConfigurationUC.AUTOCALC_DATASET = null;
            this.autoCalcConfigurationUC.AUTOCALC_PERIOD = "";
            this.autoCalcConfigurationUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.autoCalcConfigurationUC.BackColorHtml = "#DCDCDC";
            this.autoCalcConfigurationUC.BssClass = null;
            this.autoCalcConfigurationUC.CALC_COUNT = "";
            this.autoCalcConfigurationUC.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_MAIN;
            this.autoCalcConfigurationUC.CONFIG_RAWID = null;
            this.autoCalcConfigurationUC.CONTROL_THRESHOLD = "";
            this.autoCalcConfigurationUC.DEFAULT_PERIOD = "";
            this.autoCalcConfigurationUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoCalcConfigurationUC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.autoCalcConfigurationUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.autoCalcConfigurationUC.ForeColorHtml = "#000000";
            this.autoCalcConfigurationUC.FunctionName = "";
            this.autoCalcConfigurationUC.Location = new System.Drawing.Point(0, 0);
            this.autoCalcConfigurationUC.MAX_PERIOD = "";
            this.autoCalcConfigurationUC.MIN_SAMPLES = "";
            this.autoCalcConfigurationUC.Name = "autoCalcConfigurationUC";
            this.autoCalcConfigurationUC.sessionData = null;
            this.autoCalcConfigurationUC.Size = new System.Drawing.Size(751, 421);
            this.autoCalcConfigurationUC.TabIndex = 0;
            this.autoCalcConfigurationUC.Title = null;
            this.autoCalcConfigurationUC.URL = "";
            // 
            // gbSPCModelName
            // 
            this.gbSPCModelName.Controls.Add(this.tableLayoutPanel1);
            this.gbSPCModelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSPCModelName.Location = new System.Drawing.Point(0, 0);
            this.gbSPCModelName.Name = "gbSPCModelName";
            this.gbSPCModelName.Size = new System.Drawing.Size(759, 72);
            this.gbSPCModelName.TabIndex = 1;
            this.gbSPCModelName.TabStop = false;
            this.gbSPCModelName.Text = "SPC Model";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 343F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btxtChartDesc, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.blblChartDesc, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.blblSPCModelName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btxtSPCModelName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btxtSPCModelDesc, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.blblSPCModelDesc, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.blblSPCGroup, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.bComboGroupList, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(753, 53);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // blblSPCModelName
            // 
            this.blblSPCModelName.BssClass = "";
            this.blblSPCModelName.CustomTextAlign = "";
            this.blblSPCModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblSPCModelName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblSPCModelName.IsMultiLanguage = true;
            this.blblSPCModelName.Key = "";
            this.blblSPCModelName.Location = new System.Drawing.Point(3, 0);
            this.blblSPCModelName.Name = "blblSPCModelName";
            this.blblSPCModelName.Size = new System.Drawing.Size(94, 26);
            this.blblSPCModelName.TabIndex = 0;
            this.blblSPCModelName.Text = "Model Name";
            this.blblSPCModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtSPCModelName
            // 
            this.btxtSPCModelName.BssClass = "";
            this.btxtSPCModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtSPCModelName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtSPCModelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtSPCModelName.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtSPCModelName.IsMultiLanguage = true;
            this.btxtSPCModelName.Key = "";
            this.btxtSPCModelName.Location = new System.Drawing.Point(103, 3);
            this.btxtSPCModelName.Name = "btxtSPCModelName";
            this.btxtSPCModelName.Size = new System.Drawing.Size(337, 21);
            this.btxtSPCModelName.TabIndex = 1;
            // 
            // btxtSPCModelDesc
            // 
            this.btxtSPCModelDesc.BssClass = "";
            this.btxtSPCModelDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtSPCModelDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtSPCModelDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtSPCModelDesc.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtSPCModelDesc.IsMultiLanguage = true;
            this.btxtSPCModelDesc.Key = "";
            this.btxtSPCModelDesc.Location = new System.Drawing.Point(521, 3);
            this.btxtSPCModelDesc.Name = "btxtSPCModelDesc";
            this.btxtSPCModelDesc.Size = new System.Drawing.Size(229, 21);
            this.btxtSPCModelDesc.TabIndex = 2;
            // 
            // blblSPCModelDesc
            // 
            this.blblSPCModelDesc.BssClass = "";
            this.blblSPCModelDesc.CustomTextAlign = "";
            this.blblSPCModelDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblSPCModelDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblSPCModelDesc.IsMultiLanguage = true;
            this.blblSPCModelDesc.Key = "";
            this.blblSPCModelDesc.Location = new System.Drawing.Point(446, 0);
            this.blblSPCModelDesc.Name = "blblSPCModelDesc";
            this.blblSPCModelDesc.Size = new System.Drawing.Size(69, 26);
            this.blblSPCModelDesc.TabIndex = 3;
            this.blblSPCModelDesc.Text = "Description";
            this.blblSPCModelDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(751, 466);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage1";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(751, 466);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage2";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // blblChartDesc
            // 
            this.blblChartDesc.BssClass = "";
            this.blblChartDesc.CustomTextAlign = "";
            this.blblChartDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblChartDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblChartDesc.IsMultiLanguage = false;
            this.blblChartDesc.Key = "";
            this.blblChartDesc.Location = new System.Drawing.Point(3, 26);
            this.blblChartDesc.Name = "blblChartDesc";
            this.blblChartDesc.Size = new System.Drawing.Size(94, 27);
            this.blblChartDesc.TabIndex = 4;
            this.blblChartDesc.Text = "Chart Description";
            this.blblChartDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtChartDesc
            // 
            this.btxtChartDesc.BssClass = "";
            this.btxtChartDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtChartDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtChartDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtChartDesc.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtChartDesc.IsMultiLanguage = true;
            this.btxtChartDesc.Key = "";
            this.btxtChartDesc.Location = new System.Drawing.Point(103, 29);
            this.btxtChartDesc.Name = "btxtChartDesc";
            this.btxtChartDesc.Size = new System.Drawing.Size(337, 21);
            this.btxtChartDesc.TabIndex = 5;
            // 
            // blblSPCGroup
            // 
            this.blblSPCGroup.BssClass = "";
            this.blblSPCGroup.CustomTextAlign = "";
            this.blblSPCGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblSPCGroup.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblSPCGroup.IsMultiLanguage = true;
            this.blblSPCGroup.Key = "";
            this.blblSPCGroup.Location = new System.Drawing.Point(520, 23);
            this.blblSPCGroup.Name = "blblSPCGroup";
            this.blblSPCGroup.Size = new System.Drawing.Size(81, 23);
            this.blblSPCGroup.TabIndex = 6;
            this.blblSPCGroup.Text = "Group";
            this.blblSPCGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bComboGroupList
            // 
            this.bComboGroupList.AutoDropDownWidth = true;
            this.bComboGroupList.BssClass = "";
            this.bComboGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bComboGroupList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bComboGroupList.FormattingEnabled = true;
            this.bComboGroupList.IsMultiLanguage = true;
            this.bComboGroupList.Key = "";
            this.bComboGroupList.Location = new System.Drawing.Point(607, 26);
            this.bComboGroupList.Name = "bComboGroupList";
            this.bComboGroupList.Size = new System.Drawing.Size(269, 20);
            this.bComboGroupList.TabIndex = 7;
            // 
            // SPCConfigurationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlSPCBase);
            this.Name = "SPCConfigurationUC";
            this.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.ResumeLayout(false);
            this.bTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.gbSPCModelName.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.Panel pnlSPCBase;
        private BISTel.PeakPerformance.Client.BISTelControl.BTabControl bTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private ContextConfigurationUC contextConfigurationUC;
        private RuleConfigurationUC ruleConfigurationUC;
        private OptionalConfigurationUC optionalConfigurationUC;
        private System.Windows.Forms.GroupBox gbSPCModelName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblSPCModelName;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtSPCModelName;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtSPCModelDesc;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblSPCModelDesc;
        private System.Windows.Forms.TabPage tabPage6;
        private AutoCalcConfigurationUC autoCalcConfigurationUC;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtChartDesc;
        private PeakPerformance.Client.BISTelControl.BLabel blblChartDesc;

        private PeakPerformance.Client.BISTelControl.BLabel blblSPCGroup;
        private PeakPerformance.Client.BISTelControl.BComboBox bComboGroupList;
    }
}
