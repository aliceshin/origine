using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace BISTel.eSPC.Page.ATT.Common.Popup
{
    public partial class ChartFilteredDataPopup : ChartDataPopup
    {
        private DataSet filteredParam = null;
        private DataSet filteredRaw = null;

        public ChartFilteredDataPopup()
        {
            InitializeComponent();
        }

        public override void InitializeBSpread()
        {
            RemoveSuperfluousColumns(this.filteredParam);

            this.bsprData.ActiveSheet.RowCount = 0;
            this.bsprData.ActiveSheet.ColumnCount = 0;

            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.Locked = true;
            this.bsprData.AutoGenerateColumns = true;

            this.bsprData.DataSource = this.filteredParam;

            foreach(DataTable dt in filteredParam.Tables)
            {
                SheetView sheet = new SheetView();
                sheet.Reset();
                sheet.SheetName = dt.TableName;
                this.bsprData.Sheets.Add(sheet);
                sheet.DataSource = dt;

                for (int cIdx = 0; cIdx < sheet.Columns.Count; cIdx++)
                {
                    sheet.ColumnHeader.Cells[0, cIdx].Text = dt.Columns[cIdx].ToString();
                }
            }

            this.bsprData_Sheet1.Visible = false;

            UpdateRowColor(this.bsprData);

            if (this.isVisibleRawData)
            {
                RemoveSuperfluousColumns(this.filteredRaw);

                this.bsprRawData.ActiveSheet.RowCount = 0;
                this.bsprRawData.ActiveSheet.ColumnCount = 0;

                this.bsprRawData.ClearHead();
                this.bsprRawData.AddHeadComplete();
                this.bsprRawData.UseSpreadEdit = false;
                this.bsprRawData.Locked = true;
                this.bsprRawData.AutoGenerateColumns = true;

                this.bsprRawData.DataSource = filteredRaw;

                foreach(DataTable dt in filteredRaw.Tables)
                {
                    SheetView sheet = new SheetView();
                    sheet.Reset();
                    sheet.SheetName = dt.TableName;
                    this.bsprRawData.Sheets.Add(sheet);
                    sheet.DataSource = dt;

                    for (int cIdx = 0; cIdx < sheet.Columns.Count; cIdx++)
                    {
                        sheet.ColumnHeader.Cells[0, cIdx].Text = dt.Columns[cIdx].ToString();
                    }
                }

                this.bsprRawData_Sheet1.Visible = false;

                UpdateRowColor(this.bsprRawData);
            }
        }

        protected override void Export()
        {
            DataSet ds = null;
            if(this.bTabControl1.SelectedIndex == 0)
            {
                ds = this.filteredParam;
            }
            else
            {
                ds = this.filteredRaw;
            }

            SaveFileDialog openDlg = new SaveFileDialog();
            openDlg.Filter = "Excel Files (*.xls)|*.xls";
            openDlg.FileName = "";
            openDlg.DefaultExt = ".xls";
            openDlg.CheckFileExists = false;
            openDlg.CheckPathExists = true;

            DialogResult res = openDlg.ShowDialog();

            if (res != DialogResult.OK)
            {
                return;
            }

            using(ExcelWriter writer = new ExcelWriter(openDlg.FileName))
            {
                writer.WriteStartDocument();
                
                foreach(DataTable dt in ds.Tables)
                {
                    writer.WriteSheet(dt.TableName, dt);
                }

                writer.WriteEndDocument();
            }
        }

        public override void AddParamData(object paramData)
        {
            filteredParam = (DataSet) paramData;
        }

        public override void AddRawData(object rawData)
        {
            filteredRaw = (DataSet) rawData;
        }
    }
}
