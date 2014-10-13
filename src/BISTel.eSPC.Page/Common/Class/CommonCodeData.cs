using System;
using System.Collections.Generic;
using System.Text;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

using System.Data;

namespace BISTel.eSPC.Page.Common
{
    public class CommonCodeData
    {
        #region : Field

        private eSPCWebService.eSPCWebService _spcWebService = null;

        #endregion

        #region : Initialization

        public CommonCodeData()
        {
            _spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
        }

        #endregion

        #region : Public

        public string GetConditionType(string name)
        {
            //condition type을 확인
            LinkedList llst = new LinkedList();
            llst.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_CONDITION_TYPE);
            byte[] btdata = llst.GetSerialData();

            DataSet ds = _spcWebService.GetCodeData(btdata);
            string sKey = name;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string config = ds.Tables[0].Rows[0][Definition.VARIABLE_CODE].ToString();
                if (config == Definition.CODE_CATEGORY_USE_TREE)
                {
                    sKey = name;
                    BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;
                }
                else
                {
                    sKey = "x";
                    BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = false;
                }
            }
            else
            {
                sKey = "x";
                BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = false;
            }

            return sKey;
        }


        public string GetMsgBusType()
        {
            string rtnType = Definition.TARGET_MSG_BUS_INFO_TIB;

            LinkedList conditoin = new LinkedList();
            conditoin.Add(Definition.CONDITION_KEY_TARGET_NAME, Definition.TARGET_MSG_BUS_INFO);

            DataSet dsMsgBusType = _spcWebService.GetTargetConfig(conditoin.GetSerialData());

            DataRow[] drMsgBusType = null;

            drMsgBusType = dsMsgBusType.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_MSG_BUS_TYPE));

            if (drMsgBusType != null && drMsgBusType.Length > 0)
            {
                rtnType = drMsgBusType[0]["ITEM_NAME"].ToString();
            }

            return rtnType;
        }


        public TIBConfig GetTIBConfig(string targetName)
        {
            //#TIB RECIVE CONIFG
            TIBConfig tibConfig = new TIBConfig();

            LinkedList conditoin = new LinkedList();
            conditoin.Add(Definition.CONDITION_KEY_TARGET_NAME, targetName);

            DataSet dsTibCfgClient = _spcWebService.GetTargetConfig(conditoin.GetSerialData());

            DataRow[] drTibCfgClinet = null;

            //SUBJECT
            drTibCfgClinet = dsTibCfgClient.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_SUBJECT));
            if (drTibCfgClinet != null && drTibCfgClinet.Length > 0)
                tibConfig.SUBJECT = drTibCfgClinet[0]["ITEM_NAME"].ToString();
            //SERVICE
            drTibCfgClinet = dsTibCfgClient.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_SERVICE));
            if (drTibCfgClinet != null && drTibCfgClinet.Length > 0)
                tibConfig.SERVICE = drTibCfgClinet[0]["ITEM_NAME"].ToString();
            //NETWORK
            drTibCfgClinet = dsTibCfgClient.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_NETWORK));
            if (drTibCfgClinet != null && drTibCfgClinet.Length > 0)
                tibConfig.NETWORK = drTibCfgClinet[0]["ITEM_NAME"].ToString();
            //DAEMON
            drTibCfgClinet = dsTibCfgClient.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_DAEMON));
            if (drTibCfgClinet != null && drTibCfgClinet.Length > 0)
                tibConfig.DAEMON = drTibCfgClinet[0]["ITEM_NAME"].ToString();
            //DATA TYPE CODE
            drTibCfgClinet = dsTibCfgClient.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_DATA_TYPE_CD));
            if (drTibCfgClinet != null && drTibCfgClinet.Length > 0)
                tibConfig.DATA_TYPE_CD = drTibCfgClinet[0]["ITEM_NAME"].ToString();

            return tibConfig;
        }



        public ActiveMQConfig GetActiveMQConfig(string targetName)
        {
            //#Active RECIVE CONIFG
            ActiveMQConfig amqConfig = new ActiveMQConfig();

            LinkedList conditoin = new LinkedList();
            conditoin.Add(Definition.CONDITION_KEY_TARGET_NAME, targetName);

            DataSet dsAMQConfig = _spcWebService.GetTargetConfig(conditoin.GetSerialData());

            DataRow[] drAMQConfig = null;

            //SUBJECT
            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_SUBJECT));
            if (drAMQConfig != null && drAMQConfig.Length > 0)
                amqConfig.SUBJECT = drAMQConfig[0]["ITEM_NAME"].ToString();
            //BROKER URL
            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_BROKER_URL));
            if (drAMQConfig != null && drAMQConfig.Length > 0)
                amqConfig.BROKER_URL = drAMQConfig[0]["ITEM_NAME"].ToString();
            //DESTINATION TYPE
            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_DESTINATION_TYPE));
            if (drAMQConfig != null && drAMQConfig.Length > 0)
                amqConfig.DESTINATION_TYPE = drAMQConfig[0]["ITEM_NAME"].ToString();
            //DATA TYPE CODE
            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_DATA_TYPE_CD));
            if (drAMQConfig != null && drAMQConfig.Length > 0)
                amqConfig.DATA_TYPE_CD = drAMQConfig[0]["ITEM_NAME"].ToString();

            return amqConfig;
        }


        #endregion




    }

    public class TIBConfig
    {
        public string SUBJECT = string.Empty;
        public string NETWORK = string.Empty;
        public string DAEMON = string.Empty;
        public string SERVICE = string.Empty;
        public string DATA_TYPE_CD = string.Empty;
    }

    public class ActiveMQConfig
    {
        public string SUBJECT = string.Empty;
        public string BROKER_URL = string.Empty;
        public string DESTINATION_TYPE = string.Empty;
        public string DATA_TYPE_CD = string.Empty;
    }
}
