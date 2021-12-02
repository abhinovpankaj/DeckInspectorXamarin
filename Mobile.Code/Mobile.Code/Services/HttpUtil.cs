using Mobile.Code.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mobile.Code.Services
{
    public class HttpUtil
    {
        public static bool Upload(string FileName, String endpointUrl, IEnumerable<MultiImage> list)
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                using (var formData = new MultipartFormDataContent())
                {
                    int Index = 1000;
                    foreach (MultiImage img in list)
                    {
                        Index++;
                        var extension = Path.GetExtension(img.Image);
                        string ServerFileName = FileName.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + "_" + Index + ".png";
                        formData.Add(new ByteArrayContent(File.ReadAllBytes(img.Image)), Index.ToString(), ServerFileName);
                    }
                    var response = client.PostAsync(endpointUrl, formData).Result;

                    //   Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                        //var responseBody = await response.Content.ReadAsStringAsync();
                        //Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                    }
                    else
                    {

                    }

                }
                return true;
            }
        }

        public static bool UploadFromGallary(string FileName, String endpointUrl, IEnumerable<string> list)
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                using (var formData = new MultipartFormDataContent())
                {
                    int Index = 1000;
                    foreach (string img in list)
                    {
                        Index++;
                        var extension = Path.GetExtension(img);
                        string ServerFileName = FileName.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + "_" + Index + extension;
                        formData.Add(new ByteArrayContent(File.ReadAllBytes(img)), Index.ToString(), ServerFileName);
                    }
                    var response = client.PostAsync(endpointUrl, formData).Result;

                    //   Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                        //var responseBody = await response.Content.ReadAsStringAsync();
                        //Response result = JsonConvert.DeserializeObject<Response>(responseBody);
                    }
                    else
                    {

                    }

                }
                return true;
            }
        }
        public static async Task<Response> UploadSingleImage(string Name, string ImageUrl, string endpointUrl, Dictionary<string, string> parameters)
        {


            Response result = new Response();
            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                using (var formData = new MultipartFormDataContent())
                {
                    //int Index = 1000;
                    //foreach (MultiImage img in list)
                    //{
                    //    Index++;
                    //    var extension = Path.GetExtension(img.Image);

                    //}
                    string ServerFileName = Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + ".Jpeg";
                    if (!string.IsNullOrEmpty(ImageUrl))
                    {
                        Regex UrlMatch = new Regex(@"^(http|https)://", RegexOptions.Singleline);
                        if (ImageUrl == "blank.png" || UrlMatch.IsMatch(ImageUrl))
                        {
                            ImageUrl = null;
                        }
                        else
                        {
                            //byte[] resizedImage = DependencyService.Get<IImageService>().ResizeTheImage(File.ReadAllBytes(ImageUrl), 2000, 1500);
                            formData.Add(new ByteArrayContent(File.ReadAllBytes(ImageUrl)), "fileToUpload", ServerFileName);
                        }
                       // formData.Add(new ByteArrayContent(resizedImage), "fileToUpload", ServerFileName);
                    }
                    formData.Add(DictionaryItems, "Model");
                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);

                }

            }
        }
        public static async Task<Response> Update_Image(string Name, string ImageUrl, string endpointUrl)
        {
            string UserID = App.LogUser.Id.ToString();

            Response result = new Response();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                using (var formData = new MultipartFormDataContent())
                {

                    string ServerFileName = Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + ".png";
                    if (!string.IsNullOrEmpty(ImageUrl))
                    {
                        formData.Add(new ByteArrayContent(File.ReadAllBytes(ImageUrl)), "fileToUpload", ServerFileName);
                    }

                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);

                }

            }
        }


        public static async Task<Response> VisualDataAdd(string Name, string endpointUrl, Dictionary<string, string> parameters, IEnumerable<string> list)
        {



            Response result = new Response();
            HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.AzureBackendUrl);
                client.Timeout = TimeSpan.FromSeconds(60);
                using (var formData = new MultipartFormDataContent())
                {
                    //int Index = 1000;
                    //foreach (MultiImage img in list)
                    //{
                    //    Index++;
                    //    var extension = Path.GetExtension(img.Image);

                    //}
                    int Index = 1000;
                    foreach (string img in list)
                    {
                        Index++;
                        // var extension = Path.GetExtension(img);
                        string ServerFileName = Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + "_" + Index + ".png";
                        formData.Add(new ByteArrayContent(File.ReadAllBytes(img)), Index.ToString(), ServerFileName);
                    }
                    formData.Add(DictionaryItems, "Model");
                    //string ServerFileName = Name.Replace(" ", "_") + DateTime.Now.ToString("ddMMMyyyyHHmmss.FFF") + ".png";
                    //if (!string.IsNullOrEmpty(ImageUrl))
                    //{
                    //    formData.Add(new ByteArrayContent(File.ReadAllBytes(ImageUrl)), "fileToUpload", ServerFileName);
                    //}

                    var response = client.PostAsync(endpointUrl, formData).Result;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Response>(responseBody);
                    return await Task.FromResult(result);

                }

            }
        }
        public static async Task<string> GetImageUrl(string ImageUrl)
        {
            Regex UrlMatch = new Regex(@"^(http|https)://", RegexOptions.Singleline);
            string returnImage = ImageUrl;
            if (ImageUrl == "blank.png")
            {
                returnImage = string.Empty;
            }
            if (UrlMatch.IsMatch(ImageUrl))
            {
                returnImage = ImageUrl;
                // parameters.Add("IsFileExist", "False");
            }
            return await Task.FromResult(returnImage);
        }

    }
}
