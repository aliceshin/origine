using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;




namespace BISTel.eSPC.Page.Report.Popup
{
    public partial class SPCTimeBaseRawPopup : BasePopupFrm
    {

        #region : Constructor
        public SPCTimeBaseRawPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field
        Initialization _Initialization;
        CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;
        
        SessionData _SessionData;
        DataTable _dtOriData;
        DataTable _dtChartData;
        List<string> _lstRawColumn;

        ChartUtility _chartUtil;
        SortedList _slButtonList = new SortedList();

        #endregion


        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeVariable();
            this.InitializeLayout();
            this.InitializeChart();
            this.InitializeDataButton();
            this.ParseDataTable();
            this.MakeDataTable();
            this.DrawChart();
            this.bTChrt_RawData.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(bTChrt_RawData_ClickSeries);
        }

        void bTChrt_RawData_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                if (s.Title == "RAW")
                {
                    for (int i = 0; i < this.bTChrt_RawData.Chart.Tools.Count; i++)
                    {
                        if (this.bTChrt_RawData.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                        {
                            Annotation ann = (Annotation)this.bTChrt_RawData.Chart.Tools[i];
                            this._chartUtil.ShowAnnotate(this.bTChrt_RawData.Chart, ann, e.X, e.Y, CreateToolTipString("", "", valueIndex, ""));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //EESUtil.DebugLog(ex);
                //iValueIndex = -1;
            } 
        }

        public void InitializeVariable()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();
            this._lstRawColumn = new List<string>();
            this._chartUtil = new ChartUtility();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        }

        public void InitializeLayout()
        {
            //this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_PROCESS_CAPABILITY);
            this.Title = "Time-Base Raw Data";
        }

        public virtual void InitializeChart()
        {
            this.bTChrt_RawData.ClearChart();
            this.bbtnList.ButtonClick += new ButtonList.ImageDelegate(bbtnList_ButtonClick);

            this.bTChrt_RawData.Chart.Legend.CheckBoxes = true;
            this.bTChrt_RawData.IsVisibleLegendScrollBar = true;
            this.bTChrt_RawData.IsVisibleShadow = true;
            this.bTChrt_RawData.AxisDateTimeFormat = "yy-MM-dd";
            this.bTChrt_RawData.IsUseAutomaticAxisOfTeeChart = true;
            this.bTChrt_RawData.IsUseChartEditor = true;
            this.bTChrt_RawData.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.bTChrt_RawData.Chart.Axes.DrawBehind = true;
            this.bTChrt_RawData.Chart.Axes.Left.Grid.Visible = false;
            this.bTChrt_RawData.Chart.Axes.Right.Grid.Visible = false;
            this.bTChrt_RawData.Chart.Axes.Top.Grid.Visible = false;
            this.bTChrt_RawData.Chart.Axes.Bottom.Grid.Visible = false;

            this.bTChrt_RawData.SetXAxisRegularInterval(true);
            this.bTChrt_RawData.Chart.ClickLegend += new MouseEventHandler(Chart_ClickLegend);
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnList, ref this.bSpread1, Definition.PAGE_KEY_SPC_CHART_POPUP, Definition.BUTTONLIST_KEY_CHART_DATA, this.SessionData);
        }

        private void ParseDataTable()
        {
            LinkedList _llstData = new LinkedList();
            DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
            LinkedList mllstContextType = SetContextType(dsContextType);
            mllstContextType.Add("RAW", null);
            mllstContextType.Add("RAW_USL", null);
            mllstContextType.Add("RAW_UCL", null);
            mllstContextType.Add("RAW_TARGET", null);
            mllstContextType.Add("RAW_LCL", null);
            mllstContextType.Add("RAW_LSL", null);
            mllstContextType.Add("PARAM_LIST", null);
            mllstContextType.Add("RAW_DTTS", null);
            if(!mllstContextType.Contains("EQP_ID"))
                mllstContextType.Add("EQP_ID", null);
            for(int i=0;i<this._dtOriData.Columns.Count;i++)
            {
                if(!mllstContextType.Contains(this._dtOriData.Columns[i].ColumnName.ToUpper()))
                {
                    this._dtOriData.Columns.Remove(this._dtOriData.Columns[i].ColumnName);
                    i--;
                }
            }
        }

