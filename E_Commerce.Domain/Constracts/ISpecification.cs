using E_Commerce.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    //this  Specification For Includes+Filteration => GetById For Filteration + Includes
    //this Specification Used For includes only For GetAll or Filteration only or Fildetration+Includes for GetById
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region For Includes
        ICollection<Expression<Func<TEntity, object>>>? Includesexpression { get; }//Get in this Interface => is Signutaure of Property in  interface 
        //For Example => ProductWithTypeandBrandSpecification => is Class Implement this Interface => ISpecification<TEntity, TKey>

        #endregion
        //==============================================
        #region For Filteration [Where]
        //Second Cretaria For GetById For Filteration + Includes 
        Expression<Func<TEntity, bool>>? Cretaria { get; }
        #endregion
        //==============================================
        #region For Sorting

        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDescending{ get; }

        #endregion
        //==============================================
        #region For Pagination
         int Skip { get;}//this Property معناها انا هسيب كام Product عشان اخد الProduct اللى انا عايزه 
         int Take { get;}//انا هاخد كام Product From Page اللى انا قسمتها عليه 
         bool IsPaginated { get;}//For Check that pagination is appliyed for query or not => This Property تشوف هل هنطبق الPagination in this Query Or Not
        #endregion
    }
}
