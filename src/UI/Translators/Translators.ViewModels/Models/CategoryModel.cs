using System.Linq;
using Translators.Contracts.Common;

namespace Translators.Models
{
    public enum ServiceType : byte
    {
        None = 0,
        Category = 1,
        Book = 2
    }

    public class CategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ServiceType Type { get; set; }

        public static implicit operator CategoryModel(CategoryContract category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Names.First(x => x.IsMain).Value,
                Type = ServiceType.Category
            };
        }

        public static implicit operator CategoryModel(BookContract book)
        {
            return new CategoryModel()
            {
                Id = book.Id,
                Name = book.Names.First(x => x.IsMain).Value,
                Type = ServiceType.Book
            };
        }
    }
}
