using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using jokesWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace jokesWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<jokesWebApp.Models.joke> joke { get; set; } = default!;
        public DbSet<jokesWebApp.Models.Category> Category { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Knock-Knock", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Dad Jokes", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Puns", DisplayOrder = 3 }
                );
        }
    }
}
