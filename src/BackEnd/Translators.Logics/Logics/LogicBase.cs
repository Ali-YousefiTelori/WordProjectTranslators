using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Translators.Contracts.Common;
using Translators.Database.Contexts;

namespace Translators.Logics
{
    public class LogicBase<TContext, TContract, TEntity>
        where TContext : TranslatorContext, new()
        where TEntity : class
    {
        async Task<TEntity> AddToDatabase(TEntity entity)
        {
            TContext context = new TContext();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        async Task<TContract> AddToDatabase(TContract contract)
        {
            TContext context = new TContext();
            var entity = contract.Map<TEntity>();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Map<TContract>();
        }

        async Task<TContract> UpdateToDatabase(TContract contract)
        {
            TContext context = new TContext();
            var entity = contract.Map<TEntity>();
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity.Map<TContract>();
        }

        async Task<List<TContract>> GetAllFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
            TContext context = new TContext();
            var query =  context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entities = await query.ToListAsync();
            return entities.Select(x => x.Map<TContract>()).ToList();
        }

        async Task<TContract> FindFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
            TContext context = new TContext();
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entity = await query.FirstOrDefaultAsync();
            return entity.Map<TContract>();
        }

        public async Task<MessageContract<TEntity>> Add(TEntity entity)
        {
            return await AddToDatabase(entity);
        }

        public async Task<MessageContract<TContract>> Add(TContract contract)
        {
            return await AddToDatabase(contract);
        }

        public async Task<MessageContract<TContract>> Update(TContract contract)
        {
            return await UpdateToDatabase(contract);
        }

        public async Task<MessageContract<List<TContract>>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery = null)
        {
            return await GetAllFromDatabase(getQuery);
        }

        public async Task<MessageContract<TContract>> Find(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery = null)
        {
            return await FindFromDatabase(getQuery);
        }
    }
}
