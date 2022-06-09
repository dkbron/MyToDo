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
    public class ToDoViewModel : BindableBase
    {
        public ToDoViewModel()
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            AddToDoCommand = new DelegateCommand(AddToDo);
            CreateToDoData();
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand AddToDoCommand { get; private set; }

        public void AddToDo()
        {
            IsRightDrawerOpen = true;
        } 

        public void CreateToDoData()
        {
            for (int i = 1; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Id = i, Title = $"代办事项{i}", Content = $"{i}事件.....", Status = 1, CreateDate = DateTime.Now.Date }); 
            }
        }

    }
}
