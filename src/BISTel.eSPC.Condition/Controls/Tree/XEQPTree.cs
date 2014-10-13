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
using System.Drawing;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XEQPTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XEQPTree(BTreeView btv)
        {
            this.XTreeView = btv;
        }

        #endregion

        #region : Public

        public void AddRootNode(TreeNode node)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
                AddEQPNode(tnCurrent, dcValue.Value);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void AddRootNode()
        {
            AddSPCEQPNode();
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)e.Node))
                e.Node.Nodes.Clear();

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            TreeNode tnCurrent = e.Node;


            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
            {
                if (tnCurrent.Nodes.Count > 0) return;

                XDcpTree dcpTree = new XDcpTree(this.XTreeView);

                dcpTree.EqpRawid = dcValue.Value;
                dcpTree.IsShowCheckModule = this.IsShowCheckModule;
                dcpTree.IsShowCheckProduct = this.IsShowCheckProduct;
                dcpTree.IsShowCheckRecipe = this.IsShowCheckRecipe;
                dcpTree.ParamTypeCode = this.ParamTypeCode;
                dcpTree.IsLastNodeModule = this.IsLastNodeModule;
                dcpTree.IsCheckParamType = this.IsCheckParamType;
                dcpTree.IsShowRecipeWildcard = this.IsShowRecipeWildcard;

                dcpTree.AddRootNode(e.Node);
            }
        }

        #endregion

        #region : Private

        private void AddEQPNode(TreeNode tnCurrent, string sEQPModelRawid)
        {
            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);
            //llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, sEQPModelRawid);            

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetEQP(baData);


            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQP_ID, "*", "*");
            btn.IsVisibleCheckBox = this.IsShowCheck;
            btn.IsFolder = false;
            btn.IsVisibleNodeType = true;
            btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP);
            btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP;
            if (!this.IsLastNode)
                btn.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btn);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sEQPID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_ID].ToString();
                string sModuelName = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODULE_NAME].ToString();

                btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQP_ID, sEQPID, sModuelName);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }


        private void AddSPCEQPNode()
        {

            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);
            //llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, sEQPModelRawid);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetEQP(baData);


            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQP_ID, "*", "*");
            btn.IsVisibleCheckBox = this.IsShowCheck;
            btn.IsFolder = false;
            btn.IsVisibleNodeType = true;
            btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP);
            btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP;
            if (!this.IsLastNode)
                btn.Nodes.Add(BTreeView.CreateDummyNode());


            if (ds == null || ds.Tables.Count <= 0)
                return;


            this.XTreeView.Nodes.Add(btn);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sEQPID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_ID].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();

                btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQP_ID, sRawid, sEQPID);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                this.XTreeView.Nodes.Add(btn);
            }
        }

        #endregion

    }
}
