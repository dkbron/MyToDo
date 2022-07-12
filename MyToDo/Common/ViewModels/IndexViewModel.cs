using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
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

        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        public IndexViewModel(IDialogHostService dialog, IContainerProvider container) : base(container)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            CompletedCommand = new DelegateCommand<ToDoDto>(Completed);

            this.dialog = dialog;
            this.container = container;

            toDoService = container.Resolve<IToDoService>();
            memoService = container.Resolve<IMemoService>();

            UpdateTaskBar();
            CreateDataAsync();
        }

        private void Execute(string obj)
        {
            if (String.IsNullOrWhiteSpace(obj))
                return;
            switch (obj)
            {
                case "添加待办":
                    CurrentToDoDto = new ToDoDto();
                    AddToDo();
                    break;
                case "添加备忘":
                    CurrentMemoDto = new MemoDto();
                    AddMemo();
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

        private async void AddToDo()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Model", CurrentToDoDto);
            var dialogResult = await dialog.ShowDialog("AddToDoView", param);

            if (dialogResult.Result == ButtonResult.No)
                return;

            var toDo = dialogResult.Parameters.GetValue<ToDoDto>("Model");

            if (toDo == null)
                return;

            if (CurrentToDoDto.Id > 0)
            {
                var updateResult = await toDoService.UpdateAsync(toDo);
                if (updateResult.Status)
                {
                    var toDoDto = SummaryDtos.ToDoDtos.FirstOrDefault(t => t.Id == CurrentToDoDto.Id);
                    if (toDoDto != null)
                    {
                        int index = SummaryDtos.ToDoDtos.IndexOf(toDoDto);

                        SummaryDtos.ToDoDtos.Remove(toDoDto);
                        SummaryDtos.ToDoDtos.Insert(index, toDoDto);
                    }
                }
            }
            else
            {
                var addResult = await toDoService.AddAsync(toDo);
                if (addResult.Status)
                    SummaryDtos.ToDoDtos.Add(addResult.Result);
            }
        }

        private async void AddMemo()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Model", CurrentMemoDto);
            var dialogResult = await dialog.ShowDialog("AddMemoView", param);

            if (dialogResult.Result == ButtonResult.No)
                return;

            var memo = dialogResult.Parameters.GetValue<MemoDto>("Model");

            if (memo == null)
                return;

            if (CurrentMemoDto.Id > 0)
            {
                var updateResult = await memoService.UpdateAsync(memo);
                if (updateResult.Status)
                {
                    var memoDto = SummaryDtos.MemoDtos.FirstOrDefault(t => t.Id == CurrentMemoDto.Id);
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
        }

        #region 属性
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> CompletedCommand { get; private set; }

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
                    StatusIndex = null
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

        private async void Completed(ToDoDto obj)
        {
            var updateResult = await toDoService.UpdateAsync(obj);
            if (updateResult.Status)
            {
                var todo = summaryDtos.ToDoDtos.FirstOrDefault(x => x.Id == obj.Id);
                if (todo != null)
                    SummaryDtos.ToDoDtos.Remove(todo);
            }

        }


        public async void UpdateTaskBar()
        {
            var summary = await toDoService.GetSummaryAsync();
            if (summary.Status)
            {
                SummaryDtos = summary.Result;
                TaskBars = new ObservableCollection<Models.TaskBar>();
                TaskBars.Add(new Models.TaskBar() { Icon = "ClockFast", Title = "汇总", Content = SummaryDtos.Summary.ToString(), Color = "#4294F7", Target = "" });
                TaskBars.Add(new Models.TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = SummaryDtos.CompletedCount.ToString(), Color = "#51AE4A", Target = "" }); ;
                TaskBars.Add(new Models.TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = SummaryDtos.CompletedRatio, Color = "#50B0DA", Target = "" });
                TaskBars.Add(new Models.TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = SummaryDtos.MemoCount.ToString(), Color = "#F2A43A", Target = "" });
            }


        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateTaskBar();
            base.OnNavigatedTo(navigationContext);
        }
    }
}
