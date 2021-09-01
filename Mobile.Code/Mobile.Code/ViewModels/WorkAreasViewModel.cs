using Mobile.Code.Models;
using Mobile.Code.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class WorkAreasViewModel : BaseViewModel
    {
        private WorkArea _workArea;

        public WorkArea WorkArea
        {
            get { return _workArea; }
            set { _workArea = value; OnPropertyChanged("WorkArea"); }
        }

        public Command ProjectDetailCommand { get; set; }
       
        public ObservableCollection<ItemImage> WorkAreaImages { get; set; }
       
        //async Task ExecuteProjectDetailCommand(Project parm)
        //{
         
        // //   await Application.Current.MainPage.DisplayAlert("Selected Peron", project.ProjectName, "Ok", "cancel");
        //    // await Shell.Current.GoToAsync("projectdetail");
        //}
        public WorkAreasViewModel(WorkArea parm)
        {
            WorkArea = parm;
            //WorkAreaImages = parm.WorkAreaImages;
            // AllProjects = new ObservableCollection<Project>();
            //      StatrtedProject = new ObservableCollection<Project>();
            //  ProjectDetailCommand = new Command<Project>(async (Project project) => await ExecuteProjectDetailCommand(project));

        }
        public void LoadData()
        {

            //foreach (var item in WorkArea.WorkAreaImages)
            //{
            //    WorkAreaImages.Add(new ItemImage() { 
                
                
            //    });
            //}
                //var popular = new List<Place>();


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
