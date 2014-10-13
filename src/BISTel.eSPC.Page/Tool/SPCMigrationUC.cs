using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.CodeDom;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;
using System.IO.Compression;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;


namespace BISTel.eSPC.Page.Tool
{
    public partial class SPCMigrationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        private string _sLineRawid;
        private string _sLine;
        private string _sAreaRawid;
        private string _sArea;
        private string _sEQPModel;
        private string _sSPCModelRawid;
        private string _sSPCModelName;
        private DataSet _dsSPCModeData = new DataSet();
        private DataSet _dsSPCProductData = new DataSet();
        private SortedList srtLstProductID = new SortedList();


        enum iColIdx
        {
            SELECT,
            RAWID,
            PARAM_ALIAS,
            MAIN_YN
        }


        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public SPCMigrationUC()
        {
            InitializeComponent();
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = "BISTel.eSPC.Page.Modeling.SPCModelingUC";

            this.InitializePage();
        }

        LinkedList llstDynamicCondition = new LinkedList();
        private bool _bUseComma;
        public override void PageSearch(LinkedList llCondition)
        {
            //초기화
            this.InitializePageData();


            llstDynamicCondition = llCondition;

            if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_AREA", null, null);
                return;
            }

            DataTable dtArea = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_AREA];

            this._sAreaRawid = DCUtil.GetValueData(dtArea);
            this._sArea = dtArea.Rows[0][DCUtil.DISPLAY_FIELD].ToString();

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            {
                dtArea = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_LINE];
                this._sLine = dtArea.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                this._sLineRawid = DCUtil.GetValueData(dtArea);
            }

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
            {
                DataTable dtEQPModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_EQPMODEL];

                if (dtEQPModel.Rows[0]["CHECKED"].Equals("T"))
                {
                    if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        this._sEQPModel = dtEQPModel.Rows[0][DCUtil.VALUE_FIELD].ToString();

                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                        return;
                    }
                    else
                    {
                        DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                        this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                        this._sEQPModel = dtEQPModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                    }
                }
            }
            else
            {
                if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                    return;
                }

                DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                this._sEQPModel = "";
            }

            this.ConfigListDataBinding();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeBSpread();
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }


        public void InitializeBSpread()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.AutoGenerateColumns = true;
            this.bsprData.UseAutoSort = false;

            this.bsprProductData.ClearHead();
            this.bsprProductData.AddHeadComplete();
            this.bsprProductData.UseSpreadEdit = false;
            this.bsprProductData.AutoGenerateColumns = true;
            this.bsprProductData.UseAutoSort = false;

            this.bsprBLOBData.ClearHead();
            this.bsprBLOBData.AddHeadComplete();
            this.bsprBLOBData.UseSpreadEdit = false;
            this.bsprBLOBData.AutoGenerateColumns = true;
            this.bsprBLOBData.UseAutoSort = false;

            this.bsprTempTrxData.ClearHead();
            this.bsprTempTrxData.AddHeadComplete();
            this.bsprTempTrxData.UseSpreadEdit = false;
            this.bsprTempTrxData.AutoGenerateColumns = true;
            this.bsprTempTrxData.UseAutoSort = false;

            this.bsprOOCTRXData.ClearHead();
            this.bsprOOCTRXData.AddHeadComplete();
            this.bsprOOCTRXData.UseSpreadEdit = false;
            this.bsprOOCTRXData.AutoGenerateColumns = true;
            this.bsprOOCTRXData.UseAutoSort = false;

            this.bsprContextMSTData.ClearHead();
            this.bsprContextMSTData.AddHeadComplete();
            this.bsprContextMSTData.UseSpreadEdit = false;
            this.bsprContextMSTData.AutoGenerateColumns = true;
            this.bsprContextMSTData.UseAutoSort = false;
        }

        public void InitializePageData()
        {
            this._sLineRawid = null;
            this._sAreaRawid = null;
            this._sEQPModel = null;
            this._sSPCModelRawid = string.Empty;
            this._sSPCModelName = string.Empty;

            this.InitializeBSpread();

            this.srtLstProductID = new SortedList();

            pgBarModel.Value = 0;
            pgBarData.Value = 0;
            pgBarDataTemp.Value = 0;
            pgBarOOC.Value = 0;
            pgBarModelContext.Value = 0;
        }

        #endregion


        #region ::: User Defined Method.

        private void ConfigListDataBinding()
        {
            string strParamAlias = "";
            //초기화
            _llstSearchCondition.Clear();
            _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, this._sSPCModelRawid);
            _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

            _dsSPCModeData = _wsSPC.GetSPCModelData(_llstSearchCondition.GetSerialData());
            this._dsSPCProductData = _wsSPC.GetProductIDMappingData();

            if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ALREADY_ELIMINATED", new string[]{_sSPCModelName}, null);
                return;
            }

            DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];
            DataTable dtRuleMst = _dsSPCModeData.Tables[TABLE.MODEL_RULE_MST_SPC];

            //#01. SPC Model Chart List를 위한 Datatable 생성
            DataTable dtSPCModelChartList = new DataTable();

            dtSPCModelChartList.Columns.Add(COLUMN.RAWID);
            dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
            dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);

            //CONTEXT COLUMN 생성
            DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);

            if (drConfigs != null && drConfigs.Length > 0)
            {
                DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                foreach (DataRow drMainContext in drMainContexts)
                {
                    dtSPCModelChartList.Columns.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                }
            }

            //2009-12-08 dkshin 추가 : CREATE_BY, CREATE_TIME COLUMN
            dtSPCModelChartList.Columns.Add(COLUMN.CREATE_BY);
            dtSPCModelChartList.Columns.Add(COLUMN.CREATE_DTTS);

            //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
            foreach (DataRow drConfig in dtConfig.Rows)
            {
                DataRow drChartList = dtSPCModelChartList.NewRow();

                drChartList[COLUMN.RAWID] = drConfig[COLUMN.RAWID].ToString();
                drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();

                if (strParamAlias == "")
                    strParamAlias = drConfig[COLUMN.PARAM_ALIAS].ToString();

                DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                foreach (DataRow drContext in drContexts)
                {
                    //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                    if (!dtSPCModelChartList.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                        dtSPCModelChartList.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                    drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                }

                //MODEL 정보                
                drChartList[COLUMN.CREATE_BY] = drConfig[COLUMN.CREATE_BY].ToString();
                drChartList[COLUMN.CREATE_DTTS] = drConfig[COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();
                dtSPCModelChartList.Rows.Add(drChartList);
            }

            dtSPCModelChartList.AcceptChanges();

            this.bsprData.DataSet = dtSPCModelChartList;

            for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
            {
                this.bsprData.ActiveSheet.Columns[cIdx].Width = this.bsprData.ActiveSheet.Columns[cIdx].GetPreferredWidth();
            }

            this.bsprProductData.DataSource = this._dsSPCProductData;

            for (int i = 0; i < this._dsSPCProductData.Tables[0].Rows.Count; i++)
            {
                this.srtLstProductID.Add(this._dsSPCProductData.Tables[0].Rows[i][0].ToString(), this._dsSPCProductData.Tables[0].Rows[i][1].ToString());
            }

            this.bsprData.Locked = true;
            this.bsprProductData.Locked = true;

        }

        #endregion

        private void bbtnStart_Click(object sender, EventArgs e)
        {
            BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();
            FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();

            pgBarModel.Value = 0;
            pgBarData.Value = 0;
            pgBarDataTemp.Value = 0;
            pgBarOOC.Value = 0;
            pgBarModelContext.Value = 0;

            string _startDate = BISTel.eSPC.Page.Common.CommonPageUtil.StartDate(StartDate.Value.ToString());
            string _endDate = BISTel.eSPC.Page.Common.CommonPageUtil.EndDate(EndDate.Value.ToString());
            
            if (this.bsprData.ActiveSheet.RowCount > 0)
            {
                if (this.srtLstProductID.Count > 0)
                {
                    pgBarModel.Maximum = this.bsprData.ActiveSheet.RowCount;

                    for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                    {
                        pgBarData.Value = 0;
                        pgBarDataTemp.Value = 0;
                        pgBarOOC.Value = 0;
                        pgBarModelContext.Value = 0;

                        this.bsprBLOBData.ClearHead();
                        this.bsprBLOBData.AddHeadComplete();
                        this.bsprBLOBData.UseSpreadEdit = false;
                        this.bsprBLOBData.AutoGenerateColumns = true;
                        this.bsprBLOBData.UseAutoSort = false;

                        this.bsprTempTrxData.ClearHead();
                        this.bsprTempTrxData.AddHeadComplete();
                        this.bsprTempTrxData.UseSpreadEdit = false;
                        this.bsprTempTrxData.AutoGenerateColumns = true;
                        this.bsprTempTrxData.UseAutoSort = false;

                        this.bsprOOCTRXData.ClearHead();
                        this.bsprOOCTRXData.AddHeadComplete();
                        this.bsprOOCTRXData.UseSpreadEdit = false;
                        this.bsprOOCTRXData.AutoGenerateColumns = true;
                        this.bsprOOCTRXData.UseAutoSort = false;

                        this.bsprContextMSTData.ClearHead();
                        this.bsprContextMSTData.AddHeadComplete();
                        this.bsprContextMSTData.UseSpreadEdit = false;
                        this.bsprContextMSTData.AutoGenerateColumns = true;
                        this.bsprContextMSTData.UseAutoSort = false;

                        this.bsprData.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                        Thread.Sleep(50);
                        Application.DoEvents();

                        string _sModelConfigRawid = this.bsprData.ActiveSheet.Cells[i, 0].Text.ToString();
                        LinkedList lnkListTemp = new LinkedList();
                        lnkListTemp.Add("MODEL_CONFIG_RAWID", _sModelConfigRawid);
                        lnkListTemp.Add("START_DTTS", _startDate);
                        lnkListTemp.Add("END_DTTS", _endDate);

                        #region DATA_TRX_SPC BLOB안에 PRODUCT_ID 바꾸기
                        DataSet _dsTemp = this._wsSPC.GetSPCBLOBData(lnkListTemp.GetSerialData());
                        this.bsprBLOBData.DataSource = _dsTemp;
                        if (_dsTemp.Tables[0].Rows.Count > 0)
                        {
                            pgBarData.Maximum = this.bsprBLOBData.ActiveSheet.RowCount;
                            this.bsprBLOBData.ActiveSheet.Cells[0, 0, this.bsprBLOBData.ActiveSheet.RowCount - 1, this.bsprBLOBData.ActiveSheet.ColumnCount - 1].CellType = tc;
                            for (int j = 0; j < this.bsprBLOBData.ActiveSheet.RowCount; j++)
                            {
                                string _sDataRawid = this.bsprBLOBData.ActiveSheet.Cells[j, 0].Text.ToString();
                                this.bsprBLOBData.ActiveSheet.Rows[j].BackColor = Color.Yellow;
                                Thread.Sleep(50);
                                Application.DoEvents();
                                //BLOB안에 DATA를 바꾸자.
                                object File_Data = _dsTemp.Tables[0].Rows[j][COLUMN.FILE_DATA];
                                //StreamReader sr = gZip.DecompressForStream(File_Data);
                                StreamReader sr = CommonUtility.ConvertBLOBToStreamReader(File_Data);
                                string _sline = string.Empty;
                                StringBuilder sbFileData = new StringBuilder();
                                while (sr.Peek() > -1)
                                {
                                    _sline = sr.ReadLine();
                                    if (_sline.IndexOf("PRODUCT_ID") == 0)
                                    {
                                        string[] strArrTemp = _sline.Split('\t');
                                        for (int k = 0; k < this.srtLstProductID.Count; k++)
                                        {
                                            string sTempKey = this.srtLstProductID.GetKey(k).ToString();
                                            string sTempValue = this.srtLstProductID.GetByIndex(k).ToString();
                                            for (int idxTemp = 0; idxTemp < strArrTemp.Length; idxTemp++)
                                            {
                                                if (strArrTemp[idxTemp].Equals(sTempKey))
                                                {
                                                    strArrTemp[idxTemp] = sTempValue;
                                                }
                                            }
                                        }
                                        _sline = "";
                                        for (int idxTemp_1 = 0; idxTemp_1 < strArrTemp.Length; idxTemp_1++)
                                        {
                                            _sline += "\t" + strArrTemp[idxTemp_1];
                                        }
                                        _sline = _sline.Substring(1);
                                    }
                                    sbFileData.AppendLine(_sline);
                                }
                                string _sFileData = sbFileData.ToString();
                                byte[] btBlob = Encoding.Default.GetBytes(_sFileData);
                                MemoryStream msDestination = new MemoryStream(btBlob.Length);
                                GZipStream compressedzipStream = new GZipStream(msDestination, CompressionMode.Compress, true);
                                compressedzipStream.Write(btBlob, 0, btBlob.Length);
                                compressedzipStream.Close();
                                msDestination.Position = 0;
                                btBlob = new byte[msDestination.Length];
                                msDestination.Read(btBlob, 0, (int)msDestination.Length);
                                LinkedList fieldData = new LinkedList();
                                fieldData.Add("RAWID", _sDataRawid);
                                fieldData.Add("FILE_DATA", btBlob);
                                bool bResult = this._wsSPC.ModifySPCBLOBData(fieldData.GetSerialData());
                                if (!bResult)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_ERROR_PARSING", null, null);
                                    return;
                                }

                                this.bsprBLOBData.ActiveSheet.Rows[j].BackColor = Color.LightGreen;
                                pgBarData.Increment(1);
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            pgBarData.Maximum = 1;
                            pgBarData.Value = 1;
                        }
                        #endregion

                        #region DATA_TEMP_TRX_SPC안에 PRODUCT_ID 바꾸기

                        DataSet _dsTempTrx = this._wsSPC.GetSPCTempTrxData(lnkListTemp.GetSerialData());
                        if (_dsTempTrx != null && _dsTempTrx.Tables != null && _dsTempTrx.Tables[0].Rows.Count > 0)
                        {
                            this.bsprTempTrxData.DataSource = _dsTempTrx;

                            pgBarDataTemp.Maximum = this.bsprTempTrxData.ActiveSheet.RowCount;
                            this.bsprTempTrxData.ActiveSheet.Cells[0, 0, this.bsprTempTrxData.ActiveSheet.RowCount - 1, this.bsprTempTrxData.ActiveSheet.ColumnCount - 1].CellType = tc;
                            for (int j = 0; j < this.bsprTempTrxData.ActiveSheet.RowCount; j++)
                            {
                                string _sDataRawid = this.bsprTempTrxData.ActiveSheet.Cells[j, 0].Text.ToString();
                                this.bsprTempTrxData.ActiveSheet.Rows[j].BackColor = Color.Yellow;
                                Thread.Sleep(50);
                                Application.DoEvents();
                                //CLOB안에 DATA를 바꾸자.
                                string sContext_List = _dsTempTrx.Tables[0].Rows[j][COLUMN.CONTEXT_LIST].ToString();
                                if (sContext_List.IndexOf("PRODUCT_ID") > -1)
                                {
                                    string[] strArrTemp = sContext_List.Split('\t');
                                    for (int idxTemp = 0; idxTemp < strArrTemp.Length; idxTemp++)
                                    {
                                        if (strArrTemp[idxTemp].Split('=').Length > 1)
                                        {
                                            if (strArrTemp[idxTemp].Split('=')[0].Equals("PRODUCT_ID"))
                                            {
                                                if (this.srtLstProductID.Contains(strArrTemp[idxTemp].Split('=')[1]))
                                                {
                                                    int _idxOfKey = this.srtLstProductID.IndexOfKey(strArrTemp[idxTemp].Split('=')[1]);
                                                    string sTempKey = this.srtLstProductID.GetKey(_idxOfKey).ToString();
                                                    string sTempValue = this.srtLstProductID.GetByIndex(_idxOfKey).ToString();
                                                    strArrTemp[idxTemp] = strArrTemp[idxTemp].Replace(sTempKey, sTempValue);
                                                }
                                            }
                                            else if (strArrTemp[idxTemp].Split('=')[0].Equals("CONTEXT_KEY"))
                                            {
                                                string[] strContextKeyValueTemp = strArrTemp[idxTemp].Split('=')[1].Split(';');
                                                if (strContextKeyValueTemp.Length > 0)
                                                {
                                                    for (int _iTemp = 0; _iTemp < strContextKeyValueTemp.Length; _iTemp++)
                                                    {
                                                        if (this.srtLstProductID.Contains(strContextKeyValueTemp[_iTemp]))
                                                        {
                                                            strContextKeyValueTemp[_iTemp] = this.srtLstProductID[strContextKeyValueTemp[_iTemp]].ToString();
                                                        }
                                                    }
                                                    string _sTemp = "";
                                                    for (int _idx = 0; _idx < strContextKeyValueTemp.Length; _idx++)
                                                    {
                                                        _sTemp += ";" + strContextKeyValueTemp[_idx];
                                                    }
                                                    _sTemp = _sTemp.Substring(1);
                                                    strArrTemp[idxTemp] = "CONTEXT_KEY=" + _sTemp;
                                                }
                                            }
                                        }
                                    }

                                    sContext_List = "";
                                    for (int idxTemp_1 = 0; idxTemp_1 < strArrTemp.Length; idxTemp_1++)
                                    {
                                        sContext_List += "\t" + strArrTemp[idxTemp_1];
                                    }
                                    sContext_List = sContext_List.Substring(1);
                                }

                                LinkedList fieldData = new LinkedList();
                                fieldData.Add("RAWID", _sDataRawid);
                                fieldData.Add("CONTEXT_LIST", sContext_List);
                                bool bResult = this._wsSPC.ModifySPCTempTrxData(fieldData.GetSerialData());
                                if (!bResult)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_ERROR_PARSING", null, null);
                                    return;
                                }

                                this.bsprTempTrxData.ActiveSheet.Rows[j].BackColor = Color.LightGreen;
                                pgBarDataTemp.Increment(1);
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            pgBarDataTemp.Maximum = 1;
                            pgBarDataTemp.Value = 1;
                        }
                        #endregion

                        #region OOC_TRX_SPC 안에 PRODUCT_ID 바꾸기

                        DataSet _dsTempOOCTrx = this._wsSPC.GetSPCOOCData(lnkListTemp.GetSerialData());
                        if (_dsTempOOCTrx != null && _dsTempOOCTrx.Tables != null && _dsTempOOCTrx.Tables[0].Rows.Count > 0)
                        {
                            this.bsprOOCTRXData.DataSource = _dsTempOOCTrx;

                            pgBarOOC.Maximum = this.bsprOOCTRXData.ActiveSheet.RowCount;
                            this.bsprOOCTRXData.ActiveSheet.Cells[0, 0, this.bsprOOCTRXData.ActiveSheet.RowCount - 1, this.bsprOOCTRXData.ActiveSheet.ColumnCount - 1].CellType = tc;
                            for (int j = 0; j < this.bsprOOCTRXData.ActiveSheet.RowCount; j++)
                            {
                                string _sDataRawid = this.bsprOOCTRXData.ActiveSheet.Cells[j, 0].Text.ToString();
                                this.bsprOOCTRXData.ActiveSheet.Rows[j].BackColor = Color.Yellow;
                                Thread.Sleep(50);
                                Application.DoEvents();
                                //CLOB안에 DATA를 바꾸자.
                                string sContext_List = _dsTempOOCTrx.Tables[0].Rows[j][COLUMN.CONTEXT_LIST].ToString();
                                if (sContext_List.IndexOf("PRODUCT_ID") > -1)
                                {
                                    string[] strArrTemp = sContext_List.Split('\t');
                                    for (int idxTemp = 0; idxTemp < strArrTemp.Length; idxTemp++)
                                    {
                                        if (strArrTemp[idxTemp].Split('=').Length > 1)
                                        {
                                            if (strArrTemp[idxTemp].Split('=')[0].Equals("PRODUCT_ID"))
                                            {
                                                if (this.srtLstProductID.Contains(strArrTemp[idxTemp].Split('=')[1]))
                                                {
                                                    int _idxOfKey = this.srtLstProductID.IndexOfKey(strArrTemp[idxTemp].Split('=')[1]);
                                                    string sTempKey = this.srtLstProductID.GetKey(_idxOfKey).ToString();
                                                    string sTempValue = this.srtLstProductID.GetByIndex(_idxOfKey).ToString();
                                                    strArrTemp[idxTemp] = strArrTemp[idxTemp].Replace(sTempKey, sTempValue);
                                                }
                                            }
                                            else if (strArrTemp[idxTemp].Split('=')[0].Equals("CONTEXT_KEY"))
                                            {
                                                string[] strContextKeyValueTemp = strArrTemp[idxTemp].Split('=')[1].Split(';');
                                                if (strContextKeyValueTemp.Length > 0)
                                                {
                                                    for (int _iTemp = 0; _iTemp < strContextKeyValueTemp.Length; _iTemp++)
                                                    {
                                                        if (this.srtLstProductID.Contains(strContextKeyValueTemp[_iTemp]))
                                                        {
                                                            strContextKeyValueTemp[_iTemp] = this.srtLstProductID[strContextKeyValueTemp[_iTemp]].ToString();
                                                        }
                                                    }
                                                    string _sTemp = "";
                                                    for (int _idx = 0; _idx < strContextKeyValueTemp.Length; _idx++)
                                                    {
                                                        _sTemp += ";" + strContextKeyValueTemp[_idx];
                                                    }
                                                    _sTemp = _sTemp.Substring(1);
                                                    strArrTemp[idxTemp] = "CONTEXT_KEY=" + _sTemp;
                                                }
                                            }
                                        }
                                    }

                                    sContext_List = "";
                                    for (int idxTemp_1 = 0; idxTemp_1 < strArrTemp.Length; idxTemp_1++)
                                    {
                                        sContext_List += "\t" + strArrTemp[idxTemp_1];
                                    }
                                    sContext_List = sContext_List.Substring(1);
                                }

                                LinkedList fieldData = new LinkedList();
                                fieldData.Add("RAWID", _sDataRawid);
                                fieldData.Add("CONTEXT_LIST", sContext_List);
                                bool bResult = this._wsSPC.ModifySPCOOCData(fieldData.GetSerialData());
                                if (!bResult)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_ERROR_PARSING", null, null);
                                    return;
                                }

                                this.bsprOOCTRXData.ActiveSheet.Rows[j].BackColor = Color.LightGreen;
                                pgBarOOC.Increment(1);
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            pgBarOOC.Maximum = 1;
                            pgBarOOC.Value = 1;
                        }
                        #endregion

                        #region MODEL_CONTEXT_MST_SPC 안에 PRODUCT_ID 바꾸기

                        DataSet _dsTempModelContext = this._wsSPC.GetSPCModelContextData(lnkListTemp.GetSerialData());
                        if (_dsTempModelContext != null && _dsTempModelContext.Tables != null && _dsTempModelContext.Tables[0].Rows.Count > 0)
                        {
                            this.bsprContextMSTData.DataSource = _dsTempModelContext;

                            pgBarModelContext.Maximum = this.bsprContextMSTData.ActiveSheet.RowCount;
                            this.bsprContextMSTData.ActiveSheet.Cells[0, 0, this.bsprContextMSTData.ActiveSheet.RowCount - 1, this.bsprContextMSTData.ActiveSheet.ColumnCount - 1].CellType = tc;
                            for (int j = 0; j < this.bsprContextMSTData.ActiveSheet.RowCount; j++)
                            {
                                string _sDataRawid = this.bsprContextMSTData.ActiveSheet.Cells[j, 0].Text.ToString();
                                this.bsprContextMSTData.ActiveSheet.Rows[j].BackColor = Color.Yellow;
                                Thread.Sleep(50);
                                Application.DoEvents();
                                //CONTEXT_VALUE안에 DATA를 바꾸자.
                                string sContext_Value = _dsTempModelContext.Tables[0].Rows[j][COLUMN.CONTEXT_VALUE].ToString();
                                string[] strArrTemp = sContext_Value.Split(';');
                                for (int idxTemp = 0; idxTemp < strArrTemp.Length; idxTemp++)
                                {
                                    if (this.srtLstProductID.Contains(strArrTemp[idxTemp]))
                                    {
                                        string sTempValue = this.srtLstProductID[strArrTemp[idxTemp]].ToString();
                                        strArrTemp[idxTemp] = sTempValue;
                                    }
                                }
                                
                                sContext_Value = "";
                                for (int idxTemp_1 = 0; idxTemp_1 < strArrTemp.Length; idxTemp_1++)
                                {
                                    sContext_Value += ";" + strArrTemp[idxTemp_1];
                                }
                                sContext_Value = sContext_Value.Substring(1);

                                LinkedList fieldData = new LinkedList();
                                fieldData.Add("RAWID", _sDataRawid);
                                fieldData.Add("CONTEXT_VALUE", sContext_Value);
                                bool bResult = this._wsSPC.ModifySPCModelContextData(fieldData.GetSerialData());
                                if (!bResult)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_ERROR_PARSING", null, null);
                                    return;
                                }

                                this.bsprContextMSTData.ActiveSheet.Rows[j].BackColor = Color.LightGreen;
                                pgBarModelContext.Increment(1);
                                Thread.Sleep(50);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            pgBarModelContext.Maximum = 1;
                            pgBarModelContext.Value = 1;
                        }
                        #endregion

                        this.bsprData.ActiveSheet.Rows[i].BackColor = Color.LightGreen;
                        pgBarModel.Increment(1);
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_INFO_PROD_MAPPING", null, null);
                    return;
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "Please search SPC Model first.");
                return;
            }
        }
    }
}
