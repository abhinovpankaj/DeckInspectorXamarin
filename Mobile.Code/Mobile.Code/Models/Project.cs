using SQLite;

namespace Mobile.Code.Models
{
    public class Project : BindingModel
    {

        // public ProjectType ProjectType { get; set; }
        //  public string ProjectImage { get; set; }
        private string _pimage;

        public string ImageUrl
        {
            get { return _pimage; }
            set { _pimage = value; OnPropertyChanged("ImageUrl"); }
        }
        public string ProjectType { get; set; }
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string Address { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }

        private bool _inv;

        public bool IsInvasive
        {
            get { return _inv; }
            set { _inv = value; OnPropertyChanged("IsInvasive"); }
        }
        public bool IsAssignVisible { get; set; }
        private bool _isVisible;

        public bool IsVisible
        {
            get
            {
                if (IsInvasive == false)
                {
                    return false;
                }
                else if (App.LogUser.Id.ToString() == UserId && IsInvasive == true || App.LogUser.RoleName == "Admin" && IsInvasive == true)
                {
                    return true;
                }
                return false;

            }
            set { _isVisible = value; OnPropertyChanged("IsVisible"); }
        }


        public string CreatedOn { get; set; }
        public string InvasiveProjectID { get; set; }
        public bool IsInvaisveExist { get; set; }

        private bool _IsAccess;

        public bool IsAccess
        {
            get { return _IsAccess; }
            set { _IsAccess = value; OnPropertyChanged("IsAccess"); }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; OnPropertyChanged("Category"); }
        }
       

        private bool _isSynced;

        public bool IsSynced
        {
            get { return _isSynced; }
            set { _isSynced = value; OnPropertyChanged("IsSynced"); }
        }
        
    }

    public enum ProjectCategory
    {
        MultiLevel,
        SingleLevel
    }
}
