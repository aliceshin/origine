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
    public class ParseCLOB
    {
        private BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();

        public enum ParseType
        {
            Recipe,
            RecipeStep,
            LotRecipe,
            LotRecipeStep
        }

        public ParseCLOB()
        {


        }

        protected LinkedList _llstLineInfo = null;
        protected LinkedList _llstParamName = null;

        #region Decompress
        /// <summary>
        /// OCAPListUC
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>

        public DataSet DecompressData(DataTable _dtData, LinkedList _llstCondition, LinkedList _llstContextType, bool bDataParsing, bool includingToggleOffData)
        {
            DataSet dsRetrun = null;
            string sRef = string.Empty;
            CommonUtility _comUtil = new CommonUtility();
            string[] arrData = null;
            string[] arrContext = null;
            string strData_List = "";
            string strContext_List = "";
            try
            {
                int idxTemp = 0;
                for (int i = 0; i < _dtData.Columns.Count; i++)
                {
                    if (_dtData.Columns[i].ToString().IndexOf(COLUMN.DATA_LIST) >= 0)
                    {
                        idxTemp++;
                    }
                }
                if (_dtData != null && _dtData.Rows.Count > 0)
                {
                    dsRetrun = new DataSet();
                    foreach (DataRow drData in _dtData.Rows)
                    {
                        strData_List = "";
                        if (idxTemp <= 1)
                        {
                            strData_List = drData[COLUMN.DATA_LIST].ToString();
                            strContext_List = drData[COLUMN.CONTEXT_LIST].ToString();
                        }
                        else
                        {
                            strData_List = drData[COLUMN.DATA_LIST].ToString();
                            strContext_List = drData[COLUMN.CONTEXT_LIST].ToString();
                            for (int i = 1; i < idxTemp; i++)
                            {
                                strData_List += drData[COLUMN.DATA_LIST + i.ToString()].ToString();
                                strContext_List += drData[COLUMN.CONTEXT_LIST + i.ToString()].ToString();
                            }
                        }

                        arrContext = strContext_List.ToString().Split('\t');
                        arrData = strData_List.ToString().Split('\t');

                        AddColumn(arrContext, _dtData, drData, _llstContextType, bDataParsing);
                        AddColumn(arrData, _dtData, drData, _llstContextType, bDataParsing);
                    }

                    if (!_dtData.Columns.Contains(Definition.CHART_COLUMN.TABLENAME))
                    {
                        _dtData.Columns.Add(Definition.CHART_COLUMN.TABLENAME);
                    }
                    
                    for(int i=0; i<_dtData.Rows.Count; i++)
                    {
                        _dtData.Rows[i][Definition.CHART_COLUMN.TABLENAME] = Definition.TableName.USERNAME_TEMPDATA_SHORT;
                    }

                    if(!includingToggleOffData)
                        DeleteToggleOffData(_dtData);

                    DataRow[] drSelectParams = _dtData.Select(_comUtil.NVL(_llstCondition[Definition.CONDITION_KEY_CONTEXT_KEY_LIST]));
                    dsRetrun.Merge(drSelectParams);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                if (_dtData != null) _dtData.Dispose();
            }

            return dsRetrun;
        }

        private void DeleteToggleOffData(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                string toggleYN = dr[Definition.COL_TOGGLE_YN].ToString();
                if(toggleYN == "Y")
                    dr.Delete();
            }
        }

        /// <summary>
        /// OCAPListUC
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        public DataSet DecompressOCAPDataTRXData(DataTable _dtData, LinkedList _llstContextType, bool bDataParsing)
        {
            DataSet dsRetrun = null;
            try
            {
                if (_dtData != null && _dtData.Rows.Count > 0)
                {

                    foreach (DataRow drData in _dtData.Rows)
                    {
                        string[] arrContext = drData[COLUMN.CONTEXT_LIST].ToString().Split('\t');
                        AddColumn(arrContext, _dtData, drData, _llstContextType, bDataParsing);
                    }


                    DataTable dt = new DataTable();
                    dsRetrun = new DataSet();
                    dt = _dtData.Copy();
                    dt.TableName = Definition.TableName.USERNAME_TEMPDATA;
                    dsRetrun.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                if (_dtData != null) _dtData.Dispose();
            }

            return dsRetrun;
        }

        #endregion


        #region AddColumn()
        private void AddColumn(string[] arrContext, DataTable dtData, DataRow drData, LinkedList _llstContextType, bool bDataParsing)
        {
            string strKey = string.Empty;
            string strContextKey = string.Empty;
            string strContextValue = string.Empty;
            string[] arr = null;
            string param_alias = string.Empty;
            SPCStruct.ContextTypeInfo mContextTypeInfo = null;
            for (int k = 0; k < arrContext.Length; k++)
            {
                if (arrContext[k].IndexOf("=") > -1)
                {
                    arr = arrContext[k].Split('=');
                    strContextKey = arr[0].ToUpper();
                    strContextValue = arr[1];

                    string strContextKey2 = strContextKey;
                    if (_llstContextType[strContextKey] != null)
                    {
                        mContextTypeInfo = _llstContextType[strContextKey] as SPCStruct.ContextTypeInfo;
                        strContextKey2 = mContextTypeInfo.NAME;
                    }

                    SetAddColumn(dtData, strContextKey, strContextValue, drData, strContextKey2, bDataParsing);
                }
            }
        }

        #region Etc. AddColumn Methods
        private void SetAddColumn(DataTable _dt, string _strContextKey, string _strContextValue, DataRow _dr, string _strContextKey2, bool bDataParsing)
        {
            if (!_dt.Columns.Contains(_strContextKey2) && _strContextKey2.Length > 0)
            {
                if (_strContextKey2.ToUpper().Equals(Definition.CHART_COLUMN.P_USL)
                        || _strContextKey2.ToUpper().Equals(Definition.CHART_COLUMN.P_LSL)
                        || _strContextKey2.ToUpper().Equals(Definition.CHART_COLUMN.U_USL)
                        || _strContextKey2.ToUpper().Equals(Definition.CHART_COLUMN.U_LSL)
                        )
                    return;
                else
                    _dt.Columns.Add(_strContextKey2);
            }

            if (_strContextValue.IndexOf("@") > -1)
            {
                if (_strContextKey == Definition.BLOB_FIELD_NAME.PN ||
                    _strContextKey == Definition.BLOB_FIELD_NAME.C
                    )
                {
                    if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._LSL]))
                        _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._LSL], typeof(string));

                    if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._USL]))
                        _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._USL], typeof(string));
                }

                if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._TARGET]))
                    _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._TARGET], typeof(string));

                if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._LCL]))
                    _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._LCL], typeof(string));

                if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._UCL]))
                    _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._UCL], typeof(string));

                if (!_dt.Columns.Contains(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._RULE]))
                    _dt.Columns.Add(_strContextKey + Definition.ChartDataList[(int)enumChartDataList._RULE], typeof(string));
            }

            if (bDataParsing && _strContextValue.IndexOf("@") > -1)
            {
                string[] arr = _strContextValue.Split('@');
                if (arr.Length > 1)
                {
                    string[] arrSub = arr[1].Split('^');
                    if (_dt.Columns.Contains(_strContextKey))
                    {
                        if (arr[0].Equals("NaN"))
                        {
                        }
                        else
                        {
                            _dr[_strContextKey] = arr[0];
                        }
                    }
                    else
                    {
                        _dt.Columns.Add(_strContextKey);
                        if (arr[0].Equals("NaN"))
                        {
                        }
                        else
                        {
                            _dr[_strContextKey] = arr[0];
                        }
                    }
                    
                    for (int k = 0; k < arrSub.Length; k++)
                    {
                        if (string.IsNullOrEmpty(arrSub[k])) continue;

                        string _colName = string.Format("{0}{1}", _strContextKey, Definition.ChartDataList[k]);
                        if (_dt.Columns.Contains(_colName))
                        {
                            if (arrSub[k].Equals("NaN"))
                            {
                                continue;
                            }
                            else
                            {
                                _dr[_colName] = arrSub[k];
                            }
                        }
                        else
                        {
                            //modified by enkim 2012.05.11 SPC-851
                            if (!(_colName.ToUpper().Equals(Definition.CHART_COLUMN.PN_USL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.PN_LSL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.C_USL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.C_LSL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.PN_UCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.PN_LCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.P_UCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.P_LCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.C_UCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.C_LCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.U_UCL)
                                || _colName.ToUpper().Equals(Definition.CHART_COLUMN.U_LCL)
                                ))
                            {
                                _dt.Columns.Add(_colName);
                                if (arrSub[k].Equals("NaN"))
                                {
                                    continue;
                                }
                                else
                                {
                                    _dr[_colName] = arrSub[k];
                                }
                            }
                            //modified end SPC-851
                        }
                    }
                }
            }
            else
            {
                if (_strContextValue.Equals("NULL") || _strContextValue.Equals("NaN"))
                {
                }
                else
                {
                    _dr[_strContextKey2] = _strContextValue;
                }
            }
        }



        private void SetParamValue(DataTable _dt, string _strContextKey, string _strContextValue, DataRow _dr, string _ref)
        {
            if (string.IsNullOrEmpty(_strContextKey)) return;
            string[] arr = _strContextKey.Split(';');
            string[] arrValue = _strContextValue.Split(';');
            for (int i = 0; i < arr.Length; i++)
            {
                if (string.IsNullOrEmpty(arr[i])) continue;

                if (!_dt.Columns.Contains(_ref + arr[i]))
                {
                    _dr[_ref + arr[i]] = arrValue[i];
                }
            }
        }

        #endregion

        #endregion
    }
}
