using System.Threading.Tasks;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services
{
    public class PostService
    {
        private readonly WarehouseContext _context;

        public PostService(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<T> PostAsync<T>(T postObject)
        {
            await _context.AddAsync(postObject);
            await _context.SaveChangesAsync();

            return postObject;
        }
    }
}