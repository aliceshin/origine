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
    public class XSPCModelsTreeControl : IAddChildNodeToDCTreeView
    {
        private eSPCWebService.eSPCWebService _spcWebService = null;
        private LinkedList _llstParent = null;

        private BTreeView _btvCondition = null;
        private MultiLanguageHandler _mlthandler;
        
        private string _sLineRawid = string.Empty;
        private string _sAreaRawid = string.Empty;
        private DataTable _dtLine = null;
        private DataTable _dtArea = null;
        private string _sParamTypeCD = string.Empty;
        private string _sEQPModelName = string.Empty;
        private string _sEqpID = string.Empty;        
        private string _sFab = string.Empty;
        private string _sSite = string.Empty;
        private string _sSPCModelLevel = string.Empty;
        private string _sFilter = string.Empty;
        

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
            
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            btv.AfterExpand += new TreeViewEventHandler(btv_AfterExpand);
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            
            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);
           // btv.CheckCountType = BTreeView.CheckCountTypes.Single;

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

           // _sSPCModelLevel = Definition.CONDITION_KEY_EQP_MODEL;
        }
        
           
        public void SetParentValue(LinkedList llParentValue)
        {

            _llstParent = llParentValue;
            DataTable dt = null;            
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.PARAM_TYPE];
                _sParamTypeCD = DCUtil.GetValueData(dt);
            }
                        
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.SITE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.SITE];
                this._sSite = DCUtil.GetValueData(dt);                            
            }

            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.FAB))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.FAB];
                _sFab = DCUtil.GetValueData(dt);
            }
            
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.LINE))
            {               
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.LINE];
                _dtLine = dt;
                _sLineRawid = DCUtil.GetValueData(dt);
            }
            else
                _dtLine = null;

            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.AREA];
                _dtArea = dt;                    
                _sAreaRawid = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);
            }
            else
                _dtArea = null;

            DataTable dtFilter = null;
            this._sFilter = "";
            if (_llstParent.Contains("FILTER"))
            {
                dtFilter = (DataTable)_llstParent["FILTER"];
                this._sFilter = DataUtil.GetConditionKeyDataList(dtFilter, DCUtil.VALUE_FIELD);
            }


            _btvCondition.Nodes.Clear();

            if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_PARAM_TYPE))
            {
                XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                spcModelTree.dtLine = _dtLine;
                spcModelTree.dtArea = _dtArea;
                if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                {
                    spcModelTree.EqpModelName = _sEQPModelName;
                }
                spcModelTree.ParamTypeCD = _sParamTypeCD;
                spcModelTree.IsShowCheck = true;
                spcModelTree.RecipeTypeCode = RecipeType.NONE;
                spcModelTree.ParamTypeCode = ParameterType.NONE;
                spcModelTree.IsLastNode = true;
                spcModelTree.FilterValue = _sFilter;
                spcModelTree.AddRootNode();

                ToolStripMenuItem tsmi_SelectAll = new ToolStripMenuItem();
                tsmi_SelectAll.Text = "Select All";
                ToolStripMenuItem tsmi_ClearAll = new ToolStripMenuItem();
                tsmi_ClearAll.Text = "Clear Selection";

                this._btvCondition.AddContextMenu(tsmi_SelectAll);
                this._btvCondition.AddContextMenu(tsmi_ClearAll);

                tsmi_SelectAll.Click += new EventHandler(tsmi_SelectAll_Click);
                tsmi_ClearAll.Click += new EventHandler(tsmi_ClearAll_Click);
            }

            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                if (_llstParent.Contains(Definition.DynamicCondition_Search_key.EQPMODEL))
                {
                    dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.EQPMODEL];                                        
                    this._sEQPModelName = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD, true);
                    if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.EqpModelName = _sEQPModelName;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.EqpModelName = _sEQPModelName;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = _sParamTypeCD;
                        spcModelTree.EqpModelName = _sEQPModelName;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.AddRootNode();
                    }
                    if (_btvCondition.CheckCountType == BTreeView.CheckCountTypes.Multi)
                    {
                        ToolStripMenuItem tsmi_SelectAll = new ToolStripMenuItem();
                        tsmi_SelectAll.Text = "Select All";
                        ToolStripMenuItem tsmi_ClearAll = new ToolStripMenuItem();
                        tsmi_ClearAll.Text = "Clear Selection";

                        this._btvCondition.AddContextMenu(tsmi_SelectAll);
                        this._btvCondition.AddContextMenu(tsmi_ClearAll);

                        tsmi_SelectAll.Click += new EventHandler(tsmi_SelectAll_Click);
                        tsmi_ClearAll.Click += new EventHandler(tsmi_ClearAll_Click);
                    }
                }
                
            }else
            {
                if (_llstParent.Contains(Definition.DynamicCondition_Search_key.AREA))
                {
                    if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelsTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.dtLine = _dtLine;
                        spcModelTree.dtArea = _dtArea;
                        spcModelTree.ParamTypeCD = _sParamTypeCD;
                        spcModelTree.FilterValue = _sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    if (_btvCondition.CheckCountType == BTreeView.CheckCountTypes.Multi)
                    {
                        ToolStripMenuItem tsmi_SelectAll = new ToolStripMenuItem();
                        tsmi_SelectAll.Text = "Select All";
                        ToolStripMenuItem tsmi_ClearAll = new ToolStripMenuItem();
                        tsmi_ClearAll.Text = "Clear Selection";

                        this._btvCondition.AddContextMenu(tsmi_SelectAll);
                        this._btvCondition.AddContextMenu(tsmi_ClearAll);

                        tsmi_SelectAll.Click += new EventHandler(tsmi_SelectAll_Click);
                        tsmi_ClearAll.Click += new EventHandler(tsmi_ClearAll_Click);
                    }
                }                        
            }
        }

        void tsmi_ClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this._btvCondition.Nodes.Count; i++)
            {
                BTreeNode node = (BTreeNode)this._btvCondition.Nodes[i];
                node.Checked = false;
            }
        }

        void tsmi_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this._btvCondition.Nodes.Count; i++)
            {
                BTreeNode node = (BTreeNode)this._btvCondition.Nodes[i];
                node.Checked = true;
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
        
        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {      
                XEQPModelTree eqpModelTree = new XEQPModelTree(_btvCondition);
                eqpModelTree.LineRawid = _sLineRawid;
                eqpModelTree.AreaRawid = _sAreaRawid;
                eqpModelTree.ParamTypeCD = _sParamTypeCD;
                eqpModelTree.IsShowCheck = false;
                eqpModelTree.IsSPCModelTree = true;

                eqpModelTree.Tree_BeforeExpand(sender, e);
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
