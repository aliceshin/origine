using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.Tool
{
    public delegate void ExportStartedEventHandler(object sender, ExportStartedEventHandlerArgs args);
    
    public class ExportStartedEventHandlerArgs
    {
        public string folderPath = string.Empty;
        public int mainModelNum = -1;
    }
}
