namespace BISTel.eSPC.Condition.Controls.Lot
{
    partial class LotSearchControl
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
            this.bcpnlLot = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_LotID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblLotID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel2 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnl_RadioBtn = new System.Windows.Forms.Panel();
            this.brbtn_OOC = new BISTel.PeakPerformance.Client.BISTelControl.BRadioButton();
            this.brbtn_OCAP = new BISTel.PeakPerformance.Client.BISTelControl.BRadioButton();
            this.bcpnlRecipe = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_RecipeID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblRecipeID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblbullet01 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlSubstrate = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_SubstrateID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblSubstrateID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlEQPID = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_EQPID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblEQPID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel5 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlChartID = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_ChartID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblChartID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel3 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlModuleID = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.btxt_ModuleID = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.lblModuleID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel6 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlLot.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnl_RadioBtn.SuspendLayout();
            this.bcpnlRecipe.SuspendLayout();
            this.bcpnlSubstrate.SuspendLayout();
            this.bcpnlEQPID.SuspendLayout();
            this.bcpnlChartID.SuspendLayout();
            this.bcpnlModuleID.SuspendLayout();
            this.SuspendLayout();
            // 
            // bcpnlLot
            // 
            this.bcpnlLot.BottomImage = null;
            this.bcpnlLot.BottomLeftImage = null;
            this.bcpnlLot.BssClass = "";
            this.bcpnlLot.Controls.Add(this.btxt_LotID);
            this.bcpnlLot.Controls.Add(this.lblLotID);
            this.bcpnlLot.Controls.Add(this.bLabel2);
            this.bcpnlLot.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlLot.DownImage = null;
            this.bcpnlLot.Key = "";
            this.bcpnlLot.LeftImage = null;
            this.bcpnlLot.LeftTopImage = null;
            this.bcpnlLot.Location = new System.Drawing.Point(0, 87);
            this.bcpnlLot.Name = "bcpnlLot";
            this.bcpnlLot.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlLot.RightBottomImage = null;
            this.bcpnlLot.RightImage = null;
            this.bcpnlLot.Size = new System.Drawing.Size(341, 29);
            this.bcpnlLot.TabIndex = 0;
            this.bcpnlLot.TopImage = null;
            this.bcpnlLot.TopRightImage = null;
            this.bcpnlLot.UpImage = null;
            // 
            // btxt_LotID
            // 
            this.btxt_LotID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_LotID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_LotID.BssClass = "";
            this.btxt_LotID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_LotID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_LotID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_LotID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_LotID.IsMultiLanguage = true;
            this.btxt_LotID.Key = "";
            this.btxt_LotID.Location = new System.Drawing.Point(89, 4);
            this.btxt_LotID.MaxLength = 100;
            this.btxt_LotID.Name = "btxt_LotID";
            this.btxt_LotID.Size = new System.Drawing.Size(237, 21);
            this.btxt_LotID.TabIndex = 4;
            // 
            // lblLotID
            // 
            this.lblLotID.BackColor = System.Drawing.Color.White;
            this.lblLotID.BssClass = "";
            this.lblLotID.CustomTextAlign = "";
            this.lblLotID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLotID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLotID.ForeColor = System.Drawing.Color.Black;
            this.lblLotID.IsMultiLanguage = true;
            this.lblLotID.Key = "";
            this.lblLotID.Location = new System.Drawing.Point(20, 4);
            this.lblLotID.Name = "lblLotID";
            this.lblLotID.Size = new System.Drawing.Size(69, 21);
            this.lblLotID.TabIndex = 6;
            this.lblLotID.Text = "LOT ID";
            this.lblLotID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel2
            // 
            this.bLabel2.BackColor = System.Drawing.Color.White;
            this.bLabel2.BssClass = "";
            this.bLabel2.CustomTextAlign = "";
            this.bLabel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel2.ForeColor = System.Drawing.Color.Black;
            this.bLabel2.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel2.IsMultiLanguage = true;
            this.bLabel2.Key = "";
            this.bLabel2.Location = new System.Drawing.Point(10, 4);
            this.bLabel2.Name = "bLabel2";
            this.bLabel2.Size = new System.Drawing.Size(10, 21);
            this.bLabel2.TabIndex = 11;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.Controls.Add(this.pnl_RadioBtn);
            this.pnlBody.Controls.Add(this.bcpnlRecipe);
            this.pnlBody.Controls.Add(this.bcpnlSubstrate);
            this.pnlBody.Controls.Add(this.bcpnlLot);
            this.pnlBody.Controls.Add(this.bcpnlModuleID);
            this.pnlBody.Controls.Add(this.bcpnlEQPID);
            this.pnlBody.Controls.Add(this.bcpnlChartID);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(341, 208);
            this.pnlBody.TabIndex = 2;
            // 
            // pnl_RadioBtn
            // 
            this.pnl_RadioBtn.Controls.Add(this.brbtn_OOC);
            this.pnl_RadioBtn.Controls.Add(this.brbtn_OCAP);
            this.pnl_RadioBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_RadioBtn.Location = new System.Drawing.Point(0, 174);
            this.pnl_RadioBtn.Name = "pnl_RadioBtn";
            this.pnl_RadioBtn.Size = new System.Drawing.Size(341, 33);
            this.pnl_RadioBtn.TabIndex = 4;
            // 
            // brbtn_OOC
            // 
            this.brbtn_OOC.BssClass = "";
            this.brbtn_OOC.IsMultiLanguage = true;
            this.brbtn_OOC.Key = "";
            this.brbtn_OOC.Location = new System.Drawing.Point(107, 3);
            this.brbtn_OOC.Name = "brbtn_OOC";
            this.brbtn_OOC.Size = new System.Drawing.Size(89, 26);
            this.brbtn_OOC.TabIndex = 1;
            this.brbtn_OOC.Text = "OOC";
            this.brbtn_OOC.UseVisualStyleBackColor = true;
            // 
            // brbtn_OCAP
            // 
            this.brbtn_OCAP.BssClass = "";
            this.brbtn_OCAP.Checked = true;
            this.brbtn_OCAP.IsMultiLanguage = true;
            this.brbtn_OCAP.Key = "";
            this.brbtn_OCAP.Location = new System.Drawing.Point(13, 3);
            this.brbtn_OCAP.Name = "brbtn_OCAP";
            this.brbtn_OCAP.Size = new System.Drawing.Size(89, 26);
            this.brbtn_OCAP.TabIndex = 0;
            this.brbtn_OCAP.TabStop = true;
            this.brbtn_OCAP.Text = "OCAP";
            this.brbtn_OCAP.UseVisualStyleBackColor = true;
            // 
            // bcpnlRecipe
            // 
            this.bcpnlRecipe.BottomImage = null;
            this.bcpnlRecipe.BottomLeftImage = null;
            this.bcpnlRecipe.BssClass = "";
            this.bcpnlRecipe.Controls.Add(this.btxt_RecipeID);
            this.bcpnlRecipe.Controls.Add(this.lblRecipeID);
            this.bcpnlRecipe.Controls.Add(this.blblbullet01);
            this.bcpnlRecipe.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlRecipe.DownImage = null;
            this.bcpnlRecipe.Key = "";
            this.bcpnlRecipe.LeftImage = null;
            this.bcpnlRecipe.LeftTopImage = null;
            this.bcpnlRecipe.Location = new System.Drawing.Point(0, 145);
            this.bcpnlRecipe.Name = "bcpnlRecipe";
            this.bcpnlRecipe.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlRecipe.RightBottomImage = null;
            this.bcpnlRecipe.RightImage = null;
            this.bcpnlRecipe.Size = new System.Drawing.Size(341, 29);
            this.bcpnlRecipe.TabIndex = 2;
            this.bcpnlRecipe.TopImage = null;
            this.bcpnlRecipe.TopRightImage = null;
            this.bcpnlRecipe.UpImage = null;
            // 
            // btxt_RecipeID
            // 
            this.btxt_RecipeID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_RecipeID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_RecipeID.BssClass = "";
            this.btxt_RecipeID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_RecipeID.Enabled = false;
            this.btxt_RecipeID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_RecipeID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_RecipeID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_RecipeID.IsMultiLanguage = true;
            this.btxt_RecipeID.Key = "";
            this.btxt_RecipeID.Location = new System.Drawing.Point(89, 4);
            this.btxt_RecipeID.MaxLength = 100;
            this.btxt_RecipeID.Name = "btxt_RecipeID";
            this.btxt_RecipeID.Size = new System.Drawing.Size(237, 21);
            this.btxt_RecipeID.TabIndex = 4;
            // 
            // lblRecipeID
            // 
            this.lblRecipeID.BackColor = System.Drawing.Color.White;
            this.lblRecipeID.BssClass = "";
            this.lblRecipeID.CustomTextAlign = "";
            this.lblRecipeID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblRecipeID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRecipeID.ForeColor = System.Drawing.Color.Black;
            this.lblRecipeID.IsMultiLanguage = true;
            this.lblRecipeID.Key = "";
            this.lblRecipeID.Location = new System.Drawing.Point(20, 4);
            this.lblRecipeID.Name = "lblRecipeID";
            this.lblRecipeID.Size = new System.Drawing.Size(69, 21);
            this.lblRecipeID.TabIndex = 8;
            this.lblRecipeID.Text = "Recipe ID";
            this.lblRecipeID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blblbullet01
            // 
            this.blblbullet01.BackColor = System.Drawing.Color.White;
            this.blblbullet01.BssClass = "";
            this.blblbullet01.CustomTextAlign = "";
            this.blblbullet01.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblbullet01.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblbullet01.ForeColor = System.Drawing.Color.Black;
            this.blblbullet01.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.blblbullet01.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.blblbullet01.IsMultiLanguage = true;
            this.blblbullet01.Key = "";
            this.blblbullet01.Location = new System.Drawing.Point(10, 4);
            this.blblbullet01.Name = "blblbullet01";
            this.blblbullet01.Size = new System.Drawing.Size(10, 21);
            this.blblbullet01.TabIndex = 10;
            // 
            // bcpnlSubstrate
            // 
            this.bcpnlSubstrate.BottomImage = null;
            this.bcpnlSubstrate.BottomLeftImage = null;
            this.bcpnlSubstrate.BssClass = "";
            this.bcpnlSubstrate.Controls.Add(this.btxt_SubstrateID);
            this.bcpnlSubstrate.Controls.Add(this.lblSubstrateID);
            this.bcpnlSubstrate.Controls.Add(this.bLabel1);
            this.bcpnlSubstrate.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlSubstrate.DownImage = null;
            this.bcpnlSubstrate.Key = "";
            this.bcpnlSubstrate.LeftImage = null;
            this.bcpnlSubstrate.LeftTopImage = null;
            this.bcpnlSubstrate.Location = new System.Drawing.Point(0, 116);
            this.bcpnlSubstrate.Name = "bcpnlSubstrate";
            this.bcpnlSubstrate.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlSubstrate.RightBottomImage = null;
            this.bcpnlSubstrate.RightImage = null;
            this.bcpnlSubstrate.Size = new System.Drawing.Size(341, 29);
            this.bcpnlSubstrate.TabIndex = 1;
            this.bcpnlSubstrate.TopImage = null;
            this.bcpnlSubstrate.TopRightImage = null;
            this.bcpnlSubstrate.UpImage = null;
            // 
            // btxt_SubstrateID
            // 
            this.btxt_SubstrateID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_SubstrateID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_SubstrateID.BssClass = "";
            this.btxt_SubstrateID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_SubstrateID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_SubstrateID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_SubstrateID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_SubstrateID.IsMultiLanguage = true;
            this.btxt_SubstrateID.Key = "";
            this.btxt_SubstrateID.Location = new System.Drawing.Point(89, 4);
            this.btxt_SubstrateID.MaxLength = 100;
            this.btxt_SubstrateID.Name = "btxt_SubstrateID";
            this.btxt_SubstrateID.Size = new System.Drawing.Size(237, 21);
            this.btxt_SubstrateID.TabIndex = 4;
            // 
            // lblSubstrateID
            // 
            this.lblSubstrateID.BackColor = System.Drawing.Color.White;
            this.lblSubstrateID.BssClass = "";
            this.lblSubstrateID.CustomTextAlign = "";
            this.lblSubstrateID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSubstrateID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSubstrateID.ForeColor = System.Drawing.Color.Black;
            this.lblSubstrateID.IsMultiLanguage = true;
            this.lblSubstrateID.Key = "";
            this.lblSubstrateID.Location = new System.Drawing.Point(20, 4);
            this.lblSubstrateID.Name = "lblSubstrateID";
            this.lblSubstrateID.Size = new System.Drawing.Size(69, 21);
            this.lblSubstrateID.TabIndex = 7;
            this.lblSubstrateID.Text = "Substrate ID";
            this.lblSubstrateID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel1
            // 
            this.bLabel1.BackColor = System.Drawing.Color.White;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.ForeColor = System.Drawing.Color.Black;
            this.bLabel1.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(10, 4);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(10, 21);
            this.bLabel1.TabIndex = 11;
            // 
            // bcpnlEQPID
            // 
            this.bcpnlEQPID.BottomImage = null;
            this.bcpnlEQPID.BottomLeftImage = null;
            this.bcpnlEQPID.BssClass = "";
            this.bcpnlEQPID.Controls.Add(this.btxt_EQPID);
            this.bcpnlEQPID.Controls.Add(this.lblEQPID);
            this.bcpnlEQPID.Controls.Add(this.bLabel5);
            this.bcpnlEQPID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlEQPID.DownImage = null;
            this.bcpnlEQPID.Key = "";
            this.bcpnlEQPID.LeftImage = null;
            this.bcpnlEQPID.LeftTopImage = null;
            this.bcpnlEQPID.Location = new System.Drawing.Point(0, 29);
            this.bcpnlEQPID.Name = "bcpnlEQPID";
            this.bcpnlEQPID.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlEQPID.RightBottomImage = null;
            this.bcpnlEQPID.RightImage = null;
            this.bcpnlEQPID.Size = new System.Drawing.Size(341, 29);
            this.bcpnlEQPID.TabIndex = 5;
            this.bcpnlEQPID.TopImage = null;
            this.bcpnlEQPID.TopRightImage = null;
            this.bcpnlEQPID.UpImage = null;
            // 
            // btxt_EQPID
            // 
            this.btxt_EQPID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_EQPID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_EQPID.BssClass = "";
            this.btxt_EQPID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_EQPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_EQPID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_EQPID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_EQPID.IsMultiLanguage = true;
            this.btxt_EQPID.Key = "";
            this.btxt_EQPID.Location = new System.Drawing.Point(89, 4);
            this.btxt_EQPID.MaxLength = 100;
            this.btxt_EQPID.Name = "btxt_EQPID";
            this.btxt_EQPID.Size = new System.Drawing.Size(237, 21);
            this.btxt_EQPID.TabIndex = 4;
            // 
            // lblEQPID
            // 
            this.lblEQPID.BackColor = System.Drawing.Color.White;
            this.lblEQPID.BssClass = "";
            this.lblEQPID.CustomTextAlign = "";
            this.lblEQPID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblEQPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEQPID.ForeColor = System.Drawing.Color.Black;
            this.lblEQPID.IsMultiLanguage = true;
            this.lblEQPID.Key = "";
            this.lblEQPID.Location = new System.Drawing.Point(20, 4);
            this.lblEQPID.Name = "lblEQPID";
            this.lblEQPID.Size = new System.Drawing.Size(69, 21);
            this.lblEQPID.TabIndex = 6;
            this.lblEQPID.Text = "EQP ID";
            this.lblEQPID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel5
            // 
            this.bLabel5.BackColor = System.Drawing.Color.White;
            this.bLabel5.BssClass = "";
            this.bLabel5.CustomTextAlign = "";
            this.bLabel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel5.ForeColor = System.Drawing.Color.Black;
            this.bLabel5.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel5.IsMultiLanguage = true;
            this.bLabel5.Key = "";
            this.bLabel5.Location = new System.Drawing.Point(10, 4);
            this.bLabel5.Name = "bLabel5";
            this.bLabel5.Size = new System.Drawing.Size(10, 21);
            this.bLabel5.TabIndex = 11;
            // 
            // bcpnlChartID
            // 
            this.bcpnlChartID.BottomImage = null;
            this.bcpnlChartID.BottomLeftImage = null;
            this.bcpnlChartID.BssClass = "";
            this.bcpnlChartID.Controls.Add(this.btxt_ChartID);
            this.bcpnlChartID.Controls.Add(this.lblChartID);
            this.bcpnlChartID.Controls.Add(this.bLabel3);
            this.bcpnlChartID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlChartID.DownImage = null;
            this.bcpnlChartID.Key = "";
            this.bcpnlChartID.LeftImage = null;
            this.bcpnlChartID.LeftTopImage = null;
            this.bcpnlChartID.Location = new System.Drawing.Point(0, 0);
            this.bcpnlChartID.Name = "bcpnlChartID";
            this.bcpnlChartID.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlChartID.RightBottomImage = null;
            this.bcpnlChartID.RightImage = null;
            this.bcpnlChartID.Size = new System.Drawing.Size(341, 29);
            this.bcpnlChartID.TabIndex = 3;
            this.bcpnlChartID.TopImage = null;
            this.bcpnlChartID.TopRightImage = null;
            this.bcpnlChartID.UpImage = null;
            // 
            // btxt_ChartID
            // 
            this.btxt_ChartID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_ChartID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_ChartID.BssClass = "";
            this.btxt_ChartID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_ChartID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_ChartID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_ChartID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_ChartID.IsMultiLanguage = true;
            this.btxt_ChartID.Key = "";
            this.btxt_ChartID.Location = new System.Drawing.Point(89, 4);
            this.btxt_ChartID.MaxLength = 100;
            this.btxt_ChartID.Name = "btxt_ChartID";
            this.btxt_ChartID.Size = new System.Drawing.Size(237, 21);
            this.btxt_ChartID.TabIndex = 4;
            // 
            // lblChartID
            // 
            this.lblChartID.BackColor = System.Drawing.Color.White;
            this.lblChartID.BssClass = "";
            this.lblChartID.CustomTextAlign = "";
            this.lblChartID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblChartID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChartID.ForeColor = System.Drawing.Color.Black;
            this.lblChartID.IsMultiLanguage = true;
            this.lblChartID.Key = "";
            this.lblChartID.Location = new System.Drawing.Point(20, 4);
            this.lblChartID.Name = "lblChartID";
            this.lblChartID.Size = new System.Drawing.Size(69, 21);
            this.lblChartID.TabIndex = 6;
            this.lblChartID.Text = "Chart ID";
            this.lblChartID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel3
            // 
            this.bLabel3.BackColor = System.Drawing.Color.White;
            this.bLabel3.BssClass = "";
            this.bLabel3.CustomTextAlign = "";
            this.bLabel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel3.ForeColor = System.Drawing.Color.Black;
            this.bLabel3.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel3.IsMultiLanguage = true;
            this.bLabel3.Key = "";
            this.bLabel3.Location = new System.Drawing.Point(10, 4);
            this.bLabel3.Name = "bLabel3";
            this.bLabel3.Size = new System.Drawing.Size(10, 21);
            this.bLabel3.TabIndex = 11;
            // 
            // bcpnlModuleID
            // 
            this.bcpnlModuleID.BottomImage = null;
            this.bcpnlModuleID.BottomLeftImage = null;
            this.bcpnlModuleID.BssClass = "";
            this.bcpnlModuleID.Controls.Add(this.btxt_ModuleID);
            this.bcpnlModuleID.Controls.Add(this.lblModuleID);
            this.bcpnlModuleID.Controls.Add(this.bLabel6);
            this.bcpnlModuleID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlModuleID.DownImage = null;
            this.bcpnlModuleID.Key = "";
            this.bcpnlModuleID.LeftImage = null;
            this.bcpnlModuleID.LeftTopImage = null;
            this.bcpnlModuleID.Location = new System.Drawing.Point(0, 58);
            this.bcpnlModuleID.Name = "bcpnlModuleID";
            this.bcpnlModuleID.Padding = new System.Windows.Forms.Padding(10, 4, 15, 4);
            this.bcpnlModuleID.RightBottomImage = null;
            this.bcpnlModuleID.RightImage = null;
            this.bcpnlModuleID.Size = new System.Drawing.Size(341, 29);
            this.bcpnlModuleID.TabIndex = 6;
            this.bcpnlModuleID.TopImage = null;
            this.bcpnlModuleID.TopRightImage = null;
            this.bcpnlModuleID.UpImage = null;
            // 
            // btxt_ModuleID
            // 
            this.btxt_ModuleID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.btxt_ModuleID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.btxt_ModuleID.BssClass = "";
            this.btxt_ModuleID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxt_ModuleID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btxt_ModuleID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxt_ModuleID.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxt_ModuleID.IsMultiLanguage = true;
            this.btxt_ModuleID.Key = "";
            this.btxt_ModuleID.Location = new System.Drawing.Point(89, 4);
            this.btxt_ModuleID.MaxLength = 100;
            this.btxt_ModuleID.Name = "btxt_ModuleID";
            this.btxt_ModuleID.Size = new System.Drawing.Size(237, 21);
            this.btxt_ModuleID.TabIndex = 4;
            // 
            // lblModuleID
            // 
            this.lblModuleID.BackColor = System.Drawing.Color.White;
            this.lblModuleID.BssClass = "";
            this.lblModuleID.CustomTextAlign = "";
            this.lblModuleID.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblModuleID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblModuleID.ForeColor = System.Drawing.Color.Black;
            this.lblModuleID.IsMultiLanguage = true;
            this.lblModuleID.Key = "";
            this.lblModuleID.Location = new System.Drawing.Point(20, 4);
            this.lblModuleID.Name = "lblModuleID";
            this.lblModuleID.Size = new System.Drawing.Size(69, 21);
            this.lblModuleID.TabIndex = 6;
            this.lblModuleID.Text = "MODULE ID";
            this.lblModuleID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bLabel6
            // 
            this.bLabel6.BackColor = System.Drawing.Color.White;
            this.bLabel6.BssClass = "";
            this.bLabel6.CustomTextAlign = "";
            this.bLabel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel6.ForeColor = System.Drawing.Color.Black;
            this.bLabel6.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel6.IsMultiLanguage = true;
            this.bLabel6.Key = "";
            this.bLabel6.Location = new System.Drawing.Point(10, 4);
            this.bLabel6.Name = "bLabel6";
            this.bLabel6.Size = new System.Drawing.Size(10, 21);
            this.bLabel6.TabIndex = 11;
            // 
            // LotSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBody);
            this.Name = "LotSearchControl";
            this.Size = new System.Drawing.Size(341, 208);
            this.bcpnlLot.ResumeLayout(false);
            this.bcpnlLot.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnl_RadioBtn.ResumeLayout(false);
            this.bcpnlRecipe.ResumeLayout(false);
            this.bcpnlRecipe.PerformLayout();
            this.bcpnlSubstrate.ResumeLayout(false);
            this.bcpnlSubstrate.PerformLayout();
            this.bcpnlEQPID.ResumeLayout(false);
            this.bcpnlEQPID.PerformLayout();
            this.bcpnlChartID.ResumeLayout(false);
            this.bcpnlChartID.PerformLayout();
            this.bcpnlModuleID.ResumeLayout(false);
            this.bcpnlModuleID.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlLot;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_LotID;
        private System.Windows.Forms.Panel pnlBody;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlSubstrate;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_SubstrateID;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlRecipe;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_RecipeID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblLotID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblRecipeID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblSubstrateID;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlChartID;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_ChartID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblChartID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblbullet01;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel3;
        private System.Windows.Forms.Panel pnl_RadioBtn;
        private BISTel.PeakPerformance.Client.BISTelControl.BRadioButton brbtn_OOC;
        private BISTel.PeakPerformance.Client.BISTelControl.BRadioButton brbtn_OCAP;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlEQPID;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_EQPID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblEQPID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel5;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlModuleID;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxt_ModuleID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblModuleID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel6;

    }
}
