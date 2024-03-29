﻿using ImageEditor.ViewModels;
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("projectLocationID", "projectLocationID")]
    //[QueryProperty("Title", "Id")]
    public class ProjectLocationDetailViewModel : BaseViewModel
    {

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public ICommand GoHomeCommand => new Command(async () => await GoHome());
        private async Task GoHome()
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }

        private Project project;

        public Project Project
        {
            get { return project; }
            set { project = value; OnPropertyChanged("Project"); }
        }
        private ProjectLocation _projectLocation;

        public ProjectLocation ProjectLocation
        {
            get { return _projectLocation; }
            set { _projectLocation = value; OnPropertyChanged("ProjectLocation"); }
        }

        private string _projectLocationID;

        public string projectLocationID
        {
            get { return _projectLocationID; }
            set { _projectLocationID = value; OnPropertyChanged("projectLocationID"); }
        }

        public ICommand ChoosePhotoCommand { get; set; }

        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }
        public Command EditCommand { get; set; }

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
        private async Task GoBack()
        {
            //await Shell.Current.Navigation.PopAsync();
            tokenSource.Cancel();
            await Shell.Current.Navigation.PopAsync();

        }
        private async Task Save()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ProjectLocationDetail());

        }
        public ObservableCollection<ProjectCommonLocationImages> _projectCommonLocationImagesItems { get; set; }

        public ObservableCollection<ProjectCommonLocationImages> ProjectCommonLocationImagesItems
        {
            get { return _projectCommonLocationImagesItems; }
            set { _projectCommonLocationImagesItems = value; OnPropertyChanged("ProjectCommonLocationImagesItems"); }
        }

        public Command ImageDetailCommand { get; set; }

        private ProjectCommonLocationImages _projectCommonLocationImages;

        public ProjectCommonLocationImages ProjectCommonLocationImages
        {
            get { return _projectCommonLocationImages; }
            set { _projectCommonLocationImages = value; OnPropertyChanged("ProjectCommonLocationImages"); }
        }

        async Task ExecuteImageDetailCommand(ProjectCommonLocationImages parm)
        {
            ProjectCommonLocationImages = parm;
            ImgData.Path = parm.ImageUrl;
            ImgData.ProjectCommonLocationImages = parm;
            ImgData.FormType = "P";
            await CurrentWithoutDetail.EditImage(ImgData, GetImageProjectCommonLocationImages);

        }
        private async void GetImageProjectCommonLocationImages(ImageData ImgData)
        {
            ProjectCommonLocationImages.ImageUrl = ImgData.Path;

            if (App.IsAppOffline) { } //todo
                                      // await ProjectCommonLocationImagesSqLiteDataStore.UpdateItemAsync(ProjectCommonLocationImages);
            else
                await ProjectCommonLocationImagesDataStore.UpdateItemAsync(ProjectCommonLocationImages);
        }
        public ProjectLocationDetailViewModel()
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
            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            EditCommand = new Command(async () => await Edit());
            ImageDetailCommand = new Command<ProjectCommonLocationImages>(async (ProjectCommonLocationImages parm) => await ExecuteImageDetailCommand(parm));
            ChoosePhotoCommand = new Command(async () => await ChoosePhotoCommandExecute());
            ImgData = new ImageData();
            Task.Run(() =>
            {
                //MessagingCenter.Unsubscribe<VisualProjectLocationFormViewModel, string>(this, "LocationSwipped");

                MessagingCenter.Subscribe<VisualProjectLocationFormViewModel, string>(this, "LocationSwipped", async (sender, arg) =>
                {
                    ProjectLocation_Visual currentVisualLocation = null;

                    if (arg == "Right")
                    {
                        if (currentLocationSeq + 1 < VisualFormProjectLocationItems.Count)
                        {
                            currentVisualLocation = VisualFormProjectLocationItems[currentLocationSeq + 1];
                        }

                    }
                    else
                    {
                        if (currentLocationSeq - 1 >= 0)
                        {
                            currentVisualLocation = VisualFormProjectLocationItems[currentLocationSeq - 1];
                        }
                    }

                    try
                    {
                        IsBusyProgress = true;
                        if (currentVisualLocation != null)
                            await GoToVisualForm(currentVisualLocation, true);
                        else
                        {
                            var _lastPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                            if (_lastPage.GetType() == typeof(VisualProjectLocationForm))
                            {
                                await Shell.Current.Navigation.PopAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        IsBusyProgress = false;
                    }

                });
            });          

        }

        private int currentLocationSeq;
        public ICommand NewViusalReportCommand => new Command(async () => await NewViusalReportCommandExecue());
        private ObservableCollection<ProjectLocation_Visual> _visualFormProjectLocationItems;

        public ObservableCollection<ProjectLocation_Visual> VisualFormProjectLocationItems
        {
            get { return _visualFormProjectLocationItems; }
            set { _visualFormProjectLocationItems = value; OnPropertyChanged("VisualFormProjectLocationItems"); }
        }

        //public ObservableCollection<VisualFormProjectLocation> VisualFormProjectLocationItems { get; set; }
        private async Task NewViusalReportCommandExecue()
        {
            IsBusyProgress = true;
            App.VisualEditTracking = new List<MultiImage>();
            App.VisualEditTrackingForInvasive = new List<MultiImage>();
            //await Task.Run(async () =>
            //{
                ProjectLocation_Visual visualForm = new ProjectLocation_Visual();
                visualForm = new ProjectLocation_Visual();
                //visualForm.Id = Guid.NewGuid().ToString();
                visualForm.ProjectLocationId = ProjectLocation.Id;


                VisualProjectLocationPhotoDataStore.Clear();               

                InvasiveVisualProjectLocationPhotoDataStore.Clear();


                if (App.IsInvasive == false)
                {
                    App.FormString = JsonConvert.SerializeObject(visualForm);
                    App.IsNewForm = true;
                    await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = new VisualProjectLocationFormViewModel() { ProjectLocation = ProjectLocation, VisualForm = visualForm } });
                }
                else

                {
                    App.IsNewForm = false;

                    if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                        await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive() { BindingContext = new VisualProjectLocationFormViewModel() { ProjectLocation = ProjectLocation, VisualForm = visualForm } });

                }
            //});

            IsBusyProgress = false;
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        public ICommand NewImagCommand => new Command(async () => await NewImage());
        private async Task NewImage()
        {
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
                 new string[] { "Take New Photo", "From Gallery" });

            switch (selectedOption)
            {
                case "Take New Photo":
                    await Shell.Current.Navigation.PushModalAsync(new Camera2Forms.CameraPage() { BindingContext = new CameraViewModel() { ProjectLocation = ProjectLocation, IsProjectLocation = true } });
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
                            //If we have selected images, put them into the carousel view.
                            if (images.Count > 0)
                            {
                                IsBusyProgress = true;
                                // var listOfImages = images as List<string>;
                                Task.Run(() => UploadGallary(images));

                            }
                        });
                    }
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        // List<MultiImage> imageList = new List<MultiImage>();
                        DependencyService.Get<IMediaService>().OpenGallery();
                        MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                        MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                        {
                            if (images.Count > 0)
                            {
                                IsBusyProgress = true;
                                // var listOfImages = images as List<string>;
                                Task.Run(() => UploadGallary(images));

                            }

                        });


                    }
                    break;
                default:
                    break;
            }
            IsBusyProgress = false;
            //await Shell.Current.Navigation.PushAsync(new EditProjectLocationImage() 
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl="blank.png"} , ProjectLocation = ProjectLocation } });
        }
        public async Task<bool> UploadGallary(List<string> images)
        {

            IsBusyProgress = true;
            bool result = HttpUtil.UploadFromGallary(ProjectLocation.Name, "/api/ProjectLocationImage/AddEdit?ParentId=" + ProjectLocation.Id + "&UserId=" + App.LogUser.Id.ToString(), images);
            if (result == true)
            {

                IsBusyProgress = false;
            }
            ProjectCommonLocationImagesItems = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            return await Task.FromResult(true);


        }
        private VisualProjectLocationFormViewModel _apartmentViewModel;


        public VisualProjectLocationFormViewModel ProjectLocationViewModel
        {
            get { return _apartmentViewModel; }
            set { _apartmentViewModel = value; OnPropertyChanged("ProjectLocationViewModel"); }
        }

        public async Task AddNewPhoto(ProjectCommonLocationImages obj)
        {

            await ProjectCommonLocationImagesDataStore.AddItemAsync(obj);
            ProjectCommonLocationImagesItems = new ObservableCollection<ProjectCommonLocationImages>(await ProjectCommonLocationImagesDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
            // UnitPhotoCount = VisualApartmentLocationPhotoItems.Count.ToString();
        }

        //public ICommand GoToVisualSwipeViewCommand => new Command(async () => await GotoVisualSwipeView());

        //private async Task GotoVisualSwipeView()
        //{
        //    VisualProjectLocationSwipeViewModel vm = new VisualProjectLocationSwipeViewModel();
        //    vm.VisualFormProjectLocationItems = this.VisualFormProjectLocationItems;
        //    await Shell.Current.Navigation.PushAsync(new VisualProjectLocationsSwipeView() { BindingContext = vm });
        //}
        
        public ICommand GoToVisualFormCommand => new Command<ProjectLocation_Visual>(async (ProjectLocation_Visual parm) =>
        {
            IsBusyProgress = true;
            try
            {
                await GoToVisualForm(parm);
            }
            catch (Exception ex)
            {
                //Task cancelled moving to new location
            }
            finally { IsBusyProgress = false; }
                
        });
        
        private async Task GoToVisualForm(ProjectLocation_Visual parm, bool isSwipped=false)
        {
            IsBusyProgress = true;
            
            //await Task.Run(async () =>
            //{                         
                currentLocationSeq = VisualFormProjectLocationItems.IndexOf(parm);
                App.IsNewForm = false;
                VisualProjectLocationFormViewModel vm = new VisualProjectLocationFormViewModel();
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

                vm.ProjectVisualLocations = VisualFormProjectLocationItems;
                if (App.IsInvasive)
                {
                    //For Conclusive 
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

                VisualProjectLocationPhotoDataStore.Clear();
                InvasiveVisualProjectLocationPhotoDataStore.Clear();


                vm.VisualForm = parm;
            await Task.Run(async () =>
            {
                if (App.IsAppOffline)
                {
                    vm.VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false));
                }
                else

                    vm.VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true));

            });

            vm.ProjectLocation = ProjectLocation;
                ProjectLocationViewModel = vm;
                App.FormString = JsonConvert.SerializeObject(vm.VisualForm);

                if (App.IsInvasive == false)
                {
                    if (isSwipped)
                    {
                        var _lastPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                        await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = vm });
                        if (_lastPage.GetType() == typeof(VisualProjectLocationForm))
                        {
                            Shell.Current.Navigation.RemovePage(_lastPage);
                        }

                    }
                    else
                    {
                        if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(VisualProjectLocationForm))
                        {
                            //Device.BeginInvokeOnMainThread(async () =>
                            //{
                                await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = vm });
                            //});
                            

                        }
                    }

                }
                else
                {

                    IEnumerable<VisualProjectLocationPhoto> photos;
                await Task.Run(async () =>
                {
                    if (App.IsAppOffline)
                        {
                            photos = await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false);
                        }
                        else
                            photos = await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true);
                        vm.InvasiveVisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                        vm.ConclusiveVisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                });

                 
                //think later of this serialization
                App.InvaiveImages = JsonConvert.SerializeObject(vm.InvasiveVisualProjectLocationPhotoItems);

                    if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                    {
                        //Device.BeginInvokeOnMainThread(async () =>
                        //{
                            await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive(ProjectLocationViewModel));
                        //});
                    }

                }

            //});
            IsBusyProgress = false;
        }


        public ICommand DeleteVisualFormCommand => new Command<ProjectLocation_Visual>(async (ProjectLocation_Visual obj) => await DeleteVisualFormCommandExecute(obj));
        private async Task DeleteVisualFormCommandExecute(ProjectLocation_Visual obj)
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");
            Response response = new Response();
            if (result)
            {
                IsBusyProgress = true;
                if (App.IsAppOffline)
                {
                    response = await Task.Run(() =>
                     VisualFormProjectLocationSqLiteDataStore.DeleteItemAsync(obj));
                }
                else
                    response = await Task.Run(() =>
                      VisualFormProjectLocationDataStore.DeleteItemAsync(obj));
                
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }

            }
        }
        public ICommand DeleteImagCommand => new Command<ProjectCommonLocationImages>(async (ProjectCommonLocationImages obj) => await DeleteImagCommandExecute(obj));
        private async Task DeleteImagCommandExecute(ProjectCommonLocationImages obj) //not being used perhaps.
        {
            var result = await Shell.Current.DisplayAlert(
                  "Alert",
                  "Are you sure you want to remove?",
                  "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                var response = await Task.Run(() =>
                    ProjectCommonLocationImagesDataStore.DeleteItemAsync(obj)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                    // await Shell.Current.Navigation.PopAsync();
                }
                //  await ProjectCommonLocationImagesDataStore.DeleteItemAsync(obj);
                // Shell.Current.Navigation.RemovePage(new BuildingLocationDetail());
                await LoadData();
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

            }
        }

        public ICommand DeleteCommand => new Command(async () => await Delete());


        private async Task Delete()
        {
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Are you sure you want to remove?",
                "Yes", "No");
            Response response = new Response();
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
                     ProjectLocationDataStore.DeleteItemAsync(ProjectLocation)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }

            }
        }
        private async Task Edit()
        {
            tokenSource.Cancel();
            await Shell.Current.Navigation.PushAsync(new AddProjectLocation() { BindingContext = new ProjectLocationAddEditViewModel() { Title = "Edit Project Common Image", Project = Project, ProjectLocation = ProjectLocation } },true);
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
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
            set { _imgData = value; OnPropertyChanged("ImgData"); }
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

        private void GetImageDetail(ImageData ImgData)
        {
            ImageData data = ImgData;
            ProjectCommonLocationImages _locImage = new ProjectCommonLocationImages();
            _locImage.Id = Guid.NewGuid().ToString();
            _locImage.ImageUrl = data.Path;
            _locImage.ImageName = data.Name;
            _locImage.ImageDescription = data.Description;
            _locImage.ProjectLocationId = ProjectLocation.Id;

            ProjectCommonLocationImagesDataStore.AddItemAsync(_locImage);
            //Task.Run(() => this.LoadData()).Wait();
            //LoadData();
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
                if (ProjectLocation != null)
                {
                    //await Task.Run(async() =>
                    //{
                        ProjectLocation = await ProjectLocationSqLiteDataStore.GetItemAsync(ProjectLocation.Id);
                        VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>(await VisualFormProjectLocationSqLiteDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
                    //});
                    
                }
            }
            else
            {
                if (App.LogUser.RoleName == "Admin")
                {
                    IsEditDeleteAccess = true;
                }
                else if (ProjectLocation.UserId == App.LogUser.Id.ToString())
                {
                    IsEditDeleteAccess = true;
                }
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    IsBusyProgress = false;
                }

                if (ProjectLocation != null)
                {
                    //await Task.Run(async () =>
                    //{
                        ProjectLocation = await ProjectLocationDataStore.GetItemAsync(ProjectLocation.Id);
                        VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>(await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(ProjectLocation.Id));
                    //});                   

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
            bool complete = await Task.Run(()=>Running(new CancellationTokenSource().Token));
            if (complete == true)
            {
                IsBusyProgress = false;
            }
            return true;
            // ProjectLocation = await ProjectLocationDataStore.GetItemAsync(ProjectLocation.Id);

        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }

    }

}
