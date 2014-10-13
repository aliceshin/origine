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
    public class XEQPModelsTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        private bool _bIsSPCModelTree = false;
        private bool _bIsSPCModelContextTree = false;

        #endregion

        #region : Initialization

        public XEQPModelsTree(BTreeView btv)
        {
            this.XTreeView = btv;
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            //btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
        }

        #endregion

        #region : Public

        public void AddRootNode()
        {

            AddEQPModelNode();

        }


        public void AddRootNode(TreeNode node, string _AreaRawid)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
                AddEQPModelNode(tnCurrent, _AreaRawid);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void AddSearchTypeProductsRootNode(TreeNode node, string _AreaRawid)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
                AddEQPModelSearchTypeProductsNode(tnCurrent, _AreaRawid);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Condition_key.DCP_ID)
            {
                //e.TargetNode.Nodes.Clear();
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)e.Node))
                e.Node.Nodes.Clear();

            if (_treeNode != null)
            {
                if (_treeNode.Level > e.Node.Level) return;
            }

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            TreeNode tnCurrent = e.Node;

            if (dcValue.ContextId == Definition.DynamicCondition_Condition_key.EQP)
            {
                if (tnCurrent.Nodes.Count > 0) return;

                if (dcValue.Value == this.EqpRawid)//_sEqpRawid)
                {
                    AddEQPModelNode(tnCurrent, dcValue.Value);
                }
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Condition_key.DCP_ID)
            {
                DCValueOfTree dcParent = TreeDCUtil.GetDCValue(e.Node.Parent);
                if (dcParent.Value == this.EqpRawid)                            //_sEqpRawid)
                {
                    if (e.Node.Nodes.Count > 0) return;

                    e.Node.Nodes.Clear();

                    XModuleTree moduleTree = new XModuleTree(this.XTreeView);   //_btvTreeView);
                    moduleTree.EqpRawid = this.EqpRawid;                        //_sEqpRawid;
                    moduleTree.DcpRawid = dcValue.Value;
                    moduleTree.IsShowCheck = this.IsShowCheckModule;            // _bIsShowCheckModule;
                    moduleTree.IsShowCheckProduct = this.IsShowCheckProduct;    //._bIsShowCheckProduct;
                    moduleTree.IsShowCheckRecipe = this.IsShowCheckRecipe;      //._bIsShowCheckRecipe;
                    moduleTree.IsCheckParamType = this.IsCheckParamType;        //._bIsCheckParamType;
                    moduleTree.RecipeTypeCode = this.RecipeTypeCode;            // _recipeType;
                    moduleTree.ParamTypeCode = this.ParamTypeCode;              // _paramType;
                    moduleTree.IsLastNode = this.IsLastNodeModule;

                    moduleTree.AddRootNode(e.Node);
                }
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQPMODEL)
            {
                if (_bIsSPCModelTree || IsSPCModelContextTree)
                {
                    if (e.Node.Nodes.Count > 0) return;

                    e.Node.Nodes.Clear();

                    XSPCModelTree spcModelTree = new XSPCModelTree(this.XTreeView);

                    spcModelTree.LineRawid = this.LineRawid;
                    spcModelTree.AreaRawid = this.AreaRawid;
                    spcModelTree.EqpModelName = dcValue.Value;
                    spcModelTree.ParamTypeCD = this.ParamTypeCD;

                   
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;

                    if (IsSPCModelContextTree)
                    {
                        spcModelTree.IsShowCheck = false;
                        spcModelTree.IsLastNode = false;
                    }
                    else
                    {
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.IsLastNode = true;
                    }

                    spcModelTree.AddRootNode(e.Node);

                    //if (e.Node.Nodes.Count > 0)
                    //    ((BTreeNode)e.Node).IsVisibleCheckBox = false;
                    //else
                    //    ((BTreeNode)e.Node).IsVisibleCheckBox = true;
                }
            }

        }

        public bool IsSPCModelTree
        {
            set { _bIsSPCModelTree = value; }
            get { return _bIsSPCModelTree; }
        }

        public bool IsSPCModelContextTree
        {
            set { _bIsSPCModelContextTree = value; }
            get { return _bIsSPCModelContextTree; }
        }

        #endregion

        #region : Private

        private void AddEQPModelNode()
        {

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetEqpModel(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sEQPModel = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQPMODEL, sRawid, sEQPModel);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());
                if (this.IsShowCheck)
                    btn.IsVisibleCheckBox = true;

                this.XTreeView.Nodes.Add(btn);
            }
        }

        private void AddEQPModelNode(TreeNode tnCurrent, string sAreaRawID)
        {
            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, sAreaRawID);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetEqpModel(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sEQPModel = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQPMODEL, sRawid, sEQPModel);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());
                if (this.IsShowCheck)
                    btn.IsVisibleCheckBox = true;

                tnCurrent.Nodes.Add(btn);
            }
        }



        private void AddEQPModelSearchTypeProductsNode(TreeNode tnCurrent, string sAreaRawID)
        {
            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, sAreaRawID);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetEqpModel(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sEQPModel = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EQPMODEL_PRODUCTS, sRawid, sEQPModel);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP_MODEL;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());
                if (this.IsShowCheck)
                    btn.IsVisibleCheckBox = true;

                tnCurrent.Nodes.Add(btn);
            }
        }

        #endregion

    }
}
