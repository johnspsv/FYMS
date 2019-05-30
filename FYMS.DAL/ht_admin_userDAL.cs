
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ht_admin_userDAL:PublicDAL.PubClass<ht_admin_user>
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ObjectResult<proc_Login_Result> Login(string username,string password)
        {
            ObjectResult<proc_Login_Result> queryable = dbcontext.proc_Login(username, password);
            return queryable;
        }
    }
}
