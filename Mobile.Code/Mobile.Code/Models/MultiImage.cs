using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code.Models
{
    public class MultiImage
    {
        public string Id { get; set; }
        public MultiImage()
        {
            CreateOn = DateTime.Now;
        }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreateOn { get; set; }
        public byte[] ImageArray { get; set; }

        public string Status{ get; set; }
        public string ParentId { get; set; }

        public string ImageType { get; set; }
        public bool IsServerData { get; set; }
        public bool IsDelete { get; set; }

    }
}
