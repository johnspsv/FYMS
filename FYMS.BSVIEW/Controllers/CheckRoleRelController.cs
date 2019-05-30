using FYMS.Common.ViewModel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYMS.BSVIEW.Controllers
{
    public class CheckRoleRelController : Controller
    {

        MainService.MainServiceClient client = new MainService.MainServiceClient();
        // GET: CheckRoleRel

        /// <summary>
        /// 审核角色
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult CheckRoleRelMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {

                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/CheckType/CheckTypeMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<CheckRoleRel> list = new List<CheckRoleRel>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                        var i = BLL.ht_CheckRoleRelBLL.CheckRoleRelAll(); //client.AdminRoleRelAll();
                        //var i = BLL.ht_userroleRelBLL.All();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<CheckRoleRel>>(i);
                            var b = list.Select(x => x.RoleID).Distinct();
                            List<CheckRole> rolelist = new List<CheckRole>();
                            foreach (var x in b)
                            {
                                CheckRole checkRole = new CheckRole();
                                checkRole.ID = x;

                                checkRole.RoleName = list.Where(q => q.RoleID == x).Select(q => q.RoleName).FirstOrDefault().ToString();
                                checkRole.ST = list.Where(q => q.RoleID == x).Select(q => q.ST).FirstOrDefault();
                                rolelist.Add(checkRole);
                            }


                            var users = from u in rolelist select u;

                            if (!string.IsNullOrEmpty(searchString))
                            {
                                page = 1;
                                users = users.Where(u => u.RoleName.Contains(searchString));
                            }
                            else
                            {
                                searchString = currentFilter;
                            }
                            ViewBag.CurrentFilter = searchString;

                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.RoleName);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.RoleName);
                                    break;
                            }
                            int j = 1;
                            foreach (var x in users)
                            {
                                x.number = j++;
                            }
                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(users.ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            IQueryable<CheckRole> iq = null;
                            IPagedList<CheckRole> pglist = new PagedList<CheckRole>(iq, 1, 1);
                            list = null;
                            return View(pglist);

                        }
                    }
                    else
                    {
                        return Redirect("/Index/ErrorPage404");
                    }
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckRoleRel" + "," + "审核角色查询_CheckRoleRelMain" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckRoleRelCreate()
        {
            try
            {

                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/CheckType/CheckTypeMain");
                    if (look != null)
                    {
                        List<Admin_Role> roles = JsonConvert.DeserializeObject<List<Admin_Role>>(client.AdminRoleAll());
                        return View(roles);
                    }
                    else
                    {
                        return Redirect("/Index/ErrorPage404");
                    }
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckRoleRel" + "," + "审核角色查询_CheckRoleRelCreate" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckRoleRelDetail(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/CheckType/CheckTypeMain");
                    if (look != null)
                    {
                        ViewData["roleID"] = ID;
                        List<CheckRoleRel> list = JsonConvert.DeserializeObject<List<CheckRoleRel>>(BLL.ht_CheckRoleRelBLL.DataByRoleID(ID)).ToList();
                        ViewData["rolename"] = list.Select(x => x.RoleName).FirstOrDefault().ToString();
                        string checktypes = "";
                        foreach (var a in list)
                        {
                            checktypes += a.CheckID.ToString() + ",";
                        }
                        ViewData["checktypes"] = checktypes;
                        return View();
                    }
                    else
                    {
                        return Redirect("/Index/ErrorPage404");
                    }
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckRoleRel" + "," + "审核角色查询_CheckRoleRelCreate" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 新增treeview
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TreeData()
        {
            List<CheckType> list = new List<CheckType>();
            var i = client.CheckTypeAll();
            list = JsonConvert.DeserializeObject<List<CheckType>>(i);
            var parent = list.Where(x => x.ST > 0).ToList();

            List<CheckTypeJson> checkTypeJsons = new List<CheckTypeJson>();

            foreach (var j in parent)
            {
                CheckTypeJson node = new CheckTypeJson();
                node.state = new state();
                node.nodeid = j.ID.ToString();
                node.text = j.CheckName;
                node.color = "red";
                node.state.@checked = false;

                checkTypeJsons.Add(node);
            }

            return Json(checkTypeJsons, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Save(FormCollection fc)
        {
            try
            {
                string menu = "";
                CheckRoleRel checkRoleRel = new CheckRoleRel();
                if (Convert.ToInt32(fc["checkrolerel_role"]) == 0)
                {
                    return "请选择角色";
                }
                checkRoleRel.RoleID = Convert.ToInt32(fc["checkrolerel_role"]);
                if (JsonConvert.DeserializeObject<List<CheckRoleRel>>(BLL.ht_CheckRoleRelBLL.CheckRoleRelAll()).Where(x => x.RoleID == checkRoleRel.RoleID).Count() > 0)
                {
                    return "该角色已经授予权限，请选择其他角色";
                }
                menu = fc["menu_id"] == null ? "" : fc["menu_id"].ToString();
                if (menu == "")
                {
                    return "该角色没有授予任何权限，请为该角色选择相应的权限";
                }
                List<CheckRoleRel> list = new List<CheckRoleRel>();
                if (!string.IsNullOrEmpty(menu))
                {
                    string[] menuids = menu.Remove(menu.Length - 1).Split(',');
                    foreach (string i in menuids)
                    {
                        CheckRoleRel checkRoleRel1 = new CheckRoleRel();
                        checkRoleRel1.CheckID = Convert.ToInt32(i);
                        checkRoleRel1.RoleID = checkRoleRel.RoleID;
                        list.Add(checkRoleRel1);
                    }
                }
                string str = JsonConvert.SerializeObject(list);

                if (BLL.ht_CheckRoleRelBLL.CheckRoleRelAdd(str))
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
                client.ErrorlogAsync("CheckRoleRel" + "|" + "审核权限新增_Save" + "|" + ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult CheckRoleRelEdit(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/CheckType/CheckTypeMain");
                    if (look != null)
                    {
                        ViewData["roleID"] = ID;
                        List<CheckRoleRel> list = JsonConvert.DeserializeObject<List<CheckRoleRel>>(BLL.ht_CheckRoleRelBLL.DataByRoleID(ID)).ToList();
                        ViewData["rolename"] = list.Select(x => x.RoleName).FirstOrDefault().ToString();
                        string checktypes = "";
                        foreach (var a in list)
                        {
                            checktypes += a.CheckID.ToString() + ",";
                        }
                        ViewData["checktypes"] = checktypes;
                        return View();
                    }
                    else
                    {
                        return Redirect("/Index/ErrorPage404");
                    }
                }

                else
                {
                    return Redirect("/Login/Login");
                }
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckRoleRel" + "," + "审核角色查询_CheckRoleRelCreate" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 新增treeview
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TreeDataEdit(int ID)
        {
            List<CheckType> list = new List<CheckType>();
            var i = BLL.ht_CheckRoleRelBLL.CheckRoleRelEdit(ID);
            var parent = JsonConvert.DeserializeObject<List<CheckType>>(i[0]).ToList();
            var checks= JsonConvert.DeserializeObject<List<CheckRoleRel>>(i[1]).ToList();

            List<CheckTypeJson> checkTypeJsons = new List<CheckTypeJson>();

            foreach (var j in parent)
            {
                CheckTypeJson node = new CheckTypeJson();
                node.state = new state();
                node.nodeid = j.ID.ToString();
                node.text = j.CheckName;
                node.color = "red";
                if (checks.Where(x => x.CheckID == j.ID).Count() > 0)
                    node.state.@checked = true;
                else
                    node.state.@checked = false;
                checkTypeJsons.Add(node);
            }

            return Json(checkTypeJsons, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public string Update(FormCollection fc)
        {
            try
            {
                string menu = "";
                CheckRoleRel checkRoleRel = new CheckRoleRel();

                checkRoleRel.RoleID = Convert.ToInt32(fc["role_id"]);
                
                menu = fc["menu_id"] == null ? "" : fc["menu_id"].ToString();
                if (menu == "")
                {
                    return "该角色没有授予任何权限，请为该角色选择相应的权限";
                }
                List<CheckRoleRel> list = new List<CheckRoleRel>();
                if (!string.IsNullOrEmpty(menu))
                {
                    string[] menuids = menu.Remove(menu.Length - 1).Split(',');
                    foreach (string i in menuids)
                    {
                        CheckRoleRel checkRoleRel1 = new CheckRoleRel();
                        checkRoleRel1.CheckID = Convert.ToInt32(i);
                        checkRoleRel1.RoleID = checkRoleRel.RoleID;
                        list.Add(checkRoleRel1);
                    }
                }
                string str = JsonConvert.SerializeObject(list);

                if (BLL.ht_CheckRoleRelBLL.CheckRoleRelUpdate(str))
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
                client.ErrorlogAsync("CheckRoleRel" + "|" + "审核权限更新_Update" + "|" + ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool CheckRoleRelDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            return BLL.ht_CheckRoleRelBLL.CheckRoleRelDelete(i);
        }
    }
}