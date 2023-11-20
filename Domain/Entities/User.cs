using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }
        public UserGroup Group { get; set; }
        public UserState State { get; set; }
        public override bool Equals(object? obj)
        {
            var other = obj as User;
            return 
                UserID.Equals(other.UserID) && 
                Login.Equals(other.Login) && 
                Password.Equals(other.Password) && 
                CreatedDate.Equals(other.CreatedDate) && 
                UserGroupId.Equals(other.UserGroupId) && 
                UserStateId.Equals(other.UserStateId);
        }
    }
}
