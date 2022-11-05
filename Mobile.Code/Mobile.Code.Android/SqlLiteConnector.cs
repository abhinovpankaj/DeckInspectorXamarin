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

        private SQLiteConnection _connection;
        public SQLiteConnection GetConnection()
        {
            try
            {
                if (_connection == null)
                {
                    var dbase = "DeckInspectorsLocalDB.db";

                    //var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                    var documentsPath = Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath;
                    documentsPath = documentsPath.Replace("Android/data/com.deckinspectors.mobile/files", "");
                    var filePath = System.IO.Path.Combine(documentsPath, "DeckInspectors");
                    Directory.CreateDirectory(filePath);
                    var path = Path.Combine(filePath, dbase);
                    //if (!File.Exists(path))
                    //{
                    //    File.Copy("DeckInspectorsLocalDB.db", path);
                    //}
                    _connection = new SQLiteConnection(path);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            
            return _connection;
        }
    }
}
