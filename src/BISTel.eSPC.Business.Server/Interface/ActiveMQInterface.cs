using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.ActiveMQ;

namespace BISTel.eSPC.Business.Server.Interface
{
    /*
     * modified by hanson 2010.09.01
     * 
     * */

    public class ActiveMQInterface
    {
        private ActiveMQAgent mMsgAgent = null;

        public static string _sBrokerUrl = "";
        public static string _sDestinationType = "";
        public static string _sSubject = "";


        public ActiveMQInterface()
        {
            this.SetTragetConfig();

            this.mMsgAgent = new ActiveMQAgent();
        }

        ~ActiveMQInterface()
        {
            if (mMsgAgent != null)
            {
                mMsgAgent = null;
            }
        }

        public void SetTragetConfig()
        {
            try
            {
                //#ActiveMQ SEND CONIFG
                BISTel.eSPC.Data.Server.InterfaceData tibData = new BISTel.eSPC.Data.Server.InterfaceData();

                LinkedList condition = new LinkedList();
                condition.Add(Definition.CONDITION_KEY_TARGET_NAME, Definition.TARGET_SEND_SERVER_AMQ);

                DataSet dsAMQConfig = tibData.QueryTargetConfig(condition.GetSerialData());


                foreach (DataRow row in dsAMQConfig.Tables[0].Rows)
                {
                    if (row["ITEM_TYPE"].ToString().ToUpper() == Definition.TARGET_CONFIG_ITEM_BROKER_URL)
                    {
                        _sBrokerUrl = row["ITEM_NAME"].ToString();
                    }
                    else if (row["ITEM_TYPE"].ToString().ToUpper() == Definition.TARGET_CONFIG_ITEM_SUBJECT)
                    {
                        _sSubject = row["ITEM_NAME"].ToString();
                    }
                    else if (row["ITEM_TYPE"].ToString().ToUpper() == Definition.TARGET_CONFIG_ITEM_DESTINATION_TYPE)
                    {
                        _sDestinationType = row["ITEM_NAME"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite("ActiveMQInterface SetTragetConfig", ex);
            }
        }

        public void Send(string sMessage)
        {
            try
            {
                this.mMsgAgent.Send(_sBrokerUrl, _sDestinationType, _sSubject, sMessage);
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite("ActiveMQInterface Send", ex);
            }
        }
    }

    //public class ActiveMQInterface
    //{
    //    private ActiveMQSender _sender = null;
        
    //    public static string _sBrokerUrl = "";
    //    public static string _sDestinationType = "";
    //    public static string _sSubject = "";


    //    public ActiveMQInterface()
    //    {
    //        this.SetTragetConfig();
    //    }

    //    ~ActiveMQInterface()
    //    {
    //        if (_sender != null) _sender.disconnect();
    //    }

    //    public void SetTragetConfig()
    //    {
    //        try
    //        {
    //            //#ActiveMQ SEND CONIFG
    //            BISTel.eSPC.Data.Server.InterfaceData tibData = new BISTel.eSPC.Data.Server.InterfaceData();

    //            LinkedList condition = new LinkedList();
    //            condition.Add(Definition.CONDITION_KEY_TARGET_NAME, Definition.TARGET_SEND_SERVER_AMQ);

    //            DataSet dsAMQConfig = tibData.QueryTargetConfig(condition.GetSerialData());

    //            DataRow[] drAMQConfig = null;

    //            //SUBJECT
    //            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_SUBJECT));
    //            if (drAMQConfig != null && drAMQConfig.Length > 0)
    //                _sSubject = drAMQConfig[0]["ITEM_NAME"].ToString();
    //            //BROKER URL
    //            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_BROKER_URL));
    //            if (drAMQConfig != null && drAMQConfig.Length > 0)
    //                _sBrokerUrl = drAMQConfig[0]["ITEM_NAME"].ToString();
    //            //DESTINATION TYPE
    //            drAMQConfig = dsAMQConfig.Tables[0].Select(string.Format("ITEM_TYPE = '{0}'", Definition.TARGET_CONFIG_ITEM_DESTINATION_TYPE));
    //            if (drAMQConfig != null && drAMQConfig.Length > 0)
    //                _sDestinationType = drAMQConfig[0]["ITEM_NAME"].ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHandler.ExceptionLogWrite("ActiveMQInterface SetTragetConfig", ex);
    //        }
    //    }


    //    public void Send(string sMessage)
    //    {
    //        try
    //        {
    //            if (_sender == null)
    //            {
    //                _sender = new ActiveMQSender(_sBrokerUrl);
    //                _sender.connect();
    //            }

    //            if (_sDestinationType.ToUpper().Equals(Definition.ACTIVEMQ_DESTINATION_TYPE_QUEUE))
    //            {
    //                _sender.sendToQueue(_sSubject, sMessage);
    //            }
    //            else if (_sDestinationType.ToUpper().Equals(Definition.ACTIVEMQ_DESTINATION_TYPE_TOPIC))
    //            {
    //                _sender.sendToTopic(_sSubject, sMessage);
    //            }

    //            System.Diagnostics.Debug.WriteLine("Sent : Subject = " + _sSubject + ", msg = " + sMessage);
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Diagnostics.Debug.WriteLine(ex.Message);
    //            LogHandler.ExceptionLogWrite("ActiveMQInterface Send", ex);
    //        }
    //    }
    //}
}
