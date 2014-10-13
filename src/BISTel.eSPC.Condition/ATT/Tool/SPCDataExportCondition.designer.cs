namespace BISTel.eSPC.Condition.ATT.Tool
{
    partial class SPCDataExportCondition
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
            this.conditionComboTree1 = new BISTel.eSPC.Condition.ATT.Controls.ComboTree.ConditionComboTree();
            this.spcDataExportConditionTree1 = new BISTel.eSPC.Condition.ATT.Tool.SPCDataExportConditionTree();
            this.SuspendLayout();
            // 
            // conditionComboTree1
            // 
            this.conditionComboTree1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.conditionComboTree1.BackColorHtml = "#DCDCDC";
            this.conditionComboTree1.CheckCountType = BISTel.PeakPerformance.Client.BISTelControl.BTreeView.CheckCountTypes.Multi;
            this.conditionComboTree1.Dock = System.Windows.Forms.DockStyle.Top;
            this.conditionComboTree1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.conditionComboTree1.ForeColorHtml = "#000000";
            this.conditionComboTree1.Location = new System.Drawing.Point(0, 0);
            this.conditionComboTree1.Name = "conditionComboTree1";
            this.conditionComboTree1.Size = new System.Drawing.Size(204, 54);
            this.conditionComboTree1.TabIndex = 2;
            this.conditionComboTree1.Title = "Title";
            // 
            // spcDataExportConditionTree1
            // 
            this.spcDataExportConditionTree1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcDataExportConditionTree1.BackColorHtml = "#DCDCDC";
            this.spcDataExportConditionTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcDataExportConditionTree1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcDataExportConditionTree1.ForeColorHtml = "#000000";
            this.spcDataExportConditionTree1.Location = new System.Drawing.Point(0, 54);
            this.spcDataExportConditionTree1.Name = "spcDataExportConditionTree1";
            this.spcDataExportConditionTree1.Size = new System.Drawing.Size(204, 545);
            this.spcDataExportConditionTree1.TabIndex = 3;
            this.spcDataExportConditionTree1.Title = "Title";
            // 
            // SPCDataExportCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spcDataExportConditionTree1);
            this.Controls.Add(this.conditionComboTree1);
            this.Name = "SPCDataExportCondition";
            this.Controls.SetChildIndex(this.conditionComboTree1, 0);
            this.Controls.SetChildIndex(this.spcDataExportConditionTree1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.eSPC.Condition.ATT.Controls.ComboTree.ConditionComboTree conditionComboTree1;
        private SPCDataExportConditionTree spcDataExportConditionTree1;

    }
}
