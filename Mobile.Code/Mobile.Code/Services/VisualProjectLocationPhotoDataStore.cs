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
        Task<bool> AddItemAsync(VisualProjectLocationPhoto item, bool isOffline = false);

        void Clear();
        Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<VisualProjectLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer, bool isSync=false);

        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally);
        Task<IEnumerable<MultiImage>>  GetMultiImagesAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally);
    }
    public class VisualProjectLocationPhotoDataStore : IVisualProjectLocationPhotoDataStore
    {
        List<VisualProjectLocationPhoto> items;
        List<MultiImage> multiImages;
        private SQLiteConnection _connection;
        public VisualProjectLocationPhotoDataStore()
        {
            items = new List<VisualProjectLocationPhoto>();
            multiImages = new List<MultiImage>();
            //if (App.IsAppOffline)
            //{
                createConnection();
           // }
        }
        private void createConnection()
        {
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<VisualProjectLocationPhoto>();
            _connection.CreateTable<MultiImage>();
        }
        public async Task<bool> AddItemAsync(VisualProjectLocationPhoto item, bool isOffline = false)
        {
           
            items.Add(item);
            var multiImage = new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "New", IsServerData = false };
            App.VisualEditTracking.Add(multiImage);
            
            if (isOffline)
            {
                InsertPhoto(item);
                InsertMultiPhoto(multiImage);
            }
            return await Task.FromResult(true);
        }

        private void InsertMultiPhoto(MultiImage multiImage)
        {
            Response res = new Response();
            var multi=_connection.Table<MultiImage>().FirstOrDefault(t => t.Id == multiImage.Id);
            if (multi==null)
            {
                res.TotalCount = _connection.Insert(multiImage);
            }
            else
                res.TotalCount = _connection.Update(multiImage);


            res.ID = multiImage.Id;
            res.Data = multiImage;
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
                var updateMulti = new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Update", IsServerData = true };
                App.VisualEditTracking.Add(updateMulti);
                if (App.IsAppOffline)
                {
                    DeleteMultiPhoto(oldITRaktem);
                    InsertMultiPhoto(updateMulti);
                }
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
                var delMulti = new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Delete", IsDelete = true, IsServerData = true };
                App.VisualEditTracking.Add(delMulti);
                if (App.IsAppOffline)
                {
                    DeleteMultiPhoto(oldITRaktem);
                    InsertMultiPhoto(delMulti);
                }


            }
            var oldDelete = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").SingleOrDefault();
            if (oldDelete != null)
            {
                App.VisualEditTracking.Remove(oldDelete);
                if (App.IsAppOffline)
                {
                    DeleteMultiPhoto(oldDelete);
                }

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


        public async Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer, bool isSync=false)
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
                            var onlineImage = new MultiImage() { Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true };
                            if (isSync)
                            {
                                InsertMultiPhoto(onlineImage);
                            }
                            App.VisualEditTracking.Add(onlineImage);
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
                var allitems = _connection.Table<VisualProjectLocationPhoto>().ToList();

                items = _connection.Table<VisualProjectLocationPhoto>().Where(t => t.VisualLocationId == locationVisualID).ToList();
                
                //items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                
                foreach (var item in items)
                {
                    var multiimg = new MultiImage() { Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true };
                    App.VisualEditTracking.Add(multiimg);
                    //InsertMultiPhoto(multiimg);
                    App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                }
            }
           
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<MultiImage>> GetMultiImagesAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally)
        {
            if (loadLocally)
            {
                return await Task.FromResult(multiImages.Where(c => c.ParentId == locationVisualID && c.IsDelete == false));
            }
            else
            {
                multiImages = _connection.Table<MultiImage>().Where(t => t.ParentId == locationVisualID).ToList();
                
            }

            return await Task.FromResult(multiImages);
        }
        public void Clear()
        {
            items.Clear();
            multiImages.Clear();
            //_connection.DeleteAll<MultiImage>();
        }

        private void InsertPhoto(VisualProjectLocationPhoto item)
        {

            Response res = new Response();
            var pic = _connection.Table<VisualProjectLocationPhoto>().FirstOrDefault(t => t.Id == item.Id);
            if(pic==null)
                res.TotalCount = _connection.Insert(item);
            else
                res.TotalCount = _connection.Update(item);
            res.ID = item.Id;
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
        private void DeleteMultiPhoto(MultiImage oldITRaktem)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<MultiImage>(oldITRaktem.Id);

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
                res.Status = ApiResult.Fail;
            }
        }

    }
}