        private void MakeDataTable()
        {
            if (this._dtOriData.Rows.Count > 0)
            {
                this._dtChartData = this._dtOriData.Clone();
                DataTable dtTemp = new DataTable();
                dtTemp = this._dtOriData.Clone();
                foreach (DataRow dr in _dtOriData.Rows)
                {
                    dtTemp.Clear();
                    if (dr[COLUMN.RAW_DTTS].ToString().Length != 0)
                    {
                        if (dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length)
                        {
                            for (int i = 0; i < dr["RAW"].ToString().Split(';').Length; i++)
                            {
                                if(dr["RAW"].ToString().Split(';')[i].Length > 0)
                                    dtTemp.ImportRow(dr);
                            }

                            for (int j = 0; j < dtTemp.Rows.Count; j++)
                            {
                                dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                dtTemp.Rows[j][COLUMN.PARAM_LIST] = dtTemp.Rows[j][COLUMN.PARAM_LIST].ToString().Split(';')[j];
                                dtTemp.Rows[j][COLUMN.RAW_DTTS] = dtTemp.Rows[j][COLUMN.RAW_DTTS].ToString().Split(';')[j];
                                if (dtTemp.Rows[j]["SUBSTRATE_ID"] != null && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Length > 0 && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';').Length > 1)
                                {
                                    dtTemp.Rows[j]["SUBSTRATE_ID"] = dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';')[j];
                                }
                            }

                            this._dtChartData.Merge(dtTemp);
                        }
                    }
                }

                if (this._dtChartData.Rows.Count > 0)
                {
                    this._dtChartData = DataUtil.DataTableImportRow(this._dtChartData.Select(null, COLUMN.RAW_DTTS));                                                 
                }

