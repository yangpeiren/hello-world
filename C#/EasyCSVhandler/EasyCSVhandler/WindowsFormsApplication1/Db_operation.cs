using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace EasyCSVHandler
{

    class Db_operation
    {
        private static string constring = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + getpath();
        public static OleDbConnection con = new OleDbConnection(constring);
        public static string getpath()
        {
            return XMLHandle.readPath();
        }
    }
}
