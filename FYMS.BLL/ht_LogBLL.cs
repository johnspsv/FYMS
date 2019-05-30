using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class ht_LogBLL
    {
        static ht_error_logDAL errorLogDAL = new ht_error_logDAL();

        static ht_admin_logDAL adminlogDAL = new ht_admin_logDAL();

        static ht_control_logDAL controllogDAL = new ht_control_logDAL();

        static ct_ErrorLogDAL cterrorLogDAL = new ct_ErrorLogDAL();

        static ct_LoginLogDAL ctloginLogDAL = new ct_LoginLogDAL();

        static ct_ControlLogDAL ctcontrolLogDAL = new ct_ControlLogDAL();
        /// <summary>
        /// 获取错误日志数据
        /// </summary>
        /// <returns></returns>
        public static string ErrorLogAll()
        {
            var a = errorLogDAL.SelectEntities(x => x.ST > 0);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取错误日志明细数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string ErrorLogByID(int ID)
        {
            var a = errorLogDAL.SelectEntities(x => x.ID == ID);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取后台登录日志数据
        /// </summary>
        /// <returns></returns>
        public static string AdminLogAll()
        {
            var a = adminlogDAL.SelectEntities(x => x.ST > 0);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取后台操作日志数据
        /// </summary>
        /// <returns></returns>
        public static string ControlLogAll()
        {
            var a = controllogDAL.ControlLogAll();
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取操作日志明细数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string ControlLogByID(int ID)
        {
            var a = controllogDAL.ControlLogByID(ID);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取前台所有错误日志
        /// </summary>
        /// <returns></returns>
        public static string CTErrorLogAll()
        {
            var a = cterrorLogDAL.CTErrorLogAll();
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取前台登录日志数据
        /// </summary>
        /// <returns></returns>
        public static string CTLoginLogAll()
        {
            var a = ctloginLogDAL.SelectEntities(x => x.ST > 0);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取前台错误日志明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string CTErrorLogByID(int ID)
        {
            var a = cterrorLogDAL.CTErrorLogByID(ID);
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取前台操作日志所有数据
        /// </summary>
        /// <returns></returns>
        public static string CTControlLogAll()
        {
            var a = ctcontrolLogDAL.CTControlLogAll();
            return JsonConvert.SerializeObject(a);
        }

        /// <summary>
        /// 获取前台操作日志所有数据
        /// </summary>
        /// <returns></returns>
        public static string CTControlLogByID(int ID)
        {
            var a = ctcontrolLogDAL.CTControlLogByID(ID);
            return JsonConvert.SerializeObject(a);
        }
    }
}
