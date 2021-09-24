using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mobile.Code.Services
{
    public interface IVisualFormBuildingLocationDataStore
    {
        Task<Response> AddItemAsync(BuildingLocation_Visual item, IEnumerable<string> ImageList);
        Task<Response> UpdateItemAsync(BuildingLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE");
        Task<Response> DeleteItemAsync(BuildingLocation_Visual item);
        Task<BuildingLocation_Visual> GetItemAsync(string id);
        Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId);

    }
    public class VisualFormBuildingLocationDataStore : IVisualFormBuildingLocationDataStore
    {
        List<BuildingLocation_Visual> items;

        public VisualFormBuildingLocationDataStore()
        {
            items = new List<BuildingLocation_Visual>();

        }
        public async Task<Response> AddItemAsync(BuildingLocation_Visual item, IEnumerable<string> ImageList)
        {
            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Name", item.Name);
            parameters.Add("BuildingLocationId", item.BuildingLocationId);
            parameters.Add("AdditionalConsideration", item.AdditionalConsideration);

            parameters.Add("ExteriorElements", item.ExteriorElements);
            parameters.Add("WaterProofingElements", item.WaterProofingElements);
            parameters.Add("ConditionAssessment", item.ConditionAssessment);
            parameters.Add("VisualReview", item.VisualReview);

            parameters.Add("AnyVisualSign", item.AnyVisualSign);
            parameters.Add("FurtherInasive", item.FurtherInasive);
            parameters.Add("LifeExpectancyEEE", item.LifeExpectancyEEE);
            parameters.Add("LifeExpectancyLBC", item.LifeExpectancyLBC);

            parameters.Add("LifeExpectancyAWE", item.LifeExpectancyAWE);
            parameters.Add("UserID", App.LogUser.Id.ToString());

            parameters.Add("ImageDescription", item.ImageDescription);


            result = await HttpUtil.VisualDataAdd(item.Name, "api/VisualBuildingLocation/Add", parameters, ImageList);


            return await Task.FromResult(result);
        }

        public async Task<Response> UpdateItemAsync(BuildingLocation_Visual item, List<MultiImage> finelList, string imgType = "TRUE")
        {
            Response result = new Response();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", item.Id);
            parameters.Add("Name", item.Name);
            parameters.Add("BuildingLocationId", item.BuildingLocationId);
            parameters.Add("AdditionalConsideration", item.AdditionalConsideration);

            parameters.Add("ExteriorElements", item.ExteriorElements);
            parameters.Add("WaterProofingElements", item.WaterProofingElements);
            parameters.Add("ConditionAssessment", item.ConditionAssessment);
            parameters.Add("VisualReview", item.VisualReview);

            parameters.Add("AnyVisualSign", item.AnyVisualSign);
            parameters.Add("FurtherInasive", item.FurtherInasive);
            parameters.Add("LifeExpectancyEEE", item.LifeExpectancyEEE);
            parameters.Add("LifeExpectancyLBC", item.LifeExpectancyLBC);

            parameters.Add("LifeExpectancyAWE", item.LifeExpectancyAWE);


            parameters.Add("ImageDescription", item.ImageDescription);
            parameters.Add("UserID", App.LogUser.Id.ToString());


            parameters.Add("ConclusiveComments", item.ConclusiveComments);
            parameters.Add("ConclusiveLifeExpEEE", item.ConclusiveLifeExpEEE);
            parameters.Add("ConclusiveLifeExpLBC", item.ConclusiveLifeExpLBC);
            parameters.Add("ConclusiveLifeExpAWE", item.ConclusiveLifeExpAWE);
            parameters.Add("ConclusiveAdditionalConcerns", item.ConclusiveAdditionalConcerns);
            parameters.Add("IsPostInvasiveRepairsRequired", item.IsPostInvasiveRepairsRequired.ToString());
            parameters.Add("IsInvasiveRepairApproved", item.IsInvasiveRepairApproved.ToString());
            parameters.Add("IsInvasiveRepairComplete", item.IsInvasiveRepairComplete.ToString());


            if (App.IsInvasive == true)
            {

                parameters.Add("IsInvaiveImage", imgType);

            }
            else
            {
                parameters.Add("IsInvaiveImage", null);


            }


            //  result = await HttpUtil.VisualDataAdd(item.Name, "api/VisualProjectLocation/Add", parameters, ImageList);

            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);

                using (var formData = new MultipartFormDataContent())
                {
                    //https://www.dotnetperls.com/split

                    int Index = 1000;
                    foreach (MultiImage img in finelList)
                    {
                        Index++;
                        if (img.IsServerData == false && img.Status != "FromServer")
                        {
                            string ServerFileName = "New_" + img.Id;

                            string[] array = ServerFileName.Split('_');
                            string operation = array[0];
                            string searchId = array[1];
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Update")
                        {
                            string ServerFileName = "Update_" + img.Id;
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                        }
                        if (img.IsServerData == true && img.Status == "Delete")
                        {
                            string ServerFileName = "Delete_" + img.Id;
                            var sevenThousandItems = new byte[7000];
                            formData.Add(new ByteArrayContent(sevenThousandItems), Index.ToString(), ServerFileName);
                        }
                        //if (img.IsServerData == true && img.Status == "Delete")
                        //{
                        //    string ServerFileName = "Delete" + img.Id + ".png";
                        //    formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                    }
                    // var extension = Path.GetExtension(img);
                    formData.Add(DictionaryItems, "Model");


                    var response = client.PostAsync("api/VisualBuildingLocation/Edit", formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);
                }
            }
        }

        public async Task<Response> DeleteItemAsync(BuildingLocation_Visual item)
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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"api/VisualBuildingLocation/DeleteVisualBuildingLocation", item))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(result);


                }
            }
        }

        public async Task<BuildingLocation_Visual> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }


        public async Task<IEnumerable<BuildingLocation_Visual>> GetItemsAsyncByBuildingLocationId(string buildingLocationId)
        {

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync($"api/VisualBuildingLocation/GetVisualBuildingLocationByBuildingLocationId?BuildingLocationId=" + buildingLocationId + "&isInvasive=" + App.IsInvasive))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Response result = JsonConvert.DeserializeObject<Response>(responseBody);


                    items = JsonConvert.DeserializeObject<List<BuildingLocation_Visual>>(result.Data.ToString());

                    response.EnsureSuccessStatusCode();

                    return await Task.FromResult(items);


                }
            }
        }




    }
}