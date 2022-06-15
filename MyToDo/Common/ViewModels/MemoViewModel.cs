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

namespace MyToDo.Common.ViewModels
{
    public class MemoViewModel:NavigationViewModel
    {
        public MemoViewModel(IMemoService service, IContainerProvider containerProvider):base(containerProvider)
        { 
            MemoDtos = new ObservableCollection<MemoDto>();

            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);

            this.service = service; 
        }

        private MemoDto currentDto;

        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;  RaisePropertyChanged(); }
        }


        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;
        private readonly IMemoService service;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
          
        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }



        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand {get; private set; }

        public void Add()
        {
            IsRightDrawerOpen = true;
            CurrentDto = new MemoDto();
        }

        public async void CreateDataAsync()
        {
            UpdateLoading(true); 

            var result = await service.GetAllAsync(
                new QueryParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = this.Search, 
                });

            if (result.Status)
            {
                memoDtos.Clear();
                foreach(var item in result.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            } 
            UpdateLoading(false);
        }

        private async void Delete(MemoDto memoDto)
        {
            try
            {
                UpdateLoading(true);
                var result = await service.DeleteAsync(memoDto.Id);

                if (result != null && result.Status)
                {
                    var item = MemoDtos.FirstOrDefault(x => x.Id == memoDto.Id);
                    if (item != null)
                        MemoDtos.Remove(item);
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

        private async void Selected(MemoDto obj)
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
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (memo != null)
                        {
                            int index = MemoDtos.IndexOf(memo);

                            memo.Title = CurrentDto.Title;
                            memo.Content = CurrentDto.Content;
                            memo.Status = CurrentDto.Status;

                            //上方数据改变后前端数据没有更新，是深浅拷贝的问题，先移除数据后在插入数据可正常更新
                            MemoDtos.Remove(memo);
                            MemoDtos.Insert(index, memo);
                        } 
                    }
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            CreateDataAsync();
        }

    }
}
