using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services.Suppliers
{
    public class PutSuppliersService
    {
        private readonly WarehouseContext _context;

        public PutSuppliersService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<bool> PutAsync(Supplier supplier, int supplierId)
        {
            bool hasOverwritten = false;
            supplier.Id = supplierId;

            var existingSupplier = await _context.Suppliers.FindAsync(supplierId);

            if (existingSupplier == null)
            {
                await AddSupplierAsync(supplier);
            }
            else
            {
                await OverwriteSupplierAsync(supplier, existingSupplier);
                hasOverwritten = true;
            }

            return hasOverwritten;
        }
        
        private async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }
        
        private async Task OverwriteSupplierAsync(Supplier supplier, Supplier existingSupplier)
        {
            _context.Remove(existingSupplier);

            await AddSupplierAsync(supplier);
        }
    }
}