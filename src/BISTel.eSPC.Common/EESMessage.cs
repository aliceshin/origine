using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;

using System.Collections;


namespace BISTel.eSPC.Common
{
    public class EESMessage : DataBase
    {
        #region :Field

        private string _sServiceID = "";
        private string _sTarget = "";
        private string _sReplySubject = "";
        private string _sTid = "";
        private string _sSource = "";
        private string _sUser = "";
        private string _sEESTid = "";
        private string _sPid = "";
        private string _sType = "";
        private string _sMethodID = "";
        private string _sPcsid = "";
        private string _sEQPID = "";
        private string _sRecipeID = "";
        private string _sData = "";
        private string _sTimeOut = "";
        private string _sResult = "";
        private string _sError = "";
        private string _sError_Code = "";
        private string _sRMSModel = "";
        private string _sRecipe_List = "";
        private string _sProcess_Type = "";

        private Hashtable _htData = new Hashtable();
        private Hashtable _htReply = new Hashtable();

        #endregion

        #region :Properties

        public string ServiceID
        {
            set
            {
                this._sServiceID = value;
            }
            get
            {
                return _sServiceID;
            }
        }

        public string Target
        {
            set
            {
                this._sTarget = value;
            }
            get
            {
                return _sTarget;
            }
        }


        public string ReplySubject
        {
            set
            {
                this._sReplySubject = value;
            }
            get
            {
                return _sReplySubject;
            }
        }

        public string Tid
        {
            set
            {
                this._sTid = value;
            }
            get
            {
                return _sTid;
            }
        }

        public string Source
        {
            set
            {
                this._sSource = value;
            }
            get
            {
                return _sSource;
            }
        }

        public string User
        {
            set
            {
                this._sUser = value;
            }
            get
            {
                return _sUser;
            }
        }

        public string EESTid
        {
            set
            {
                this._sEESTid = value;
            }
            get
            {
                return _sEESTid;
            }
        }

        public string Pid
        {
            set
            {
                this._sPid = value;
            }
            get
            {
                return _sPid;
            }
        }


        public string Type
        {
            set
            {
                this._sType = value;
            }
            get
            {
                return _sType;
            }
        }


        public string MethodID
        {
            set
            {
                this._sMethodID = value;
            }
            get
            {
                return _sMethodID;
            }
        }

        public string Pcsid
        {
            set
            {
                this._sPcsid = value;
            }
            get
            {
                return _sPcsid;
            }
        }

        public string EQPID
        {
            set
            {
                this._sEQPID = value;
            }
            get
            {
                return _sEQPID;
            }
        }

        public string RecipeID
        {
            set
            {
                this._sRecipeID = value;
            }
            get
            {
                return _sRecipeID;
            }
        }

        public string Data
        {
            set
            {
                this._sData = value;
            }
            get
            {
                return _sData;
            }
        }

        public string TimeOut
        {
            set
            {
                this._sTimeOut = value;
            }
            get
            {
                return _sTimeOut;
            }
        }

        public string Result
        {
            set
            {
                this._sResult = value;
            }
            get
            {
                return _sResult;
            }
        }

        public string Error
        {
            set
            {
                this._sError = value;
            }
            get
            {
                return _sError;
            }
        }

        public string Error_Code
        {
            set
            {
                this._sError_Code = value;
            }
            get
            {
                return _sError_Code;
            }
        }

        public string Recipe_List
        {
            set
            {
                this._sRecipe_List = value;
            }
            get
            {
                return _sRecipe_List;
            }
        }

        public string RMS_Model
        {
            set
            {
                this._sRMSModel = value;
            }
            get
            {
                return _sRMSModel;
            }
        }

        public string ProcessType
        {
            set
            {
                this._sProcess_Type = value;
            }
            get
            {
                return _sProcess_Type;
            }
        }

        #endregion

        #region :Make Message
        public string MakeSendMessage()
        {
            string sMessage = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<service id=""{0}"" target=""{1}"" reply_subject=""{2}"" tid=""{3}"" source=""{4}"" user=""{5}"" ees_tid=""{6}"" _pid=""{7}"" pcsid=""{8}""> ",
                    this._sServiceID, this._sTarget, this._sReplySubject, this._sTid, this._sSource, this._sUser, this._sEESTid, this._sPid, this._sPcsid);
                sb.AppendFormat(@"<method id=""{0}"" type=""{1}"" >", this._sMethodID, this._sType);
                sb.AppendFormat(@"<data eqpid=""{0}"" recipeid=""{1}"" />", this._sEQPID, this._sRecipeID);
                sb.Append(@"</method>");
                sb.Append(@"</service>");

                sMessage = sb.ToString();
            }
            catch
            {

            }
            return sMessage;
        }


        public void AddValue(string sKey, string sValue)
        {
            this._htData.Add(sKey, sValue);
        }

        public string GetValue(string sType, string sKey)
        {
            string sValue = "";

            if (sType == "REPLY")
            {
                if (this._htReply.Contains(sKey))
                {
                    sKey = this._htReply[sKey].ToString();
                }
            }
            else
            {
                if (this._htData.Contains(sKey))
                {
                    sKey = this._htData[sKey].ToString();
                }
            }

            return sValue;
        }

        public string ToString()
        {
            //<service id="FDC" target="EQP05" reply_subject="" tid="0" source="FDCCLIENT" user="" ees_tid="" tid="" _pid="" pcsid="">
            //<method id="SET_TRACE_MODEL">
            //<data eqp_id="EQP05"/>
            //</method>
            //</service>


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<service id=""{0}"" target=""{1}"" reply_subject=""{2}"" tid=""{3}"" source=""{4}"" user=""{5}"" ees_tid=""{6}"" _pid=""{7}"" pcsid=""{8}""> ",
                this._sServiceID, this._sTarget, this._sReplySubject, this._sTid, this._sSource, this._sUser, this._sEESTid, this._sPid, this._sPcsid);
            //sb.AppendFormat(@"<method id=""{0}"" type=""{1}"" >", this._sMethodID, this._sType);
            sb.AppendFormat(@"<method id=""{0}"" >", this._sMethodID);

            if (this._sMethodID.Equals(MESSAGE._METHOD_ID_REVOKEWORKFLOW))
            {
                sb.Append(@"<revokeworkflow ");
            }
            else
            {
                sb.Append(@"<data ");
            }

            foreach (string key in this._htData.Keys)
            {
                string sValue = this._htData[key].ToString();
                sb.AppendFormat("{0}=\"{1}\" ", key, sValue);
            }
            sb.Append(@"/> ");

            sb.Append(@"</method>");
            sb.Append(@"</service>");

            return sb.ToString();
        }
        #endregion


        #region :Send and Wait Interface


        public string GetResult(string recvMsg)
        {
            string returnValue = null;
            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(recvMsg);
                XmlElement root = xmldoc.DocumentElement;

                xmldoc.LoadXml(root.InnerXml);
                root = xmldoc.DocumentElement;

                xmldoc.LoadXml(root.InnerXml);
                root = xmldoc.DocumentElement;

                returnValue = root.Attributes["result"].Value.ToString();
            }
            catch
            {
                return "";
            }

            return returnValue;
        }

        #endregion
    }
}
