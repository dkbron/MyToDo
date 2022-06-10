using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.Common.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        public ToDoViewModel(IToDoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            AddToDoCommand = new DelegateCommand(AddToDo); 
            this.service = service; 
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

        public async void CreateDataAsync()
        {
            UpdateLoading(true);

            var toDoResult = await service.GetAllAsync(
                new QueryParameter {
                PageIndex = 0,
                PageSize = 100,   
                }  );

            if(toDoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var toDoItem in toDoResult.Result.Items)
                {
                    ToDoDtos.Add(toDoItem);
                }
                //new Task(() =>
                //{
                //    Application.Current.Dispatcher.Invoke(() =>
                //    {
                //        foreach (var toDoItem in toDoResult.Result.Items)
                //        {
                //            ToDoDtos.Add(toDoItem);
                //        }
                //    }
                //    ); 
                //}).Start(); 
            }

            UpdateLoading(false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            CreateDataAsync();
        }

    }
}
