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
            // this.BindingContext = vm= new BuildingLocationDetailViewModel();
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b1.jpg" });

            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b3.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b4.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b5.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b6.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b7.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b8.jpg" });
            //WorkImage.Add(new WorkImage { ImageUrl = "http://umesh83-001-site1.btempurl.com/b9.jpg" });

            //if (App.IsInvasive == true)
            //{
            //    btnNewVisual.IsVisible = btnNewVisualLabel.IsVisible= false;
            //}
            //    this.BindingContext = this;

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            await ((BuildingLocationDetailViewModel)this.BindingContext).LoadData();
            //vm.LoadData();
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