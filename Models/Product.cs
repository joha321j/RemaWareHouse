using RemaWareHouse.DataTransferObjects;

namespace RemaWareHouse.Models
{
    public class Product : IModel
    {
        public Product()
        {
        }
        public Product(ProductDto productDto)
        {
            Name = productDto.Name;
            Unit = productDto.Unit;
            AmountInPackage = productDto.AmountInPackage;
            Price = productDto.Price;
            Category = productDto.Category;
            Supplier = productDto.Supplier;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Unit Unit { get; set; }
        
        public int AmountInPackage { get; set; }
        
        public double Price { get; set; }
        
        public Category Category { get; set; }
        
        public Supplier Supplier { get; set; }
    }
}