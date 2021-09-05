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
        public TabbedPageInvasive()
        {
            InitializeComponent();


            //if (vm.GetType() == typeof(VisualProjectLocationFormViewModel))
            //{
            //    var viewModel = vm as VisualProjectLocationFormViewModel;
            //    this.BindingContext = viewModel;
            //}
            //if (vm.GetType() == typeof(VisualBuildingLocationFormViewModel))
            //{
            //    var viewModel = vm as VisualBuildingLocationFormViewModel;
            //    this.BindingContext = viewModel;
            //}
            //if (vm.GetType() == typeof(VisualApartmentFormViewModel))
            //{
            //    var viewModel = vm as VisualApartmentFormViewModel;
            //    this.BindingContext = viewModel;
            //}

            //AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
           // AddRemoveConclusiveTab();

        }
        VisualApartmentFormViewModel localVM;
        VisualProjectLocationFormViewModel localLocVM;
        public TabbedPageInvasive(VisualProjectLocationFormViewModel vm)
        {
            vm.IsBusy = true;
            InitializeComponent();

            localLocVM = vm;
            this.BindingContext = localLocVM;

            //AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
            //AddRemoveConclusiveTab();
            vm.IsBusy = false;
        }
        public TabbedPageInvasive(VisualBuildingLocationFormViewModel vm)
        {
            vm.IsBusy = true;
            InitializeComponent();

            //localLocVM = vm;
            this.BindingContext = vm;

            //AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
            //AddRemoveConclusiveTab();
            vm.IsBusy = false;
        }
        public TabbedPageInvasive(VisualApartmentFormViewModel vm)
        {
            vm.IsBusy = true;
            InitializeComponent();
            localVM = vm;

            this.BindingContext = localVM;

            //AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
            //AddRemoveConclusiveTab();
            vm.IsBusy = false;
        }
        protected async override void OnAppearing()
        {

            if (this.BindingContext is VisualProjectLocationFormViewModel)
                await ((VisualProjectLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualBuildingLocationFormViewModel)
                await ((VisualBuildingLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualApartmentFormViewModel)
                await ((VisualApartmentFormViewModel)this.BindingContext).Load();
            base.OnAppearing();

            //AddRemoveConclusiveTab();

        }

        private void AdditionalInvasive_AllowFurthurInvasive(object sender, EventArgs e)
        {
            //AddRemoveConclusiveTab();
        }
        private bool added;
        private void AddRemoveConclusiveTab()
        {
            try
            {
                var vm = this.BindingContext;
                
                if (vm.GetType() == typeof(VisualProjectLocationFormViewModel))
                {
                    var viewModel = vm as VisualProjectLocationFormViewModel;
                    if (viewModel != null)
                    {
                        if (viewModel.VisualForm.IsPostInvasiveRepairsRequired)
                        {
                            if (!added)
                            {
                                TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(viewModel));

                                tabbedControl.AddTab(conclusiveTab, 2, false);
                                added = true;
                            }
                            
                        }
                        else
                        {
                            if (added)
                            {
                                tabbedControl.RemoveTab(2);
                                added = false;
                            }
                            
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
                            if (!added)
                            {
                                TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(viewModel));

                                tabbedControl.AddTab(conclusiveTab, 2, false);
                                added = true;
                            }

                        }
                        else
                        {
                            if (added)
                            {
                                tabbedControl.RemoveTab(2);
                                added = false;
                            }
                        }

                    }
                }
                if (vm.GetType() == typeof(VisualApartmentFormViewModel))
                {
                    var viewModel = vm as VisualApartmentFormViewModel;
                    if (viewModel != null)
                    {
                        if (viewModel.VisualForm.IsPostInvasiveRepairsRequired)
                        {
                            if (!added)
                            {
                                TabItem conclusiveTab = new TabItem("Conclusive", new ConclusiveInfo(viewModel));

                                tabbedControl.AddTab(conclusiveTab, 2, false);
                                added = true;
                            }

                        }
                        else
                        {
                            if (added)
                            {
                                tabbedControl.RemoveTab(2);
                                added = false;
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}