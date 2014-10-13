using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using System.Data;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XStepTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;
        private string _sRecipeRawid = string.Empty;
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

        public string RecipeRawid
        {
            get
            {
                return _sRecipeRawid;
            }
            set
            {
                _sRecipeRawid = value;
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

        public XStepTree(BTreeView btv)
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

            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.STEP_TYPE, dcValue.Value, "STEPS");
            btn.IsVisibleCheckBox = false;
            btn.IsFolder = true;
            btn.IsVisibleNodeType = true;
            btn.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btn);
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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.STEP_TYPE)
            {
                if (tnCurrent.Nodes.Count > 0) return;

                System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
                for (int i = 0; i < e.Node.Level; i++)
                {
                    if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                    {
                        this.ModuleRawid = lstPathValue[i].Value;
                        break;
                    }
                }

                for (int i = 0; i < e.Node.Level; i++)
                {
                    if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.PRODUCT))
                    {
                        this.ProductID = lstPathValue[i].Value;
                        break;
                    }
                }

                AddStepNode(tnCurrent, dcValue.Value, this.ModuleRawid);
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.SUBSTRATE_ID)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                //if (_bStepNode)
                //{
                //    XSubstrateTree substrateTree = new XSubstrateTree(this.XTreeView);
                //    substrateTree.ModuleRawid = _sModuleRawid;
                //    substrateTree.RecipeRawid = dcValue.Value;
                //    substrateTree.IsExistAll = _bExistAll;

                //    substrateTree.AddRootNode(e.Node);
                //}
            }
        }

        #endregion

        #region : Private

        private void AddStepNode(TreeNode tnCurrent, string sRecipeID, string sModuleID)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, sModuleID);

            llstData.Add(Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.ProductID);
            llstData.Add(Definition.DynamicCondition_Condition_key.RECIPE_ID, sRecipeID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetRecipeStep(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sStep = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.STEP_ID].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.STEP, sStep, sStep);
                btn.IsVisibleCheckBox = true;
                btn.IsFolder = false;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.STEP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.STEP;

                tnCurrent.Nodes.Add(btn);
            }
        }

        #endregion
    }
}
