using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;

using BISTel.PeakPerformance.Client.SQLHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;
using BISTel.eSPC.Data.Server;


using TIBCO.Rendezvous;

namespace BISTel.eSPC.Business.Server.Interface
{
    public class TIBInterface : DataBase
    {
        //private static volatile TiBInferface  
        //public static SortedList FrameworkList = new SortedList();

        //KEY : TABLE NAME, VALUE : CONFIG RAWID
        public static SortedList _slViewTableList = new SortedList();

        public static DataSet _dsConfigList = new DataSet();


        public static DataSet _dsConfigPropertyList = new DataSet();

        public static string _sEQPID = "";
        public static string _sConfig_RAWID = "";

        public static string _sFrameworkID = "";
        public static string _sMemberID = "";

        public static string _sProtocol = "";
        public static string _sSubjectName = "";

        public static string _sService = "";
        public static string _sNetwork = "";
        public static string _sDaemon = "";

        //private TIBInterface _tibinterface = null;

        public static string _sDataTypeCD = "";

        public TIBInterface()
        {
            this.SetTragetConfig();
        }

        ~TIBInterface()
        {
        }

        #region Se Traget Config

        public void SetTragetConfig()
        {
            try
            {
                //_sSubjectName = "AMOLED.A1.PROD.EQP_ID";
                //_sProtocol = "";
                //_sService = "8900";
                //_sNetwork = ";224.12.12.9";
                //_sDaemon = "";

                //Tib Config를 DB에서 가져오도록 변경
                BISTel.eSPC.Data.Server.InterfaceData tibData = new BISTel.eSPC.Data.Server.InterfaceData();

                LinkedList condition = new LinkedList();
                condition.Add(Definition.CONDITION_KEY_TARGET_NAME, Definition.TARGET_SEND_SERVER_TIB);

                DataSet dsTibCfgServer = tibData.QueryTargetConfig(condition.GetSerialData());

                DataRow[] drTibCfgServer = null;

                drTibCfgServer = dsTibCfgServer.Tables[0].Select("ITEM_TYPE = 'SUBJECT'");
                if (drTibCfgServer != null && drTibCfgServer.Length > 0)
                {
                    _sSubjectName = drTibCfgServer[0]["ITEM_NAME"].ToString();
                    _sSubjectName = _sSubjectName + ".EQP_ID";
                }

                drTibCfgServer = dsTibCfgServer.Tables[0].Select("ITEM_TYPE = 'SERVICE'");
                if (drTibCfgServer != null && drTibCfgServer.Length > 0)
                    _sService = drTibCfgServer[0]["ITEM_NAME"].ToString();

                drTibCfgServer = dsTibCfgServer.Tables[0].Select("ITEM_TYPE = 'NETWORK'");
                if (drTibCfgServer != null && drTibCfgServer.Length > 0)
                    _sNetwork = drTibCfgServer[0]["ITEM_NAME"].ToString();

                drTibCfgServer = dsTibCfgServer.Tables[0].Select("ITEM_TYPE = 'DAEMON'");
                if (drTibCfgServer != null && drTibCfgServer.Length > 0)
                    _sDaemon = drTibCfgServer[0]["ITEM_NAME"].ToString();

                drTibCfgServer = dsTibCfgServer.Tables[0].Select("ITEM_TYPE = 'DATA_TYPE_CD'");
                if (drTibCfgServer != null && drTibCfgServer.Length > 0)
                    _sDataTypeCD = drTibCfgServer[0]["ITEM_NAME"].ToString();



                //TIBData tibdata = new TIBData();
                //tibdata.ParentSQLHandler = this;

                //string sConfig_RAWID = "";

                //if (_slViewTableList.Count == 0)
                //{
                //    DataSet ds = tibdata.QueryViewTableList();

                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        {
                //            string sTableName = ds.Tables[0].Rows[i]["PROPERTY_VALUE"].ToString();
                //            string sConfigRawID = ds.Tables[0].Rows[i]["CONFIG_RAWID"].ToString();

                //            sConfig_RAWID = sConfigRawID;

                //            DataSet dsEQPList = tibdata.QueryViewEQPList(sTableName);

                //            if (dsEQPList.Tables.Count > 0)
                //            {
                //                ArrayList alEQPIDList = new ArrayList();

                //                for (int j = 0; j < dsEQPList.Tables[0].Rows.Count; j++)
                //                {
                //                    string sEQPID = dsEQPList.Tables[0].Rows[j]["EQP_ID"].ToString();

                //                    alEQPIDList.Add(sEQPID);
                //                }

                //                Hashtable htValues = new Hashtable();

                //                htValues.Add("CONFIG_RAWID", sConfigRawID);
                //                htValues.Add("EQP_LIST_DATASET", dsEQPList);
                //                htValues.Add("EQP_LIST", alEQPIDList);

                //                if (_slViewTableList[sTableName] == null)
                //                    _slViewTableList.Add(sTableName, htValues);
                //            }
                //        }
                //    }
                //}

                //if (_dsConfigList.Tables.Count == 0)
                //{
                //    _dsConfigList = tibdata.QueryConfigList();
                //}

                //if (_dsConfigPropertyList.Tables.Count == 0)
                //{
                //    _dsConfigPropertyList = tibdata.QueryConfigPropertyList();
                //}

                //if (_sSubjectName == "")
                //{
                //    DataSet dsSubjectInfo = tibdata.QuerySubjectName();

                //    if (dsSubjectInfo.Tables[0].Rows.Count > 0)
                //    {
                //        _sSubjectName = dsSubjectInfo.Tables[0].Rows[0]["PROPERTY_VALUE"].ToString();
                //    }
                //}

                //if (sConfig_RAWID != "")
                //{
                //    _sConfig_RAWID = sConfig_RAWID;
                //    string sFilter = string.Format("RAWID = '{0}'", sConfig_RAWID);
                //    DataRow[] dr = _dsConfigList.Tables[0].Select(sFilter);

                //    _sFrameworkID = dr[0]["FRAMEWORK_ID"].ToString();
                //    _sMemberID = dr[0]["MEMBER_ID"].ToString();

                //    string sFilter_Property = string.Format("CONFIG_RAWID = '{0}'", sConfig_RAWID);

                //    DataRow[] drProperty = _dsConfigPropertyList.Tables[0].Select(sFilter_Property);

                //    for (int k = 0; k < drProperty.Length; k++)
                //    {
                //        string sName = drProperty[k]["PROPERTY_NAME"].ToString();

                //        if (sName == "PROTOCOL")
                //        {
                //            _sProtocol = drProperty[k]["PROPERTY_VALUE"].ToString();
                //        }
                //        else if (sName == "SERVICE")
                //        {
                //            _sService = drProperty[k]["PROPERTY_VALUE"].ToString();
                //        }
                //        else if (sName == "NETWORK")
                //        {
                //            _sNetwork = drProperty[k]["PROPERTY_VALUE"].ToString();
                //        }
                //        else if (sName == "DAEMON")
                //        {
                //            _sDaemon = drProperty[k]["PROPERTY_VALUE"].ToString();
                //        }
                //    }
                //}
            }
            catch
            {
            }
        }

        #endregion

        #region TIB SEND


        public void Send(string data, EESMessage msgbuilder)
        {
            Transport transport = null;

            try
            {
                TIBCO.Rendezvous.Environment.Open();
            }
            catch (RendezvousException exception)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME+":TIBCO Initialize", exception);
            }

