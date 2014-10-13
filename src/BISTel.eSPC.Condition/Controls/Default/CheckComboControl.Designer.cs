namespace BISTel.eSPC.Condition.Controls.Default
{
    partial class CheckComboControl
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
            this.cboValue = new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo();
            this.bcpnlBody = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.lblTitle = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboValue
            // 
            this.cboValue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cboValue.BackColor = System.Drawing.Color.White;
            this.cboValue.BssClass = "";
            this.cboValue.CausesValidation = false;
            this.cboValue.DataSource = null;
            this.cboValue.DisplayMember = "";
            this.cboValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cboValue.ForeColor = System.Drawing.SystemColors.Window;
            this.cboValue.IsIncludeAll = false;
            this.cboValue.Key = "";
            this.cboValue.Location = new System.Drawing.Point(94, 4);
            this.cboValue.Margin = new System.Windows.Forms.Padding(0);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(209, 21);
            this.cboValue.TabIndex = 5;
            this.cboValue.ValueMember = "";
            this.cboValue.TextChanged += new System.EventHandler(this.cboValue_TextChanged);
            this.cboValue.DropDown += new System.EventHandler(this.cboValue_DropDown);
            this.cboValue.CheckCombo_Select += new System.EventHandler(this.cboValue_CheckCombo_Select);
            // 
            // bcpnlBody
            // 
            this.bcpnlBody.BottomImage = null;
            this.bcpnlBody.BottomLeftImage = null;
            this.bcpnlBody.BssClass = "";
            this.bcpnlBody.Controls.Add(this.cboValue);
            this.bcpnlBody.Controls.Add(this.lblTitle);
            this.bcpnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcpnlBody.DownImage = null;
            this.bcpnlBody.Key = "";
            this.bcpnlBody.LeftImage = null;
            this.bcpnlBody.LeftTopImage = null;
            this.bcpnlBody.Location = new System.Drawing.Point(0, 0);
            this.bcpnlBody.Name = "bcpnlBody";
            this.bcpnlBody.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.bcpnlBody.RightBottomImage = null;
            this.bcpnlBody.RightImage = null;
            this.bcpnlBody.Size = new System.Drawing.Size(315, 27);
            this.bcpnlBody.TabIndex = 4;
            this.bcpnlBody.TopImage = null;
            this.bcpnlBody.TopRightImage = null;
            this.bcpnlBody.UpImage = null;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.BssClass = "";
            this.lblTitle.CustomTextAlign = "";
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.IsMultiLanguage = true;
            this.lblTitle.Key = "";
            this.lblTitle.Location = new System.Drawing.Point(12, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(82, 19);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Line";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheckComboControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bcpnlBody);
            this.Name = "CheckComboControl";
            this.Size = new System.Drawing.Size(315, 27);
            this.bcpnlBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo cboValue;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlBody;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblTitle;

    }
}
