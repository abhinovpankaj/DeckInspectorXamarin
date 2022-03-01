using Mobile.Code.Droid;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(SqlLiteConnector))]
namespace Mobile.Code.Droid
{
    public class SqlLiteConnector : ISQLite
    {
        public SqlLiteConnector() { }
        public SQLiteConnection GetConnection()
        {
            var dbase = "DeckInspectorsLocalDB.db";

            //var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var documentsPath = Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath; 
            documentsPath = documentsPath.Replace("Android/data/com.deckinspectors.mobile/files", "");
            var filePath = System.IO.Path.Combine(documentsPath, "DeckInspectors");
            Directory.CreateDirectory(filePath);
            var path = Path.Combine(filePath, dbase);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
