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
    public class XProductTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;

        private bool _bExistAll = false;
        private TreeNode _treeNode = null;

        public string ModuleRawid
        {
            get
            {
                return _sModuleRawid;
            }
            set
            {
                _sModuleRawid = value;
            }
        }

        public bool IsExistAll
        {
            get
            {
                return _bExistAll;
            }
            set
            {
                _bExistAll = value;
            }
        }

        #endregion

        #region : Initialization

        public XProductTree(BTreeView btv)
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

            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCT_TYPE, _sModuleRawid, "PRODUCTS");
            btn.IsVisibleCheckBox = false;
            btn.IsFolder = true;
            btn.IsVisibleNodeType = true;
            btn.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btn);
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.PRODUCT_TYPE || dcValue.ContextId == Definition.DynamicCondition_Search_key.PRODUCT)
            {
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)e.Node))
                e.Node.Nodes.Clear();

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            TreeNode tnCurrent = e.Node;

            System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
            for (int i = 0; i < e.Node.Level; i++)
            {
                if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                {
                    _sModuleRawid = lstPathValue[i].Value;
                    break;
                }
            }

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.PRODUCT_TYPE)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                AddProductNode(tnCurrent, dcValue.Value, _sModuleRawid);
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.PRODUCT)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                XRecipeTree recipeTree = new XRecipeTree(this.XTreeView);

                recipeTree.ModuleRawid = _sModuleRawid;
                recipeTree.IsExistAll = _bExistAll;
                recipeTree.IsShowCheck = this.IsShowCheckRecipe;

                if (this.RecipeTypeCode == RecipeType.STEP)
                {
                    recipeTree.IsStepNode = true;
                    recipeTree.IsMvaModelNode = false;
                    recipeTree.IsSpecGroupNode = false;
                }
                else if (this.RecipeTypeCode == RecipeType.SPEC_GROUP)
                {
                    recipeTree.IsStepNode = false;
                    recipeTree.IsMvaModelNode = false;
                    recipeTree.IsSpecGroupNode = true;
                }
                else if (this.RecipeTypeCode == RecipeType.MVA_MODEL)
                {
                    recipeTree.IsStepNode = true;
                    recipeTree.IsMvaModelNode = true;
                    recipeTree.IsSpecGroupNode = false;
                }
                else if (this.RecipeTypeCode == RecipeType.ALL)
                {
                    recipeTree.IsStepNode = true;
                    recipeTree.IsMvaModelNode = false;
                    recipeTree.IsSpecGroupNode = true;
                }
                else //RECIPE인 경우
                {
                    recipeTree.IsStepNode = false;
                    recipeTree.IsMvaModelNode = false;
                    recipeTree.IsSpecGroupNode = false;
                }

                recipeTree.AddRootNode(e.Node);
            }
        }

        #endregion

        #region : Private

        private void AddProductNode(TreeNode tnCurrent, string rawid, string moduleID)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, moduleID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetProduct(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sProduct = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString();

                if (sProduct.Length > 0)
                {
                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCT, sProduct, sProduct);
                    btn.IsVisibleCheckBox = this.IsShowCheck;
                    btn.IsFolder = false;
                    btn.IsVisibleNodeType = true;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                    tnCurrent.Nodes.Add(btn);
                }
            }

            // Product이 없는 Recipe를 위한 Node를 추가
            BTreeNode btnTemp = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PRODUCT, "", "NON PRODUCTS");
            btnTemp.IsVisibleCheckBox = this.IsShowCheck;
            btnTemp.IsFolder = false;
            btnTemp.IsVisibleNodeType = true;
            btnTemp.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT);
            btnTemp.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PRODUCT;
            btnTemp.Nodes.Add(BTreeView.CreateDummyNode());
            tnCurrent.Nodes.Add(btnTemp);
        }

        #endregion
    }
}
