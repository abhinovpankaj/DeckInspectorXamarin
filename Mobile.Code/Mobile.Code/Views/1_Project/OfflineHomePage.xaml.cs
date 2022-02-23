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
    public partial class OfflineHomePage : ContentPage
    {
        OfflineProjectViewModel vm;
        public OfflineHomePage()
        {
            InitializeComponent();
            this.BindingContext = vm = new OfflineProjectViewModel();
        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            
            await vm.LoadData();

        }
    }
}