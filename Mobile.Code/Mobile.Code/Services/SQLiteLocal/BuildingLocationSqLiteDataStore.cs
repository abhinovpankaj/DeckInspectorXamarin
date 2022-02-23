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
    public class BuildingLocationSqLiteDataStore : IBuildingLocation
    {
        List<BuildingLocation> items;
        private SQLiteConnection _connection;
        public BuildingLocationSqLiteDataStore()
        {

            items = new List<BuildingLocation>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<BuildingLocation>();

        }
        public async Task<Response> AddItemAsync(BuildingLocation item)
        {
            Response res = new Response(); //not being used now.
            try
            {
                var buildingLocation = new BuildingLocation
                {
                    Id = item.Id??Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Description = item.Description,
                    BuildingId = item.BuildingId,
                    UserId = App.LogUser.Id.ToString(),
                    ImageDescription = item.ImageDescription,
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    OnlineId =item.OnlineId
                };

                res.TotalCount = _connection.Insert(buildingLocation);
               

                res.ID = buildingLocation.Id;
                res.Data = buildingLocation;
                res.Message = "Record Inserted Successfully";
                res.Status = ApiResult.Success;


            }
            catch (Exception ex)
            {
                res.Message = "Insertion failed." + ex.Message;
                res.Status = ApiResult.Fail;

            }
            return await Task.FromResult(res);


        }


        public async Task<BuildingLocation> GetItemAsync(string id)
        {
            return await Task.FromResult(_connection.Table<BuildingLocation>().FirstOrDefault(t => t.Id == id));
        }

        public async Task<bool> UpdateItemAsync(BuildingLocation item)
        {
            Response res = new Response();
            try
            {
                _connection.Update(item);
                res.Message = "Record Updated Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Updation Failed";
                res.Status = ApiResult.Fail;
            }

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingLocation item)
        {
            Response res = new Response();
            try
            {                             
                _connection.Delete<BuildingLocation>(item.Id);
                foreach (var location in _connection.Table<BuildingLocation_Visual>().Where(x => x.BuildingLocationId == item.Id))
                {
                    VisualFormBuildingLocationSqLiteDataStore dq = new VisualFormBuildingLocationSqLiteDataStore();
                    await dq.DeleteItemAsync(location);
                    //_connection.Delete<BuildingLocation_Visual>(location.Id);
                                      
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



        public async Task<IEnumerable<BuildingLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<BuildingLocation>() select t);
        }


        public async Task<IEnumerable<BuildingLocation>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            return await Task.FromResult(_connection.Table<BuildingLocation>().Where(t => t.BuildingId == BuildingId));
        }
    }
}
