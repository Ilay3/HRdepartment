using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRdepartment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Adress { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime Date_birth { get; set; }

        public string Passport_number { get; set; }
        public string Passport_series { get; set; }
        public string Education { get; set; }
        public string Driver_license { get; set; }
        public string Rank { get; set; }


        public int? Id_Post { get; set; }
        public int? Department_Id { get; set; }




        public Post Post { get; set; }
        public Department Department { get; set; }

       
    
        public ICollection<UserLoginHistory> UserLoginHistory { get; set; }

       
    }
}
