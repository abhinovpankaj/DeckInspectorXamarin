using Android.Content;
using Android.Graphics;
using Android.Widget;
using Mobile.Code.Droid;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace Mobile.Code.Droid
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFilesForCameraApi(string filename, byte[] bytes)
        {
            Bitmap originalImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);

            Matrix matrix = new Matrix();
            matrix.PostRotate(90);
            Bitmap resizedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);

            int imageWidth = originalImage.Width;
            int imageHeight = originalImage.Height;
            int newHeight = (imageHeight * 2000) / imageWidth;
            Bitmap newresizedImage = Bitmap.CreateScaledBitmap(resizedImage, 2000, newHeight, false);


            using (MemoryStream ms = new MemoryStream())
            {
                newresizedImage.Compress(Bitmap.CompressFormat.Jpeg, 30, ms);
                bytes = ms.ToArray();
            }
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = System.IO.Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytes);
            OpenFile(filePath, filename);
            return filePath;

        }
        public async Task<string> SaveFiles(string filename, byte[] bytes)
        {

            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = System.IO.Path.Combine(documentsPath, filename);
            File.WriteAllBytes(filePath, bytes);
            OpenFile(filePath, filename);
            return filePath;

        }
        public void OpenFile(string filePath, string filename)
        {

            var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            string application = "";

            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + filename + extension;
            File.WriteAllBytes(externalPath, bytes);

            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Photo saved successfully.", ToastLength.Short).Show();
            }
        }

        public string SavePicture(string name, Stream data, string location = "temp")
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = System.IO.Path.Combine(documentsPath, "DeckInspectors", location);
            Directory.CreateDirectory(documentsPath);

            string filePath = System.IO.Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
            return filePath;
        }

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
    }
}