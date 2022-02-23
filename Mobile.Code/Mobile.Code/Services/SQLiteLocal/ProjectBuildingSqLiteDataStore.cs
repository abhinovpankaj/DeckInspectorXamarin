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
    public class ProjectBuildingSqLiteDataStore: IProjectBuilding
    {
        List<ProjectBuilding> items;
        private SQLiteConnection _connection;
        public ProjectBuildingSqLiteDataStore()
        {

            items = new List<ProjectBuilding>();
            
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<ProjectBuilding>();

        }

        public async Task<bool> UpdateItemAsync(ProjectBuilding item)
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

        public async Task<Response> DeleteItemAsync(ProjectBuilding item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<ProjectBuilding>(item.Id);
                foreach (var buildingLoc in _connection.Table<BuildingLocation>().Where(x => x.BuildingId == item.Id))
                {
                    BuildingLocationSqLiteDataStore dq = new BuildingLocationSqLiteDataStore();
                    await dq.DeleteItemAsync(buildingLoc);
                    //_connection.Delete<ProjectBuilding>(buildingLoc.Id);
                }
               
                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }

            return await Task.FromResult(res);
        }
        public async Task<Response> AddItemAsync(ProjectBuilding item)
        {

            Response res = new Response(); //not being used now.
            try
            {
                var projectLocation = new ProjectBuilding
                {
                    Id = item.Id??Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Description = item.Description,
                    ProjectId = item.ProjectId,
                    UserId = App.LogUser.Id.ToString(),
                    ImageDescription = item.ImageDescription,
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    OnlineId =item.OnlineId
                };

                res.TotalCount = _connection.Insert(projectLocation);
               

                res.ID = projectLocation.Id;
                res.Data = projectLocation;
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


        public async Task<ProjectBuilding> GetItemAsync(string id)
        {
            return await Task.FromResult(_connection.Table<ProjectBuilding>().FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<ProjectBuilding>() select t);
        }


        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectID(string projectId)
        {

            return await Task.FromResult(_connection.Table<ProjectBuilding>().Where(t => t.ProjectId == projectId));
        }
    }
}
