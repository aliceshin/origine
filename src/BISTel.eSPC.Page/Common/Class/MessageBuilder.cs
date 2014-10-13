using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BISTel.eSPC.Page.Common
{
    /// <summary>
    /// MessageBuilder에 대한 요약 설명입니다.
    /// </summary>
    public class MessageBuilder
    {
        private XmlDocument _xmldoc = null;

        public MessageBuilder(string user)
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        #region :Request Method

        /// <summary>
        /// DCP Client ==> DCP Server
        /// Request server to verify if the VIDs can be linked to the CEIC
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendRequestLink(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"ceid_list=""{0}""", data);
            string message = MakeMessage("DCP", eqpID, "REQUEST_LINK", sb.ToString());
            return message;
        }

        /// <summary>
        /// FDC Client ==> FDC Server
        /// Request to server to save event message with SML format
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetRealTime_FDC(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}"" second=""{1}""", eqpID, data);
            string message = MakeMessage("FDC", eqpID, "SET_REAL_TIME", sb.ToString());
            return message;
        }

        /// <summary>
        /// FDC Client ==> DCP Server
        /// Request to server to save event message with SML format
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetRealTime_DCP(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"second=""{0}""", data);
            string message = MakeMessage("DCP", eqpID, "SET_REAL_TIME", sb.ToString());
            return message;
        }

        public string SetModelUpdate_FDC(string eqpID, string moduleRawID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}"" module_rawid=""{1}""", eqpID, moduleRawID);
            string message = MakeMessage("FDC", eqpID, "SET_REAL_TIME", sb.ToString());
            return message;

            //<service id="FDC" target="CCDN01" reply_subject="" tid="0" source="DCP" user="" ees_tid="" _pid="" pcsid="" >
            //<method id="SET_MODEL_UPDATE" >
            //    <data eqp_id="CCDN01" module_id="1" module_rawid="65" recipe_name="PROCESS_RCP01" module_type="" 
            //      step_id="1" data_category_cd="TR" unit_type_cd="RE" spec_type_cd="NS" param_rawid="1" param_name="SiH4_SET" spec_rawid="1"  >
            //    </data>
            //</method>
            //</service>
        }

        /// <summary>
        /// DCP Client  ==> DCP Server
        /// 두 서버에 별도로 보내고 별도의 응답메시지 처리
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetEventModel_DCP(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("DCP", eqpID, "SET_EVENT_MODEL", sb.ToString());
            return message;
        }

        /// <summary>
        /// DCP Client  ==> FDC Server
        /// 두 서버에 별도로 보내고 별도의 응답메시지 처리
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetEventModel_FDC(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("FDC", eqpID, "SET_EVENT_MODEL", sb.ToString());
            return message;
        }

        /// <summary>
        /// DCP Client  ==> DCP Server
        /// 두 서버에 별도로 보내고 별도의 응답 메시지 처리
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetTraceModel_DCP(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("DCP", eqpID, "SET_TRACE_MODEL", sb.ToString());
            return message;
        }

        /// <summary>
        /// DCP Client  ==> FDC Server
        /// 두 서버에 별도로 보내고 별도의 응답 메시지 처리
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetTraceModel_FDC(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("FDC", eqpID, "SET_TRACE_MODEL", sb.ToString());
            return message;
        }

        /// <summary>
        /// DCP Client ==> DCP Server
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetSummayModel_DCP(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("DCP", eqpID, "SET_SUMMARY_MODEL", sb.ToString());
            return message;
        }

        /// <summary>
        /// DCP Client ==> FDC Server
        /// </summary>
        /// <param name="eqpID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SetSummayModel_FDC(string eqpID, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"eqp_id=""{0}""", data);
            string message = MakeMessage("FDC", eqpID, "SET_SUMMARY_MODEL", sb.ToString());
            return message;
        }



        #endregion

        #region :Make Message
        private string MakeMessage(string serviceId, string target, string method, string data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<service id=""{0}"" target=""{1}"" reply_subject=""{2}"" tid=""{3}"" source=""{4}"" user="""" ees_tid="""" _pid="""" pcsid=""""> ", serviceId, target, "", "", "CLIENT", "");
            sb.AppendFormat(@"<method id=""{0}"" >", method);
            sb.AppendFormat(@"<data {0} />", data);
            sb.Append(@"</method>");
            sb.Append(@"</service>");

            return sb.ToString();
        }
        #endregion

    }
}
