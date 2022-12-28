using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PsssD
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
 