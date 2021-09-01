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
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<BuildingLocation>();
            //{
            //    new BuildingLocation
            //    {
            //        Id = "1",
            //        BuildingId="1",
            //        BuildingLocationName  = "Building Common Location 1 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingLocation
            //    {
            //        Id = "2",
            //        BuildingId="1",
            //        BuildingLocationName  = "Building Common Location 2 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",

            //    },
            //    new BuildingLocation
            //    {
            //        Id = "3",
            //        BuildingId="1",
            //        BuildingLocationName  = "Building Common Location 3 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingLocation
            //    {
            //        Id = "4",
            //        BuildingId="2",
            //        BuildingLocationName  = "Building Common Location 4 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://www.thenbs.com/-/media/uk/new-images/by-size/1500_hero/helmetcolours_1500.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingLocation
            //    {
            //        Id = "5",
            //        BuildingId="2",
            //        BuildingLocationName  = "Building Common Location 5 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new BuildingLocation
            //    {
            //        Id = "6",
            //        BuildingId="2",
            //        BuildingLocationName  = "Building Common Location 6 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //      new BuildingLocation
            //    {
            //        Id = "6",
            //        BuildingId="2",
            //        BuildingLocationName  = "Building Common Location 6 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //       new BuildingLocation
            //    {
            //        Id = "6",
            //        BuildingId="1",
            //        BuildingLocationName  = "Building Common Location 6 ",
            //        Description="This is sample project description.",
            //        BuildingLocationImage="https://upload.wikimedia.org/wikipedia/commons/e/ec/The_crane_and_the_Main_Street_midrise_on_the_Infinity_(300_Spear_Street)_construction_site,_SF.JPG",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },

            //};

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