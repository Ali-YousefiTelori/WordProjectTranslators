using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IPageManager
    {
        Task<object> PushPage(long id, long rootId, object data, PageType pageType);
        Task Clean();
    }
}
