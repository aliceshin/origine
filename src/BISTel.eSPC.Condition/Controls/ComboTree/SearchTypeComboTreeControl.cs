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

using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Tree;

namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    class SearchTypeComboTreeControl : IAddChildNodeToDCTreeView
    {
        #region : Field

        private string _sUseDefaultCondition = "";
        private Condition.eSPCWebService.eSPCWebService _spcWebService = null;
        private BTreeView _btvCondition = null;
        private LinkedList _llstParent =new LinkedList();

        #endregion

        #region : IAddChildNodeToDCTreeView Members

        public void GetParameter(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {
        }

        public void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {
        }

        public void SetDCTree(BISTel.PeakPerformance.Client.BISTelControl.BTreeView btv)
        {
            this._spcWebService = new BISTel.eSPC.Condition.eSPCWebService.eSPCWebService();

            _btvCondition = btv;
            //_btvCondition.CheckCountType = BTreeView.CheckCountTypes.SingleByNode;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            btv.AfterExpand += new TreeViewEventHandler(btv_AfterExpand);

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);

            _btvCondition.AddImageForLevel(0, _btvCondition.ImageList.Images[0], _btvCondition.ImageList.Images[0]);
            _btvCondition.AddImageForLevel(1, _btvCondition.ImageList.Images[1], _btvCondition.ImageList.Images[1]);
            _btvCondition.AddImageForLevel(2, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[2]);
            _btvCondition.AddImageForLevel(3, _btvCondition.ImageList.Images[3], _btvCondition.ImageList.Images[3]);
            _btvCondition.AddImageForLevel(4, _btvCondition.ImageList.Images[4], _btvCondition.ImageList.Images[4]);

            //_btvCondition.CheckTreeLevel.Add(4);
            //_btvCondition.AddLevelForCheck(4);

            btv.CheckCountType = BTreeView.CheckCountTypes.Single;

            //InitializeDefaultCondition();
        }

        public void SetParentValue(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llParentValue)
        {
        
             _btvCondition.Nodes.Clear();

            _llstParent = llParentValue;

            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.PARAM))
            {

                XSearchTypeTree searchTypeTree = new XSearchTypeTree(_btvCondition);                
                searchTypeTree.IsShowCheck = true;                
                searchTypeTree.IsLastNode=true;
                searchTypeTree.AddRootNode();      
            }
        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.CONDITION_SEARCH_KEY_EQP)
            {
                e.TargetNode.Nodes.Clear();
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            //DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            //if (dcValue == null) return;

            //if (dcValue.ContextId == Definition.DynamicCondition_Search_key.AREA)
            //{

            //    e.Node.Nodes.Clear();

            //    string sLineRawid = string.Empty;
            //    string sAreaRawid = string.Empty;

            //    System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
            //    for (int i = 0; i < e.Node.Level; i++)
            //    {
            //        if (lstPathValue[i].ContextId.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            //        {
            //            sLineRawid = lstPathValue[i].Value;
            //        }
            //    }


            //    XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);

            //    //dcpTree.EqpID = dcValue.Value;
            //    spcModelTree.LineRawid = sLineRawid;
            //    spcModelTree.AreaRawid = dcValue.Value;

            //    spcModelTree.IsShowCheck = true;
            //    spcModelTree.RecipeTypeCode = RecipeType.NONE;
            //    spcModelTree.ParamTypeCode = ParameterType.NONE;
            //    spcModelTree.IsLastNode = true;

            //    spcModelTree.AddRootNode(e.Node);
            //}
        }

        void btv_AfterExpand(object sender, TreeViewEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.CONDITION_SEARCH_KEY_LINE)
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
