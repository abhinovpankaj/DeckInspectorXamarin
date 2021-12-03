using Mobile.Code.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.Services
{
    public interface IVisualApartmentLocationPhotoDataStore
    {
        Task<bool> AddItemAsync(VisualApartmentLocationPhoto item , bool isOffline=false);
        Task<bool> UpdateItemAsync(VisualApartmentLocationPhoto item);
        Task<bool> DeleteItemAsync(VisualApartmentLocationPhoto item);
        Task<VisualApartmentLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectIDSqLite(string aptId, bool loadLocally);
        void Clear();
    }
    public class VisualApartmentLocationPhotoDataStore : IVisualApartmentLocationPhotoDataStore
    {
        List<VisualApartmentLocationPhoto> items;

        private SQLiteConnection _connection;
        public VisualApartmentLocationPhotoDataStore()
        {
            items = new List<VisualApartmentLocationPhoto>();
            //if (App.IsAppOffline)
            //{
                _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
                _connection.CreateTable<VisualApartmentLocationPhoto>();
            //}
        }
        public async Task<bool> AddItemAsync(VisualApartmentLocationPhoto item, bool isOffline)
        {
            items.Add(item);
            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualApartmentId, Status = "New", IsServerData = false });
            if (isOffline)
            {
                InsertPhoto(item);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualApartmentLocationPhoto item)
        {

            var oldItem = items.Where((VisualApartmentLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
           

            items.Add(item);
            if (App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").Any())
            {
                var t = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").Single();
                t.Image = item.ImageUrl;

            }
            var oldITRaktem = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTracking.Remove(oldITRaktem);


                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualApartmentId, Status = "Update", IsServerData = true });

            }
            if (App.IsAppOffline)
            {
                DeletePhoto(oldItem);
                InsertPhoto(item);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(VisualApartmentLocationPhoto item)
        {
            //var oldItem = items.Where((VisualApartmentLocationPhoto arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            //return await Task.FromResult(true);

            item.IsDelete = true;
            var oldItem = items.Where((VisualApartmentLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            var oldITRaktem = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTracking.Remove(oldITRaktem);

                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualApartmentId, Status = "Delete", IsDelete = true, IsServerData = true });



            }
            var oldDelete = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").SingleOrDefault();
            if (oldDelete != null)
            {
                App.VisualEditTracking.Remove(oldDelete);

            }
            if (App.IsAppOffline)
            {
                DeletePhoto(item);
                
            }
            return await Task.FromResult(true);
        }

        public async Task<VisualApartmentLocationPhoto> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {

            if (loadServer == false)
            {
                return await Task.FromResult(items.Where(c => c.VisualApartmentId == locationVisualID && c.IsDelete == false));
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60);
                    client.BaseAddress = new Uri(App.AzureBackendUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingApartmentImage/GetVisualBuildingApartmentImageByVisualApartmentId?VisualApartmentId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                        items = JsonConvert.DeserializeObject<List<VisualApartmentLocationPhoto>>(result.Data.ToString());
                        items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                        foreach (var item in items)
                        {
                            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualApartmentId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                            App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                        }
                        response.EnsureSuccessStatusCode();

                        return await Task.FromResult(items);


                    }
                }
            }
        }

        public async Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectIDSqLite(string aptId, bool loadLocally)
        {
            if (loadLocally)
            {
                return await Task.FromResult(items.Where(c => c.VisualApartmentId == aptId && c.IsDelete == false));
            }
            else
            {
                items = _connection.Table<VisualApartmentLocationPhoto>().Where(t => t.VisualApartmentId == aptId).ToList();

                //items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                foreach (var item in items)
                {
                    App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualApartmentId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                    App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                }
            }

            return await Task.FromResult(items);
        }
        public void Clear()
        {
            items.Clear();
        }


        //db part
        private void InsertPhoto(VisualApartmentLocationPhoto item)
        {

            Response res = new Response();
            
            res.TotalCount = _connection.Insert(item);
            SQLiteCommand Command = new SQLiteCommand(_connection);

            Command.CommandText = "select last_insert_rowid()";

            Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

            res.ID = LastRowID64.ToString();
            res.Data = item;
        }

        public void DeletePhoto(VisualApartmentLocationPhoto item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<VisualApartmentLocationPhoto>(item.Id);

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }

           
        }

        public void UpdatePhoto(VisualApartmentLocationPhoto item)
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
            
        }
    }
}