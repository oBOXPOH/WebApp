using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ViewModels
{
    public class IndexUserViewModel
    {
        public List<User> Users { get; set; }
        public List<string> Roles { get; set; }
        public List<int> AmountOfRoles { get; set; }
    }
}
