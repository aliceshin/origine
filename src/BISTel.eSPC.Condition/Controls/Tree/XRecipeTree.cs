using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using System.Data;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XRecipeTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;

        private bool _bStepNode = false;
        private bool _bSpecGroupNode = false;
        private bool _bMvaModelNode = false;
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

        public bool IsStepNode
        {
            get
            {
                return _bStepNode;
            }
            set
            {
                _bStepNode = value;
            }
        }

        public bool IsMvaModelNode
        {
            get
            {
                return _bMvaModelNode;
            }
            set
            {
                _bMvaModelNode = value;
            }
        }

        public bool IsSpecGroupNode
        {
            get
            {
                return _bSpecGroupNode;
            }
            set
            {
                _bSpecGroupNode = value;
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

        public XRecipeTree(BTreeView btv)
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
            string value = string.Empty;
            if (this.UseDefaultCondition.Equals(Definition.YES))
                value = dcValue.Value;//_sProductRawid;
            else
                value = dcValue.Value + "_ROOT";

            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.RECIPE_TYPE, value, "RECIPES");
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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.RECIPE_TYPE || dcValue.ContextId == Definition.DynamicCondition_Search_key.RECIPE)
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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.RECIPE_TYPE)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                AddRecipeNode(tnCurrent, dcValue.Value, _sModuleRawid);
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.RECIPE)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                if (_bStepNode)
                {
                    XStepTree stepTree = new XStepTree(this.XTreeView);

                    stepTree.ModuleRawid = _sModuleRawid;
                    stepTree.RecipeRawid = dcValue.Value;
                    stepTree.IsExistAll = _bExistAll;

                    stepTree.AddRootNode(e.Node);
                }
            }
        }

        #endregion

        #region : Private

        private void AddRecipeNode(TreeNode tnCurrent, string productID, string moduleID)
        {
            LinkedList llstData = new LinkedList();

            if (productID == moduleID + "_0")
            {
                productID = "-1";
            }

            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, moduleID);
            if (this.UseDefaultCondition.Equals(Definition.YES))
            {
                llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, productID);
            }

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetRecipe(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sRecipe = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RECIPE_ID].ToString();

                if (!IsShowRecipeWildcard)
                {
                    if (sRecipe == "*")
                        continue;
                }

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.RECIPE, sRecipe, sRecipe);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.RECIPE);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.RECIPE;
                if (_bStepNode || _bSpecGroupNode || _bMvaModelNode)
                {
                    btn.IsVisibleNodeType = true;
                    btn.Nodes.Add(BTreeView.CreateDummyNode());
                }

                tnCurrent.Nodes.Add(btn);
            }
        }

        #endregion
    }
}
