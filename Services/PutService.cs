using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;

namespace RemaWareHouse.Services
{
    public class PutService<T> where T : class, IModel
    {
        private readonly WarehouseContext _context;
        private readonly DbSet<T> _set;

        public PutService (WarehouseContext context, DbSet<T> set)
        {
            _context = context;
            _set = set;
        }

        public async Task<bool> PutAsync (
            IModel modelToPut,
            int objectId)
        {
            bool hasOverwritten = false;
            modelToPut.Id = objectId;

            T existingModel = await _set.FindAsync(objectId);

            if (existingModel == null)
            {
                await AddModelAsync(modelToPut);
            }
            else
            {
                await OverwriteExistingModel(modelToPut, existingModel);
                hasOverwritten = true;
            }

            return hasOverwritten;
        }

        private async Task AddModelAsync (IModel modelToPut)
        {
            await _context.AddAsync(modelToPut);
            await _context.SaveChangesAsync();
        }
        
        
        private async Task OverwriteExistingModel(IModel modelToPut, T existingModel)
        {
            _context.Remove(existingModel);
            await AddModelAsync(modelToPut);
        }
    }
}