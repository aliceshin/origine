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
    /// <summary>
    /// Multivariate Model 에서 사용 (Step + Mva Model) Tree
    /// </summary>
    public class XSPCOperationTreeControl : IAddChildNodeToDCTreeView
    {
        private Condition.eSPCWebService.eSPCWebService _spcWebService = null;
        private LinkedList _llstParent = null;

        private BTreeView _btvCondition = null;
        private MultiLanguageHandler _mlthandler;

        //private string _sEqpRawid = string.Empty;
        //private string _sDcpRawid = string.Empty;
        private string _sLineRawid = string.Empty;
        private string _sAreaRawid = string.Empty;
        private string _sEqpID = string.Empty;
        private string _sDcpID = string.Empty;
        private string _sOperationID = string.Empty;
        private string _sProductID = string.Empty;
        private string _sSearchTypeCode = string.Empty;
        
        
        

        #region : IAddChildNodeToDCTreeView Members

        /// 
        /// DC에서 설정된 값으로 만들어진 BTreeView 인스턴스를 받아서
        /// 추가적으로 노드 혹은 이벤트를 연결한다.
        /// 
        /// 
        public void SetDCTree(BTreeView btv)
        {
            this._spcWebService = new BISTel.eSPC.Condition.eSPCWebService.eSPCWebService();
            this._mlthandler = MultiLanguageHandler.getInstance();

            _btvCondition = btv;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            
            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);
        }

        public void SetParentValue(LinkedList llParentValue)
        {
            _btvCondition.Nodes.Clear();

            _llstParent = llParentValue;

            if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                DataTable dtLine = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.LINE];
                 _sLineRawid = DCUtil.GetValueData(dtLine);

                DataTable dtArea = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.AREA];
                _sAreaRawid = DCUtil.GetValueData(dtArea);
                

                if (dtArea.Rows.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage(Definition.MSG_KEY_ALLOW_SINGLE_ONE), _mlthandler.GetVariable(Definition.LABEL_KEY_AREA)));
                    return;
                }

           
                XSPCOperationTree spcOperationTree = new XSPCOperationTree(_btvCondition);
                spcOperationTree.LineRawid = _sLineRawid;
                spcOperationTree.AreaRawid = _sAreaRawid;
                spcOperationTree.IsShowCheck = true;
                spcOperationTree.RecipeTypeCode = RecipeType.NONE;
                spcOperationTree.ParamTypeCode = ParameterType.NONE;
                spcOperationTree.IsLastNode = true;
                spcOperationTree.AddRootNode();                             
                                       
            }
                                   

            _btvCondition.CheckCountType = BTreeView.CheckCountTypes.Single;
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

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node);
            if (dcValue == null) return;


            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.OPERATION)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();

                _sOperationID = dcValue.Value;

                XSPCParamTreeControl spcParamType = new XSPCParamTreeControl();                           
                spcParamType.SetParentValue(_llstParent);                                
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
