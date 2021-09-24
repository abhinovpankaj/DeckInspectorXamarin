﻿namespace Mobile.Code.Models
{
    public class BuildingLocation : BindingModel

    {
        private string _image;
        public string ImageUrl
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("ImageUrl"); }
        }
        public string Id { get; set; }
        public string BuildingId { get; set; }





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
