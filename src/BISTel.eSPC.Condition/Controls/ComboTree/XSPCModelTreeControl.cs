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
    public class XSPCModelTreeControl : IAddChildNodeToDCTreeView
    {
        private Condition.eSPCWebService.eSPCWebService _spcWebService = null;
        private LinkedList _llstParent = null;

        private BTreeView _btvCondition = null;
        private MultiLanguageHandler _mlthandler;

        private string _sLineRawid = string.Empty;
        private string _sAreaRawid = string.Empty;
        private string _sParamTypeCD = string.Empty;
        private string _sEQPModelName = string.Empty;
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

            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);

            //SPC MODEL LEVEL을 가져옴
            LinkedList llstCondtion = new LinkedList();
            if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelTreeControl")
            {
                llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelTreeControl")
            {
                llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MET_MODEL_LEVEL");
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelTreeControl")
            {
                llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_ATT_MODEL_LEVEL");
            }
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
            #region Condition Table 구조 Setting

            bool bUseComponent = Configuration.GetConfig("configuration/general/componentcondition").GetAttribute("isuse", false);

            if (llParentValue.Count == 0 && ComponentCondition.GetInstance().Count > 0 && bUseComponent)
            {
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(Definition.CONDITION_SEARCH_KEY_VALUEDATA);
                dtTemp.Columns.Add(Definition.CONDITION_SEARCH_KEY_DISPLAYDATA);
                dtTemp.Columns.Add(Definition.CONDITION_SEARCH_KEY_CHECKED);

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_SITE))
                {
                    DataTable dtSite = dtTemp.Clone();
                    dtSite.Columns.Add(Definition.CONDITION_KEY_SITE);

                    DataRow dr = dtSite.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "F";
                    dr[Definition.CONDITION_KEY_SITE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dtSite.Rows.Add(dr);

                    llParentValue.Add(Definition.DynamicCondition_Search_key.SITE, dtSite);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_FAB))
                {
                    DataTable dtFab = dtTemp.Clone();
                    dtFab.Columns.Add(Definition.CONDITION_KEY_FAB);
                    dtFab.Columns.Add(Definition.CONDITION_SEARCH_KEY_SITE);

                    DataRow dr = dtFab.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "F";
                    dr[Definition.CONDITION_KEY_FAB] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_SITE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dtFab.Rows.Add(dr);

                    llParentValue.Add(Definition.DynamicCondition_Search_key.FAB, dtFab);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_LINE))
                {
                    DataTable dtLine = dtTemp.Clone();
                    dtLine.Columns.Add(Definition.CONDITION_KEY_LINE);
                    dtLine.Columns.Add(Definition.CONDITION_SEARCH_KEY_FAB);
                    dtLine.Columns.Add(Definition.CONDITION_SEARCH_KEY_SITE);

                    DataRow dr = dtLine.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "F";
                    dr[Definition.CONDITION_KEY_LINE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE);
                    dr[Definition.CONDITION_SEARCH_KEY_FAB] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_SITE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dtLine.Rows.Add(dr);

                    llParentValue.Add(Definition.DynamicCondition_Search_key.LINE, dtLine);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_AREA))
                {
                    DataTable _dtArea = dtTemp.Clone();
                    _dtArea.Columns.Add(Definition.CONDITION_KEY_AREA);
                    _dtArea.Columns.Add(Definition.CONDITION_SEARCH_KEY_LINE);
                    _dtArea.Columns.Add(Definition.CONDITION_SEARCH_KEY_FAB);
                    _dtArea.Columns.Add(Definition.CONDITION_SEARCH_KEY_SITE);

                    DataRow dr = _dtArea.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_AREA_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_AREA);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "T";
                    dr[Definition.CONDITION_KEY_AREA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_AREA);
                    dr[Definition.CONDITION_SEARCH_KEY_LINE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_FAB] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_SITE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    _dtArea.Rows.Add(dr);

                    llParentValue.Add(Definition.DynamicCondition_Search_key.AREA, _dtArea);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
                {
                    DataTable dtEQPModel = dtTemp.Clone();
                    dtEQPModel.Columns.Add(Definition.CONDITION_KEY_EQPMODEL);
                    dtEQPModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_AREA);
                    dtEQPModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_LINE);
                    dtEQPModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_FAB);
                    dtEQPModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_SITE);

                    DataRow dr = dtEQPModel.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_EQPMODEL);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_EQPMODEL);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "T";
                    dr[Definition.CONDITION_KEY_EQPMODEL] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_EQPMODEL);
                    dr[Definition.CONDITION_SEARCH_KEY_AREA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_AREA_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_LINE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_FAB] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FAB);
                    dr[Definition.CONDITION_SEARCH_KEY_SITE] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SITE);
                    dtEQPModel.Rows.Add(dr);

                    llParentValue.Add(Definition.CONDITION_SEARCH_KEY_EQPMODEL, dtEQPModel);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_FILTER))
                {
                    DataTable _dtFilter = new DataTable();
                    _dtFilter.Columns.Add(Definition.CONDITION_SEARCH_KEY_VALUEDATA);
                    _dtFilter.Columns.Add(Definition.CONDITION_SEARCH_KEY_DISPLAYDATA);

                    DataRow dr = _dtFilter.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FILTER);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_FILTER);
                    _dtFilter.Rows.Add(dr);

                    llParentValue.Add(Definition.CONDITION_KEY_FILTER, _dtFilter);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_SPC_MODEL_LIST))
                {
                    DataTable dtSPCModelList = dtTemp.Clone();

                    DataRow dr = dtSPCModelList.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = "SPC MODEL LIST";
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = "SPC MODEL LIST";
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "F";

                    dtSPCModelList.Rows.Add(dr);

                    llParentValue.Add("SPC MODEL LIST", dtSPCModelList);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_GROUP_NAME))
                {
                    DataTable dtGroup = dtTemp.Clone();
                    dtGroup.Columns.Add("SPC MODEL LIST");

                    DataRow dr = dtGroup.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_GROUP_NAME);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_GROUP_NAME);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "F";
                    dr["SPC MODEL LIST"] = "SPC MODEL LIST";

                    dtGroup.Rows.Add(dr);

                    llParentValue.Add(Definition.CONDITION_KEY_GROUP_NAME, dtGroup);
                }

                if (ComponentCondition.GetInstance().Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                {
                    DataTable dtSPCModel = dtTemp.Clone();
                    dtSPCModel.Columns.Add(Definition.CONDITION_KEY_LOCATION_RAWID);
                    dtSPCModel.Columns.Add(Definition.CONDITION_KEY_AREA_RAWID);
                    dtSPCModel.Columns.Add(Definition.CONDITION_KEY_GROUP_NAME);
                    dtSPCModel.Columns.Add("SPC MODEL LIST");

                    DataRow dr = dtSPCModel.NewRow();
                    dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA] = ComponentCondition.GetInstance().GetValue("ESPC_" + Definition.CONDITION_SEARCH_KEY_MODEL_CONFIG_RAWID);
                    dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_SPCMODEL);
                    dr[Definition.CONDITION_SEARCH_KEY_CHECKED] = "T";
                    dr[Definition.CONDITION_KEY_LOCATION_RAWID] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_LINE_RAWID);
                    dr[Definition.CONDITION_KEY_AREA_RAWID] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_AREA_RAWID);
                    dr[Definition.CONDITION_KEY_GROUP_NAME] = ComponentCondition.GetInstance().GetValue(Definition.CONDITION_SEARCH_KEY_GROUP_NAME);
                    dr["SPC MODEL LIST"] = "SPC MODEL LIST";

                    dtSPCModel.Rows.Add(dr);

                    llParentValue.Add(Definition.CONDITION_SEARCH_KEY_SPCMODEL, dtSPCModel);
                }
            }

            #endregion

            _btvCondition.Nodes.Clear();

            _llstParent = llParentValue;


            DataTable dt = null;
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.LINE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.LINE];
                _sLineRawid = DCUtil.GetValueData(dt);
            }

            DataTable dtArea = null;
            if (_llstParent.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dtArea = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.AREA];
                this._sAreaRawid = DataUtil.GetConditionKeyDataList(dtArea, DCUtil.VALUE_FIELD);
            }

            DataTable dtFilter = null;
            this._sFilter = "";
            if (_llstParent.Contains(Definition.CONDITION_KEY_FILTER))
            {
                dtFilter = (DataTable)_llstParent[Definition.CONDITION_KEY_FILTER];
                this._sFilter = DataUtil.GetConditionKeyDataList(dtFilter, DCUtil.VALUE_FIELD);
            }

            if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_PARAM_TYPE))
            {
                dt = (DataTable)_llstParent[Definition.DynamicCondition_Search_key.PARAM_TYPE];
                _sParamTypeCD = DCUtil.GetValueData(dt);

                _btvCondition.Nodes.Clear();
                if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelTreeControl")
                {
                    XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                    spcModelTree.LineRawid = _sLineRawid;
                    spcModelTree.AreaRawid = _sAreaRawid;
                    if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        spcModelTree.EqpModelName = _sEQPModelName;
                    }
                    spcModelTree.ParamTypeCD = _sParamTypeCD;
                    spcModelTree.IsShowCheck = true;
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;
                    spcModelTree.FilterValue = this._sFilter;
                    spcModelTree.IsLastNode = true;
                    spcModelTree.AddRootNode();
                }
                else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelTreeControl")
                {
                    BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree(_btvCondition);
                    spcModelTree.LineRawid = _sLineRawid;
                    spcModelTree.AreaRawid = _sAreaRawid;
                    if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        spcModelTree.EqpModelName = _sEQPModelName;
                    }
                    spcModelTree.ParamTypeCD = _sParamTypeCD;
                    spcModelTree.IsShowCheck = true;
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;
                    spcModelTree.FilterValue = this._sFilter;
                    spcModelTree.IsLastNode = true;
                    spcModelTree.AddRootNode();
                }
                else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelTreeControl")
                {
                    BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree(_btvCondition);
                    spcModelTree.LineRawid = _sLineRawid;
                    spcModelTree.AreaRawid = _sAreaRawid;
                    if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        spcModelTree.EqpModelName = _sEQPModelName;
                    }
                    spcModelTree.ParamTypeCD = _sParamTypeCD;
                    spcModelTree.IsShowCheck = true;
                    spcModelTree.RecipeTypeCode = RecipeType.NONE;
                    spcModelTree.ParamTypeCode = ParameterType.NONE;
                    spcModelTree.FilterValue = this._sFilter;
                    spcModelTree.IsLastNode = true;
                    spcModelTree.AddRootNode();
                }
            }
            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
                {
                    _btvCondition.Nodes.Clear();

                    dt = (DataTable)_llstParent[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                    this._sEQPModelName = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD, true);
                    if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        spcModelTree.EqpModelName = _sEQPModelName;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        spcModelTree.EqpModelName = _sEQPModelName;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                        {
                            spcModelTree.EqpModelName = _sEQPModelName;
                        }
                        spcModelTree.ParamTypeCD = _sParamTypeCD;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                }
            }
            else
            {
                if (_llstParent.Contains(Definition.CONDITION_SEARCH_KEY_AREA) || _llstParent.Contains(Definition.CONDITION_KEY_AREA))
                {
                    _btvCondition.Nodes.Clear();
                    if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        XSPCModelTree spcModelTree = new XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        spcModelTree.ParamTypeCD = "MET";
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                    else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.XSPCModelTreeControl")
                    {
                        BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree spcModelTree = new BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree(_btvCondition);
                        spcModelTree.LineRawid = _sLineRawid;
                        spcModelTree.AreaRawid = _sAreaRawid;
                        if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                        {
                            spcModelTree.EqpModelName = _sEQPModelName;
                        }
                        spcModelTree.ParamTypeCD = _sParamTypeCD;
                        spcModelTree.IsShowCheck = true;
                        spcModelTree.RecipeTypeCode = RecipeType.NONE;
                        spcModelTree.ParamTypeCode = ParameterType.NONE;
                        spcModelTree.FilterValue = this._sFilter;
                        spcModelTree.IsLastNode = true;
                        spcModelTree.AddRootNode();
                    }
                }
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
            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node);
                if (dcValue == null) return;

                XEQPModelTree eqpModelTree = new XEQPModelTree(_btvCondition);
                eqpModelTree.LineRawid = _sLineRawid;
                eqpModelTree.AreaRawid = _sAreaRawid;
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
