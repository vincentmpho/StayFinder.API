using Microsoft.EntityFrameworkCore;
using StayFinder.API.Data;
using StayFinder.API.Repository.Interface;

namespace StayFinder.API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StayFinderDbContext _Context;

        public GenericRepository(StayFinderDbContext Context)
        {
            _Context = Context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _Context.AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _Context.Set<T>().Remove(entity);
            await _Context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
           var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();   
        }

        public  async Task<T> GetAsync(int? id)
        {
            if(id is null)
            {
                return null;
            }

            return await _Context.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _Context.Update(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }
    }
}
