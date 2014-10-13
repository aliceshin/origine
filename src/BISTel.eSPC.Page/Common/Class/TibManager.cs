using System;
using System.Collections.Generic;
using System.Text;
using TIBCO.Rendezvous;
using System.Net;
using System.IO;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Common
{
    class TibManager
    {
        public delegate void ReceiveEventHandler(object sender);

        public event ReceiveEventHandler ReceivedXml;

        private NetTransport transport = null;
        private Listener listeners = null;
        private Dispatcher dispatcher = null;
        private TIBCO.Rendezvous.Queue queue = null;

        public ushort FLAG = 1;

        public bool Enable = false;

        public bool _isNull = true;
        //public string _subject = "L8ZFAB.HT.EESRT.TEST";
        public string _subject = "CLIENT_TIB.FDCTEST1";

        private System.DateTime LastMessageTime = DateTime.Now;

        public string ERROR_MESSAGE = string.Empty;

        public TibManager()
        {
            _isNull = true;
        }

        ~TibManager()
        {
            if (_isNull)
            {
                this.CloseTibManager();
            }
        }

        protected void OnReceived(object ob, EventArgs e)
        {
            try
            {
                if (ob != null)
                    if (ob.ToString().Length > 0)
                        ReceivedXml(ob);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void OnMessageReceived(object listener, MessageReceivedEventArgs messageReceivedEventArgs)
        {
            try
            {
                TIBCO.Rendezvous.Message message = messageReceivedEventArgs.Message;
                LastMessageTime = System.DateTime.Now;
                if (ReceivedXml != null)
                    this.ReceivedXml.BeginInvoke(message, null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void OpenTibManager(string subject, string service, string network, string daemon)
        {
            try
            {
                this.setTibDll();

                this._subject = subject;

                TIBCO.Rendezvous.Environment.Open();
                Enable = true;

                transport = new NetTransport(service, network, daemon);
            }
            catch (RendezvousException ex)
            {
                ex.ToString();
                try
                {
                    transport = new NetTransport("", "", "");
                    //WriteLog("OpenTibManager(2)", ex.ToString());
                }
                catch (RendezvousException e)
                {
                    this.ERROR_MESSAGE = e.Message;
                }
            }

            // Create Queue
            try
            {
                queue = new TIBCO.Rendezvous.Queue();
            }
            catch (RendezvousException ex)
            {
                this.ERROR_MESSAGE = ex.Message;
            }

            // Create Dispatcher
            try
            {
                dispatcher = new Dispatcher(queue);
            }
            catch (RendezvousException ex)
            {
                this.ERROR_MESSAGE = ex.Message;
            }
            try
            {
                CreateTibManager();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void CloseTibManager()
        {
            try
            {
                DestroyTibManager();
                Enable = false;
                //WriteLog("CloseTibManager(1)", "Close");
            }
            catch (Exception ex)
            {
                //WriteLog("CloseTibManager(2)", ex.ToString());
                this.ERROR_MESSAGE = ex.Message;
            }

            // Closing Environment
            try
            {
                dispatcher.Destroy();
                TIBCO.Rendezvous.Environment.Close();
                _isNull = false;
                ReceivedXml = null;

                //WriteLog("CloseTibManager(3)", "");
            }
            catch (RendezvousException ex)
            {
                this.ERROR_MESSAGE = ex.Message;
            }
        }

        public void CreateTibManager()
        {
            // create listener using default queue
            try
            {
                listeners = new Listener(queue, transport, _subject, null);
                listeners.MessageReceived += new MessageReceivedEventHandler(OnMessageReceived);
                LastMessageTime = System.DateTime.Now;
            }
            catch (RendezvousException ex)
            {
                this.ERROR_MESSAGE = ex.Message;
            }
        }

        public void DestroyTibManager()
        {
            try
            {
                if (listeners != null)
                {
                    listeners.MessageReceived -= new MessageReceivedEventHandler(OnMessageReceived);
                    listeners.Destroy();
                    listeners = null;
                }
            }
            catch (Exception ex)
            {
                this.ERROR_MESSAGE = ex.Message;
            }
        }

        private void AddObject(Message message, string xDoc, ushort nFlag, string sFildName)
        {
            message.AddField(sFildName, xDoc);
        }

        public string GetReplaySubjectName()
        {
            return _subject;
        }

        public void setTibDll()
        {
            //string sClientPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @"\EES\";
            //string sClientPath = System.Environment.SystemDirectory;

            string sClientPath = System.Windows.Forms.Application.ExecutablePath;

            int iPos = sClientPath.LastIndexOf(@"\");
            sClientPath = sClientPath.Substring(0, iPos);

            string sServerPath = BISTel.PeakPerformance.Client.CommonLibrary.Configuration.getInstance().URL + "/DLLS/";

            string sRealPath = System.Environment.GetEnvironmentVariable("path").ToUpper();
            if (sRealPath.IndexOf(sClientPath.ToUpper()) < 0)
            {
                sRealPath = sRealPath + ";" + sClientPath;
                System.Environment.SetEnvironmentVariable("path", sRealPath);
            }

            if (!System.IO.Directory.Exists(sClientPath))
            {
                System.IO.Directory.CreateDirectory(sClientPath);
            }

            string[] sFiles = { "TIBCO.Rendezvous.dll", "TIBCO.Rendezvous.netmodule", "tibrv.dll", "tibrv.tkt", 
                                  "tibrvcm.dll", "tibrvcmq.dll", "tibrvft.dll", "tibrvj.dll", /*"tibrvnative.dll", "tibrvnativesd.dll",*/ "rvd.dll" };

            try
            {

                for (int i = 0; i < sFiles.Length; i++)
                {
                    string sClientFilePath = sClientPath + @"\" + sFiles[i];
                    string sServerFilePath = sServerPath + sFiles[i];

                    if (i == sFiles.Length - 1)
                    {
                        sClientFilePath = sClientFilePath.Replace("dll", "exe");
                    }

                    if (!System.IO.File.Exists(sClientFilePath))
                    {
                        WebRequest req = WebRequest.Create(sServerFilePath);
                        WebResponse rep = req.GetResponse();

                        System.IO.Stream sm = rep.GetResponseStream();

                        byte[] bytes = new byte[1024];
                        int iCount = 0;

                        if (i == sFiles.Length - 1)
                        {
                            sClientFilePath.Replace("dll", "exe");
                        }

                        System.IO.FileStream sw = new System.IO.FileStream(sClientFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        do
                        {
                            iCount = sm.Read(bytes, 0, bytes.Length);

                            if (iCount != 0)
                            {
                                sw.Write(bytes, 0, iCount);
                            }
                        } while (iCount > 0);


                        sw.Close();
                        sm.Close();

                        if (sClientFilePath.Substring(sClientFilePath.Length - 3).ToUpper().Equals("DLL"))
                        {
                            //System.Diagnostics.Process.Start("regsvr32.exe", "\"" + sClientFilePath + "\"");
                        }

                    }
                }
            }
            catch (Exception e)
            {
                //MSGHandler.DisplayMessage(MSGType.Information, e.Message);

                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ClientLogWrite(e.Message);
            }

        }

        public void WriteLog(String logTitle, String logMessage)
        {
            string path = @"C:\log\Tib";
            string fileName = @"C:\log\Tib\Log_";
            string Date = "";

            Date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

            fileName = fileName + Date + ".txt";

            try
            {
                if (Directory.Exists(path) == false)
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine("The process failed: {0}", e.ToString());
                e.ToString();
            }

            StreamWriter w = File.AppendText(fileName);

            w.WriteLine("\r\nLog => *** {0} *** ({1} {2})", logTitle, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            w.WriteLine(" :: {0}", logMessage);
            w.Flush();

            w.Close();
        }
    }
}
