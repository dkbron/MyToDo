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
            AddMemoCommand = new DelegateCommand(CreateDataAsync); 
            MemoDtos = new ObservableCollection<MemoDto>(); 
            this.service = service; 
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


        public DelegateCommand AddMemoCommand { get; private set; }

        public void AddMemo()
        {
            IsRightDrawerOpen = true;
        }

        public async void CreateDataAsync()
        {
            UpdateLoading(true);
            var result = await service.GetAllAsync(new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
            );

            if(result.Status)
            {
                memoDtos.Clear();
                foreach(var item in result.Result.Items)
                {
                    MemoDtos.Add(item);
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
