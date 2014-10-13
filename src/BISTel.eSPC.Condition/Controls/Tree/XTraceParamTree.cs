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
    public class XTraceParamTree : TreeInterface
    {
        #region : Field

        private string _sModuleRawid = string.Empty;

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

        #endregion

        #region : Initialization

        public XTraceParamTree(BTreeView btv)
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

            //if (tnCurrent.Nodes.Count > 0) return;

            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Condition_key.TRACE_PARAM_TYPE, node.Name + _sModuleRawid, "TRACE");
            btn.IsVisibleCheckBox = true;
            btn.IsFolder = true;
            btn.IsVisibleNodeType = true;
            btn.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btn);
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Condition_key.TRACE_PARAM_TYPE)
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)e.Node))
                e.Node.Nodes.Clear();

            //if (_treeNode.Level > e.Node.Level) return;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            TreeNode tnCurrent = e.Node;

            System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
            for (int i = 0; i < e.Node.Level; i++)
            {
                if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                {
                    this.ModuleRawid = lstPathValue[i].Value;
                    break;
                }
            }

            if (dcValue.ContextId == Definition.DynamicCondition_Condition_key.TRACE_PARAM_TYPE)
            {
                if (tnCurrent.Nodes.Count <= 0)
                {
                    //AddGroupNode(tnCurrent, "-1", this.ModuleRawid);
                    AddParamNode(tnCurrent, "-1", this.ModuleRawid);
                }
            }
            else if (dcValue.ContextId.Contains(Definition.DynamicCondition_Condition_key.GROUP))
            {
                if (tnCurrent.Nodes.Count <= 0)
                {
                    //AddGroupNode(tnCurrent, dcValue.Value, this.ModuleRawid);
                    AddParamNode(tnCurrent, dcValue.Value, this.ModuleRawid);
                }
            }
        }

        #endregion

        #region : Private

        private void AddGroupNode(TreeNode tnCurrent, string value, string moduleRawid)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.EQP_RAWID, moduleRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.PARENT, value);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetParamGroup(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            tnCurrent.Nodes.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sGroup = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.GROUP_NAME].ToString();
                string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Condition_key.GROUP + "_" + (tnCurrent.Level + 1), sRawid, sGroup);
                btn.IsVisibleCheckBox = true;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PARAM_GROUP;
                btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }

        private void AddParamNode(TreeNode tnCurrent, string groupRawid, string moduleID)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, moduleID);
            llstData.Add(Definition.DynamicCondition_Condition_key.EQP_PG_RAWID, groupRawid);
            llstData.Add(Definition.DynamicCondition_Search_key.DCP_ID, this.DcpID); //::

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetParamByGroup(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            DataSet dsCheck = new DataSet();
            if (ds != null && ds.Tables.Count > 0 && this.IsCheckParamType)
            {
                dsCheck.Merge(ds.Tables[0].Select("data_type_cd IN ('FLT', 'INT')"));
                ds = dsCheck;
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();
                    string sAlias = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                    string sVirtual = ds.Tables[0].Rows[i]["VIRTUAL_YN"].ToString();
                    string sType = ds.Tables[0].Rows[i]["data_type_cd"].ToString();

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Condition_key.PARAM, sRawid, sAlias);
                    btn.IsVisibleCheckBox = true;
                    btn.IsFolder = false;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.TRACE_PARAM);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.TRACE_PARAM;
                    if (sVirtual == Definition.YES)
                    {
                        btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.VIRTUAL_PARAM);
                        btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.VIRTUAL_PARAM;
                    }

                    tnCurrent.Nodes.Add(btn);
                }
            }
        }

        #endregion

    }
}
