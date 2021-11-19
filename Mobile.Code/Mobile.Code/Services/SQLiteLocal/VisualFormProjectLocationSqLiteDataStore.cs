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
        }
        public async Task<Response> AddItemAsync(ProjectLocation_Visual item, IEnumerable<string> ImageList = null)
        {
            Response res = new Response();
            try
            {
                var visualApt = new ProjectLocation_Visual
                {
                    Id = Guid.NewGuid().ToString(),
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
                    ImageDescription = item.ImageDescription
                };
                res.TotalCount = _connection.Insert(visualApt);
                SQLiteCommand Command = new SQLiteCommand(_connection);

                Command.CommandText = "select last_insert_rowid()";

                Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

                res.ID = LastRowID64.ToString();
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
            return await Task.FromResult(_connection.Table<ProjectLocation_Visual>().FirstOrDefault(t => t.Id == id));
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
                var visualApt = new ProjectLocation_Visual
                {
                    Id = Guid.NewGuid().ToString(),
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
                    LifeExpectancyLBC= item.LifeExpectancyLBC,
                    ImageDescription = item.ImageDescription,
                    ConclusiveComments = item.ConclusiveComments,
                    ConclusiveLifeExpEEE = item.ConclusiveLifeExpEEE,
                    ConclusiveLifeExpLBC = item.ConclusiveLifeExpLBC,
                    ConclusiveLifeExpAWE = item.ConclusiveLifeExpAWE,
                    ConclusiveAdditionalConcerns = item.ConclusiveAdditionalConcerns,
                    IsPostInvasiveRepairsRequired = item.IsPostInvasiveRepairsRequired,
                    IsInvasiveRepairApproved = item.IsInvasiveRepairApproved,
                    IsInvasiveRepairComplete = item.IsInvasiveRepairComplete
                };
                res.TotalCount = _connection.Update(visualApt);
                SQLiteCommand Command = new SQLiteCommand(_connection);

                res.Data = visualApt;
                res.Message = "Record Updated Successfully";
                res.Status = ApiResult.Success;
            }
            catch (Exception)
            {
                res.Message = "Updation Failed";
                res.Status = ApiResult.Fail;
            }


            if (App.IsInvasive == true)
            {


                //TODO
            }
            else
            {



            }

            return await Task.FromResult(res);
        }
    }
}
