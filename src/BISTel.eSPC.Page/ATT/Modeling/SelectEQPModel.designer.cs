namespace BISTel.eSPC.Page.ATT.Modeling
{
    partial class SelectEQPModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectEQPModel));
            this.btnIgnore = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.btnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.btnOK = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bplEQPModel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlContentsArea.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.bplEQPModel);
            this.pnlContentsArea.Controls.Add(this.panel1);
            this.pnlContentsArea.Size = new System.Drawing.Size(588, 37);
            // 
            // btnIgnore
            // 
            this.btnIgnore.AutoButtonSize = false;
            this.btnIgnore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnIgnore.BackgroundImage")));
            this.btnIgnore.BssClass = "";
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIgnore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnIgnore.IsMultiLanguage = true;
            this.btnIgnore.Key = "";
            this.btnIgnore.Location = new System.Drawing.Point(160, 5);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(106, 20);
            this.btnIgnore.TabIndex = 2;
            this.btnIgnore.Text = "Use Current Model";
            this.btnIgnore.ToolTipText = "";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoButtonSize = false;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BssClass = "";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnCancel.IsMultiLanguage = true;
            this.btnCancel.Key = "";
            this.btnCancel.Location = new System.Drawing.Point(82, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 20);
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
            this.btnOK.BssClass = "";
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btnOK.IsMultiLanguage = true;
            this.btnOK.Key = "";
            this.btnOK.Location = new System.Drawing.Point(5, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 20);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.ToolTipText = "";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // bplEQPModel
            // 
            this.bplEQPModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bplEQPModel.Location = new System.Drawing.Point(3, 3);
            this.bplEQPModel.Name = "bplEQPModel";
            this.bplEQPModel.Size = new System.Drawing.Size(298, 31);
            this.bplEQPModel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnIgnore);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(301, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 31);
            this.panel1.TabIndex = 3;
            // 
            // SelectEQPModel
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(600, 70);
            this.ContentsAreaMinHeight = 80;
            this.ContentsAreaMinWidth = 300;
            this.MaximumSize = new System.Drawing.Size(600, 70);
            this.MinimumSize = new System.Drawing.Size(600, 70);
            this.Name = "SelectEQPModel";
            this.Text = "Select Target to Save";
            this.Title = "Select Target to Save";
            this.Load += new System.EventHandler(this.SelectEQPModel_Load);
            this.pnlContentsArea.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnOK;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton btnIgnore;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel bplEQPModel;
    }
}