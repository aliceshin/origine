using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Tree;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.DataHandler;


namespace BISTel.eSPC.Condition.Controls.ComboTree
{
    class AreasComboTreeControlForOCAPList : IAddChildNodeToDCTreeView
    {
        #region : Field

        private string _sUseDefaultCondition = "";
        private eSPCWebService.eSPCWebService _spcWebService = null;
        private MultiLanguageHandler _mlthandler;
        private BTreeView _btvCondition = null;
        string sLineRawid = string.Empty;
        string sAreaRawid = string.Empty;
        string _sSPCModelLevel = string.Empty;

        #endregion

        #region : IAddChildNodeToDCTreeView Members

        public void GetParameter(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {
        }

        public void RefreshCondition(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llData)
        {
        }

        public void SetDCTree(BISTel.PeakPerformance.Client.BISTelControl.BTreeView btv)
        {
            this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            //SPC MODEL LEVEL을 가져옴
            LinkedList llstCondtion = new LinkedList();
            if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.ComboTree.AreasComboTreeControlForOCAPList")
            {
                llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.ComboTree.AreasComboTreeControlForOCAPList")
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

           //_sSPCModelLevel = Definition.CONDITION_KEY_EQP_MODEL;


            _btvCondition = btv;
            //_btvCondition.CheckCountType = BTreeView.CheckCountTypes.SingleByNode;
            btv.BeforeExpand += new TreeViewCancelEventHandler(Tree_BeforeExpand);
            btv.RefreshNode += new BTreeView.RefreshNodeEventHandler(Tree_RefreshNode);
            btv.AfterExpand += new TreeViewEventHandler(btv_AfterExpand);
            btv.BeforeCheck += new TreeViewCancelEventHandler(btv_BeforeCheck);
            btv.CheckCountType = BTreeView.CheckCountTypes.Multi;

            _btvCondition.ImageList.Images.AddRange(ImageLoaderHandler.Instance.TreeArrayImage);
            _btvCondition.AddImageForLevel(0, _btvCondition.ImageList.Images[0], _btvCondition.ImageList.Images[0]);
            _btvCondition.AddImageForLevel(1, _btvCondition.ImageList.Images[1], _btvCondition.ImageList.Images[1]);
            _btvCondition.AddImageForLevel(2, _btvCondition.ImageList.Images[2], _btvCondition.ImageList.Images[2]);
            _btvCondition.AddImageForLevel(3, _btvCondition.ImageList.Images[3], _btvCondition.ImageList.Images[3]);

            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                _btvCondition.AddImageForLevel(4, _btvCondition.ImageList.Images[4], _btvCondition.ImageList.Images[4]);
            }
        }

        public void SetParentValue(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llParentValue)
        {
        
           

        }

        #endregion

        #region : Event Handling

        void Tree_RefreshNode(object sender, RefreshNodeEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.TargetNode);
            if (dcValue == null) return;

            if (dcValue.ContextId == Definition.CONDITION_SEARCH_KEY_EQP)
            {
                e.TargetNode.Nodes.Clear();
                Tree_BeforeExpand(null, new TreeViewCancelEventArgs(e.TargetNode, false, TreeViewAction.Expand));
            }
        }

        void Tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

          

        }

        void btv_AfterExpand(object sender, TreeViewEventArgs e)
        {
            DCValueOfTree dcValue = TreeDCUtil.GetDCValue(e.Node); // TreeNode에서 DCValueOfTree값을 추출.
            if (dcValue == null) return;

             if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
             {
                 if (dcValue.ContextId == Definition.CONDITION_SEARCH_KEY_AREA)
                 {
                     for (int i = 0; i < e.Node.Nodes.Count; i++)
                     {
                         BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                         btn.Nodes.Clear();
                         btn.IsVisibleCheckBox = true;
                         btn.IsVisibleNodeType = true;
                     }
                 }
                 else if(this._btvCondition.CheckCountType == BTreeView.CheckCountTypes.Multi
                     && dcValue.ContextId == Definition.CONDITION_SEARCH_KEY_LINE)
                 {
                     for (int i = 0; i < e.Node.Nodes.Count; i++)
                     {
                         BTreeNode btn = (BTreeNode)e.Node.Nodes[i];

                         btn.IsVisibleCheckBox = true;
                         btn.IsVisibleNodeType = true;
                     }
                 }
             }
             else
             {
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
        }

        void btv_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node.IsExpanded == false)
            {
                e.Node.ExpandAll();
                e.Node.Collapse();
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
