using Mobile.Code.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.Services.SQLiteLocal
{
    public class BuildingCommonLocationImagesSqLiteDataStore : IBuildingCommonLocationImages
    {
        List<BuildingCommonLocationImages> items;
        private SQLiteConnection _connection;
        
        public BuildingCommonLocationImagesSqLiteDataStore()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<BuildingCommonLocationImages>();
            items = new List<BuildingCommonLocationImages>(); 
        }
        public Task<bool> AddItemAsync(BuildingCommonLocationImages item)
        {
            items.Add(item);
            bool status = false;
            Response res = new Response(); //not being used now.
            try
            {
                var buildingApartment = new BuildingCommonLocationImages
                {
                    BuildingLocationId = item.BuildingLocationId,
                    
                    UserId = App.LogUser.Id.ToString(),
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    ImageDescription = item.ImageDescription
                };

                res.TotalCount = _connection.Insert(buildingApartment);
                SQLiteCommand Command = new SQLiteCommand(_connection);

                Command.CommandText = "select last_insert_rowid()";

                Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

                res.ID = LastRowID64.ToString();

                res.Message = "Record Inserted Successfully";
                res.Status = ApiResult.Success;
                status = true;

            }
            catch (Exception ex)
            {
                res.Message = "Insertion failed." + ex.Message;
                res.Status = ApiResult.Fail;

            }
            return Task.FromResult(status);
        }

        public Task<Response> DeleteItemAsync(BuildingCommonLocationImages item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<BuildingCommonLocationImages>(item.Id);

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }

            return Task.FromResult(res);
        }

        public Task<BuildingCommonLocationImages> GetItemAsync(string id)
        {
            return Task.FromResult(_connection.Table<BuildingCommonLocationImages>().FirstOrDefault(t => t.Id == id));
        }

        public Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false)
        {
            return Task.FromResult(
               from t in _connection.Table<BuildingCommonLocationImages>()
               select t
                   );
        }

        public Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            items = (from t in _connection.Table<BuildingCommonLocationImages>().Where(x => x.BuildingLocationId == BuildingId)
                     select t).ToList();
            return Task.FromResult(items.AsEnumerable());
        }

        public Task<bool> UpdateItemAsync(BuildingCommonLocationImages item)
        {
            bool result;
            try
            {
                _connection.Update(item);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return Task.FromResult(result);
        }
    }
}
