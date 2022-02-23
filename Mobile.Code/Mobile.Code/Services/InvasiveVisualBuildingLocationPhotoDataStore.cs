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
    public interface IInvasiveVisualBuildingLocationPhotoDataStore
    {
        void Clear();
        Task<bool> AddItemAsync(VisualBuildingLocationPhoto item, bool isOffline = false);
        Task<bool> UpdateItemAsync(VisualBuildingLocationPhoto item);
        Task<bool> DeleteItemAsync(VisualBuildingLocationPhoto item);
        Task<VisualBuildingLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);

        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally);
    }
    public class InvasiveVisualBuildingLocationPhotoDataStore : IInvasiveVisualBuildingLocationPhotoDataStore
    {
        List<VisualBuildingLocationPhoto> items;
        List<MultiImage> multiImages;
        private SQLiteConnection _connection;
        public InvasiveVisualBuildingLocationPhotoDataStore()
        {
            items = new List<VisualBuildingLocationPhoto>();
            
            multiImages = new List<MultiImage>();

            createConnection();

        }
        private void createConnection()
        {
            _connection = DependencyService.Get<SqlLiteConnector>().GetConnection();
            _connection.CreateTable<VisualBuildingLocationPhoto>();
            _connection.CreateTable<MultiImage>();
        }
        public async Task<bool> AddItemAsync(VisualBuildingLocationPhoto item, bool isOffline = false)
        {
            if (App.IsInvasive == true)
            {
                item.InvasiveImage = true;
            }
            var multiImage = new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "New", IsServerData = false, ImageType = item.ImageDescription };
            items.Add(item);
            App.VisualEditTrackingForInvasive.Add(multiImage);
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
            var multi = _connection.Table<MultiImage>().FirstOrDefault(t => t.Id == multiImage.Id);
            if (multi == null)
            {
                res.TotalCount = _connection.Insert(multiImage);
            }
            else
                res.TotalCount = _connection.Update(multiImage);


            res.ID = multiImage.Id;
            res.Data = multiImage;
        }
        private void InsertPhoto(VisualBuildingLocationPhoto item)
        {
            Response res = new Response();
            var pic = _connection.Table<VisualBuildingLocationPhoto>().FirstOrDefault(t => t.Id == item.Id);
            if (pic == null)
                res.TotalCount = _connection.Insert(item);
            else
                res.TotalCount = _connection.Update(item);
            res.ID = item.Id;
            res.Data = item;
        }

        public async Task<bool> UpdateItemAsync(VisualBuildingLocationPhoto item)
        {
            
            var oldItem = items.Where((VisualBuildingLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);

            var oldITRaktem = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                items.Add(item);
                App.VisualEditTrackingForInvasive.Remove(oldITRaktem);
                var updateMulti = new MultiImage() { ImageType = item.ImageDescription, Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "Update", IsServerData = true };
                App.VisualEditTrackingForInvasive.Add(updateMulti);
                if (App.IsAppOffline)
                {
                    DeleteMultiPhoto(oldITRaktem);
                    InsertMultiPhoto(updateMulti);
                }
            }
            else
            {
                items.Add(item);
                var oldDelete = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id).SingleOrDefault();
                if (oldDelete != null)
                {
                    oldDelete.Image = item.ImageUrl;
                    App.VisualEditTrackingForInvasive.Remove(oldDelete);

                    App.VisualEditTrackingForInvasive.Add(oldDelete);

                }
            }

            if (App.IsAppOffline)
            {
                DeletePhoto(oldItem);
                InsertPhoto(item);
            }

            return await Task.FromResult(true);
        }
        public void DeletePhoto(VisualBuildingLocationPhoto item)
        {
            Response res = new Response();
            try
            {
                _connection.Delete<VisualBuildingLocationPhoto>(item.Id);

                res.Message = "Record Deleted Successfully";
                res.Status = ApiResult.Success;

            }
            catch (Exception)
            {
                res.Message = "Deletion Failed";
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
        public async Task<bool> DeleteItemAsync(VisualBuildingLocationPhoto item)
        {
            //var oldItem = items.Where((VisualBuildingLocationPhoto arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            //return await Task.FromResult(true);
            item.IsDelete = true;
            var oldItem = items.Where((VisualBuildingLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            var oldITRaktem = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTrackingForInvasive.Remove(oldITRaktem);
                var delMulti = new MultiImage() { ImageType = item.ImageDescription, Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "Delete", IsDelete = true, IsServerData = true };
                App.VisualEditTrackingForInvasive.Add(delMulti);
                if (App.IsAppOffline)
                {
                    DeleteMultiPhoto(oldITRaktem);
                    InsertMultiPhoto(delMulti);
                }
            }
            var oldDelete = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").SingleOrDefault();
            if (oldDelete != null)
            {
                App.VisualEditTrackingForInvasive.Remove(oldDelete);
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

        public async Task<VisualBuildingLocationPhoto> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer)
        {
            if (loadServer == false)
            {
                return await Task.FromResult(items.Where(c => c.VisualBuildingId == locationVisualID && c.IsDelete == false));
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
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingLocationImage/GetVisualBuildingLocationImageByVisualBuildingId?VisualBuildingId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                        items = JsonConvert.DeserializeObject<List<VisualBuildingLocationPhoto>>(result.Data.ToString());

                        items = items.Where(c => c.ImageDescription == "TRUE" || c.ImageDescription == "CONCLUSIVE").ToList();
                        foreach (var item in items)
                        {
                            App.VisualEditTrackingForInvasive.Add(new MultiImage() { ImageType = item.ImageDescription, Id = item.Id, ParentId = item.VisualBuildingId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                        }
                        response.EnsureSuccessStatusCode();

                        return await Task.FromResult(items);


                    }
                }
            }
        }

        public async Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByLoacationIDSqLite(string locationVisualID, bool loadLocally)
        {
            if (loadLocally)
            {
                return await Task.FromResult(items.Where(c => c.VisualBuildingId == locationVisualID && c.IsDelete == false));
            }
            else
            {
               
                items = _connection.Table<VisualBuildingLocationPhoto>().Where(t => t.VisualBuildingId == locationVisualID).ToList();
                //items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription != "CONCLUSIVE").ToList();
                foreach (var item in items)
                {
                    var multiimg = new MultiImage() { Id = item.Id, ParentId = item.VisualBuildingId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true };
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
        }


    }
}