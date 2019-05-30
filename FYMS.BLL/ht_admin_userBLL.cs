using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYMS.DAL;
using FYMS.Model.Entities;
using FYMS.Common.ViewModel;
using Newtonsoft.Json;

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
        public static int Login(string username, string password)
        {
            IList<ht_admin_user> list = dal.SelectEntities(x => (x.user_code == username || x.user_email == username || x.user_phone == username) && x.user_password == password);
            if (list.Count > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static string All()
        {
            List<ht_admin_user> a = dal.SelectEntities(x => x.ST == 1).ToList();
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
            PublicBLL.PubBll<ht_admin_user> bll = new PublicBLL.PubBll<ht_admin_user>();
            ht_admin_user entity = bll.ReturnEntity(str);


            entity.CT = DateTime.Now;
            if (dal.SelectEntities(x => x.user_code == entity.user_code).Count > 0)
            {
                return "工号重复";
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
                entity.CU = 1;
                entity.LU = 1;
                entity.LT = DateTime.Now;
                entity.ST = 1;
                if (dal.Add(entity))
                {
                    return "保存成功";
                }
                else
                {
                    return "保存失败";
                }
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
            PublicBLL.PubBll<User_admin> bll = new PublicBLL.PubBll<User_admin>();
            User_admin user = bll.ReturnEntity(str);

            if (dal.SelectEntities(x => x.user_code == user.user_code).Count > 1)
            {
                return "工号重复";
            }
            else if (dal.SelectEntities(x => x.user_phone == user.user_phone).Count > 1)
            {
                return "手机号重复";
            }
            else if (dal.SelectEntities(x => x.user_email == user.user_email).Count > 1)
            {
                return "邮箱重复";
            }
            else if (!string.IsNullOrEmpty(user.user_idno) && dal.SelectEntities(x => x.user_idno == user.user_idno).Count > 1)
            {
                return "身份证号重复";
            }
            else
            {
                ht_admin_user htuser = dal.SelectEntities(x => x.ID == user.ID).FirstOrDefault();
                PublicBLL.PubBll<ht_admin_user> bll1 = new PublicBLL.PubBll<ht_admin_user>();
                htuser = bll1.Edit<User_admin>(user, htuser);
                htuser.LU = 1;
                htuser.LT = DateTime.Now;
                if (dal.Update(htuser))
                {
                    return "保存成功";
                }
                else
                {
                    return "保存失败";
                }
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
                bean.ST = 0;
                dal.UpdateEntity(bean);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
