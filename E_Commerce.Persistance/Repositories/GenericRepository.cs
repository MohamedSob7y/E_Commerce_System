using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Persistance.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDBContext _storeDBContext;
        public GenericRepository(StoreDBContext storeDBContext)
        {
            _storeDBContext = storeDBContext;
        }
        public async Task AddAsync(TEntity entity)=> await _storeDBContext.Set<TEntity>().AddAsync(entity);
        public void Delete(TEntity entity) => _storeDBContext.Set<TEntity>().Remove(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _storeDBContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id) => await _storeDBContext.Set<TEntity>().FindAsync(id);//Find بدور على 1object in Local Before Database
        public void Update(TEntity entity) => _storeDBContext.Set<TEntity>().Update(entity); 
    }
}
