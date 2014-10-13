using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public partial class ConditionTree : ADynamicCondition
    {
        public string Title
        {
            get { return this.btpnlTitle.Title; }
            set { this.btpnlTitle.Title = value; }
        }
        protected TreeDCControlMng mng = new TreeDCControlMng();
        public ConditionTree()
        {
            this.bTreeView1 = (BTreeView) mng.Create(true, "");
            InitializeComponent();
        }

        public override void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList ll)
        {

        }
    }
}
