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
            RefreshCommand = CommandHelper.Create(LoadData);
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


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task LoadData()
        {
            try
            {
                IsLoading = true;
                await FetchData();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                IsLoading = false;
            }
        }

        public virtual Task FetchData()
        {
            return Task.CompletedTask;
        }
    }
}
