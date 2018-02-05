using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class ForgotUserViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите корректный Email")]
        public string EMail { get; set; }
    }
}
