using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SecondLevelSection
    {
        public string Id { get; set; }
        public string FirstLevelTitleId { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
