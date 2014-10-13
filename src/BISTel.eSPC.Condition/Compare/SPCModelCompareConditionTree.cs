using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls;
using BISTel.eSPC.Condition.Controls.Tree;

using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Compare
{
    public partial class SPCModelCompareConditionTree : ConditionTreeWithFilter
    {
        private eSPCWebService.eSPCWebService ws = null;
        private string sLineRawID = string.Empty;
        private string sAreaRawID = string.Empty;
        private string sEQPModel = string.Empty;
        private bool isMET = false;
        private bool isATT = false;

        public BTreeView.CheckCountTypes CheckCountType
        {
            get { return this.bTreeView1.CheckCountType; }
            set { this.bTreeView1.CheckCountType = value; }
        }

        public bool ISMET
        {
            get { return this.isMET; }
            set { this.isMET = value; }
        }

        public bool ISATT
        {
            get { return this.isATT; }
            set { this.isATT = value; }
        }

        public SPCModelCompareConditionTree()
        {
            if(!DesignMode)
            {
                this.bTreeView1.AfterCheck += new TreeViewEventHandler(bTreeView1_AfterCheck);
            }

            base.InitializeComponent();
            InitializeComponent();

            try
            {
                ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

                this.bTreeView1.AddLevelForCheck(1);
                this.bTreeView1.BeforeExpand += new TreeViewCancelEventHandler(bTreeView1_BeforeExpand);
                this.ClickSearch += new ClickSearchEventHandler(SPCModelCompareConditionTree_ClickSearch);
                //added by enkim 2012.10.17 SPC-915
                this.bTreeView1.RefreshNode += new BTreeView.RefreshNodeEventHandler(bTreeView1_RefreshNode);
                //added end SPC-915
            }catch
            {
            }
        }

        void bTreeView1_RefreshNode(object sender, RefreshNodeEventArgs rne)
        {
            bTreeView1_BeforeExpand(null, new TreeViewCancelEventArgs(rne.TargetNode, false, TreeViewAction.Expand));
            bTreeView1.ExpandAll();
        }

        void bTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            LinkedList ll = new LinkedList();
            this.mng.GetSelectedValue("", ll);
            ((ADynamicCondition) this.Parent).RefreshCondition(ll);
        }

        void SPCModelCompareConditionTree_ClickSearch(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ClickSearchEventArgs csea)
        {
            mng.GetSelectedValue("", csea.LinkedListCondition);
        }

        public override void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList ll)
        {
            bool needRefresh = false;

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_LINE]).Rows.Count > 0)
                {
                    sLineRawID = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sLineRawID = string.Empty;
                }

                needRefresh = true;
            }

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_AREA]).Rows.Count > 0)
                {
                    sAreaRawID = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sAreaRawID = string.Empty;
                }

                needRefresh = true;
            }

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_EQPMODEL]).Rows.Count > 0)
                {
                    sEQPModel = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sEQPModel = string.Empty;
                }

                needRefresh = true;
            }

            if(needRefresh)
                RefreshTreeView();
        }

        public void RefreshTreeView()
        {
            this.bTreeView1.Nodes.Clear();

            if(string.IsNullOrEmpty(this.sEQPModel)
                && string.IsNullOrEmpty(this.sAreaRawID))
                return;

            if (isATT)
            {
                BTreeNode node = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM_TYPE, "trs", "AttributeName");
                node.Nodes.Add(BTreeView.CreateDummyNode());
                this.bTreeView1.Nodes.Add(node);
                this.bTreeView1.ExpandAll();
            }
            else
            {
                LinkedList llstCondition = new LinkedList();
                if (isMET)
                {
                    llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_MET_PARAM_TYPE);
                }
                else
                {
                    llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PARAM_TYPE);
                }
                DataSet ds = ws.GetCodeData(llstCondition.GetSerialData());

                if (ds == null || ds.Tables[0].Rows.Count == 0)
                    return;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BTreeNode node = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM_TYPE, row["CODE"].ToString(),
                                                                row["NAME"].ToString());
                    if (isMET)
                    {
                        node.Nodes.Add(BTreeView.CreateDummyNode());
                        this.bTreeView1.Nodes.Add(node);
                    }
                    else
                    {
                        if (node.Text != "METROLOGY")
                        {
                            node.Nodes.Add(BTreeView.CreateDummyNode());
                            this.bTreeView1.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        //added by enkim 2012.10.17 SPC-915

        void bTreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node.Nodes.Count == 0)
                AddChildNodes((BTreeNode)e.Node);
        }

        //added end SPC-915

        void AddChildNodes(BTreeNode node)
        {
            DCValueOfTree dcvalue = TreeDCUtil.GetDCValue(node);

            string[] paramAlias = null;

            if (isATT)
            {
                paramAlias = ws.GetATTParamListHavingSPCModel(sLineRawID, sAreaRawID, sEQPModel, dcvalue.Value);
            }
            else
            {
                paramAlias = ws.GetParamListHavingSPCModel(sLineRawID, sAreaRawID, sEQPModel, dcvalue.Value);
            }

            if(paramAlias == null)
                return;

            foreach(string param in paramAlias)
            {
                node.Nodes.Add(TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM_ALIAS, param, param));
            }
        }

        public override void Filter(string text)
        {
            Unfilter();
            this.bTreeView1.ExpandAll();

            //SPC-950 by Louis 
            //this.bTreeView1.CollapseAll();

            for (int i = bTreeView1.Nodes.Count - 1; i >= 0; i--)
            {
                Filter((BTreeNode) bTreeView1.Nodes[i], 0, text);
            }
        }

        private void Filter(BTreeNode node, int depth, string text)
        {
            if(depth < 1)
            {
                depth++;
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    Filter((BTreeNode)node.Nodes[i], depth, text);
            }
            else
            {
                if(!node.Text.ToUpper().Contains(text.ToUpper()))
                    node.Remove();
            }
        }

        public override void Unfilter()
        {
            RefreshTreeView();
        }

    }
}
