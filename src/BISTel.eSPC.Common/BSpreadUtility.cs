using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

using BISTel.PeakPerformance.Client.BISTelControl;

using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using FarPoint.Win.Spread.CellType;

namespace BISTel.eSPC.Common
{
    public class BSpreadUtility
    {
        private int _iAddCol = -1;
        SortedList _slDefaultValue = new SortedList();

        public ArrayList GetColumnNames(Control control, int iColHeaderIndex)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            ArrayList alColumnNames = new ArrayList();

            if (bSpread.ActiveSheet.RowCount > 0)
            {
                for (int i = 0; i < bSpread.ActiveSheet.ColumnCount; i++)
                {
                    if (bSpread.ActiveSheet.Columns[i].Visible)
                    {
                        string sColumnName = bSpread.ActiveSheet.ColumnHeader.Cells[iColHeaderIndex, i].Text;
                        alColumnNames.Add(sColumnName);
                    }
                }
            }
            return alColumnNames;
        }

        public void SetColumnHeaderStyle(Control control, bool bEdit)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int iColHeaderRowCount = bSpread.ActiveSheet.ColumnHeaderRowCount;
            int iRowHeaderColCount = bSpread.ActiveSheet.RowHeaderColumnCount;

            if (bEdit)
            {
                if (iColHeaderRowCount <= 1)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor = Color.FromArgb(0, 153, 204);
                    bSpread.ActiveSheet.ColumnHeader.Rows[0].ForeColor = Color.White;
                }

