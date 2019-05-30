using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ct_ControlLogDAL:PublicDAL.PubClass<ct_ControlLog>
    {
        /// <summary>
        /// 获取后台登录日志的数据
        /// </summary>
        /// <returns></returns>
        public Object CTControlLogAll()
        {
            var a = from i in dbcontext.ct_ControlLog
                    join j in dbcontext.ct_Company on i.companyID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    select new
                    {
                        i.ID,
                        i.CU,
                        i.CT,
                        i.ctrl_name,
                        i.ctrl_bool,
                        x.CompanyName,
                        i.userName
                    };
            return a;

        }

        /// <summary>
        /// 获取后台登录日志的数据
        /// </summary>
        /// <returns></returns>
        public Object CTControlLogByID(int id)
        {
            var a = from i in dbcontext.ct_ControlLog
                    join j in dbcontext.ct_Company on i.companyID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    where i.ID == id
                    select new
                    {
                        i.ID,
                        i.CU,
                        i.CT,
                        i.ctrl_name,
                        i.ctrl_bool,
                        x.CompanyName,
                        i.userName
                    };
            return a;

        }
    }
}
