using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels
{
    public class CreateArticleViewModel
    {
        [Required(ErrorMessage = "Введите название статьи")]
        [Display(Name = "Название статьи")]
        [StringLength(32, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите краткое описание статьи")]
        [Display(Name = "Краткое описание статьи")]
        [StringLength(256, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 5)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Введите полное описание статьи")]
        [Display(Name = "Полное описание статьи")]
        [StringLength(5000, ErrorMessage = "{0} должно содержать от {2} до {1} символов", MinimumLength = 5)]
        public string FullDescription { get; set; }
    }
}
