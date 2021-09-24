using System;

namespace Mobile.Code.Models
{
    public class CheckBoxItem : BindingModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }

    }
}
