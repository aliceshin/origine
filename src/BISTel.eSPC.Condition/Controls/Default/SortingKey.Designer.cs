namespace BISTel.eSPC.Condition.Controls.Default
{
    partial class SortingKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SortingKey));
            this.lstSKeyUnSelected = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnUnSelect = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlHidden = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.lstSKeySelected = new System.Windows.Forms.ListBox();
            this.panel3.SuspendLayout();
            this.pnlHidden.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstSKeyUnSelected
            // 
            this.lstSKeyUnSelected.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstSKeyUnSelected.FormattingEnabled = true;
            this.lstSKeyUnSelected.ItemHeight = 12;
            this.lstSKeyUnSelected.Location = new System.Drawing.Point(0, 26);
            this.lstSKeyUnSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lstSKeyUnSelected.Name = "lstSKeyUnSelected";
            this.lstSKeyUnSelected.ScrollAlwaysVisible = true;
            this.lstSKeyUnSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSKeyUnSelected.Size = new System.Drawing.Size(106, 148);
            this.lstSKeyUnSelected.TabIndex = 48;
            this.lstSKeyUnSelected.DoubleClick += new System.EventHandler(this.lstSKeyUnSelected_DoubleClick);
            // 
            // btnSelect
            // 
            this.btnSelect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSelect.BackgroundImage")));
            this.btnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSelect.Location = new System.Drawing.Point(2, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(23, 23);
            this.btnSelect.TabIndex = 24;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnUnSelect
            // 
            this.btnUnSelect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnSelect.BackgroundImage")));
            this.btnUnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUnSelect.Location = new System.Drawing.Point(2, 34);
            this.btnUnSelect.Name = "btnUnSelect";
            this.btnUnSelect.Size = new System.Drawing.Size(23, 23);
            this.btnUnSelect.TabIndex = 25;
            this.btnUnSelect.UseVisualStyleBackColor = true;
            this.btnUnSelect.Click += new System.EventHandler(this.btnUnSelect_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUnSelect);
            this.panel3.Controls.Add(this.btnSelect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(106, 26);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(27, 156);
            this.panel3.TabIndex = 49;
            // 
            // pnlHidden
            // 
            this.pnlHidden.Controls.Add(this.panel1);
            this.pnlHidden.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHidden.Location = new System.Drawing.Point(0, 0);
            this.pnlHidden.Name = "pnlHidden";
            this.pnlHidden.Size = new System.Drawing.Size(231, 26);
            this.pnlHidden.TabIndex = 50;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPlus);
            this.panel1.Controls.Add(this.btnMinus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(157, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 26);
            this.panel1.TabIndex = 26;
            // 
            // btnPlus
            // 
            this.btnPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlus.BackgroundImage")));
            this.btnPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlus.Location = new System.Drawing.Point(50, 3);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(20, 20);
            this.btnPlus.TabIndex = 45;
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMinus.BackgroundImage")));
            this.btnMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinus.Location = new System.Drawing.Point(28, 3);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(20, 20);
            this.btnMinus.TabIndex = 44;
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // lstSKeySelected
            // 
            this.lstSKeySelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSKeySelected.FormattingEnabled = true;
            this.lstSKeySelected.ItemHeight = 12;
            this.lstSKeySelected.Location = new System.Drawing.Point(133, 26);
            this.lstSKeySelected.Margin = new System.Windows.Forms.Padding(0);
            this.lstSKeySelected.Name = "lstSKeySelected";
            this.lstSKeySelected.ScrollAlwaysVisible = true;
            this.lstSKeySelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSKeySelected.Size = new System.Drawing.Size(98, 148);
            this.lstSKeySelected.TabIndex = 51;
            this.lstSKeySelected.DoubleClick += new System.EventHandler(this.lstSKeySelected_DoubleClick);
            // 
            // SortingKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lstSKeySelected);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lstSKeyUnSelected);
            this.Controls.Add(this.pnlHidden);
            this.Name = "SortingKey";
            this.Size = new System.Drawing.Size(231, 182);
            this.panel3.ResumeLayout(false);
            this.pnlHidden.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.ListBox lstSKeyUnSelected;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnUnSelect;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlHidden;
        private System.Windows.Forms.ListBox lstSKeySelected;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;

    }
}
