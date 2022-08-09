using DryIoc;
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
    class MainViewModel : BindableBase, IConfigureService
    {

        public MainViewModel(IRegionManager regionManager, IContainer container)
        { 
            this.regionManager = regionManager;
            this.container = container;
            LogoutCommand = new DelegateCommand(() =>
            {
                App.Logout(container);
            });
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);

            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });

            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }


        private void Navigate(MenuBar obj)
        {
            if (obj == null)
                return;
              

           MenuBar munubar = (MenuBar)obj;
            if (string.IsNullOrEmpty(munubar.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(munubar.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }
        
        public DelegateCommand LogoutCommand { get; private set; }


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
        private readonly IContainer container;

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

        public void Configure() 
        {
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");

            UserName = AppSession.UserName;
        } 
    }
}
