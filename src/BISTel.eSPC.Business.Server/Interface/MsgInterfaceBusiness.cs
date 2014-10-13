using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Threading;


using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;
using BISTel.eSPC.Data.Server;

using TIBCO.Rendezvous;

namespace BISTel.eSPC.Business.Server.Interface
{
    public class MsgInterfaceBusiness : DataBase
    {

        #region : Thread

        private Hashtable Result_List = new Hashtable();
        private Hashtable Variable_List = new Hashtable();

        private string _MsgBusType = string.Empty;

        #endregion

        public MsgInterfaceBusiness()
        {
            this.GetMsgBusType();
        }


        private void GetMsgBusType()
        {
            //Default는 ActiveMQ
            _MsgBusType = Definition.TARGET_MSG_BUS_INFO_TIB;


            BISTel.eSPC.Data.Server.InterfaceData tibData = new BISTel.eSPC.Data.Server.InterfaceData();

            LinkedList condition = new LinkedList();
            condition.Add(Definition.CONDITION_KEY_TARGET_NAME, Definition.TARGET_MSG_BUS_INFO);

            DataSet dsMsgBusType = tibData.QueryTargetConfig(condition.GetSerialData());

            DataRow[] drMsgBusType = null;

            drMsgBusType = dsMsgBusType.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_MSG_BUS_TYPE));

