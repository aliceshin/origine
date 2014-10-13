using System;
using System.Collections.Generic;
using System.Text;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.History
{
    public class SPCModelUCControllerForHistory : Common.SPCModelUCController
    {
        public override string[] GetSPCModelBasicColumnNames()
        {
            return new string[4]
            {
                BISTel.eSPC.Common.COLUMN.CHART_ID,
                BISTel.eSPC.Common.COLUMN.MAIN_YN,
                BISTel.eSPC.Common.COLUMN.VERSION,
                "MODE"
            }
            ;
        }

        public override void AddRowDataFromConfigRow(System.Data.DataRow configRow, System.Data.DataRow newRow, Dictionary<string, string> codeData)
        {
            base.AddRowDataFromConfigRow(configRow, newRow, codeData);

            newRow[BISTel.eSPC.Common.COLUMN.VERSION] = configRow[BISTel.eSPC.Common.COLUMN.VERSION].ToString();
        }
    }
}
