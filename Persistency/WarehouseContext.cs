using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RemaWareHouse.Models;

namespace RemaWareHouse.Persistency
{
    public class WarehouseContext : DbContext
    {
        public DbSet<Unit> Units { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Supplier> Suppliers { get; set; }
        
        public DbSet<Product> Products { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }   
    }
}