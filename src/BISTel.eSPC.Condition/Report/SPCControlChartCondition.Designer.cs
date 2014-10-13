namespace BISTel.eSPC.Condition.Report
{
    partial class SPCControlChartCondition
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpPeriod = new System.Windows.Forms.GroupBox();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.bChkProduct = new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo();
            this.grpEpqID = new System.Windows.Forms.GroupBox();
            this.bchkEqpID = new BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo();
            this.grpParameter = new System.Windows.Forms.GroupBox();
            this.bcboParameter = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.bcboOperation = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.panel1.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.grpEpqID.SuspendLayout();
            this.grpParameter.SuspendLayout();
            this.grpOperation.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.grpPeriod);
            this.panel1.Controls.Add(this.grpProduct);
            this.panel1.Controls.Add(this.grpEpqID);
            this.panel1.Controls.Add(this.grpParameter);
            this.panel1.Controls.Add(this.grpOperation);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 314);
            this.panel1.TabIndex = 0;
            // 
            // grpPeriod
            // 
            this.grpPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPeriod.Location = new System.Drawing.Point(0, 185);
            this.grpPeriod.Name = "grpPeriod";
            this.grpPeriod.Size = new System.Drawing.Size(235, 125);
            this.grpPeriod.TabIndex = 6;
            this.grpPeriod.TabStop = false;
            this.grpPeriod.Text = "Period";
            // 
            // grpProduct
            // 
            this.grpProduct.Controls.Add(this.bChkProduct);
            this.grpProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProduct.Location = new System.Drawing.Point(0, 138);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(235, 47);
            this.grpProduct.TabIndex = 2;
            this.grpProduct.TabStop = false;
            this.grpProduct.Text = "Product";
            // 
            // bChkProduct
            // 
            this.bChkProduct.BackColor = System.Drawing.Color.White;
            this.bChkProduct.BssClass = "";
            this.bChkProduct.CausesValidation = false;
            this.bChkProduct.DataSource = null;
            this.bChkProduct.DisplayMember = "";
            this.bChkProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bChkProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bChkProduct.IsIncludeAll = false;
            this.bChkProduct.Key = "";
            this.bChkProduct.Location = new System.Drawing.Point(3, 17);
            this.bChkProduct.Name = "bChkProduct";
            this.bChkProduct.Size = new System.Drawing.Size(229, 21);
            this.bChkProduct.TabIndex = 3;
            this.bChkProduct.ValueMember = "";
            this.bChkProduct.CheckCombo_Select += new System.EventHandler(this.bChkProduct_CheckCombo_Select);
            this.bChkProduct.CheckCombo_Select_Legend += new System.EventHandler(this.bChkProduct_CheckCombo_Select_Legend);
            // 
            // grpEpqID
            // 
            this.grpEpqID.Controls.Add(this.bchkEqpID);
            this.grpEpqID.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpEpqID.Location = new System.Drawing.Point(0, 91);
            this.grpEpqID.Name = "grpEpqID";
            this.grpEpqID.Size = new System.Drawing.Size(235, 47);
            this.grpEpqID.TabIndex = 3;
            this.grpEpqID.TabStop = false;
            this.grpEpqID.Text = "EQP ID";
            // 
            // bchkEqpID
            // 
            this.bchkEqpID.BackColor = System.Drawing.Color.White;
            this.bchkEqpID.BssClass = "";
            this.bchkEqpID.CausesValidation = false;
            this.bchkEqpID.DataSource = null;
            this.bchkEqpID.DisplayMember = "";
            this.bchkEqpID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bchkEqpID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bchkEqpID.IsIncludeAll = false;
            this.bchkEqpID.Key = "";
            this.bchkEqpID.Location = new System.Drawing.Point(3, 17);
            this.bchkEqpID.Name = "bchkEqpID";
            this.bchkEqpID.Size = new System.Drawing.Size(229, 21);
            this.bchkEqpID.TabIndex = 2;
            this.bchkEqpID.ValueMember = "";
            this.bchkEqpID.CheckCombo_Select += new System.EventHandler(this.bchkEqpID_CheckCombo_Select);
            this.bchkEqpID.CheckCombo_Select_Legend += new System.EventHandler(this.bchkEqpID_CheckCombo_Select_Legend);
            // 
            // grpParameter
            // 
            this.grpParameter.Controls.Add(this.bcboParameter);
            this.grpParameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpParameter.Location = new System.Drawing.Point(0, 44);
            this.grpParameter.Name = "grpParameter";
            this.grpParameter.Size = new System.Drawing.Size(235, 47);
            this.grpParameter.TabIndex = 1;
            this.grpParameter.TabStop = false;
            this.grpParameter.Text = "Parameter";
            // 
            // bcboParameter
            // 
            this.bcboParameter.BssClass = "";
            this.bcboParameter.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboParameter.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboParameter.FormattingEnabled = true;
            this.bcboParameter.IsMultiLanguage = true;
            this.bcboParameter.Key = "";
            this.bcboParameter.Location = new System.Drawing.Point(3, 17);
            this.bcboParameter.Name = "bcboParameter";
            this.bcboParameter.Size = new System.Drawing.Size(229, 21);
            this.bcboParameter.TabIndex = 5;
            this.bcboParameter.SelectedIndexChanged += new System.EventHandler(this.bcboParameter_SelectedIndexChanged);
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.bcboOperation);
            this.grpOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOperation.Location = new System.Drawing.Point(0, 0);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(235, 44);
            this.grpOperation.TabIndex = 0;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "Operation";
            // 
            // bcboOperation
            // 
            this.bcboOperation.BssClass = "";
            this.bcboOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboOperation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboOperation.FormattingEnabled = true;
            this.bcboOperation.IsMultiLanguage = true;
            this.bcboOperation.Key = "";
            this.bcboOperation.Location = new System.Drawing.Point(3, 17);
            this.bcboOperation.Name = "bcboOperation";
            this.bcboOperation.Size = new System.Drawing.Size(229, 21);
            this.bcboOperation.TabIndex = 6;
            this.bcboOperation.SelectedIndexChanged += new System.EventHandler(this.bcboOperation_SelectedIndexChanged);
            // 
            // SPCControlChartCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "SPCControlChartCondition";
            this.Size = new System.Drawing.Size(235, 314);
            this.panel1.ResumeLayout(false);
            this.grpProduct.ResumeLayout(false);
            this.grpEpqID.ResumeLayout(false);
            this.grpParameter.ResumeLayout(false);
            this.grpOperation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       
       

     




        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.GroupBox grpEpqID;
        private System.Windows.Forms.GroupBox grpProduct;
        private System.Windows.Forms.GroupBox grpParameter;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboParameter;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboOperation;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo bchkEqpID;
        private BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo bChkProduct;
        private System.Windows.Forms.GroupBox grpPeriod;
        private BISTel.eSPC.Condition.Controls.Date.DateCondition dateCondition1;
    }
}
