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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("Id", "Id")]
    //[QueryProperty("Title", "Id")]
    public class BuildingApartmentDetailViewModel : BaseViewModel
    {
        public ICommand GoHomeCommand => new Command(async () => await GoHome());
        private async Task GoHome()
        {
            Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2]);
            await Shell.Current.Navigation.PopAsync();

        }
        private BuildingApartment _app;

        public BuildingApartment BuildingApartment
        {
            get { return _app; }
            set { _app = value; OnPropertyChanged("BuildingApartment"); }
        }
        public ICommand ChoosePhotoCommand { get; set; }

        public ObservableCollection<BuildingApartmentImages> _bcimage { get; set; }

        public ObservableCollection<BuildingApartmentImages> BuildingApartmentImages
        {
            get { return _bcimage; }
            set { _bcimage = value; OnPropertyChanged("BuildingApartmentImages"); }
        }
        

        public Command ImageDetailCommand { get; set; }
        private async void GetImageFromEditor(ImageData ImgData)
        {
            BuildingApartmentImage.ImageUrl = ImgData.Path;
            await BuildingApartmentImagesDataStore.UpdateItemAsync(BuildingApartmentImage);
        }
        async Task ExecuteImageDetailCommand(BuildingApartmentImages parm)
        {

            BuildingApartmentImage = parm;
            ImgData.Path = parm.ImageUrl;

            ImgData.BuildingApartmentImages = parm;
            ImgData.FormType = "A";
            await CurrentWithoutDetail.EditImage(ImgData, GetImageFromEditor);

        }

        public ICommand DeleteImagCommand => new Command<BuildingApartmentImages>(async (BuildingApartmentImages obj) => await DeleteImagCommandExecute(obj));
        private async Task DeleteImagCommandExecute(BuildingApartmentImages obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");

            if (result)
            {

                IsBusyProgress = true;
                var response = await Task.Run(() =>
                    BuildingApartmentImagesDataStore.DeleteItemAsync(obj)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }

            }
        }
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopAsync();

        }

        private async Task Save()
        {
            await Task.FromResult(true);
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }

        public Command EditCommand { get; set; }
        private async Task Edit()
        {
            await Shell.Current.Navigation.PushAsync(new AddBuildingApartment() { BindingContext = new BuildingAprartmentAddEditViewModel() { Title = "Edit Building Apartment", BuildingApartment = BuildingApartment } });
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
                Response response = new Response();
                if (App.IsAppOffline)
                {
                     response = await Task.Run(() =>
                    BuildingApartmentSqLiteDataStore.DeleteItemAsync(BuildingApartment));
                }
                else
                     response = await Task.Run(() => BuildingApartmentDataStore.DeleteItemAsync(BuildingApartment));
                
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }

            }
        }
        public BuildingApartmentDetailViewModel()
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
            ImageDetailCommand = new Command<BuildingApartmentImages>(async (BuildingApartmentImages parm) => await ExecuteImageDetailCommand(parm));
            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            ImgData = new ImageData();


        }
        public ICommand NewImagCommand => new Command(async () => await NewImage());
        private async Task NewImage()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
               new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { BuildingApartment = BuildingApartment, IsApartment = true } });
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
            bool result = HttpUtil.UploadFromGallary(BuildingApartment.Name, "/api/BuildingApartmentImage/AddEdit?ParentId=" + BuildingApartment.Id + "&UserId=" + App.LogUser.Id.ToString(), images);
            if (result == true)
            {

                IsBusyProgress = false;
            }
            BuildingApartmentImages = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(BuildingApartment.Id));
            return await Task.FromResult(true);


        }
        private BuildingApartmentImages _buildingApartmentImage;

        public BuildingApartmentImages BuildingApartmentImage
        {
            get { return _buildingApartmentImage; }
            set { _buildingApartmentImage = value; OnPropertyChanged("BuildingApartmentImage"); }
        }

        public async Task AddNewPhoto(BuildingApartmentImages obj)
        {

            await BuildingApartmentImagesDataStore.AddItemAsync(obj);
            BuildingApartmentImages = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(BuildingApartment.Id));
            // UnitPhotoCount = VisualApartmentLocationPhotoItems.Count.ToString();
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
            BuildingApartmentImages _locImage = new BuildingApartmentImages();
            _locImage.Id = Guid.NewGuid().ToString();
            _locImage.ImageUrl = data.Path;
            _locImage.ImageName = data.Name;
            _locImage.ImageDescription = data.Description;
            _locImage.BuildingApartmentId = BuildingApartment.Id;

            BuildingApartmentImagesDataStore.AddItemAsync(_locImage);

            Task.Run(() => this.LoadData()).Wait();
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
            Apartment_Visual visualForm = new Apartment_Visual();
            visualForm = new Apartment_Visual();
            // visualForm.Id = Guid.NewGuid().ToString();
            visualForm.BuildingApartmentId = BuildingApartment.Id;



            App.VisualEditTracking = new List<MultiImage>();
            App.VisualEditTrackingForInvasive = new List<MultiImage>();
            VisualApartmentLocationPhotoDataStore.Clear();
            InvasiveVisualApartmentLocationPhotoDataStore.Clear();

            App.FormString = JsonConvert.SerializeObject(visualForm);
            App.IsNewForm = true;

            await Shell.Current.Navigation.PushAsync(new VisualApartmentLocationForm() { BindingContext = new VisualApartmentFormViewModel() { BuildingApartment = BuildingApartment, VisualForm = visualForm } });
            //{ BindingContext = new EditProjectLocationeageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        private ObservableCollection<Apartment_Visual> _visualFormApartmentLocationItems;

        public ObservableCollection<Apartment_Visual> VisualFormApartmentLocationItems
        {
            get { return _visualFormApartmentLocationItems; }
            set { _visualFormApartmentLocationItems = value; OnPropertyChanged("VisualFormApartmentLocationItems"); }
        }
        public ICommand GoToVisualFormCommand => new Command<Apartment_Visual>(async (Apartment_Visual parm) => await GoToVisualForm(parm));

        private VisualApartmentFormViewModel _apartmentViewModel;


        public VisualApartmentFormViewModel vm
        {
            get { return _apartmentViewModel; }
            set { _apartmentViewModel = value; OnPropertyChanged("vm"); }
        }
        private async Task GoToVisualForm(Apartment_Visual parm)
        {
            App.IsNewForm = false;
            vm = new VisualApartmentFormViewModel();
            vm.ExteriorElements = new ObservableCollection<string>(parm.ExteriorElements.Split(',').ToList());
            vm.WaterProofingElements = new ObservableCollection<string>(parm.WaterProofingElements.Split(',').ToList());
            vm.CountExteriorElements = vm.ExteriorElements.Count.ToString();
            vm.CountWaterProofingElements = vm.WaterProofingElements.Count.ToString();
            vm.RadioList_VisualReviewItems.Single(c => c.Name == parm.VisualReview).IsSelected = true;

            vm.RadioList_AnyVisualSignItems.Single(c => c.Name == parm.AnyVisualSign).IsSelected = true;
            vm.RadioList_FurtherInasiveItems.Single(c => c.Name == parm.FurtherInasive).IsSelected = true;
            vm.RadioList_ConditionAssessment.Single(c => c.Name == parm.ConditionAssessment).IsSelected = true;
            vm.RadioList_LifeExpectancyEEE.Single(c => c.Name == parm.LifeExpectancyEEE).IsSelected = true;
            vm.RadioList_LifeExpectancyLBC.Single(c => c.Name == parm.LifeExpectancyLBC).IsSelected = true;
            vm.RadioList_LifeExpectancyAWE.Single(c => c.Name == parm.LifeExpectancyAWE).IsSelected = true;

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
            VisualApartmentLocationPhotoDataStore.Clear();
            InvasiveVisualApartmentLocationPhotoDataStore.Clear();
            // InvasiveVisualApartmentLocationPhotoDataStore.Clear();

            //vm.WaterProofingElements.selectedList = parm.ExteriorElements.Split(',').ToList();
            vm.VisualForm = parm;
            vm.BuildingApartment = BuildingApartment;
            if (App.IsAppOffline)
            {
                vm.VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(parm.Id, false));
            }
            else
                vm.VisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true));



            App.FormString = JsonConvert.SerializeObject(vm.VisualForm);
            if (App.IsInvasive == false)
            {

                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(VisualApartmentLocationForm))
                {
                    await Shell.Current.Navigation.PushAsync(new VisualApartmentLocationForm() { BindingContext = vm });
                }
            }
            else
            {
                IEnumerable<VisualApartmentLocationPhoto> photos;
                if (App.IsAppOffline)
                {
                    photos = await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false);
                }
                else
                    photos = await InvasiveVisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true);

                
                vm.InvasiveVisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                App.InvaiveImages = JsonConvert.SerializeObject(vm.InvasiveVisualApartmentLocationPhotoItems);

                vm.ConclusiveVisualApartmentLocationPhotoItems = new ObservableCollection<VisualApartmentLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));

                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                    await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive(vm));

            }

        }

        public ICommand DeleteVisualFormCommand => new Command<Apartment_Visual>(async (Apartment_Visual obj) => await DeleteVisualFormCommandExecute(obj));
        private async Task DeleteVisualFormCommandExecute(Apartment_Visual obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                Response response = new Response();
                if (App.IsAppOffline)
                {
                    response = await Task.Run(() =>
                    VisualFormApartmentSqLiteDataStore.DeleteItemAsync(obj));
                }
                else
                     response = await Task.Run(() =>
                     VisualFormApartmentDataStore.DeleteItemAsync(obj));
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }


            }
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
            if (App.IsAppOffline)
            {
                if (BuildingApartment!=null)
                {
                    BuildingApartment = await BuildingApartmentSqLiteDataStore.GetItemAsync(BuildingApartment.Id);

                    VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>(await VisualFormApartmentSqLiteDataStore.GetItemsAsyncByApartmentId(BuildingApartment.Id));
                }
                
                
                IsEditDeleteAccess = true;
                
            }
            else
            {
                BuildingApartment = await BuildingApartmentDataStore.GetItemAsync(BuildingApartment.Id);

                //   BuildingApartmentImages = new ObservableCollection<BuildingApartmentImages>(await BuildingApartmentImagesDataStore.GetItemsAsyncByApartmentID(BuildingApartment.Id));
                VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>(await VisualFormApartmentDataStore.GetItemsAsyncByApartmentId(BuildingApartment.Id));
                if (App.LogUser.RoleName == "Admin")
                {

                    IsEditDeleteAccess = true;
                }
                else if (BuildingApartment.UserId == App.LogUser.Id.ToString())
                {

                    IsEditDeleteAccess = true;
                }
               
            }
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
                IsEditDeleteAccess = false;
            }
            return await Task.FromResult(true);
        }
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
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
    }

}
