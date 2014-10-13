using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

using System.Xml;
using System.IO;
using System.Net;
using System.Reflection;
using System.Drawing;

using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

using System.IO.Compression;

namespace BISTel.eSPC.Common
{
    public class CommonUtility
    {
        #region :Data Validation

        /// <summary>
        /// Null Value Check
        /// </summary>
        /// <param name="obj">Null Check 대상 Object</param>
        /// <returns>Object Value</returns>        
        public string NVL(object obj)
        {
            return NVL(obj, string.Empty);
        }

        /// <summary>
        /// Null Value Check
        /// </summary>
        /// <param name="obj">Null Check 대상 Object</param>
        /// <param name="nullValue">Null 값일 경우 대체할 값</param>
        /// <returns>Object Value</returns>
        public string NVL(object obj, string nullValue)
        {
            if (obj == null)
            {
                if (nullValue == null)
                    return string.Empty;
                else
                    return nullValue;
            }
            else
                return obj.ToString();
        }

        /// <summary>
        /// Null Value Check
        /// </summary>
        /// <param name="obj">Null Check 대상 Object</param>
        /// <param name="nullValue">Null 값일 경우 대체할 값</param>
        /// <param name="isEmptyCheck">Emptry도 Null로 Check할 것인지에 대한 여부</param>
        /// <returns>Object Value</returns>
        public string NVL(object obj, string nullValue, bool isEmptyCheck)
        {
            if (obj == null)
            {
                if (nullValue == null)
                    return string.Empty;
                else
                    return nullValue;
            }
            else if (isEmptyCheck && obj.ToString().Length.Equals(0))
            {
                if (nullValue == null)
                    return string.Empty;
                else
                    return nullValue;
            }
            else
                return obj.ToString();
        }

        //## Numeric Check Funtion
        Regex regDoubleNotExp = new Regex(@"^-?\d+(\.\d+)?$");
        Regex regDouble = new Regex(@"^-?\d+(\.\d+)?((e|E)?-?\d+)?$");
        Regex regInteger = new Regex(@"^-?\d+$");

        /// <summary>
        /// String 값이 Double 형태인지를 Check한다.
        /// </summary>
        /// <param name="numberValue">Check할 String Value. 기본적으로 지수도 Number로 인식.</param>
        /// <returns>Double 형태 여부</returns>             
        public bool IsDouble(string numberValue)
        {
            return this.IsDouble(numberValue, true);
        }

        /// <summary>
        /// String 값이 Double 형태인지를 Check한다.
        /// </summary>
        /// <param name="numberValue">Check할 String Value</param>
        /// <param name="checkExponent">지수형태도 Number로 Check할지 여부</param>
        /// <returns>Double 형태 여부</returns>          
        public bool IsDouble(string numberValue, bool checkExponent)
        {
            bool isNum = false;

            if (numberValue.Length > 0)
            {
                if (checkExponent)
                    isNum = regDouble.IsMatch(numberValue);
                else
                    isNum = regDoubleNotExp.IsMatch(numberValue);
            }

            return isNum;
        }

        /// <summary>
        /// String 값이 Integer 형태인지를 Check한다.
        /// </summary>
        /// <param name="numberValue">Check할 String Value</param>
        /// <returns>Integer 형태 여부</returns>
        public bool IsInteger(string numberValue)
        {
            bool isNum = false;

            if (numberValue.Length > 0)
                isNum = regInteger.IsMatch(numberValue);

            return isNum;
        }

        #endregion



        public void InformSystemManager()
        {
            InformSystemManager("");
        }

        public void InformSystemManager(string Msg)
        {
            MSGHandler.DisplayMessage(MSGType.Error, Msg);
        }


        #region :Image Util

        /// <summary>
        /// Configuration에 정의된 Button Node의 Name에 해당하는 Path를 이용하여 Image 개체를 생성한다.
        /// </summary>
        /// <param name="name">Button Node Name</param>
        /// <returns>Image</returns>
        public Image GetImage(string name)
        {
            return this.GetImage(name, Configuration.getInstance());
        }

