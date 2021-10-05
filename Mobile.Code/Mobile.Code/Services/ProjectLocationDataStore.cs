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
    public interface IProjectLocation
    {
        Task<Response> AddItemAsync(ProjectLocation item);
        Task<bool> UpdateItemAsync(ProjectLocation item);
        Task<Response> DeleteItemAsync(ProjectLocation item);
        Task<ProjectLocation> GetItemAsync(string id);

        //   Task<bool> AddProjectCommonLocationAsync(ProjectLocation item);
        Task<IEnumerable<ProjectLocation>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectID(string projectId);

    }
    public class ProjectLocationDataStore : IProjectLocation
    {
        List<ProjectLocation> items;

        public ProjectLocationDataStore()
        {

            items = new List<ProjectLocation>();

        }
        public async Task<Response> AddItemAsync(ProjectLocation item)
        {


            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", item.Id);
            parameters.Add("Name", item.Name);

            parameters.Add("Description", item.Description);
            parameters.Add("ProjectId", item.ProjectId);
            parameters.Add("UserID", App.LogUser.Id.ToString());
            parameters.Add("ImageName", item.ImageName);
            parameters.Add("ImageUrl", item.ImageUrl);
            parameters.Add("ImageDescription", item.ImageDescription);


            string ImageUrl = HttpUtil.GetImageUrl(item.ImageUrl).Result;
            result = await HttpUtil.UploadSingleImage(item.Name, ImageUrl, "api/ProjectLocation/AddEdit", parameters);


            return await Task.FromResult(result);

        }


        public async Task<bool> UpdateItemAsync(ProjectLocation item)
        {
            var oldItem = items.Where((ProjectLocation arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(ProjectLocation item)
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectLocation/DeleteProjectLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);
                }
            }
        }

        public async Task<ProjectLocation> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectLocation/GetProjectLocationByID?Id=" + id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    ProjectLocation projectlocation = JsonConvert.DeserializeObject<ProjectLocation>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(projectlocation);


                }
            }
        }

        public async Task<IEnumerable<ProjectLocation>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<ProjectLocation>> GetItemsAsyncByProjectID(string projectId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectLocation/GetLocationByProjectID?ProjectID=" + projectId + "&UserId=" + App.LogUser.Id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<ProjectLocation>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}