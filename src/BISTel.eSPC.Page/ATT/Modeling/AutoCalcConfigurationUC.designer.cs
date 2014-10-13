namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class AutoCalcConfigurationUC
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
            this.pnlSPCBase = new System.Windows.Forms.Panel();
            this.gbAutoCalcSetting = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btxtUCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.btxtCCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.btxtPCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.btxtPNCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bchkCCalcYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkPNCalcYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkPCalcYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkUCalcYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.gbAutoCalcSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bLabel3 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bchkAutoCalcIQR = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bchkAutoCalcShift = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.blblAutoCalcPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel4 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtAutoCalcPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.btxtAutoCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel2 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblCalcCntDesc = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bchkThresholdOffYN = new BISTel.PeakPerformance.Client.BISTelControl.BCheckBox();
            this.bLabel9 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtControlThreshold = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblControlThreshold = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel7 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtMaxPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblMaxPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblDefaultPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblMinSamples = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtDefaultPeriod = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel6 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblMinSampleCnt = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtMinSamples = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.blblAutoInitialCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtAutoInitialCalcCount = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.pnlSPCBase.SuspendLayout();
            this.gbAutoCalcSetting.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbAutoCalcSelection.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSPCBase
            // 
            this.pnlSPCBase.Controls.Add(this.gbAutoCalcSetting);
            this.pnlSPCBase.Controls.Add(this.gbAutoCalcSelection);
            this.pnlSPCBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSPCBase.Location = new System.Drawing.Point(0, 0);
            this.pnlSPCBase.Name = "pnlSPCBase";
            this.pnlSPCBase.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.TabIndex = 4;
            // 
            // gbAutoCalcSetting
            // 
            this.gbAutoCalcSetting.Controls.Add(this.tableLayoutPanel2);
            this.gbAutoCalcSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAutoCalcSetting.Location = new System.Drawing.Point(0, 251);
            this.gbAutoCalcSetting.Name = "gbAutoCalcSetting";
            this.gbAutoCalcSetting.Size = new System.Drawing.Size(759, 244);
            this.gbAutoCalcSetting.TabIndex = 3;
            this.gbAutoCalcSetting.TabStop = false;
            this.gbAutoCalcSetting.Text = "Set Auto Calculation Item";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btxtUCalcCount, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btxtCCalcCount, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btxtPCalcCount, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btxtPNCalcCount, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.bchkCCalcYN, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.bchkPNCalcYN, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.bchkPCalcYN, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.bchkUCalcYN, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(753, 225);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // btxtUCalcCount
            // 
            this.btxtUCalcCount.BssClass = "";
            this.btxtUCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtUCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtUCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtUCalcCount.IsMultiLanguage = true;
            this.btxtUCalcCount.Key = "";
            this.btxtUCalcCount.Location = new System.Drawing.Point(396, 33);
            this.btxtUCalcCount.Name = "btxtUCalcCount";
            this.btxtUCalcCount.ReadOnly = true;
            this.btxtUCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtUCalcCount.TabIndex = 28;
            // 
            // btxtCCalcCount
            // 
            this.btxtCCalcCount.BssClass = "";
            this.btxtCCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtCCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtCCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtCCalcCount.IsMultiLanguage = true;
            this.btxtCCalcCount.Key = "";
            this.btxtCCalcCount.Location = new System.Drawing.Point(135, 33);
            this.btxtCCalcCount.Name = "btxtCCalcCount";
            this.btxtCCalcCount.ReadOnly = true;
            this.btxtCCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtCCalcCount.TabIndex = 27;
            // 
            // btxtPCalcCount
            // 
            this.btxtPCalcCount.BssClass = "";
            this.btxtPCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtPCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtPCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtPCalcCount.IsMultiLanguage = true;
            this.btxtPCalcCount.Key = "";
            this.btxtPCalcCount.Location = new System.Drawing.Point(396, 6);
            this.btxtPCalcCount.Name = "btxtPCalcCount";
            this.btxtPCalcCount.ReadOnly = true;
            this.btxtPCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtPCalcCount.TabIndex = 24;
            // 
            // btxtPNCalcCount
            // 
            this.btxtPNCalcCount.BssClass = "";
            this.btxtPNCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtPNCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtPNCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtPNCalcCount.IsMultiLanguage = true;
            this.btxtPNCalcCount.Key = "";
            this.btxtPNCalcCount.Location = new System.Drawing.Point(135, 6);
            this.btxtPNCalcCount.Name = "btxtPNCalcCount";
            this.btxtPNCalcCount.ReadOnly = true;
            this.btxtPNCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtPNCalcCount.TabIndex = 23;
            // 
            // bchkCCalcYN
            // 
            this.bchkCCalcYN.BssClass = "";
            this.bchkCCalcYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkCCalcYN.IsMultiLanguage = true;
            this.bchkCCalcYN.Key = "";
            this.bchkCCalcYN.Location = new System.Drawing.Point(6, 33);
            this.bchkCCalcYN.Name = "bchkCCalcYN";
            this.bchkCCalcYN.Size = new System.Drawing.Size(123, 21);
            this.bchkCCalcYN.TabIndex = 5;
            this.bchkCCalcYN.Text = "C Control Limit";
            this.bchkCCalcYN.UseVisualStyleBackColor = true;
            // 
            // bchkPNCalcYN
            // 
            this.bchkPNCalcYN.BssClass = "";
            this.bchkPNCalcYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkPNCalcYN.IsMultiLanguage = true;
            this.bchkPNCalcYN.Key = "";
            this.bchkPNCalcYN.Location = new System.Drawing.Point(6, 6);
            this.bchkPNCalcYN.Name = "bchkPNCalcYN";
            this.bchkPNCalcYN.Size = new System.Drawing.Size(123, 21);
            this.bchkPNCalcYN.TabIndex = 4;
            this.bchkPNCalcYN.Text = "PN Control Limit";
            this.bchkPNCalcYN.UseVisualStyleBackColor = true;
            // 
            // bchkPCalcYN
            // 
            this.bchkPCalcYN.BssClass = "";
            this.bchkPCalcYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkPCalcYN.IsMultiLanguage = true;
            this.bchkPCalcYN.Key = "";
            this.bchkPCalcYN.Location = new System.Drawing.Point(225, 6);
            this.bchkPCalcYN.Name = "bchkPCalcYN";
            this.bchkPCalcYN.Size = new System.Drawing.Size(165, 21);
            this.bchkPCalcYN.TabIndex = 8;
            this.bchkPCalcYN.Text = "P Control Limit";
            this.bchkPCalcYN.UseVisualStyleBackColor = true;
            // 
            // bchkUCalcYN
            // 
            this.bchkUCalcYN.BssClass = "";
            this.bchkUCalcYN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkUCalcYN.IsMultiLanguage = true;
            this.bchkUCalcYN.Key = "";
            this.bchkUCalcYN.Location = new System.Drawing.Point(225, 33);
            this.bchkUCalcYN.Name = "bchkUCalcYN";
            this.bchkUCalcYN.Size = new System.Drawing.Size(165, 21);
            this.bchkUCalcYN.TabIndex = 10;
            this.bchkUCalcYN.Text = "U Control Limit";
            this.bchkUCalcYN.UseVisualStyleBackColor = true;
            // 
            // gbAutoCalcSelection
            // 
            this.gbAutoCalcSelection.Controls.Add(this.tableLayoutPanel1);
            this.gbAutoCalcSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbAutoCalcSelection.Location = new System.Drawing.Point(0, 0);
            this.gbAutoCalcSelection.Name = "gbAutoCalcSelection";
            this.gbAutoCalcSelection.Size = new System.Drawing.Size(759, 251);
            this.gbAutoCalcSelection.TabIndex = 2;
            this.gbAutoCalcSelection.TabStop = false;
            this.gbAutoCalcSelection.Text = "Auto Calculation Option";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.bLabel3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.bchkAutoCalcIQR, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.bchkAutoCalcShift, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.blblAutoCalcPeriod, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bLabel4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btxtAutoCalcPeriod, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btxtAutoCalcCount, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bLabel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.blblCalcCntDesc, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.bLabel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bLabel7, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btxtMaxPeriod, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.blblMaxPeriod, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.blblDefaultPeriod, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.blblMinSamples, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btxtDefaultPeriod, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.bLabel6, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.blblMinSampleCnt, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btxtMinSamples, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.blblAutoInitialCalcCount, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btxtAutoInitialCalcCount, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.blblControlThreshold, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btxtControlThreshold, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.bLabel9, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.bchkThresholdOffYN, 3, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(753, 232);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // bLabel3
            // 
            this.bLabel3.BssClass = "";
            this.bLabel3.CustomTextAlign = "";
            this.bLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel3.IsMultiLanguage = true;
            this.bLabel3.Key = "";
            this.bLabel3.Location = new System.Drawing.Point(212, 56);
            this.bLabel3.Name = "bLabel3";
            this.bLabel3.Size = new System.Drawing.Size(80, 23);
            this.bLabel3.TabIndex = 23;
            this.bLabel3.Text = "count";
            this.bLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bchkAutoCalcIQR
            // 
            this.bchkAutoCalcIQR.BssClass = "";
            this.bchkAutoCalcIQR.IsMultiLanguage = true;
            this.bchkAutoCalcIQR.Key = "";
            this.bchkAutoCalcIQR.Location = new System.Drawing.Point(298, 59);
            this.bchkAutoCalcIQR.Name = "bchkAutoCalcIQR";
            this.bchkAutoCalcIQR.Size = new System.Drawing.Size(394, 21);
            this.bchkAutoCalcIQR.TabIndex = 19;
            this.bchkAutoCalcIQR.Text = "Auto calculation without IQR Filter";
            this.bchkAutoCalcIQR.UseVisualStyleBackColor = true;
            // 
            // bchkAutoCalcShift
            // 
            this.bchkAutoCalcShift.BssClass = "";
            this.bchkAutoCalcShift.IsMultiLanguage = true;
            this.bchkAutoCalcShift.Key = "";
            this.bchkAutoCalcShift.Location = new System.Drawing.Point(298, 6);
            this.bchkAutoCalcShift.Name = "bchkAutoCalcShift";
            this.bchkAutoCalcShift.Size = new System.Drawing.Size(394, 21);
            this.bchkAutoCalcShift.TabIndex = 16;
            this.bchkAutoCalcShift.Text = "Auto calculation with shift compensation";
            this.bchkAutoCalcShift.UseVisualStyleBackColor = true;
            // 
            // blblAutoCalcPeriod
            // 
            this.blblAutoCalcPeriod.BssClass = "";
            this.blblAutoCalcPeriod.CustomTextAlign = "";
            this.blblAutoCalcPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblAutoCalcPeriod.IsMultiLanguage = true;
            this.blblAutoCalcPeriod.Key = "";
            this.blblAutoCalcPeriod.Location = new System.Drawing.Point(6, 3);
            this.blblAutoCalcPeriod.Name = "blblAutoCalcPeriod";
            this.blblAutoCalcPeriod.Size = new System.Drawing.Size(131, 23);
            this.blblAutoCalcPeriod.TabIndex = 0;
            this.blblAutoCalcPeriod.Text = "Auto Calculation Period";
            this.blblAutoCalcPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel4
            // 
            this.bLabel4.BssClass = "";
            this.bLabel4.CustomTextAlign = "";
            this.bLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel4.IsMultiLanguage = true;
            this.bLabel4.Key = "";
            this.bLabel4.Location = new System.Drawing.Point(212, 3);
            this.bLabel4.Name = "bLabel4";
            this.bLabel4.Size = new System.Drawing.Size(80, 23);
            this.bLabel4.TabIndex = 6;
            this.bLabel4.Text = "days";
            this.bLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtAutoCalcPeriod
            // 
            this.btxtAutoCalcPeriod.BssClass = "";
            this.btxtAutoCalcPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtAutoCalcPeriod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtAutoCalcPeriod.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtAutoCalcPeriod.IsMultiLanguage = true;
            this.btxtAutoCalcPeriod.Key = "";
            this.btxtAutoCalcPeriod.Location = new System.Drawing.Point(143, 6);
            this.btxtAutoCalcPeriod.Name = "btxtAutoCalcPeriod";
            this.btxtAutoCalcPeriod.Size = new System.Drawing.Size(63, 21);
            this.btxtAutoCalcPeriod.TabIndex = 12;
            // 
            // btxtAutoCalcCount
            // 
            this.btxtAutoCalcCount.BssClass = "";
            this.btxtAutoCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtAutoCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtAutoCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtAutoCalcCount.IsMultiLanguage = true;
            this.btxtAutoCalcCount.Key = "";
            this.btxtAutoCalcCount.Location = new System.Drawing.Point(143, 33);
            this.btxtAutoCalcCount.Name = "btxtAutoCalcCount";
            this.btxtAutoCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtAutoCalcCount.TabIndex = 13;
            this.btxtAutoCalcCount.Validated += new System.EventHandler(this.btxtSpec_Validated);
            // 
            // bLabel2
            // 
            this.bLabel2.BssClass = "";
            this.bLabel2.CustomTextAlign = "";
            this.bLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel2.IsMultiLanguage = true;
            this.bLabel2.Key = "";
            this.bLabel2.Location = new System.Drawing.Point(212, 30);
            this.bLabel2.Name = "bLabel2";
            this.bLabel2.Size = new System.Drawing.Size(80, 23);
            this.bLabel2.TabIndex = 18;
            this.bLabel2.Text = "count";
            this.bLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblCalcCntDesc
            // 
            this.blblCalcCntDesc.BssClass = "";
            this.blblCalcCntDesc.CustomTextAlign = "";
            this.blblCalcCntDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblCalcCntDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblCalcCntDesc.IsMultiLanguage = true;
            this.blblCalcCntDesc.Key = "";
            this.blblCalcCntDesc.Location = new System.Drawing.Point(298, 30);
            this.blblCalcCntDesc.Name = "blblCalcCntDesc";
            this.blblCalcCntDesc.Size = new System.Drawing.Size(449, 26);
            this.blblCalcCntDesc.TabIndex = 7;
            this.blblCalcCntDesc.Text = "If the Count has no value, Auto Calculation will be repeated.";
            this.blblCalcCntDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel1
            // 
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(6, 30);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(131, 23);
            this.bLabel1.TabIndex = 21;
            this.bLabel1.Text = "Auto Calculation Count";
            this.bLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bchkThresholdOffYN
            // 
            this.bchkThresholdOffYN.BssClass = "";
            this.bchkThresholdOffYN.IsMultiLanguage = true;
            this.bchkThresholdOffYN.Key = "";
            this.bchkThresholdOffYN.Location = new System.Drawing.Point(298, 169);
            this.bchkThresholdOffYN.Name = "bchkThresholdOffYN";
            this.bchkThresholdOffYN.Size = new System.Drawing.Size(394, 21);
            this.bchkThresholdOffYN.TabIndex = 20;
            this.bchkThresholdOffYN.Text = "Threshold Funtion Off";
            this.bchkThresholdOffYN.UseVisualStyleBackColor = true;
            // 
            // bLabel9
            // 
            this.bLabel9.BssClass = "";
            this.bLabel9.CustomTextAlign = "";
            this.bLabel9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel9.IsMultiLanguage = true;
            this.bLabel9.Key = "";
            this.bLabel9.Location = new System.Drawing.Point(212, 166);
            this.bLabel9.Name = "bLabel9";
            this.bLabel9.Size = new System.Drawing.Size(80, 23);
            this.bLabel9.TabIndex = 11;
            this.bLabel9.Text = "% of spec limit";
            this.bLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtControlThreshold
            // 
            this.btxtControlThreshold.BssClass = "";
            this.btxtControlThreshold.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtControlThreshold.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtControlThreshold.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtControlThreshold.IsMultiLanguage = true;
            this.btxtControlThreshold.Key = "";
            this.btxtControlThreshold.Location = new System.Drawing.Point(143, 169);
            this.btxtControlThreshold.Name = "btxtControlThreshold";
            this.btxtControlThreshold.Size = new System.Drawing.Size(63, 21);
            this.btxtControlThreshold.TabIndex = 17;
            // 
            // blblControlThreshold
            // 
            this.blblControlThreshold.BssClass = "";
            this.blblControlThreshold.CustomTextAlign = "";
            this.blblControlThreshold.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblControlThreshold.IsMultiLanguage = true;
            this.blblControlThreshold.Key = "";
            this.blblControlThreshold.Location = new System.Drawing.Point(6, 166);
            this.blblControlThreshold.Name = "blblControlThreshold";
            this.blblControlThreshold.Size = new System.Drawing.Size(131, 23);
            this.blblControlThreshold.TabIndex = 5;
            this.blblControlThreshold.Text = "Control Limit Threshold";
            this.blblControlThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel7
            // 
            this.bLabel7.BssClass = "";
            this.bLabel7.CustomTextAlign = "";
            this.bLabel7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel7.IsMultiLanguage = true;
            this.bLabel7.Key = "";
            this.bLabel7.Location = new System.Drawing.Point(212, 138);
            this.bLabel7.Name = "bLabel7";
            this.bLabel7.Size = new System.Drawing.Size(80, 23);
            this.bLabel7.TabIndex = 9;
            this.bLabel7.Text = "days";
            this.bLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtMaxPeriod
            // 
            this.btxtMaxPeriod.BssClass = "";
            this.btxtMaxPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtMaxPeriod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtMaxPeriod.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtMaxPeriod.IsMultiLanguage = true;
            this.btxtMaxPeriod.Key = "";
            this.btxtMaxPeriod.Location = new System.Drawing.Point(143, 141);
            this.btxtMaxPeriod.Name = "btxtMaxPeriod";
            this.btxtMaxPeriod.Size = new System.Drawing.Size(63, 21);
            this.btxtMaxPeriod.TabIndex = 15;
            // 
            // blblMaxPeriod
            // 
            this.blblMaxPeriod.BssClass = "";
            this.blblMaxPeriod.CustomTextAlign = "";
            this.blblMaxPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblMaxPeriod.IsMultiLanguage = true;
            this.blblMaxPeriod.Key = "";
            this.blblMaxPeriod.Location = new System.Drawing.Point(6, 138);
            this.blblMaxPeriod.Name = "blblMaxPeriod";
            this.blblMaxPeriod.Size = new System.Drawing.Size(131, 23);
            this.blblMaxPeriod.TabIndex = 3;
            this.blblMaxPeriod.Text = "Maximum period to use";
            this.blblMaxPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblDefaultPeriod
            // 
            this.blblDefaultPeriod.BssClass = "";
            this.blblDefaultPeriod.CustomTextAlign = "";
            this.blblDefaultPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblDefaultPeriod.IsMultiLanguage = true;
            this.blblDefaultPeriod.Key = "";
            this.blblDefaultPeriod.Location = new System.Drawing.Point(6, 111);
            this.blblDefaultPeriod.Name = "blblDefaultPeriod";
            this.blblDefaultPeriod.Size = new System.Drawing.Size(131, 23);
            this.blblDefaultPeriod.TabIndex = 2;
            this.blblDefaultPeriod.Text = "Default Period";
            this.blblDefaultPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblMinSamples
            // 
            this.blblMinSamples.BssClass = "";
            this.blblMinSamples.CustomTextAlign = "";
            this.blblMinSamples.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblMinSamples.IsMultiLanguage = true;
            this.blblMinSamples.Key = "";
            this.blblMinSamples.Location = new System.Drawing.Point(6, 83);
            this.blblMinSamples.Name = "blblMinSamples";
            this.blblMinSamples.Size = new System.Drawing.Size(131, 23);
            this.blblMinSamples.TabIndex = 1;
            this.blblMinSamples.Text = "Minimum samples to use";
            this.blblMinSamples.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtDefaultPeriod
            // 
            this.btxtDefaultPeriod.BssClass = "";
            this.btxtDefaultPeriod.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtDefaultPeriod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtDefaultPeriod.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtDefaultPeriod.IsMultiLanguage = true;
            this.btxtDefaultPeriod.Key = "";
            this.btxtDefaultPeriod.Location = new System.Drawing.Point(143, 114);
            this.btxtDefaultPeriod.Name = "btxtDefaultPeriod";
            this.btxtDefaultPeriod.Size = new System.Drawing.Size(63, 21);
            this.btxtDefaultPeriod.TabIndex = 14;
            // 
            // bLabel6
            // 
            this.bLabel6.BssClass = "";
            this.bLabel6.CustomTextAlign = "";
            this.bLabel6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel6.IsMultiLanguage = true;
            this.bLabel6.Key = "";
            this.bLabel6.Location = new System.Drawing.Point(212, 111);
            this.bLabel6.Name = "bLabel6";
            this.bLabel6.Size = new System.Drawing.Size(80, 23);
            this.bLabel6.TabIndex = 8;
            this.bLabel6.Text = "days";
            this.bLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblMinSampleCnt
            // 
            this.blblMinSampleCnt.BssClass = "";
            this.blblMinSampleCnt.CustomTextAlign = "";
            this.blblMinSampleCnt.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblMinSampleCnt.IsMultiLanguage = true;
            this.blblMinSampleCnt.Key = "";
            this.blblMinSampleCnt.Location = new System.Drawing.Point(212, 83);
            this.blblMinSampleCnt.Name = "blblMinSampleCnt";
            this.blblMinSampleCnt.Size = new System.Drawing.Size(80, 23);
            this.blblMinSampleCnt.TabIndex = 7;
            this.blblMinSampleCnt.Text = "Lot/Wafer";
            this.blblMinSampleCnt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtMinSamples
            // 
            this.btxtMinSamples.BssClass = "";
            this.btxtMinSamples.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtMinSamples.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtMinSamples.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtMinSamples.IsMultiLanguage = true;
            this.btxtMinSamples.Key = "";
            this.btxtMinSamples.Location = new System.Drawing.Point(143, 86);
            this.btxtMinSamples.Name = "btxtMinSamples";
            this.btxtMinSamples.Size = new System.Drawing.Size(63, 21);
            this.btxtMinSamples.TabIndex = 13;
            // 
            // blblAutoInitialCalcCount
            // 
            this.blblAutoInitialCalcCount.BssClass = "";
            this.blblAutoInitialCalcCount.CustomTextAlign = "";
            this.blblAutoInitialCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblAutoInitialCalcCount.IsMultiLanguage = true;
            this.blblAutoInitialCalcCount.Key = "";
            this.blblAutoInitialCalcCount.Location = new System.Drawing.Point(6, 56);
            this.blblAutoInitialCalcCount.Name = "blblAutoInitialCalcCount";
            this.blblAutoInitialCalcCount.Size = new System.Drawing.Size(131, 23);
            this.blblAutoInitialCalcCount.TabIndex = 0;
            this.blblAutoInitialCalcCount.Text = "Initial Calculation Count";
            this.blblAutoInitialCalcCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btxtAutoInitialCalcCount
            // 
            this.btxtAutoInitialCalcCount.BssClass = "";
            this.btxtAutoInitialCalcCount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxtAutoInitialCalcCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtAutoInitialCalcCount.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtAutoInitialCalcCount.IsMultiLanguage = true;
            this.btxtAutoInitialCalcCount.Key = "";
            this.btxtAutoInitialCalcCount.Location = new System.Drawing.Point(143, 59);
            this.btxtAutoInitialCalcCount.Name = "btxtAutoInitialCalcCount";
            this.btxtAutoInitialCalcCount.ReadOnly = true;
            this.btxtAutoInitialCalcCount.Size = new System.Drawing.Size(63, 21);
            this.btxtAutoInitialCalcCount.TabIndex = 22;
            // 
            // AutoCalcConfigurationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlSPCBase);
            this.Name = "AutoCalcConfigurationUC";
            this.Size = new System.Drawing.Size(759, 495);
            this.pnlSPCBase.ResumeLayout(false);
            this.gbAutoCalcSetting.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.gbAutoCalcSelection.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.PropertyGrid propGrid;        
        //private BISTel.eSPC.Page.Common.BTChartUC unitedChartUC;
        private System.Windows.Forms.Panel pnlSPCBase;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblAutoCalcPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblMinSamples;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblDefaultPeriod;
        private System.Windows.Forms.GroupBox gbAutoCalcSelection;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblMinSampleCnt;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel6;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtAutoCalcPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtMinSamples;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtDefaultPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblCalcCntDesc;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtAutoCalcCount;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblAutoInitialCalcCount;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel2;
        private System.Windows.Forms.GroupBox gbAutoCalcSetting;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkUCalcYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkPCalcYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkCCalcYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkPNCalcYN;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblControlThreshold;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtControlThreshold;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel9;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblMaxPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtMaxPeriod;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkAutoCalcShift;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckBox bchkAutoCalcIQR;
                

        private PeakPerformance.Client.BISTelControl.BCheckBox bchkThresholdOffYN;
        private PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private PeakPerformance.Client.BISTelControl.BLabel bLabel3;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtAutoInitialCalcCount;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtUCalcCount;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtCCalcCount;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtPCalcCount;
        private PeakPerformance.Client.BISTelControl.BTextBox btxtPNCalcCount;
    }
}
