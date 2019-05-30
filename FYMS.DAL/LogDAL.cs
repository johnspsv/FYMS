using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class LogDAL
    {
        protected FYMSEntities dbcontext = new FYMSEntities();
        #region 日志
        /// <summary>
        /// 返回日志操作对象
        /// </summary>
        /// <param name="str"></param>
        /// <param name="functionname"></param>
        /// <returns></returns>
        public virtual ht_control_log log(string str, string functionname, string other)
        {
            ht_control_log log = new ht_control_log();
            log.LT = DateTime.Now;
            log.LU = Common.Common.UserID;
            log.CT = DateTime.Now;
            log.CU = Common.Common.UserID;
            log.ST = 1;
            log.ctrl_bool = 1;
            switch (str)
            {
                case "add":
                    log.ctrl_name = "add" + "|" + functionname +"|"+ other;
                    break;
                case "delete":
                    log.ctrl_name = "delete" + "|" + functionname + "|" + other;
                    break;
                case "modify":
                    log.ctrl_name = "modify" + "|" + functionname + "|" + other;
                    break;
                default:
                    log.ctrl_name = str + "|" + functionname + "|" + other;
                    break;
            }

            return log;
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="str"></param>
        /// <param name="functionname"></param>
        /// <param name="other"></param>
        public virtual void inlog(string str, string functionname, string other)
        {
            ht_control_log log = new ht_control_log();
            log.LT = DateTime.Now;
            log.LU = Common.Common.UserID;
            log.CT = DateTime.Now;
            log.CU = Common.Common.UserID;
            log.ST = 1;
            log.ctrl_bool = 1;
            switch (str)
            {
                case "add":
                    log.ctrl_name = "add" + "|" + functionname + "|" + other;
                    break;
                case "delete":
                    log.ctrl_name = "delete" + "|" + functionname + "|" + other;
                    break;
                case "modify":
                    log.ctrl_name = "modify" + "|" + functionname + "|" + other;
                    break;
                default:
                    log.ctrl_name = str + "|" + functionname + "|" + other;
                    break;
            }

            dbcontext.ht_control_log.Add(log);
        }

        /// <summary>
        /// 错误操作日志
        /// </summary>
        /// <param name="controlstr"></param>
        /// <param name="ex"></param>
        public virtual void errorControlLog(string controlstr,string functionname,string other,string ex)
        {
            ht_control_log log = new ht_control_log();
            log.LT = DateTime.Now;
            log.LU = Common.Common.UserID;
            log.CT = DateTime.Now;
            log.CU = Common.Common.UserID;
            log.ST = 1;
            log.ctrl_bool = 0;
            switch (controlstr)
            {
                case "add":
                    log.ctrl_name = "add" + "|" + functionname + "|" + other;
                    break;
                case "delete":
                    log.ctrl_name = "delete" + "|" + functionname + "|" + other;
                    break;
                case "modify":
                    log.ctrl_name = "modify" + "|" + functionname + "|" + other;
                    break;
                default:
                    log.ctrl_name = controlstr + "|" + functionname + "|" + other;
                    break;
            }

            dbcontext.ht_control_log.Add(log);

            ht_error_log errorlog = new ht_error_log();
            errorlog.LT = DateTime.Now;
            errorlog.LU = Common.Common.UserID;
            errorlog.CU = Common.Common.UserID;
            errorlog.CT = DateTime.Now;
            errorlog.ST = 1;
            errorlog.error_name = Common.Common.UserName;
            errorlog.PID = Common.Common.UserID;
            errorlog.error_catch = ex;
            dbcontext.ht_error_log.Add(errorlog);
            dbcontext.SaveChanges();
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        public virtual void loginlog(string[] str)
        {
            ht_admin_log log = new ht_admin_log();
            log.LT = DateTime.Now;
            log.LU = Common.Common.UserID;
            log.CU = Common.Common.UserID;
            log.CT = DateTime.Now;
            log.ST = 1;
            log.log_date = DateTime.Now;
            log.log_ip = str[0];
            log.log_mac = str[1];
            log.log_name = Common.Common.UserName;
            dbcontext.ht_admin_log.Add(log);
            dbcontext.SaveChanges();
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex"></param>
        public virtual void errorlog(string ex)
        {
            ht_error_log log = new ht_error_log();
            log.LT = DateTime.Now;
            log.LU = Common.Common.UserID;
            log.CU = Common.Common.UserID;
            log.CT = DateTime.Now;
            log.ST = 1;
            log.error_name = Common.Common.UserName;
            log.PID = Common.Common.UserID;
            log.error_catch = ex;
            dbcontext.ht_error_log.Add(log);
            dbcontext.SaveChanges();
        }
        #endregion 
    }
}
