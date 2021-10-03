using Mobile.Code.Models;
using Mobile.Code.Views;
using System.Collections.ObjectModel;
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


        //public ObservableCollection<ProjectLocation> ProjectLocationItems { get; set; }
        //public ObservableCollection<ProjectBuilding> ProjectBuildingItems { get; set; }

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

        //string porjectId;
        //public string Id
        //{
        //    set => porjectId = Uri.UnescapeDataString(value);
        //    get => porjectId;

        //}

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
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "Edit Project", Project = Project, ProjectType = Project.ProjectType } });
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
            var result = await Shell.Current.DisplayAlert(
                "Alert",
                "Project will be deleted completely. Are you sure?",
                "Yes", "No");

            if (result)
            {
                IsBusyProgress = true;
                var response = await Task.Run(() =>
                    ProjectDataStore.DeleteItemAsync(Project)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }
                // Shell.Current.Navigation.RemovePage(new BuildingLocationDetail());

                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

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
            EditCommand = new Command(async () => await Edit());
            NewProjectCommonLocationCommand = new Command(async () => await NewProjectCommonLocation());
            NewProjectBuildingCommand = new Command(async () => await NewProjectBuilding());

            LocationDetailCommand = new Command<ProjectLocation>(async (ProjectLocation parm) => await ExecuteLocationDetailCommand(parm));
            BuildingDetailCommand = new Command<ProjectBuilding>(async (ProjectBuilding parm) => await ExecuteBuildingDetailCommand(parm));

            // ProjectEditCommand = new Command(async () => await ExecuteProjectEditCommand());

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

            // ShellNavigationState state = Shell.Current.CurrentState;
            //  await App.Current.MainPage.Navigation.PushModalAsync(new ShowImage() { BindingContext = new ShowImageViewModel(image.ImageUrl) });
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjectDetail() ));

            //  await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("ProjectLocationDetail");
            if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(ProjectLocationDetail))
                await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail()
                { BindingContext = new ProjectLocationDetailViewModel() { ProjectLocation = parm } }).ConfigureAwait(false);
        }
        async Task ExecuteBuildingDetailCommand(ProjectBuilding parm)
        {
            await Shell.Current.Navigation.PushAsync(new ProjectBuildingDetail()
            { BindingContext = new ProjectBuildingDetailViewModel() { ProjectBuilding = parm } });
            // await Shell.Current.GoToAsync("ProjectBuildingDetail");
            //  await Shell.Current.GoToAsync($"ProjectBuildingDetail/?projectBuildingID={parm.Id}");
            // ShellNavigationState state = Shell.Current.CurrentState;
            // await App.Current.MainPage.Navigation.PushModalAsync(new ShowImage() { BindingContext = new ShowImageViewModel(image.ImageUrl) });
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjectDetail() ));

            //  await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("projectdetail");
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
            Project = await ProjectDataStore.GetItemAsync(Project.Id);
            //if(Project.IsInvasive==true)
            //{
            //    IsCreateOrRefreshInvasive = true;
            //}
            //else
            //{
            //    IsCreateOrRefreshInvasive = false;
            //}
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

            return await Task.FromResult(true);


        }
    }
    //public class ProjectViewModel : BaseViewModel
    //{
    //    public Command ProjectDetailCommand { get; set; }
    //    public ProjectViewModel()
    //    {
    //        ProjectDetailCommand = new Command(async () => await ExecuteProjectDetailCommand());

    //    }
    //    public void Load()
    //    {

    //    }
    //    async Task ExecuteProjectDetailCommand()
    //    {
    //      await  Application.Current.MainPage.DisplayAlert("Selected Peron", "Person id : ", "Ok","cancel");
    //        // await Shell.Current.GoToAsync("projectdetail");
    //    }
    //}
}
