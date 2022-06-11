using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        bool CheckIfExists(Expression<Func<TEntity, bool>> predicate);

        long GetCount();

        IQueryable<TEntity> GetAll();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Get(int skip, int take);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
