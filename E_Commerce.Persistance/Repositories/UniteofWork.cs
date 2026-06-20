using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Persistance.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repositories
{
    public class UniteofWork : IUniteofWork
    {
        private readonly StoreDBContext _DBContext;
        private readonly Dictionary<Type, object> _repositories=[];
        public UniteofWork(StoreDBContext storeDBContext)
        {
            _DBContext = storeDBContext;
           
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //If Object مش موجود اعمله وحطه فى Dictionary عشان لما اطلبه تانى 
            var entityType = typeof(TEntity);//Get name Of Class
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))//لو لاقيت Key دا حط الValue بتاعته فى Passsing By out this Variable
            {//بقؤل للDictionary دور عندك على Entity اللى من Type دا 
                return (IGenericRepository<TEntity,TKey>)repository;
            }
            var newrepo=new GenericRepository<TEntity,TKey>(_DBContext);
             _repositories[entityType]=newrepo;//حط Object دا فى Dictionry عشان لما اطلبه تانى 
            return newrepo;
        }

        public async Task<int> SaveChangesAsync() => await _DBContext.SaveChangesAsync();
       
    }
}
