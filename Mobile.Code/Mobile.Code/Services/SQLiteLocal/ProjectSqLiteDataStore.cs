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
    public class ProjectSqLiteDataStore : IProjectDataStore
    {
        List<Project> items;
        private SQLiteConnection _connection;

        public ProjectSqLiteDataStore()
        {

            items = new List<Project>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<Project>();

        }
        public Task<Response> AddItemAsync(Project item)
        {

            items.Add(item);
           
            Response res = new Response(); //not being used now.
            try
            {
                var project = new Project
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Address = item.Address,
                    Description = item.Description,
                    ProjectType = item.ProjectType,
                    UserId = App.LogUser.Id.ToString(),
                    ImageDescription = item.ImageDescription,
                    ImageName = item.ImageName,
                    ImageUrl = item.ImageUrl,
                    Category = item.Category
               
                };

                res.TotalCount = _connection.Insert(project);
                SQLiteCommand Command = new SQLiteCommand(_connection);

                Command.CommandText = "select last_insert_rowid()";

                Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

                res.ID = LastRowID64.ToString();
                res.Data = project;
                res.Message = "Record Inserted Successfully";
                res.Status = ApiResult.Success;
                

            }
            catch (Exception ex)
            {
                res.Message = "Insertion failed." + ex.Message;
                res.Status = ApiResult.Fail;

            }
            return Task.FromResult(res);
            
        }

        public Task<Response> CreateInvasiveReport(Project item)
        {
            //need to implement.
            return Task.FromResult(new Response());
        }

        public Task<Response> DeleteItemAsync(Project item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<Project>(item.Id);

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

        public Task<Project> GetItemAsync(string id)
        {
            return Task.FromResult(_connection.Table<Project>().FirstOrDefault(t => t.Id == id));
        }

        public Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false)
        {
            return Task.FromResult(
              from t in _connection.Table<Project>()
              select t
                  );
        }

        public Task<Response> UpdateItemAsync(Project item)
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

            return Task.FromResult(res);
        }
    }
}
