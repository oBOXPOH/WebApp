using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels.Sections
{
    public class DetailsFirstLevelSectionViewModel
    {
        public string Title { get; set; }
        public DateTime EditDate { get; set; }

        public List<SecondLevelSection> SecondLevelSections { get; set; }
    }
}
