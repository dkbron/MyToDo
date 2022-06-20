using DryIoc;
using MyToDo.Common;
using MyToDo.Common.ViewModels;
using MyToDo.Common.ViewModels.Dialogs;
using MyToDo.Common.Views;
using MyToDo.Common.Views.Dialogs;
using MyToDo.Service;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if(service!=null)
                service.Configure();

            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"https://localhost:44356/", serviceKey: "webUrl");

            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();   
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<SystemConfigView, SystemConfigViewModel>();
            containerRegistry.RegisterForNavigation<AboutMoreView, AboutMoreViewModel>();
        }
    }
}
