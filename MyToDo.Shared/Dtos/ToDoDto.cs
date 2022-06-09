using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class ToDoDto: BaseDto
    {
        private string title;
        private string content;
        private bool status;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            } 
        }

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        public bool Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }
    }
}
