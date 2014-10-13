using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Page.ATT.Modeling;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.ATT.Compare
{
    public interface ICompareResultController
    {
        DataTable GetComparedDataTable(LinkedList llCondition);

        //SPC-632
        LinkedList Paste(SPCCopySpec popup, string targetChartID, string mainYN, string userRawID);

        string PasteModel(LinkedList pasteModelList);

        DialogResult Modify(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId, string mainYN, string groupName);

        DialogResult ViewModel(SessionData sessionData, string URL, string Port, string line, string area, string eqpModel, string chartId,
                               string mainYN, string version, string groupName);

        //void ViewChart(SessionData sessionData, string URL, string line, string area, string spcModelName, string paramAlias, string chartId,
        //               string paramType);

        int GetRowIndex(string name);

        //SPC-855

        DataSet GetATTTargetConfigSpecData(string[] targetRawid);
        DataSet GetATTSourseConfigSpecData(string sourceRawid);
        LinkedList GetGroupNameByChartID(string chartId); //SPC-1292, KBLEE
    }
}
