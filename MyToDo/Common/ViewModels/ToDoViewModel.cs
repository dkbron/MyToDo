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
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);

            this.service = service;
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
            set { toDoDtos = value; RaisePropertyChanged(); }
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

        public void Add()
        {
            IsRightDrawerOpen = true;
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
                        var toDo = ToDoDtos.FirstOrDefault(t => t.Id == currentDto.Id);
                        if (toDo != null)
                        {
                            toDo.Title = currentDto.Title;
                            toDo.Content = currentDto.Content;
                            toDo.Status = currentDto.Status;
                        }
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(currentDto);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
                isRightDrawerOpen = false;
            }



        }

        public async void CreateDataAsync()
        {
            UpdateLoading(true);

            var toDoResult = await service.GetAllAsync(
                new QueryParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = this.Search
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
