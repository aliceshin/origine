namespace BISTel.eSPC.Page.Report.Popup
{
    partial class SeriesGroupingAndFilteringPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeriesGroupingAndFilteringPopup));
            this.baplButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.btnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.btnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.flplColumns = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlContentsArea.SuspendLayout();
            this.baplButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.flplColumns);
            this.pnlContentsArea.Controls.Add(this.baplButton);
            this.pnlContentsArea.Size = new System.Drawing.Size(768, 510);
            // 
            // baplButton
            // 
            this.baplButton.AutoSize = true;
            this.baplButton.BackColor = System.Drawing.Color.White;
            this.baplButton.BLabelAutoPadding = true;
            this.baplButton.BssClass = "";
            this.baplButton.Controls.Add(this.btnCancel);
            this.baplButton.Controls.Add(this.btnOK);
            this.baplButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.baplButton.IsCondition = false;
            this.baplButton.Key = "";
            this.baplButton.Location = new System.Drawing.Point(3, 477);
            this.baplButton.Name = "baplButton";
            this.baplButton.Padding_Bottom = 5;
            this.baplButton.Padding_Left = 5;
            this.baplButton.Padding_Right = 5;
            this.baplButton.Padding_Top = 5;
            this.baplButton.Size = new System.Drawing.Size(762, 30);
            this.baplButton.Space = 5;
            this.baplButton.TabIndex = 0;
            this.baplButton.Title = "";
            // 
            // btnCancel
            // 
            this.btnCancel.AutoButtonSize = false;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BssClass = "ConditionButton";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnCancel.IsMultiLanguage = true;
            this.btnCancel.Key = "";
            this.btnCancel.Location = new System.Drawing.Point(682, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.ToolTipText = "";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AutoButtonSize = false;
            this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
            this.btnOK.BssClass = "ConditionButton";
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnOK.IsMultiLanguage = true;
            this.btnOK.Key = "";
            this.btnOK.Location = new System.Drawing.Point(605, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.ToolTipText = "";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // flplColumns
            // 
            this.flplColumns.AutoScroll = true;
            this.flplColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flplColumns.Location = new System.Drawing.Point(3, 3);
            this.flplColumns.Name = "flplColumns";
            this.flplColumns.Padding = new System.Windows.Forms.Padding(20);
            this.flplColumns.Size = new System.Drawing.Size(762, 474);
            this.flplColumns.TabIndex = 1;
            // 
            // SeriesGroupingAndFilteringPopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(780, 543);
            this.ContentsAreaMinWidth = 780;
            this.Name = "SeriesGroupingAndFilteringPopup";
            this.Resizable = true;
            this.Text = "Filtering";
            this.Title = "Filtering";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnlContentsArea.PerformLayout();
            this.baplButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel baplButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnOK;
        private System.Windows.Forms.FlowLayoutPanel flplColumns;

    }
}