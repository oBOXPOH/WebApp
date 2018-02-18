using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.Sections
{
    public class CreateFirstLevelSectionViewModel
    {
        [Required(ErrorMessage = "Введите назвние раздела")]
        [Display(Name = "Название раздела")]
        [StringLength(32, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 2)]
        public string Title { get; set; }
    }
}
