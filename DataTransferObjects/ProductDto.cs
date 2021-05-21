using RemaWareHouse.Models;

namespace RemaWareHouse.DataTransferObjects
{
    public class ProductDto
    {
        public string Name { get; set; }
        
        public Unit Unit { get; set; }
        
        public int AmountInPackage { get; set; }
        
        public double Price { get; set; }
        
        public Category Category { get; set; }
        
        public Supplier Supplier { get; set; }
    }
}