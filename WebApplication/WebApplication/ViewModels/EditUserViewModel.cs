using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        [StringLength(16, ErrorMessage = "{0} должен содержать от {2} до {1} символов", MinimumLength = 5)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите корректный Email")]
        public string EMail { get; set; }

        public DateTime DOB { get; set; }

        public string UserRole { get; set; }
    }
}
