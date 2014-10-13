using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;
using BISTel.PeakPerformance.Statistics.Application.Common;

using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Common
{
    
    public class CommonSPCStat : BISTel.eSPC.Page.Common.CommonSPCStat
    {
        new public List<double> AddDataList(DataTable dt)
        {
            List<double> listRawData = new List<double>();            
            if(!DataUtil.IsNullOrEmptyDataTable(dt)
                && dt.Columns.Contains(Definition.CHART_COLUMN.C))
            {            
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string strRaw = dt.Rows[j][Definition.CHART_COLUMN.C].ToString();
                    string[] arr = strRaw.Split('@');
                    string[] arrRaw = arr[0].Split(';');
                    for (int k = 0; k < arrRaw.Length; k++)
                    {
                        if (string.IsNullOrEmpty(arrRaw[k].Trim())) continue;
                        listRawData.Add(double.Parse(arrRaw[k]));
                    }
                }
            }            
            return listRawData;
        }
    }
}
