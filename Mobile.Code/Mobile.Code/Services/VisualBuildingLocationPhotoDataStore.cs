using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mobile.Code.Models;
using Newtonsoft.Json;

namespace Mobile.Code.Services
{
    public interface IVisualBuildingLocationPhotoDataStore
    {
        void Clear();
        Task<bool> AddItemAsync(VisualBuildingLocationPhoto item);
        Task<bool> UpdateItemAsync(VisualBuildingLocationPhoto item);
        Task<bool> DeleteItemAsync(VisualBuildingLocationPhoto item);
        Task<VisualBuildingLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualBuildingLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);

    }
    public class VisualBuildingLocationPhotoDataStore : IVisualBuildingLocationPhotoDataStore
    {
         List<VisualBuildingLocationPhoto> items;

        public VisualBuildingLocationPhotoDataStore()
        {
            items = new List<VisualBuildingLocationPhoto>();

        }
        public async Task<bool> AddItemAsync(VisualBuildingLocationPhoto item)
        {
            //items.Add(item);

            //return await Task.FromResult(true);
            items.Add(item);
            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "New", IsServerData = false });
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualBuildingLocationPhoto item)
        {
            //var oldItem = items.Where((VisualBuildingLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);

            //return await Task.FromResult(true);
            var oldItem = items.Where((VisualBuildingLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
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

                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "Update", IsServerData = true });

            }




            return await Task.FromResult(true);
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
            var oldITRaktem = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTracking.Remove(oldITRaktem);

                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualBuildingId, Status = "Delete", IsDelete = true, IsServerData = true });



            }
            var oldDelete = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").SingleOrDefault();
            if (oldDelete != null)
            {
                App.VisualEditTracking.Remove(oldDelete);

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
                    client.BaseAddress = new Uri(App.AzureBackendUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingLocationImage/GetVisualBuildingLocationImageByVisualBuildingId?VisualBuildingId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                        items = JsonConvert.DeserializeObject<List<VisualBuildingLocationPhoto>>(result.Data.ToString());
                        items = items.Where(c => c.ImageDescription != "TRUE").ToList();
                        foreach (var item in items)
                        {
                            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualBuildingId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                            App.ImageFormString = JsonConvert.SerializeObject(App.VisualEditTracking);
                        }
                        response.EnsureSuccessStatusCode();

                        return await Task.FromResult(items);


                    }
                }
            }
        }

        public void Clear()
        {
            items.Clear();
        }


    }
}