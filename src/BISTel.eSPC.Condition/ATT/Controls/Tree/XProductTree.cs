using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using System.Data;

namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    public class XProductTree : BISTel.eSPC.Condition.Controls.Tree.XProductTree
    {
        public XProductTree(BTreeView btv) : base(btv) { }
    }
}
