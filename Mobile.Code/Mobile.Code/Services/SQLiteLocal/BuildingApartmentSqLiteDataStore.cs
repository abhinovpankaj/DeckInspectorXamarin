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
    public class BuildingApartmentSqLiteDataStore : IBuildingApartment
    {
        List<BuildingApartment> items;

        private SQLiteConnection _connection;

        public BuildingApartmentSqLiteDataStore()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<BuildingApartment>();
            items = new List<BuildingApartment>();
        }
        public Task<Response> AddItemAsync(BuildingApartment item)
        {
            Response res = new Response();
            try
            {
                var buildingApartment = new BuildingApartment
                {
                    Id =  item.Id??Guid.NewGuid().ToString(),
                    BuildingId = item.BuildingId,
                    Name = item.Name,
                    OnlineId = item.OnlineId,
                    Description = item.Description,

                    UserId = App.LogUser.Id.ToString(),
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    ImageDescription = item.ImageDescription
                };

                res.TotalCount = _connection.Insert(buildingApartment);
                //SQLiteCommand Command= new SQLiteCommand(_connection);
                
                //Command.CommandText = "select last_insert_rowid()";

                //Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

                res.ID = buildingApartment.Id;
                res.Data = buildingApartment;
                res.Message = "Record Inserted Successfully";
                res.Status = ApiResult.Success;
                
            }
            catch (Exception ex)
            {
                res.Message = "Insertion failed."+ ex.Message;
                res.Status = ApiResult.Fail;
            }
            return Task.FromResult(res);
           
        }

        public async Task<Response> DeleteItemAsync(BuildingApartment item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<BuildingApartment>(item.Id);
                
                foreach (var location in _connection.Table<Apartment_Visual>().Where(x => x.BuildingApartmentId == item.Id))
                {
                    VisualFormApartmentSqLiteDataStore dq = new VisualFormApartmentSqLiteDataStore();
                    await dq.DeleteItemAsync(location);
                    //_connection.Delete<Apartment_Visual>(location.Id);

                }
                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }

            return res;
        }

        public Task<BuildingApartment> GetItemAsync(string id)
        {
            return Task.FromResult(_connection.Table<BuildingApartment>().FirstOrDefault(t => t.Id == id));
        }

        public Task<IEnumerable<BuildingApartment>> GetItemsAsync(bool forceRefresh = false)
        {
            return Task.FromResult(
                from t in _connection.Table<BuildingApartment>()
                select t
                    );
        }

        public Task<IEnumerable<BuildingApartment>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            items = (from t in _connection.Table<BuildingApartment>().Where(x => x.BuildingId == BuildingId)
                    select t).ToList();
            return Task.FromResult(items.AsEnumerable());
        }

        public Task<bool> UpdateItemAsync(BuildingApartment item)
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
