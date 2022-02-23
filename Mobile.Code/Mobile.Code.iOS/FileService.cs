using Foundation;
using Mobile.Code.iOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Mobile.Code.iOS
{
    public class FileService : IFileService
    {
        public string DownloadImage(string URL, string loc)
        {
            var webClient = new WebClient();
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = System.IO.Path.Combine(documentsPath, "DeckInspectors", "offline_" + loc);
            Directory.CreateDirectory(documentsPath);
            var partedURL = URL.Split('/');
            string localFilename = partedURL[partedURL.Length - 1];


            string localPath = System.IO.Path.Combine(documentsPath, localFilename);

            webClient.DownloadDataCompleted += (s, e) =>
            {
                byte[] bytes = new byte[e.Result.Length];
                bytes = e.Result; // get the downloaded data

                File.WriteAllBytes(localPath, bytes); // writes to local storage

            };
            var url = new Uri(URL);
            webClient.DownloadDataAsync(url);
            return localPath;
        }

        public string SavePicture(string name, Stream data, string location = "temp")
        {
            return "";
        }
    }
}