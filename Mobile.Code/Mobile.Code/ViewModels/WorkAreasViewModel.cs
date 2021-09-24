using Mobile.Code.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Mobile.Code.ViewModels
{
    public class WorkAreasViewModel : BaseViewModel
    {
        private WorkArea _workArea;

        public WorkArea WorkArea
        {
            get { return _workArea; }
            set { _workArea = value; OnPropertyChanged("WorkArea"); }
        }

        public Command ProjectDetailCommand { get; set; }

        public ObservableCollection<ItemImage> WorkAreaImages { get; set; }

        public WorkAreasViewModel(WorkArea parm)
        {
            WorkArea = parm;

        }
        public void LoadData()
        {



        }
    }

}
