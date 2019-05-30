using FYMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public class LogBLL
    {
        static LogDAL dal = new LogDAL();

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="str"></param>
        public static void LoginAdmin(string [] str)
        {
            dal.loginlog(str);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(string ex)
        {
            dal.errorlog(ex);
        }

        /// <summary>
        /// 错误操作日志
        /// </summary>
        /// <param name=""></param>
        public static void errorControlLog(string controlstr, string functionname, string other, string ex)
        {
            dal.errorControlLog(controlstr, functionname, other, ex);
        }

        /// <summary>
        /// 操作记录日志
        /// </summary>
        /// <param name="str"></param>
        /// <param name="functionname"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ht_control_log controllog(string str, string functionname, string other)
        {
            return dal.log(str, functionname, other);
        }

        public static void inlog(string str, string functionname, string other)
        {
            dal.inlog(str, functionname, other);
        }
    }
}
