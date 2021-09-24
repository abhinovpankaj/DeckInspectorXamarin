using System.Collections.ObjectModel;

namespace Mobile.Code.Models
{
    public class WorkArea
    {
        public ObservableCollection<ItemImage> WorkAreaImages { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

    }
}