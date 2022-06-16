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
    }
}
