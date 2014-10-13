using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Tool
{
    public partial class ExportResult : BasePageUCtrl
    {
        private Initialization _initialization = null;
        private int indexOfSelect = 0;
        private int indexOfFileFullPath = 0;

        public ExportResult()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(BISTel.PeakPerformance.Client.CommonLibrary.LinkedList llCondition)
        {
            InitializePage();
        }

        public void InitializePage(string functionName, string pageKey, SessionData session)
        {
            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.sessionData = session;

            this.InitializePage();
        }

        private void InitializePage()
        {
            InitializeVariables();

            InitializeSpread();

            this._initialization.InitializeButtonList(this.bbtnList, ref this.bSpread1, this.KeyOfPage, 
                "BTNLIST_SPC_EXPORT_RESULT", this.sessionData);

            this.btxtSavedPath.Text = string.Empty;
            this.btxtSavedPath.Enabled = false;
        }

        private void InitializeVariables()
        {
            if(this._initialization == null)
            {
                this._initialization = new Initialization();
                this._initialization.InitializePath();
            }
        }

        private void InitializeSpread()
        {
            this.bSpread1.ClearHead();
            this.bSpread1.AddHeadComplete();
            this.bSpread1.UseSpreadEdit = false;
        }

        public void ExportStarted(object sender, ExportStartedEventHandlerArgs e)
        {
            InitializeSpread();

            btxtSavedPath.Text = e.folderPath;
        }

        public void ModelExported(object sender, ModelExportCompletedEventHandlerArgs e)
        {
            if(bSpread1.ActiveSheet.Columns.Count == 0)
            {
                this.bSpread1.AddHead(0, "SELECT", "SELECT", 50, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
                this.indexOfSelect = 0;
                this.bSpread1.AddHead(1, "SPC MODEL NAME", "SPC MODEL NAME", 150, 100, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(2, "TOTAL MODEL COUNT(MAIN_SUB)", "TOTAL MODEL COUNT(MAIN_SUB)", 200, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(3, "FILE NAME", "FILE NAME", 250, 100, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(4, "FILE FULL PATH", "FILE FULL PATH", 250, 500, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, false);
                this.indexOfFileFullPath = 4;
                this.bSpread1.AddHeadComplete();
                
                this.bSpread1.ActiveSheet.Columns[0].Locked = false;
            }

            int idxRow = bSpread1.ActiveSheet.Rows.Count;
            bSpread1.ActiveSheet.AddRows(idxRow, 1);
            bSpread1.ActiveSheet.Cells[idxRow, 1].Text = e.SPCModelName;
            bSpread1.ActiveSheet.Cells[idxRow, 2].Text = e.exportedModelNum.ToString();
            string[] path = e.filePath.Split('\\');
            bSpread1.ActiveSheet.Cells[idxRow, 3].Text = path[path.Length-1];
            bSpread1.ActiveSheet.Cells[idxRow, 4].Text = e.filePath;
            if(string.IsNullOrEmpty(e.filePath))
                bSpread1.ActiveSheet.Rows[idxRow].Locked = true;
        }

        void bbtnList_ButtonClick(string name)
        {
            if (bSpread1.ActiveSheet.Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EXPORT_DATA_FIRST", null, null);
                return;
            }           

            if (name == "Open")
            {
                OpenExcelFile();
            }
        }

        void OpenExcelFile()
        {
            //Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Applications\EXCEL.EXE");

            //if (key == null)
            //{
            //    MSGHandler.DisplayMessage(MSGType.Error, "You need microsoft office to read this file.");
            //    return;
            //}
            int selectitemCount = 0;
            for (int i = 0; i < this.bSpread1.ActiveSheet.Rows.Count; i++)
            {
                if(this.bSpread1.GetCellText(i, indexOfSelect) == "True"
                    && this.bSpread1.GetCellText(i, indexOfFileFullPath) != string.Empty)
                {
                    selectitemCount++;
                    
                    //SPC-1289, KBLEE, Start
                    string filePath = this.bSpread1.GetCellText(i, indexOfFileFullPath);

                    if (File.Exists(filePath))
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = filePath;
                        process.Start();
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_WARNING_NO_FILE", new string[] { filePath }, null);
                    }
                    //SPC-1289, KBLEE, End
                }
                else if (this.bSpread1.GetCellText(i, indexOfSelect) == "True"
                    && this.bSpread1.GetCellText(i, indexOfFileFullPath) == string.Empty)
                {
                    selectitemCount++;
                    string modelName = this.bSpread1.GetCellText(i,1);
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_HAS_NO_DATA", new string[]{modelName}, null);
                }
                
            }

            if (selectitemCount == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_A_ROW", null, null);
            }

        }

       
    }
}
