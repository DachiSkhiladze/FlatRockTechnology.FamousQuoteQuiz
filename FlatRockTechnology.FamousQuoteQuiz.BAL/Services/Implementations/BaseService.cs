using FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Abstractions;
using FlatRockTechnology.FamousQuoteQuiz.DataAccess.Repository.Abstractions;
using System.Linq.Expressions;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Services.Implementations
{
    public abstract class BaseService<TEntity, TModel> : IBaseService<TEntity, TModel> 
                                                            where TEntity : class, new() 
                                                            where TModel : class, new()
    {
        protected IRepository<TEntity> Repository;
        public BaseService(IRepository<TEntity> repository)
        {
            Repository = repository;    // Initializing Repository
        }

        public bool CheckIfExists(Expression<Func<TEntity, bool>> predicate) => this.Repository.CheckIfExists(predicate);

        public IEnumerable<TModel> GetModels()
        {
            foreach (var item in Repository.GetAll())
            {
                yield return ConvertToModel(item); // Returning Type Models
            }
        }

        public IEnumerable<TModel> GetModels(Expression<Func<TEntity, bool>> predicate)
        {
            var result = Repository.Get(predicate);
            foreach (var item in result)
            {
                yield return ConvertToModel(item); // Returning Type Models
            }
        }

        public async Task<TModel> InsertAsync(TModel model)
        {
            var dto = ConvertToDTO(model);
            return ConvertToModel(await Repository.AddAsync(dto));  // Inserting New Data
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            var dto = ConvertToDTO(model);
            return ConvertToModel(await Repository.UpdateAsync(dto)); // Updating Data
        }

        public async Task DeleteAsync(TModel model)
        {
            var dto = ConvertToDTO(model);
            await Repository.DeleteAsync(dto); // Deleting Data
        }

        protected abstract TEntity ConvertToDTO(TModel model);

        protected abstract TModel ConvertToModel(TEntity entity);

        protected abstract IEnumerable<TModel> ConvertToModels(IQueryable<TEntity> entities);
    }
}
