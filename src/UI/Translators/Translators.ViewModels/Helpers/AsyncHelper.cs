using System;
using System.Threading.Tasks;
using Translators.ServiceManagers;

namespace Translators.Helpers
{
    public static class AsyncHelper
    {
        public static Action<Action> RunOnUAction { private get; set; }
        public static Func<Func<Task>, Task> RunOnUIFunc { private get; set; }
        public static Func<Func<Task<object>>, Task<object>> RunOnUIResultFunc { private get; set; }
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

        public static async Task RunOnUI(Func<Task> action)
        {
            try
            {
                await RunOnUIFunc.Invoke(action);
            }
            catch (Exception ex)
            {
                TranslatorService.LogException(ex.ToString());
            }
        }

        public static async Task<T> RunOnUI<T>(Func<Task<T>> action)
        {
            try
            {
                return (T)await RunOnUIResultFunc.Invoke(async () =>
                {
                    var result = await action();
                    return result;
                });
            }
            catch (Exception ex)
            {
                TranslatorService.LogException(ex.ToString());
            }
            return default(T);
        }
    }
}
