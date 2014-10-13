using System;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Collections;
using System.Data;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition
{
    #region : DataInfo

    public class DateInfo
    {
        public DateTime start;
        public DateTime end;

        public DateInfo()
        {
        }
    }

    #endregion

    #region : ConditionInfo

    public class ConditionInfo
    {
        public string sline = string.Empty;
        public string sArea = string.Empty;
        public string sEQPModel = string.Empty;
        public string sDCPID = string.Empty;
        public string sOption = string.Empty;
        public string option = string.Empty;
        public string sSpecModelGroup = string.Empty;


        public LinkedList llEQPList = null;
        public LinkedList llModuleList = null;
        public LinkedList llProductList = null;
        public LinkedList llRecipeList = null;
        public LinkedList llStepList = null;
        public LinkedList llTraceParamList = null;
        public LinkedList llSummaryParamList = null;
        public LinkedList llEventParamList = null;
        public LinkedList llLotList = null;

        public LinkedList llMSPCList = null;
        public LinkedList llResultList = null;
        public LinkedList llSpecModelGroupList = null;

        public int count = 0;
        public object[,] datas = null;

        public DateTime dtStartTime = DateTime.Now;
        public DateTime dtEndTime = DateTime.Now;

        public buttonType button = buttonType.SEARCH;

        public string _sLine = "";
        public string _sArea = "";
        public string _sEQPModel = "";
        public string _sEQPID = "";
        public string _sDCPID = "";
        public string _sModuleID = "";
        public string _sProductID = "";
        public string _sRecipeID = "";
        public string _sLineRawID = "";
        public string _sAreaRawID = "";
        public string _sEQPRawID = "";
        public string _sDCPRawID = "";
        public string _sModuleRawID = "";
        public string _sProductRawID = "";
        public string _sRecipeRawID = "";
        public string _sOption = string.Empty;
        public string _sSpecModelGroupID = "";
        public string _sSpecModelGroupRawID = "";

        public LinkedList _llstEQPList = null;
        public LinkedList _llstModuleList = null;
        public LinkedList _llstProductList = null;
        public LinkedList _llstRecipeList = null;
        public LinkedList _llstStepList = null;
        public LinkedList _llstTraceParamList = null;
        public LinkedList _llstSummaryParamList = null;
        public LinkedList _llstEventParamList = null;
        public LinkedList _llstLotList = null;

        public LinkedList _llstMSPCList = null;
        public LinkedList _llstResultList = null;
        public LinkedList _llstSpecModelGroupList = null;

        public int _iCount = 0;
        public object[,] _objDatas = null;

        public DateTime _dtStartTime = DateTime.Now;
        public DateTime _dtEndTime = DateTime.Now;

        public buttonType _button = buttonType.SEARCH;


        public ConditionInfo()
        {
        }

        public ConditionInfo(LinkedList condition)
        {
            this.SetCondition(condition);
        }

        public void SetCondition(LinkedList condition)
        {
            sline = GetLine(condition);
            sArea = GetArea(condition);
            sEQPModel = GetModel(condition);
            sDCPID = GetDCP(condition);
            llEQPList = GetEQP(condition);
            llModuleList = GetModule(condition);
            llProductList = GetProduct(condition);
            llRecipeList = GetRecipe(condition);
            llStepList = GetStep(condition);
            llTraceParamList = GetTraceParam(condition);
            llSummaryParamList = GetSummaryParam(condition);
            llEventParamList = GetEventParam(condition);
            llMSPCList = GetMSPC(condition);
            llLotList = GetLot(condition);
            count = GetCount(condition);
            datas = GetDatas(condition);
            dtStartTime = GetStart(condition);
            dtEndTime = GetEnd(condition);
            sOption = GetOption(condition);
            llResultList = GetResult(condition);
            button = GetButton(condition);


            this._sLine = GetLine(condition);
            this._sLineRawID = GetLineRawID(condition);
            this._sArea = GetArea(condition);
            this._sAreaRawID = GetAreaRawID(condition);
            this._sEQPModel = GetModel(condition);
            this._sDCPID = GetDCP(condition);
            this._llstEQPList = GetEQP(condition);
            this._llstModuleList = GetModule(condition);
            this._llstProductList = GetProduct(condition);
            this._llstRecipeList = GetRecipe(condition);
            this._llstStepList = GetStep(condition);
            this._llstTraceParamList = GetTraceParam(condition);
            this._llstSummaryParamList = GetSummaryParam(condition);
            this._llstEventParamList = GetEventParam(condition);
            this._llstMSPCList = GetMSPC(condition);
            this._llstLotList = GetLot(condition);
            this._iCount = GetCount(condition);
            this._objDatas = GetDatas(condition);
            this._dtStartTime = GetStart(condition);
            this._dtEndTime = GetEnd(condition);
            this._sOption = GetOption(condition);
            this._llstResultList = GetResult(condition);
            this._button = GetButton(condition);
        }

        public static string GetData(LinkedList condition, string key)
        {
            string strData = string.Empty;
            object objData = condition[key];

            if (objData == null)
            {
                return strData;
            }

            try
            {
                strData = Convert.ToString(objData);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return strData;
        }

        public static void AddLine(LinkedList condition, string line)
        {
            condition.Add(Definition.CONDITION_KEY_LINE, line);
        }

        public static string GetLine(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_LINE);
        }

        public static void AddLineRawID(LinkedList condition, string lineRawID)
        {
            condition.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawID);
        }

        public static string GetLineRawID(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_LINE_RAWID);
        }


        public static void AddArea(LinkedList condition, string area)
        {
            condition.Add(Definition.CONDITION_KEY_AREA, area);
        }

        public static string GetArea(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_AREA);
        }

        public static void AddAreaRawID(LinkedList condition, string areaRawID)
        {
            condition.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawID);
        }

        public static string GetAreaRawID(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_AREA_RAWID);
        }

        public static void AddModel(LinkedList condition, string model)
        {
            condition.Add(Definition.CONDITION_KEY_EQP_MODEL, model);
        }

        public static string GetModel(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_EQP_MODEL);
        }


        public static void AddDCP(LinkedList condition, string dcp)
        {
            condition.Add(Definition.CONDITION_KEY_DCP_ID, dcp);
        }

        public static string GetDCP(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_DCP_ID);
        }

        public static void AddDCPRawID(LinkedList condition, string dcpRawID)
        {
            condition.Add(Definition.CONDITION_KEY_DCP_RAWID, dcpRawID);
        }

        public static string GetDCPRawID(LinkedList condition)
        {
            return GetData(condition, Definition.CONDITION_KEY_DCP_RAWID);
        }

        public static void AddEQP(LinkedList condition, LinkedList eqp)
        {
            condition.Add(Definition.CONDITION_KEY_EQP_ID, eqp);
        }

        public static LinkedList GetEQP(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_EQP_ID];
            return (LinkedList)objData;
        }

        public static void AddModule(LinkedList condition, LinkedList module)
        {
            condition.Add(Definition.CONDITION_KEY_MODULE_ID, module);
        }

        public static LinkedList GetModule(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_MODULE_ID];
            return (LinkedList)objData;
        }

        public static void AddProduct(LinkedList condition, LinkedList module)
        {
            condition.Add(Definition.CONDITION_KEY_PRODUCT_ID, module);
        }

        public static LinkedList GetProduct(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_PRODUCT_ID];
            return (LinkedList)objData;
        }

        public static void AddRecipe(LinkedList condition, LinkedList recipe)
        {
            condition.Add(Definition.CONDITION_KEY_RECIPE_ID, recipe);
        }

        public static LinkedList GetRecipe(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_RECIPE_ID];
            return (LinkedList)objData;
        }

        public static void AddSpecModelGroup(LinkedList condition, LinkedList specmodelgroup)
        {
            condition.Add(Definition.CONDITION_KEY_SPEC_MODEL_GROUP_NAME, specmodelgroup);
        }

        public static LinkedList GetSpecModelGroup(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_SPEC_MODEL_GROUP_NAME];
            return (LinkedList)objData;
        }

        public static void AddStep(LinkedList condition, LinkedList step)
        {
            condition.Add(Definition.CONDITION_KEY_STEP_ID, step);
        }

        public static LinkedList GetStep(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_STEP_ID];
            return (LinkedList)objData;
        }

        public static void AddTraceParam(LinkedList condition, LinkedList sensor)
        {
            condition.Add(Definition.CONDITION_KEY_PARAM_TRACE, sensor);
        }

        public static LinkedList GetTraceParam(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_PARAM_TRACE];
            return (LinkedList)objData;
        }

        public static void AddSummaryParam(LinkedList condition, LinkedList sensor)
        {
            condition.Add(Definition.CONDITION_KEY_PARAM_SUMMARY, sensor);
        }

        public static LinkedList GetSummaryParam(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_PARAM_SUMMARY];
            return (LinkedList)objData;
        }

        public static void AddEventParam(LinkedList condition, LinkedList sensor)
        {
            condition.Add(Definition.CONDITION_KEY_PARAM_EVENT, sensor);
        }

        public static LinkedList GetEventParam(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_PARAM_EVENT];
            return (LinkedList)objData;
        }

        public static void AddMSPC(LinkedList condition, LinkedList sensor)
        {
            condition.Add(Definition.CONDITION_KEY_PARAM_MSPC, sensor);
        }

        public static LinkedList GetMSPC(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_PARAM_MSPC];
            return (LinkedList)objData;
        }

        public static void AddDatas(LinkedList condition, object[,] datas)
        {
            condition.Add(Definition.CONDITION_KEY_DATA, datas);
        }

        public static object[,] GetDatas(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_DATA];
            return (object[,])objData;
        }

        public static void AddCount(LinkedList condition, int count)
        {
            condition.Add(Definition.CONDITION_KEY_COUNT, count);
        }

        public static int GetCount(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_COUNT];

            if (objData != null)
            {
                return (int)objData;
            }

            return 0;
        }

        public static void AddOption(LinkedList condition, string option)
        {
            condition.Add("OPTION", option);
        }

        public static string GetOption(LinkedList condition)
        {
            object objData = condition["OPTION"];

            return (string)objData;
        }

        public static void AddStart(LinkedList condition, DateTime start)
        {
            condition.Add(Definition.CONDITION_KEY_START_DATE, start);
        }

        public static DateTime GetStart(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_START_DATE];

            if (objData != null)
            {
                return (DateTime)objData;
            }

            return DateTime.Now;
        }

        public static void AddEnd(LinkedList condition, DateTime end)
        {
            condition.Add(Definition.CONDITION_KEY_END_DATE, end);
        }

        public static DateTime GetEnd(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_END_DATE];

            if (objData != null)
            {
                return (DateTime)objData;
            }

            return DateTime.Now;
        }

        public static void AddResult(LinkedList condition, LinkedList sensor)
        {
            condition.Add(Definition.CONDITION_KEY_RESULT, sensor);
        }

        public static LinkedList GetResult(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_RESULT];
            return (LinkedList)objData;
        }

        public static void AddButton(LinkedList condition, buttonType button)
        {
            condition.Add(Definition.CONDITION_KEY_BUTTON, button);
        }

        public static buttonType GetButton(LinkedList condition)
        {
            buttonType btnData = buttonType.SEARCH;
            object objData = condition[Definition.CONDITION_KEY_BUTTON];

            if (objData == null)
            {
                return btnData;
            }

            try
            {
                btnData = (buttonType)objData;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            return btnData;
        }

        public static void AddLot(LinkedList condition, LinkedList lot)
        {
            condition.Add(Definition.CONDITION_KEY_LOT_ID, lot);
        }

        public static LinkedList GetLot(LinkedList condition)
        {
            object objData = condition[Definition.CONDITION_KEY_LOT_ID];
            return (LinkedList)objData;
        }

        public static void AddCondition(LinkedList condition, string line, string area, string model, string dcp,
                                        LinkedList eqp, LinkedList module, LinkedList product, LinkedList recipe,
                                        LinkedList step, LinkedList traceParam, LinkedList summaryParam, LinkedList eventParam,
                                        LinkedList mspc, LinkedList lot, int count, object[,] datas,
                                        DateTime start, DateTime end, string option, LinkedList result, buttonType button)
        {
            AddLine(condition, line);
            AddArea(condition, area);
            AddModel(condition, model);
            AddDCP(condition, dcp);
            AddEQP(condition, eqp);
            AddModule(condition, module);
            AddProduct(condition, product);
            AddRecipe(condition, recipe);
            AddStep(condition, step);
            AddTraceParam(condition, traceParam);
            AddSummaryParam(condition, summaryParam);
            AddEventParam(condition, eventParam);
            AddMSPC(condition, mspc);
            AddLot(condition, lot);
            AddCount(condition, count);
            AddDatas(condition, datas);
            AddStart(condition, start);
            AddEnd(condition, end);
            AddOption(condition, option);
            AddResult(condition, result);
            AddButton(condition, button);
        }

        public static void AddCondition(LinkedList condition, string line, string lineRawID, string area, string areaRawID,
                                        string eqpModel, string dcpID, string dcpRawID,
                                       LinkedList eqp, LinkedList module, LinkedList product, LinkedList recipe,
                                       LinkedList step, LinkedList traceParam, LinkedList summaryParam, LinkedList eventParam,
                                       LinkedList mspc, LinkedList lot, int count, object[,] datas,
                                       DateTime start, DateTime end, string option, LinkedList result, buttonType button)
        {
            AddLine(condition, line);
            AddLineRawID(condition, lineRawID);
            AddArea(condition, area);
            AddAreaRawID(condition, areaRawID);
            AddModel(condition, eqpModel);
            AddDCP(condition, dcpID);
            AddDCPRawID(condition, dcpRawID);
            AddEQP(condition, eqp);
            AddModule(condition, module);
            AddProduct(condition, product);
            AddRecipe(condition, recipe);
            AddStep(condition, step);
            AddTraceParam(condition, traceParam);
            AddSummaryParam(condition, summaryParam);
            AddEventParam(condition, eventParam);
            AddMSPC(condition, mspc);
            AddLot(condition, lot);
            AddCount(condition, count);
            AddDatas(condition, datas);
            AddStart(condition, start);
            AddEnd(condition, end);
            AddOption(condition, option);
            AddResult(condition, result);
            AddButton(condition, button);
        }

        public static void AddCondition(LinkedList condition, string line, string lineRawID, string area, string areaRawID,
            string eqpModel, string dcpID, string dcpRawID, string eqpID, string eqpRawID, string moduleID,
            string moduleRawID, string productID, string productRawID, string recipeID, string recipeRawID,
            DataSet dsStep, DataSet dsTraceParam, DataSet dsSummaryParam, DataSet dsEventParam)
        {
            //AddLine(condition, line);
            //AddLineRawID(condition, lineRawID);
            //AddArea(condition, area);
            //AddAreaRawID(condition, areaRawID);
            //AddModel(condition, eqpModel);
            //AddDCP(condition, dcpID);
            //AddDCPRawID(condition, dcpRawID);
            //AddEQP(condition, eqp);
            //AddModule(condition, module);
            //AddProduct(condition, product);
            //AddRecipe(condition, recipe);
            //AddStep(condition, step);
            //AddTraceParam(condition, traceParam);
            //AddSummaryParam(condition, summaryParam);
            //AddEventParam(condition, eventParam);
            //AddMSPC(condition, mspc);
            //AddLot(condition, lot);
            //AddCount(condition, count);
            //AddDatas(condition, datas);
            //AddStart(condition, start);
            //AddEnd(condition, end);
            //AddOption(condition, option);
            //AddResult(condition, result);
            //AddButton(condition, button);
        }

        #region :public

        public string psLine
        {
            set
            {
                this._sLine = value;
            }
            get
            {
                return this._sLine;
            }
        }

        #endregion
    }




    #endregion

    #region : DefaultInfo

    public class DefaultInfo
    {
        public string _sLine = "";
        public string _sArea = "";
        public string _sEQPModel = "";
        public ArrayList _alEqpIDList = null;
        public string _sEQPID = "";
        public string _sDCPID = "";
        public ArrayList _alModuleIDList = null;
        public string _sModuleID = "";
        public string _sProductID = "";
        public ArrayList _alRecipeList = null;
        public string _sRecipeID = "";
        public ArrayList _sLegend = null;
        public string _sType = "";

        public string _sSpecModelGroupID = "";
        public ArrayList _alSpecModelGroupList = null;

        public Hashtable _htLineRawid = new Hashtable();
        public Hashtable _htAreaRawid = new Hashtable();
        public Hashtable _htEQPRawid = new Hashtable();
        public Hashtable _htDCPRawid = new Hashtable();
        public Hashtable _htModuleRawid = new Hashtable();
        public Hashtable _htProductRawid = new Hashtable();
        public Hashtable _htSensorRawid = new Hashtable();
        public Hashtable _htRecipeRawid = new Hashtable();

        public Hashtable _htSpecModelGroupRawid = new Hashtable();


        public DataTable _dtParamData = new DataTable();
        public DataTable _dtStepData = new DataTable();

        public DefaultInfo()
        {
        }

        public void Reset(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.LINE:
                case ConditionType.MULTITYPE:
                    {
                        _sLine = string.Empty;
                        _sArea = string.Empty;
                        _sEQPModel = string.Empty;
                        _sEQPID = string.Empty;
                        _sDCPID = string.Empty;
                        _sModuleID = string.Empty;
                        _sProductID = string.Empty;
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _alEqpIDList = null;
                        _alModuleIDList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.AREA:
                    {
                        _sEQPModel = string.Empty;
                        _sEQPID = string.Empty;
                        _sDCPID = string.Empty;
                        _sModuleID = string.Empty;
                        _sProductID = string.Empty;
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _alEqpIDList = null;
                        _alModuleIDList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.EQPMODEL:
                    {
                        _sEQPID = string.Empty;
                        _sDCPID = string.Empty;
                        _sModuleID = string.Empty;
                        _sProductID = string.Empty;
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _alEqpIDList = null;
                        _alModuleIDList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.EQPID:
                    {
                        _sDCPID = string.Empty;
                        _sModuleID = string.Empty;
                        _sProductID = string.Empty;
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _alModuleIDList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.DCPID:
                    {
                        _sModuleID = string.Empty;
                        _sProductID = string.Empty;
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _alModuleIDList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.MODULEID:
                    {
                        _sRecipeID = string.Empty;
                        _sProductID = string.Empty;
                        _alRecipeList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.PRODUCTID:
                    {
                        _sRecipeID = string.Empty;
                        _alRecipeList = null;
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
                case ConditionType.RECIPEID:
                    {
                        _sSpecModelGroupID = string.Empty;
                        _alSpecModelGroupList = null;
                    }
                    break;
            }
        }
    }

    #endregion
}
