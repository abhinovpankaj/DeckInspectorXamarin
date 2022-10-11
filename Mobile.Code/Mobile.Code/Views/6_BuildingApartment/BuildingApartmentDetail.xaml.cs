using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingApartmentDetail : ContentPage
    {


        public BuildingApartmentDetail()
        {
            InitializeComponent();

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            await ((BuildingApartmentDetailViewModel)this.BindingContext).LoadData();

        }

        private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                
                    gridtoolbar.TranslateTo(0, e.VerticalDelta);
                
            });
        }
    }
}