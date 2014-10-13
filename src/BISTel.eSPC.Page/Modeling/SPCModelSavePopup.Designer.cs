namespace BISTel.eSPC.Page.Modeling
{
    partial class SPCModelSavePopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCModelSavePopup));
            this.blblMain = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblCopy = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblSub = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.btxtComment = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bbtnYes = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.bbtnNo = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bpnlMain = new System.Windows.Forms.Panel();
            this.bpnlCopy = new System.Windows.Forms.Panel();
            this.bpnlSub = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.bpnlTop = new System.Windows.Forms.Panel();
            this.btxtChangedItems = new System.Windows.Forms.TextBox();
            this.blblChanged = new System.Windows.Forms.Label();
            this.pnlContentsArea.SuspendLayout();
            this.bArrangePanel1.SuspendLayout();
            this.bTitlePanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.bpnlMain.SuspendLayout();
            this.bpnlCopy.SuspendLayout();
            this.bpnlSub.SuspendLayout();
            this.bpnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.bArrangePanel1);
            this.pnlContentsArea.Controls.Add(this.panel1);
            this.pnlContentsArea.Controls.Add(this.bpnlMain);
            this.pnlContentsArea.Controls.Add(this.bpnlCopy);
            this.pnlContentsArea.Controls.Add(this.bpnlSub);
            this.pnlContentsArea.Controls.Add(this.panel6);
            this.pnlContentsArea.Controls.Add(this.bpnlTop);
            this.pnlContentsArea.Size = new System.Drawing.Size(540, 267);
            // 
            // blblMain
            // 
            this.blblMain.BackColor = System.Drawing.Color.Transparent;
            this.blblMain.BssClass = "";
            this.blblMain.CustomTextAlign = "";
            this.blblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblMain.ForeColor = System.Drawing.Color.Black;
            this.blblMain.IsMultiLanguage = true;
            this.blblMain.Key = "";
            this.blblMain.Location = new System.Drawing.Point(0, 0);
            this.blblMain.Name = "blblMain";
            this.blblMain.Size = new System.Drawing.Size(534, 40);
            this.blblMain.TabIndex = 0;
            this.blblMain.Text = "Yes: The Added(Modified) Grouping Context(Exclude List) of Main Configuration wil" +
                "l be applied to each Sub Configuration also.\r\nNo: Only Main Configuration will b" +
                "e changed.\r\nCancel: Cancel Save.\r\n";
            // 
            // blblCopy
            // 
            this.blblCopy.BackColor = System.Drawing.Color.Transparent;
            this.blblCopy.BssClass = "";
            this.blblCopy.CustomTextAlign = "";
            this.blblCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblCopy.ForeColor = System.Drawing.Color.Black;
            this.blblCopy.IsMultiLanguage = true;
            this.blblCopy.Key = "";
            this.blblCopy.Location = new System.Drawing.Point(0, 0);
            this.blblCopy.Name = "blblCopy";
            this.blblCopy.Size = new System.Drawing.Size(534, 24);
            this.blblCopy.TabIndex = 0;
            this.blblCopy.Text = "Are you sure to copy selected configuration?";
            // 
            // blblSub
            // 
            this.blblSub.BackColor = System.Drawing.Color.Transparent;
            this.blblSub.BssClass = "";
            this.blblSub.CustomTextAlign = "";
            this.blblSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blblSub.ForeColor = System.Drawing.Color.Black;
            this.blblSub.IsMultiLanguage = true;
            this.blblSub.Key = "";
            this.blblSub.Location = new System.Drawing.Point(0, 0);
            this.blblSub.Name = "blblSub";
            this.blblSub.Size = new System.Drawing.Size(534, 24);
            this.blblSub.TabIndex = 0;
            this.blblSub.Text = "Do you want to save?";
            // 
            // bArrangePanel1
            // 
            this.bArrangePanel1.BLabelAutoPadding = true;
            this.bArrangePanel1.BssClass = "";
            this.bArrangePanel1.Controls.Add(this.bTitlePanel1);
            this.bArrangePanel1.Controls.Add(this.panel5);
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bArrangePanel1.IsCondition = true;
            this.bArrangePanel1.Key = "";
            this.bArrangePanel1.Location = new System.Drawing.Point(3, 128);
            this.bArrangePanel1.Name = "bArrangePanel1";
            this.bArrangePanel1.Padding_Bottom = 5;
            this.bArrangePanel1.Padding_Left = 5;
            this.bArrangePanel1.Padding_Right = 5;
            this.bArrangePanel1.Padding_Top = 5;
            this.bArrangePanel1.Size = new System.Drawing.Size(534, 113);
            this.bArrangePanel1.Space = 5;
            this.bArrangePanel1.TabIndex = 3;
            this.bArrangePanel1.Title = "";
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.btxtComment);
            this.bTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitlePanel1.ImageBottom = null;
            this.bTitlePanel1.ImageBottomRight = null;
            this.bTitlePanel1.ImageClose = null;
            this.bTitlePanel1.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageLeft")));
            this.bTitlePanel1.ImageLeftBottom = null;
            this.bTitlePanel1.ImageLeftFill = null;
            this.bTitlePanel1.ImageMax = null;
            this.bTitlePanel1.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageMiddle")));
            this.bTitlePanel1.ImageMin = null;
            this.bTitlePanel1.ImagePanelMax = null;
            this.bTitlePanel1.ImagePanelMin = null;
            this.bTitlePanel1.ImagePanelNormal = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.MaxVisibleStateForMaxNormal = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.SelectedColor = System.Drawing.Color.Blue;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.ShowMaxNormalButton = false;
            this.bTitlePanel1.ShowMinButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(534, 105);
            this.bTitlePanel1.TabIndex = 0;
            this.bTitlePanel1.Title = "Comment";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.Black;
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // btxtComment
            // 
            this.btxtComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btxtComment.Location = new System.Drawing.Point(0, 32);
            this.btxtComment.MaxLength = 256;
            this.btxtComment.Multiline = true;
            this.btxtComment.Name = "btxtComment";
            this.btxtComment.Size = new System.Drawing.Size(534, 73);
            this.btxtComment.TabIndex = 7;
            this.btxtComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btxtComment_KeyPress);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 105);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(534, 8);
            this.panel5.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bbtnYes);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.bbtnNo);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.bbtnCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 241);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 23);
            this.panel1.TabIndex = 4;
            // 
            // bbtnYes
            // 
            this.bbtnYes.AutoButtonSize = false;
            this.bbtnYes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnYes.BackgroundImage")));
            this.bbtnYes.BssClass = "ConditionButton";
            this.bbtnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bbtnYes.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnYes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnYes.IsMultiLanguage = true;
            this.bbtnYes.Key = "";
            this.bbtnYes.Location = new System.Drawing.Point(270, 0);
            this.bbtnYes.Name = "bbtnYes";
            this.bbtnYes.Size = new System.Drawing.Size(80, 23);
            this.bbtnYes.TabIndex = 4;
            this.bbtnYes.Text = "Yes";
            this.bbtnYes.ToolTipText = "";
            this.bbtnYes.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(350, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(8, 23);
            this.panel4.TabIndex = 3;
            // 
            // bbtnNo
            // 
            this.bbtnNo.AutoButtonSize = false;
            this.bbtnNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnNo.BackgroundImage")));
            this.bbtnNo.BssClass = "ConditionButton";
            this.bbtnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.bbtnNo.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnNo.IsMultiLanguage = true;
            this.bbtnNo.Key = "";
            this.bbtnNo.Location = new System.Drawing.Point(358, 0);
            this.bbtnNo.Name = "bbtnNo";
            this.bbtnNo.Size = new System.Drawing.Size(80, 23);
            this.bbtnNo.TabIndex = 3;
            this.bbtnNo.Text = "No";
            this.bbtnNo.ToolTipText = "";
            this.bbtnNo.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(438, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 23);
            this.panel3.TabIndex = 2;
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.AutoButtonSize = false;
            this.bbtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCancel.BackgroundImage")));
            this.bbtnCancel.BssClass = "ConditionButton";
            this.bbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bbtnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bbtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCancel.IsMultiLanguage = true;
            this.bbtnCancel.Key = "";
            this.bbtnCancel.Location = new System.Drawing.Point(446, 0);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 23);
            this.bbtnCancel.TabIndex = 1;
            this.bbtnCancel.Text = "Cancel";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(526, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(8, 23);
            this.panel2.TabIndex = 0;
            // 
            // bpnlMain
            // 
            this.bpnlMain.Controls.Add(this.blblMain);
            this.bpnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.bpnlMain.Location = new System.Drawing.Point(3, 88);
            this.bpnlMain.Name = "bpnlMain";
            this.bpnlMain.Size = new System.Drawing.Size(534, 40);
            this.bpnlMain.TabIndex = 5;
            // 
            // bpnlCopy
            // 
            this.bpnlCopy.Controls.Add(this.blblCopy);
            this.bpnlCopy.Dock = System.Windows.Forms.DockStyle.Top;
            this.bpnlCopy.Location = new System.Drawing.Point(3, 64);
            this.bpnlCopy.Name = "bpnlCopy";
            this.bpnlCopy.Size = new System.Drawing.Size(534, 24);
            this.bpnlCopy.TabIndex = 6;
            // 
            // bpnlSub
            // 
            this.bpnlSub.Controls.Add(this.blblSub);
            this.bpnlSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.bpnlSub.Location = new System.Drawing.Point(3, 40);
            this.bpnlSub.Name = "bpnlSub";
            this.bpnlSub.Size = new System.Drawing.Size(534, 24);
            this.bpnlSub.TabIndex = 7;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 35);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(534, 5);
            this.panel6.TabIndex = 0;
            // 
            // bpnlTop
            // 
            this.bpnlTop.Controls.Add(this.btxtChangedItems);
            this.bpnlTop.Controls.Add(this.blblChanged);
            this.bpnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.bpnlTop.Location = new System.Drawing.Point(3, 3);
            this.bpnlTop.Name = "bpnlTop";
            this.bpnlTop.Size = new System.Drawing.Size(534, 32);
            this.bpnlTop.TabIndex = 8;
            // 
            // btxtChangedItems
            // 
            this.btxtChangedItems.Location = new System.Drawing.Point(96, 5);
            this.btxtChangedItems.Name = "btxtChangedItems";
            this.btxtChangedItems.ReadOnly = true;
            this.btxtChangedItems.Size = new System.Drawing.Size(416, 21);
            this.btxtChangedItems.TabIndex = 1;
            // 
            // blblChanged
            // 
            this.blblChanged.AutoSize = true;
            this.blblChanged.Location = new System.Drawing.Point(1, 8);
            this.blblChanged.Name = "blblChanged";
            this.blblChanged.Size = new System.Drawing.Size(87, 13);
            this.blblChanged.TabIndex = 0;
            this.blblChanged.Text = "Changed Items :";
            // 
            // SPCModelSavePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 300);
            this.MaximumSize = new System.Drawing.Size(552, 300);
            this.MinimumSize = new System.Drawing.Size(552, 250);
            this.Name = "SPCModelSavePopup";
            this.Text = "SPCModelSavePopup";
            this.pnlContentsArea.ResumeLayout(false);
            this.bArrangePanel1.ResumeLayout(false);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.bpnlMain.ResumeLayout(false);
            this.bpnlCopy.ResumeLayout(false);
            this.bpnlSub.ResumeLayout(false);
            this.bpnlTop.ResumeLayout(false);
            this.bpnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PeakPerformance.Client.BISTelControl.BArrangePanel bArrangePanel1;
        private PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private PeakPerformance.Client.BISTelControl.BButton bbtnYes;
        private System.Windows.Forms.Panel panel4;
        private PeakPerformance.Client.BISTelControl.BButton bbtnNo;
        private System.Windows.Forms.Panel panel3;
        private PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private System.Windows.Forms.Panel panel2;
        private PeakPerformance.Client.BISTelControl.BLabel blblMain;
        private PeakPerformance.Client.BISTelControl.BLabel blblSub;
        private PeakPerformance.Client.BISTelControl.BLabel blblCopy;
        private System.Windows.Forms.Panel bpnlSub;
        private System.Windows.Forms.Panel bpnlCopy;
        private System.Windows.Forms.Panel bpnlMain;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel bpnlTop;
        private System.Windows.Forms.TextBox btxtComment;
        private System.Windows.Forms.TextBox btxtChangedItems;
        private System.Windows.Forms.Label blblChanged;
    }
}