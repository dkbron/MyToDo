using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class MenuBar
    {
        private string icon;

        public string Icon { get => icon; set => icon = value; }

        private string title;

        public string Title { get => title; set => title = value; }
         
        private string nameSpace;
        public string NameSpace { get => nameSpace; set => nameSpace = value; }

        public void Navigate(string nameSpace)
        {

        }
    }
}
