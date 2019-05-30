using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public class ct_AccountBLL
    {
        static ct_AccountDAL dal = new ct_AccountDAL();

        static PublicBLL.PubBll<ct_Account> pubbll = new PublicBLL.PubBll<ct_Account>();

        static LogDAL logDAL = new LogDAL();

        /// <summary>
        /// 获取所有账号类型
        /// </summary>
        /// <returns></returns>
        public static string AccountTypeAll()
        {
            List<ct_Account> a = dal.SelectEntities(x => x.ST >= 0).ToList();
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
        /// 新增账号类型
        /// </summary>
        /// <param name="str"></param>
        public static string AccountTypeAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<ct_Account> bll = new PublicBLL.PubBll<ct_Account>();
                ct_Account entity = bll.ReturnEntity(str);
                if (dal.SelectEntities(x => x.AccountName == entity.AccountName).Count > 0)
                {
                    return "账号类型名称重复";
                }
                else
                {
                    entity.CT = DateTime.Now;
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity, logDAL.log("add", "新增账号类型_AccountTypeAdd", str)))
                    {
                        return "保存成功";
                    }
                    else
                    {
                        return "保存失败";
                    }
                }
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("add", "新增账号类型_AccountTypeAdd", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 更新账号类型
        /// </summary>
        /// <param name="str"></param>
        public static string AccountTypeUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<ct_Account> bll = new PublicBLL.PubBll<ct_Account>();
                ct_Account entity = bll.ReturnEntity(str);
                ct_Account model = dal.SelectEntities(x => x.ID == entity.ID).FirstOrDefault();
                PublicBLL.PubBll<ct_Account> bll1 = new PublicBLL.PubBll<ct_Account>();
                model = bll1.Edit(entity, model);
                model.LU = Common.Common.UserID;
                model.LT = DateTime.Now;
                if (dal.Update(model, logDAL.log("modify", "账号类型修改_AccountTypeUpdate", str)))
                {
                    return "保存成功";
                }
                else
                {
                    return "保存失败";
                }
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("modify", "账号类型修改_AccountTypeUpdate", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <returns></returns>
        public static string AccountTypeDelete(int id)
        {
            try
            {
                ct_Account bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
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
                dal.UpdateEntity(bean, logDAL.log("delete", "账号类型禁用开启_AccountTypeDelete", JsonConvert.SerializeObject(bean)));
                return "成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "账号类型禁用开启_AccountTypeDelete", id.ToString(), ex.ToString());
                return "失败";
            }
        }
    }
}
