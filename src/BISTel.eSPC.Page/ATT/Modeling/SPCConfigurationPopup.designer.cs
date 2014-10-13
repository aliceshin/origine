namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class SPCConfigurationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCConfigurationPopup));
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bbtnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bapnlBottom = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            // [SPC-658]
            //this.spcConfigurationUC = new BISTel.eSPC.Page.Modeling.SPCConfigurationUC();
            this.spcConfigurationUC = new BISTel.eSPC.Page.ATT.Modeling.SPCConfigurationUC(this._isDefaultModel);
            this.pnlContentsArea.SuspendLayout();
            this.bapnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.spcConfigurationUC);
            this.pnlContentsArea.Controls.Add(this.bapnlBottom);
            this.pnlContentsArea.Size = new System.Drawing.Size(806, 606);
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
            this.bbtnCancel.Location = new System.Drawing.Point(712, 6);
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
            this.bbtnOK.Key = "SPC_BUTTON_SAVE";
            this.bbtnOK.Location = new System.Drawing.Point(627, 6);
            this.bbtnOK.Name = "bbtnOK";
            this.bbtnOK.Size = new System.Drawing.Size(80, 25);
            this.bbtnOK.TabIndex = 0;
            this.bbtnOK.TabStop = false;
            this.bbtnOK.Text = "SAVE";
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
            this.bapnlBottom.Location = new System.Drawing.Point(3, 566);
            this.bapnlBottom.Name = "bapnlBottom";
            this.bapnlBottom.Padding_Bottom = 5;
            this.bapnlBottom.Padding_Left = 5;
            this.bapnlBottom.Padding_Right = 5;
            this.bapnlBottom.Padding_Top = 5;
            this.bapnlBottom.Size = new System.Drawing.Size(800, 37);
            this.bapnlBottom.Space = 5;
            this.bapnlBottom.TabIndex = 4;
            this.bapnlBottom.Title = "";
            // 
            // spcConfigurationUC
            // 
            this.spcConfigurationUC.ApplicationName = "";
            this.spcConfigurationUC.AREA_RAWID = null;
            this.spcConfigurationUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcConfigurationUC.BackColorHtml = "#DCDCDC";
            this.spcConfigurationUC.BssClass = null;
            this.spcConfigurationUC.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_SUB;
            this.spcConfigurationUC.CONFIG_RAWID = null;
            this.spcConfigurationUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcConfigurationUC.EQP_MODEL = null;
            this.spcConfigurationUC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcConfigurationUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcConfigurationUC.ForeColorHtml = "#000000";
            this.spcConfigurationUC.FunctionName = "";
            this.spcConfigurationUC.LINE_RAWID = null;
            this.spcConfigurationUC.Location = new System.Drawing.Point(3, 3);
            this.spcConfigurationUC.MAIN_YN = null;
            this.spcConfigurationUC.ModelConfigCondition = ((BISTel.PeakPerformance.Client.CommonLibrary.LinkedList)(resources.GetObject("spcConfigurationUC.ModelConfigCondition")));
            this.spcConfigurationUC.Name = "spcConfigurationUC";
            this.spcConfigurationUC.sessionData = null;
            this.spcConfigurationUC.Size = new System.Drawing.Size(800, 563);
            this.spcConfigurationUC.TabIndex = 5;
            this.spcConfigurationUC.Title = null;
            this.spcConfigurationUC.URL = "";
            // 
            // SPCConfigurationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 639);
            this.ContentsAreaMinHeight = 600;
            this.ContentsAreaMinWidth = 800;
            this.Name = "SPCConfigurationPopup";
            this.Resizable = true;
            this.Text = "SPC Configuration";
            this.Title = "SPC Configuration";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlBottom;
        private SPCConfigurationUC spcConfigurationUC;
    }
}