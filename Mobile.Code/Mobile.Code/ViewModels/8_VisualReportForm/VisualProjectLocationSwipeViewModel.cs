using Mobile.Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.Code.ViewModels
{
    public class VisualProjectLocationSwipeViewModel : BaseViewModel
    {
        private ObservableCollection<ProjectLocation_Visual> _visualFormProjectLocationItems;
        public ObservableCollection<ProjectLocation_Visual> VisualFormProjectLocationItems
        {
            get { return _visualFormProjectLocationItems; }
            set { _visualFormProjectLocationItems = value; OnPropertyChanged("VisualFormProjectLocationItems"); }
        }
    }
}
