﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mobile.Code
{
    public class SqlLiteConnector : ISQLite
    {
        public SqlLiteConnector() { }
        public SQLiteConnection GetConnection()
        {
            var dbase = "DeckInspectorsLocalDB.db";

            var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(dbpath, dbase);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
