using Mobile.Code.Models;
using Mobile.Code.Services.SQLiteLocal;
using Mobile.Code.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("Id", "Id")]
    //[QueryProperty("Title", "Id")]
    public class ProjectDetailViewModel : BaseViewModel
    {
        private bool _isOnline;

        public bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; OnPropertyChanged("IsOnline"); }
        }

        private Project _project;

        public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged("Project"); }
        }

        public Command ProjectDetailCommand { get; set; }


        public ObservableCollection<Project> Items { get; set; }
        public ObservableCollection<Project> AllProjects { get; set; }
        public ObservableCollection<Project> StatrtedProject { get; set; }


        private ObservableCollection<ProjectLocation> _projectLocationItems;

        public ObservableCollection<ProjectLocation> ProjectLocationItems
        {
            get { return _projectLocationItems; }
            set { _projectLocationItems = value; OnPropertyChanged("ProjectLocationItems"); }
        }

        private ObservableCollection<ProjectBuilding> _projectBuildingItems;

        public ObservableCollection<ProjectBuilding> ProjectBuildingItems
        {
            get { return _projectBuildingItems; }
            set { _projectBuildingItems = value; OnPropertyChanged("ProjectBuildingItems"); }
        }


        async Task ExecuteProjectEditCommand()
        {
            // ShellNavigationState state = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"newProject?Id={Id}");

            //   await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            //   await Shell.Current.GoToAsync("projectdetail");
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

       

        public Command LocationDetailCommand { get; set; }
        public Command BuildingDetailCommand { get; set; }
        public Command GoBackCommand { get; set; }
        public Command SaveCommand { get; set; }

        public Command EditCommand { get; set; }
        public Command DownloadOfflineCommand { get; set; }
        private async Task GoBack()
        {
            //var result = await Shell.Current.DisplayAlert(
            //    "Alert",
            //    "Are you sure you want to go back?",
            //    "Yes", "No");

            //if (result)
            //{
            await Shell.Current.GoToAsync("//main");
            // await Shell.Current.Navigation.Cle ;
            //}
        }
        private async Task Save()
        {
            await Task.FromResult(true);
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        private async Task Edit()
        {
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "Edit Project", Project = Project, ProjectType = Project.ProjectType,ProjectCategory=Project.Category } });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }
        public Command NewProjectCommonLocationCommand { get; set; }
        public Command NewProjectBuildingCommand { get; set; }
        private async Task NewProjectCommonLocation()
        {
            await Shell.Current.Navigation.PushAsync(new AddProjectLocation()
            {
                BindingContext = new ProjectLocationAddEditViewModel()
                { Title = "New Project Common Location", ProjectLocation = new ProjectLocation() { ImageUrl = "blank.png" }, Project = Project }
            });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }
        private async Task NewProjectBuilding()
        {
            await Shell.Current.Navigation.PushAsync(new AddProjectBuilding()
            {
                BindingContext = new ProjectBuildingAddEditViewModel()
                { Title = "New Building", ProjectBuilding = new ProjectBuilding() { ImageUrl = "blank.png" }, Project = Project }
            });
            // await Shell.Current.Navigation.PushAsync(new AddProjectBuilding() { BindingContext = new ProjectBuildingAddEditViewModel() { Title = "Edit Project Building" } });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }
        public ICommand DeleteCommand => new Command(async () => await Delete());
        private async Task Delete()
        {
            Response response;
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Project will be deleted completely. Are you sure?",
                "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;

                if (App.IsAppOffline)
                {
                   response = await Task.Run(() =>
                    ProjectSQLiteDataStore.DeleteItemAsync(Project)
                    );
                }
                else
                    response =  await Task.Run(() =>
                    ProjectDataStore.DeleteItemAsync(Project)
                    );

                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }             
            }
        }
        private bool _IsAccess;

        public bool IsAccessRole
        {
            get { return _IsAccess; }
            set { _IsAccess = value; OnPropertyChanged("IsAccessRole"); }
        }

        public bool IsPickerVisible { get; set; }
        
        public ProjectDetailViewModel()
        {
            CreateInvasiveCommand = new Command(async () => await CreateInvasive());
            GoBackCommand = new Command(async () => await GoBack());
            //  SaveCommand = new Command(async () => await Save());
            EditCommand = new Command(async () => await Edit());
            SaveCommand = new Command(async () => await Save());
            
            NewProjectCommonLocationCommand = new Command(async () => await NewProjectCommonLocation());
            NewProjectBuildingCommand = new Command(async () => await NewProjectBuilding());

            LocationDetailCommand = new Command<ProjectLocation>(async (ProjectLocation parm) => await ExecuteLocationDetailCommand(parm));
            BuildingDetailCommand = new Command<ProjectBuilding>(async (ProjectBuilding parm) => await ExecuteBuildingDetailCommand(parm));

            ShowPickerCommand = new Command( () =>  OpenOfflineProjectList());
            
            DownloadOfflineCommand = new Command(async () => await DownloadOffline());
        }
       
        private bool _isInvasive;
        public bool IsInvasive
        {
            get { return _isInvasive; }
            set { _isInvasive = value; OnPropertyChanged("IsInvasive"); }
        }

        private async Task DownloadOffline()
        {
            IsBusyProgress = true;
            //create an offline Project
            Response res1 = await ProjectSQLiteDataStore.AddItemAsync(Project);
           
            //get project common loc and Building details
            var ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationDataStore
                .GetItemsAsyncByProjectID(Project.Id));

            Response response = new Response();
            foreach (var item in ProjectLocationItems)
            {

                //insert projectlocations
                item.OnlineId = item.Id;
                Response resultProjLocation;
                var existinprojLoc = await ProjectLocationSqLiteDataStore.GetItemAsync(item.Id);
                if (existinprojLoc==null)
                {
                    resultProjLocation = await ProjectLocationSqLiteDataStore.AddItemAsync(item);
                }
                 else
                    _  = await ProjectLocationSqLiteDataStore.UpdateItemAsync(item);

                response.Message = response.Message + "\n" + item.Name + ": " + item.Id + " added successully, locations will be added now.";
                var VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>
                    (await VisualFormProjectLocationDataStore
                    .GetItemsAsyncByProjectLocationId(item.Id));

                foreach (var projLocForm in VisualFormProjectLocationItems)
                {
                    var images = await VisualProjectLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(projLocForm.Id, true);
                    foreach (var img in images.ToList())
                    {
                        var localPath = DependencyService.Get<IFileService>().DownloadImage(img.ImageUrl, img.VisualLocationId);
                        img.ImageUrl = localPath;
                        await VisualProjectLocationPhotoDataStore.AddItemAsync(img, true);
                    }
                    projLocForm.OnlineId = projLocForm.Id;
                    var existinLoc= await VisualFormProjectLocationSqLiteDataStore.GetItemAsync(projLocForm.Id);
                    Response res = new Response();
                    if (existinLoc==null)
                    {
                        res  = await VisualFormProjectLocationSqLiteDataStore.AddItemAsync(projLocForm);
                    }
                    else
                        res = await VisualFormProjectLocationSqLiteDataStore.UpdateItemAsync(projLocForm,null);
                    
                    DependencyService.Get<IToast>().Show(item.Name + " Location is available offline now.");
                }
              
            }

            var ProjectBuildingItems = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore
                .GetItemsAsyncByProjectID(Project.Id));
            foreach (var item in ProjectBuildingItems)
            {
                item.OnlineId = item.Id;
                Response resultBuild;
                var existinbuild = await ProjectBuildingSqLiteDataStore.GetItemAsync(item.Id);
                if (existinbuild == null)
                {
                    resultBuild = await ProjectBuildingSqLiteDataStore.AddItemAsync(item);
                }
                else
                    _ = await ProjectBuildingSqLiteDataStore.UpdateItemAsync(item);
                var resultBuilding = await ProjectBuildingSqLiteDataStore.AddItemAsync(item);
                

                var BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationDataStore
                .GetItemsAsyncByBuildingId(item.Id));

                foreach (var buildingLoc in BuildingLocations)
                {
                    buildingLoc.OnlineId = buildingLoc.Id;

                    Response resultBuildLoc;
                    var existinbuildLoc = await BuildingLocationSqLiteDataStore.GetItemAsync(buildingLoc.Id);
                    if (existinbuildLoc == null)
                    {
                        resultBuildLoc = await BuildingLocationSqLiteDataStore.AddItemAsync(buildingLoc);
                    }
                    else
                        _ = await BuildingLocationSqLiteDataStore.UpdateItemAsync(buildingLoc);

                    
                    //if (resultBuildLoc.Status == ApiResult.Success)
                    //{
                    response.Message = response.Message + "\n" + buildingLoc.Name + "added successully. Locations will be added";

                    var VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationDataStore
                    .GetItemsAsyncByBuildingLocationId(buildingLoc.Id));


                    foreach (var buildLocForm in VisualFormBuildingLocationItems)
                    {
                        var images = await VisualBuildingLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(buildLocForm.Id, true);
                        foreach (var img in images.ToList())
                        {
                            var localPath = DependencyService.Get<IFileService>().DownloadImage(img.ImageUrl, img.VisualBuildingId);
                            img.ImageUrl = localPath;
                            await VisualBuildingLocationPhotoDataStore.AddItemAsync(img, true);
                        }
                        buildLocForm.OnlineId = buildLocForm.Id;
                        var exisBuilLoc= await VisualFormBuildingLocationSqLiteDataStore.GetItemAsync(buildLocForm.Id);
                        if (exisBuilLoc==null)
                        {
                            _ = await VisualFormBuildingLocationSqLiteDataStore.AddItemAsync(buildLocForm, null);
                        }    
                        else
                            _ = await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(buildLocForm, null);
                            
                        DependencyService.Get<IToast>().Show(buildLocForm.Name + " Location is available offline now.");

                    }
                    //}

                }

                var BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentDataStore
                    .GetItemsAsyncByBuildingId(item.Id));

                foreach (var apartment in BuildingApartments)
                {
                    apartment.OnlineId = apartment.Id;
                    Response resultApt;
                    var existinApt = await BuildingApartmentSqLiteDataStore.GetItemAsync(apartment.Id);
                    if (existinApt == null)
                    {
                        resultApt = await BuildingApartmentSqLiteDataStore.AddItemAsync(apartment);
                    }
                    else
                        _ = await BuildingApartmentSqLiteDataStore.UpdateItemAsync(apartment);

                    
                    response.Message = response.Message + "\n" + apartment.Name + "added successully. Locations will be added";

                    var VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>
                        (await VisualFormApartmentDataStore.GetItemsAsyncByApartmentId(apartment.Id));


                    foreach (var aptLoc in VisualFormApartmentLocationItems)
                    {

                        var images = await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectVisualID(aptLoc.Id, true);
                        foreach (var img in images.ToList())
                        {
                            var localPath = DependencyService.Get<IFileService>().DownloadImage(img.ImageUrl, img.VisualApartmentId);
                            img.ImageUrl = localPath;
                            await VisualApartmentLocationPhotoDataStore.AddItemAsync(img, true);
                        }
                        aptLoc.OnlineId = aptLoc.Id;
                        var existinAptLoc = await VisualFormApartmentSqLiteDataStore.GetItemAsync(aptLoc.Id);
                        if (existinAptLoc==null)
                        {
                            _ = await VisualFormApartmentSqLiteDataStore.AddItemAsync(aptLoc, null);
                        }
                        else
                            _ = await VisualFormApartmentSqLiteDataStore.UpdateItemAsync(aptLoc, null);
                            
                        DependencyService.Get<IToast>().Show(aptLoc.Name + " apartment is available offline now.");
                    }
                    //}
                }
               
            }
            

            IsBusyProgress = false;
        }

        private void OpenOfflineProjectList()
        {
            IsPickerVisible = true;
            OnPropertyChanged("IsPickerVisible");
        }
        public Command ShowPickerCommand { get; set; }
        public Command CreateInvasiveCommand { get; set; }
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

                    //if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectDetail))
                    //{
                    await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
                    //}
                    IsBusyProgress = false;
                    //  await Shell.Current.Navigation.PopAsync();
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
                    await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = Project } });

                    IsBusyProgress = false;
                    //  await Shell.Current.Navigation.PopAsync();
                }
            }
        }
        async Task ExecuteLocationDetailCommand(ProjectLocation parm)
        {

           
            if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectLocationDetail))
                await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail()
                { BindingContext = new ProjectLocationDetailViewModel() { ProjectLocation = parm } }).ConfigureAwait(false);
        }
        async Task ExecuteBuildingDetailCommand(ProjectBuilding parm)
        {
            await Shell.Current.Navigation.PushAsync(new ProjectBuildingDetail()
            { BindingContext = new ProjectBuildingDetailViewModel() { ProjectBuilding = parm } });
           
        }
        public async Task LoadData()
        {
            IsBusyProgress = true;

            bool complete = await Task.Run(Running);
            if (complete == true)
            {

                IsBusyProgress = false;
            }
        }
        private bool _isEditDeleteAccess;

        public bool IsEditDeleteAccess
        {
            get { return _isEditDeleteAccess; }
            set { _isEditDeleteAccess = value; OnPropertyChanged("IsEditDeleteAccess"); }
        }
        private bool _canInvasiveCreate;

        public bool CanInvasiveCreate
        {
            get { return _canInvasiveCreate; }
            set { _canInvasiveCreate = value; OnPropertyChanged("CanInvasiveCreate"); }
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

        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }

        async Task<bool> Running()
        {
            IsOnline = !App.IsAppOffline;
            IsInvasive = (IsOnline && App.IsInvasive) ? true : false;
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
            }
            if (App.IsAppOffline)
            {
                Project = await ProjectSQLiteDataStore.GetItemAsync(Project.Id);
                CanInvasiveCreate = false;
                if (Project.ProjectType != "Invasive")
                {

                    App.IsInvasive = false;
                }
                else
                {
                    App.IsInvasive = true;
                }

                IsEditDeleteAccess = true;

                ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationSqLiteDataStore.GetItemsAsyncByProjectID(Project.Id));
                ProjectBuildingItems = new ObservableCollection<ProjectBuilding>(await ProjectBuildingSqLiteDataStore.GetItemsAsyncByProjectID(Project.Id));
            }
            //online part
            else
            {
                Project = await ProjectDataStore.GetItemAsync(Project.Id);

                if (App.LogUser.RoleName == "Admin")
                {
                    if (Project.ProjectType != "Invasive")
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            CanInvasiveCreate = true;
                            BtnInvasiveText = "Invasive";
                        }
                    }
                    else
                    {

                        CanInvasiveCreate = true;
                        BtnInvasiveText = "Refresh";
                    }

                    IsEditDeleteAccess = true;
                }
                else if (Project.UserId == App.LogUser.Id.ToString())
                {
                    if (Project.ProjectType != "Invasive")
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            CanInvasiveCreate = true;
                            BtnInvasiveText = "Invasive";
                        }
                    }
                    else
                    {
                        if (Project.IsAccess == true)
                        {
                            CanInvasiveCreate = true;
                            BtnInvasiveText = "Refresh";
                        }
                        else
                        {
                            CanInvasiveCreate = false;
                        }
                    }

                    IsEditDeleteAccess = true;
                }
                else
                {
                    if (Project.ProjectType != "Invasive" && Project.IsAccess)
                    {
                        if (Project.IsInvaisveExist == true)
                        {
                            CanInvasiveCreate = true;
                            BtnInvasiveText = "Invasive";
                        }

                    }
                    if (Project.ProjectType == "Invasive" && Project.IsAccess)
                    {
                        CanInvasiveCreate = true;
                        BtnInvasiveText = "Refresh";
                    }

                }

                ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationDataStore.GetItemsAsyncByProjectID(Project.Id));
                ProjectBuildingItems = new ObservableCollection<ProjectBuilding>(await ProjectBuildingDataStore.GetItemsAsyncByProjectID(Project.Id));
            }

            var allOffProjs = await ProjectSQLiteDataStore.GetItemsAsync(true);
            OfflineProjects = new ObservableCollection<Project>(allOffProjs.Where(x => x.Category == Project.Category));
            if (App.IsInvasive)
            {
                OfflineProjects = new ObservableCollection<Project>(allOffProjs.Where(x => x.Id == Project.Id));
            }
            return await Task.FromResult(true);


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
        
        
        //Sync Project Data
        public async Task PushProjectToServer()
        {
            
            bool syncedSuccessfully = true;
            IsBusyProgress = true;

            var res = await Task.Run(async () =>
            {
                Response response = new Response();
                
                var localProject = await ProjectSQLiteDataStore.GetItemAsync(SelectedOfflineProject.Id);

                //insert in online db
                
                //get project common loc and Building details
                var ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationSqLiteDataStore
                    .GetItemsAsyncByProjectID(SelectedOfflineProject.Id));

                foreach (var item in ProjectLocationItems)
                {
                   
                    //insert projectlocations
                   
                    string localId = item.Id;
                    
                    //check if projectlocation exists on central repo
                    if (item.OnlineId == null)
                    {
                        item.Id = null;
                    }
                    else
                    {
                        var existingProjLoc = await ProjectLocationDataStore.GetItemAsync(item.OnlineId);

                        if (existingProjLoc.Id == null)
                        {
                            item.Id = null;
                        }
                        else
                        {
                            if (existingProjLoc.ProjectId != Project.Id)
                            {
                                item.Id = null;
                            }
                            else
                                item.Id = item.IsDelete ? null : item.OnlineId;
                        }
                        
                    }
                    
                    item.ProjectId = Project.Id;

                    var resultProjLocation = await ProjectLocationDataStore.AddItemAsync(item);

                    if (resultProjLocation.Status == ApiResult.Success)
                    {
                        var uploadedProjectLocation = JsonConvert.DeserializeObject<ProjectLocation>(resultProjLocation.Data.ToString());
                        //update local projectlocation
                        item.Id = localId;
                        item.OnlineId = uploadedProjectLocation.Id;
                        
                        item.ProjectId = SelectedOfflineProject.Id;
                        _ = await ProjectLocationSqLiteDataStore.UpdateItemAsync(item);
                       

                        response.Message = response.Message + "\n" + item.Name + ": " + item.Id +" added successully, locations will be added now.";
                        var VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>
                            (await VisualFormProjectLocationSqLiteDataStore
                            .GetItemsAsyncByProjectLocationId(localId));
                        _ = await VisualFormProjectLocationDataStore.GetItemsAsyncByProjectLocationId(uploadedProjectLocation.Id);
                        
                        foreach (var formLocationItem in VisualFormProjectLocationItems)
                        {
                            //add lowest level  location data
                            string localFormId = formLocationItem.Id;
                            var images = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore
                                .GetItemsAsyncByLoacationIDSqLite(formLocationItem.Id, false));
                            
                            List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                            
                            if (formLocationItem.OnlineId==null)
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
                                    if (existingformLocation.ProjectLocationId != uploadedProjectLocation.Id)
                                    {
                                        formLocationItem.Id = null;
                                    }
                                    else
                                        formLocationItem.Id = formLocationItem.IsDelete ? null : formLocationItem.OnlineId;
                                }                                
                            }
                            formLocationItem.ProjectLocationId = uploadedProjectLocation.Id;
                            Response locationResult;
                            if (formLocationItem.Id==null)
                            {
                               
                                locationResult  = await VisualFormProjectLocationDataStore.AddItemAsync(formLocationItem, imageList);
                                List<MultiImage> FilteredImages = new List<MultiImage>();
                                if (locationResult.Status == ApiResult.Success)
                                {
                                    List<MultiImage> ImagesList = new List<MultiImage>(await VisualProjectLocationPhotoDataStore.GetMultiImagesAsyncByLoacationIDSqLite
                                        (localFormId, false));

                                    foreach (var syncImage in ImagesList)
                                    {
                                        if (imageList.Contains(syncImage.Image))
                                        {
                                            syncImage.IsSynced = true;
                                            FilteredImages.Add(syncImage);
                                        }
                                    }
                                }

                                formLocationItem.OnlineId = locationResult.ID;
                                formLocationItem.Id = localFormId;
                                formLocationItem.ProjectLocationId = localId;
                               
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
                                foreach (var syncImg in ImagesList)
                                {

                                    syncImg.IsSynced = true;
                                    FilteredImages.Add(syncImg);

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

                        }
                    }
                    else
                    {
                        syncedSuccessfully = false;
                        response.Message = response.Message + "\n" + item.Name + "failed to sync, skipping the children";
                    }
                }

                var ProjectBuildingItems = new ObservableCollection<ProjectBuilding>(await ProjectBuildingSqLiteDataStore
                    .GetItemsAsyncByProjectID(SelectedOfflineProject.Id));

                foreach (var item in ProjectBuildingItems)
                {
                    //insert buildinglocations
                    
                    string localId = item.Id;
                    
                    if (item.OnlineId == null)
                    {
                        item.Id = null;
                    }
                    else
                    {
                        
                        var existingBuild = await ProjectBuildingDataStore.GetItemAsync(item.OnlineId);
                        if (existingBuild.Id == null)
                        {
                            item.Id = null;
                        }
                        else
                        {
                            if (existingBuild.ProjectId != Project.Id)
                            {
                                item.Id = null;
                            }
                            else
                                item.Id = item.IsDelete ? null : item.OnlineId;
                        }                        
                    }


                    item.ProjectId = Project.Id;

                    var resultBuilding = await ProjectBuildingDataStore.AddItemAsync(item);
                    if (resultBuilding.Status == ApiResult.Success)
                    {                       
                        item.Id = localId;
                        //if(item.OnlineId==null)
                        item.OnlineId= resultBuilding.ID;

                        item.ProjectId = SelectedOfflineProject.Id;
                        _ = await ProjectBuildingSqLiteDataStore.UpdateItemAsync(item);

                        response.Message = response.Message + "\n" + item.Name + "added successully.";
                       
                        var BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationSqLiteDataStore
                        .GetItemsAsyncByBuildingId(localId));
                        
                        foreach (var buildingLoc in BuildingLocations)
                        {
                            
                            string localBuildId = buildingLoc.Id;

                            //check if projectlocation exists on central repo
                            if (buildingLoc.OnlineId == null)
                            {
                                buildingLoc.Id = null;
                            }
                            else
                            {
                                
                                var existingBuildLoc = await BuildingLocationDataStore.GetItemAsync(buildingLoc.OnlineId);
                                if (existingBuildLoc.Id == null)
                                {
                                    buildingLoc.Id = null;
                                }
                                else
                                {
                                    if (existingBuildLoc.BuildingId!=resultBuilding.ID)
                                    {
                                        buildingLoc.Id = null;
                                    }
                                    else
                                        buildingLoc.Id = buildingLoc.IsDelete ? null : buildingLoc.OnlineId;
                                }                              
                            }

                            buildingLoc.BuildingId = resultBuilding.ID==null?item.OnlineId:resultBuilding.ID;
                            
                            var resultBuildLoc = await BuildingLocationDataStore.AddItemAsync(buildingLoc);
                            if (resultBuildLoc.Status == ApiResult.Success)
                            {
                                //var uploadedBuildingLocation = JsonConvert.DeserializeObject<BuildingLocation>(resultBuildLoc.Data.ToString());
                                //resultBuilding.ID = uploadedBuildingLocation.Id;//added now
                                
                                buildingLoc.Id = localBuildId;
                               // if(buildingLoc.OnlineId ==null)
                                buildingLoc.OnlineId = resultBuildLoc.ID;
                                buildingLoc.BuildingId = localId;
                                await BuildingLocationSqLiteDataStore.UpdateItemAsync(buildingLoc);

                                response.Message = response.Message + "\n" + buildingLoc.Name + "added successully. Locations will be added";
                                
                                var VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationSqLiteDataStore
                                .GetItemsAsyncByBuildingLocationId(localBuildId));
                                _= await VisualFormBuildingLocationDataStore.GetItemsAsyncByBuildingLocationId(resultBuildLoc.ID);

                                foreach (var buildLocForm in VisualFormBuildingLocationItems)
                                {
                                    string localBuildFormId = buildLocForm.Id;
                                    var images = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore
                                .GetItemsAsyncByProjectIDSqLite(localBuildFormId, false));
                                    List<string> imageList = images.Select(c => c.ImageUrl).ToList();

                                    if (buildLocForm.OnlineId == null)
                                    {
                                        buildLocForm.Id = null;
                                    }
                                    else
                                    {
                                        var existngBuildLocForm = await VisualFormBuildingLocationDataStore.GetItemAsync(buildLocForm.OnlineId);
                                        if (existngBuildLocForm == null)
                                        {
                                            buildLocForm.Id = null;
                                        }
                                        else
                                        {
                                            if (existngBuildLocForm.BuildingLocationId!=resultBuilding.ID)
                                            {
                                                buildLocForm.Id = null;
                                            }
                                            else
                                                buildLocForm.Id = buildLocForm.IsDelete ? null : buildLocForm.OnlineId;
                                        }                                        
                                    }
                                  
                                    buildLocForm.BuildingLocationId = resultBuildLoc.ID;
                                    Response BuildlocationResult;
                                    if (buildLocForm.Id == null)
                                    {

                                        BuildlocationResult = await VisualFormBuildingLocationDataStore.AddItemAsync(buildLocForm, imageList);
                                        List<MultiImage> FilteredImages = new List<MultiImage>();
                                        if (BuildlocationResult.Status == ApiResult.Success)
                                        {
                                            List<MultiImage> ImagesList = new List<MultiImage>(await VisualBuildingLocationPhotoDataStore.GetMultiImagesAsyncByProjectIDSqLite
                                                (localBuildFormId, false));

                                            foreach (var syncImage in ImagesList)
                                            {
                                                if (imageList.Contains(syncImage.Image))
                                                {
                                                    syncImage.IsSynced = true;
                                                    FilteredImages.Add(syncImage);
                                                }
                                            }
                                        }
                                        buildLocForm.OnlineId = BuildlocationResult.ID;
                                        buildLocForm.Id = localBuildFormId;
                                        buildLocForm.BuildingLocationId = localBuildId;
                                        await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(buildLocForm, FilteredImages);
                                    }
                                    else
                                    {
                                        
                                        List<MultiImage> ImagesList = new List<MultiImage>(await VisualBuildingLocationPhotoDataStore.GetMultiImagesAsyncByProjectIDSqLite
                                    (localBuildFormId, false));
                                        //List<MultiImage> OnlineImagesList = new List<MultiImage>(await VisualBuildingLocationPhotoDataStore.GetMultiImagesAsyncByProjectIDSqLite
                                        //    (buildLocForm.Id, false));
                                       
                                        //ImagesList.AddRange(OnlineImagesList);

                                        if (App.IsInvasive)
                                        {
                                            BuildlocationResult =await  VisualFormBuildingLocationDataStore.UpdateItemAsync(buildLocForm, ImagesList.Where(x => x.ImageType == "TRUE").ToList());
                                            BuildlocationResult = await VisualFormBuildingLocationDataStore.UpdateItemAsync(buildLocForm, ImagesList.Where(x => x.ImageType == "CONCLUSIVE").ToList(), "CONCLUSIVE");
                                        }
                                        else
                                            BuildlocationResult = await VisualFormBuildingLocationDataStore.UpdateItemAsync(buildLocForm, ImagesList);
                                        
                                        List<MultiImage> FilteredImages = new List<MultiImage>();
                                        foreach (var syncImg in ImagesList)
                                        {

                                            syncImg.IsSynced = true;
                                            FilteredImages.Add(syncImg);

                                        }
                                        _=await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(buildLocForm, FilteredImages);
                                    }


                                    if (BuildlocationResult.Status == ApiResult.Success)
                                    {
                                       
                                        response.Message = response.Message + "\n" + buildLocForm.Name + "added successully.";
                                    }
                                    else
                                    {
                                        syncedSuccessfully = false;
                                        response.Message = response.Message + "\n" + buildLocForm.Name + "failed to added.";
                                    }
                                }
                            }

                        }   
                        
                        var BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentSqLiteDataStore
                            .GetItemsAsyncByBuildingId(localId));

                        foreach (var apartment in BuildingApartments)
                        {

                            string localaptId = apartment.Id;

                            //check if projectlocation exists on central repo
                            if (apartment.OnlineId == null)
                            {
                                apartment.Id = null;
                            }
                            else
                            {                               
                                var existingApt = await BuildingApartmentDataStore.GetItemAsync(apartment.OnlineId);
                                if (existingApt.Id == null)
                                {
                                    apartment.Id = null;
                                }
                                else
                                {
                                    if (existingApt.BuildingId!=resultBuilding.ID)
                                    {
                                        apartment.Id = null;
                                    }
                                    else
                                        apartment.Id = apartment.IsDelete ? null : apartment.OnlineId;
                                }                               
                            }

                            apartment.BuildingId = resultBuilding.ID == null ? item.OnlineId : resultBuilding.ID;

        
                            var aptResult = await BuildingApartmentDataStore.AddItemAsync(apartment);
                            if (aptResult.Status == ApiResult.Success)
                            {
                                response.Message = response.Message + "\n" + apartment.Name + "added successully. Locations will be added";
                                apartment.Id = localaptId;
                                //if (apartment.OnlineId == null)
                                    apartment.OnlineId = aptResult.ID;
                                
                                apartment.BuildingId = localId;
                                await BuildingApartmentSqLiteDataStore.UpdateItemAsync(apartment);
                                
                                var VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>
                                    (await VisualFormApartmentSqLiteDataStore.GetItemsAsyncByApartmentId(localaptId));
                                _ = await VisualFormApartmentDataStore.GetItemsAsyncByApartmentId(aptResult.ID);
                                
                                foreach (var aptLoc in VisualFormApartmentLocationItems)
                                {
                                    string localAptLocFormId = aptLoc.Id;
                                    var images = new ObservableCollection<VisualApartmentLocationPhoto>
                                        (await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(aptLoc.Id, false));
                                    List<string> imageList = images.Select(c => c.ImageUrl).ToList();

                                    if (aptLoc.OnlineId == null)
                                    {
                                        aptLoc.Id = null;
                                    }
                                    else
                                    {
                                        var existingaptLoc = await VisualFormApartmentDataStore.GetItemAsync(aptLoc.OnlineId);
                                        if (existingaptLoc == null)
                                        {
                                            aptLoc.Id = null;
                                        }
                                        else
                                        {
                                            if (existingaptLoc.BuildingApartmentId != resultBuilding.ID)
                                            {
                                                aptLoc.Id = null;
                                            }
                                            else
                                                aptLoc.Id = aptLoc.IsDelete ? null : aptLoc.OnlineId;
                                        }
                                    }

                                    
                                    aptLoc.BuildingApartmentId = aptResult.ID;

                                    Response aptlocationResult;
                                    if (aptLoc.Id == null)
                                    {

                                        aptlocationResult = await VisualFormApartmentDataStore.AddItemAsync(aptLoc, imageList);
                                        List<MultiImage> FilteredImages = new List<MultiImage>();
                                        if (aptlocationResult.Status == ApiResult.Success)
                                        {
                                            List<MultiImage> ImagesList = new List<MultiImage>(await VisualApartmentLocationPhotoDataStore.GetMultiImagesAsyncByLocationIDSqLite
                                                (localAptLocFormId, false));
                                           
                                            foreach (var syncImage in ImagesList)
                                            {
                                                if (imageList.Contains(syncImage.Image))
                                                {
                                                    syncImage.IsSynced = true;
                                                    FilteredImages.Add(syncImage);
                                                }
                                            }
                                        }
                                        aptLoc.OnlineId = aptlocationResult.ID;
                                        aptLoc.Id = localAptLocFormId;
                                        aptLoc.BuildingApartmentId = localaptId;
                                        await VisualFormApartmentSqLiteDataStore.UpdateItemAsync(aptLoc, FilteredImages);
                                    }
                                    else
                                    {
                                        //CREATE MULTIIMAGE LIST
                                        var onLineImages = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore
                                        .GetItemsAsyncByProjectVisualID(aptLoc.Id, false));


                                        List<MultiImage> ImagesList = new List<MultiImage>(await VisualApartmentLocationPhotoDataStore.GetMultiImagesAsyncByLocationIDSqLite
                                    (localAptLocFormId, false));
                                       
                                        if (App.IsInvasive)
                                        {
                                            aptlocationResult = await VisualFormApartmentDataStore.UpdateItemAsync(aptLoc, ImagesList.Where(x => x.ImageType == "TRUE").ToList());
                                            aptlocationResult = await VisualFormApartmentDataStore.UpdateItemAsync(aptLoc, ImagesList.Where(x => x.ImageType == "CONCLUSIVE").ToList(), "CONCLUSIVE");
                                        }
                                        else
                                            aptlocationResult = await VisualFormApartmentDataStore.UpdateItemAsync(aptLoc, ImagesList);


                                        List<MultiImage> FilteredImages = new List<MultiImage>();
                                        foreach (var syncImg in ImagesList)
                                        {

                                            syncImg.IsSynced = true;
                                            FilteredImages.Add(syncImg);

                                        }
                                        await VisualFormApartmentSqLiteDataStore.UpdateItemAsync(aptLoc, FilteredImages);
                                    }


                                    
                                    if (aptlocationResult.Status == ApiResult.Success)
                                    {
                                       
                                        response.Message = response.Message + "\n" + aptLoc.Name + "added successully.";
                                    }
                                    else
                                    {
                                        syncedSuccessfully = false;
                                        response.Message = response.Message + "\n" + aptLoc.Name + "failed to added.";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        response.Message = response.Message + item.Name + "failed to be added.";
                    }
                }
                Project.IsSynced = syncedSuccessfully;
                await ProjectSQLiteDataStore.UpdateItemAsync(Project);
                return response;
            });
            await LoadData();
            IsBusyProgress = false;
        }
    }
    
}
