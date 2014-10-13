using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Drawing;
using System.Data;

using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;


using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Tree;


namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    class XSPCChartComboTreeControl : IAddChildNodeToDCTreeView
    {
        #region : Field

        private string _sUseDefaultCondition = "";
        private eSPCWebService.eSPCWebService _spcWebService = null;
        private BTreeView _btvCondition = null;
        private string _sParamType = "MET";
        private LinkedList _llstParent = null;

        #endregion

        #region : IAddChildNodeToDCTreeView Members

        public void GetParameter(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {
        }

        public void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {

            if (llData.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
            {
                DataTable dt = (DataTable)llData[Definition.DynamicCondition_Search_key.PARAM_TYPE];
                string _sParamType = DCUtil.GetValueData(dt);
            }
        }

        public void SetDCTree(BISTel.PeakPerformance.Client.BISTelControl.BTreeView btv)
        {
            this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();


            _btvCondition = btv;
            //_btvCondition.CheckCountType = BTreeView.CheckCountTypes.SingleByNode;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            btv.AfterExpand += new TreeViewEventHandler(btv_AfterExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(btv_RefreshNode);

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);

            _btvCondition.AddImageForLevel(0, _btvCondition.ImageList.Images[0], _btvCondition.ImageList.Images[0]);
            _btvCondition.AddImageForLevel(1, _btvCondition.ImageList.Images[1], _btvCondition.ImageList.Images[1]);
            _btvCondition.AddImageForLevel(2, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[2]);
            _btvCondition.AddImageForLevel(3, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[3]);

            btv.CheckCountType = BTreeView.CheckCountTypes.Single;

            // btv.Nodes.Clear();


        }

        public void btv_RefreshNode(object sender, RefreshNodeEventArgs rne)
        {

        }

        public void SetParentValue(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llParentValue)
        {
            _llstParent = llParentValue;
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
            {
                DataTable dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.PARAM_TYPE];
                string _sParamType = DCUtil.GetValueData(dt);

                for (int i = 0; i < _btvCondition.Nodes.Count; i++)
                {


                }
            }
        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.PARAM_TYPE)
            {
                e.TargetNode.Nodes.Clear();
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            if (_sParamType == "MET" && dcValue.ContextId == Definition.DynamicCondition_Search_key.FAB)
            {
                for (int i = 0; i < e.Node.Nodes.Count; i++)
                {
                    BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                    btn.Nodes.Clear();
                    btn.IsVisibleCheckBox = true;
                    btn.IsVisibleNodeType = true;
                }
            }
            else if (_sParamType != "MET" && dcValue.ContextId == Definition.DynamicCondition_Search_key.LINE)
            {
                for (int i = 0; i < e.Node.Nodes.Count; i++)
                {
                    BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                    btn.Nodes.Clear();
                    btn.IsVisibleCheckBox = true;
                    btn.IsVisibleNodeType = true;
                }
            }

        }

        void btv_AfterExpand(object sender, TreeViewEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            if (_sParamType == "MET" && dcValue.ContextId == Definition.DynamicCondition_Search_key.FAB)
            {
                for (int i = 0; i < e.Node.Nodes.Count; i++)
                {
                    BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                    btn.Nodes.Clear();
                    btn.IsVisibleCheckBox = true;
                    btn.IsVisibleNodeType = true;
                }
            }
            else if (_sParamType != "MET" && dcValue.ContextId == Definition.DynamicCondition_Search_key.LINE)
            {
                for (int i = 0; i < e.Node.Nodes.Count; i++)
                {
                    BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                    btn.Nodes.Clear();
                    btn.IsVisibleCheckBox = true;
                    btn.IsVisibleNodeType = true;
                }
            }
        }

        #endregion


        #region IAddChildNodeToDCTreeView Members

        public bool IsAddNode()
        {
            return true;
        }

        #endregion

    }
}
