namespace BISTel.eSPC.Condition.ATT.Controls
{
    partial class ConditionContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionContainer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.panel1.SuspendLayout();
            this.bArrangePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.bArrangePanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 599);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 43);
            this.panel1.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoButtonSize = false;
            this.btnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearch.BackgroundImage")));
            this.btnSearch.BssClass = "ConditionButton";
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnSearch.IsMultiLanguage = true;
            this.btnSearch.Key = "";
            this.btnSearch.Location = new System.Drawing.Point(104, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 20);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.ToolTipText = "";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // bArrangePanel1
            // 
            this.bArrangePanel1.BLabelAutoPadding = true;
            this.bArrangePanel1.BssClass = "";
            this.bArrangePanel1.Controls.Add(this.btnSearch);
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bArrangePanel1.IsCondition = false;
            this.bArrangePanel1.Key = "";
            this.bArrangePanel1.Location = new System.Drawing.Point(0, 0);
            this.bArrangePanel1.Name = "bArrangePanel1";
            this.bArrangePanel1.Padding_Bottom = 5;
            this.bArrangePanel1.Padding_Left = 5;
            this.bArrangePanel1.Padding_Right = 5;
            this.bArrangePanel1.Padding_Top = 5;
            this.bArrangePanel1.Size = new System.Drawing.Size(204, 43);
            this.bArrangePanel1.Space = 5;
            this.bArrangePanel1.TabIndex = 1;
            this.bArrangePanel1.Title = "";
            // 
            // ConditionContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ConditionContainer";
            this.Size = new System.Drawing.Size(204, 642);
            this.panel1.ResumeLayout(false);
            this.bArrangePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnSearch;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bArrangePanel1;
    }
}
