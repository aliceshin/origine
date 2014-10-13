using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;


using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public class MakeDataSetDataTrxFileData
    {

        protected List<string> _lstRawColumn=new List<string>();
        protected DataTable _dtRawData;
        protected string _strRawCol = string.Empty;

        public List<string> lstRawColumn
        {
            get { return _lstRawColumn; }
            set { _lstRawColumn = value; }
        }

        public DataTable dtRawData
        {
            get { return _dtRawData; }
            set { _dtRawData = value; }
        }
        

        private List<string> GetRawCol()
        {
            List<string> lstRawCol = new List<string>();            
            for (int i = 0; i < dtRawData.Columns.Count; i++)
            {
                string strCol = dtRawData.Columns[i].ColumnName.ToString();
                if (strCol.IndexOf(";") > -1)
                {                  
                    _strRawCol = strCol;
                    break;
                }
            }

            if (string.IsNullOrEmpty(_strRawCol)) return lstRawCol;
            string[] arrRaw = _strRawCol.Split(';');
            for (int i = 0; i < arrRaw.Length; i++)
            {
                lstRawCol.Add(arrRaw[i].ToString());
            }

            return lstRawCol;
        }
        
        
        public DataTable GetMakeDataTable()
        {            
            DataTable dt = dtRawData.Copy();            
            _lstRawColumn.Clear();
            _lstRawColumn = GetRawCol();
            for (int i = 0; i < CommonChart.PARAM_ITEM.Count; i++)
            {
                string strKey = CommonChart.PARAM_ITEM.GetKey(i).ToString();
                string strType = CommonChart.PARAM_ITEM.GetValue(i).ToString();
               
                if (strType.Equals("1"))
                {                    
                    dt.Columns.Add(strKey + "_TARGET", typeof(String));
                    dt.Columns.Add(strKey + "_LSL", typeof(String));
                    dt.Columns.Add(strKey + "_LCL", typeof(String));
                    dt.Columns.Add(strKey + "_UCL", typeof(String));
                    dt.Columns.Add(strKey + "_USL", typeof(String));
                    dt.Columns.Add(strKey + "_CL", typeof(String));
                }
                else
                {
                    dt.Columns.Add(strKey, typeof(String));
                }
               
            }
            
            for(int i=0; i<_lstRawColumn.Count;i++)
            {
                dt.Columns.Add(_lstRawColumn[i].ToString(), typeof(String));
            }

            DataRow drNew = null;
            foreach (DataRow dr in dt.Rows)
            {

                for (int i = 0; i < CommonChart.PARAM_ITEM.Count; i++)
                {
                    string strKey = CommonChart.PARAM_ITEM.GetKey(i).ToString();
                    string strType = CommonChart.PARAM_ITEM.GetValue(i).ToString();                    
                    if (strType.Equals("1")) //split
                    {
                        //value format : value@target^lsl^lcl^ucl^usl^fault_rule_list
                        string[] arr = dr[strKey].ToString().Split('@');
                        if (arr.Length > 1)
                        {
                            string[] arrSub = arr[1].Split('^');
                            dr[strKey] = arr[0];
                            dr[strKey + "_TARGET"] = arrSub[0];
                            dr[strKey + "_LSL"] = arrSub[1];
                            dr[strKey + "_LCL"] = arrSub[2];
                            dr[strKey + "_UCL"] = arrSub[3];
                            dr[strKey + "_USL"] = arrSub[4];
                            //(ucl-lcl)/2+lcl
                            dr[strKey + "_CL"] = ((double.Parse(arrSub[3]) - double.Parse(arrSub[2])) / 2) + double.Parse(arrSub[2]);
                        }
                    }
                    else
                    {
                        dr[strKey] = dr[strKey];
                    }                   
                }

                for (int j = 0; j < lstRawColumn.Count; j++)
                {                    
                    string strCol = lstRawColumn[j].ToString();
                    string[] arrRaw = dr[_strRawCol].ToString().Split(';');
                    dr[strCol] = arrRaw[j];
                }                
            }
       
            return dt;
        }  
    }
}
