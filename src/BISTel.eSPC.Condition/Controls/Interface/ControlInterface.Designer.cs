namespace BISTel.eSPC.Condition.Controls.Interface
{
    partial class ControlInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlInterface));
            this.btpnlTitle = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.btpnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // btpnlTitle
            // 
            this.btpnlTitle.BackColor = System.Drawing.Color.White;
            this.btpnlTitle.BssClass = "TitlePanel";
            this.btpnlTitle.Controls.Add(this.pnlBody);
            this.btpnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btpnlTitle.ImageBottom = null;
            this.btpnlTitle.ImageBottomRight = null;
            this.btpnlTitle.ImageLeft = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageLeft")));
            this.btpnlTitle.ImageLeftBottom = null;
            this.btpnlTitle.ImageLeftFill = null;
            this.btpnlTitle.ImageMax = null;
            this.btpnlTitle.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageMiddle")));
            this.btpnlTitle.ImageMin = null;
            this.btpnlTitle.ImageRight = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageRight")));
            this.btpnlTitle.ImageRightFill = null;
            this.btpnlTitle.Key = "";
            this.btpnlTitle.Location = new System.Drawing.Point(0, 0);
            this.btpnlTitle.MaxControlName = "";
            this.btpnlTitle.MaxResizable = false;
            this.btpnlTitle.Name = "btpnlTitle";
            this.btpnlTitle.Padding = new System.Windows.Forms.Padding(1, 28, 1, 1);
            this.btpnlTitle.PaddingTop = 4;
            this.btpnlTitle.RightPadding = 5;
            this.btpnlTitle.Size = new System.Drawing.Size(341, 279);
            this.btpnlTitle.TabIndex = 0;
            this.btpnlTitle.Title = "Title";
            this.btpnlTitle.TitleColor = System.Drawing.Color.Black;
            this.btpnlTitle.TopPadding = 5;
            this.btpnlTitle.Click += new System.EventHandler(this.btpnlTitle_Click);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.Transparent;
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(1, 28);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(339, 250);
            this.pnlBody.TabIndex = 2;
            // 
            // ControlInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btpnlTitle);
            this.Name = "ControlInterface";
            this.Size = new System.Drawing.Size(341, 279);
            this.Load += new System.EventHandler(this.ControlInterface_Load);
            this.btpnlTitle.ResumeLayout(false);
            this.btpnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel btpnlTitle;
        public System.Windows.Forms.Panel pnlBody;
    }
}
