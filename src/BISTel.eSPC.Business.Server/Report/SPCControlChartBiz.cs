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

namespace BISTel.eSPC.Business.Server.Report
{
    public class SPCControlChartBiz : DataBase
    {

        private BISTel.eSPC.Data.Server.Report.SPCControlChartData _dacData;
        private BISTel.eSPC.Data.Server.Common.CommonData _commonData;

        public SPCControlChartBiz()
        {
            this._dacData = new BISTel.eSPC.Data.Server.Report.SPCControlChartData();
            this._commonData = new BISTel.eSPC.Data.Server.Common.CommonData();
        }


        #region : SELECT

        public DataSet GetSPCControlChartData(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            DataSet _dsSelect = new DataSet();
            DataTable dtResult = new DataTable();
            try
            {
                #region DATA COMPRESS PROCESS ENDED OR NOT
                bool isPassedComp = true;
                LinkedList llstData = new LinkedList();
                string strSQL = "";

                llstData.Add("PROPERTY", "SPC_DATA_COMP_TIME");
                strSQL = " SELECT PROPERTY_VALUE FROM CONFIG_PROPERTY_MST_PP WHERE PROPERTY_NAME = :PROPERTY ";
                DataSet dsTemp = base.Query(strSQL, llstData);

                if (dsTemp != null && dsTemp.Tables != null && dsTemp.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        int idxTemp = Convert.ToInt32(dsTemp.Tables[0].Rows[0][0].ToString());
                        strSQL = "SELECT TRUNC((SYSDATE - TRUNC(SYSDATE, 'DD')) * 24) FROM DUAL";
                        dsTemp = base.Query(strSQL);
                        if (dsTemp != null && dsTemp.Tables != null && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                int idxCurrent = Convert.ToInt32(dsTemp.Tables[0].Rows[0][0].ToString());

                                if (idxCurrent >= idxTemp)
                                {
                                    isPassedComp = true;
                                }
                                else
                                {
                                    isPassedComp = false;
                                }
                            }
                            catch
                            {
                                isPassedComp = true;
                            }
                        }
                    }
                    catch
                    {
                        isPassedComp = true;
                    }
                }
                else
                {
                    isPassedComp = true;
                }

                #endregion

