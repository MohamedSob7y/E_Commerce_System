using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    //this Container For Any Specification هيورث منى 
    //عملته abstract عشان محدش يعمل منه object ولا يكون فى Constructor وبالتالى يكون معمول فقط للوراثة 
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Filteration
        protected BaseSpecification(Expression<Func<TEntity, bool>>? cretaria)
        {
            Cretaria = cretaria;
        }
        public Expression<Func<TEntity, bool>>? Cretaria { get; }
        #endregion
        //===========================================
        #region Includes
        public ICollection<Expression<Func<TEntity, object>>>? Includesexpression { get; } = [];
        //this  Get For Automatic Property in Implementation of Interface => ISpecification => يعنى مش Signature قصدى 
        //انا كل شوية ببعت مثلا p => p.ProductBrand => Convert To [p => p.ProductBrand]  bu this Method and  send This p => p.ProductType   => will Convert To this [p => p.ProductBrand,p => p.ProductType]      
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpres)
        //محتاج ابعتله Expression يتضافه جوه الCollection دى 
        {
            Includesexpression?.Add(includeExpres);  //بعدى على expression واضيفه جوه ال Collection 
        }//this Method For Includes عشان حتة الInitlaization    
        //this Method عشان اعمل Set For Property IncludeExpression
        #endregion
        //===========================================
        #region Sorting
        public Expression<Func<TEntity, object>>? OrderBy { private set; get; }

        public Expression<Func<TEntity, object>>? OrderByDescending { private set; get; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpres)
        {
            OrderBy = orderByExpres;//دا كدة غلط فى حالة ان Property OrderBy معندهاش set accessor 
            //طب منا مش هينفع اعملها set على طول عشان كدة خلاص اعملها private set معناها ان هخلى عندها set بس هنا فى المكان دا فقط مش فى كله 
        }
        protected void AddOrderByDescening(Expression<Func<TEntity, object>> orderByDescendingExpres)
        {
            OrderBy = orderByDescendingExpres;//دا كدة غلط فى حالة ان Property OrderBy معندهاش set accessor 
            //طب منا مش هينفع اعملها set على طول عشان كدة خلاص اعملها private set معناها ان هخلى عندها set بس هنا فى المكان دا فقط مش فى كله 
            //طب اشمعنا معملتش كدا مع IncludesExpression لان عندها Add فدى بديلة يعنى 
        }
        #endregion
        //===========================================
        #region Pagniation
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }
        //make Method Apply Paination To Set This Property 
       //this Method For Setting Pagination Property 
        protected void ApplyPagination(int PageSize,int PageIndex )
        {
            IsPaginated=true;//كدى معناها ان Pagination مطبقة 
            Take=PageSize;
            Skip = (PageIndex-1 )*PageSize;
        }
        #endregion
    }
}
