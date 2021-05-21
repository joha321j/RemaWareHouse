using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Persistency;
using RemaWareHouse.Models;

namespace RemaWareHouse.Services.ProductsServices
{
    public class GetProductsService
    {
        private readonly WarehouseContext _context;

        public GetProductsService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAsync(
            int? productId,
            bool withCategory = false,
            bool withSupplier = false,
            bool withUnit = false)
        {
            
            IQueryable<Product> queryable = IncludeDependencies(withCategory, withSupplier, withUnit);
            
            List<Product> products = new List<Product>();
            
            if (productId.HasValue)
            {
                Product product = await FindProductAsync(productId.Value, queryable);
                
                products.Add(product);
            }
            else
            {
                products = await queryable.ToListAsync();
            }
            
            return products;
        }

        private IQueryable<Product> IncludeDependencies(bool withCategory, bool withSupplier, bool withUnit)
        {
            IQueryable<Product> set = _context.Products;

            if (withCategory)
            {
                set = set.Include(p => p.Category);
            }
            
            if (withSupplier)
            {
                set = set.Include(p => p.Supplier);
            }

            if (withUnit)
            {
                set = set.Include(p => p.Unit);
            }

            return set;
        }

        private static async Task<Product> FindProductAsync(int productId, IQueryable<Product> set)
        {
            List<Product> products = await set.ToListAsync();

            Product product = products.Find(p => p.Id == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(Product), productId);
            }

            return product;
        }
    }
}