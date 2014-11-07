using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace BISTel.eSPC.Common
{
    public class DataRestriction
    {
        
        private LinkedList _llIncludeFilter;
        private LinkedList _llExcludeFilter;
        private LinkedList llIncludePattern;
        private LinkedList llExcludePattern;

        public DataRestriction()
        {
            _llIncludeFilter = new LinkedList();
            _llExcludeFilter = new LinkedList();
            //_Data = new DataRestrictionData();
            llIncludePattern = new LinkedList();
            llExcludePattern = new LinkedList();
        }

        public string MakeQuery(LinkedList llPattern, string filterKey)
        {
            string sQuery = string.Empty;

            for (int i = 0; i < llPattern.Count; i++)
            {
                string key = llPattern.GetKey(i).ToString();
                //string[] lstFilter = llPattern[key] as string[];

                ArrayList arrAllFilter = new ArrayList();
                ArrayList arrSingleFilter = new ArrayList();

                if (filterKey == Definition.INCLUDE)
                {
                    if (llPattern[key] is List<string>)
                    {
                        List<string> lstFilter = llPattern[key] as List<string> ;
                        foreach (string str in lstFilter)
                        {
                            if (str.Contains("*"))
                            {
                                arrAllFilter.Add(str.Replace('*', '%').ToString());
                            }
                            else
                            {
                                arrSingleFilter.Add(str);
                            }
                        }
                    }
                    else
                    {
                        string[] lstFilter = llPattern[key] as string[];
                        foreach (string str in lstFilter)
                        {
                            if (str.Contains("*"))
                            {
                                arrAllFilter.Add(str.Replace('*', '%').ToString());
                            }
                            else
                            {
                                arrSingleFilter.Add(str);
                            }
                        }
                    }
                    

                    sQuery += "(";
                    for (int arrCount = 0; arrCount < arrAllFilter.Count; arrCount++)
                    {
                        sQuery += key + " LIKE '" + arrAllFilter[arrCount].ToString() + "' OR ";
                        
                        if (arrSingleFilter.Count == 0 && arrCount == arrAllFilter.Count-1)
                        {
                            sQuery = sQuery.Remove(sQuery.Length - 4, 4);
                            sQuery += ") AND ";
                        }
                    }

                    if (arrSingleFilter.Count > 0)
                    {
                        sQuery += key + " IN (";

                        for (int arrCount = 0; arrCount < arrSingleFilter.Count; arrCount++)
                        {
                            sQuery += "'" + arrSingleFilter[arrCount].ToString() + "', ";
                        }

                        sQuery = sQuery.Remove(sQuery.Length - 2, 2);
                        sQuery += ")) AND ";
                    }

                }
                else
                {
                    if (llPattern[key] is List<string>)
                    {
                        List<string> lstFilter = llPattern[key] as List<string>;
                        foreach (string str in lstFilter)
                        {
                            if (str.Contains("*"))
                            {
                                arrAllFilter.Add(str.Replace('*', '%').ToString());
                            }
                            else
                            {
                                arrSingleFilter.Add(str);
                            }
                        }
                    }
                    else
                    {
                        string[] lstFilter = llPattern[key] as string[];
                        foreach (string str in lstFilter)
                        {
                            if (str.Contains("*"))
                            {
                                arrAllFilter.Add(str.Replace('*', '%').ToString());
                            }
                            else
                            {
                                arrSingleFilter.Add(str);
                            }
                        }
                    }

                    sQuery += "(";
                    for (int arrCount = 0; arrCount < arrAllFilter.Count; arrCount++)
                    {
                        sQuery += key + " LIKE '" + arrAllFilter[arrCount].ToString() + "' OR ";

                        if (arrSingleFilter.Count == 0 && arrCount == arrAllFilter.Count - 1)
                        {
                            sQuery = sQuery.Remove(sQuery.Length - 4, 4);
                            sQuery += ") AND ";
                        }
                    }

                    if (arrSingleFilter.Count > 0)
                    {
                        sQuery += key + " IN (";

                        for (int arrCount = 0; arrCount < arrSingleFilter.Count; arrCount++)
                        {
                            sQuery += "'" + arrSingleFilter[arrCount].ToString() + "', ";
                        }

                        sQuery = sQuery.Remove(sQuery.Length - 2, 2);
                        sQuery += ")) AND ";
                    }
                }
                
            }

            if (filterKey == Definition.INCLUDE)
            {
                sQuery = sQuery.Remove(sQuery.Length - 5, 5);
                //_Data._filterData.Add(Definition.INCLUDE, sQuery);
            }
            else
            {
                sQuery = sQuery.Remove(sQuery.Length - 4, 4);
                //_Data._filterData.Add(Definition.EXCLUDE, sQuery);
            }

            return sQuery;
        }

         public string GetEQPFilterDataByRaw(DataSet dsFilter)
        {
            ArrayList arrRawid = new ArrayList();
            LinkedList llValue = new LinkedList();

            string strInclude = string.Empty;
            ArrayList arrContextKey = new ArrayList();
            ArrayList arrContextValue = new ArrayList();

            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr["RESTRICT_TYPE_CD"].ToString() == "EQ")
                {
                    foreach (string str in dr["DATA_FILTER"].ToString().Split('^'))
                    {
                        arrContextValue.Add(str);
                    }
                }
            }

            for (int i = 0; i < arrContextValue.Count; i++)
            {
                string[] context = arrContextValue[i].ToString().Split('=');
                string sKey = context[0];
                string[] sValues = context[1].Split(',');

                if (sKey.Equals(Definition.CONDITION_KEY_EQP_ID))
                {

                    strInclude += sKey + " IN (";

                    foreach (string sValue in sValues)
                    {
                        strInclude += "'" + sValue + "',";
                    }

                    strInclude = strInclude.Remove(strInclude.Length - 1, 1) + ") AND ";
                    
                }
            }

            if (strInclude.Length == 0)
                return string.Empty;

            strInclude = strInclude.Remove(strInclude.Length - 5, 5);

            return strInclude;
        }

        public DataTable GetEQPFilterData(DataSet dsFilter, DataSet dsContextType, DataTable dtOriginal)
        {
            DataTable dtResult = dtOriginal.Clone();
            LinkedList llValue = new LinkedList();

            ArrayList arrContextKey = new ArrayList();
            ArrayList arrContextValue = new ArrayList();

            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr["RESTRICT_TYPE_CD"].ToString() == "EQ")
                {
                    foreach (string str in dr["DATA_FILTER"].ToString().Split('^'))
                    {
                        arrContextValue.Add(str);
                    }
                }
            }

            if (arrContextValue.Count == 0)
            {
                return dtOriginal;
            }

            LinkedList llContextMapping = new LinkedList();
            foreach (DataRow row in dsContextType.Tables[0].Rows)
            {
                if (!llContextMapping.Contains(row[Definition.CONDITION_KEY_CODE]))
                {
                    llContextMapping.Add(row[Definition.CONDITION_KEY_CODE], row[Definition.CONDITION_KEY_NAME]);
                }
            }

            foreach (DataRow dr in dtOriginal.Rows)
            {
                bool isExist = false;
                for (int i = 0; i < arrContextValue.Count; i++)
                {
                    isExist = false;
                    
                    string[] context = arrContextValue[i].ToString().Split('=');
                    string sKey = context[0];

                    if (llContextMapping.Contains(sKey))
                    {
                        sKey = llContextMapping[sKey].ToString();
                    }
                    string value = dr[sKey].ToString().Split(';')[0];
                    string[] sValues = context[1].Split(',');

                    if (sKey.Equals(Definition.CONDITION_KEY_EQP_ID))
                    {
                        foreach (string sVal in sValues)
                        {
                            if (value == sVal)
                            {
                                dtResult.ImportRow(dr);
                            }
                        }
                    }
                }
            }
            return dtResult;
        }

        public DataTable GetIncludeFilterResultDataByRaw(DataSet dsFilter, DataSet dsContextType, DataTable dtOriginalData)
        {
            DataTable dtResult = dtOriginalData.Clone();
            string query = string.Empty;
            this._llExcludeFilter.Clear();
            this._llIncludeFilter.Clear();

            LinkedList llContextMapping = new LinkedList();

            foreach (DataRow row in dsContextType.Tables[0].Rows)
            {
                if (!llContextMapping.Contains(row[Definition.CONDITION_KEY_CODE]))
                {
                    llContextMapping.Add(row[Definition.CONDITION_KEY_CODE], row[Definition.CONDITION_KEY_NAME]);
                }
            }

            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr[Definition.SpreadHeaderColKey.EXCLUDE_YN].ToString() != "Y" && dr[Definition.CONDITION_KEY_RAWID].ToString() != "" && dr["RESTRICT_TYPE_CD"].ToString() != "EQ")
                {
                    string[] sFilter = dr["DATA_FILTER"].ToString().Split('^');
                    ArrayList arrFilter = new ArrayList();

                    _llIncludeFilter.Clear();
                    _llExcludeFilter.Clear();

                    for (int i = 0; i < sFilter.Length; i++)
                    {
                        string[] sContext = sFilter[i].Split('=');
                        string[] Values = sContext[1].Split(',');

                        if (llContextMapping.Contains(sContext[0]))
                        {
                            sContext[0] = llContextMapping[sContext[0]].ToString();
                        }

                        arrFilter.Add(sContext[0]);
                        _llIncludeFilter.Add(sContext[0], Values);
                    }

                    query = query + "(" + MakeQuery(_llIncludeFilter, Definition.INCLUDE);
                    query += ") OR ";
                }
            }

            if (!String.IsNullOrEmpty(query) && query.Length > 4)
            {
                query = query.Remove(query.Length - 4, 4);

                DataRow[] rowFilter = dtOriginalData.Select(query);

                foreach (DataRow dr in rowFilter)
                {
                    dtResult.ImportRow(dr);
                }
            }
            else
            {
                dtResult = dtOriginalData;
            }
            return dtResult;
        }

        public DataTable GetExcludeFilterResultDataByRaw(DataSet dsFilter, DataSet dsContextType, DataTable dtOriginalData)
        {
            DataTable dtResult = dtOriginalData.Clone();
            string query = string.Empty;
            this._llExcludeFilter.Clear();
            this._llIncludeFilter.Clear();

            LinkedList llContextMapping = new LinkedList();

            foreach (DataRow row in dsContextType.Tables[0].Rows)
            {
                if (!llContextMapping.Contains(row[Definition.CONDITION_KEY_CODE]))
                {
                    llContextMapping.Add(row[Definition.CONDITION_KEY_CODE], row[Definition.CONDITION_KEY_NAME]);
                }
            }

            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr[Definition.SpreadHeaderColKey.EXCLUDE_YN].ToString() == "Y" && dr[Definition.CONDITION_KEY_RAWID].ToString() != "" && dr["RESTRICT_TYPE_CD"].ToString() != "EQ")
                {
                    string[] sFilter = dr["DATA_FILTER"].ToString().Split('^');
                    ArrayList arrFilter = new ArrayList();

                    _llIncludeFilter.Clear();
                    _llExcludeFilter.Clear();

                    for (int i = 0; i < sFilter.Length; i++)
                    {
                        string[] sContext = sFilter[i].Split('=');
                        string[] Values = sContext[1].Split(',');

                        if (llContextMapping.Contains(sContext[0]))
                        {
                            sContext[0] = llContextMapping[sContext[0]].ToString();
                        }

                        arrFilter.Add(sContext[0]);
                        _llExcludeFilter.Add(sContext[0], Values);
                    }

                    query = query + "(" + MakeQuery(_llExcludeFilter, Definition.EXCLUDE);
                    query += ") OR ";
                }
            }

            if (!String.IsNullOrEmpty(query) && query.Length > 4)
            {
                query = query.Remove(query.Length - 4, 4);
                DataRow[] rowFilter = dtOriginalData.Select(query);

                for (int cntRow = dtOriginalData.Rows.Count - 1; cntRow >= 0; cntRow--)
                {
                    DataRow row = dtOriginalData.Rows[cntRow];
                    foreach (DataRow dr in rowFilter)
                    {
                        if (row == dr)
                        {
                            dtOriginalData.Rows.RemoveAt(cntRow);
                            break;
                        }
                    }
                }
            }

            dtResult = dtOriginalData;
            return dtResult;
        }

        public DataTable GetIncludeFilterResultData(DataSet dsFilter, DataSet dsContextType, DataTable dtOriginalData)
        {
            DataTable dtResult = new DataTable();
            string query = string.Empty;
            this._llIncludeFilter.Clear();

            LinkedList llContextMapping = new LinkedList();
            

            foreach (DataRow row in dsContextType.Tables[0].Rows)
            {
                if (!llContextMapping.Contains(row[Definition.CONDITION_KEY_CODE]))
                {
                    llContextMapping.Add(row[Definition.CONDITION_KEY_CODE], row[Definition.CONDITION_KEY_NAME]);
                }
            }

            int index = 0;
            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr[Definition.SpreadHeaderColKey.EXCLUDE_YN].ToString() != "Y" && dr[Definition.CONDITION_KEY_RAWID].ToString() != "" && dr["RESTRICT_TYPE_CD"].ToString() != "EQ")
                {
                    string[] sFilter = dr["DATA_FILTER"].ToString().Split('^');
                    LinkedList llFilterCondition = new LinkedList();

                    for (int i = 0; i < sFilter.Length; i++)
                    {
                        string[] sContext = sFilter[i].Split('=');
                        string[] Values = sContext[1].Split(',');

                        if (llContextMapping.Contains(sContext[0]))
                        {
                            sContext[0] = llContextMapping[sContext[0]].ToString();
                        }


                        if (sContext[0].Equals(Definition.CONDITION_KEY_EQP_ID) || sContext[0].Equals(Definition.CONDITION_KEY_MODULE_ID) ||
                        sContext[0].Equals(Definition.CONDITION_KEY_MODULE_NAME) || sContext[0].Equals(Definition.CONDITION_KEY_ALIAS))
                        {
                            //if(dtOriginalData.Columns.co
                            continue;
                        }

                        llFilterCondition.Add(sContext[0], sContext[1]);
                    }

                    if (llFilterCondition.Count > 0)
                    {
                        _llIncludeFilter.Add(index, llFilterCondition);
                        index++;
                    }
                }
            }

            LinkedList llExpression = this.GetFilterExpression(_llIncludeFilter);

            dtResult = this.FilteringIncludeData(dtOriginalData, llExpression);
            return dtResult;
        }

        public DataTable GetExcludeFilterResultData(DataSet dsFilter, DataSet dsContextType, DataTable dtOriginalData)
        {
            DataTable dtResult = new DataTable();
            string query = string.Empty;
            this._llExcludeFilter.Clear();

            LinkedList llContextMapping = new LinkedList();

            foreach (DataRow row in dsContextType.Tables[0].Rows)
            {
                if (!llContextMapping.Contains(row[Definition.CONDITION_KEY_CODE]))
                {
                    llContextMapping.Add(row[Definition.CONDITION_KEY_CODE], row[Definition.CONDITION_KEY_NAME]);
                }
            }

            int index = 0;
            foreach (DataRow dr in dsFilter.Tables[0].Rows)
            {
                if (dr[Definition.SpreadHeaderColKey.EXCLUDE_YN].ToString() == "Y" && dr[Definition.CONDITION_KEY_RAWID].ToString() != "" && dr["RESTRICT_TYPE_CD"].ToString() != "EQ")
                {
                    string[] sFilter = dr["DATA_FILTER"].ToString().Split('^');
                    LinkedList llFilterCondition = new LinkedList();

                    for (int i = 0; i < sFilter.Length; i++)
                    {
                        string[] sContext = sFilter[i].Split('=');
                        string[] Values = sContext[1].Split(',');

                        if (llContextMapping.Contains(sContext[0]))
                        {
                            sContext[0] = llContextMapping[sContext[0]].ToString();
                        }

                        llFilterCondition.Add(sContext[0], sContext[1]);
                    }
                    if (llFilterCondition.Count > 0)
                    {
                        _llExcludeFilter.Add(index, llFilterCondition);
                        index++;
                    }
                }
            }

            LinkedList llExpression = this.GetFilterExpression(_llExcludeFilter);

            dtResult = this.FilteringExcludeData(dtOriginalData, llExpression);
            return dtResult;
        }

        public DataTable FilteringIncludeData(DataTable dtOriginal, LinkedList llExpression)
        {
            DataTable dtResult = dtOriginal.Clone();

            SortedDictionary<string, DataTable> filteredTables = new SortedDictionary<string, DataTable>();
            List<string> lstValues = new List<string>();

            if (llExpression.Count == 0)
            {
                return dtOriginal;
            }

            foreach (DataRow dr in dtOriginal.Rows)
            {
                bool isExist = false;

                for (int cnt = 0; cnt < llExpression.Count; cnt++)
                {
                    LinkedList llTemp = llExpression[cnt] as LinkedList;
                    LinkedList llPattern = new LinkedList();

                    if (llTemp.Count > 0)
                    {
                        foreach (var key in llTemp.GetKeyList())
                        {
                            isExist = false;
                            ArrayList arrTemp = llTemp[key] as ArrayList;
                            string sValue = dr[key.ToString()].ToString().Split(';')[0];

                            foreach (Regex value in arrTemp)
                            {
                                //Regex rg = new Regex(value.ToString());

                                if (value.IsMatch(sValue))
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                break;
                            }
                        }
                        if (isExist)
                        {
                            dtResult.ImportRow(dr);
                        }
                    }

                }

            }

            return dtResult;
        }

        public DataTable FilteringExcludeData(DataTable dtOriginal, LinkedList llExpression)
        {
            DataTable dtResult = dtOriginal.Clone();

            SortedDictionary<string, DataTable> filteredTables = new SortedDictionary<string, DataTable>();
            List<string> lstValues = new List<string>();

            if (llExpression.Count == 0)
            {
                return dtOriginal;
            }

            foreach (DataRow dr in dtOriginal.Rows)
            {
                bool isExist = false;

                if (llExpression.Count == 0)
                {
                    return dtOriginal;
                }

                for (int cnt = 0; cnt < llExpression.Count; cnt++)
                {
                    LinkedList llTemp = llExpression[cnt] as LinkedList;
                    LinkedList llPattern = new LinkedList();

                    if (llTemp.Count > 0)
                    {
                        foreach (var key in llTemp.GetKeyList())
                        {
                            isExist = false;
                            ArrayList arrTemp = llTemp[key] as ArrayList;
                            string sValue = dr[key.ToString()].ToString().Split(';')[0];

                            foreach (Regex value in arrTemp)
                            {
                                //Regex rg = new Regex(value.ToString());

                                if (value.IsMatch(sValue))
                                {
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            dtResult.ImportRow(dr);
                        }
                    }

                }

            }

            return dtResult;
        }

        public LinkedList GetFilterExpression(LinkedList llFilter)
        {
            LinkedList llExpression = new LinkedList();
            string sExpression = string.Empty;

            for (int cnt = 0; cnt < llFilter.Count; cnt++)
            {
                LinkedList llTemp = llFilter[cnt] as LinkedList;
                LinkedList llPattern = new LinkedList();

                if (llTemp.Count > 0)
                {
                    foreach(var key in llTemp.GetKeyList())
                    {
                        string strValue = llTemp[key].ToString();
                        string[] strValues = strValue.Split(',');
                        ArrayList arrPattern = new ArrayList();

                        foreach (string sInclude in strValues)
                        {
                            string sPatten = string.Empty;

                            if (sInclude.Contains("*"))
                            {
                                foreach (char ch in sInclude.ToCharArray())
                                {
                                    if (ch == '*')
                                    {
                                        sPatten += @"\w*";
                                    }
                                    else
                                    {
                                        sPatten += ch;
                                    }
                                }
                            }
                            else
                            {
                                foreach (char ch in sInclude.ToCharArray())
                                {
                                    sPatten += ch;
                                }
                            }
                            arrPattern.Add(new Regex(sPatten));
                        }
                        
                        llPattern.Add(key, arrPattern);
                    }

                }
                llExpression.Add(cnt, llPattern);
            }
            return llExpression;
        }


    }
}
