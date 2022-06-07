using Microsoft.EntityFrameworkCore;
using SignalGo.Shared.DataTypes;
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
        public async Task<MessageContract<List<CategoryContract>>> GetCategories()
        {
            return await new LogicBase<TranslatorContext, CategoryContract, CategoryEntity>().GetAll(x => x.Include(q => q.Name));
        }

        public async Task<MessageContract<List<BookContract>>> GetBooks()
        {
            return await new LogicBase<TranslatorContext, BookContract, BookEntity>().GetAll(x => x.Include(q => q.Name));
        }

        public async Task<MessageContract<List<BookContract>>> FilterBooks(long categoryId)
        {
            return await new LogicBase<TranslatorContext, BookContract, BookEntity>().GetAll(x => x.Include(q => q.Name).Where(q => q.CategoryId == categoryId));
        }
    }
}
