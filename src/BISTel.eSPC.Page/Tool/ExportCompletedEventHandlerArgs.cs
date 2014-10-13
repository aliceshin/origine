using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.Tool
{
    public delegate void ExportCompletedEventHandler(object sender, ExportCompletedEventHandlerArgs args);
    
    public class ExportCompletedEventHandlerArgs
    {
        public int exportedModelNum = 0;
    }
}
