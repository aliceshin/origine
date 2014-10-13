using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using System.Data;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    public class XStepTree : BISTel.eSPC.Condition.Controls.Tree.XStepTree
    {
        public XStepTree(BTreeView btv) : base(btv) { }
    }
}
