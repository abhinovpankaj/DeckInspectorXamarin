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
    class VisualFormProjectLocationSqLiteDataStore : IVisualFormProjectLocationDataStore
    {
        List<ProjectLocation_Visual> items;
        private SQLiteConnection _connection;
        public VisualFormProjectLocationSqLiteDataStore()
        {
            items = new List<ProjectLocation_Visual>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<ProjectLocation_Visual>();
            _connection.CreateTable<MultiImage>();
        }
        public async Task<Response> AddItemAsync(ProjectLocation_Visual item, IEnumerable<string> ImageList = null)
        {
            Response res = new Response();
            try
            {
                var visualApt = new ProjectLocation_Visual
                {
                    Id = item.Id??Guid.NewGuid().ToString(),
                    Name = item.Name,
                    ProjectLocationId = item.ProjectLocationId,
                    AdditionalConsideration = item.AdditionalConsideration,
                    ExteriorElements = item.ExteriorElements,
                    WaterProofingElements = item.WaterProofingElements,
                    ConditionAssessment = item.ConditionAssessment,
                    VisualReview = item.VisualReview,
                    AnyVisualSign = item.AnyVisualSign,
                    FurtherInasive = item.FurtherInasive,
                    LifeExpectancyEEE = item.LifeExpectancyEEE,
                    LifeExpectancyAWE = item.LifeExpectancyAWE,
                    LifeExpectancyLBC = item.LifeExpectancyLBC,
                    ImageDescription = item.ImageDescription,
                    OnlineId=item.OnlineId
                };
                res.TotalCount = _connection.Insert(visualApt);


                res.ID = visualApt.Id;
                res.Data = visualApt;
                //add images
                
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

        public async Task<Response> DeleteItemAsync(ProjectLocation_Visual item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<ProjectLocation_Visual>(item.Id);

                //delete images
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                documentsPath = System.IO.Path.Combine(documentsPath, "DeckInspectors", "offline_" + item.Id);
                if (System.IO.File.Exists(documentsPath))
                    System.IO.File.Delete(documentsPath);
                
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

        public async Task<ProjectLocation_Visual> GetItemAsync(string id)
        {
            var projVisLoc = await Task.Run(() => _connection.Table<ProjectLocation_Visual>().FirstOrDefault(t => t.Id == id));
            
            return projVisLoc;
        }

        public async Task<IEnumerable<ProjectLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<ProjectLocation_Visual>() select t);
        }

        public async Task<IEnumerable<ProjectLocation_Visual>> GetItemsAsyncByProjectLocationId(string projectLocationId)
        {
            return await Task.FromResult(_connection.Table<ProjectLocation_Visual>().Where(t => t.ProjectLocationId == projectLocationId));
        }

        public async Task<Response> UpdateItemAsync(ProjectLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            Response res = new Response();

            try
            {
                res.TotalCount = _connection.Update(item);
                
                res.Data = item;
                res.Message = "Record Updated Successfully";
                res.Status = ApiResult.Success;
                if (finelList!=null)
                {
                    foreach (var img in finelList)
                    {
                        var updateRes = _connection.Update(img);
                    }
                }
                
            }
            catch (Exception)
            {
                res.Message = "Updation Failed";
                res.Status = ApiResult.Fail;
            }



            return await Task.FromResult(res);
        }
    }
}
