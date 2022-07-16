using MaterialDesignThemes.Wpf;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels.Dialogs
{
    public class AddMemoViewModel :BindableBase, IDialogHostAware
    {
        public AddMemoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));

        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Model",MemoDto);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private MemoDto memoDto;

        public MemoDto MemoDto
        {
            get { return memoDto; }
            set { memoDto = value;RaisePropertyChanged(); }
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Model"))
                MemoDto = parameters.GetValue<MemoDto>("Model");
        }
    }
}
