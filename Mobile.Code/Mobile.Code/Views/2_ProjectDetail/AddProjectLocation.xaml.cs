using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProjectLocation : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        public AddProjectLocation()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                recordDes.IsVisible = true;
                recordDes.IsEnabled = false;
            }
            txtName.Completed += (s, e) => txtDes.Focus();
            _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {

                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                ImageButton btn = sender as ImageButton;
                if (btn.ClassId == "recordName")
                {
                    recordName.IsEnabled = true;

                }

                else if (btn.ClassId == "recordDes")
                {
                    recordDes.IsEnabled = true;

                }

            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

            //this.BindingContext = new ProjectLocationAddEditViewModel();
        }
        private void SpeechToTextFinalResultRecieved(string args)
        {
            if (txtName.IsFocused)
            {
                txtName.Text += args;
            }

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
                if (btn.ClassId == "recordName")
                {
                    txtName.Focus();

                }

                else if (btn.ClassId == "recordDes")
                {
                    txtDes.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {

                txtName.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                recordName.IsEnabled = false;

                recordDes.IsEnabled = false;
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //   Shell.Current.Navigation.RemovePage(this);
        }
    }
}