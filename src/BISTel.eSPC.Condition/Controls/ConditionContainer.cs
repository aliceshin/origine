using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Condition.Controls
{
    public partial class ConditionContainer : ADynamicCondition
    {
        public ConditionContainer()
        {
            InitializeComponent();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            LinkedList ll = new LinkedList();
            Search(ll);
        }

        public override void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList ll)
        {
            foreach(Control con in this.Controls)
            {
                if(con is ADynamicCondition)
                {
                    ((ADynamicCondition) con).RefreshCondition(ll);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
