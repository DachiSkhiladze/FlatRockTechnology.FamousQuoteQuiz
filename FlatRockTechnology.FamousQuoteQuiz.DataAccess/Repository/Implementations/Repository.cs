using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Database;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;
using System.Data.Entity;
using System.Linq.Expressions;

namespace FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly QuoteQuizContext _quoteQuizContext;
        static readonly object _object = new object();
        public Repository(QuoteQuizContext quoteQuizContext)
        {
            _quoteQuizContext = quoteQuizContext;
        }

        public bool CheckIfExists(Expression<Func<TEntity, bool>> predicate) => this.GetAll().Any(predicate.Compile());

        public long GetCount() => this.GetAll().Count();
        
        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _quoteQuizContext.Set<TEntity>().AsNoTracking();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not be returned: {ex.Message}");
            }
        }

        public IQueryable<TEntity> Get(int skip, int take)
        {
            try
            {
                return _quoteQuizContext.Set<TEntity>().AsNoTracking().Skip(skip).Take(take);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not be returned: {ex.Message}");
            }
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _quoteQuizContext.Set<TEntity>().AsNoTracking().Where(predicate.Compile());
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not be returned: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                var result = await _quoteQuizContext.AddAsync(entity);
                await _quoteQuizContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.InnerException}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _quoteQuizContext.Update(entity);
                _quoteQuizContext.Entry<TEntity>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _quoteQuizContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _quoteQuizContext.Remove(entity);
                _quoteQuizContext.Entry<TEntity>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                await _quoteQuizContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
