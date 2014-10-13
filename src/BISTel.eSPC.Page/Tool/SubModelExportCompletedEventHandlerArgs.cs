using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.Tool
{
    public delegate void SubModelExportCompletedEventHandler(object sender, SubModelExportCompletedEventHandlerArgs args);

    public class SubModelExportCompletedEventHandlerArgs
    {
        public string SubModelRawID = string.Empty;
    }
}
