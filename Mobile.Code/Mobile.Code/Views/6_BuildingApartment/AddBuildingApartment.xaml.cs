﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBuildingApartment : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        public AddBuildingApartment()
        {
            InitializeComponent();
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    recordDes.IsVisible = true;
            //    recordDes.IsEnabled = false;
            //}
            txtName.Completed += (s, e) => txtDes.Focus();
            //this.BindingContext = new BuildingAprartmentAddEditViewModel();
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
                if (btn.ClassId == "recordDes")
                {
                    txtDes.Focus();

                }
                if (btn.ClassId == "recordName")
                {
                    txtName.Focus();

                }
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {

                txtName.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                
                recordDes.IsEnabled = false;
            }

        }
    }
}