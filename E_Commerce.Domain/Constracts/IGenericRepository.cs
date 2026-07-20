using E_Commerce.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        #region Before Specification Design Pattern
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);//void لانها شغالة Local يعنى مفيش اى Database Hits دا فى حالة Update+Delete
        void Delete(TEntity entity); 
        #endregion
        //=============================================
        #region After Make Specification Design Pattern
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity,TKey> specification);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification);//For includes + Filteration by id 
        #endregion
    }
}
