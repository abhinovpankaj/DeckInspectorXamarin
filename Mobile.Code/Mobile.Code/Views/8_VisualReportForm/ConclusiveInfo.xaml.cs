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
    public partial class ConclusiveInfo : ContentView
    {
        private ISpeechToText _speechRecongnitionInstance;
        public ConclusiveInfo()
        {
            InitializeComponent();
            
            recordDesInv.Clicked += recordDesInv_Clicked;
            if (Device.RuntimePlatform == Device.iOS)
            {
                recordDesInv.IsVisible = true;
                recordDesInv.IsEnabled = false;
            }
            _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {

                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                ImageButton btn = sender as ImageButton;

                if (btn.ClassId == "recordDesInv")
                {
                    recordDesInv.IsEnabled = true;

                }

            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
        }
        private void recordDesInv_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            try
            {


                if (btn.ClassId == "recordDesInv")
                {
                    txtDesConclusive.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception )
            {

            }

            if (Device.RuntimePlatform == Device.iOS)
            {


                recordDesInv.IsEnabled = false;
            }
        }

        private void SpeechToTextFinalResultRecieved(string args)
        {

            if (txtDesConclusive.IsFocused)
            {
                txtDesConclusive.Text += args;
            }
        }

        private void recordClick(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            try
            {

                if (btn.ClassId == "recordDesInv")
                {
                    txtDesConclusive.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception )
            {


            }

            if (Device.RuntimePlatform == Device.iOS)
            {


                recordDesInv.IsEnabled = false;
            }

        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}