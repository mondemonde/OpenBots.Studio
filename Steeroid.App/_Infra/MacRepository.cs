///using SteeroidUpdate.Main.SharedKernel.Interfaces;
//using SteeroidUpdate.Main.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteeroidUpdate.Main.Infrastructure.Data
{
    public class MacRepository : IRepository
    {
        private readonly MachineDbContext _dbContext;

        public MacRepository(MachineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseModel
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public Task<T> GetByIdAsync<T>(int id) where T : BaseModel
        {
            return _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public Task<List<T>> ListAsync<T>() where T : BaseModel
        {
            return _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseModel
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseModel
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseModel
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
