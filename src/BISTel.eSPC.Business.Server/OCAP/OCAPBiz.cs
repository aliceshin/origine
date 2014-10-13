using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using BISTel.eSPC.Data.Server;

namespace BISTel.eSPC.Business.Server.OCAP
{
    public class OCAPBiz : DataBase
    {

        private BISTel.eSPC.Data.Server.OCAP.OCAPData _ocapData;

        public OCAPBiz()
        {
            this._ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();
        }


        #region : SELECT



        public DataSet GetOCAPList(byte[] baData)
        {
            //ParseCLOB parseClob = new ParseCLOB();
            //DataSet _ds = this._ocapData.GetOCAPListData(baData);
            //if (DataUtil.IsNullOrEmptyDataSet(_ds)) return _ds;
            //DataSet dsDecompressData = parseClob.DecompressOCAPDataTRXData(_ds);
            return this._ocapData.GetOCAPListData(baData);
        }

        #endregion

        #region : INSERT


        #endregion

        #region : Update

        #endregion

        #region : Delete


        #endregion


    }


}

