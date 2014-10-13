using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Business.Server.ATT.Report
{
    public class ProcessCapabilityBiz : DataBase
    {

        private BISTel.eSPC.Data.Server.Report.ATTProcessCapabilityData _pcData;

        public ProcessCapabilityBiz()
        {
            this._pcData = new BISTel.eSPC.Data.Server.Report.ATTProcessCapabilityData();
        }

        #region : SELECT

        public DataSet GetPpkReport(byte[] baData)
        {
            DataTable dtResult = new DataTable();
            DataSet _dsSelect = this._pcData.GetPpkReport(baData);
            if (DataUtil.IsNullOrEmptyDataSet(_dsSelect)) return _dsSelect;

            dtResult = _dsSelect.Tables[0].Copy();
            dtResult.TableName = Definition.TableName.USERNAME_DATA.ToString();

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtResult);
            return dsReturn;
        }

        /// <summary>
        /// ProcessCapabilityUC 화면에서 사용됨
        /// </summary>
        /// <param name="baData"></param>
        /// <returns></returns>
        public DataSet GetDataTRXData(byte[] baData)
        {

            DataTable dtResult = new DataTable();
            DataSet _dsSelect = this._pcData.GetDataTRXData(baData);
            if (DataUtil.IsNullOrEmptyDataSet(_dsSelect)) return _dsSelect;

            dtResult = _dsSelect.Tables[0].Copy();
            dtResult.TableName = Definition.TableName.USERNAME_DATA.ToString();

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtResult);
            return dsReturn;
        }
        #endregion




    }


}

