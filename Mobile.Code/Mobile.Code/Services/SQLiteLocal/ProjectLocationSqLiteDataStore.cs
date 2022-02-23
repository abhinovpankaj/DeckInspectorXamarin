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
    class ProjectLocationSqLiteDataStore : IProjectLocation
    {
        List<ProjectLocation> items;
        private SQLiteConnection _connection;

        public ProjectLocationSqLiteDataStore()
        {

            items = new List<ProjectLocation>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<ProjectLocation>();
        }
        public async Task<Response> AddItemAsync(ProjectLocation item)
        {
            Response res = new Response(); //not being used now.
            try
            {
                var projectLocation = new ProjectLocation
                {
                    Id = item.Id??Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Description = item.Description,
                    ProjectId = item.ProjectId,
                    UserId = App.LogUser.Id.ToString(),
                    ImageDescription = item.ImageDescription,
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    OnlineId= item.OnlineId
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

        public async Task<bool> UpdateItemAsync(ProjectLocation item)
        {
            Response res = new Response();
            try
            {
                var reult= _connection.Update(item);
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

        public async Task<Response> DeleteItemAsync(ProjectLocation item)
        {
            Response res = new Response();
            try
            {
                
                _connection.Delete<ProjectLocation>(item.Id);
                foreach (var location in _connection.Table<ProjectLocation_Visual>().Where(x => x.ProjectLocationId == item.Id))
                {
                    VisualFormProjectLocationSqLiteDataStore dq = new VisualFormProjectLocationSqLiteDataStore();
                    await dq.DeleteItemAsync(location);
                   // _connection.Delete<ProjectLocation_Visual>(location.Id);
                    
                }

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception ex)
            {
                res.Message = "Deletion Failed" + ex.Message;
                res.Status = ApiResult.Fail;
            }

            return await Task.FromResult(res);
        }

        public async Task<ProjectLocation> GetItemAsync(string id)
        {
            return await Task.FromResult(_connection.Table<ProjectLocation>().FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<ProjectLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<ProjectLocation>() select t );
        }


        public async Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectID(string projectId)
        {
            return await Task.FromResult(_connection.Table<ProjectLocation>().Where(t => t.ProjectId == projectId));
        }
    }
}
