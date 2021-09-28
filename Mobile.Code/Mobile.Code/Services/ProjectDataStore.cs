using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mobile.Code.Services
{
    public interface IProjectDataStore
    {
        Task<Response> AddItemAsync(Project item);

        Task<Response> UpdateItemAsync(Project item);
        Task<Response> DeleteItemAsync(Project item);
        Task<Project> GetItemAsync(string id);
        Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false);

        Task<Response> CreateInvasiveReport(Project item);


    }
    public class ProjectDataStore : IProjectDataStore
    {
        List<Project> items;

        public ProjectDataStore()
        {

            items = new List<Project>();


        }
        public async Task<Response> AddItemAsync(Project item)
        {

            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", item.Id);
            parameters.Add("Name", item.Name);
            parameters.Add("Address", item.Address);
            parameters.Add("Description", item.Description);
            parameters.Add("ProjectType", item.ProjectType);
            parameters.Add("UserID", App.LogUser.Id.ToString());
            parameters.Add("ImageName", item.ImageName);
            parameters.Add("ImageUrl", item.ImageUrl);
            parameters.Add("ImageDescription", item.ImageDescription);

            parameters.Add("Category", item.Category);
            //Regex UrlMatch = new Regex(@"^(http|https)://", RegexOptions.Singleline);
            //string ImageUrl = HttpUtil.GetImageUrl(item.ImageUrl).Result;

            result = await HttpUtil.UploadSingleImage(item.Name, item.ImageUrl, "api/Project/AddEdit", parameters);
            return await Task.FromResult(result);

        }


        public async Task<Response> UpdateItemAsync(Project item)
        {
            Response result = new Response();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Id", item.Id);
                parameters.Add("Name", item.Name);
                parameters.Add("Address", item.Address);
                parameters.Add("Description", item.Description);
                parameters.Add("ProjectType", item.ProjectType);
                parameters.Add("UserID", App.LogUser.Id.ToString());
                parameters.Add("ImageName", item.ImageName);
                parameters.Add("ImageDescription", item.ImageDescription);
                parameters.Add("ImageUrl", item.ImageUrl);

                parameters.Add("Category", item.Category);

                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
                HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);

                form.Add(DictionaryItems, "Model");
                Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
                if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
                {
                    item.ImageUrl = null;
                }
                else
                {
                    form.Add(content, "fileToUpload");
                    /// System.IO.File.OpenRead(filePath)
                    using (var stream = System.IO.File.OpenRead(item.ImageUrl))
                    {
                        content = new StreamContent(stream);

                    }

                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "fileToUpload",
                        //  FileName = (item.Name + .Replace(" ","_") + ".png"
                        FileName = item.Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".png"
                    };

                    form.Add(content);
                }

                HttpResponseMessage response = null;

                try
                {
                    response = await client.PostAsync(App.AzureBackendUrl + "api/Project/AddEdit?IsEdit=true", form);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    //item.Id = result.ID;
                    //Project obj = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
                    return await Task.FromResult(result);

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    //  Console.WriteLine(ex.Message);
                }
                if (response.IsSuccessStatusCode == true)
                {

                }
            }


            return await Task.FromResult(result);
        }

        public async Task<Response> DeleteItemAsync(Project item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/DeleteProject", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }

        public async Task<Response> CreateInvasiveReport(Project item)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/Project/CreateInvasiveReport", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }

        }

        public async Task<Project> GetItemAsync(string id)
        {


            string Query = string.Format("api/Project/GetProjectByIDMobile?Id={0}&UserId={1}", id, App.LogUser.Id.ToString());


            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(Query))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    Project p = JsonConvert.DeserializeObject<Project>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(p);


                }
            }
        }

        public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false)
        {
            string Query = string.Empty;
            if (App.LogUser.RoleName == "Admin")
            {
                Query = string.Format("api/Project/GetProjectList?CreatedOn={0}&isActive={1}&searchText={2}&ProjectType={3}", null, false, string.Empty, string.Empty);
            }
            else
            {
                Query = string.Format("api/Project/GetProjectForMobile?UserID={0}&CreatedOn={1}&isActive={2}&searchText={3}&ProjectType={4}", App.LogUser.Id.ToString(), null, false, string.Empty, string.Empty);
            }

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(Query))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<Project>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }
    }
}