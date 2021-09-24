using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingApartmentDetail : ContentPage
    {


        public BuildingApartmentDetail()
        {
            InitializeComponent();


            //if (App.IsInvasive == true)
            //{
            //    btnNewVisual.IsVisible = btnNewVisualLabel.IsVisible= false;
            //}


        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            await ((BuildingApartmentDetailViewModel)this.BindingContext).LoadData();

        }
        //private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    WorkArea p = e.CurrentSelection.FirstOrDefault() as WorkArea;
        //    await Navigation.PushModalAsync(new Xamarin.Forms.NavigationPage(new WorkAreasPage() { BindingContext = new WorkAreasViewModel(p) }));
        //}

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