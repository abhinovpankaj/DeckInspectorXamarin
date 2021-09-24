using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class ShowImageViewModel : BaseViewModel
    {
        private string _imgUrl;

        public string ImageUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _des;

        public string Description
        {
            get { return _des; }
            set { _des = value; }
        }

        private string CreatedOn;

        public string MyProperty
        {
            get { return CreatedOn; }
            set { CreatedOn = value; }
        }

        public ICommand ClosePageCommand { get; set; }
        private async void ClosePageCommandExecute(object obj)
        {

            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        public ShowImageViewModel(string Image, string name, string des, string _date)
        {
            ClosePageCommand = new Command(ClosePageCommandExecute);
            ImageUrl = Image;
            Name = name;
            Description = des;
            CreatedOn = _date;
        }
    }
}
