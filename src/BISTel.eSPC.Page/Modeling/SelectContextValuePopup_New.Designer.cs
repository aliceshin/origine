namespace BISTel.eSPC.Page.Modeling
{
    partial class SelectContextValuePopup_New
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectContextValuePopup_New));
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlBottom = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bTabControl1 = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.pnlContentsArea.SuspendLayout();
            this.bapnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.bTabControl1);
            this.pnlContentsArea.Controls.Add(this.bapnlBottom);
            this.pnlContentsArea.Size = new System.Drawing.Size(556, 326);
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.AutoButtonSize = false;
            this.bbtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCancel.BackgroundImage")));
            this.bbtnCancel.BssClass = "ConditionButton";
            this.bbtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCancel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCancel.IsMultiLanguage = true;
            this.bbtnCancel.Key = "SPC_BUTTON_CANCEL";
            this.bbtnCancel.Location = new System.Drawing.Point(462, 6);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(80, 25);
            this.bbtnCancel.TabIndex = 1;
            this.bbtnCancel.TabStop = false;
            this.bbtnCancel.Text = "CANCEL";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bbtnOK
            // 
            this.bbtnOK.AutoButtonSize = false;
            this.bbtnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnOK.BackgroundImage")));
            this.bbtnOK.BssClass = "ConditionButton";
            this.bbtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnOK.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnOK.IsMultiLanguage = true;
            this.bbtnOK.Key = "SPC_BUTTON_OK";
            this.bbtnOK.Location = new System.Drawing.Point(377, 6);
            this.bbtnOK.Name = "bbtnOK";
            this.bbtnOK.Size = new System.Drawing.Size(80, 25);
            this.bbtnOK.TabIndex = 0;
            this.bbtnOK.TabStop = false;
            this.bbtnOK.Text = "OK";
            this.bbtnOK.ToolTipText = "";
            this.bbtnOK.UseVisualStyleBackColor = true;
            this.bbtnOK.Click += new System.EventHandler(this.bbtnOK_Click);
            // 
            // bapnlBottom
            // 
            this.bapnlBottom.BLabelAutoPadding = true;
            this.bapnlBottom.BssClass = "DataArrPanel";
            this.bapnlBottom.Controls.Add(this.bbtnOK);
            this.bapnlBottom.Controls.Add(this.bbtnCancel);
            this.bapnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlBottom.IsCondition = false;
            this.bapnlBottom.Key = "";
            this.bapnlBottom.Location = new System.Drawing.Point(3, 286);
            this.bapnlBottom.Name = "bapnlBottom";
            this.bapnlBottom.Padding_Bottom = 5;
            this.bapnlBottom.Padding_Left = 5;
            this.bapnlBottom.Padding_Right = 5;
            this.bapnlBottom.Padding_Top = 5;
            this.bapnlBottom.Size = new System.Drawing.Size(550, 37);
            this.bapnlBottom.Space = 5;
            this.bapnlBottom.TabIndex = 4;
            this.bapnlBottom.Title = "";
            // 
            // bTabControl1
            // 
            this.bTabControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(161)))), ((int)(((byte)(184)))));
            this.bTabControl1.BssClass = "DataMainTab";
            this.bTabControl1.CloseImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseImage")));
            this.bTabControl1.CloseRollOverImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseRollOverImage")));
            this.bTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bTabControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTabControl1.FontSizeSet = "11";
            this.bTabControl1.IsDrawBorder = true;
            this.bTabControl1.IsFillBorder = false;
            this.bTabControl1.IsSpace = true;
            this.bTabControl1.Key = "";
            this.bTabControl1.Location = new System.Drawing.Point(3, 3);
            this.bTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.bTabControl1.Name = "bTabControl1";
            this.bTabControl1.SelectedIndex = 0;
            this.bTabControl1.Size = new System.Drawing.Size(550, 283);
            this.bTabControl1.TabBackColor = System.Drawing.Color.White;
            this.bTabControl1.TabBackImage = null;
            this.bTabControl1.TabIndex = 5;
            this.bTabControl1.TabPageClose = false;
            this.bTabControl1.TabSelectFontColor = System.Drawing.Color.White;
            this.bTabControl1.TabSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectLeftImage")));
            this.bTabControl1.TabSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectMiddleImage")));
            this.bTabControl1.TabSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectRightImage")));
            this.bTabControl1.TabSelectRollOverFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectLeftImage")));
            this.bTabControl1.TabUnSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectMiddleImage")));
            this.bTabControl1.TabUnSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectRightImage")));
            this.bTabControl1.TabUnSelectRollOverFontColor = System.Drawing.Color.White;
            this.bTabControl1.TextLeftPosition = 5;
            this.bTabControl1.UseShadowFont = false;
            // 
            // SelectContextValuePopup_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 359);
            this.ContentsAreaMinHeight = 320;
            this.ContentsAreaMinWidth = 550;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "SelectContextValuePopup_New";
            this.Resizable = true;
            this.Text = "Set Context Value";
            this.Title = "Set Context Value";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlBottom;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprParam;
        private BISTel.PeakPerformance.Client.BISTelControl.BTabControl bTabControl1;
    }
}