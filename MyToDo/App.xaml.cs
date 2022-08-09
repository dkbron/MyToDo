using DryIoc;
using MyToDo.Common;
using MyToDo.Common.ViewModels;
using MyToDo.Common.ViewModels.Dialogs;
using MyToDo.Common.Views;
using MyToDo.Common.Views.Dialogs;
using MyToDo.Service;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
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

        public static void Logout(IContainer container)
        {
            var dialog = container.Resolve<IDialogService>();

            Current.MainWindow.Hide();
            dialog.ShowDialog("LoginView", callback: callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }
                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                    service.Configure();
                Current.MainWindow.Show();
            });
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure(); 
            base.OnInitialized();
            App.Current.MainWindow.Hide();
            dialog.ShowDialog("LoginView", callback: callback =>
            {

                if (callback.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }
                App.Current.MainWindow.Show();
            });

            //var service = App.Current.MainWindow.DataContext as IConfigureService;
            //if (service != null)
            //    service.Configure();


            //base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().RegisterInstance(@"http://localhost:3344/", serviceKey: "webUrl");

            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();   
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<IUserService, UserService>();

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

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
