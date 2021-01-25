using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steeroid.Models.Interfaces
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T : BaseModel;
        Task<List<T>> ListAsync<T>() where T : BaseModel;
        Task<T> AddAsync<T>(T entity) where T : BaseModel;
        Task UpdateAsync<T>(T entity) where T : BaseModel;
        Task DeleteAsync<T>(T entity) where T : BaseModel;

       Task ClearAsync<T>() where T : BaseModel;
       
    }
}