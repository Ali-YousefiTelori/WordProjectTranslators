using System.Collections.Generic;
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
        public List<ValueContract> Names { get; set; }
        public ServiceType Type { get; set; }

        public static implicit operator CategoryModel(CategoryContract category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Names = category.Names,
                Type = ServiceType.Category
            };
        }

        public static implicit operator CategoryModel(BookContract book)
        {
            return new CategoryModel()
            {
                Id = book.Id,
                Names = book.Names,
                Type = ServiceType.Book
            };
        }
    }
}
