using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Compare
{
    public partial class ColumnFiltering : BasePageUCtrl
    {
        private DataTable dtData = null;
        private string columnName = string.Empty;
        public bool ValueSorting = true;
        private static string ALL = "ALL";
        private static string NOTEXIST = "NOT EXIST";

        public string ConditionQuery
        {
            get
            {
                if(this.chlbValues== null
                    || this.chlbValues.GetItemChecked(0))
                {
                    return "(1 = 1)";
                }

                string[] checkedValues = GetCheckedItemStrings();
                if(checkedValues.Length == 0)
                    return "(1 > 1)";

                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                foreach(string s in checkedValues)
                {
                    if(sb.Length > 1)
                        sb.Append(" or ");

                    if(s == NOTEXIST)
                    {
                        sb.Append(columnName + " is null");
                    }
                    else
                    {
                        sb.Append(columnName + " = '" + s + "'");
                    }
                }
                sb.Append(")");

                return sb.ToString();
            }
        }

        public string ColumnName
        {
            get { return columnName; }
        }

        public ColumnFiltering()
        {
            InitializeComponent();
        }

        public ColumnFiltering(DataTable dtData, string columnName)
        {
            InitializeComponent();

            this.dtData = dtData;
            this.columnName = columnName;

            Initialize();

            AddValuesToCheckedListBox();
        }

        private void Initialize()
        {
            this.btxtFiltering.Text = "";
            this.groupBox1.Text = columnName;
            this.chlbValues.Items.Clear();
        }

        private void AddValuesToCheckedListBox()
        {
            this.chlbValues.Items.Clear();

            List<string> values = new List<string>();
            foreach (DataRow dr in dtData.Rows)
            {
                string value = dr[columnName].ToString().ToUpper();
                if (!values.Contains(value) && !string.IsNullOrEmpty(value))
                    values.Add(value);
            }

            if (ValueSorting)
                values.Sort();

            chlbValues.Items.Add(ALL, true);
            chlbValues.Items.AddRange(values.ToArray());
            chlbValues.Items.Add(NOTEXIST);
        }

        public void chlbValues_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.chlbValues.ItemCheck -= chlbValues_ItemCheck;
            // if 'All' is checked
            if (e.Index == 0 && e.NewValue == CheckState.Checked)
            {
                for (int i = 1; i < this.chlbValues.Items.Count; i++)
                {
                    this.chlbValues.SetItemChecked(i, false);
                }
            }
            else
            {
                this.chlbValues.SetItemChecked(0, false);
            }

            this.chlbValues.ItemCheck += chlbValues_ItemCheck;
        }

        private string[] GetCheckedItemStrings()
        {
            List<string> checkedItems = new List<string>();
            for (int i = 0; i < this.chlbValues.Items.Count; i++)
            {
                if(chlbValues.GetItemChecked(i))
                {
                    checkedItems.Add(chlbValues.Items[i].ToString());
                }
            }

            return checkedItems.ToArray();
        }

        public void btxtFiltering_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                string text = this.btxtFiltering.Text.ToUpper();

                Dictionary<string, bool> allAndNotExsit = new Dictionary<string, bool>();
                allAndNotExsit.Add(ALL, chlbValues.GetItemChecked(0));
                allAndNotExsit.Add(NOTEXIST, chlbValues.GetItemChecked(chlbValues.Items.Count - 1));
                chlbValues.Items.RemoveAt(0);
                chlbValues.Items.RemoveAt(chlbValues.Items.Count - 1);

                SortedDictionary<string, bool> containingItem = new SortedDictionary<string, bool>();
                SortedDictionary<string, bool> notContainingItem = new SortedDictionary<string, bool>();

                for(int i=this.chlbValues.Items.Count - 1; i>=0; i--)
                {
                    if(chlbValues.Items[i].ToString().ToUpper().Contains(text))
                    {
                        containingItem.Add(chlbValues.Items[i].ToString(), chlbValues.GetItemChecked(i));
                    }
                    else
                    {
                        notContainingItem.Add(chlbValues.Items[i].ToString(), chlbValues.GetItemChecked(i));
                    }
                    chlbValues.Items.RemoveAt(i);
                }

                chlbValues.Items.Add(ALL, allAndNotExsit[ALL]);
                foreach(var kvp in containingItem)
                {
                    chlbValues.Items.Add(kvp.Key, kvp.Value);
                }
                foreach(var kvp in notContainingItem)
                {
                    chlbValues.Items.Add(kvp.Key, kvp.Value);
                }
                chlbValues.Items.Add(NOTEXIST, allAndNotExsit[NOTEXIST]);

            }
        }
    }
}