            try
            {
                // Real/Local 시
                transport = new TIBCO.Rendezvous.NetTransport(_sService, _sNetwork, _sDaemon);

                //transport = new NetTransport(service, network, daemon);


            }
            catch (RendezvousException exception)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME+":NetTransport Initialize", exception);
            }



            // Create the message
            //TIBCO.Rendezvous.Message
            TIBCO.Rendezvous.Message message = new TIBCO.Rendezvous.Message();

            // Set send subject into the message
            try
            {
                //#if (DEBUG)
                //                // Test 시
                //                message.SendSubject = "OYCP.HT.EES.TEST_DB";
                //#else
                //                // Real 시
                message.SendSubject = _sSubjectName;
                //#endif

            }
            catch (RendezvousException exception)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME+":message Initialize", exception);
            }

            try
            {
                if (_sDataTypeCD.Equals("BIN"))
                {
                    //#BINARY
                    Opaque opaque = new Opaque();
                    opaque.Value = Encoding.GetEncoding(51949).GetBytes(data);
                    message.AddField("DATA", opaque);
                }
                else
                {
                    //#TEXT
                    message.AddField("DATA", data);
                }

                transport.Send(message);

            }
            catch (RendezvousException exception)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME+":Send ", exception);
            }
            catch (Exception err)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, err);
            }


            // Close Environment, it will cleanup all underlying memory, destroy
            // transport and guarantee delivery.
            try
            {
                transport.Destroy();

                TIBCO.Rendezvous.Environment.Close();
            }
            catch (RendezvousException exception)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, exception);
            }
        }

        #endregion
    }
}
