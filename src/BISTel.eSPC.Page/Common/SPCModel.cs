using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Common
{
    public class SPCModel
    {
        public string SPCModelName = string.Empty;

        /// <summary>
        /// RAWID COLUM of MODEL_MST_SPC Table
        /// </summary>
        public string SPCModelRawID = string.Empty;
        public string ChartID = string.Empty;
        public List<SPCModel> SubModels = new List<SPCModel>();
        public string ParamType = string.Empty;
        public string Version = string.Empty;
        public bool IsMainModel = false;

        public int ChartCount
        {
            get { return SubModels.Count + 1; }
        }

        private eSPCWebService.eSPCWebService _ws = null;

        public override string ToString()
        {
            return SPCModelName;
        }

        internal DataTable GetRawData(string startDtts, string endDtts, string chartID)
        {
            if(_ws == null)
                _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            LinkedList llstCondition = new LinkedList();
            llstCondition.Add(Definition.CONDITION_KEY_START_DTTS, startDtts);
            llstCondition.Add(Definition.CONDITION_KEY_END_DTTS, endDtts);
            llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, chartID);

            DataSet ds = _ws.GetSPCControlChartData(llstCondition.GetSerialData());
            
            if(this.ParamType.ToUpper() == "MET")
            {
                return CommonPageUtil.CLOBnBLOBParsing(ds, llstCondition, false, false, false);
            }
            
            return CommonPageUtil.CLOBnBLOBParsingRaw(ds, llstCondition, false, false);
        }
    }
}
