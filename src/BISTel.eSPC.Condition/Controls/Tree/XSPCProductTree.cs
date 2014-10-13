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

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XSPCProductTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;
        
        private TreeNode _treeNode = null;

        
        #endregion

        #region : Initialization

        public XSPCProductTree(BTreeView btv)
        {
            this.XTreeView = btv;
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            //btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
        }

        #endregion

        #region : Public

        public void AddSearchTypeProductsNode()
        {

            AddSearchProductsNode();

           
        }

        public void AddRootNode(TreeNode node)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
                AddProductNode(tnCurrent, this.AreaRawid, this.AreaRawid);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node);
            if (dcValue == null) return;
            
           
            
        }

        #endregion

        #region : Private

        private void AddProductNode(TreeNode tnCurrent,string rawid, string moduleID)
        {
            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);            
            DataSet ds = this._wsSPC.GetSPCProduct(llstData.GetSerialData());


            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCT, "*", "*");
            btn.IsVisibleCheckBox = this.IsShowCheck;
            btn.IsFolder = false;
            btn.IsVisibleNodeType = true;
            btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
            btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
            if (!this.IsLastNode)
                btn.Nodes.Add(BTreeView.CreateDummyNode());
            tnCurrent.Nodes.Add(btn);
                            
                
            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sPRODUCT_ID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString();
                
                btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCT, sPRODUCT_ID, sPRODUCT_ID);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }


        /// <summary>
        /// Search Type : Product
        /// </summary>
        /// <param name="tnCurrent"></param>
        /// <param name="rawid"></param>
        /// <param name="moduleID"></param>
        private void AddSearchProductsNode()
        {
            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);

            if (!string.IsNullOrEmpty(this.OperationID))
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);

            if (!string.IsNullOrEmpty(this.EqpID))
            llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);
            DataSet ds = this._wsSPC.GetSPCProduct(llstData.GetSerialData());


            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCTS, "*", "*");
            btn.IsVisibleCheckBox = this.IsShowCheck;
            btn.IsFolder = false;
            btn.IsVisibleNodeType = true;
            btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
            btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
            if (!this.IsLastNode)
                btn.Nodes.Add(BTreeView.CreateDummyNode());

            this.XTreeView.Nodes.Add(btn);              

            if (ds == null || ds.Tables.Count <= 0)
                return;


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sPRODUCT_ID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString();

                btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCTS, sPRODUCT_ID, sPRODUCT_ID);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                this.XTreeView.Nodes.Add(btn);
            }
        }

        #endregion
    }
}
