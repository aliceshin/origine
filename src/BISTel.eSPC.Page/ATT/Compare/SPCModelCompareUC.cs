using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Modeling;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.Compare
{
    public partial class SPCModelCompareUC : BasePageUCtrl
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private Initialization _initialization = null;
        internal SPCCopySpec copyPopup = null;

        //SPC-1292, KBLEE, START
        private string sSite;
        private string sFab;
        private string _eqpModel;
        private string _area;
        private string _line;
        private string _groupName;
        //SPC-1292, KBLEE, END

        public SPCModelCompareUC()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            this.KeyOfPage = Definition.PAGE_KEY_SPC_ATT_MODEL_COMPARE_UC;

            this.ActiveDynamicConditions();

            InitializePage();
        }

        public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        {
            return new BISTel.eSPC.Condition.ATT.Compare.SPCModelCompareCondition();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            InitializePage();

            if(!this.ApplyAuthory(llCondition))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                return;
            }

            if(llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS] != null)
            {
                string value = 
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                this.btxtParamName.ReadOnly = false;
                this.btxtParamName.Text = value;
                this.btxtParamName.ReadOnly = true;
            }

            this.ucSPCModelList.PageSearch(llCondition);

            this.ucSpcModelCompareResultUC.PageSearch(llCondition);

        }

        public void InitializePage()
        {
            InitializeVariables();

            InitializeSPCModelList();

            InitializeSPCModelCompareResult();

            InitializeLayout();
        }

        private void InitializeVariables()
        {
            if(this._ws == null)
                this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            
            if(this._initialization == null)
            {
                this._initialization = new Initialization();
                this._initialization.InitializePath();
            }
        }

        private void InitializeLayout()
        {
            this.btxtParamName.Text = "";

            this.bTabControl1.SelectedIndex = this.tabSPCModelList.TabIndex;

            this.tabCompareResult.Enabled = false;
        }

        private void InitializeSPCModelList()
        {
            this.ucSPCModelList.ApplicationName = this.ApplicationName;
            this.ucSPCModelList.InitializePage(Definition.FUNC_KEY_SPC_ATT_MODEL_COMPARE, Definition.PAGE_KEY_SPC_ATT_MODEL_COMPARE_UC,
                                               "SPC_BTNLIST_SPC_MODELING_LIST", this.sessionData);
        }

        private void InitializeSPCModelCompareResult()
        {
            this.ucSpcModelCompareResultUC.ApplicationName = this.ApplicationName;
            this.ucSpcModelCompareResultUC.InitializePage(Definition.FUNC_KEY_SPC_ATT_MODEL_COMPARE, Definition.PAGE_KEY_SPC_ATT_MODEL_COMPARE_UC,
                                                          "SPC_BTNLIST_SPC_COMPARE_RESUT", this.sessionData, this.URL, this.Port);
            this.ucSpcModelCompareResultUC.linkTraceDataViewEventResult -= new SPCModelCompareResultUC.LinkTraceDataViewEventHandlerResult(ucSpcModelCompareResultUC_linkTraceDataViewEventResult);
            this.ucSpcModelCompareResultUC.linkTraceDataViewEventResult += new SPCModelCompareResultUC.LinkTraceDataViewEventHandlerResult(ucSpcModelCompareResultUC_linkTraceDataViewEventResult);
        }

        void ucSpcModelCompareResultUC_linkTraceDataViewEventResult(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            this.SendMessage("TRACE_DATA", true, llstTraceLinkData, 0);
        }

        public void Compare(LinkedList llCondition)
        {
            //SPC-1292, KBLEE, START
            this._line = ucSpcModelCompareResultUC.LOCATION_RAWID;
            this._area = ucSpcModelCompareResultUC.AREA_RAWID;
            this._eqpModel = ucSpcModelCompareResultUC.EQP_MODEL;
            this._groupName = ucSpcModelCompareResultUC.GROUP_NAME;
            //SPC-1292, KBLEE, END

            this.InitializeSPCModelCompareResult();

            this.ucSpcModelCompareResultUC.Compare(llCondition);

            //SPC-1292, KBLEE, START
            ucSpcModelCompareResultUC.LOCATION_RAWID = this._line;
            ucSpcModelCompareResultUC.AREA_RAWID = this._area;
            ucSpcModelCompareResultUC.EQP_MODEL = this._eqpModel;
            ucSpcModelCompareResultUC.GROUP_NAME = this._groupName;
            //SPC-1292, KBLEE, END

            this.tabCompareResult.Enabled = true;
            this.bTabControl1.SelectedIndex = this.tabCompareResult.TabIndex;
        }

        private bool ApplyAuthory(LinkedList llCondition)
        {
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;
            string sArea = string.Empty;

            if(llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
            {
                 sSite =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
            {
                 sFab =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
            {
                sLine =  ((DataTable) llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
            }

            if(llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
            {
                sArea = ((DataTable) llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            return UserAuthorityHandler.isAvailableFunction(this.ApplicationName, true, this.FunctionName, this.sessionData.UserId, sSite, sFab, sLine, sArea);
        }
    }
}
