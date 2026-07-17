using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Persistance.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        //==================================================================
        #region Before Make Specification Design Pattern
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _storeDBContext.Set<TEntity>().ToListAsync();



        //public async Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity,bool>? contintion=default)
        //{
        //    if(contintion is null)
        //    {
        //        return await _storeDBContext.Set<TEntity>().Where(contintion).ToList();
        //        //where has 4 Overload شغالة مع IEquable + Ienumberable طب ليه مفيش TolistAsync بعد where دى؟
        //        //where دى طالما بتاخد FunC يبقى شغالة مع Ienumberable Not IEQuerable => عشان كدة مفيش TolistAsync طب اية العلاقة 
        //        //any Function لو شغالة مع iequable يبقى لازم يكون عندها TolistAsync لان دى Database Operation 
        //        //بالتالى الWhere شغالة مع Ienumberable عشان كدة مفيش TolistAsync 
        //        //وطالما شغالة مع Ienumberable يبقى هروح اجيب الData From Database and make Fuletration in Memory So This More Complex 
        //    }
        //    else
        //    {
        //        return _storeDBContext.Set<TEntity>().Where(contintion).ToList();
        //    }
        //}

        //=======================================================================================
        //Solveing Probelms =>take Expression of FunC in GetAll =>
        //1: Where شغالة على IEquerable Not Ieumerable ==> لانها واخد Expression of FunC not FunC=>So Filteration Make in Database وبعد كدة اجيب الData after Filetratinon in Memory
        //So هستخدم TolistAsync لان where شغالة مع IEquerable
        //public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? contintion = default)
        //{
        //    if (contintion is null)
        //    {
        //        return await _storeDBContext.Set<TEntity>().Where(contintion).ToListAsync();

        //    }
        //    else
        //    {
        //        return _storeDBContext.Set<TEntity>().Where(contintion).ToList();
        //    }
        //}

        //=======================================================================================
        ////طب وانا محتاج اعمل include عشان الProducttype+ ProductBrand وانا هنا فى Generic Repository يعنى لو عملت الinclude مش شايفه اى حاجة غير الEntity اللى بيورث منها كل الClass
        ////فانا كدة مضطر اعمل Specific Repository دا كدة لكل حاجة عايز اعملها include 
        ////ممكن احلها كدة 
        ////بس كدة more Complex as in Generic Repository => اما يبعت الCondition only    2: Includes only    3: Includes + Condition

        //public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? contintion = default,
        //    List<Expression<Func<TEntity,object>>>? includes=default)
        //{
        //    //===================================================================
        //    //includes only
        //    if (includes is not null)
        //    {
        //        IQueryable<TEntity> entrypiont= _storeDBContext.Set<TEntity>();//ماسك الAll Products
        //        foreach (var includeexpression in includes)
        //        {
        //            entrypiont=entrypiont.Include(includeexpression);
        //        }
        //    }
        //    //===================================================================
        //    //Condition only
        //    if (contintion is not null)
        //    {
        //        return await _storeDBContext.Set<TEntity>().Where(contintion).ToListAsync();

        //    }
        //    //===================================================================
        //    //Condition + includes
        //    if(contintion is not null&&includes is not null)
        //    {
        //        IQueryable<TEntity> entrypiont = _storeDBContext.Set<TEntity>().Where(contintion);//All Products ماسك ال
        //        foreach (var includeexpression in includes)
        //        {
        //            entrypiont = entrypiont.Include(includeexpression);
        //        }
        //    }
        //    //==============================================================================
        //    else
        //    {
        //        return _storeDBContext.Set<TEntity>().Where(contintion).ToList();
        //    }
        //}
        #endregion
        //==================================================================
        #region After Make Specification Design Pattern
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            //recive Query From SpecificationEvaluator Class to Excuted in Database
            var Query= SpecificationEvaluator.CreateQuery(_storeDBContext.Set<TEntity>(), specification);
            return await Query.ToListAsync();//Excute Query in Database 
        }
        #endregion
        //==================================================================
        public async Task<TEntity?> GetByIdAsync(TKey id) => await _storeDBContext.Set<TEntity>().FindAsync(id);//Find بدور على 1object in Local Before Database
        public void Update(TEntity entity) => _storeDBContext.Set<TEntity>().Update(entity); 
    }
}
