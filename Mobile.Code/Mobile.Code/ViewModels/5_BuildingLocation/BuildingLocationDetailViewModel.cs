using Mobile.Code.Models;
using Mobile.Code.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Mobile.Code.Data;
using System.Windows.Input;
using ImageEditor.ViewModels;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Mobile.Code.Views;
using System.Linq;
using Mobile.Code.Media;
using Mobile.Code.Services;
using Newtonsoft.Json;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("Id", "Id")]
    //[QueryProperty("Title", "Id")]
    public class BuildingLocationDetailViewModel : BaseViewModel
    {

        public ICommand GoHomeCommand => new Command(async () => await GoHome());
        private async Task GoHome()
        {
            Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2]);
            await Shell.Current.Navigation.PopAsync();

        }

        private Project _project;

        public Project project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("project"); }
        }

        private BuildingLocation buildingLocation;

        public BuildingLocation BuildingLocation
        {
            get { return buildingLocation; }
            set { buildingLocation = value; OnPropertyChanged("BuildingLocation"); }
        }

        public ICommand ChoosePhotoCommand { get; set; }

        public ObservableCollection<BuildingCommonLocationImages> _bcimage { get; set; }

        public ObservableCollection<BuildingCommonLocationImages> BuildingCommonLocationImages
        {
            get { return _bcimage; }
            set { _bcimage = value; OnPropertyChanged("BuildingCommonLocationImages"); }
        }
        public ObservableCollection<WorkImage> Items { get; set; }

        private BuildingCommonLocationImages _buildingCommonLocationImages;

        public BuildingCommonLocationImages BuildingCommonLocationImage
        {
            get { return _buildingCommonLocationImages; }
            set { _buildingCommonLocationImages = value; OnPropertyChanged("BuildingCommonLocationImages"); }
        }

        public Command ImageDetailCommand { get; set; }
        async Task ExecuteImageDetailCommand(BuildingCommonLocationImages parm)
        {
            BuildingCommonLocationImage = parm;
            ImgData.Path = parm.ImageUrl;
         
            ImgData.BuildingCommonLocationImages = parm;
            ImgData.FormType = "B";
            await CurrentWithoutDetail.EditImage(ImgData, GetImageFromEditor);
          //  await Shell.Current.Navigation.PushAsync(new EditBuildingLocationImage() { BindingContext = new EditBuildingLocationImageViewModel() { Title = "Edit Building Common Location Image", BuildingCommonLocationImages = parm, BuildingLocation = BuildingLocation } });

            // await App.Current.MainPage.Navigation.PushModalAsync(new ShowImage() { BindingContext = new ShowImageViewModel(parm.Image,parm.Name,parm.Description,parm.CreatedOn) });

        }
        private async void GetImageFromEditor(ImageData ImgData)
        {
            BuildingCommonLocationImage.ImageUrl = ImgData.Path;
            await BuildingCommonLocationImagesDataStore.UpdateItemAsync(BuildingCommonLocationImage);
        }
        public ICommand DeleteImagCommand => new Command<BuildingCommonLocationImages>(async (BuildingCommonLocationImages obj) => await DeleteImagCommandExecute(obj));
        private async Task DeleteImagCommandExecute(BuildingCommonLocationImages obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                var response = await Task.Run(() =>
                      BuildingCommonLocationImagesDataStore.DeleteItemAsync(obj)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }
               
                // Shell.Current.Navigation.RemovePage(new BuildingLocationDetail());
               
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

            }
        }
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }
        private async Task GoBack()
        {
            //await Shell.Current.GoToAsync("ProjectBuildingDetail");
            await Shell.Current.Navigation.PopAsync(); 
        }
        private async Task Save()
        {
             await Task.FromResult(true);
           // await App.Current.MainPage.Navigation.PushAsync(new ProjectLocationDetail());

        }
        
        public Command EditCommand { get; set; }
        private async Task Edit()
        {
            await Shell.Current.Navigation.PushAsync(new AddBuildingLocation() { BindingContext = new BuildingLocationAddEditViewModel() { Title = "Edit Building Common Location", BuildingLocation = BuildingLocation } });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
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
                   BuildingLocationDataStore.DeleteItemAsync(BuildingLocation)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }


            }
        }
        public BuildingLocationDetailViewModel()
        {
            if (App.ReportType == ReportType.Visual)
            {
                IsViusalReport = true;
                IsFinelOrInvasiveReport = false;
            }
            else
            {
                IsViusalReport = false;
                IsFinelOrInvasiveReport = true;
            }
            EditCommand = new Command(async () => await Edit());
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            ImageDetailCommand = new Command<BuildingCommonLocationImages>(async (BuildingCommonLocationImages parm) => await ExecuteImageDetailCommand(parm));
            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            ImgData = new ImageData();
           

        }
        public string SelectedImage { get; set; }
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
                await Current.EditImage(ImgData, GetImageDetail);
            }
        }

        private ImageData _imgData;

        public ImageData ImgData
        {
            get { return _imgData; }
            set { _imgData = value; }
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
                    SaveMetaData = true,
                    DefaultCamera = CameraDevice.Rear,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    CompressionQuality = App.CompressionQuality
                });

            IsBusy = false;
            if (file == null)
                return null;

            return file.Path;
        }
        private string _imgPath;

        public string ImgPatah
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }

        private void GetImageDetail(ImageData ImgData)
        {
            ImageData data = ImgData;
             BuildingCommonLocationImages _locImage = new BuildingCommonLocationImages();
            _locImage.Id = Guid.NewGuid().ToString();
            _locImage.ImageUrl = data.Path;
            _locImage.ImageName = data.Name;
            _locImage.ImageDescription = data.Description;
            _locImage.BuildingLocationId = buildingLocation.Id;

            BuildingCommonLocationImagesDataStore.AddItemAsync(_locImage);
            Task.Run(() => this.LoadData()).Wait();
           // LoadData();
            //ImgPatah = ImgData.Path;
            // await App.Current.MainPage.DisplayAlert(ImgData.Name, ImgData.Path, "ok", "cancel");
        }
        public ICommand NewImagCommand => new Command(async () => await NewImage());
        private async Task NewImage()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
                new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { BuildingLocation = BuildingLocation, IsBuildingLocation = true } });
                    break;
                case "From Gallery":
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                        bool imageModifiedWithDrawings = false;
                        if (imageModifiedWithDrawings)
                        {
                            await GMMultiImagePicker.Current.PickMultiImage(true);
                        }
                        else
                        {
                            await GMMultiImagePicker.Current.PickMultiImage();
                        }

                        //  MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                        {
                            //If we have selected images, put them into the carousel view.
                            //if (images.Count > 0)
                            //{
                            //    foreach (var item in images)
                            //    {
                            //        BuildingCommonLocationImages obj = new BuildingCommonLocationImages() { ImageUrl = item, Id = Guid.NewGuid().ToString(), BuildingId = BuildingLocation.Id, DateCreated = DateTime.Now };
                            //        _ = AddNewPhoto(obj);
                            //    }
                            //}
                            if (images.Count > 0)
                            {
                                IsBusyProgress = true;

                                Task.Run(() => UploadGallary(images));

                            }
                        });
                    }
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        DependencyService.Get<IMediaService>().OpenGallery();
                        MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                        {
                            if (images.Count > 0)
                            {
                                IsBusyProgress = true;

                                Task.Run(() => UploadGallary(images));

                            }
                           
                        });
                    }
                    break;
                default:
                    break;
            }
            
        }
        public async Task<bool> UploadGallary(List<string> images)
        {

            IsBusyProgress = true;
            bool result = HttpUtil.UploadFromGallary(BuildingLocation.Name, "/api/BuildingLocationImage/AddEdit?ParentId=" + BuildingLocation.Id + "&UserId=" + App.LogUser.Id.ToString(), images);
            if (result == true)
            {

                IsBusyProgress = false;
            }
            BuildingCommonLocationImages = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(BuildingLocation.Id));
            return await Task.FromResult(true);


        }
        public async Task AddNewPhoto(BuildingCommonLocationImages obj)
        {

            await BuildingCommonLocationImagesDataStore.AddItemAsync(obj);
            BuildingCommonLocationImages = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(BuildingLocation.Id));
            // UnitPhotoCount = VisualApartmentLocationPhotoItems.Count.ToString();
        }
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        private bool _isEditDeleteAccess;

        public bool IsEditDeleteAccess
        {
            get { return _isEditDeleteAccess; }
            set { _isEditDeleteAccess = value; OnPropertyChanged("IsEditDeleteAccess"); }
        }
        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }
        private async Task<bool> Running()
        {
           
            if (BuildingLocation != null)
            {

                BuildingLocation = await BuildingLocationDataStore.GetItemAsync(BuildingLocation.Id).ConfigureAwait(true);
               // BuildingCommonLocationImages = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(BuildingLocation.Id));

                VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationDataStore.GetItemsAsyncByBuildingLocationId(BuildingLocation.Id));
                if (App.LogUser.RoleName == "Admin")
                {

                    IsEditDeleteAccess = true;
                }
                else if (BuildingLocation.UserId == App.LogUser.Id.ToString())
                {

                    IsEditDeleteAccess = true;
                }
                if (App.IsInvasive)
                {
                    IsInvasiveControlDisable = true;
                    IsEditDeleteAccess = false;
                }
            }
            return await Task.FromResult(true);
        }
        public async Task<bool> LoadData()
        {
            IsBusyProgress = true;
            bool complete = await Task.Run(Running).ConfigureAwait(false);
            if (complete == true)
            {



                IsBusyProgress = false;


            }
            return await Task.FromResult(true);

        }

        private bool _isVisualReport = false;

        public bool IsViusalReport
        {
            get { return _isVisualReport; }
            set { _isVisualReport = value; OnPropertyChanged("IsViusalReport"); }
        }

        private bool _isFinelOrInvasiveReport = false;
        public bool IsFinelOrInvasiveReport
        {
            get { return _isFinelOrInvasiveReport; }
            set { _isFinelOrInvasiveReport = value; OnPropertyChanged("IsFinelOrInvasiveReport"); }
        }
        public ICommand NewViusalReportCommand => new Command(async () => await NewViusalReportCommandExecue());

        private async Task NewViusalReportCommandExecue()
        {
            BuildingLocation_Visual visualForm = new BuildingLocation_Visual();
            visualForm = new BuildingLocation_Visual();
            //visualForm.Id = Guid.NewGuid().ToString();
            visualForm.BuildingLocationId = BuildingLocation.Id;
            VisualBuildingLocationPhotoDataStore.Clear();

            App.VisualEditTracking = new List<MultiImage>();
            App.VisualEditTrackingForInvasive = new List<MultiImage>();
            VisualBuildingLocationPhotoDataStore.Clear();
            InvasiveVisualBuildingLocationPhotoDataStore.Clear();

            App.FormString = JsonConvert.SerializeObject(visualForm);
            App.IsNewForm = true;
            //await VisualFormProjectLocationDataStore.AddItemAsync(vloc);
            //  VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>(await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            await Shell.Current.Navigation.PushAsync(new VisualBuildingLocationForm() { BindingContext = new VisualBuildingLocationFormViewModel() { BuildingLocation = BuildingLocation, VisualForm = visualForm } });
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        private ObservableCollection<BuildingLocation_Visual> _visualFormBuildingLocationItems;

        public ObservableCollection<BuildingLocation_Visual> VisualFormBuildingLocationItems
        {
            get { return _visualFormBuildingLocationItems; }
            set { _visualFormBuildingLocationItems = value; OnPropertyChanged("VisualFormBuildingLocationItems"); }
        }
        public ICommand GoToVisualFormCommand => new Command<BuildingLocation_Visual>(async (BuildingLocation_Visual parm) => await GoToVisualForm(parm));
        private async Task GoToVisualForm(BuildingLocation_Visual parm)
        {
            App.IsNewForm = false;
            VisualBuildingLocationFormViewModel vm = new VisualBuildingLocationFormViewModel();
            vm.ExteriorElements = new ObservableCollection<string>(parm.ExteriorElements.Split(',').ToList());
            vm.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());
            vm.CountExteriorElements = vm.ExteriorElements.Count.ToString();
            vm.CountWaterProofingElements = vm.WaterProofingElements.Count.ToString();
            vm.RadioList_VisualReviewItems.Where(c => c.Name == parm.VisualReview).Single().IsSelected = true;
            vm.RadioList_AnyVisualSignItems.Where(c => c.Name == parm.AnyVisualSign).Single().IsSelected = true;
            vm.RadioList_FurtherInasiveItems.Where(c => c.Name == parm.FurtherInasive).Single().IsSelected = true;
            vm.RadioList_ConditionAssessment.Where(c => c.Name == parm.ConditionAssessment).Single().IsSelected = true;
            vm.RadioList_LifeExpectancyEEE.Where(c => c.Name == parm.LifeExpectancyEEE).Single().IsSelected = true;
            vm.RadioList_LifeExpectancyLBC.Where(c => c.Name == parm.LifeExpectancyLBC).Single().IsSelected = true;
            vm.RadioList_LifeExpectancyAWE.Where(c => c.Name == parm.LifeExpectancyAWE).Single().IsSelected = true;

            if (App.IsInvasive)
            {
                if (parm.IsPostInvasiveRepairsRequired)
                {
                    vm.RadioList_ConclusiveLifeExpectancyEEE.Where(c => c.Name == parm.ConclusiveLifeExpEEE).Single().IsSelected = true;
                    vm.RadioList_ConclusiveLifeExpectancyLBC.Where(c => c.Name == parm.ConclusiveLifeExpLBC).Single().IsSelected = true;
                    vm.RadioList_ConclusiveLifeExpectancyAWE.Where(c => c.Name == parm.ConclusiveLifeExpAWE).Single().IsSelected = true;

                    string isChked = parm.IsInvasiveRepairApproved ? "Yes" : "No";
                    vm.RadioList_OwnerAgreedToRepair.Where(c => c.Name == isChked).Single().IsSelected = true;
                    isChked = parm.IsInvasiveRepairComplete ? "Yes" : "No";
                    vm.RadioList_RepairComplete.Where(c => c.Name == isChked).Single().IsSelected = true;
                }
            }


            App.VisualEditTracking = new List<MultiImage>();
            App.VisualEditTrackingForInvasive = new List<MultiImage>();
            VisualBuildingLocationPhotoDataStore.Clear();
            InvasiveVisualBuildingLocationPhotoDataStore.Clear();
           
            
            vm.VisualForm = parm;
            vm.BuildingLocation = BuildingLocation;

            App.FormString = JsonConvert.SerializeObject(vm.VisualForm);
            vm.VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true));
            if (App.IsInvasive == false)
            {
                

                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(VisualBuildingLocationForm))
                {
                    await Shell.Current.Navigation.PushAsync(new VisualBuildingLocationForm() { BindingContext = vm });
                }
            }
            else
            {
                var photos = await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true);
                vm.InvasiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x=>x.ImageDescription=="TRUE"));
                vm.ConclusiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                App.InvaiveImages = JsonConvert.SerializeObject(vm.InvasiveVisualBuildingLocationPhotoItems);
                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(Views._8_VisualReportForm.TabbedPageInvasive))
                    await Shell.Current.Navigation.PushAsync(new Views._8_VisualReportForm.TabbedPageInvasive() { BindingContext = vm });
            }
                //vm.WaterProofingElements.selectedList = parm.ExteriorElements.Split(',').ToList();
               
          
            //await Shell.Current.Navigation.PushAsync(new EditProjectLocationImage()
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        public ICommand DeleteVisualFormCommand => new Command<BuildingLocation_Visual>(async (BuildingLocation_Visual obj) => await DeleteVisualFormCommandExecute(obj));
        private async Task DeleteVisualFormCommandExecute(BuildingLocation_Visual obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                var response = await Task.Run(() =>
                    VisualFormBuildingLocationDataStore.DeleteItemAsync(obj)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }
                 
                // Shell.Current.Navigation.RemovePage(new BuildingLocationDetail());
               
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

            }
        }
    }

}
