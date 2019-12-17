using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Admin.Validators;
using GovUk.Education.ExploreEducationStatistics.Common.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static GovUk.Education.ExploreEducationStatistics.Admin.Validators.ValidationUtils;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Controllers.Utils
{
    public class PersistenceHelper<TEntity, TEntityId, TDbContext> : IPersistenceHelper<TEntity, TEntityId> 
        where TEntity : class 
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly DbSet<TEntity> _entitySet;
        private readonly ValidationErrorMessages _notFoundErrorMessage;
            
        public PersistenceHelper(
            TDbContext context,
            DbSet<TEntity> entitySet,
            ValidationErrorMessages notFoundErrorMessage = ValidationErrorMessages.EntityNotFound)
        {
            _context = context;
            _entitySet = entitySet;
            _notFoundErrorMessage = notFoundErrorMessage;
        }
        
        public Task<Either<ActionResult, T>> CheckEntityExistsActionResult<T>(
            TEntityId id, 
            Func<TEntity, Task<Either<ActionResult, T>>> successAction,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)   
        {
            return HandleErrorsAsync(
                async () =>
                {
                    var queryableEntities = _entitySet
                        .FindByPrimaryKey(_context, id);

                    var hydratedEntities = hydrateEntityFn != null
                        ? hydrateEntityFn.Invoke(queryableEntities)
                        : queryableEntities;
                    
                    var entity = await hydratedEntities
                        .FirstOrDefaultAsync();

                    return entity == null
                        ? new NotFoundResult()
                        : new Either<ActionResult, TEntity>(entity);
                },
                successAction.Invoke);
        } 
        
        // TODO EES-919 - return ActionResults rather than ValidationResults
        public Task<Either<ValidationResult, T>> CheckEntityExists<T>(
            TEntityId id, 
            Func<TEntity, Task<Either<ValidationResult, T>>> successAction,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)   
        {
            return HandleValidationErrorsAsync(
                async () =>
                {
                    var queryableEntities = _entitySet
                        .FindByPrimaryKey(_context, id);

                    var hydratedEntities = hydrateEntityFn != null
                        ? hydrateEntityFn.Invoke(queryableEntities)
                        : queryableEntities;
                    
                    var entity = await hydratedEntities
                        .FirstOrDefaultAsync();

                    return entity == null
                        ? ValidationResult(_notFoundErrorMessage)
                        : new Either<ValidationResult, TEntity>(entity);
                },
                successAction.Invoke);
        } 
        
        // TODO EES-919 - return ActionResults rather than ValidationResults
        public Task<Either<ValidationResult, T>> CheckEntityExists<T>(
            TEntityId id, 
            Func<TEntity, T> successAction,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)
        {
            Task<T> Success(TEntity entity) => Task.FromResult(successAction.Invoke(entity));

            return CheckEntityExists(
                id, 
                Success,
                hydrateEntityFn);
        } 
        
        // TODO EES-919 - return ActionResults rather than ValidationResults
        public Task<Either<ValidationResult, T>> CheckEntityExists<T>(
            TEntityId id, 
            Func<TEntity, Task<T>> successAction,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)
        {
            async Task<Either<ValidationResult, T>> Success(TEntity entity)
            {
                var result = await successAction.Invoke(entity);
                return new Either<ValidationResult, T>(result);
            }

            return CheckEntityExists(
                id, 
                Success,
                hydrateEntityFn);
        }

        // TODO EES-919 - return ActionResults rather than ValidationResults
        public Task<Either<ValidationResult, TEntity>> CheckEntityExistsChainable(
            TEntityId id, 
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)
        {
            return CheckEntityExists(id, entity => entity, hydrateEntityFn);
        }

        // TODO EES-919 - return ActionResults rather than ValidationResults
        public Task<Either<ActionResult, TEntity>> CheckEntityExistsChainableActionResult(
            TEntityId id, 
            Func<IQueryable<TEntity>, IQueryable<TEntity>> hydrateEntityFn = null)
        {
            return CheckEntityExistsActionResult(id, 
                entity => Task.FromResult(new Either<ActionResult, TEntity>(entity)), 
                hydrateEntityFn);
        }
    }
}