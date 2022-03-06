﻿using Mobile.Code.iOS;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(SqlLiteConnector))]
namespace Mobile.Code.iOS
{
    public class SqlLiteConnector : ISQLite
    {
        public SqlLiteConnector() { }
        public SQLiteConnection GetConnection()
        {
            var dbase = "DeckInspectorsLocalDB.db";
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = Path.Combine(documents, "..", "Library");
            //var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(library, dbase);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
