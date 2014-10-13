using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

namespace BISTel.eSPC.Page.Common
{
    public partial class CheckComboControl : UserControl
    {
        #region : Field

        private DataTable _dtComboValue = null;
        LinkedList _llsResult = null;
        LinkedList _llsDefaultChart = null;

        #endregion

        #region : Initialization



        public CheckComboControl()
        {
            InitializeComponent();
        }

        #endregion

        #region : Editor

        [Editor(typeof(string), typeof(string))]
        public string TitleText
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        #endregion

        #region : Override

        public void ResetValue()
        {
            cboValue.DataSource = null;
            cboValue.Items.Clear();
            cboValue.Text = string.Empty;
        }


        public void SetComboValue(object[] listValue, object[] listDisplay)
        {
            if (cboValue.SelectedValue != string.Empty) return;
            //if (cboValue.Text != string.Empty) return;
            ResetValue();
            //foreach (string text in list)
            //    cboValue.Items.Add(text);

            if (ComboValueTable == null)
            {
                ComboValueTable = DCUtil.MakeDataTableForDCValue((string[])listValue, (string[])listDisplay);
            }

            cboValue.DataSource = ComboValueTable;
            cboValue.DisplayMember = Definition.CONDITION_SEARCH_KEY_DISPLAYDATA;
            cboValue.ValueMember = Definition.CONDITION_SEARCH_KEY_VALUEDATA;
            InitializeValue();

        }

        public void InitializeValue()
        {
            for (int k = 0; k < cboValue.Items.Count; k++)
            {
                string sValue = cboValue.chkBox.Items[k].ToString();

                for (int j = 0; j < llstDefault.Count; j++)
                {
                    string strKey = llstDefault.GetKey(j).ToString();
                    if (strKey.Equals(sValue))
                    {
                        cboValue.chkBox.SetItemChecked(k, true);
                        break;
                    }
                }
            }

            MakeChartLinkedList(GetSelectedValue());

        }

        public void EnableCombo(bool enable)
        {
            cboValue.Enabled = enable;
        }

        public DataTable GetSelectedValue()
        {
            ArrayList valueList = new ArrayList();

            string value = cboValue.SelectedValue;
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

            if (valueList.Count == 0) return null;

            DataTable dt = null;
            if (ComboValueTable != null)
            {
                dt = ComboValueTable.Clone();
                for (int i = 0; i < ComboValueTable.Rows.Count; i++)
                {
                    string rawid = ComboValueTable.Rows[i][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                    if (valueList.Contains(rawid))
                        dt.ImportRow(ComboValueTable.Rows[i]);
                }
            }

            //return valueList;
            return dt;
        }

        public void SetTitleText(string text)
        {
            this.lblTitle.Text = text;
        }

        public void SetComboText(string text)
        {
            this.cboValue.Text = text;
        }


        private void MakeChartLinkedList(DataTable dt)
        {
            _llsResult = new LinkedList();
            foreach (DataRow dr in dt.Rows)
            {
                _llsResult.Add(dr[Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString(), dr[Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString());
            }
        }
        #endregion

        #region : Event handling

        private void cboValue_CheckCombo_Select(object sender, EventArgs e)
        {
            string str = cboValue.Text;
            MakeChartLinkedList(GetSelectedValue());
        }

        private void cboValue_TextChanged(object sender, EventArgs e)
        {
            string str = cboValue.Text;
            MakeChartLinkedList(GetSelectedValue());
        }

        #endregion



        #region : Public
        public DataTable ComboValueTable
        {
            set
            {
                _dtComboValue = value;
            }
            get
            {
                return _dtComboValue;
            }
        }

        public LinkedList llsResult
        {
            set
            {
                _llsResult = value;
            }
            get
            {
                return _llsResult;
            }
        }


        public LinkedList llstDefault
        {

            set
            {
                _llsDefaultChart = value;
            }
            get
            {
                return _llsDefaultChart;
            }
        }
        #endregion
    }
}
