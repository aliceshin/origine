namespace BISTel.eSPC.Page.Common.Popup
{
    partial class ChartViewSelectedPopup
    {
        /// <summary>
        /// Required designer ChartVariable.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartViewSelectedPopup));
            this.pnlAutoCalculationPopup = new System.Windows.Forms.Panel();
            this.bArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnSave = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.pnlContentsArea.SuspendLayout();
            this.pnlAutoCalculationPopup.SuspendLayout();
            this.bapnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnlAutoCalculationPopup);
            this.pnlContentsArea.Size = new System.Drawing.Size(363, 151);
            // 
            // pnlAutoCalculationPopup
            // 
            this.pnlAutoCalculationPopup.Controls.Add(this.bArrangePanel1);
            this.pnlAutoCalculationPopup.Controls.Add(this.bapnlButton);
            this.pnlAutoCalculationPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAutoCalculationPopup.Location = new System.Drawing.Point(3, 3);
            this.pnlAutoCalculationPopup.Name = "pnlAutoCalculationPopup";
            this.pnlAutoCalculationPopup.Size = new System.Drawing.Size(357, 145);
            this.pnlAutoCalculationPopup.TabIndex = 14;
            // 
            // bArrangePanel1
            // 
            this.bArrangePanel1.BLabelAutoPadding = true;
            this.bArrangePanel1.BssClass = "DataArrPanel";
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bArrangePanel1.IsCondition = true;
            this.bArrangePanel1.Key = "";
            this.bArrangePanel1.Location = new System.Drawing.Point(0, 0);
            this.bArrangePanel1.Name = "bArrangePanel1";
            this.bArrangePanel1.Padding_Bottom = 5;
            this.bArrangePanel1.Padding_Left = 5;
            this.bArrangePanel1.Padding_Right = 5;
            this.bArrangePanel1.Padding_Top = 5;
            this.bArrangePanel1.Size = new System.Drawing.Size(357, 111);
            this.bArrangePanel1.Space = 5;
            this.bArrangePanel1.TabIndex = 5;
            this.bArrangePanel1.Title = "";
            // 
            // bapnlButton
            // 
            this.bapnlButton.BLabelAutoPadding = true;
            this.bapnlButton.BssClass = "";
            this.bapnlButton.Controls.Add(this.bbtnCancel);
            this.bapnlButton.Controls.Add(this.bbtnSave);
            this.bapnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlButton.IsCondition = false;
            this.bapnlButton.Key = "";
            this.bapnlButton.Location = new System.Drawing.Point(0, 111);
            this.bapnlButton.Name = "bapnlButton";
            this.bapnlButton.Padding_Bottom = 5;
            this.bapnlButton.Padding_Left = 5;
            this.bapnlButton.Padding_Right = 5;
            this.bapnlButton.Padding_Top = 5;
            this.bapnlButton.Size = new System.Drawing.Size(357, 34);
            this.bapnlButton.Space = 5;
            this.bapnlButton.TabIndex = 4;
            this.bapnlButton.Title = "";
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
            this.bbtnCancel.Key = "FDC_POPUP_BUTTON_CANCEL";
            this.bbtnCancel.Location = new System.Drawing.Point(277, 7);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(72, 20);
            this.bbtnCancel.TabIndex = 3;
            this.bbtnCancel.Text = "Cancel";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // bbtnSave
            // 
            this.bbtnSave.AutoButtonSize = false;
            this.bbtnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnSave.BackgroundImage")));
            this.bbtnSave.BssClass = "ConditionButton";
            this.bbtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnSave.IsMultiLanguage = true;
            this.bbtnSave.Key = "FDC_POPUP_BUTTON_OK";
            this.bbtnSave.Location = new System.Drawing.Point(200, 7);
            this.bbtnSave.Name = "bbtnSave";
            this.bbtnSave.Size = new System.Drawing.Size(72, 20);
            this.bbtnSave.TabIndex = 2;
            this.bbtnSave.Text = "Save";
            this.bbtnSave.ToolTipText = "";
            this.bbtnSave.UseVisualStyleBackColor = true;
            this.bbtnSave.Click += new System.EventHandler(this.bbtnSave_Click);
            // 
            // ChartViewSelectedPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 184);
            this.ContentsAreaMinHeight = 145;
            this.ContentsAreaMinWidth = 357;
            this.Name = "ChartViewSelectedPopup";
            this.Text = "ChartViewSelectedPopup";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnlAutoCalculationPopup.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlAutoCalculationPopup;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnSave;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bArrangePanel1;
    }
}