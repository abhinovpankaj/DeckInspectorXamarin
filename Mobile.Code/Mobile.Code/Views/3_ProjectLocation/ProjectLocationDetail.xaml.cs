using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.Code.Data;
using Mobile.Code.Models;
using Mobile.Code.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectLocationDetail : ContentPage
    {

        //  ProjectLocationDetailViewModel vm;
        public ProjectLocationDetail()
        {
            InitializeComponent();
           
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
         
            
             lblInvasive.IsVisible = App.IsInvasive;
            

            //base.OnAppearing();
            bool complete = await ((ProjectLocationDetailViewModel)this.BindingContext).LoadData();


        }


    }
}