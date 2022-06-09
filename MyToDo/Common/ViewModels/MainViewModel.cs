using MaterialDesignThemes.Wpf;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
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
    class MainViewModel : BindableBase
    {
        public MainViewModel(IRegionManager regionManager)
        {
            CreateMenuBar();
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);

            GoForwardCommand = new DelegateCommand(() =>
            { 
                if(journal!=null && journal.CanGoForward)
                    journal.GoForward(); 
            });

            GoBackCommand = new DelegateCommand(() =>
            {
                if(journal!=null && journal.CanGoBack)
                    journal.GoBack(); 
            }); 
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        private ObservableCollection<MenuBar> menubars;

        public ObservableCollection<MenuBar> Menubars { 
            get => menubars; 
            set
            {
                menubars = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; set; }
        
        public DelegateCommand GoForwardCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }

        public IRegionNavigationJournal journal;
        public IRegionManager regionManager { get; private set; }

        public void CreateMenuBar()
        {
            Menubars = new ObservableCollection<MenuBar>();
            Menubars.Add(new MenuBar() { Icon = "Home", NameSpace = "IndexView", Title = "首页" });
            Menubars.Add(new MenuBar() { Icon = "BookPlay", NameSpace = "ToDoView", Title = "待办事项" });
            Menubars.Add(new MenuBar() { Icon = "BookmarkMultiple", NameSpace = "MemoView", Title = "备忘录" });
            Menubars.Add(new MenuBar() { Icon = "CogOutline", NameSpace = "SettingView", Title = "设置" });
;        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }
    }
}
