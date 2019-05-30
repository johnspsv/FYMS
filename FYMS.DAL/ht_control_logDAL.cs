using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ht_control_logDAL:PublicDAL.PubClass<ht_control_log>
    {
        /// <summary>
        /// 获取后台登录日志的数据
        /// </summary>
        /// <returns></returns>
        public Object ControlLogAll()
        {
            var a = from i in dbcontext.ht_control_log
                    join j in dbcontext.ht_admin_user on i.CU equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    select new
                    {
                        i.ID,
                        i.CU,
                        i.CT,
                        i.ctrl_name,
                        i.ctrl_bool,
                        x.user_name,
                        x.user_phone                      
                    };
            return a;
                  
        }

        /// <summary>
        /// 获取后台登录日志的数据
        /// </summary>
        /// <returns></returns>
        public Object ControlLogByID(int id)
        {
            var a = from i in dbcontext.ht_control_log
                    join j in dbcontext.ht_admin_user on i.CU equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    where i.ID==id
                    select new
                    {
                        i.ID,
                        i.CU,
                        i.CT,
                        i.ctrl_name,
                        i.ctrl_bool,
                        x.user_name,
                        x.user_phone
                    };
            return a;

        }

    }
}
