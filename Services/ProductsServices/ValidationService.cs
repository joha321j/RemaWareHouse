using System.Threading.Tasks;
using RemaWareHouse.DataTransferObjects;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services.ProductsServices
{
    public class ValidationService
    {
        private readonly WarehouseContext _context;

        public ValidationService(WarehouseContext context)
        {
            _context = context;
        }
        public async Task EnsureValidDependencies(ProductDto newProduct)
        {
            await EnsureValidCategory(newProduct.CategoryId);
            await EnsureValidSupplier(newProduct.SupplierId);
            await EnsureValidUnit(newProduct.UnitId);
        }
        
        private async Task EnsureValidCategory(int categoryId)
        {
            var result = await _context.Suppliers.FindAsync(categoryId);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(Category), categoryId);
            }
        }
        
        private async Task EnsureValidSupplier(int supplierId)
        {
            var result = await _context.Suppliers.FindAsync(supplierId);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(Supplier), supplierId);
            }
        }

        private async Task EnsureValidUnit(int unitId)
        {
            var result = await _context.Units.FindAsync(unitId);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(Unit), unitId);
            }
        }
    }
}