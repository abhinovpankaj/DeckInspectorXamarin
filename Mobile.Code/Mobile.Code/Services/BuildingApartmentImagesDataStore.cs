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
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
            items = new List<BuildingApartmentImages>();
            //{
            //    new BuildingApartmentImages
            //    {
            //        Id = "1",
            //        ApartmentID="1",
            //        Name  = "Apartment Image 1 ",
            //        Description="This is sample project description.",
            //        Image="https://thumbs.dreamstime.com/z/construction-site-construction-workers-area-people-working-construction-group-people-professional-construction-118630790.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",
            //        CreatedOn=" May 3 ,2020",
                  

            //    },
            //    new BuildingApartmentImages
            //    {
            //        Id = "2",
            //        ApartmentID="1",
            //        Name  = "Apartment Image 2 ",
            //        Description="This is sample project description.",
            //        Image="https://m.economictimes.com/thumb/msid-69127844,width-1200,height-900,resizemode-4,imgsize-347903/construction-site-generators-types-features-of-generators-used-at-construction-sites.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",

            //    },
            //    new BuildingApartmentImages
            //    {
            //        Id = "3",
            //        ApartmentID="1",
            //        Name  = "Apartment Image 3 ",
            //        Description="This is sample project description.",
            //        Image="https://images.globest.com/contrib/content/uploads/sites/304/2020/04/Construction-resized.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingApartmentImages
            //    {
            //        Id = "4",
            //        ApartmentID="2",
            //        Name  = "Apartment Image 4 ",
            //        Description="This is sample project description.",
            //        Image="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //    new BuildingApartmentImages
            //    {
            //        Id = "5",
            //        ApartmentID="2",
            //        Name  = "Apartment Image 5 ",
            //        Description="This is sample project description.",
            //        Image="https://media.istockphoto.com/photos/professional-engineer-worker-at-the-house-building-construction-site-picture-id905891244",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },
            //     new BuildingApartmentImages
            //    {
            //        Id = "6",
            //        ApartmentID="2",
            //        Name  = "Apartment Image 6 ",
            //        Description="This is sample project description.",
            //        Image="https://www.ukconstructionmedia.co.uk/wp-content/uploads/Screen-Shot-2016-04-21-at-11.55.06.jpg",
            //        Attendent="Attendent Abhinov",
            //        EmployeeName="Point5Nyble",

            //        CreatedOn=" May 3 ,2020",


            //    },

            //};

        }
        public async Task<bool> AddItemAsync(BuildingApartmentImages item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BuildingApartmentImages item)
        {
            //var oldItem = items.Where((BuildingApartmentImages arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);
            Regex UrlMatch = new Regex(@"(?i)(http(s)?:\/\/)?(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
            if (item.ImageUrl == "blank.png" || UrlMatch.IsMatch(item.ImageUrl))
            {
                item.ImageUrl = null;
                return await Task.FromResult(true);

            }
            Response result=  HttpUtil.Update_Image("Apartment", item.ImageUrl, "/api/BuildingApartmentImage/AddEdit?ParentId=" + item.BuildingApartmentId + "&UserId=" + App.LogUser.Id.ToString() + "&Id=" + item.Id).Result;

            return await Task.FromResult(true);
        }

        public async Task<Response> DeleteItemAsync(BuildingApartmentImages item)
        {
            item.IsDelete = true;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/BuildingApartmentImage/DeleteBuildingApartmentImage", item))
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