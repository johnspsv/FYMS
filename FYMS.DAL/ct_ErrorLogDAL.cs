using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ct_ErrorLogDAL:PublicDAL.PubClass<ct_ErrorLog>
    {
        /// <summary>
        /// 获取前台所有错误日志
        /// </summary>
        /// <returns></returns>
        public Object CTErrorLogAll()
        {
            var a = from i in dbcontext.ct_ErrorLog
                    join j in dbcontext.ct_Company on i.companyID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    select new
                    {
                        i.ID,
                        i.CT,
                        i.error_name,
                        i.error_catch,
                        i.error_userid,
                        i.companyID,
                        x.CompanyName
                    };
            return a;
        }

        /// <summary>
        /// 获取前台所有错误日志
        /// </summary>
        /// <returns></returns>
        public Object CTErrorLogByID(int id)
        {
            var a = from i in dbcontext.ct_ErrorLog
                    join j in dbcontext.ct_Company on i.companyID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    where i.ID==id
                    select new
                    {
                        i.ID,
                        i.CT,
                        i.error_name,
                        i.error_catch,
                        i.error_userid,
                        i.companyID,
                        x.CompanyName
                    };
            return a;
        }
    }
}
