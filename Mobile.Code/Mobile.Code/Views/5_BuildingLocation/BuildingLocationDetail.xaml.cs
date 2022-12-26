using Mobile.Code.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingLocationDetail : ContentPage
    {
        private Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        //  BuildingLocationDetailViewModel vm;
        public BuildingLocationDetail()
        {
            InitializeComponent();
          

        }
        protected override void OnAppearing()
        {
            lblInvasive.IsVisible = App.IsInvasive;
            base.OnAppearing();
            var vm = (BuildingLocationDetailViewModel)this.BindingContext;

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
            
            
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            tokenSource.Cancel();
        }
    }
}