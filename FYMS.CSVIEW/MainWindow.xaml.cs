using FYMS.CSVIEW.Domain;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FYMS.CSVIEW
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TabItemClose item = new TabItemClose();
            
            item.Header = string.Format("首页");
            item.Margin = new Thickness(0, 0, 1, 0);
            item.Height = 28;
            
            Index index = new Index();
            item.Content = index;
            MainTab.Items.Add(item);
        }

        /// <summary>
        /// 登录界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Max_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Sample4Dialog
            {
                Message = { Text = "确定关闭程序吗？" }
            };

            dialog.SureClick += closewindow;

            DialogHost.Show(dialog);
        }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        public void closewindow()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Close(object sender, RoutedEventArgs e)
        {
            
        }



        /// <summary>
        /// 初始大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ReSize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// 菜单隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// 菜单隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuToggleButton1_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 菜单隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuToggleButton1_Click(object sender, RoutedEventArgs e)
        {
            if (docvisible.Visibility == Visibility.Collapsed)
            {
                docvisible.Visibility = Visibility.Visible;
                MenuToggleButton.IsChecked = true;
                MenuToggleButton1.IsChecked = true;
                return;
            }
            if (docvisible.Visibility == Visibility.Visible)
            {
                docvisible.Visibility = Visibility.Collapsed;
                MenuToggleButton.IsChecked = false;
                MenuToggleButton1.IsChecked = false;
                return;
            }
        }

        /// <summary>
        /// 菜单隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (docvisible.Visibility == Visibility.Collapsed)
            {
                docvisible.Visibility = Visibility.Visible;
                MenuToggleButton.IsChecked = true;
                MenuToggleButton1.IsChecked = true;
                return;
            }
            if (docvisible.Visibility == Visibility.Visible)
            {
                docvisible.Visibility = Visibility.Collapsed;
                MenuToggleButton.IsChecked = false;
                MenuToggleButton1.IsChecked = false;
                return;
            }
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TabItemClose item = new TabItemClose();
            item.Header = string.Format("Header{0}", MainTab.Items.Count);
            item.Margin = new Thickness(0, 0, 1, 0);
            item.Height = 28;

            Index index = new Index();
            item.Content = index;
            MainTab.Items.Add(item);
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ReLogin_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Sample4Dialog
            {
                Message = { Text = "确定重新登录吗？" }
            };

            dialog.SureClick += relogin;

            DialogHost.Show(dialog);
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        public void relogin()
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }


}
