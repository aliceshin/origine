namespace BISTel.eSPC.Condition.Compare.MET
{
    partial class SPCModelCompareCondition
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
            this.conditionComboTree1 = new BISTel.eSPC.Condition.Controls.ComboTree.ConditionComboTree();
            this.spcModelCompareConditionTree1 = new BISTel.eSPC.Condition.Compare.MET.SPCModelCompareConditionTree();
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
            this.conditionComboTree1.Size = new System.Drawing.Size(204, 68);
            this.conditionComboTree1.TabIndex = 4;
            this.conditionComboTree1.Title = "Title";
            // 
            // spcModelCompareConditionTree1
            // 
            this.spcModelCompareConditionTree1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcModelCompareConditionTree1.BackColorHtml = "#DCDCDC";
            this.spcModelCompareConditionTree1.CheckCountType = BISTel.PeakPerformance.Client.BISTelControl.BTreeView.CheckCountTypes.Multi;
            this.spcModelCompareConditionTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcModelCompareConditionTree1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcModelCompareConditionTree1.ForeColorHtml = "#000000";
            this.spcModelCompareConditionTree1.Location = new System.Drawing.Point(0, 68);
            this.spcModelCompareConditionTree1.Name = "spcModelCompareConditionTree1";
            this.spcModelCompareConditionTree1.Size = new System.Drawing.Size(204, 531);
            this.spcModelCompareConditionTree1.TabIndex = 5;
            this.spcModelCompareConditionTree1.Title = "Title";
            // 
            // SPCModelCompareCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spcModelCompareConditionTree1);
            this.Controls.Add(this.conditionComboTree1);
            this.Name = "SPCModelCompareCondition";
            this.Controls.SetChildIndex(this.conditionComboTree1, 0);
            this.Controls.SetChildIndex(this.spcModelCompareConditionTree1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.eSPC.Condition.Controls.ComboTree.ConditionComboTree conditionComboTree1;
        private BISTel.eSPC.Condition.Compare.MET.SPCModelCompareConditionTree spcModelCompareConditionTree1;
    }
}
