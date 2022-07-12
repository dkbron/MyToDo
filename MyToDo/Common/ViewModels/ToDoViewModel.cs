using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.Common.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;

        public ToDoViewModel(IToDoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            this.service = service;

            dialogHost = containerProvider.Resolve<IDialogHostService>();
        }

        private async void Selected(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var result = await service.GetFirstOfDefaultAsync(obj.Id);

                if (result.Status)
                {
                    CurrentDto = result.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": CreateDataAsync(); break;
                case "更新": Save(); break; 
            }
        }
         
        private string search;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        private int statusIndex;

        public int StatusIndex
        {
            get { return statusIndex; }
            set { statusIndex = value;  RaisePropertyChanged(); }
        }



        private ToDoDto currentDto;

        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set
            {
                toDoDtos = value; 
                RaisePropertyChanged(); 
            }
        }

        private bool isRightDrawerOpen;
        private readonly IToDoService service;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }

        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }

        public void Add()
        {
            IsRightDrawerOpen = true;
            CurrentDto = new ToDoDto();
        }

        private async void Save()
        {
            if (String.IsNullOrWhiteSpace(CurrentDto.Title) || String.IsNullOrWhiteSpace(CurrentDto.Content))
                return;

            UpdateLoading(true); 
            try
            {
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto); 
                    if (updateResult.Status)
                    { 
                        var toDo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);  
                        if (toDo != null)
                        {
                            int index = ToDoDtos.IndexOf(toDo);

                            toDo.Title = CurrentDto.Title;
                            toDo.Content = CurrentDto.Content;
                            toDo.Status = CurrentDto.Status;

                            //上方数据改变后前端数据没有更新，是深浅拷贝的问题，先移除数据后在插入数据可正常更新
                            ToDoDtos.Remove(toDo);
                            ToDoDtos.Insert(index, toDo); 
                        }

                        //ObservableCollection<ToDoDto> tempCollection = ToDoDtos;
                        //ToDoDtos = new ObservableCollection<ToDoDto>();

                        //ToDoDtos = tempCollection;
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false); 
                IsRightDrawerOpen = false;
            }
        }

        private async void Delete(ToDoDto toDoDto)
        {
            var dialogResult = await dialogHost.Question("温馨提示",$"确认删除待办事项:{toDoDto.Title}");
            if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.No)
                return;
             
            var toDoResult = await service.DeleteAsync(toDoDto.Id);

            if (toDoResult != null && toDoResult.Status)
            {
                var item = ToDoDtos.FirstOrDefault(x => x.Id == toDoDto.Id);
                if (item != null)
                    ToDoDtos.Remove(item);
            }
        }

        public async void CreateDataAsync()
        {
            UpdateLoading(true);

            int? statusIndex = StatusIndex == 0 ? null : StatusIndex == 2 ? 1 : 0;

            var toDoResult = await service.GetAllFilterAsync(
                new ToDoParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = this.Search,
                    StatusIndex = statusIndex
                });

            if (toDoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var toDoItem in toDoResult.Result.Items)
                {
                    ToDoDtos.Add(toDoItem);
                }
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
