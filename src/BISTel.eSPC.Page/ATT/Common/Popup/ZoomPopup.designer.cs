namespace BISTel.eSPC.Page.ATT.Common
{
    partial class ZoomPopup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Base = new System.Windows.Forms.Panel();
            this.pnl_Combobox = new System.Windows.Forms.Panel();
            this.bcb_Kind = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.pnl_Label = new System.Windows.Forms.Panel();
            this.blb_Kind = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblbullet01 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnl_Space = new System.Windows.Forms.Panel();
            this.pnlContentsArea.SuspendLayout();
            this.pnl_Base.SuspendLayout();
            this.pnl_Combobox.SuspendLayout();
            this.pnl_Label.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnl_Base);
            this.pnlContentsArea.Padding = new System.Windows.Forms.Padding(5);
            this.pnlContentsArea.Size = new System.Drawing.Size(270, 40);
            // 
            // pnl_Base
            // 
            this.pnl_Base.Controls.Add(this.pnl_Combobox);
            this.pnl_Base.Controls.Add(this.pnl_Label);
            this.pnl_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Base.Location = new System.Drawing.Point(5, 5);
            this.pnl_Base.Name = "pnl_Base";
            this.pnl_Base.Size = new System.Drawing.Size(260, 30);
            this.pnl_Base.TabIndex = 0;
            // 
            // pnl_Combobox
            // 
            this.pnl_Combobox.Controls.Add(this.bcb_Kind);
            this.pnl_Combobox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Combobox.Location = new System.Drawing.Point(53, 0);
            this.pnl_Combobox.Name = "pnl_Combobox";
            this.pnl_Combobox.Size = new System.Drawing.Size(207, 30);
            this.pnl_Combobox.TabIndex = 1;
            // 
            // bcb_Kind
            // 
            this.bcb_Kind.BssClass = "";
            this.bcb_Kind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcb_Kind.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcb_Kind.FormattingEnabled = true;
            this.bcb_Kind.IsMultiLanguage = true;
            this.bcb_Kind.Items.AddRange(new object[] {
            "Vertical",
            "Horizontal",
            "Both"});
            this.bcb_Kind.Key = "";
            this.bcb_Kind.Location = new System.Drawing.Point(0, 0);
            this.bcb_Kind.Name = "bcb_Kind";
            this.bcb_Kind.Size = new System.Drawing.Size(207, 21);
            this.bcb_Kind.TabIndex = 0;
            this.bcb_Kind.SelectedValueChanged += new System.EventHandler(this.bcb_Kind_SelectedValueChanged);
            // 
            // pnl_Label
            // 
            this.pnl_Label.Controls.Add(this.blb_Kind);
            this.pnl_Label.Controls.Add(this.blblbullet01);
            this.pnl_Label.Controls.Add(this.pnl_Space);
            this.pnl_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_Label.Location = new System.Drawing.Point(0, 0);
            this.pnl_Label.Name = "pnl_Label";
            this.pnl_Label.Size = new System.Drawing.Size(53, 30);
            this.pnl_Label.TabIndex = 0;
            // 
            // blb_Kind
            // 
            this.blb_Kind.BssClass = "";
            this.blb_Kind.CustomTextAlign = "";
            this.blb_Kind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blb_Kind.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blb_Kind.IsMultiLanguage = true;
            this.blb_Kind.Key = "";
            this.blb_Kind.Location = new System.Drawing.Point(10, 5);
            this.blb_Kind.Name = "blb_Kind";
            this.blb_Kind.Size = new System.Drawing.Size(43, 25);
            this.blb_Kind.TabIndex = 1;
            this.blb_Kind.Text = "Kind :";
            // 
            // blblbullet01
            // 
            this.blblbullet01.BackColor = System.Drawing.Color.Transparent;
            this.blblbullet01.BssClass = "";
            this.blblbullet01.CustomTextAlign = "";
            this.blblbullet01.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblbullet01.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblbullet01.ForeColor = System.Drawing.Color.Black;
            this.blblbullet01.Image = global::BISTel.eSPC.Page.Properties.Resources.bullet_01;
            this.blblbullet01.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.blblbullet01.IsMultiLanguage = true;
            this.blblbullet01.Key = "";
            this.blblbullet01.Location = new System.Drawing.Point(0, 5);
            this.blblbullet01.Name = "blblbullet01";
            this.blblbullet01.Size = new System.Drawing.Size(10, 25);
            this.blblbullet01.TabIndex = 7;
            // 
            // pnl_Space
            // 
            this.pnl_Space.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Space.Location = new System.Drawing.Point(0, 0);
            this.pnl_Space.Name = "pnl_Space";
            this.pnl_Space.Size = new System.Drawing.Size(53, 5);
            this.pnl_Space.TabIndex = 0;
            // 
            // ZoomPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 73);
            this.ContentsAreaMinHeight = 30;
            this.ContentsAreaMinWidth = 260;
            this.Name = "ZoomPopup";
            this.Text = "ZoomPopup";
            this.Title = "Zoom Style";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnl_Base.ResumeLayout(false);
            this.pnl_Combobox.ResumeLayout(false);
            this.pnl_Label.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Base;
        private System.Windows.Forms.Panel pnl_Combobox;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcb_Kind;
        private System.Windows.Forms.Panel pnl_Label;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blb_Kind;
        private System.Windows.Forms.Panel pnl_Space;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblbullet01;
    }
}