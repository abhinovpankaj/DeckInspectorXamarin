using Mobile.Code.Models;
using Mobile.Code.Services.SQLiteLocal;
using Mobile.Code.Views;
using Newtonsoft.Json;
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
    public class ProjectDetailViewModel : BaseViewModel
    {
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

            

        }
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
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
            }
            if (App.IsAppOffline)
            {
                Project = await ProjectSQLiteDataStore.GetItemAsync(Project.Id);
                
                if (Project.ProjectType != "Invasive")
                {
                    if (Project.IsInvaisveExist)
                    {
                        CanInvasiveCreate = true;
                        BtnInvasiveText = "Invasive";
                    }
                    else
                    {

                        CanInvasiveCreate = true;
                        BtnInvasiveText = "Refresh";
                    }
                }
                else
                {
                    CanInvasiveCreate = true;
                    BtnInvasiveText = "Refresh";
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

            OfflineProjects = new ObservableCollection<Project>(await ProjectSQLiteDataStore.GetItemsAsync(true));
            
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

                //insert in db
                
                //get project common loc and Building details
                var ProjectLocationItems = new ObservableCollection<ProjectLocation>(await ProjectLocationSqLiteDataStore
                    .GetItemsAsyncByProjectID(SelectedOfflineProject.Id));

                foreach (var item in ProjectLocationItems)
                {
                    //insert projectlocations
                    item.ProjectId = Project.Id;
                    string localId = item.Id;
                    //check if projectlocation exists on central repo
                    var existingProjLoc = await ProjectLocationDataStore.GetItemAsync(item.Id);
                    if (existingProjLoc==null)
                    {
                        item.Id = null;
                    }
                    var resultProjLocation = await ProjectLocationDataStore.AddItemAsync(item);

                    

                    if (resultProjLocation.Status == ApiResult.Success)
                    {
                        var uploadedProjectLocation = JsonConvert.DeserializeObject<ProjectLocation>(resultProjLocation.Data.ToString());
                        //update local projectlocation
                        item.Id = uploadedProjectLocation.Id;
                        await ProjectLocationSqLiteDataStore.UpdateItemAsync(item);

                        response.Message = response.Message + "\n" + item.Name + "added successully, locations will be added now.";
                        var VisualFormProjectLocationItems = new ObservableCollection<ProjectLocation_Visual>
                            (await VisualFormProjectLocationSqLiteDataStore
                            .GetItemsAsyncByProjectLocationId(localId));

                        foreach (var formLocationItem in VisualFormProjectLocationItems)
                        {
                            //add lowest level  location data
                            var images = new ObservableCollection<VisualProjectLocationPhoto>(await VisualProjectLocationPhotoDataStore
                                .GetItemsAsyncByLoacationIDSqLite(formLocationItem.Id, false));
                            List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                            formLocationItem.ProjectLocationId = uploadedProjectLocation.Id;
                            var existingformLocation = await VisualFormProjectLocationDataStore.GetItemAsync(formLocationItem.Id);
                            if (existingformLocation==null)
                            {
                                formLocationItem.Id = null;
                            }
                            
                            var locationResult = await VisualFormProjectLocationDataStore.AddItemAsync(formLocationItem, imageList);
                            if (response.Status == ApiResult.Success)
                            {
                                response.Message = response.Message + "\n" + formLocationItem.Name + "added successully.";
                                formLocationItem.Id = locationResult.ID;
                                await VisualFormProjectLocationSqLiteDataStore.UpdateItemAsync(formLocationItem,null);
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
                    item.ProjectId = Project.Id;
                    string localId = item.Id;
                    var existingBuild = await ProjectBuildingDataStore.GetItemAsync(item.Id);
                    if (existingBuild==null)
                    {
                        item.Id = null;
                    }
                    
                    var resultBuilding = await ProjectBuildingDataStore.AddItemAsync(item);
                    if (resultBuilding.Status == ApiResult.Success)
                    {
                        response.Message = response.Message + "\n" + item.Name + "added successully.";
                        item.Id = resultBuilding.ID;
                        await ProjectBuildingSqLiteDataStore.UpdateItemAsync(item);
                        
                        var BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationSqLiteDataStore
                        .GetItemsAsyncByBuildingId(localId));
                        
                        foreach (var buildingLoc in BuildingLocations)
                        {
                            buildingLoc.BuildingId = resultBuilding.ID;
                            string localBuildId = buildingLoc.Id;
                            var existingBuildLoc = await BuildingLocationDataStore.GetItemAsync(buildingLoc.Id);
                            if (existingBuildLoc==null)
                            {
                                buildingLoc.Id = null;
                            }
                            
                            var resultBuildLoc = await BuildingLocationDataStore.AddItemAsync(buildingLoc);
                            if (resultBuildLoc.Status == ApiResult.Success)
                            {
                                buildingLoc.Id = resultBuildLoc.ID;
                                await BuildingLocationSqLiteDataStore.UpdateItemAsync(buildingLoc);
                                response.Message = response.Message + "\n" + buildingLoc.Name + "added successully. Locations will be added";
                                
                                var VisualFormBuildingLocationItems = new ObservableCollection<BuildingLocation_Visual>(await VisualFormBuildingLocationSqLiteDataStore
                                .GetItemsAsyncByBuildingLocationId(localBuildId));

                                foreach (var buildLocForm in VisualFormBuildingLocationItems)
                                {
                                    var images = new ObservableCollection<VisualBuildingLocationPhoto>(await VisualBuildingLocationPhotoDataStore
                                .GetItemsAsyncByProjectIDSqLite(localBuildId, false));
                                    List<string> imageList = images.Select(c => c.ImageUrl).ToList();
                                    var existngBuildLocForm = await VisualFormBuildingLocationDataStore.GetItemAsync(buildLocForm.Id);
                                    Response locationResult;
                                    if (existngBuildLocForm==null)
                                    {
                                        buildLocForm.Id = null;
                                        
                                    }
                                    locationResult = await VisualFormBuildingLocationDataStore.AddItemAsync(buildLocForm, imageList);
                                    //else
                                    //{
                                    //    locationResult = await VisualFormBuildingLocationDataStore.AddItemAsync(buildLocForm, imageList);
                                    //}
                                    buildLocForm.BuildingLocationId = resultBuildLoc.ID;
                                    
                                    if (response.Status == ApiResult.Success)
                                    {
                                        buildLocForm.Id = locationResult.ID;
                                        await VisualFormBuildingLocationSqLiteDataStore.UpdateItemAsync(buildLocForm, null);
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
                            apartment.BuildingId = resultBuilding.ID;
                            string localaptId = apartment.Id;
                            var existingApt = await BuildingApartmentDataStore.GetItemAsync(apartment.Id);
                            if (existingApt==null)
                            {
                                apartment.Id = null;
                            }
                            
                            var aptResult = await BuildingApartmentDataStore.AddItemAsync(apartment);
                            if (aptResult.Status == ApiResult.Success)
                            {
                                response.Message = response.Message + "\n" + apartment.Name + "added successully. Locations will be added";
                                apartment.Id = aptResult.ID;
                                await BuildingApartmentDataStore.UpdateItemAsync(apartment);
                                
                                var VisualFormApartmentLocationItems = new ObservableCollection<Apartment_Visual>
                                    (await VisualFormApartmentSqLiteDataStore.GetItemsAsyncByApartmentId(localaptId));
                                
                                foreach (var aptLoc in VisualFormApartmentLocationItems)
                                {
                                    var images = new ObservableCollection<VisualApartmentLocationPhoto>
                                        (await VisualApartmentLocationPhotoDataStore.GetItemsAsyncByProjectIDSqLite(aptLoc.Id, false));
                                    List<string> imageList = images.Select(c => c.ImageUrl).ToList();

                                    var existingaptLoc = await VisualFormApartmentDataStore.GetItemAsync(aptLoc.Id);

                                    if (existingaptLoc==null)
                                    {
                                        aptLoc.Id = null;
                                    }
                                    
                                    aptLoc.BuildingApartmentId = aptResult.ID;
                                    var locationResult = await VisualFormApartmentDataStore.AddItemAsync(aptLoc, imageList);
                                    if (response.Status == ApiResult.Success)
                                    {
                                        aptLoc.Id = locationResult.ID;
                                        await VisualFormApartmentSqLiteDataStore.UpdateItemAsync(aptLoc,null);
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
                
                return response;
            });
            Project.IsSynced = syncedSuccessfully;
            await ProjectSQLiteDataStore.UpdateItemAsync(Project);
            IsBusyProgress = false;
        }
    }
    
}
