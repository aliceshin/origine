using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.eSPCWebService
{
    public partial class eSPCWebService : BISTel.PeakPerformance.Client.DataHandler.IWebAsyncCall
    {
        #region IWebAsyncCall Members

        public void CancelAsyncMethod(object userState)
        {
            base.CancelAsync(userState);
        }

        public void InvokeAsyncMethod(string methodName, object[] parameters, System.Threading.SendOrPostCallback callBack)
        {
            base.InvokeAsync(methodName, parameters, callBack);
        }

        #endregion
    }
}
