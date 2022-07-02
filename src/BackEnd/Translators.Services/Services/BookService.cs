﻿using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
using Translators.Attributes;
using Translators.Contracts.Common;
using Translators.Database.Contexts;
using Translators.Database.Entities;
using Translators.Logics;

namespace Translators.Services
{
    [ServiceContract("Book", ServiceType.HttpService, InstanceType.SingleInstance)]
    [ServiceContract("Book", ServiceType.ServerService, InstanceType.SingleInstance)]
    public class BookService
    {
        [JsonCustomSerialization]
        public async Task<MessageContract<List<CategoryContract>>> GetCategories()
        {
            return await new LogicBase<TranslatorContext, CategoryContract, CategoryEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator));
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<List<BookContract>>> GetBooks()
        {
            return await new LogicBase<TranslatorContext, BookContract, BookEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(x => !x.IsHidden));
        }

        [JsonCustomSerialization]
        public async Task<MessageContract<List<BookContract>>> FilterBooks(long categoryId)
        {
            return await new LogicBase<TranslatorContext, BookContract, BookEntity>().GetAll(x => x.Include(q => q.Names).ThenInclude(n => n.Language).Include(q => q.Names).ThenInclude(n => n.Translator).Where(q => q.CategoryId == categoryId && !q.IsHidden));
        }
    }
}
