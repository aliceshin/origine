namespace BISTel.eSPC.Page.Common
{
    partial class ColumnVisibilityControl
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
            this.bapnlTextBoxUserControl = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCheckCombo = new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo();
            this.panel2 = new System.Windows.Forms.Panel();
            this.blblLabel = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblbullet01 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bapnlTextBoxUserControl.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bapnlTextBoxUserControl
            // 
            this.bapnlTextBoxUserControl.BLabelAutoPadding = true;
            this.bapnlTextBoxUserControl.BssClass = "TitlePanel";
            this.bapnlTextBoxUserControl.Controls.Add(this.panel1);
            this.bapnlTextBoxUserControl.Controls.Add(this.panel2);
            this.bapnlTextBoxUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bapnlTextBoxUserControl.IsCondition = true;
            this.bapnlTextBoxUserControl.Key = "";
            this.bapnlTextBoxUserControl.Location = new System.Drawing.Point(0, 0);
            this.bapnlTextBoxUserControl.Name = "bapnlTextBoxUserControl";
            this.bapnlTextBoxUserControl.Padding_Bottom = 5;
            this.bapnlTextBoxUserControl.Padding_Left = 5;
            this.bapnlTextBoxUserControl.Padding_Right = 5;
            this.bapnlTextBoxUserControl.Padding_Top = 5;
            this.bapnlTextBoxUserControl.Size = new System.Drawing.Size(258, 29);
            this.bapnlTextBoxUserControl.Space = 5;
            this.bapnlTextBoxUserControl.TabIndex = 1;
            this.bapnlTextBoxUserControl.Title = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.bCheckCombo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(65, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(193, 29);
            this.panel1.TabIndex = 2;
            // 
            // bCheckCombo
            // 
            this.bCheckCombo.BackColor = System.Drawing.Color.White;
            this.bCheckCombo.BssClass = "";
            this.bCheckCombo.CausesValidation = false;
            this.bCheckCombo.DataSource = null;
            this.bCheckCombo.DisplayMember = "";
            this.bCheckCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCheckCombo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bCheckCombo.ForeColor = System.Drawing.SystemColors.Window;
            this.bCheckCombo.IsIncludeAll = false;
            this.bCheckCombo.Key = "";
            this.bCheckCombo.Location = new System.Drawing.Point(4, 4);
            this.bCheckCombo.Name = "bCheckCombo";
            this.bCheckCombo.Size = new System.Drawing.Size(185, 21);
            this.bCheckCombo.TabIndex = 0;
            this.bCheckCombo.ValueMember = "";
            this.bCheckCombo.Item_Check += new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo.ItemClickDelegate(this.bCheckCombo_Item_Check);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.blblbullet01);
            this.panel2.Controls.Add(this.blblLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(65, 29);
            this.panel2.TabIndex = 3;
            // 
            // blblLabel
            // 
            this.blblLabel.BackColor = System.Drawing.Color.Transparent;
            this.blblLabel.BssClass = "";
            this.blblLabel.CustomTextAlign = "";
            this.blblLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblLabel.ForeColor = System.Drawing.Color.Black;
            this.blblLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.blblLabel.IsMultiLanguage = true;
            this.blblLabel.Key = "FDC_LBL_COLUMNS";
            this.blblLabel.Location = new System.Drawing.Point(0, 0);
            this.blblLabel.Name = "blblLabel";
            this.blblLabel.Padding = new System.Windows.Forms.Padding(4);
            this.blblLabel.Size = new System.Drawing.Size(74, 29);
            this.blblLabel.TabIndex = 0;
            this.blblLabel.Text = "Columns";
            this.blblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blblbullet01
            // 
            this.blblbullet01.BackColor = System.Drawing.Color.Transparent;
            this.blblbullet01.BssClass = "";
            this.blblbullet01.CustomTextAlign = "";
            this.blblbullet01.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblbullet01.ForeColor = System.Drawing.Color.Black;
            this.blblbullet01.Image = global::BISTel.eSPC.Page.Properties.Resources.bullet_01;
            this.blblbullet01.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.blblbullet01.IsMultiLanguage = true;
            this.blblbullet01.Key = "";
            this.blblbullet01.Location = new System.Drawing.Point(3, 4);
            this.blblbullet01.Name = "blblbullet01";
            this.blblbullet01.Size = new System.Drawing.Size(10, 20);
            this.blblbullet01.TabIndex = 8;
            // 
            // ColumnVisibilityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bapnlTextBoxUserControl);
            this.Name = "ColumnVisibilityControl";
            this.Size = new System.Drawing.Size(258, 29);
            this.bapnlTextBoxUserControl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlTextBoxUserControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblLabel;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo bCheckCombo;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblbullet01;
    }
}
