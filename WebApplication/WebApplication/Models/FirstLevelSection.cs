using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class FirstLevelSection
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime EditDate { get; set; }
        public string Slug { get; set; }
    }
}
