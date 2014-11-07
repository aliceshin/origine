using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public class EqpSummaryTrxDataParse
    {
        private string[] _lineInfos;
        private string[] _paramAlias;
        
        private string[] _units;
        private string[] _specSet = new string[] { "Target", "LSL", "LCL", "UCL", "USL", "FaultCode", "SpecType", "Comment" };
        private string _undefinedRecipe = "UNDEFINED_RECIPE";
        private string _undefinedLot = "UNDEFINED_LOT";

        private DataTable _defaultTable;

        private List<string> _listParamAlias;
        private List<string> _listStep;
        private List<int> _listSkipedStepIdx;
        
        private List<string> _listLotID;
        private List<int> _listSkipedLotIDIdx;
        private List<string> _listRecipe;
        private List<int> _listSkipedRecipeIdx;
        private List<string> _listProductID;

        private List<string> _listSlotID;

        CommonUtility _comUtil;

        // 2011.09.06 Dylan
        private bool _isCheckRSDCol = false;
        private Dictionary<string, string> _llstRSDCol = null;
        private List<string> _filterHeaders = null;

        private Dictionary<string, string> _contextBlobHeaderMapping = new Dictionary<string, string>();

        private AsyncCallManager _asyncManager = null; //SPC-929, KBLEE

        eSPCWebService.eSPCWebService _wsSPC = new eSPCWebService.eSPCWebService(); //SPC-929, KBLEE

        //SPC-929, KBLEE
        public AsyncCallManager ASYNCMANAGER
        {
            get { return _asyncManager; }
            set { _asyncManager = value; }
        }
        
        //SPC-929, KBLEE, START
        enum SummaryParseType
        {
            Recipe,
            RecipeStep,
            LotRecipe,
            LotRecipeStep,
            ModuleParam         // for view summary data grouping.
        }
        //SPC-929, KBLEE, END

        // 2011.09.06 Dylan
        public EqpSummaryTrxDataParse() 
        {
            //SPC-929, KBLEE, START
            this._comUtil = new CommonUtility();

            this._listParamAlias = new List<string>();

            this._listStep = new List<string>();
            this._listSkipedStepIdx = new List<int>();
            this._listLotID = new List<string>();
            this._listSkipedLotIDIdx = new List<int>();
            this._listRecipe = new List<string>();
            this._listSkipedRecipeIdx = new List<int>();
            this._listProductID = new List<string>();
            this._listSlotID = new List<string>();
            this._llstRSDCol = new Dictionary<string, string>();
            //SPC-929, KBLEE, END
        }

        //public EqpSummaryTrxDataParse(bool isCheckRSDCol)
        //{
        //    //this._ws = new eFDCWebService.eFDCWebService();

        //    this._comUtil = new CommonUtility();

        //    this._listParamAlias = new List<string>();

        //    this._listStep = new List<string>();
        //    this._listSkipedStepIdx = new List<int>();
        //    this._listLotID = new List<string>();
        //    this._listSkipedLotIDIdx = new List<int>();
        //    this._listRecipe = new List<string>();
        //    this._listSkipedRecipeIdx = new List<int>();
        //    this._listProductID = new List<string>();
        //    this._listSlotID = new List<string>();
        //    this._llstRSDCol = new Dictionary<string, string>();

        //    // isCheckRSDCol
        //    // true : USE_YN = 'Y' 인 RSD Context 만 가져온다
        //    // false : 모든 RSD Context 를 가져온다 (CODE 와 NAME 을 매칭하기 위함)
        //    this._isCheckRSDCol = isCheckRSDCol;

        //    //DataSet dsResult = (DataSet)Async.FDCWSCall("GetUsedRSDColumnList", isCheckRSDCol);

        //    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
        //    {
        //        _llstRSDCol.Add(dsResult.Tables[0].Rows[i][0].ToString(), dsResult.Tables[0].Rows[i][1].ToString());
        //    }

        //    dsResult.Dispose();
        //    dsResult = null;

        //    // BLOB Parsing 시 제외할 line_info 를 추가
        //    this._filterHeaders = new List<string>();
        //    this._filterHeaders.Add("lottype");
        //}


        public DataSet GetSummaryDataSet(LinkedList llstData)
        {
            if (llstData.Contains(Definition.CONDITION_KEY_SUM_CATEGORY_CD))
            {
                if (llstData[Definition.CONDITION_KEY_SUM_CATEGORY_CD].ToString() == "MP")
                {
                    // for view summary data grouping.
                    return this.GetSummaryDataSet(llstData, SummaryParseType.ModuleParam);
                }
                else if (llstData[Definition.CONDITION_KEY_SUM_CATEGORY_CD].ToString() == "STEP")
                {
                    return this.GetSummaryDataSet(llstData, SummaryParseType.RecipeStep);
                }
                else
                {
                    return this.GetSummaryDataSet(llstData, SummaryParseType.Recipe);
                }
            }
            else
            {
                return this.GetSummaryDataSet(llstData, SummaryParseType.RecipeStep);
            }
        }


        private DataSet GetSummaryDataSet(LinkedList llstData, SummaryParseType sumParseType)
        {
            /**
            * [R]equre, [O]ption
            * 
            * #1.CONTEXT 정보로 BLOB를 가져올 시
            * [ ] CONDITION_KEY_EQP_ID
            * [R] CONDITION_KEY_DCP_ID
            * [R] CONDITION_KEY_MODULE_ID
            * [O] CONDITION_KEY_LOT_ID             //LOT 단위 조회
            * [O] CONDITION_KEY_LOT_ID_LIST        //LOT LIST 조회             
            * [O] CONDITION_KEY_PRODUCT_ID
            * [R] CONDITION_KEY_RECIPE_ID
            * [R] CONDITION_KEY_START_DTTS
            * [R] CONDITION_KEY_END_DTTS
            * 
            * #2.LOT List DataSet으로 가져올 시 (DB Column 정보와 일치하여야함)
            * [R] CONDITION_KEY_DATASET
            * 
            * #3.RAWID로 BLOB DATA를 가져올 시
            * [R] CONDITION_KEY_RAWID OR CONDITION_KEY_PARAM_RAWID_LIST
            * 
            * ## 공통 옵션
            * [O] CONDITION_KEY_PARAM_ALIAS OR CONDITION_KEY_PARAM_ALIAS_LIST
            * [O] CONDITION_KEY_STEP_ID OR CONDITION_KEY_STEP_ID_LIST
            * */


            DataSet dsTraceDataSet = new DataSet();

            AsyncCallHandler ach = new AsyncCallHandler(_asyncManager); //SPC-929, KBLEE

            //DataSet dsFile = new DataSet();

            if (llstData[Definition.CONDITION_KEY_DATASET] != null)
            {
                DataSet dsLotSubstrateList = (DataSet)llstData[Definition.CONDITION_KEY_DATASET];

                if (DSUtil.CheckRowCount(dsLotSubstrateList))
                {
                    _comUtil.SortDataSet(dsLotSubstrateList, "START_DTTS", SortType.asc);

                    LinkedList llstFile = new LinkedList();

                    //bool hasSubstrateColumn = dsLotSubstrateList.Tables[0].Columns.Contains("SUBSTRATE_ID");

                    foreach (DataRow drLotSubstrate in dsLotSubstrateList.Tables[0].Rows)
                    {
                        llstFile.Clear();

                        if (dsLotSubstrateList.Tables[0].Columns.Contains("DCP_ID"))
                        {
                            llstFile.Add(Definition.CONDITION_KEY_DCP_ID, drLotSubstrate["DCP_ID"].ToString());
                        }

                        llstFile.Add(Definition.CONDITION_KEY_MODULE_ID, drLotSubstrate["MODULE_ID"].ToString());
                        llstFile.Add(Definition.CONDITION_KEY_PRODUCT_ID, drLotSubstrate["PRODUCT_ID"].ToString());
                        llstFile.Add(Definition.CONDITION_KEY_START_DTTS, drLotSubstrate["START_DTTS"]);
                        llstFile.Add(Definition.CONDITION_KEY_END_DTTS, drLotSubstrate["END_DTTS"]);
                        llstFile.Add(Definition.CONDITION_KEY_LOT_ID, drLotSubstrate["LOT_ID"].ToString());
                        llstFile.Add(Definition.CONDITION_KEY_RECIPE_ID, drLotSubstrate["RECIPE_ID"].ToString());
                        llstFile.Add(Definition.CONDITION_KEY_DATA_CATEGORY_CD, llstData[Definition.CONDITION_KEY_SUM_CATEGORY_CD].ToString());

                        //dsFile = _wsFDC.GetEqpSummaryTrxFileData(llstFile.GetSerialData());
                        //DataSet dsFile = Sync.FDCWS.GetEqpSummaryTrxFileData(llstFile.GetSerialData());
                        DataSet dsFile = (DataSet)ach.SendWait(_wsSPC, "GetEqpSummaryTrxFileData", new object[] {llstFile.GetSerialData()});

                        if (DSUtil.CheckRowCount(dsFile))
                        {
                            //dsTraceDataSet.Merge(this.GetSummaryDataSet(llstData, dsFile, sumParseType));
                            DataSet dsTemp = this.GetSummaryDataSet(llstData, dsFile, sumParseType);
                            //DataSet dsTemp = (DataSet)Async.FuncCall(this, "GetSummaryDataSet", llstData, dsFile, sumParseType);

                            dsTraceDataSet.Merge(dsTemp);

                            dsTemp.Dispose();
                            dsTemp = null;
                        }

                        dsFile.Dispose();
                        dsFile = null;
                    }

                    llstFile.Clear();
                    llstFile.Dispose();
                    llstFile = null;
                }
            }
            else
            {
                #region WebService에서 BLOB/CLOB Parsing 한다.
                //dsTraceDataSet = _wsFDC.GetEqpSummaryChartData(llstData.GetSerialData(), (object)sumParseType);
                #endregion

                #region CLIENT에서 BLOB/CLOB Parsing 한다.
                //dsFile = _wsFDC.GetEqpSummaryTrxFileData(llstData.GetSerialData());
                //DataSet dsFile = Sync.FDCWS.GetEqpSummaryTrxFileData(llstData.GetSerialData());
                //DataSet dsFile = (DataSet)ach.SendWait(_wsSPC, "GetEqpSummaryTrxFileData", new object[] {llstData.GetSerialData()});
                DataSet dsFile = _wsSPC.GetEqpSummaryTrxFileData(llstData.GetSerialData());

                //if (DSUtil.CheckRowCount(dsFile))
                if (dsFile.Tables.Count > 0)
                {
                    dsTraceDataSet = this.GetSummaryDataSet(llstData, dsFile, sumParseType);
                    //dsTraceDataSet = (DataSet)Async.FuncCall(this, "GetSummaryDataSet", llstData, dsFile, sumParseType);
                }

                dsFile.Dispose();
                dsFile = null;

                #endregion
            }

            return dsTraceDataSet;
        }

        // 2009.10.09.Sean. [parse recipe_id, lot_id from blob file]
        // lot, recipe 별로 table 생성하는 부분을 이전에 step 처리하던 부분 이용해서 단순 roop 반복함.
        // 작동 확인 후 바로 개선할 것!!!
        private DataSet GetSummaryDataSet(LinkedList llstData, DataSet dsFile, SummaryParseType sumParseType)
        {
            string[] saParamAlias = null;
            string[] saStep = null;
            string[] saLotId = null;
            string[] saRecipe = null;
            string[] saProductID = null;

            //PARAM ALIAS
            if (llstData[Definition.CONDITION_KEY_PARAM_ALIAS] != null)
            {
                saParamAlias = new string[1];
                saParamAlias[0] = llstData[Definition.CONDITION_KEY_PARAM_ALIAS].ToString();
            }

            if (llstData[Definition.CONDITION_KEY_PARAM_ALIAS_LIST] != null)
            {
                ArrayList alParamAliasList = (ArrayList)llstData[Definition.CONDITION_KEY_PARAM_ALIAS_LIST];
                saParamAlias = (string[])alParamAliasList.ToArray(typeof(string));
            }

            //STEP ID
            if (llstData[Definition.CONDITION_KEY_STEP_ID] != null)
            {
                if (llstData[Definition.CONDITION_KEY_STEP_ID].ToString() != "DEFAULT_ALL")
                {
                    saStep = new string[1];
                    saStep[0] = llstData[Definition.CONDITION_KEY_STEP_ID].ToString();
                }
            }

            if (llstData[Definition.CONDITION_KEY_STEP_ID_LIST] != null)
            {
                object obj = llstData[Definition.CONDITION_KEY_STEP_ID_LIST];
                if (obj is ArrayList)
                {
                    ArrayList alStepIDList = (ArrayList)obj;
                    saStep = (string[])alStepIDList.ToArray(typeof(string));
                }
                else if (obj is IEnumerable)
                {
                    saStep = (string[])obj;
                }
            }

            // 2009.10.09.Sean. [parse recipe_id, lot_id from blob file]
            //RECIPE ID
            if (llstData[Definition.CONDITION_KEY_RECIPE_ID] != null)
            {
                if (llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString() != "DEFAULT_ALL")
                {
                    saRecipe = new string[1];
                    saRecipe[0] = llstData[Definition.CONDITION_KEY_RECIPE_ID].ToString();
                }
            }
            if (llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST] != null)
            {
                object obj = llstData[Definition.CONDITION_KEY_RECIPE_ID_LIST];
                if (obj is ArrayList)
                {
                    ArrayList alRecipeIDList = (ArrayList)obj;
                    saRecipe = (string[])alRecipeIDList.ToArray(typeof(string));
                }
                else if (obj is IEnumerable)
                {
                    saRecipe = (string[])obj;
                }
            }
            //LOT ID
            if (llstData[Definition.CONDITION_KEY_LOT_ID] != null)
            {
                saLotId = new string[1];
                saLotId[0] = llstData[Definition.CONDITION_KEY_LOT_ID].ToString();
            }
            if (llstData[Definition.CONDITION_KEY_LOT_ID_LIST] != null)
            {
                ArrayList alLotIDList = (ArrayList)llstData[Definition.CONDITION_KEY_LOT_ID_LIST];
                saLotId = (string[])alLotIDList.ToArray(typeof(string));
            }
            //PRODUCT ID
            if (llstData[Definition.CONDITION_KEY_PRODUCT_ID] != null)
            {
                saProductID = new string[1];
                saProductID[0] = llstData[Definition.CONDITION_KEY_PRODUCT_ID].ToString();
            }

            DataSet dsSummary = null;

            try
            {
                if (DSUtil.CheckRowCount(dsFile))
                {
                    //BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();

                    string eqp_id = string.Empty;
                    string module_id = string.Empty;
                    string module_alias = string.Empty;

                    DataSet dsTempSummary = null;
                    DataSet dsTemp = new DataSet();
                    // 2009.10.29.Sean. report by Ha and Emily Kim. [merge dataset with same lot and different date]
                    dsSummary = new DataSet();


                    foreach (DataRow drFile in dsFile.Tables[0].Rows)
                    {
                        // Summary TRX Data.
                        eqp_id = drFile[Definition.BLOB_FIELD_NAME.EQP_ID].ToString();
                        module_id = drFile[Definition.BLOB_FIELD_NAME.MODULE_ID].ToString();
                        module_alias = drFile[Definition.BLOB_FIELD_NAME.MODULE_ALIAS].ToString();

                        //using (StreamReader srTrace = gZip.DecompressForStream(drFile["FILE_DATA"]))
                        StreamReader srTrace = CommonUtility.ConvertBLOBToStreamReader(drFile["FILE_DATA"]);

                        if (srTrace != null)
                        {
                            // recipe 별로 쪼개는 것은 공통이므로 먼저 처리.
                            dsTempSummary = this.BLOBParse(srTrace, sumParseType, saParamAlias, saStep, saRecipe, saProductID, saLotId, eqp_id, module_id, module_alias);

                            if (sumParseType != SummaryParseType.ModuleParam)
                            {
                                dsTemp = MakeDetailedDataSet(dsTempSummary, Definition.BLOB_FIELD_NAME.RECIPE, this._undefinedRecipe);
                            }
                            else
                            {
                                dsTemp = dsTempSummary;
                                dsTemp.Tables[0].TableName = saParamAlias[0] + "^" + module_id;
                            }

                            switch (sumParseType)
                            {
                                case SummaryParseType.Recipe:
                                case SummaryParseType.ModuleParam:
                                    //dsSummary = dsTemp;
                                    // 2009.10.29.Sean. report by Ha and Emily Kim. [merge dataset with same lot and different date]
                                    dsSummary.Merge(dsTemp);
                                    break;
                                case SummaryParseType.LotRecipe:
                                    // Lot 별로 쪼갠다.
                                    // 2009.10.29.Sean. report by Ha and Emily Kim. [merge dataset with same lot and different date]
                                    dsSummary.Merge(dsTemp);
                                    break;
                                case SummaryParseType.RecipeStep:
                                    // Step 별로 쪼갠다.
                                    DataSet dsTempStep = MakeDetailedDataSet(dsTemp, Definition.BLOB_FIELD_NAME.STEP, null);
                                    dsTemp.Clear();
                                    dsTemp.Dispose();
                                    dsTemp = null;
                                    // 2009.10.29.Sean. report by Ha and Emily Kim. [merge dataset with same lot and different date]
                                    dsSummary.Merge(dsTempStep);
                                    break;
                                case SummaryParseType.LotRecipeStep:
                                    // Step별로 쪼갠다.
                                    DataSet dsTempStep2 = MakeDetailedDataSet(dsTemp, Definition.BLOB_FIELD_NAME.STEP, null);
                                    dsTemp.Clear();
                                    dsTemp.Dispose();
                                    dsTemp = null;
                                    // Lot별로 쪼갠다.
                                    DataSet dsTempLot = MakeDetailedDataSet(dsTempStep2, Definition.BLOB_FIELD_NAME.LOTID, null);
                                    dsTempStep2.Clear();
                                    dsTempStep2.Dispose();
                                    dsTempStep2 = null;

                                    // 2009.10.29.Sean. report by Ha and Emily Kim. [merge dataset with same lot and different date]
                                    dsSummary.Merge(dsTempLot);
                                    break;
                            }
                            dsTempSummary.Clear();
                            dsTempSummary.Dispose();
                            dsTempSummary = null;
                        }
                    }
                }
                if (dsFile.Tables.Count > 1 && dsFile.Tables[1] != null && dsFile.Tables[1].Rows.Count > 0)
                {
                    bool isdtTempSummaryCloned = false;
                    DataTable dtTempSummary = new DataTable();

                    if (dsSummary != null && dsSummary.Tables.Count > 0 && dsSummary.Tables[0].Rows.Count > 0)
                    {
                        dtTempSummary = dsSummary.Tables[0].Clone();
                        isdtTempSummaryCloned = true;
                    }

                    if (dsSummary == null)
                    {
                        dsSummary = new DataSet();
                    }

                    ArrayList alParamAliasList = new ArrayList(saParamAlias);
                    this.CLOBParse(ref dtTempSummary, dsFile.Tables[1], alParamAliasList, isdtTempSummaryCloned, sumParseType);
                    DataSet dsTempSummary = new DataSet();
                    dsTempSummary.Tables.Add(dtTempSummary);
                    dsSummary.Merge(dsTempSummary);
                }
                //GC.Collect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsSummary;
        }

        private DataSet MakeDetailedDataSet(DataSet dsOrigin, string keyField, string replaceFlag)
        {
            DataSet dsDetailed = new DataSet();

            foreach (DataTable dtTempSummary in dsOrigin.Tables)
            {
                string[] arrDetail = _comUtil.ConvertDataColumnIntoArray(dtTempSummary, keyField, true);

                if (arrDetail != null && arrDetail.Length > 0)
                {
                    foreach (string detail in arrDetail)
                    {
                        DataRow[] drTempDetails = dtTempSummary.Select(String.Format("{0}='{1}'", keyField, detail));

                        if (drTempDetails != null && drTempDetails.Length > 0)
                        {
                            DataTable dtTempDetail = dtTempSummary.Clone();

                            if (replaceFlag != null && replaceFlag.Trim().Length > 0)
                            {
                                dtTempDetail.TableName = dtTempSummary.TableName.Replace(replaceFlag, detail);
                            }
                            else
                            {
                                dtTempDetail.TableName = dtTempSummary.TableName + "^" + detail;
                            }

                            foreach (DataRow drTemp in drTempDetails)
                            {
                                dtTempDetail.ImportRow(drTemp);
                            }

                            if (dsDetailed.Tables.Contains(dtTempDetail.TableName))
                            {
                                dsDetailed.Merge(dtTempDetail);
                            }
                            else
                            {
                                dsDetailed.Tables.Add(dtTempDetail.Copy());
                            }

                            dtTempDetail.Clear();
                            dtTempDetail.Dispose();
                            dtTempDetail = null;
                        }
                    }
                }

                dtTempSummary.Clear();
                dtTempSummary.Dispose();
            }
            return dsDetailed;
        }


        /// <summary>
        /// At this time, just suppert only one recipe or one lot for parsing 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="parseType"></param>
        /// <returns></returns>
        private DataSet BLOBParse(StreamReader sr, SummaryParseType sumParseType, string[] paramAlias, string[] step, string[] saRecipe, string[] saProductID, string[] saLotId, string eqp_id, string module_id, string module_alias)
        {
            this.Translate(paramAlias, this._listParamAlias);
            this.Translate(step, this._listStep);
            this.Translate(saLotId, this._listLotID);
            this.Translate(saRecipe, this._listRecipe);
            this.Translate(saProductID, this._listProductID);

            this.ParseHeader(sr);
            this.MakeDefaultDataTable();
            this.ParseLineInfo(sr);

            this._defaultTable.Columns.Add(Definition.BLOB_FIELD_NAME.SUM_VALUE, typeof(Double));
            this._defaultTable.Columns.Add(Definition.BLOB_FIELD_NAME.MODULE_ID);
            this._defaultTable.Columns.Add(Definition.BLOB_FIELD_NAME.EQP_ID);
            this._defaultTable.Columns.Add(Definition.BLOB_FIELD_NAME.MODULE_ALIAS);
            this._defaultTable.Columns.Add(Definition.BLOB_FIELD_NAME.PARAM_ALIAS);

            //============================================================================
            // SpecArray Column 생성
            // 2011.09.07 Dylan
            // Summary Data View 에서 Target, LSL, LCL, UCL, USL, FaultCode, SpecType 제거
            // 'ParseParameter' Method 에서 Spec 값을 저장하는 부분과 연계하여 사용할 것

            //if (this._specSet != null && this._specSet.Length > 0)
            //{
            //    foreach (string specType in this._specSet)
            //    {
            //        this._defaultTable.Columns.Add(specType);
            //    }
            //}
            //============================================================================

            DataSet dsTrace = null;
            dsTrace = this.ParseParameter(sr, sumParseType, eqp_id, module_id, module_alias);

            //Resource Clear
            //_listRAWID.Clear();
            //_listParamName.Clear();
            _listParamAlias.Clear();

            _listStep.Clear();
            _listSkipedStepIdx.Clear();
            // 2009.10.29.Sean. report by Emily Kim. [clear garbage resoure _listRecipe]
            _listRecipe.Clear();
            _listSkipedRecipeIdx.Clear();

            _listSlotID.Clear();

            _defaultTable.Clear();
            _defaultTable.Dispose();
            _defaultTable = null;

            _lineInfos = null;
            _paramAlias = null;

            return dsTrace;
        }

        /// <summary>
        /// At this time, just suppert only one recipe or one lot for parsing 
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="parseType"></param>
        /// <returns></returns>
        //private DataSet Parse(StreamReader sr, TraceParseType parseType, string[] paramRawIDs, string[] paramNames, string[] paramAlias, string module_id, string module_alias)
        private void CLOBParse(ref DataTable _dsTempSummary, DataTable _dsCurrentSummaryData, ArrayList alParamAliasList, bool isdtTempSummaryCloned, SummaryParseType sumParseType)
        {
            for (int i = _dsCurrentSummaryData.Columns.Count - 1; -1 < i; i--)
            {
                if (_isCheckRSDCol)
                {
                    if (_dsCurrentSummaryData.Columns[i].ColumnName.StartsWith("RSD_", true, null))
                    {
                        if (_llstRSDCol.ContainsKey(_dsCurrentSummaryData.Columns[i].ColumnName))
                        {
                            _dsCurrentSummaryData.Columns[i].ColumnName = _llstRSDCol[_dsCurrentSummaryData.Columns[i].ColumnName];
                        }
                        else
                        {
                            _dsCurrentSummaryData.Columns.Remove(_dsCurrentSummaryData.Columns[i].ColumnName);
                        }

                        continue;
                    }
                }

                //switch (_dsCurrentSummaryData.Columns[i].ColumnName)
                //{
                //    case "LOT_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "lotid";
                //        break;

                //    case "LOT_TYPE_CD":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "lottype";
                //        break;

                //    case "SUBSTRATE_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "substrateid";
                //        break;

                //    case "RECIPE_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "recipe";
                //        break;

                //    case "SUM_DTTS":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "time";
                //        break;

                //    case "OPERATION_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "operation";
                //        break;

                //    case "PRODUCT_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "product";
                //        break;

                //    case "CASSETTE_SLOT":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "slot";
                //        break;

                //    case "STEP_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "step";
                //        break;

                //    case "MODULE_ID":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "module_id";
                //        break;

                //    case "MODULE_ALIAS":
                //        _dsCurrentSummaryData.Columns[i].ColumnName = "module_alias";
                //        break;
                //}

            }

            _dsCurrentSummaryData.Columns.Add(Definition.BLOB_FIELD_NAME.PARAM_ALIAS);
            _dsCurrentSummaryData.Columns.Add(Definition.BLOB_FIELD_NAME.SUM_VALUE, typeof(Double));

            if (!isdtTempSummaryCloned)
            {
                _dsTempSummary = _dsCurrentSummaryData.Clone();
            }

            foreach (DataRow dr in _dsCurrentSummaryData.Rows)
            {
                string strFileData = dr["FILE_DATA"].ToString();

                //string[] sArrFileDatas = strFileData.Split('@'); 
                string[] sArrFileDatas = strFileData.Split('\t');

                for (int k = 0; k < sArrFileDatas.Length; k++)
                {
                    //string sTempParam = sArrFileDatas[k].Split('=')[0].Trim(); 
                    //string sTempParamValue = sArrFileDatas[k].Split('=')[1].Trim(); 
                    string[] sTempParamData = sArrFileDatas[k].Split('@');
                    string sTempParam = sTempParamData[0].Split('=')[0].Trim();
                    string sTempParamValue = sTempParamData[0].Split('=')[1].Trim();

                    if (alParamAliasList.Contains(sTempParam))
                    {
                        dr[Definition.BLOB_FIELD_NAME.PARAM_ALIAS] = sTempParam;
                        //dr[Definition.BLOB_FIELD_NAME.SUM_VALUE] = sTempParamValue; 
                        //dr[Definition.BLOB_FIELD_NAME.SUM_VALUE] = _ComUtil.IsDouble(sTempParamValue) ? EESUtil.ConvertStringToNumber(sTempParamValue).ToString() : double.NaN.ToString();
                        dr[Definition.BLOB_FIELD_NAME.SUM_VALUE] = _comUtil.IsDouble(sTempParamValue) ? EESUtil.ConvertStringToNumber(sTempParamValue) : double.NaN;
                        _dsTempSummary.ImportRow(dr);
                        break;
                    }
                }
                //_dsTempSummary.ImportRow(dr); 
            }

            if (!isdtTempSummaryCloned)
            {
                _dsTempSummary.Columns.Remove("FILE_DATA");

                _dsTempSummary.TableName = _dsTempSummary.Rows[0][Definition.BLOB_FIELD_NAME.PARAM_ALIAS].ToString()
                                    + "^" + _dsTempSummary.Rows[0][Definition.BLOB_FIELD_NAME.MODULE_ID].ToString()
                                    + "^" + _dsTempSummary.Rows[0][Definition.BLOB_FIELD_NAME.RECIPE].ToString()
                                    + "^" + _dsTempSummary.Rows[0][Definition.BLOB_FIELD_NAME.STEP].ToString();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        //private void PrepareHeader(StreamReader sr)
        private void ParseHeader(StreamReader sr)
        {
            string line = string.Empty;

            // skip open tag of <header>
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();

                if (line.IndexOf(Definition.BLOB_FIELD_NAME.HEADER_OPENING) > -1)
                {
                    break;
                }
            }

            // string of header
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();

                if (line.IndexOf(Definition.BLOB_FIELD_NAME.HEADER_CLOSING) > -1)
                {
                    break;
                }

                if (line.IndexOf(Definition.BLOB_FIELD_NAME.LINE_INFO) > -1)
                {
                    this._lineInfos = ParseHeader(line);
                }
            }
        }

        private string[] ParseHeader(string source)
        {
            string[] name_value = source.Split('=');

            if (name_value.Length == 2)
            {
                if (name_value[1].Length > 0)
                {
                    // 2010.09.06 Dylan 추가
                    // 사용하는 RSD 컬럼, Filter 되지 않는 컬럼만 테이블에 추가  ==========
                    return this.FilterHeader(name_value[1]);
                    //return name_value[1].Trim().Split('\t');
                }
            }

            return null;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseType"></param>
        private void MakeDefaultDataTable()
        {
            this._defaultTable = new DataTable();

            if (this._lineInfos.Length > 0)
            {
                foreach (string item in this._lineInfos)
                {
                    // 2011.09.06 Dylan : RSD 컬럼은 CODE 가 아닌, NAME 으로 컬럼명을 생성
                    if (item.Equals(Definition.BLOB_FIELD_NAME.TIME))
                    {
                        this._defaultTable.Columns.Add(item);
                        this._defaultTable.Columns[Definition.BLOB_FIELD_NAME.TIME].DataType = typeof(DateTime);
                    }
                    else if (item.ToUpper().StartsWith("RSD_") && this._llstRSDCol.ContainsKey(item.ToUpper()))
                    {
                        this._defaultTable.Columns.Add(this._llstRSDCol[item.ToUpper()]);
                    }
                    else
                    {
                        this._defaultTable.Columns.Add(item);
                    }
                    //this._defaultTable.Columns.Add(item);

                    //if (item.Equals(Definition.BLOB_FIELD_NAME.TIME))
                    //{
                    //    this._defaultTable.Columns[Definition.BLOB_FIELD_NAME.TIME].DataType = typeof(DateTime);
                    //}
                }
            }
        }





        /// <summary>
        /// filtering step, lot, recipe.
        /// </summary>
        /// <param name="sr"></param>
        private void ParseLineInfo(StreamReader sr)
        {
            string[][] lineInfoDatas = new string[this._lineInfos.Length][];

            string line = string.Empty;

            for (int i = 0; i < this._lineInfos.Length && sr.Peek() > -1; /*i++*/) // 2011.09.06 Dylan
            {
                line = sr.ReadLine();

                if (line.IndexOf(this._lineInfos[i]) > -1)
                {
                    lineInfoDatas[i] = line.Split('\t');
                    i++; // 2011.09.06 Dylan : line_info Skip 시 index 가 넘어가지 않도록 함
                }
            }

            if (lineInfoDatas.Length < 1)
            {
                return;
            }

            if (lineInfoDatas[0].Length > 0)
            {
                DataRow dr = null;

                for (int row = 1; row < lineInfoDatas[0].Length; row++)
                {
                    try
                    {
                        dr = this._defaultTable.NewRow();

                        for (int col = 0; col < lineInfoDatas.Length; col++)
                        {
                            if (lineInfoDatas[col][0].Equals(Definition.BLOB_FIELD_NAME.TIME))
                            {
                                dr[col] = EESUtil.ConvertStringToDateTime(lineInfoDatas[col][row]);
                            }
                            else if (lineInfoDatas[col][0].Equals(Definition.BLOB_FIELD_NAME.STEP))
                            {
                                //특정 Step의 Data만 가져오길 원할때 불필요한 Step Skip
                                if (_listStep.Count > 0)
                                {
                                    if (!_listStep.Contains(lineInfoDatas[col][row]))
                                    {
                                        this._listSkipedStepIdx.Add(row);
                                        break;
                                    }
                                }

                                dr[col] = lineInfoDatas[col][row];
                            }
                            else if (lineInfoDatas[col][0].Equals(Definition.BLOB_FIELD_NAME.RECIPE))
                            {
                                //특정 RECIPE의 Data만 가져오길 원할때 불필요한 RECIPE Skip
                                if (_listRecipe.Count > 0)
                                {
                                    if (!_listRecipe.Contains(lineInfoDatas[col][row]))
                                    {
                                        this._listSkipedRecipeIdx.Add(row);
                                        break;
                                    }
                                }

                                dr[col] = lineInfoDatas[col][row];
                            }
                            else if (lineInfoDatas[col][0].Equals(Definition.BLOB_FIELD_NAME.PRODUCT))
                            {
                                //특정 PRODUCT의 Data만 가져오길 원할때 불필요한 PRODUCT Skip
                                if (_listProductID.Count > 0)
                                {
                                    if (!_listProductID.Contains(lineInfoDatas[col][row]))
                                    {
                                        this._listSkipedRecipeIdx.Add(row);
                                        break;
                                    }
                                }

                                dr[col] = lineInfoDatas[col][row];
                            }
                            else if (lineInfoDatas[col][0].Equals(Definition.BLOB_FIELD_NAME.LOTID))
                            {
                                //특정 LOT의 Data만 가져오길 원할때 불필요한 LOT Skip
                                if (_listLotID.Count > 0)
                                {
                                    if (!_listLotID.Contains(lineInfoDatas[col][row]))
                                    {
                                        this._listSkipedLotIDIdx.Add(row);
                                        break;
                                    }
                                }

                                dr[col] = lineInfoDatas[col][row];
                            }
                            else
                            {
                                dr[col] = lineInfoDatas[col][row];
                            }
                        }

                        if (this._listStep.Count > 0 && this._listSkipedStepIdx.Contains(row))
                            continue;
                        if (this._listRecipe.Count > 0 && this._listSkipedRecipeIdx.Contains(row))
                            continue;
                        if (this._listLotID.Count > 0 && this._listSkipedLotIDIdx.Contains(row))
                            continue;

                        this._defaultTable.Rows.Add(dr);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }


        private void Translate(string[] sourceArray, List<string> targetList)
        {
            targetList.Clear();

            if (sourceArray == null)
            {
                return;
            }

            if (sourceArray.Length == 0)
            {
                return;
            }

            // 2009.10.29.Sean. report by Edwin Song. [skip blank data]
            foreach (string source in sourceArray)
            {
                if (source.Trim().Length > 0) targetList.Add(source);
            }
        }

        private DataSet ParseParameter(StreamReader sr, SummaryParseType sumParseType, string eqp_id, string module_id, string module_alias)
        {
            DataSet dsParameter = new DataSet();
            string sErrorParam = "";

            try
            {
                string line = string.Empty;
                string recipe = string.Empty;
                string sumValue = string.Empty;
                string[] values = null;
                string[] valueUnitArr = null;
                string[] specValue = null;

                // recipe 를 지정하지 않았으면, 첫번째 recipe만 처리.
                //if (this._listRecipe == null || this._listRecipe.Count < 1) recipe = _undefinedRecipe;
                //else recipe = this._listRecipe[0];

                for (int i = 0; sr.Peek() > -1; i++)
                {
                    if (this._listParamAlias[i].Length > 0)
                    {
                        // 2011.09.06 : Dylan
                        // RSD 의 USE_YN = 'Y' 인 경우, RSD Value 를 Skip 하면서 멈춰버린 StreamReader 의 Seek 값을 변경
                        // (Parameter 의 Value 값을 읽어오는 곳으로 이동시키기 위함)
                        //line = sr.ReadLine();
                        while (!(line = sr.ReadLine()).Contains("value")) ;

                        DataTable paramTable = this._defaultTable.Copy();

                        switch (sumParseType)
                        {
                            case SummaryParseType.Recipe:
                            case SummaryParseType.RecipeStep:
                                paramTable.TableName = string.Format("{0}^{1}^{2}", this._listParamAlias[i], module_id, _undefinedRecipe); //recipe);
                                break;
                            case SummaryParseType.LotRecipe:
                            case SummaryParseType.LotRecipeStep:
                                paramTable.TableName = string.Format("{0}^{1}^{2}^{3}", this._listParamAlias[i],
                                                                                        module_id,
                                                                                        _undefinedLot, //paramTable.Rows[0][Definition.BLOB_FIELD_NAME.LOTID].ToString(),
                                                                                        recipe);
                                break;
                        }


                        values = line.Split('\t');

                        if (values.Length > 1)
                        {
                            if (this._listStep.Count > 0)
                            {
                                int tidx = 0;

                                for (int row = 1; row < values.Length; row++)
                                {
                                    if (this._listSkipedStepIdx.Contains(row))
                                    {
                                        continue;
                                    }
                                    if (this._listSkipedRecipeIdx.Contains(row))
                                    {
                                        continue;
                                    }

                                    if (paramTable.Rows.Count > tidx)
                                    {
                                        //EQP_ID, MODULE ID, MODULE ALIAS
                                        paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.EQP_ID] = eqp_id;
                                        paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.MODULE_ID] = module_id;
                                        paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.MODULE_ALIAS] = module_alias;

                                        //PARAM ALIAS
                                        if (this._listParamAlias != null && this._listParamAlias.Count > 0)
                                        {
                                            paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.PARAM_ALIAS] = this._listParamAlias[i];
                                        }

                                        //PARAM SUM VALUE
                                        valueUnitArr = values[row].Split('@');
                                        if (valueUnitArr != null && valueUnitArr.Length > 0)
                                        {
                                            sumValue = valueUnitArr[0];
                                            //paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _ComUtil.NVL(sumValue, double.NaN.ToString(), true);
                                            //paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _ComUtil.IsDouble(sumValue) ? EESUtil.ConvertStringToNumber(sumValue).ToString() : double.NaN.ToString();
                                            paramTable.Rows[tidx][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _comUtil.IsDouble(sumValue) ? EESUtil.ConvertStringToNumber(sumValue) : double.NaN;

                                            //====================================================================================
                                            // 2011.09.07 Dylan : Summary Data View 테이블 에서
                                            // Target, LSL, LCL, UCL, USL, FaultCode, SpecType 컬럼 제거에 따른 Spec 값 저장 제거
                                            // 'BLOBParse' Method 의: SpecArray Column 생성 부분과 연계하여 사용할 것

                                            //specValue = valueUnitArr[1].Split('^');
                                            //if (specValue != null && specValue.Length > 0)
                                            //{
                                            //    for (int si = 0; si < this._specSet.Length; si++)
                                            //    {
                                            //        // 2009.10.07 Emily
                                            //        // spec값이 없는 경우에 exception이 발생하여 조건문 추가.
                                            //        if (specValue.Length > si)
                                            //            paramTable.Rows[tidx][this._specSet[si]] = _ComUtil.IsDouble(specValue[si]) ? EESUtil.ConvertStringToNumber(specValue[si]).ToString() : double.NaN.ToString();
                                            //        //paramTable.Rows[tidx][this._specSet[si]] = _ComUtil.NVL(specValue[si], double.NaN.ToString(), true);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    for (int si = 0; si < this._specSet.Length; si++)
                                            //    {
                                            //        paramTable.Rows[tidx][this._specSet[si]] = double.NaN.ToString();
                                            //    }
                                            //}

                                            //====================================================================================
                                        }
                                        tidx++;
                                    }
                                }
                            }
                            else
                            {
                                int iRowIndex = 0;

                                for (int row = 1; row < values.Length; row++)
                                {
                                    //
                                    if (this._listSkipedRecipeIdx.Contains(row))
                                    {
                                        continue;
                                    }
                                    //if (paramTable.Rows.Count > row - 1)
                                    //{

                                    //EQP_ID, MODULE ID, MODULE ALIAS
                                    paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.EQP_ID] = eqp_id;
                                    paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.MODULE_ID] = module_id;
                                    paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.MODULE_ALIAS] = module_alias;

                                    //PARAM ALIAS
                                    if (this._listParamAlias != null && this._listParamAlias.Count > 0)
                                    {
                                        paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.PARAM_ALIAS] = this._listParamAlias[i];
                                    }

                                    //PARAM SUM VALUE
                                    valueUnitArr = values[row].Split('@');

                                    if (valueUnitArr != null && valueUnitArr.Length > 0)
                                    {
                                        sumValue = valueUnitArr[0];

                                        //paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _ComUtil.NVL(sumValue, double.NaN.ToString(), true);
                                        //paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _ComUtil.IsDouble(sumValue) ? EESUtil.ConvertStringToNumber(sumValue).ToString() : double.NaN.ToString();

                                        paramTable.Rows[iRowIndex][Definition.BLOB_FIELD_NAME.SUM_VALUE] = _comUtil.IsDouble(sumValue) ? EESUtil.ConvertStringToNumber(sumValue) : double.NaN;


                                        //====================================================================================
                                        // 2011.09.07 Dylan : Summary Data View 테이블 에서
                                        // Target, LSL, LCL, UCL, USL, FaultCode, SpecType 컬럼 제거에 따른 Spec 값 저장 제거
                                        // 'BLOBParse' Method 의: SpecArray Column 생성 부분과 연계하여 사용할 것

                                        //specValue = valueUnitArr[1].Split('^');
                                        //if (specValue != null && specValue.Length >= this._specSet.Length)
                                        //{
                                        //    for (int si = 0; si < this._specSet.Length; si++)
                                        //    {
                                        //        //paramTable.Rows[iRowIndex][this._specSet[si]] = _ComUtil.NVL(specValue[si], double.NaN.ToString(), true);
                                        //        paramTable.Rows[iRowIndex][this._specSet[si]] = _ComUtil.IsDouble(specValue[si]) ? EESUtil.ConvertStringToNumber(specValue[si]).ToString() : double.NaN.ToString();
                                        //    }
                                        //}
                                        //else
                                        //{   
                                        //    for (int si = 0; si < this._specSet.Length; si++)
                                        //    {
                                        //        paramTable.Rows[iRowIndex][this._specSet[si]] = double.NaN.ToString();
                                        //    }
                                        //}
                                        //====================================================================================
                                    }
                                    //}
                                    iRowIndex++;
                                }
                            }

                            if (dsParameter.Tables.Contains(paramTable.TableName))
                            {
                                dsParameter.Merge(paramTable);
                            }
                            else
                            {
                                dsParameter.Tables.Add(paramTable.Copy());
                            }

                            paramTable.Clear();
                            paramTable.Dispose();
                            paramTable = null;
                        }
                    }
                }
            }
            catch (FormatException fex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, fex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);

            }
            return dsParameter;
        }


        private DataSet ConvertTableForSlot(DataSet dsParam)
        {
            DataSet ds = new DataSet();

            try
            {
                foreach (DataTable dt in dsParam.Tables)
                {
                    for (int i = 0; i < this._listSlotID.Count; i++)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp.Merge(dt.Select(String.Format("{0}='{1}'", Definition.BLOB_FIELD_NAME.SLOT, this._listSlotID[i])));

                        if (dsTemp.Tables.Count > 0)
                        {
                            if (ds.Tables.Contains(this._listSlotID[i]))
                            {
                                ds.Tables[this._listSlotID[i]].Merge(dsTemp.Tables[0].Copy());
                            }
                            else
                            {
                                DataTable newTable = dsTemp.Tables[0].Copy();
                                newTable.TableName = this._listSlotID[i];
                                ds.Tables.Add(newTable);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return ds;
        }



        //BLOB Info   

        #region BLOB Conetext Info Parsing

        //public DataSet GetContextInfo(LinkedList llstData)
        //{
        //    //if (this._ws == null) this._ws = new WebServiceController<eFDCWebService.eFDCWebService>().Create();

        //    /**
        //     * [R]equre, [O]ption
        //     * 
        //     * #1.CONTEXT 정보로 BLOB를 가져올 시
        //     * [ ] CONDITION_KEY_EQP_RAWID
        //     * [R] CONDITION_KEY_MODULE_RAWID
        //     * [R] CONDITION_KEY_LOT_ID             //LOT 단위 조회
        //     * [O] CONDITION_KEY_LOT_ID_LIST        //LOT LIST 조회
        //     * [O] CONDITION_KEY_SLOT               //SLOT 단위 조희
        //     * [O] CONDITION_KEY_SUBSTRATE_ID       //SUBSTRATE 단위 조회
        //     * [ ] CONDITION_KEY_PRODUCT_ID
        //     * [R] CONDITION_KEY_RECIPE_ID
        //     * [R] CONDITION_KEY_START_DTTS
        //     * [R] CONDITION_KEY_END_DTTS
        //     * 
        //     * #2.LOT_SUBSTRATE DataSet으로 가져올 시 (DB Column 정보와 일치하여야함)
        //     * [R] CONDITION_KEY_DATASET
        //     * 
        //     * #3.RAWID로 BLOB DATA를 가져올 시
        //     * [R] CONDITION_KEY_RAWID OR CONDITION_KEY_PARAM_RAWID_LIST
        //     * 
        //     * ## 공통 옵션
        //     * [O] CONDITION_KEY_TRACE_PARSE_TYPE
        //     * [ ] CONDITION_KEY_PARAM_NAME OR CONDITION_KEY_PARAM_NAME_LIST
        //     * [O] CONDITION_KEY_PARAM_RAWID OR CONDITION_KEY_PARAM_RAWID_LIST
        //     * [O] CONDITION_KEY_STEP_ID
        //     * */

        //    //DataSet dsTraceDataSet = new DataSet();


        //    DataSet dsTraceInfo = new DataSet();

        //    DataTable dtParamter = new DataTable(Definition.BLOB_FIELD_NAME.PARAM_INFO);
        //    //dtParamter.Columns.Add(Definition.BLOB_FIELD_NAME.PARAM_RAWID);
        //    //dtParamter.Columns.Add(Definition.BLOB_FIELD_NAME.PARAM_NAME);
        //    dtParamter.Columns.Add(Definition.BLOB_FIELD_NAME.PARAM_ALIAS);
        //    //dtParamter.Columns.Add(Definition.BLOB_FIELD_NAME.DATA_TYPE_CD);
        //    dtParamter.Columns.Add(Definition.BLOB_FIELD_NAME.UNIT);

        //    DataTable dtSubstrate = new DataTable(Definition.BLOB_FIELD_NAME.SUBSTRATE_INFO);
        //    dtSubstrate.Columns.Add(Definition.BLOB_FIELD_NAME.SLOT);
        //    dtSubstrate.Columns.Add(Definition.BLOB_FIELD_NAME.SUBSTRATEID);

        //    DataTable dtStep = new DataTable(Definition.BLOB_FIELD_NAME.STEP_INFO);
        //    dtStep.Columns.Add(Definition.BLOB_FIELD_NAME.STEP);
        //    dtStep.Columns.Add(Definition.BLOB_FIELD_NAME.STEPNAME);

        //    dsTraceInfo.Tables.Add(dtParamter);
        //    dsTraceInfo.Tables.Add(dtSubstrate);
        //    dsTraceInfo.Tables.Add(dtStep);



        //    //DataSet dsFile = new DataSet();

        //    if (llstData[Definition.CONDITION_KEY_DATASET] != null)
        //    {
        //        DataSet dsLotSubstrateList = (DataSet)llstData[Definition.CONDITION_KEY_DATASET];

        //        if (DSUtil.CheckRowCount(dsLotSubstrateList))
        //        {
        //            _comUtil.SortDataSet(dsLotSubstrateList, "START_DTTS", SortType.asc);

        //            LinkedList llstFile = new LinkedList();

        //            bool hasSubstrateColumn = dsLotSubstrateList.Tables[0].Columns.Contains("SUBSTRATE_ID");

        //            foreach (DataRow drLotSubstrate in dsLotSubstrateList.Tables[0].Rows)
        //            {
        //                llstFile.Clear();
        //                //llstFile.Add(Definition.CONDITION_KEY_MODULE_RAWID, drLotSubstrate["EQP_RAWID"].ToString());
        //                llstFile.Add(Definition.CONDITION_KEY_MODULE_RAWID, drLotSubstrate["MODULE_ID"].ToString());
        //                llstFile.Add(Definition.CONDITION_KEY_LOT_ID, drLotSubstrate["LOT_ID"].ToString());

        //                if (hasSubstrateColumn)
        //                {
        //                    llstFile.Add(Definition.CONDITION_KEY_SUBSTRATE_ID, _comUtil.NVL(drLotSubstrate["SUBSTRATE_ID"]));
        //                }

        //                llstFile.Add(Definition.CONDITION_KEY_PRODUCT_ID, drLotSubstrate["PRODUCT_ID"].ToString());
        //                llstFile.Add(Definition.CONDITION_KEY_RECIPE_ID, drLotSubstrate["RECIPE_ID"].ToString());
        //                llstFile.Add(Definition.CONDITION_KEY_START_DTTS, drLotSubstrate["START_DTTS"]);
        //                llstFile.Add(Definition.CONDITION_KEY_END_DTTS, drLotSubstrate["END_DTTS"]);

        //                //dsFile = this._ws.GetTraceTrxFileData(llstFile.GetSerialData());
        //                //DataSet dsFile = Sync.FDCWS.GetTraceTrxFileData(llstFile.GetSerialData());
        //                DataSet dsFile = (DataSet)Async.FDCWSCall("GetTraceTrxFileData", llstFile.GetSerialData());

        //                if (DSUtil.CheckRowCount(dsFile))
        //                {
        //                    this.GetTraceInfo(dsTraceInfo, dsFile);
        //                    //Async.FuncCall(this, "GetTraceInfo", dsTraceInfo, dsFile);
        //                }

        //                dsFile.Dispose();
        //                dsFile = null;
        //            }

        //            llstFile.Clear();
        //            llstFile.Dispose();
        //            llstFile = null;
        //        }
        //    }
        //    else
        //    {
        //        //dsFile = this._ws.GetTraceTrxFileData(llstData.GetSerialData());
        //        //DataSet dsFile = Sync.FDCWS.GetTraceTrxFileData(llstData.GetSerialData());
        //        DataSet dsFile = (DataSet)Async.FDCWSCall("GetTraceTrxFileData", llstData.GetSerialData());

        //        if (DSUtil.CheckRowCount(dsFile))
        //        {
        //            this.GetTraceInfo(dsTraceInfo, dsFile);
        //            //Async.FuncCall(this, "dsTraceInfo", dsFile);
        //        }

        //        dsFile.Dispose();
        //        dsFile = null;
        //    }

        //    _comUtil.SortDataSet(dsTraceInfo, Definition.BLOB_FIELD_NAME.PARAM_NAME, SortType.asc);
        //    //_ComUtil.SortDataSet(dsTraceInfo, Definition.BLOB_FIELD_NAME.SLOT, SortType.asc);
        //    //_ComUtil.SortDataSet(dsTraceInfo, Definition.BLOB_FIELD_NAME.STEP, SortType.asc);

        //    return dsTraceInfo;
        //}



        private DataSet GetTraceInfo(DataSet dsTraceInfo, DataSet dsFile)
        {
            DataTable dtParamter = dsTraceInfo.Tables[Definition.BLOB_FIELD_NAME.PARAM_INFO];
            DataTable dtSubstrate = dsTraceInfo.Tables[Definition.BLOB_FIELD_NAME.SUBSTRATE_INFO];
            DataTable dtStep = dsTraceInfo.Tables[Definition.BLOB_FIELD_NAME.STEP_INFO];

            try
            {
                if (DSUtil.CheckRowCount(dsFile))
                {
                    BISTel.PeakPerformance.Client.CommonUtil.GZip gZip = new BISTel.PeakPerformance.Client.CommonUtil.GZip();

                    string module_id = string.Empty;
                    string module_alias = string.Empty;

                    foreach (DataRow drFile in dsFile.Tables[0].Rows)
                    {
                        module_id = drFile[Definition.BLOB_FIELD_NAME.MODULE_ID].ToString();
                        module_alias = drFile[Definition.BLOB_FIELD_NAME.MODULE_ALIAS].ToString();

                        //using (StreamReader srTrace = gZip.DecompressForStream(drFile["FILE_DATA"]))
                        StreamReader srTrace = CommonUtility.ConvertBLOBToStreamReader(drFile["FILE_DATA"]);

                        if (srTrace != null)
                        {
                            this.ParseHeader(srTrace);

                            this.ParseContextLineInfo(srTrace);

                            //Parameter
                            if (_paramAlias != null && _paramAlias.Length > 0)
                            {
                                DataRow[] drExitParam = null;

                                string paramRawID = string.Empty;
                                string paramName = string.Empty;
                                string paramAlias = string.Empty;
                                string dataTypeCD = string.Empty;
                                string unit = string.Empty;

                                for (int i = 0; i < _paramAlias.Length; i++)
                                {
                                    //if (_paramInfos != null) paramRawID = _ComUtil.NVL(_paramInfos[i]);
                                    //if (_paramNames != null) paramName = _ComUtil.NVL(_paramNames[i]);
                                    if (_paramAlias != null) paramAlias = _comUtil.NVL(_paramAlias[i]);
                                    //if (_dataTypeCD != null) dataTypeCD = _ComUtil.NVL(_dataTypeCD[i]);
                                    if (_units != null) unit = _comUtil.NVL(_units[i]);

                                    //drExitParam = dtParamter.Select(string.Format("{0} = '{1}'", Definition.BLOB_FIELD_NAME.PARAM_RAWID, paramRawID));
                                    drExitParam = dtParamter.Select(string.Format("{0} = '{1}'", Definition.BLOB_FIELD_NAME.PARAM_ALIAS, paramAlias));

                                    if (drExitParam != null && drExitParam.Length > 0)
                                        continue;

                                    DataRow drNewParam = dtParamter.NewRow();

                                    //drNewParam[Definition.BLOB_FIELD_NAME.PARAM_RAWID] = paramRawID;
                                    //drNewParam[Definition.BLOB_FIELD_NAME.PARAM_NAME] = paramName;
                                    drNewParam[Definition.BLOB_FIELD_NAME.PARAM_ALIAS] = paramAlias;
                                    //drNewParam[Definition.BLOB_FIELD_NAME.DATA_TYPE_CD] = dataTypeCD;
                                    drNewParam[Definition.BLOB_FIELD_NAME.UNIT] = unit;

                                    dtParamter.Rows.Add(drNewParam);
                                }
                            }

                            //Step
                            if (_step != null && _step.Length > 0)
                            {
                                DataRow[] drExitStep = null;

                                string step = string.Empty;
                                string stepName = string.Empty;

                                for (int i = 1; i < _step.Length; i++)
                                {
                                    if (_step != null) step = _comUtil.NVL(_step[i]);
                                    if (_stepName != null) stepName = _comUtil.NVL(_stepName[i]);

                                    drExitStep = dtStep.Select(string.Format("{0} = '{1}' AND {2} = '{3}'", Definition.BLOB_FIELD_NAME.STEP, step,
                                                                                                            Definition.BLOB_FIELD_NAME.STEPNAME, stepName));

                                    if (drExitStep != null && drExitStep.Length > 0)
                                        continue;

                                    DataRow drNewStep = dtStep.NewRow();

                                    drNewStep[Definition.BLOB_FIELD_NAME.STEP] = step;
                                    drNewStep[Definition.BLOB_FIELD_NAME.STEPNAME] = stepName;

                                    dtStep.Rows.Add(drNewStep);
                                }
                            }

                            //Substrate
                            if (_substrateID != null && _substrateID.Length > 0)
                            {
                                DataRow[] drExitSubstrate = null;

                                string slot = string.Empty;
                                string substrate = string.Empty;

                                for (int i = 1; i < _substrateID.Length; i++)
                                {
                                    if (_slot != null) slot = _comUtil.NVL(_slot[i]);
                                    if (_substrateID != null) substrate = _comUtil.NVL(_substrateID[i]);

                                    if (substrate.Equals("N/A"))
                                        continue;

                                    drExitSubstrate = dtSubstrate.Select(string.Format("{0} = '{1}' AND {2} = '{3}'", Definition.BLOB_FIELD_NAME.SLOT, slot,
                                                                                                                      Definition.BLOB_FIELD_NAME.SUBSTRATEID, substrate));

                                    if (drExitSubstrate != null && drExitSubstrate.Length > 0)
                                        continue;

                                    DataRow drNewSubstrate = dtSubstrate.NewRow();

                                    drNewSubstrate[Definition.BLOB_FIELD_NAME.SLOT] = slot;
                                    drNewSubstrate[Definition.BLOB_FIELD_NAME.SUBSTRATEID] = substrate;

                                    dtSubstrate.Rows.Add(drNewSubstrate);
                                }
                            }


                            //Resource Clear
                            //_listRAWID.Clear();
                            _listSlotID.Clear();
                            _defaultTable = null;

                            _lineInfos = null;
                            _paramAlias = null;
                            _units = null;

                            _step = null;
                            _stepName = null;
                            _slot = null;
                            _substrateID = null;
                        }
                    }
                }
                //GC.Collect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return dsTraceInfo;
        }



        private string[] _step;
        private string[] _stepName;

        private string[] _slot;
        private string[] _substrateID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sr"></param>
        private void ParseContextLineInfo(StreamReader sr)
        {
            string line = string.Empty;

            string[] tempLine = null;

            for (int i = 0; i < this._lineInfos.Length && sr.Peek() > -1; i++)
            {
                line = sr.ReadLine();

                tempLine = line.Split('\t');

                if (tempLine == null || tempLine.Length < 2)
                    continue;

                if (tempLine[0].Equals(Definition.BLOB_FIELD_NAME.SUBSTRATEID))
                {
                    _substrateID = tempLine;
                }
                if (tempLine[0].Equals(Definition.BLOB_FIELD_NAME.SLOT))
                {
                    _slot = tempLine;
                }
                if (tempLine[0].Equals(Definition.BLOB_FIELD_NAME.STEP))
                {
                    _step = tempLine;
                }
                if (tempLine[0].Equals(Definition.BLOB_FIELD_NAME.STEPNAME))
                {
                    _stepName = tempLine;
                }
            }
        }

        #endregion

        #region Filtering : RSD Context and line_info List

        // 2011.09.07 Dylan
        // 사용하지 않는 RSD 컬럼 제거, Filter List 에 포함된 Header 컬럼 제거
        private string[] FilterHeader(string value)
        {
            string[] lineinfos = value.Split('\t');

            if (this._filterHeaders == null) this._filterHeaders = new List<string>();

            List<string> lstResult = new List<string>();
            for (int i = 0; i < lineinfos.Length; i++)
            {
                // RSD 컬럼 필터링
                if (lineinfos[i].ToUpper().Contains("RSD_"))
                {
                    if (this._llstRSDCol.ContainsKey(lineinfos[i].ToUpper()))
                    {
                        lstResult.Add(lineinfos[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
                // 중복 컬럼과 'Filter Header' 리스트 필터링
                if (!this._filterHeaders.Contains(lineinfos[i]) && !lstResult.Contains(lineinfos[i]))
                {
                    lstResult.Add(lineinfos[i]);
                }
            }

            return lstResult.ToArray();
        }

        #endregion
    }
}
