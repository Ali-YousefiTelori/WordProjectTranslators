using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Models.Interfaces
{
    public interface IClipboardManager
    {
        Task CopyText(string text);
    }
}
