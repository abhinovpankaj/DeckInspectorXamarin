﻿using Mobile.Code.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.Plugin.TabView;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageInvasive : ContentPage
    {
        public TabbedPageInvasive(object vm)
        {
            InitializeComponent();
            loadingcontrol.IsVisible = true;
            LoadAllTabs(vm);
            loadingcontrol.IsVisible = false;
        }

        private void  LoadAllTabs(object vm)
        {
            
                if (vm.GetType() == typeof(VisualProjectLocationFormViewModel))
                {
                    var viewModel = vm as VisualProjectLocationFormViewModel;
                    this.BindingContext = viewModel;
                    TabItem visualTab = new TabItem("Visual", new InvasiveVisualProjectLocationForm(viewModel));

                    tabbedControl.AddTab(visualTab);
                }
                if (vm.GetType() == typeof(VisualBuildingLocationFormViewModel))
                {
                    var viewModel = vm as VisualBuildingLocationFormViewModel;
                    this.BindingContext = viewModel;
                    TabItem visualTab = new TabItem("Visual", new InvasiveVisualBuildingLocationForm(viewModel));

                    tabbedControl.AddTab(visualTab);
                }
                if (vm.GetType() == typeof(VisualApartmentFormViewModel))
                {
                    var viewModel = vm as VisualApartmentFormViewModel;
                    this.BindingContext = viewModel;
                    TabItem visualTab = new TabItem("Visual", new InvasiveVisualApartmentLocationForm(viewModel));

                    tabbedControl.AddTab(visualTab);
                }

                AdditionalInvasive.AllowFurthurInvasive += AdditionalInvasive_AllowFurthurInvasive;
                AddRemoveConclusiveTab();
           

        }
        public TabbedPageInvasive( )
        {
            InitializeComponent();
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext is VisualProjectLocationFormViewModel)
                await ((VisualProjectLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualBuildingLocationFormViewModel)
                await ((VisualBuildingLocationFormViewModel)this.BindingContext).Load();
            if (this.BindingContext is VisualApartmentFormViewModel)
                await ((VisualApartmentFormViewModel)this.BindingContext).Load();
            

            //AddRemoveConclusiveTab();

        }

        private void AdditionalInvasive_AllowFurthurInvasive(object sender, EventArgs e)
        {
            AddRemoveConclusiveTab();
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

                                tabbedControl.AddTab(conclusiveTab);
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

                                tabbedControl.AddTab(conclusiveTab);
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

                                tabbedControl.AddTab(conclusiveTab);
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