
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBuildingImage : ContentPage
    {
        public AddBuildingImage()
        {
            InitializeComponent();
            //this.BindingContext = new ProjectAddEditViewModel();
        }
    }
}