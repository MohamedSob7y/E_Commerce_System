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
        protected BaseSpecification(Expression<Func<TEntity, bool>>? cretaria)
        {
            Cretaria = cretaria;
        }
        public ICollection<Expression<Func<TEntity, object>>>? Includesexpression { get; } = [];

        public Expression<Func<TEntity, bool>>? Cretaria { get; } 

        //this  Get For Automatic Property in Implementation of Interface => ISpecification => يعنى مش Signature قصدى 


        //انا كل شوية ببعت مثلا p => p.ProductBrand => Convert To [p => p.ProductBrand]  bu this Method and  send This p => p.ProductType   => will Convert To this [p => p.ProductBrand,p => p.ProductType]      
        protected void AddInclude(Expression<Func<TEntity,object>>includeExpres)
        //محتاج ابعتله Expression يتضافه جوه الCollection دى 
        {
            Includesexpression?.Add(includeExpres);  //بعدى على expression واضيفه جوه ال Collection 
        }//this Method For Includes عشان حتة الInitlaization    
    }
}
