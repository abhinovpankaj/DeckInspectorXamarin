using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBuildingLocationImage : ContentPage
    {
        //ProjectAddEditViewModel vm;
        public EditBuildingLocationImage()
        {
            InitializeComponent();
            txtName.Completed += (s, e) => txtDes.Focus();
            //this.BindingContext =vm= new ProjectAddEditViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //vm.Load();
        }
        //protected override bool OnBackButtonPressed()
        //{

        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        if (await DisplayAlert("Exit?", "Are you sure you want to exit from this page?", "Yes", "No"))
        //        {
        //            base.OnBackButtonPressed();
        //            await App.Current.MainPage.Navigation.PopAsync();
        //        }
        //    });

        //    return true;
        //}
        //protected  override void OnNavigating(object sender, ShellNavigatingEventArgs e)
        //{
        //    // Cancel back navigation if data is unsaved
        //    if (e.Source == ShellNavigationSource.Pop)
        //    {
        //        if (await DisplayAlert("Exit?", "Are you sure you want to exit from this page?", "Yes", "No"))
        //        {
        //            base.OnBackButtonPressed();
        //            await App.Current.MainPage.Navigation.PopAsync();
        //        }
        //        e.Cancel();
        //    }
        //}
       
      
    }
}