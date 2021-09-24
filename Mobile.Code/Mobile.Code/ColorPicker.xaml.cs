using Mobile.Code.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPicker : ContentPage
    {
        ColorPickerViewModel vm;
        public ColorPicker()
        {

            InitializeComponent();
            BindingContext = vm = new ColorPickerViewModel();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Send(this, "SelectedColor", vm.SelectedColor);

        }


    }
}