using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services
{
    public class DeleteService<T> where T : class
    {
        private readonly WarehouseContext _context;
        private readonly DbSet<T> _set;
        private readonly string _modelName;

        public DeleteService(WarehouseContext context, DbSet<T> set, string modelName)
        {
            _context = context;
            _set = set;
            _modelName = modelName;
        }

        public async Task DeleteAsync(int modelId)
        {
            T model = await _set.FindAsync(modelId);

            if (model != null)
            {
                _set.Remove(model);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException(_modelName, modelId);
            }
        }
    }
}