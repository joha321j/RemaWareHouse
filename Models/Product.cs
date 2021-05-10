namespace RemaWareHouse.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Unit Unit { get; set; }
        
        public int AmountInPackage { get; set; }
        
        public double Price { get; set; }
        
        public Category Category { get; set; }
        
        public Supplier Supplier { get; set; }
    }
}