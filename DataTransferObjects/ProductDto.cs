using RemaWareHouse.Models;

namespace RemaWareHouse.DataTransferObjects
{
    public class ProductDto
    {
        public string Name { get; set; }
        
        public int UnitId { get; set; }
        
        public int AmountInPackage { get; set; }
        
        public double Price { get; set; }
        
        public int CategoryId { get; set; }
        
        public int SupplierId { get; set; }
        
        public string Description { get; set; }

        public int InStock { get; set; }
    }
}