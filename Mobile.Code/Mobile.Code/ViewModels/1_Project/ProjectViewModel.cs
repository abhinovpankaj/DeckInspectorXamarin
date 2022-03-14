using Mobile.Code.Models;
using Mobile.Code.Services.SQLiteLocal;
using Mobile.Code.Views;
using Mobile.Code.Views._3_ProjectLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {

        public Command ProjectDetailCommand { get; set; }
        public Command SwitchModeCommand { get; set; }
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

            if (project.Category=="MultiLevel" || string.IsNullOrEmpty( project.Category))
            {
                await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
            }
            else
            {
                
                await Shell.Current.Navigation.PushAsync(new SingleLevelProjectLocation()
                { BindingContext = new SingleLevelProjectDetailViewModel() { Project=project } }).ConfigureAwait(false);
            }
                
           
        }

        async Task ExecuteInvasiveDetailCommand(Project project)
        {
            App.IsInvasive = true;
            string action = project.ProjectType;

            App.IsInvasive = true;
            project.Id = project.InvasiveProjectID;
            if (project.Category == "MultiLevel" || string.IsNullOrEmpty(project.Category))
            {
                await Shell.Current.Navigation.PushAsync(new ProjectDetail() { BindingContext = new ProjectDetailViewModel() { Project = project } });
                
            }
            else
            {

                await Shell.Current.Navigation.PushAsync(new SingleLevelProjectLocation()
                { BindingContext = new SingleLevelProjectDetailViewModel() { Project = project } }).ConfigureAwait(false);
            }
            

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
            SwitchModeCommand = new Command(async () => await ExecuteAppModeSwitch());
            //LoadData();
        }

        private bool _isOnline;
        public bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; OnPropertyChanged("IsOnline"); }
        }
        private async Task ExecuteAppModeSwitch()
        {
            App.IsAppOffline = !App.IsAppOffline;

            if (!IsOnline)
            {
                if (App.LogUser != null)
                {
                    if (App.LogUser.Id == Guid.Empty)
                    {
                        App.AutoLogin = true;
                        App.Current.MainPage = new AppShell();
                        await Task.FromResult(true);
                    }
                    else
                    {
                        IsOnline = !IsOnline;
                        await LoadData();
                    }


                }
                else
                {
                    App.Current.MainPage = new AppShell();
                    await Task.FromResult(true);
                }

            }
            else
            {
                IsOnline = !IsOnline;
                await LoadData();
            }

        }
        async Task ExecuteAddNewCommand()
        {
            string ProjectType = string.Empty;
            string ProjectCategory = string.Empty;
            string selectedOption = await App.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", null,
               new string[] { "Single Level", "Multi Level" });

            switch (selectedOption)
            {
                case "Single Level":
                    ProjectCategory = "SingleLevel";
                    break;
                case "Multi Level":
                    ProjectCategory = "MultiLevel"; 
                    break;
                default:
                    break;
            }

            App.IsInvasive = false;

            ProjectType = "Visual Report";
            await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project", ProjectType = ProjectType, ProjectCategory= ProjectCategory } });

        }
        private async Task<bool> Running()
        {
            Debug.WriteLine("On Project list page");
            IsOnline = !App.IsAppOffline;
            IsBusyProgress = true;
            if (App.IsAppOffline)
            {
                AllProjects = new ObservableCollection<Project>(await ProjectSQLiteDataStore.GetItemsAsync(true));
            }
            else
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
