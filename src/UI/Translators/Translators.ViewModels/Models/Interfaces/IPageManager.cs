using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.ViewModels;

namespace Translators.Models.Interfaces
{
    public interface IPageManager
    {
        void SwitchPage(PageType pageType);
        Task<object> PushPage(long id, long rootId, object data, PageType pageType, BaseViewModel fromBaseViewModel);
        Task Clean();
    }
}
