

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.Code.Models
{
    public class Apartment
    {
        public string AptImage { get; set; }
        public string AptId { get; set; }
        public string AptName { get; set; }
        public string AptDescription { get; set; }

        public ObservableCollection<WorkArea> Locations { get; set; }

    }
}
