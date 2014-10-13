using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Common
{
    public class CreateChartDataTable : BISTel.eSPC.Page.Common.CreateChartDataTable
    {
        new public DataTable GetMakeDataTable(DataTable _dtResourceData)
        {
            return CreateDataTable(_dtResourceData, CommonChart.PARAM_ITEM);
        }

        new public DataTable GetMakeDataTable(DataTable _dtResourceData, LinkedList _PARAM_ITEM)
        {
            return CreateDataTable(_dtResourceData, _PARAM_ITEM);
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
    }
}
