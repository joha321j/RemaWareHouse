using RemaWareHouse.DataTransferObjects;

namespace RemaWareHouse.Models
{
    public class Unit : IModel
    {
        public Unit()
        {
            
        }
        public Unit(UnitDto unitDto)
        {
            Name = unitDto.Name;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}