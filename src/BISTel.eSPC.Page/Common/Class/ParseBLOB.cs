using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
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

        
        public  DataSet getDataSetDATA_TRX_Data(DataSet dsData)
        {                
            DataSet dsRetrun = new DataSet();
            
            try
            {
                if (DSUtil.CheckRowCount(dsData))
                {
                    BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();
                    string module_id = string.Empty;
                    string module_alias = string.Empty;

                    foreach (DataRow drData in dsData.Tables[0].Rows)
                    {
                        using (StreamReader sr = gZip.DecompressForStream(drData["DATA_FILE"]))
                        {
                            this.ParseHeader(sr);
                            _llstLineInfo = this.ParseContextLineInfo(sr,arrLineInfo);
                            _llstParamName = this.ParseContextLineInfo(sr, arrLineInfo);
                                                                                   
                            //this.ParseContextLineInfo(sr);                            
                            
                            //switch (ParseType)
                            //{
                            //    default:
                            //        break;
                            //}
                        }                    
                    }
               }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsRetrun;      
        }


        private void ParseHeader(StreamReader sr)
        {
            string line = string.Empty;

                        
            // skip open tag of <header>
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();
                if (line.IndexOf(Definition.BLOB_FIELD_NAME.HEADER_CLOSING) > -1)
                {
                    break;
                }
            }

            // string of header
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();
                if (line.IndexOf(Definition.BLOB_FIELD_NAME.HEADER_OPENING) > -1)
                {
                    break;
                }
                if (line.IndexOf(Definition.BLOB_FIELD_NAME.LINE_INFO) > -1)
                {                  
                    arrLineInfo=ParseLine(line);
                }
                else if (line.IndexOf(Definition.BLOB_FIELD_NAME.PARAM_NAME) > -1)
                {
                    arrParamName = ParseLine(line);                            
                }                           
            }
        }

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="_llstInfo"></param>
        /// <param name="arrValue"></param>
        private LinkedList ParseContextLineInfo(StreamReader sr, string [] arrValue)
        {
            LinkedList _llstInfo = new LinkedList();
                      
            string line = string.Empty; 
            string []arrStr =null;     
            for (int i = 0; i < arrValue.Length && sr.Peek() > -1; i++)
            {
                line = sr.ReadLine();
                string strKey = arrValue[i].ToString();
                if (line.IndexOf(strKey) > -1)
                {
                    arrStr = line.Split('\t');
                    if (!_llstInfo.Contains(strKey))
                    {
                        _llstInfo.Add(strKey, arrStr);
                    }
                }
            }
            
            return _llstInfo;
                        
        }
    }
}
