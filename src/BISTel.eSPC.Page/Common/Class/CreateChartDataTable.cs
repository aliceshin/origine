using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public class CreateChartDataTable
    {
        protected List<string> _lstRawColumn=new List<string>();
        protected DataTable _dtResourceData;
        protected string _strRawCol = string.Empty;
        protected string _complex_yn = "N";        
                                
        public List<string> lstRawColumn
        {
            get { return _lstRawColumn; }
        }
                                
        public string COMPLEX_YN
        {
            get { return _complex_yn; }
            set { _complex_yn = value; }
        }



        public List<string> CallRefCol(DataTable dtResourceData)
        {
            List<string> lstRawCol = new List<string>();            
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();
            DataTable dtParamList =null;   
            string sColumn = string.Empty;         
            if(COMPLEX_YN.Equals("Y") && dtResourceData.Columns.Contains(Definition.BLOB_FIELD_NAME.PARAM_LIST.ToUpper()))
            {
                    sColumn = Definition.BLOB_FIELD_NAME.PARAM_LIST.ToUpper();
                    dtParamList = DataUtil.DataTableImportRow(dtResourceData.Select(null, sColumn + " desc"));  
                                         
            }else
            {            
               if (dtResourceData.Columns.Contains(Definition.BLOB_FIELD_NAME.REF_PARAM_LIST.ToUpper()))
               {
                   sColumn = Definition.BLOB_FIELD_NAME.PARAM_LIST.ToUpper();
                   dtParamList = DataUtil.DataTableImportRow(dtResourceData.Select(null, sColumn + " desc"));
                   
               }
            }

            if (DataUtil.IsNullOrEmptyDataTable(dtParamList)) return lstRawCol;
            for (int j = 0; j < dtParamList.Rows.Count; j++)
            {
                _strRawCol = dtParamList.Rows[j][sColumn].ToString();

                if (string.IsNullOrEmpty(_strRawCol)) return lstRawCol;
                string[] arrRaw = _strRawCol.Split(';');
                for (int i = 0; i < arrRaw.Length; i++)
                {
                    if (string.IsNullOrEmpty(arrRaw[i].ToString())) continue;
                    if (lstRawCol.Contains(arrRaw[i].ToString())) 
                        continue;
                    else
                        lstRawCol.Add(arrRaw[i].ToString());
                }
            }
            return lstRawCol;
        }
                     
        public DataTable GetPpkMakeDataTable(DataTable dt)
        {            
            _lstRawColumn.Clear();
            _lstRawColumn = CallRefCol(dt);
            string strKey = Definition.CHART_COLUMN.MEAN;            
            foreach (DataRow dr in dt.Rows)
            {                              
                string[] arr = dr[strKey].ToString().Split('@');                        
                if (arr.Length > 1) {
                    string[] arrSub = arr[1].Split('^');                    
                    for (int k = 0; k < arrSub.Length; k++)
                    {
                        if (string.IsNullOrEmpty(arrSub[k])) continue;
                        string sColName = string.Format("{0}{1}", strKey, Definition.ChartDataList[k]);
                        if (dt.Columns.Contains(sColName))
                        {
                            dr[sColName] = arrSub[k];
                        }
                        else
                        {
                            dt.Columns.Add(sColName);
                            dr[sColName] = arrSub[k];
                        }
                    }
                }                  
            }
            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetMakeDataTable(DataTable _dtResourceData)
        {
            return CreateDataTable(_dtResourceData, CommonChart.PARAM_ITEM);
        }

        public DataTable GetMakeDataTablePopup(DataTable _dtResourceData)
        {
            return CreateDataTablePopup(_dtResourceData, CommonChart.PARAM_CALC_ITEM, CommonChart.PARAM_CALC_EXCLUED_ITEM);
        }

        public DataTable GetMakeDataTable(DataTable _dtResourceData, LinkedList _PARAM_ITEM)
        {
            return CreateDataTable(_dtResourceData, _PARAM_ITEM);
        }
        
        public DataTable GetPpkChkartMakeDataTable(DataTable _dtResourceData)
        {
            DataTable dt = _dtResourceData.Copy();
            return CreateDataTable(dt, CommonChart.PARAM_ITEM);    
        }                       

        private DataTable CreateDataTable(DataTable dt, LinkedList PARAM_ITEM)
        {
            

            string strKey = string.Empty;
            string sValue = string.Empty;
            string sColName = string.Empty;
            string[] arrSub = null;
            string[] arr = null;

            _lstRawColumn.Clear();
            _lstRawColumn = CallRefCol(dt);
            

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < PARAM_ITEM.Count; i++)
                {                                                        
                    strKey = PARAM_ITEM.GetKey(i).ToString();                    
                    if (dt.Columns.Contains(strKey))
                    {
                        sValue = dr[strKey].ToString();
                        if (sValue.IndexOf("@")>-1)
                        {                                                
                            arr = sValue.Split('@');
                            if (arr.Length > 0)
                                dr[strKey] = arr[0];

                            if (strKey == Definition.CHART_COLUMN.MIN || strKey == Definition.CHART_COLUMN.MAX) continue;
                                                            
                            if (arr.Length > 1)  
                            {
                                arrSub = arr[1].Split('^');
                                
                                for (int k = 0; k < arrSub.Length; k++)
                                {
                                    if (string.IsNullOrEmpty(arrSub[k]) || arrSub[k].Equals("NaN")) continue;
                                    sColName = string.Format("{0}{1}", strKey, Definition.ChartDataList[k]);
                                    if (dt.Columns.Contains(sColName))
                                    {
                                        dr[sColName] = arrSub[k];
                                    }
                                    else
                                    {
                                        dt.Columns.Add(sColName);
                                        dr[sColName] = arrSub[k];
                                    }
                                }
                            }                                
                        }
                        else
                        {
                            if(dt.Columns[strKey].DataType ==typeof(double))    
                            {                        
                                if(!string.IsNullOrEmpty(sValue))
                                dr[strKey] = double.Parse(sValue);                                                
                            }
                            //else
                             //dr[strKey] = sValue;                                                
                        }
                    }                  
                }
            }
            return dt;
        }

        private DataTable CreateDataTablePopup(DataTable dt, LinkedList PARAM_ITEM, LinkedList PARAM_EXCLUDE_ITEM)
        {


            string strKey = string.Empty;
            string sValue = string.Empty;
            string sColName = string.Empty;
            string[] arrSub = null;
            string[] arr = null;

            _lstRawColumn.Clear();
            _lstRawColumn = CallRefCol(dt);

            for (int i = 0; i < PARAM_EXCLUDE_ITEM.Count; i++)
            {
                strKey = PARAM_EXCLUDE_ITEM.GetKey(i).ToString();
                if (dt.Columns.Contains(strKey))
                {
                    dt.Columns.Remove(strKey);

                    for (int k = 0; k < Definition.ChartDataList.Length; k++)
                    {
                        sColName = string.Format("{0}{1}", strKey, Definition.ChartDataList[k]);
                        if (dt.Columns.Contains(sColName))
                        {
                            dt.Columns.Remove(sColName);
                        }
                    }
                }
            }


            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < PARAM_ITEM.Count; i++)
                {
                    strKey = PARAM_ITEM.GetKey(i).ToString();
                    if (dt.Columns.Contains(strKey))
                    {
                        sValue = dr[strKey].ToString();
                        if (sValue.IndexOf("@") > -1)
                        {
                            arr = sValue.Split('@');
                            if (arr.Length > 0)
                                dr[strKey] = arr[0];

                            if (strKey == Definition.CHART_COLUMN.MIN || strKey == Definition.CHART_COLUMN.MAX) continue;

                            if (arr.Length > 1)
                            {
                                arrSub = arr[1].Split('^');

                                for (int k = 0; k < arrSub.Length; k++)
                                {
                                    if (string.IsNullOrEmpty(arrSub[k]) || arrSub[k].Equals("NaN")) continue;
                                    sColName = string.Format("{0}{1}", strKey, Definition.ChartDataList[k]);
                                    if (dt.Columns.Contains(sColName))
                                    {
                                        dr[sColName] = arrSub[k];
                                    }
                                    else
                                    {
                                        dt.Columns.Add(sColName);
                                        dr[sColName] = arrSub[k];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dt.Columns[strKey].DataType == typeof(double))
                            {
                                if (!string.IsNullOrEmpty(sValue))
                                    dr[strKey] = double.Parse(sValue);
                            }
                            //else
                            //dr[strKey] = sValue;                                                
                        }
                    }
                }
            }
            return dt;
        }
    }
}
