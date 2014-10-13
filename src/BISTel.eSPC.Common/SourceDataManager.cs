using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using FarPoint.Win.Spread;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Common
{
    public delegate void DelegateClosePopup(System.Windows.Forms.DialogResult popupResult);

    public interface IPopupContents
    {
        event DelegateClosePopup ClosePopup;
    }

    public enum VariableDataType
    {
        Interval,
        Nominal
    }


    public class SourceDataManager
    {
        const int START_RAW_DATA_INDEX = 0;
        const string SystemNamedColumn = "Data Set";

        DataTable dtRawData = new DataTable();   //Raw Data
        DataTable dtRawDataOriginal = new DataTable();   //Raw Data
        FpSpread sprDataView = null;
        LinkedList _userDefinedData = null;
        private DataSet filteredRawDataSet = null;

        BISTel.eSPC.Common.Common _com = new BISTel.eSPC.Common.Common();

        GrouperAndFilter _grouperAndFilter = new GrouperAndFilter();



        ///////////////////////////////////////////////////////////////////////////////////
        //  Property.
        ///////////////////////////////////////////////////////////////////////////////////

        #region Parameter Info.
        public ArrayList ParamList
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRawData.Columns.Count; i++)
                    {
                        string colName = dtRawData.Columns[i].ColumnName;
                        string colType = dtRawData.Rows[1][i].ToString();

                        if (colName != SystemNamedColumn && colType == Definition.VARIABLE_DATA_TYPE.INTERVAL)
                        {
                            arParams.Add(colName);
                        }

                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamListALL
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null)
                {
                    for (int i = 0; i < dtRawData.Columns.Count; i++)
                    {
                        arParams.Add(dtRawData.Columns[i].ColumnName);
                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamList_Missing
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null)
                {
                    for (int i = 0; i < dtRawData.Columns.Count; i++)
                    {
                        for (int j = 0; j < dtRawData.Rows.Count; j++)
                        {
                            if (String.IsNullOrEmpty(dtRawData.Rows[j][i].ToString()))
                            {
                                arParams.Add(dtRawData.Columns[i].ColumnName);
                                break;
                            }
                        }
                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamList_Input
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    for (int i = 1; i < dtRawData.Columns.Count; i++)
                    {
                        if (dtRawData.Rows[0][i].ToString() == Definition.VARIABLE_ROLE.INPUT)
                        {
                            arParams.Add(dtRawData.Columns[i].ColumnName);
                        }

                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamList_Input_Interval
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    for (int i = 1; i < dtRawData.Columns.Count; i++)
                    {
                        if (dtRawData.Rows[0][i].ToString() == Definition.VARIABLE_ROLE.INPUT && dtRawData.Rows[1][i].ToString() == Definition.VARIABLE_DATA_TYPE.INTERVAL)
                        {
                            arParams.Add(dtRawData.Columns[i].ColumnName);
                        }
                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamList_Output
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    for (int i = 1; i < dtRawData.Columns.Count; i++)
                    {
                        if (dtRawData.Rows[0][i].ToString() == Definition.VARIABLE_ROLE.OUTPUT)
                        {
                            arParams.Add(dtRawData.Columns[i].ColumnName);
                        }
                    }
                }

                return arParams;
            }
        }

        public ArrayList ParamList_Output_Interval
        {
            get
            {
                ArrayList arParams = new ArrayList();

                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    for (int i = 1; i < dtRawData.Columns.Count; i++)
                    {
                        if (dtRawData.Rows[0][i].ToString() == Definition.VARIABLE_ROLE.OUTPUT &&
                            dtRawData.Rows[1][i].ToString() == Definition.VARIABLE_DATA_TYPE.INTERVAL)
                        {
                            arParams.Add(dtRawData.Columns[i].ColumnName);
                        }
                    }
                }

                return arParams;
            }
        }

        public Definition.ParameterList ParamListALL_WithDataType
        {
            get
            {
                Definition.ParameterList paramList = null;

                if (dtRawData != null)
                {
                    paramList = new Definition.ParameterList();

                    for (int i = 1; i < dtRawData.Columns.Count; i++)
                    {
                        string paramName = dtRawData.Columns[i].ColumnName;
                        string dataType = dtRawData.Rows[1][i].ToString();
                        string variableRole = dtRawData.Rows[0][i].ToString();

                        paramList.Add(new Definition.ParameterInfo(paramName, dataType, variableRole));
                    }
                }

                return paramList;
            }
        }

        public string WhatIsParameterDataType(string parameter)
        {
            return this.dtRawData.Rows[1][parameter].ToString();
        }
        #endregion

        #region Data Info
        public bool HasData
        {
            get
            {
                if (dtRawData != null && dtRawData.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DataTable RawDataTable
        {
            get { return dtRawData; }
            set { dtRawData = value; }
        }

        public DataTable RawDataTableOriginal
        {
            get { return dtRawDataOriginal; }
            set { dtRawDataOriginal = value; }
        }

        public DataSet FilteredRawDataSet
        {
            get { return filteredRawDataSet; }
        }

        public FpSpread DataViewSpread
        {
            get { return this.sprDataView; }
            set { this.sprDataView = value; }
        }

        public int DataRowCount
        {
            get
            {
                try
                {
                    return this.RawDataTable.Rows.Count - START_RAW_DATA_INDEX;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public LinkedList UserDefinedData
        {
            get { return this._userDefinedData; }
            set { this._userDefinedData = value; }
        }

        public DataTable RawDataTableWithoutDataTypeInfo
        {
            get
            {
                DataTable dtNewRawDataTable = this.dtRawData.Copy();

                for (int i = START_RAW_DATA_INDEX - 1; i >= 0; i--)
                {
                    dtNewRawDataTable.Rows.RemoveAt(i);
                }

                return dtNewRawDataTable;
            }
        }
        #endregion

        public string SYSTEM_NAMED_COLUMN_NAME
        {
            get { return SystemNamedColumn; }
        }



        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////

        public int GetDataCount(string p_dataSetType)
        {
            int cnt = 0;

            if (RawDataTable == null || RawDataTable.Rows.Count < 1)
            {
                return cnt;
            }

            for (int i = START_RAW_DATA_INDEX; i < RawDataTable.Rows.Count; i++)
            {
                if (RawDataTable.Rows[i].ItemArray[0].ToString() == p_dataSetType)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        private static double GetMaxValue(double[] data)
        {
            int i;
            double dResult = Double.NaN;
            double dMax = Double.MinValue;

            for (i = 0; i < data.Length; i++)
            {
                if (dMax < data[i])
                {
                    dMax = data[i];
                }
            }

            dResult = dMax;

            return dResult;
        }


        public DataTable MakeRawDataTable(DataTable dt)
        {
            if (dt.Columns[0].ColumnName != SystemNamedColumn)
            {

                System.Data.DataTable l_DataTable = new DataTable();

                l_DataTable.Columns.Add(SystemNamedColumn, System.Type.GetType("System.String"));

                for (int i = 1; i < dt.Columns.Count + 1; i++)
                {
                    l_DataTable.Columns.Add(dt.Columns[i - 1].ColumnName.Replace("#", "."), System.Type.GetType("System.String"));
                }

                foreach (DataColumn dc in l_DataTable.Columns)
                    dc.DefaultValue = System.DBNull.Value;

                DataRow row;

                row = l_DataTable.NewRow();
                for (int j = 1; j < l_DataTable.Columns.Count; j++)
                {
                    row[j] = Definition.VARIABLE_ROLE.UNDEFINED;
                }
                l_DataTable.Rows.Add(row);


                row = l_DataTable.NewRow();
                for (int j = 1; j < l_DataTable.Columns.Count; j++)
                {
                    row[j] = Definition.VARIABLE_DATA_TYPE.INTERVAL;
                }
                l_DataTable.Rows.Add(row);

                try
                {
                    Dictionary<string, double[]> rowdata = new Dictionary<string, double[]>(); //column name, data

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = l_DataTable.NewRow();

                        try
                        {
                            for (int j = 1; j < l_DataTable.Columns.Count; j++)
                            {
                                row[j] = dt.Rows[i][j - 1];

                                string data = dt.Rows[i][j - 1].ToString();

                                //TODO : Jiny [2009.03.31] min/max setting
                                if (_com.Check_Numeric(data))
                                {
                                    if (!rowdata.ContainsKey(l_DataTable.Columns[j].ColumnName))
                                    {
                                        double[] datalist = new double[dt.Rows.Count];
                                        datalist[0] = double.Parse(data);

                                        rowdata.Add(l_DataTable.Columns[j].ColumnName, datalist);
                                    }
                                    else
                                    {
                                        rowdata[l_DataTable.Columns[j].ColumnName][i] = double.Parse(data);
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }

                        l_DataTable.Rows.Add(row);
                    }
                }
                catch
                {

                }

                l_DataTable.Rows[0][0] = "Role";
                l_DataTable.Rows[1][0] = "Data Type";

                this.dtRawData = l_DataTable;

                return l_DataTable;
            }
            else
            {
                this.dtRawData = dt;

                return dt;
            }
        }

        public bool MakeRawDataTable(FpSpread objSpread)
        {
            bool bNewTable = false;
            System.Data.DataTable l_DataTable = new DataTable();

            if (objSpread.ActiveSheet.Columns[0].Label == SystemNamedColumn || objSpread.ActiveSheet.Cells[0, 0].Text.Trim() == SystemNamedColumn)
            {
                if (objSpread.ActiveSheet.Columns[0].Label == SystemNamedColumn)
                {
                    for (int i = 0; i < objSpread.ActiveSheet.Columns.Count; i++)
                    {
                        l_DataTable.Columns.Add(objSpread.ActiveSheet.Columns[i].Label.Replace("#", "."), System.Type.GetType("System.String"));
                    }
                }
                else
                {
                    for (int i = 0; i < objSpread.ActiveSheet.Columns.Count; i++)
                    {
                        l_DataTable.Columns.Add(objSpread.ActiveSheet.Cells[0, i].Text.Replace("#", "."), System.Type.GetType("System.String"));
                    }

                    objSpread.ActiveSheet.RemoveRows(0, 1);
                }

                DataRow row;

                try
                {
                    for (int i = 0; i < objSpread.ActiveSheet.Rows.Count; i++)
                    {
                        row = l_DataTable.NewRow();

                        try
                        {
                            for (int j = 0; j < l_DataTable.Columns.Count; j++)
                            {
                                row[j] = objSpread.ActiveSheet.Cells[i, j].Text;
                            }
                        }
                        catch
                        {
                        }

                        l_DataTable.Rows.Add(row);

                    }
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                bNewTable = true;

                if (objSpread.ActiveSheet.Columns[0].Label == "A" && objSpread.ActiveSheet.Columns[1].Label == "B")
                {
                    l_DataTable.Columns.Add(SystemNamedColumn, System.Type.GetType("System.String"));

                    for (int i = 0; i < objSpread.ActiveSheet.Columns.Count; i++)
                    {
                        string columnName = objSpread.ActiveSheet.Cells[0, i].Text.Trim();

                        if (l_DataTable.Columns.Contains(columnName))
                        {
                            int cnt = 1;

                            while (!l_DataTable.Columns.Contains(columnName + "_" + cnt.ToString()))
                            {
                                cnt++;
                            }

                            l_DataTable.Columns.Add(columnName + "_" + cnt.ToString(), System.Type.GetType("System.String"));
                        }
                        else
                        {
                            l_DataTable.Columns.Add(columnName, System.Type.GetType("System.String"));
                        }
                    }

                    objSpread.ActiveSheet.RemoveRows(0, 1);
                }
                else
                {
                    l_DataTable.Columns.Add(SystemNamedColumn, System.Type.GetType("System.String"));

                    for (int i = 0; i < objSpread.ActiveSheet.Columns.Count; i++)
                    {
                        string columnName = objSpread.ActiveSheet.Columns[i].Label.Replace("#", ".");

                        if (l_DataTable.Columns.Contains(columnName))
                        {
                            int cnt = 1;
                            while (!l_DataTable.Columns.Contains(columnName + "_" + cnt.ToString()))
                            {
                                cnt++;
                            }

                            l_DataTable.Columns.Add(columnName + "_" + cnt.ToString(), System.Type.GetType("System.String"));
                        }
                        else
                        {
                            l_DataTable.Columns.Add(columnName, System.Type.GetType("System.String"));
                        }
                    }
                }

                DataRow row;

                row = l_DataTable.NewRow();
                for (int j = 1; j < l_DataTable.Columns.Count; j++)
                {
                    row[j] = Definition.VARIABLE_ROLE.UNDEFINED;
                }
                l_DataTable.Rows.Add(row);


                row = l_DataTable.NewRow();
                for (int j = 1; j < l_DataTable.Columns.Count; j++)
                {
                    row[j] = Definition.VARIABLE_DATA_TYPE.INTERVAL;
                }
                l_DataTable.Rows.Add(row);

                try
                {
                    Dictionary<string, double[]> rowdata = new Dictionary<string, double[]>(); //column name, data

                    for (int i = 0; i < objSpread.ActiveSheet.Rows.Count; i++)
                    {
                        row = l_DataTable.NewRow();

                        try
                        {
                            for (int j = 1; j < l_DataTable.Columns.Count; j++)
                            {
                                row[j] = objSpread.ActiveSheet.Cells[i, j - 1].Text;

                                string data = objSpread.ActiveSheet.Cells[i, j - 1].Text;

                                //TODO : Jiny [2009.03.31] min/max setting
                                if (_com.Check_Numeric(data))
                                {
                                    if (!rowdata.ContainsKey(l_DataTable.Columns[j].ColumnName))
                                    {
                                        double[] datalist = new double[objSpread.ActiveSheet.RowCount];
                                        datalist[0] = double.Parse(data);

                                        rowdata.Add(l_DataTable.Columns[j].ColumnName, datalist);
                                    }
                                    else
                                    {
                                        rowdata[l_DataTable.Columns[j].ColumnName][i] = double.Parse(data);
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }

                        l_DataTable.Rows.Add(row);
                    }

                    l_DataTable.Rows[0][0] = "Role";
                    l_DataTable.Rows[1][0] = "Data Type";
                }
                catch (Exception ex)
                {

                }
            }

            this.dtRawData = l_DataTable;

            return bNewTable;
        }

        public void Filter(Dictionary<string, string[]> groupingAndFilteringCondition)
        {
            // update filter information
            if(groupingAndFilteringCondition.Count == 0)
            {
                filteredRawDataSet = null;
                return;
            }

            filteredRawDataSet = _grouperAndFilter.GroupingAndFiltering(this.RawDataTable, groupingAndFilteringCondition);
        }

        public void ReleaseData()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref dtRawData);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref dtRawDataOriginal);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref filteredRawDataSet);
        }
    }
}
