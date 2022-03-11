using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TEST.Data.Entities
{
    public class EfDBContex : IdentityDbContext<User>
    {
        public DbSet<Article> Articles  { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<HeroImage> HeroImage { get; set; }


        public EfDBContex(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public EfDBContex()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
