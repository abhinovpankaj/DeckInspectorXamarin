namespace CodeRepo.Mobile.Models
{
    public enum MenuItemType
    {
        Projects,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
