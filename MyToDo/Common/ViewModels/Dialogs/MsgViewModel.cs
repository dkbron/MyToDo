using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels.Dialogs
{
    public class MsgViewModel : IDialogHostAware
    {
        public string DialogHostName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DelegateCommand SaveCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DelegateCommand CancelCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
