using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class ResetPasswordUserViewModel
    {
        public string UserId { get; set; }

        public string UserCode { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(16, ErrorMessage = "{0} должен содержать от {2} до {1} символов", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