                this._dtChartData.Columns.Add("TIME", typeof(DateTime)); 
                this._dtChartData.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                this._dtChartData.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));
                

                for (int i = 0; i < this._dtChartData.Rows.Count; i++)
                {
                    string sTime = this._dtChartData.Rows[i]["raw_dtts"].ToString();
                    this._dtChartData.Rows[i][Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 16);
                    this._dtChartData.Rows[i]["TIME"] = sTime;
                    this._dtChartData.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX] = i;
                }
            }
        }

        private void DrawChart()
        {
            SeriesInfo si = null;
            if (this._dtChartData.Rows.Count > 0)
            {
                for (int i = 0; i < this._dtChartData.Columns.Count; i++)
                {
                    string seriesName = this._dtChartData.Columns[i].ColumnName.ToUpper();
                    if (seriesName == "RAW" || seriesName == "RAW_UCL" || seriesName == "RAW_LCL" || seriesName == "RAW_LSL" || seriesName == "RAW_USL" || seriesName == "RAW_TARGET")
                    {
                        si = new SeriesInfo(typeof(Line), seriesName, this._dtChartData, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                        switch (seriesName)
                        {
                            case "RAW_UCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                            case "RAW_LCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                            case "RAW_LSL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                            case "RAW_USL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                            case "RAW_TARGET": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                            case "RAW": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG); break;
                        }
                        this.bTChrt_RawData.AddSeries(si);
                    }
                }

                Series s = null;
                for (int i = 0; i < this.bTChrt_RawData.Chart.Series.Count; i++)
                {
                    s = this.bTChrt_RawData.Chart.Series[i];

                    ((Line)s).LinePen.Width = 2;
                    ((Line)s).OutLine.Visible = false;
                    ((Line)s).ColorEachLine = false;
                    ((Line)s).Pointer.Visible = false;
                    ((Line)s).Active = true;

                    if (s.Title == "RAW")
                    {
                        ((Line)s).LinePen.Width = 1;
                        ((Line)s).Pointer.Visible = true;
                    }
                }
                //added by enkim 2012.12.12 SPC-839
                if (!this.bTChrt_RawData.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
                {
                    this.bTChrt_RawData.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
                }
                this.bTChrt_RawData.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                this.bTChrt_RawData.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                this.bTChrt_RawData.ChangeXLabelColumnDefault("TIME2");
                this.bTChrt_RawData.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                this.bTChrt_RawData.Refresh();
                
            }
        }

        public static LinkedList SetContextType(DataSet argDataSet)
        {
            LinkedList _llst = new LinkedList();
            if (DataUtil.IsNullOrEmptyDataSet(argDataSet)) return _llst;

            SPCStruct.ContextTypeInfo contextTypeInfo = null;
            foreach (DataRow dr in argDataSet.Tables[0].Rows)
            {
                contextTypeInfo = new SPCStruct.ContextTypeInfo(dr[COLUMN.CODE].ToString(), dr[COLUMN.NAME].ToString());
                _llst.Add(dr[COLUMN.CODE].ToString(), contextTypeInfo);
            }
            return _llst;
        }

        

        void bbtnList_ButtonClick(string name)
        {
            if (this.bTChrt_RawData.Chart.Series.Count < 1)
                return;

            try
            {
                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    this.Click_EXCEL_EXPORT_Button();
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {

            }
        }

        void Chart_ClickLegend(object sender, MouseEventArgs e)
        {
        }

        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            string _sEQPID = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";
            string _sProductID = "-";
            string _sTime = "-";
            string _sParamList = "-";
            string _sRawValue = "-";
            string _sRawUSL = "-";
            string _sRawUCL = "-";
            string _sRawTarget = "-";
            string _sRawLCL = "-";
            string _sRawLSL = "-";

            if (this._dtChartData.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                _sEQPID = this._dtChartData.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

            if (this._dtChartData.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                _sLOTID = this._dtChartData.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

            if (this._dtChartData.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                _sOperationID = this._dtChartData.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

            if (this._dtChartData.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                _sSubstrateID = this._dtChartData.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

            if (this._dtChartData.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                _sProductID = this._dtChartData.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();

            if (this._dtChartData.Columns.Contains("TIME"))
                _sTime = this._dtChartData.Rows[rowIndex]["TIME"].ToString();

            if (this._dtChartData.Columns.Contains(COLUMN.PARAM_LIST))
                _sParamList = this._dtChartData.Rows[rowIndex][COLUMN.PARAM_LIST].ToString();

            if (this._dtChartData.Columns.Contains("RAW"))
                _sRawValue = this._dtChartData.Rows[rowIndex]["RAW"].ToString();

            if (this._dtChartData.Columns.Contains("RAW_USL"))
                _sRawUSL = this._dtChartData.Rows[rowIndex]["RAW_USL"].ToString();

            if (this._dtChartData.Columns.Contains("RAW_UCL"))
                _sRawUCL = this._dtChartData.Rows[rowIndex]["RAW_UCL"].ToString();

            if (this._dtChartData.Columns.Contains("RAW_TARGET"))
                _sRawTarget = this._dtChartData.Rows[rowIndex]["RAW_TARGET"].ToString();

            if (this._dtChartData.Columns.Contains("RAW_LCL"))
                _sRawLCL = this._dtChartData.Rows[rowIndex]["RAW_LCL"].ToString();

            if (this._dtChartData.Columns.Contains("RAW_LSL"))
                _sRawLSL = this._dtChartData.Rows[rowIndex]["RAW_LSL"].ToString();


            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);
            sb.AppendFormat("{0} : {1}\r\n", "TIME", _sTime);
            sb.AppendFormat("{0} : {1}\r\n", "PARAM LIST", _sParamList);
            sb.AppendFormat("{0} : {1}\r\n", "VALUE", _sRawValue);
            sb.AppendFormat("{0} : {1}\r\n", "USL", _sRawUSL);
            sb.AppendFormat("{0} : {1}\r\n", "UCL", _sRawUCL);
            sb.AppendFormat("{0} : {1}\r\n", "TARGET", _sRawTarget);
            sb.AppendFormat("{0} : {1}\r\n", "LCL", _sRawLCL);
            sb.AppendFormat("{0} : {1}\r\n", "LSL", _sRawLSL);

            return sb.ToString();
        }

        #endregion

        #region Button Function
        private void Click_EXCEL_EXPORT_Button()
        {

            try
            {
                DataTable dt = this._dtChartData.Copy();
                if (dt.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                {
                    dt.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                }
                if (dt.Columns.Contains("TIME"))
                {
                    dt.Columns.Remove("TIME");
                }
                if (dt.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
                {
                    dt.Columns.Remove(CommonChart.COLUMN_NAME_SEQ_INDEX);
                }

                this.bSpread1.ActiveSheet.RowCount = 0;
                this.bSpread1.ActiveSheet.ColumnCount = 0;
                this.bSpread1.ClearHead();
                this.bSpread1.AddHeadComplete();
                this.bSpread1.UseSpreadEdit = false;
                this.bSpread1.AutoGenerateColumns = true;
                this.bSpread1.DataSource = dt;
                this.bSpread1_Sheet1.SheetName = this.bTChrt_RawData.Name;

                string file = "";
                bool bProtect = this.bSpread1.ActiveSheet.Protect;

                this.bSpread1.ActiveSheet.Protect = false;

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
                this.bSpread1.SetFilterVisible(false);

                try
                {
                    stream = new System.IO.MemoryStream();
                    this.bSpread1.Save(stream, false);
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
                this.bSpread1.ActiveSheet.Protect = bProtect;

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
                //this.bSpread1.Export(false);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.ToString());
                EESUtil.DebugLog("BTeeChart", "ExportDataForExcel", ex);
            }


        }

        #endregion


        

        #region : Popup Logic

        #region :: Data Condition

        #region ::: Function
        #endregion

        #region ::: Event
        #endregion

        #endregion

        #region :: Button

        #region ::: Function

        #endregion

        #region ::: Event

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        #endregion

        #endregion

        #endregion

        #region : Public


        public DataTable dtOriData
        {
            get { return _dtOriData; }
            set { _dtOriData = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }


        public List<string>  lstRawColumn
        {
            set
            {
                this._lstRawColumn = value;
            }
            get
            {
                return this._lstRawColumn;
            }
        }

        #endregion

        
    }
}
