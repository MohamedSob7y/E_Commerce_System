using E_Commerce.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    public interface IUniteofWork
    {
        IGenericRepository<TEntity,TKey> GetRepository<TEntity,TKey>() where TEntity : BaseEntity<TKey>;//this No Database Hits لانها شغالة Local يعنى مفيش اى Database Hits this No Async
        Task<int> SaveChangesAsync();//ممكن ترجع bool بدل int
    }
}
