using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectAddEdit : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        //ProjectAddEditViewModel vm;
        public ProjectAddEdit()
        {
            InitializeComponent();
            int t = 0;
            _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();

            if (Device.RuntimePlatform == Device.iOS)
            {
                recordDes.IsVisible = true;
                recordDes.IsEnabled = false;
            }
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
                else if (btn.ClassId == "recordAddress")
                {
                    recordAddress.IsEnabled = true;

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

            txtName.Completed += (s, e) => txtAddress.Focus();
            txtAddress.Completed += (s, e) => txtDes.Focus();
            //this.BindingContext =vm= new ProjectAddEditViewModel();
        }
        private void SpeechToTextFinalResultRecieved(string args)
        {
            if (txtName.IsFocused)
            {
                txtName.Text += args;
            }
            if (txtAddress.IsFocused)
            {
                txtAddress.Text += args;
            }
            if (txtDes.IsFocused)
            {
                txtDes.Text += args;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //vm.Load();
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
                else if (btn.ClassId == "recordAddress")
                {
                    txtAddress.Focus();

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
                recordAddress.IsEnabled = false;
                recordDes.IsEnabled = false;
            }

        }
       
    }
}