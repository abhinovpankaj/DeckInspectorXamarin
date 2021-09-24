using Mobile.Code.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = vm = new LoginViewModel();
        }
        protected async override void OnAppearing()
        {
            await vm.Load();
            base.OnAppearing();
        }

    }
}