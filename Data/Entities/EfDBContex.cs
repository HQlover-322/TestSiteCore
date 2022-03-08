using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TEST.Data.Entities
{
    public class EfDBContex : DbContext
    {
        public DbSet<Article> Articles  { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }


        public EfDBContex(DbContextOptions options) : base(options)
        {
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
