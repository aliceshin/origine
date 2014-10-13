using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Data.Server.Tool
{
    public class SPCMigrationData : DataBase
    {
        CommonUtility _ComUtil = new CommonUtility();
        Common.CommonData _commondata = new BISTel.eSPC.Data.Server.Common.CommonData();

        public DataSet GetProductIDMappingData()
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append(" SELECT * FROM PRODUCTID_MAPPING_SPC ");

                dsReturn = base.Query(sb.ToString());
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }

            return dsReturn;
        }

        public DataSet GetSPCBLOBData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                    llstCondition.Add("START_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.START_DTTS]));
                    llstCondition.Add("END_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.END_DTTS]));
                }

                //#00. MODEL_MST_SPC
                sb.Append("SELECT * FROM DATA_TRX_SPC ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                sb.Append(" and  ( ");
                sb.Append(" (spc_start_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND spc_start_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) OR ");
                sb.Append(" (spc_end_dtts>=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') AND spc_end_dtts<=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3')) ");
                sb.Append(" ) ");

                dsReturn = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCTempTrxData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                    llstCondition.Add("START_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.START_DTTS]));
                    llstCondition.Add("END_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.END_DTTS]));
                }

                /* 오늘일짜 Date*/
                string sEndDate = string.Empty;
                if (llstParam[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                    sEndDate = llstParam[Definition.DynamicCondition_Condition_key.END_DTTS].ToString();

                if (int.Parse(DateTime.Parse(sEndDate).ToString(Definition.DATETIME_FORMAT_YMD)) >= int.Parse(DateTime.Today.ToString(Definition.DATETIME_FORMAT_YMD)))
                {
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.START_DTTS);
                    llstCondition.Remove(Definition.DynamicCondition_Condition_key.END_DTTS);
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, DateTime.Today.ToString(Definition.DATETIME) + " 00:00:00");
                    llstCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, DateTime.Today.ToString(Definition.DATETIME) + " 23:59:59");

                    //#00. MODEL_MST_SPC
                    sb.Append("SELECT * FROM DATA_TEMP_TRX_SPC ");
                    sb.Append(" WHERE MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                    sb.Append(" and spc_data_dtts >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') ");
                    sb.Append(" and spc_data_dtts <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') ");

                    dsReturn = this.Query(sb.ToString(), llstCondition);

                    if (base.ErrorMessage.Length > 0)
                    {
                        DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                        return dsReturn;
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCOOCData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                    llstCondition.Add("START_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.START_DTTS]));
                    llstCondition.Add("END_DTTS", _ComUtil.NVL(llstParam[Definition.DynamicCondition_Condition_key.END_DTTS]));
                }

                sb.Append("SELECT * FROM OOC_TRX_SPC ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                sb.Append(" and OOC_DTTS >=TO_TIMESTAMP(:START_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') ");
                sb.Append(" and OOC_DTTS <=TO_TIMESTAMP(:END_DTTS,'yyyy-MM-dd HH24:MI:SS.FF3') ");

                dsReturn = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }

        public DataSet GetSPCModelContextData(byte[] param)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sb = new StringBuilder();
            LinkedList llstParam = new LinkedList();
            LinkedList llstCondition = new LinkedList();

            try
            {
                llstParam.SetSerialData(param);

                if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                {
                    llstCondition.Add("MODEL_CONFIG_RAWID", _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID]));
                }

                sb.Append("SELECT * FROM MODEL_CONTEXT_MST_SPC ");
                sb.Append(" WHERE MODEL_CONFIG_RAWID = :MODEL_CONFIG_RAWID ");
                sb.Append(" AND CONTEXT_KEY = 'PRODUCT_ID' ");

                dsReturn = this.Query(sb.ToString(), llstCondition);

                if (base.ErrorMessage.Length > 0)
                {
                    DSUtil.SetResult(dsReturn, 0, "", base.ErrorMessage);
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
            }
            finally
            {
                llstParam.Clear(); llstParam = null;
                llstCondition.Clear(); llstCondition = null;
                sb.Remove(0, sb.Length); sb = null;
            }

            return dsReturn;
        }



        public bool ModifySPCBLOBData(byte[] param)
        {
            LinkedList llstConditionData = new LinkedList();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();
            
            string sWhereQuery = string.Empty;

            try
            {
                llstConditionData.SetSerialData(param);
                llstFieldData.Add("FILE_DATA", llstConditionData["FILE_DATA"]);
                llstWhereData.Add("RAWID", llstConditionData["RAWID"].ToString());
                base.BeginTrans();
                
                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);

                base.Update("DATA_TRX_SPC", llstFieldData, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }
            finally
            {
                base.Commit();
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return true;
        }

        public bool ModifySPCTempTrxData(byte[] param)
        {
            LinkedList llstConditionData = new LinkedList();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;

            try
            {
                llstConditionData.SetSerialData(param);
                llstFieldData.Add("CONTEXT_LIST", llstConditionData["CONTEXT_LIST"]);
                llstWhereData.Add("RAWID", llstConditionData["RAWID"].ToString());
                base.BeginTrans();

                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);

                base.Update("DATA_TEMP_TRX_SPC", llstFieldData, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }
            finally
            {
                base.Commit();
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return true;
        }

        public bool ModifySPCOOCData(byte[] param)
        {
            LinkedList llstConditionData = new LinkedList();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;

            try
            {
                llstConditionData.SetSerialData(param);
                llstFieldData.Add("CONTEXT_LIST", llstConditionData["CONTEXT_LIST"]);
                llstWhereData.Add("RAWID", llstConditionData["RAWID"].ToString());
                base.BeginTrans();

                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);

                base.Update("OOC_TRX_SPC", llstFieldData, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }
            finally
            {
                base.Commit();
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return true;
        }

        public bool ModifySPCModelContextData(byte[] param)
        {
            LinkedList llstConditionData = new LinkedList();
            LinkedList llstFieldData = new LinkedList();
            LinkedList llstWhereData = new LinkedList();

            string sWhereQuery = string.Empty;

            try
            {
                llstConditionData.SetSerialData(param);
                llstFieldData.Add("CONTEXT_VALUE", llstConditionData["CONTEXT_VALUE"]);
                llstWhereData.Add("RAWID", llstConditionData["RAWID"].ToString());
                base.BeginTrans();

                sWhereQuery = string.Format("WHERE {0} = :{0}", COLUMN.RAWID);

                base.Update("MODEL_CONTEXT_MST_SPC", llstFieldData, sWhereQuery, llstWhereData);

                if (base.ErrorMessage.Length > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.RollBack();
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, new string[] { ex.Message, ex.Source, ex.StackTrace });
                return false;
            }
            finally
            {
                base.Commit();
                llstFieldData.Clear(); llstFieldData = null;
                llstWhereData.Clear(); llstWhereData = null;
                sWhereQuery = null;
            }

            return true;
        }
    }
}
