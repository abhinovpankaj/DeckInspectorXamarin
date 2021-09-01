
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.Code.Models
{
    public class Building
    {
        public string BuildingImage { get; set; }
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDescription { get; set; }
        public ObservableCollection<Apartment> Apartments { get; set; }

    }
}
