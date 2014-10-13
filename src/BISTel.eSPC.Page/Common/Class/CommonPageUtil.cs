using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.DataHandler;


namespace BISTel.eSPC.Page.Common
{
    public class CommonPageUtil
    {

        public static LinkedList GetOCAPParameter(DataTable dt, int iRow)
        {
            LinkedList _llstParam = new LinkedList();
            if (dt == null) return _llstParam;            
                                              
            _llstParam.Add(Definition.CONDITION_KEY_PARAM_ALIAS, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_PARAM_ALIAS, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_LOT_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_LOT_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_SUBSTRATE_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_SUBSTRATE_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_CASSETTE_SLOT, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_CASSETTE_SLOT, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_RECIPE_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_RECIPE_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_PRODUCT_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_PRODUCT_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_OPERATION_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_OPERATION_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_EQP_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_EQP_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_MODULE_NAME, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_MODULE_NAME, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_STEP_ID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_STEP_ID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_CONTEXT_LIST, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_CONTEXT_LIST, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_OOC_PROBLEM, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_OOC_PROBLEM, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_OOC_CAUSE, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_OOC_CAUSE, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_OOC_SOLUTION, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_OOC_SOLUTION, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_CONTEXT_KEY, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_CONTEXT_KEY, iRow, dt));
            _llstParam.Add(Definition.CONDITION_KEY_MODEL_NAME, CommonPageUtil.ColumnsContains(Definition.CONDITION_KEY_MODEL_NAME, iRow, dt));
            _llstParam.Add(Definition.CHART_COLUMN.TIME, CommonPageUtil.ColumnsContains(Definition.CHART_COLUMN.TIME, iRow, dt));
         
            return  _llstParam;      
        }
             
        public static string ColumnsContains(string _column,int iRow, DataTable _dt)
        {
            if (_dt.Columns.Contains(_column))
               return _dt.Rows[iRow][_column].ToString() ;
            else
               return null;                                    
        }

       
        public static string ToDay()
        {
            return DateTime.Today.ToString(Definition.DATETIME);
        }

        public static string ToDayStart()
        {
            return DateTime.Today.ToString(Definition.DATETIME) + " 00:00:00";
        }


        public static string ToDayEnd()
        {
            return DateTime.Today.ToString(Definition.DATETIME) + " 23:59:59";
        }
     
        public static string ConvertDate(string sDateTime)
        {
            return DateTime.Parse(sDateTime).ToString(Definition.DATETIME);
        }

        public static string StartDate(string sDateTime)
        {
            //return DateTime.Parse(sDateTime).ToString(Definition.DATETIME) +" 00:00:00";
            return DateTime.Parse(sDateTime).ToString(Definition.DATETIME_FORMAT);
        }

        public static string EndDate(string sDateTime)
        {
            //return DateTime.Parse(sDateTime).ToString(Definition.DATETIME) +" 23:59:59";
            return DateTime.Parse(sDateTime).ToString(Definition.DATETIME_FORMAT);
        }

        //SPC Calculation DateFormat
        public static string CalcStartDate(string sDateTime)
        {
            return DateTime.Parse(sDateTime).ToString(Definition.DATETIME) +" 00:00:00";
            
        }

        public static string CalcEndDate(string sDateTime)
        {
            return DateTime.Parse(sDateTime).ToString(Definition.DATETIME) +" 23:59:59";            
        }

        public static int SearchPeriod(string sStartDateTime, string sEndDateTime )
        {
            int iDay = DateTime.Parse(sEndDateTime).AddDays(-DateTime.Parse(sStartDateTime).Day).Day;
            return iDay;
        }

       
                
        public static List<string> DefaultChartSplit(string _sDefaultChart)
        {
            string[] arrChart = _sDefaultChart.Split(';');            
            List<string> lst = new List<string>();            
            for (int i = 0; i < arrChart.Length; i++)
            {
                lst.Add(arrChart[i].ToString());
            }
        
            return lst;
        }


        #region Condition



