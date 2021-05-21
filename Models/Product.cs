using RemaWareHouse.DataTransferObjects;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Models
{
    public class Product : IModel
    {
        public Product()
        {
        }

        public Product(ProductDto productDto, WarehouseContext context)
        {
            Name = productDto.Name;
            Unit = context.Units.Find(productDto.UnitId);
            AmountInPackage = productDto.AmountInPackage;
            Price = productDto.Price;
            Category = context.Categories.Find(productDto.CategoryId);
            Supplier = context.Suppliers.Find(productDto.SupplierId);
            Description = productDto.Description;
            InStock = productDto.InStock;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Unit Unit { get; set; }
        
        public int AmountInPackage { get; set; }
        
        public double Price { get; set; }
        
        public Category Category { get; set; }
        
        public Supplier Supplier { get; set; }

        public string Description { get; set; }

        public int InStock { get; set; }
    }
}