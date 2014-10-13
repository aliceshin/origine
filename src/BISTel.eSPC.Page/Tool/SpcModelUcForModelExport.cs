using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Tool
{
    public partial class SpcModelUcForModelExport : SPCModelUC2
    {
        public SpcModelUcForModelExport()
        {
            InitializeComponent();
        }

        public void ExportStarted(object sender, ExportStartedEventHandlerArgs e)
        {
            for(int i=0; i<bSpread.ActiveSheet.Rows.Count; i++)
            {
                bSpread.ActiveSheet.Rows[i].ResetBackColor();
            }
        }

        public void SubModelExported(object sender, SubModelExportCompletedEventHandlerArgs e)
        {
            int indexOfRawid = GetIndexOfColumn("CHART_ID");
            if(indexOfRawid < 0)
                return;
           
            for(int i=0; i< bSpread.ActiveSheet.Rows.Count; i++)
            {
                if(bSpread.ActiveSheet.Cells[i, indexOfRawid].Text == e.SubModelRawID)
                {
                    bSpread.ActiveSheet.Rows[i].BackColor = Color.LightGreen;
                }
            }
        }

        private int GetIndexOfColumn(string columnName)
        {
            for(int i=0; i < bSpread.ActiveSheet.Columns.Count; i++)
            {
                if(bSpread.ActiveSheet.Columns.Get(i).Label.ToUpper() == columnName.ToUpper())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
