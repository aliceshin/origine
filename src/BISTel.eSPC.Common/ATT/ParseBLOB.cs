using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Common.ATT
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

        public ParseBLOB()
        {


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

        #region Decompress


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
                            lstColumn.Add(col.ColumnName.ToString());
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
                                        sColumnData[iColCount] = sData[iRow];
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
                
                if(string.IsNullOrEmpty(toggle))
                    continue;

                if(toggle == "Y")
                    dr.Delete();
            }
        }

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

            this._llstLineInfo = new LinkedList();

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
                        strTempKey = strKey;
                    }
                    if (string.IsNullOrEmpty(strTempKey)) continue;

                    if (strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.P_USL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.P_LSL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.U_USL)
                        || strTempKey.ToUpper().Equals(Definition.CHART_COLUMN.U_LSL)
                        )
                    {
                        line = sr.ReadLine();
                        continue;
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

        #endregion
    }
}
