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
    public class VisualFormBuildingLocationSqLiteDataStore : IVisualFormBuildingLocationDataStore
    {
        List<BuildingLocation_Visual> items;
        private SQLiteConnection _connection;
        public VisualFormBuildingLocationSqLiteDataStore()
        {
            items = new List<BuildingLocation_Visual>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<BuildingLocation_Visual>();
        }
        public async Task<Response> AddItemAsync(BuildingLocation_Visual item, IEnumerable<string> ImageList)
        {
            Response res = new Response();
            try
            {
                var visualApt = new BuildingLocation_Visual
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    BuildingLocationId = item.BuildingLocationId,
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
                res.Message = "Record Inserted Successfully";
                res.Status = ApiResult.Success;
            }
            catch (Exception ex)
            {
                res.Message = "Insertion failed." + ex.Message;
                res.Status = ApiResult.Fail;

            }
            return await  Task.FromResult(res);
        }

        public async Task<Response> DeleteItemAsync(BuildingLocation_Visual item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<BuildingLocation_Visual>(item.Id);

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

        public async  Task<BuildingLocation_Visual> GetItemAsync(string id)
        {
            return await Task .FromResult(_connection.Table<BuildingLocation_Visual>().FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<BuildingLocation_Visual>() select t);
        }

        public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId)
        {
            return await Task.FromResult(_connection.Table<BuildingLocation_Visual>().Where(t => t.BuildingLocationId == buildingLocationId));
        }

        public async Task<Response> UpdateItemAsync(BuildingLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            Response res = new Response();

            try
            {
                var visualApt = new BuildingLocation_Visual
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    BuildingLocationId = item.BuildingLocationId,
                    AdditionalConsideration = item.AdditionalConsideration,
                    ExteriorElements = item.ExteriorElements,
                    WaterProofingElements = item.WaterProofingElements,
                    ConditionAssessment = item.ConditionAssessment,
                    VisualReview = item.VisualReview,
                    AnyVisualSign = item.AnyVisualSign,
                    FurtherInasive = item.FurtherInasive,
                    LifeExpectancyEEE = item.LifeExpectancyEEE,
                    LifeExpectancyAWE = item.LifeExpectancyAWE,
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
