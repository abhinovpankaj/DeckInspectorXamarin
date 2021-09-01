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
    public interface IProjectBuilding
    {
        Task<Response> AddItemAsync(ProjectBuilding item);
        Task<bool> UpdateItemAsync(ProjectBuilding item);
        Task<Response> DeleteItemAsync(ProjectBuilding item);
        Task<ProjectBuilding> GetItemAsync(string id);
        Task<IEnumerable<ProjectBuilding>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectID(string projectId);

    }
    public class ProjectBuildingDataStore : IProjectBuilding
    {
         List<ProjectBuilding> items;

        public ProjectBuildingDataStore()
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
            items = new List<ProjectBuilding>();
            //{
            //    new ProjectBuilding
            //    {
            //        Id = "1",
            //        ProjectId="1",
            //        BuildingName  = "Project Building 1 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://upload.wikimedia.org/wikipedia/commons/e/ec/The_crane_and_the_Main_Street_midrise_on_the_Infinity_(300_Spear_Street)_construction_site,_SF.JPG",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectBuilding
            //    {
            //         Id = "2",
            //        ProjectId="1",
            //        BuildingName  = "Project Building 2 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectBuilding
            //    {
            //        Id = "3",
            //        ProjectId="1",
            //        BuildingName  = "Project Building 3 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",



            //    },
            //    new ProjectBuilding
            //    {
            //        Id = "4",
            //        ProjectId="2",
            //        BuildingName  = "Project Building 4 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",



            //    },
            //    new ProjectBuilding
            //    {
            //     Id = "5",
            //        ProjectId="2",
            //        BuildingName  = "Project Building 5 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",



            //    },
            //     new ProjectBuilding
            //    {
            //         Id = "6",
            //        ProjectId="2",
            //        BuildingName  = "Project Building 6 ",
            //        Description="This is sample project description.",
            //        BuildingImage="https://upload.wikimedia.org/wikipedia/commons/e/ec/The_crane_and_the_Main_Street_midrise_on_the_Infinity_(300_Spear_Street)_construction_site,_SF.JPG",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",



            //    },

            //};


        }
        public async Task<bool> UpdateItemAsync(ProjectBuilding item)
        {
            var oldItem = items.Where((ProjectBuilding arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(ProjectBuilding item)
        {
            item.IsDelete = true;
            item.UserId = App.LogUser.Id.ToString();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/ProjectBuilding/DeleteProjectBuilding", item))
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
        public async Task<Response> AddItemAsync(ProjectBuilding item)
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


            //Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            //if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            //{
            //    item.ImageUrl = null;

            //}
            string ImageUrl = HttpUtil.GetImageUrl(item.ImageUrl).Result;
            result = await HttpUtil.UploadSingleImage(item.Name, ImageUrl, "api/ProjectBuilding/AddEdit", parameters);


            return await Task.FromResult(result);

        }
       

        public async Task<ProjectBuilding> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectBuilding/GetProjectBuildingByID?Id=" + id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    ProjectBuilding projectlocation = JsonConvert.DeserializeObject<ProjectBuilding>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(projectlocation);


                }
            }
        }

        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
      

        public async Task<IEnumerable<ProjectBuilding>> GetItemsAsyncByProjectID(string projectId)
        {

            items = new List<ProjectBuilding>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/ProjectBuilding/GetBuildingByProjectID?ProjectID=" + projectId+ "&UserId="+App.LogUser.Id))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    if (result.Status == ApiResult.Success)
                    {
                        items = JsonConvert.DeserializeObject<List<ProjectBuilding>>(result.Data.ToString());

                        response.EnsureSuccessStatusCode();
                    }
                    return await Task.FromResult(items);


                }
            }
        }
    }
}