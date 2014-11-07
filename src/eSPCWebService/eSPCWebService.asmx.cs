using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using BISTel.eSPC.Data.Server.Compare;
using BISTel.eSPC.Data.Server.History;
using BISTel.eSPC.Data.Server.Report;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using BISTel.eSPC.Business.Server;
using BISTel.eSPC.Data.Server;
using BISTel.eSPC.Data.Server.Common;


namespace eSPCWebService
{
    /// <summary>
    /// Summary description for eSPCWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class eSPCWebService : System.Web.Services.WebService
    {

        #region : Field

        private DataSet _ds;
        private bool _bSuccess = false;

        #endregion

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public DataSet GetTableSchemaInfo(string sTableName)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();

                this._ds = commonData.GetTableSchemaInfo(sTableName);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataTable GetTableSchema(string sTableName)
        {
            DataTable dtSchema = new DataTable();

            try
            {
                BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();

                dtSchema = commonData.GetTableSchema(sTableName);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dtSchema;
        }

        #region : SPC MODEL

        [WebMethod]
        public DataSet GetSPCModel(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();

                this._ds = conditionData.GetSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //MET Model
        [WebMethod]
        public DataSet GetMETSPCModel(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();

                this._ds = conditionData.GetMETSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }
        //


        [WebMethod]
        public DataSet GetSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            try
            {
                SPCModelCompareData modelCompareData = new SPCModelCompareData();

                this._ds = modelCompareData.GetSPCModelList(lineRawID, areaRawID, eqpModel, paramAlias, paramTypeCd, useComma);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            try
            {
                SPCModelCompareData modelCompareData = new SPCModelCompareData();
                this._ds = modelCompareData.GetSPCSpecAndRule(modelConfigRawIDs);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCSpecAndRuleOfHistory(string modelConfigRawid, string[] versions)
        {
            try
            {
                SPCModelHistoryData historyData = new SPCModelHistoryData();
                this._ds = historyData.GetSPCSpecAndRuleOfHistory(modelConfigRawid, versions);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCModelOfLines(string[] lineRawid, string[] areaRawid, string eqpModelName, string paramTypeCD, string filterValue)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();

                this._ds = conditionData.GetSPCModelOfLines(lineRawid, areaRawid, eqpModelName, paramTypeCD, filterValue);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// 2009-10-26 bskwon추가
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetSPCModelContext(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();

                this._ds = conditionData.GetSPCModelContext(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }



        [WebMethod]
        public DataSet GetSPCDefaultModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCDefaultModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetSPCModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCModelVersionData(string modelConfigRawid, string version)
        {
            try
            {
                BISTel.eSPC.Data.Server.History.SPCModelHistoryData historyData = new SPCModelHistoryData();
                this._ds = historyData.GetSPCModelVersionData(modelConfigRawid, version);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCModelsData(string[] modelRawids, bool useComma)
        {
            try
            {
                BISTel.eSPC.Data.Server.Tool.SPCDataExport spcDataExport = new BISTel.eSPC.Data.Server.Tool.SPCDataExport();

                this._ds = spcDataExport.GetSPCModelsData(modelRawids, useComma);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        
        public DataSet GetSPCModelDatabyChartID(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCModelDatabyChartID(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCCalcModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //SPC-812 SPC Calculation
        [WebMethod]
        public DataSet GetSPCCalcModelDataSave(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCCalcModelDataSave(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;

        }
        //

        [WebMethod]
        public DataSet GetATTSPCCalcModelDataSave(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcAttModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcAttModelData.GetATTSPCCalcModelDataSave(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;

        }

        [WebMethod]
        public DataSet GetSPCCalcModelDataPopup(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCCalcModelDataPopup(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //SPC-704
        [WebMethod]
        public DataSet GetSPCMulCalcModelDataPopup(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCMulCalcModelDataPopup(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCParamList(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCParamList(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet SaveSPCModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();

                this._ds = spcModelBusiness.SaveSPCModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public bool SaveSPCModelSpecData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();

                bResult = spcModelBusiness.SaveSPCModelSpecData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }

            return bResult;
        }

        [WebMethod]
        public bool SaveSPCCalcModelData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();
                bResult = spcModelBusiness.SaveSPCCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }

            return bResult;
        }

        [WebMethod]
        public bool SaveSPCMulitiCalcModelData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();
                bResult = spcModelBusiness.SaveSPCMultiCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet DeleteSPCModel(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();
                this._ds = spcModelBusiness.DeleteSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet DeleteSPCModelConfig(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();
                this._ds = spcModelBusiness.DeleteSPCModelConfig(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCRuleMasterData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCRuleMasterData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCContextKeyTableAndColumns()
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCContextKeyTableAndColumns();
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCContextValueList(string sTableName, string sColumnName, string sDisplayColumns, string sWhereCondition, string sWhereValue)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                this._ds = spcModelData.GetSPCContextValueList(sTableName, sColumnName, sDisplayColumns, sWhereCondition, sWhereValue);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public string CheckDuplicateSPCModel(byte[] param)
        {
            string strResult = "";
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                strResult = spcModelData.CheckDuplicateSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return strResult;
        }

        [WebMethod]
        public DataSet GetSPCCalcContextKey(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                this._ds = spcModelData.GetSPCCalcContextKey(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion


        #region Common

        /// <summary>
        /// Get default data settings.
        /// </summary>
        /// <returns>Default data settings that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetDefaultSettingData()
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.QueryDefaultSettingData();
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Get custom data setttings.
        /// </summary>
        /// <param name="baData">spec rawid,data category cd,spec type cd,key type</param>
        /// <returns>Custom data settings that data type is DataSet </returns>
        [WebMethod]
        public DataSet GetCustomSettingData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.QuerySPECSettingData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        /// <summary>
        /// Gets auto calculation data.
        /// </summary>
        /// <returns>Auto calculation data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetAutoCalculationData()
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.QueryAutoCalculationData();
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public void GetPreConnection()
        {
            CommonData common = new CommonData();
            common.GetPreConnection();
        }

        #endregion

        #region : Condition

        /// <summary>
        /// Gets line information
        /// </summary>
        /// <param name="baData">Site</param>
        /// <returns>Site information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetSite(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new BISTel.eSPC.Data.Server.Common.ConditionData();

            try
            {
                this._ds = conditiondata.GetSite(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        /// <summary>
        /// Gets Fab information
        /// </summary>
        /// <param name="baData">Site</param>
        /// <returns>Fab information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetFab(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new BISTel.eSPC.Data.Server.Common.ConditionData();

            try
            {
                this._ds = conditiondata.GetFab(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets line information
        /// </summary>
        /// <param name="baData">Site</param>
        /// <returns>Line information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetLine(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new BISTel.eSPC.Data.Server.Common.ConditionData();

            try
            {
                this._ds = conditiondata.GetLine(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets area information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line</param>
        /// <returns>Area information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetArea(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new BISTel.eSPC.Data.Server.Common.ConditionData();

            try
            {
                this._ds = conditiondata.GetArea(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets equipment model information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line, Area</param>
        /// <returns>Equipment model information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetEqpModel(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetEqpModel(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets equipment information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line, Area, EQP Model</param>
        /// <returns>Equipment information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetEQP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetEQP(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets DCP information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line, Area, EQP Model, EQP ID</param>
        /// <returns>DCP information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetDCP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetDCP(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets module information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line, Area, EQP Model, EQP ID, DCP ID</param>
        /// <returns>Module information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetModuleByEQP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetModuleByEQP(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets sub module information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Module</param>
        /// <returns>Sub module information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetSubModuleByEQP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetSubModuleByEQP(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets module information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Line, Area, EQP Model, EQP ID, DCP ID</param>
        /// <returns>Module information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetModule(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetModule(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets product information that meets the conditions. 
        /// </summary>
        /// <param name="baData">EQP ID, Module ID</param>
        /// <returns>Product information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetProduct(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetProduct(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets recipe information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Module ID, Product ID</param>
        /// <returns>Recipe information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetRecipe(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetRecipe(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }



        /// <summary>
        /// Gets recipe information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Module ID, Product ID</param>
        /// <returns>Recipe information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetSubstrate(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetSubstrate(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets the group from the parameter.
        /// </summary>
        /// <param name="baData">Parameter</param>
        /// <returns>Group list that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetParamGroup(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetParamGroup(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets the parameter from the group information.
        /// </summary>
        /// <param name="baData">Parameter</param>
        /// <returns>Parameter list that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetParamByGroup(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetParamByGroup(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets the specification value from the parameter.
        /// </summary>
        /// <param name="baData">Parameter</param>
        /// <returns>Parameter specification value that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetParamSpec(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetParamSpec(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets specification information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Module ID, DCP ID, Recipe ID, Spec Exist</param>
        /// <returns>Parameter information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetParameter(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetParameter(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets multivariate model information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Module ID</param>
        /// <returns>Multivariate Model information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetMultivariateModel(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetMultivariateModel(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets step information that meets the conditions. 
        /// </summary>
        /// <param name="baData">Recipe ID, Spec Exist</param>
        /// <returns>Step information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetRecipeStep(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetRecipeStep(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets information about the step of equipment that you select. 
        /// </summary>
        /// <param name="baData">Module Rawid, Recipe Rawid</param>
        /// <returns>Step information that data type is DataSet</returns>
        [WebMethod]
        private DataSet GetRecipeStepByEQP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.QueryRecipeStepByEQP(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets information about the all step.
        /// </summary>
        /// <param name="baData">Recipe Rawid</param>
        /// <returns>Step information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetAllStep(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                this._ds = conditiondata.GetAllStep(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Gets Lot ID.
        /// </summary>
        /// <param name="btdata">None</param>
        /// <returns>Lot information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetLotIDList(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetLotIDList(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        /// <summary>
        /// Of the compressed trace data, gets trace data that meets the conditions, and gets also information about the trace data.
        /// </summary>
        /// <param name="btdata">Module ID, Recipe ID, Start Date, End Date</param>
        /// <returns>Trace data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetTraceLotHistory(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetTraceLotHistory(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        /// <summary>
        /// Of the compressed trace data, gets trace data that meets the conditions, and gets also information about the trace data.
        /// </summary>
        /// <param name="btdata">Module ID, Recipe ID, Start Date, End Date</param>
        /// <returns>Trace data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetTraceSubstrateHistory(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetTraceSubstrateHistory(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        /// <summary>
        /// Of the compressed trace data, gets trace data that meets the conditions and specified lot ID, and gets also information about the trace data.
        /// </summary>
        /// <param name="btdata">Line, Area, EQP Model, Module ID, Recipe ID</param>
        /// <returns>Trace data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetTraceLotHistoryByLot(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetTraceLotHistoryByLot(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        /// <summary>
        /// Of the compressed trace data, gets trace data that meets the conditions and specified lot ID, and gets also information about the trace data.
        /// </summary>
        /// <param name="btdata">Line, Area, EQP Model, Module ID, Recipe ID</param>
        /// <returns>Trace data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetTraceSubstrateHistoryByLot(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetTraceSubstrateHistoryByLot(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        /// <summary>
        /// Gets information about the step of equipment that meets the conditions. 
        /// </summary>
        /// <param name="btdata">Line, EQP ID</param>
        /// <returns>Step information that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetRecipeStepInfos(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.QueryRecipeStepInfos(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }


        #region SPC


        [WebMethod]
        public DataSet GetSPCOperation(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetSPCOperation(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }


        [WebMethod]
        public DataSet GetSPCProduct(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetSPCProduct(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        [WebMethod]
        public DataSet GetSPCEQP(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetSPCEQP(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }



        [WebMethod]
        public DataSet GetParamName(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ConditionData conditiondata = new ConditionData();

            try
            {
                ds = conditiondata.GetParamName(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        #endregion

        #endregion

        #region : CODE

        /// <summary>
        /// Gets the value of code that belongs to the category.
        /// </summary>
        /// <param name="baData">Category, Code List, USE_YN, DEFAULT_COL</param>
        /// <returns>Code data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetCodeData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.QueryCodeData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        /// <summary>
        /// Deletes or modifies the value of pre-defined code.
        /// </summary>
        /// <param name="baData">Code Data, User Rawid</param>
        /// <returns>Save result</returns>
        [WebMethod]
        public DataSet SaveCodeData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.SaveCodeData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion

        #region : Web.Config Read

        /// <summary>
        /// Gets values from web.config by means of keys.
        /// </summary>
        /// <param name="name">app name</param>
        /// <returns>Value</returns>
        [WebMethod(Description = "Get Web Config")]
        public string GetWebConfig(string key)
        {
            string data = string.Empty;
            try
            {
                data = WebConfigHandler.getInstance().GetValue(key);
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return data;
        }


        #endregion


        #region : TIBRV Message Send Request
        /// <summary>
        /// Gets configurations to connection.
        /// </summary>
        /// <param name="btdata">Target Name</param>
        /// <returns>configurations</returns>
        [WebMethod]
        public DataSet GetTargetConfig(byte[] btdata)
        {
            BISTel.eSPC.Data.Server.InterfaceData tibData = new BISTel.eSPC.Data.Server.InterfaceData();

            try
            {
                this._ds = tibData.QueryTargetConfig(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion


        #region OCAP


        /// <summary>
        /// OCAP Data Save
        /// </summary>
        /// <param name="baData"></param>
        /// <returns>true/false</returns>
        [WebMethod]
        public bool SaveOCAP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._bSuccess = ocapData.UpdateOCAPDate(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._bSuccess;
        }


        /// <summary>
        ///  Get OCAP Context List raw data.
        /// </summary>
        /// <param name="baData">LINE,START_DATE,END_DATE,PARAMETER_CHARACTERISTIC</param>
        /// <returns>OCAP data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetOCAPList(byte[] baData)
        {
            BISTel.eSPC.Business.Server.OCAP.OCAPBiz ocapBiz = new BISTel.eSPC.Business.Server.OCAP.OCAPBiz();

            try
            {
                this._ds = ocapBiz.GetOCAPList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }




        /// <summary>
        /// OCAP Data Save
        /// </summary>
        /// <param name="baData"></param>
        /// <returns>true/false</returns>
        [WebMethod]
        public DataSet GetOCAPDetails(byte[] baData)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._ds = ocapData.GetOCAPDetails(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        

        [WebMethod]
        public DataSet GetOCAPCommentList(string[] OOCRawid)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._ds = ocapData.GetOCAPCommentList(OOCRawid);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetOCAPCommentList_New(byte[] baData)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._ds = ocapData.GetOCAPCommentList_New(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet getGroupInfoByChartId(string chartId)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._ds = ocapData.getGroupInfoByChartId(chartId);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }
        #endregion

        #region Ppk Report

        [WebMethod]
        public DataSet GetProcessCapabilityList(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.ProcessCapabilityData pcData = new BISTel.eSPC.Data.Server.Report.ProcessCapabilityData();
            try
            {
                this._ds = pcData.GetProcessCapabilityList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetPpkReport(byte[] baData)
        {
            BISTel.eSPC.Business.Server.Report.ProcessCapabilityBiz pcData = new BISTel.eSPC.Business.Server.Report.ProcessCapabilityBiz();
            try
            {
                this._ds = pcData.GetPpkReport(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetOperationID(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.ProcessCapabilityData pcData = new BISTel.eSPC.Data.Server.Report.ProcessCapabilityData();
            try
            {
                this._ds = pcData.GetOperationID(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return this._ds;
        }
        #endregion


        #region SPC Control Chart

        [WebMethod]
        public DataSet GetSPCModelConfigSearch(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.SPCControlChartData dacControlChart = new BISTel.eSPC.Data.Server.Report.SPCControlChartData();
            try
            {
                this._ds = dacControlChart.GetSPCModelConfigSearch(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetSPCControlChartData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.Report.SPCControlChartBiz();
            try
            {
                this._ds = bizControlChart.GetSPCControlChartData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //SPC-704 MultiCac
        [WebMethod]
        public DataSet GetSPCMulControlChartData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.Report.SPCControlChartBiz();
            try
            {
                this._ds = bizControlChart.GetSPCMulControlChartData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;

        }


        [WebMethod]
        public DataSet GetSPCChartViewData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.Report.SPCControlChartBiz();
            try
            {
                this._ds = bizControlChart.GetSPCChartViewData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //[WebMethod]
        //public DataSet GetSPCControlChartToDayData(byte[] baData)
        //{
        //    BISTel.eSPC.Business.Server.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.Report.SPCControlChartBiz();
        //    try
        //    {
        //        this._ds = bizControlChart.GetSPCControlChartToDayData(baData);
        //    }
        //    catch (Exception ex)
        //    {
        //        EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
        //    }

        //    return this._ds;
        //}


        [WebMethod]
        public DataSet GetMetEQPData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.SPCControlChartData dacControlChartData = new BISTel.eSPC.Data.Server.Report.SPCControlChartData();
            try
            {
                this._ds = dacControlChartData.GetMetEQPData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public bool SaveToggleInformation(string[] rawIDs, string[] toggles, string[] modelConfigRawIDs, string[] spcDataDttses, string[] toggleYNs)
        {
            SPCControlChartData controlChart = new SPCControlChartData();
            try
            {
                return controlChart.SaveToggleInformation(rawIDs, toggles, modelConfigRawIDs, spcDataDttses, toggleYNs);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///  Get compressed OCAP raw data.
        /// </summary>
        /// <param name="baData">LINE,START_DATE,END_DATE,PARAMETER_CHARACTERISTIC</param>
        /// <returns>OCAP data that data type is DataSet</returns>
        [WebMethod]
        public DataSet GetDataTRXData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.Report.ProcessCapabilityBiz bizData = new BISTel.eSPC.Business.Server.Report.ProcessCapabilityBiz();

            try
            {
                this._ds = bizData.GetDataTRXData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion

        #region EQP_MST_PP


        [WebMethod]
        public DataSet GetEQPInfo(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.ProcessCapabilityData pcData = new BISTel.eSPC.Data.Server.Report.ProcessCapabilityData();
            try
            {
                this._ds = pcData.GetProcessCapabilityList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion



        #region Analysis


        [WebMethod]
        public DataSet GetParamList(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetParamList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public string[] GetParamListHavingSPCModel(string locationRawID, string areaRawID, string eqpModel, string paramTypeCD)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData condition = new ConditionData();
            try
            {
                return condition.GetParamListHavingSPCModel(locationRawID, areaRawID, eqpModel, paramTypeCD);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return null;
        }

        [WebMethod]
        public DataSet GetAnalysisLine(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetAnalysisLine(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetAnalysisArea(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetAnalysisArea(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetAnalysisTargetMachine(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetAnalysisTargetMachine(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }




        #endregion

        #region MultiData

        [WebMethod]
        public DataSet GetIsNullMultiOperation(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetIsNullMultiOperation(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetMultiData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            try
            {
                this._ds = dacData.GetMultiData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public string GetMOCVDName(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Analysis.AnalysisData dacData = new BISTel.eSPC.Data.Server.Analysis.AnalysisData();
            string sRtn = string.Empty;
            try
            {
                sRtn = dacData.GetMOCVDName(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return sRtn;
        }





        #endregion

        #region Condition Context
        [WebMethod]
        public DataSet GetSPCContextList(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetSPCContextList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetSiteForCondition()
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetSiteForCondition();
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }
        
        [WebMethod]
        public DataSet GetFabForCondition(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetFabForCondition(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetLineForCondition(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetLineForCondition(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetAreaForCondition(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetAreaForCondition(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetEqpModelForCondition(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ConditionData dacData = new BISTel.eSPC.Data.Server.Common.ConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetEqpModelForCondition(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetMETSPCModeList2(string locationRawid, string areaRawid, string eqpModel, string paramTypeCD)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, locationRawid);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
            llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
            llstData.Add(Definition.CONDITION_KEY_PARAM_TYPE_CD, paramTypeCD);

            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new ConditionData();
                this._ds = conditionData.GetMETSPCModel(llstData.GetSerialData());
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCModeList2(string locationRawid, string areaRawid, string eqpModel, string paramTypeCD)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, locationRawid);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
            llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
            llstData.Add(Definition.CONDITION_KEY_PARAM_TYPE_CD, paramTypeCD);

            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new ConditionData();
                this._ds = conditionData.GetSPCModel(llstData.GetSerialData());
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        #endregion 



        #region GetContextType : OCAP_VIEW : Context Info, SPC Control Chart : Information
        [WebMethod]
        public DataSet GetContextType(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.SPCControlChartData dacData = new BISTel.eSPC.Data.Server.Report.SPCControlChartData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetContextType(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return _ds;
        }
        #endregion 

        [WebMethod]
        public bool SaveInterlockYN(byte[] baData)
        {
            bool bResult = false;
            BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBiz = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();
            
            try
            {
                bResult = spcModelBiz.SaveInterlockYN(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetProductIDMappingData()
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                dsReturn = spcMigrationData.GetProductIDMappingData();
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public DataSet GetSPCBLOBData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                dsReturn = spcMigrationData.GetSPCBLOBData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public bool ModifySPCBLOBData(byte[] param)
        {
            bool bResult = false;
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                bResult = spcMigrationData.ModifySPCBLOBData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetSPCTempTrxData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                dsReturn = spcMigrationData.GetSPCTempTrxData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public bool ModifySPCTempTrxData(byte[] param)
        {
            bool bResult = false;
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                bResult = spcMigrationData.ModifySPCTempTrxData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetSPCOOCData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                dsReturn = spcMigrationData.GetSPCOOCData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public bool ModifySPCOOCData(byte[] param)
        {
            bool bResult = false;
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                bResult = spcMigrationData.ModifySPCOOCData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetSPCModelContextData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                dsReturn = spcMigrationData.GetSPCModelContextData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public bool ModifySPCModelContextData(byte[] param)
        {
            bool bResult = false;
            BISTel.eSPC.Data.Server.Tool.SPCMigrationData spcMigrationData = new BISTel.eSPC.Data.Server.Tool.SPCMigrationData();

            try
            {
                bResult = spcMigrationData.ModifySPCModelContextData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetGroupContextValue(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = spcModelData.GetGroupContextValue(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsReturn;
        }

        [WebMethod]
        public DataSet SearchByArea(byte[] param)
        {
            DataSet dsReturn = null;
            BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = spcModelData.SearchByArea(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }

        ///// <summary>
        ///// Search Area Popup에서 Area 검사후 SPC Model Name로 검색하는 기능
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public DataSet SearchByAreaAndSPCModelName(byte[] param)
        //{
        //    DataSet dsReturn = null;
        //    BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

        //    try
        //    {
        //        dsReturn = spcModelData.SearchByAreaAndSPCModelName(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
        //    }

        //    return dsReturn;
        //}



        [WebMethod]
        public DataSet CopyModelInfo(byte[] param)
        {
            DataSet dsReturn = null;
            BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();

            try
            {
                dsReturn = spcModelBusiness.CopyModelInfo(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }

        [WebMethod]
        public DataSet SearchByAreaSPCModelName(byte[] param)
        {
            DataSet dsReturn = null;
            BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = spcModelData.SearchByAreaSPCModelName(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }

        [WebMethod]
        public DataSet FindAnnotationOCAP(byte[] param)
        {
            DataSet dsReturn = null;
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                dsReturn = ocapData.FindAnnotationOCAP(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }

        [WebMethod]
        public string GetSPCParamType(byte[] baData)
        {
            string sParamType = string.Empty;
            ConditionData conditionData = new ConditionData();

            try
            {
                sParamType = conditionData.GetSPCParamType(baData);

            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return sParamType;
        }

        //added by enkim 2012.10.05 SPC-910

        [WebMethod]

        public bool GetUseNormalValuebyChartID(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                bResult = spcModelData.GetUseNormalValuebyChartID(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return bResult;
        }

        //added end SPC-910

        #region : SPC_MODEL_CONFIG_HISTORY

        [WebMethod]
        public DataSet GetSPCModelContextInfo(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Modeling.SPCModelData spcmodel = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                this._ds = spcmodel.GetSPCModelContextInfo(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCModelHistoryInfo(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Modeling.SPCModelData spcmodel = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                this._ds = spcmodel.GetSPCModelHistoryInfo(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public int GetTheNumberOfSubConfigOfModel(string modelConfigRawid)
        {
            BISTel.eSPC.Data.Server.Report.SPCControlChartData chart = new SPCControlChartData();

            try
            {
                return chart.GetTheNumberOfSubConfigOfModel(modelConfigRawid);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return -1;
        }

        [WebMethod]
        public DataTable GetSPCModelVersionHistory(string spcModelRawid)
        {
            BISTel.eSPC.Data.Server.History.SPCModelHistoryData historyData = new SPCModelHistoryData();

            try
            {
                return historyData.GetVersionHistory(spcModelRawid);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return null;
        }

        [WebMethod]
        public string GetSPCLastestVersion(string chartID)
        {
            BISTel.eSPC.Data.Server.History.SPCModelHistoryData historyData = new SPCModelHistoryData();

            try
            {
                return historyData.GetSPCLastestVersion(chartID);
            }
            catch(Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return string.Empty;
        }

        [WebMethod]
        public bool DeleteSPCModelHistory(byte[] baData)
        {
            bool bResult = true;
            BISTel.eSPC.Data.Server.History.SPCModelHistoryData historyData = new SPCModelHistoryData();

            try
            {
                bResult = historyData.DeleteSPCModelHistory(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return bResult;
        }

        [WebMethod]
        public DataSet GetTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelingData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsResult = modelingData.GetTargetConfigSpecData(targetList);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }


            return dsResult;
        }
        [WebMethod]
        public DataSet GetSourseConfigSpecData(string sourceRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelingData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsResult = modelingData.GetSourceConfigSpecData(sourceRawid);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsResult;
        }

        //SPC-678
        [WebMethod]
        public DataSet GetLastTRXDataDTTs(string modelConfigRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Business.Server.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.Report.SPCControlChartBiz();
            try
            {
                dsResult = bizControlChart.GetLastTRXDataDTTs(modelConfigRawid);
            }
            catch
            {
            }
            return dsResult;
        }
        #endregion

        //SPC-703 by Louis
        [WebMethod]
        public DataSet GetOCAPCommends(string ocapRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();
            try
            {
                dsResult = ocapData.GetOOCCommentList(ocapRawid);
            }
            catch
            {
            }
            return dsResult;
        }

        //SPC_679 2012.11.27 added by enkim
        [WebMethod]
        public string GetParamAlias(byte[] baData)
        {
            string strParamAlias = "";
            SPCControlChartData spcControlChartData = new SPCControlChartData();
            try
            {
                strParamAlias = spcControlChartData.GetParamAlias(baData);
            }
            catch
            {
            }
            return strParamAlias;
        }
        //added end

        //SPC-929, KBLEE, START
        /// <summary>
        /// Get Compressed Summary Data
        /// </summary>
        /// <param name="btdata"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetEqpSummaryTrxFileData(byte[] badata)
        {
            BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness sumBiz = new BISTel.eSPC.Business.Server.Modeling.SPCModelBusiness();

            try
            {
                this._ds = sumBiz.GetEQPSummaryData(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        [WebMethod]
        public DataSet GetEqpModuleInfoByParamAlias(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetEqpModuleInfoByParamAlias(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        [WebMethod]
        public DataSet GetRecipeStepByEqpModuleId(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetRecipeStepByEqpModuleId(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        [WebMethod]
        public DataSet GetConfigInfoByRawId(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetConfigInfoByRawId(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        [WebMethod]
        public DataSet GetModuleNameByModuleId(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetModuleNameByModuleId(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-929, KBLEE, END

        //SPC-930, KBLEE, START
        [WebMethod]
        public DataSet GetRuleListByChartType(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetRuleListByChartType(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-930, KBLEE, END

        //SPC-930, KBLEE, START
        [WebMethod]
        public DataSet GetRuleOptionByRuleNo(byte[] badata)
        {
            DataSet dsReturn = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

            try
            {
                dsReturn = modelData.GetRuleOptionByRuleNo(badata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }
        //SPC-930, KBLEE, END

        ///////////////////////////////////////////////////////////////////////
        ///////////////////ATT SPC WEB SERVICE///////////////////////////////
        [WebMethod]
        public DataSet GetATTCodeData(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commondata.QueryCodeData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCModel(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Common.ATTConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ATTConditionData();

                this._ds = conditionData.GetATTSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetATTSPCModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCDefaultModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetSPCDefaultModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCParamList(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetSPCParamList(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public string CheckDuplicateATTSPCModel(byte[] param)
        {
            string strResult = "";
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                strResult = spcModelData.CheckDuplicateSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return strResult;
        }

        [WebMethod]
        public DataSet SaveATTSPCModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();

                this._ds = spcModelBusiness.SaveSPCModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCATTModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetATTSPCModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet DeleteATTSPCModel(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();
                this._ds = spcModelBusiness.DeleteSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //SPC-1292, KBLEE, START
        [WebMethod]
        public DataSet DeleteATTSPCModelConfig(byte[] param)
        {
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();
                this._ds = spcModelBusiness.DeleteSPCModelConfig(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }
        //SPC-1292, KBLEE, END

        [WebMethod]
        public bool SaveATTSPCModelSpecData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();

                bResult = spcModelBusiness.SaveSPCModelSpecData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }

            return bResult;
        }

        [WebMethod]
        public bool SaveATTInterlockYN(byte[] baData)
        {
            bool bResult = false;
            BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBiz = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();

            try
            {
                bResult = spcModelBiz.SaveInterlockYN(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return bResult;
        }

        [WebMethod]
        public DataSet ATTCopyModelInfo(byte[] param)
        {
            DataSet dsReturn = null;
            BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();

            try
            {
                dsReturn = spcModelBusiness.CopyModelInfo(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return dsReturn;
        }

        [WebMethod]
        public DataSet GetATTOCAPList(byte[] baData)
        {
            BISTel.eSPC.Business.Server.ATT.OCAP.OCAPBiz ocapBiz = new BISTel.eSPC.Business.Server.ATT.OCAP.OCAPBiz();

            try
            {
                this._ds = ocapBiz.GetOCAPList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTOCAPCommends(string ocapRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.OCAP.ATTOCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.ATTOCAPData();
            try
            {
                dsResult = ocapData.GetOOCCommentList(ocapRawid);
            }
            catch
            {
            }
            return dsResult;
        }

        [WebMethod]
        public bool SaveATTOCAP(byte[] baData)
        {
            BISTel.eSPC.Data.Server.OCAP.OCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.OCAPData();

            try
            {
                this._bSuccess = ocapData.UpdateOCAPDate(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._bSuccess;
        }

        [WebMethod]
        public DataSet GetATTSPCModelDatabyChartID(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetSPCModelDatabyChartID(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public string[] GetATTParamListHavingSPCModel(string locationRawID, string areaRawID, string eqpModel, string paramTypeCD)
        {
            BISTel.eSPC.Data.Server.Common.ATTConditionData condition = new BISTel.eSPC.Data.Server.Common.ATTConditionData();
            try
            {
                return condition.GetParamListHavingSPCModel(locationRawID, areaRawID, eqpModel, paramTypeCD);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return null;
        }

        [WebMethod]
        public DataSet GetATTSPCModelList(string lineRawID, string areaRawID, string eqpModel, string paramAlias, string paramTypeCd, bool useComma)
        {
            try
            {
                BISTel.eSPC.Data.Server.Compare.SPCATTModelCompareData modelCompareData = new BISTel.eSPC.Data.Server.Compare.SPCATTModelCompareData();

                this._ds = modelCompareData.GetSPCModelList(lineRawID, areaRawID, eqpModel, paramAlias, paramTypeCd, useComma);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public DataSet GetATTSPCSpecAndRule(string[] modelConfigRawIDs)
        {
            try
            {
                BISTel.eSPC.Data.Server.Compare.SPCATTModelCompareData modelCompareData = new BISTel.eSPC.Data.Server.Compare.SPCATTModelCompareData();
                this._ds = modelCompareData.GetSPCSpecAndRule(modelConfigRawIDs);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCModeList2(string locationRawid, string areaRawid, string eqpModel)
        {
            LinkedList llstData = new LinkedList();

            llstData.Add(Definition.CONDITION_KEY_LINE_RAWID, locationRawid);
            llstData.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
            llstData.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);

            try
            {
                BISTel.eSPC.Data.Server.Common.ATTConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ATTConditionData();
                this._ds = conditionData.GetSPCModel(llstData.GetSerialData());
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTParamName(byte[] btdata)
        {
            DataSet ds = new DataSet();
            BISTel.eSPC.Data.Server.Common.ATTConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ATTConditionData();

            try
            {
                ds = conditionData.GetParamName(btdata);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return ds;
        }

        [WebMethod]
        public DataSet GetATTSPCModelsData(string[] modelRawids, bool useComma)
        {
            try
            {
                BISTel.eSPC.Data.Server.Tool.SPCATTDataExport spcDataExport = new BISTel.eSPC.Data.Server.Tool.SPCATTDataExport();

                this._ds = spcDataExport.GetATTSPCModelsData(modelRawids, useComma);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataTable GetATTSPCModelVersionHistory(string spcModelRawid)
        {
            BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData historyData = new BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData();

            try
            {
                return historyData.GetATTVersionHistory(spcModelRawid);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return null;
        }

        [WebMethod]
        public DataSet GetATTSPCSpecAndRuleOfHistory(string modelConfigRawid, string[] versions)
        {
            try
            {
                BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData historyData = new BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData();
                this._ds = historyData.GetATTSPCSpecAndRuleOfHistory(modelConfigRawid, versions);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public string GetATTSPCLastestVersion(string chartID)
        {
            BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData historyData = new BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData();

            try
            {
                return historyData.GetATTSPCLastestVersion(chartID);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return string.Empty;
        }

        [WebMethod]
        public DataSet GetATTSPCModelVersionData(string modelConfigRawid, string version)
        {
            try
            {
                BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData historyData = new BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData();
                this._ds = historyData.GetATTSPCModelVersionData(modelConfigRawid, version);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCCalcModelData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetATTSPCCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCContextList(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.ATTConditionData dacData = new BISTel.eSPC.Data.Server.Common.ATTConditionData();
            string sRtn = string.Empty;
            try
            {
                this._ds = dacData.GetSPCContextList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetATTSPCCalcModelDataPopup(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetATTSPCCalcModelDataPopup(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCControlChartData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz();
            try
            {
                this._ds = bizControlChart.GetSPCControlChartData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTLastTRXDataDTTs(string modelConfigRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz();
            try
            {
                dsResult = bizControlChart.GetLastTRXDataDTTs(modelConfigRawid);
            }
            catch
            {
            }
            return dsResult;
        }


        [WebMethod]
        public DataSet GetATTSPCModelHistoryInfo(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

            try
            {
                this._ds = spcModelData.GetATTSPCModelHistoryInfo(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCModelContextInfo(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcmodel = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

            try
            {
                this._ds = spcmodel.GetATTSPCModelContextInfo(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        //SPC-1292, KBLEE
        [WebMethod]
        public DataSet GetATTGroupInfoByChartId(string chartId)
        {
            BISTel.eSPC.Data.Server.OCAP.ATTOCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.ATTOCAPData();

            try
            {
                this._ds = ocapData.GetGroupInfoByChartId(chartId);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }


        [WebMethod]
        public bool SaveATTSPCCalcModelData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();
                bResult = spcModelBusiness.SaveSPCCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }

            return bResult;
        }

        [WebMethod]
        public DataSet GetATTTargetConfigSpecData(string[] targetList)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCATTModelData modelingData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

            try
            {
                dsResult = modelingData.GetATTTargetConfigSpecData(targetList);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }


            return dsResult;
        }

        [WebMethod]
        public DataSet GetATTSourseConfigSpecData(string sourceRawid)
        {
            DataSet dsResult = new DataSet();
            BISTel.eSPC.Data.Server.Modeling.SPCATTModelData modelingData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

            try
            {
                dsResult = modelingData.GetATTSourceConfigSpecData(sourceRawid);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            return dsResult;
        }

        [WebMethod]
        public DataSet GetATTSPCMulCalcModelDataPopup(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetATTSPCMulCalcModelDataPopup(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetSPCATTRuleMasterData(byte[] param)
        {
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

                this._ds = spcModelData.GetSPCATTRuleMasterData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTSPCMulControlChartData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz bizControlChart = new BISTel.eSPC.Business.Server.ATT.Report.SPCControlChartBiz();
            try
            {
                this._ds = bizControlChart.GetSPCMulControlChartData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;

        }

        [WebMethod]
        public DataSet GetATTOCAPDetails(byte[] baData)
        {
            BISTel.eSPC.Data.Server.OCAP.ATTOCAPData ocapData = new BISTel.eSPC.Data.Server.OCAP.ATTOCAPData();

            try
            {
                this._ds = ocapData.GetOCAPDetails(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public bool SaveSPCATTMulitiCalcModelData(byte[] param)
        {
            bool bResult = false;
            try
            {
                BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness spcModelBusiness = new BISTel.eSPC.Business.Server.ATT.Modeling.SPCModelBusiness();
                bResult = spcModelBusiness.SaveSPCATTMultiCalcModelData(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
                return false;
            }
            return bResult;
        }

        [WebMethod]
        public DataSet GetATTProcessCapabilityList(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Report.ATTProcessCapabilityData pcData = new BISTel.eSPC.Data.Server.Report.ATTProcessCapabilityData();
            try
            {
                this._ds = pcData.GetProcessCapabilityList(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet GetATTDataTRXData(byte[] baData)
        {
            BISTel.eSPC.Business.Server.ATT.Report.ProcessCapabilityBiz bizData = new BISTel.eSPC.Business.Server.ATT.Report.ProcessCapabilityBiz();

            try
            {
                this._ds = bizData.GetDataTRXData(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public bool DeleteSPCATTModelHistory(byte[] baData)
        {
            bool bResult = true;
            BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData historyData = new BISTel.eSPC.Data.Server.History.SPCATTModelHistoryData();

            try
            {
                bResult = historyData.DeleteSPCATTModelHistory(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return bResult;
        }

        [WebMethod]
        public DataSet QueryWeeklyStartEnd(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commonData.QueryWeeklyStartEnd(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet QueryMonthStartEnd(byte[] baData)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();

            try
            {
                this._ds = commonData.QueryMonthStartEnd(baData);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return this._ds;
        }

        [WebMethod]
        public DataSet getTimeData()
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();
                _ds = conditionData.getTimeData();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public DataSet CreateAutoCalculationData(byte[] baData)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _ds = modelData.CreateAutoCalculationDataSheet(baData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetAutoCalculationOption(string category)
        {
            DataSet _ds = new DataSet();
            try
            {
                CommonData commonData = new CommonData();
                _ds = commonData.GetAutoCalculationOption(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        //spc-977 version update.
        [WebMethod]
        public bool SetIncreaseVersion(byte[] baData)
        {
            bool _bResult;
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData modelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _bResult = modelData.SetIncreaseVersion(baData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _bResult;
        }

        //spc-1226
        [WebMethod]
        public DataSet GetSPCModelGroupList(bool isMET)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _ds = data.GetSPCModelGroupList(isMET);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetSPCModelListbyGroup(byte[] param, bool isMET)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _ds = data.GetSPCModelListbyGroup(param, isMET);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public bool SaveSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool _bResult = false;
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _bResult = data.SaveSPCGroupList(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _bResult;
        }

        [WebMethod]
        public bool SaveSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool _bResult = false;
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _bResult = data.SaveSPCModelMapping(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _bResult;
        }

        [WebMethod]
        public DataSet GetGroupList(byte[] param)
        {
            DataSet ds = new DataSet();

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                ds = data.GetGroupList(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        [WebMethod]
        public DataSet GetATTGroupList(byte[] param)
        {
            DataSet ds = new DataSet();

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                ds = data.GetATTGroupList(param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        [WebMethod]
        public DataSet GetGroupNameByChartId(string ChartId)
        {
            DataSet ds = new DataSet();

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                ds = data.GetGroupNameByChartId(ChartId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //SPC-1292, KBLEE
        [WebMethod]
        public DataSet GetATTGroupNameByChartId(string ChartId)
        {
            DataSet ds = new DataSet();

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                ds = data.GetATTGroupNameByChartId(ChartId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        [WebMethod]
        public bool CheckDuplicateGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                bResult = data.CheckDuplicateGroupName(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bResult;
        }

        [WebMethod]
        public string CheckDuplicateMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string strResult = "";
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                strResult = spcModelData.CheckDuplicateMapping(dt, isEmptyGroupRawid);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return strResult;
        }

        [WebMethod]
        public int CheckCountSPCModel(byte[] param)
        {
            int cntModel = -1;
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();

                cntModel = spcModelData.CheckCountSPCModel(param);
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }

            return cntModel;
        }

        [WebMethod]
        public DataSet GetATTSPCModelGroupList()
        {
            DataSet ds = new DataSet();

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                ds = data.GetATTSPCModelGroupList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        [WebMethod]
        public bool CheckDuplicateATTGroupName(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                bResult = data.CheckDuplicateATTGroupName(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bResult;
        }

        [WebMethod]
        public string CheckDuplicateATTMapping(DataTable dt, bool isEmptyGroupRawid)
        {
            string sResult = "";

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                sResult = data.CheckDuplicateATTMapping(dt, isEmptyGroupRawid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResult;
        }

        [WebMethod]
        public bool SaveATTSPCGroupList(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                bResult = data.SaveATTSPCGroupList(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bResult;
        }

        [WebMethod]
        public bool SaveATTSPCModelMapping(DataSet dsSave, byte[] param)
        {
            bool bResult = false;

            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCATTModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();
                bResult = data.SaveATTSPCModelMapping(dsSave, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bResult;
        }

        #region TOSHIBA

        [WebMethod]
        public DataSet GetBaseInfoWithEQPID(byte[] baData)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();
                _ds = conditionData.GetBaseInfoWithEQPID(baData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }


        [WebMethod]
        public DataSet GetSPCModelListbyBaseInfo(byte[] baData)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Common.ConditionData conditionData = new BISTel.eSPC.Data.Server.Common.ConditionData();
                _ds = conditionData.GetSPCModelListbyBaseInfo(baData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetRestrictionFilter(string userRawid)
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Modeling.SPCModelData data = new BISTel.eSPC.Data.Server.Modeling.SPCModelData();
                _ds = data.GetRestrictionFilter(userRawid);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        [WebMethod]
        public DataSet GetContextTypeData()
        {
            DataSet _ds = new DataSet();
            try
            {
                BISTel.eSPC.Data.Server.Common.CommonData data = new BISTel.eSPC.Data.Server.Common.CommonData();
                _ds = data.GetContextTypeData();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;
        }

        #endregion
    }
}
