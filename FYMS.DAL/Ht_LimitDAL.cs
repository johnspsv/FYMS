using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class Ht_LimitDAL : PublicDAL.PubClass<Ht_Limit>
    {
        /// <summary>
        /// 主从关系保存
        /// </summary>
        /// <param name="ht_Limit"></param>
        /// <param name="ht_LimitDetails"></param>
        /// <param name="ht_Menulist"></param>
        /// <returns></returns>
        public bool LimitSave(Ht_Limit ht_Limit, List<Ht_LimitDetails> ht_LimitDetails, List<Ht_Menu> ht_Menulist,ht_control_log log)
        {

            dbcontext.Ht_Limit.Add(ht_Limit);
            foreach (var i in ht_LimitDetails)
            {
                i.Ht_Limit = ht_Limit;
                Ht_Menu ht_Menu = ht_Menulist.Where(x => x.ID == i.MenuID).FirstOrDefault();
                i.Ht_Menu = ht_Menu;
                dbcontext.Ht_LimitDetails.Add(i);
            }
            dbcontext.ht_control_log.Add(log);
            if (dbcontext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回菜单表所有对象
        /// </summary>
        /// <returns></returns>
        public List<Ht_Menu> MenuALL()
        {
            return dbcontext.Ht_Menu.ToList();
        }

        /// <summary>
        /// 返回所有角色
        /// </summary>
        /// <returns></returns>
        public List<ht_admin_userrole> RoleALL()
        {
            return dbcontext.ht_admin_userrole.ToList();
        }

        /// <summary>
        /// 根据权限ID 返回所有明细
        /// </summary>
        /// <param name="limitid"></param>
        /// <returns></returns>
        public List<Ht_LimitDetails> DetailsALL(int limitid)
        {
            return dbcontext.Ht_LimitDetails.Where(x => x.LimitID == limitid).ToList();
        }

        /// <summary>
        /// 查询需要更新的list
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<Ht_LimitDetails> DetailsUpdate(Func<Ht_LimitDetails, bool> func)
        {
            return dbcontext.Ht_LimitDetails.Where(func).ToList();
        }


        /// <summary>
        /// 更新主从表
        /// </summary>
        /// <param name="ht_Limit"></param>
        /// <param name="addlist"></param>
        /// <param name="deletelist"></param>
        public void UpdateList(Ht_Limit ht_Limit, List<Ht_LimitDetails> addlist, List<Ht_LimitDetails> deletelist, List<Ht_LimitDetails> updatelist,ht_control_log log)
        {
            dbcontext.Set<Ht_Limit>().Attach(ht_Limit);
            dbcontext.Entry<Ht_Limit>(ht_Limit).State = System.Data.Entity.EntityState.Modified;
            foreach (var i in updatelist)
            {
                dbcontext.Entry<Ht_LimitDetails>(i).State = System.Data.Entity.EntityState.Modified;
            }
            foreach (var i in addlist)
            {
                dbcontext.Ht_LimitDetails.Add(i);
            }
            foreach (var i in deletelist)
            {
                dbcontext.Entry<Ht_LimitDetails>(i).State = System.Data.Entity.EntityState.Deleted;
            }
            dbcontext.ht_control_log.Add(log);
            dbcontext.SaveChanges();
        }
    }
}
