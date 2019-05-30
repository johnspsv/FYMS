using FYMS.Common.ViewModel;
using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class Ht_MenuBLL
    {
        static Ht_MenuDAL dal = new Ht_MenuDAL();

        static PublicBLL.PubBll<Ht_Menu> pubBll = new PublicBLL.PubBll<Ht_Menu>();

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static string MenuAll()
        {
            List<Ht_Menu> a = dal.SelectEntities(x => x.ST>=0).ToList();
            if (a != null)
            {
                return pubBll.ReturnStr(a);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增导航栏
        /// </summary>
        /// <param name="str"></param>
        public static string MenuAdd(string str)
        {
            try
            {
                PublicBLL.PubBll<Ht_Menu> bll = new PublicBLL.PubBll<Ht_Menu>();
                Ht_Menu entity = bll.ReturnEntity(str);
                if (dal.SelectEntities(x => x.Name == entity.Name).Count > 0)
                {
                    return "该人员已经被赋予角色";
                }
                else
                {
                    entity.CT = DateTime.Now;
                    entity.CU = Common.Common.UserID;
                    entity.LU = Common.Common.UserID;
                    entity.LT = DateTime.Now;
                    entity.ST = 1;
                    if (dal.Add(entity,LogBLL.controllog("add","新增导航栏_MenuAdd",str)))
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
                LogBLL.errorControlLog("add", "新增导航栏_MenuAdd", str, ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 更新导航栏
        /// </summary>
        /// <param name="str"></param>
        public static string MenuUpdate(string str)
        {
            try
            {
                PublicBLL.PubBll<HtMenu> bll = new PublicBLL.PubBll<HtMenu>();
                HtMenu entity = bll.ReturnEntity(str);
                Ht_Menu model = dal.SelectEntities(x => x.ID == entity.ID).FirstOrDefault();
                PublicBLL.PubBll<Ht_Menu> bll1 = new PublicBLL.PubBll<Ht_Menu>();
                model = bll1.Edit(entity, model);
                model.LU = Common.Common.UserID; ;
                model.LT = DateTime.Now;
                if (dal.Update(model, LogBLL.controllog("modify", "更新导航栏_MenuUpdate", str)))
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
                LogBLL.errorControlLog("modify", "更新导航栏_MenuUpdate", str, ex.ToString());
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
            Ht_Menu bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
            string str = JsonConvert.SerializeObject(bean);
            return str;
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <returns></returns>
        public static string MenuDelete(int id)
        {
            try
            {
                Ht_Menu bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
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
                dal.UpdateEntity(bean,LogBLL.controllog("delete", "导航栏禁用开启_MenuDelete",id.ToString()));
                return "成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("delete", "导航栏禁用开启_MenuDelete", id.ToString(), ex.ToString());
                return "失败";
            }
        }
    }
}
