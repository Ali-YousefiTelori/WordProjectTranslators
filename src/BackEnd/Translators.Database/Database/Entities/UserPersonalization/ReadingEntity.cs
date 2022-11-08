using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Database.Entities.Authentications;

namespace Translators.Database.Entities.UserPersonalization
{
    public class ReadingEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public int StartPageNumber { get; set; }

        public long CategoryId { get; set; }
        public long BookId { get; set; }
        public long CatalogId { get; set; }
        public long PageId { get; set; }
        public long UserId { get; set; }

        public BookEntity Book { get; set; }
        public CatalogEntity Catalog { get; set; }
        public PageEntity Page { get; set; }
        public UserEntity User { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
