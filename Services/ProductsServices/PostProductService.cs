using System.Threading.Tasks;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services.ProductsServices
{
    public class PostProductService
    {
        private readonly WarehouseContext _context;

        public PostProductService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<Product> PostAsync(Product newProduct)
        {
            await EnsureValidDependencies(newProduct);

            await _context.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

        private async Task EnsureValidDependencies(Product newProduct)
        {
            await EnsureValidCategory(newProduct.Category);
            await EnsureValidSupplier(newProduct.Supplier);
            await EnsureValidUnit(newProduct.Unit);
        }

        private async Task EnsureValidUnit(Unit unit)
        {
            var result = await _context.Units.FindAsync(unit.Id);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(unit), unit.Id);
            }
        }

        private async Task EnsureValidSupplier(Supplier supplier)
        {
            var result = await _context.Suppliers.FindAsync(supplier.Id);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(supplier), supplier.Id);
            }
        }

        private async Task EnsureValidCategory(Category category)
        {
            var result = await _context.Suppliers.FindAsync(category.Id);
            if (result == null)
            {
                throw new EntityNotFoundException(nameof(category), category.Id);
            }
        }
    }
}