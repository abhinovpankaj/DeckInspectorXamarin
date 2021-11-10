using Mobile.Code.ViewModels;
using Mobile.Code.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        async Task GoLogoutCommandExecute()
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
            if (page.GetType() == typeof(VisualProjectLocationForm) || page.GetType() == typeof(VisualBuildingLocationForm) || page.GetType() == typeof(ProjectAddEdit) ||
                page.GetType() == typeof(AddBuildingLocation) || page.GetType() == typeof(AddBuildingApartment) ||
                page.GetType() == typeof(VisualApartmentLocationForm)
                )
            {
                return true;
           
            }
            else
                return base.OnBackButtonPressed();
        }
      
        protected override void OnAppearing()
        {
            base.OnAppearing();


        }
        private string _logUserName;

        public string LogUserName
        {
            get { return _logUserName; }
            set { _logUserName = value; OnPropertyChanged("LogUserName"); }
        }

        void RegisterRoutes()
        {
            Routing.RegisterRoute("home", typeof(HomePage));
            Routing.RegisterRoute("newProject", typeof(ProjectAddEdit));
            Routing.RegisterRoute("projectDetail", typeof(ProjectDetail));
          
            Routing.RegisterRoute("ProjectBuildingDetail", typeof(ProjectBuildingDetail));
            Routing.RegisterRoute("ProjectLocationDetail", typeof(ProjectLocationDetail));




            Routing.RegisterRoute("addprojectlocation", typeof(AddProjectLocation));
            Routing.RegisterRoute("addprojectbuilding", typeof(AddProjectBuilding));



            Routing.RegisterRoute("AddbuildingLocation", typeof(AddBuildingLocation));

            Routing.RegisterRoute("AddBuildingApartment", typeof(AddBuildingApartment));

            Routing.RegisterRoute("BuildingLocationDetail", typeof(BuildingLocationDetail));
            Routing.RegisterRoute("BuildingApartmentDetail", typeof(BuildingApartmentDetail));


            Routing.RegisterRoute("buildingImages", typeof(BuildingImage));



            Routing.RegisterRoute("AddbuildingImages", typeof(AddBuildingImage));

           
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
           
            if (!string.IsNullOrEmpty(ProjectType))
                await Shell.Current.Navigation.PushAsync(new ProjectAddEdit() { BindingContext = new ProjectAddEditViewModel() { Title = "New Project", ProjectType = ProjectType } });

        }
        async Task GoSettingCommandExecute()
        {
            Shell.Current.FlyoutIsPresented = false;

            await Shell.Current.Navigation.PushAsync(new SettingPage());


        }

        void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            
        }

        void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {

        }
    }
}
