using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ht_CheckRoleRelDAL:PublicDAL.PubClass<ht_CheckRoleRel>
    {
        /// <summary>
        /// 获取审核关系数据
        /// </summary>
        /// <returns></returns>
        public Object CheckRoleRelAll()
        {
            var a = from i in dbcontext.ht_CheckRoleRel
                    join j in dbcontext.ht_CheckType on i.CheckID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    join y in dbcontext.ht_admin_userrole on i.RoleID equals y.ID into d
                    from z in d.DefaultIfEmpty()
                    select new
                    {
                        ID = i.ID,
                        ST = i.ST,
                        CheckID = i.CheckID,
                        RoleID = i.RoleID,
                        CheckName = x.CheckName,
                        RoleName = z.role_name
                    };
            return a;
           
        }

        /// <summary>
        /// 通过角色ID获取数据
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Object CheckRoleRelByRoleID(int roleid)
        {
            var a = from i in dbcontext.ht_CheckRoleRel
                    join j in dbcontext.ht_CheckType on i.CheckID equals j.ID into b
                    from x in b.DefaultIfEmpty()
                    join y in dbcontext.ht_admin_userrole on i.RoleID equals y.ID into d
                    from z in d.DefaultIfEmpty()
                    where i.ST > 0 && i.RoleID == roleid
                    select new
                    {
                        ID = i.ID,
                        ST = i.ST,
                        CheckID = i.CheckID,
                        RoleID = i.RoleID,
                        CheckName = x.CheckName,
                        RoleName = z.role_name
                    };
            return a;
        }

        /// <summary>
        /// 审核关系更新
        /// </summary>
        /// <param name="addlist"></param>
        /// <param name="deletelist"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool CheckRoleRelUpdate(List<ht_CheckRoleRel> addlist,List<ht_CheckRoleRel> deletelist,ht_control_log log)
        {
            foreach(var x in addlist)
            {
                dbcontext.Entry<ht_CheckRoleRel>(x).State = System.Data.Entity.EntityState.Added;
            }
            foreach(var x in deletelist)
            {
                dbcontext.Entry<ht_CheckRoleRel>(x).State = System.Data.Entity.EntityState.Deleted;
            }
            if(dbcontext.SaveChanges()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
