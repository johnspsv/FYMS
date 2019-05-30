using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class ht_CheckRoleRelBLL
    {
        static ht_CheckRoleRelDAL dal = new ht_CheckRoleRelDAL();

        static PublicBLL.PubBll<ht_CheckType> pubbll = new PublicBLL.PubBll<ht_CheckType>();

        /// <summary>
        /// 获取审核权限的角色数据
        /// </summary>
        /// <returns></returns>
        public static string CheckRoleRelAll()
        {
            string str=JsonConvert.SerializeObject(dal.CheckRoleRelAll());
            return str;
        }

        /// <summary>
        /// 审核权限新增
        /// </summary>
        /// <returns></returns>
        public static bool CheckRoleRelAdd(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    List<ht_CheckRoleRel> list = JsonConvert.DeserializeObject<List<ht_CheckRoleRel>>(str);

                    foreach(var a in list)
                    {
                        a.ST = 1;
                        a.CU = Common.Common.UserID;
                        a.CT = DateTime.Now;
                        a.LT = DateTime.Now;
                        a.LU = Common.Common.UserID;
                    }
                    
                    if (dal.Add(list, LogBLL.controllog("add", "审核权限新增_CheckRoleRelAdd", str)))
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
            catch (Exception ex)
            {
                LogBLL.errorControlLog("add", "审核权限新增_CheckRoleRelAdd", str, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 审核权限更新
        /// </summary>
        /// <returns></returns>
        public static bool CheckRoleRelUpdate(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    List<ht_CheckRoleRel> list = JsonConvert.DeserializeObject<List<ht_CheckRoleRel>>(str);
                    int roleid = Convert.ToInt32(list.Select(y => y.RoleID).FirstOrDefault());
                    List<ht_CheckRoleRel> list1 = dal.SelectEntities(x=>x.RoleID==roleid).ToList();

                    List<ht_CheckRoleRel> listAdd = new List<ht_CheckRoleRel>();
                    List<ht_CheckRoleRel> listDelete = new List<ht_CheckRoleRel>();
                    foreach (var a in list)
                    {
                        var b = list1.Where(x => x.CheckID == a.CheckID);
                        if(b.Count()>0)
                        {
                            var d = list1.Where(x => x.CheckID == a.CheckID).FirstOrDefault();
                            list1.Remove(d);
                        }
                        else
                        {
                            var d = list1.Where(x => x.CheckID == a.CheckID).FirstOrDefault();
                            d.CU = Common.Common.UserID;
                            d.CT = DateTime.Now;
                            d.LU = Common.Common.UserID;
                            d.LT = DateTime.Now;
                            listAdd.Add(d);
                        }
                    }
                    listDelete = list1;

                    if (listAdd.Count > 0 || listDelete.Count > 0)
                    {
                        if (dal.CheckRoleRelUpdate(listAdd, listDelete, LogBLL.controllog("modify", "审核权限更新_CheckRoleRelUpdate", str)))
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
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("modify", "审核权限更新_CheckRoleRelUpdate", str, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 通过角色ID获取数据
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public static string DataByRoleID(int roleid)
        {
            string str = JsonConvert.SerializeObject(dal.CheckRoleRelByRoleID(roleid));
            return str;
        }

        /// <summary>
        /// 获取审核权限的角色数据
        /// </summary>
        /// <returns></returns>
        public static string[] CheckRoleRelEdit(int roleid)
        {
            ht_CheckTypeDAL dal1 = new ht_CheckTypeDAL();
            string[] str = new string[2];
            string str0 = JsonConvert.SerializeObject(dal1.SelectEntities(x=>x.ST>0).ToList());
            str[0] = str0;
            string str1 = JsonConvert.SerializeObject(dal.CheckRoleRelByRoleID(roleid));
            str[1] = str1;
            return str;
        }


        /// <summary>
        /// 根据角色ID批量修改ST 
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public static bool CheckRoleRelDelete(int roleid)
        {
            try
            {
                ht_CheckRoleRel bean = new ht_CheckRoleRel();
                bean = dal.SelectEntities(x => x.RoleID == roleid).FirstOrDefault();
                string[] str = new string[3];
                str[0] = "ST";
                str[1] = "LT";
                str[2] = "LU";
                if (bean.ST == 0)
                {
                    bean.ST = 1;
                    bean.LU = Common.Common.UserID;
                    bean.LT = DateTime.Now;
                    if (dal.UpdateEntities(bean, (x => x.RoleID == roleid), LogBLL.controllog("delete", "审核权限禁用启用_CheckRoleRelUpdate", roleid.ToString()), str))
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
                    bean.ST = 0;
                    bean.LU = Common.Common.UserID;
                    bean.LT = DateTime.Now;
                    if (dal.UpdateEntities(bean, (x => x.RoleID == roleid), LogBLL.controllog("delete", "审核权限禁用启用_CheckRoleRelUpdate", roleid.ToString()), str))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("delete", "审核权限禁用开启_CheckTypeDelete", roleid.ToString(), ex.ToString());
                return false;
            }
       }
    }
}
