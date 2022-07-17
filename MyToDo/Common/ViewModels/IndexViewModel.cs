using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialog;
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;

        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        public IndexViewModel(IDialogHostService dialog, IContainerProvider container, IRegionManager regionManager, IEventAggregator aggregator) : base(container)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            CompletedToDoCommand = new DelegateCommand<ToDoDto>(ToDoCompleted); 
            UpdateToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            UpdateMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            NevigateToCommand = new DelegateCommand<Models.TaskBar>(NevigateTo);

            this.dialog = dialog;
            this.container = container;
            this.regionManager = regionManager;
            this.aggregator = aggregator;
            toDoService = container.Resolve<IToDoService>();
            memoService = container.Resolve<IMemoService>();

            

            CreateDataAsync();
        }

        private void NevigateTo(Models.TaskBar obj)
        {
            if(string.IsNullOrEmpty(obj.Target)) 
                return;   

            NavigationParameters param = new NavigationParameters();
            if (obj.Title == "已完成")
                param.Add("Value",2);
            else
                param.Add("Value",0); 
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target,param);
        }

        private void Execute(string obj)
        {
            if (String.IsNullOrWhiteSpace(obj))
                return;
            switch (obj)
            {
                case "添加待办":
                    CurrentToDoDto = new ToDoDto();
                    AddToDo(null);
                    break;
                case "添加备忘":
                    CurrentMemoDto = new MemoDto();
                    AddMemo(null);
                    break; 
            }
        }

        private ToDoDto currentToDoDto;

        public ToDoDto CurrentToDoDto
        {
            get { return currentToDoDto; }
            set { currentToDoDto = value; RaisePropertyChanged(); }
        }


        private MemoDto currentMemoDto;

        public MemoDto CurrentMemoDto
        {
            get { return currentMemoDto; }
            set { currentMemoDto = value; RaisePropertyChanged(); }
        }

        private async void AddToDo(ToDoDto model)
        {
            try
            { 
                DialogParameters param = new DialogParameters();

                if (model != null)
                    param.Add("Model", model);
                else
                    param.Add("Model", CurrentToDoDto);
                var dialogResult = await dialog.ShowDialog("AddToDoView", param);

                if (dialogResult.Result == ButtonResult.No)
                    return;

                var toDo = dialogResult.Parameters.GetValue<ToDoDto>("Model");

                if (toDo == null)
                    return;

                if (toDo.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(toDo);
                    if (updateResult.Status)
                    {
                        var toDoDto = SummaryDtos.ToDoDtos.FirstOrDefault(t => t.Id == toDo.Id);
                        if (toDoDto != null)
                        {
                            int index = SummaryDtos.ToDoDtos.IndexOf(toDoDto);

                            SummaryDtos.ToDoDtos.Remove(toDoDto);
                            SummaryDtos.ToDoDtos.Insert(index, toDoDto);
                        }
                        aggregator.SendMessage("已完成");
                    }
                }
                else
                {
                    var addResult = await toDoService.AddAsync(toDo);
                    if (addResult.Status)
                        SummaryDtos.ToDoDtos.Add(addResult.Result);
                }

                await UpdateTaskBar();  
                aggregator.SendMessage("更新待办事项成功");
            }
            finally
            { 
            }
           
        }

        private async void AddMemo(MemoDto model)
        {
            try
            { 
                DialogParameters param = new DialogParameters();
                if (model != null)
                    param.Add("Model", model);
                else
                    param.Add("Model", CurrentMemoDto);
                var dialogResult = await dialog.ShowDialog("AddMemoView", param);

                if (dialogResult.Result == ButtonResult.No)
                    return;

                var memo = dialogResult.Parameters.GetValue<MemoDto>("Model");

                if (memo == null)
                    return;

                if (memo.Id > 0)
                {
                    var updateResult = await memoService.UpdateAsync(memo);
                    if (updateResult.Status)
                    {
                        var memoDto = SummaryDtos.MemoDtos.FirstOrDefault(t => t.Id == memo.Id);
                        if (memoDto != null)
                        {
                            int index = SummaryDtos.MemoDtos.IndexOf(memoDto);

                            SummaryDtos.MemoDtos.Remove(memoDto);
                            SummaryDtos.MemoDtos.Insert(index, memoDto);
                        }
                    }
                }
                else
                {
                    var addResult = await memoService.AddAsync(memo);
                    if (addResult.Status)
                        SummaryDtos.MemoDtos.Add(addResult.Result);
                }

                await UpdateTaskBar();  
                aggregator.SendMessage("更新备忘录成功");
            }
            finally
            { 
            }
        } 

        #region 属性
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> CompletedToDoCommand { get; private set; } 

        public DelegateCommand<ToDoDto> UpdateToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> UpdateMemoCommand { get; private set; }

        public DelegateCommand<Models.TaskBar> NevigateToCommand { get; private set; }

        private ObservableCollection<Models.TaskBar> taskBars;
        public ObservableCollection<Models.TaskBar> TaskBars
        {
            get
            {
                return taskBars;
            }
            set
            {
                taskBars = value;
                RaisePropertyChanged();
            }
        }

        private SummaryDto summaryDtos;

        public SummaryDto SummaryDtos
        {
            get { return summaryDtos; }
            set { summaryDtos = value; RaisePropertyChanged(); }
        }


        #endregion

        private async void CreateDataAsync()
        {
            UpdateLoading(true);
             
            await UpdateTaskBar();

            var result = await memoService.GetAllAsync(
                new QueryParameter
                {
                    PageIndex = 0,
                    PageSize = 100, 
                }); 

            if (result.Status)
            {
                SummaryDtos.MemoDtos.Clear();
                foreach (var item in result.Result.Items)
                {
                    SummaryDtos.MemoDtos.Add(item);
                }
            }

            var toDoResult = await toDoService.GetAllFilterAsync(
                new ToDoParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    StatusIndex = 0
                });

            if (toDoResult.Status)
            {
                SummaryDtos.ToDoDtos.Clear();
                foreach (var toDoItem in toDoResult.Result.Items)
                {
                    SummaryDtos.ToDoDtos.Add(toDoItem);
                }
            }

            UpdateLoading(false);
        }

        private async void ToDoCompleted(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(obj);
                if (updateResult.Status)
                {
                    var todo = summaryDtos.ToDoDtos.FirstOrDefault(x => x.Id == obj.Id);
                    if (todo != null)
                    {
                        todo.Status = 1;
                        summaryDtos.ToDoDtos.Remove(obj);
                    }

                    aggregator.SendMessage("已完成");
                }
                await UpdateTaskBar();
            }
            finally
            { 
                UpdateLoading(false);
            }
        } 


        public async Task<bool> UpdateTaskBar()
        {
            var summary = await toDoService.GetSummaryAsync();
            if (summary.Status)
            {
                SummaryDtos = summary.Result; 
                TaskBars = new ObservableCollection<Models.TaskBar>();
                TaskBars.Add(new Models.TaskBar() { Icon = "ClockFast", Title = "汇总", Content = SummaryDtos.Summary.ToString(), Color = "#4294F7", Target = "ToDoView" });
                TaskBars.Add(new Models.TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = SummaryDtos.CompletedCount.ToString(), Color = "#51AE4A", Target = "ToDoView" }); ;
                TaskBars.Add(new Models.TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = SummaryDtos.CompletedRatio, Color = "#50B0DA", Target = "" });
                TaskBars.Add(new Models.TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = SummaryDtos.MemoCount.ToString(), Color = "#F2A43A", Target = "MemoView" });
            }

            if(SummaryDtos!=null)
                return true; 
            return false;

        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateTaskBar();
            base.OnNavigatedTo(navigationContext);
        }
    }
}
