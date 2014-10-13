using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Tree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Tool
{
    public partial class SPCDataExportConditionTree : ConditionTreeWithFilter
    {
        private string sLineRawID = string.Empty;
        private string sAreaRawID = string.Empty;
        private string sEQPModel = string.Empty;
        private eSPCWebService.eSPCWebService ws = null;

        private bool isMET = false;
        private bool isATT = false;
        private BTreeNode node;
        private BTreeNode groupNode;
        private Initialization _Initialization;

        public bool ISMET
        {
            get { return this.isMET; }
            set { this.isMET = value; }
        }

        public bool ISATT
        {
            get { return this.isATT; }
            set { this.isATT = value; }
        }

        public BTreeView.CheckCountTypes CheckCountType
        {
            get { return this.bTreeView1.CheckCountType; }
            set { this.bTreeView1.CheckCountType = value; }
        }

        public SPCDataExportConditionTree()
        {
            _Initialization = new Initialization();

            if(!DesignMode)
            {
                this.bTreeView1.AfterCheck += new TreeViewEventHandler(bTreeView1_AfterCheck);
            }

            base.InitializeComponent();
            InitializeComponent();

            try
            {
                ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

                //this.bTreeView1.AddLevelForCheck(1);
                this.bTreeView1.BeforeExpand += new TreeViewCancelEventHandler(bTreeView1_BeforeExpand);
                this.ClickSearch += new ClickSearchEventHandler(SPCDataExportConditionTree_ClickSearch);
                //added by enkim 2012.10.17 SPC-915
                this.bTreeView1.RefreshNode += new BTreeView.RefreshNodeEventHandler(bTreeView1_RefreshNode);
                //added end SPC-915
            }catch
            {
            }
        }

        public override void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList ll)
        {
 	         bool needRefresh = false;

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_LINE]).Rows.Count > 0)
                {
                    sLineRawID = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sLineRawID = string.Empty;
                }

                needRefresh = true;
            }

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_AREA]).Rows.Count > 0)
                {
                    sAreaRawID = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sAreaRawID = string.Empty;
                }

                needRefresh = true;
            }

            if(ll.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
            {
                if(((DataTable) ll[Definition.CONDITION_SEARCH_KEY_EQPMODEL]).Rows.Count > 0)
                {
                    sEQPModel = ((DataTable) ll[Definition.CONDITION_SEARCH_KEY_EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                }
                else
                {
                    sEQPModel = string.Empty;
                }

                needRefresh = true;
            }

            if(needRefresh)
                RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            this.bTreeView1.Nodes.Clear();

            if(string.IsNullOrEmpty(this.sEQPModel)
                && string.IsNullOrEmpty(this.sAreaRawID))
                return;
            
            //spc-880 add Group
            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLineRawID);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawID);

            if (!string.IsNullOrEmpty(sEQPModel))
                llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, sEQPModel);

            node = TreeDCUtil.CreateBTreeNode("SPC MODEL LIST", "SPC MODEL LIST", "SPC MODEL LIST");
            //node.Nodes.Add(BTreeView.CreateDummyNode());
            node.IsVisibleCheckBox = false;
            node.IsFolder = true;
            node.IsVisibleNodeType = true;
            this.bTreeView1.Nodes.Add(node);
            AddChildNodes(node);
            this.bTreeView1.ExpandAll();
        }

        //added by enkim 2012.10.17 SPC-915
        void bTreeView1_RefreshNode(object sender, RefreshNodeEventArgs rne)
        {
            this.RefreshTreeView();
            //bTreeView1_BeforeExpand(null, new TreeViewCancelEventArgs(rne.TargetNode, false, TreeViewAction.Expand));
            //bTreeView1.ExpandAll();
        }
        //added end SPC-915

        void SPCDataExportConditionTree_ClickSearch(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ClickSearchEventArgs csea)
        {
            mng.GetSelectedValue("", csea.LinkedListCondition);
        }

        void bTreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //if (e.Node.Nodes.Count == 0)
            //    AddChildNodes((BTreeNode)e.Node);
        }

        void bTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            LinkedList ll = new LinkedList();
            this.mng.GetSelectedValue("", ll);
            ((ADynamicCondition) this.Parent).RefreshCondition(ll);
        }

        void AddChildNodes(BTreeNode node)
        {
            DataSet ds = null;
            DataSet dsGroup = null;

            #region previous
            //if (isATT)
            //{
            //    ds = ws.GetATTSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'");
            //}
            //else
            //{
            //    LinkedList llParam = new LinkedList();
            //    llParam.Add(COLUMN.LOCATION_RAWID, sLineRawID);
            //    llParam.Add(COLUMN.AREA_RAWID, sAreaRawID);

            //    if(!String.IsNullOrEmpty(sEQPModel))
            //        llParam.Add(COLUMN.EQP_MODEL, sEQPModel);

                //if (node.Text != Definition.VARIABLE_UNASSIGNED_MODEL)
                //    llParam.Add(COLUMN.GROUP_NAME, node.Text);

                //LinkedList llstData = new LinkedList();
                //llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLineRawID);
                //llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawID);

                //dsGroup = this.ws.GetGroupList(llstData.GetSerialData());

                //if (isMET)
                //{
                //    //ds = ws.GetMETSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'", "MET");
                //    ds = ws.GetSPCModelListbyGroup(llParam.GetSerialData(), true);

                //}
                //else
                //{
                //    //ds = ws.GetSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'", "MET");
                //    ds = ws.GetSPCModelListbyGroup(llParam.GetSerialData(), false);
                //}
            //}

            //if(ds == null
            //    || ds.Tables.Count == 0)
            //    return;
            #endregion

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, sLineRawID);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawID);

            if (!String.IsNullOrEmpty(sEQPModel))
            {
                llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, sEQPModel);
            }

            if (isATT)
            {
                dsGroup = this.ws.GetATTGroupList(llstData.GetSerialData());
            }
            else
            {
                dsGroup = this.ws.GetGroupList(llstData.GetSerialData());
            }

            DataRow drModel = dsGroup.Tables[0].NewRow();
            drModel[COLUMN.GROUP_NAME] = Definition.VARIABLE_UNASSIGNED_MODEL;
            dsGroup.Tables[0].Rows.Add(drModel);
            
            foreach (DataRow row in dsGroup.Tables[0].Rows)
            {
                if (isATT)
                {
                    ds = ws.GetATTSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'");
                }
                else
                {
                    LinkedList llParam = new LinkedList();
                    llParam.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                    llParam.Add(COLUMN.AREA_RAWID, sAreaRawID);

                    if (!String.IsNullOrEmpty(sEQPModel))
                        llParam.Add(COLUMN.EQP_MODEL, "'"+ sEQPModel +"'");

                    if (row[COLUMN.GROUP_NAME].ToString()!=Definition.VARIABLE_UNASSIGNED_MODEL)
                        llParam.Add(COLUMN.GROUP_RAWID, row[COLUMN.RAWID].ToString());

                    if (isMET)
                    {
                        //ds = ws.GetMETSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'", "MET");
                        ds = ws.GetSPCModelListbyGroup(llParam.GetSerialData(), true);

                    }
                    else
                    {
                        //ds = ws.GetSPCModeList2(sLineRawID, sAreaRawID, "'" + sEQPModel + "'", "MET");
                        ds = ws.GetSPCModelListbyGroup(llParam.GetSerialData(), false);
                    }
                }

                if (ds == null
                || ds.Tables.Count == 0)
                    return;

                if (this._Initialization.GetChartModeGrouping(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC))
                {
                    LinkedList llstCondition = new LinkedList();
                    llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
                    DataSet ds_chartmode = this.ws.GetCodeData(llstCondition.GetSerialData());

                    if (ds_chartmode == null || ds_chartmode.Tables[0].Rows.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        DataRow[] drSelect;

                        if (string.IsNullOrEmpty(row[COLUMN.RAWID].ToString()))
                        {
                            drSelect = ds.Tables[0].Select("GROUP_RAWID IS NULL", "SPC_MODEL_NAME ASC");
                        }
                        else
                        {
                            drSelect = ds.Tables[0].Select(string.Format("GROUP_RAWID = '{0}'", row[COLUMN.RAWID].ToString()), "SPC_MODEL_NAME ASC");
                        }

                        if (string.IsNullOrEmpty(row[COLUMN.RAWID].ToString()) && drSelect.Length == 0)
                            break;

                        groupNode = TreeDCUtil.CreateBTreeNode(row[COLUMN.GROUP_NAME].ToString(), row[COLUMN.GROUP_NAME].ToString(), row[COLUMN.GROUP_NAME].ToString());
                        groupNode.IsVisibleCheckBox = false;
                        groupNode.IsFolder = true;
                        groupNode.IsVisibleNodeType = true;
                        ((DCValueOfTree)groupNode.Tag).ContextId = Definition.CONDITION_KEY_GROUP_NAME;
                        node.Nodes.Add(groupNode);

                        foreach (DataRow dRow in ds_chartmode.Tables[0].Rows)
                        {
                            DataRow[] drArrTemp;
                            if (String.IsNullOrEmpty(row[COLUMN.RAWID].ToString()))
                            {
                                drArrTemp = ds.Tables[0].Select(string.Format("CHART_MODE_CD = '{0}' AND GROUP_RAWID IS NULL", dRow["CODE"].ToString()));
                            }
                            else
                            {
                                drArrTemp = ds.Tables[0].Select(string.Format("CHART_MODE_CD = '{0}' AND GROUP_RAWID='{1}'", dRow["CODE"].ToString(), row[COLUMN.RAWID].ToString()));
                            }


                            if (drArrTemp.Length > 0)
                            {
                                BTreeNode chartNode = TreeDCUtil.CreateBTreeNode("CHART_MODE", dRow["CODE"].ToString(),
                                                                            dRow["NAME"].ToString());
                                foreach (DataRow drTemp in drArrTemp)
                                {
                                    string sRawid = drTemp[Definition.CONDITION_KEY_RAWID].ToString();
                                    string sSPCModelName = drTemp[Definition.CONDITION_KEY_SPC_MODEL_NAME].ToString();
                                    string sGroupRawid = drTemp[COLUMN.GROUP_RAWID].ToString();

                                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, sRawid, sSPCModelName);
                                    ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, drTemp[COLUMN.LOCATION_RAWID].ToString());
                                    ((DCValueOfTree)btn.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, drTemp[COLUMN.AREA_RAWID].ToString());
                                    btn.IsVisibleCheckBox = true;
                                    btn.IsFolder = false;
                                    btn.IsVisibleNodeType = true;
                                    chartNode.Nodes.Add(btn);

                                }


                                groupNode.Nodes.Add(chartNode);
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
                                btn.IsVisibleCheckBox = true;
                                btn.IsFolder = false;
                                btn.IsVisibleNodeType = true;
                                node_nodefine.Nodes.Add(btn);
                            }


                            groupNode.Nodes.Add(node_nodefine);
                        }

                    }

                    //added end
                }
                else
                {
                    DataRow[] drSelect;

                    if (string.IsNullOrEmpty(row[COLUMN.RAWID].ToString()))
                    {
                        drSelect = ds.Tables[0].Select("GROUP_RAWID IS NULL");
                    }
                    else
                    {
                        drSelect = ds.Tables[0].Select(string.Format("GROUP_RAWID = '{0}'", row[COLUMN.RAWID].ToString()));
                    }

                    if (string.IsNullOrEmpty(row[COLUMN.RAWID].ToString()) && drSelect.Length == 0)
                        break;

                    groupNode = TreeDCUtil.CreateBTreeNode(row[COLUMN.GROUP_NAME].ToString(), row[COLUMN.GROUP_NAME].ToString(), row[COLUMN.GROUP_NAME].ToString());
                    groupNode.IsVisibleCheckBox = false;
                    groupNode.IsFolder = true;
                    groupNode.IsVisibleNodeType = true;
                    ((DCValueOfTree)groupNode.Tag).ContextId = Definition.CONDITION_KEY_GROUP_NAME;
                    node.Nodes.Add(groupNode);

                    if (drSelect.Length > 0)
                    {
                        foreach (DataRow dr in drSelect)
                        {
                            BTreeNode modelNode = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPCMODEL, dr[COLUMN.RAWID].ToString(), dr[COLUMN.SPC_MODEL_NAME].ToString());
                            ((DCValueOfTree)modelNode.Tag).AdditionalValue.Add(COLUMN.LOCATION_RAWID, sLineRawID);
                            ((DCValueOfTree)modelNode.Tag).AdditionalValue.Add(COLUMN.AREA_RAWID, sAreaRawID);
                            modelNode.IsVisibleCheckBox = true;
                            modelNode.IsFolder = false;
                            modelNode.IsVisibleNodeType = true;
                            groupNode.Nodes.Add(modelNode);
                        }
                    }
                }
            }
        }

        public override void Filter(string text)
        {
            Unfilter();
            this.bTreeView1.ExpandAll();

            for (int i = bTreeView1.Nodes.Count - 1; i >= 0; i--)
            {
                Filter((BTreeNode) bTreeView1.Nodes[i], 0, text);
            }
        }

        private void Filter(BTreeNode node, int depth, string text)
        {
            if(depth < 2)
            {
                depth++;
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    Filter((BTreeNode)node.Nodes[i], depth, text);
            }
            else
            {
                if(!node.Text.ToUpper().Contains(text.ToUpper()))
                    node.Remove();
            }
        }

        public override void Unfilter()
        {
            RefreshTreeView();
        }
    }
}
