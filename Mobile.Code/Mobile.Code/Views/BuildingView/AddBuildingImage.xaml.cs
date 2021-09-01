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
    public partial class AddBuildingImage : ContentPage
    {
        public AddBuildingImage()
        {
            InitializeComponent();
            //this.BindingContext = new ProjectAddEditViewModel();
        }
    }
}