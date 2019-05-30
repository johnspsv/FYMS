using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{

    public class Ht_LimitBLL
    {
        static Ht_LimitDAL dal = new Ht_LimitDAL();

        static PublicBLL.PubBll<Ht_Limit> pubBll = new PublicBLL.PubBll<Ht_Limit>();

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static string LimitAll()
        {
            List<Ht_Limit> a = dal.SelectEntities(x => x.ST >= 0).ToList();
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
        /// 保存
        /// </summary>
        /// <returns></returns>
        public static bool LimitSave(string str, string str1)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Ht_Limit ht_Limits = JsonConvert.DeserializeObject<Ht_Limit>(str);
                    List<Ht_LimitDetails> ht_LimitDetailslist = JsonConvert.DeserializeObject<List<Ht_LimitDetails>>(str1);
                    foreach (var i in ht_LimitDetailslist)
                    {
                        i.CT = DateTime.Now;
                        i.CU = Common.Common.UserID;
                        i.LU = Common.Common.UserID;
                        i.LT = DateTime.Now;
                        i.ST = 1;
                    }
                    ht_Limits.Ht_LimitDetails = ht_LimitDetailslist;
                    ht_admin_userrole userrole = dal.RoleALL().Where(x => x.ID == ht_Limits.RoleID).FirstOrDefault();
                    ht_Limits.RoleName = userrole.role_name;
                    ht_Limits.CT = DateTime.Now;
                    ht_Limits.CU = Common.Common.UserID;
                    ht_Limits.LU = Common.Common.UserID;
                    ht_Limits.LT = DateTime.Now;
                    ht_Limits.ST = 1;

                    List<Ht_Menu> ht_Menulist = dal.MenuALL();
                    if (dal.LimitSave(ht_Limits, ht_LimitDetailslist, ht_Menulist,LogBLL.controllog("add", "权限保存_LimitSave",str)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("add", "权限保存_LimitSave", str, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool LimitUpdate(string[] str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str[0]))
                {
                    Ht_Limit ht_Limit = JsonConvert.DeserializeObject<Ht_Limit>(str[0]);
                    Ht_Limit ht_Limit1 = dal.SelectEntities(x => x.ST >= 0).ToList().Where(x => x.ID == ht_Limit.ID).FirstOrDefault();
                    ht_Limit1.Common = ht_Limit.Common;
                    ht_Limit1.LT = DateTime.Now;
                    ht_Limit1.LU = Common.Common.UserID;

                    List<Ht_LimitDetails> newlist = JsonConvert.DeserializeObject<List<Ht_LimitDetails>>(str[1]);
                    List<Ht_LimitDetails> oldlist = dal.DetailsUpdate(x => x.LimitID == ht_Limit.ID);

                    List<Ht_LimitDetails> addlist = new List<Ht_LimitDetails>();
                    List<Ht_LimitDetails> deletelist = new List<Ht_LimitDetails>();
                    List<Ht_LimitDetails> updatelist = new List<Ht_LimitDetails>();

                    foreach (var i in oldlist)
                    {
                        if (newlist.Where(x => x.MenuID == i.MenuID).Count() > 0)
                        {
                            if (newlist.Where(x => x.MenuID == i.MenuID && x.funAdd == i.funAdd && x.funDelete == i.funDelete && x.funSelect == i.funSelect && x.funUpdate == i.funUpdate).Count() > 0)
                            {
                                newlist.Remove(newlist.Where(x => x.MenuID == i.MenuID).FirstOrDefault());
                            }
                            else
                            {
                                var q = newlist.Where(x => x.MenuID == i.MenuID).FirstOrDefault();
                                i.funAdd = q.funAdd;
                                i.funDelete = q.funDelete;
                                i.funSelect = q.funSelect;
                                i.funUpdate = q.funUpdate;
                                i.LT = DateTime.Now;
                                i.LU = Common.Common.UserID;
                                
                                updatelist.Add(i);
                                newlist.Remove(newlist.Where(x => x.MenuID == i.MenuID).FirstOrDefault());
                            }
                        }
                        else
                        {
                            deletelist.Add(i);
                        }
                    }

                    addlist = newlist;
                    foreach(var i in addlist)
                    {
                        i.CT = DateTime.Now;
                        i.CU = Common.Common.UserID;
                        i.LT = DateTime.Now;
                        i.LU = Common.Common.UserID;
                        i.ST = 1;
                        i.LimitID = ht_Limit.ID;
                    }

                    

                    dal.UpdateList(ht_Limit1, addlist, deletelist,updatelist,LogBLL.controllog("modify", "权限更新_LimitUpdate",JsonConvert.SerializeObject(str)));

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("modify", "权限更新_LimitUpdate", JsonConvert.SerializeObject(str), ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 返回明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string[] LimitEditData(int id)
        {
            string[] strarry = new string[2];
            Ht_Limit ht_Limit = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
            List<Ht_LimitDetails> ht_LimitDetails = dal.DetailsALL(id);
            strarry[0] = JsonConvert.SerializeObject(ht_Limit);
            strarry[1] = JsonConvert.SerializeObject(ht_LimitDetails);
            return strarry;
        }

        /// <summary>
        /// 禁用开启
        /// </summary>
        /// <returns></returns>
        public static string LimitDelete(int id)
        {
            try
            {
                Ht_Limit bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
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
                dal.UpdateEntity(bean,LogBLL.controllog("delete", "权限禁用开启_LimitDelete",id.ToString()));
                return "成功";
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("modify", "权限禁用开启_LimitDelete", id.ToString(), ex.ToString());
                return "失败";
            }
        }
    }
}
