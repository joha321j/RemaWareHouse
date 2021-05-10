using System.Threading.Tasks;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services.Suppliers
{
    public class DeleteSuppliersService
    {
        private readonly WarehouseContext _context;

        public DeleteSuppliersService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);

            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException(
                    "No supplier with given ID exists. Given ID was: "
                    + supplierId);
            }
        }
    }
}