using System;
using System.Collections.Generic;
using System.Text;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.History
{
    public class SPCModelUCControllerForHistory : Common.SPCModelUCController
    {
        public override string[] GetSPCModelBasicColumnNames()
        {
            return new string[4]
            {
                COLUMN.CHART_ID,
                COLUMN.MAIN_YN,
                COLUMN.VERSION,
                "MODE"
            }
            ;
        }

        public override void AddRowDataFromConfigRow(System.Data.DataRow configRow, System.Data.DataRow newRow, Dictionary<string, string> codeData)
        {
            base.AddRowDataFromConfigRow(configRow, newRow, codeData);

            newRow[COLUMN.VERSION] = configRow[COLUMN.VERSION].ToString();
        }
    }
}
