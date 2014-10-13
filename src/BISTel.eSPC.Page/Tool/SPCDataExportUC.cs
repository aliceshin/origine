using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Tool
{
    public partial class SPCDataExportUC : BasePageUCtrl
    {
        public SPCDataExportUC()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            this.KeyOfPage = Definition.PAGE_KEY_SPC_DATA_EXPORT;

            this.ActiveDynamicConditions();

            InitializePage();

            this.dataExportUC1.modelExportCompletedEvent += ModelExported;
            this.dataExportUC1.exportStartedEventHandler += ExportStarted;
            this.dataExportUC1.subModelExportCompletedEvent += SubModelExported;
        }

        public override void PageSearch(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llCondition)
        {
            if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_OR_CREATE_MODEL", null, null);
                return;
            }
            InitializePage();

            if (!this.ApplyAuthory(llCondition))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                return;
            }

            this.spcModelUC1.PageSearch(llCondition);
            this.dataExportUC1.PageSearch(llCondition);
            this.exportResult1.PageSearch(llCondition);

            SPCModel[] spcModels = this.spcModelUC1.SpcModels;
            //SPC-1109 by stella
            if (spcModels != null)
            {
                this.dataExportUC1.SetSPCModels(spcModels);
            }
        }

        public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        {
            if (this.GetType().FullName == "BISTel.eSPC.Page.Tool.SPCDataExportUC")
            {
                return new BISTel.eSPC.Condition.Tool.SPCDataExportCondition();
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Page.Tool.MET.SPCDataExportUC")
            {
                return new BISTel.eSPC.Condition.Tool.MET.SPCDataExportCondition();
            }
            else
            {
                return new BISTel.eSPC.Condition.Tool.SPCDataExportCondition();
            }
        }

        private void InitializePage()
        {
            this.spcModelUC1.InitializePage(this.FunctionName, this.KeyOfPage, this.sessionData);
            this.dataExportUC1.InitializePage(this.FunctionName, this.KeyOfPage, this.sessionData);
            this.exportResult1.InitializePage(this.FunctionName, this.KeyOfPage, this.sessionData);
        }

        public void ModelExported(object sender, ModelExportCompletedEventHandlerArgs e)
        {
            this.exportResult1.ModelExported(sender, e);
        }

        public void ExportStarted(object sender, ExportStartedEventHandlerArgs e)
        {
            this.exportResult1.ExportStarted(sender, e);
            this.spcModelUC1.ExportStarted(sender, e);
        }

        public void SubModelExported(object sender, SubModelExportCompletedEventHandlerArgs e)
        {
            this.spcModelUC1.SubModelExported(sender, e);
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
