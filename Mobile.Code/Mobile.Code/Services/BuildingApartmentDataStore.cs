using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mobile.Code.Services
{


    public interface IBuildingApartment
    {
        Task<Response> AddItemAsync(BuildingApartment item);
        Task<bool> UpdateItemAsync(BuildingApartment item);
        Task<Response> DeleteItemAsync(BuildingApartment item);
        Task<BuildingApartment> GetItemAsync(string id);
        Task<IEnumerable<BuildingApartment>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingApartment>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class BuildingApartmentDataStore : IBuildingApartment
    {
        List<BuildingApartment> items;

        public BuildingApartmentDataStore()
        {

            items = new List<BuildingApartment>();


        }
        public async Task<Response> AddItemAsync(BuildingApartment item)
        {

            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", item.Id);
            parameters.Add("Name", item.Name);

            parameters.Add("Description", item.Description);
            parameters.Add("BuildingId", item.BuildingId);
            parameters.Add("UserID", App.LogUser.Id.ToString());
            parameters.Add("ImageName", item.ImageName);
            parameters.Add("ImageUrl", item.ImageUrl);
            parameters.Add("ImageDescription", item.ImageDescription);


            string ImageUrl = HttpUtil.GetImageUrl(item.ImageUrl).Result;
            result = await HttpUtil.UploadSingleImage(item.Name, ImageUrl, "api/BuildingApartment/AddEdit", parameters);


            return await Task.FromResult(result);

        }


        public async Task<bool> UpdateItemAsync(BuildingApartment item)
        {
            var oldItem = items.Where((BuildingApartment arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingApartment item)
        {
            item.IsDelete = true;
            item.UserId = App.LogUser.Id.ToString();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingApartment/DeleteBuildingApartment", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<BuildingApartment> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingApartment/GetBuildingApartmentByID?Id=" + id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    BuildingApartment location = JsonConvert.DeserializeObject<BuildingApartment>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(location);


                }
            }
        }

        public async Task<IEnumerable<BuildingApartment>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<BuildingApartment>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingApartment/GetBuildingApartmentByBuildingID?BuildingId=" + BuildingId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingApartment>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}