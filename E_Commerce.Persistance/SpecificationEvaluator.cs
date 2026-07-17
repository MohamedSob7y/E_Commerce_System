using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance
{
    //this class Combine Specification Object To Create Query and Send This Query To Repository to Excuted in Database  
    internal static class SpecificationEvaluator
    {
        //must has Method To Create Query From Specification Object
        public static IQueryable<TEntity>CreateQuery<TEntity,TKey>(IQueryable<TEntity>Entrypiont,ISpecification<TEntity,TKey> specification)where TEntity : BaseEntity<TKey>
        {
            var query = Entrypiont;//DbContext.Product
            if(specification is not null)
            {
                if (specification.Includesexpression is not null && specification.Includesexpression.Any())
                {
                    #region Before Aggregate Linq Method 
                    //foreach (var includeEx in specification.Includesexpression)
                    //{
                    //    query.Include(includeEx);
                    //} 
                    #endregion
                    //==================================
                    #region After Aggregate Linq Method

                    query= specification.Includesexpression.Aggregate(query, (currentquery, includeExpre) => currentquery.Include(includeExpre));//DbContext.Product.include() وكل شوية بعمل include For ProductType+ ProductBrand
                    #endregion
                }
            }
            return query;

            //فى بديل للForeach مستخدم فى الSpecification Pattern موجود ضمن LinQ Method => aggregate بعمل دى عشان مش عايز حد يعدل عليها لان Foreach ممكن احط جواها اى Condition عشان كدة هستخدم ال Linq Method Call Aggregate 
        }
    }
}
