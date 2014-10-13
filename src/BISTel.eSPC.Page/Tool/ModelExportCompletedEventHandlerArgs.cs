using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Page.Tool
{
    public delegate void ModelExportCompletedEventHandler(object sender, ModelExportCompletedEventHandlerArgs args);

    public class ModelExportCompletedEventHandlerArgs
    {
        public string SPCModelName = string.Empty;
        public int exportedModelNum = 0;
        public string filePath = string.Empty;
    }
}
