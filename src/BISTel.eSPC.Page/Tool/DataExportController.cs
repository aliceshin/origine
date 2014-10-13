using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Tool
{
    public class DataExportController
    {
        public SubModelExportCompletedEventHandler subModelExportCompletedEvent;
        public ModelExportCompletedEventHandler modelExportCompletedEvent;
        public ModelExportStartedEventHandler modelExportedStartedEvent;
        public ExportStartedEventHandler exportStartedEvent;
        public ExportCompletedEventHandler exportCompletedEvent;

        public List<SPCModelDataExport> spcModelDataExports = new List<SPCModelDataExport>();

        public void SetSPCModelDataExport(SPCModel[] spcModels)
        {
            spcModelDataExports = new List<SPCModelDataExport>();
            try
            {
                foreach (SPCModel model in spcModels)
                {
                    spcModelDataExports.Add(new SPCModelDataExport(model));
                }
            }catch(Exception ex)
            {
            }
        }

        public void Export(string folderPath, string startDtts, string endDtts)
        {
            var exportStartedArgs = new ExportStartedEventHandlerArgs();
            exportStartedArgs.folderPath = folderPath;
            exportStartedArgs.mainModelNum = this.spcModelDataExports.Count;
            exportStartedEvent(this, exportStartedArgs);

            string[] fileName = new string[spcModelDataExports.Count];

            //파일명 유효성 검사
            for (int i = 0; i < spcModelDataExports.Count; i++)
            {
                fileName[i] = spcModelDataExports[i].SPCModelName;
                foreach (char ch in System.IO.Path.GetInvalidFileNameChars())
                {
                    if (spcModelDataExports[i].SPCModelName.Contains(ch.ToString()))
                    {
                        string[] str = fileName[i].Split(ch);
                        fileName[i] = string.Join("", str);
                    }
                }

                //foreach (char ch in System.IO.Path.GetInvalidFileNameChars())
                //{
                //    this.controller.spcModelDataExports[i].SPCModelName = this.controller.spcModelDataExports[i].SPCModelName.Replace(ch, '_');
                //}
            }

            if(!folderPath.EndsWith(@"\"))
                folderPath += @"\";

            string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int iTotalCnt = 0;

            for (int i = 0; i < spcModelDataExports.Count; i++)
            {
                spcModelDataExports[i].modelExportCompletedEvent -= export_modelExportCompletedEvent;
                spcModelDataExports[i].subModelExportCompletedEvent -= export_subModelExportCompletedEvent;
                spcModelDataExports[i].modelExportStartedEvent -= DataExportController_modelExportStartedEvent;

                spcModelDataExports[i].modelExportCompletedEvent += export_modelExportCompletedEvent;
                spcModelDataExports[i].subModelExportCompletedEvent += export_subModelExportCompletedEvent;
                spcModelDataExports[i].modelExportStartedEvent += DataExportController_modelExportStartedEvent;

                string filePath = folderPath + fileName[i] + "_" + dateTime + ".xls";
                iTotalCnt += spcModelDataExports[i].Export(filePath, startDtts, endDtts);
            }

            var exportCompletedArgs = new ExportCompletedEventHandlerArgs();
            exportCompletedArgs.exportedModelNum = iTotalCnt;
            exportCompletedEvent(this, exportCompletedArgs);
        }

        void DataExportController_modelExportStartedEvent(object sender, ModelExportStartedEventHandlerArgs args)
        {
            if(modelExportedStartedEvent != null)
            {
                modelExportedStartedEvent(sender, args);
            }
        }

        void export_subModelExportCompletedEvent(object sender, SubModelExportCompletedEventHandlerArgs args)
        {
            if(subModelExportCompletedEvent != null)
                subModelExportCompletedEvent(sender, args);
        }

        void export_modelExportCompletedEvent(object sender, ModelExportCompletedEventHandlerArgs args)
        {
            if (modelExportCompletedEvent != null)
            {
                modelExportCompletedEvent(sender, args);
            }
        }
    }
}
