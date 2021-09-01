using Mobile.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Mobile.Code.Views;
using Xamarin.Forms.Xaml;
using Mobile.Code.ViewModels;
using Mobile.Code.Models;

namespace Mobile.Code
{
    public partial class AppShell : Shell
    {
        Random rand = new Random();
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }

        //     public ICommand HelpCommand => new Command<string>((url) => Device.OpenUri(new Uri(url)));
        public ICommand GoSettingCommand => new Command(async () => await GoSettingCommandExecute());
        public ICommand GoNewProjectCommand => new Command(async () => await GoNewProjectCommandExecute());

        public ICommand LogoutCommand => new Command(async () => await GoLogoutCommandExecute());
        async  Task GoLogoutCommandExecute()
        {
            App.LogUser = null;
            App.Current.MainPage = new AppShell();
             await Task.FromResult(true);
        }
            public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = this;
            
        }
       
        protected override bool OnBackButtonPressed()
        {//page.GetType() == typeof(EditBuildingApartmentImage)|| page.GetType() == typeof(EditBuildingLocationImage) || page.GetType() == typeof(EditProjectLocationImage)||
            var page = (Shell.Current?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;
            if (page.GetType() == typeof(VisualProjectLocationForm)|| page.GetType() == typeof(VisualBuildingLocationForm)|| page.GetType() == typeof(ProjectAddEdit)||
                page.GetType() == typeof(AddBuildingLocation)|| page.GetType() == typeof(AddBuildingApartment) || 
                page.GetType() == typeof(VisualApartmentLocationForm)
                )
            {
                return true;
                //if (page.SendBackButtonPressed())
                //    return base.OnBackButtonPressed();
                //else
                //    return base.OnBackButtonPressed();
            }
            else
                return base.OnBackButtonPressed();
        }
        //protected async override void OnNavigating(ShellNavigatingEventArgs args)
        //{

        //    base.OnNavigating(args);
        //    if (args.Current != null)
        //    {
        //        if (args.Current.Location.OriginalString.Contains("newProject"))
        //        {
        //            if (args.Source == ShellNavigationSource.Pop)
        //            {
        //                args.Cancel();
        //                if (await DisplayAlert("Exit?", "Are you sure you want to exit from this page?", "Yes", "No"))
        //                {
        //                    //base.OnBackButtonPressed();

        //                     Shell.Current.SendBackButtonPressed();

        //                }

        //            }
        //        }
        //    }

        //}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
           
        }
        private string _logUserName;

        public string LogUserName
        {
            get { return _logUserName; }
            set { _logUserName = value;OnPropertyChanged("LogUserName"); }
        }

        void RegisterRoutes()
        {
            Routing.RegisterRoute("home", typeof(HomePage));
            Routing.RegisterRoute("newProject", typeof(ProjectAddEdit));
            Routing.RegisterRoute("projectDetail", typeof(ProjectDetail));
            // Routing.RegisterRoute("projectLocation", typeof(ProjectLocationPage));
            // Routing.RegisterRoute("projectbuilding", typeof(ProjectBuildingPage));




            Routing.RegisterRoute("ProjectBuildingDetail", typeof(ProjectBuildingDetail));
            Routing.RegisterRoute("ProjectLocationDetail", typeof(ProjectLocationDetail));




            Routing.RegisterRoute("addprojectlocation", typeof(AddProjectLocation));
            Routing.RegisterRoute("addprojectbuilding", typeof(AddProjectBuilding));



            Routing.RegisterRoute("AddbuildingLocation", typeof(AddBuildingLocation));

            Routing.RegisterRoute("AddBuildingApartment", typeof(AddBuildingApartment));

            Routing.RegisterRoute("BuildingLocationDetail", typeof(BuildingLocationDetail));
            Routing.RegisterRoute("BuildingApartmentDetail", typeof(BuildingApartmentDetail));



            //  Routing.RegisterRoute("addprojectlocationImage", typeof(AddProjectLocationImage));

            // Routing.RegisterRoute("projectCommonlocationdetail", typeof(ProjectCommonLocationDetail));

            //Routing.RegisterRoute("BuildingDetail", typeof(BuildingDetail));
            //Routing.RegisterRoute("buildingLocation", typeof(BuildingLocation));
            //   Routing.RegisterRoute("BuildingApartment", typeof(BuildingApartment));








            Routing.RegisterRoute("buildingImages", typeof(BuildingImage));



            Routing.RegisterRoute("AddbuildingImages", typeof(AddBuildingImage));

            //Routing.RegisterRoute("addprojectlocation", typeof(AddProjectLocation));


            //routes.Add("monkeydetails", typeof(MonkeyDetailPage));
            //routes.Add("beardetails", typeof(BearDetailPage));
            //  routes.Add("catdetails", typeof(CatDetailPage));
            //   routes.Add("dogdetails", typeof(DogDetailPage));
            //  routes.Add("elephantdetails", typeof(ElephantDetailPage));
            //   routes.Add("projectdetail", typeof(ProjectDetail));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
            if (App.LogUser != null)
            {

                LogUserName = App.LogUser.FullName;
            }
           
        }

     //   protected override bool OnBackButtonPressed() => true;
        async Task GoNewProjectCommandExecute()
        {
            Shell.Current.FlyoutIsPresented = false;
            string ProjectType = "Visual Report";
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
            if (!string.IsNullOrEmpty(ProjectType))
                await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project", ProjectType = ProjectType } });


            // await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project" } });
        }
        async Task GoSettingCommandExecute()
        {
            Shell.Current.FlyoutIsPresented = false;

            await Shell.Current.Navigation.PushAsync(new SettingPage());


            //string destinationRoute = routes.ElementAt(rand.Next(0, routes.Count)).Key;
            //string animalName = null;

            //switch (destinationRoute)
            //{
            //    case "monkeydetails":
            //        animalName = MonkeyData.Monkeys.ElementAt(rand.Next(0, MonkeyData.Monkeys.Count)).Name;
            //        break;
            //    case "beardetails":
            //        animalName = BearData.Bears.ElementAt(rand.Next(0, BearData.Bears.Count)).Name;
            //        break;
            //    case "catdetails":
            //        animalName = CatData.Cats.ElementAt(rand.Next(0, CatData.Cats.Count)).Name;
            //        break;
            //    case "dogdetails":
            //        animalName = DogData.Dogs.ElementAt(rand.Next(0, DogData.Dogs.Count)).Name;
            //        break;
            //    case "elephantdetails":
            //        animalName = ElephantData.Elephants.ElementAt(rand.Next(0, ElephantData.Elephants.Count)).Name;
            //        break;
            //    //case "projectdetail":
            //    //    //animalName = ElephantData.Elephants.ElementAt(rand.Next(0, ElephantData.Elephants.Count)).Name;
            //    //    break;

            //}

            //ShellNavigationState state = Shell.Current.CurrentState;
            //await Shell.Current.GoToAsync($"{state.Location}/{destinationRoute}?name={animalName}");
            //Shell.Current.FlyoutIsPresented = false;
        }

        void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            // Cancel any back navigation
            //if (e.Source == ShellNavigationSource.Pop)
            //{
            //    e.Cancel();
            //}
        }

        void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
           
        }
    }
}
