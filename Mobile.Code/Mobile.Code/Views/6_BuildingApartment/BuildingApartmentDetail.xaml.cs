using Mobile.Code.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingApartmentDetail : ContentPage
    {


        public BuildingApartmentDetail()
        {
            InitializeComponent();

        }
        private Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        protected async override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            var vm = (BuildingApartmentDetailViewModel)this.BindingContext;

            if (task != null && (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingToRun
                || task.Status == TaskStatus.WaitingForActivation))
            {
                //Debug.WriteLine("Task has attempted to start while already running");
            }
            else
            {
                var token = tokenSource.Token;
                task = Task.Run(async () =>
                {
                    await vm.Running(token);
                }, token);
            }
            await ((BuildingApartmentDetailViewModel)this.BindingContext).LoadData();

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            tokenSource.Cancel();
        }
        //private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        //{
        //    Device.BeginInvokeOnMainThread(() => {

        //            gridtoolbar.TranslateTo(0, e.VerticalDelta);

        //    });
        //}
    }
}