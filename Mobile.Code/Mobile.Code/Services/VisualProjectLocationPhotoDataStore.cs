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
    public interface IVisualProjectLocationPhotoDataStore
    {
        Task<bool> AddItemAsync(VisualProjectLocationPhoto item);

        void Clear();
        Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<VisualProjectLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);

        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally);

    }
    public class VisualProjectLocationPhotoDataStore : IVisualProjectLocationPhotoDataStore
    {
        List<VisualProjectLocationPhoto> items;
        private SQLiteConnection _connection;
        public VisualProjectLocationPhotoDataStore()
        {
            items = new List<VisualProjectLocationPhoto>();
            //if (App.IsAppOffline)
            //{
                createConnection();
           // }
        }
        private void createConnection()
        {
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<VisualProjectLocationPhoto>();
        }
        public async Task<bool> AddItemAsync(VisualProjectLocationPhoto item)
        {
            //if(App.IsInvasive==true)
            //{
            //    item.InvasiveImage = true;
            //}
            items.Add(item);
            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "New", IsServerData = false });
            if (App.IsAppOffline)
            {
                InsertPhoto(item);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {



            var oldItem = items.Where((VisualProjectLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
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

                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Update", IsServerData = true });

            }

            if (App.IsAppOffline)
            {
                DeletePhoto(oldItem);
                InsertPhoto(item);
            }



            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            item.IsDelete = true;
            var oldItem = items.Where((VisualProjectLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            var oldITRaktem = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTracking.Remove(oldITRaktem);

                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Delete", IsDelete = true, IsServerData = true });



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

        public async Task<VisualProjectLocationPhoto> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {

            if (loadServer == false)
            {
                return await Task.FromResult(items.Where(c => c.VisualLocationId == locationVisualID && c.IsDelete == false));

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
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualProjectLocationImage/GetVisualProjectLocationImageByVisualLocationId?VisualLocationId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                        items = JsonConvert.DeserializeObject<List<VisualProjectLocationPhoto>>(result.Data.ToString());
                        items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                        foreach (var item in items)
                        {
                            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                            App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                        }

                        response.EnsureSuccessStatusCode();

                        return await Task.FromResult(items);


                    }
                }
            }


        }
        public async Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByLoacationIDSqLite(string locationVisualID,bool loadLocally)
        {
            if (loadLocally)
            {
                return await Task.FromResult(items.Where(c => c.VisualLocationId == locationVisualID && c.IsDelete == false));
            }
            else
            {
                items = _connection.Table<VisualProjectLocationPhoto>().Where(t => t.VisualLocationId == locationVisualID).ToList();
                //items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                foreach (var item in items)
                {
                    App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                    App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                }
            }
           
            return await Task.FromResult(items);
        }
        public void Clear()
        {
            items.Clear();
        }

        private void InsertPhoto(VisualProjectLocationPhoto item)
        {

            Response res = new Response();

            res.TotalCount = _connection.Insert(item);
            SQLiteCommand Command = new SQLiteCommand(_connection);

            Command.CommandText = "select last_insert_rowid()";

            Int64 LastRowID64 = Command.ExecuteScalar<Int64>();

            res.ID = LastRowID64.ToString();
            res.Data = item;
        }

        public void DeletePhoto(VisualProjectLocationPhoto item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<VisualProjectLocationPhoto>(item.Id);

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }


        }

        public void UpdatePhoto(VisualProjectLocationPhoto item)
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