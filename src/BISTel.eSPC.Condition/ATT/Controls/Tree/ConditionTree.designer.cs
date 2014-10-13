namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    partial class ConditionTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionTree));
            //this.bTreeView1 = new BISTel.PeakPerformance.Client.BISTelControl.BTreeView(this.components);
            this.pnlBody = new System.Windows.Forms.Panel();
            this.btpnlTitle = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.pnlBody.SuspendLayout();
            this.btpnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTreeView1
            // 
            this.bTreeView1.CheckCountType = BISTel.PeakPerformance.Client.BISTelControl.BTreeView.CheckCountTypes.Multi;
            this.bTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTreeView1.Location = new System.Drawing.Point(0, 0);
            this.bTreeView1.Name = "bTreeView1";
            this.bTreeView1.Size = new System.Drawing.Size(148, 121);
            this.bTreeView1.TabIndex = 0;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.Transparent;
            this.pnlBody.Controls.Add(this.bTreeView1);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(1, 28);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(148, 121);
            this.pnlBody.TabIndex = 2;
            // 
            // btpnlTitle
            // 
            this.btpnlTitle.BackColor = System.Drawing.Color.White;
            this.btpnlTitle.BssClass = "TitlePanel";
            this.btpnlTitle.Controls.Add(this.pnlBody);
            this.btpnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btpnlTitle.ImageBottom = null;
            this.btpnlTitle.ImageBottomRight = null;
            this.btpnlTitle.ImageClose = null;
            this.btpnlTitle.ImageLeft = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageLeft")));
            this.btpnlTitle.ImageLeftBottom = null;
            this.btpnlTitle.ImageLeftFill = null;
            this.btpnlTitle.ImageMax = null;
            this.btpnlTitle.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageMiddle")));
            this.btpnlTitle.ImageMin = null;
            this.btpnlTitle.ImagePanelMax = null;
            this.btpnlTitle.ImagePanelMin = null;
            this.btpnlTitle.ImagePanelNormal = null;
            this.btpnlTitle.ImageRight = ((System.Drawing.Image)(resources.GetObject("btpnlTitle.ImageRight")));
            this.btpnlTitle.ImageRightFill = null;
            this.btpnlTitle.IsPopupMax = false;
            this.btpnlTitle.IsSelected = false;
            this.btpnlTitle.Key = "";
            this.btpnlTitle.Location = new System.Drawing.Point(0, 0);
            this.btpnlTitle.MaxControlName = "";
            this.btpnlTitle.MaxResizable = false;
            this.btpnlTitle.MaxVisibleStateForMaxNormal = false;
            this.btpnlTitle.Name = "btpnlTitle";
            this.btpnlTitle.Padding = new System.Windows.Forms.Padding(1, 28, 1, 1);
            this.btpnlTitle.PaddingTop = 4;
            this.btpnlTitle.PopMaxHeight = 0;
            this.btpnlTitle.PopMaxWidth = 0;
            this.btpnlTitle.RightPadding = 5;
            this.btpnlTitle.ShowCloseButton = false;
            this.btpnlTitle.ShowMaxNormalButton = false;
            this.btpnlTitle.ShowMinButton = false;
            this.btpnlTitle.Size = new System.Drawing.Size(150, 150);
            this.btpnlTitle.TabIndex = 1;
            this.btpnlTitle.Title = "Title";
            this.btpnlTitle.TitleColor = System.Drawing.Color.Black;
            this.btpnlTitle.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btpnlTitle.TopPadding = 5;
            // 
            // ConditionTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btpnlTitle);
            this.Name = "ConditionTree";
            this.pnlBody.ResumeLayout(false);
            this.btpnlTitle.ResumeLayout(false);
            this.btpnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected BISTel.PeakPerformance.Client.BISTelControl.BTreeView bTreeView1;
        public System.Windows.Forms.Panel pnlBody;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel btpnlTitle;
    }
}
