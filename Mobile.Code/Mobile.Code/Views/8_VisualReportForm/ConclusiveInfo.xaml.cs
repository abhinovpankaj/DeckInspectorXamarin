using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConclusiveInfo : ContentView
    {
        private ISpeechToText _speechRecongnitionInstance;
        
        public ConclusiveInfo(object vm)
        {
            InitializeComponent();
           
            ConclusiveDetails.IsVisible = false;
            if (vm.GetType() == typeof(VisualProjectLocationFormViewModel))
            {
                var viewModel = vm as VisualProjectLocationFormViewModel;
                if (viewModel != null)
                {
                    this.BindingContext = viewModel;
                    if (viewModel.VisualForm.IsInvasiveRepairComplete)
                    {
                        ConclusiveDetails.IsVisible = true;
                    }
                }
                   
            }
            if (vm.GetType() == typeof(VisualBuildingLocationFormViewModel))
            {
                var viewModel = vm as VisualBuildingLocationFormViewModel;
                if (viewModel != null)
                {
                    this.BindingContext = viewModel;
                    if (viewModel.VisualForm.IsInvasiveRepairComplete)
                    {
                        ConclusiveDetails.IsVisible = true;
                    }
                }
            }
            if (vm.GetType() == typeof(VisualApartmentFormViewModel))
            {
                var viewModel = vm as VisualApartmentFormViewModel;
                if (viewModel != null)
                {
                    this.BindingContext = viewModel;
                    if (viewModel.VisualForm.IsInvasiveRepairComplete)
                    {
                        ConclusiveDetails.IsVisible = true;
                    }
                }
            }

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
            catch (Exception)
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
            catch (Exception)
            {


            }

            if (Device.RuntimePlatform == Device.iOS)
            {

                recordDesInv.IsEnabled = false;
            }

        }

        private void repairCOmplete_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var selected = sender as RadioButton;
           
            ConclusiveDetails.IsVisible = selected.Content.ToString()=="Yes";

            var source= BindableLayout.GetItemsSource(ownerControl);
            if(selected.Content.ToString() == "Yes")
            {
                var enumerator = source.GetEnumerator();
                enumerator.MoveNext();
                Models.CustomRadioItem radios = enumerator.Current as Models.CustomRadioItem;
                
                
                radios.IsSelected = true;
            }
                

        }
    }
}