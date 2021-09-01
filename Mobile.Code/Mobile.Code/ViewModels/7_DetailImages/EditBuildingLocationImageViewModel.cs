using ImageEditor.ViewModels;
using Mobile.Code.Models;
using Mobile.Code.Utils;
using Mobile.Code.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class EditBuildingLocationImageViewModel : BaseViewModel
    {
        private ImageData _imgData;
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }

        public ImageData ImgData
        {
            get { return _imgData; }
            set { _imgData = value; }
        }

        public string SelectedImage { get; set; }

        public ICommand ChoosePhotoCommand { get; set; }
        private string _heading;

        public string Heading
        {
            get { return _heading; }
            set { _heading = value; OnPropertyChanged("Heading"); }
        }
        public Command LoadItemsCommand { get; set; }
        private async Task GoBack()
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to go back?",
                  "Yes", "No");



            if (result)
            {
                await Shell.Current.Navigation.PopAsync();
                // await Shell.Current.Navigation.Cle ;
            }
        }
        private BuildingCommonLocationImages _BuildingCommonLocationImages;

        public BuildingCommonLocationImages BuildingCommonLocationImages
        {
            get { return _BuildingCommonLocationImages; }
            set { _BuildingCommonLocationImages = value; OnPropertyChanged("BuildingCommonLocationImages"); }
        }

        private async Task Save()
        {
            if (string.IsNullOrEmpty(BuildingCommonLocationImages.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Project name is required", "OK");
                return;
            }
            if (string.IsNullOrEmpty(BuildingCommonLocationImages.Id))
            {
                BuildingCommonLocationImages.Id = Guid.NewGuid().ToString();

                BuildingCommonLocationImages.BuildingId = BuildingLocation.Id;
                ////Project.
                ////   Project.ProjectImage = ImgPata;

                await BuildingCommonLocationImagesDataStore.AddItemAsync(BuildingCommonLocationImages);
                await Shell.Current.Navigation.PopAsync();
               // await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail() { BindingContext = new ProjectLocationDetailViewModel() { ProjectLocation = ProjectLocation } });
            }
            else
            {
                await BuildingCommonLocationImagesDataStore.UpdateItemAsync(BuildingCommonLocationImages);
                await Shell.Current.Navigation.PopAsync();
            }
            //await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
        }
        private BuildingLocation _bloc;

        public BuildingLocation BuildingLocation
        {
            get { return _bloc; }
            set { _bloc = value;OnPropertyChanged("BuildingLocation"); }
        }

        public EditBuildingLocationImageViewModel()
        {
            //ProjectCommonLocationImages = new ProjectCommonLocationImages();
            //ProjectCommonLocationImages.ImageUrl = "blank.png";
            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());

            //MessagingCenter.Subscribe<ImageEditor.Pages.ImageEditorPage, string>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as string;
            //    await App.Current.MainPage.DisplayAlert(newItem,newItem,"ok","cancel");
            //});
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //Load();
            ImgData = new ImageData();
        }
        public void Load()
        {

        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        private async Task ChoosePhotoCommandExecute()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", "",
                new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    SelectedImage = await TakePictureFromCamera();
                    break;
                case "From Gallery":
                    SelectedImage = await TakePictureFromLibrary();
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(SelectedImage))
            {
                //var fileLength = new FileInfo(SelectedImage).Length;
                //string size = GetFileSize(fileLength);
                //ImgData.Size = size;
                ImgData.Path = SelectedImage;
                await Current.EditImage(ImgData, GetImage);
            }
        }



        private async Task<string> TakePictureFromLibrary()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.PickPhotoAsync
                (new PickMediaOptions()
                {
                    SaveMetaData = true,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    CompressionQuality = App.CompressionQuality


                }); ;

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;

        }
        public static string GetFileSize(long length)
        {
            //string[] sizes = { "B", "KB", "MB", "GB" };
            //int order = 0;
            //while (fileLength >= 1024 && order + 1 < sizes.Length)
            //{
            //    order++;
            //    fileLength = fileLength / 1024;
            //}
            //string result = String.Format("{0:0.##} {1}", fileLength, sizes[order]);
            //return result;
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            string suffix = nameof(B);

            if (length >= TB)
            {
                size = Math.Round((double)length / TB, 2);
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                size = Math.Round((double)length / GB, 2);
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                size = Math.Round((double)length / MB, 2);
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                size = Math.Round((double)length / KB, 2);
                suffix = nameof(KB);
            }

            return $"{size} {suffix}";
        }
        private async Task<string> TakePictureFromCamera()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.TakePhotoAsync
                (new StoreCameraMediaOptions()
                {
                    SaveMetaData = true,
                    DefaultCamera = CameraDevice.Rear,
                    CompressionQuality = App.CompressionQuality
                });

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;
        }
        private string _imgPath;

        public string ImgPata
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private void GetImage(ImageData ImgData)
        {
            BuildingCommonLocationImages.ImageUrl = ImgData.Path;
            //using (var fs = new FileStream(ImgData.Path, FileMode.Open, FileAccess.Read))
            //{
            //    var imageData = new byte[fs.Length];
            //    fs.Read(imageData, 0, (int)fs.Length);
            //    var base64String = Convert.ToBase64String(imageData);
            //    project.ProjectImage = base64String;
            //}
            //Project.ProjectImage = ImgData.Path;

            // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }

    }

}
