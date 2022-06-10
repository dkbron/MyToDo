using MyToDo.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel()
        {
            CreateTaskBar();
            CreateListBoxData();
        }

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
                ToDoDtos.Add(new ToDoDto() { Id = i, Title =$"代办事项{i}", Content=$"{i}事件.....", Status=true, CreateDate= DateTime.Now.Date  });
                MemoDtos.Add(new MemoDto() { Id = i, Title =$"备忘录{i}", Content=$"{i}事件.....", Status= true, CreateDate= DateTime.Now.Date  });
            }
        }
    }
}
