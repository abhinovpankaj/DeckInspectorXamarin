using ImageEditor.ViewModels;
using Mobile.Code.Models;
using Mobile.Code.Services.SQLiteLocal;
using Mobile.Code.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
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
            set { _projectLocation = value; OnPropertyChanged("ProjectLocation"); }
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
                await Shell.Current.Navigation.PopAsync();

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
            ProjectLocation location;
            Response result = await Task.Run(Running);
            if (result.Status == ApiResult.Success)
            {
                IsBusyProgress = false;
                if (App.IsAppOffline)
                {
                    location = (ProjectLocation)result.Data;
                }
                else
                    location = JsonConvert.DeserializeObject<ProjectLocation>(result.Data.ToString());
                
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
            }

            if (App.IsAppOffline)
            {
                result = await ProjectLocationSqLiteDataStore.AddItemAsync(ProjectLocation);
            }
            else
                result = await ProjectLocationDataStore.AddItemAsync(ProjectLocation);
            
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

            ImgData = new ImageData();
        }
        public ICommand DeleteCommand => new Command(async () => await Delete());
        private async Task Delete()
        {
            Response response;
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Are you sure you want to remove?",
                "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                if (App.IsAppOffline)
                {
                    response = await Task.Run(() =>
                     ProjectLocationSqLiteDataStore.DeleteItemAsync(ProjectLocation));
                }
                else
                     response = await Task.Run(() =>
                     ProjectLocationDataStore.DeleteItemAsync(ProjectLocation));
                
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                
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
                    SaveMetaData = false,
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
                    SaveMetaData = true,
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

        private void testphoto(ImageData ImgData)
        {
            ProjectLocation.ImageName = ImgData.Name;
            ProjectLocation.ImageDescription = ImgData.Description;
            ProjectLocation.ImageUrl = ImgData.Path;
            // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }

    }
   
}
