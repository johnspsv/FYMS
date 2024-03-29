﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYMS.DAL;

using FYMS.Common.ViewModel;
using Newtonsoft.Json;

namespace FYMS.BLL
{

    public static class ht_admin_userroleBLL
    {
        static ht_admin_userroleDAL dal = new ht_admin_userroleDAL();

        static PublicBLL.PubBll<ht_admin_userrole> pubbll = new PublicBLL.PubBll<ht_admin_userrole>();


        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static string All()
        {
            List<ht_admin_userrole> a = dal.SelectEntities(x => x.ST == 1).ToList();
            if (a != null)
            {
                return pubbll.ReturnStr(a);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增人员角色
        /// </summary>
        /// <param name="str"></param>
        public static string UserRoleAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<ht_admin_userrole> bll = new PublicBLL.PubBll<ht_admin_userrole>();
                ht_admin_userrole entity = bll.ReturnEntity(str);


                entity.CT = DateTime.Now;
                if (dal.SelectEntities(x => x.role_name == entity.role_name).Count > 0)
                {
                    return "角色名重复";
                }
                else if (dal.SelectEntities(x => x.role_code == entity.role_code).Count > 0)
                {
                    return "角色编号重复";
                }
                else
                {
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity, LogBLL.controllog("add", "新增人员角色_UserAdd", str)))
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
                LogBLL.errorControlLog("add", "新增人员角色_UserRoleAdd", str, ex.ToString());
                return "保存失败";
            }
        }


        /// <summary>
        /// 根据ID获取管理角色对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ModelByID(int id)
        {
            ht_admin_userrole bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
            string str = JsonConvert.SerializeObject(bean);
            return str;
        }

        /// <summary>
        /// 管理员角色更新
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserRoleUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<Admin_Role> bll = new PublicBLL.PubBll<Admin_Role>();
                Admin_Role user = bll.ReturnEntity(str);

                if (dal.SelectEntities(x => x.role_name == user.role_name && x.ID != user.ID).Count > 0)
                {
                    return "工号重复";
                }
                else if (dal.SelectEntities(x => x.role_code == user.role_code && x.ID != user.ID).Count > 0)
                {
                    return "手机号重复";
                }
                else
                {
                    ht_admin_userrole htuser = dal.SelectEntities(x => x.ID == user.ID).FirstOrDefault();
                    PublicBLL.PubBll<ht_admin_userrole> bll1 = new PublicBLL.PubBll<ht_admin_userrole>();
                    htuser = bll1.Edit<Admin_Role>(user, htuser);
                    htuser.LU = Common.Common.UserID;
                    htuser.LT = DateTime.Now;
                    if (dal.Update(htuser, LogBLL.controllog("modify", "人员角色更新_UserRoleUpdate", str)))
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
                LogBLL.errorControlLog("modify", "人员角色更新_UserRoleUpdate", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public static string Delete(int id)
        {
            try
            {
                ht_admin_userrole bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
                bean.LT = DateTime.Now;
                bean.LU = Common.Common.UserID;
                bean.ST = 0;
                dal.UpdateEntity(bean, LogBLL.controllog("delete", "角色删除_Delete", id.ToString()));
                return "删除成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "角色删除_Delete", id.ToString(), ex.ToString());
                return ex.ToString();
            }
        }
    }
}
