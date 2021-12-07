using SQLite;

namespace Mobile.Code.Models
{
    public class BuildingApartment : BindingModel
    {

        private string _img;

        public string ImageUrl
        {
            get { return _img; }
            set { _img = value; OnPropertyChanged("ImageUrl"); }
        }


        [PrimaryKey]
        public string Id { get; set; }
        public string BuildingId { get; set; }


        public string OnlineId { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }




        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public int SeqNo { get; set; }
        public string Username { get; set; }
    }
}
