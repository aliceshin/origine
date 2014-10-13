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
using System.Drawing;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class XSPCModelContextTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XSPCModelContextTree(BTreeView btv)
        {
            this.XTreeView = btv;
            //btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            //btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
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
                AddSPCModelContextNode(tnCurrent);

            if (tnCurrent.Nodes.Count > 0) return;
        }

        public void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }

        #endregion

        #region : Private

        private void AddSPCModelContextNode(TreeNode tnCurrent)
        {
            if (tnCurrent.Nodes.Count > 0)
                return;

            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, this.ModelRawID);
            DataSet ds = _wsSPC.GetSPCModelContext(llstData.GetSerialData());

            if (ds == null || ds.Tables.Count <= 0)
                return;

            string sOldRawID = string.Empty;
            string sContextList = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sRawid = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_RAWID].ToString();
                string sParamAlias = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                string sMAIN_YN = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MAIN_YN].ToString();
                string sContextKey = ds.Tables[0].Rows[i]["CONTEXT_KEY"].ToString();
                string sContextValue = ds.Tables[0].Rows[i]["CONTEXT_VALUE"].ToString();

                if (i == ds.Tables[0].Rows.Count - 1)
                {
                    if (!sMAIN_YN.Equals("Y"))
                        sb.AppendFormat("/{0}", sContextValue);
                }

                if ((i > 0 && sRawid != sOldRawID) || (i == ds.Tables[0].Rows.Count - 1))
                {
                    BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID, sOldRawID, sb.ToString());
                    btn.IsVisibleCheckBox = this.IsShowCheck;
                    btn.IsFolder = false;
                    btn.IsVisibleNodeType = true;
                    if (!this.IsLastNode)
                        btn.Nodes.Add(BTreeView.CreateDummyNode());

                    tnCurrent.Nodes.Add(btn);
                    sb = new StringBuilder();

                    //if (sMAIN_YN.Equals("Y"))
                    //    sb.AppendFormat("{0}/{1}/*", sParamAlias, sMAIN_YN);
                    //else
                    //    sb.AppendFormat("{0}/{1}", sParamAlias, sMAIN_YN);

                    if (sMAIN_YN.Equals("Y"))
                        sb.AppendFormat("{0}/*", sParamAlias);
                    else
                        sb.AppendFormat("{0}", sParamAlias);
                }

                if (i == 0)
                {
                    //if (sMAIN_YN.Equals("Y"))
                    //    sb.AppendFormat("{0}/{1}/*", sParamAlias, sMAIN_YN);
                    //else
                    //    sb.AppendFormat("{0}/{1}/{2}", sParamAlias, sMAIN_YN, sContextValue);
                    
                    if (sMAIN_YN.Equals("Y"))
                        sb.AppendFormat("{0}/*", sParamAlias);
                    else
                        sb.AppendFormat("{0}/{1}", sParamAlias, sContextValue);
                }
                else
                {
                    //if (!sMAIN_YN.Equals("Y"))
                    //    sb.AppendFormat("/{0}", sContextValue);

                    if (!sMAIN_YN.Equals("Y"))
                    {
                        if(sb.Length.Equals(0))
                            sb.AppendFormat("{0}", sContextValue);
                        else
                            sb.AppendFormat("/{0}", sContextValue);
                    }
                }

                sOldRawID = sRawid;
            }
        }

        #endregion

    }
}
