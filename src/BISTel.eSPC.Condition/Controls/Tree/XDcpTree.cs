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
    public class XDcpTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XDcpTree(BTreeView btv)
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
                AddDcpNode(tnCurrent, dcValue.Value);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.DCP_ID)
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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.DCP_ID)
            {
                if (e.Node.Nodes.Count > 0) return;

                System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
                for (int i = 0; i < e.Node.Level; i++)
                {
                    if (lstPathValue[i].ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
                    {
                        this.EqpID = lstPathValue[i].Value;
                        break;
                    }
                }

                XModuleTree moduleTree = new XModuleTree(this.XTreeView);//_btvTreeView);

                moduleTree.LineRawid = this.LineRawid;
                moduleTree.AreaRawid = this.AreaRawid;
                moduleTree.EqpID = this.EqpID;
                moduleTree.DcpID = dcValue.Value;
                moduleTree.IsShowCheck = this.IsShowCheckModule;
                moduleTree.IsShowCheckProduct = this.IsShowCheckProduct;
                moduleTree.IsShowCheckRecipe = this.IsShowCheckRecipe;
                moduleTree.IsCheckParamType = this.IsCheckParamType;
                moduleTree.RecipeTypeCode = this.RecipeTypeCode;
                moduleTree.ParamTypeCode = this.ParamTypeCode;
                moduleTree.IsLastNode = this.IsLastNodeModule;
                moduleTree.IsShowRecipeWildcard = this.IsShowRecipeWildcard;

                moduleTree.AddRootNode(e.Node);
            }
        }

        #endregion

        #region : Private

        private void AddDcpNode(TreeNode tnCurrent, string dcpRawid)
        {
            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetDCP(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //[2009.04.27] CHANDLER - DCP_NAME => DCP_ID
                string sDcp = ds.Tables[0].Rows[i][Definition.COL_DCP_ID].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.COL_RAW_ID].ToString();
                string sState = ds.Tables[0].Rows[i][Definition.COL_DCP_STATE_CD].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.DCP_ID, sDcp, sDcp);

                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;

                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                if (this.IsShowCheck)
                    btn.IsVisibleCheckBox = true;

                if (sState.Length > 0 && sState == "ACT")
                {
                    btn.ForeColor = Color.Red;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.DCP_ACTIVE);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.DCP_ACTIVE;
                }
                else
                {
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.DCP_DEACTIVE);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.DCP_DEACTIVE;
                }

                tnCurrent.Nodes.Add(btn);
            }
        }

        #endregion

    }
}
