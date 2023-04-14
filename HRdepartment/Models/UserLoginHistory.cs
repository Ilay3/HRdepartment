using System;
using System.Collections.Generic;

namespace HRdepartment.Models
{
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
