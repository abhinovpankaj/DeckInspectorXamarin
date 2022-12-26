using ImageEditor.ViewModels;
using Mobile.Code.Media;
using Mobile.Code.Models;
using Mobile.Code.Services;
using Mobile.Code.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("Id", "Id")]
    //[QueryProperty("Title", "Id")]
    public class BuildingLocationDetailViewModel : BaseViewModel
    {

        public ICommand GoHomeCommand => new Command(async () => await GoHome());
        private async Task GoHome()
        {
            await Shell.Current.Navigation.PopToRootAsync();

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
                Response response = new Response();
                IsBusyProgress = true;
                if (App.IsAppOffline)
                {
                     response = await Task.Run(() =>
                   BuildingLocationSqLiteDataStore.DeleteItemAsync(BuildingLocation));
                }
                else
                     response = await Task.Run(() =>
                       BuildingLocationDataStore.DeleteItemAsync(BuildingLocation));

                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }


            }
        }
        private int currentLocationSeq;
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

            Task.Run(() =>
            {
                //MessagingCenter.Unsubscribe<VisualBuildingLocationFormViewModel, string>(this, "LocationSwipped");

                MessagingCenter.Subscribe<VisualBuildingLocationFormViewModel, string>(this, "LocationSwipped", async (sender, arg) =>
            {
                BuildingLocation_Visual currentVisualLocation = null;

                if (arg == "Right")
                {
                    if (currentLocationSeq + 1 < VisualFormBuildingLocationItems.Count)
                    {
                        currentVisualLocation = VisualFormBuildingLocationItems[currentLocationSeq + 1];
                    }

                }
                else
                {
                    if (currentLocationSeq - 1 >= 0)
                    {
                        currentVisualLocation = VisualFormBuildingLocationItems[currentLocationSeq - 1];
                    }
                }

                try
                {
                    if (currentVisualLocation != null)
                        await GoToVisualForm(currentVisualLocation, true);
                    else
                    {
                        var _lastPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                        if (_lastPage.GetType() == typeof(VisualBuildingLocationForm))
                        {
                            await Shell.Current.Navigation.PopAsync();
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            });
            });
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
            //Task.Run(() => this.LoadData()).Wait();
            
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
                        bool imageModifiedWithDrawings = true;
                        if (imageModifiedWithDrawings)
                        {
                            await GMMultiImagePicker.Current.PickMultiImage(true);
                        }
                        else
                        {
                            await GMMultiImagePicker.Current.PickMultiImage();
                        }

                         MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                        {
                           
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
        public async Task<bool> Running(CancellationToken token)
        {
            IsBusyProgress = true;
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
                IsBusyProgress = false;
            }
            if (App.IsAppOffline)
            {
                IsEditDeleteAccess = true;
                //await Task.Run(async ()=>
                //{
                    BuildingLocation = await BuildingLocationSqLiteDataStore.GetItemAsync(BuildingLocation.Id);

                    VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationSqLiteDataStore.GetItemsAsyncByBuildingLocationId(BuildingLocation.Id));
                //});
                
            }
            else
            {
                if (BuildingLocation != null)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                        IsBusyProgress = false;
                    }
                    //await Task.Run(async () =>
                    //{
                        BuildingLocation = await BuildingLocationDataStore.GetItemAsync(BuildingLocation.Id).ConfigureAwait(true);
                        // BuildingCommonLocationImages = new ObservableCollection<BuildingCommonLocationImages>(await BuildingCommonLocationImagesDataStore.GetItemsAsyncByBuildingId(BuildingLocation.Id));
                        VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationDataStore.GetItemsAsyncByBuildingLocationId(BuildingLocation.Id));
                    //});

                    
                    if (App.LogUser.RoleName == "Admin")
                    {

                        IsEditDeleteAccess = true;
                    }
                    else if (BuildingLocation.UserId == App.LogUser.Id.ToString())
                    {

                        IsEditDeleteAccess = true;
                    }
                   
                }
            }
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
                IsEditDeleteAccess = false;
            }
            IsBusyProgress = false;
            return true;
        }
        public async Task<bool> LoadData()
        {
            IsBusyProgress = true;
            bool complete = await Task.Run(() => Running(new CancellationTokenSource().Token));
            if (complete == true)
            {

                IsBusyProgress = false;
            }
            return true;

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
            IsBusyProgress = true;
            BuildingLocation_Visual visualForm = new BuildingLocation_Visual();
            visualForm = new BuildingLocation_Visual();
            //visualForm.Id = Guid.NewGuid().ToString();
            visualForm.BuildingLocationId = BuildingLocation.Id;
            

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
            IsBusyProgress = false;
        }

        private ObservableCollection<BuildingLocation_Visual> _visualFormBuildingLocationItems;

        public ObservableCollection<BuildingLocation_Visual> VisualFormBuildingLocationItems
        {
            get { return _visualFormBuildingLocationItems; }
            set { _visualFormBuildingLocationItems = value; OnPropertyChanged("VisualFormBuildingLocationItems"); }
        }
        public ICommand GoToVisualFormCommand => new Command<BuildingLocation_Visual>(async (BuildingLocation_Visual parm) => await GoToVisualForm(parm));
        private async Task GoToVisualForm(BuildingLocation_Visual parm, bool isSwipped = false)
        {
            App.IsNewForm = false;
            IsBusyProgress = true;
            await Task.Run(async () =>
            {
                currentLocationSeq = VisualFormBuildingLocationItems.IndexOf(parm);
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
                        if (parm.IsInvasiveRepairApproved)
                        {
                            if (parm.IsInvasiveRepairComplete)
                            {
                                if (parm.ConclusiveLifeExpEEE.Length > 0)
                                {
                                    vm.RadioList_ConclusiveLifeExpectancyEEE.Single(c => c.Name == parm.ConclusiveLifeExpEEE).IsSelected = true;
                                    vm.RadioList_ConclusiveLifeExpectancyLBC.Single(c => c.Name == parm.ConclusiveLifeExpLBC).IsSelected = true;
                                    vm.RadioList_ConclusiveLifeExpectancyAWE.Single(c => c.Name == parm.ConclusiveLifeExpAWE).IsSelected = true;
                                }

                            }
                            string isChked = parm.IsInvasiveRepairComplete ? "Yes" : "No";
                            vm.RadioList_RepairComplete.Single(c => c.Name == isChked).IsSelected = true;

                        }

                        string isChked1 = parm.IsInvasiveRepairApproved ? "Yes" : "No";
                        vm.RadioList_OwnerAgreedToRepair.Single(c => c.Name == isChked1).IsSelected = true;
                    }
                }


                App.VisualEditTracking = new List<MultiImage>();
                App.VisualEditTrackingForInvasive = new List<MultiImage>();
                VisualBuildingLocationPhotoDataStore.Clear();
                InvasiveVisualBuildingLocationPhotoDataStore.Clear();


                vm.VisualForm = parm;
                vm.BuildingLocation = BuildingLocation;

                App.FormString = JsonConvert.SerializeObject(vm.VisualForm);
                if (App.IsAppOffline)
                {
                    vm.VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(parm.Id, false));
                }
                else
                    vm.VisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true));


                if (App.IsInvasive == false)
                {
                    if (isSwipped)
                    {
                        var _lastPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                        await Shell.Current.Navigation.PushAsync(new VisualBuildingLocationForm() { BindingContext = vm });
                        if (_lastPage.GetType() == typeof(VisualBuildingLocationForm))
                            Shell.Current.Navigation.RemovePage(_lastPage);
                    }
                    else
                    {
                        if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(VisualBuildingLocationForm))
                        {
                            await Shell.Current.Navigation.PushAsync(new VisualBuildingLocationForm() { BindingContext = vm });
                        }
                    }

                }
                else
                {
                    IEnumerable<VisualBuildingLocationPhoto> photos;
                    if (App.IsAppOffline)
                    {
                        photos = await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false);
                    }
                    else
                        photos = await InvasiveVisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true);


                    vm.InvasiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                    vm.ConclusiveVisualBuildingLocationPhotoItems = new ObservableCollection<VisualBuildingLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                    App.InvaiveImages = JsonConvert.SerializeObject(vm.InvasiveVisualBuildingLocationPhotoItems);

                    if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                        await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive(vm));
                }
            });            
            
            IsBusyProgress = false;
        }

        public ICommand DeleteVisualFormCommand => new Command<BuildingLocation_Visual>(async (BuildingLocation_Visual obj) => await DeleteVisualFormCommandExecute(obj));
        private async Task DeleteVisualFormCommandExecute(BuildingLocation_Visual obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");
            Response response;
            if (result)
            {
                IsBusyProgress = true;
                if (App.IsAppOffline)
                {
                    response = await Task.Run(() =>
                    VisualFormBuildingLocationSqLiteDataStore.DeleteItemAsync(obj));
                }
                else
                    response = await Task.Run(() =>
                    VisualFormBuildingLocationDataStore.DeleteItemAsync(obj));
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }

              

            }
        }
    }

}
