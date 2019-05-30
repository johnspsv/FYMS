using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYMS.DAL;
using FYMS.Common.ViewModel;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;

namespace FYMS.BLL
{
    public static class ht_admin_userBLL
    {
        static ht_admin_userDAL dal = new ht_admin_userDAL();

        static PublicBLL.PubBll<ht_admin_user> pubbll = new PublicBLL.PubBll<ht_admin_user>();

        /// <summary>
        /// 查询账户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string[] Login(string username, string password)
        {

            string[] str = new string[2];
            if (username == "admin")
            {
                ht_admin_user entity = dal.SelectEntities(x => x.user_code == username && x.user_password == password).FirstOrDefault();
                if (entity != null)
                {
                    Common.Common.UserID = entity.ID;
                    Common.Common.UserName = entity.user_name;
                    Common.Common.RoleID = 0;
                    Common.Common.RoleName = "admin";
                    Common.Common.menulist = new List<HtMenu>();
                    Ht_MenuDAL ht_MenuDAL = new Ht_MenuDAL();
                    foreach (var item in ht_MenuDAL.SelectEntities(x => x.ST >= 0))
                    {
                        HtMenu htMenu = new HtMenu();
                        htMenu.ID = item.ID;
                        htMenu.Name = item.Name;
                        htMenu.Path = item.Path;
                        htMenu.Floor = item.Floor == null ? 0 : Convert.ToInt32(item.Floor);
                        htMenu.FloorID = item.FloorID == null ? 0 : Convert.ToInt32(item.FloorID);
                        htMenu.FloorName = item.FloorName;
                        htMenu.funAdd = 1;
                        htMenu.funSelect = 1;
                        htMenu.funDelete = 1;
                        htMenu.funUpdate = 1;
                        Common.Common.menulist.Add(htMenu);
                    }
                    str[0] = JsonConvert.SerializeObject(entity);
                    str[1] = JsonConvert.SerializeObject(ht_MenuDAL.SelectEntities(x => x.ST >= 0));
                    return str;
                }
                else
                {
                    return str;
                }
            }
            else
            {
                ObjectResult<proc_Login_Result> list1 = dal.Login(username, password);
                var list = list1.ToList();
                if (list.Count > 0)
                {
                    Common.Common.UserID = list[0].UserID;
                    Common.Common.UserName = list[0].UserName;
                    Common.Common.RoleID = list[0].RoleID == null ? 0 : Convert.ToInt32(list[0].RoleID);
                    Common.Common.RoleName = list[0].RoleName;
                    Common.Common.menulist = new List<HtMenu>();
                    foreach (var item in list)
                    {
                        HtMenu htMenu = new HtMenu();
                        htMenu.ID = item.MenuID == null ? 0 : Convert.ToInt32(item.MenuID);
                        htMenu.Name = item.MenuName;
                        htMenu.Path = item.MenuPath;
                        htMenu.Floor = item.MenuFloor == null ? 0 : Convert.ToInt32(item.MenuFloor);
                        htMenu.FloorID = item.FloorID == null ? 0 : Convert.ToInt32(item.FloorID);
                        htMenu.FloorName = item.FloorName;
                        htMenu.funAdd = item.funAdd == null ? 0 : Convert.ToInt32(item.funAdd);
                        htMenu.funDelete = item.funDelete == null ? 0 : Convert.ToInt32(item.funDelete);
                        htMenu.funSelect = item.funSelect == null ? 0 : Convert.ToInt32(item.funSelect);
                        htMenu.funUpdate = item.funUpdate == null ? 0 : Convert.ToInt32(item.funUpdate);
                        Common.Common.menulist.Add(htMenu);
                    }
                    str[0] = JsonConvert.SerializeObject(list);
                    str[1] = JsonConvert.SerializeObject(Common.Common.menulist);
                    return str;
                }
                else
                {
                    return str;
                }
            }
        }

