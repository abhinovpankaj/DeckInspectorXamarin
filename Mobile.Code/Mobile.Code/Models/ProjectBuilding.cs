using SQLite;

namespace Mobile.Code.Models
{
    public class ProjectBuilding : BindingModel
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ProjectId { get; set; }


        public bool IsAssign { get; set; }

        public string AssignTo { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string OnlineId { get; set; }



        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public int SeqNo { get; set; }
        public string Username { get; set; }





        private string _bimage;

        public string ImageUrl
        {
            get { return _bimage; }
            set { _bimage = value; OnPropertyChanged("ImageUrl"); }
        }


    }
}
