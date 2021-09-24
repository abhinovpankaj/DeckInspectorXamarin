using Mobile.Code.Models;
using Mobile.Code.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    [QueryProperty("projectBuildingID", "projectBuildingID")]
    //[QueryProperty("Title", "Id")]
    public class ProjectBuildingDetailViewModel : BaseViewModel
    {
        public ICommand GoHomeCommand => new Command(async () => await GoHome());
        private async Task GoHome()
        {
            await Shell.Current.Navigation.PopAsync();
            // int count=Shell.Current.Navigation.NavigationStack.Count;
            //for (var count = 1; count <= 3; count++)
            //{
            //    Navigation.RemovePage(this.Navigation.NavigationStack[6 - count]);
            //}
            //Navigation.PopAsync();

        }

        private ProjectBuilding _projectbuilding;

        public ProjectBuilding ProjectBuilding
        {
            get { return _projectbuilding; }
            set { _projectbuilding = value; OnPropertyChanged("ProjectBuilding"); }
        }

        private string _projectBuildingID;

        public string projectBuildingID
        {
            get { return _projectBuildingID; }
            set { _projectBuildingID = value; OnPropertyChanged("projectBuildingID"); }
        }

        public Command ProjectDetailCommand { get; set; }
        public Command ProjectEditCommand { get; set; }

        public ObservableCollection<Project> Items { get; set; }
        public ObservableCollection<Project> AllProjects { get; set; }
        public ObservableCollection<Project> StatrtedProject { get; set; }
        //public ObservableCollection<BuildingLocation> ProjectLocationItems { get; set; }
        //public ObservableCollection<BuildingApartment> ProjectBuildingItems { get; set; }

        private ObservableCollection<BuildingLocation> buildingLocation;

        public ObservableCollection<BuildingLocation> BuildingLocations
        {
            get { return buildingLocation; }
            set { buildingLocation = value; OnPropertyChanged("BuildingLocations"); }
        }

        private ObservableCollection<BuildingApartment> buildingApartment;

        public ObservableCollection<BuildingApartment> BuildingApartments
        {
            get { return buildingApartment; }
            set { buildingApartment = value; OnPropertyChanged("BuildingApartments"); }
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
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopAsync();
            //await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
            //var result = await Shell.Current.DisplayAlert(
            //    "Alert",
            //    "Are you sure you want to go back?",
            //    "Yes", "No");

            //if (result)
            //{
            //    await Shell.Current.GoToAsync("//main");
            //    // await Shell.Current.Navigation.Cle ;
            //}
        }
        private async Task Save()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ProjectBuildingDetail());
        }
        public Command NewBuildingLocationCommand { get; set; }
        public Command NewBuildingApartmentCommand { get; set; }

        public Command EditCommand { get; set; }
        public ProjectBuildingDetailViewModel()
        {
            EditCommand = new Command(async () => await Edit());

            GoBackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await Save());
            LocationDetailCommand = new Command<BuildingLocation>(async (BuildingLocation parm) => await ExecuteLocationDetailCommand(parm));
            BuildingDetailCommand = new Command<BuildingApartment>(async (BuildingApartment parm) => await ExecuteBuildingDetailCommand(parm));
            //ProjectLocationItems = new ObservableCollection<WorkImage>();
            // ProjectBuildingItems = new ObservableCollection<WorkImage>();
            NewBuildingLocationCommand = new Command(async () => await ExecuteNewBuildingLocationCommand());
            NewBuildingApartmentCommand = new Command(async () => await ExecuteNewBuildingApartmentCommand());
            ProjectEditCommand = new Command(async () => await ExecuteProjectEditCommand());

        }

        private async Task ExecuteNewBuildingLocationCommand()
        {
            await Shell.Current.Navigation.PushAsync(new AddBuildingLocation()
            {
                BindingContext = new BuildingLocationAddEditViewModel()
                { Title = "New Building Common Location", BuildingLocation = new BuildingLocation() { ImageUrl = "blank.png" }, ProjectBuilding = ProjectBuilding }
            });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }
        private async Task ExecuteNewBuildingApartmentCommand()
        {
            await Shell.Current.Navigation.PushAsync(new AddBuildingApartment()
            {
                BindingContext = new BuildingAprartmentAddEditViewModel()
                { Title = "New Building Apartment", BuildingApartment = new BuildingApartment() { ImageUrl = "blank.png" }, ProjectBuilding = ProjectBuilding }
            });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }

        private async Task Edit()
        {
            await Shell.Current.Navigation.PushAsync(new AddProjectBuilding() { BindingContext = new ProjectBuildingAddEditViewModel() { Title = "Edit Building", ProjectBuilding = ProjectBuilding } });
            // await App.Current.MainPage.Navigation.PushAsync(new ProjectDetail());
        }
        async Task ExecuteLocationDetailCommand(ProjectLocation parm)
        {

            // ShellNavigationState state = Shell.Current.CurrentState;
            //  await App.Current.MainPage.Navigation.PushModalAsync(new ShowImage() { BindingContext = new ShowImageViewModel(image.ImageUrl) });
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjectDetail() ));

            //  await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("ProjectLocationDetail");
            await Shell.Current.Navigation.PushAsync(new ProjectLocationDetail()
            { BindingContext = new ProjectLocationDetailViewModel() { ProjectLocation = parm } });
        }
        async Task ExecuteLocationDetailCommand(BuildingLocation parm)
        {
            if (parm != null)
            {
                // await Shell.Current.Navigation.PushAsync(new BuildingLo() { BindingContext = new ProjectDetailViewModel() { Project = Project } });
                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(BuildingLocationDetail))
                    await Shell.Current.Navigation.PushAsync(new BuildingLocationDetail() { BindingContext = new BuildingLocationDetailViewModel() { BuildingLocation = parm } });
            }
        }
        async Task ExecuteBuildingDetailCommand(BuildingApartment parm)
        {
            if (parm != null)
            {
                //await Shell.Current.GoToAsync("BuildingApartmentDetail");
                if (Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1].GetType() != typeof(BuildingApartmentDetail))
                    await Shell.Current.Navigation.PushAsync(new BuildingApartmentDetail() { BindingContext = new BuildingApartmentDetailViewModel() { BuildingApartment = parm } });
            }
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
                  ProjectBuildingDataStore.DeleteItemAsync(ProjectBuilding)
                );
                if (response.Status == ApiResult.Success)
                {
                    IsBusyProgress = false;
                    await Shell.Current.Navigation.PopAsync();
                }

                //     await ProjectBuildingDataStore.DeleteItemAsync(ProjectBuilding.Id);
                // Shell.Current.Navigation.RemovePage(new BuildingLocationDetail());
                //    await Shell.Current.Navigation.PopAsync();
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

            }
        }

        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }

        private bool _isInvasiveControlDisable;

        public bool IsInvasiveControlDisable
        {
            get { return _isInvasiveControlDisable; }
            set { _isInvasiveControlDisable = value; OnPropertyChanged("IsInvasiveControlDisable"); }
        }
        private async Task<bool> Running()
        {


            ProjectBuilding = await ProjectBuildingDataStore.GetItemAsync(ProjectBuilding.Id);

            if (App.LogUser.RoleName == "Admin")
            {
                IsEditDeleteAccess = true;
            }
            else if (ProjectBuilding.UserId == App.LogUser.Id.ToString())
            {
                IsEditDeleteAccess = true;
            }
            if (App.IsInvasive)
            {
                IsInvasiveControlDisable = true;
                IsEditDeleteAccess = false;
            }
            BuildingLocations = new ObservableCollection<BuildingLocation>(await BuildingLocationDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id));
            BuildingApartments = new ObservableCollection<BuildingApartment>(await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id));
            //var items = await BuildingApartmentDataStore.GetItemsAsyncByBuildingId(ProjectBuilding.Id);
            //if(items.Count()!=0)
            //{
            //    BuildingApartments = new ObservableCollection<BuildingApartment>(items);
            //}
            return await Task.FromResult(true);
        }
        private bool _isEditDeleteAccess;

        public bool IsEditDeleteAccess
        {
            get { return _isEditDeleteAccess; }
            set { _isEditDeleteAccess = value; OnPropertyChanged("IsEditDeleteAccess"); }
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
            // ProjectLocation = await ProjectLocationDataStore.GetItemAsync(ProjectLocation.Id);

        }
    }

}
