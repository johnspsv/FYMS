using FYMS.CSVIEW.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private string filepath;

        MainService.MainServiceClient client = new MainService.MainServiceClient();

        public Regsiter()
        {
            InitializeComponent();
            CTRegister.gender = 0;
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
            
            if(string.IsNullOrEmpty(CTRegister.CompanyName)||string.IsNullOrEmpty(CTRegister.CompanyNo)||string.IsNullOrEmpty(CTRegister.mobilephone)||string.IsNullOrEmpty(CTRegister.name)||string.IsNullOrEmpty(CTRegister.username)||string.IsNullOrEmpty(CTRegister.email))
            {
                SureDialog sureDialog = new SureDialog()
                {
                    Message = { Text = "请将资料填写完整" }
                };
                DialogHost.Show(sureDialog);
                return;
            }
            if(string.IsNullOrEmpty(Password.Password)||string.IsNullOrEmpty(ComfirmPassword.Password))
            {
                SureDialog sureDialog = new SureDialog()
                {
                    Message = { Text = "密码不可以为空" }
                };
                DialogHost.Show(sureDialog);
                return;
            }
            if(Password.Password!=ComfirmPassword.Password)
            {
                SureDialog sureDialog = new SureDialog()
                {
                    Message = { Text = "密码与确认密码不一致" }
                };
                DialogHost.Show(sureDialog);
                return;
            }
            CTRegister.password = Common.Common.GetMD5Str(Password.Password);
            CTRegister.CompanyPhoto= UploadPic(filepath);

            string str = JsonConvert.SerializeObject(CTRegister);
            
            if(client.ht_CheckTableSave(str))
            {
                Sample4Dialog sampleDialog = new Sample4Dialog()
                {
                    Message = { Text = "保存成功！" }
                };
                sampleDialog.SureClick += Returnlogin;
                DialogHost.Show(sampleDialog);           
            }
            else
            {
                SureDialog sureDialog = new SureDialog()
                {
                    Message = { Text = "保存失败！" }
                };
                DialogHost.Show(sureDialog);
                return;
            }
            
        }

        /// <summary>
        /// 性别男选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Male_Click(object sender, RoutedEventArgs e)
        {
            if (Male.IsChecked == true)
            {
                Famale.IsChecked = false;
                CTRegister.gender = 0;
            }
            else
            {
                Famale.IsChecked = true;
                CTRegister.gender = 1;
            }
        }

       
        private void Famale_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 性别女选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Famale_Click(object sender, RoutedEventArgs e)
        {
            if (Famale.IsChecked == true)
            {
                Male.IsChecked = false;
                CTRegister.gender = 1;
            }
            else
            {
                Male.IsChecked = true;
                CTRegister.gender = 0;
            }
        }

        /// <summary>
        /// 图片选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "jpg图片|*.jpg|png图片|*.png|jpeg图片|*.jpeg|bmp图片|*.bmp";
            if(file.ShowDialog()==true)
            {
                filepath = file.FileName;
                ShowImg.Source = new BitmapImage(new Uri(file.FileName));
                CompanyPhoto.Text = file.FileName;
            }
        }
        
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string UploadPic(string filepath)
        {
            string[] str=filepath.Split('\\');
            string filename = str[str.Length - 1];
            string[] filehz = filename.Split('.');
            string name = filehz[filehz.Length - 1];
            string picname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + name;
            string serverpath = "D://test"+"/"+picname;
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                webClient.UploadFile(new Uri(serverpath), "PUT", filepath);
                return picname;
            }
            catch(Exception ex)
            {
                return "0";
            }
        }

    }
}
