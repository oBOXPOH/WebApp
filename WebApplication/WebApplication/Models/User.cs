﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class User : IdentityUser
    {
        public DateTime DOB { get; set; }
        public string UserRole { get; set; }
    }
}
