using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Code.Models
{
    public class CustomRadioItem:BindingModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        private bool _isChk;

        public string GroupName { get; set; }
        public bool IsChecked
        {
            get { return _isChk; }
            set { _isChk = value; OnPropertyChanged("IsSelected"); }
        }

    }
}
