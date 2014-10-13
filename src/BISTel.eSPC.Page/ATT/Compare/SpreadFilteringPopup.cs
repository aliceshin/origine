using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.ATT.Compare
{
    public partial class SpreadFilteringPopup : BasePopupFrm
    {
        public string ConditionQuery
        {
            get
            {
                if (flplColumns == null)
                    return "1 = 1";

                StringBuilder sb = new StringBuilder();
                foreach (Control con in flplColumns.Controls)
                {
                    if(con is ColumnFiltering)
                    {
                        ColumnFiltering colFiltering = (ColumnFiltering) con;
                        if(sb.Length > 0)
                        {
                            sb.Append(" AND ");
                        }
                        sb.Append(colFiltering.ConditionQuery);
                    }
                }

                return sb.ToString();
            }
        }

        public SpreadFilteringPopup()
        {
            InitializeComponent();
        }

        public SpreadFilteringPopup(DataTable dataTable, string[] columnNames)
        {
            InitializeComponent();

            List<ColumnFiltering> columnFilterings = new List<ColumnFiltering>();
            for (int i = 0; i < columnNames.Length; i++)
            {
                ColumnFiltering columnFiltering = new ColumnFiltering(dataTable, columnNames[i]);
                columnFilterings.Add(columnFiltering);

                this.flplColumns.Controls.Add(columnFiltering);                
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
