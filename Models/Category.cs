using RemaWareHouse.DataTransferObjects;

namespace RemaWareHouse.Models
{
    public class Category : IModel
    {
        public Category()
        {
        }
        public Category(CategoryDto categoryDto)
        {
            Name = categoryDto.Name;
            Description = categoryDto.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}