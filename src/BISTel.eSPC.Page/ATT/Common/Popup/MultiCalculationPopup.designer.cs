namespace BISTel.eSPC.Page.ATT.Common.Popup
{
    partial class MultiCalculationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiCalculationPopup));
            this.pnlDefaultSettingPopup = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.spcMultiCalcUC = new BISTel.eSPC.Page.ATT.Modeling.SPCMultiCalculationUC();
            this.pnlContentsArea.SuspendLayout();
            this.pnlDefaultSettingPopup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.BackColor = System.Drawing.Color.Transparent;
            this.pnlContentsArea.Controls.Add(this.pnlDefaultSettingPopup);
            this.pnlContentsArea.Size = new System.Drawing.Size(1114, 736);
            // 
            // pnlDefaultSettingPopup
            // 
            this.pnlDefaultSettingPopup.BackColor = System.Drawing.Color.Transparent;
            this.pnlDefaultSettingPopup.Controls.Add(this.panel1);
            this.pnlDefaultSettingPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDefaultSettingPopup.Location = new System.Drawing.Point(3, 3);
            this.pnlDefaultSettingPopup.Name = "pnlDefaultSettingPopup";
            this.pnlDefaultSettingPopup.Size = new System.Drawing.Size(1108, 730);
            this.pnlDefaultSettingPopup.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.spcMultiCalcUC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1108, 730);
            this.panel1.TabIndex = 0;
            // 
            // spcMultiCalcUC
            // 
            this.spcMultiCalcUC.ApplicationName = "";
            this.spcMultiCalcUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcMultiCalcUC.BackColorHtml = "#DCDCDC";
            this.spcMultiCalcUC.BssClass = null;
            this.spcMultiCalcUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcMultiCalcUC.DonotCloseProgress = false;
            this.spcMultiCalcUC.dtDataSource = null;
            this.spcMultiCalcUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcMultiCalcUC.ForeColorHtml = "#000000";
            this.spcMultiCalcUC.FunctionName = "";
            this.spcMultiCalcUC.Location = new System.Drawing.Point(0, 0);
            this.spcMultiCalcUC.Name = "spcMultiCalcUC";
            this.spcMultiCalcUC.sessionData = null;
            this.spcMultiCalcUC.SessionData = null;
            this.spcMultiCalcUC.Size = new System.Drawing.Size(1108, 730);
            this.spcMultiCalcUC.SMulModelConfigRawID = ((System.Collections.ArrayList)(resources.GetObject("spcMultiCalcUC.SMulModelConfigRawID")));
            this.spcMultiCalcUC.SMulValueModelConfigRawID = ((System.Collections.ArrayList)(resources.GetObject("spcMultiCalcUC.SMulValueModelConfigRawID")));
            this.spcMultiCalcUC.SParamAlias = null;
            this.spcMultiCalcUC.TabIndex = 0;
            this.spcMultiCalcUC.Title = null;
            this.spcMultiCalcUC.URL = "";
            // 
            // MultiCalculationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 769);
            this.ContentsAreaMinHeight = 730;
            this.ContentsAreaMinWidth = 1108;
            this.Name = "MultiCalculationPopup";
            this.Resizable = true;
            this.Text = "MultiCalculationPopup";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnlDefaultSettingPopup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlDefaultSettingPopup;
        private System.Windows.Forms.Panel panel1;
        private Modeling.SPCMultiCalculationUC spcMultiCalcUC;
    }
}