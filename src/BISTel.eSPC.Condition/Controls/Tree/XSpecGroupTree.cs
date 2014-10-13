using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Data;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XSpecGroupTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;
        private string _sRecipeRawid = string.Empty;

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

        #endregion

        #region : Initialization

        public XSpecGroupTree(BTreeView btv)
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

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPEC_GROUP_TYPE, dcValue.Value, "SPEC GROUPS");
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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.SPEC_GROUP_TYPE)
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



            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.SPEC_GROUP_TYPE)
            {
                System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
                for (int i = 0; i < e.Node.Level; i++)
                {
                    if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                    {
                        _sModuleRawid = lstPathValue[i].Value;
                        break;
                    }
                }

                if (tnCurrent.Nodes.Count > 0) return;

                //if (dcValue.Value == _sRecipeRawid)
                //{
                //    tnCurrent.Nodes.Clear();
                AddSpecGroupNode(tnCurrent, dcValue.Value, _sModuleRawid);
                //}
            }
        }

        #endregion

        #region : Private

        private void AddSpecGroupNode(TreeNode tnCurrent, string sRecipeRawid, string sModuleRawid)
        {
            //LinkedList llstData = new LinkedList();

            //llstData.Add(Definition.CONDITION_KEY_EQP_RAWID, sModuleRawid);
            ////llstData.Add(Definition.CONDITION_KEY_EQP_PG_RAWID, groupRawid);
            //llstData.Add(Definition.CONDITION_KEY_RECIPE_RAWID, sRecipeRawid);

            //byte[] baData = llstData.GetSerialData();
            //DataSet ds = _spcWebService.GetSpecModelGroupList(baData);

            //if (ds == null || ds.Tables.Count <= 0)
            //    return;


            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    string sStep = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_SPEC_MODEL_GROUP_NAME].ToString();
            //    string sRawid = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_RAWID].ToString();

            //    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.CONDITION_SEARCH_KEY_SPEC_GROUP, sRawid, sStep);
            //    btn.IsVisibleCheckBox = true;
            //    btn.IsFolder = false;
            //    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.SPECGROUP);
            //    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.SPECGROUP;

            //    tnCurrent.Nodes.Add(btn);
            //}
        }

        #endregion
    }
}
