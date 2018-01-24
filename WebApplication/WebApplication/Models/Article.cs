using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Article
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EditTime { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
    }
}
