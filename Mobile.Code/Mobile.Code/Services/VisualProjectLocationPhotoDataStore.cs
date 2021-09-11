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
    public interface IVisualProjectLocationPhotoDataStore
    {
        Task<bool> AddItemAsync(VisualProjectLocationPhoto item);

        void Clear();
        Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<bool> DeleteItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual);
        Task<VisualProjectLocationPhoto> GetItemAsync(string id);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<VisualProjectLocationPhoto>> GetItemsAsyncByProjectVisualID(string locationVisualID, bool loadServer);

    }
    public class VisualProjectLocationPhotoDataStore : IVisualProjectLocationPhotoDataStore
    {
        List<VisualProjectLocationPhoto> items;

        public VisualProjectLocationPhotoDataStore()
        {
            items = new List<VisualProjectLocationPhoto>();

        }
        public async Task<bool> AddItemAsync(VisualProjectLocationPhoto item)
        {
            //if(App.IsInvasive==true)
            //{
            //    item.InvasiveImage = true;
            //}
            items.Add(item);
            App.VisualEditTracking.Add(new MultiImage() { Id = item.Id,Image=item.ImageUrl, ParentId = item.VisualLocationId, Status = "New" , IsServerData = false });
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(VisualProjectLocationPhoto item, bool IsEditVisual)
        {
            

          
            var oldItem = items.Where((VisualProjectLocationPhoto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            if(App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false&&c.Status== "New").Any())
            {
                var t = App.VisualEditTracking.Where(c => c.Id == item.Id && c.IsServerData == false && c.Status == "New").Single();
                t.Image = item.ImageUrl;

            }

            var oldITRaktem = App.VisualEditTracking.Where(c => c.Id == item.Id&&c.IsServerData==true).SingleOrDefault();
            if(oldITRaktem!=null)
            {
                App.VisualEditTracking.Remove(oldITRaktem);
                
                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, Image = item.ImageUrl, ParentId = item.VisualLocationId, Status = "Update" , IsServerData = true });
              
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
            if (oldDelete!=null)
            {
                App.VisualEditTracking.Remove(oldDelete);
              
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
                            items = items.Where(c => c.ImageDescription != "TRUE" && c.ImageDescription!="CONCLUSIVE").ToList();
                        foreach (var item in items)
                            {
                                App.VisualEditTracking.Add(new MultiImage() { Id = item.Id, ParentId = item.VisualLocationId, Status = "FromServer", Image = item.ImageUrl, IsDelete = false, IsServerData = true });
                                App.ImageFormString= JsonConvert.SerializeObject(App.VisualEditTracking);
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