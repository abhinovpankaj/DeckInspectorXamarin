using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mobile.Code.Services
{
    public interface IBuildingCommonLocationImages
    {
        Task<bool> AddItemAsync(BuildingCommonLocationImages item);
        Task<bool> UpdateItemAsync(BuildingCommonLocationImages item);
        Task<Response> DeleteItemAsync(BuildingCommonLocationImages item);
        Task<BuildingCommonLocationImages> GetItemAsync(string id);
        Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class BuildingCommonLocationImagesDataStore : IBuildingCommonLocationImages
    {
        List<BuildingCommonLocationImages> items;

        public BuildingCommonLocationImagesDataStore()
        {
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<BuildingCommonLocationImages>();


        }
        public async Task<bool> AddItemAsync(BuildingCommonLocationImages item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BuildingCommonLocationImages item)
        {
            Regex UrlMatch = new Regex(@"^(http|https)://", RegexOptions.Singleline);
            // Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            {
                item.ImageUrl = null;
                return await Task.FromResult(true);

            }
            Response result = HttpUtil.Update_Image("BuildingLocation", item.ImageUrl, "/api/BuildingLocationImage/AddEdit?ParentId=" + item.BuildingLocationId + "&UserId=" + App.LogUser.Id.ToString() + "&Id=" + item.Id).Result;

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingCommonLocationImages item)
        {
            item.UserId = App.LogUser.Id.ToString();
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingLocationImage/DeleteBuildingLocationImage", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();
                    //if (response.IsSuccessStatusCode == false)
                    //{
                    //    throw new ApiException
                    //    {
                    //        StatusCode = (int)response.StatusCode,
                    //        Content = result.Message
                    //    };
                    //}
                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<BuildingCommonLocationImages> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<BuildingCommonLocationImages>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingLocationImage/GetBuildingLocationImageByBuildingLocationId?BuildingLocationId=" + BuildingId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingCommonLocationImages>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}