using Mobile.Code.Models;
using Mobile.Code.Utils;
using Mobile.Code.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            //  string action = await Shell.Current.DisplayActionSheet("Select Report Type", "Cancel", null, "Visual Report", "Invasive Report", "Finel Report");
            //  ShellNavigationState state = Shell.Current.CurrentState;
            //  await Shell.Current.GoToAsync($"projectDetail/?Id={project.Id}");
            if (action == "Visual Report")
            {
                App.ReportType = ReportType.Visual;
                //ProjectType = action;
                // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            }
            else if (action == "Invasive Report")
            {
                App.ReportType = ReportType.Invasive;
                //  ProjectType = action;
                //  await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            }
            else if (action == "Final Report")
            {
                App.ReportType = ReportType.Final;
                //   ProjectType = action;

            }

            await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjectDetail() ));

            //  await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("projectdetail");
        }

        async Task ExecuteInvasiveDetailCommand(Project project)
        {
            App.IsInvasive = true;
            string action = project.ProjectType;
            //  string action = await Shell.Current.DisplayActionSheet("Select Report Type", "Cancel", null, "Visual Report", "Invasive Report", "Finel Report");
            //  ShellNavigationState state = Shell.Current.CurrentState;
            //  await Shell.Current.GoToAsync($"projectDetail/?Id={project.Id}");
            //if (action == "Visual Report")
            //{
            //    App.ReportType = ReportType.Visual;
            //    //ProjectType = action;
            //    // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //}
            //else if (action == "Invasive Report")
            //{
            //    App.ReportType = ReportType.Invasive;
            //    //  ProjectType = action;
            //    //  await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //}
            //else if (action == "Final Report")
            //{
            //    App.ReportType = ReportType.Final;
            //    //   ProjectType = action;

            //}
            App.IsInvasive = true;
            project.Id = project.InvasiveProjectID;
            await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjectDetail() ));

            //  await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("projectdetail");
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
            CreateInvasiveCommand= new Command<Project>(async (Project project) => await CreateInvasive(project));
            AddNewCommand = new Command(async () => await ExecuteAddNewCommand());
            InvasiveDetailCommand = new Command<Project>(async (Project project) => await ExecuteInvasiveDetailCommand(project));
            //LoadData();
        }
        async Task ExecuteAddNewCommand()
        {
            string ProjectType = string.Empty;
            //string action = await Shell.Current.DisplayActionSheet("Select Report Type", "Cancel", null, "Visual Report", "Invasive Report", "Final Report");
            ////  ShellNavigationState state = Shell.Current.CurrentState;
            ////  await Shell.Current.GoToAsync($"projectDetail/?Id={project.Id}");
            //if (action == "Visual Report")
            //{
            //    App.ReportType = ReportType.Visual;
            //    ProjectType = action;
            //    // await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //}
            //else if (action == "Invasive Report")
            //{
            //    App.ReportType = ReportType.Invasive;
            //    ProjectType = action;
            //    //  await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            //}
            //else if (action == "Final Report")
            //{
            //    App.ReportType = ReportType.Final;
            //    ProjectType = action;

            //}
            //if (!string.IsNullOrEmpty(ProjectType))
            App.IsInvasive = false;

            ProjectType = "Visual Report";
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project", ProjectType = ProjectType } });


            //string action = await Application.Current.MainPage.DisplayActionSheet("Choose ProjectType", "Cancel", "MultiLevel Project", "SingleLevel Project");
            //if (action == "Common Location")
            //{
            //    //ShellNavigationState state = Shell.Current.CurrentState;
            //    //await Shell.Current.GoToAsync($"{state.Location}/newProject?action={animalName}");
            //}
            //if (action == "Building")
            //{
            //    //await Shell.Current.GoToAsync("projectdetail");
            //}
            //await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
            // await Shell.Current.GoToAsync("projectdetail");
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

                //foreach (var item in Projects)
                //{
                //    AllProjects.Add(new Project
                //    {

                //        Attendent = item.Attendent,
                //        CreatedOn = item.CreatedOn,
                //        Description = item.Description,
                //        EmployeeName = item.EmployeeName,
                //        IdAnimation = $"All{Guid.NewGuid()}",
                //        IsStarred = item.IsStarred,
                //        Id = item.Id,
                //        ProjectType = item.ProjectType,
                //        ImageUrl = item.ImageUrl,
                //        Name = item.Name,
                //        Location = item.Location,

                //        // WorkAreas = item.WorkAreas

                //    }); ; ;
                    
                    //DependencyService.Get<ILodingPageService>().HideLoadingPage();
              //  }


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
