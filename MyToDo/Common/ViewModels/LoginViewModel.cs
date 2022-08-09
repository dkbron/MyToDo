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
using Prism.Events;
using MyToDo.Extensions;

namespace MyToDo.Common.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IUserService service;
        private readonly IEventAggregator aggregator;

        public string Title => "登录界面";

        public event Action<IDialogResult> RequestClose;

        public LoginViewModel(IUserService service, IEventAggregator aggregator)
        {
            this.service = service;
            this.aggregator = aggregator;
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value;  RaisePropertyChanged(); }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login":
                    Login();
                    break;
                case "LoginOut":
                    LoginOut();
                    break;
                case "Register":
                    Register();
                    break;
                case "Forget":
                    ForgetPassword();
                    break;
                case "GoToRegister":
                    InitValue();
                    SelectedIndex = 1;
                    break;
                case "Return":
                    InitValue();
                    SelectedIndex = 0;
                    break;
            }
        }

        private void InitValue()
        {
            Account = String.Empty;
            Password = String.Empty;
            RePassword = String.Empty;
            UserName = String.Empty;
            ErrorMsg = String.Empty;
        }

        private async void Login()
        {

            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
            { 
                aggregator.SendMessage("输入不能为空！", "Login"); 
                return;
            }
            var result = await service.LoginAsync(Account, Password);

            if (result.Status)
            {
                AppSession.UserName = result.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                Password = String.Empty; 
                aggregator.SendMessage(result.Message,"Login");
            }
        }

        private void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(Account) 
                || string.IsNullOrEmpty(Password)
                ||string.IsNullOrEmpty(RePassword)
                ||string.IsNullOrEmpty(UserName)
                )
            {
                aggregator.SendMessage("输入不能为空！", "Login");
                return;
            }

            if(Password != RePassword)
            {
                aggregator.SendMessage("密码重复输入错误！", "Login"); 
                return;
            }
            var result = await service.RegisterAsync(new Shared.Dtos.UserDto() { Account = Account, Password = Password, UserName = UserName});
             
            if(result == null)
            {
                return;
            }

            if (result.Status)
            { 
                SelectedIndex = 0;
            }
            else
            { 
                Account = String.Empty;
                Password = String.Empty;
                RePassword = String.Empty;
                UserName = String.Empty;
                aggregator.SendMessage(result.Message, "Login"); 
            }
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

        private string hintColor = "Black";

        public string HintColor
        {
            get { return hintColor; }
            set { hintColor = value;  RaisePropertyChanged(); }
        }



        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }

        private string rePassword;

        public string RePassword
        {
            get { return rePassword; }
            set { rePassword = value; RaisePropertyChanged(); }
        }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string errorMsg = string.Empty;

        public string ErrorMsg
        {
            get { return errorMsg; }
            set { errorMsg = value; RaisePropertyChanged(); }
        }




    }
}
