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
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class ChartInformation : UserControl
    {

        #region : Field
        MultiLanguageHandler _mlthandler;
        Initialization _Initialization = null;           
        SessionData _SessionData;
        ChartInterface _ChartVariable;
        DataTable _RawDataTable = new DataTable();
        DataSet _dsChart = null;
        SortedList _sl = null;
        CommonSPCStat comSPCStat;
        
        DataSet _dsContextType;
        SPCStruct.ChartInfoList _chartInfoList;
        eSPCWebService.eSPCWebService _wsSPC = null;
       
              
        #endregion



       

        #region : Constructor
        public ChartInformation()
        {            
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();    
            this.comSPCStat = new CommonSPCStat();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            InitializeComponent();
        }
        #endregion 
        


        public void InitializePopup()
        {           
            this.InitializeLayout();
            this.InitializeCode();
            this.InitializeBSpread();            
        }

        public void InitializeLayout()
        {
            //this.bsprData.ActiveSheet.FrozenColumnCount = 0;
            //this.bsprData.ActiveSheet.FrozenRowCount = 0;
            this.bsprData.ColFronzen = 0;
            this.bsprData.RowFronzen = 0;
        }

        public void InitializeBSpread()
        {
            this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC, false, Definition.PAGE_KEY_SPC_CHART_UC_HEADER_CHART_INFOMATION_KEY);
            this.bsprData.UseHeadColor = true;
            this.bsprData.EditMode =false;
            this.bsprData.IsCellCopy = true;
            this.bsprData.UseAutoSort = false;
            this.bsprData.ActiveSheet.RowHeader.Visible = false;
            this.bsprData.UseGeneralContextMenu = false;

            if (ChartVariable == null)
            {
                return;
            }
            CreateInfomationDataTable();
        }


        public void InitializeCode()
        {
            LinkedList lk = new LinkedList();            
            lk.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
            lk.Add(Definition.DynamicCondition_Condition_key.EXCLUDE, string.Format("'{0}','{1}'",Definition.CONTEXT_TYPE_CONTEXT_KEY,Definition.CONTEXT_TYPE_NON));
            _dsContextType = this._wsSPC.GetContextType(lk.GetSerialData());

            _chartInfoList = new SPCStruct.ChartInfoList();
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.ChartInfomationData.LINE, Definition.ChartInfomationData.LINE, this._mlthandler.GetVariable(Definition.ChartInfomationData.LINE), BISTel.eSPC.Common.ChartInfoDataType.FIX));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.ChartInfomationData.AREA, Definition.ChartInfomationData.AREA, this._mlthandler.GetVariable(Definition.ChartInfomationData.AREA), BISTel.eSPC.Common.ChartInfoDataType.FIX));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.SPC_MODEL_NAME, Definition.CHART_COLUMN.SPC_MODEL_NAME, this._mlthandler.GetVariable(Definition.ChartInfomationData.SPC_MODEL), BISTel.eSPC.Common.ChartInfoDataType.FIX));
            _chartInfoList.Add(new SPCStruct.ChartInfo(BISTel.eSPC.Common.COLUMN.PARAM_ALIAS, BISTel.eSPC.Common.COLUMN.PARAM_ALIAS, this._mlthandler.GetVariable(Definition.ChartInfomationData.PARAM_ALIAS), BISTel.eSPC.Common.ChartInfoDataType.FIX));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.ChartInfomationData.CHART_CONTEXT, Definition.ChartInfomationData.CHART_CONTEXT, this._mlthandler.GetVariable(Definition.ChartInfomationData.CHART_CONTEXT), BISTel.eSPC.Common.ChartInfoDataType.TITLE));            
                                    
            if (!DataUtil.IsNullOrEmptyDataSet(_dsContextType))
            {
                if (ChartVariable.dtParamData.Columns.Contains(Definition.COL_MODEL_CONFIG_RAWID))
                {
                    _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.COL_MODEL_CONFIG_RAWID, Definition.COL_MODEL_CONFIG_RAWID, this._mlthandler.GetVariable(Definition.COL_MODEL_CONFIG_RAWID), BISTel.eSPC.Common.ChartInfoDataType.CONTEXT));
                }

                foreach (DataRow dr in _dsContextType.Tables[0].Rows)
                {
                    if (ChartVariable.dtParamData.Columns.Contains(dr[BISTel.eSPC.Common.COLUMN.NAME].ToString()))
                    {
                        _chartInfoList.Add(new SPCStruct.ChartInfo(dr[BISTel.eSPC.Common.COLUMN.NAME].ToString(), dr[BISTel.eSPC.Common.COLUMN.CODE].ToString(), this._mlthandler.GetVariable(Definition.SPC_LABEL_ + dr[BISTel.eSPC.Common.COLUMN.NAME].ToString()), BISTel.eSPC.Common.ChartInfoDataType.CONTEXT));
                    }
                }
            }
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.ChartInfomationData.CHART_DATA, Definition.ChartInfomationData.CHART_DATA, this._mlthandler.GetVariable(Definition.ChartInfomationData.CHART_DATA), BISTel.eSPC.Common.ChartInfoDataType.TITLE));

            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME, this._mlthandler.GetVariable(Definition.CHART_COLUMN.TIME), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PN_USL, Definition.CHART_COLUMN.USL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.USL), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PN_LSL, Definition.CHART_COLUMN.LSL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.LSL), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PN_TARGET, Definition.CHART_COLUMN.TARGET, this._mlthandler.GetVariable(Definition.CHART_COLUMN.TARGET), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PN_UCL, Definition.CHART_COLUMN.UCL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.UCL), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PN_LCL, Definition.CHART_COLUMN.LCL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.LCL), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.SAMPLE_COUNT, Definition.CHART_COLUMN.SAMPLE_COUNT, this._mlthandler.GetVariable(Definition.CHART_COLUMN.SAMPLE_COUNT), BISTel.eSPC.Common.ChartInfoDataType.RELATED));
            _chartInfoList.Add(new SPCStruct.ChartInfo("UNIT_COUNT", "UNIT_COUNT", "UNIT COUNT", BISTel.eSPC.Common.ChartInfoDataType.RELATED));    
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PPK, Definition.CHART_COLUMN.PPK, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPK), BISTel.eSPC.Common.ChartInfoDataType.DATA));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PP, Definition.CHART_COLUMN.PP, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PP), BISTel.eSPC.Common.ChartInfoDataType.DATA));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PPL, Definition.CHART_COLUMN.PPL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPL), BISTel.eSPC.Common.ChartInfoDataType.DATA));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.PPU, Definition.CHART_COLUMN.PPU, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPU), BISTel.eSPC.Common.ChartInfoDataType.DATA));
                    
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.ChartInfomationData.ANNOTATION_DATA, Definition.ChartInfomationData.ANNOTATION_DATA, this._mlthandler.GetVariable(Definition.ChartInfomationData.ANNOTATION_DATA), BISTel.eSPC.Common.ChartInfoDataType.TITLE));            
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.OCAP_PROBLEM, Definition.CHART_COLUMN.OCAP_PROBLEM, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.OOC_PROBLEM), BISTel.eSPC.Common.ChartInfoDataType.OCAP));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.OCAP_CAUSE, Definition.CHART_COLUMN.OCAP_CAUSE, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.OOC_CAUSE), BISTel.eSPC.Common.ChartInfoDataType.OCAP));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.OCAP_SOLUTION, Definition.CHART_COLUMN.OCAP_SOLUTION, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.OOC_SOLUTION), BISTel.eSPC.Common.ChartInfoDataType.OCAP));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.OCAP_COMMENT, Definition.CHART_COLUMN.OCAP_COMMENT, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.OOC_COMMENT), BISTel.eSPC.Common.ChartInfoDataType.OCAP));
            _chartInfoList.Add(new SPCStruct.ChartInfo(Definition.CHART_COLUMN.OCAP_FALSE_ALARM_YN, Definition.CHART_COLUMN.OCAP_FALSE_ALARM_YN, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.FALSE_ALARM_YN), BISTel.eSPC.Common.ChartInfoDataType.OCAP));
        }


        #region  ::: User Defined Method.


        private void AddDataRow(DataTable dtInfo, string colKey, string colDesc, string colValue)
        {
            DataRow dr  = dtInfo.NewRow();
            dr[Definition.SpreadHeaderColKey.INFO_KEY] = colDesc;
            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = colValue;
            dtInfo.Rows.Add(dr);
            this._sl.Add(colKey, this._sl.Count);         
        }

        private void CreateInfomationDataTable()
        {  
            DataRow dr = null;            
            DataTable dtInfo = new DataTable();   
                     
            dtInfo.Columns.Add(Definition.SpreadHeaderColKey.INFO_KEY, typeof(string));
            dtInfo.Columns.Add(Definition.SpreadHeaderColKey.INFO_VALUE, typeof(string));
            this._sl = new SortedList();
            int iRow = ChartVariable.dtParamData.Rows.Count -1;
            bool ocapCheck = false;
            bool dataExcuted = false;
            DataRow ocapDatarow = null;
            SPCStruct.ChartInfo _chartInfo = null;
            for (int i = 0; i < this._chartInfoList.Count; i++)
            {
                _chartInfo = this._chartInfoList[i] as SPCStruct.ChartInfo;                
                string strKey = _chartInfo.CODE;
                string strValue = _chartInfo.NAME;
                if (_chartInfo.DATA_TYPE != BISTel.eSPC.Common.ChartInfoDataType.DATA)
                {
                    dr = dtInfo.NewRow();
                    dr[Definition.SpreadHeaderColKey.INFO_KEY] = _chartInfo.DESC;
                    
                    if (_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.FIX)
                    {
                        switch (i)
                        {
                            case (int)BISTel.eSPC.Common.enum_ChartInfomationData.LINE: dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.LINE; break;
                            case (int)BISTel.eSPC.Common.enum_ChartInfomationData.AREA: dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.AREA; break;
                            case (int)BISTel.eSPC.Common.enum_ChartInfomationData.SPC_MODEL_NAME: dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.SPC_MODEL; break;
                            case (int)BISTel.eSPC.Common.enum_ChartInfomationData.PARAM_ALIAS: dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.PARAM_ALIAS; break;
                            default:
                                break;
                        }
                    }
                    else if (_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.CONTEXT || _chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.RELATED)
                    {
                        if (DataUtil.IsNullOrEmptyDataTable(ChartVariable.dtParamData)) break;

                        if (ChartVariable.dtParamData.Columns.Contains(strKey))
                        {
                            if (strKey == Definition.CHART_COLUMN.SUBSTRATE_ID)
                            {
                                if (ChartVariable.dtParamData.Rows[iRow][strKey].ToString().Split(';').Length > 1)
                                {
                                    dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.dtParamData.Rows[iRow][strKey].ToString().Replace(";", "\r\n");
                                }
                                else
                                {
                                    dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.dtParamData.Rows[iRow][strKey].ToString();
                                }
                            }
                            // JIRA SPC-615 [GF] Include Chart ID under "Information" column in SPC charts. - 2011.10.04 by ANDREW KO
                            // Add CHART_ID Info in ChartInfomation Spread : GF Communication Sheet 20110612 V1.21  - 2011.10.04 by ANDREW KO 
                            else if (strKey == Definition.COL_MODEL_CONFIG_RAWID)
                            {
                                dr[Definition.SpreadHeaderColKey.INFO_KEY] = Definition.CHART_COLUMN.CHART_ID;
                                dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.dtParamData.Rows[iRow][strKey].ToString();
                            }
                            else
                            {
                                dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ChartVariable.dtParamData.Rows[iRow][strKey].ToString();
                            }
                        }
                    }
                    else if(_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.OCAP)
                    {
                        if(ocapCheck == false)
                        {
                            ocapCheck = true;
                            if(ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                            {
                                string rawid = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.OCAP_RAWID].ToString();
                                string[] ids = rawid.Split(';');
                                List<string> splitedIDs = new List<string>();
                                foreach(string id in ids)
                                {
                                    if(string.IsNullOrEmpty(id)) continue;
                                    if(!splitedIDs.Contains(id))
                                        splitedIDs.Add(id);
                                }

                                if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST))
                                {
                                    rawid = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.P_OCAP_LIST].ToString();
                                    ids = rawid.Replace(";", "^").Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id)) continue;
                                        if (!splitedIDs.Contains(id))
                                            splitedIDs.Add(id);
                                    }
                                }

                                if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST))
                                {
                                    rawid = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString();
                                    ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id)) continue;
                                        if (!splitedIDs.Contains(id))
                                            splitedIDs.Add(id);
                                    }
                                }

                                if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST))
                                {
                                    rawid = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.C_OCAP_LIST].ToString();
                                    ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id)) continue;
                                        if (!splitedIDs.Contains(id))
                                            splitedIDs.Add(id);
                                    }
                                }

                                if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST))
                                {
                                    rawid = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.U_OCAP_LIST].ToString();
                                    ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id)) continue;
                                        if (!splitedIDs.Contains(id))
                                            splitedIDs.Add(id);
                                    }
                                }

                                if(splitedIDs.Count > 0)
                                {
                                    if(ChartVariable.dtOCAP != null && ChartVariable.dtOCAP.Rows.Count > 0)
                                    {
                                        DataRow[] drs = ChartVariable.dtOCAP.Select(Definition.COL_RAW_ID + " in ('" + string.Join("', '", splitedIDs.ToArray()) + "')");
                                        if(drs.Length >0)
                                        {
                                            ocapDatarow = drs[0];
                                        }
                                        foreach(DataRow datarow in drs)
                                        {
                                            if(!string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_PROBLEM].ToString())
                                                || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_CAUSE].ToString())
                                                || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_SOLUTION].ToString())
                                                || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_COMMENT].ToString()))
                                            {
                                                ocapDatarow = datarow;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if(ocapDatarow != null)
                        {
                            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = ocapDatarow[_chartInfo.CODE].ToString();
                        }
                        else
                        {
                            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = "NO DATA";
                        }
                    }

                    this._sl.Add(strKey, this._sl.Count);
                    dtInfo.Rows.Add(dr);
                }
                else
                {
                    if(dataExcuted == false)
                    {
                        dataExcuted = true;
                        string param_alias = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.PARAM_ALIAS].ToString();
                        string operation = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        string sUsl = "";
                        if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.C_USL))
                        {
                            sUsl = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.C_USL].ToString();
                        }
                        string sLsl = "";
                        if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.C_USL))
                        {
                            sLsl = ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.C_USL].ToString();
                            ChartVariable.dtParamData.Rows[iRow][Definition.CHART_COLUMN.C_USL].ToString();
                        }

                        double usl = double.NaN;
                        double lsl = double.NaN;

                        if (sUsl != null && sUsl != string.Empty)
                        {
                            string[] lSplitUsl = sUsl.Split(';');

                            if (lSplitUsl.Length > 0)
                            {
                                usl = double.Parse(lSplitUsl[0]);
                            }
                            else
                            {
                                usl = double.Parse(sUsl);
                            }
                        }

                        if (sLsl != null && sLsl != string.Empty)
                        {
                            string[] lSplitLsl = sLsl.Split(';');

                            if (lSplitLsl.Length > 0)
                            {
                                lsl = double.Parse(lSplitLsl[0]);
                            }
                            else
                            {
                                lsl = double.Parse(sLsl);
                            }
                        }

                        DataRow[] drSelect = new DataRow[1];
                        drSelect[0] = ChartVariable.dtParamData.Rows[iRow];

                        DataTable dt = DataUtil.DataTableImportRow(drSelect);

                        List<double> listRawData = comSPCStat.AddDataList(dt);
                        if (listRawData.Count == 0) continue;

                        comSPCStat.CalcPpk(listRawData.ToArray(), usl, lsl);

                        AddDataRow(dtInfo, Definition.CHART_COLUMN.AVG, this._mlthandler.GetVariable(Definition.CHART_COLUMN.AVG),
                                   comSPCStat.mean.ToString());
                        AddDataRow(dtInfo, Definition.CHART_COLUMN.PPK, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPK),
                                   comSPCStat.ppk.ToString());
                        AddDataRow(dtInfo, Definition.CHART_COLUMN.PP, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PP),
                                   comSPCStat.pp.ToString());
                        AddDataRow(dtInfo, Definition.CHART_COLUMN.PPL, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPL),
                                   comSPCStat.ppl.ToString());
                        AddDataRow(dtInfo, Definition.CHART_COLUMN.PPU, this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPU),
                                   comSPCStat.ppu.ToString());
                    }
                }
                
            }
            
            this.bsprData.ActiveSheet.RowCount = 0;
            this.bsprData.ActiveSheet.Columns[0].DataField = Definition.SpreadHeaderColKey.INFO_KEY;
            this.bsprData.ActiveSheet.Columns[1].DataField = Definition.SpreadHeaderColKey.INFO_VALUE;
            this.bsprData.DataSource = dtInfo;
            this.bsprData.ActiveSheet.ColumnHeader.Columns[0].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.INFO_KEY);
            this.bsprData.ActiveSheet.ColumnHeader.Columns[1].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.INFO_VALUE);
            this.bsprData.ActiveSheet.RowHeader.Visible = false;
           
            for (int i = 0, notContain = 0; i < this._chartInfoList.Count; i++)
            {
                _chartInfo = this._chartInfoList[i] as SPCStruct.ChartInfo;
                if(this.bsprData.ActiveSheet.Rows.Count <= i - notContain)
                    break;

                if(this.bsprData.ActiveSheet.Cells[i - notContain, 0].Text != _chartInfo.DESC)
                {
                    notContain++;
                    continue;
                }

                if (_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.TITLE)
                {
                    this.bsprData.ActiveSheet.Cells[i - notContain, 0].ColumnSpan = 2;
                    this.bsprData.ActiveSheet.Rows[i - notContain].Font = new Font("Gulim", 9, FontStyle.Bold);
                    this.bsprData.ActiveSheet.Rows[i - notContain].ForeColor = Color.White;
                    this.bsprData.ActiveSheet.Rows[i - notContain].BackColor = Color.SteelBlue;
                }
            }
            this.bsprData.ActiveSheet.Columns[0].BackColor = Color.WhiteSmoke;
            this.bsprData.ActiveSheet.Columns[1].BackColor = Color.White;
            this.bsprData.ActiveSheet.Columns[0].Width = 100;
            this.bsprData.ActiveSheet.Columns[1].Width = 150;
            this.bsprData.ActiveSheet.Columns[0].Locked = true;
            this.bsprData.ActiveSheet.Columns[1].Locked = true;

            FarPoint.Win.Spread.CellType.TextCellType tct = new FarPoint.Win.Spread.CellType.TextCellType();
            tct.Multiline = true;
            tct.MaxLength = 500;
            this.bsprData.ActiveSheet.Cells[0, 0, this.bsprData.ActiveSheet.RowCount - 1, this.bsprData.ActiveSheet.ColumnCount - 1].CellType = tct;

            for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
            {
                this.bsprData.ActiveSheet.Rows[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                this.bsprData.ActiveSheet.Rows[i].Height = this.bsprData.ActiveSheet.Rows[i].GetPreferredHeight();
                if (this.bsprData.ActiveSheet.Rows[i].Height > 20)
                {
                    this.bsprData.ActiveSheet.Rows[i].Height += 5;
                }
            }
        }


        /// <summary>
        /// series클릭시 값 변경
        /// (소스 정리 필히 해야함)
        /// </summary>
        /// <param name="row"></param>
        /// modified by enkim 2012.08.30 for ocap data
        public void InfomationSpreadReSet(DataRow row, string strChartName)
        {
            if (row == null) return;            
            bool bData = false;
            bool ocapCheck = false;
            DataRow ocapDatarow = null;

            SPCStruct.ChartInfo _chartInfo = null;
            bool dataExcuted = false;
            for (int i = 0; i < this._chartInfoList.Count; i++)
            {
                _chartInfo = this._chartInfoList[i] as SPCStruct.ChartInfo;
                if (_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.CONTEXT || _chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.RELATED)
                {
                    if (ChartVariable.dtParamData.Columns.Contains(_chartInfo.CODE))
                    {
                        if (_chartInfo.CODE == Definition.CHART_COLUMN.SUBSTRATE_ID)
                        {
                            if (row[_chartInfo.CODE].ToString().Split(';').Length > 1)
                            {
                                this.bsprData.ActiveSheet.Cells[i, 1].Text = row[_chartInfo.CODE].ToString().Replace(";", "\r\n");
                            }
                            else
                            {
                                this.bsprData.ActiveSheet.Cells[i, 1].Text = row[_chartInfo.CODE].ToString();
                            }
                        }
                        else
                        {
                            this.bsprData.ActiveSheet.Cells[i, 1].Text = row[_chartInfo.CODE].ToString();
                        }
                    }
                }
                else if (_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.DATA && dataExcuted == false)                          
                {      
                    dataExcuted = true;
                               
                    string param_alias = row[Definition.CHART_COLUMN.PARAM_ALIAS].ToString();
                    string operation = row[Definition.CHART_COLUMN.OPERATION_ID].ToString();
                    string sUsl = row[Definition.CHART_COLUMN.C_USL].ToString();
                    string sLsl = row[Definition.CHART_COLUMN.C_LSL].ToString();
                    double usl = double.NaN;
                    double lsl = double.NaN;
                   if (sUsl != null && sUsl != string.Empty)
                    {
                        string[] lSplitUsl = sUsl.Split(';');

                        if (lSplitUsl.Length > 0)
                        {
                            usl = double.Parse(lSplitUsl[0]);
                        }
                        else
                        {
                            usl = double.Parse(sUsl);
                        }
                    }

                    if (sLsl != null && sLsl != string.Empty)
                    {
                        string[] lSplitLsl = sLsl.Split(';');

                        if (lSplitLsl.Length > 0)
                        {
                            lsl = double.Parse(lSplitLsl[0]);
                        }
                        else
                        {
                            lsl = double.Parse(sLsl);
                        }
                    }

                    DataRow[] drSelect = new DataRow[1];
                    drSelect[0] = row;
                    DataTable dt = DataUtil.DataTableImportRow(drSelect);
                    List<double> listRawData = comSPCStat.AddDataList(dt);
                    if (listRawData.Count == 0) continue;
                    comSPCStat.CalcPpk(listRawData.ToArray(), usl, lsl);                                           
                                                                                  
                    this.bsprData.ActiveSheet.Cells[(int)_sl[this._mlthandler.GetVariable(Definition.CHART_COLUMN.AVG)], 1].Text = comSPCStat.mean.ToString();
                    this.bsprData.ActiveSheet.Cells[(int)_sl[this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPK)], 1].Text = comSPCStat.ppk.ToString();
                    this.bsprData.ActiveSheet.Cells[(int)_sl[this._mlthandler.GetVariable(Definition.CHART_COLUMN.PP)], 1].Text = comSPCStat.pp.ToString();
                    this.bsprData.ActiveSheet.Cells[(int)_sl[this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPL)], 1].Text = comSPCStat.ppl.ToString();
                    this.bsprData.ActiveSheet.Cells[(int)_sl[this._mlthandler.GetVariable(Definition.CHART_COLUMN.PPU)], 1].Text = comSPCStat.ppu.ToString();
                }
                else if(_chartInfo.DATA_TYPE == BISTel.eSPC.Common.ChartInfoDataType.OCAP)
                {
                    if(ocapCheck == false)
                    {
                        ocapCheck = true;
                        if(row.Table.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                        {
                            string rawid = row[Definition.CHART_COLUMN.OCAP_RAWID].ToString();
                            string[] ids = rawid.Split(';');
                            List<string> splitedIDs = new List<string>();
                            foreach (string id in ids)
                            {
                                if (string.IsNullOrEmpty(id)) continue;
                                if (!splitedIDs.Contains(id))
                                    splitedIDs.Add(id);
                            }

                            switch (strChartName)
                            {
                                case Definition.CHART_TYPE.P:
                                    if (row.Table.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST))
                                    {
                                        rawid = row[Definition.CHART_COLUMN.P_OCAP_LIST].ToString();
                                        ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                            {
                                                continue;
                                            }
                                            if (!splitedIDs.Contains(id))
                                            {
                                                splitedIDs.Add(id);
                                            }
                                        }
                                    }
                                    break;

                                case Definition.CHART_TYPE.PN:
                                    if (row.Table.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST))
                                    {
                                        rawid = row[Definition.CHART_COLUMN.PN_OCAP_LIST].ToString();
                                        ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                            {
                                                continue;
                                            }
                                            if (!splitedIDs.Contains(id))
                                            {
                                                splitedIDs.Add(id);
                                            }
                                        }
                                    }
                                    break;

                                case Definition.CHART_TYPE.C:
                                    if (row.Table.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST))
                                    {
                                        rawid = row[Definition.CHART_COLUMN.C_OCAP_LIST].ToString();
                                        ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                            { 
                                                continue; 
                                            }
                                            if (!splitedIDs.Contains(id))
                                            {
                                                splitedIDs.Add(id);
                                            }
                                        }
                                    }
                                    break;
                                
                                case Definition.CHART_TYPE.U:
                                    if (row.Table.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST))
                                    {
                                        rawid = row[Definition.CHART_COLUMN.U_OCAP_LIST].ToString();
                                        ids = rawid.Split('^');
                                        foreach (string id in ids)
                                        {
                                            if (string.IsNullOrEmpty(id))
                                            {
                                                continue;
                                            }
                                            if (!splitedIDs.Contains(id))
                                            {
                                                splitedIDs.Add(id);
                                            }
                                        }
                                    }
                                    break;
                            }

                            if (splitedIDs.Count > 0 && ChartVariable.dtOCAP != null)
                            {
                                DataRow[] drs =
                                    ChartVariable.dtOCAP.Select(Definition.COL_RAW_ID + " in ('" + string.Join("', '", splitedIDs.ToArray()) + "')");

                                if(drs.Length >0)
                                {
                                    ocapDatarow = drs[0];
                                }
                                foreach (DataRow datarow in drs)
                                {
                                    if (!string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_PROBLEM].ToString())
                                        || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_CAUSE].ToString())
                                        || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_SOLUTION].ToString())
                                        || !string.IsNullOrEmpty(datarow[Definition.CHART_COLUMN.OCAP_COMMENT].ToString())
                                        || "N" != datarow[Definition.CHART_COLUMN.OCAP_FALSE_ALARM_YN].ToString())
                                    {
                                        ocapDatarow = datarow;
                                    }
                                }
                            }
                        }
                    }

                    if(ocapDatarow != null)
                    {
                        this.bsprData.ActiveSheet.Cells[i, 1].Text = ocapDatarow[_chartInfo.CODE].ToString();
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[i, 1].Text = "NO DATA";
                    }
                }
            }

            for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
            {
                this.bsprData.ActiveSheet.Rows[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                this.bsprData.ActiveSheet.Rows[i].Height = this.bsprData.ActiveSheet.Rows[i].GetPreferredHeight();
                if (this.bsprData.ActiveSheet.Rows[i].Height > 20)
                {
                    this.bsprData.ActiveSheet.Rows[i].Height += 5;
                }
            }
        }
        //modified end

        #endregion 
        
        

        public BSpread bsprInformationData
        {
            get { return this.bsprData; }
            set { this.bsprData = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }
        }

        public DataSet dsChart
        {
            get { return this._dsChart; }
            set { this._dsChart = value; }
        }
                
        public SortedList sListInformation
        {
            get { return this._sl; }
            set { this._sl = value; }
        }



    }
}
