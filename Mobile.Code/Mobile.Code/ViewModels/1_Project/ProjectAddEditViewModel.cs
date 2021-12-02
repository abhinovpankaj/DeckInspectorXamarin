using ImageEditor.ViewModels;
using Mobile.Code.Models;
using Mobile.Code.Views;
using Mobile.Code.Views._3_ProjectLocation;
using Newtonsoft.Json;
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
    public class ProjectAddEditViewModel : BaseViewModel
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
                if (!string.IsNullOrEmpty(Project.Id))
                {
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Shell.Current.GoToAsync("//main");
                }
                // await Shell.Current.Navigation.Cle ;
            }
        }
        private Project project;

        public Project Project
        {
            get { return project; }
            set { project = value; OnPropertyChanged("Project"); }
        }

        private async Task Save()
        {
            if (string.IsNullOrEmpty(Project.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Project name is required", "OK");
                return;
            }
            IsBusyProgress = true;
            //   DependencyService.Get<ILodingPageService>().InitLoadingPage(new LoadingIndicatorPage1());
            // DependencyService.Get<ILodingPageService>().ShowLoadingPage();
            bool complete = await Task.Run(Running);
            if (complete == true)
            {
                IsBusyProgress = false;
                if (Project.Category== "SingleLevel")
                {
                    await Shell.Current.Navigation.PushAsync(new SingleLevelProjectLocation()
                    { BindingContext = new SingleLevelProjectDetailViewModel() { Project = Project } }).ConfigureAwait(false);
                }
                else
                    await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
            }

        }
        private async Task<bool> Running()
        {
            Response result;
            if (string.IsNullOrEmpty(Project.Id))
            {

                
                Project.ProjectType = ProjectType;

                Project.Category = ProjectCategory;
                if (App.IsAppOffline)
                {
                    result = await ProjectSQLiteDataStore.AddItemAsync(Project);

                    Project = (Project)result.Data;
                }
                else
                {
                    result = await ProjectDataStore.AddItemAsync(Project);
                    Project = JsonConvert.DeserializeObject<Project>(result.Data.ToString());
                }
                    


                return await Task.FromResult(true);
            }
            else
            {
                Project.ProjectType = ProjectType;

                Project.Category = ProjectCategory;
                if (App.IsAppOffline)
                {
                    await ProjectSQLiteDataStore.UpdateItemAsync(project);
                }
                else
                    await ProjectDataStore.AddItemAsync(project);
                
                return await Task.FromResult(true);
            }


        }


        private string projectType;

        public string ProjectType
        {
            get { return projectType; }
            set { projectType = value; OnPropertyChanged("ProjectType"); }
        }

        private string projectCat;
        public string ProjectCategory
        {
            get { return projectCat; }
            set { projectCat = value; OnPropertyChanged("ProjectCategory"); }
        }

        public ProjectAddEditViewModel()
        {

            Project = new Project();
            project.ProjectType = ProjectType;
            project.Category = ProjectCategory;
            Project.ImageUrl = "blank.png";
            Title = "New Project";
            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());


            ImgData = new ImageData();
            IsBusyProgress = false;
        }
        public void Load()
        {
            IsBusyProgress = true;
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

                ImgData.Name = Project.ImageName;
                ImgData.Description = Project.ImageDescription;
                ImgData.Path = SelectedImage;
                await Current.EditImage(ImgData, testphoto);
            }
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
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
                    

                }); ;

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;

        }
        public static string GetFileSize(long length)
        {

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

        public string ImgPata
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private void testphoto(ImageData ImgData)
        {
            Project.ImageName = ImgData.Name;
            Project.ImageDescription = ImgData.Description;


            Project.ImageUrl = ImgData.Path;

            // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }

    }

}
