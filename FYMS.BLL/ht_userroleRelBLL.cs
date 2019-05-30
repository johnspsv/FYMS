using FYMS.Common.ViewModel;
using FYMS.DAL;
using FYMS.DAL.PublicDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class ht_userroleRelBLL
    {
        static ht_userroleRelDAL dal = new ht_userroleRelDAL();

        /// <summary>
        /// 查询所有数据left join
        /// </summary>
        /// <returns></returns>
        public static string All()
        {
            PubClass<ht_userroleRel> reldal = new PubClass<ht_userroleRel>();
            PubClass<ht_admin_user> userdal = new PubClass<ht_admin_user>();
            PubClass<ht_admin_userrole> userroledal = new PubClass<ht_admin_userrole>();
            IList<ht_userroleRel> listrel = reldal.SelectEntities(x => x.ST < 2);
            IList<ht_admin_user> listuser = userdal.SelectEntities(x => x.ST == 1);
            IList<ht_admin_userrole> listuserrole = userroledal.SelectEntities(x => x.ST == 1);
            var data = from rel in listrel
                       join user in listuser on rel.admin_id equals user.ID into d
                       from dd in d.DefaultIfEmpty()
                       join userrole in listuserrole on rel.role_id equals userrole.ID into e
                       from ee in e.DefaultIfEmpty()
                       select new AdminRoleRel
                       {
                           ID = rel.ID,
                           ST = rel.ST,
                           admin_id = rel.admin_id,
                           admin_name = rel.admin_name,
                           role_id = rel.role_id,
                           role_name = rel.role_name
                       };
            return JsonConvert.SerializeObject(data).ToString();
        }

        /// <summary>
        /// 新增人员关系
        /// </summary>
        /// <param name="str"></param>
        public static string UserRoleRelAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<ht_userroleRel> bll = new PublicBLL.PubBll<ht_userroleRel>();
                ht_userroleRel entity = bll.ReturnEntity(str);


                entity.CT = DateTime.Now;
                if (dal.SelectEntities(x => x.admin_id == entity.admin_id).Count > 0)
                {
                    return "该人员已经被赋予角色";
                }
                else
                {
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity,LogBLL.controllog("add", "新增人员关系_UserRoleRelAdd",str)))
                    {
                        return "保存成功";
                    }
                    else
                    {
                        return "保存失败";
                    }
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("add", "新增人员关系_UserRoleRelAdd", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 更新人员关系
        /// </summary>
        /// <param name="str"></param>
        public static string UserRoleRelUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<AdminRoleRel> bll = new PublicBLL.PubBll<AdminRoleRel>();
                AdminRoleRel entity = bll.ReturnEntity(str);
                ht_userroleRel model = dal.SelectEntities(x => x.ID == entity.ID).FirstOrDefault();
                PublicBLL.PubBll<ht_userroleRel> bll1 = new PublicBLL.PubBll<ht_userroleRel>();
                model = bll1.Edit(entity, model);
                model.LU = Common.Common.UserID;
                model.LT = DateTime.Now;
                if (dal.Update(model, LogBLL.controllog("modify", "更新人员关系_UserRoleRelUpdate", str)))
                {
                    return "保存成功";
                }
                else
                {
                    return "保存失败";
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("modify", "更新人员关系_UserRoleRelUpdate", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 根据ID获取人员角色关系对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ModelByID(int id)
        {
            ht_userroleRel bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
            string str = JsonConvert.SerializeObject(bean);
            return str;
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <returns></returns>
        public static string RoleRelDelete(int id)
        {
            try
            {
                ht_userroleRel bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
                if (bean.ST == 0)
                {
                    bean.LU = Common.Common.UserID;
                    bean.LT = DateTime.Now;
                    bean.ST = 1;
                }
                else
                {
                    bean.LU = Common.Common.UserID;
                    bean.LT = DateTime.Now;
                    bean.ST = 0;
                }
                dal.UpdateEntity(bean,LogBLL.controllog("delete", "人员关系禁用开启_RoleRelDelete",id.ToString()));
                return "成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "人员关系禁用开启_RoleRelDelete", id.ToString(), ex.ToString());
                return "失败";
            }
        }
    }
}
