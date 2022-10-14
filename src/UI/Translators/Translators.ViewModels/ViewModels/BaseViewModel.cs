using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;
using Translators.ViewModels.Pages;

namespace Translators.ViewModels
{
    public class BaseViewModel : BaseModel
    {
        public static ConcurrentDictionary<Type, BaseViewModel> Pages { get; set; } = new ConcurrentDictionary<Type, BaseViewModel>();
        public BaseViewModel()
        {
            RefreshCommand = CommandHelper.Create(() => LoadData(true));
            Pages.AddOrUpdate(this.GetType(), this, (t, vm) => this);
        }

        public ICommand RefreshCommand { get; set; }
        public string SelectedName { get; set; }

        bool _IsLoading;
        public static int _FontSize = 15;
        public static bool _UseDuplexProtocol = false;
        public static double _PlaybackSpeedRato = 1.0;

        bool isFirstTime = true;

        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                _IsLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


        public int FontSize
        {
            get => _FontSize;
            set
            {
                _FontSize = value;
                OnPropertyChanged(nameof(FontSize));
                ApplicationSettingData.Current.Save();
            }
        }

        public bool UseDuplexProtocol
        {
            get => _UseDuplexProtocol;
            set
            {
                _UseDuplexProtocol = value;
                OnPropertyChanged(nameof(UseDuplexProtocol));
                TranslatorService.IsDuplexProtocol = value;
                ApplicationSettingData.Current.Save();
            }
        }

        public bool IsInSearchTab { get; set; }

        public async Task LoadData(bool isForce = false)
        {
            if (IsLoading)
                return;
            if (isFirstTime)
            {
                isForce = false;
                isFirstTime = false;
            }
            try
            {
                IsLoading = true;
                await FetchData(isForce);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual Task WaitToFetchData()
        {
            return Task.CompletedTask;
        }

        public virtual Task FetchData(bool isForce = false)
        {
            return Task.CompletedTask;
        }

        public static string FixArabicForSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return CleanArabicChars(text).Replace("ٱ", "ا").Replace("آ", "ا").Replace("إ", "ا").Replace("أ", "ا").Replace("ء", "").Replace("ؤ", "و").Replace("ة", "ه").Replace("ۀ", "ه").Replace('ك', 'ک').Replace('ڪ', 'ک').Replace('ئ', 'ی').Replace('ي', 'ی').Replace('ى', 'ی')
                .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9")
                .Replace("٠", "0").Replace("١", "1").Replace("٢", "2").Replace("٣", "3").Replace("٤", "4").Replace("٥", "5").Replace("٦", "6").Replace("٧", "7").Replace("٨", "8").Replace("٩", "9")
                .Replace(" ", "");
        }

        public static string CleanArabicChars(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return text.Replace("ۛ", "").Replace("ۖ", "").Replace("ۗ", "").Replace("ۚ", "").Replace("ۙ", "").Replace("ۘ", "").Replace("ۜ", "").Replace("ِ", "").Replace("ُ", "").Replace("َ", "").Replace("ً", "").Replace("ٌ", "").Replace("ٍ", "").Replace("ّ", "").Replace("ۡ", "");
        }

        public static string GetSelectedTitleByType(Type type)
        {
            if (Pages.TryGetValue(type, out var vm))
                return vm.SelectedName;
            return default;
        }

        public static async Task OnSelectedTitleByType(Type type, long id, long parentId)
        {
            if (Pages.TryGetValue(type, out var vm))
            {
                await vm.WaitToFetchData();
                vm.OnSelected(id, parentId);
            }
        }

        public virtual void OnSelected(long id, long parentId)
        {

        }

        public static async Task AlertContract(MessageContract messageContract)
        {
            if (!messageContract.IsSuccess)
                await AlertHelper.Alert("خطا", messageContract.Error?.Message);
        }

        public static async Task AlertExcepption(Exception exception)
        {
            try
            {
                await AlertHelper.Alert("خطا", exception.Message);
            }
            catch
            {

            }
        }
    }
}
