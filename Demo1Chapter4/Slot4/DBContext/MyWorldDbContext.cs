using Microsoft.EntityFrameworkCore;
using Slot4.model;

namespace Slot4.DBContext
{
    public class MyWorldDbContext : DbContext 
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options) { 
        }
        public DbSet<Gadgets> Gadgets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
    
    
}
