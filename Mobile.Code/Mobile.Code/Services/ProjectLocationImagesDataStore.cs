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
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<ProjectCommonLocationImages>();
            //{
            //    new ProjectCommonLocationImages
            //    {
            //        Id = "1",
            //        ProjectLocationId="1",
            //        Name  = "Project Location Image 1 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",
            //        CreatedOn=" May 3 ,2020",
                  

            //    },
            //    new ProjectCommonLocationImages
            //    {
            //        Id = "2",
            //        ProjectLocationId="1",
            //        Name  = "Project Location Image 2 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",

            //    },
            //    new ProjectCommonLocationImages
            //    {
            //        Id = "3",
            //        ProjectLocationId="1",
            //        Name  = "Project Location Image 3 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectCommonLocationImages
            //    {
            //        Id = "4",
            //        ProjectLocationId="2",
            //        Name  = "Project Location Image 4 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new ProjectCommonLocationImages
            //    {
            //        Id = "5",
            //        ProjectLocationId="2",
            //        Name  = "Project Location Image 5 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new ProjectCommonLocationImages
            //    {
            //        Id = "6",
            //        ProjectLocationId="2",
            //        Name  = "Project Location Image 6 ",
            //        Description="This is sample project description.",
            //        ImageUrl="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },

            //};

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