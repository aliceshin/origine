using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public enum MessageType
    {
        REAL_TRACE_DATA,
        REAL_STATUS_DATA,
        REAL_ALARM_DATA,
        REAL_INTERLOCK_DATA
    }

    public class MessageParser
    {
        // Method
        public string method = string.Empty;

        // Data
        public string eqpID = string.Empty;
        public string eventTime = string.Empty;
        public string eqpState = string.Empty;
        public string lotIDList = string.Empty;
        public string lotTypeList = string.Empty;
        public string carrierID = string.Empty;
        public string process = string.Empty;
        public string rCode = string.Empty;

        public string alarmID = string.Empty;
        public string alarmCode = string.Empty;
        public string alarmText = string.Empty;

        // Data or Record
        public string slot = string.Empty;
        public string lotID = string.Empty;
        public string substrateID = string.Empty;
        public string moduleID = string.Empty;
        public string moduleRawID = string.Empty;
        public string moduleName = string.Empty;
        public string ppid = string.Empty;
        public string product = string.Empty;
        public string recipe = string.Empty;
        public string status = string.Empty;
        public string step = string.Empty;
        public string stepDuration = string.Empty;

        public string trxRawID = string.Empty;
        public string specRawID = string.Empty;
        public string dataCategory = string.Empty;
        public string specType = string.Empty;
        public string faultType = string.Empty;
        public string value = string.Empty;
        public string target = string.Empty;
        public string usl = string.Empty;
        public string ucl = string.Empty;
        public string lcl = string.Empty;
        public string lsl = string.Empty;
        public string comment = string.Empty;
        public string send2mes = string.Empty;
        public string send2bc = string.Empty;

        public string paramAlias = string.Empty;//public string paramRawID = string.Empty;
        public string paramName = string.Empty;
        public string faultLevel = string.Empty;

        public string dataList = string.Empty;

        public string[] paramAliasList = null;//public string[] paramRawIDList = null;
        public string[] paramValueList = null;

        public ParamData[] paramDataList = null;
        public ParamData[] modelDataList = null;
        public Hashtable htParamDataList = null;

        SortedList _slContextTypeList = new SortedList();

        public MessageParser()
        {
        }

        public MessageParser(string message, string rawid)
        {
            Parser(message, rawid);
        }

        public MessageParser(string message, string rawid, SortedList slContextTypeList)
        {
            this._slContextTypeList = slContextTypeList;
            Parser(message, rawid);

        }

        public void Parser(string message, string rawid)
        {
            try
            {
                XmlDocument xmlDocu = new XmlDocument();
                xmlDocu.LoadXml(message);

                XmlNode nodeService = xmlDocu.ChildNodes[0];
                XmlNode nodeMethod = nodeService.ChildNodes[0];

                this.method = GetAttributeNamedValue(nodeMethod, "id");

                //if (this.method == MessageType.REAL_TRACE_DATA.ToString())
                //{
                //    this.ParseTraceData(nodeMethod, rawid);
                //}
                //else if (this.method == MessageType.REAL_STATUS_DATA.ToString())
                //{
                //    this.ParseStatusData(nodeMethod);
                //}
                //else if (this.method == MessageType.REAL_ALARM_DATA.ToString())
                //{
                //    this.ParseAlarmData(nodeMethod);
                //}
                //else if (this.method == MessageType.REAL_INTERLOCK_DATA.ToString())
                //{
                //    this.ParseInterlockData(nodeMethod);
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite("eSPC_MessageParser", ex);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ClientLogWrite("[RealTime Message Parsing Exception]\r\nReceiveMsg : " + message);
            }

            //BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite("", 
            //BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.LogWrite("RealTimeMsgParse", "Parsing Exception", BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.LogType.Error, e.Message + "\r\n" + message);
        }

        //Message Example
        //<service id="CLIENT_BMS" target="CCDN01" replysubject="" tid="0" 
        //    ees_tid="CCDN01_TRACE_REPORT_8169_2007-08-31 13:15:25.625" source="FDC" user="" _pid="30015984" 
        //    replyKey="">
        //<method id="REAL_TRACE_DATA" type="T">
        //<data eqp_id="CCDN01" eventtime="2007-08-31 13:15:25.609" module_rawid_list="65">

        //    <record carrier_id="CAR01" cassette_slot="2" lot_id_list="LOT2915" lot_type_list="P" module_id="1" 
        //    module_name="CCDN01-01" module_rawid="65" 
        //    param_rawid_list="1,2,6,7,8,9,10" param_value_list="32,56,36,34.5016126611448,,," 
        //    ppid="PPID" product_id="N/A" recipe_name="PROCESS_RCP01" status="R" step_duration="93000" 
        //    step_id="2" step_start_dtts="2007-08-31 13:15:25.609" substrate_id="LOT2915-02" 
        //    data_list="	10=@	
        //            7=34.5016126611448@51.50161266114482^3.50161266114483^3.50161266114483^99.5016126611448^99.5016126611448^S^PM^	
        //            2=56.0@51.50161266114482^3.50161266114483^3.50161266114483^99.5016126611448^99.5016126611448^S^PM^	
        //            9=@	
        //            8=@	
        //            6=36@75.0^53.0^55.0^95.0^98.0^F^MB^~25.0^2.0^5.0^45.0^48.0^S^MB^	
        //            1=32@75.0^53.0^55.0^95.0^98.0^F^MB^~25.0^2.0^5.0^45.0^48.0^S^MB^
        //    "/>
        //    [#text: 
        //        ][#text: 
        //    ]
        //</data>
        //</method>
        //</service>


        private string GetAttributeNamedValue(XmlNode node, string name)
        {
            string data = string.Empty;

            try
            {
                if (node.Attributes.GetNamedItem(name) != null)
                {
                    data = node.Attributes.GetNamedItem(name).Value;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
            }

            return data;
        }

        private void DataParser(string dataList)
        {
            string paramDataString = GetString(dataList, string.Empty, "\n");//Environment.NewLine);
            string modelDataString = GetString(dataList, "\n", string.Empty);//Environment.NewLine, string.Empty);

            string[] paramInfoList = paramDataString.Split('\t');
            string[] modelInfoList = modelDataString.Split('\t');

            bool isOK = true;

            if (paramDataString != string.Empty && paramInfoList.Length > 0)
            {
                this.paramDataList = new ParamData[paramInfoList.Length];
                this.htParamDataList = new Hashtable();

                for (int i = 0; i < paramInfoList.Length; i++)
                {
                    ParamData paramData = new ParamData();
                    isOK = paramData.Parser(paramInfoList[i]);

                    if (isOK)
                    {
                        this.paramDataList[i] = paramData;
                        htParamDataList.Add(this.paramDataList[i].ParamAlias, paramData);
                    }
                }
            }

            if (modelDataString != string.Empty && modelInfoList.Length > 0)
            {
                this.modelDataList = new ParamData[modelInfoList.Length];

                for (int i = 0; i < modelInfoList.Length; i++)
                {
                    ParamData modelData = new ParamData();
                    isOK = modelData.Parser(modelInfoList[i]);

                    if (isOK)
                    {
                        this.modelDataList[i] = modelData;
                    }
                }
            }

        }

        /// <summary>
        /// 해당 String Data에서 원하는 부분의 String을 추출
        /// </summary>
        /// <param name="source">전체 문자열</param>
        /// <param name="first">시작하는 문자열</param>
        /// <param name="dest">끝나는 문자열</param>
        /// <returns>추출된 문자열</returns>
        public static string GetString(string source, string first, string dest)
        {
            if ((source == null) || (first == null) || (dest == null))
            {
                return string.Empty;
            }

            if (source == string.Empty)
            {
                return source;
            }

            // 전체 String에서 시작문자열이 끝나는 Index를 찾는다.
            int iFirstEnd = 0;
            if (first != string.Empty)
            {
                iFirstEnd = source.IndexOf(first);
                if (iFirstEnd == -1)
                {
                    return string.Empty;
                }
                else
                {
                    iFirstEnd += first.Length;
                }
            }

            // 시작문자열이 끝나는 Index부터 Data를 저장한다.
            string source1 = source.Substring(iFirstEnd);

            if (dest == string.Empty)
            {
                return source1;
            }

            // 저장된 문자열에서 dest가 시작하는 Index를 찾는다.
            int iDestStart = source1.IndexOf(dest);

            if (iDestStart == -1)
            {
                return string.Empty;
            }

            // 저장된 문자열에서 dest index까지 문자열을 추출한다.
            string source2 = source1.Substring(0, iDestStart);

            return source2;
        }

        /// <summary>
        /// 해당 String Data에서 원하는 부분의 String을 추출  마지막 dest를 뒤에서 부터 Find
        /// </summary>
        /// <param name="source">전체 문자열</param>
        /// <param name="first">시작하는 문자열</param>
        /// <param name="dest">마지막으로 끝나는 문자열</param>
        /// <returns>추출된 문자열</returns>
        public static string GetLastString(string source, string first, string dest)
        {
            if ((source == null) || (first == null) || (dest == null))
            {
                return string.Empty;
            }

            if (source == string.Empty)
            {
                return source;
            }

            // 전체 String에서 시작문자열이 끝나는 Index를 찾는다.
            int iFirstEnd = 0;
            if (first != string.Empty)
            {
                iFirstEnd = source.IndexOf(first);
                if (iFirstEnd == -1)
                {
                    return string.Empty;
                }
                else
                {
                    iFirstEnd += first.Length;
                }
            }

            // 시작문자열이 끝나는 Index부터 Data를 저장한다.
            string source1 = source.Substring(iFirstEnd);

            if (dest == string.Empty)
            {
                return source1;
            }

            // 저장된 문자열에서 마지막 dest가 시작하는 Index를 찾는다.
            int iDestStart = source1.LastIndexOf(dest);

            if (iDestStart == -1)
            {
                return string.Empty;
            }

            // 저장된 문자열에서 dest index까지 문자열을 추출한다.
            string source2 = source1.Substring(0, iDestStart);

            return source2;
        }

    }

    public class ParamData
    {
        public string ParamAlias = string.Empty;
        public string ParamValue = string.Empty;
        public string ParamName = string.Empty;

        public ArrayList alSpecData = new ArrayList();

        public ParamData()
        {
        }

        public bool Parser(string paramData)
        {
            // ex) 1=32@75.0^53.0^55.0^95.0^98.0^F^MB^~25.0^2.0^5.0^45.0^48.0^S^MB^

            this.ParamAlias = MessageParser.GetString(paramData, string.Empty, "=");
            this.ParamValue = MessageParser.GetString(paramData, "=", "@");

            if (this.ParamAlias == string.Empty || this.ParamValue == string.Empty)
            {
                return false;
            }

            string specInfo = MessageParser.GetString(paramData, "@", string.Empty);

            string[] specList = specInfo.Split('~');

            for (int i = 0; i < specList.Length; i++)
            {
                SpecData specData = new SpecData();
                bool isOK = specData.Parser(specList[i]);

                if (isOK)
                {
                    alSpecData.Add(specData);
                }
            }

            return true;
        }
    }

    public class SpecData
    {
        public string Target = string.Empty;
        public string USL = string.Empty;
        public string UCL = string.Empty;
        public string LCL = string.Empty;
        public string LSL = string.Empty;
        public string IsFault = string.Empty;
        public string SpecType = string.Empty;
        public string Description = string.Empty;

        public SpecData()
        {
        }

        public bool Parser(string specData)
        {
            // ex) 75.0^53.0^55.0^95.0^98.0^F^MB^_ 
            string[] specInfo = specData.Split('^');

            if (specInfo.Length == 8)
            {
                this.Target = specInfo[0];
                this.LSL = specInfo[1];
                this.LCL = specInfo[2];
                this.UCL = specInfo[3];
                this.USL = specInfo[4];
                this.IsFault = specInfo[5];
                this.SpecType = specInfo[6];
                this.Description = specInfo[7];

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
