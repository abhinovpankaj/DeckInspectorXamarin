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
    public partial class BuildingLocation : ContentPage
    {
        public BuildingLocation()
        {
            InitializeComponent();
        }

        private async  void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddbuildingLocation");

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("BuildingLocationDetail");
        }
    }
}