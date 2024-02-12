using EasyMicroservices.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Database.Contexts;

namespace Translators.Logics
{
    public class LogicBase<TContract, TEntity>
        where TEntity : class
    {
        TranslatorContext context;
        public LogicBase(TranslatorContext translatorContext)
        {
            context = translatorContext;
        }
        async Task<List<TEntity>> AddRangeToDatabase(List<TEntity> entities)
        {
            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }

        protected async Task<TEntity> AddToDatabase(TEntity entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        async Task<TContract> AddToDatabase(TContract contract)
        {
            var entity = contract.Map<TEntity>();
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Map<TContract>();
        }

        async Task<TContract> UpdateToDatabase(TContract contract)
        {
            var entity = contract.Map<TEntity>();
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity.Map<TContract>();
        }

        async Task<List<TContract>> UpdateAllToDatabase(List<TContract> contracts)
        {
            var entities = contracts.Map<List<TEntity>>();
            context.UpdateRange(entities);
            await context.SaveChangesAsync();
            return entities.MapToList<TContract>();
        }

        async Task<List<TContract>> GetAllFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entities = await query.ToListAsync();
            return entities.MapToList<TContract>();
        }

        async Task<TContract> GetFirstOrDefaultFromDatabase(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery)
        {
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
            var query = context.Set<TEntity>().AsQueryable();
            if (getQuery != null)
                query = getQuery(query);
            var entity = await query.FirstOrDefaultAsync();
            if (entity is TContract contract)
                return contract;
            return entity.Map<TContract>();
        }

        public async Task<ListMessageContract<TEntity>> AddRange(List<TEntity> entities)
        {
            return await AddRangeToDatabase(entities);
        }

        public async Task<MessageContract<TContract>> Add(TContract contract)
        {
            return await AddToDatabase(contract);
        }

        public async Task<MessageContract<TContract>> Update(TContract contract)
        {
            return await UpdateToDatabase(contract);
        }

        public async Task<ListMessageContract<TContract>> UpdateAll(IEnumerable<TContract> contracts)
        {
            return await UpdateAllToDatabase(contracts.ToList());
        }

        public async Task<ListMessageContract<TContract>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> getQuery = null)
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

    public class LogicBase<TEntity> : LogicBase<TEntity, TEntity>
        where TEntity : class
    {
        public LogicBase(TranslatorContext translatorContext) : base(translatorContext)
        {
        }
    }
}
