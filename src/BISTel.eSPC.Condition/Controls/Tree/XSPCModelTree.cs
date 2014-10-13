using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Data;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.eSPC.Common;
using System.Drawing;
using System.Collections;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XSPCModelTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;
        BTreeNode btnRoot = null;
        public DataTable dtLine = null;
        public DataTable dtArea = null;
        Initialization _Initialization;
        BTreeNode btnSPCModel = null;

        #endregion

        #region : Initialization

        public XSPCModelTree(BTreeView btv)
        {
            this.XTreeView = btv;
            _Initialization = new Initialization();
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
        }

        #endregion

        #region : Public

        public void AddRootNode(TreeNode node)
        {
            if (BTreeView.IsOnlyDummyNode((BTreeNode)node))
                node.Nodes.Clear();

            TreeNode tnCurrent = node;
            _treeNode = node;

            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(node);

            if (tnCurrent.Nodes.Count <= 0)
                AddSPCModelNode(tnCurrent);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void AddRootNode()
        {
            _treeNode = null;
            AddSPCModelNode();
        }

        public void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            _treeNode = null;
            AddSPCModelNode();
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }

        #endregion

        #region : Private

        private string getReplace(string argValue)
        {
            if(string.IsNullOrEmpty(argValue))
            return argValue;
            else
            return "'"+argValue.Replace(",","','")+"'";        
        }
        private void AddSPCModelNode()
        {
            DataSet ds = null;
            DataSet dsGroup = new DataSet();

            if(this.dtArea == null)
            {
                LinkedList llstData = new LinkedList();

                llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, this.LineRawid);
                llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, this.AreaRawid);

                if (!string.IsNullOrEmpty(this.EqpModelName))
                    llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, EqpModelName);

                if (!string.IsNullOrEmpty(this.ParamTypeCD))
                    llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.ParamTypeCD);

                if (!string.IsNullOrEmpty(this.FilterValue))
                    llstData.Add("FILTER", this.FilterValue);

                if (this.LineRawid == null || this.LineRawid.Length == 0)
                    return;
                if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.Tree.XSPCModelTree")
                {
                    ds = _wsSPC.GetSPCModel(llstData.GetSerialData());
                }
                else if (this.GetType().FullName == "BISTel.eSPC.Condition.MET.Controls.Tree.XSPCModelTree")
                {
                    ds = _wsSPC.GetMETSPCModel(llstData.GetSerialData());
                }
                else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree")
                {
                    ds = _wsSPC.GetATTSPCModel(llstData.GetSerialData());
                }

                if (String.IsNullOrEmpty(this.ParamTypeCD))             //ATT
                {
                    dsGroup = _wsSPC.GetATTGroupList(llstData.GetSerialData());
                }

                else
                {
                    dsGroup = _wsSPC.GetGroupList(llstData.GetSerialData());
                }
                
            }
            else
            {
                List<string> lineRawids = new List<string>();
                List<string> areaRawids = new List<string>();

                foreach (DataRow dr in dtArea.Rows)
                {
                    areaRawids.Add(dr[DCUtil.VALUE_FIELD].ToString());
                    lineRawids.Add(dr[Definition.DynamicCondition_Search_key.LINE].ToString());
                }

                DataSet dsGroupList = new DataSet();

                for (int i = 0; i < dtArea.Rows.Count; i++)
                {
                    LinkedList llstData = new LinkedList();

                    llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawids[i]);
                    llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawids[i]);

                    if (String.IsNullOrEmpty(this.ParamTypeCD))
                    {
                        dsGroup = _wsSPC.GetATTGroupList(llstData.GetSerialData());
                    }

                    else
                    {
                        dsGroup = _wsSPC.GetGroupList(llstData.GetSerialData());
                    }

                    //if (dsGroupList.Tables.Count > 0)
                    //{
                        if (dsGroupList.Tables.Count == 0)
                        {
                            dsGroupList.Tables.Add(dsGroup.Tables[0].Copy());
                        }
                        else
                        {
                            dsGroupList.Tables[0].Merge(dsGroup.Tables[0].Copy());
                        }
                    //}
                }
                DataView dv = new DataView(dsGroupList.Tables[0]);
                dv.Sort = "GROUP_NAME ASC";
                dsGroup.Tables.Remove(dsGroup.Tables[0]);
                dsGroup.Tables.Add(dv.ToTable());
                
                ds = _wsSPC.GetSPCModelOfLines(lineRawids.ToArray(), areaRawids.ToArray(), EqpModelName, ParamTypeCD, FilterValue);
            }
            //SPC-1089 by stella - filter한 항목이 존재하지 않더라도 최상위 노드는 보이도록 수정.
            //if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count == 0)
            //{
            //    return;
            //}

            //SPC-880 by stella Group List 추가
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drSelect = ds.Tables[0].Select("GROUP_RAWID IS NULL", "SPC_MODEL_NAME ASC");

                if (drSelect.Length > 0)
                {
                    DataRow drModel = dsGroup.Tables[0].NewRow();
                    drModel[COLUMN.GROUP_NAME] = Definition.VARIABLE_UNASSIGNED_MODEL;
                    dsGroup.Tables[0].Rows.Add(drModel);
                }
            }

            ArrayList arrGroup = new ArrayList();

            if (this.btnSPCModel == null)
            {
                this.btnSPCModel = TreeDCUtil.CreateBTreeNode("SPC MODEL LIST", "SPC MODEL LIST", "SPC MODEL LIST");
                btnSPCModel.IsVisibleCheckBox = false;
                btnSPCModel.IsFolder = true;
                btnSPCModel.IsVisibleNodeType = true;
                this.XTreeView.Nodes.Add(btnSPCModel);
            }
            else
            {
                this.btnSPCModel.Nodes.Clear();
            }

            foreach (DataRow dr in dsGroup.Tables[0].Rows)
            {
                if (!arrGroup.Contains(dr[COLUMN.GROUP_NAME].ToString()))
                {
                    string sGroupName = dr[COLUMN.GROUP_NAME].ToString();
                    DataRow[] drGroup = dsGroup.Tables[0].Select(string.Format("GROUP_NAME = '{0}'", sGroupName), "GROUP_NAME ASC");

                    this.btnRoot = TreeDCUtil.CreateBTreeNode(dr[COLUMN.GROUP_NAME].ToString(), dr[COLUMN.GROUP_NAME].ToString(), dr[COLUMN.GROUP_NAME].ToString());
                    btnRoot.IsVisibleCheckBox = false;
                    btnRoot.IsFolder = true;
                    btnRoot.IsVisibleNodeType = true;
                    ((DCValueOfTree)btnRoot.Tag).ContextId = Definition.CONDITION_KEY_GROUP_NAME;
                    btnSPCModel.Nodes.Add(btnRoot);
                   
                    arrGroup.Add(btnRoot.Text);
                    //added by enkim chart mode group

                    //SPC-1292, KBLEE, ATT일 때와 아닐 때 각각의 xml file에서 설정값 불러오게 하기
                    bool isATTTree = this._Initialization.GetChartModeGrouping(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC) &&
                        this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree";
                    bool isNormalTree = this._Initialization.GetChartModeGrouping(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC) &&
                        this.GetType().FullName != "BISTel.eSPC.Condition.ATT.Controls.Tree.XSPCModelTree";

                    if (isATTTree || isNormalTree)
                    {

                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
                        DataSet ds_chartmode = _wsSPC.GetCodeData(llstCondition.GetSerialData());

                        if (ds_chartmode == null || ds_chartmode.Tables[0].Rows.Count == 0)
                        {
                            return;
                        }
                        else
                        {
                            //string sGroupName = dr[COLUMN.GROUP_NAME].ToString();
                            //DataRow[] drGroup = dsGroup.Tables[0].Select(string.Format("GROUP_NAME = '{0}'", sGroupName), "GROUP_NAME ASC");

                            foreach (DataRow dRow in drGroup)
                            {
                                foreach (DataRow row in ds_chartmode.Tables[0].Rows)
                                {
                                    DataRow[] drArrTemp;
                                    if (String.IsNullOrEmpty(dRow[COLUMN.RAWID].ToString()))
                                    {
                                        drArrTemp = ds.Tables[0].Select(string.Format("CHART_MODE_CD = '{0}' AND GROUP_RAWID IS NULL", row["CODE"].ToString()));
                                    }
                                    else
                                    {
                                        drArrTemp = ds.Tables[0].Select(string.Format("CHART_MODE_CD = '{0}' AND GROUP_RAWID='{1}'", row["CODE"].ToString(), dRow[COLUMN.RAWID].ToString()));
                                    }
                                    
                                    
                                    if (drArrTemp.Length > 0)
                                    {
                                        BTreeNode node = TreeDCUtil.CreateBTreeNode("CHART_MODE", row["CODE"].ToString(),
                                                                                    row["NAME"].ToString());

                                        foreach (DataRow drTemp in drArrTemp)
                                        {
                                            string sRawid = drTemp[Definition.CONDITION_KEY_RAWID].ToString();
                                            string sSPCModelName = drTemp[Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();
                                            string sGroupRawid = drTemp[COLUMN.GROUP_RAWID].ToString();

                                            BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, sRawid, sSPCModelName);
                                            ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, drTemp[COLUMN.LOCATION_RAWID].ToString());
                                            ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, drTemp[COLUMN.AREA_RAWID].ToString());
                                            btn.IsVisibleCheckBox = this.IsShowCheck;
                                            btn.IsFolder = false;
                                            btn.IsVisibleNodeType = true;
                                            if (!this.IsLastNode)
                                                btn.Nodes.Add(BTreeView.CreateDummyNode());
                                            node.Nodes.Add(btn);
                                            
                                        }


                                        btnRoot.Nodes.Add(node);
                                    }
                                }

                                DataRow[] drArrnodefine = ds.Tables[0].Select("CHART_MODE_CD = 'NOT DEFINED'");

                                if (drArrnodefine.Length > 0)
                                {
                                    BTreeNode node_nodefine = TreeDCUtil.CreateBTreeNode("CHART_MODE", "NOT DEFINED",
                                                                                "NOT DEFINED");


                                    foreach (DataRow drTemp in drArrnodefine)
                                    {
                                        string sRawid = drTemp[Definition.CONDITION_KEY_RAWID].ToString();
                                        string sSPCModelName = drTemp[Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();

                                        BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, sRawid, sSPCModelName);
                                        ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, drTemp[COLUMN.LOCATION_RAWID].ToString());
                                        ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, drTemp[COLUMN.AREA_RAWID].ToString());
                                        btn.IsVisibleCheckBox = this.IsShowCheck;
                                        btn.IsFolder = false;
                                        btn.IsVisibleNodeType = true;
                                        if (!this.IsLastNode)
                                            btn.Nodes.Add(BTreeView.CreateDummyNode());
                                        node_nodefine.Nodes.Add(btn);
                                    }


                                    btnRoot.Nodes.Add(node_nodefine);
                                }
                            }
                        }

                        //added end

                    }
                    else
                    {
                        foreach (DataRow row in drGroup)
                        {
                            string sGroupRawid = row[COLUMN.RAWID].ToString();

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string sRawid = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_RAWID].ToString();
                                string sSPCModelName = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();

                                if (sGroupRawid == ds.Tables[0].Rows[i][COLUMN.GROUP_RAWID].ToString())
                                {
                                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, sRawid, sSPCModelName);
                                    ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, ds.Tables[0].Rows[i][COLUMN.LOCATION_RAWID].ToString());
                                    ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, ds.Tables[0].Rows[i][COLUMN.AREA_RAWID].ToString());
                                    btn.IsVisibleCheckBox = this.IsShowCheck;
                                    btn.IsFolder = false;
                                    btn.IsVisibleNodeType = true;
                                    if (!this.IsLastNode)
                                        btn.Nodes.Add(BTreeView.CreateDummyNode());

                                    btnRoot.Nodes.Add(btn);
                                }
                            }
                        }
                    }
                    btnRoot.ExpandAll();
                    //this.XTreeView.Nodes.Add(btnRoot);
                }
            }

            this.btnSPCModel.ExpandAll();
        }

        private void AddSPCModelNode(TreeNode tnCurrent)
        {

            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, this.AreaRawid);

            if (!string.IsNullOrEmpty(this.EqpModelName))
            llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, getReplace(EqpModelName));
            
            if(!string.IsNullOrEmpty(this.ParamTypeCD))
                llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.ParamTypeCD);

            if (!string.IsNullOrEmpty(this.FilterValue))
                llstData.Add("FILTER", this.FilterValue);

            DataSet ds = _wsSPC.GetSPCModel(llstData.GetSerialData());

            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sRawid = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_RAWID].ToString();
                string sSPCModelName = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, sRawid, sSPCModelName);
                ((DCValueOfTree) btn.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, ds.Tables[0].Rows[i][COLUMN.LOCATION_RAWID].ToString());
                ((DCValueOfTree) btn.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, ds.Tables[0].Rows[i][COLUMN.AREA_RAWID].ToString());
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                //btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.EQP);
                //btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.EQP;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }

        #endregion

    }
}
