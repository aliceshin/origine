using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Data;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    public class XModuleTree : BISTel.eSPC.Condition.Controls.Tree.XModuleTree
    {
        public XModuleTree(BTreeView btv) : base(btv) { }
    }
}
