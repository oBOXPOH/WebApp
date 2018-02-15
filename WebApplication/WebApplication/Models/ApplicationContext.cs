using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<FirstLevelSection> FirstLevelSections { get; set; }
        public DbSet<FirstLevelSection> SecondLevelSections { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Event> Events { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
