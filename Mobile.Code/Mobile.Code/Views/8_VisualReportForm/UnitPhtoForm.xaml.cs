using Mobile.Code.Models;
using Mobile.Code.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitPhtoForm : ContentPage
    {
        
        //ProjectAddEditViewModel vm;
        public VisualFormType VisualFormType { get; set; }
        public UnitPhtoForm()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            UnitPhotoViewModel vm = ((UnitPhotoViewModel)this.BindingContext);
            await ((UnitPhotoViewModel)this.BindingContext).LoadAsync();
            base.OnAppearing();
            
        }

        private void recordDes_Clicked(object sender, EventArgs e)
        {

        }
       


    }
}