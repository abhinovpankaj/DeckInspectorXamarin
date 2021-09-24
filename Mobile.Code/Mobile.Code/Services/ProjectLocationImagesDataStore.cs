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
    public interface IProjectCommonLocationImages
    {
        Task<bool> AddItemAsync(ProjectCommonLocationImages item);
        Task<bool> UpdateItemAsync(ProjectCommonLocationImages item);
        Task<Response> DeleteItemAsync(ProjectCommonLocationImages item);
        Task<ProjectCommonLocationImages> GetItemAsync(string id);
        Task<IEnumerable<ProjectCommonLocationImages>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<ProjectCommonLocationImages>> GetItemsAsyncByProjectLocationId(string projectLocationId);

    }
    public class ProjectCommonLocationImagesDataStore : IProjectCommonLocationImages
    {
        List<ProjectCommonLocationImages> items;

        public ProjectCommonLocationImagesDataStore()
        {

            items = new List<ProjectCommonLocationImages>();

        }
        public async Task<bool> AddItemAsync(ProjectCommonLocationImages item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ProjectCommonLocationImages item)
        {
            Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            {
                item.ImageUrl = null;
                return await Task.FromResult(true);

            }
            Response result = HttpUtil.Update_Image("Location", item.ImageUrl, "/api/ProjectLocationImage/AddEdit?ParentId=" + item.ProjectLocationId + "&UserId=" + App.LogUser.Id.ToString() + "&Id=" + item.Id).Result;

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(ProjectCommonLocationImages item)
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocationImage/DeleteProjectLocationImage", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<ProjectCommonLocationImages> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ProjectCommonLocationImages>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<ProjectCommonLocationImages>> GetItemsAsyncByProjectLocationId(string projectLocationId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectLocationImage/GetProjectLocationImageByProjectLocationId?ProjectLocationId=" + projectLocationId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<ProjectCommonLocationImages>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}