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
            // this.BindingContext = vm= new ProjectLocationDetailViewModel();

            //if (App.IsInvasive == true)
            //{
            //    btnNewVisual.IsVisible = btnNewVisualLabel.IsVisible = false;
            //}
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();


            lblInvasive.IsVisible = App.IsInvasive;


            //base.OnAppearing();
            bool complete = await ((ProjectLocationDetailViewModel)this.BindingContext).LoadData();


            //foreach (var item in Shell.Current.Navigation.NavigationStack.ToList())
            //{

            //    if (item.GetType().Name == "AddProjectLocation")


            //    {

            //        Shell.Current.Navigation.RemovePage(item);


            //    }
            //}
            //if (complete == true)
            //{
            //    Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count -2]);
            //   // await Shell.Current.Navigation.PopAsync();
            //}
            // await  vm.LoadData();
        }


    }
}