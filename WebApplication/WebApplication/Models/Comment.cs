using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ArticleId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EditDate { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
    }
}
