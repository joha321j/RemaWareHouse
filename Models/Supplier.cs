using System.ComponentModel.DataAnnotations;
using RemaWareHouse.DataTransferObjects;

namespace RemaWareHouse.Models
{
    public class Supplier : IModel
    {
        public Supplier()
        {
            
        }
        public Supplier(SupplierDto supplierDto)
        {
            Name = supplierDto.Name;
            Address = supplierDto.Address;
            ZipCode = supplierDto.ZipCode;
            NameOfContactPerson = supplierDto.NameOfContactPerson;
            Email = supplierDto.Email;
            PhoneNumber = supplierDto.PhoneNumber;
        }

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