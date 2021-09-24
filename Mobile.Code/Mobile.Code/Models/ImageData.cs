using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;



namespace Mobile.Code.Models
{
    public class ImageData
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public string ParentID { get; set; }
        public string FormType { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool IsEditVisual { get; set; }
        public VisualProjectLocationPhoto VisualProjectLocationPhoto { get; set; }
        public VisualBuildingLocationPhoto VisualBuildingLocationPhoto { get; set; }
        public VisualApartmentLocationPhoto VisualApartmentLocationPhoto { get; set; }

        public ProjectCommonLocationImages ProjectCommonLocationImages { get; set; }
        public BuildingCommonLocationImages BuildingCommonLocationImages { get; set; }
        public BuildingApartmentImages BuildingApartmentImages { get; set; }
        public ObservableCollection<VisualProjectLocationPhoto> VisualProjectLocationPhotos { get; set; }
        public byte[] mediaFile { get; set; }

        public List<MultiImage> MultiImages { get; set; }
    }
}
