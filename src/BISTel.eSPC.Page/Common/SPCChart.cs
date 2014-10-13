using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Common
{
    public enum CHART_VIEW_TYPE
    {
        DEFAULT,
        SELECT
    }

    public partial class SPCChart : BasePageUCtrl
    {


        #region ::: Field


        protected CommonUtility _ComUtil = new CommonUtility();
        protected ChartUtility _chartUtil = new ChartUtility();

        protected eSPCWebService.eSPCWebService _wsSPC = null;
        protected MultiLanguageHandler _mlthandler;

        protected SourceDataManager _dataManager = null;

        protected Initialization _Initialization = null;

        protected LinkedList _llstChartSearchCondition = new LinkedList();
        protected LinkedList _llstValue = null;
        protected LinkedList _llstChart = new LinkedList();
        protected List<string> _lstDefaultChart = null;

        protected DataSet _dsChart = null;
        protected DataSet _dsOCAPValue = null;
        LinkedList _llstChartSeriesVisibleType = null;

        ChartInterface _ChartVariable;
        ChartInformation _chartInfomationUI = new ChartInformation();
        SortedList sListInformation = null;
        DataTableGroupBy dtGroupBy;

        protected double xval;
        int _valueIndex = -1;


        #endregion


        #region ::: Constructor
        public SPCChart()
        {
            Init();
            InitializeComponent();
        }


        #endregion



        #region ::: Override Method


        #endregion

        #region ::: PageLoad & Initialize

        public void Init()
        {
            _wsSPC = new BISTel.eSPC.Page.eSPCWebService.eSPCWebService();
            this._mlthandler = MultiLanguageHandler.getInstance();
            _dataManager = new SourceDataManager();
            _Initialization = new Initialization();
            this._Initialization.InitializePath();
        }

        public void InitializePage()
        {
            if (ChartVariable == null) return;
            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
            this.InitializeChartSeriesVisibleType();
            this.InitializeChart();
        }

        private void InitializeChartSeriesVisibleType()
        {
            _llstChartSeriesVisibleType = new LinkedList();
            this.panel1.Controls.Clear();
            Hashtable htButtonList = this._Initialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            BCheckBox bchk = null;
            for (int i = htButtonList.Count - 1; i >= 0; i--)
            {
                Hashtable htButtonAttributes = (Hashtable)htButtonList[i.ToString()];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();
                string DefaultVisible = htButtonAttributes[Definition.XML_VARIABLE_DEFAULTVALUE].ToString().ToUpper();

                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                {
                    bchk = new BCheckBox();
                    bchk.Text = this._mlthandler.GetVariable(sButtonName);
                    bchk.Name = sButtonName;
                    bchk.Click += new EventHandler(bchk_Click);
                    bchk.Dock = DockStyle.Left;
                    bchk.TabIndex = i;
                    if (DefaultVisible == Definition.VARIABLE_TRUE.ToUpper())
                        bchk.Checked = true;
                    else
                        bchk.Checked = false;

                    if (sButtonName == Definition.CHART_SERIES.POINT)
                    {
                        if (ChartVariable.complex_yn != "Y")
                            bchk.Visible = false;
                        else
                            bchk.Visible = true;
                    }
                    _llstChartSeriesVisibleType.Add(bchk.Text, bchk.Checked);
                    this.panel1.Controls.Add(bchk);
                }
            }


        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            _dsChart = this._wsSPC.GetCodeData(lk.GetSerialData());
            _llstChart.Clear();
            _llstChart = GetDefaultChart();
        }


        public void InitializeChart()
        {

            if (ChartVariable.dtParamData != null && ChartVariable.dtParamData.Rows.Count > 0)
                _dataManager.RawDataTable = ChartVariable.dtParamData;

            int iChart = _llstChart.Count;
            int iHeight = 300;

            if (iChart == 1)
                iHeight = this.pnlChart.Height;
            else if (iChart > 1)
                iHeight = this.pnlChart.Height / 2;

            this.pnlChart.Controls.Clear();
            DefaultChart chartBase = null;
            Steema.TeeChart.Tools.CursorTool _cursorTool = null;
            for (int i = _llstChart.Count - 1; i >= 0; i--)
            {

                string strKey = this._llstChart.GetKey(i).ToString();
                string strValue = this._llstChart.GetValue(i).ToString();
                chartBase = new DefaultChart(_dataManager);
                chartBase.ClearChart();
                chartBase.Title = strValue;
                chartBase.Name = strKey;
                chartBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                chartBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                chartBase.SPCChartTitlePanel.Name = strKey;
                chartBase.Height = iHeight;
                chartBase.URL = this.URL;
                chartBase.Dock = System.Windows.Forms.DockStyle.Top;
                chartBase.ContextMenu = this.bbtnListChart.ContextMenu;
                chartBase.ParentControl = this.pnlChart;
                chartBase.StartDateTime = ChartVariable.dateTimeStart;
                chartBase.EndDateTime = ChartVariable.dateTimeEnd;
                chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, this._llstChartSeriesVisibleType);
                if (!chartBase.DrawSPCChart()) continue;
                _cursorTool = new Steema.TeeChart.Tools.CursorTool();
                _cursorTool.Active = true;
                _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                _cursorTool.Pen.Color = Color.Gray;
                _cursorTool.FollowMouse = true;
                _cursorTool.Change += new Steema.TeeChart.Tools.CursorChangeEventHandler(_cursorTool_Change);
                chartBase.SPCChart.Chart.Tools.Add(_cursorTool);
                chartBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                this.pnlChart.Controls.Add(chartBase);
            }

        }

        public void InitialUserInfo()
        {
        }

        public void InitializeLayout()
        {

        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_CHART_UC, Definition.BUTTONLIST_KEY_CHART, this.sessionData);
        }

        public void InitializeBSpread()
        {
            _chartInfomationUI.SessionData = this.sessionData;
            _chartInfomationUI.ChartVariable = ChartVariable;
            _chartInfomationUI.dsChart = this._dsChart;
            _chartInfomationUI.InitializePopup();
            _chartInfomationUI.Dock = DockStyle.Fill;
            sListInformation = _chartInfomationUI.sListInformation;
            this.bTitlePanel1.Controls.Add(_chartInfomationUI);
            this.bTitlePanel1.MaxResizable = true;
        }

        #endregion

        #region ::: User Defined Method.



        private void GetOCAPRawID()
        {
            if (ChartVariable.dtParamData == null) return;
            if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            {
                string _fieldList = Definition.CHART_COLUMN.OCAP_RAWID;
                string _groupby = Definition.CHART_COLUMN.OCAP_RAWID;
                string _rowFilter = "ocap_rawid <>''";

                dtGroupBy = new DataTableGroupBy();
                DataTable dtOCAP = dtGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);

                _llstChartSearchCondition.Clear();
                _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.OCAP_DATATABLE, dtOCAP);
                _dsOCAPValue = _wsSPC.GetOCAPDetails(_llstChartSearchCondition.GetSerialData());
            }
        }

        private LinkedList GetDefaultChart()
        {
            LinkedList _llstDefaultChart = new LinkedList();
            _llstDefaultChart.Clear();
            if (DataUtil.IsNullOrEmptyDataSet(_dsChart)) return _llstDefaultChart;
            if (ChartVariable == null) return _llstDefaultChart;
            foreach (DataRow dr in _dsChart.Tables[0].Rows)
            {
                string strkey = dr["CODE"].ToString();
                if (ChartVariable.lstDefaultChart.Contains(strkey))
                {
                    _llstDefaultChart.Add(strkey, dr["DESCRIPTION"].ToString());
                }
            }
            return _llstDefaultChart;
        }


        #endregion

        #region ::: EventHandler

        #region ::: Event Methods
        private BaseChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;

            while (parentControl != null)
            {
                if (parentControl is BaseChart)
                {
                    return parentControl as BaseChart;
                }
                else
                {
                    parentControl = parentControl.Parent;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCol"></param>
        /// <param name="strValue"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();

            string _sEQPID = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";
            string _sProductID = "-";


            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                _sEQPID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                _sLOTID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                _sOperationID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                _sSubstrateID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PRODUCT_ID))
                _sProductID = _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.PRODUCT_ID].ToString();

            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.PRODUCT_ID), _sProductID);
            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID), _sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);

            if (_dataManager.RawDataTable.Columns.Contains(strChartName))
            {
                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                for (int i = 0; i < llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString();

                    if (sValue.Equals(Definition.CHART_COLUMN.USL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LSL) ||
                        sValue.Equals(Definition.CHART_COLUMN.UCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;

                    if (_dataManager.RawDataTable.Columns.Contains(sKey))
                    {
                        sb.AppendFormat("{0} : {1}\r\n", this._mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());
                    }
                }
                llstChartSeries = null;
            }

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            {
                string strRawid = GetOCAPRawID(_dataManager.RawDataTable, rowIndex);
                DataTable dtOCAP = null;

                this.dtGroupBy = new DataTableGroupBy();
                if (!string.IsNullOrEmpty(strRawid))
                {
                    string _fieldList = "ocap_rawid,RAW_RULE";
                    string _groupby = "ocap_rawid,RAW_RULE";
                    string _sRuleNo = string.Empty;
                    if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();
                    if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        string _rowFilter = string.Format("ocap_rawid ='{0}'", strRawid);
                        dtOCAP = this.dtGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);
                        foreach (DataRow dr in dtOCAP.Rows)
                        {
                            if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
                            _sRuleNo += dr["RAW_RULE"].ToString().Replace(";", ",");
                        }
                        sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);
                    }
                }
                this.dtGroupBy = null;
                if (dtOCAP != null) dtOCAP.Dispose();
            }

            return sb.ToString();
        }

        private string GetOCAPRawID(DataTable dt, int rowIndex)
        {
            string strRawid = string.Empty;
            if (dt.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                strRawid = dt.Rows[rowIndex][Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();

            return strRawid;
        }

        private void ClickButtonCalc()
        {

        }

        private void ClickButtonChartView()
        {

            Popup.ChartViewSelectedPopup chartViewPop = new Popup.ChartViewSelectedPopup();
            chartViewPop.URL = this.URL;
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.DataSetChart = _dsChart; //Chart 종류   
            chartViewPop.llstDefaultChart = _llstChart;
            chartViewPop.InitializePopup();
            DialogResult result = chartViewPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _llstChart.Clear();
                _llstChart = chartViewPop.llstResultListChart;
                this.InitializeChartSeriesVisibleType();
                this.InitializeChart(); //chart 다시 그림  

                chartViewPop.Dispose();
            }
        }

        private void ClickButtonConfiguration()
        {
            Modeling.SPCConfigurationPopup spcConifgPopup = new Modeling.SPCConfigurationPopup();
            spcConifgPopup.SESSIONDATA = this.sessionData;
            spcConifgPopup.URL = this.URL;
            spcConifgPopup.PORT = this.Port;
            spcConifgPopup.CONFIG_MODE = ConfigMode.VIEW;
            spcConifgPopup.CONFIG_RAWID = ChartVariable.MODEL_CONFIG_RAWID;
            spcConifgPopup.MAIN_YN = ChartVariable.MAIN_YN;
            spcConifgPopup.InitializePopup();
            DialogResult result = spcConifgPopup.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                spcConifgPopup.Dispose();
            }

        }

        private void ClickButtonShowChartData()
        {
            DataTable dt = _dataManager.RawDataTable.Copy();            
            ChartDataPopup chartDataPop = null;
            try
            {           
                chartDataPop = new ChartDataPopup();
                chartDataPop.URL = this.URL;
                chartDataPop.SessionData = this.sessionData;
                chartDataPop.DataTableParam = CommonPageUtil.ExcelExport(dt, ChartVariable.lstRawColumn);
                chartDataPop.InitializePopup();
                DialogResult result = chartDataPop.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    chartDataPop.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                dt.Dispose();                
            }

        }


        private void CursorSynchronize(Steema.TeeChart.Tools.CursorTool SRC, Steema.TeeChart.Tools.CursorTool DEST)
        {
            DEST.XValue = SRC.XValue;
            DEST.YValue = SRC.YValue;

        }

        #endregion



        void SPCChart_Resize(object sender, System.EventArgs e)
        {
            BaseChart baseChart = null;
            int iCount = this.pnlChart.Controls.Count;
            int iHeight = 0;
            if (iCount == 1)
            {
                iHeight = this.pnlChart.Height;
            }
            else if (iCount > 1)
            {
                iHeight = this.pnlChart.Height / 2;
            }

            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.Height = iHeight;
            }
        }


        private void bchk_Click(object sender, System.EventArgs e)
        {
            BCheckBox chk = (BCheckBox)sender;
            if (chk == null) return;
            BaseChart baseChart = null;
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.llstChartSeriesVisibleType = _llstChartSeriesVisibleType;
                baseChart.ChangeSeriesStyle(chk.Text, chk.Checked);
            }
        }



        private void _cursorTool_Change(object sender, Steema.TeeChart.Tools.CursorChangeEventArgs e)
        {
            Steema.TeeChart.Tools.CursorTool _currentCursor = sender as Steema.TeeChart.Tools.CursorTool;
            double xValue = _currentCursor.XValue;
            double yValue = _currentCursor.YValue;
            ControlCollection parentControl = this.pnlChart.Controls;
            for (int i = 0; i < parentControl.Count; i++)
            {
                if (parentControl[i] is BaseChart)
                {
                    BaseChart baseChart = parentControl[i] as BaseChart;
                    for (int j = 0; j < baseChart.SPCChart.Chart.Tools.Count; j++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[j].GetType() == typeof(Steema.TeeChart.Tools.CursorTool))
                        {

                            Steema.TeeChart.Tools.CursorTool _CursorDesc = (Steema.TeeChart.Tools.CursorTool)baseChart.SPCChart.Chart.Tools[j];
                            if (_CursorDesc.Pen.Color == Color.Gray)
                            {
                                if (xValue != _CursorDesc.XValue && yValue != _CursorDesc.YValue)
                                {
                                    CursorSynchronize(_currentCursor, _CursorDesc);
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }

        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip
                BaseChart baseChart = FindSPCChart((Control)sender);
                if (baseChart is DefaultChart)
                {
                    this._chartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);
                    _valueIndex = valueIndex;

                    string strCol = s.Title;
                    if (strCol == Definition.CHART_COLUMN.USL_LSL)
                        strCol = Definition.CHART_COLUMN.USL;
                    else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                        strCol = Definition.CHART_COLUMN.UCL;

                    string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();

                    for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                        {
                            Annotation ann = (Annotation)baseChart.SPCChart.Chart.Tools[i];
                            this._chartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseChart.NAME));
                            break;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                _valueIndex = -1;
            }
        }


        private void bbtnListChart_ButtonClick(string name)
        {
            try
            {
                if (ChartVariable.dtParamData == null || ChartVariable.dtParamData.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                if (name.ToUpper() == Definition.ButtonKey.CALC)
                    this.ClickButtonCalc();
                else if (name.ToUpper() == Definition.ButtonKey.SELECT_CHART_TO_VIEW)
                    this.ClickButtonChartView();
                else if (name.ToUpper() == Definition.ButtonKey.DEFAULT_CHART)
                {

                    _llstChart.Clear();
                    _llstChart = GetDefaultChart();
                    this.InitializeChartSeriesVisibleType();
                    this.InitializeChart();
                }
                else if (name.ToUpper() == Definition.ButtonKey.CONFIGURATION)
                    this.ClickButtonConfiguration();
                else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)
                    this.ClickButtonShowChartData();
                else if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW)
                {

                    if (_chartInfomationUI.bsprInformationData.ActiveSheet.RowCount == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    if (_valueIndex == -1) return;
                    string strRawid = GetOCAPRawID(_dataManager.RawDataTable, _valueIndex);
                    string[] arr = strRawid.Split(';');
                    string sRawid = string.Empty;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i])) continue;
                        sRawid += arr[i] + ",";
                    }
                    if (sRawid.EndsWith(",")) sRawid = sRawid.Substring(0, sRawid.Length - 1);

                    if (string.IsNullOrEmpty(sRawid))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    BISTel.eSPC.Page.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.OCAP.OCAPDetailsPopup();
                    popupOCAP.ChartVariable = ChartVariable;
                    //popupOCAP.ChartVariable.OPERATION_ID = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)enum_ChartInfomationData.OPERATION_ID, 1].Text;
                    //popupOCAP.ChartVariable.PRODUCT_ID = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)enum_ChartInfomationData.PRODUCT_ID, 1].Text;
                    popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(_dataManager.RawDataTable, _valueIndex);
                    popupOCAP.ChartVariable.OCAPRawID = sRawid;
                    popupOCAP.PopUpType = enum_PopupType.View;
                    popupOCAP.SessionData = this.sessionData;
                    popupOCAP.URL = this.URL;
                    popupOCAP.InitializePopup();
                    DialogResult result = popupOCAP.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        popupOCAP.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }



        private void bTitPanelInformation_MinimizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 300;
            BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
            bTitlePanel.Dock = DockStyle.Fill;
        }

        private void bTitPanelInformation_MaximizedPanel(object sender, System.EventArgs e)
        {
            this.bplInformation.Width = 30;
            BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
            bTitlePanel.Dock = DockStyle.Fill;
        }

        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                BaseChart baseChart = FindSPCChart((Control)sender);
                Annotation ann;
                if (baseChart is DefaultChart)
                {
                    for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Annotation))
                        {
                            ann = baseChart.SPCChart.Chart.Tools[i] as Annotation;
                            if (ann.Active)
                            {
                                ann.Active = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }
        #endregion

        #region ::: Public

        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }


        }
        #endregion
    }
}
