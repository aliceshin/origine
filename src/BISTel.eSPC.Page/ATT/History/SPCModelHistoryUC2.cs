﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Compare;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.History
{
    public partial class SPCModelHistoryUC2 : BasePageUCtrl
    {
        //SPC-1292, KBLEE, START
        private string sSite;
        private string sFab;
        private string _eqpModel;
        private string _area;
        private string _line;
        private string _groupName;
        //SPC-1202, KBLEE, END

        public SPCModelHistoryUC2()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            this.KeyOfPage = Definition.PAGE_KEY_SPC_ATT_MODEL_HISTORY;

            this.ActiveDynamicConditions();

            InitializePage();
        }

        public void InitializePage()
        {
            InitializeSPCModelListUC();
            InitializeSPCModelVersionHistory();
            InitializeSPCModelVersionCompareResult();
        }

        private void InitializeSPCModelListUC()
        {
            this.spcModelUCForHistory31.ApplicationName = this.ApplicationName;
            this.spcModelUCForHistory31.InitializePage(this.FunctionName, Definition.PAGE_KEY_SPC_ATT_MODEL_HISTORY, "SPC_MODEL_HISTORY_MODEL_LIST",
                this.sessionData);
            this.spcModelUCForHistory31.VersionHistoryButtonClicked -= new VersionHistoryButtonClickEventHandler(VersionHistoryButtonClicked);
            this.spcModelUCForHistory31.VersionHistoryButtonClicked += new VersionHistoryButtonClickEventHandler(VersionHistoryButtonClicked);
        }

        private void InitializeSPCModelVersionHistory()
        {
            this.spcModelVersionHistory1.ApplicationName = this.ApplicationName;
            this.spcModelVersionHistory1.InitializePage(this.FunctionName, Definition.PAGE_KEY_SPC_ATT_MODEL_HISTORY, "SPC_MODEL_HISTORY_VERSION_LIST",
                                                        this.sessionData);
            this.spcModelVersionHistory1.VersionCompareButtonClicked -= new VersionCompareButtonClickEventHandler(VersionCompareButtonClicked);
            this.spcModelVersionHistory1.VersionCompareButtonClicked += new VersionCompareButtonClickEventHandler(VersionCompareButtonClicked);

        }

        private void InitializeSPCModelVersionCompareResult()
        {
            this.spcModelVersionCompareResultUC1.ApplicationName = this.ApplicationName;
            this.spcModelVersionCompareResultUC1.compareType = CompareType.Version;
            this.spcModelVersionCompareResultUC1.InitializePage(this.FunctionName, Definition.PAGE_KEY_SPC_ATT_MODEL_HISTORY,
                                                                "SPC_MODEL_HISTORY_COMPRARE_RESULT",
                                                                this.sessionData, this.URL, this.Port);

            this.spcModelVersionCompareResultUC1.modelModifiedEventHandler -= new ModelModifiedEventHandler(ModelModified);
            this.spcModelVersionCompareResultUC1.modelModifiedEventHandler += new ModelModifiedEventHandler(ModelModified);
        }


        public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        {
            BISTel.eSPC.Condition.ATT.Tool.SPCDataExportCondition condition = new Condition.ATT.Tool.SPCDataExportCondition();
            condition.CheckCountType = BTreeView.CheckCountTypes.Single;
            return condition;
        }

        public override void PageSearch(LinkedList llCondition)
        {
            if (!llCondition.Contains(Definition.DynamicCondition_Search_key.SPCMODEL))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }

            if (!this.ApplyAuthory(llCondition))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                return;
            }

            //SPC-1292, KBLEE, START
            if (llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
            {
                sSite =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
            {
                sFab =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
            {
                _line =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                //sLine = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
            }

            if (llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
            {
                _area =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                //sArea = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.EQPMODEL] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows.Count > 0)
            {
                _eqpModel =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.CONDITION_KEY_GROUP_NAME] != null
                && ((DataTable)llCondition[Definition.CONDITION_KEY_GROUP_NAME]).Rows.Count > 0)
            {
                _groupName =
                    ((DataTable)llCondition[Definition.CONDITION_KEY_GROUP_NAME]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            //SPC-1292, KBLEE, END

            this.bTabControl1.SelectedIndex = this.tabSPCModelList.TabIndex;

            string modelRawid = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SPCMODEL]).Rows[0]["VALUEDATA"].ToString();

            eSPCWebService.eSPCWebService ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            LinkedList condition = new LinkedList(Definition.DynamicCondition_Condition_key.MODEL_RAWID, modelRawid);
            DataSet ds = ws.GetATTParamName(condition.GetSerialData());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                this.btxtParamName.Text = ds.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();

            InitializePage();

            this.spcModelUCForHistory31.PageSearch(llCondition);

            this.MsgClose();
        }

        public void VersionHistoryButtonClicked(object sender, VersionHistoryButtonClickedEventArgs args)
        {
            LinkedList llCondition = new LinkedList(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, args.ModelID);
            this.spcModelVersionHistory1.PageSearch(llCondition);
        }

        public void VersionCompareButtonClicked(object sender, VersionCompareButtonClickedEventArgs args)
        {
            LinkedList llCondition = new LinkedList("CHART_ID", args.ModelID);
            llCondition.Add("VERSIONS", args.Versions);
            this.spcModelVersionCompareResultUC1.LOCATION_RAWID = this._line;
            this.spcModelVersionCompareResultUC1.AREA_RAWID = this._area;
            this.spcModelVersionCompareResultUC1.EQP_MODEL = this._eqpModel;
            this.spcModelVersionCompareResultUC1.GROUP_NAME = this._groupName; //SPC-1292, KBLEE
            this.spcModelVersionCompareResultUC1.PageSearch(llCondition);
            this.spcModelVersionCompareResultUC1.Compare(llCondition);

            bTabControl1.SelectedIndex = 1;
        }

        public void ModelModified(object sender, EventArgs args)
        {
            bTabControl1.SelectedIndex = 0;
            this.spcModelUCForHistory31.PageRefresh();
            this.spcModelVersionHistory1.PageRefresh();

        }

        private bool ApplyAuthory(LinkedList llCondition)
        {
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;
            string sArea = string.Empty;

            if (llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
            {
                sSite =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
            {
                sFab =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
            {
                sLine = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
            }

            if (llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
            {
                sArea = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            return UserAuthorityHandler.isAvailableFunction(this.ApplicationName, true, this.FunctionName, this.sessionData.UserId, sSite, sFab, sLine, sArea);
        }
    }
}
