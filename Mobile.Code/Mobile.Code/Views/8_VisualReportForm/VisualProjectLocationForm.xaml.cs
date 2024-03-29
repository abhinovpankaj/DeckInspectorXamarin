﻿using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualProjectLocationForm : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        //ProjectAddEditViewModel vm;
        public VisualProjectLocationForm()
        {
            InitializeComponent();
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
        }
        private void SpeechToTextFinalResultRecieved(string args)
        {

            if (txtDes.IsFocused)
            {
                txtDes.Text += args;
            }
            if (txtName.IsFocused )
            {
                txtName.Text += args;

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


                recordDes.IsEnabled = false;
            }

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
           // carouselView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal) { ItemSpacing = 10  };
            await ((VisualProjectLocationFormViewModel)this.BindingContext).Load();
            //vm.Load();
        }


        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            VisualProjectLocationFormViewModel vm = (VisualProjectLocationFormViewModel)this.BindingContext;
            vm.CheckAnyRadioButtonChecked = true;
        }
       
    }
}