using Prism.DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDo.Common.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            InitColorZoneNavigateBtn();

            ListBoxMenuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
        }
         

        private void InitColorZoneNavigateBtn()
        {

            btnMax.Click += (sender, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    btnMax.Content = "❐";
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    btnMax.Content = "☐";
                }
            };

            btnMin.Click += (sender, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };

            btnClose.Click += (sender, e) =>
            {
                Close();
            };

            colorZoneNavigate.MouseDown += (sender, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };

            colorZoneNavigate.MouseDoubleClick += (sender, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    btnMax.Content = "❐";
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    btnMax.Content = "☐";
                }
            };
        }

        private void testbtn_Click(object sender, RoutedEventArgs e)
        {
             Background = new SolidColorBrush(Colors.White);
        }
    }
}
