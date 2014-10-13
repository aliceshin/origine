namespace BISTel.eSPC.Condition.Modeling
{
    partial class ContextCondition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextCondition));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bcboStepID = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bControlArrangePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel();
            this.blblStepID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcboRecipeID = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bControlArrangePanel2 = new BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel();
            this.blblRecipeID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel3 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcboModuleID = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bControlArrangePanel4 = new BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel();
            this.blblModuleID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel6 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcboEQPID = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bControlArrangePanel3 = new BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel();
            this.blblEQPID = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel4 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.panel1.SuspendLayout();
            this.bControlArrangePanel1.SuspendLayout();
            this.bControlArrangePanel2.SuspendLayout();
            this.bControlArrangePanel4.SuspendLayout();
            this.bControlArrangePanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.bcboStepID);
            this.panel1.Controls.Add(this.bControlArrangePanel1);
            this.panel1.Controls.Add(this.bcboRecipeID);
            this.panel1.Controls.Add(this.bControlArrangePanel2);
            this.panel1.Controls.Add(this.bcboModuleID);
            this.panel1.Controls.Add(this.bControlArrangePanel4);
            this.panel1.Controls.Add(this.bcboEQPID);
            this.panel1.Controls.Add(this.bControlArrangePanel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 180);
            this.panel1.TabIndex = 0;
            // 
            // bcboStepID
            // 
            this.bcboStepID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.bcboStepID.BssClass = "";
            this.bcboStepID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboStepID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboStepID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboStepID.FormattingEnabled = true;
            this.bcboStepID.IsMultiLanguage = true;
            this.bcboStepID.Key = "";
            this.bcboStepID.Location = new System.Drawing.Point(0, 155);
            this.bcboStepID.Margin = new System.Windows.Forms.Padding(0);
            this.bcboStepID.Name = "bcboStepID";
            this.bcboStepID.Size = new System.Drawing.Size(235, 21);
            this.bcboStepID.TabIndex = 33;
            this.bcboStepID.SelectionChangeCommitted += new System.EventHandler(this.bcboStepID_SelectionChangeCommitted);
            // 
            // bControlArrangePanel1
            // 
            this.bControlArrangePanel1.ActiveControl = null;
            this.bControlArrangePanel1.Controls.Add(this.blblStepID);
            this.bControlArrangePanel1.Controls.Add(this.bLabel1);
            this.bControlArrangePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bControlArrangePanel1.HeaderSize = 20;
            this.bControlArrangePanel1.Location = new System.Drawing.Point(0, 132);
            this.bControlArrangePanel1.Name = "bControlArrangePanel1";
            this.bControlArrangePanel1.Size = new System.Drawing.Size(235, 23);
            this.bControlArrangePanel1.TabIndex = 3;
            // 
            // blblStepID
            // 
            this.blblStepID.BssClass = "";
            this.blblStepID.CustomTextAlign = "";
            this.blblStepID.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblStepID.IsMultiLanguage = false;
            this.blblStepID.Key = "";
            this.blblStepID.Location = new System.Drawing.Point(13, 0);
            this.blblStepID.Margin = new System.Windows.Forms.Padding(4, 0, 3, 0);
            this.blblStepID.Name = "blblStepID";
            this.blblStepID.Size = new System.Drawing.Size(86, 23);
            this.blblStepID.TabIndex = 1;
            this.blblStepID.Text = "STEP ID";
            this.blblStepID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel1
            // 
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel1.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(0, 0);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(13, 23);
            this.bLabel1.TabIndex = 0;
            this.bLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcboRecipeID
            // 
            this.bcboRecipeID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.bcboRecipeID.BssClass = "";
            this.bcboRecipeID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboRecipeID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboRecipeID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboRecipeID.FormattingEnabled = true;
            this.bcboRecipeID.IsMultiLanguage = true;
            this.bcboRecipeID.Key = "";
            this.bcboRecipeID.Location = new System.Drawing.Point(0, 111);
            this.bcboRecipeID.Margin = new System.Windows.Forms.Padding(0);
            this.bcboRecipeID.Name = "bcboRecipeID";
            this.bcboRecipeID.Size = new System.Drawing.Size(235, 21);
            this.bcboRecipeID.TabIndex = 32;
            this.bcboRecipeID.SelectionChangeCommitted += new System.EventHandler(this.bcboRecipeID_SelectionChangeCommitted);
            // 
            // bControlArrangePanel2
            // 
            this.bControlArrangePanel2.ActiveControl = null;
            this.bControlArrangePanel2.Controls.Add(this.blblRecipeID);
            this.bControlArrangePanel2.Controls.Add(this.bLabel3);
            this.bControlArrangePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.bControlArrangePanel2.HeaderSize = 20;
            this.bControlArrangePanel2.Location = new System.Drawing.Point(0, 88);
            this.bControlArrangePanel2.Name = "bControlArrangePanel2";
            this.bControlArrangePanel2.Size = new System.Drawing.Size(235, 23);
            this.bControlArrangePanel2.TabIndex = 4;
            // 
            // blblRecipeID
            // 
            this.blblRecipeID.BssClass = "";
            this.blblRecipeID.CustomTextAlign = "";
            this.blblRecipeID.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblRecipeID.IsMultiLanguage = false;
            this.blblRecipeID.Key = "";
            this.blblRecipeID.Location = new System.Drawing.Point(13, 0);
            this.blblRecipeID.Margin = new System.Windows.Forms.Padding(4, 0, 3, 0);
            this.blblRecipeID.Name = "blblRecipeID";
            this.blblRecipeID.Size = new System.Drawing.Size(86, 23);
            this.blblRecipeID.TabIndex = 1;
            this.blblRecipeID.Text = "RECIPE ID";
            this.blblRecipeID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel3
            // 
            this.bLabel3.BssClass = "";
            this.bLabel3.CustomTextAlign = "";
            this.bLabel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel3.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel3.IsMultiLanguage = true;
            this.bLabel3.Key = "";
            this.bLabel3.Location = new System.Drawing.Point(0, 0);
            this.bLabel3.Name = "bLabel3";
            this.bLabel3.Size = new System.Drawing.Size(13, 23);
            this.bLabel3.TabIndex = 0;
            this.bLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcboModuleID
            // 
            this.bcboModuleID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.bcboModuleID.BssClass = "";
            this.bcboModuleID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboModuleID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboModuleID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboModuleID.FormattingEnabled = true;
            this.bcboModuleID.IsMultiLanguage = true;
            this.bcboModuleID.Key = "";
            this.bcboModuleID.Location = new System.Drawing.Point(0, 67);
            this.bcboModuleID.Margin = new System.Windows.Forms.Padding(0);
            this.bcboModuleID.Name = "bcboModuleID";
            this.bcboModuleID.Size = new System.Drawing.Size(235, 21);
            this.bcboModuleID.TabIndex = 31;
            this.bcboModuleID.SelectionChangeCommitted += new System.EventHandler(this.bcboModuleID_SelectionChangeCommitted);
            // 
            // bControlArrangePanel4
            // 
            this.bControlArrangePanel4.ActiveControl = null;
            this.bControlArrangePanel4.Controls.Add(this.blblModuleID);
            this.bControlArrangePanel4.Controls.Add(this.bLabel6);
            this.bControlArrangePanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.bControlArrangePanel4.HeaderSize = 20;
            this.bControlArrangePanel4.Location = new System.Drawing.Point(0, 44);
            this.bControlArrangePanel4.Name = "bControlArrangePanel4";
            this.bControlArrangePanel4.Size = new System.Drawing.Size(235, 23);
            this.bControlArrangePanel4.TabIndex = 29;
            // 
            // blblModuleID
            // 
            this.blblModuleID.BssClass = "";
            this.blblModuleID.CustomTextAlign = "";
            this.blblModuleID.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblModuleID.IsMultiLanguage = false;
            this.blblModuleID.Key = "";
            this.blblModuleID.Location = new System.Drawing.Point(13, 0);
            this.blblModuleID.Margin = new System.Windows.Forms.Padding(4, 0, 3, 0);
            this.blblModuleID.Name = "blblModuleID";
            this.blblModuleID.Size = new System.Drawing.Size(86, 23);
            this.blblModuleID.TabIndex = 1;
            this.blblModuleID.Text = "MODULE ID";
            this.blblModuleID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel6
            // 
            this.bLabel6.BssClass = "";
            this.bLabel6.CustomTextAlign = "";
            this.bLabel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel6.Image = ((System.Drawing.Image)(resources.GetObject("bLabel6.Image")));
            this.bLabel6.IsMultiLanguage = true;
            this.bLabel6.Key = "";
            this.bLabel6.Location = new System.Drawing.Point(0, 0);
            this.bLabel6.Name = "bLabel6";
            this.bLabel6.Size = new System.Drawing.Size(13, 23);
            this.bLabel6.TabIndex = 0;
            this.bLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bcboEQPID
            // 
            this.bcboEQPID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.bcboEQPID.BssClass = "";
            this.bcboEQPID.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcboEQPID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcboEQPID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bcboEQPID.FormattingEnabled = true;
            this.bcboEQPID.IsMultiLanguage = true;
            this.bcboEQPID.Key = "";
            this.bcboEQPID.Location = new System.Drawing.Point(0, 23);
            this.bcboEQPID.Margin = new System.Windows.Forms.Padding(0);
            this.bcboEQPID.Name = "bcboEQPID";
            this.bcboEQPID.Size = new System.Drawing.Size(235, 21);
            this.bcboEQPID.TabIndex = 30;
            this.bcboEQPID.SelectionChangeCommitted += new System.EventHandler(this.bcboEQPID_SelectionChangeCommitted);
            // 
            // bControlArrangePanel3
            // 
            this.bControlArrangePanel3.ActiveControl = null;
            this.bControlArrangePanel3.Controls.Add(this.blblEQPID);
            this.bControlArrangePanel3.Controls.Add(this.bLabel4);
            this.bControlArrangePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.bControlArrangePanel3.HeaderSize = 20;
            this.bControlArrangePanel3.Location = new System.Drawing.Point(0, 0);
            this.bControlArrangePanel3.Name = "bControlArrangePanel3";
            this.bControlArrangePanel3.Size = new System.Drawing.Size(235, 23);
            this.bControlArrangePanel3.TabIndex = 28;
            // 
            // blblEQPID
            // 
            this.blblEQPID.BssClass = "";
            this.blblEQPID.CustomTextAlign = "";
            this.blblEQPID.Dock = System.Windows.Forms.DockStyle.Left;
            this.blblEQPID.IsMultiLanguage = false;
            this.blblEQPID.Key = "";
            this.blblEQPID.Location = new System.Drawing.Point(13, 0);
            this.blblEQPID.Name = "blblEQPID";
            this.blblEQPID.Size = new System.Drawing.Size(86, 23);
            this.blblEQPID.TabIndex = 1;
            this.blblEQPID.Text = "EQP ID";
            this.blblEQPID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel4
            // 
            this.bLabel4.BssClass = "";
            this.bLabel4.CustomTextAlign = "";
            this.bLabel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel4.Image = ((System.Drawing.Image)(resources.GetObject("bLabel4.Image")));
            this.bLabel4.IsMultiLanguage = true;
            this.bLabel4.Key = "";
            this.bLabel4.Location = new System.Drawing.Point(0, 0);
            this.bLabel4.Name = "bLabel4";
            this.bLabel4.Size = new System.Drawing.Size(13, 23);
            this.bLabel4.TabIndex = 0;
            this.bLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContextCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ContextCondition";
            this.Size = new System.Drawing.Size(235, 180);
            this.panel1.ResumeLayout(false);
            this.bControlArrangePanel1.ResumeLayout(false);
            this.bControlArrangePanel2.ResumeLayout(false);
            this.bControlArrangePanel4.ResumeLayout(false);
            this.bControlArrangePanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       
       
  
       
      



       
      
      

      
     




        #endregion

        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel bControlArrangePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblStepID;
        private BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel bControlArrangePanel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblRecipeID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel3;
        private BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel bControlArrangePanel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblModuleID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel6;
        private BISTel.PeakPerformance.Client.BISTelControl.BControlArrangePanel bControlArrangePanel3;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblEQPID;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel4;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboStepID;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboRecipeID;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboModuleID;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox bcboEQPID;
    }
}
