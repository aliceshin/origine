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

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XModuleTree : TreeInterface
    {
        #region : Field

        private string _sEqpID = string.Empty;
        private string _sAreaRawid = string.Empty;
        private string _sLineRawid = string.Empty;
        private string _sMainModuleRawid = string.Empty;

        private TreeNode _treeNode = null;
        private int _TreeLevel = 0;

        private CommonUtility _ComUtil = new CommonUtility();

        #endregion

        #region : Initialization

        public XModuleTree(BTreeView btv)
        {
            this.XTreeView = btv;
        }

        #endregion

        #region : Public

        public void AddRootNode(TreeNode node)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;
            _TreeLevel = node.Level;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
            {
                AddModuleNode(tnCurrent, dcValue.Value);
            }

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void AddRootNode()
        {
            _treeNode = null;
            AddModuleNode();
        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);

            if (dcValue == null) return;

            if (dcValue.ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
            {
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)e.Node))
                e.Node.Nodes.Clear();

            int nodeLevel = 0;
            if (_treeNode != null)
            {
                nodeLevel = this._TreeLevel;//_treeNode.Level;
                if (this._TreeLevel > e.Node.Level) return;
            }

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

            TreeNode tnCurrent = e.Node;

            if (this.UseDefaultCondition.Equals(Definition.YES))
            {
                if (dcValue.ContextId == Definition.DynamicCondition_Search_key.DCP_ID)
                {
                    if (tnCurrent.Nodes.Count > 0) return;

                    if (dcValue.Value == this.DcpRawid)
                    {
                        AddModuleNode(tnCurrent, dcValue.Value);
                    }
                }
                else if (dcValue.ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                {
                    if (tnCurrent.Nodes.Count > 0) return;

                    AddSubModuleNode(tnCurrent, dcValue.Value);

                    if (this.RecipeTypeCode != RecipeType.NONE)
                    {
                        XProductTree productTree = new XProductTree(this.XTreeView);

                        productTree.ModuleRawid = dcValue.Value;
                        productTree.IsShowCheck = this.IsShowCheckProduct;
                        productTree.IsShowCheckRecipe = this.IsShowCheckRecipe;
                        productTree.RecipeTypeCode = this.RecipeTypeCode;
                        productTree.IsShowRecipeWildcard = this.IsShowRecipeWildcard;

                        productTree.AddRootNode(e.Node);
                    }

                    if (this.ParamTypeCode == ParameterType.TRACE)
                    {
                        XTraceParamTree traceTree = new XTraceParamTree(this.XTreeView);

                        traceTree.ModuleRawid = dcValue.Value;
                        traceTree.DcpRawid = this.DcpRawid;
                        traceTree.IsCheckParamType = this.IsCheckParamType;

                        traceTree.AddRootNode(e.Node);
                    }
                    else if (this.ParamTypeCode == ParameterType.TRACE_SUMMARY || this.ParamTypeCode == ParameterType.EVENT_SUMMARY)
                    {
                        XSummaryParamTree sumTree = new XSummaryParamTree(this.XTreeView);

                        sumTree.ModuleRawid = dcValue.Value;
                        sumTree.DcpRawid = this.DcpRawid;
                        sumTree.IsCheckParamType = this.IsCheckParamType;

                        sumTree.AddRootNode(e.Node);
                    }
                }
            }
            else
            {
                if (dcValue.ContextId.Contains(Definition.DynamicCondition_Search_key.MODULE))
                {
                    if (tnCurrent.Nodes.Count > 0) return;

                    AddSubModuleNode(tnCurrent, dcValue.Value);

                    if (this.RecipeTypeCode != RecipeType.NONE)
                    {
                        XRecipeTree recipeTree = new XRecipeTree(this.XTreeView);

                        recipeTree.ModuleRawid = dcValue.Value;
                        recipeTree.IsShowCheck = this.IsShowCheckRecipe;
                        recipeTree.IsShowRecipeWildcard = this.IsShowRecipeWildcard;

                        if (this.RecipeTypeCode == RecipeType.STEP)
                        {
                            recipeTree.IsStepNode = true;
                            recipeTree.IsMvaModelNode = false;
                            recipeTree.IsSpecGroupNode = false;
                        }
                        else if (this.RecipeTypeCode == RecipeType.SPEC_GROUP)
                        {
                            recipeTree.IsStepNode = false;
                            recipeTree.IsMvaModelNode = false;
                            recipeTree.IsSpecGroupNode = true;
                        }
                        else if (this.RecipeTypeCode == RecipeType.MVA_MODEL)
                        {
                            recipeTree.IsStepNode = true;
                            recipeTree.IsMvaModelNode = true;
                            recipeTree.IsSpecGroupNode = false;
                        }
                        else if (this.RecipeTypeCode == RecipeType.ALL)
                        {
                            recipeTree.IsStepNode = true;
                            recipeTree.IsMvaModelNode = false;
                            recipeTree.IsSpecGroupNode = true;
                        }
                        else
                        {
                            recipeTree.IsStepNode = false;
                            recipeTree.IsMvaModelNode = false;
                            recipeTree.IsSpecGroupNode = false;
                        }

                        recipeTree.AddRootNode(e.Node);
                    }

                   
                }
            }
        }

        #endregion

        #region : Private

        private void AddModuleNode()
        {
            try
            {
                LinkedList llstData = new LinkedList();

                llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.DCP_ID, this.DcpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, this.EqpID);

                byte[] baData = llstData.GetSerialData();
                DataSet ds = _wsSPC.GetModuleByEQP(baData);

                if (ds == null || ds.Tables.Count <= 0)
                    return;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sRawID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();
                    string sModule = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.ALIAS].ToString();
                    string sModuleID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODULE_ID].ToString();
                    string sParentModuleID = ds.Tables[0].Rows[i]["parent_moduleid"].ToString();

                    if (sParentModuleID == string.Empty)
                        _sMainModuleRawid = sModuleID;

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.MODULE, sModuleID, sModule);

                    btn.IsVisibleCheckBox = this.IsShowCheck;
                    btn.IsFolder = false;
                    btn.IsVisibleNodeType = true;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.MODULE);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.MODULE;
                    DCValueOfTree dcValue = TreeDCUtil.GetDCValue(btn);

                    dcValue.AdditionalValue.Add(Definition.DynamicCondition_Search_key.ADDTIONALVALUEDATA, sRawID);

                    dcValue.Tag = sModuleID;

                    if (this.IsLastNode)
                    {
                        if (_sMainModuleRawid != sModule)
                        {
                            if (llstData.Contains(Definition.DynamicCondition_Condition_key.MODULE_ID))
                                llstData[Definition.DynamicCondition_Condition_key.MODULE_ID] = sModuleID;
                            else
                                llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, sModuleID);

                            byte[] data = llstData.GetSerialData();
                            DataSet dsSubModule = _wsSPC.GetSubModuleByEQP(data);
                            if (dsSubModule.Tables.Count > 0 && dsSubModule.Tables[0].Rows.Count > 0)
                                btn.Nodes.Add(BTreeView.CreateDummyNode());
                        }
                    }
                    else
                    {
                        btn.Nodes.Add(BTreeView.CreateDummyNode());
                    }

                    this.XTreeView.Nodes.Add(btn);
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }

        private void AddModuleNode(TreeNode tnCurrent, string dcpRawid)
        {
            try
            {
                if (tnCurrent.Nodes.Count > 0)
                    return;

                LinkedList llstData = new LinkedList();

                llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.DCP_ID, this.DcpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, this.EqpID);

                byte[] baData = llstData.GetSerialData();
                DataSet ds = _wsSPC.GetModuleByEQP(baData);

                if (ds == null || ds.Tables.Count <= 0)
                    return;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sModule = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.ALIAS].ToString();
                    string sModuleID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODULE_ID].ToString();
                    string sParentModuleID = ds.Tables[0].Rows[i]["parent_moduleid"].ToString();
                    string sRawID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();

                    if (sParentModuleID == string.Empty)
                        _sMainModuleRawid = sModuleID;

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.MODULE, sModuleID, sModule);
                    btn.IsVisibleCheckBox = this.IsShowCheck;
                    btn.IsFolder = false;
                    btn.IsVisibleNodeType = true;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.MODULE);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.MODULE;
                    DCValueOfTree dcValue = TreeDCUtil.GetDCValue(btn);
                    dcValue.AdditionalValue.Add(Definition.DynamicCondition_Search_key.ADDTIONALVALUEDATA, sRawID);
                    dcValue.Tag = sModuleID;

                    if (this.IsLastNode)
                    {
                        if (_sMainModuleRawid != sModule)
                        {
                            if (llstData.Contains(Definition.DynamicCondition_Condition_key.MODULE_ID))
                                llstData[Definition.DynamicCondition_Condition_key.MODULE_ID] = sModuleID;
                            else
                                llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, sModuleID);

                            byte[] data = llstData.GetSerialData();
                            DataSet dsSubModule = _wsSPC.GetSubModuleByEQP(data);
                            if (dsSubModule.Tables.Count > 0 && dsSubModule.Tables[0].Rows.Count > 0)
                                btn.Nodes.Add(BTreeView.CreateDummyNode());
                        }
                    }
                    else
                    {
                        btn.Nodes.Add(BTreeView.CreateDummyNode());
                    }

                    tnCurrent.Nodes.Add(btn);
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }

        private void AddSubModuleNode(TreeNode tnCurrent, string rawid)
        {
            try
            {
                if (rawid == this.EqpID)
                    return;

                LinkedList llstData = new LinkedList();

                llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.EqpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.DCP_ID, this.DcpID);
                llstData.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, rawid);

                byte[] baData = llstData.GetSerialData();
                DataSet ds = _wsSPC.GetSubModuleByEQP(baData);

                if (ds == null || ds.Tables.Count <= 0)
                    return;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sRawid = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.RAWID].ToString();
                    string sModule = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.ALIAS].ToString();
                    string sModuleID = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODULE_ID].ToString();
                    string sParentModuleID = ds.Tables[0].Rows[i]["parent_moduleid"].ToString();
                    string sLevel = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODULE_LEVEL].ToString();

                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.MODULE + "_" + sLevel, sModuleID, sModule);
                    btn.IsVisibleCheckBox = this.IsShowCheck;
                    btn.IsFolder = false;
                    btn.IsVisibleNodeType = true;
                    btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.MODULE);
                    btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.MODULE;

                    DCValueOfTree dcValue = TreeDCUtil.GetDCValue(btn);
                    dcValue.AdditionalValue.Add(Definition.DynamicCondition_Search_key.ADDTIONALVALUEDATA, sRawid);


                    if (this.IsLastNode)
                    {
                        llstData[Definition.DynamicCondition_Condition_key.MODULE_ID] = sModuleID;
                        byte[] data = llstData.GetSerialData();
                        DataSet dsSubModule = _wsSPC.GetSubModuleByEQP(data);
                        if (dsSubModule != null && dsSubModule.Tables.Count > 0 && dsSubModule.Tables[0].Rows.Count > 0)
                            btn.Nodes.Add(BTreeView.CreateDummyNode());
                    }
                    else
                    {
                        btn.Nodes.Add(BTreeView.CreateDummyNode());
                    }

                    tnCurrent.Nodes.Add(btn);
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPName, ex);
            }
        }

        #endregion

    }
}
