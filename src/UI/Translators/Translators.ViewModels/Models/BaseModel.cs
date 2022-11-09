using System;
using System.ComponentModel;
using Translators.ServiceManagers;

namespace Translators.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    TranslatorService.LogException(ex.ToString());
                }
            }
        }
    }
}
