﻿using Newtonsoft.Json;
using Translators.Contracts.Common;
using Translators.Database.Contexts;

namespace Translators.Logics
{
    public class LogicBase<TContext, TContract, TEntity>
        where TContext : TranslatorContext, new()
    {
        TEntity Map(TContract contract)
        {
            var json = JsonConvert.SerializeObject(contract);
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        TContract Map(TEntity entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            return JsonConvert.DeserializeObject<TContract>(json);
        }

        async Task<TContract> AddToDatabase(TContract contract)
        {
            TContext context = new TContext();
            var entity = Map(contract);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return Map(entity);
        }

        async Task<TContract> UpdateToDatabase(TContract contract)
        {
            TContext context = new TContext();
            var entity = Map(contract);
            context.Update(entity);
            await context.SaveChangesAsync();
            return Map(entity);
        }

        public async Task<MessageContract<TContract>> Add(TContract contract)
        {
            return await AddToDatabase(contract);
        }

        public async Task<MessageContract<TContract>> Update(TContract contract)
        {
            return await UpdateToDatabase(contract);
        }
    }
}