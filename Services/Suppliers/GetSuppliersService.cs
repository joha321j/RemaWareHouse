using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Exceptions;
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
            List<Supplier> suppliers = new List<Supplier>();

            Supplier supplier = await _context.Suppliers.FindAsync(supplierId);

            if (supplier == null)
            {
                throw new EntityNotFoundException(
                    "No supplier with given ID exists. Id given was: " 
                    + supplierId);
            }

            suppliers.Add(supplier);
            return suppliers;
        }

        private async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
    }
}