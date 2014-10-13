using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using Microsoft.Win32;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class ChartDataPopup : BasePopupFrm
    {

        #region : Constructor
        public ChartDataPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field
        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _ws;
        DataTable _dtParam;
        DataTable _dtRaw;
        SessionData _SessionData;

        public bool isVisibleRawData = false;
        
        MenuItem fiterHide;
        MenuItem freeze;
        MenuItem unfreeze;
        MenuItem filter;
        SpreadFilter _sfilter;
        private bool filterClicked = false;
            
        SpreadFilter _Rawsfilter;
     
        
        #endregion


        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeVariable();
            this.InitializeLayout();
            this.InitializeDataButton();
            this.InitializeBSpread();
        }

        public void InitializeVariable()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._bspreadutility = new BSpreadUtility();
            this._ComUtil = new CommonUtility();
        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_CHART_DATA_VIEW);
            if (!isVisibleRawData)
            {
                this.bTabControl1.Controls.RemoveAt(1);
            }
           
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_CHART_POPUP, Definition.BUTTONLIST_KEY_CHART_DATA, this.SessionData);

            this.bsprRawData.ContextMenu = this.bbtnList.ContextMenu;
        }

        public virtual void InitializeBSpread()
        {
            RemoveSuperfluousColumns(this._dtParam);

            this.bsprData.ActiveSheet.RowCount = 0;
            this.bsprData.ActiveSheet.ColumnCount = 0;

            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.Locked = true;
            this.bsprData.AutoGenerateColumns = true;
            this.bsprData.UseGeneralContextMenu = false;
            this.bsprData.DataSource = this._dtParam;

            for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
            {
                this.bsprData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = this._dtParam.Columns[cIdx].ToString();
            }

            UpdateRowColor(this.bsprData);

            //ContextMenu추가
            _sfilter = new SpreadFilter(this.bsprData);
            
            filter = new MenuItem("Filter");
            filter.Click += new EventHandler(filter_Click);
            this.bsprData.ContextMenu.MenuItems.Add(filter);

            freeze = new MenuItem("Freeze");
            freeze.Click += new EventHandler(freeze_Click);
            this.bsprData.ContextMenu.MenuItems.Add(freeze);


            if (this.isVisibleRawData)
            {
                RemoveSuperfluousColumns(this._dtRaw);

                this.bsprRawData.ActiveSheet.RowCount = 0;
                this.bsprRawData.ActiveSheet.ColumnCount = 0;

                this.bsprRawData.ClearHead();
                this.bsprRawData.AddHeadComplete();
                this.bsprRawData.UseSpreadEdit = false;
                this.bsprRawData.Locked = true;
                this.bsprRawData.AutoGenerateColumns = true;
                // ContextMenu 삭제
                this.bsprRawData.UseGeneralContextMenu = false;

                this.bsprRawData.DataSource = this._dtRaw;

                for (int cIdx = 0; cIdx < this.bsprRawData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprRawData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = this._dtRaw.Columns[cIdx].ToString();
                }

                _Rawsfilter = new SpreadFilter(this.bsprRawData);

                UpdateRowColor(this.bsprRawData);
            }
        }

        protected void UpdateRowColor(BSpread spread)
        {
            for(int j=0; j<spread.Sheets.Count; j++)
            {
                int columnToggleYNIndex = -1;

                for (int i = 0; i < spread.Sheets[j].Columns.Count; i++)
                {
                    Column col = spread.Sheets[j].Columns.Get(i);
                    if (col.Label == Definition.COL_TOGGLE_YN)
                    {
                        columnToggleYNIndex = i;
                        break;
                    }
                }

                if(columnToggleYNIndex == -1)
                continue;

                for (int i = 0; i < spread.Sheets[j].Rows.Count; i++)
                {
                    if(spread.Sheets[j].Cells[i, columnToggleYNIndex].Text == "Y")
                        spread.Sheets[j].Rows[i].BackColor = Color.LightGreen;
                }
            }
        }

        protected void RemoveSuperfluousColumns(DataSet ds)
        {
            for(int j=0; j<ds.Tables.Count; j++)
            {
                RemoveSuperfluousColumns(ds.Tables[j]);
            }
        }

        protected void RemoveSuperfluousColumns(DataTable dt)
        {
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (dt.Columns[i].Caption == Definition.COL_TOGGLE ||
                    dt.Columns[i].Caption == Definition.CHART_COLUMN.ORDERINFILEDATA ||
                    dt.Columns[i].Caption == Definition.CHART_COLUMN.TOTALDATA ||
                    dt.Columns[i].Caption == Definition.CHART_COLUMN.DTSOURCEID ||
                    dt.Columns[i].Caption == Definition.CHART_COLUMN.TABLENAME)
                    dt.Columns.RemoveAt(i);
            }
        }

        #endregion

        //ContextMenu 추가 ( filter, FilterHide,freeze,unfreeze)

        void filter_Click(object sender, EventArgs e)
        {
            
            filterClicked = true;
            if (bTabControl1.SelectedTab.Text.Equals("Raw Data        "))
            {
                _Rawsfilter.Visible = true;
                fiterHide = new MenuItem("Filter_Hide");
                fiterHide.Click += new EventHandler(filter_Hide_Click);
                this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                this.bsprData.ContextMenu.MenuItems.Remove(filter);
            }
            else 
            {
                _sfilter.Visible = true;
                if (filterClicked)
                {
                    fiterHide = new MenuItem("Filter_Hide");
                    fiterHide.Click += new EventHandler(filter_Hide_Click);
                    this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                    this.bsprData.ContextMenu.MenuItems.Remove(filter);
                }
            }
        }
        void filter_Hide_Click(object sender, EventArgs e)
        {
            if (bTabControl1.SelectedTab.Text.Equals("Raw Data        "))
            {
                _Rawsfilter.Visible = false;
            }
            else
            {
                _sfilter.Visible = false;
            }
            filterClicked = false;
            if (!filterClicked)
            {
                bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                bsprData.ContextMenu.MenuItems.Add(filter);
            }
        }
        void freeze_Click(object sender, EventArgs e)
        {

            if (bsprData.ActiveSheet.ColumnCount <= 0)
                return;

            if (bTabControl1.SelectedTab.Text.Equals("Raw Data        "))
            {
                bsprRawData.ActiveSheet.FrozenColumnCount = bsprRawData.ActiveSheet.ActiveColumnIndex;
                _Rawsfilter.SetColumnFreeze(this.bsprData.ActiveSheet.ActiveColumnIndex);
            }
            else
            {
                bsprData.ActiveSheet.FrozenColumnCount = bsprData.ActiveSheet.ActiveColumnIndex;
                _sfilter.SetColumnFreeze(this.bsprData.ActiveSheet.ActiveColumnIndex);
            }
            if (this.bsprData.ActiveSheet.ActiveColumnIndex >= 0)
            {
                
                unfreeze = new MenuItem("UnFreeze");
                unfreeze.Click += new EventHandler(unfreeze_Click);
                unfreeze.Name = "UnFreeze";
                if (!this.bsprData.ContextMenu.MenuItems.ContainsKey("UnFreeze"))
                {
                    this.bsprData.ContextMenu.MenuItems.Add(unfreeze);
                }
                
            }
            else if (this.bsprRawData.ActiveSheet.ActiveColumnIndex >= 0)
            {
                unfreeze = new MenuItem("UnFreeze");
                unfreeze.Click += new EventHandler(unfreeze_Click);
                if (!this.bsprData.ContextMenu.MenuItems.ContainsKey("UnFreeze"))
                {
                    this.bsprData.ContextMenu.MenuItems.Add(unfreeze);
                }
            }


            
        }

        void unfreeze_Click(object sender, EventArgs e)
        {
            if (bTabControl1.SelectedTab.Text.Equals("Raw Data        "))
            {
                bsprRawData.ActiveSheet.FrozenColumnCount = 0;
                _Rawsfilter.SetColumnFreeze(0);
            }
            else
            {
                bsprData.ActiveSheet.FrozenColumnCount = 0;
                _sfilter.SetColumnFreeze(0);
            }

            if (this.bsprData.ContextMenu.MenuItems.ContainsKey("UnFreeze"))
            {
                this.bsprData.ContextMenu.MenuItems.RemoveByKey("UnFreeze");
            }

        }
       
        #region ::: Event

        private void bbtnList_ButtonClick(string name)
        {
            try
            {
                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    if (this.bTabControl1.SelectedIndex == 0)
                        this.bsprData.Export(true);
                    else
                        this.bsprRawData.Export(true);
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        protected virtual void Export()
        {
            if (this.bTabControl1.SelectedIndex == 0)
            {
                string file = "";
                bool bProtect = this.bsprData.ActiveSheet.Protect;

                this.bsprData.ActiveSheet.Protect = false;

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

                file = openDlg.FileName;

                FarPoint.Win.Spread.SheetView spread_Sheet1 = new FarPoint.Win.Spread.SheetView();
                spread_Sheet1.SheetName = "_ExcelExportSheet";

                FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();

                spread.Sheets.Add(spread_Sheet1);
                spread_Sheet1.Visible = true;
                spread.ActiveSheet = spread_Sheet1;

                byte[] buffer = null;
                System.IO.MemoryStream stream = null;
                this.bsprData.SetFilterVisible(false);

                try
                {
                    stream = new System.IO.MemoryStream();
                    this.bsprData.Save(stream, false);
                    buffer = stream.ToArray();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                }

                stream = new System.IO.MemoryStream(buffer);
                spread.Open(stream);

                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

                for (int i = spread.ActiveSheet.Columns.Count - 1; i >= 0; i--)
                {
                    if (!spread.ActiveSheet.Columns[i].Visible)
                        spread.ActiveSheet.Columns[i].Remove();
                }

                spread.SaveExcel(file, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                this.bsprData.ActiveSheet.Protect = bProtect;

                string strMessage = "It was saved successfully. Do you open saved file?";

                DialogResult result = MessageBox.Show(strMessage, "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Applications\EXCEL.EXE");

                    if (key == null)
                    {
                        MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_NEED_MS_OFFICE", null, null);
                    }
                    else
                    {

                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = file;
                        process.Start();
                    }
                }
            }
            else
            {
                string file = "";
                bool bProtect = this.bsprRawData.ActiveSheet.Protect;

                this.bsprRawData.ActiveSheet.Protect = false;

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

                file = openDlg.FileName;

                FarPoint.Win.Spread.SheetView spread_Sheet1 = new FarPoint.Win.Spread.SheetView();
                spread_Sheet1.SheetName = "_ExcelExportSheet";

                FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();

                spread.Sheets.Add(spread_Sheet1);
                spread_Sheet1.Visible = true;
                spread.ActiveSheet = spread_Sheet1;

                byte[] buffer = null;
                System.IO.MemoryStream stream = null;
                this.bsprRawData.SetFilterVisible(false);

                try
                {
                    stream = new System.IO.MemoryStream();
                    this.bsprRawData.Save(stream, false);
                    buffer = stream.ToArray();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                }

                stream = new System.IO.MemoryStream(buffer);
                spread.Open(stream);

                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

                for (int i = spread.ActiveSheet.Columns.Count - 1; i >= 0; i--)
                {
                    if (!spread.ActiveSheet.Columns[i].Visible)
                        spread.ActiveSheet.Columns[i].Remove();
                }

                spread.SaveExcel(file, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                this.bsprRawData.ActiveSheet.Protect = bProtect;

                string strMessage = "It was saved successfully. Do you open saved file?";

                DialogResult result = MessageBox.Show(strMessage, "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Applications\EXCEL.EXE");

                    if (key == null)
                    {
                        MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_NEED_MS_OFFICE", null, null);
                    }
                    else
                    {

                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = file;
                        process.Start();
                    }
                }

            }
        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion


        #region : Public

        public DataTable DataTableParam
        {
            get { return _dtParam; }
            set { _dtParam = value; }
        }

        public DataTable DataTableRaw
        {
            get { return _dtRaw; }
            set { _dtRaw = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public virtual void AddRawData(object rawData)
        {
            DataTableRaw = (DataTable) rawData;
        }

        public virtual void AddParamData(object paramData)
        {
            DataTableParam = (DataTable) paramData;
        }
        #endregion

        private void bTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (this._Rawsfilter != null)
            {
                this._Rawsfilter.Visible = false;
                this._sfilter.Visible = false;
                this.bsprData.SetFilterVisible(false);
                this.bsprRawData.SetFilterVisible(false);

                if (!this.bsprData.ContextMenu.MenuItems.Contains(filter))
                {
                    filter = new MenuItem("Filter");
                    filter.Click += new EventHandler(filter_Click);
                    this.bsprData.ContextMenu.MenuItems.Add(filter);
                    this.bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                }
                if (!this.bsprRawData.ContextMenu.MenuItems.Contains(filter))
                {
                    filter = new MenuItem("Filter");
                    filter.Click += new EventHandler(filter_Click);
                    this.bsprData.ContextMenu.MenuItems.Add(filter);
                    this.bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                }
            }

        }

        private void bsprData_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                {
                    
                    if (e.KeyCode == Keys.F)
                    {
                        if (_sfilter != null)
                        {
                            if (this.bsprData.ContextMenu.MenuItems.Contains(fiterHide))
                            {
                                this.bsprData.SetFilterVisible(false);
                                _sfilter.Visible = false;
                                bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                                fiterHide = new MenuItem("Filter_Hide");
                                fiterHide.Click += new EventHandler(filter_Click);
                                this.bsprData.ContextMenu.MenuItems.Add(filter);

                            }
                            else if (this.bsprData.ContextMenu.MenuItems.Contains(filter))
                            {
                                this.bsprData.SetFilterVisible(false);
                                _sfilter.Visible = true;
                                bsprData.ContextMenu.MenuItems.Remove(filter);
                                fiterHide = new MenuItem("Filter_Hide");
                                fiterHide.Click += new EventHandler(filter_Hide_Click);
                                this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void bsprData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.F)
                {
                    if (_sfilter != null)
                    {
                        this.bsprData.SetFilterVisible(false);
                    }
                }
            }

            if (e.KeyCode == Keys.F)
            {
                if (!e.Control)
                {
                    if (_sfilter != null)
                    {
                        this.bsprData.SetFilterVisible(false);
                    }
                }
            }
        }

        private void bsprRawData_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                {

                    if (e.KeyCode == Keys.F)
                    {
                        if (_Rawsfilter != null)
                        {
                            if (this.bsprData.ContextMenu.MenuItems.Contains(fiterHide))
                            {
                                this.bsprRawData.SetFilterVisible(false);
                                _Rawsfilter.Visible = false;
                                bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                                fiterHide = new MenuItem("Filter_Hide");
                                fiterHide.Click += new EventHandler(filter_Click);
                                this.bsprData.ContextMenu.MenuItems.Add(filter);

                            }
                            else if (this.bsprData.ContextMenu.MenuItems.Contains(filter))
                            {
                                this.bsprRawData.SetFilterVisible(false);
                                _Rawsfilter.Visible = true;
                                bsprData.ContextMenu.MenuItems.Remove(filter);
                                fiterHide = new MenuItem("Filter_Hide");
                                fiterHide.Click += new EventHandler(filter_Hide_Click);
                                this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void bsprRawData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.F)
                {
                    if (_Rawsfilter != null)
                    {
                        this.bsprRawData.SetFilterVisible(false);
                    }
                }
            }

            if (e.KeyCode == Keys.F)
            {
                if (!e.Control)
                {
                    if (_Rawsfilter != null)
                    {
                        this.bsprRawData.SetFilterVisible(false);
                    }
                }
            }
        }

    }
}
