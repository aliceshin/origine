using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Common.DataQuery
{
    public class eSPCWebServiceAsync : BISTel.PeakPerformance.Client.DataHandler.WebAsyncCall
    {
        public eSPCWebServiceAsync()
		{
            base._Service = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
		}

        public DataSet GetSPCControlChartDataAsync(byte[] btdata)
		{
			object[] param = new object[1];
			param[0] = btdata;
            return (DataSet)base.AsyncMethodCall("GetSPCControlChartData", param);
		}

        public DataSet GetOCAPListAsync(byte[] btdata)
        {
            object[] param = new object[1];
            param[0] = btdata;
            return (DataSet)base.AsyncMethodCall("GetOCAPList", param);
        }
    }
}
