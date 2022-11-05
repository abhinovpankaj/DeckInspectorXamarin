using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

namespace Mobile.Code.Models
{
    public class MultiImage
    {
        [PrimaryKey]
        public string Id { get; set; }
        public MultiImage()
        {
            CreateOn = DateTime.Now;
        }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreateOn { get; set; }
        public byte[] ImageArray { get; set; }

        public string Status { get; set; }
        public string ParentId { get; set; }

        public string ImageType { get; set; }
        public bool IsServerData { get; set; }
        public bool IsDelete { get; set; }

        public string OnlineId { get; set; }

        public bool IsSynced { get; set; }
       // public bool IsLoading { get; set; }
    }

    public class LoadingImage
    {
        public string Id { get; set; }
        //public bool IsLoading { get; set; }
        public ImageSource ImageSourceData { get; set; }
    }
}
