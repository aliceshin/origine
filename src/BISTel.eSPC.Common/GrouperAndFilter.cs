using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BISTel.eSPC.Common
{
    public class GrouperAndFilter
    {
        public DataSet GroupingAndFiltering(DataTable originalData, Dictionary<string, string[]> groupingAndFilteringCondition)
        {
            SortedDictionary<string, DataTable> filteredTables = new SortedDictionary<string, DataTable>();
            foreach(DataRow dr in originalData.Rows)
            {
                if(IsRowNeededToBeAdded(dr, groupingAndFilteringCondition))
                {
                    string newTableName = GetTableName(dr, groupingAndFilteringCondition);

                    DataTable dt = null;
                    if(!filteredTables.ContainsKey(newTableName))
                    {
                        dt = originalData.Clone();
                        dt.TableName = newTableName;
                        filteredTables.Add(newTableName, dt);
                    }
                    dt = filteredTables[newTableName];

                    dt.ImportRow(dr);
                }
            }

            DataSet result = new DataSet();
            foreach(var kvp in filteredTables)
            {
                result.Tables.Add(kvp.Value);
            }

            return result;
        }


        /// <summary>
        /// Check the row of the raw table is needed to be added to the series
        /// </summary>
        /// <param name="dr">The row of the raw table</param>
        /// <param name="groupingAndFilteringValue">Filtering and grouping condition</param>
        /// <returns></returns>
        public bool IsRowNeededToBeAdded(DataRow dr, Dictionary<string, string[]> groupingAndFilteringValue)
        {
            foreach(var kvp in groupingAndFilteringValue)
            {
                string value = GetFirstValue(dr[kvp.Key].ToString());

                bool exists = Array.Exists(kvp.Value, delegate(string s) 
                {
                    return s.Equals(value);
                });

                if(!exists)
                    return false;
            }

            return true;
        }

        public string GetFirstValue(string s)
        {
            if(s == null)
                return string.Empty;

            return s.Split(';')[0];
        }

        private string GetTableName(DataRow dr, Dictionary<string, string[]> groupingAndFilteringCondition)
        {
            string tableName = string.Empty;

            foreach(var kvp in groupingAndFilteringCondition)
            {
                string value = GetFirstValue(dr[kvp.Key].ToString());
                foreach(string s in kvp.Value)
                {
                    if(s.ToUpper() == value.ToUpper())
                    {
                        if(tableName.Length > 0)
                            tableName += "^";

                        tableName += s.ToUpper();
                    }
                }
            }

            return tableName;
        }
    }
}
