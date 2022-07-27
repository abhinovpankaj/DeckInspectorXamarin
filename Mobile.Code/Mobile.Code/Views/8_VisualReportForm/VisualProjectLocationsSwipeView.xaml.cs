using Mobile.Code.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualProjectLocationsSwipeView : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        //ProjectAddEditViewModel vm;
        public VisualProjectLocationsSwipeView()
        {
            InitializeComponent();
            
        }

    }
}