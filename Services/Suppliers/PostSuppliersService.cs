using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;
using SQLitePCL;

namespace RemaWareHouse.Services.Suppliers
{
    public class PostSuppliersService
    {
        private readonly WarehouseContext _context;

        public PostSuppliersService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<int> PostAsync(Supplier supplier)
        {
            supplier.Id = 0;

            await _context.Suppliers.AddRangeAsync(supplier);
            await _context.SaveChangesAsync();

            return supplier.Id;
        }
    }
}