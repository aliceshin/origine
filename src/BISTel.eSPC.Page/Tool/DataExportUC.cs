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
using BISTel.PeakPerformance.Client.DataHandler;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Tool
{
    public partial class DataExportUC : BasePageUCtrl
    {
        private eSPCWebService.eSPCWebService _ws = null;
        private DataExportController controller = null;

        internal SubModelExportCompletedEventHandler subModelExportCompletedEvent;
        internal ModelExportCompletedEventHandler modelExportCompletedEvent;
        internal ModelExportStartedEventHandler modelExportedStartedEvent;
        internal ExportStartedEventHandler exportStartedEventHandler;
        internal ExportCompletedEventHandler exportCompletedEventHandler;

        public DataExportUC()
        {
            InitializeComponent();

            this.ToDate.StartDateTime = DateTime.Now;
            this.FromDate.StartDateTime = this.ToDate.StartDateTime.AddDays(-1);
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llCondition)
        {
            InitializePage();
        }

        public void InitializePage(string functionName, string pageKey, SessionData session)
        {
            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.sessionData = session;

            this.InitializePage();
        }

        private void InitializePage()
        {
            InitializeVariables();

            InitializeInterface();
        }

        private void InitializeVariables()
        {
            if (this._ws == null)
                this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            controller = new DataExportController();
            controller.modelExportedStartedEvent += DataExportUC_modelExportStartedEvent;
            controller.modelExportCompletedEvent += DataExportUC_modelExportCompletedEvent;
            controller.subModelExportCompletedEvent += DataExportUC_subModelExportCompletedEvent;
            controller.exportStartedEvent += DataExportUC_exportStartedEvent;
            controller.exportCompletedEvent += DataExportUC_exportCompletedEvent;
        }

        private void InitializeInterface()
        {
            this.pbarMain.Value = 0;
            this.pbarSub.Value = 0;
            this.blblMain.Text = string.Empty;
            this.blblSub.Text = string.Empty;
        }

        public void SetSPCModels(SPCModel[] spcModels)
        {
            if(controller == null)
                return;

            controller.SetSPCModelDataExport(spcModels);
        }

        private void bbtnExport_Click(object sender, EventArgs e)
        {
            //SPC-828 Louis
            this.bbtnExport.Enabled = false;
            this.bbtnOpenFolder.Enabled = false;

            if (FromDate.StartDateTime >= ToDate.StartDateTime )
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIO", null, null);
                //SPC-832 Louis
                this.bbtnExport.Enabled = true;
                this.bbtnOpenFolder.Enabled = true;
                return;
            }

            if(string.IsNullOrEmpty(btxtSavedFolder.Text))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_FOLDER", null, null);
                //SPC-832 Louis
                this.bbtnExport.Enabled = true;
                this.bbtnOpenFolder.Enabled = true;
                return;
            }

            if(!System.IO.Directory.Exists(btxtSavedFolder.Text))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_FOLDER_NOT_EXIST", null, null);
                //SPC-832 Louis
                this.bbtnExport.Enabled = true;
                this.bbtnOpenFolder.Enabled = true;
                return;
            }

            if(this.controller == null)
                return;
            this.controller.Export(btxtSavedFolder.Text, this.FromDate.StartDateTime.ToString(Definition.DATETIME_FORMAT_MS), 
                this.ToDate.StartDateTime.ToString(Definition.DATETIME_FORMAT_MS));

            this.bbtnExport.Enabled = true;
            this.bbtnOpenFolder.Enabled = true;
        }

        private void bbtnOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(DialogResult.OK == dialog.ShowDialog())
            {
                btxtSavedFolder.Text = dialog.SelectedPath;
            }
        }

        void DataExportUC_modelExportStartedEvent(object sender, ModelExportStartedEventHandlerArgs e)
        {
            SPCModelDataExport export = (SPCModelDataExport) sender;
            this.pbarSub.Value = 0;
            this.pbarSub.Maximum = export.ChartCount;
            this.blblMain.Text = this.pbarMain.Value + " / " + this.pbarMain.Maximum;
            this.blblSub.Text = "0 / " + export.ChartCount;

            if(modelExportedStartedEvent != null)
                modelExportedStartedEvent(sender, e);

            System.Threading.Thread.Sleep(50);
            Application.DoEvents();
        }

        void DataExportUC_modelExportCompletedEvent(object sender, ModelExportCompletedEventHandlerArgs e)
        {
            this.pbarMain.Value++;
            this.blblMain.Text = this.pbarMain.Value + " / " + this.pbarMain.Maximum;

            if(modelExportCompletedEvent != null)
                this.modelExportCompletedEvent(sender, e);

            System.Threading.Thread.Sleep(50);
            Application.DoEvents();
        }

        void DataExportUC_subModelExportCompletedEvent(object sender, SubModelExportCompletedEventHandlerArgs e)
        {
            this.pbarSub.Value++;
            this.blblSub.Text = this.pbarSub.Value + " / " + this.pbarSub.Maximum;

            if(subModelExportCompletedEvent != null)
                this.subModelExportCompletedEvent(sender, e);

            System.Threading.Thread.Sleep(50);
            Application.DoEvents();
        }

        void DataExportUC_exportCompletedEvent(object sender, ExportCompletedEventHandlerArgs e)
        {
            if (e.exportedModelNum == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EXPORT_DATA_NOT_EXIST", null, null);
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EXPORT_COMPLETE", null, null);
            }
            

            if(exportCompletedEventHandler != null)
                exportCompletedEventHandler(sender, e);
        }

        void DataExportUC_exportStartedEvent(object sender, ExportStartedEventHandlerArgs e)
        {
            this.pbarMain.Value = 0;
            this.pbarSub.Value = 0;
            this.pbarMain.Maximum = e.mainModelNum;
            this.blblMain.Text = "0 / " + e.mainModelNum;
            this.blblSub.Text = "0 / 0";

            if(exportStartedEventHandler != null)
                exportStartedEventHandler(sender, e);

            System.Threading.Thread.Sleep(50);
            Application.DoEvents();
        }
    }
}
