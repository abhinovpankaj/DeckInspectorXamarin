﻿using Mobile.Code.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

using System.Windows.Input;
using ImageEditor.ViewModels;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Mobile.Code.Views;
using System.Linq;
using Mobile.Code.Services;
using Mobile.Code.Media;
using Newtonsoft.Json;
using Mobile.Code.Views._3_ProjectLocation;
using Mobile.Code.Services.SQLiteLocal;

namespace Mobile.Code.ViewModels
{
    //[QueryProperty("Id", "Id")]
    //[QueryProperty("Title", "Id")]
    public class SingleLevelProjectDetailViewModel : BaseViewModel
    {
        #region Commands
        public ICommand GoHomeCommand => new Command(async () => await GoHome());

        public ICommand ChoosePhotoCommand { get; set; }

        public Command GoBackCommand { get; set; }

        private async Task GoBack()
        {

            await Shell.Current.Navigation.PopToRootAsync();

        }

        public Command ImageDetailCommand { get; set; }
        async Task ExecuteProjectEditCommand()
        {
            await Shell.Current.GoToAsync($"newProject?Id={Id}");
        }
        private async Task GoHome()
        {
            await Shell.Current.Navigation.PopToRootAsync();

        }

        private bool canDownloadOffline(object arg)
        {
            return IsInvasive;
        }

        private async void DownloadOffline(object obj)
        {
            await DownloadOffline();
        }

        private bool canDelete(object arg)
        {
            return IsEditDeleteAccess;
        }

        private async void Delete(object obj)
        {
            await Delete();
        }

        private bool canEdit(object arg)
        {
            return IsEditDeleteAccess;
        }

        private async void Edit(object obj)
        {
            await Edit();
        }

        private bool canCreateInvasive(object arg)
        {
            return CanInvasiveCreate;
        }

        private async void CreateInvasive(object obj)
        {
            await CreateInvasive();
        }

        public Command CreateInvasiveCommand { get; set; }  
        public Command EditCommand { get; set; } 
        public Command DeleteCommand { get; set; } 
        public Command DownloadOfflineCommand { get; set; } 

        public Command NewViusalReportCommand => new Command(async () => await NewViusalReportCommandExecue());

        public Command ShowPickerCommand { get; set; }
        public ICommand GoToVisualFormCommand => new Command<ProjectLocation_Visual>(async (ProjectLocation_Visual parm) => await GoToVisualForm(parm));
        #endregion



