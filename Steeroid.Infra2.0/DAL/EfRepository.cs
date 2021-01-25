//using SteeroidUpdate.Main.SharedKernel.Interfaces;
//using SteeroidUpdate.Main.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Steeroid.Infra2._0.DAL;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeroid.Infra2._0.DAL
{
    public class EfRepository : IRepository
    {
        private readonly MyDbContext _dbContext;

        public EfRepository(MyDbContext dbContext)
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

            _dbContext.Set<T>().Add(entity);
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

        public async Task ClearAsync<T>() where T : BaseModel
        {
            var set =  _dbContext.Set<T>();//.ToListAsync();
            set.RemoveRange(set.ToList());
            await _dbContext.SaveChangesAsync();
        }

    }
}
