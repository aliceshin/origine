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
    public class XSPCParamNameTree : TreeInterface
    {
        #region : Field

        private TreeNode _treeNode = null;

        #endregion

        #region : Initialization

        public XSPCParamNameTree(BTreeView btv)
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
                AddParamNameNode(tnCurrent);

            if (tnCurrent.Nodes.Count > 0) return;
        }



        public void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
        }

        public void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;
       
            if (dcValue.ContextId == Definition.DynamicCondition_Search_key.OPERATION)
            {
                if (e.Node.Nodes.Count > 0) return;

                e.Node.Nodes.Clear();
            }         
        }

        #endregion

        #region : Private

        private void AddParamNameNode(TreeNode tnCurrent)
        {
            LinkedList llstData = new LinkedList();     
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.ParamType);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.OperationID);
            
            DataSet ds = this._wsSPC.GetParamName(llstData.GetSerialData());
          
            if (ds == null || ds.Tables.Count <= 0)
                return;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sCode = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                string sDesc = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();

                BTreeNode btn = TreeDCUtil.CreateBTreeNode(Definition.DynamicCondition_Search_key.PARAM, sCode, sDesc);
                btn.IsVisibleCheckBox = this.IsShowCheck;
                btn.IsFolder = false;
                btn.IsVisibleNodeType = true;
                btn.ImageIndexList.Add((int)ImageLoader.TREE_IMAGE_INDEX.PARAMETER);
                btn.ImageIndex = (int)ImageLoader.TREE_IMAGE_INDEX.PARAMETER;
                if (!this.IsLastNode)
                    btn.Nodes.Add(BTreeView.CreateDummyNode());

                tnCurrent.Nodes.Add(btn);
            }
        }


        private void AddSearchTypeNode(TreeNode node)
        {
         
        }

        #endregion

    }
}
