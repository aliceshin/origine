using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.eSPC.Condition.Controls.Tree;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    class ComboTreeControl : IAddChildNodeToDCTreeView
    {
        #region : Field

        private string _sUseDefaultCondition = "";
        private eSPCWebService.eSPCWebService _spcWebService = null;
        private BTreeView _btvCondition = null;

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
            this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            _btvCondition = btv;
            //_btvCondition.CheckCountType = BTreeView.CheckCountTypes.SingleByNode;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);

            _btvCondition.AddImageForLevel(0, _btvCondition.ImageList.Images[0], _btvCondition.ImageList.Images[0]);
            _btvCondition.AddImageForLevel(1, _btvCondition.ImageList.Images[1], _btvCondition.ImageList.Images[1]);
            _btvCondition.AddImageForLevel(2, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[2]);
            _btvCondition.AddImageForLevel(3, _btvCondition.ImageList.Images[3], _btvCondition.ImageList.Images[3]);
            _btvCondition.AddImageForLevel(4, _btvCondition.ImageList.Images[4], _btvCondition.ImageList.Images[4]);

            btv.CheckCountType = BTreeView.CheckCountTypes.Single;

            //InitializeDefaultCondition();
        }

        public void SetParentValue(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llParentValue)
        {
        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
            {
                e.TargetNode.Nodes.Clear();
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            //if (this._sUseDefaultCondition.Equals(Definition.VARIABLE_Y))
            //{
            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                string sLineRawid = string.Empty;
                string sAreaRawid = string.Empty;
                string sEqpID = string.Empty;

                System.Collections.Generic.List<DCValueOfTree> lstPathValue = TreeDCUtil.GetDCValueOfAllParent(e.Node); // 상위 노드의 선택된 값들을 가져온다.
                for (int i = 0; i < e.Node.Level; i++)
                {
                    if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.LINE))
                    {
                        sLineRawid = lstPathValue[i].Value;
                    }
                    else if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.AREA))
                    {
                        sAreaRawid = lstPathValue[i].Value;
                    }
                    else if (lstPathValue[i].ContextId.Contains(Definition.DynamicCondition_Search_key.EQP_ID))
                    {
                        sEqpID = lstPathValue[i].Value;
                    }
                }

                XDcpTree dcpTree = new XDcpTree(_btvCondition);
                dcpTree.EqpRawid = dcValue.Value;
                dcpTree.EqpID = dcValue.Value;
                dcpTree.LineRawid = sLineRawid;
                dcpTree.AreaRawid = sAreaRawid;

                dcpTree.IsShowCheck = true;
                dcpTree.RecipeTypeCode = RecipeType.NONE;
                dcpTree.ParamTypeCode = ParameterType.NONE;
                dcpTree.IsLastNode = true;

                dcpTree.AddRootNode(e.Node);
            }

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.DCP_ID)
            {
                XDcpTree dcpTree = new XDcpTree(_btvCondition);

                dcpTree.EqpRawid = dcValue.Value;

                dcpTree.IsShowCheck = true;
                dcpTree.RecipeTypeCode = RecipeType.NONE;
                dcpTree.ParamTypeCode = ParameterType.NONE;
                dcpTree.IsLastNode = true;

                dcpTree.Tree_BeforeExpand(sender, e);
            }
            //}
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
