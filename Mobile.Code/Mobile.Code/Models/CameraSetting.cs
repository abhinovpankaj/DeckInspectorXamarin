namespace Mobile.Code.Models
{
    public class CameraSetting : BindingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int compression { get; set; }

        private bool _isSelect;

        public bool IsSelected
        {
            get { return _isSelect; }
            set { _isSelect = value; OnPropertyChanged("IsSelected"); }
        }

        // public bool IsSelected { get; set; } = false;
    }
}
