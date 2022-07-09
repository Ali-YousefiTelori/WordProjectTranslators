﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel()
        {
            RegisterCommand = CommandHelper.Create(Register);
            _ = LoadData();
        }

        public ICommand RegisterCommand { get; set; }

        string _UserName;
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        bool _CanRegister;
        public bool CanRegister
        {
            get => _CanRegister;
            set
            {
                _CanRegister = value;
                OnPropertyChanged(nameof(CanRegister));
            }
        }


        private async Task Register()
        {
            var userName = await AlertHelper.DisplayPrompt("نام کاربری", "نام کاربری را وارد کنید");
            userName = userName == null ? "" : userName;
            userName = new string(userName.Where(x => "abcdefghijklmnopqrstuvwxyz".Contains(x.ToString().ToLower())).ToArray());
            if (string.IsNullOrEmpty(userName) || userName.Length < 3)
            {
                await AlertHelper.Alert("خطا", "نام کاربری نمی تواند کمتر از سه حرف داشته باشد و فقط باید شامل حروف انگلیسی باشد.");
                return;
            }
            try
            {
                IsLoading = true;
                var result = await TranslatorService.GetAuthenticationService(true).RegisterAsync(userName);
                if (result.IsSuccess)
                {
                    ApplicationProfileData.Current.Value.Session = Guid.Parse(result.Result.Key);
                    UserName = result.Result.UserName;
                    ApplicationProfileData.Current.Save();
                    CanRegister = false;
                }
                else
                    await AlertContract(result);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override async Task FetchData(bool isForce = false)
        {
            if (ApplicationProfileData.Current.Value.Session.HasValue)
            {
                var result = await ApplicationProfileData.Login(ApplicationProfileData.Current.Value.Session);
                if (result.IsSuccess)
                {
                    UserName = result.Result.UserName;
                    ApplicationProfileData.Current.Save();
                }
                else
                {
                    await AlertContract(result);
                    CanRegister = true;
                }
            }
            else
                CanRegister = true;
        }
    }
}
