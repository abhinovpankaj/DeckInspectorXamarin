﻿using Mobile.Code.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        ProjectViewModel vm;
        public HomePage()
        {
            InitializeComponent();

            this.BindingContext = vm = new ProjectViewModel();
        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            // vm.IsBusyProgress = true;
            //DependencyService.Get<ILodingPageService>().InitLoadingPage(new LoadingIndicatorPage1());
            //DependencyService.Get<ILodingPageService>().ShowLoadingPage();
            await vm.LoadData();

        }


    }
}