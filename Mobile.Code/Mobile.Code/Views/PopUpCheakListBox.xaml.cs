﻿using Mobile.Code.Models;
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
    public partial class PopUpCheakListBox : ContentPage
    {
       // PopUpCheakListBoxViewModel vm;
        public PopUpCheakListBox()
        {
            InitializeComponent();
          //  this.BindingContext =vm= new PopUpCheakListBoxViewModel();
        }
     
        protected override void OnAppearing()
        {

            PopUpCheakListBoxViewModel vm = (PopUpCheakListBoxViewModel)this.BindingContext;
            if (vm.CheakBoxSelectedItems.Count != 0)
            {
                foreach (var item in vm.CheakBoxSelectedItems)
                {
                    vm.Items.Where(c => c.Name == item).Single().IsSelected = true;
                }
            }
            if (vm.CheakBoxSelectedItems.Count == vm.Items.Count)
            {
                vm.IsAllSelect = true;
            }
            base.OnAppearing();
        }

        void btnDone_Clicked(System.Object sender, System.EventArgs e)
        {
            PopUpCheakListBoxViewModel vm = (PopUpCheakListBoxViewModel)this.BindingContext;
            vm.CheakBoxSelectedItems = new System.Collections.ObjectModel.ObservableCollection<string>();
            //List<Cheac response = new CheakBoxListReturntModel();
            foreach (var item in vm.Items)
            {
                if (item.IsSelected == true)
                {
                    // response.selectedList.Add(item.Name);
                    vm.CheakBoxSelectedItems.Add(item.Name);
                }
            }
            //response.Count = vm.Items.Where(c => c.IsSelected == true).Count();

            MessagingCenter.Send(this, "SelectedItem", vm.CheakBoxSelectedItems);
            vm.GoBackCommand.Execute(null);
        }
    }
}