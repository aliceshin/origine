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
    public class XSearchTypeTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XSearchTypeTree(BTreeView btv)
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
                AddSearchTypeNode(tnCurrent);

            if (tnCurrent.Nodes.Count > 0) return;
        }


        public void AddRootNode()
        {
            AddSearchTypeNode();
        }


   
        public void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
          
        }

        #endregion

        #region : Private

        private void AddSearchTypeNode(TreeNode tnCurrent)
        {
            LinkedList llstData = new LinkedList();     
            llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_SEARCH_TYPE);
            DataSet ds = this._wsSPC.GetCodeData(llstData.GetSerialData());
          
            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString();
                string sDesc = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.CATEGORY, sRawid, sDesc);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.MODULE);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.MODULE;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }

        private void AddSearchTypeNode()
        {
            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_SEARCH_TYPE);
            DataSet ds = this._wsSPC.GetCodeData(llstData.GetSerialData());

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString();
                string sDesc = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.CATEGORY, sRawid, sDesc);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.MODULE);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.MODULE;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                this.XTreeView.Nodes.Add(btn);
            }
        }


        #endregion

    }
}
