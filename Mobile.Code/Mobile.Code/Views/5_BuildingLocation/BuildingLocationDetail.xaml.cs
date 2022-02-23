using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingLocationDetail : ContentPage
    {

        //  BuildingLocationDetailViewModel vm;
        public BuildingLocationDetail()
        {
            InitializeComponent();
          

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            await ((BuildingLocationDetailViewModel)this.BindingContext).LoadData();
            //vm.LoadData();
        }
        

        private async void TapGestureRecognizer_TappedLN(object sender, EventArgs e)
        {
            await Task.FromResult(true);
            // await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new AddProjectLocation()));
        }
        private async void TapGestureRecognizer_CommonLocation(object sender, EventArgs e)
        {
            await Task.FromResult(true);
            //     await Shell.Current.GoToAsync("buildingLocation");
            //   await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new ProjectLocationPage()));

        }
        private async void TapGestureRecognizer_TappedBN(object sender, EventArgs e)
        {
            await Task.FromResult(true);
            // await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new AddProjectBuilding()));
        }
        private async void TapGestureRecognizer_Building(object sender, EventArgs e)
        {
            await Task.FromResult(true);
            //await Shell.Current.GoToAsync("BuildingApartment");
            //await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new ProjectBuildingPage()));

        }
    }
}