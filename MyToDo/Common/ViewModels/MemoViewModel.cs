using MyToDo.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels
{
    public class MemoViewModel:BindableBase
    {
        public MemoViewModel()
        {
            AddMemoCommand = new DelegateCommand(CreateMemoData);

            MemoDtos = new ObservableCollection<MemoDto>();
            CreateMemoData();
        }
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand AddMemoCommand { get; private set; }

        public void AddMemo()
        {
            IsRightDrawerOpen = true;
        }

        public void CreateMemoData()
        {
            for (int i = 1; i < 10; i++)
            {
                MemoDtos.Add(new MemoDto() { Id = i, Title = $"备忘{i}", Content = $"{i}事件.....", Status = 1, CreateDate = DateTime.Now.Date });
            }
        }

    }
}
