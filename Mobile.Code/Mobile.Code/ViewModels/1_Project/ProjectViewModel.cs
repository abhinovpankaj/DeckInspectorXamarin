using Mobile.Code.Models;
using Mobile.Code.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {

        public Command ProjectDetailCommand { get; set; }
        public Command AddNewCommand { get; set; }

        public ObservableCollection<Project> Items { get; set; }
        // public ObservableCollection<Project> AllProjects { get; set; }

        private ObservableCollection<Project> _allProjects;

        public ObservableCollection<Project> AllProjects
        {
            get { return _allProjects; }
            set { _allProjects = value; OnPropertyChanged("AllProjects"); }
        }
        private ObservableCollection<Project> _aProjectslist;

        public ObservableCollection<Project> Projects
        {
            get { return _aProjectslist; }
            set { _aProjectslist = value; OnPropertyChanged("Projects"); }
        }
        public ObservableCollection<Project> StatrtedProject { get; set; }
        async Task ExecuteProjectDetailCommand(Project project)
        {
            App.IsInvasive = false;
            string action = project.ProjectType;

            if (action == "Visual Report")
            {
                App.ReportType = ReportType.Visual;

            }
            else if (action == "Invasive Report")
            {
                App.ReportType = ReportType.Invasive;

            }
            else if (action == "Final Report")
            {
                App.ReportType = ReportType.Final;
                //   ProjectType = action;

            }

            await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

        }

        async Task ExecuteInvasiveDetailCommand(Project project)
        {
            App.IsInvasive = true;
            string action = project.ProjectType;

            App.IsInvasive = true;
            project.Id = project.InvasiveProjectID;
            await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });

        }
        private bool _Isbusyprog;

        public bool IsBusyProgress
        {
            get { return _Isbusyprog; }
            set { _Isbusyprog = value; OnPropertyChanged("IsBusyProgress"); }
        }
        public Command<Project> InvasiveDetailCommand { get; set; }
        public ProjectViewModel()
        {
            Items = new ObservableCollection<Project>();
            //AllProjects = new ObservableCollection<Project>();
            StatrtedProject = new ObservableCollection<Project>();
            ProjectDetailCommand = new Command<Project>(async (Project project) => await ExecuteProjectDetailCommand(project));
            CreateInvasiveCommand = new Command<Project>(async (Project project) => await CreateInvasive(project));
            AddNewCommand = new Command(async () => await ExecuteAddNewCommand());
            InvasiveDetailCommand = new Command<Project>(async (Project project) => await ExecuteInvasiveDetailCommand(project));
            //LoadData();
        }
        async Task ExecuteAddNewCommand()
        {
            string ProjectType = string.Empty;

            App.IsInvasive = false;

            ProjectType = "Visual Report";
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project", ProjectType = ProjectType } });


        }
        private async Task<bool> Running()
        {
            IsBusyProgress = true;
            AllProjects = new ObservableCollection<Project>(await ProjectDataStore.GetItemsAsync(true));
            return await Task.FromResult(true);


        }
        public async Task LoadData()
        {
            // AllProjects = new ObservableCollection<Project>();
            bool complete = await Task.Run(Running);
            if (complete == true)
            {
                IsBusyProgress = false;

            }
        }
        public Command<Project> CreateInvasiveCommand { get; set; }
        //  CreateInvasiveCommand => new Command<Project>(async (Project project) => await CreateInvasive(project));
        private async Task CreateInvasive(Project project)
        {
            //var result = await Shell.Current.DisplayAlert(
            //    "Alert",
            //    "Are you sure you want to remove?",
            //    "Yes", "No");

            //if (result)
            //{
            IsBusyProgress = true;
            var response = await Task.Run(() =>
                ProjectDataStore.CreateInvasiveReport(project)
            );
            if (response.Status == ApiResult.Success)
            {
                App.IsInvasive = true;
                IsBusyProgress = false;
                await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            }


            //}
        }
    }


}
