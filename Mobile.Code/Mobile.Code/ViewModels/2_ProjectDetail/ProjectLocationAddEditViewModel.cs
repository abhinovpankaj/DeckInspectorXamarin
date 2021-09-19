using ImageEditor.ViewModels;
using Mobile.Code.Models;
using Mobile.Code.Utils;
using Mobile.Code.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    
    [QueryProperty("Id", "Id")]
    public class ProjectLocationAddEditViewModel : BaseViewModel
    {
        private ImageData _imgData;
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }

       
        private ProjectLocation _projectLocation;

        public ProjectLocation ProjectLocation
        {
            get { return _projectLocation; }
            set { _projectLocation = value;OnPropertyChanged("ProjectLocation"); }
        }

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
                await Shell.Current.Navigation.PopAsync() ;

            }
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }

        private async Task Save()
        {
            if (string.IsNullOrEmpty(ProjectLocation.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Location name is required", "OK");
                return;
            }
            IsBusyProgress = true;

            Response result = await Task.Run(Running);
            if (result.Status == ApiResult.Success)
            {
                IsBusyProgress = false;
               //  await Shell.Current.Navigation.PopAsync().ConfigureAwait(false);
                 ProjectLocation location = JsonConvert.DeserializeObject<ProjectLocation>(result.Data.ToString());
                // DependencyService.Get<ILodingPageService>().HideLoadingPage();
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
                await Shell.Current.Navigation.PopAsync();


                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectLocationDetail))
                    await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail() { BindingContext = new ProjectLocationDetailViewModel() { ProjectLocation = location } }).ConfigureAwait(false); ;
              
                //await Shell.Current.Navigation.PopAsync().ConfigureAwait(true); ;
            }
            
        }

        private async Task<Response> Running()
        {
            Response result = new Response();
            if (string.IsNullOrEmpty(ProjectLocation.Id))
            {

                ProjectLocation.ProjectId = Project.Id;
                 result = await ProjectLocationDataStore.AddItemAsync(ProjectLocation);


                //ProjectLocation = JsonConvert.DeserializeObject<ProjectLocation>(result.Data.ToString());

                //return await Task.FromResult(true);
            }
            else
            {
                 result = await ProjectLocationDataStore.AddItemAsync(ProjectLocation);
                //ProjectLocation = JsonConvert.DeserializeObject<ProjectLocation>(result.Data.ToString());

                ////await ProjectLocationDataStore.AddItemAsync(ProjectLocation);
                //return await Task.FromResult(true);
            }
            return await Task.FromResult(result);

        }
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
        }
        public ProjectLocationAddEditViewModel()
        {

            
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
        public ICommand DeleteCommand => new Command(async () => await Delete());
        private async Task Delete()
        {
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Are you sure you want to remove?",
                "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                var response = await Task.Run(() =>
                     ProjectLocationDataStore.DeleteItemAsync(ProjectLocation)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopToRootAsync();
                }
              //  await ProjectLocationDataStore.DeleteItemAsync(ProjectLocation);
               // await Shell.Current.Navigation.PopToRootAsync();
               // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

            }
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
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
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
                ImgData.Path = SelectedImage;
                ImgData.Name = ProjectLocation.ImageName;
                ImgData.Description = ProjectLocation.ImageDescription;
                await Current.EditImage(ImgData, testphoto);
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
                });
            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;

        }
        private async Task<string> TakePictureFromCamera()
        {
            IsBusy = true;
            var file = await CrossMedia.Current.TakePhotoAsync
                (new StoreCameraMediaOptions()
                {
                    //  SaveToAlbum = true,
                    SaveMetaData = false,
                    DefaultCamera = CameraDevice.Rear,
                    CompressionQuality = App.CompressionQuality
                });

            IsBusy = false;
            if (file == null)
                return null;
            //   return file.Path;
            if (Device.RuntimePlatform == Device.iOS)
            {

                byte[] arr = null;
                var buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = 0;
                    var readstream = file.GetStreamWithImageRotatedForExternalStorage();
                    while ((read = readstream.Read(buffer, 0, buffer.Length)) > 0)
                        ms.Write(buffer, 0, read);

                    file.GetStream().CopyTo(ms);

                    //  file.Dispose();
                    readstream.Dispose();
                    arr = ms.ToArray();
                    readstream = null;
                }
                string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(Guid.NewGuid().ToString(), arr);
                //  ImgData.mediaFile = arr;
                return filepath;
                //   byte[] arr = null;
                //  using (MemoryStream ms = new MemoryStream())
                // {
                //     file.GetStream().CopyTo(ms);
                //     file.Dispose();
                //     arr = ms.ToArray();
                //  }
                // string filepath = await DependencyService.Get<ISaveFile>().SaveFilesForCameraApi(Guid.NewGuid().ToString(), arr);
                //ImgData.mediaFile = file;
                // return filepath;
            }
            else
            {


                return file.Path;

            }


        }
        private string _imgPath;

        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private  void testphoto(ImageData ImgData)
        {
            ProjectLocation.ImageName = ImgData.Name;
            ProjectLocation.ImageDescription = ImgData.Description;
            ProjectLocation.ImageUrl = ImgData.Path;
           // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }

    }
   
}
