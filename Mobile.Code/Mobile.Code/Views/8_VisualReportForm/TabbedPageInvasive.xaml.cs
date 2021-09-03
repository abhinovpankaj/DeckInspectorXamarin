using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.TabView;
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
 
            this.BindingContext = vm;
            //AddRemoveConclusiveTab();

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
            AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
        }

        private void AdditionalInvasive_AllowFurthurInvasive(object sender, EventArgs e)
        {
            AddRemoveConclusiveTab();
        }

        private void AddRemoveConclusiveTab()
        {
            var vm = this.BindingContext;
            var ProjectFormVM = vm as VisualProjectLocationFormViewModel;
            if (vm.GetType() == typeof(VisualProjectLocationFormViewModel))
            {
                var viewModel = vm as VisualProjectLocationFormViewModel;
                if (viewModel != null)
                {
                    if (viewModel.VisualForm.IsPostInvasiveRepairsRequired)
                    {
                        TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(vm));
                        tabbedControl.AddTab(conclusiveTab, 2, false);
                    }
                    else
                    {                       
                        tabbedControl.RemoveTab(2);                       
                    }
                }
            }

            if (vm.GetType() == typeof(VisualBuildingLocationFormViewModel))
            {
                var viewModel = vm as VisualBuildingLocationFormViewModel;
                if (viewModel != null)
                {
                    if (viewModel.VisualForm.IsPostInvasiveRepairsRequired)
                    {
                        TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(vm));
                        tabbedControl.AddTab(conclusiveTab, 2, false);
                    }
                    else
                        tabbedControl.RemoveTab(2);

                }
            }
            if (vm.GetType() == typeof(VisualApartmentFormViewModel))
            {
                var viewModel = vm as VisualApartmentFormViewModel;
                if (viewModel != null)
                {
                    if (viewModel.VisualForm.IsPostInvasiveRepairsRequired)
                    {
                        TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(vm));
                        tabbedControl.AddTab(conclusiveTab, 2, false);
                    }
                    else
                        tabbedControl.RemoveTab(2);

                }
            }
        }
    }
}