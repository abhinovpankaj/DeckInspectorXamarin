using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mobile.Code.Models;
using Newtonsoft.Json;

namespace Mobile.Code.Services
{
    public interface IInvasiveVisualProjectLocationPhotoDataStore
    {
        Task<bool> AddItemAsync(VisualProjectLocationPhoto item);

        void Clear();
        Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<VisualProjectLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);

    }
    public class InvasiveVisualProjectLocationPhotoDataStore : IInvasiveVisualProjectLocationPhotoDataStore
    {
        List<VisualProjectLocationPhoto> items;

        public InvasiveVisualProjectLocationPhotoDataStore()
        {
            items = new List<VisualProjectLocationPhoto>();

        }
        public async Task<bool> AddItemAsync(VisualProjectLocationPhoto item)
        {
            if(App.IsInvasive==true)
            {
                item.InvasiveImage = true;
            }
            items.Add(item);
            App.VisualEditTrackingForInvasive.Add(new MultiImage() {  Id = item.Id,Image=item.ImageUrl, ParentId = item.VisualLocationId, Status = "New" , IsServerData = false, ImageType=item.ImageDescription});
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            

          
            var oldItem = items.Where((VisualProjectLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
           
            var oldITRaktem = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id&&c.IsServerData==true).SingleOrDefault();
            if(oldITRaktem!=null)
            {
                items.Add(item);
                App.VisualEditTrackingForInvasive.Remove(oldITRaktem);
                
                App.VisualEditTrackingForInvasive.Add(new MultiImage() { ImageType = item.ImageDescription, Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Update" , IsServerData = true });
              
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


            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            item.IsDelete = true;
            var oldItem = items.Where((VisualProjectLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            var oldITRaktem = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id && c.IsServerData == true).SingleOrDefault();
            if (oldITRaktem != null)
            {
                App.VisualEditTrackingForInvasive.Remove(oldITRaktem);
               
                App.VisualEditTrackingForInvasive.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Delete", IsDelete = true, IsServerData = true });
               
               
                
            }
            var oldDelete = App.VisualEditTrackingForInvasive.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").SingleOrDefault();
            if (oldDelete!=null)
            {
                App.VisualEditTrackingForInvasive.Remove(oldDelete);
              
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
                   
                    client.BaseAddress = new Uri(App.AzureBackendUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync($"api/VisualProjectLocationImage/GetVisualProjectLocationImageByVisualLocationId?VisualLocationId=" + locationVisualID))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                        
                        items = JsonConvert.DeserializeObject<List<VisualProjectLocationPhoto>>(result.Data.ToString());
                        items = items.Where(c => c.ImageDescription == "TRUE" || c.ImageDescription == "CONCLUSIVE").ToList(); // changed for conclusiveimages
                        foreach (var item in items)
                        {
                            App.VisualEditTrackingForInvasive.Add(new MultiImage() { ImageType = item.ImageDescription, Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
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