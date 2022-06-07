using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IPageManager
    {
        Task PushPage(long id, PageType pageType);
    }
}
