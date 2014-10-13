namespace BISTel.eSPC.Page.Common
{
    partial class DefaultCalcChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultCalcChart));
            this.bbtnTimeDisplay = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.SuspendLayout();
            // 
            // bbtnTimeDisplay
            // 
            this.bbtnTimeDisplay.AutoButtonSize = false;
            this.bbtnTimeDisplay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnTimeDisplay.BackgroundImage")));
            this.bbtnTimeDisplay.BssClass = "ConditionButton";
            this.bbtnTimeDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            this.bbtnTimeDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnTimeDisplay.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnTimeDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnTimeDisplay.IsMultiLanguage = true;
            this.bbtnTimeDisplay.Key = "";
            this.bbtnTimeDisplay.Location = new System.Drawing.Point(0, 0);
            this.bbtnTimeDisplay.Name = "bbtnTimeDisplay";
            this.bbtnTimeDisplay.Size = new System.Drawing.Size(120, 26);
            this.bbtnTimeDisplay.TabIndex = 2;
            this.bbtnTimeDisplay.TabStop = false;
            this.bbtnTimeDisplay.Text = "Time-Base Display";
            this.bbtnTimeDisplay.ToolTipText = "";
            this.bbtnTimeDisplay.UseVisualStyleBackColor = true;
            // 
            // DefaultCalcChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DefaultCalcChart";
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnTimeDisplay;
    }
}
