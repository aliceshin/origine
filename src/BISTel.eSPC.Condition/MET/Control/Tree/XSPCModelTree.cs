using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Data;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.eSPC.Common;
using System.Drawing;
using BISTel.eSPC.Condition.Controls.Tree;

namespace BISTel.eSPC.Condition.MET.Controls.Tree
{
    public class XSPCModelTree : BISTel.eSPC.Condition.Controls.Tree.XSPCModelTree
    {
        public XSPCModelTree(BTreeView btv) : base(btv) { }
    }
}
