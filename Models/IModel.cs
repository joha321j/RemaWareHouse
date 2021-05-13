using System.ComponentModel.DataAnnotations;

namespace RemaWareHouse.Models
{
    public interface IModel
    {
        [Key]
        public int Id { get; set; }
    }
}