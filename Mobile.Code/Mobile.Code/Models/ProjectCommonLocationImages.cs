using SQLite;

namespace Mobile.Code.Models
{
    public class ProjectCommonLocationImages : BindingModel
    {

        private string _image;

        public string ImageUrl
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("ImageUrl"); }
        }
        [PrimaryKey]
        public string Id { get; set; }
        public string ProjectLocationId { get; set; }

        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public int SeqNo { get; set; }
    }
}
