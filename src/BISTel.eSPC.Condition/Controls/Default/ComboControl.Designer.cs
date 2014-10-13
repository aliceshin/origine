namespace BISTel.eSPC.Condition.Controls.Default
{
    partial class ComboControl
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
            this.cboValue = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.lblTitle = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlCombo = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.bcpnlCombo.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboValue
            // 
            this.cboValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboValue.BssClass = "";
            this.cboValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cboValue.FormattingEnabled = true;
            this.cboValue.IsMultiLanguage = true;
            this.cboValue.Key = "";
            this.cboValue.Location = new System.Drawing.Point(94, 4);
            this.cboValue.Margin = new System.Windows.Forms.Padding(0);
            this.cboValue.Name = "cboValue";
            this.cboValue.Size = new System.Drawing.Size(209, 21);
            this.cboValue.TabIndex = 2;
            this.cboValue.SelectedIndexChanged += new System.EventHandler(this.cboValue_SelectedIndexChanged);
            this.cboValue.TextUpdate += new System.EventHandler(this.cboValue_TextUpdate);
            this.cboValue.DropDown += new System.EventHandler(this.cboValue_DropDown);
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
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Line";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcpnlCombo
            // 
            this.bcpnlCombo.BottomImage = null;
            this.bcpnlCombo.BottomLeftImage = null;
            this.bcpnlCombo.BssClass = "TitlePanel";
            this.bcpnlCombo.Controls.Add(this.cboValue);
            this.bcpnlCombo.Controls.Add(this.lblTitle);
            this.bcpnlCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcpnlCombo.DownImage = null;
            this.bcpnlCombo.Key = "TitlePanel";
            this.bcpnlCombo.LeftImage = null;
            this.bcpnlCombo.LeftTopImage = null;
            this.bcpnlCombo.Location = new System.Drawing.Point(0, 0);
            this.bcpnlCombo.Name = "bcpnlCombo";
            this.bcpnlCombo.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.bcpnlCombo.RightBottomImage = null;
            this.bcpnlCombo.RightImage = null;
            this.bcpnlCombo.Size = new System.Drawing.Size(315, 27);
            this.bcpnlCombo.TabIndex = 4;
            this.bcpnlCombo.TopImage = null;
            this.bcpnlCombo.TopRightImage = null;
            this.bcpnlCombo.UpImage = null;
            // 
            // ComboControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bcpnlCombo);
            this.Name = "ComboControl";
            this.Size = new System.Drawing.Size(315, 27);
            this.bcpnlCombo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox cboValue;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblTitle;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlCombo;
    }
}