        public static List<string> GetDataListToListType(DataTable dt, string _conditionKey)
        {
            List<string> _lst = new List<string>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _lst.Add(dt.Rows[i][_conditionKey].ToString());
                }
            }
            catch (Exception ex)
            {
                return _lst;
            }
            return _lst;
        }

        public static ArrayList GetConditionKeyDataListArr(DataTable dt, string _conditionKey)
        {
            ArrayList _arr = new ArrayList();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _arr.Add(dt.Rows[i][_conditionKey].ToString());
                }
            }
            catch (Exception ex)
            {
                return _arr;
            }
            return _arr;
        }
        
        public static ArrayList GetConditionKeyDataList(DataTable dt, string _conditionKey, ArrayList _arr)
        {            
            if(_arr==null) _arr=new ArrayList();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _arr.Add(dt.Rows[i][_conditionKey].ToString());                                     
                }
            }
            catch (Exception ex)
            {
                return _arr;
            }
            return _arr;
        }


        
        

        public static string  GetConditionKeyArrayList(ArrayList arlst)
        {
            if (arlst == null) return null;
            
            string str = string.Empty;
            try
            {
                for (int i = 0; i < arlst.Count; i++)
                {                    
                    str += arlst[i].ToString()+",";                    
                }

                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);                    
                }
                
            }
            catch (Exception ex)
            {
                return str;
            }

            return str;
        }


        public static DataTable GetDataRowSelect(DataTable _dt, LinkedList _llstCondition)
        {
            if (DataUtil.IsNullOrEmptyDataTable(_dt)) return null;     
            DataRow[] drSelect = _dt.Select(CommonPageUtil.GetDataRowSelectString(_llstCondition));
            return DataUtil.DataTableImportRow(drSelect);
        }


        public static string GetDataRowSelectString (LinkedList _llstCondition)
        {            
            StringBuilder sb = new StringBuilder();
            if(_llstCondition !=null && _llstCondition.Count ==0) return null;
            sb.Append(" 1 = 1");
            for (int i = 0; i < _llstCondition.Count; i++)
            {                
                    string  sValue = _llstCondition.GetValue(i).ToString();
                    string sKey = _llstCondition.GetKey(i).ToString();
                    string [] arr = sValue.Split(',');
                    if(arr.Length>1)
                    {                        
                        sb.AppendFormat(" AND {0} IN ({1})", sKey, sValue);                        
                    }
                    else
                    {
                        sb.AppendFormat(" AND {0} ={1}", sKey, sValue);
                    }
            }
            return sb.ToString();
        }


        public static string GetConCatString(string _value)
        {        
            if(string.IsNullOrEmpty(_value)) 
            return _value;
            else
                return "'"+_value+"'";
        }

        public static string GetConditionKeyStringArray(string[] arrString) //추가
        {
            if (arrString == null) return null;

            string str = string.Empty;
            try
            {
                for (int i = 0; i < arrString.Length; i++)
                {
                    str += arrString[i].ToString() + ",";
                }

                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                }

            }
            catch (Exception ex)
            {
                return str;
            }

            return str;
        }

        public static ArrayList ConvertStringarrayToArrayList(string[] arString) //추가
        {
            if (arString == null) return null;

            ArrayList al = new ArrayList();
            try
            {
                for (int i = 0; i < arString.Length; i++)
                {
                    al.Add(arString[i].ToString());
                }
            }
            catch (Exception ex)
            {
                return al;
            }
            return al;
        }

        
        #endregion 
        
        

        public static ArrayList AddArrayList(string argArr)
        {        
            ArrayList arrList = new ArrayList();
            arrList.Add(argArr);
            return arrList;
        }


        public static ArrayList AddArrayList(LinkedList _llst)
        {    
            ArrayList arrList = new ArrayList();
            arrList.Add(_llst);
            return arrList;
        }

        
        #region Parsing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="_llstWhere"></param>
        /// <param name="bDataParsing"></param>
        /// <returns></returns>
        public static DataTable CLOBnBLOBParsing(DataSet _ds, LinkedList _llstWhere, bool bDataParsing)
        {                       
            return CommonPageUtil.Parsing(_ds, _llstWhere, bDataParsing, false, false);
        }
        
        public static DataTable CLOBnBLOBParsing(DataSet _ds, LinkedList _llstWhere, bool bDataParsing, bool bMoCVD, bool includingToggleData)
        {
            return CommonPageUtil.Parsing(_ds, _llstWhere, bDataParsing, bMoCVD, includingToggleData);
        }

        public static DataTable CLOBnBLOBParsingRaw(DataSet _ds, LinkedList _llstWhere, bool bDataParsing, bool includingToggleData)
        {
            return CommonPageUtil.ParsingRaw(_ds, _llstWhere, bDataParsing, includingToggleData);
        }

        public static LinkedList SetContextType( DataSet argDataSet)
        {
            LinkedList _llst = new LinkedList();
            if (DataUtil.IsNullOrEmptyDataSet(argDataSet)) return _llst;

            SPCStruct.ContextTypeInfo contextTypeInfo = null;
            foreach (DataRow dr in argDataSet.Tables[0].Rows)
            {
                contextTypeInfo = new SPCStruct.ContextTypeInfo(dr[COLUMN.CODE].ToString(), dr[COLUMN.NAME].ToString());                
                _llst.Add(dr[COLUMN.CODE].ToString(), contextTypeInfo);
            }
            return _llst;
        }
        public static DataTable Parsing(DataSet _ds, LinkedList _llstWhere, bool bDataParsing, bool bMoCVD, bool includingToggleData)
        {
            ParseBLOB parseBlob = null;
            ParseCLOB parseClob = null;
            DataSet _dsSelect = null;
            DataTable dtResult = new DataTable();
            bool bResultTable = false;
            try
            {
                parseBlob = new ParseBLOB();
                parseClob = new ParseCLOB();
                _dsSelect = new DataSet();
                eSPCWebService.eSPCWebService _wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
                LinkedList _llstData = new LinkedList();
                //_llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
                DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());                

                LinkedList mllstContextType = SetContextType( dsContextType);
                DataTableGroupBy dtGroupBy = null;


                for (int i = 0; i < _ds.Tables.Count; i++)
                {
                    string sTableName = _ds.Tables[i].TableName.ToString();
                    if (sTableName == Definition.TableName.USERNAME_DATA)
                    {
                        bResultTable = true;
                        _dsSelect = parseBlob.DecompressDATA_TRX_DATA(_ds.Tables[i], _llstWhere, mllstContextType,bDataParsing, includingToggleData);
                        if (_dsSelect.Tables.Count > 0)                        
                            dtResult = _dsSelect.Tables[0].Copy();
                                                    
                        _dsSelect.Dispose();
                        if (dtResult.Columns.Contains(COLUMN.FILE_DATA)) dtResult.Columns.Remove(COLUMN.FILE_DATA);
                    }
                    else if (sTableName == Definition.TableName.USERNAME_TEMPDATA)
                    {
                        _dsSelect = parseClob.DecompressData(_ds.Tables[i], _llstWhere, mllstContextType, bDataParsing, includingToggleData);
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect)){                            
                            DataTable dt = _dsSelect.Tables[0];                           
                            if (bResultTable)
                            {
                            
                                foreach(DataColumn dc in dt.Columns)
                                {            
                                    if( dc.ColumnName == COLUMN.DATA_LIST ||dc.ColumnName ==COLUMN.CONTEXT_LIST ) continue;                                               
                                    if (!dtResult.Columns.Contains(dc.ColumnName.ToString()))
                                        dtResult.Columns.Add(dc.ColumnName.ToString(), dc.DataType);
                                }
                                
                                foreach (DataRow dr in dt.Rows)
                                    dtResult.ImportRow(dr);
                            }
                            else
                            {
                                dtResult.Merge(dt);
                                if (dtResult.Columns.Contains(COLUMN.DATA_LIST)) dtResult.Columns.Remove(COLUMN.DATA_LIST);
                                if (dtResult.Columns.Contains(COLUMN.CONTEXT_LIST)) dtResult.Columns.Remove(COLUMN.CONTEXT_LIST);
                            }
                        }
                    }
                }

                dtResult.Columns.Add(Definition.CHART_COLUMN.DTSOURCEID);
                for(int i=0; i < dtResult.Rows.Count; i++)
                {
                    dtResult.Rows[i][Definition.CHART_COLUMN.DTSOURCEID] = i;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (_ds != null) _ds.Dispose();
                if (parseBlob != null) parseBlob = null;
                if (parseClob != null) parseClob = null;
                if (_dsSelect != null) _dsSelect.Dispose();
            }

            return dtResult;
        }

        public static DataTable ParsingRaw(DataSet _ds, LinkedList _llstWhere, bool bDataParsing, bool includingToggleData)
        {
            ParseBLOB parseBlob = null;
            ParseCLOB parseClob = null;
            DataSet _dsSelect = null;
            DataTable dtResult = new DataTable();
            bool bResultTable = false;
            try
            {
                parseBlob = new ParseBLOB();
                parseClob = new ParseCLOB();
                _dsSelect = new DataSet();
                eSPCWebService.eSPCWebService _wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
                LinkedList _llstData = new LinkedList();
                DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());

                LinkedList mllstContextType = SetContextType(dsContextType);
                DataTableGroupBy dtGroupBy = null;


                for (int i = 0; i < _ds.Tables.Count; i++)
                {
                    string sTableName = _ds.Tables[i].TableName.ToString();
                    if (sTableName == Definition.TableName.USERNAME_DATA)
                    {
                        bResultTable = true;
                        _dsSelect = parseBlob.DecompressDATA_TRX_DATA(_ds.Tables[i], _llstWhere, mllstContextType, bDataParsing, includingToggleData);
                        if (_dsSelect.Tables.Count > 0)
                            dtResult = _dsSelect.Tables[0].Copy();

                        _dsSelect.Dispose();
                        if (dtResult.Columns.Contains(COLUMN.FILE_DATA)) dtResult.Columns.Remove(COLUMN.FILE_DATA);
                    }
                    else if (sTableName == Definition.TableName.USERNAME_TEMPDATA)
                    {
                        _dsSelect = parseClob.DecompressData(_ds.Tables[i], _llstWhere, mllstContextType, bDataParsing, includingToggleData);
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                        {
                            DataTable dt = _dsSelect.Tables[0];
                            if (bResultTable)
                            {

                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (dc.ColumnName.IndexOf(COLUMN.DATA_LIST) >= 0 || dc.ColumnName.IndexOf(COLUMN.CONTEXT_LIST) >= 0) continue;
                                    if (!dtResult.Columns.Contains(dc.ColumnName.ToString()))
                                        dtResult.Columns.Add(dc.ColumnName.ToString(), dc.DataType);
                                }

                                foreach (DataRow dr in dt.Rows)
                                    dtResult.ImportRow(dr);
                            }
                            else
                            {
                                dtResult.Merge(dt);
                                ArrayList arrTempColName = new ArrayList();
                                foreach (DataColumn dc in dtResult.Columns)
                                {
                                    if (dc.ColumnName.IndexOf(COLUMN.DATA_LIST) >= 0 || dc.ColumnName.IndexOf(COLUMN.CONTEXT_LIST) >= 0)
                                    {
                                        arrTempColName.Add(dc.ColumnName);
                                    }
                                }
                                if(arrTempColName.Count > 0)
                                {
                                    for(int iTmp=0;iTmp<arrTempColName.Count;iTmp++)
                                    {
                                        dtResult.Columns.Remove(arrTempColName[iTmp].ToString());
                                    }
                                }
                                //if (dtResult.Columns.Contains(COLUMN.DATA_LIST)) dtResult.Columns.Remove(COLUMN.DATA_LIST);
                                //if (dtResult.Columns.Contains(COLUMN.CONTEXT_LIST)) dtResult.Columns.Remove(COLUMN.CONTEXT_LIST);
                            }
                        }
                        bResultTable = true;
                    }
                    else if (sTableName == Definition.TableName.USERNAME_SUMTEMPDATA)
                    {
                        _dsSelect = parseClob.DecompressDataRaw(_ds.Tables[i], _llstWhere, mllstContextType, bDataParsing);
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSelect))
                        {
                            DataTable dt = _dsSelect.Tables[0];
                            if (bResultTable)
                            {

                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (dc.ColumnName == COLUMN.DATA_LIST || dc.ColumnName == COLUMN.CONTEXT_LIST) continue;
                                    if (!dtResult.Columns.Contains(dc.ColumnName.ToString()))
                                        dtResult.Columns.Add(dc.ColumnName.ToString(), dc.DataType);
                                }

                                foreach (DataRow dr in dt.Rows)
                                    dtResult.ImportRow(dr);
                            }
                            else
                            {
                                dtResult.Merge(dt);
                                if (dtResult.Columns.Contains(COLUMN.DATA_LIST)) dtResult.Columns.Remove(COLUMN.DATA_LIST);
                                if (dtResult.Columns.Contains(COLUMN.CONTEXT_LIST)) dtResult.Columns.Remove(COLUMN.CONTEXT_LIST);
                            }
                        }
                    }
                }

                dtResult.Columns.Add(Definition.CHART_COLUMN.DTSOURCEID);
                for(int i=0; i < dtResult.Rows.Count; i++)
                {
                    dtResult.Rows[i][Definition.CHART_COLUMN.DTSOURCEID] = i;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (_ds != null) _ds.Dispose();
                if (parseBlob != null) parseBlob = null;
                if (parseClob != null) parseClob = null;
                if (_dsSelect != null) _dsSelect.Dispose();
            }

            return dtResult;
        }
        
        #endregion 

        public static DataTable ExcelExport(DataTable dt, List<string> _lstRawColumn)
        {
            DataTable dtExcel = new DataTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();
                if ((sCol.IndexOf("_RULE") > -1)
                    || sCol.Equals("WORKDATE")
                    || sCol.Equals("SAMPLE_QTY")
                    || (sCol.IndexOf(COLUMN.DEFAULT_CHART_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.DATA_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_KEY) > -1)
                    || (sCol.IndexOf(COLUMN.FILE_DATA) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_DAYS) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_COUNT) > -1)
                    || (sCol.IndexOf(COLUMN.COMPLEX_YN) > -1)
                    || (sCol.IndexOf(COLUMN.MAIN_YN) > -1)
                    || (sCol.IndexOf(Definition.COL_TOGGLE_YN) > -1)
                    || (sCol.IndexOf(Definition.COL_TOGGLE) > -1)
                    || (sCol.IndexOf(COLUMN.SAMPLE_COUNT) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.ORDERINFILEDATA) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.TABLENAME) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.DTSOURCEID) > -1)
                   )
                    dt.Columns.RemoveAt(i);
                else if (sCol.IndexOf(Definition.CHART_COLUMN.RAW) > -1)
                {
                    if (_lstRawColumn.Count > 0)
                        dt.Columns.RemoveAt(i);
                }
                else if (sCol.IndexOf("PARAM_LIST") > -1)
                {
                    if (_lstRawColumn.Count > 0)
                        dt.Columns.RemoveAt(i);
                }
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();

                if (!dtExcel.Columns.Contains(sCol))
                {
                    if (CommonChart.PARAM_ITEM.Contains(sCol))
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else if (sCol == Definition.CHART_COLUMN.TIME)
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else if (sCol == Definition.COL_MODEL_CONFIG_RAWID)
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else
                        dtExcel.Columns.Add(sCol, dt.Columns[i].DataType);
                }
            }

            dt.AcceptChanges();

            foreach (DataRow dr in dt.Rows)
                dtExcel.ImportRow(dr);

            return dtExcel;

        }

        public static DataTable ExcelExportWithRaw(DataTable dt, List<string> _lstRawColumn)
        {
            DataTable dtExcel = new DataTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();
                if ((sCol.IndexOf("_RULE") > -1)
                    || sCol.Equals("WORKDATE")
                    || sCol.Equals("SAMPLE_QTY")
                    || (sCol.IndexOf(COLUMN.DEFAULT_CHART_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.DATA_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_KEY) > -1)
                    || (sCol.IndexOf(COLUMN.FILE_DATA) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_DAYS) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_COUNT) > -1)
                    || (sCol.IndexOf(COLUMN.COMPLEX_YN) > -1)
                    || (sCol.IndexOf(COLUMN.MAIN_YN) > -1)
                   )
                    dt.Columns.RemoveAt(i);
                else if (sCol.IndexOf(Definition.CHART_COLUMN.RAW) > -1)
                {
                    if (sCol != COLUMN.RAW_UCL && sCol != COLUMN.RAW_LCL)
                    {
                        if (_lstRawColumn.Count > 0)
                            dt.Columns.RemoveAt(i);
                    }
                }
                else if (sCol.IndexOf("PARAM_LIST") > -1)
                {
                    if (_lstRawColumn.Count > 0)
                        dt.Columns.RemoveAt(i);
                }
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();

                if (!dtExcel.Columns.Contains(sCol))
                {
                    if (CommonChart.PARAM_ITEM.Contains(sCol))
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else if (sCol == Definition.CHART_COLUMN.TIME)
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else
                        dtExcel.Columns.Add(sCol, dt.Columns[i].DataType);
                }
            }

            dt.AcceptChanges();

            foreach (DataRow dr in dt.Rows)
                dtExcel.ImportRow(dr);

            return dtExcel;

        }

        public static DataTable ExcelExportRaw(DataTable dt, List<string> _lstRawColumn)
        {
            DataTable dtExcel = new DataTable();
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();
                if ((sCol.IndexOf("_RULE") > -1)
                    || sCol.Equals("WORKDATE")
                    || sCol.Equals("SAMPLE_QTY")
                    || (sCol.IndexOf(COLUMN.DEFAULT_CHART_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.DATA_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_LIST) > -1)
                    || (sCol.IndexOf(COLUMN.CONTEXT_KEY) > -1)
                    || (sCol.IndexOf(COLUMN.FILE_DATA) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_DAYS) > -1)
                    || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_COUNT) > -1)
                    || (sCol.IndexOf(COLUMN.COMPLEX_YN) > -1)
                    || (sCol.IndexOf(COLUMN.MAIN_YN) > -1)
                    || (sCol.IndexOf("WAFER") > -1)
                    || (sCol.IndexOf(Definition.COL_TOGGLE_YN) > -1)
                    || (sCol.IndexOf(Definition.COL_TOGGLE) > -1)
                    || (sCol.IndexOf(COLUMN.SAMPLE_COUNT) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.ORDERINFILEDATA) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.TABLENAME) > -1)
                    || (sCol.IndexOf(Definition.CHART_COLUMN.DTSOURCEID) > -1)
                   )
                    dt.Columns.RemoveAt(i);
                else if (sCol.IndexOf("PARAM_LIST") > -1)
                {
                    if (_lstRawColumn.Count > 0)
                        dt.Columns.RemoveAt(i);
                }
            }

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();

                if (!dtExcel.Columns.Contains(sCol))
                {
                    if (CommonChart.PARAM_ITEM.Contains(sCol))
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else if (sCol == Definition.CHART_COLUMN.TIME)
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else if (sCol == Definition.COL_MODEL_CONFIG_RAWID)
                        dtExcel.Columns.Add(sCol, typeof(string));
                    else
                        dtExcel.Columns.Add(sCol, dt.Columns[i].DataType);
                }
            }

            dt.AcceptChanges();

            foreach (DataRow dr in dt.Rows)
                dtExcel.ImportRow(dr);

            return dtExcel;

        }
        
        
    }
    
}
