using ImageEditor.ViewModels;
using Mobile.Code.Models;

using Mobile.Code.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{

    [QueryProperty("Id", "Id")]
    public class ProjectBuildingAddEditViewModel : BaseViewModel
    {
        private ProjectBuilding _projecBilding;


        public ProjectBuilding ProjectBuilding
        {
            get { return _projecBilding; }
            set { _projecBilding = value; OnPropertyChanged("ProjectBuilding"); }
        }

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
            }
        }
      
        private async Task Save()
        {
            if (string.IsNullOrEmpty(ProjectBuilding.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Location name is required", "OK");
                return;
            }
            IsBusyProgress = true;

            Response result = await Task.Run(Running);
            if (result.Status == ApiResult.Success)
            {
                IsBusyProgress = false;
                if (App.IsAppOffline)
                {
                    ProjectBuilding = (ProjectBuilding)result.Data;
                }
                else
                {
                    if (!string.IsNullOrEmpty(result.ID))
                    {
                        ProjectBuilding.Id = result.ID.ToString();
                    }

                }

                await Shell.Current.Navigation.PopAsync();


                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectBuildingDetail))
                    await Shell.Current.Navigation.PushAsync(new ProjectBuildingDetail() { BindingContext = new ProjectBuildingDetailViewModel() { ProjectBuilding = ProjectBuilding } });
               
            }
            

        }

        private async Task<Response> Running()
        {
            Response result = new Response();
            if (string.IsNullOrEmpty(ProjectBuilding.Id))
            {
                ProjectBuilding.ProjectId = Project.Id;
            }
          
            if (App.IsAppOffline)
            {
                result = await ProjectBuildingSqLiteDataStore.AddItemAsync(ProjectBuilding);
            }
            else
                result = await ProjectBuildingDataStore.AddItemAsync(ProjectBuilding);
            
            return await Task.FromResult(result);

        }
        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
        }
        public ProjectBuildingAddEditViewModel()
        {
            ProjectBuilding = new ProjectBuilding();

            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());

            ImgData = new ImageData();
        }
        public void Load()
        {
            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    Heading = "New Project";
                }
                else
                {
                    Heading = "Project Name";
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
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
                ImgData.Name = ProjectBuilding.ImageName;
                ImgData.Description = ProjectBuilding.ImageDescription;

                ImgData.Path = SelectedImage;
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

        public string ImgPatah
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private void testphoto(ImageData ImgData)
        {
            ProjectBuilding.ImageName = ImgData.Name;
            ProjectBuilding.ImageDescription = ImgData.Description;
            ProjectBuilding.ImageUrl = ImgData.Path;
            // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }


    }

}
