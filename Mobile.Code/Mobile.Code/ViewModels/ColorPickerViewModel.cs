using Mobile.Code.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class ColorPickerViewModel : BaseViewModel
    {
        private string _sc;

        public string SelectedColor
        {
            get { return _sc; }
            set { _sc = value; OnPropertyChanged("SelectedColor"); }
        }

        public ObservableCollection<ColorSet> _bcimage { get; set; }

        public ObservableCollection<ColorSet> Colors
        {
            get { return _bcimage; }
            set { _bcimage = value; OnPropertyChanged("Colors"); }
        }
        public ICommand GoBackCommand => new Command(async () => await GoBack());
        private async Task GoBack()
        {
            await Shell.Current.Navigation.PopModalAsync();

        }
        public ICommand ColorCommand => new Command<ColorSet>(async (ColorSet obj) => await ColorCommandExecute(obj));
        private async Task ColorCommandExecute(ColorSet obj)
        {
            SelectedColor = obj.Color;

            await Shell.Current.Navigation.PopModalAsync();
        }
        public ColorPickerViewModel()
        {
            SelectedColor = "#FF0000";

            Colors = new ObservableCollection<ColorSet>();

            Colors.Add(new ColorSet() { Color = "#FFFFFF" });
            Colors.Add(new ColorSet() { Color = "#C0C0C0" });
            Colors.Add(new ColorSet() { Color = "#00FF00" });
            Colors.Add(new ColorSet() { Color = "#808080" });
            Colors.Add(new ColorSet() { Color = "#000000" });
            Colors.Add(new ColorSet() { Color = "#FF0000" });
            Colors.Add(new ColorSet() { Color = "#800000" });
            Colors.Add(new ColorSet() { Color = "#FFFF00" });

            Colors.Add(new ColorSet() { Color = "#808000" });
            Colors.Add(new ColorSet() { Color = "#00FF00" });
            Colors.Add(new ColorSet() { Color = "#008000" });
            Colors.Add(new ColorSet() { Color = "#000000" });
            Colors.Add(new ColorSet() { Color = "#00FFFF" });
            Colors.Add(new ColorSet() { Color = "#008080" });


            Colors.Add(new ColorSet() { Color = "#0000FF" });
            Colors.Add(new ColorSet() { Color = "#000080" });
            Colors.Add(new ColorSet() { Color = "#FF00FF" });
            Colors.Add(new ColorSet() { Color = "#800080" });




        }
    }
}
