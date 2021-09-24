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
    public interface IBuildingApartmentImages
    {
        Task<bool> AddItemAsync(BuildingApartmentImages item);
        Task<bool> UpdateItemAsync(BuildingApartmentImages item);
        Task<Response> DeleteItemAsync(BuildingApartmentImages item);
        Task<BuildingApartmentImages> GetItemAsync(string id);
        Task<IEnumerable<BuildingApartmentImages>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingApartmentImages>> GetItemsAsyncByApartmentID(string ApartmentID);

    }
    public class BuildingApartmentImagesDataStore : IBuildingApartmentImages
    {
        List<BuildingApartmentImages> items;

        public BuildingApartmentImagesDataStore()
        {

            items = new List<BuildingApartmentImages>();

        }
        public async Task<bool> AddItemAsync(BuildingApartmentImages item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BuildingApartmentImages item)
        {

            Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            {
                item.ImageUrl = null;
                return await Task.FromResult(true);

            }
            Response result = HttpUtil.Update_Image("Apartment", item.ImageUrl, "/api/BuildingApartmentImage/AddEdit?ParentId=" + item.BuildingApartmentId + "&UserId=" + App.LogUser.Id.ToString() + "&Id=" + item.Id).Result;

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingApartmentImages item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingApartmentImage/DeleteBuildingApartmentImage", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<BuildingApartmentImages> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BuildingApartmentImages>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<BuildingApartmentImages>> GetItemsAsyncByApartmentID(string ApartmentID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingApartmentImage/GetBuildingApartmentImageByBuildingApartmentId?BuildingApartmentId=" + ApartmentID))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingApartmentImages>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}