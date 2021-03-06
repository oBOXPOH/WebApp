﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string SubLevelTitleId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime EditDate { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
    }
}
