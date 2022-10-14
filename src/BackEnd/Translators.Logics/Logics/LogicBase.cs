using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.Models;
using Translators.Contracts.Common;
using Translators.Database.Contexts;

namespace Translators.Logics
{
    public class LogicBase<TContext, TContract, TEntity>
        where TContext : TranslatorContext, new()
        where TEntity : class
    {
        async Task<List<TEntity>> AddRangeToDatabase(List<TEntity> entities)
        {
            TContext context = new TContext();
            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }

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
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entities = await query.ToListAsync();
            return entities.Select(x => x.Map<TContract>()).ToList();
        }

        async Task<TContract> GetFirstOrDefaultFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
            TContext context = new TContext();
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entity = await query.FirstOrDefaultAsync();
            if (entity == default)
                return default;
            return entity.Map<TContract>();
        }

        async Task<TContract> FindFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
            TContext context = new TContext();
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entity = await query.FirstOrDefaultAsync();
            if (entity is TContract contract)
                return contract;
            return entity.Map<TContract>();
        }

        public async Task<MessageContract<List<TEntity>>> AddRange(List<TEntity> entities)
        {
            return await AddRangeToDatabase(entities);
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

        public async Task<MessageContract<TContract>> FirstOrDefault(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery = null)
        {
            var contract = await GetFirstOrDefaultFromDatabase(getQuery);
            if (contract is null)
                return FailedReasonType.NotFound;
            return contract;
        }

        public async Task<MessageContract<TContract>> Find(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery = null)
        {
            return await FindFromDatabase(getQuery);
        }
    }

    public class LogicBase<TContext, TEntity> : LogicBase<TContext, byte, TEntity>
        where TContext : TranslatorContext, new()
        where TEntity : class
    {
    }
}
