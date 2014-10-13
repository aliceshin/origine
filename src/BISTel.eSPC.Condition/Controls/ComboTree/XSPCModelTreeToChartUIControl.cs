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
    /// <summary>
    /// Multivariate Model 에서 사용 (Step + Mva Model) Tree
    /// </summary>
    public class XSPCModelTreeToChartUIControl : IAddChildNodeToDCTreeView
    {
        private eSPCWebService.eSPCWebService _spcWebService = null;
        private LinkedList _llstParent = null;

        private BTreeView _btvCondition = null;
        private MultiLanguageHandler _mlthandler;

        //private string _sEqpRawid = string.Empty;
        //private string _sDcpRawid = string.Empty;
        private string _sLineRawid = string.Empty;
        private string _sAreaRawid = string.Empty;
        private string _sParamTypeCD = string.Empty;
        private string _sEqpID = string.Empty;
        private string _sDcpID = string.Empty;
        private string _sSPCModelLevel = string.Empty;
        private string _sEQPModelName = string.Empty;        

        #region : IAddChildNodeToDCTreeView Members

        /// 
        /// DC에서 설정된 값으로 만들어진 BTreeView 인스턴스를 받아서
        /// 추가적으로 노드 혹은 이벤트를 연결한다.
        /// 
        /// 
        public void SetDCTree(BTreeView btv)
        {
            this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            _btvCondition = btv;
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            //btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            //btv.AfterExpand += new TreeViewEventHandler(btv_AfterExpand);
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);


            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);
            btv.CheckCountType = BTreeView.CheckCountTypes.Single;

            //SPC MODEL LEVEL을 가져옴
            LinkedList llstCondtion = new LinkedList();
            llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
            llstCondtion.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            //llstCondtion.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

            DataSet dsModelLevel = this._spcWebService.GetCodeData(llstCondtion.GetSerialData());

            if (DSUtil.CheckRowCount(dsModelLevel))
            {
                this._sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][COLUMN.CODE].ToString();
            }
        }

        public void SetParentValue(LinkedList llParentValue)
        {
            _btvCondition.Nodes.Clear();

            _llstParent = llParentValue;
            DataTable dt = null;


            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.PARAM_TYPE];
                _sParamTypeCD = DCUtil.GetValueData(dt);
            }


            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.LINE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.LINE];
                _sLineRawid = DCUtil.GetValueData(dt);
            }

            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dt = (DataTable)_llstParent[Definition.CONDITION_SEARCH_KEY_AREA];
                _sAreaRawid = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);             
            }
        

            if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_PARAM_TYPE))
            {
                _btvCondition.Nodes.Clear();
                XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                spcModelTree.LineRawid = _sLineRawid;
                spcModelTree.AreaRawid = _sAreaRawid;
                if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                {
                    spcModelTree.EqpModelName = _sEQPModelName;
                }
                spcModelTree.ParamTypeCD = _sParamTypeCD;
                spcModelTree.IsShowCheck = false;
                spcModelTree.RecipeTypeCode = RecipeType.NONE;
                spcModelTree.ParamTypeCode = ParameterType.NONE;
                spcModelTree.IsLastNode = false;
                spcModelTree.AddRootNode();

            }
            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
                {

                    dt = (DataTable)_llstParent[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                    this._sEQPModelName = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD, true);

                    XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                    spcModelTree.LineRawid = _sLineRawid;
                    spcModelTree.AreaRawid = _sAreaRawid;
                    spcModelTree.EqpModelName = _sEQPModelName;
                    spcModelTree.ParamTypeCD = _sParamTypeCD;
                    spcModelTree.IsShowCheck = false;
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;
                    spcModelTree.IsLastNode = false;
                    spcModelTree.AddRootNode();
                }
            }
            else
            {
                if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
                {
                    XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                    spcModelTree.LineRawid = _sLineRawid;
                    spcModelTree.AreaRawid = _sAreaRawid;
                    spcModelTree.ParamTypeCD = _sParamTypeCD;
                    spcModelTree.IsShowCheck = false;
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;
                    spcModelTree.IsLastNode = false;
                    spcModelTree.AddRootNode();
                }
            }
           

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

            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.SPCMODEL)
            {

                e.Node.Nodes.Clear();

                _sParamTypeCD = dcValue.Value;
                XSPCModelContextTree spcModelTree = new XSPCModelContextTree(_btvCondition);
                spcModelTree.LineRawid = _sLineRawid;
                spcModelTree.AreaRawid = _sAreaRawid;
                spcModelTree.ModelRawID = _sParamTypeCD;
                spcModelTree.IsShowCheck = true;
                spcModelTree.RecipeTypeCode = RecipeType.NONE;
                spcModelTree.ParamTypeCode = ParameterType.NONE;
                spcModelTree.IsLastNode = true;
                spcModelTree.AddRootNode(e.Node);
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
