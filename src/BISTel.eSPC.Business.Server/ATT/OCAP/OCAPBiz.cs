using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Business.Server.ATT.OCAP
{
    public class OCAPBiz : DataBase
    {
        private BISTel.eSPC.Data.Server.OCAP.ATTOCAPData _ocapData;

        public OCAPBiz()
        {
            this._ocapData = new BISTel.eSPC.Data.Server.OCAP.ATTOCAPData();
        }
        
        #region : SELECT
        public DataSet GetOCAPList(byte[] baData)
        {
            return this._ocapData.GetOCAPListData(baData);
        }
        #endregion
    }
}

