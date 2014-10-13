using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using Microsoft.CSharp;
using System.IO;
using System.CodeDom.Compiler;

namespace BISTel.eSPC.Common.ATT
{
    public class DynamicCodeGenerator : BISTel.eSPC.Common.DynamicCodeGenerator
    {
        public DynamicCodeGenerator(string nameSpace, string className) : base(nameSpace, className) { }

        public DynamicCodeGenerator(string nameSpace, string className, string baseType) : base(nameSpace, className, baseType) { }
    }
}