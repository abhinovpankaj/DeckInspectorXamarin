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
    public interface IBuildingLocation
    {
        Task<Response> AddItemAsync(BuildingLocation item);
        Task<bool> UpdateItemAsync(BuildingLocation item);
        Task<Response> DeleteItemAsync(BuildingLocation item);
        Task<BuildingLocation> GetItemAsync(string id);
        Task<IEnumerable<BuildingLocation>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingLocation>> GetItemsAsyncByBuildingId(string BuildingId);

    }
    public class BuildingLocationDataStore : IBuildingLocation
    {
        List<BuildingLocation> items;

        public BuildingLocationDataStore()
        {
           
            items = new List<BuildingLocation>();
            

        }
        public async Task<Response> AddItemAsync(BuildingLocation item)
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


            //Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            //if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            //{
            //    item.ImageUrl = null;

            //}
            string ImageUrl = HttpUtil.GetImageUrl(item.ImageUrl).Result;
            result = await HttpUtil.UploadSingleImage(item.Name, ImageUrl, "api/BuildingLocation/AddEdit", parameters);


            return await Task.FromResult(result);

        }


        public async Task<BuildingLocation> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingLocation/GetBuildingLocationByID?Id=" + id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    BuildingLocation location = JsonConvert.DeserializeObject<BuildingLocation>(result.Data.ToString());


                    response.EnsureSuccessStatusCode();
                    return await Task.FromResult(location);


                }
            }
        }

        public async Task<bool> UpdateItemAsync(BuildingLocation item)
        {
            var oldItem = items.Where((BuildingLocation arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingLocation item)
        {
            item.IsDelete = true;
            item.UserId = App.LogUser.Id.ToString();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingLocation/DeleteBuildingLocation", item))
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



        public async Task<IEnumerable<BuildingLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<BuildingLocation>> GetItemsAsyncByBuildingId(string BuildingId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/BuildingLocation/GetBuildingLocationByBuildingID?BuildingId=" + BuildingId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingLocation>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}