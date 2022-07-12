using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class SummaryDto : BaseDto
    {
        private int summary;

        public int Summary
        {
            get { return summary; }
            set { summary = value; OnPropertyChanged(); }
        }

        private int completedCount;

        public int CompletedCount
        {
            get { return completedCount; }
            set { completedCount = value; OnPropertyChanged(); }
        }

        private string completedRatio;

        public string CompletedRatio
        {
            get { return completedRatio; }
            set { completedRatio = value; OnPropertyChanged(); }
        }

        private int memoCount;

        public int MemoCount
        {
            get { return memoCount; }
            set { memoCount = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; OnPropertyChanged(); }
        }


    }
}
