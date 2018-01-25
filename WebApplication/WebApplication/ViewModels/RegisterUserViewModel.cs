using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string EMail { get; set; }
        
        public DateTime DOB { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
