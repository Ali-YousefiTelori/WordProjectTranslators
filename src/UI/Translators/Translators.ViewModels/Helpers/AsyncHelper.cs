using System;
using Translators.ServiceManagers;

namespace Translators.Helpers
{
    public static class AsyncHelper
    {
        public static Action<Action> RunOnUAction { get; set; }
        public static void RunOnUI(Action action)
        {
            try
            {
                RunOnUAction.Invoke(action);
            }
            catch (Exception ex)
            {
                TranslatorService.LogException(ex.ToString());
            }
        }
    }
}
