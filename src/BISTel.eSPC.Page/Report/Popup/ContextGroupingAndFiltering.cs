using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;


using BISTel.eSPC.Common;
using System.Collections;

namespace BISTel.eSPC.Page.Report
{
    public partial class ContextGroupingAndFiltering : BasePageUCtrl
    {
        private string contextName = string.Empty;
        SortedList alSelect;

        public string ContextName
        {
            get { return contextName; }
        }

        public bool IsGroupingSelected
        {
            get { return bchkbGroup.Checked; }
            set { bchkbGroup.Checked = value; }
        }

        public string[] SelectedValues
        {
            get 
            {
                List<string> values = new List<string>();
                ArrayList alContextValue = bsprContextValue.GetCheckedList(0);
                if (alContextValue.Count > 0)
                {
                    for (int i = 0; i < alContextValue.Count; i++)
                    {
                       values.Add(bsprContextValue.ActiveSheet.GetText((int)alContextValue[i],1));
                    }
                }
                //foreach (var checkedItem in this.chlbValues.CheckedItems)
                //{
                //    values.Add(checkedItem.ToString());
                //}

                return values.ToArray();
            }
            set
            {
                string[] values = value;
                string strContextvalue = string.Empty;

                ArrayList alContextValue = bsprContextValue.GetCheckedList(0);

                for (int i = 0; i < bsprContextValue.RowCount(); i++)
                {
                    if (values.Length < 1)
                    {
                        bsprContextValue.ActiveSheet.SetValue(i, 0, false);
                    }
                    else
                    {
                        foreach (string str in values)
                        {
                            if (bsprContextValue.ActiveSheet.GetText(i, 1).Equals(str))
                            {
                                bsprContextValue.ActiveSheet.SetValue(i, 0, true);
                                break;
                            }
                            else
                                bsprContextValue.ActiveSheet.SetValue(i, 0, false);
                        }
                    }
                }
            }
        }

        private ContextGroupingAndFiltering()
        {
            InitializeComponent();
        }


        //SPC-697 By Louis -> Filter Popup CheckedList에서 Spread로 변경 Binding
        public ContextGroupingAndFiltering(DataTable dataTable, string contextName) : this()
        {
            this.groupBox1.Text = contextName;
            this.contextName = contextName;

            var values = new List<string>();


            DataTable dtContexValue = new DataTable();
            dtContexValue.Columns.Add(COLUMN.SELECT, typeof(Boolean));
            dtContexValue.Columns.Add(COLUMN.CONTEXT_VALUE);
               

            foreach(DataRow dr in dataTable.Rows)
            {
                string value = dr[contextName].ToString();
                if(value.Contains(";"))
                {
                    value = value.Split(';')[0];
                }

                if (!values.Contains(value))
                {
                    values.Add(value);
                    DataRow drContext = dtContexValue.NewRow();
                    drContext[COLUMN.CONTEXT_VALUE] = value;
                    dtContexValue.Rows.Add(drContext);
                }
            }

            dtContexValue.AcceptChanges();
            //bsprContextValue.AutoGenerateColumns = true;
            
            this.bsprContextValue.AddHead(0, "SELECT", "SELECT", 30, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
            switch (contextName)
            {
                case "EQP_ID":
                    this.bsprContextValue.AddHead(1, "EQP_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
                case "RECIPE_ID":
                    this.bsprContextValue.AddHead(1, "RECIPE_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
                case "PRODUCT_ID":
                    this.bsprContextValue.AddHead(1, "PRODUCT_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
                case "OPERATION_ID":
                    this.bsprContextValue.AddHead(1, "OPERATION_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
                case "MODULE_ID":
                    this.bsprContextValue.AddHead(1, "MODULE_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
                case "STEP_ID":
                    this.bsprContextValue.AddHead(1, "STEP_ID", "CONTEXT_VALUE", 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    break;
            }
            
            this.bsprContextValue.AddHeadComplete();
            
            bsprContextValue.DataSet = dtContexValue;
            bsprContextValue.UseGeneralContextMenu = false;
            //bsprContextValue.ActiveSheet.RowHeaderVisible = false;
            //bsprContextValue.ActiveSheet.ColumnHeaderVisible = false;
            bsprContextValue.ActiveSheet.Columns[0].Locked = false;
            bsprContextValue.ActiveSheet.Columns[0].Width = bsprContextValue.ActiveSheet.GetPreferredColumnWidth(0, false);
            bsprContextValue.ActiveSheet.Columns[1].Width = bsprContextValue.ActiveSheet.GetPreferredColumnWidth(1, false);
            
            bsprContextValue.Enabled = false;
            bsprContextValue.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            bsprContextValue.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            //values.Sort();
           // chlbValues.Items.AddRange(values.ToArray());
        }

        private void bchkbGroup_CheckedChanged(object sender, EventArgs e)
        {
            BCheckBox box = (BCheckBox) sender;
            if(box.Checked)
            {
               // btxtFiltering.Enabled = true;
                bsprContextValue.Enabled = true;
            }
            else
            {
               // btxtFiltering.Enabled = false;
                bsprContextValue.Enabled = false;
            }
        }

        private void btxtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                //string text = this.btxtFiltering.Text.ToUpper();

                //SortedDictionary<string, bool> matchedItem = new SortedDictionary<string, bool>();
                //SortedDictionary<string, bool> notMatchedItem = new SortedDictionary<string, bool>();

                //for(int i=this.chlbValues.Items.Count - 1; i>=0; i--)
                //{
                //    if(chlbValues.Items[i].ToString().ToUpper().Contains(text))
                //    {
                //        matchedItem.Add(chlbValues.Items[i].ToString(), chlbValues.GetItemChecked(i));
                //    }
                //    else
                //    {
                //        notMatchedItem.Add(chlbValues.Items[i].ToString(), chlbValues.GetItemChecked(i));
                //    }
                //    chlbValues.Items.RemoveAt(i);
                //}

                //foreach(var kvp in matchedItem)
                //{
                //    chlbValues.Items.Add(kvp.Key, kvp.Value);
                //}
                //foreach(var kvp in notMatchedItem)
                //{
                //    chlbValues.Items.Add(kvp.Key, kvp.Value);
                //}
            }
        }
        //SPC-697 by Louis -> Multi select
        private void bsprContextValue_MouseDown(object sender, MouseEventArgs e)
        {
            alSelect = this.bsprContextValue.GetSelectedRows();
        }

        private void bsprContextValue_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (alSelect.Count > 0)
            {
                if (!(bool)this.bsprContextValue.GetCellValue(e.Row, 0))
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprContextValue.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "False");
                    }
                }
                else
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprContextValue.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "True");
                    }
                }
            }
        }
        //////////////////////////////////////
    }
}
