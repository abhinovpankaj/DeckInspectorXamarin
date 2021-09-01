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
    public interface IVisualApartmentLocationPhotoDataStore
    {
        Task<bool> AddItemAsync(VisualApartmentLocationPhoto item);
        Task<bool> UpdateItemAsync(VisualApartmentLocationPhoto item);
        Task<bool> DeleteItemAsync(VisualApartmentLocationPhoto item);
        Task<VisualApartmentLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualApartmentLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);
        void Clear();
    }
    public class VisualApartmentLocationPhotoDataStore : IVisualApartmentLocationPhotoDataStore
    {
         List<VisualApartmentLocationPhoto> items;

        public VisualApartmentLocationPhotoDataStore()
        {
            items = new List<VisualApartmentLocationPhoto>();

        }
        public async Task<bool> AddItemAsync(VisualApartmentLocationPhoto item)
        {
            items.Add(item);
            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualApartmentId, Status = "New", IsServerData = false });
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualApartmentLocationPhoto item)
        {
            //var oldItem = items.Where((VisualApartmentLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);

            //return await Task.FromResult(true);
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
                    client.BaseAddress = new Uri(App.AzureBackendUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingApartmentImage/GetVisualBuildingApartmentImageByVisualApartmentId?VisualApartmentId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                        items = JsonConvert.DeserializeObject<List<VisualApartmentLocationPhoto>>(result.Data.ToString());
                        items = items.Where(c => c.ImageDescription != "TRUE").ToList();
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

        public void Clear()
        {
            items.Clear();
        }


    }
}