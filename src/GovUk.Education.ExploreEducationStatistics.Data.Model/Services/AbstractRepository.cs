using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Database;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Model.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GovUk.Education.ExploreEducationStatistics.Data.Model.Services
{
    public abstract class AbstractRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly StatisticsDbContext _context;

        protected readonly ILogger _logger;

        protected AbstractRepository(StatisticsDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        protected DbSet<TEntity> DbSet()
        {
            return _context.Set<TEntity>();
        }

        public Task<int> Count()
        {
            return DbSet().CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet().Count(expression);
        }

        public IQueryable<TEntity> All(List<Expression<Func<TEntity, object>>> include = null)
        {
            var queryable = DbSet().AsQueryable();
            include?.ForEach(i => queryable = queryable.Include(i));
            return queryable;
        }

        public TEntity Find(TKey id, List<Expression<Func<TEntity, object>>> include)
        {
            var queryable = DbSet().AsQueryable();
            include.ForEach(i => queryable = queryable.Include(i));
            return queryable
                .FindByPrimaryKey(_context, id)
                .SingleOrDefault();
        }

        public TEntity Find(TKey id)
        {
            return DbSet().Find(id);
        }

        public IQueryable<TEntity> Find(TKey[] ids)
        {
            var idField = typeof(TEntity).GetProperty("Id");
            
            var list = ids.ToList();
            // build lambda expression
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var methodInfo = typeof(List<TKey>).GetMethod("Contains");
            var body = Expression.Call(Expression.Constant(list, typeof(List<TKey>)), methodInfo, Expression.MakeMemberAccess(parameter, idField));
            var predicateExpression = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            // run query
            return DbSet().Where(predicateExpression);
            
//            return DbSet().FindAll(_context, ids.Cast<object>().ToArray());
        }

        public IQueryable<TEntity> FindMany(Expression<Func<TEntity, bool>> expression,
            List<Expression<Func<TEntity, object>>> include = null)
        {
            var queryable = DbSet().Where(expression);
            include?.ForEach(i => queryable = queryable.Include(i));
            return queryable;
        }

        public int Max(Expression<Func<TEntity, int>> expression)
        {
            return DbSet().Max(expression);
        }

        public long TopWithPredicate(Expression<Func<TEntity, long>> expression,
            Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet().Where(predicate)
                .OrderByDescending(expression)
                .Select(expression)
                .FirstOrDefault();
        }

        protected static SqlParameter CreateIdListType(string parameterName, IEnumerable<long> values)
        {
            return CreateListType(parameterName, values.AsIdListTable(), "dbo.IdListIntegerType");
        }

        protected static SqlParameter CreateIdListType(string parameterName, IEnumerable<string> values)
        {
            return CreateListType(parameterName, values.AsIdListTable(), "dbo.IdListVarcharType");
        }

        protected static SqlParameter CreateListType(string parameterName, object value, string typeName)
        {
            return new SqlParameter(parameterName, value)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = typeName
            };
        }
    }
}