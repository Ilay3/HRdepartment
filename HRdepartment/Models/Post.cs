using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRdepartment.Models
{
    public class Post
    {
        [Key]
        public int Id_Post { get; set; }

        [Required(ErrorMessage = "Введите название должности")]
        public string Title_Post { get; set; }

        public ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}
