namespace Mobile.Code.Models
{
    public class CustomRadioItem : BindingModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        private bool _isChk;

        public string GroupName { get; set; }
        public bool IsSelected
        {
            get { return _isChk; }
            set
            {
                //if (_isChk != value)
                //{
                _isChk = value;
                OnPropertyChanged("IsSelected");
                //}

            }
        }

    }
}
