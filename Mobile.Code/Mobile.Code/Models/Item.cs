namespace Mobile.Code.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class ItemImage
    {
        public string ImageID { get; set; }

        public string Name { get; set; }
        public string ImageDescription { get; set; }

        public string ImageNotes { get; set; }
        public string ImagePath { get; set; }

    }
    public enum ListLayoutOptions
    {
        Big,
        Small
    }
}