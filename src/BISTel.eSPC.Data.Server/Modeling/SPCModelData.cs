using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Data.Server.Modeling
{
    public class SPCModelData : DataBase
    {
        SPCModelDataCall _dataCall = new SPCModelDataCall();
        Common.CommonData _commondata = new BISTel.eSPC.Data.Server.Common.CommonData();
        bool _isATT = false;

        public DataSet GetSPCModelData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCModelData(param, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCModelDatabyChartID(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCModelDatabyChartID(param, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCCalcModelData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelData(param, _isATT);

            return dsReturn;
        }

        //SPC-812 SPC Calculation.
        public DataSet GetSPCCalcModelDataSave(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelDataSave(param, _isATT);

            return dsReturn;
        }

        //SPC-704 MultiCalculation
        public DataSet GetSPCMulCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCMulCalcModelDataPopup(param, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelDataPopup(param, _isATT);

            return dsReturn;
        }

        public DataSet CreateSPCModel(ConfigMode configMode, string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, ref string ref_Config_RawID, string groupRawid, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, ref ref_Config_RawID, groupRawid, useComma, _isATT);

            return dsResult;
        }

        public DataSet ModifySPCModel(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, List<string> lstChangedMasterColList, string comment, string changedItems, string groupRawid, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCModel(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, lstChangedMasterColList, comment, changedItems, groupRawid, useComma, _isATT);

            return dsResult;
        }

        public DataSet DeleteSPCModel(byte[] param)
        {
            DataSet dsResult = _dataCall.DeleteSPCModel(param, _isATT);

            return dsResult;
        }

        public DataSet DeleteSPCModelConfig(byte[] param)
        {
            DataSet dsResult = _dataCall.DeleteSPCModelConfig(param, _isATT);

            return dsResult;
        }

        public bool DeleteSPCModelConfig(string strSubconfig)
        {
            _dataCall.ParentSQLHandler = this;

            bool bResult = _dataCall.DeleteSPCModelConfig(strSubconfig, _isATT);

            return bResult;
        }

        public DataSet GetSPCRuleMasterData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCRuleMasterData(param, _isATT);

            return dsReturn;
        }



        #region :DEFAULT

        public DataSet GetSPCDefaultModelData(byte[] param)
        {
            //CONFIGURATION 화면을 공통적으로 사용하기 위해
            //MODEL_* TABLE 을 기준으로 COLUMN의 NAME을 맞추도록 한다. 

            DataSet dsReturn = _dataCall.GetSPCDefaultModelData(param, _isATT);

            return dsReturn;
        }

        public DataSet SaveDefaultConfig(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.SaveDefaultConfig(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, useComma, _isATT);

            return dsResult;
        }

        #endregion



        public DataSet QueryModelConfigMstSPC(byte[] param)
        {
            DataSet dsReturn = _dataCall.QueryModelConfigMstSPC(param);

            return dsReturn;
        }

        public DataSet QueryModelConfigOPTMstSPC(byte[] param)
        {
            DataSet dsReturn = _dataCall.QueryModelConfigOPTMstSPC(param);

            return dsReturn;
        }

        public DataSet QueryModelContextMstSPC(byte[] param)
        {
            DataSet dsReturn = _dataCall.QueryModelContextMstSPC(param);

            return dsReturn;
        }

        public DataSet QueryModelRuleMstSPC(byte[] param)
        {
            DataSet dsReturn = _dataCall.QueryModelRuleMstSPC(param);

            return dsReturn;
        }

        public DataSet QueryModelRuleOPTMstSPC(byte[] param)
        {
            DataSet dsReturn = _dataCall.QueryModelRuleOPTMstSPC(param);

            return dsReturn;
        }

        public DataSet GetSPCParamList(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCParamList(param);

            return dsReturn;
        }

        public DataSet GetSPCContextKeyTableAndColumns()
        {
            DataSet dsReturn = _dataCall.GetSPCContextKeyTableAndColumns();

            return dsReturn;
        }

        public DataSet GetSPCContextValueList(string sTableName, string sColumnName, string sDisplayColumns, string sWhereCondition, string sWhereValue)
        {
            DataSet dsReturn = _dataCall.GetSPCContextValueList(sTableName, sColumnName, sDisplayColumns, sWhereCondition, sWhereValue);

            return dsReturn;
        }

        public string CheckDuplicateSPCModel(byte[] param)
        {
            string strResult = _dataCall.CheckDuplicateSPCModel(param, _isATT);

            return strResult;
        }

        public bool SaveInterlockYN(byte[] param)
        {
            bool bReturn = _dataCall.SaveInterlockYN(param, _isATT);

            return bReturn;
        }

        public bool SaveSPCModelSpecData(byte[] param)
        {
            bool bReturn = _dataCall.SaveSPCModelSpecData(param, _isATT);

            return bReturn;
        }

        public bool SaveSpecData(byte[] param)
        {
            bool bReturn = _dataCall.SaveSpecData(param);

            return bReturn;
        }

        public bool SaveSpecMultiCalcData(byte[] param)
        {
            _dataCall.ParentSQLHandler = this;

            bool bReturn = _dataCall.SaveSpecMultiCalcData(param, _isATT);

            return bReturn;

        }

        public bool SaveSpecCalcData(byte[] param)
        {
            _dataCall.ParentSQLHandler = this;

            bool bReturn = _dataCall.SaveSpecCalcData(param, _isATT);

            return bReturn;
        }

        public DataSet ModifySPCSubModel(string sConfigRawID, DataTable dtRule, DataTable dtRuleOpt, string sUserID, ref string sConfigRawid)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCSubModel(sConfigRawid, dtRule, dtRuleOpt, sUserID, ref sConfigRawid, _isATT);

            return dsResult;
        }

        public DataSet ModifySPCSubModelContext(string sConfigRawID, DataTable dtContext, string sUserID, bool isOnlyMainGroup, string groupRawid)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCSubModelContext(sConfigRawID, dtContext, sUserID, isOnlyMainGroup, groupRawid, _isATT);

            return dsResult;
        }

        public DataSet GetGroupContextValue(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetGroupContextValue(param);

            return dsReturn;
        }

        public DataSet SearchByArea(byte[] param)
        {
            DataSet dsReturn = _dataCall.SearchByArea(param);

            return dsReturn;
        }

        public DataSet SearchByAreaSPCModelName(byte[] param)
        {
            DataSet dsReturn = _dataCall.SearchByAreaSPCModelName(param);

            return dsReturn;
        }

        public DataSet GetSPCCalcContextKey(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcContextKey(param);

            return dsReturn;
        }

        public DataSet CopyModelInfo(LinkedList llstParam)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.CopyModelInfo(llstParam, _isATT);

            return dsResult;
        }

        public DataSet ModifySPCSubModelForCopy(LinkedList llstParam, ref string subConfigRawIDs)
        {
            DataSet dsResult = _dataCall.ModifySPCSubModelForCopy(llstParam, ref subConfigRawIDs);

            return dsResult;
        }

        public DataSet GetSPCModelRuleMstInfo(string targetConfigRawID)
        {
            DataSet dsResult = _dataCall.GetSPCModelRuleMstInfo(targetConfigRawID);

            return dsResult;
        }

        public DataSet GetSPCModelRuleOptMstInfo(string targetConfigRawID)
        {
            DataSet dsResult = _dataCall.GetSPCModelRuleOptMstInfo(targetConfigRawID);

            return dsResult;
        }

        public DataSet GetSPCModelContextInfo(byte[] baData)
        {
            DataSet dsResult = _dataCall.GetSPCModelContextInfo(baData, _isATT);

            return dsResult;
        }

        public DataSet GetSPCModelHistoryInfo(byte[] baData)
        {
            DataSet dsResult = _dataCall.GetSPCModelHistoryInfo(baData, _isATT);

            return dsResult;
        }

        public bool IncreaseVersionOfSubConfigs(string sMainConfigRawID)
        {
            _dataCall.ParentSQLHandler = this;

            bool bResult = _dataCall.IncreaseVersionOfSubConfigs(sMainConfigRawID, _isATT);

            return bResult;
        }

        //added by enkim 2012.10.05 SPC-910
        public bool GetUseNormalValuebyChartID(byte[] param)
        {
            bool bResult = _dataCall.GetUseNormalValuebyChartID(param);

            return bResult;
        }
        //added end SPC-910

        //SPC-855 By Louis
        public DataSet GetTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = _dataCall.GetTargetConfigSpecData(targetList, _isATT);

            return dsResult;
        }

        public DataSet GetSourceConfigSpecData(string rawid)
        {
            DataSet dsResult = _dataCall.GetSourceConfigSpecData(rawid, _isATT);

            return dsResult;
        }

        public DataSet CreateAutoCalculationDataSheet(byte[] baData)
        {
            DataSet dsReturn = _dataCall.CreateAutoCalculationDataSheet(baData);

            return dsReturn;
        }

        //spc-977 rawid를 받아 version을 update해줌.
        public bool SetIncreaseVersion(byte[] param)
        {
            bool bReturn = _dataCall.SetIncreaseVersion(param);

            return bReturn;
        }

        public DataSet GetSPCModelGroupList(bool isMET)
        {
            DataSet dsResult = _dataCall.GetSPCModelGroupList(isMET, _isATT);

            return dsResult;
        }

        public DataSet GetSPCModelListbyGroup(byte[] param, bool isMET)
        {
            DataSet dsResult = _dataCall.GetSPCModelListbyGroup(param, isMET);

            return dsResult;
        }

        public bool SaveSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.SaveSPCGroupList(dsSave, param, _isATT);

            return bResult;
        }

        public bool SaveSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.SaveSPCModelMapping(dsSave, param, _isATT);

            return bResult;
        }

        public DataSet GetGroupList(byte[] param)
        {
            DataSet dsResult = _dataCall.GetGroupList(param, _isATT);

            return dsResult;
        }

        public DataSet GetGroupNameByChartId(string chartId)
        {
            DataSet dsResult = _dataCall.GetGroupNameByChartId(chartId, _isATT);

            return dsResult;
        }

        public bool CheckDuplicateGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.CheckDuplicateGroupName(dsSave, param, _isATT);

            return bResult;
        }

        public string CheckDuplicateMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string strResult = _dataCall.CheckDuplicateMapping(dt, isEmptyGroupRawid, _isATT);

            return strResult;
        }

        public int CheckCountSPCModel(byte[] param)
        {
            int cntModel = _dataCall.CheckCountSPCModel(param);

            return cntModel;
        }

        public DataSet GetEqpSummaryTrxFileData(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetEqpSummaryTrxFileData(baData);

            return dsReturn;
        }

        public DataSet GetEqpSummaryTempTrxCLOBData(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetEqpSummaryTempTrxCLOBData(baData);

            return dsReturn;
        }

        public DataSet GetEqpModuleInfoByParamAlias(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetEqpModuleInfoByParamAlias(baData);

            return dsReturn;
        }

        public DataSet GetRecipeStepByEqpModuleId(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetRecipeStepByEqpModuleId(baData);

            return dsReturn;
        }

        public DataSet GetConfigInfoByRawId(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetConfigInfoByRawId(baData);

            return dsReturn;
        }

        public DataSet GetModuleNameByModuleId(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetModuleNameByModuleId(baData);

            return dsReturn;
        }

        public DataSet GetRestrictionFilter(string userRawid)
        {
            DataSet dsReturn = _dataCall.GetRestrictionFilter(userRawid);

            return dsReturn;
        }

        public DataSet GetRuleListByChartType(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetRuleListByChartType(baData);

            return dsReturn;
        }

        public DataSet GetRuleOptionByRuleNo(byte[] baData)
        {
            DataSet dsReturn = _dataCall.GetRuleOptionByRuleNo(baData);

            return dsReturn;
        }
    }

    public class SPCATTModelData : DataBase
    {
        SPCModelDataCall _dataCall = new SPCModelDataCall();
        bool _isATT = true;

        public SPCATTModelData()
        {
            _dataCall.ParentSQLHandler = this;
        }

        #region ::ATT Model Data Selection

        public DataSet GetATTSPCModelData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCModelData(param, _isATT);

            return dsReturn;
        }

        #endregion

        #region ::ATT Default Model Selection

        public DataSet GetSPCDefaultModelData(byte[] param)
        {
            //CONFIGURATION 화면을 공통적으로 사용하기 위해
            //MODEL_* TABLE 을 기준으로 COLUMN의 NAME을 맞추도록 한다. 

            DataSet dsReturn = _dataCall.GetSPCDefaultModelData(param, _isATT);

            return dsReturn;
        }

        #endregion

        public DataSet GetSPCParamList(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCParamList(param);

            return dsReturn;
        }

        #region ::CheckDuplicateATTSPCModel

        public string CheckDuplicateSPCModel(byte[] param)
        {
            string strResult = _dataCall.CheckDuplicateSPCModel(param, _isATT);

            return strResult;
        }

        #endregion

        #region ::SaveCreate ATT Model

        public DataSet CreateSPCModel(ConfigMode configMode, string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, ref string ref_Config_RawID, string groupRawId, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, ref ref_Config_RawID, groupRawId, useComma, _isATT);

            return dsResult;
        }

        #endregion

        #region ::Save Modify ATT Model

        public DataSet ModifySPCModel(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, List<string> lstChangedMasterColList, string groupRawid, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCModel(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, lstChangedMasterColList, null, null, groupRawid, useComma, _isATT);

            return dsResult;
        }

        #endregion

        #region ::Save Default ATT Model

        public DataSet SaveDefaultConfig(string sUserID, DataTable dtModel, DataTable dtConfig, DataTable dtConfigOpt, DataTable dtContext, DataTable dtRule, DataTable dtRuleOpt, DataTable dtAutoCalc, bool useComma)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.SaveDefaultConfig(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, useComma, _isATT);

            return dsResult;
        }

        #endregion

        #region ::Delete SPC ATTModel

        public DataSet DeleteSPCModel(byte[] param)
        {
            DataSet dsResult = _dataCall.DeleteSPCModel(param, _isATT);

            return dsResult;
        }

        #endregion


        #region ::SaveATTSPCModelSpecData

        public bool SaveSPCModelSpecData(byte[] param)
        {
            bool bReturn = _dataCall.SaveSPCModelSpecData(param, _isATT);

            return bReturn;
        }

        #endregion

        public bool SaveInterlockYN(byte[] param)
        {
            bool bReturn = _dataCall.SaveInterlockYN(param, _isATT);

            return bReturn;
        }


        #region ::ATTCopyModelInfo

        public DataSet CopyModelInfo(LinkedList llstParam)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.CopyModelInfo(llstParam, _isATT);

            return dsResult;
        }


        #endregion

        public DataSet GetSPCModelDatabyChartID(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCModelDatabyChartID(param, _isATT);

            return dsReturn;
        }

        public DataSet GetATTSPCCalcModelData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelData(param, _isATT);

            return dsReturn;
        }

        public DataSet GetATTSPCCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelDataPopup(param, _isATT);

            return dsReturn;
        }

        public DataSet GetATTSPCModelHistoryInfo(byte[] baData)
        {
            DataSet dsResult = _dataCall.GetSPCModelHistoryInfo(baData, _isATT);

            return dsResult;
        }

        public DataSet GetATTSPCModelContextInfo(byte[] baData)
        {
            DataSet dsResult = _dataCall.GetSPCModelContextInfo(baData, _isATT);

            return dsResult;
        }

        public bool SaveATTSpecCalcData(byte[] param)
        {
            _dataCall.ParentSQLHandler = this;

            bool bReturn = _dataCall.SaveSpecCalcData(param, _isATT);

            return bReturn;
        }

        public DataSet ModifyATTSPCSubModel(string sConfigRawID, DataTable dtRule, DataTable dtRuleOpt, string sUserID, ref string sConfigRawid)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCSubModel(sConfigRawID, dtRule, dtRuleOpt, sUserID, ref sConfigRawid, _isATT);

            return dsResult;
        }

        public DataSet GetATTTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = _dataCall.GetTargetConfigSpecData(targetList, _isATT);

            return dsResult;
        }

        public DataSet GetATTSourceConfigSpecData(string rawid)
        {
            DataSet dsResult = _dataCall.GetSourceConfigSpecData(rawid, _isATT);

            return dsResult;
        }

        //SPC-704 MultiCalculation
        public DataSet GetATTSPCMulCalcModelDataPopup(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCMulCalcModelDataPopup(param, _isATT);

            return dsReturn;
        }

        public DataSet GetSPCATTRuleMasterData(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCRuleMasterData(param, _isATT);

            return dsReturn;
        }

        public bool IncreaseVersionOfSubConfigs(string sMainConfigRawID)
        {
            _dataCall.ParentSQLHandler = this;

            bool bResult = _dataCall.IncreaseVersionOfSubConfigs(sMainConfigRawID, _isATT);

            return bResult;
        }

        public DataSet ModifyATTSPCSubModelContext(string sConfigRawID, DataTable dtContext, string sUserID, bool isOnlyMainGroup, string groupRawid)
        {
            _dataCall.ParentSQLHandler = this;

            DataSet dsResult = _dataCall.ModifySPCSubModelContext(sConfigRawID, dtContext, sUserID, isOnlyMainGroup, groupRawid, _isATT);

            return dsResult;
        }

        public DataSet DeleteATTSPCModelConfig(byte[] param)
        {
            DataSet dsResult = _dataCall.DeleteSPCModelConfig(param, _isATT);

            return dsResult;
        }

        public bool DeleteATTSPCModelConfig(string strSubconfig)
        {
            _dataCall.ParentSQLHandler = this;

            bool bResult = _dataCall.DeleteSPCModelConfig(strSubconfig, _isATT);

            return bResult;
        }

        public bool SaveATTSpecMultiCalcData(byte[] param)
        {
            _dataCall.ParentSQLHandler = this;

            bool bReturn = _dataCall.SaveSpecMultiCalcData(param, _isATT);

            return bReturn;

        }

        //SPC-1292, KBLEE
        public DataSet GetATTGroupNameByChartId(string chartId)
        {
            DataSet dsResult = _dataCall.GetGroupNameByChartId(chartId, _isATT);

            return dsResult;

        }

        //SPC-812 SPC Calculation.
        public DataSet GetATTSPCCalcModelDataSave(byte[] param)
        {
            DataSet dsReturn = _dataCall.GetSPCCalcModelDataSave(param, _isATT);

            return dsReturn;
        }

        public DataSet GetATTGroupList(byte[] param)
        {
            DataSet dsResult = _dataCall.GetGroupList(param, _isATT);

            return dsResult;
        }

        public DataSet GetATTSPCModelGroupList()
        {
            DataSet dsResult = _dataCall.GetSPCModelGroupList(false, _isATT);

            return dsResult;
        }

        public bool CheckDuplicateATTGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.CheckDuplicateGroupName(dsSave, param, _isATT);

            return bResult;
        }

        public string CheckDuplicateATTMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string strResult = _dataCall.CheckDuplicateMapping(dt, isEmptyGroupRawid, _isATT);

            return strResult;
        }

        public bool SaveATTSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.SaveSPCModelMapping(dsSave, param, _isATT);

            return bResult;
        }

        public bool SaveATTSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool bResult = _dataCall.SaveSPCGroupList(dsSave, param, _isATT);

            return bResult;
        }

    }

}