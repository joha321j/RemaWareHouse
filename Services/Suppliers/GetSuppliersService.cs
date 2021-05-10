using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services.Suppliers
{
    public class GetSuppliersService
    {
        private readonly WarehouseContext _context;

        public GetSuppliersService(WarehouseContext context)
        {
            _context = context;
        }

        public Task<ActionResult<IEnumerable<Supplier>>> GetSuppliersAsync(int? supplierId)
        {
            return supplierId.HasValue ? GetSpecificSupplierAsync(supplierId.Value) : GetAllSuppliersAsync();
        }

        private async Task<ActionResult<IEnumerable<Supplier>>> GetSpecificSupplierAsync(int supplierId)
        {
            List<Supplier> suppliers = new List<Supplier>
            {
                await _context.Suppliers.FirstOrDefaultAsync(supplier => supplier.Id == supplierId)
            };

            return suppliers;
        }

        private async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}