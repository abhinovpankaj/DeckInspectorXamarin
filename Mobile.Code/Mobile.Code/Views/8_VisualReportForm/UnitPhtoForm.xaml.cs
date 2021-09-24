using Mobile.Code.Models;
using Mobile.Code.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitPhtoForm : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        //ProjectAddEditViewModel vm;
        public VisualFormType VisualFormType { get; set; }
        public UnitPhtoForm()
        {
            InitializeComponent();
            recordDes.Clicked += RecordDes_Clicked;
            if (Device.RuntimePlatform == Device.iOS)
            {
                recordDes.IsVisible = true;
                recordDes.IsEnabled = false;
            }
            _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {

                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                ImageButton btn = sender as ImageButton;

                if (btn.ClassId == "recordDes")
                {
                    recordDes.IsEnabled = true;

                }

            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
            //this.BindingContext =vm= new ProjectAddEditViewModel();
        }

        private void RecordDes_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            try
            {


                if (btn.ClassId == "recordDes")
                {
                    txtDes.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception)
            {

            }

            if (Device.RuntimePlatform == Device.iOS)
            {


                recordDes.IsEnabled = false;
            }
        }

        private void SpeechToTextFinalResultRecieved(string args)
        {

            if (txtDes.IsFocused)
            {
                txtDes.Text += args;
            }
        }

        private void recordClick(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            try
            {

                if (btn.ClassId == "recordDes")
                {
                    txtDes.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception)
            {


            }

            if (Device.RuntimePlatform == Device.iOS)
            {


                recordDes.IsEnabled = false;
            }

        }
        protected async override void OnAppearing()
        {
            UnitPhotoViewModel vm = ((UnitPhotoViewModel)this.BindingContext);
            await ((UnitPhotoViewModel)this.BindingContext).LoadAsync();


            base.OnAppearing();
            //vm.Load();
        }

        private void recordDes_Clicked(object sender, EventArgs e)
        {

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