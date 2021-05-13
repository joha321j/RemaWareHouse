using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Persistency;
using RemaWareHouse.Models;
using SQLitePCL;

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
            List<Product> products = new List<Product>();
            
            DbSet<Product> set = _context.Products;
            
            if (withCategory)
            {
                set.Include(c => c.Category);
            }

            if (withSupplier)
            {
                set.Include(c => c.Supplier);
            }

            if (withUnit)
            {
                set.Include(c => c.Unit);
            }

            if (productId.HasValue)
            {
                Product product = await FindProduct(productId.Value, set);
                
                products.Add(product);
            }
            else
            {
                products = await set.ToListAsync();
            }

            return products;
        }

        private static async Task<Product> FindProduct(int productId, DbSet<Product> set)
        {
            Product product = await set.FindAsync(productId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(Product), productId);
            }

            return product;
        }
    }
}