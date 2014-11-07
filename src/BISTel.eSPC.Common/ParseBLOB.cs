using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Common
{
    public class ParseBLOB
    {
        private BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();

        public enum ParseType
        {
            Recipe,
            RecipeStep,
            LotRecipe,
            LotRecipeStep
        }

        protected LinkedList _llstLineInfo = null;
        protected LinkedList _llstParamName = null;

        string[] arrLineInfo = null;
        string[] arrParamName = null;

        //private DataTable _dtDefault = null;
        //private DataTable _dtParam = null;
        private string param_alias = string.Empty;

        private Dictionary<string, List<string[]>> _ColumnDatas = new Dictionary<string, List<string[]>>();
        private List<int> _DataCount = new List<int>();

        //ATT쪽과 code 합치는데 ATT와의 구분을 위하여 만들어주는 변수, KBLEE
        private bool isATT = false;

        public ParseBLOB()
        {
            //ATT와의 중복률 관련 refactoring, KBLEE
            isATT = this.GetType().FullName == "BISTel.eSPC.Common.ATT.ParseBLOB";
        }

        #region Decompress


        public DataSet DecompressDATA_TRX_DATA(DataTable _dtData, LinkedList _llstCondition, LinkedList _llstContextType)
        {
            return DecompressDATA_TRX_DATA(_dtData, _llstCondition, _llstContextType, false, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dtData"></param>
        /// <param name="_llstCondition"></param>
        /// <param name="bDataParsing"></param>
        /// <returns></returns>
        public DataSet DecompressDATA_TRX_DATA(DataTable _dtData, LinkedList _llstCondition, LinkedList _llstContextType, bool bDataParsing, bool includingToggleData)
        {
            DataSet dsReturn = null;
            DataTable dt = new DataTable();
            CommonUtility _comUtil = new CommonUtility();

            try
            {
                if (_dtData != null && _dtData.Rows.Count > 0)
                {
                    dsReturn = new DataSet();
                    List<string> lstColumn = new List<string>();
                    List<string> lstData = new List<string>();

                    foreach (DataColumn col in _dtData.Columns)
                    {
                        if (col.ColumnName.ToString() != COLUMN.FILE_DATA)
                        {
                            lstColumn.Add(col.ColumnName.ToString());
                        }
                    }

                    BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();
                    int iIndex = 0;

                    foreach (DataRow drData in _dtData.Rows)
                    {
                        lstData.Clear();

                        for (int i = 0; i < lstColumn.Count; i++)
                        {
                            lstData.Add(drData[lstColumn[i]].ToString());
                        }

                        //using (StreamReader sr = gZip.DecompressForStream(drData[COLUMN.FILE_DATA]))
                        using (StreamReader sr = CommonUtility.ConvertBLOBToStreamReader(drData[COLUMN.FILE_DATA]))
                        {
                            this.ParseHeader(sr);
                            this.ParseContextLineInfo(iIndex++, sr, _llstContextType, lstColumn, lstData);
                            //MakeDataTable(this._llstLineInfo, this._dtDefault, drData, lstColumn, _llstContextType);

                            //this.ParseContextParamName(sr, drData,bDataParsing);


                            //foreach (DataColumn dc in this._dtDefault.Columns)
                            //{
                            //    if (dc.ColumnName == COLUMN.FILE_DATA ) continue;
                            //    if (!dt.Columns.Contains(dc.ColumnName.ToString()))
                            //        dt.Columns.Add(dc.ColumnName.ToString().ToUpper(), dc.DataType);
                            //}                                                       

                            //string sWhere  = _comUtil.NVL(_llstCondition[Definition.CONDITION_KEY_CONTEXT_KEY_LIST]);
                            //if (!string.IsNullOrEmpty(sWhere))
                            //{
                            //    DataRow[] drSelectParams = this._dtDefault.Select(sWhere);                               
                            //    foreach (DataRow dr in drSelectParams)
                            //        dt.ImportRow(dr);
                            //}
                            //else
                            //    dt.Merge(this._dtDefault);
                        }
                    }

                    DataTable dtTable = new DataTable();
                    int iCol = 0;
                    int iLoopCount = 0;
                    List<string> columnNames = new List<string>();

                    foreach (KeyValuePair<string, List<string[]>> keyval in _ColumnDatas)
                    {
                        if (iCol == 0)
                        {
                            iLoopCount = keyval.Value.Count;
                            iCol++;
                        }
                        dtTable.Columns.Add(keyval.Key);
                        columnNames.Add(keyval.Key);
                    }

                    dtTable.Columns.Add(Definition.CHART_COLUMN.ORDERINFILEDATA, typeof (int));
                    columnNames.Add(Definition.CHART_COLUMN.ORDERINFILEDATA);
                    dtTable.Columns.Add(Definition.CHART_COLUMN.TABLENAME);
                    columnNames.Add(Definition.CHART_COLUMN.TABLENAME);
                    dtTable.Columns.Add(Definition.COL_TOGGLE_YN);
                    columnNames.Add(Definition.COL_TOGGLE_YN);

                    for (int i = 0; i < iLoopCount; i++)
                    {

                        for (int iRow = 0; iRow < _DataCount[i]; iRow++)
                        {
                            string[] sColumnData = new string[columnNames.Count];

                            for (int iColCount = 0; iColCount < _ColumnDatas.Count; iColCount++)
                            {
                                if (_ColumnDatas[columnNames[iColCount]].Count > i)
                                {
                                    string[] sData = _ColumnDatas[columnNames[iColCount]][i];

                                    if (sData != null && sData.Length > iRow && sData[iRow] != "NaN")
                                    {
                                        sColumnData[iColCount] = sData[iRow];
                                    }
                                }
                            }

                            sColumnData[columnNames.Count - 3] = iRow.ToString();
                            sColumnData[columnNames.Count - 2] = Definition.TableName.USERNAME_DATA_SHORT;

                            dtTable.Rows.Add(sColumnData);
                        }
                    }

                    FillToggleColumn(dtTable);

                    if(!includingToggleData)
                    {
                        DeleteToggleOffData(dtTable);
                    }

                    dsReturn.Tables.Add(dtTable);

                    //dsRetrun.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return dsReturn;
        }

        private void FillToggleColumn(DataTable dt)
        {
            for(int i=0; i<dt.Rows.Count; i++)
            {
                string[] temp = null;
                string toggle = "";

                if (dt.Columns.Contains(Definition.COL_TOGGLE))
                {
                    toggle = dt.Rows[i][Definition.COL_TOGGLE].ToString();
                }
                if(string.IsNullOrEmpty(toggle))
                {
                    dt.Rows[i][Definition.COL_TOGGLE_YN] = "N";
                }
                else
                {
                    temp = dt.Rows[i][Definition.COL_TOGGLE].ToString().Split(';');
                    dt.Rows[i][Definition.COL_TOGGLE_YN] = temp[int.Parse(dt.Rows[i][Definition.CHART_COLUMN.ORDERINFILEDATA].ToString())];
                }
            }
        }

        private void DeleteToggleOffData(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                string toggle = dr[Definition.COL_TOGGLE_YN].ToString();

                if (string.IsNullOrEmpty(toggle))
                {
                    continue;
                }

                if (toggle == "Y")
                {
                    dr.Delete();
                }
            }
        }

        #endregion

        #region MakeDataTable
        //private void MakeDataTable(LinkedList _llstInfo, DataTable dt, DataRow drParent, List<string> lstColumn, LinkedList _llstContextType)
        //{
        //    DataRow dr = null;
        //    SPCStruct.ContextTypeInfo mContextTypeInfo = null;


        //    for (int i = 0; i < _llstInfo.Count; i++)
        //    {
        //        string col = _llstInfo.GetKey(i).ToString();
        //        if (string.IsNullOrEmpty(col)) continue;

        //        string[] arr = (string[])_llstInfo.GetValue(i);
        //        for (int j = 1; j < arr.Length; j++)
        //        {
        //            if (i == 0)
        //                dr = dt.NewRow();
        //            else
        //            {
        //                if (_dtDefault.Rows.Count <= (j - 1)) continue;
        //                dr = _dtDefault.Rows[j - 1];
        //            }

        //            if (string.IsNullOrEmpty(arr[j].ToString())) continue;

        //            if (col.Equals(Definition.BLOB_FIELD_NAME.TIME))
        //            {
        //                dr[Definition.CHART_COLUMN.TIME] = DateTime.Parse(arr[j].ToString()).ToString(Definition.DATETIME_FORMAT_MS);
        //            }
        //            else
        //            {
        //                if (_llstContextType[col] != null)
        //                {
        //                    mContextTypeInfo = _llstContextType[col] as SPCStruct.ContextTypeInfo;
        //                    dr[mContextTypeInfo.NAME] = arr[j].ToString();
        //                }
        //                else
        //                    dr[col] = arr[j].ToString();
        //            }

        //            for (int k = 0; k < lstColumn.Count; k++)
        //            {
        //                dr[lstColumn[k].ToString()] = drParent[lstColumn[k].ToString()].ToString();
        //            }

        //            if (i == 0)
        //                dt.Rows.Add(dr);
        //        }
        //    }
        //}

        #endregion

        #region ::: Parsing
        /// <summary>
        /// header 값 parsing
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string[] ParseLine(string source)
        {
            string[] name_value = source.Split('=');

            if (name_value.Length == 2)
            {
                if (name_value[1].Length > 0)
                {
                    return name_value[1].Split('\t');
                }
            }

            return null;
        }

        private void ParseHeader(StreamReader sr)
        {
            string line = string.Empty;
            // string of header
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();

                if (line.IndexOf(Definition.BLOB_FIELD_NAME.HEADER_CLOSING) > -1)
                {
                    break;
                }
                if (line.IndexOf(Definition.BLOB_FIELD_NAME.LINE_INFO) > -1)
                {
                    arrLineInfo = ParseLine(line);
                }
                else if (line.IndexOf(Definition.BLOB_FIELD_NAME.PARAM_NAME) > -1)
                {
                    arrParamName = ParseLine(line);
                }
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="_llstInfo"></param>
        /// <param name="arrValue"></param>
        private void ParseContextLineInfo(int index, StreamReader sr, LinkedList _llstContextType, List<string> lstColumn, List<string> lstData)
        {
            string line = string.Empty;
            string[] arrStr = null;
            string[] arrInitStr = null;
            string[] arrStrRaw = null;
            string[] arrStrRefParamList = null;

            //this._dtDefault = new DataTable();
            this._llstLineInfo = new LinkedList();
            //SPCStruct.ContextTypeInfo mContextTypeInfo = null;
            
            try
            {
                for (int i = 0; i < arrLineInfo.Length + arrParamName.Length && sr.Peek() > -1; i++)
                {
                    string strKey = "";
                    string strTempKey = "";

                    if (i < arrLineInfo.Length)
                    {
                        strKey = arrLineInfo[i].ToString();
                        strTempKey = strKey;
                    }
                    else
                    {
                        strKey = arrParamName[i - arrLineInfo.Length].ToString();

                        if (strKey == "raw" && !isATT) //ATT와의 중복률 관련 refactoring, KBLEE
                        {
                            int _iTemp = lstColumn.IndexOf(Definition.CHART_COLUMN.PARAM_ALIAS);
                            strTempKey = lstData[_iTemp].ToString();
                        }
                        else
                        {
                            strTempKey = strKey;
                        }
                    }

                    if (string.IsNullOrEmpty(strTempKey))
                    {
                        continue;
                    }

                    //ATT와의 중복률 관련 refactoring, KBLEE
                    if (isATT)
                    {
                        if (strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.P_USL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.P_LSL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.U_USL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.U_LSL)
                        )
                        {
                            line = sr.ReadLine();
                            continue;
                        }
                    }
                    else
                    {
                        //modified by enkim 2012.05.11 SPC-851
                        if (strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.STDDEV_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.STDDEV_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.RANGE_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.RANGE_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MA_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MA_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MSD_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MSD_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MR_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.MR_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMARANGE_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMARANGE_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMAMEAN_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMAMEAN_LSL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMASTDDEV_USL)
                            || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.EWMASTDDEV_LSL)
                            )
                        {
                            line = sr.ReadLine();
                            continue;
                        }
                        //modified end SPC-851
                    }

                    line = sr.ReadLine();

                    if (line.IndexOf(strTempKey) > -1)
                    {
                        arrInitStr = line.Split('\t');

                        if (arrInitStr.Length == 1)
                        {
                            continue;
                        }

                        if (arrStrRaw != null && arrStrRaw.Length > 0)
                        {
                            if (arrInitStr.Length < arrStrRaw.Length + 1)
                            {
                                arrStr = new string[arrStrRaw.Length];

                                for (int _idxInit = 0; _idxInit < arrStrRaw.Length; _idxInit++)
                                {
                                    if (_idxInit < arrStrRaw.Length + 1 - arrInitStr.Length)
                                    {
                                        arrStr[_idxInit] = "";
                                    }
                                    else
                                    {
                                        arrStr[_idxInit] = arrInitStr[_idxInit - (arrStrRaw.Length - arrInitStr.Length)];
                                    }
                                }
                            }
                            else
                            {
                                arrStr = new string[arrInitStr.Length - 1];

                                for (int _idxInit = 1; _idxInit < arrInitStr.Length; _idxInit++)
                                {
                                    arrStr[_idxInit - 1] = arrInitStr[_idxInit];
                                }
                            }
                        }
                        else
                        {
                            arrStr = new string[arrInitStr.Length - 1];

                            for (int _idxInit = 1; _idxInit < arrInitStr.Length; _idxInit++)
                            {
                                arrStr[_idxInit - 1] = arrInitStr[_idxInit];
                            }
                        }

                        if (!isATT)
                        {
                            if (strKey == "raw")
                            {
                                arrStrRaw = (string[])arrStr.Clone();
                            }

                            if (strTempKey.Equals(Definition.BLOB_FIELD_NAME.REF_PARAM_LIST))
                            {
                                arrStrRefParamList = (string[])arrStr.Clone();
                            }
                            else if (strTempKey.Equals(Definition.BLOB_FIELD_NAME.REF_DATA))
                            {
                                LinkedList lnkLstParamList = new LinkedList();

                                for (int j = 0; j < arrStr.Length; j++)
                                {
                                    string[] strArrRefParamList = arrStrRefParamList[j].Split(';');
                                    string[] strArrRefDataList = arrStr[j].Split('@')[0].Split(';');

                                    for (int k = 0; k < strArrRefParamList.Length; k++)
                                    {
                                        if (strArrRefParamList[k] == null || strArrRefParamList[k].Length == 0)
                                        {
                                            continue;
                                        }

                                        if (lnkLstParamList.Contains(strArrRefParamList[k]))
                                        {
                                            ArrayList arrLstRawData = (ArrayList)lnkLstParamList[strArrRefParamList[k]];
                                            int _iArrCount = arrLstRawData.Count;

                                            for (int m = 0; m < j - _iArrCount; m++)
                                            {
                                                arrLstRawData.Add(null);
                                            }

                                            arrLstRawData.Add(strArrRefDataList[k]);
                                            lnkLstParamList.Remove(strArrRefParamList[k]);
                                            lnkLstParamList.Add(strArrRefParamList[k], arrLstRawData);
                                        }
                                        else
                                        {
                                            ArrayList arrLstRawData = new ArrayList();

                                            for (int m = 0; m < j; m++)
                                            {
                                                arrLstRawData.Add(null);
                                            }

                                            arrLstRawData.Add(strArrRefDataList[k]);
                                            lnkLstParamList.Add(strArrRefParamList[k], arrLstRawData);
                                        }
                                    }
                                }

                                for (int j = 0; j < lnkLstParamList.Count; j++)
                                {
                                    string strParamKey = lnkLstParamList.GetKey(j).ToString();
                                    string[] strarrParamDataList = (string[])((ArrayList)lnkLstParamList.GetValue(j)).ToArray(typeof(string));

                                    if (_ColumnDatas.ContainsKey(strParamKey))
                                    {
                                        if (_ColumnDatas[strParamKey].Count == index)
                                        {
                                            _ColumnDatas[strParamKey].Add(strarrParamDataList);
                                        }
                                        else
                                        {
                                            int iColumnDatasCnt = _ColumnDatas[strParamKey].Count;

                                            for (int iColTemp = 0; iColTemp < index - iColumnDatasCnt; iColTemp++)
                                            {
                                                _ColumnDatas[strParamKey].Add(null);
                                            }

                                            _ColumnDatas[strParamKey].Add(strarrParamDataList);
                                        }
                                    }
                                    else
                                    {
                                        if (index > 0)
                                        {
                                            List<string[]> data = new List<string[]>();
                                            data.Add(null);

                                            for (int iEmp = 0; iEmp < index - 1; iEmp++)
                                            {
                                                data.Add(null);
                                            }

                                            data.Add(strarrParamDataList);
                                            _ColumnDatas.Add(strParamKey, data);
                                        }
                                        else
                                        {
                                            List<string[]> data = new List<string[]>();
                                            data.Add(strarrParamDataList);
                                            _ColumnDatas.Add(strParamKey, data);
                                        }
                                    }
                                }
                            }
                            else if (strTempKey.Equals(Definition.BLOB_FIELD_NAME.PARAM_LIST))
                            {
                                LinkedList lnkLstParamList = new LinkedList();

                                for (int j = 0; j < arrStr.Length; j++)
                                {
                                    string[] strArrParamList = arrStr[j].Split(';');
                                    string[] strArrRawDataList = arrStrRaw[j].Split('@')[0].Split(';');

                                    for (int k = 0; k < strArrParamList.Length; k++)
                                    {
                                        if (strArrParamList[k] == null || strArrParamList[k].Length == 0 || string.IsNullOrEmpty(strArrRawDataList[k]))
                                        {
                                            continue;
                                        }

                                        if (lnkLstParamList.Contains(strArrParamList[k]))
                                        {
                                            ArrayList arrLstRawData = (ArrayList)lnkLstParamList[strArrParamList[k]];
                                            int _iArrCount = arrLstRawData.Count;

                                            for (int m = 0; m < j - _iArrCount; m++)
                                            {
                                                arrLstRawData.Add(null);
                                            }

                                            arrLstRawData.Add(strArrRawDataList[k]);
                                            lnkLstParamList.Remove(strArrParamList[k]);
                                            lnkLstParamList.Add(strArrParamList[k], arrLstRawData);
                                        }
                                        else
                                        {
                                            ArrayList arrLstRawData = new ArrayList();

                                            for (int m = 0; m < j; m++)
                                            {
                                                arrLstRawData.Add(null);
                                            }

                                            arrLstRawData.Add(strArrRawDataList[k]);
                                            lnkLstParamList.Add(strArrParamList[k], arrLstRawData);
                                        }
                                    }
                                }

                                for (int j = 0; j < lnkLstParamList.Count; j++)
                                {
                                    string strParamKey = lnkLstParamList.GetKey(j).ToString();
                                    string[] strarrParamDataList = (string[])((ArrayList)lnkLstParamList.GetValue(j)).ToArray(typeof(string));

                                    if (_ColumnDatas.ContainsKey(strParamKey))
                                    {
                                        if (_ColumnDatas[strParamKey].Count == index)
                                        {
                                            _ColumnDatas[strParamKey].Add(strarrParamDataList);
                                        }
                                        else
                                        {
                                            int iColumnDatasCnt = _ColumnDatas[strParamKey].Count;

                                            for (int iColTemp = 0; iColTemp < index - iColumnDatasCnt; iColTemp++)
                                            {
                                                _ColumnDatas[strParamKey].Add(null);
                                            }

                                            _ColumnDatas[strParamKey].Add(strarrParamDataList);
                                        }
                                    }
                                    else
                                    {
                                        if (index > 0)
                                        {
                                            List<string[]> data = new List<string[]>();
                                            data.Add(null);

                                            for (int iEmp = 0; iEmp < index - 1; iEmp++)
                                            {
                                                data.Add(null);
                                            }

                                            data.Add(strarrParamDataList);
                                            _ColumnDatas.Add(strParamKey, data);
                                        }
                                        else
                                        {
                                            List<string[]> data = new List<string[]>();
                                            data.Add(strarrParamDataList);
                                            _ColumnDatas.Add(strParamKey, data);
                                        }
                                    }
                                }
                            }
                        }

                        if (i == 0)
                        {
                            _DataCount.Add(arrStr.Length);

                            for (int k = 0; k < lstColumn.Count; k++)
                            {
                                string[] arrStrTemp = new string[arrStr.Length];

                                for (int j = 0; j < arrStrTemp.Length; j++)
                                {
                                    arrStrTemp[j] = lstData[k];
                                }

                                if (_ColumnDatas.ContainsKey(lstColumn[k]))
                                {
                                    if (_ColumnDatas[lstColumn[k]].Count == index)
                                    {
                                        _ColumnDatas[lstColumn[k]].Add(arrStrTemp);
                                    }
                                    else
                                    {
                                        int iColumnDatasCnt = _ColumnDatas[lstColumn[k]].Count;

                                        for (int iColTemp = 0; iColTemp < index - iColumnDatasCnt; iColTemp++)
                                        {
                                            _ColumnDatas[lstColumn[k]].Add(null);
                                        }

                                        _ColumnDatas[lstColumn[k]].Add(arrStrTemp);
                                    }
                                }
                                else
                                {
                                    if (index > 0)
                                    {
                                        List<string[]> data = new List<string[]>();
                                        data.Add(null);

                                        for (int iEmp = 0; iEmp < index - 1; iEmp++)
                                        {
                                            data.Add(null);
                                        }

                                        data.Add(arrStrTemp);
                                        _ColumnDatas.Add(lstColumn[k], data);
                                    }
                                    else
                                    {
                                        List<string[]> data = new List<string[]>();
                                        data.Add(arrStrTemp);
                                        _ColumnDatas.Add(lstColumn[k], data);
                                    }
                                }
                            }
                        }

                        if (!isATT)
                        {
                            if (_llstContextType[strKey] != null)
                            {
                                SPCStruct.ContextTypeInfo mContextTypeInfo = _llstContextType[strKey] as SPCStruct.ContextTypeInfo;
                                strKey = mContextTypeInfo.NAME.ToUpper();
                            }
                        }

                        if (_ColumnDatas.ContainsKey(strKey))
                        {
                            if (_ColumnDatas[strKey].Count == index)
                            {
                                _ColumnDatas[strKey].Add(arrStr);
                            }
                            else
                            {
                                int iColumnDatasCnt = _ColumnDatas[strKey].Count;

                                for (int iColTemp = 0; iColTemp < index - iColumnDatasCnt; iColTemp++)
                                {
                                    _ColumnDatas[strKey].Add(null);
                                }

                                _ColumnDatas[strKey].Add(arrStr);
                            }
                        }
                        else
                        {
                            if (index > 0)
                            {
                                List<string[]> data = new List<string[]>();
                                data.Add(null);

                                for (int iEmp = 0; iEmp < index - 1; iEmp++)
                                {
                                    data.Add(null);
                                }

                                data.Add(arrStr);
                                _ColumnDatas.Add(strKey, data);
                            }
                            else
                            {
                                List<string[]> data = new List<string[]>();
                                data.Add(arrStr);
                                _ColumnDatas.Add(strKey, data);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }


        //private void AddColumnParamList(string[] arrStr, DataTable _dtDefault, string _colkey, string _ref, bool bDataParsing)
        //{
        //    DataRow row = null;

        //    for (int j = 1; j < arrStr.Length; j++)
        //    {
        //        if (_dtDefault.Rows.Count <= (j - 1)) continue;
        //        if (arrStr[j].Trim().Length == 0 || arrStr[j].Trim().ToLower() == "null") continue;

        //        string[] arrRefCol = arrStr[j].Split(';');
        //        row = _dtDefault.Rows[j - 1];

        //        string[] arrRaw = null;
        //        if (string.IsNullOrEmpty(_ref))
        //        {
        //            string[] arrRawMain = row[Definition.CHART_COLUMN.RAW].ToString().Split('@');
        //            arrRaw = arrRawMain[0].Split(';');
        //        }

        //        for (int k = 0; k < arrRefCol.Length; k++)
        //        {
        //            if (string.IsNullOrEmpty(arrRefCol[k])) continue;

        //            if (!this._dtDefault.Columns.Contains(_ref + arrRefCol[k]))
        //                this._dtDefault.Columns.Add(_ref + arrRefCol[k], typeof(double));

        //            if (arrRaw != null && arrRaw.Length > k)
        //            {
        //                if (string.IsNullOrEmpty(arrRaw[k])) continue;
        //                row[_ref + arrRefCol[k]] = arrRaw[k];
        //            }
        //        }

        //        if (string.IsNullOrEmpty(_colkey)) continue;

        //        if (!this._dtDefault.Columns.Contains(_colkey))
        //            this._dtDefault.Columns.Add(_colkey, typeof(string));


        //        row[_colkey] = arrStr[j];
        //    }
        //}


        //private void SetParamValue(string[] arrStr, DataTable _dtDefault, string _colkey, string _ref)
        //{
        //    DataRow row = null;

        //    for (int j = 1; j < arrStr.Length; j++)
        //    {
        //        if (_dtDefault.Rows.Count <= (j - 1)) continue;
        //        if (arrStr[j].Trim().Length == 0 || arrStr[j].Trim().ToLower() == "null") continue;

        //        row = _dtDefault.Rows[j - 1];
        //        string[] arrRefValue = arrStr[j].Split(';');
        //        string[] arrRefCol = row[_colkey].ToString().Split(';');
        //        for (int k = 0; k < arrRefCol.Length; k++)
        //        {
        //            if (string.IsNullOrEmpty(arrRefCol[k])) continue;
        //            if (arrRefCol[k].Equals("null") || arrRefCol[k].Equals("NULL")) continue;
        //            if (k >= arrRefValue.Length) continue;
        //            if (string.IsNullOrEmpty(arrRefValue[k])) continue;

        //            if (this._dtDefault.Columns.Contains(_ref + arrRefCol[k]))
        //            {
        //                row[_ref + arrRefCol[k]] = arrRefValue[k];
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// PARAM_NAME,VALUE,TARGET,LSL,LCL,UCL,USL,FAULT_RULE_LIST 테이블 생성
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="arrValue"></param>
        //private void ParseContextParamName(StreamReader sr, DataRow nRow, bool bDataParsing)
        //{
        //    string line = string.Empty;
        //    string strKey = string.Empty;
        //    string[] arrStr = null;
        //    DataRow row = null;
        //    string _ref = "ref_";


        //    for (int i = 0; i < arrParamName.Length && sr.Peek() > -1; i++)
        //    {
        //        strKey = arrParamName[i].ToString().ToUpper();
        //        if (string.IsNullOrEmpty(strKey))
        //            continue;

        //        line = sr.ReadLine();
        //        arrStr = line.Split('\t');


        //        if (line.IndexOf(Definition.BLOB_FIELD_NAME.REF_PARAM_LIST) > -1)
        //        {
        //            AddColumnParamList(arrStr, _dtDefault, Definition.BLOB_FIELD_NAME.REF_PARAM_LIST, _ref, bDataParsing);
        //        }
        //        else if (line.IndexOf(Definition.BLOB_FIELD_NAME.REF_DATA) > -1)
        //        {
        //            SetParamValue(arrStr, _dtDefault, Definition.BLOB_FIELD_NAME.REF_PARAM_LIST, _ref);
        //        }
        //        else if (line.IndexOf(Definition.BLOB_FIELD_NAME.PARAM_LIST) > -1)
        //        {
        //            AddColumnParamList(arrStr, _dtDefault, Definition.BLOB_FIELD_NAME.PARAM_LIST, null, bDataParsing);
        //        }
        //        else
        //        {
        //            param_alias = string.Empty;
        //            if (strKey == Definition.CHART_COLUMN.PARAM_ALIAS && this._dtDefault.Columns.Contains(Definition.CHART_COLUMN.PARAM_ALIAS))
        //            {
        //                param_alias = _dtDefault.Rows[0][Definition.CHART_COLUMN.PARAM_ALIAS].ToString();
        //            }
        //            if (arrStr[0] == param_alias)
        //            {
        //                strKey = Definition.CHART_COLUMN.RAW;
        //            }

        //            if (!this._dtDefault.Columns.Contains(strKey))
        //                this._dtDefault.Columns.Add(strKey);

        //            for (int j = 1; j < arrStr.Length; j++)
        //            {
        //                if (_dtDefault.Rows.Count <= (j - 1)) continue;

        //                string sData = arrStr[j].Trim();
        //                if (sData.Length == 0) continue;

        //                row = _dtDefault.Rows[j - 1];

        //                if (j == 1 && sData.IndexOf("@") > -1)
        //                {
        //                    if (strKey == Definition.BLOB_FIELD_NAME.MIN ||
        //                        strKey == Definition.BLOB_FIELD_NAME.MAX)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        if (strKey == Definition.BLOB_FIELD_NAME.RAW || strKey == Definition.BLOB_FIELD_NAME.MEAN)
        //                        {
        //                            if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._LSL]))
        //                                this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._LSL], typeof(double));

        //                            if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._USL]))
        //                                this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._USL], typeof(double));
        //                        }

        //                        if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._TARGET]))
        //                            this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._TARGET], typeof(double));

        //                        if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._LCL]))
        //                            this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._LCL], typeof(double));

        //                        if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._UCL]))
        //                            this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._UCL], typeof(double));

        //                        if (!this._dtDefault.Columns.Contains(strKey + Definition.ChartDataList[(int)enumChartDataList._RULE]))
        //                            this._dtDefault.Columns.Add(strKey + Definition.ChartDataList[(int)enumChartDataList._RULE], typeof(string));
        //                    }
        //                }

        //                if (bDataParsing && sData.IndexOf("@") > -1)
        //                {
        //                    string[] arr = sData.Split('@');
        //                    if (arr.Length > 1)
        //                    {
        //                        string[] arrSub = arr[1].Split('^');
        //                        row[strKey] = arr[0];
        //                        for (int k = 0; k < arrSub.Length; k++)
        //                        {
        //                            if (string.IsNullOrEmpty(arrSub[k])) continue;

        //                            string _colName = string.Format("{0}{1}", strKey, Definition.ChartDataList[k]);
        //                            if (this._dtDefault.Columns.Contains(_colName))
        //                                row[_colName] = arrSub[k];
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    if (string.IsNullOrEmpty(sData)) continue;
        //                    row[strKey] = sData.Equals("NULL") ? DBNull.Value.ToString() : sData;
        //                }
        //            }
        //        }

        //    }
        //}

        #endregion
    }
}