        /// <summary>
        /// Configuration에 정의된 Button Node의 Name에 해당하는 Path를 이용하여 Image 개체를 생성한다.
        /// </summary>
        /// <param name="name">Button Node Name</param>
        /// <param name="config">Configuration Instance</param>
        /// <returns>Image</returns>
        public Image GetImage(string name, Configuration config)
        {
            Image img = null;

            string configPath = config.Path;
            WebRequest req = WebRequest.Create(configPath);
            WebResponse rep = req.GetResponse();
            Encoding encode = Encoding.GetEncoding("UTF-8");

            XmlDocument m_doc = new XmlDocument();
            m_doc.Load(new StreamReader(rep.GetResponseStream(), encode));
            
            string xPath = "/configuration/buttonlist/button[@name='" + name + "']";
            XmlNodeList nodeList = m_doc.SelectNodes(xPath);

            try
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    string sNameValue = node.Attributes.GetNamedItem("name").Value;

                    if (sNameValue == name)
                    {
                        if (node.Attributes.GetNamedItem("path") != null)
                        {
                            string url = config.URL + node.Attributes.GetNamedItem("path").Value;

                            WebRequest reqimage = WebRequest.Create(url);
                            WebResponse repimage = reqimage.GetResponse();
                            img = Image.FromStream(repimage.GetResponseStream());
                        }
                        break;
                    }
                }
            }
            catch
            {
            }

