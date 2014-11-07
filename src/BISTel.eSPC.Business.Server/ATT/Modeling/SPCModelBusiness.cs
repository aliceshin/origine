using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Business.Server.ATT.Modeling
{
    public class SPCModelBusiness : DataBase
    {
        CommonUtility _ComUtil = new CommonUtility();

        BISTel.eSPC.Data.Server.Modeling.SPCATTModelData spcModelData = new BISTel.eSPC.Data.Server.Modeling.SPCATTModelData();

        public DataSet SaveSPCModelData(byte[] param)
        {

            string configRawID = "";

            DataSet dsReturn = new DataSet();

            bool _isGroup = false;
            string groupRawid = null; //SPC-1292, KBLEE, GroupRawId 정보 담는 변수

            try
            {
                spcModelData.ParentSQLHandler = this;

                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                bool useComma = false;

                if (llstParam[Definition.VARIABLE_USE_COMMA] is bool)
                    useComma = (bool)llstParam[Definition.VARIABLE_USE_COMMA];

                string sUserID = llstParam[Definition.CONDITION_KEY_USER_ID].ToString();
                BISTel.eSPC.Common.ConfigMode configMode = (BISTel.eSPC.Common.ConfigMode)llstParam[Definition.CONDITION_KEY_CONFIG_MODE];

                DataTable dtModel = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                DataTable dtConfig = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtConfigOpt = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC];
                DataTable dtContext = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];
                DataTable dtRule = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC];
                DataTable dtRuleOpt = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC];
                DataTable dtAutoCalc = (DataTable)llstParam[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC];

                string _sMainYN = "";
                string sConfigRawid = "";
                string srefConfigRawid = "";
                bool bOnlyMain = false;
                bool bOnlyMainGroup = false;

                if (llstParam[Definition.CONDITION_KEY_MAIN_YN] != null)
                {
                    _sMainYN = llstParam[Definition.CONDITION_KEY_MAIN_YN].ToString();
                }

                bool _hasSubconfigs = Convert.ToBoolean(llstParam[Definition.CONDITION_KEY_HAS_SUBCONFIG].ToString());

                if (llstParam.Contains(BISTel.eSPC.Common.COLUMN.GROUP_YN))
                {
                    _isGroup = true;
                }

                if (llstParam.Contains(BISTel.eSPC.Common.COLUMN.GROUP_RAWID))
                {
                    groupRawid = llstParam[BISTel.eSPC.Common.COLUMN.GROUP_RAWID].ToString();
                }

                base.BeginTrans();

                switch (configMode)
                {
                    case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                        dsReturn = spcModelData.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, ref srefConfigRawid, groupRawid, useComma);
                        break;
                    case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                        dsReturn = spcModelData.CreateSPCModel(configMode, sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, ref srefConfigRawid, groupRawid, useComma);
                        break;

                    case BISTel.eSPC.Common.ConfigMode.MODIFY :
                    case BISTel.eSPC.Common.ConfigMode.ROLLBACK :
                        List<string> lstChangedMasterColList = (List<string>)llstParam["CHANGED_MASTER_COL_LIST"];
                        dsReturn = spcModelData.ModifySPCModel(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, lstChangedMasterColList, groupRawid, useComma);
                        break;

                    case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                        dsReturn = spcModelData.SaveDefaultConfig(sUserID, dtModel, dtConfig, dtConfigOpt, dtContext, dtRule, dtRuleOpt, dtAutoCalc, useComma);
                        break;
                }

                if (base.ErrorMessage.Length > 0 || DSUtil.GetResultSucceed(dsReturn) == 0)
                {
                    this.RollBack();
                    return dsReturn;
                }

                if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK))
                {
                    configRawID = dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                }

                if ((configMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK)) && _sMainYN.Equals(Definition.VARIABLE_Y) 
                    && _hasSubconfigs)
                {
                    if (llstParam.Contains("ONLY_MAIN"))
                    {
                        if (llstParam["ONLY_MAIN"].ToString() == Definition.VARIABLE_Y)
                        {
                            bOnlyMain = true;
                        }
                    }
                    if (llstParam.Contains("ONLY_MAIN_GROUP"))
                    {
                        if (llstParam["ONLY_MAIN_GROUP"].ToString() == Definition.VARIABLE_Y)
                        {
                            bOnlyMainGroup = true;
                        }
                    }
                    if (!bOnlyMain)
                    {
                        dsReturn = spcModelData.ModifyATTSPCSubModel(configRawID, dtRule, dtRuleOpt, sUserID, ref sConfigRawid);

                        if (sConfigRawid.Length > 0)
                        {
                            sConfigRawid = sConfigRawid.Substring(1);
                        }
                    }

                    dsReturn = spcModelData.ModifyATTSPCSubModelContext(configRawID, dtContext, sUserID, bOnlyMainGroup, groupRawid);

                    if (_isGroup) //기존 Sub 전체 삭제.
                    {
                        bool bResult = spcModelData.DeleteATTSPCModelConfig(sConfigRawid);
                        if (!bResult)
                            base.RollBack();
                    }
                }

                this.Commit();

                // Increase version
                BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();
                switch (configMode)
                {
                    case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                    case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                        this.BeginTrans();
                        foreach (string query in commonData.GetIncreaseATTVersionQuery(srefConfigRawid.ToString()))
                        {
                            this.Query(query);
                        }
                        this.Commit();
                        break;
                    case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    case BISTel.eSPC.Common.ConfigMode.ROLLBACK :
                        this.BeginTrans();
                        foreach (DataRow dr in dtConfig.Rows)
                        {
                            foreach (string query in commonData.GetIncreaseATTVersionQuery(dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString()))
                            {
                                this.Query(query);
                            }
                        }

                        if (!bOnlyMain && _sMainYN.Equals(Definition.VARIABLE_Y) && _hasSubconfigs)
                        {
                            //modified by enkim 2012.05.18 SPC-739
                            //IncreaseVersionOfSubConfigs(sConfigRawid.Split(';'));
                            //ATT//
                           bool bResultTmp = spcModelData.IncreaseVersionOfSubConfigs(configRawID);
                          /// ATT//
                            //modified end SPC-739

                        }
                        this.Commit();
                        break;
                }

                if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK))
                {
                    LinkedList llstCondition = new LinkedList();
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, configRawID);
                    llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, _sMainYN);
                    llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                    llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                    //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                    Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                    msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                }

                if ((configMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK)) && _sMainYN.Equals(Definition.VARIABLE_Y) && _hasSubconfigs)
                {
                    if (!_isGroup)
                    {
                        if (!bOnlyMain)
                        {
                            LinkedList llstCondition = new LinkedList();
                            llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sConfigRawid);
                            llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                            llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                            llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                            //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                            Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                            msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                        }
                    }
                    else
                    {
                        if (!bOnlyMain)
                        {
                            LinkedList llstCondition = new LinkedList();
                            llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sConfigRawid);
                            llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                            llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "remove");
                            llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                            //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                            Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                            msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                        }
                    }
                }

                if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN) || configMode.Equals(BISTel.eSPC.Common.ConfigMode.SAVE_AS))
                {
                    if (srefConfigRawid.Length > 0)
                    {
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, srefConfigRawid);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_Y);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "add");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }

                if (configMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_SUB))
                {
                    if (srefConfigRawid.Length > 0)
                    {
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, srefConfigRawid);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "add");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }


            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                base.RollBack();
            }
            finally
            {
                this.Close();
            }

            return dsReturn;
        }

        /// <summary>
        /// Increase versions of sub spc models
        /// </summary>
        /// <param name="subConfigRawids">sub spc model's rawid</param>
        /// <returns>error message or empty</returns>
        private string IncreaseVersionOfSubConfigs(string[] subConfigRawids)
        {
            BISTel.eSPC.Data.Server.Common.CommonData commonData = new BISTel.eSPC.Data.Server.Common.CommonData();
            foreach (string s in subConfigRawids)
            {
                foreach (string query in commonData.GetIncreaseATTVersionQuery(s))
                {
                    this.Query(query);
                    if (this.ErrorMessage.Length > 0)
                    {
                        return ErrorMessage;
                    }
                }
            }

            return string.Empty;
        }
        public bool SaveSPCATTMultiCalcModelData(byte[] param)
        {
            bool bReturn = true;

            try
            {
                spcModelData.ParentSQLHandler = this;

                base.BeginTrans();

                bReturn = spcModelData.SaveATTSpecMultiCalcData(param);

                if (base.ErrorMessage.Length > 0)
                {
                    this.RollBack();
                    return false;
                }

                this.Commit();

                if (bReturn)
                {
                    LinkedList llstParam = new LinkedList();
                    llstParam.SetSerialData(param);
                    if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                    {
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString());
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, llstParam[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString());
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                        //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                base.RollBack();
                return false;
            }
            finally
            {
                this.Close();
            }

            return bReturn;
        }

        public bool SaveSPCCalcModelData(byte[] param)
        {

            bool bReturn = true;

            try
            {
                spcModelData.ParentSQLHandler = this;

                base.BeginTrans();

                bReturn = spcModelData.SaveATTSpecCalcData(param);

                if (base.ErrorMessage.Length > 0)
                {
                    this.RollBack();
                    return false;
                }

                this.Commit();

                if (bReturn)
                {
                    LinkedList llstParam = new LinkedList();
                    llstParam.SetSerialData(param);
                    if (llstParam.Contains(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID))
                    {
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString());
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, llstParam[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString());
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                        //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                base.RollBack();
                return false;
            }
            finally
            {
                this.Close();
            }

            return bReturn;
        }

        public bool SaveInterlockYN(byte[] param)
        {
            bool bResult = true;

            string sMainConfigRawID = "";
            string sSubConfigRawID = "";

            try
            {
                bResult = spcModelData.SaveInterlockYN(param);
                if (bResult)
                {
                    LinkedList llstParam = new LinkedList();
                    llstParam.SetSerialData(param);

                    ArrayList arrRawID = new ArrayList();
                    ArrayList arrMainYN = new ArrayList();

                    if (llstParam.Contains(Definition.DynamicCondition_Condition_key.RAWID))
                    {
                        arrRawID = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.RAWID];
                    }
                    if (llstParam.Contains(Definition.DynamicCondition_Condition_key.MAIN_YN))
                    {
                        arrMainYN = (ArrayList)llstParam[Definition.DynamicCondition_Condition_key.MAIN_YN];
                    }

                    if (arrRawID.Count == arrMainYN.Count)
                    {
                        for (int i = 0; i < arrMainYN.Count; i++)
                        {
                            if (arrMainYN[i].ToString() == Definition.VARIABLE_Y)
                            {
                                sMainConfigRawID += ";" + arrRawID[i].ToString();
                            }
                            else
                            {
                                sSubConfigRawID += ";" + arrRawID[i].ToString();
                            }
                        }
                    }

                    if (sMainConfigRawID.Length > 0)
                    {
                        sMainConfigRawID = sMainConfigRawID.Substring(1);
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sMainConfigRawID);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_Y);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }

                    if (sSubConfigRawID.Length > 0)
                    {
                        sSubConfigRawID = sSubConfigRawID.Substring(1);
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sSubConfigRawID);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return bResult;
        }

        public DataSet DeleteSPCModelConfig(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);
                ArrayList arrTemp = (ArrayList)llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID];

                dsReturn = spcModelData.DeleteATTSPCModelConfig(param);

                string sConfigRawID = "";

                for (int i = 0; i < arrTemp.Count; i++)
                {
                    sConfigRawID += ";" + arrTemp[i].ToString().Trim();
                }

                if (sConfigRawID.Length > 0)
                {
                    sConfigRawID = sConfigRawID.Substring(1);
                    LinkedList llstCondition = new LinkedList();
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sConfigRawID);
                    llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                    llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "remove");
                    llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                    Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                    msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                }
            }
            catch
            {
            }
            finally
            {
            }

            return dsReturn;
        }

        public DataSet DeleteSPCModel(byte[] param)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                LinkedList llstParam = new LinkedList();
                llstParam.SetSerialData(param);

                dsReturn = spcModelData.DeleteSPCModel(param);

                string sConfigRawID = llstParam[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();

                if (sConfigRawID.Length > 0)
                {
                    sConfigRawID = sConfigRawID.Substring(1);
                    LinkedList llstCondition = new LinkedList();
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, _ComUtil.NVL(llstParam[Definition.CONDITION_KEY_MODEL_RAWID]));
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sConfigRawID);
                    llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_Y);
                    llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "remove");
                    llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                    Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                    msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                }
            }
            catch
            {
            }
            finally
            {
            }

            return dsReturn;
        }

        public DataSet CopyModelInfo(byte[] param)
        {
            DataSet dsReturn = null;

            try
            {
                spcModelData.ParentSQLHandler = this;

                LinkedList llstTotalConfigInfo = new LinkedList();
                LinkedList llstParam = null;
                llstTotalConfigInfo.SetSerialData(param);

                ArrayList arrMainTargetRawid = new ArrayList();
                ArrayList arrSubTargetRawid = new ArrayList();

                base.BeginTrans();

                for (int i = 0; i < llstTotalConfigInfo.Count; i++)
                {
                    llstParam = (LinkedList)llstTotalConfigInfo[i];

                    string sUserID = llstParam[Definition.DynamicCondition_Condition_key.USER_ID].ToString();

                    string sourceConfigRawID = llstParam[Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID].ToString();
                    string targetConfigRawID = llstParam[Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID].ToString();
                    string mainYN = llstParam[Definition.CONDITION_KEY_MAIN_YN].ToString();

                    if (mainYN == "Y")
                        arrMainTargetRawid.Add(targetConfigRawID);
                    else
                        arrSubTargetRawid.Add(targetConfigRawID);

                    bool hasSubconfigs = Convert.ToBoolean(llstParam[Definition.CONDITION_KEY_HAS_SUBCONFIG].ToString());

                    dsReturn = spcModelData.CopyModelInfo(llstParam);

                    if (base.ErrorMessage.Length > 0 || DSUtil.GetResultSucceed(dsReturn) == 0)
                    {
                        this.RollBack();
                        return dsReturn;
                    }
                }

                //modified by enkim Gemini P3-3816
                //string subConfigRawIDs = "";
                //if (mainYN.Equals(Definition.VARIABLE_Y) && hasSubconfigs)
                //{
                //    dsReturn = spcModelData.ModifySPCSubModelForCopy(llstParam, ref subConfigRawIDs);
                //    if (subConfigRawIDs.Length > 0)
                //    {
                //        subConfigRawIDs = subConfigRawIDs.Substring(1);
                //    }
                //}

                //modified end

                this.Commit();

                if (arrMainTargetRawid.Count > 0)
                {
                    LinkedList llstCondition = new LinkedList();
                    llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, arrMainTargetRawid[0].ToString());
                    llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, "Y");
                    llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                    llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                    //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                    Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                    msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                }
                if (arrSubTargetRawid.Count > 0)
                {
                    string subConfigRawIDs = "";

                    for (int i = 0; i < arrSubTargetRawid.Count; i++)
                    {
                        subConfigRawIDs += ";" + arrSubTargetRawid[i].ToString();
                    }

                    if (subConfigRawIDs.Length > 0)
                    {
                        subConfigRawIDs = subConfigRawIDs.Substring(1);
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, subConfigRawIDs);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, "N");
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());

                        //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }


                

                //modified by enkim Gemini P3-3816

                //if (mainYN.Equals(Definition.VARIABLE_Y) && hasSubconfigs)
                //{
                //    llstCondition = new LinkedList();
                //    llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, subConfigRawIDs);
                //    llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                //    llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");

                //    //수정했을 경우 Server로 변경에 대한 Inform을 준다.
                //    msgBussiness = new BISTel.eSPC.Business.Server.Interface.MsgInterfaceBusiness();
                //    msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                //}

                //modified end
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                base.RollBack();
            }
            finally
            {
                this.Close();
            }

            return dsReturn;
        }

        public bool SaveSPCModelSpecData(byte[] param)
        {

            bool bResult = true;

            string sMainConfigRawID = "";
            string sSubConfigRawID = "";

            try
            {
                bResult = spcModelData.SaveSPCModelSpecData(param);
                if (bResult)
                {
                    LinkedList llstParam = new LinkedList();
                    llstParam.SetSerialData(param);

                    ArrayList arrRawID = new ArrayList();
                    ArrayList arrMainYN = new ArrayList();

                    if (llstParam.Contains(BISTel.eSPC.Common.COLUMN.RAWID))
                    {
                        arrRawID = (ArrayList)llstParam[BISTel.eSPC.Common.COLUMN.RAWID];
                    }
                    if (llstParam.Contains(BISTel.eSPC.Common.COLUMN.MAIN_YN))
                    {
                        arrMainYN = (ArrayList)llstParam[BISTel.eSPC.Common.COLUMN.MAIN_YN];
                    }

                    if (arrRawID.Count == arrMainYN.Count)
                    {
                        for (int i = 0; i < arrMainYN.Count; i++)
                        {
                            if (arrMainYN[i].ToString() == Definition.VARIABLE_Y)
                            {
                                sMainConfigRawID += ";" + arrRawID[i].ToString();
                            }
                            else
                            {
                                sSubConfigRawID += ";" + arrRawID[i].ToString();
                            }
                        }
                    }

                    if (sMainConfigRawID.Length > 0)
                    {
                        sMainConfigRawID = sMainConfigRawID.Substring(1);
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sMainConfigRawID);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_Y);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }

                    if (sSubConfigRawID.Length > 0)
                    {
                        sSubConfigRawID = sSubConfigRawID.Substring(1);
                        LinkedList llstCondition = new LinkedList();
                        llstCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, sSubConfigRawID);
                        llstCondition.Add(Definition.CONDITION_KEY_MAIN_YN, Definition.VARIABLE_N);
                        llstCondition.Add(Definition.CONDITION_KEY_FUNCTION, "update");
                        llstCondition.Add(Definition.CONDITION_KEY_USER_ID, llstParam[BISTel.eSPC.Common.COLUMN.USER_ID].ToString());
                        Interface.MsgInterfaceBusiness msgBussiness = new BISTel.eSPC.Business.Server.ATT.Interface.MsgInterfaceBusiness();
                        msgBussiness.SetSPCModel(llstCondition.GetSerialData());
                    }
                }
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

            return bResult;
        }        
    }
}
