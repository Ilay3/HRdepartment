using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HRdepartment.Models
{
    public class Department
    {
        [Key]
        public int Department_Id { get; set; }

        [Required(ErrorMessage = "Введите название отдела")]
        public string Department_Title { get; set; }

        [Required(ErrorMessage = "Введите номер телефона отдела")]
        public string Number_Phone { get; set;}
        [Required(ErrorMessage = "Введите ФИО начальника отдела")]
        public string Head_Department { get; set; }

        public ICollection<ApplicationUser> ApplicationUser { get; set; }

    }
}
