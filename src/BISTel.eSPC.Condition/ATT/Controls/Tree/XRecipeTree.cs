using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using System.Data;

namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    public class XRecipeTree : BISTel.eSPC.Condition.Controls.Tree.XRecipeTree
    {
        public XRecipeTree(BTreeView btv) : base(btv) { }
    }
}
