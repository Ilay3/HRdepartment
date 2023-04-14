using HRdepartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRdepartment.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            Post = new List<string>();
        }
        public string Id { get; set; }

        public int? Id_Post { get; set; }
        public int? Department_Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public string NumberPhone { get; set; }
        [Required]
        public DateTime Date_birth { get; set; }

        [Required]
        public string Passport_number { get; set; }

        [Required]
        public string Passport_series { get; set; }

        [Required]
        public string Driver_license { get; set; }

        [Required]
        public string Rank { get; set; }

        [Required]
        public string Education { get; set; }


        public List<string> Claims { get; set; }

        public IList<string> Department { get; set; }
        public IList<string> Post { get; set; }
        public IList<string> Roles { get; set; }


    }
}

