using Mobile.Code.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectLocationDetail : ContentPage
    {

        //  ProjectLocationDetailViewModel vm;
        private Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public ProjectLocationDetail()
        {
            InitializeComponent();           
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblInvasive.IsVisible = App.IsInvasive;
            var vm = (ProjectLocationDetailViewModel)this.BindingContext;            

            if (task != null && (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingToRun
                || task.Status == TaskStatus.WaitingForActivation))
            {
                Debug.WriteLine("Task has attempted to start while already running");
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