        /// <summary>
        /// 判断该用户是否有登录权限（用户存在，导航列表为null时，视为无权限）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Loginlimit(string username, string password)
        {
            string str;
            IList<ht_admin_user> list = dal.SelectEntities(x => (x.user_code == username || x.user_email == username || x.user_phone == username) && x.user_password == password);
            if (list.Count > 0)
            {
                if (list[0].ST > 0)
                {
                    str = "登录失败！人员无登录权限";
                }
                else
                {
                    str = "登录失败！请检查用户名和密码";
                }
            }
            else
            {
                str = "登录失败！请检查用户名和密码";
            }
            return str;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static string All()
        {
            List<ht_admin_user> a = dal.SelectEntities(x => x.ST == 1 && x.user_code != "admin").ToList();
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
        /// 新增管理员
        /// </summary>
        /// <param name="str"></param>
        public static string UserAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<ht_admin_user> bll = new PublicBLL.PubBll<ht_admin_user>();
                ht_admin_user entity = bll.ReturnEntity(str);

                entity.CT = DateTime.Now;
                if (dal.SelectEntities(x => x.user_code == entity.user_code).Count > 0)
                {
                    return "账号重复";
                }
                else if (dal.SelectEntities(x => x.user_phone == entity.user_phone).Count > 0)
                {
                    return "手机号重复";
                }
                else if (dal.SelectEntities(x => x.user_email == entity.user_email).Count > 0)
                {
                    return "邮箱重复";
                }
                else if (!string.IsNullOrEmpty(entity.user_idno) && dal.SelectEntities(x => x.user_idno == entity.user_idno).Count > 0)
                {
                    return "身份证号重复";
                }
                else
                {
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity,LogBLL.controllog("add", "新增人员信息_UserAdd",str)))
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
                LogBLL.errorControlLog("add", "新增人员信息_UserAdd", str, ex.ToString());
                return "保存失败";
            }
        }


        /// <summary>
        /// 根据ID获取管理员对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ModelByID(int id)
        {
            ht_admin_user bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
            string str = JsonConvert.SerializeObject(bean);
            return str;
        }

        /// <summary>
        /// 管理员更新
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<User_admin> bll = new PublicBLL.PubBll<User_admin>();
                User_admin user = bll.ReturnEntity(str);
                if (dal.SelectEntities(x => x.user_code == user.user_code && x.ID != user.ID).Count > 0)
                {
                    return "账号重复";
                }
                else if (dal.SelectEntities(x => x.user_phone == user.user_phone && x.ID != user.ID).Count > 0)
                {
                    return "手机号重复";
                }
                else if (dal.SelectEntities(x => x.user_email == user.user_email && x.ID != user.ID).Count > 0)
                {
                    return "邮箱重复";
                }
                else if (!string.IsNullOrEmpty(user.user_idno) && dal.SelectEntities(x => x.user_idno == user.user_idno && x.ID != user.ID).Count > 0)
                {
                    return "身份证号重复";
                }
                else
                {
                    ht_admin_user htuser = dal.SelectEntities(x => x.ID == user.ID).FirstOrDefault();
                    PublicBLL.PubBll<ht_admin_user> bll1 = new PublicBLL.PubBll<ht_admin_user>();
                    htuser = bll1.Edit<User_admin>(user, htuser);
                    htuser.LU = Common.Common.UserID;
                    htuser.LT = DateTime.Now;
                    LogDAL logdal = new LogDAL();
                    if (dal.Update(htuser, logdal.log("modify", "人员信息更新_UserUpdate", str)))
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
                LogBLL.errorControlLog("add", "人员信息更新_UserUpdate", str, ex.ToString());
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
                ht_admin_user bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
                bean.LU = Common.Common.UserID;
                bean.LT = DateTime.Now;
                bean.ST = 0;
                dal.UpdateEntity(bean,LogBLL.controllog("delete", "人员删除_Delete", id.ToString()));
                return "删除成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "人员删除_Delete", id.ToString(), ex.ToString());
                return "删除失败";
            }
        }
    }
}
