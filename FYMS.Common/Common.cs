using FYMS.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Management;


namespace FYMS.Common
{
    public static class Common
    {


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="toCryString">要加密的字符串</param>
        /// <returns></returns>
        public static string GetMD5Str(string toCryString)
        {
            MD5CryptoServiceProvider hashmd5;
            hashmd5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(toCryString))).Replace("-", "").ToLower();//asp是小写,把所有字符变小写
        }



        /// <summary>
        /// 登录人ID
        /// </summary>
        public static int UserID { get; set; }

        /// <summary>
        /// 登录人名称
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public static int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public static string RoleName { get; set; }

        /// <summary>
        /// 获取导航栏
        /// </summary>
        public static List<HtMenu> menulist { get; set; }


        /// <summary>
        /// 判断当前登录用户是否有权限打开页面（防止用户复制链接进行页面的篡改）
        /// </summary>
        /// <param name="route">路由</param>
        /// <returns></returns>
        public static HtMenu CanRead(string route)
        {
            var a = menulist.Where(x => x.Path == route);
            if (a.Count()>0)
            {
                return a.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
