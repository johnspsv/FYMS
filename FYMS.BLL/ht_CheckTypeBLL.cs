using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class ht_CheckTypeBLL
    {
        static ht_CheckTypeDAL dal = new ht_CheckTypeDAL();

        static PublicBLL.PubBll<ht_CheckType> pubbll = new PublicBLL.PubBll<ht_CheckType>();

        static LogDAL logDAL = new LogDAL();

        /// <summary>
        /// 获取所有审核类型
        /// </summary>
        /// <returns></returns>
        public static string CheckTypeAll()
        {
            List<ht_CheckType> a = dal.SelectEntities(x => x.ST >= 0).ToList();
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
        /// 新增审核类型
        /// </summary>
        /// <param name="str"></param>
        public static string CheckTypeAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<ht_CheckType> bll = new PublicBLL.PubBll<ht_CheckType>();
                ht_CheckType entity = bll.ReturnEntity(str);
                if (dal.SelectEntities(x => x.CheckName == entity.CheckName).Count > 0)
                {
                    return "审核类型名称重复";
                }
                else if (dal.SelectEntities(x => x.CheckCode == entity.CheckCode).Count > 0)
                {
                    return "审核类型编号重复";
                }
                else
                {
                    entity.CT = DateTime.Now;
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity, logDAL.log("add", "新增审核类型_CheckTypeAdd", str)))
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
                LogBLL.errorControlLog("add", "新增审核类型_CheckTypeAdd", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 更新审核类型
        /// </summary>
        /// <param name="str"></param>
        public static string CheckTypeUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<ht_CheckType> bll = new PublicBLL.PubBll<ht_CheckType>();
                ht_CheckType entity = bll.ReturnEntity(str);
                ht_CheckType model = dal.SelectEntities(x => x.ID == entity.ID).FirstOrDefault();
                PublicBLL.PubBll<ht_CheckType> bll1 = new PublicBLL.PubBll<ht_CheckType>();
                model = bll1.Edit(entity, model);
                model.LU = Common.Common.UserID;
                model.LT = DateTime.Now;
                if (dal.Update(model, logDAL.log("modify", "审核类型修改_CheckTypeUpdate", str)))
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
                LogBLL.errorControlLog("modify", "审核类型修改_CheckTypeUpdate", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <returns></returns>
        public static string CheckTypeDelete(int id)
        {
            try
            {
                ht_CheckType bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
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
                dal.UpdateEntity(bean,logDAL.log("delete", "审核类型禁用开启_CheckTypeDelete",JsonConvert.SerializeObject(bean)));
                return "成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "审核类型禁用开启_CheckTypeDelete", id.ToString(), ex.ToString());
                return "失败";
            }
        }
    }
}
