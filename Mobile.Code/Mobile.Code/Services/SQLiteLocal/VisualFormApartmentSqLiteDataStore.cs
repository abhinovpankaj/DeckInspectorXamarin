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
    public class VisualFormApartmentSqLiteDataStore : IVisualFormApartmentDataStore
    {
        List<Apartment_Visual> items;
        private SQLiteConnection _connection;
        public VisualFormApartmentSqLiteDataStore()
        {
            items = new List<Apartment_Visual>();
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<Apartment_Visual>();
        }
        public async Task<Response> AddItemAsync(Apartment_Visual item, IEnumerable<string> ImageList)
        {

            Response res = new Response();
            try
            {
                var visualApt = new Apartment_Visual
                {
                    Id = item.Id??Guid.NewGuid().ToString(),
                    Name = item.Name,
                    BuildingApartmentId = item.BuildingApartmentId,
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
                    OnlineId = item.OnlineId
                };
                res.TotalCount = _connection.Insert(visualApt);
                
                res.ID = visualApt.Id;
                res.Data = visualApt;
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

        public async Task<Response> UpdateItemAsync(Apartment_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            Response res = new Response();

            try
            {
                res.TotalCount = _connection.Update(item);
                SQLiteCommand Command = new SQLiteCommand(_connection);

                res.Data = item;
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

        public async Task<Response> DeleteItemAsync(Apartment_Visual item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<Apartment_Visual>(item.Id);

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


        public async Task<Apartment_Visual> GetItemAsync(string id)
        {
            return await Task.FromResult(_connection.Table<Apartment_Visual>().FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<Apartment_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(from t in _connection.Table<Apartment_Visual>() select t);
        }


        public async Task<IEnumerable<Apartment_Visual>> GetItemsAsyncByApartmentId(string ApartmentLocationId)
        {
            return await Task.FromResult(_connection.Table<Apartment_Visual>().Where(t => t.BuildingApartmentId == ApartmentLocationId));
        }
    }
}
