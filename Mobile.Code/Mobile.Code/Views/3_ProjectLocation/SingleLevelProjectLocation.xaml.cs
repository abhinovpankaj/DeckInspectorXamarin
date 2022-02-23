using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views._3_ProjectLocation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleLevelProjectLocation : ContentPage
    {
        public SingleLevelProjectLocation()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            lblInvasive.IsVisible = App.IsInvasive;

            //base.OnAppearing();
            bool complete = await ((SingleLevelProjectDetailViewModel)BindingContext).LoadData();


        }

        private async void offlineProjectPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (offlineProjectPicker.SelectedIndex > -1)
            {
                //Project prj = offlineProjectPicker.Items[offlineProjectPicker.SelectedIndex];

                SingleLevelProjectDetailViewModel vm = (SingleLevelProjectDetailViewModel)BindingContext;
                await vm.PushProjectToServer();
                vm.IsPickerVisible = false;
            }

            //offlineProjectPicker.IsEnabled = false;
            //offlineProjectPicker.IsVisible = false;
        }
    }
}