            return img;
        }

        #endregion



        #region :BComboBox Databinding Util

        /// <summary>
        /// Set BComboBox Data
        /// </summary>
        /// <param name="bcbo">BComboBox</param>
        /// <param name="ds">DataSet</param>
        /// <param name="displaymember">Display Member</param>
        /// <param name="valuemember">Value Member</param>
        /// <param name="selectvalue">Default Select Value</param>
        /// <param name="isDefaultSelect">if default select value is not exist, it will select first Item</param>
        public void SetBComboBoxData(BComboBox bcbo, DataSet ds, string displaymember, string valuemember, string selectvalue, bool isDefaultSelect)
        {
            SetBComboBoxData(bcbo, ds, displaymember, valuemember, "", "", selectvalue, "", isDefaultSelect);
        }

        /// <summary>
        /// Set BComboBox Data
        /// </summary>
        /// <param name="bcbo">BComboBox</param>
        /// <param name="ds">DataSet</param>
        /// <param name="displaymember">Display Member</param>
        /// <param name="valuemember">Value Member</param>
        /// <param name="selectvalue">Default Select Value</param>
        /// <param name="InsertCustomText">Add Custom Text (ex> ALL, SELECT)</param>
        /// <param name="isDefaultSelect">if default select value is not exist, it will select first Item</param>
        public void SetBComboBoxData(BComboBox bcbo, DataSet ds, string displaymember, string valuemember, string selectvalue, string InsertCustomText, bool isDefaultSelect)
        {
            SetBComboBoxData(bcbo, ds, displaymember, valuemember, "", "", selectvalue, InsertCustomText, isDefaultSelect);
        }

        /// <summary>
        /// Set BComboBox Data
        /// </summary>
        /// <param name="bcbo">BComboBox</param>
        /// <param name="ds">DataSet</param>
        /// <param name="displaymember">Display Member</param>
        /// <param name="valuemember">Value Member</param>
        /// <param name="filter">Filtering By DataTable Select Fuction</param>
        /// <param name="sort">Sort By DataTable Select Function</param>
        /// <param name="selectvalue">Default Select Value</param>
        /// <param name="isDefaultSelect">if default select value is not exist, it will select first Item</param>
        public void SetBComboBoxData(BComboBox bcbo, DataSet ds, string displaymember, string valuemember, string filter, string sort, string selectvalue, bool isDefaultSelect)
        {
            SetBComboBoxData(bcbo, ds, displaymember, valuemember, filter, sort, selectvalue, "", isDefaultSelect);
        }

        /// <summary>
        /// Set BComboBox Data
        /// </summary>
        /// <param name="bcbo">BComboBox</param>
        /// <param name="ds">DataSet</param>
        /// <param name="displaymember">Display Member</param>
        /// <param name="valuemember">Value Member</param>
        /// <param name="filter">Filtering by DataTable Select Function</param>
        /// <param name="sort">Sort by DataTable Select Function </param>
        /// <param name="selectvalue">Default Select Value</param>
        /// <param name="InsertCustomText">Add Custom Text (ex> ALL, SELECT)</param>
        /// <param name="isDefaultSelect">if default select value is not exist, it will select first Item</param>
        public void SetBComboBoxData(BComboBox bcbo, DataSet ds, string displaymember, string valuemember, string filter, string sort, string selectvalue, string InsertCustomText, bool isDefaultSelect)
        {
            DataSet dsSource = null;

            bcbo.DataSource = null;

            if (ds.Tables.Count > 0)
            {
                if (displaymember.Length > 0)
                    bcbo.DisplayMember = displaymember;
                if (valuemember.Length > 0)
                    bcbo.ValueMember = valuemember;

                if (filter.Length > 0)
                {
                    dsSource = ds.Clone();

                    if (InsertCustomText.Length > 0)
                    {
                        DataRow drCTxt = dsSource.Tables[0].NewRow();
                        drCTxt[displaymember] = InsertCustomText;
                        drCTxt[valuemember] = InsertCustomText;

                        dsSource.Tables[0].Rows.Add(drCTxt);
                    }

                    DataRow[] drFilters = null;
                    if (sort.Length > 0)
                        drFilters = ds.Tables[0].Select(filter, sort);
                    else
                        drFilters = ds.Tables[0].Select(filter, sort);

                    foreach (DataRow drFilter in drFilters)
                        dsSource.Tables[0].ImportRow(drFilter);
                }
                else
                {
                    if (InsertCustomText.Length > 0)
                    {
                        dsSource = ds.Copy();

                        DataRow drCTxt = dsSource.Tables[0].NewRow();
                        drCTxt[displaymember] = InsertCustomText;
                        drCTxt[valuemember] = InsertCustomText;
                        dsSource.Tables[0].Rows.InsertAt(drCTxt, 0);
                    }
                    else
                        dsSource = ds;
                }

                bcbo.DataSource = dsSource.Tables[0];

                if (dsSource.Tables[0].Rows.Count > 0)
                {
                    if (selectvalue.Length > 0)
                    {
                        bcbo.SelectedValue = selectvalue;
                    }
                    else if (isDefaultSelect)
                    {
                        bcbo.SelectedValue = dsSource.Tables[0].Rows[0][valuemember].ToString();
                    }
                    else
                    {
                        bcbo.SelectedIndex = -1;
                    }
                }
            }
        }


        /// <summary>
        /// Set BComboBox Data
        /// </summary>
        /// <param name="bcbo">BComboBox</param>
        /// <param name="llKeyValueData">LinkedList KEY&VALUE DATA</param>
        /// <param name="selectvalue">Default Select Value</param>
        /// <param name="isDefaultSelect">if default select value is not exist, it will select first Item</param>
        public void SetBComboBoxData(BComboBox bcbo, LinkedList llKeyValueData, string selectvalue, bool isDefaultSelect)
        {
            if (llKeyValueData != null)
            {
                DataSet dsData = new DataSet();
                DataTable dtData = new DataTable();
                dtData.TableName = "COMBODATA";
                dtData.Columns.Add("KEY");
                dtData.Columns.Add("VALUE");

                DataRow drNew = null;

                for (int i = 0; i < llKeyValueData.Count; i++)
                {
                    drNew = dtData.NewRow();

                    drNew["KEY"] = llKeyValueData.GetKey(i).ToString();
                    drNew["VALUE"] = llKeyValueData.GetValue(i).ToString();

                    dtData.Rows.Add(drNew);
                }

                dsData.Tables.Add(dtData);

                SetBComboBoxData(bcbo, dsData, "VALUE", "KEY", selectvalue, isDefaultSelect);
            }
        }

        #endregion



        #region :Data Util

        /// <summary>
        /// DataTable의 특정 Column의 최소 값을 구한다
        /// </summary>
        /// <param name="dtData">DataTable</param>
        /// <param name="columnName">Check 대상 Column</param>
        /// <returns>min value</returns>
        public double MinValue(DataTable dataTable, string columnName)
        {
            double dMinValue = Double.MaxValue;

            string sTemp = string.Empty;

            foreach (DataRow drData in dataTable.Rows)
            {
                sTemp = drData[columnName].ToString();

                if (IsDouble(sTemp))
                {
                    if (dMinValue > double.Parse(sTemp))
                    {
                        dMinValue = double.Parse(sTemp);
                    }
                }
            }

            return dMinValue;
        }


        /// <summary>
        /// DataTable의 특정 Column의 최대 값을 구한다
        /// </summary>
        /// <param name="dtData">DataTable</param>
        /// <param name="columnName">Check 대상 Column</param>
        /// <returns>max value</returns>
        public double MaxValue(DataTable dataTable, string columnName)
        {
            double dMaxValue = Double.MinValue;

            string sTemp = string.Empty;

            foreach (DataRow drData in dataTable.Rows)
            {
                sTemp = drData[columnName].ToString();

                if (IsDouble(sTemp))
                {
                    if (dMaxValue < double.Parse(sTemp))
                    {
                        dMaxValue = double.Parse(sTemp);
                    }
                }
            }

            return dMaxValue;
        }



        public double[] ConvertDataColumnIntoDoubleArray(DataTable dataTable, string columnName)
        {
            if (dataTable == null) return null;

            double[] valueList = new double[dataTable.Rows.Count];

            string value = string.Empty;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                value = dataTable.Rows[i][columnName].ToString();

                if (this.IsDouble(value))
                    valueList[i] = double.Parse(value);
                else
                    valueList[i] = 0;
            }

            return valueList;
        }


        /// <summary>
        /// DataTable의 특정 Column을 String 배열로 반환한다.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="columnName">Column Name</param>
        /// <returns>string 배열</returns>
        public string[] ConvertDataColumnIntoArray(DataTable dataTable, string columnName)
        {
            if (dataTable == null) return null;

            string[] strList = new string[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                strList[i] = dataTable.Rows[i][columnName].ToString();
            }

            return strList;
        }

        /// <summary>
        /// DataTable의 특정 Column을 String 배열로 반환한다.
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="columnName">Column Name</param>
        /// <param name="isDistinct">Column의 값을 Distinct할지 여부를 지정</param>
        /// <returns>string 배열</returns>
        public string[] ConvertDataColumnIntoArray(DataTable dataTable, string columnName, bool isDistinct)
        {
            if (dataTable == null) return null;

            string[] strList = null;

            if (isDistinct)
            {
                List<string> list = new List<string>();

                foreach (DataRow dr in dataTable.Rows)
                {
                    if (!list.Contains(dr[columnName].ToString()))
                        list.Add(dr[columnName].ToString());
                }

                list.Sort();

                strList = list.ToArray();
            }
            else
            {
                strList = this.ConvertDataColumnIntoArray(dataTable, columnName);
            }

            return strList;
        }


        /// <summary>
        /// DataTable의 특정 Column의 Data를 [,] 구분된 string 값을 반환
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string ConvertDataColumnIntoStringList(DataTable dataTable, string columnName)
        {
            return ConvertDataColumnIntoStringList(dataTable, columnName, ",");
        }

        /// <summary>
        /// DataTable의 특정 Column의 Data를 지정한 Separator로 구분된 string 값을 반환
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ConvertDataColumnIntoStringList(DataTable dataTable, string columnName, string separator)
        {
            return ConvertDataColumnIntoStringList(dataTable, columnName, "", separator);
        }

        /// <summary>
        /// DataTable의 특정 Column의 Data를 Wapper로 감싸고 지정한 Separator로 구분된 string 값을 반환
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnName"></param>
        /// <param name="wrapper"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ConvertDataColumnIntoStringList(DataTable dataTable, string columnName, string stringWrapper, string separator)
        {
            if (dataTable == null) return string.Empty;

            StringBuilder strList = new StringBuilder();

            int rowCount = dataTable.Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {
                if (i > 0) strList.Append(separator);

                strList.AppendFormat("{0}{1}{0}", stringWrapper, dataTable.Rows[i][columnName].ToString());
            }

            return strList.ToString();
        }


        /// <summary>
        /// DataTable을 filter expression으로 filter 후 특정 Column의 Data를 Wapper로 감싸고 지정한 Separator로 구분된 string 값을 반환
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="filterExpression"></param>
        /// <param name="columnName"></param>
        /// <param name="stringWrapper"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ConvertDataColumnIntoStringList(DataTable dataTable, string filterExpression, string columnName, string stringWrapper, string separator)
        {
            if (dataTable == null) return string.Empty;

            StringBuilder strList = new StringBuilder();

            if (filterExpression != null && filterExpression.Length > 0)
            {
                DataRow[] drs = dataTable.Select(filterExpression);

                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (i > 0) strList.Append(separator);

                        strList.AppendFormat("{0}{1}{0}", stringWrapper, drs[i][columnName].ToString());
                    }
                }
            }
            else
            {
                strList.Append(this.ConvertDataColumnIntoStringList(dataTable, columnName, stringWrapper, separator));

            }

            return strList.ToString();
        }

        /// <summary>
        /// Array Data를 [,] 구분된 string 값을 반환
        /// </summary>
        /// <param name="array">IEnumerable을 상속받은 배열 또는 ArrayList</param>
        /// <returns>A,B,C,D</returns>
        public string ConvertArrayIntoStringList(IEnumerable array)
        {
            return ConvertArrayIntoStringList(array, ",");
        }

        /// <summary>
        /// Array Data를 지정한 Separator로 구분된 string 값을 반환
        /// </summary>
        /// <param name="array">IEnumerable을 상속받은 배열 또는 ArrayList</param>
        /// <param name="separator">구분자</param>
        /// <returns>string</returns>
        public string ConvertArrayIntoStringList(IEnumerable array, string separator)
        {
            if (array == null) return string.Empty;

            StringBuilder strList = new StringBuilder();

            foreach (object item in array)
            {
                strList.Append(item.ToString());
                strList.Append(separator);
            }

            if (strList.ToString().Length > 0) strList.Remove(strList.Length - 1, 1);

            return strList.ToString();
        }


        /// <summary>
        /// Array Data를 Wrapper로 감싸고 지정한 Separator로 구분된 string 값을 반환
        /// </summary>
        /// <param name="array">IEnumerable을 상속받은 배열 또는 ArrayList</param>
        /// <param name="stringWrapper"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ConvertArrayIntoStringList(IEnumerable array, string stringWrapper, string separator)
        {
            if (array == null) return string.Empty;

            StringBuilder strList = new StringBuilder();

            foreach (object item in array)
            {
                strList.AppendFormat("{0}{1}{0}{2}", stringWrapper, item.ToString(), separator);
            }

            if (strList.ToString().Length > 0) strList.Remove(strList.Length - 1, 1);

            return strList.ToString();
        }

        

        /// <summary>
        /// DataSet의 지정한 Column을 SortType(asc, desc)에 따라 정렬
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortType"></param>
        public void SortDataSet(DataSet dataSet, string sortColumn, SortType sortType)
        {
            DataSet dsTemp = new DataSet();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (!dataTable.Columns.Contains(sortColumn))
                    continue;

                dsTemp.Merge(dataTable.Select("", string.Format("{0} {1}", sortColumn, sortType)));

                dataTable.Clear();
                dataTable.Dispose();
            }

            dataSet.Merge(dsTemp);

            dsTemp.Clear();
            dsTemp.Dispose();
            dsTemp = null;
        }


        public bool DuplicateCheck(DataTable dataTable, params string[] keyFields)
        {
            bool bRetVal = false;

            DataColumn[] Cols = new DataColumn[keyFields.Length];
            int i = 0;

            foreach (string keyField in keyFields)
            {
                Cols[i] = dataTable.Columns[keyField];
                i++;
            }

            try
            {
                //Unique 설정
                dataTable.Constraints.Add("BISTELUNIQUE", Cols, false); 
            }
            catch
            {
                bRetVal = true;
            }
            finally
            {
                if (dataTable.Constraints.Contains("BISTELUNIQUE"))
                    dataTable.Constraints.Remove("BISTELUNIQUE");
            }

            return bRetVal;
        }


        public ArrayList ConvertStringToArray(string items)
        {
            ArrayList valueList = new ArrayList();

            string value = items;
            int start = 0;
            int found = 0;
            do
            {
                found = value.IndexOf(",", start);
                if (found > 0)
                    valueList.Add(value.Substring(start, found - start));
                else
                    valueList.Add(value.Substring(start));
                start = found + 1;
            }
            while (found > 0);


            return valueList;
        }

        #endregion



        /// <summary>
        /// Splits up data by ','. This distinguisher is used for search condition.
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string[] SplitConditionData(string strData)
        {
            string[] saData = strData.Split(',');

            ArrayList alData = new ArrayList();
            for (int i = 0; i < saData.Length; i++)
            {
                alData.Add(saData[i].Trim().ToString());
            }

            string[] strData_2 = (string[])alData.ToArray(typeof(string));
            return strData_2;
        }

        public string GetConditionData(DataTable dt, string sDataKey)
        {
            string sData = "";

            try
            {
                sData = dt.Rows[0][sDataKey].ToString();
            }
            catch
            {
                return sData;
            }

            return sData;
        }

        public ArrayList GetConditionDatas(DataTable dt, string sDataKey)
        {
            ArrayList alData = new ArrayList();

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sData = dt.Rows[i][sDataKey].ToString();
                    alData.Add(sData);
                }
            }
            catch
            {
                return alData;
            }

            return alData;
        }


        public string GetConditionString(DataTable dt, string sDataKey)
        {
            string strParam=string.Empty;

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sData = dt.Rows[i][sDataKey].ToString();
                    if (i > 0) strParam += ",";
                    strParam += "'" + sData + "'";                  
                }
            }
            catch (Exception ex)
            {
                return strParam;
            }

            return strParam;
        }


        public string GetConditionString(LinkedList _llstParamt, string sDataKey, string sTablename)
        {
            string sData = "";
            try
            {
                if (_llstParamt[sTablename] == null) return sData;
                DataTable dt = (DataTable)_llstParamt[sTablename];
                sData = dt.Rows[0][sDataKey].ToString();
            }
            catch (Exception ex)
            {
                return sData;
            }

            return sData;
        }



        public ArrayList GetConditionKeyArrayList(DataTable dt, string _conditionKey)
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



        public bool SetBCheckCombo(BCheckCombo BCombo, DataSet ds, string displaymember, string valuemember, ArrayList arr)
        {
            string list = string.Empty;

            BCombo.DataSource = ds.Tables[0];
            BCombo.DisplayMember = displaymember;
            BCombo.ValueMember = valuemember;
                        
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (arr.Contains(ds.Tables[0].Rows[i][valuemember].ToString()))
                    {
                        list += ds.Tables[0].Rows[i][displaymember].ToString() + ",";
                        BCombo.isSearchComboCheckBox = true;
                    }
                }

                if(!string.IsNullOrEmpty(list))
                {
                    list = list.Substring(0, list.Length - 1);
                    BCombo.Text = list;
                }
            }
            return true;
        }

        public bool SetBCheckCombo2(BCheckCombo BCombo, DataTable dt, string displaymember,string valuemember, ArrayList arr)
        {
            string list = string.Empty;

            BCombo.DataSource = dt;
            BCombo.DisplayMember = displaymember;
            BCombo.ValueMember = valuemember;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (arr.Contains(dt.Rows[i][displaymember].ToString()))
                    {
                        list += dt.Rows[i][displaymember].ToString() + ",";
                        BCombo.isSearchComboCheckBox = true;
                    }
                }

                if (!string.IsNullOrEmpty(list))
                {
                    list = list.Substring(0, list.Length - 1);
                    BCombo.Text = list;
                }
            }
            return true;
        }



        public void SetBCheckCombo(ArrayList arr, DataTable dt, BCheckCombo bchkCbo)
        {

            string sSelectedItem = bchkCbo.chkBox.SelectedItem.ToString();            
            string sSelectedValue = bchkCbo.Text;
            string[] arrSelected = sSelectedValue.Split(',');

            arr.Clear();
            if (sSelectedItem != Definition.VARIABLE.STAR)
            {
                for (int i = 0; i < arrSelected.Length; i++)
                {
                    if (arrSelected[i] != Definition.VARIABLE.STAR)
                    {
                        arr.Add(arrSelected[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < arrSelected.Length; i++)
                {
                    if (arrSelected[i] == Definition.VARIABLE.STAR)
                    {
                        arr.Clear();
                        arr.Add(Definition.VARIABLE.STAR);
                        SetBCheckCombo2(bchkCbo, dt, bchkCbo.DisplayMember, bchkCbo.ValueMember, arr);
                        break;
                    }
                    else
                    {
                        arr.Add(arrSelected[i]);
                    }
                }
            }
            SetBCheckCombo2(bchkCbo, dt, bchkCbo.DisplayMember, bchkCbo.ValueMember, arr);

        }


        public string GetArrayListString(ArrayList arlst)
        {
            if (arlst == null) return null;

            string str = string.Empty;
            try
            {
                for (int i = 0; i < arlst.Count; i++)
                {
                    string sData = arlst[i].ToString();
                    if (i > 0) str += ",";
                    str += sData;
                }
                
            }
            catch (Exception ex)
            {
                return str;
            }

            return str;
        }


        public string GetSelectedString(BCheckCombo _bchkCombo)
        {
            string str = string.Empty;
            if (_bchkCombo == null) return str;
            DataTable dt = (DataTable)_bchkCombo.DataSource;            
            for (int i = 0; i < _bchkCombo.chkBox.CheckedItems.Count; i++)
            {
                string sValue = _bchkCombo.chkBox.CheckedItems[i].ToString();
                if(sValue==Definition.VARIABLE.STAR)
                {
                    str += sValue + ",";
                }else
                {
                    string [] arr = sValue.Split(' ');
                    DataRow[] dtSelect = dt.Select(string.Format(" {0}='{1}'", _bchkCombo.ValueMember, arr[0]));

                    foreach (DataRow nRow in dtSelect)
                    {                                        
                        str += nRow[_bchkCombo.ValueMember].ToString() +",";
                    }
                }

            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
                str = "'" + str.Replace(",", "','") + "'";
            }

            return str;
        }

        /// <summary>
        /// It returns Cell Range of Clipboard's data
        /// </summary>
        /// <returns>first array value is row count, second is column count</returns>
        private int[] GetCellRangeOfClipboard()
        {
            IDataObject iData = Clipboard.GetDataObject();
            string str = (String)iData.GetData(DataFormats.Text);

            if (string.IsNullOrEmpty(str))
                return null;

            int[] result = new int[2];

            string[] rows = Regex.Split(str, "\r\n");
            result[0] = rows.Length;

            string[] columns = Regex.Split(rows[0], "\t");
            result[1] = columns.Length;

            return result;
        }

        /// <summary>
        /// It returns cell values which can be used by Rollback function after pasting clipboard.
        /// </summary>
        /// <param name="bspread">bspread which will be back up</param>
        /// <param name="rowIndex">start row index</param>
        /// <param name="columnIndex">start column index</param>
        /// <returns>back up object</returns>
        public object[,] GetBackUpDataForPasting(BSpread bspread, int rowIndex, int columnIndex)
        {
            int[] range = GetCellRangeOfClipboard();
            return bspread.ActiveSheet.GetArray(rowIndex, columnIndex, range[0],
                                                range[1]);
        }

        /// <summary>
        /// It restore cell values of spread using by backup data
        /// </summary>
        /// <param name="bspread">bspread which will be restored</param>
        /// <param name="rowIndex">start row index</param>
        /// <param name="columnIndex">start column index</param>
        /// <param name="backupData">back up object made by GetBackUpDataForPasting function</param>
        public void Rollback(BSpread bspread, int rowIndex, int columnIndex, object[,] backupData)
        {
            bspread.ActiveSheet.SetArray(rowIndex, columnIndex, backupData);
        }

        /// <summary>
        /// It check if the page is to needed to process parameters of the requsted URL.
        /// Example of a url needed to process : http://192.168.1.94/ees/EES.application?func=SPCChartViewUC&dllname=eSPC/BISTel.eSPC.Page.dll&classname=BISTel.eSPC.Page.Report.SPCChartViewUC
        /// &queryxml=%3Cees_querystring%3E%3Cpage_info%3E%3Cdll%3EeSPC%2FBISTel.eSPC.Page.dll%3C%2Fdll%3E%3Cclass%3EBISTel.eSPC.Page.Report.SPCChartViewUC%3C%2Fclass%3E%3Cpage%3ESPCChartViewUC
        /// %3C%2Fpage%3E%3CpageTitle%3ESPCChartData%3C%2FpageTitle%3E%3Cpage_type%3Elink%3C%2Fpage_type%3E%3C%2Fpage_info%3E%3Crequest%3E%3CMODEL_CONFIG_RAWID%3E%3Cvalue%3E600104%3C%2Fvalue
        /// %3E%3C%2FMODEL_CONFIG_RAWID%3E%3CCHART_TYPE%3E%3Cvalue%3ERAW%3C%2Fvalue%3E%3C%2FCHART_TYPE%3E%3COCAP_RAWID%3E%3Cvalue%3E100037977%3C%2Fvalue%3E%3C%2FOCAP_RAWID%3E%3COCAP_DTTS
        /// %3E%3Cvalue%3E2010-06-09+10%3A55%3A46.508%3C%2Fvalue%3E%3C%2FOCAP_DTTS%3E%3C%2Frequest%3E%3C%2Fees_querystring%3E
        /// </summary>
        /// <param name="functionName">Function name of page</param>
        /// <returns>Wheather it is needed to process</returns>
        public bool CheckIfNeedToProcessURLQuery(string functionName)
        {
            if (string.IsNullOrEmpty(EESUtil.GetHttpQueryString()))
                return false;

            NameValueCollection nvcQueryString = EESUtil.ParsingHttpQueryString(EESUtil.GetHttpQueryString());
            if (String.IsNullOrEmpty(nvcQueryString["func"])
                || String.IsNullOrEmpty(nvcQueryString["queryxml"]))
                return false;

            if (functionName != nvcQueryString["func"])
                return false;

            return true;
        }

        /// <summary>
        /// It makes XmlDocument from 'queryxml' parameter of the requested URL
        /// </summary>
        /// <returns>If it is success, return XmlDocument
        /// If it is not needed to proceess, return null
        /// If not matched functionName or if parameters of the requested url is invalid, return null</returns>
        public XmlDocument GetXmlDocFromQueryXmlOfRequestedUrl(string functionName)
        {
            if (!this.CheckIfNeedToProcessURLQuery(functionName))
                return null;

            NameValueCollection nvcQueryString = EESUtil.ParsingHttpQueryString(EESUtil.GetHttpQueryString());
            XmlDocument xmldocQueryXml = new XmlDocument();
            try
            {
                xmldocQueryXml.LoadXml(nvcQueryString["queryxml"]);
            }
            catch
            {
                return null;
            }

            return xmldocQueryXml;
        }

        public static StreamReader ConvertBLOBToStreamReader(object objData)
        {
            StreamReader srReturn = null;

            byte[] buffers = (byte[])objData;

            using (MemoryStream msSource = new MemoryStream(buffers))
            {
                MemoryStream msDestination = new MemoryStream();

                Decompress(msSource, msDestination);


                srReturn = new StreamReader(msDestination, Encoding.Default);
            }

            return srReturn;
        }

        public static void Decompress(Stream source, Stream destination)
        {
            using (GZipStream input = new GZipStream(source, CompressionMode.Decompress))
            {
                Pump(input, destination);
            }
        }

        public static void Pump(Stream input, Stream output)
        {
            byte[] buffers = new byte[8192];
            int n;

            while ((n = input.Read(buffers, 0, buffers.Length)) != 0)
            {
                output.Write(buffers, 0, n);
            }

            output.Position = 0;
        }
    }
}
