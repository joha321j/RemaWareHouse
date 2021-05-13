using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Exceptions;

namespace RemaWareHouse.Services
{
    public class GetService<T> where T : class
    {
        private readonly DbSet<T> _set;
        private readonly string _modelName;

        public GetService(DbSet<T> set, string modelName)
        {
            _set = set;
            _modelName = modelName;
        }

        public async Task<IEnumerable<T>> GetAsync(int? modelId)
        {
            return modelId.HasValue ? await GetSpecificModelAsync(modelId.Value) : await GetModelsAsync();
        }

        private async Task<IEnumerable<T>> GetSpecificModelAsync(int modelId)
        {
            List<T> models = new List<T>();

            T model = await _set.FindAsync(modelId);

            if (model == null)
            {
                throw new EntityNotFoundException(_modelName, modelId);
            }

            models.Add(model);
            return models;
        }
        
        private async Task<IEnumerable<T>> GetModelsAsync()
        {
            return await _set.ToListAsync();
        }
    }
}