            if (drMsgBusType != null && drMsgBusType.Length > 0)
            {
                _MsgBusType = drMsgBusType[0]["ITEM_NAME"].ToString();
            }
        }



        public string SetTraceModel(byte[] baData)
        {
            //string sSuccess = "";
            string sResult = "";

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sEQPID = "";

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();
                
                EESMessage eesmessage = new EESMessage();

                eesmessage.ServiceID = MESSAGE._SERVICE_ID;
                eesmessage.Target = sEQPID;
                eesmessage.ReplySubject = "";
                eesmessage.Tid = "0";
                eesmessage.Source = MESSAGE._SOURCE;
                eesmessage.User = llstData[Definition.CONDITION_KEY_USER_ID].ToString();
                eesmessage.EESTid = "";
                eesmessage.Pid = "";
                eesmessage.Type = "T";
                eesmessage.MethodID = MESSAGE._METHOD_ID_SET_TRACE_MODEL;
                

                eesmessage.AddValue(MESSAGE._EES_DATA_KEY_EQP_ID, sEQPID);

                int iTimeOut = MESSAGE._TIME_OUT;

                string sSendMessage = eesmessage.ToString();

                if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_TIB))
                {
                    TIBInterface tibinterface = new TIBInterface();
                    tibinterface.ParentSQLHandler = this;
                    //tibinterface.SetTragetConfig();

                    tibinterface.Send(sSendMessage, eesmessage);
                }
                else if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_AMQ))
                {
                    ActiveMQInterface amqinterface = new ActiveMQInterface();
                    amqinterface.Send(sSendMessage);
                    
                }
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            finally
            {
            }

            return sResult;
        }


        public string SetSummaryModel(byte[] baData)
        {
            //string sSuccess = "";
            string sResult = "";
            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sEQPID = "";

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                EESMessage eesmessage = new EESMessage();

                eesmessage.ServiceID = MESSAGE._SERVICE_ID;
                eesmessage.Target = sEQPID;
                eesmessage.ReplySubject = "";
                eesmessage.Tid = "0";
                eesmessage.Source = MESSAGE._SOURCE;
                eesmessage.User = llstData[Definition.CONDITION_KEY_USER_ID].ToString();
                eesmessage.EESTid = "";
                eesmessage.Pid = "";
                eesmessage.Type = "T";
                eesmessage.MethodID = MESSAGE._METHOD_ID_SET_SUMMARY_MODEL;

                eesmessage.AddValue(MESSAGE._EES_DATA_KEY_EQP_ID, sEQPID);

                int iTimeOut = MESSAGE._TIME_OUT;

                string sSendMessage = eesmessage.ToString();

                if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_TIB))
                {
                    TIBInterface tibinterface = new TIBInterface();
                    tibinterface.ParentSQLHandler = this;
                    //tibinterface.SetTragetConfig();

                    tibinterface.Send(sSendMessage, eesmessage);
                }
                else if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_AMQ))
                {
                    ActiveMQInterface amqinterface = new ActiveMQInterface();
                    amqinterface.Send(sSendMessage);
                }
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            finally
            {
            }

            return sResult;
        }


        public string SetSPCModel(byte[] baData)
        {
            string sResult = "";

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sEQPID = string.Empty;
                string sConfigRawID = string.Empty;
                string sMainYN = string.Empty;
                string sFunction = string.Empty;
                string sModelRawID = string.Empty;

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                if (llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID] != null)
                    sConfigRawID = llstData[Definition.CONDITION_KEY_MODEL_CONFIG_RAWID].ToString();

                if (llstData[Definition.CONDITION_KEY_MAIN_YN] != null)
                    sMainYN = llstData[Definition.CONDITION_KEY_MAIN_YN].ToString();

                if (llstData[Definition.CONDITION_KEY_FUNCTION] != null)
                    sFunction = llstData[Definition.CONDITION_KEY_FUNCTION].ToString();

                if(llstData[Definition.CONDITION_KEY_MODEL_RAWID] != null)
                    sModelRawID = llstData[Definition.CONDITION_KEY_MODEL_RAWID].ToString();


                EESMessage eesmessage = new EESMessage();

                eesmessage.ServiceID = MESSAGE._SERVICE_ID;
                eesmessage.Target = sEQPID;
                eesmessage.ReplySubject = "";
                eesmessage.Tid = "1";
                eesmessage.Source = MESSAGE._SOURCE;
                eesmessage.User = llstData[Definition.CONDITION_KEY_USER_ID].ToString();
                eesmessage.EESTid = "";
                eesmessage.Pid = "";
                eesmessage.Type = "T";
                eesmessage.MethodID = MESSAGE._METHOD_ID_SET_SPC_MODEL;

                eesmessage.AddValue("model_rawid", sModelRawID);
                eesmessage.AddValue("model_config_rawid", sConfigRawID);
                eesmessage.AddValue("is_main_model", sMainYN);
                eesmessage.AddValue("function", sFunction);

                int iTimeOut = MESSAGE._TIME_OUT;

                string sSendMessage = eesmessage.ToString();

                if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_TIB))
                {
                    TIBInterface tibinterface = new TIBInterface();
                    tibinterface.ParentSQLHandler = this;
                    //tibinterface.SetTragetConfig();

                    tibinterface.Send(sSendMessage, eesmessage);
                }
                else if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_AMQ))
                {
                    ActiveMQInterface amqinterface = new ActiveMQInterface();
                    amqinterface.Send(sSendMessage);

                }
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            finally
            {
            }

            return sResult;
        }


        public string SetRevokeWorkflow_ModelGroup(byte[] baData)
        {
            string sResult = "";

            try
            {
                LinkedList llstData = new LinkedList();
                llstData.SetSerialData(baData);

                string sEQPID = string.Empty;

                if (llstData[Definition.CONDITION_KEY_EQP_ID] != null)
                    sEQPID = llstData[Definition.CONDITION_KEY_EQP_ID].ToString();

                EESMessage eesmessage = new EESMessage();

                eesmessage.ServiceID = MESSAGE._SERVICE_ID;
                eesmessage.Target = sEQPID;
                eesmessage.ReplySubject = "";
                eesmessage.Tid = "1";
                eesmessage.Source = MESSAGE._SOURCE;
                eesmessage.User = llstData[Definition.CONDITION_KEY_USER_ID].ToString();
                eesmessage.EESTid = "";
                eesmessage.Pid = "";
                eesmessage.Type = "T";
                eesmessage.MethodID = MESSAGE._METHOD_ID_REVOKEWORKFLOW;

                eesmessage.AddValue("name", "SPC:ModelGroup");

                int iTimeOut = MESSAGE._TIME_OUT;

                string sSendMessage = eesmessage.ToString();

                if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_TIB))
                {
                    TIBInterface tibinterface = new TIBInterface();
                    tibinterface.ParentSQLHandler = this;
                    //tibinterface.SetTragetConfig();

                    tibinterface.Send(sSendMessage, eesmessage);
                }
                else if (_MsgBusType.Equals(Definition.TARGET_MSG_BUS_INFO_AMQ))
                {
                    ActiveMQInterface amqinterface = new ActiveMQInterface();
                    amqinterface.Send(sSendMessage);
                }

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ClientLogWrite(sSendMessage); 
            }
            catch (Exception ex)
            {
                EESUtil.ThrowExceptionMessage(ex.Message, ex.StackTrace);
            }
            finally
            {
            }

            return sResult;
        }
    }
}