        #region Properties
        private float _progressValue;
        public float ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged("ProgressValue");

                }

            }
        }
        public string SelectedImage { get; set; }


        private ImageData _imgData;

        public ImageData ImgData
        {
            get { return _imgData; }
            set { _imgData = value; OnPropertyChanged("ImgData"); }
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }

        private Project _selectedOfflineProject;
        public Project SelectedOfflineProject
        {
            get { return _selectedOfflineProject; }
            set { _selectedOfflineProject = value; OnPropertyChanged("SelectedOfflineProject"); }
        }
        public Command SyncProjectCommand { get; set; }
        private ObservableCollection<Project> _offlineProjects;
        public ObservableCollection<Project> OfflineProjects
        {
            get { return _offlineProjects; }
            set { _offlineProjects = value; OnPropertyChanged("OfflineProjects"); }
        }
        private string _imgPath;

        public string ImgPatah
        {
            get { return _imgPath; }
            set { _imgPath = value; OnPropertyChanged(); }
        }


        private bool _isEditDeleteAccess;

        public bool IsEditDeleteAccess
        {
            get { return _isEditDeleteAccess; }
            set { _isEditDeleteAccess = value; 
                EditCommand.ChangeCanExecute();
                DeleteCommand.ChangeCanExecute();
                OnPropertyChanged("IsEditDeleteAccess");
            }
        }
        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }

        private bool _isOnline;
        public bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; OnPropertyChanged("IsOnline"); }
        }
        private bool _isInvasive;
        public bool IsInvasive
        {
            get { return _isInvasive; }
            set 
            { 
                _isInvasive = value;
                DownloadOfflineCommand.ChangeCanExecute();
                OnPropertyChanged("IsInvasive");
                
            }
        }
        private bool _canInvasiveCreate;

        public bool CanInvasiveCreate
        {
            get { return _canInvasiveCreate; }
            set { _canInvasiveCreate = value;
                CreateInvasiveCommand.ChangeCanExecute();
                OnPropertyChanged("CanInvasiveCreate");
                
            }
        }
        private bool _isCreateOrRefreshInvasive;

        public bool IsCreateOrRefreshInvasive
        {
            get { return _isCreateOrRefreshInvasive; }
            set { _isCreateOrRefreshInvasive = value; OnPropertyChanged("IsCreateOrRefreshInvasive"); }
        }
        private string _btnInvasiveText;

        public string BtnInvasiveText
        {
            get { return _btnInvasiveText; }
            set { _btnInvasiveText = value; OnPropertyChanged("BtnInvasiveText"); }
        }

        private Project project;

        public Project Project
        {
            get { return project; }
            set { project = value; OnPropertyChanged("Project"); }
        }


        private string _projectID;

        public string ProjectID
        {
            get { return _projectID; }
            set { _projectID = value; OnPropertyChanged("ProjectID"); }
        }

        private ProjectLocation _projectLocation;

        public ProjectLocation ProjectLocation
        {
            get { return _projectLocation; }
            set { _projectLocation = value; OnPropertyChanged("ProjectLocation"); }
        }

        private string _projectLocationID;

        public string ProjectLocationID
        {
            get { return _projectLocationID; }
            set { _projectLocationID = value; OnPropertyChanged("ProjectLocationID"); }
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

        public ObservableCollection<ProjectCommonLocationImages> _projectCommonLocationImagesItems { get; set; }

        public ObservableCollection<ProjectCommonLocationImages> ProjectCommonLocationImagesItems
        {
            get { return _projectCommonLocationImagesItems; }
            set { _projectCommonLocationImagesItems = value; OnPropertyChanged("ProjectCommonLocationImagesItems"); }
        }

        private ProjectCommonLocationImages _projectCommonLocationImages;

        public ProjectCommonLocationImages ProjectCommonLocationImages
        {
            get { return _projectCommonLocationImages; }
            set { _projectCommonLocationImages = value; OnPropertyChanged("ProjectCommonLocationImages"); }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }
        public bool IsPickerVisible { get; set; }
        #endregion

        
        public SingleLevelProjectDetailViewModel()
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
            ShowPickerCommand = new Command(() => OpenOfflineProjectList());
             CreateInvasiveCommand = new Command(CreateInvasive, canCreateInvasive);
            EditCommand = new Command(Edit, canEdit);
            DeleteCommand = new Command(Delete, canDelete);
            DownloadOfflineCommand = new Command(DownloadOffline, canDownloadOffline);

            MessagingCenter.Unsubscribe<VisualProjectLocationFormViewModel, string>(this, "LocationSwipped");

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

            });

        }
        private int currentLocationSeq;
        private async Task DownloadOffline()
        {
            IsBusyProgress = true;
            //create an offline Project
            Response res = await ProjectSQLiteDataStore.AddItemAsync(Project);

            
                //get all locations
            var projLocForms = await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(Project.Id);
            float completedCount = 0;
            float totalCount = projLocForms.Count();
            foreach (var projLocForm in projLocForms)
            {
                completedCount++;
                DependencyService.Get<IToast>().Show($"Downloading {projLocForm.Name}");
                //insert projLoc offline db.
                //download image.
                var images = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(projLocForm.Id, true);
                foreach (var img in images.ToList())
                {
                    var localPath= DependencyService.Get<IFileService>().DownloadImage(img.ImageUrl, img.VisualLocationId);
                    img.ImageUrl = localPath;
                    await VisualProjectLocationPhotoDataStore.AddItemAsync(img, true);
                }
                projLocForm.OnlineId = projLocForm.Id;
                _ = await VisualFormProjectLocationSqLiteDataStore.AddItemAsync(projLocForm);
                ProgressValue = completedCount / totalCount;
            }
            
            IsBusyProgress = false;
            //DependencyService.Get<IToast>().Show("Project is available offline now.");
        }

        
        private void OpenOfflineProjectList()
        {
            IsPickerVisible = true;
            OnPropertyChanged("IsPickerVisible");
        }
        
        
        async Task CreateInvasive()
        {

            IsBusyProgress = true;

            if (App.IsInvasive == true)
            {
                Project.IsInvasive = true;
                Project.Id = Project.InvasiveProjectID;
                var response = await Task.Run(() =>
                  ProjectDataStore.CreateInvasiveReport(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    App.IsInvasive = true;

                    Project.Id = response.ID.ToString();

                    await Shell.Current.Navigation.PushAsync(new SingleLevelProjectLocation() { BindingContext = new SingleLevelProjectDetailViewModel()
                    { Project = Project } });
                    
                    IsBusyProgress = false;
                    
                }
            }
            else
            {
                Project.IsInvasive = false;
                // Project.Id = Project.InvasiveProjectID;
                var response = await Task.Run(() =>
                  ProjectDataStore.CreateInvasiveReport(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    App.IsInvasive = true;
                    Project.Id = response.ID.ToString();
                    await Shell.Current.Navigation.PushAsync(new SingleLevelProjectLocation() { BindingContext = new SingleLevelProjectDetailViewModel()
                    { Project = Project } });

                    IsBusyProgress = false;
                   
                }
            }
        }

        
        private ObservableCollection<ProjectLocation_Visual> _visualFormProjectLocationItems;

        public ObservableCollection<ProjectLocation_Visual> VisualFormProjectLocationItems
        {
            get { return _visualFormProjectLocationItems; }
            set { _visualFormProjectLocationItems = value; OnPropertyChanged("VisualFormProjectLocationItems"); }
        }

        //public ObservableCollection<VisualFormProjectLocation> VisualFormProjectLocationItems { get; set; }
        private async Task NewViusalReportCommandExecue()
        {
            ProjectLocation_Visual visualForm = new ProjectLocation_Visual();

            visualForm.ProjectLocationId = Project.Id;

            VisualProjectLocationPhotoDataStore.Clear();

            App.VisualEditTracking = new List<MultiImage>();
            App.VisualEditTrackingForInvasive = new List<MultiImage>();
            VisualProjectLocationPhotoDataStore.Clear();
            InvasiveVisualProjectLocationPhotoDataStore.Clear();


            if (App.IsInvasive == false)
            {
                App.FormString = JsonConvert.SerializeObject(visualForm);
                App.IsNewForm = true;
                await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm()
                {
                    BindingContext = new VisualProjectLocationFormViewModel()
                    { ProjectLocation = ProjectLocation, VisualForm = visualForm, ProjectID = Project.Id }
                });

            }
            else

            {
                App.IsNewForm = false;

                //await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = new VisualProjectLocationFormViewModel() { ProjectLocation = ProjectLocation, VisualForm = visualForm } });
                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                    await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive() { BindingContext = new VisualProjectLocationFormViewModel() { ProjectLocation = ProjectLocation, VisualForm = visualForm } });
                //await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = vm });
            }

            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        public async Task<bool> UploadGallary(List<string> images)
        {

            IsBusyProgress = true;
            bool result = HttpUtil.UploadFromGallary(ProjectLocation.Name, "/api/ProjectLocationImage/AddEdit?ParentId=" + ProjectLocation.Id + "&UserId=" + App.LogUser.Id.ToString(), images);
            if (result)
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
        
        private async Task GoToVisualForm(ProjectLocation_Visual parm, bool isSwipped = false)
        {
            App.IsNewForm = false;
            currentLocationSeq = VisualFormProjectLocationItems.IndexOf(parm);
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
            //  vm.VisualProjectLocationPhotoItems.Clear();
            VisualProjectLocationPhotoDataStore.Clear();
            InvasiveVisualProjectLocationPhotoDataStore.Clear();


            vm.VisualForm = parm;
            if (App.IsAppOffline)
            {
                vm.VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false));

            }
            else
                vm.VisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true));

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
                        await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = vm });
                    }
                }
                //if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(VisualProjectLocationForm))
                //{
                //    await Shell.Current.Navigation.PushAsync(new VisualProjectLocationForm() { BindingContext = vm });
                //}
            }
            else
            {
                IEnumerable<VisualProjectLocationPhoto> photos;
                if (App.IsAppOffline)
                {
                    photos = await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByLoacationIDSqLite(parm.Id, false);
                }
                else
                    photos = await InvasiveVisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(parm.Id, true);
                
                vm.InvasiveVisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(photos.Where(x => x.ImageDescription == "TRUE"));
                vm.ConclusiveVisualProjectLocationPhotoItems = new ObservableCollection<VisualProjectLocationPhoto>(photos.Where(x => x.ImageDescription == "CONCLUSIVE"));
                App.InvaiveImages = JsonConvert.SerializeObject(vm.InvasiveVisualProjectLocationPhotoItems);

                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(TabbedPageInvasive))
                {
                    await Shell.Current.Navigation.PushAsync(new TabbedPageInvasive(ProjectLocationViewModel));
                }

            }

        }

        public ICommand DeleteVisualFormCommand => new Command<ProjectLocation_Visual>(async (ProjectLocation_Visual obj) => await DeleteVisualFormCommandExecute(obj));
        private async Task DeleteVisualFormCommandExecute(ProjectLocation_Visual obj)
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
                     VisualFormProjectLocationSqLiteDataStore.DeleteItemAsync(obj));
                }
                else
                {
                    response = await Task.Run(() =>
                     VisualFormProjectLocationDataStore.DeleteItemAsync(obj));
                }
               
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await LoadData();
                }

            }
        }

        
        private async Task Delete()
        {
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Project will be deleted completely. Are you sure?",
                "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                Response response = new Response();
                if (App.IsAppOffline)
                {
                    response = await Task.Run(() =>
                    ProjectSQLiteDataStore.DeleteItemAsync(Project)
                );
                }
                else
                {
                    response = await Task.Run(() =>
                    ProjectDataStore.DeleteItemAsync(Project)
                );
                }
                
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }
              
            }
        }
        private async Task Edit()
        {
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "Edit Project", Project = Project, ProjectType = Project.ProjectType, ProjectCategory=Project.Category } });
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
       

        private async Task<bool> Running()
        {
            bool isEditdeleteAccess = false;
            bool canInvasiveCreate = false;
            string btnInvasiveText = "Invasive";
            IsOnline = !App.IsAppOffline;
            IsInvasive = (IsOnline && App.IsInvasive)?true:false;
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
            }
            if (App.IsAppOffline)
            {
                canInvasiveCreate = false;
                Project = await ProjectSQLiteDataStore.GetItemAsync(Project.Id);
                if (Project.ProjectType != "Invasive")
                {

                    App.IsInvasive = false;
                }
                else
                {
                    App.IsInvasive = true;
                }

                isEditdeleteAccess = true;
                VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>(await VisualFormProjectLocationSqLiteDataStore.GetItemsAsyncByProjectLocationId(Project.Id));
            }
            //online
            else
            {
                Project = await ProjectDataStore.GetItemAsync(Project.Id);

                if (App.LogUser.RoleName == "Admin")
                {
                    if (Project.ProjectType != "Invasive")
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            canInvasiveCreate = true;
                            btnInvasiveText = "Invasive";
                        }
                    }
                    else
                    {
                        canInvasiveCreate = true;
                        btnInvasiveText = "Refresh";
                    }

                    isEditdeleteAccess = true;
                    
                }
                else if (Project.UserId == App.LogUser.Id.ToString())
                {
                    if (Project.ProjectType != "Invasive")
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            canInvasiveCreate = true;
                            btnInvasiveText = "Invasive";
                        }
                    }
                    else
                    {
                        if (Project.IsAccess == true)
                        {
                            canInvasiveCreate = true;
                            btnInvasiveText = "Refresh";
                        }
                        else
                        {
                            CanInvasiveCreate = false;
                        }
                    }

                    isEditdeleteAccess = true;
                }
                else
                {
                    if (Project.ProjectType != "Invasive" && Project.IsAccess)
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            canInvasiveCreate = true;
                            btnInvasiveText = "Invasive";
                        }

                    }
                    if (Project.ProjectType == "Invasive" && Project.IsAccess)
                    {
                        canInvasiveCreate = true;
                        btnInvasiveText = "Refresh";
                    }

                }
                VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>(await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(Project.Id));
                var allOffProjs = await ProjectSQLiteDataStore.GetItemsAsync(true);
                
                if (App.IsInvasive)
                {
                    OfflineProjects = new ObservableCollection<Project>(allOffProjs.Where(x => x.Category == Project.Category && x.ProjectType == "Invasive"));
                }
                else
                    OfflineProjects = new ObservableCollection<Project>(allOffProjs.Where(x => x.Category == Project.Category));
            }
            Device.BeginInvokeOnMainThread(() => {
                IsEditDeleteAccess = isEditdeleteAccess;
                CanInvasiveCreate = canInvasiveCreate;
                BtnInvasiveText = btnInvasiveText;
                if (App.IsInvasive)
                {
                    IsInvasive = (IsOnline && App.IsInvasive) ? true : false;
                }
                else
                    IsInvasive = (IsOnline && (bool)Project.IsAvailableOffline) ? true : false;
            });

            return await Task.FromResult(true);

        }
        public async Task<bool> LoadData()
        {
            IsBusyProgress = true;
            bool complete = await Task.Run(Running);
            if (complete == true)
            {
                IsBusyProgress = false;
            }
            return await Task.FromResult(true);           

        }

        

        public async Task PushProjectToServer()
        {

            bool syncedSuccessfully = true;
            IsBusyProgress = true;

            var res = await Task.Run(async () =>
            {
                Response response = new Response();
                //var localProject = await ProjectSQLiteDataStore.GetItemAsync(SelectedOfflineProject.Id);

                //insert in db

                
                var VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>
                            (await VisualFormProjectLocationSqLiteDataStore.GetItemsAsyncByProjectLocationId(SelectedOfflineProject.Id));

                float totalCount = VisualFormProjectLocationItems.Count;
                float uploadCount = 0;
                _ = await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(Project.Id);
                foreach (var formLocationItem in VisualFormProjectLocationItems)
                {
                    //DependencyService.Get<IToast>().Show($"Syncing Location {formLocationItem.Name}");
                    //add lowest level  location data
                    uploadCount++;
                    string localFormId = formLocationItem.Id;
                    var images = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore
                        .GetItemsAsyncByLoacationIDSqLite(formLocationItem.Id, false));

                    List<string> imageList = images.Select(c => c.ImageUrl).ToList();

                    if (!App.IsInvasive)
                    {
                        if (formLocationItem.OnlineId == null)
                        {
                            formLocationItem.Id = null;
                        }
                        else
                        {
                            var existingformLocation = await VisualFormProjectLocationDataStore.GetItemAsync(formLocationItem.OnlineId);
                            if (existingformLocation == null)
                            {
                                formLocationItem.Id = null;
                            }
                            else
                            {
                                formLocationItem.Id = formLocationItem.IsDelete ? null : formLocationItem.OnlineId;
                            }
                        }
                        formLocationItem.ProjectLocationId = Project.Id;
                    }
                    
                    Response locationResult;
                    if (formLocationItem.Id == null)
                    {

                        locationResult = await VisualFormProjectLocationDataStore.AddItemAsync(formLocationItem, imageList);
                        List<MultiImage> FilteredImages = new List<MultiImage>();
                        if (locationResult.Status == ApiResult.Success)
                        {
                            List<MultiImage> ImagesList = new List<MultiImage>(await VisualProjectLocationPhotoDataStore.GetMultiImagesAsyncByLoacationIDSqLite
                                (localFormId, false));
                            
                            foreach (var item in ImagesList)
                            {
                                if (imageList.Contains(item.Image))
                                {
                                    item.IsSynced = true;
                                    FilteredImages.Add(item);
                                }
                            }
                        }
                        

                        formLocationItem.OnlineId = locationResult.ID;
                        formLocationItem.Id = localFormId;
                        formLocationItem.ProjectLocationId = SelectedOfflineProject.Id;
                        
                        await VisualFormProjectLocationSqLiteDataStore.UpdateItemAsync(formLocationItem, FilteredImages);
                    }
                    else
                    {

                        List<MultiImage> ImagesList = new List<MultiImage>(await VisualProjectLocationPhotoDataStore.GetMultiImagesAsyncByLoacationIDSqLite
                            (localFormId, false));
                        
                        ImagesList = ImagesList.Where(x => x.IsSynced == false).ToList();
                       
                        if (App.IsInvasive)
                        {
                            locationResult = await VisualFormProjectLocationDataStore.UpdateItemAsync(formLocationItem, ImagesList.Where(x => x.ImageType == "TRUE").ToList());
                            locationResult = await VisualFormProjectLocationDataStore.UpdateItemAsync(formLocationItem, ImagesList.Where(x => x.ImageType == "CONCLUSIVE").ToList(), "CONCLUSIVE");
                        }
                        else
                            locationResult = await VisualFormProjectLocationDataStore.UpdateItemAsync(formLocationItem, ImagesList);
                        
                        List<MultiImage> FilteredImages = new List<MultiImage>();
                        foreach (var item in ImagesList)
                        {
                            
                            item.IsSynced = true;
                            FilteredImages.Add(item);
                            
                        }
                        await VisualFormProjectLocationSqLiteDataStore.UpdateItemAsync(formLocationItem, FilteredImages);
                    }
                    if (locationResult.Status == ApiResult.Success)
                    {
                        response.Message = response.Message + "\n" + formLocationItem.Name + "added successully.";
                        
                    }
                    else
                    {
                        syncedSuccessfully = false;
                        response.Message = response.Message + "\n" + formLocationItem.Name + "failed to added.";

                    }
                    ProgressValue = uploadCount / totalCount;
                }
   
                Project.IsSynced = syncedSuccessfully;
                //if (syncedSuccessfully)
                //{
                //    DependencyService.Get<IToast>().Show("Project sync complete.");
                //}
                //else
                //    DependencyService.Get<IToast>().Show($"Project sync failed,{response.Message}.");

                await ProjectSQLiteDataStore.UpdateItemAsync(Project);
                return response;
            });
            await LoadData();
            IsBusyProgress = false;
        }
    }

}
