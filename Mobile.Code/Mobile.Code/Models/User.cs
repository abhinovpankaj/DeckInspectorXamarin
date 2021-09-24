using System;

namespace Mobile.Code.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }
        public string UserName { get; set; }

        public string ActiveStatus { get; set; }
        public bool IsOriginal { get; set; }


        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }


        public string IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ErrNo { get; set; }
        public string ErrMsg { get; set; }
    }
}
