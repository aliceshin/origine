using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Tree;
using System.IO;
using System.Net;
using System.Drawing;
using System.Data;

namespace BISTel.eSPC.Condition.Controls
{
    /// <summary>
    /// Trace Data View 에서 사용 (Trace parameter) Tree
    /// </summary>
    public class XTraceParamControl : IAddChildNodeToDCTreeView
    {
        private string _sUseDefaultCondition = "";
        private eSPCWebService.eSPCWebService _fdcWebService = null;

        private BTreeView _btvCondition = null;

        private string _sDcpRawid = string.Empty;
        private string _sEqpRawid = string.Empty;

        #region IAddChildNodeToDCTreeView Members

        /// 
        /// DC에서 설정된 값으로 만들어진 BTreeView 인스턴스를 받아서
        /// 추가적으로 노드 혹은 이벤트를 연결한다.
        /// 
        /// 
        public void SetDCTree(BTreeView btv)
        {
            this._fdcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            _btvCondition = btv;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);

            _btvCondition.AddImageForLevel(0, _btvCondition.ImageList.Images[0], _btvCondition.ImageList.Images[0]);
            _btvCondition.AddImageForLevel(1, _btvCondition.ImageList.Images[1], _btvCondition.ImageList.Images[1]);
            _btvCondition.AddImageForLevel(2, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[2]);
            _btvCondition.AddImageForLevel(3, _btvCondition.ImageList.Images[3], _btvCondition.ImageList.Images[3]);
            _btvCondition.AddImageForLevel(4, _btvCondition.ImageList.Images[4], _btvCondition.ImageList.Images[4]);

            InitializeDefaultCondition();
        }

        public void SetParentValue(LinkedList llParentValue)
        {
            LinkedList llst = llParentValue;
        }

        /// 추가적인 정보 없음.
        public void GetParameter(LinkedList llData)
        {
        }

        /// 추가적인 정보 없음.
        public void RefreshCondition(LinkedList llData)
        {
        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
            {
                if (this._sUseDefaultCondition.Equals(Definition.VARIABLE_Y))
                {
                    if (dcValue.ContextId == Definition.DynamicCondition_Search_key.EQP_ID)
                    {
                        if (e.Node.Nodes.Count > 0) return;

                        e.Node.Nodes.Clear();

                        _sEqpRawid = dcValue.Value;

                        XDcpTree dcpTree = new XDcpTree(_btvCondition);

                        dcpTree.EqpRawid = dcValue.Value;
                        dcpTree.IsShowCheck = false;
                        dcpTree.IsShowCheckProduct = false;
                        dcpTree.IsCheckParamType = false;
                        dcpTree.RecipeTypeCode = RecipeType.NONE;
                        dcpTree.ParamTypeCode = ParameterType.TRACE;

                        dcpTree.AddRootNode(e.Node);
                    }
                }
                else
                {
                    if (e.Node.Nodes.Count > 0) return;

                    e.Node.Nodes.Clear();

                    _sEqpRawid = dcValue.Value;

                    XModuleTree moduleTree = new XModuleTree(_btvCondition);

                    moduleTree.EqpRawid = dcValue.Value;
                    moduleTree.IsShowCheck = false;
                    moduleTree.IsShowCheckProduct = false;
                    moduleTree.IsCheckParamType = false;
                    moduleTree.RecipeTypeCode = RecipeType.NONE;
                    moduleTree.ParamTypeCode = ParameterType.TRACE;

                    moduleTree.AddRootNode(e.Node);
                }
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.DCP_ID)
            {
                XDcpTree dcpTree = new XDcpTree(_btvCondition);

                _sDcpRawid = dcValue.Value;

                dcpTree.EqpRawid = _sEqpRawid;
                dcpTree.IsShowCheck = false;
                dcpTree.IsShowCheckProduct = false;
                dcpTree.IsCheckParamType = false;
                dcpTree.RecipeTypeCode = RecipeType.NONE;
                dcpTree.ParamTypeCode = ParameterType.TRACE;

                dcpTree.Tree_BeforeExpand(sender, e);
            }
            else if (dcValue.ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
            {
                XModuleTree moduleTree = new XModuleTree(_btvCondition);
                moduleTree.EqpRawid = _sEqpRawid;
                moduleTree.DcpRawid = _sDcpRawid;
                moduleTree.IsShowCheckProduct = false;
                moduleTree.IsCheckParamType = false;
                moduleTree.RecipeTypeCode = RecipeType.NONE;
                moduleTree.ParamTypeCode = ParameterType.TRACE;

                moduleTree.Tree_BeforeExpand(sender, e);
            }
            else if (dcValue.ContextId == Definition.DynamicCondition_Search_key.TRACE_PARAM_TYPE || dcValue.ContextId == Definition.DynamicCondition_Search_key.PARAM)
            {
                XTraceParamTree traceTree = new XTraceParamTree(_btvCondition);

                //traceTree.ModuleRawid = dcValue.Value;
                traceTree.DcpRawid = _sDcpRawid;
                traceTree.IsLastNode = true;
                traceTree.IsShowCheck = true;
                traceTree.IsCheckParamType = false;

                traceTree.Tree_BeforeExpand(sender, e);
            }
        }

        #endregion

        public void InitializeDefaultCondition()
        {
            DataSet dsCodeData = new DataSet();

            LinkedList lnkCondition = new LinkedList();
            lnkCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_DEFAULT_CONDITION);

            byte[] baDataUseDefault = lnkCondition.GetSerialData();

            dsCodeData = this._fdcWebService.GetCodeData(baDataUseDefault);

            if (dsCodeData.Tables.Count > 0)
            {
                this._sUseDefaultCondition = dsCodeData.Tables[0].Rows[0]["CODE"].ToString();
            }
        }

        #region IAddChildNodeToDCTreeView Members

        public bool IsAddNode()
        {
            return true;
        }

        #endregion

    }
}