                #region GET DATA_TRX_SPC
                if (isPassedComp)
                {
                    _dsSelect = this._dacData.GetSPCControlChartData(baData);
                }
                else
                {
                    string sEndDateTRX = string.Empty;
                    string sStartDateTRX = string.Empty;
                    LinkedList llstConditionTRX = new LinkedList();
                    llstConditionTRX.SetSerialData(baData);

                    if (llstConditionTRX[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                    {
                        sStartDateTRX = llstConditionTRX[Definition.DynamicCondition_Condition_key.START_DTTS].ToString();
                    }
                    if (llstConditionTRX[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                    {
                        sEndDateTRX = llstConditionTRX[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();
                    }

                    if (int.Parse(DateTime.Parse(sEndDateTRX).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                    {
                        llstConditionTRX.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                        llstConditionTRX.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);

                        if (int.Parse(DateTime.Parse(sStartDateTRX).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                        {
                        }
                        else
                        {
                            llstConditionTRX.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartDateTRX);
                            llstConditionTRX.Add(Definition.DynamicCondition_Condition_key.END_DTTS, DateTime.Today.AddDays(-1).ToString(Definition.DATETIME) + " 00:00:00");
                            _dsSelect = this._dacData.GetSPCControlChartData(llstConditionTRX.GetSerialData());
                        }
                    }
                    else
                    {
                        _dsSelect = this._dacData.GetSPCControlChartData(baData);
                    }
                }
                #endregion

                if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                {
                    dtResult = _dsSelect.Tables[0].Copy();
                    dtResult.TableName = Definition.TableName.USERNAME_DATA.ToString();
                    dsReturn.Tables.Add(dtResult);
                }

                /* 오늘일짜 Date*/
                string sEndDate = string.Empty;
                string sStartDate = string.Empty;
                LinkedList llstCondition = new LinkedList();
                LinkedList llstConditionTempTRX = new LinkedList();
                llstCondition.SetSerialData(baData);
                llstConditionTempTRX.SetSerialData(baData);
                if (llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                {
                    sStartDate = llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString();
                }
                if (llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                {
                    sEndDate = llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();
                }

                #region GET DATA_TEMP_TRX_SPC
                if (isPassedComp)
                {
                    if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                    {
                        llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                        llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                        if (int.Parse(DateTime.Parse(sStartDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                            llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartDate);
                        else
                            llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.ToString(Definition.DATETIME) + " 00:00:00");
                        llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndDate);
                        _dsSelect = _dacData.GetSPCControlChartToDayData(llstCondition.GetSerialData());
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                        {
                            dtResult = _dsSelect.Tables[0].Copy();
                            dtResult.TableName = Definition.TableName.USERNAME_TEMPDATA.ToString();
                            dsReturn.Tables.Add(dtResult);
                        }
                    }
                }
                else
                {
                    if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                    {
                        llstConditionTempTRX.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                        llstConditionTempTRX.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                        if (int.Parse(DateTime.Parse(sStartDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                        {
                            llstConditionTempTRX.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartDate);
                        }
                        else
                        {
                            llstConditionTempTRX.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.AddDays(-1).ToString(Definition.DATETIME) + " 00:00:00");
                        }
                        llstConditionTempTRX.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndDate);
                        _dsSelect = _dacData.GetSPCControlChartToDayData(llstConditionTempTRX.GetSerialData());
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                        {
                            dtResult = _dsSelect.Tables[0].Copy();
                            dtResult.TableName = Definition.TableName.USERNAME_TEMPDATA.ToString();
                            dsReturn.Tables.Add(dtResult);
                        }
                    }
                }
                #endregion


                #region GET DATA_SUM_TEMP_TRX_SPC
                if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                {
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                    if (int.Parse(DateTime.Parse(sStartDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD)))
                    {
                        llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartDate);
                    }
                    else
                    {
                        llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.AddDays(-1).ToString(Definition.DATETIME) + " 00:00:00");
                    }
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndDate);

                    _dsSelect = _dacData.GetSPCChartViewSumTempData(llstCondition.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                    {
                        dtResult = _dsSelect.Tables[0].Copy();
                        dtResult.TableName = Definition.TableName.USERNAME_SUMTEMPDATA.ToString();
                        dsReturn.Tables.Add(dtResult);
                    }
                }
                #endregion
            }
            catch
            {
            }
            finally
            {
                if (_dsSelect != null) _dsSelect.Dispose();
                if (dtResult != null) dtResult.Dispose();
            }

            #region
            //ParseBLOB parseBlob = null;
            //ParseCLOB parseClob = null; 
            //DataSet _dsSelect =null;     
            //DataSet dsReturn = new DataSet();       
            //DataTable dtResult = new DataTable();
            //try{
            //    parseBlob = new ParseBLOB();
            //    _dsSelect = this._dacData.GetSPCControlChartData(baData);                
            //    if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
            //    {
            //        _dsSelect = parseBlob.DecompressDATA_TRX_DATA(_dsSelect, baData);
            //        for (int i = 0; i < _dsSelect.Tables.Count; i++)
            //            dtResult.Merge(_dsSelect.Tables[i]);                
            //        if (dtResult.Columns.Contains(COLUMN.FILE_DATA)) dtResult.Columns.Remove(COLUMN.FILE_DATA);                
            //    }

            //    /* 오늘일짜 Date*/            
            //    string sEndDate =string.Empty;
            //    LinkedList llstCondition = new LinkedList();
            //    llstCondition.SetSerialData(baData);
            //    if (llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)                        
            //        sEndDate = llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();

            //    if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
            //    {
            //        parseClob = new ParseCLOB();
            //        bool bResultTable = DataUtil.IsNullOrEmptyDataTable(dtResult) ? false : true;
            //        llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
            //        llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
            //        llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS,DateTime.Today.AddDays(-2).ToString(Definition.DATETIME) + " 00:00:00");
            //        llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS,DateTime.Today.ToString(Definition.DATETIME) + " 23:59:59");
            //        _dsSelect = _dacData.GetSPCControlChartToDayData(llstCondition.GetSerialData());

            //        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
            //        {
            //            _dsSelect = parseClob.DecompressData(_dsSelect, baData);
            //            if (bResultTable)
            //            {  
            //                foreach (DataRow dr in _dsSelect.Tables[0].Rows)
            //                {
            //                    dtResult.ImportRow(dr);
            //                } 

            //            }else                        
            //            {
            //                dtResult.Merge(_dsSelect.Tables[0]);
            //                if (dtResult.Columns.Contains(COLUMN.DATA_LIST)) dtResult.Columns.Remove(COLUMN.DATA_LIST);
            //                if (dtResult.Columns.Contains(COLUMN.CONTEXT_LIST)) dtResult.Columns.Remove(COLUMN.CONTEXT_LIST);
            //            }                                 
            //        }           
            //    }

            //    dsReturn.Tables.Add(dtResult);
            //}catch{
            //}finally{
            //    if(_dsSelect !=null) _dsSelect.Dispose();
            //    if(parseBlob != null)parseBlob=null;
            //    if (parseClob != null) parseBlob = null;
            //}

            #endregion

            return dsReturn;
        }

        //SPC-704 MultiCalcurate
        public DataSet GetSPCMulControlChartData(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            DataSet _dsSelect = new DataSet();
            DataTable dtResult = new DataTable();
            try
            {
                _dsSelect = this._dacData.GetSPCMulControlChartData(baData);
                if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                {
                    dtResult = _dsSelect.Tables[0].Copy();
                    dtResult.TableName = Definition.TableName.USERNAME_DATA.ToString();
                    dsReturn.Tables.Add(dtResult);
                }

                /* 오늘일짜 Date*/
                string sEndDate = string.Empty;
                string sStartDate = string.Empty;
                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);
                if (llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                    sStartDate = llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString();
                if (llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                    sEndDate = llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();

                if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                {
                    bool bResultTable = DataUtil.IsNullOrEmptyDataTable(dtResult) ? false : true;
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                    if (int.Parse(DateTime.Parse(sStartDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                        llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartDate);
                    else
                        llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.ToString(Definition.DATETIME) + " 00:00:00");
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndDate);
                    _dsSelect = _dacData.GetSPCControlChartToDayData(llstCondition.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                    {

                        dtResult = _dsSelect.Tables[0].Copy();
                        dtResult.TableName = Definition.TableName.USERNAME_TEMPDATA.ToString();
                        dsReturn.Tables.Add(dtResult);
                    }

                }
            }
            catch
            {
            }
            finally
            {
                if (_dsSelect != null) _dsSelect.Dispose();
                if (dtResult != null) dtResult.Dispose();
            }

            return dsReturn;
        }

        public DataSet GetSPCChartViewData(byte[] baData)
        {
            DataSet dsReturn = new DataSet();
            DataSet _dsSelect = new DataSet();
            DataTable dtResult = new DataTable();
            try
            {
                _dsSelect = this._dacData.GetSPCControlChartData(baData);
                if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                {
                    dtResult = _dsSelect.Tables[0].Copy();
                    dtResult.TableName = Definition.TableName.USERNAME_DATA.ToString();
                    dsReturn.Tables.Add(dtResult);
                }

                /* 오늘일짜 Date*/
                string sEndDate = string.Empty;
                LinkedList llstCondition = new LinkedList();
                llstCondition.SetSerialData(baData);
                if (llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                    sEndDate = llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();

                if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                {
                    bool bResultTable = DataUtil.IsNullOrEmptyDataTable(dtResult) ? false : true;
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.ToString(Definition.DATETIME) + " 00:00:00");
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndDate);
                    _dsSelect = _dacData.GetSPCControlChartToDayData(llstCondition.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                    {

                        dtResult = _dsSelect.Tables[0].Copy();
                        dtResult.TableName = Definition.TableName.USERNAME_TEMPDATA.ToString();
                        dsReturn.Tables.Add(dtResult);
                    }
                    _dsSelect = _dacData.GetSPCChartViewSumTempData(llstCondition.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                    {

                        dtResult = _dsSelect.Tables[0].Copy();
                        dtResult.TableName = Definition.TableName.USERNAME_SUMTEMPDATA.ToString();
                        dsReturn.Tables.Add(dtResult);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (_dsSelect != null) _dsSelect.Dispose();
                if (dtResult != null) dtResult.Dispose();
            }

            return dsReturn;
        }


        //public DataSet GetSPCControlChartToDayData(byte[] baData)
        //{
        //    //no use
        //    //ParseCLOB parseClob = new ParseCLOB();
        //    //DataSet _ds = this._dacData.GetSPCControlChartToDayData(baData);
        //    //if (DataUtil.IsNullOrEmptyDataSet(_ds)) return _ds;
        //    //DataSet dsReturn = parseClob.DecompressData(_ds, baData);
        //    return this._dacData.GetSPCControlChartToDayData(baData);
        //}

        public DataSet GetLastTRXDataDTTs(string modelConfigRawid)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = _commonData.GetLastTRXDataDTTs(modelConfigRawid);
            }
            catch
            {
            }
            return dsResult;
        }

        #endregion




    }


}

