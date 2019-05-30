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
using System.Windows.Shapes;

namespace FYMS.CSVIEW
{
    /// <summary>
    /// Regsiter.xaml 的交互逻辑
    /// </summary>
    public partial class Regsiter : Window
    {
        public Regsiter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnreturn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Sample4Dialog
            {
                Message = { Text = "放弃注册，返回登录吗？" }
            };

            dialog.SureClick += Returnlogin;

            DialogHost.Show(dialog);
        }

        /// <summary>
        /// 返回
        /// </summary>
        public void Returnlogin()
        {
            Login login = new Login();
            login.Show();
            this.Close();

        }

        /// <summary>
        /// 拖拽
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

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            string a = CTControllog.CompanyName;
        }
    }
}
