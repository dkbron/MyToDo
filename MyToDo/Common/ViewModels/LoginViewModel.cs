using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Service;
using Prism.Regions;

namespace MyToDo.Common.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IUserService service;  

        public string Title => "登录界面";

        public event Action<IDialogResult> RequestClose;

        public LoginViewModel(IUserService service)
        {
            this.service = service;
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login":
                    Login();
                    break;
                case "Register":
                    Register();
                    break;
                case "forget":
                    ForgetPassword();
                    break;
            }
        }

        private async void Login()
        {

            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
            {
                return;
            }
            var result = await service.LoginAsync(Account, Password);

            if (result.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                HintAccount = "账号或密码错误";
                HintPassword = "账号或密码错误";
            }
        }

        private void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Register()
        {

        }

        private async void ForgetPassword()
        {

        }

        public bool CanCloseDialog()
        {
            return true;    
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        { 
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }

        private string hintAccount = "请输入账号";

        public string HintAccount
        {
            get { return hintAccount; }
            set { hintAccount = value; RaisePropertyChanged(); }
        }

        private string hintPassword = "请输入密码";

        public string HintPassword
        {
            get { return hintPassword; }
            set { hintPassword = value; RaisePropertyChanged(); }
        }
         
    }
}
