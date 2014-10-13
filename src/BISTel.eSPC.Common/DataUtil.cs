using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Common
{
	/// <summary>
	/// APCDataUtil에 대한 요약 설명입니다.
	/// </summary>
	public class DataUtil
	{

        #region :Field
        #endregion


		public DataUtil()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.            
		}

		static public DataSet GetMultiColumn(DataSet ds,string groupField,string multiField)
		{
			DataSet tempDs = new DataSet();
			tempDs = ds.Clone();

			string sPreGroup = "";
			string sMultiField = "";
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				string sGroup = ds.Tables[0].Rows[i][groupField].ToString();
				string sMulti = ds.Tables[0].Rows[i][multiField].ToString();
				
				if(sPreGroup != sGroup && sPreGroup.Length!=0)
				{
					sMultiField = sMultiField.Substring(1);
					DataRow dataRow = ds.Tables[0].Rows[i-1];
					dataRow[multiField] = sMultiField;
					object[] sRows = dataRow.ItemArray;
					tempDs.Tables[0].Rows.Add(sRows);

					sMultiField = "";

					if(i==ds.Tables[0].Rows.Count-1)
					{
						DataRow dataRow1 = ds.Tables[0].Rows[i]; 
						object[] sRows1 = dataRow1.ItemArray;
						tempDs.Tables[0].Rows.Add(sRows1);
					}

				}
				else if(i==ds.Tables[0].Rows.Count-1)
				{
					sMultiField += "," + sMulti;
					sMultiField = sMultiField.Substring(1);
					DataRow dataRow = ds.Tables[0].Rows[i];
					dataRow[multiField] = sMultiField;
					object[] sRows = dataRow.ItemArray;
					tempDs.Tables[0].Rows.Add(sRows);
				}
				sMultiField += "," + sMulti;
				sPreGroup = sGroup;
			}

			return tempDs;
		}


		/// <summary>
		/// TODO : KATHRYN [2007-04-20] 
		/// Return of split string array 
		/// </summary>
		/// <param name="p_message">>Original Message String including delimiters</param>
		/// <param name="p_delimeter">Delimieter char set e.g., " ,.:"</param>
		/// <returns></returns>		
		public string[] SplitString(string p_message, string p_delimeter) 
		{
			string [] l_split=null;

			if (p_message.Length>0 && p_delimeter.Length>0)
			{				
				char [] delimiters = p_delimeter.ToCharArray();				
				l_split = p_message.Split(delimiters);				
			}				
			return l_split;
		}

		/// <summary>
		/// TODO : KATHRYN [2007-04-20]
		/// Return for finding the specific item value in the message including delimeters		
		/// </summary>
		/// <param name="p_message">Original Message String including delimiters</param>
		/// <param name="p_delimeter">Delimieter char set e.g., " ,.:"</param>
		/// <param name="p_itemname">ItemName of Finding itemvalue including "=" e.g., EQID=IND106, where EQID is itemname, IND106 is itemvalue</param>
		/// <returns></returns>
		public string GetSplitItemValue(string p_message, string p_delimeter, string p_itemname) 
		{
			string [] l_split;
			string l_result="";

			if (p_message.Length>0 && p_delimeter.Length>0 && p_itemname.Length>0)
			{
				
				char [] delimiters = p_delimeter.ToCharArray();				
				l_split = p_message.Split(delimiters);

				foreach (string s in l_split) 
				{	
					if (s.StartsWith(p_itemname))
					{
						l_result= s.Substring(p_itemname.Length+1);  //+1 is "=" string length.
						break; 
					}
				}				
			}				
			
			return l_result;			

		}


        static public DataTable GetTable(string p_tablename, string[] p_columnlist, string[]  p_valuelist )
        {
            DataTable dt = new DataTable();

            try
            {

                dt = new DataTable(p_tablename);
                dt.Columns.Add(p_columnlist[0], System.Type.GetType("System.String"));
                
                DataRow dr = dt.NewRow();
                for (int i = 0; i < p_valuelist.Length; i++)
                {
                    dr[p_columnlist[0]] = p_valuelist[i];
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                }
                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
            }

            return dt;
        }

        public static bool IsNumeric(string data)
        {
            if (data == null)
                return false;

            if (data.Trim('+', '-', ' ', '.') == "")
                return false;

            Regex reg = new Regex(@"^[+-]?\d*(\.?\d*)$");
            return reg.IsMatch(data);
        }


        public static NumericValidation_Result CheckNumericRange(string ParameterName, NumericValidation_DataType dataType, BTextBox bTxtBox, NumericValidation_RangeOption minValueOption, double minValue, NumericValidation_RangeOption maxValueOption, double maxValue, bool ShowMessageBoxOnError)
        {
            MultiLanguageHandler ml = null;

            if (ShowMessageBoxOnError)
                ml = MultiLanguageHandler.getInstance();

            //Check the value is empty.
            if (bTxtBox == null || bTxtBox.Text == null || bTxtBox.Text.Trim() == "")
            {
                bTxtBox.SelectionStart = 0;
                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                bTxtBox.Focus();
                if (ShowMessageBoxOnError)
                    MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_PRE_SET"), ParameterName));
                return NumericValidation_Result.Empty;
            }

            //Check the value is non-numeric.
            if (!IsNumeric(bTxtBox.Text))
            {
                bTxtBox.SelectionStart = 0;
                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                bTxtBox.Focus();
                if (ShowMessageBoxOnError)
                    MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_075"), ParameterName));
                return NumericValidation_Result.NonNumeric;
            }
            
            try
            {
                //Check range of the value.
                if (dataType == NumericValidation_DataType.Double)
                {
                    double doubleValue = double.NaN;

                    try
                    {
                        doubleValue = double.Parse(bTxtBox.Text.Trim());
                    }
                    catch
                    {
                        bTxtBox.SelectionStart = 0;
                        bTxtBox.SelectionLength = bTxtBox.Text.Length;
                        bTxtBox.Focus();
                        if (ShowMessageBoxOnError)
                            MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_075"), ParameterName));
                        return NumericValidation_Result.InvalidDataType;
                    }

                    if (minValueOption != NumericValidation_RangeOption.NotInUse)
                    {
                        if (minValueOption == NumericValidation_RangeOption.IncludeValue)
                        {
                            if (doubleValue < minValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (maxValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_069"), ParameterName, minValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_068"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                        else
                        {
                            if (doubleValue <= minValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (maxValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_069"), ParameterName, minValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_068"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                    }

                    if (maxValueOption != NumericValidation_RangeOption.NotInUse)
                    {
                        if (maxValueOption == NumericValidation_RangeOption.IncludeValue)
                        {
                            if (doubleValue > maxValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (minValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_077"), ParameterName, maxValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_068"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                        else
                        {
                            if (doubleValue >= maxValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (minValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_077"), ParameterName, maxValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_068"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }

                    }
                }
                else
                {
                    int intValue = int.MinValue;

                    try
                    {
                        intValue = int.Parse(bTxtBox.Text.Trim());
                    }
                    catch
                    {
                        bTxtBox.SelectionStart = 0;
                        bTxtBox.SelectionLength = bTxtBox.Text.Length;
                        bTxtBox.Focus();
                        if (ShowMessageBoxOnError)
                        {
                            if(minValueOption == NumericValidation_RangeOption.NotInUse && maxValueOption == NumericValidation_RangeOption.NotInUse)
                                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_078"), ParameterName));
                            else if(minValueOption == NumericValidation_RangeOption.NotInUse)
                                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_076"), ParameterName, maxValue.ToString()));
                            else if(maxValueOption == NumericValidation_RangeOption.NotInUse)
                                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_070"), ParameterName, minValue.ToString()));
                            else
                                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_074"), ParameterName, minValue.ToString(), maxValue.ToString()));
                        }
                        return NumericValidation_Result.InvalidDataType;
                    }

                    if (minValueOption != NumericValidation_RangeOption.NotInUse)
                    {
                        if (minValueOption == NumericValidation_RangeOption.IncludeValue)
                        {
                            if (intValue < minValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (maxValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_070"), ParameterName, minValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_074"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                        else
                        {
                            if (intValue <= minValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (maxValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_070"), ParameterName, minValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_074"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                    }

                    if (maxValueOption != NumericValidation_RangeOption.NotInUse)
                    {
                        if (maxValueOption == NumericValidation_RangeOption.IncludeValue)
                        {
                            if (intValue > maxValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (minValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_076"), ParameterName, maxValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_074"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                        else
                        {
                            if (intValue >= maxValue)
                            {
                                bTxtBox.SelectionStart = 0;
                                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                                bTxtBox.Focus();
                                if (ShowMessageBoxOnError)
                                {
                                    if (minValueOption == NumericValidation_RangeOption.NotInUse)
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_076"), ParameterName, maxValue.ToString()));
                                    else
                                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_INFO_074"), ParameterName, minValue.ToString(), maxValue.ToString()));
                                }
                                return NumericValidation_Result.OutOfRange;
                            }
                        }
                            
                    }

                }

                return NumericValidation_Result.OK;
                
            }
            catch
            {
                bTxtBox.SelectionStart = 0;
                bTxtBox.SelectionLength = bTxtBox.Text.Length;
                bTxtBox.Focus();
                if (ShowMessageBoxOnError)
                    MSGHandler.DisplayMessage(MSGType.Warning, string.Format(ml.GetMessage("VMS_WRONG"), ParameterName));
                return NumericValidation_Result.Etc;
            }

        }



        public static bool IsNullOrEmptyDataSet(DataSet dataSet)
        {
            try
            {
                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables.Count; i++)
                    {
                        if (dataSet.Tables[i].Rows.Count > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {

            }

            return true ;
        }

        public static bool IsNullOrEmptyDataTable(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {                        
                       return false;
                    }
                }
            }
            catch
            {

            }

            return true ;
        }



        public static string GetConditionKeyData(DataTable _dt, string _conditionKey)
        {
            if (_dt == null && _dt.Rows.Count == 0)
                return null;
            else
                return _dt.Rows[0][_conditionKey].ToString();
        }

        public static string GetDisplayData(DataTable _dt)
        {
            return GetConditionKeyData(_dt, Definition.DynamicCondition_Condition_key.DISPLAYDATA);
        }


        public static string GetDisplayDataList(DataTable dt)
        {
            return GetConditionKeyDataList(dt, Definition.DynamicCondition_Condition_key.DISPLAYDATA);
        }        

        public static string GetConditionKeyDataList(DataTable dt, string _conditionKey)
        {
            return DataUtil.GetConditionKeyDataList(dt,_conditionKey,false);
        }
        
        public static string GetConditionKeyDataList(DataTable dt, string _conditionKey, bool bReplece)
        {
            string strParam = string.Empty;
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][_conditionKey].ToString() == Definition.VARIABLE.STAR) continue;
                    strParam += dt.Rows[i][_conditionKey].ToString() + ",";
                }

                
                if (!string.IsNullOrEmpty(strParam))
                {
                    strParam = strParam.Substring(0, strParam.Length - 1);
                    
                    if(bReplece)
                    strParam = "'" + strParam.Replace(",", "','") + "'";
                }

            }
            catch (Exception ex)
            {
                return strParam;
            }

            return strParam;
        }




        public static DataTable DataTableImportRow(DataRow[] drSelectDataRow)
        {
            DataTable dt = new DataTable();
            if (drSelectDataRow.Length > 0)
            {
                dt = drSelectDataRow[0].Table.Clone();
            }
            foreach (DataRow dr in drSelectDataRow)
            {
                dt.ImportRow(dr);


            }

            return dt;
        }

       
       
      
	}
}
