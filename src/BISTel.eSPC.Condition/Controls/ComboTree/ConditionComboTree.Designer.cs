using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;

namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    partial class ConditionComboTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionComboTree));
            //this.bTreeCombo1 = new BISTel.PeakPerformance.Client.BISTelControl.BConditionTree.BTreeCombo();
            this.btpnlTitle = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.btpnlTitle.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTreeCombo1
            // 
            this.bTreeCombo1.BackColor = System.Drawing.Color.White;
            this.bTreeCombo1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bTreeCombo1.DropDownSize = new System.Drawing.Size(135, 200);
            this.bTreeCombo1.DrowDownHeigt = 200;
            this.bTreeCombo1.DrowDownWidth = 135;
            this.bTreeCombo1.Location = new System.Drawing.Point(0, 0);
            this.bTreeCombo1.Name = "bTreeCombo1";
            this.bTreeCombo1.Size = new System.Drawing.Size(343, 21);
            this.bTreeCombo1.TabIndex = 3;
            this.bTreeCombo1.TextDelimiter = ":";
            this.bTreeCombo1.TextViewType = BISTel.PeakPerformance.Client.BISTelControl.BConditionTree.BTreeCombo.TextViewTypes.CheckedNode;
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
            this.btpnlTitle.Size = new System.Drawing.Size(345, 54);
            this.btpnlTitle.TabIndex = 4;
            this.btpnlTitle.Title = "Title";
            this.btpnlTitle.TitleColor = System.Drawing.Color.Black;
            this.btpnlTitle.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btpnlTitle.TopPadding = 5;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.Transparent;
            this.pnlBody.Controls.Add(this.bTreeCombo1);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(1, 28);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(343, 25);
            this.pnlBody.TabIndex = 2;
            // 
            // ConditionComboTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btpnlTitle);
            this.Name = "ConditionComboTree";
            this.Size = new System.Drawing.Size(345, 54);
            this.btpnlTitle.ResumeLayout(false);
            this.btpnlTitle.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BConditionTree.BTreeCombo bTreeCombo1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel btpnlTitle;
        public System.Windows.Forms.Panel pnlBody;

    }
}