                if (iColHeaderRowCount > 1 && iColHeaderRowCount <= 2)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[1].BackColor = Color.FromArgb(72, 174, 200);
                    bSpread.ActiveSheet.ColumnHeader.Rows[1].ForeColor = Color.White;
                }

                if (iColHeaderRowCount > 2 && iColHeaderRowCount <= 3)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[2].BackColor = Color.FromArgb(158, 204, 221);
                    bSpread.ActiveSheet.ColumnHeader.Rows[2].ForeColor = Color.White;
                }

                bSpread.ActiveSheet.ColumnHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(199, 223, 228));
                bSpread.ActiveSheet.ColumnHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(199, 223, 228));
                if (iRowHeaderColCount != 0)
                {
                    bSpread.ActiveSheet.RowHeader.Columns[0].ForeColor = Color.White;

                    bSpread.ActiveSheet.RowHeader.Columns[-1].BackColor = bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor;
                    bSpread.ActiveSheet.RowHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(199, 223, 228));
                    bSpread.ActiveSheet.RowHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(199, 223, 228));
                }
            }
            else
            {
                if (iColHeaderRowCount <= 1)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor = Color.FromArgb(218, 231, 177);
                    bSpread.ActiveSheet.ColumnHeader.Rows[0].ForeColor = Color.Black;
                }

                if (iColHeaderRowCount > 1 && iColHeaderRowCount <= 2)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[1].BackColor = Color.FromArgb(209, 220, 177);
                    bSpread.ActiveSheet.ColumnHeader.Rows[1].ForeColor = Color.Black;
                }

                if (iColHeaderRowCount > 2 && iColHeaderRowCount <= 3)
                {
                    bSpread.ActiveSheet.ColumnHeader.Rows[2].BackColor = Color.FromArgb(164, 181, 192);
                    bSpread.ActiveSheet.ColumnHeader.Rows[2].ForeColor = Color.Black;
                }

                bSpread.ActiveSheet.ColumnHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(204, 204, 153));
                bSpread.ActiveSheet.ColumnHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(204, 204, 153));
                if (iRowHeaderColCount != 0)
                {
                    bSpread.ActiveSheet.RowHeader.Columns[0].ForeColor = Color.Black;

                    bSpread.ActiveSheet.RowHeader.Columns[-1].BackColor = Color.FromArgb(187, 197, 159);
                    bSpread.ActiveSheet.RowHeader.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(204, 204, 153));
                    bSpread.ActiveSheet.RowHeader.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.FromArgb(204, 204, 153));
                }
            }

            bSpread.ActiveSheet.ColumnHeader.DefaultStyle.BackColor = bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor;
            //bSpread.ActiveSheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            //bSpread.ActiveSheet.RowHeader.Columns.Default.Resizable = false;
            bSpread.ActiveSheet.RowHeader.DefaultStyle.BackColor = bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor;
            //bSpread.ActiveSheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            bSpread.ActiveSheet.RowHeader.DefaultStyle.BackColor = bSpread.ActiveSheet.ColumnHeader.Rows[0].BackColor;
            //bSpread.ActiveSheet.SheetCornerStyle.Parent = "HeaderDefault";
            bSpread.ActiveSheet.GrayAreaBackColor = Color.FromArgb(226, 236, 221);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iColStartIndex"></param>
        /// <param name="iRowStartIndex"></param>
        /// <param name="iColEndIndex"></param>
        /// <param name="iRowEndIndex"></param>
        /// <param name="iType">0:Cell, 1:Row, 2:column, 3:Range</param>
        public void SetCellLock(Control control, int iRowStartIndex, int iColStartIndex, int iRowEndIndex, int iColEndIndex, int iType, bool BLock)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int iRowCount = bSpread.ActiveSheet.RowCount;
            int iColCount = bSpread.ActiveSheet.ColumnCount;

            if (iRowCount > 0 && iColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iRowStartIndex, iColStartIndex].Locked = BLock;
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iRowStartIndex].Locked = BLock;
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iColStartIndex].Locked = BLock;
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iRowStartIndex, iColStartIndex, iRowEndIndex, iColEndIndex].Locked = BLock;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="sKeyField">the field name of key that use to find modified datarow.</param>
        /// <param name="sValue">the value of datarow modified.</param>
        /// <param name="Initialds">the dataset searched</param>
        /// <param name="slCheckedField">Key : column index of screen, value : fieldname of dataset</param>
        /// <returns></returns>
        public bool GetModifiedStatus(Control control, string sKeyField, string sValue, int iRowIndex, DataSet Initialds, SortedList slCheckedField)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;


            string strFilter = sKeyField + "= '" + sValue + "'";

            try
            {
                DataRow[] dr = Initialds.Tables[0].Select(strFilter);

                if (dr.Length > 0)
                {

                    for (int i = 0; i < slCheckedField.Count; i++)
                    {
                        int iKey = (int)slCheckedField.GetKey(i);
                        string sField = slCheckedField.GetByIndex(i).ToString();

                        string sModifiedValue = "";
                        string sInitialValue = "";

                        FarPoint.Win.Spread.CellType.ICellType ct = bSpread.ActiveSheet.GetCellType(iRowIndex, iKey);

                        if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                        {
                            if (bSpread.ActiveSheet.Cells[iRowIndex, iKey].Value != null)
                                sModifiedValue = bSpread.ActiveSheet.Cells[iRowIndex, iKey].Value.ToString();
                            else
                                sModifiedValue = "";

                            if (sModifiedValue.ToUpper() == Definition.VARIABLE_ZERO)
                                sModifiedValue = Definition.VARIABLE_FALSE;
                            else if (sModifiedValue.ToUpper() == Definition.VARIABLE_ONE)
                                sModifiedValue = Definition.VARIABLE_TRUE;


                            if (sModifiedValue.ToUpper() == Definition.VARIABLE_FALSE)
                                sModifiedValue = Definition.VARIABLE_FALSE;
                            else if (sModifiedValue.ToUpper() == Definition.VARIABLE_TRUE)
                                sModifiedValue = Definition.VARIABLE_TRUE;


                            sInitialValue = dr[0][sField].ToString();

                            if (sInitialValue.ToUpper() == Definition.VARIABLE_ZERO)
                                sInitialValue = Definition.VARIABLE_FALSE;
                            else if (sInitialValue.ToUpper() == Definition.VARIABLE_ONE)
                                sInitialValue = Definition.VARIABLE_TRUE;

                            if (sInitialValue.ToUpper() == Definition.VARIABLE_FALSE)
                                sInitialValue = Definition.VARIABLE_FALSE;
                            else if (sInitialValue.ToUpper() == Definition.VARIABLE_TRUE)
                                sInitialValue = Definition.VARIABLE_TRUE;
                        }
                        else
                        {
                            sModifiedValue = bSpread.ActiveSheet.Cells[iRowIndex, iKey].Text;
                            sInitialValue = dr[0][sField].ToString();
                        }

                        if (sModifiedValue.Trim() != sInitialValue.Trim())
                            return true;
                    }
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                return false;
            }

            return false;
        }


        public DataSet GetModifiedDataSet(Control control, string sKeyField, int iStartColIndex, DataSet Initialds)
        {

            DataSet dsReturn = new DataSet();
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                DataSet ds = (DataSet)bSpread.DataSource;

                DataSet dsChanged = new DataSet();

                if (ds.Tables[0].Rows.Count == 1)
                {
                    ds.AcceptChanges();
                    dsChanged = ds.Copy();
                }
                else
                {
                    dsChanged = ds.GetChanges();
                }


                bool bModified = false;

                if (Initialds != null)
                {
                    if (dsChanged != null)
                    {
                        for (int i = dsChanged.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {

                            string sFilterKeyValue = dsChanged.Tables[0].Rows[i][sKeyField].ToString();

                            DataRow drChangedRow = dsChanged.Tables[0].Rows[i];

                            string strFilter = sKeyField + "= '" + sFilterKeyValue + "'";

                            try
                            {
                                DataRow[] drInitialRow = Initialds.Tables[0].Select(strFilter);

                                if (drInitialRow != null && drInitialRow.Length != 0)
                                {
                                    for (int j = iStartColIndex; j < Initialds.Tables[0].Columns.Count; j++)
                                    {
                                        string strChangedValue = "";
                                        string strInitialValue = "";

                                        if (drChangedRow[Initialds.Tables[0].Columns[j].ColumnName] != null)
                                            strChangedValue = drChangedRow[Initialds.Tables[0].Columns[j].ColumnName].ToString();

                                        if (drInitialRow[0][Initialds.Tables[0].Columns[j].ColumnName] != null)
                                            strInitialValue = drInitialRow[0][Initialds.Tables[0].Columns[j].ColumnName].ToString();

                                        if (strChangedValue.Trim() != strInitialValue.Trim())
                                        {
                                            bModified = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    bModified = true;
                                }
                            }
                            catch
                            {
                                bModified = true;
                            }

                            if (!bModified)
                            {
                                dsChanged.Tables[0].Rows.Remove(drChangedRow);
                            }
                        }
                        dsReturn = dsChanged;
                    }
                    else
                    {
                        //dsReturn = dsChanged.Clone();

                        dsReturn = Initialds.Clone();
                    }
                }
                else
                {
                    dsReturn = dsChanged;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsReturn;
        }

        public DataSet GetSelectedDataSet(Control control, string[] saFilter, bool bBISTelControl)
        {

            DataSet dsChanged = new DataSet();
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                DataSet ds_Original = (DataSet)bSpread.DataSource;
                DataSet ds = ds_Original.Copy();
                ds.AcceptChanges();

                if (ds.Tables.Count > 0)
                {
                    SortedList slData = new SortedList();


                    for (int j = 0; j < saFilter.Length; j++)
                    {
                        string sFilter = "";
                        string sField = saFilter[j].ToString();

                        if (sField == "_INSERT")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_INSERT = 1";
                                else
                                    sFilter = sFilter + " OR _INSERT = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_INSERT = True";
                                else
                                    sFilter = sFilter + " OR (_INSERT = True)";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_INSERT", drChanged);
                        }
                        else if (sField == "_MODIFY")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_MODIFY = 1";
                                else
                                    sFilter = sFilter + " OR _MODIFY = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_MODIFY = True";
                                else
                                    sFilter = sFilter + " OR _MODIFY = True";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_MODIFY", drChanged);
                        }
                        else if (sField == "_DELETE")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_DELETE = 1";
                                else
                                    sFilter = sFilter + " OR _DELETE = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_DELETE = True";
                                else
                                    sFilter = sFilter + " OR _DELETE = True";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_DELETE", drChanged);
                        }
                    }



                    //DataTable dtChanged = new DataTable();

                    DataTable dtChanged = ds.Tables[0].Clone();

                    for (int j = 0; j < slData.Count; j++)
                    {
                        DataRow[] draChanged = (DataRow[])slData.GetByIndex(j);

                        for (int k = 0; k < draChanged.Length; k++)
                        {
                            dtChanged.ImportRow(draChanged[k]);
                        }
                    }

                    dsChanged.Tables.Add(dtChanged);
                }

                return dsChanged;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                return dsChanged;
            }

            return dsChanged;
        }

        public DataSet GetSelectedDataSet_V2(BSpread bspr, int selectCheckColumnIndex)
        {
            if (bspr.Parent != null)
                bspr.Parent.Focus();

            DataSet dsBspread = new DataSet();
            DataSet dsSelect = new DataSet();

            try
            {
                dsBspread = (DataSet)bspr.DataSet;
                dsSelect = dsBspread.Clone();

                string selectColumnFiledName = bspr.ActiveSheet.Columns[selectCheckColumnIndex].DataField;

                int Count = dsBspread.Tables[0].Rows.Count;
                int iTrue = 0;

                for (int i = 0; i < Count; i++)
                {
                    if (dsBspread.Tables[0].Rows[i][selectColumnFiledName].ToString() == "True")
                    {
                        dsSelect.Tables[0].ImportRow((DataRow)dsBspread.Tables[0].Rows[i]);
                        dsSelect.Tables[0].Rows[iTrue][selectColumnFiledName] = "True";
                        iTrue++;
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsSelect;
        }

        public DataSet GetSelectedDataSet(BSpread bspr, int selectCheckColumnIndex)
        {
            if (bspr.Parent != null)
                bspr.Parent.Focus();

            DataSet dsSelect = new DataSet();

            try
            {
                dsSelect = ((DataSet)bspr.DataSet).Copy();

                if (bspr.ActiveSheet.RowCount > 0)
                {
                    string selectColumnFiledName = bspr.ActiveSheet.Columns[selectCheckColumnIndex].DataField;

                    if (dsSelect.Tables[0].Columns.Contains(selectColumnFiledName))
                    {
                        DataRow[] drNotSelectRows = dsSelect.Tables[0].Select(string.Format("{0} = '{1}' OR {0} = '' OR {0} IS NULL", selectColumnFiledName, "False"));

                        foreach (DataRow drNotSelectRow in drNotSelectRows)
                        {
                            drNotSelectRow.Delete();
                        }
                    }
                    else
                    {
                        dsSelect.Tables[0].Columns.Add(selectColumnFiledName);

                        for (int r = 0; r < bspr.ActiveSheet.RowCount; r++)
                        {
                            if (bspr.GetCellValue(r, selectCheckColumnIndex) != null && bspr.GetCellValue(r, selectCheckColumnIndex).ToString() == "True")
                                dsSelect.Tables[0].Rows[r][selectColumnFiledName] = "True";
                            else
                                dsSelect.Tables[0].Rows[r][selectColumnFiledName] = "False";
                        }

                        DataRow[] drNotSelectRows = dsSelect.Tables[0].Select(string.Format("{0} = '{1}'", selectColumnFiledName, "False"));

                        foreach (DataRow drNotSelectRow in drNotSelectRows)
                        {
                            drNotSelectRow.Delete();
                        }

                        dsSelect.Tables[0].Columns.Remove(selectColumnFiledName);
                    }
                }
                else
                {
                    dsSelect.Clear();
                }

                dsSelect.AcceptChanges();
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsSelect;
        }

        public DataSet GetModifiedDataSet(Control control, string[] saFilter, string sKeyField, int iStartColIndex, DataSet Initialds, bool bBISTelControl)
        {

            DataSet dsReturn = new DataSet();
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                DataSet ds = (DataSet)bSpread.DataSource;
                ds.AcceptChanges();

                if (ds.Tables.Count > 0)
                {
                    SortedList slData = new SortedList();


                    for (int j = 0; j < saFilter.Length; j++)
                    {
                        string sFilter = "";
                        string sField = saFilter[j].ToString();

                        if (sField == "_INSERT")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_INSERT = 1";
                                else
                                    sFilter = sFilter + " OR _INSERT = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_INSERT = True";
                                else
                                    sFilter = sFilter + " OR (_INSERT = True)";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_INSERT", drChanged);
                        }
                        else if (sField == "_MODIFY")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_MODIFY = 1";
                                else
                                    sFilter = sFilter + " OR _MODIFY = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_MODIFY = True";
                                else
                                    sFilter = sFilter + " OR _MODIFY = True";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_MODIFY", drChanged);
                        }
                        else if (sField == "_DELETE")
                        {
                            if (!bBISTelControl)
                            {
                                if (sFilter == "")
                                    sFilter = "_DELETE = 1";
                                else
                                    sFilter = sFilter + " OR _DELETE = 1";
                            }
                            else
                            {
                                if (sFilter == "")
                                    sFilter = "_DELETE = True";
                                else
                                    sFilter = sFilter + " OR _DELETE = True";
                            }

                            DataRow[] drChanged = ds.Tables[0].Select(sFilter);
                            slData.Add("_DELETE", drChanged);
                        }

                        //DataRow[] modifydr1 = dt.Select("(_INSERT=False or _INSERT is null)  and _MODIFY=True and (_DELETE is null or _DELETE=False )");
                    }

                    //DataSet dsChanged = ds.GetChanges();
                    //DataRow[] drChanged = ds.Tables[0].Select(sFilter);

                    DataSet dsChanged = new DataSet();

                    //DataTable dtChanged = new DataTable();

                    DataTable dtChanged = ds.Tables[0].Clone();

                    for (int j = 0; j < slData.Count; j++)
                    {
                        DataRow[] draChanged = (DataRow[])slData.GetByIndex(j);

                        for (int k = 0; k < draChanged.Length; k++)
                        {
                            dtChanged.ImportRow(draChanged[k]);
                        }
                    }

                    dsChanged.Tables.Add(dtChanged);

                    bool bModified = false;

                    if (Initialds != null)
                    {
                        if (dsChanged != null)
                        {
                            for (int i = dsChanged.Tables[0].Rows.Count - 1; i >= 0; i--)
                            {

                                string sFilterKeyValue = dsChanged.Tables[0].Rows[i][sKeyField].ToString();
                                DataRow drChangedRow = dsChanged.Tables[0].Rows[i];

                                string strFilter = sKeyField + "= '" + sFilterKeyValue + "'";

                                try
                                {
                                    DataRow[] drInitialRow = Initialds.Tables[0].Select(strFilter);

                                    if (drInitialRow != null && drInitialRow.Length != 0)
                                    {
                                        for (int j = iStartColIndex; j < Initialds.Tables[0].Columns.Count; j++)
                                        {
                                            string strChangedValue = "";
                                            string strInitialValue = "";

                                            //if(Initialds.Tables[0].Columns[j].ColumnName != "_INSERT" &&
                                            //    Initialds.Tables[0].Columns[j].ColumnName != "_MODIFY" &&
                                            //    Initialds.Tables[0].Columns[j].ColumnName != "_DELETE" &&
                                            //    Initialds.Tables[0].Columns[j].ColumnName != "_SELECT")
                                            if (drChangedRow[Initialds.Tables[0].Columns[j].ColumnName] != null)
                                                strChangedValue = drChangedRow[Initialds.Tables[0].Columns[j].ColumnName].ToString();

                                            if (drInitialRow[0][Initialds.Tables[0].Columns[j].ColumnName] != null)
                                                strInitialValue = drInitialRow[0][Initialds.Tables[0].Columns[j].ColumnName].ToString();

                                            if (strChangedValue.Trim() != strInitialValue.Trim())
                                            {
                                                bModified = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bModified = true;
                                    }
                                }
                                catch
                                {
                                    bModified = true;
                                }

                                if (!bModified)
                                {
                                    dsChanged.Tables[0].Rows.Remove(drChangedRow);
                                }
                            }
                            dsReturn = dsChanged;
                        }
                        else
                        {
                            dsReturn = dsChanged.Clone();
                        }
                    }
                    else
                    {
                        dsReturn = dsChanged;
                    }
                }

                return dsReturn;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                return dsReturn;
            }

            return dsReturn;
        }

        public DataSet GetInsertDataSet(Control control, string sKeyField)
        {

            DataSet dsReturn = new DataSet();
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                DataSet ds = (DataSet)bSpread.DataSource;

                string strFilter = sKeyField + "= '1' AND " + sKeyField + "= '1' ";
                DataRow[] drInitialRow = ds.Tables[0].Select(strFilter);

            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                return dsReturn;
            }

            return dsReturn;
        }

        public void PasteData(Control control)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int iRow = 0;
                int iCol = 0;

                IDataObject ido = Clipboard.GetDataObject();
                string sData = (string)ido.GetData("Text");
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\r\\n");
                string[] sDatas = reg.Split(sData);
                iRow = sDatas.Length - 1;
                if (iRow > 0)
                {
                    string[] sCell = sDatas[0].Split('\t');
                    iCol = sCell.Length;
                }

                FarPoint.Win.Spread.Model.CellRange[] cellrange = bSpread.ActiveSheet.GetSelections();

                //if ((iRow == 1) && (iCol == 1))
                if (iCol == 1)
                {
                    if (cellrange[0].Column == -1 || iCol - 1 == bSpread.ActiveSheet.ColumnCount)
                    {

                        iCol -= 1;
                        if (iCol != bSpread.ActiveSheet.ColumnCount)
                            return;
                        bSpread.ActiveSheet.RowCount += iRow;

                        bSpread.ActiveSheet.ActiveRowIndex = bSpread.ActiveSheet.RowCount - iRow;

                        int iActRowIndex = bSpread.ActiveSheet.ActiveRowIndex;
                        int iActColIndex = bSpread.ActiveSheet.ActiveColumnIndex;

                        for (int i = 0; i < iRow; i++)
                        {
                            string[] sCell = sDatas[i].Split('\t');
                            for (int iColCount = 1; iColCount < sCell.Length; iColCount++)
                            {
                                bSpread.ActiveSheet.GetCellType(iActRowIndex + i, iColCount - 1);
                                bSpread.ActiveSheet.SetText(iActRowIndex + i, iColCount - 1, sCell[iColCount]);
                                Console.Write(sCell[iColCount] + "\\t");
                            }
                            Console.WriteLine("");
                        }


                        if (_iAddCol >= 0)
                        {
                            for (int i = 1; i <= iRow; i++)
                            {
                                bSpread.ActiveSheet.Cells[bSpread.ActiveSheet.RowCount - i, _iAddCol].Value = true;

                                for (int iCount = 0; iCount < this._slDefaultValue.Count; iCount++)
                                {
                                    bSpread.ActiveSheet.SetText(bSpread.ActiveSheet.RowCount - i, (int)this._slDefaultValue.GetKey(iCount), (string)this._slDefaultValue.GetByIndex(iCount));
                                }

                            }
                        }
                        bSpread.SetViewportTopRow(0, bSpread.ActiveSheet.RowCount - iRow);
                    }
                    else
                    {
                        int iActRowIndex = bSpread.ActiveSheet.ActiveRowIndex;
                        int iActColIndex = bSpread.ActiveSheet.ActiveColumnIndex;

                        for (int i = 0; i < iRow; i++)
                        {
                            if (bSpread.ActiveSheet.RowCount - 1 >= iActRowIndex + i)
                            {
                                string[] sCell = sDatas[i].Split('\t');
                                for (int iColCount = 0; iColCount < iCol; iColCount++)
                                {
                                    if (iActColIndex + iColCount >= bSpread.ActiveSheet.ColumnCount)
                                        break;

                                    bSpread.ActiveSheet.SetText(iActRowIndex + i, iActColIndex + iColCount, sCell[iColCount]);
                                    //FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(bSpread);
                                    //FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view, iActRowIndex + i, iActColIndex + iColCount);
                                    //bsprData_Change(bSpread, ce);
                                    //Console.Write(sCell[iColCount] + "\\t");
                                }
                            }
                            //Console.WriteLine("");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("한 컬럼 범위에서만 붙여넣기가 가능합니다. 붙여넣기가 취소 되었습니다.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteData(Control control)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int iActRowIndex = bSpread.ActiveSheet.ActiveRowIndex;
                int iActColIndex = bSpread.ActiveSheet.ActiveColumnIndex;
                bSpread.ActiveSheet.Cells[iActRowIndex, iActColIndex].Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprData);
            //FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view, iActRowIndex, iActColIndex);
            //this.bsprData_Change(this.bsprData, ce);
        }

        public ArrayList GetCheckedRowIndex(Control control, int iSelectColIndex)
        {
            ArrayList arReturn = new ArrayList();
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                for (int i = 0; i < bSpread.ActiveSheet.RowCount; i++)
                {
                    string sValue = bSpread.ActiveSheet.Cells[i, iSelectColIndex].Text.ToString().Trim();
                    if (sValue == "1" || sValue.ToUpper() == "TRUE")
                        arReturn.Add(i);
                }
            }
            catch
            {
                return arReturn;
            }

            return arReturn;
        }

        public void MergeRowAndColumn(Control control, int mergeRowCount, int startRowIndex, int endRowIndex, int startColIndex, int mergeMaxColIndex)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            string sPreValue = "";
            int iMergestartRowIndex = startRowIndex;
            for (int i = startRowIndex; i <= endRowIndex; i++)
            {
                string sCurrValue = bSpread.ActiveSheet.Cells[i, startColIndex].Text;

                if (sPreValue != "" && sPreValue != sCurrValue)
                {
                    if (mergeMaxColIndex == startColIndex)
                        return;
                    bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex, i - iMergestartRowIndex, 1);

                    if (mergeRowCount == i - iMergestartRowIndex + 1)
                    {
                        if (sCurrValue == bSpread.ActiveSheet.Cells[i, startColIndex - 1].Text)
                        {
                            int iSpanstartColIndex = this.FindColSpan(bSpread, i, startColIndex - 1);
                            if (iSpanstartColIndex > 0)
                            {
                                bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, iSpanstartColIndex, i - iMergestartRowIndex + 1, startColIndex - iSpanstartColIndex + 1);
                            }
                            else
                                bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex, i - iMergestartRowIndex + 1, 2);
                        }
                    }
                    this.MergeRowAndColumn(bSpread, i - iMergestartRowIndex, iMergestartRowIndex, i - 1, startColIndex + 1, mergeMaxColIndex);
                    sPreValue = sCurrValue;
                    iMergestartRowIndex = i;

                    if (i == endRowIndex)
                    {
                        if (mergeMaxColIndex == startColIndex)
                            return;
                        bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex, i - iMergestartRowIndex + 1, 1);
                        if (mergeRowCount == i - iMergestartRowIndex + 1)
                        {
                            if (sCurrValue == bSpread.ActiveSheet.Cells[i, startColIndex - 1].Text)
                            {
                                int iSpanstartColIndex = this.FindColSpan(bSpread, i, startColIndex - 1);
                                if (iSpanstartColIndex > 0)
                                {
                                    bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, iSpanstartColIndex, i - iMergestartRowIndex + 1, startColIndex - iSpanstartColIndex + 1);
                                }
                                else
                                    bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex - 1, i - iMergestartRowIndex + 1, 2);
                            }
                        }
                        this.MergeRowAndColumn(bSpread, i - iMergestartRowIndex + 1, iMergestartRowIndex, i, startColIndex + 1, mergeMaxColIndex);
                        return;
                    }
                }
                else if (i == endRowIndex)
                {
                    if (mergeMaxColIndex == startColIndex)
                        return;
                    bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex, i - iMergestartRowIndex + 1, 1);
                    if (mergeRowCount == i - iMergestartRowIndex + 1)
                    {
                        if (sCurrValue == bSpread.ActiveSheet.Cells[i, startColIndex - 1].Text)
                        {
                            int iSpanstartColIndex = this.FindColSpan(bSpread, i, startColIndex - 1);
                            if (iSpanstartColIndex > 0)
                            {
                                bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, iSpanstartColIndex, i - iMergestartRowIndex + 1, startColIndex - iSpanstartColIndex + 1);
                            }
                            else
                                bSpread.ActiveSheet.Models.Span.Add(iMergestartRowIndex, startColIndex - 1, i - iMergestartRowIndex + 1, 2);
                        }
                    }
                    this.MergeRowAndColumn(bSpread, i - iMergestartRowIndex + 1, iMergestartRowIndex, i, startColIndex + 1, mergeMaxColIndex);
                    return;
                }
                else
                    sPreValue = sCurrValue;
            }
        }

        private int FindColSpan(Control control, int row, int startCol)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            for (int i = startCol; i >= 0; i--)
            {
                if (bSpread.ActiveSheet.Cells.Get(row, i).ColumnSpan > 1)
                {
                    return i;
                }
            }
            return 0;

        }

        public void SetColWidth(FpSpread objSpread)
        {
            SetColWidth(objSpread, 1);
        }

        public void SetColWidth(FpSpread objSpread, float margin)
        {
            float iWidestColSize;
            int iRowCount = objSpread.ActiveSheet.RowCount;
            int iColCount = objSpread.ActiveSheet.ColumnCount;

            for (int iCol = 0; iCol < iColCount; iCol++)
            {
                iWidestColSize = objSpread.ActiveSheet.GetPreferredColumnWidth(iCol, false);
                if (iCol == 0)
                {
                    objSpread.ActiveSheet.RowHeader.Columns[iCol].Width = Convert.ToInt32(iWidestColSize + margin);
                }
                objSpread.ActiveSheet.Columns[iCol].Width = Convert.ToInt32(iWidestColSize + margin);
            }
        }

        public void SetColWidth(FpSpread objSpread, bool IncludeRowHeader)
        {
            SetColWidth(objSpread, 1, IncludeRowHeader);
        }

        public void SetColWidth(FpSpread objSpread, float margin, bool IncludeRowHeader)
        {
            float iWidestColSize;
            int iRowCount = objSpread.ActiveSheet.RowCount;
            int iColCount = objSpread.ActiveSheet.ColumnCount;

            for (int iCol = 0; iCol < iColCount; iCol++)
            {
                objSpread.ActiveSheet.Columns[iCol].ResetWidth();
                iWidestColSize = objSpread.ActiveSheet.GetPreferredColumnWidth(iCol, false);

                objSpread.ActiveSheet.Columns[iCol].Width = Convert.ToInt32(iWidestColSize + margin);
            }

            if (IncludeRowHeader == true)
            {
                iColCount = objSpread.ActiveSheet.RowHeader.ColumnCount;

                for (int iCol = 0; iCol < iColCount; iCol++)
                {
                    objSpread.ActiveSheet.RowHeader.Columns[iCol].ResetWidth();

                    iWidestColSize = objSpread.ActiveSheet.RowHeader.Columns[iCol].GetPreferredWidth();

                    objSpread.ActiveSheet.RowHeader.Columns[iCol].Width = Convert.ToInt32(iWidestColSize + margin + 5);
                }
            }
        }

        //SPC-748 By Louis Excel Export시 Select Column Name변경
        public string Export(BSpread spread, bool visibleOnly)
        {
            string sResult = string.Empty;

            // Backup And Change Label
            SortedList<int, string> slRollBack = new SortedList<int, string>();
            for (int iCol = 0; iCol < spread.ActiveSheet.Columns.Count; iCol++)
            {
                if (spread.ActiveSheet.ColumnHeader.Cells[0, iCol].CellType is CheckBoxCellType)
                {
                    ICellType cellType = spread.ActiveSheet.ColumnHeader.Cells[0, iCol].CellType;
                    string sCaption = ((CheckBoxCellType)cellType).Caption;
                    string sLabel = spread.ActiveSheet.Columns[iCol].Label;

                    slRollBack.Add(iCol, sLabel);

                    spread.ActiveSheet.Columns[iCol].Label = sCaption;
                }
            }

            // Do Export
            sResult = spread.Export(visibleOnly);

            // RollBack Label
            foreach (int iCol in slRollBack.Keys)
            {
                spread.ActiveSheet.Columns[iCol].Label = slRollBack[iCol];
            }

            return sResult;
        }

        #region : COLOR


        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetErrorColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(153, 153, 153);  //Color.Gray; 
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(153, 153, 153);  //Color.Gray; 
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(153, 153, 153);  //Color.Gray; 
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(153, 153, 153);  //Color.Gray; 
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetDuplicationColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(255, 102, 102); //Color.FromArgb(102, 153, 153); Color.FromArgb(255, 153, 102);
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(255, 102, 102);
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(255, 102, 102);
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(255, 102, 102);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetEditColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.White;
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.White;
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.White;
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.White;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetDisableColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(204, 204, 204);  //Color.Gray; 
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(204, 204, 204);  //Color.Gray; 
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(204, 204, 204);  //Color.Gray; 
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(204, 204, 204);  //Color.Gray; 
                        break;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetWarningColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(255, 255, 153);  //Color.Yellow; 
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(255, 255, 153);  //Color.Yellow; 
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(255, 255, 153);  //Color.Yellow; 
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(255, 255, 153);  //Color.Yellow; 
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetSPECOverColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(255, 204, 102);  //Color.Orange; 
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(255, 204, 102);  //Color.Orange; 
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(255, 204, 102);  //Color.Orange; 
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(255, 204, 102);  //Color.Orange; 
                        break;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetNotExistColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(153, 204, 153);  //Color.Green; 
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(153, 204, 153);  //Color.Green; 
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(153, 204, 153);  //Color.Green; 
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(153, 204, 153);  //Color.Green; 
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetTotalColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            int intRowCount = bSpread.ActiveSheet.RowCount;
            int intColCount = bSpread.ActiveSheet.ColumnCount;

            if (intRowCount > 0 && intColCount > 0)
            {
                switch (iType)
                {
                    case 0: // Cell 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(204, 153, 255);  //Color.Purple; 
                        break;
                    case 1: // Row 
                        bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(204, 153, 255);  //Color.Purple;
                        break;
                    case 2: // Column 
                        bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(204, 153, 255);  //Color.Purple;
                        break;
                    case 3: // Range 
                        bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(204, 153, 255);  //Color.Purple;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetNotPermitColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(153, 204, 255);  //Color.Blue
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(153, 204, 255);  //Color.Blue
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(153, 204, 255);  //Color.Blue 
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(153, 204, 255);  //Color.Blue
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetInsertColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.Yellow;
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.Yellow;
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.Yellow;
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.Yellow;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetModifyColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.Orange;
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.Orange;
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.Orange;
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.Orange;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetDeleteColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.LightGray;  //Color.Gray; 
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.LightGray;  //Color.Gray; 
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.LightGray;  //Color.Gray; 
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.LightGray;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetEditableColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnCount;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(226, 226, 245);
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.Rows[iStartRowIndex].BackColor = Color.FromArgb(226, 226, 245);
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.Columns[iStartColIndex].BackColor = Color.FromArgb(226, 226, 245);
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(226, 226, 245);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the color of a row, column or cell that contains enum error or enum value error. (Gray)
        /// </summary>
        /// <param name="control"></param>
        /// <param name="iStartRowIndex"></param>
        /// <param name="iStartColIndex"></param>
        /// <param name="iEndRowIndex"></param>
        /// <param name="iEndColIndex"></param>
        /// <param name="iType">
        /// iType = 0 : Cell Locked
        /// iType = 1 : Row Locked
        /// iType = 2 : Column Locked
        /// iType = 3 : Range Locked 
        /// </param>
        public void SetEditableHeaderColor(Control control, int iStartRowIndex, int iStartColIndex, int iEndRowIndex, int iEndColIndex, int iType)
        {
            try
            {
                BISTel.PeakPerformance.Client.BISTelControl.BSpread bSpread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

                int intRowCount = bSpread.ActiveSheet.ColumnHeader.RowCount;
                int intColCount = bSpread.ActiveSheet.ColumnHeader.Columns.Count;

                if (intRowCount > 0 && intColCount > 0)
                {
                    switch (iType)
                    {
                        case 0: // Cell 
                            bSpread.ActiveSheet.ColumnHeader.Cells[iStartRowIndex, iStartColIndex].BackColor = Color.FromArgb(194, 191, 236);
                            break;
                        case 1: // Row 
                            bSpread.ActiveSheet.ColumnHeader.Rows[iStartRowIndex].BackColor = Color.FromArgb(194, 191, 236);
                            break;
                        case 2: // Column 
                            bSpread.ActiveSheet.ColumnHeader.Columns[iStartColIndex].BackColor = Color.FromArgb(194, 191, 236);
                            break;
                        case 3: // Range 
                            bSpread.ActiveSheet.ColumnHeader.Cells[iStartRowIndex, iStartColIndex, iEndRowIndex, iEndColIndex].BackColor = Color.FromArgb(194, 191, 236);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region :COMBOBOX CELLTYPE

        /// <summary>
        /// Spead의 해당 Cell을 넘겨받은 Data를 이용하여 Combobox CellType으로 변경해줍니다.
        /// </summary>
        /// <param name="cell">변경할 Cell</param>
        /// <param name="data">Combo의 item data로 string 배열</param>
        /// <param name="isEditable">Combo 수정가능 여부</param>
        public void SetComboBoxCellType(FarPoint.Win.Spread.Cell cell, string[] items, bool isEditable)
        {
            FarPoint.Win.Spread.CellType.ComboBoxCellType celltype = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            if (items != null && items.Length > 0)
            {
                celltype.ItemData = items;
                celltype.Items = items;
                celltype.EditorValue = FarPoint.Win.Spread.CellType.EditorValue.ItemData;
                celltype.Editable = isEditable;

                cell.CellType = celltype;
                cell.Text = items[0];
            }
        }

        /// <summary>
        /// Spead의 해당 Cell을 넘겨받은 Data를 이용하여 Combobox CellType으로 변경해줍니다.
        /// </summary>
        /// <param name="cell">변경할 Cell</param>
        /// <param name="data">Combo의 item data로 string 배열</param>
        /// <param name="selectedText">선택 TEXT</param>
        /// <param name="isEditable">Combo 수정가능 여부</param>
        public void SetComboBoxCellType(FarPoint.Win.Spread.Cell cell, string[] items, string selectedText, bool isEditable)
        {
            FarPoint.Win.Spread.CellType.ComboBoxCellType celltype = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            if (items != null && items.Length > 0)
            {
                celltype.ItemData = items;
                celltype.Items = items;
                celltype.EditorValue = FarPoint.Win.Spread.CellType.EditorValue.ItemData;
                celltype.Editable = isEditable;

                cell.CellType = celltype;

                if (selectedText.Length > 0)
                {
                    bool isExist = false;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items.GetValue(i).ToString() == selectedText)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == true)
                        cell.Text = selectedText;
                    else
                        cell.Text = items[0];
                }
            }
            else
                cell.CellType = celltype;
        }


        /// <summary>
        /// Spead의 해당 Cell을 넘겨받은 Display Data와 Value Data를 이용하여 Combobox CellType으로 변경해줍니다.
        /// </summary>
        /// <param name="cell">변경할 Cell</param>
        /// <param name="itemData">Value Data</param>
        /// <param name="items">Display Data</param>
        public void SetComboBoxCellTypeByValue(FarPoint.Win.Spread.Cell cell, string[] itemData, string[] items)
        {
            //FarPoint.Win.Spread.CellType.ComboBoxCellType celltype = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            //if (items.Length > 0)
            //{
            //    celltype.ItemData = itemData;
            //    celltype.Items = items;
            //    celltype.EditorValue = FarPoint.Win.Spread.CellType.EditorValue.ItemData;

            //    cell.CellType = celltype;
            //}
            //else
            //    cell.CellType = celltype;

            SetComboBoxCellTypeByValue(cell, itemData, items, (string)cell.Value);
        }


        /// <summary>
        /// Spead의 해당 Cell을 넘겨받은 Display Data와 Value Data를 이용하여 Combobox CellType으로 변경해줍니다.
        /// </summary>
        /// <param name="cell">변경할 Cell</param>
        /// <param name="itemData">Value Data</param>
        /// <param name="items">Display Data</param>
        /// <param name="selectedValue">선택 Value</param>
        public void SetComboBoxCellTypeByValue(FarPoint.Win.Spread.Cell cell, string[] itemData, string[] items, string selectedValue)
        {
            FarPoint.Win.Spread.CellType.ComboBoxCellType celltype = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            cell.Value = null;

            if (items.Length > 0)
            {
                celltype.ItemData = itemData;
                celltype.Items = items;
                celltype.EditorValue = FarPoint.Win.Spread.CellType.EditorValue.ItemData;

                cell.CellType = celltype;

                if (selectedValue != null && selectedValue.Length > 0)
                {
                    bool isExist = false;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (itemData.GetValue(i).ToString() == selectedValue)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (isExist == true)
                        cell.Value = selectedValue;
                    else
                        cell.Value = itemData[0];
                }
                else
                    cell.Value = itemData[0];
            }
            else
                cell.CellType = celltype;
        }

        public int[] GetVisibleRowIndex(FpSpread spread)
        {
            List<int> lstVisibleRowIdx = new List<int>();

            if (spread.ActiveSheet.RowFilter != null)
            {
                int[] iFilteredRowIdxs = spread.ActiveSheet.RowFilter.GetIntersectedFilteredInRows();

                if (iFilteredRowIdxs != null && iFilteredRowIdxs.Length > 0)
                {
                    foreach (int rowIdx in iFilteredRowIdxs)
                    {
                        if (spread.ActiveSheet.Rows[rowIdx].Visible)
                            lstVisibleRowIdx.Add(rowIdx);
                    }

                    return lstVisibleRowIdx.ToArray();
                }
            }

            for (int rowIdx = 0; rowIdx < spread.ActiveSheet.RowCount; rowIdx++)
            {
                if (spread.ActiveSheet.Rows[rowIdx].Visible)
                    lstVisibleRowIdx.Add(rowIdx);
            }

            return lstVisibleRowIdx.ToArray();
        }


        public void SetComboBoxCellTypeForColumn(FarPoint.Win.Spread.Column col, string[] itemData)
        {
            FarPoint.Win.Spread.CellType.ComboBoxCellType cellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            cellType.Items = itemData;
            col.CellType = cellType;
        }

        public void SetNumberCellTypeForColumn(FarPoint.Win.Spread.Column col)
        {
            FarPoint.Win.Spread.CellType.NumberCellType cellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            cellType.FixedPoint = false;    //소수점 표시여부
            col.CellType = cellType;
        }

        #endregion
    }
}
