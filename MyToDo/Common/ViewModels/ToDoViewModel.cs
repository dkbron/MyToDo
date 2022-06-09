using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Parameters;
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
        public ToDoViewModel(IToDoService service)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            AddToDoCommand = new DelegateCommand(AddToDo); 
            this.service = service;
            CreateToDoData();
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;
        private readonly IToDoService service;

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

        public async void CreateToDoData()
        { 
            var toDoResult = await service.GetAllAsync(
                new QueryParameter {
                PageIndex = 0,
                PageSize = 100,   
                }  );

            if(toDoResult.Status)
            {
                toDoDtos.Clear();
                foreach (var toDoItem in toDoResult.Result.Items)
                {
                    ToDoDtos.Add(toDoItem);
                }
            }

        }

    }
}
