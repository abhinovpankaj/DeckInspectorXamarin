using Mobile.Code.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class PopUpCheakListBoxViewModel : BaseViewModel
    {
        private CheakBoxListReturntModel _selectedItems;

        public CheakBoxListReturntModel SelectedItems
        {
            get { return _selectedItems; }
            set { _selectedItems = value; OnPropertyChanged("SelectedItems"); }
        }
        private ObservableCollection<string> _selected;

        public ObservableCollection<string> CheakBoxSelectedItems
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("CheakBoxSelectedItems"); }
        }
        private ObservableCollection<CheckBoxItem> _items;

        public ObservableCollection<CheckBoxItem> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        //  public ObservableCollection<CheckBoxItem> Items { get; set; }
        private CheakBoxElementsType _cheakBoxElementsTypeVar;

        public CheakBoxElementsType CheakBoxElementsType
        {
            get { return _cheakBoxElementsTypeVar; }
            set { _cheakBoxElementsTypeVar = value; OnPropertyChanged("CheakBoxElementsType"); }
        }

        public ICommand AllSelectedCommand { get; private set; }
        //=new Command(async () => await AllSelect());
        private bool _isAllSelect;
        //  public ICommand AllSelectedCommand;
        public bool IsAllSelect
        {
            get { return _isAllSelect; }
            set { _isAllSelect = value; OnPropertyChanged("IsAllSelect"); SelectAll(); }
        }
        private void SelectAll()
        {
            //IsAllSelect = !IsAllSelect;
            //  await Task.Delay(1000);
            foreach (var item in Items)
            {
                item.IsSelected = IsAllSelect;
            }
            // OnPropertyChanged("Items");
        }
        private void AllSelect()
        {
            IsAllSelect = !IsAllSelect;
            //  await Task.Delay(1000);
            //foreach (var item in Items)
            //   {
            //       item.IsSelected = IsAllSelect;
            //   }
            // OnPropertyChanged("Items");
        }

        public ICommand doneCommand => new Command(async () => await done());
        private async Task done()
        {
            CheakBoxSelectedItems = new System.Collections.ObjectModel.ObservableCollection<string>();
            //List<Cheac response = new CheakBoxListReturntModel();
            foreach (var item in Items)
            {
                if (item.IsSelected == true)
                {
                    // response.selectedList.Add(item.Name);
                    CheakBoxSelectedItems.Add(item.Name);
                }
            }
            //response.Count = vm.Items.Where(c => c.IsSelected == true).Count();

            // MessagingCenter.Send(PopUpCheakListBox, "SelectedItem", CheakBoxSelectedItems);
            await Shell.Current.Navigation.PopModalAsync();
            // await Shell.Current.Navigation.PopModalAsync();
            //await Shell.Current.Navigation.PushAsync(new EditProjectLocationImage()
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        public ICommand GoBackCommand => new Command(async () => await GoBack());
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopModalAsync();
            //await Shell.Current.Navigation.PushAsync(new EditProjectLocationImage()
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }
        public ICommand ItemSelectedCommand => new Command<CheckBoxItem>(async (CheckBoxItem parm) => await ItemSelected(parm));
        private async Task ItemSelected(CheckBoxItem parm)
        {
            parm.IsSelected = !parm.IsSelected;
            await Task.FromResult(true);
            ///await Shell.Current.Navigation.PopModalAsync();
            //await Shell.Current.Navigation.PushAsync(new EditProjectLocationImage()
            //{ BindingContext = new EditProjectLocationImageViewModel() { Title = "New Common Location Image", ProjectCommonLocationImages = new ProjectCommonLocationImages() { ImageUrl = "blank.png" }, ProjectLocation = ProjectLocation } });
        }

        public PopUpCheakListBoxViewModel()
        {
            AllSelectedCommand = new Command(AllSelect);
            Items = new ObservableCollection<CheckBoxItem>();

            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Decks", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Porches / Entry", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Stairs", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Stairs Landing", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Walkways", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Railings", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Integrations", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Door Threshold", IsSelected = false });
            Items.Add(new CheckBoxItem() { Id = Guid.NewGuid(), Name = "Stucco Interface", IsSelected = false });

        }
    }
}
