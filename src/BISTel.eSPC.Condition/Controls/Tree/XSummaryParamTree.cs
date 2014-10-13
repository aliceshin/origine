using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using System.Data;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XSummaryParamTree : TreeInterface
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

        public XSummaryParamTree(BTreeView btv)
        {
            this.XTreeView = btv;

            _wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        }

        #endregion

        #region : Public

        public void AddRootNode(TreeNode node)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            BTreeNode btnSum = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.TRACE_SUM_PARAM_TYPE, node.Name + _sModuleRawid, "TRACE SUMMARY");
            btnSum.IsVisibleCheckBox = true;
            btnSum.IsFolder = true;
            btnSum.IsVisibleNodeType = true;
            btnSum.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btnSum);

            BTreeNode btnEvent = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EVENT_SUM_PARAM_TYPE, node.Name + _sModuleRawid, "EVENT SUMMARY");
            btnEvent.IsVisibleCheckBox = true;
            btnEvent.IsFolder = true;
            btnEvent.IsVisibleNodeType = true;
            btnEvent.Nodes.Add(BTreeView.CreateDummyNode());

            tnCurrent.Nodes.Add(btnEvent);
        }

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.TRACE_SUM_PARAM_TYPE || dcValue.ContextId == Definition.DynamicCondition_Search_key.EVENT_SUM_PARAM_TYPE)
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

            for (int i = 0; i < e.Node.Level; i++)
            {
                if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.DCP_ID))
                {
                    this.DcpID = lstPathValue[i].Value;
                    break;
                }
            }

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.TRACE_SUM_PARAM_TYPE)
            {
                if (tnCurrent.Nodes.Count > 0) return;

                tnCurrent.Nodes.Clear();

                AddSummaryParamNode(tnCurrent, dcValue.Value, _sModuleRawid);
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EVENT_SUM_PARAM_TYPE)
            {
                if (tnCurrent.Nodes.Count > 0) return;

                tnCurrent.Nodes.Clear();

                AddEventParamNode(tnCurrent, dcValue.Value, _sModuleRawid);
            }
        }

        #endregion

        #region : Private

        private void AddSummaryParamNode(TreeNode tnCurrent, string groupRawid, string moduleID)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.TYPE, ParameterType.TRACE_SUMMARY);
            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, moduleID);
            llstData.Add(Definition.DynamicCondition_Condition_key.DCP_ID, this.DcpID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetParameter(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            DataSet dsCheck = new DataSet();

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sParam = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                    string sRawid = sParam + i.ToString();

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.TRACE_SUM_PARAM, sRawid, sParam);
                    btn.IsVisibleCheckBox = true;
                    btn.IsFolder = false;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.TRACE_SUM_PARAM);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.TRACE_SUM_PARAM;

                    tnCurrent.Nodes.Add(btn);
                }
            }
        }

        private void AddEventParamNode(TreeNode tnCurrent, string groupRawid, string moduleID)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.TYPE, ParameterType.EVENT_SUMMARY);
            llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, moduleID);
            llstData.Add(Definition.DynamicCondition_Condition_key.DCP_ID, this.DcpID);

            byte[] baData = llstData.GetSerialData();
            DataSet ds = _wsSPC.GetParameter(baData);

            if (ds == null || ds.Tables.Count <= 0)
                return;

            DataSet dsCheck = new DataSet();

            tnCurrent.Nodes.Clear();

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sParam = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                    string sRawid = sParam + i.ToString();

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.EVENT_SUM_PARAM, sParam, sParam);
                    btn.IsVisibleCheckBox = true;
                    btn.IsFolder = false;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EVENT_SUM_PARAM);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EVENT_SUM_PARAM;

                    tnCurrent.Nodes.Add(btn);
                }
            }
        }

        #endregion
    }
}
