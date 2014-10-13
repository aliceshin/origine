namespace BISTel.eSPC.Page.Common
{
    partial class ChartCalculationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartCalculationPopup));
            this.pnlDefaultSettingPopup = new System.Windows.Forms.Panel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.spcCalcUC = new BISTel.eSPC.Page.Modeling.SPCCalculationUC();
            this.pnlContentsArea.SuspendLayout();
            this.pnlDefaultSettingPopup.SuspendLayout();
            this.pnlChart.SuspendLayout();
            this.bapnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnlDefaultSettingPopup);
            this.pnlContentsArea.Size = new System.Drawing.Size(1106, 906);
            // 
            // pnlDefaultSettingPopup
            // 
            this.pnlDefaultSettingPopup.Controls.Add(this.pnlChart);
            this.pnlDefaultSettingPopup.Controls.Add(this.bapnlButton);
            this.pnlDefaultSettingPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDefaultSettingPopup.Location = new System.Drawing.Point(3, 3);
            this.pnlDefaultSettingPopup.Name = "pnlDefaultSettingPopup";
            this.pnlDefaultSettingPopup.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlDefaultSettingPopup.Size = new System.Drawing.Size(1100, 900);
            this.pnlDefaultSettingPopup.TabIndex = 14;
            // 
            // pnlChart
            // 
            this.pnlChart.Controls.Add(this.spcCalcUC);
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 0);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.pnlChart.Size = new System.Drawing.Size(1100, 860);
            this.pnlChart.TabIndex = 7;
            // 
            // bapnlButton
            // 
            this.bapnlButton.BLabelAutoPadding = true;
            this.bapnlButton.BssClass = "TitleArrPanel";
            this.bapnlButton.Controls.Add(this.bbtnCancel);
            this.bapnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlButton.IsCondition = false;
            this.bapnlButton.Key = "";
            this.bapnlButton.Location = new System.Drawing.Point(0, 860);
            this.bapnlButton.Name = "bapnlButton";
            this.bapnlButton.Padding_Bottom = 5;
            this.bapnlButton.Padding_Left = 5;
            this.bapnlButton.Padding_Right = 5;
            this.bapnlButton.Padding_Top = 5;
            this.bapnlButton.Size = new System.Drawing.Size(1100, 35);
            this.bapnlButton.Space = 5;
            this.bapnlButton.TabIndex = 6;
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
            this.bbtnCancel.Key = "FDC_POPUP_BUTTON_CLOSE";
            this.bbtnCancel.Location = new System.Drawing.Point(1019, 5);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(72, 25);
            this.bbtnCancel.TabIndex = 20;
            this.bbtnCancel.Text = "Close";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // spcCalcUC
            // 
            this.spcCalcUC.ApplicationName = "";
            this.spcCalcUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcCalcUC.BackColorHtml = "#DCDCDC";
            this.spcCalcUC.BssClass = null;
            this.spcCalcUC.ChartVariable = null;
            this.spcCalcUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcCalcUC.DonotCloseProgress = false;
            this.spcCalcUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcCalcUC.ForeColorHtml = "#000000";
            this.spcCalcUC.FunctionName = "";
            this.spcCalcUC.Location = new System.Drawing.Point(5, 5);
            this.spcCalcUC.Name = "spcCalcUC";
            this.spcCalcUC.sessionData = null;
            this.spcCalcUC.SessionData = null;
            this.spcCalcUC.Size = new System.Drawing.Size(1090, 855);
            this.spcCalcUC.SModelConfigRawID = null;
            this.spcCalcUC.SParamAlias = null;
            this.spcCalcUC.TabIndex = 0;
            this.spcCalcUC.Title = null;
            this.spcCalcUC.URL = "";
            // 
            // ChartCalculationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 939);
            this.ContentsAreaMinHeight = 900;
            this.ContentsAreaMinWidth = 1100;
            this.Name = "ChartCalculationPopup";
            this.Resizable = true;
            this.Text = "ChartCalculationPopup";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnlDefaultSettingPopup.ResumeLayout(false);
            this.pnlChart.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   
        #endregion

        private System.Windows.Forms.Panel pnlDefaultSettingPopup;
        private System.Windows.Forms.Panel pnlChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.eSPC.Page.Modeling.SPCCalculationUC spcCalcUC;

    }
}