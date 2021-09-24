using Mobile.Code.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        SettingtViewModel vm;
        public SettingPage()
        {
            InitializeComponent();
            this.BindingContext = vm = new SettingtViewModel();

        }
        protected async override void OnAppearing()
        {
            await vm.LoadData();
            base.OnAppearing();
        }

    }
}