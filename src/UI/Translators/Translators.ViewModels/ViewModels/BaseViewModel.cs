using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Interfaces;

namespace Translators.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            RefreshCommand = CommandHelper.Create(()=> LoadData(true));
        }

        public ICommand RefreshCommand { get; set; }

        bool _IsLoading;

        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                _IsLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        bool isFirstTime = true;

        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
            catch(Exception ex)
            {

            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual Task FetchData(bool isForce = false)
        {
            return Task.CompletedTask;
        }

        public static string FixArabicForSearch(string text)
        {
            return text.Replace("ٱ", "ا").Replace("آ", "ا").Replace("إ", "ا").Replace("أ", "ا").Replace("ء", "").Replace("ؤ", "و").Replace("ة", "ه").Replace("ۀ", "ه").Replace('ك', 'ک').Replace('ڪ', 'ک').Replace('ئ', 'ی').Replace('ي', 'ی').Replace('ى', 'ی')
                .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9")
                .Replace("٠", "0").Replace("١", "1").Replace("٢", "2").Replace("٣", "3").Replace("٤", "4").Replace("٥", "5").Replace("٦", "6").Replace("٧", "7").Replace("٨", "8").Replace("٩", "9");
        }
    }
}
