using MyToDo.Common.Models;
using MyToDo.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
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
        public IndexViewModel(IDialogHostService dialog, IContainerProvider container):base(container)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);

            CreateTaskBar(); 
            this.dialog = dialog;
            this.container = container;

            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            toDoService = container.Resolve<IToDoService>();
            memoService = container.Resolve<IMemoService>();
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

            if(CurrentToDoDto.Id > 0)
            {
                //var updateResult = await toDoService.UpdateAsync(toDo);
                //var toDo = ToDoDtos.FirstOrDefault(t => t.Id == CurrentToDoDto.Id);
                //if (toDo != null)
                //{
                //    int index = ToDoDtos.IndexOf(toDo);
                     

                //    //上方数据改变后前端数据没有更新，是深浅拷贝的问题，先移除数据后在插入数据可正常更新
                //    ToDoDtos.Remove(toDo);
                //    ToDoDtos.Insert(index, toDo);
                //}
            }
            else
            {
                var addResult = await toDoService.AddAsync(toDo);
                if (addResult.Status)
                    ToDoDtos.Add(addResult.Result);
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
                if(updateResult.Status)
                {
                    var meMo = MemoDtos.FirstOrDefault(t => t.Id == CurrentMemoDto.Id);
                    if (meMo != null)
                    {
                        int index = MemoDtos.IndexOf(meMo); 

                        MemoDtos.Remove(meMo);
                        MemoDtos.Insert(index, meMo);
                    }
                }
            }
            else
            {
                var addResult = await memoService.AddAsync(memo);
                if (addResult.Status)
                    MemoDtos.Add(addResult.Result);
            }
        }

        #region 属性
        public DelegateCommand<string> ExecuteCommand{ get; private set; }

        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars 
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

        private ObservableCollection<MemoDto> memoDtos;


        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set
            { 
                memoDtos = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void CreateTaskBar()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color="#4294F7", Target="" }); 
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "8", Color= "#51AE4A", Target="" }); 
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "89%", Color= "#50B0DA", Target="" }); 
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "4", Color= "#F2A43A", Target="" });  
        }

        public void CreateListBoxData()
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();

            for (int i = 1; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Id = i, Title =$"代办事项{i}", Content=$"{i}事件.....", Status=1, CreateDate= DateTime.Now.Date  });
                MemoDtos.Add(new MemoDto() { Id = i, Title =$"备忘录{i}", Content=$"{i}事件.....", Status= 1, CreateDate= DateTime.Now.Date  });
            }
        }
    }
}
