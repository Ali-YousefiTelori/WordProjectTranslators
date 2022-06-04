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
        public async Task<MessageContract<List<BookContract>>> GetAll()
        {
            return await new LogicBase<TranslatorContext, BookContract, BookEntity>().GetAll();
        }
    }
}
