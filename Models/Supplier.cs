using System.ComponentModel.DataAnnotations;

namespace RemaWareHouse.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Address { get; set; }
        
        public string ZipCode { get; set; }
        
        public string NameOfContactPerson { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}