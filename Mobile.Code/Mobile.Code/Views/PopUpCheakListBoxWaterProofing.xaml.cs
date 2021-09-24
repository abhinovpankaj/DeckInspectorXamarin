using Mobile.Code.ViewModels;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Code.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpCheakListBoxWaterProofing : ContentPage
    {
        // PopUpCheakListBoxViewModel vm;
        public PopUpCheakListBoxWaterProofing()
        {
            InitializeComponent();
            //  this.BindingContext =vm= new PopUpCheakListBoxViewModel();
        }
        protected override void OnDisappearing()
        {

            /* PopUpCheakListBoxWaterproofingViewModel vm =(PopUpCheakListBoxWaterproofingViewModel) this.BindingContext;
             vm.CheakBoxSelectedItems = new System.Collections.ObjectModel.ObservableCollection<string>();
             //List<Cheac response = new CheakBoxListReturntModel();
             foreach (var item in vm.Items)
             {
                 if (item.IsSelected == true)
                 {
                     // response.selectedList.Add(item.Name);
                     vm.CheakBoxSelectedItems.Add(item.Name);
                 }
             }*/
            //response.Count = vm.Items.Where(c => c.IsSelected == true).Count();

            // MessagingCenter.Send(this, "SelectedItem", vm.CheakBoxSelectedItems);
            base.OnDisappearing();
        }
        protected override void OnAppearing()
        {

            PopUpCheakListBoxWaterproofingViewModel vm = (PopUpCheakListBoxWaterproofingViewModel)this.BindingContext;
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
            PopUpCheakListBoxWaterproofingViewModel vm = (PopUpCheakListBoxWaterproofingViewModel)this.BindingContext;
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