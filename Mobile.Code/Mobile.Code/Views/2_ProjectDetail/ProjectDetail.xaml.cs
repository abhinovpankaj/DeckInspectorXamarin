using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.Code.Models;
using Mobile.Code.ViewModels;
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
            //if(vm.Project.ProjectType == "Invasive"&&vm.CanInvasiveCreate==true)
            //{
            //    btnInvasive.Text = "Refresh";
            //    btnInvasive.IsVisible = true;
            //}
            //if (vm.Project.IsAccess==true)
            //{
            //    //btnInvasive.Text = "Refresh";
            //    btnInvasive.IsVisible = true;
            //}
            //if (vm.Project.IsAccess==true&& vm.Project.ProjectType == "Invasive")
            //{
            //    btnInvasive.Text = "Refresh";
            //    btnInvasive.IsVisible = true;
            //}
            //else
            //{
            //    btnInvasive.Text = "Refresh";
            //    btnInvasive.IsVisible = true;
            //}
            // vm.LoadData();
        }
        //private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    WorkArea p = e.CurrentSelection.FirstOrDefault() as WorkArea;
        //    await Navigation.PushModalAsync(new Xamarin.Forms.NavigationPage(new WorkAreasPage() { BindingContext = new WorkAreasViewModel(p) }));
        //}

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
    }
}