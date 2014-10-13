using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using System.Data;

namespace BISTel.eSPC.Condition.ATT.Controls.Tree
{
    public class XSummaryParamTree : BISTel.eSPC.Condition.Controls.Tree.XSummaryParamTree
    {
        public XSummaryParamTree(BTreeView btv) : base(btv) { }
    }
}
