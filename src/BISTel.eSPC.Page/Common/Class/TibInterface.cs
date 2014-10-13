using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.Common
{
    public enum RealtimeSubjectType
    {
        EVENT,
        TRACE
    }

    class TibInterface
    {
        public delegate void RealtimeEventHandler(string message);

        public event RealtimeEventHandler OnRealtimeEvent;

        private TibManager _tibManager;

        public TibInterface()
        {
        }

        ~TibInterface()
        {
        }

        public void FireOnRealtimeEvent(string message)
        {
            if (OnRealtimeEvent != null)
            {
                OnRealtimeEvent.BeginInvoke(message, null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">EVENT | TRACE</param>
        /// <param name="line"></param>
        /// <param name="eqp"></param>
        /// <param name="service"></param>
        /// <param name="network"></param>
        /// <param name="daemon"></param>
        public string TIBOpen(RealtimeSubjectType type, string line, string eqp, string service, string network, string daemon)
        {
            string result = string.Empty;

            //ex) L8ZFAB.RT.EES.EVENT.8ZFSE51
            string subject = "CLIENT_TIB." + eqp;
            service = "7500";
            network = "";
            daemon = "17.91.171.147:7500";

            //string subject = line + ".RT.EES." + type.ToString() + "." + eqp;

            _tibManager = new TibManager();
            _tibManager.ReceivedXml += new TibManager.ReceiveEventHandler(ReceiveXmlDocument);

            _tibManager.OpenTibManager(subject, service, network, daemon);

            if (_tibManager != null)
            {
                result = _tibManager.ERROR_MESSAGE;
            }

            return result;
        }

        public string TIBOpen(RealtimeSubjectType type, string line, string eqp, string subject, string service, string network, string daemon)
        {
            string result = string.Empty;

            //ex) CLIENT_TIB.eSPC.EQPID
            string sSubject = string.Format("{0}.{1}.{2}", subject, "eSPC", eqp);
            //string sSubject = subject + "." + eqp;

            //string subject = line + ".RT.EES." + type.ToString() + "." + eqp;

            _tibManager = new TibManager();
            _tibManager.ReceivedXml += new TibManager.ReceiveEventHandler(ReceiveXmlDocument);

            _tibManager.OpenTibManager(sSubject, service, network, daemon);

            if (_tibManager != null)
            {
                result = _tibManager.ERROR_MESSAGE;
            }

            return result;
        }


        public string TIBClose()
        {
            string result = string.Empty;

            try
            {
                OnRealtimeEvent = null;
                _tibManager.CloseTibManager();
                _tibManager.ReceivedXml -= new TibManager.ReceiveEventHandler(ReceiveXmlDocument);

                if (_tibManager != null)
                {
                    result = _tibManager.ERROR_MESSAGE;
                }

                _tibManager = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return result;
        }

        private void ReceiveXmlDocument(object sender)
        {
            try
            {
                TIBCO.Rendezvous.Message message = (TIBCO.Rendezvous.Message)sender;

                string strMessage = message.GetField("DATA").Value.ToString();

                //Fire Event Message
                this.FireOnRealtimeEvent(strMessage);

                //InsertQueue(sender);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //    _log.WriteErrorLog(ex.ToString());
            }
        }

    }
}
