using Mobile.Code.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectBuildingDetail : ContentPage
    {

        public ProjectBuildingDetail()
        {
            InitializeComponent();
            // this.BindingContext = vm = new ProjectBuildingDetailViewModel();

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            // await vm.LoadData();
            await ((ProjectBuildingDetailViewModel)this.BindingContext).LoadData();
            base.OnAppearing();
        }



        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddBuildingApartment");
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddbuildingLocation");
        }
    }
}