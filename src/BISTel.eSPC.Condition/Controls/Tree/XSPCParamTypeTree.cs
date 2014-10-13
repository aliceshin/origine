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
    public class XSPCParamTypeTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XSPCParamTypeTree(BTreeView btv)
        {
            this.XTreeView = btv;
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            //btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
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
                AddParamTypeNode(tnCurrent);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void AddRootNode()
        {
            _treeNode = null;
            AddParamTypeNode();
        }

        public void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;
       
            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.OPERATION)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();
            }

         
        }

        #endregion

        #region : Private

        private void AddParamTypeNode(TreeNode node)
        {
            LinkedList llstData = new LinkedList();               
           llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PARAM_TYPE);
            DataSet ds = this._wsSPC.GetCodeData(llstData.GetSerialData());
          
            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sCode = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString();
                string sDesc = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_NAME].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM_TYPE, sCode, sDesc);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                node.Nodes.Add(btn);
            }
        }


        private void AddParamTypeNode()
        {

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PARAM_TYPE);
            llstData.Add("USE_YN", "Y");
            DataSet ds = this._wsSPC.GetCodeData(llstData.GetSerialData());

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sCode = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString();
                string sDesc = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_NAME].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM_TYPE, sCode, sDesc);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                this.XTreeView.Nodes.Add(btn);
            }
        }

        #endregion

    }
}
