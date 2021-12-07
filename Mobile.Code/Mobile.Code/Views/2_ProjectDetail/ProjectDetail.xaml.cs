using Mobile.Code.Models;
using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetail : ContentPage
    {
        // ProjectDetailViewModel vm;
        public ProjectDetail()
        {
            InitializeComponent();
            //vm= this.BindingContext = vm= new ProjectDetailViewModel();

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            ProjectDetailViewModel vm = ((ProjectDetailViewModel)this.BindingContext);
            await vm.LoadData();
           
        }
        


        private async void TapGestureRecognizer_TappedLN(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new AddProjectLocation()));
        }
        private async void TapGestureRecognizer_CommonLocation(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("projectLocation");
            //   await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new ProjectLocationPage()));

        }
        private async void TapGestureRecognizer_TappedBN(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new AddProjectBuilding()));
        }
        private async void TapGestureRecognizer_Building(object sender, EventArgs e)
        {
            await Task.FromResult(true);

            //await Navigation.PushAsync(new Xamarin.Forms.NavigationPage(new ProjectBuildingPage()));

        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("addprojectbuilding");
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("addprojectlocation");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            offlineProjectPicker.IsEnabled = true;
            offlineProjectPicker.IsVisible = true;
        }

        private async void offlineProjectPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (offlineProjectPicker.SelectedIndex > -1)
            {
                //Project prj = offlineProjectPicker.Items[offlineProjectPicker.SelectedIndex];

                ProjectDetailViewModel vm = (ProjectDetailViewModel)BindingContext;
                await vm.PushProjectToServer();
                vm.IsPickerVisible = false;
            }

            //offlineProjectPicker.IsEnabled = false;
            //offlineProjectPicker.IsVisible = false;
        }
    }
}