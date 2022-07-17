using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    { 

        public string Title => "登录界面";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;    
        }

        public void OnDialogClosed()
        { 
        }

        public void OnDialogOpened(IDialogParameters parameters)
        { 
        }
    }
}
