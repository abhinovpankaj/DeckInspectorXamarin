using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingLocationDetail : ContentPage
    {

        //  BuildingLocationDetailViewModel vm;
        public BuildingLocationDetail()
        {
            InitializeComponent();
          

        }
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            await ((BuildingLocationDetailViewModel)this.BindingContext).LoadData();
            //vm.LoadData();
        }
 
    }
}