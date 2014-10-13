using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class ConditionTreeWithFilter : ConditionTree
    {
        public System.Windows.Forms.Panel panel1;
        public BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtFilter;
        public BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel3;
        public BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
    
        public void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.bLabel3 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.btxtFilter = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlBody.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTreeView1
            // 
            this.bTreeView1.LineColor = System.Drawing.Color.Black;
            this.bTreeView1.Location = new System.Drawing.Point(0, 33);
            this.bTreeView1.Size = new System.Drawing.Size(148, 88);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.panel1);
            this.pnlBody.Controls.SetChildIndex(this.panel1, 0);
            this.pnlBody.Controls.SetChildIndex(this.bTreeView1, 0);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bLabel3);
            this.panel1.Controls.Add(this.btxtFilter);
            this.panel1.Controls.Add(this.bLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 33);
            this.panel1.TabIndex = 1;
            // 
            // bLabel3
            // 
            this.bLabel3.BackColor = System.Drawing.Color.White;
            this.bLabel3.BssClass = "";
            this.bLabel3.CustomTextAlign = "";
            this.bLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel3.ForeColor = System.Drawing.Color.Black;
            this.bLabel3.Image = global::BISTel.eSPC.Condition.Properties.Resources.bullet_011;
            this.bLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel3.IsMultiLanguage = true;
            this.bLabel3.Key = "";
            this.bLabel3.Location = new System.Drawing.Point(13, 5);
            this.bLabel3.Name = "bLabel3";
            this.bLabel3.Size = new System.Drawing.Size(15, 21);
            this.bLabel3.TabIndex = 12;
            // 
            // btxtFilter
            // 
            this.btxtFilter.BssClass = "";
            this.btxtFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtFilter.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtFilter.IsMultiLanguage = true;
            this.btxtFilter.Key = "";
            this.btxtFilter.Location = new System.Drawing.Point(62, 6);
            this.btxtFilter.Name = "btxtFilter";
            this.btxtFilter.Size = new System.Drawing.Size(126, 20);
            this.btxtFilter.TabIndex = 1;
            this.btxtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btxtFilter_KeyPress);
            // 
            // bLabel1
            // 
            this.bLabel1.AutoSize = true;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(27, 9);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(29, 13);
            this.bLabel1.TabIndex = 0;
            this.bLabel1.Text = "Filter";
            // 
            // ConditionTreeWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ConditionTreeWithFilter";
            this.pnlBody.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        public void btxtFilter_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                string text = ((TextBox) sender).Text;
                if(text == string.Empty)
                {
                    Unfilter();
                }
                else
                {
                    Filter(text);
                }
            }
        }

        public virtual void Filter(string text)
        {
        }

        public virtual void Unfilter()
        {
        }
    }
}
