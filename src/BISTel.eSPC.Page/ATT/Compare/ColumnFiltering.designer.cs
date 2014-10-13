namespace BISTel.eSPC.Page.ATT.Compare
{
    partial class ColumnFiltering
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chlbValues = new System.Windows.Forms.CheckedListBox();
            this.btxtFiltering = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chlbValues);
            this.groupBox1.Controls.Add(this.btxtFiltering);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Column Name";
            // 
            // chlbValues
            // 
            this.chlbValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chlbValues.FormattingEnabled = true;
            this.chlbValues.Location = new System.Drawing.Point(3, 36);
            this.chlbValues.Name = "chlbValues";
            this.chlbValues.Size = new System.Drawing.Size(144, 154);
            this.chlbValues.TabIndex = 1;
            this.chlbValues.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlbValues_ItemCheck);
            this.chlbValues.CheckOnClick = true;
            // 
            // btxtFiltering
            // 
            this.btxtFiltering.BssClass = "";
            this.btxtFiltering.Dock = System.Windows.Forms.DockStyle.Top;
            this.btxtFiltering.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtFiltering.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtFiltering.IsMultiLanguage = true;
            this.btxtFiltering.Key = "";
            this.btxtFiltering.Location = new System.Drawing.Point(3, 16);
            this.btxtFiltering.Name = "btxtFiltering";
            this.btxtFiltering.Size = new System.Drawing.Size(144, 20);
            this.btxtFiltering.TabIndex = 0;
            this.btxtFiltering.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btxtFiltering_KeyPress);
            // 
            // ColumnFiltering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ColumnFiltering";
            this.Size = new System.Drawing.Size(150, 200);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtFiltering;
        private System.Windows.Forms.CheckedListBox chlbValues;
    }
}
