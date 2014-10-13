using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Tool
{
    public class SPCModelDataExport : SPCModel
    {
        public event ModelExportStartedEventHandler modelExportStartedEvent;
        public event ModelExportCompletedEventHandler modelExportCompletedEvent;
        public event SubModelExportCompletedEventHandler subModelExportCompletedEvent;
        private int exportedSubModel = 0;
        
        public SPCModelDataExport(SPCModel model)
        {
            this.SPCModelName = model.SPCModelName;
            this.SPCModelRawID = model.SPCModelRawID;
            this.ChartID = model.ChartID;
            this.ParamType = model.ParamType;
            this.Version = model.Version;
            this.IsMainModel = model.IsMainModel;
            this.SubModels = new List<SPCModel>();
            foreach(SPCModel sub in model.SubModels)
            {
                this.SubModels.Add(new SPCModelDataExport(sub));
            }
        }

        internal int Export(string filePath, string startDtts, string endDtts)
        {
            exportedSubModel = 0;

            if(modelExportStartedEvent != null)
            {
                var ModelStartedArgs = new ModelExportStartedEventHandlerArgs();
                modelExportStartedEvent(this, ModelStartedArgs);
            }

            bool hasData = false;
            using (ExcelWriter writer = new ExcelWriter(filePath))
            {
                writer.WriteStartDocument();

                // write a main chart
                DataTable rawData = GetRawData(startDtts, endDtts, this.ChartID);
                rawData.TableName = this.ChartID;
                RemoveSuperflousColumns(rawData);

                if(rawData.Rows.Count > 0)
                {
                    hasData = true;
                    writer.WriteSheet("Main-" + ChartID, rawData);
                    exportedSubModel++;
                }

                rawData.Dispose();
                if(subModelExportCompletedEvent != null)
                {
                    var subArgs = new SubModelExportCompletedEventHandlerArgs();
                    subArgs.SubModelRawID = ChartID;
                    subModelExportCompletedEvent(this, subArgs);
                }

                // write sub charts
                foreach (SPCModel sub in this.SubModels)
                {
                    rawData = GetRawData(startDtts, endDtts, sub.ChartID);
                    rawData.TableName = sub.ChartID;
                    RemoveSuperflousColumns(rawData);
                    
                    if(rawData.Rows.Count > 0)
                    {
                        hasData = true;
                        writer.WriteSheet(sub.ChartID, rawData);
                        exportedSubModel++;
                    }

                    rawData.Dispose();
                    if(subModelExportCompletedEvent != null)
                    {
                        var subArgs = new SubModelExportCompletedEventHandlerArgs();
                        subArgs.SubModelRawID = sub.ChartID;
                        subModelExportCompletedEvent(this, subArgs);
                    }
                }

                writer.WriteEndDocument();
                writer.Close();
            }

            var modelArgs = new ModelExportCompletedEventHandlerArgs();
            modelArgs.SPCModelName = this.SPCModelName;
            modelArgs.exportedModelNum = exportedSubModel;
            modelArgs.filePath = filePath;
            if(hasData == false)
            {
                System.IO.File.Delete(filePath);
                modelArgs.filePath = string.Empty;
            }
            

            if(modelExportCompletedEvent != null)
                modelExportCompletedEvent(this, modelArgs);

            return exportedSubModel;
        }

        private void RemoveSuperflousColumns(DataTable dt)
        {
            if(dt.Columns.Contains(Definition.CHART_COLUMN.ORDERINFILEDATA))
                dt.Columns.Remove(Definition.CHART_COLUMN.ORDERINFILEDATA);
            if(dt.Columns.Contains(Definition.CHART_COLUMN.TABLENAME))
                dt.Columns.Remove(Definition.CHART_COLUMN.TABLENAME);
            if(dt.Columns.Contains(Definition.CHART_COLUMN.DTSOURCEID))
                dt.Columns.Remove(Definition.CHART_COLUMN.DTSOURCEID);
        }
    }
}
