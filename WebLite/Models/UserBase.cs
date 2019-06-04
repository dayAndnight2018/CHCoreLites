using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Models
{
    public class UserBase : EntityBase
    {
        public string Token { get; set; }

        public string ExpiresTime { get; set; }

        public long RoleId { get; set; }

        public UserRole Role { get; set; }
    }
}
