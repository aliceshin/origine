using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    public enum ConditionLevel
    {
        SITE,
        FAB,
        LINE,
        AREA,
        EQPMODEL,
        MAINEQP,
        DCP
    }

    public partial class ConditionComboTree : ADynamicCondition
    {
        private eSPCWebService.eSPCWebService ws = null;
        private SortedList<ConditionLevel, bool> visibleLevel = new SortedList<ConditionLevel, bool>();
        TreeComboDCControlMng mng = new TreeComboDCControlMng();

        public string Title
        {
            get { return this.btpnlTitle.Title; }
            set { this.btpnlTitle.Title = value; }
        }

        public BTreeView.CheckCountTypes CheckCountType
        {
            get { return this.bTreeCombo1.TreeView.CheckCountType; }
            set { this.bTreeCombo1.TreeView.CheckCountType = value; }
        }

        public ConditionComboTree()
        {
            if(!DesignMode)
            {
                this.bTreeCombo1 = (BTreeCombo) mng.Create(true, "");
                this.bTreeCombo1.SelectionCommitted += new EventHandler(bTreeCombo1_SelectionCommitted);
            }

            InitializeComponent();
        }

        void bTreeCombo1_SelectionCommitted(object sender, EventArgs e)
        {
            LinkedList ll = new LinkedList();
            this.mng.GetSelectedValue("", ll);
            ((ADynamicCondition) this.Parent).RefreshCondition(ll);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

                LoadRootNode();

                this.bTreeCombo1.TreeView.BeforeExpand += new TreeViewCancelEventHandler(TreeView_BeforeExpand);

                this.ClickSearch += new ClickSearchEventHandler(ConditionComboTree_ClickSearch);
            }
            catch
            {
            }

            base.OnLoad(e);
        }

        void ConditionComboTree_ClickSearch(object sender, ClickSearchEventArgs csea)
        {
            mng.GetSelectedValue("", csea.LinkedListCondition);
        }

        void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if(!e.Node.Nodes[0].Tag.Equals(0))
                return;

            e.Node.Nodes.RemoveAt(0);

            AddChildNodes(e.Node);
        }

        public override void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList ll)
        {
        }

        private void ImageLoad()
        {
            ImageLoader imgLoader = new ImageLoader();
            for(int i=0; i<visibleLevel.Count; i++)
            {
                Bitmap bitmap = null;
                switch(visibleLevel.Keys[i])
                {
                    case ConditionLevel.SITE:
                        bitmap = (Bitmap)imgLoader.TreeImageList[ImageLoader.TREE_IMAGE_INDEX.SITE];
                        break;
                    case ConditionLevel.FAB:
                        bitmap = (Bitmap)imgLoader.TreeImageList[ImageLoader.TREE_IMAGE_INDEX.FAB];
                        break;
                    case ConditionLevel.LINE:
                        bitmap = (Bitmap)imgLoader.TreeImageList[ImageLoader.TREE_IMAGE_INDEX.LINE];
                        break;
                    case ConditionLevel.AREA:
                        bitmap = (Bitmap)imgLoader.TreeImageList[ImageLoader.TREE_IMAGE_INDEX.AREA];
                        break;
                    case ConditionLevel.EQPMODEL:
                        bitmap = (Bitmap)imgLoader.TreeImageList[ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL];
                        break;
                }
                this.bTreeCombo1.TreeView.AddImageForLevel(i, bitmap, bitmap);
            }
        }

        public void AddVisibleLevel(ConditionLevel level, bool checkVisible)
        {
            if(visibleLevel.ContainsKey(level))
                visibleLevel.Remove(level);

            visibleLevel.Add(level, checkVisible);
        }

        public void LoadRootNode()
        {
            ImageLoad();

            this.bTreeCombo1.TreeView.Nodes.Clear();

            if(visibleLevel.Count == 0)
                return;

            InitializeLevelForCheck();

            LinkedList llst = new LinkedList();
            DataSet ds = GetDataSet(visibleLevel.Keys[0], llst);
            string contextID = GetContextID(visibleLevel.Keys[0]);

            AddNodes(contextID, ds);
        }

        private DataSet GetDataSet(ConditionLevel level, LinkedList llstData)
        {
            DataSet ds = null;

            switch(level)
            {
                case ConditionLevel.SITE :
                    ds = ws.GetSiteForCondition();
                    break;
                case ConditionLevel.FAB :
                    ds = ws.GetFabForCondition(llstData.GetSerialData());
                    break;
                case ConditionLevel.LINE :
                    ds = ws.GetLineForCondition(llstData.GetSerialData());
                    break;
                case ConditionLevel.AREA :
                    ds = ws.GetAreaForCondition(llstData.GetSerialData());
                    break;
                case ConditionLevel.EQPMODEL :
                    ds = ws.GetEqpModelForCondition(llstData.GetSerialData());
                    break;

                    //TODO
            }

            return ds;
        }

        private void MakeWhereLinkedList(LinkedList llstWhere, TreeNode node)
        {
            TreeNode current = node;

            while(current != null)
            {
                ConditionLevel currentLevel = visibleLevel.Keys[current.Level];

                llstWhere.Add(GetWhereKey(currentLevel), TreeDCUtil.GetDCValue(current).Value);

                current = current.Parent;
            }
        }

        private string GetWhereKey(ConditionLevel level)
        {
            switch(level)
            {
                case ConditionLevel.SITE :
                    return Definition.CONDITION_KEY_SITE;
                case ConditionLevel.FAB :
                    return Definition.CONDITION_KEY_FAB;
                case ConditionLevel.LINE :
                    return Definition.CONDITION_KEY_LINE_RAWID;
                case ConditionLevel.AREA :
                    return Definition.CONDITION_KEY_AREA_RAWID;
                //TODO
            }
            return string.Empty;
        }

        private void AddNodes(string contextID, DataSet ds)
        {
            AddNodesToTreeView(GetTreeNodeFromDataSet(contextID, ds));
        }

        private void AddChildNodes(TreeNode node)
        {
            ConditionLevel currentLevel = visibleLevel.Keys[node.Level];

            LinkedList llstWhere = new LinkedList();
            MakeWhereLinkedList(llstWhere, node);

            currentLevel = GetNextLevel(currentLevel);
            string contextID = GetContextID(currentLevel);
            List<BTreeNode> childNodes = GetTreeNodeFromDataSet(contextID, GetDataSet(currentLevel, llstWhere));
            while(currentLevel != visibleLevel.Keys[node.Level + 1])
            {
                //TODO
            }

            AddChildNodesToNode(childNodes, (BTreeNode)node);
        }

        private void AddChildNodesToNode(List<BTreeNode> childNodes, BTreeNode target)
        {
            foreach(BTreeNode child in childNodes)
            {
                target.Nodes.Add(child);
                if(!IsTerminalNode(child.Level))
                    AddDummyNode(child);
            }
        }

        private void AddNodesToTreeView(List<BTreeNode> nodes)
        {
            foreach(BTreeNode node in nodes)
            {
                this.bTreeCombo1.TreeView.Nodes.Add(node);
                if(!IsTerminalNode(node.Level))
                    AddDummyNode(node);
            }
        }

        private void AddDummyNode(BTreeNode node)
        {
            node.Nodes.Add(new BTreeNode() {Text = "DUMMY", Tag = 0});
        }

        private static List<BTreeNode> GetTreeNodeFromDataSet(string contextID, DataSet ds)
        {
            List<BTreeNode> nodes = new List<BTreeNode>();
            if(ds == null || ds.Tables.Count == 0)
                return nodes;

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                nodes.Add(GetTreeNodeFromDataRow(contextID, dr));
            }

            return nodes;
        }

        private static BTreeNode GetTreeNodeFromDataRow(string contextID, DataRow dr)
        {
            BTreeNode node = TreeDCUtil.CreateBTreeNode(contextID, dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString(),
                                                        dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString());
            return node;
        }

        private void InitializeLevelForCheck()
        {
            for(int i=0; i<visibleLevel.Count; i++)
            {
                if(visibleLevel.Values[i])
                {
                    this.bTreeCombo1.TreeView.AddLevelForCheck(i);
                }
            }
        }

        private bool IsTerminalNode(int level)
        {
            if(level == visibleLevel.Count - 1)
            {
                return true;
            }
            return false;
        }

        private ConditionLevel GetNextLevel(ConditionLevel level)
        {
            switch(level)
            {
                case ConditionLevel.SITE:
                    return ConditionLevel.FAB;
                case ConditionLevel.FAB:
                    return ConditionLevel.LINE;
                case ConditionLevel.LINE:
                    return ConditionLevel.AREA;
                case ConditionLevel.AREA:
                    return ConditionLevel.EQPMODEL;
                case ConditionLevel.EQPMODEL:
                    return ConditionLevel.EQPMODEL;
            }

            return ConditionLevel.EQPMODEL;
        }

        private string GetContextID(ConditionLevel level)
        {
            switch(level)
            {
                case ConditionLevel.SITE:
                    return Definition.DynamicCondition_Search_key.SITE;
                case ConditionLevel.FAB:
                    return Definition.DynamicCondition_Search_key.FAB;
                case ConditionLevel.LINE:
                    return Definition.DynamicCondition_Search_key.LINE;
                case ConditionLevel.AREA:
                    return Definition.DynamicCondition_Search_key.AREA;
                case ConditionLevel.EQPMODEL:
                    return Definition.DynamicCondition_Search_key.EQPMODEL;
            }

            return Definition.DynamicCondition_Search_key.EQPMODEL;
        }
    }
}
