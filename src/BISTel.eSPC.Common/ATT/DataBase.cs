using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Common.ATT
{
    public class DataBase : BISTel.PeakPerformance.Client.SQLHandler.SQLHandler
    {
        protected string _myName = null;
        protected StringBuilder _sbQuery = null;

        public DataBase() : base(Definition.DB_NAME) { }
        public DataBase(string dbName) : base(dbName) { }
    }
}
