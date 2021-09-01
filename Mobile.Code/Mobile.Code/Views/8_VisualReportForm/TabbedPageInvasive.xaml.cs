using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views._8_VisualReportForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageInvasive : ContentPage
    {
        public TabbedPageInvasive(BaseViewModel vm)
        {
            InitializeComponent();
          //  On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.BindingContext = vm;
            //if(vm is VisualProjectLocationFormViewModel )
            //{
            //    Children.Add(new InvasiveVisualProjectLocationForm() { Title = "Invasive",BindingContext=vm });
            //    Children.Add(new AdditionalInvasive() { Title = "Detail",BindingContext = vm });
            //}
            //if (vm is VisualBuildingLocationFormViewModel)
            //{
            //    Children.Add(new InvasiveVisualBuildingLocationForm() { Title = "Invasive", BindingContext = vm });
            //    Children.Add(new AdditionalInvasive() { Title = "Detail", BindingContext = vm });
            //}
            //if (vm is VisualBuildingLocationFormViewModel)
            //{
            //    Children.Add(new InvasiveVisualBuildingLocationForm() { Title = "Invasive", BindingContext = vm });
            //    Children.Add(new AdditionalInvasive() { Title = "Detail", BindingContext = vm });
            //}
            // NavigationPage navigationPage = new NavigationPage(new InvasiveVisualProjectLocationForm() { BindingContext = vm });
            //  navigationPage.IconImageSource = "schedule.png";
            //   navigationPage.Title = "ProjectLocation";
            // Children.Add(new InvasiveVisualProjectLocationForm() {  Title = "Invasive" });
            //InvasiveVisualProjectLocationFormViewModel vmInvasive = new InvasiveVisualProjectLocationFormViewModel();
            //vmInvasive.VisualProjectLocationPhotoItems =new System.Collections.ObjectModel.ObservableCollection<Models.VisualProjectLocationPhoto>( vm.VisualProjectLocationPhotoItems.Where(c=>c.InvasiveImage==true));
            ////vm.WaterProofingElements.selectedList = parm.ExteriorElements.Split(',').ToList();
            //vmInvasive.VisualForm = vm.VisualForm;

            //vmInvasive.ProjectLocation = vm.ProjectLocation;
            //   Children.Add(new AdditionalInvasive() {Title= "Detail"});

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // var q=this.BindingContext;
            if (this.BindingContext is VisualProjectLocationFormViewModel)
                await ((VisualProjectLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualBuildingLocationFormViewModel)
                await ((VisualBuildingLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualApartmentFormViewModel)
                await ((VisualApartmentFormViewModel)this.BindingContext).Load();
            //vm.Load();
        }
    }
}