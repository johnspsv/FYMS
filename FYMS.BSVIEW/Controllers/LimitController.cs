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

    public class LimitController : Controller
    {
        MainService.MainServiceClient client = new MainService.MainServiceClient();

        // GET: Limit
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult LimitMain(string sortOrder, string searchString, string currentFilter, int? page)
        {

            try
            {

                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Limit/LimitMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;
                        List<HtLimit> list = new List<HtLimit>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        var i = client.LimitAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<HtLimit>>(i);
                            var users = from u in list select u;

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
                            IQueryable<HtLimit> iq = null;
                            IPagedList<HtLimit> pglist = new PagedList<HtLimit>(iq, 1, 1);
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
                client.ErrorlogAsync("CheckType" + "," + "权限查询_LimitMain" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "权限查询_LimitMain", "");
            }
        }

        /// <summary>
        /// treeview赋值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TreeData()
        {
            try
            {
                List<HtMenu> list = new List<HtMenu>();
                var i = client.MenuAll();
                list = JsonConvert.DeserializeObject<List<HtMenu>>(i);
                var parent = list.Where(x => x.FloorID == 0).ToList();

                List<HtMenuJson> nodelist = new List<HtMenuJson>();
                foreach (var parentitem in parent)
                {
                    HtMenuJson node = new HtMenuJson();
                    node.state = new state();
                    node.nodeid = parentitem.ID.ToString();
                    node.text = parentitem.Name;
                    node.color = "red";
                    node.state.@checked = false;
                    var child = list.Where(x => x.FloorID == parentitem.ID).ToList();
                    node.nodes = new List<HtMenuJson>();
                    foreach (var childitem in child)
                    {
                        HtMenuJson node1 = new HtMenuJson();
                        node1.state = new state();
                        node1.nodeid = childitem.ID.ToString();
                        node1.text = childitem.Name;
                        node1.color = "#2894FF";
                        node1.state.@checked = false;
                        node.nodes.Add(node1);
                        node1.nodes = new List<HtMenuJson>();

                        for (int q = 0; q < 4; q++)//0 add 1 delete 2 select 3 update
                        {
                            HtMenuJson node2 = new HtMenuJson();
                            node2.state = new state();
                            node2.nodeid = node1.nodeid + "_" + q;
                            if (q == 0)
                                node2.text = "增";
                            else if (q == 1)
                                node2.text = "删";
                            else if (q == 2)
                                node2.text = "查";
                            else
                                node2.text = "改";
                            node2.state.@checked = false;
                            node2.color = "green";
                            node1.nodes.Add(node2);
                        }
                    }
                    nodelist.Add(node);
                }
                //JsonResult jsonResult = new JsonResult();
                //jsonResult.Data = nodelist;
                return Json(nodelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Limit" + "|" + "treeview赋值_TreeData" + "|" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 新增页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult LimitCreate()
        {
            if (Common.Common.UserID > 0)
            {
                if (Common.Common.CanRead("/Limit/LimitMain") != null)
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

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LimitDetails(int id)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    if (Common.Common.CanRead("/Limit/LimitMain") != null)
                    {
                        string[] str = client.LimitEditData(id);
                        HtLimit htLimit = JsonConvert.DeserializeObject<HtLimit>(str[0]);
                        ViewData["rolename"] = htLimit.RoleName;
                        ViewData["common"] = htLimit.Common;
                        ViewData["limit_id"] = id;
                        List<HtLimitDts> limitDetaillist = JsonConvert.DeserializeObject<List<HtLimitDts>>(str[1]);
                        string value = "";
                        foreach (var i in limitDetaillist)
                        {
                            value += i.MenuID.ToString() + ",";
                        }
                        ViewData["value"] = value;
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
                client.ErrorlogAsync("Limit" + "|" + "权限信息查看_LimitDetails" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 编辑时显示的treeview
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TreeDataEdit(int id)
        {

            string[] str = client.LimitEditData(id);
            List<HtLimitDts> limitDetaillist = JsonConvert.DeserializeObject<List<HtLimitDts>>(str[1]);
            string value = "";
            foreach (var i in limitDetaillist)
            {
                value += i.MenuID.ToString() + ",";
            }
            ViewData["value"] = value;
            try
            {
                List<HtMenu> list = new List<HtMenu>();
                var i = client.MenuAll();
                list = JsonConvert.DeserializeObject<List<HtMenu>>(i);
                var parent = list.Where(x => x.FloorID == 0).ToList();

                List<HtMenuJson> nodelist = new List<HtMenuJson>();
                foreach (var parentitem in parent)
                {
                    HtMenuJson node = new HtMenuJson();
                    node.state = new state();
                    node.nodeid = parentitem.ID.ToString();
                    node.text = parentitem.Name;
                    node.color = "red";
                    if (limitDetaillist.Where(x => x.MenuID == parentitem.ID).Count() > 0)
                    {
                        node.state.@checked = true;
                    }
                    else
                    {
                        node.state.@checked = false;
                    }
                    var child = list.Where(x => x.FloorID == parentitem.ID).ToList();
                    node.nodes = new List<HtMenuJson>();
                    foreach (var childitem in child)
                    {
                        HtMenuJson node1 = new HtMenuJson();
                        node1.state = new state();
                        node1.nodeid = childitem.ID.ToString();
                        node1.text = childitem.Name;
                        node1.color = "#2894FF";
                        if (limitDetaillist.Where(x => x.MenuID == childitem.ID).Count() > 0)
                        {
                            node1.state.@checked = true;
                        }
                        else
                        {
                            node1.state.@checked = false;
                        }
                        node.nodes.Add(node1);
                        node1.nodes = new List<HtMenuJson>();
                        for (int q = 0; q < 4; q++)//0 add 1 delete 2 select 3 update
                        {
                            HtMenuJson node2 = new HtMenuJson();
                            node2.state = new state();
                            node2.nodeid = node1.nodeid + "_" + q;
                            var t = limitDetaillist.Where(x => x.MenuID == childitem.ID).FirstOrDefault();
                            if (t == null)
                            {
                                if (q == 0)
                                    node2.text = "增";
                                else if (q == 1)
                                    node2.text = "删";
                                else if (q == 2)
                                    node2.text = "查";
                                else
                                    node2.text = "改";
                                node2.state.@checked = false;
                            }
                            else
                            {
                                if (q == 0)
                                {
                                    node2.text = "增";
                                    if (t.funAdd == 1)
                                    {
                                        node2.state.@checked = true;
                                    }
                                    else
                                    {
                                        node2.state.@checked = false;
                                    }
                                }
                                else if (q == 1)
                                {
                                    node2.text = "删";
                                    if (t.funDelete == 1)
                                    {
                                        node2.state.@checked = true;
                                    }
                                    else
                                    {
                                        node2.state.@checked = false;
                                    }
                                }
                                else if (q == 2)
                                {
                                    node2.text = "查";
                                    if (t.funSelect == 1)
                                    {
                                        node2.state.@checked = true;
                                    }
                                    else
                                    {
                                        node2.state.@checked = false;
                                    }
                                }
                                else
                                {
                                    node2.text = "改";
                                    if (t.funUpdate == 1)
                                    {
                                        node2.state.@checked = true;
                                    }
                                    else
                                    {
                                        node2.state.@checked = false;
                                    }
                                }
                            }
                            node2.color = "green";
                            node1.nodes.Add(node2);
                        }
                    }
                    nodelist.Add(node);
                }

                return Json(nodelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Limit" + "|" + "treeview赋值_TreeDataEdit" + "|" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Update(FormCollection fc)
        {
            try
            {
                string menu = "";
                HtLimit htLimit = new HtLimit();
                htLimit.ID = Convert.ToInt32(fc["limit_id"]);
                htLimit.Common = fc["limit_common"] == null ? "" : fc["limit_common"].ToString();
                menu = fc["menu_id"] == null ? "" : fc["menu_id"].ToString();

                List<HtLimitDts> htLimitDtslist = new List<HtLimitDts>();

                if (!string.IsNullOrEmpty(menu))
                {
                    string[] menuids = menu.Remove(menu.Length - 1).Split(',');
                    List<Fun> funlist = new List<Fun>();
                    foreach (string i in menuids)
                    {
                        if (i.Contains("_"))
                        {
                            string[] a = i.Split('_');

                            Fun fun = new Fun();
                            fun.menuid = Convert.ToInt32(a[0]);
                            fun.funid = Convert.ToInt32(a[1]);
                            funlist.Add(fun);
                        }
                        else
                        {
                            HtLimitDts htLimitDts = new HtLimitDts();
                            htLimitDts.MenuID = Convert.ToInt32(i);
                            htLimitDtslist.Add(htLimitDts);
                        }
                    }

                    if (funlist.Count > 0)
                    {
                        foreach (Fun fun in funlist)
                        {
                            var a = htLimitDtslist.Where(x => x.MenuID == fun.menuid).FirstOrDefault();
                            if (fun.funid == 0)
                            {
                                a.funAdd = 1;
                            }
                            else if (fun.funid == 1)
                            {
                                a.funDelete = 1;
                            }
                            else if (fun.funid == 2)
                            {
                                a.funSelect = 1;
                            }
                            else if (fun.funid == 3)
                            {
                                a.funUpdate = 1;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                string[] str = new string[2];
                str[0] = JsonConvert.SerializeObject(htLimit);
                str[1] = JsonConvert.SerializeObject(htLimitDtslist);


                if (client.LimitUpdate(str))
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
                client.ErrorlogAsync("Limit" + "|" + "权限更新_Update" + "|" + ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 编辑页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult LimitEdit(int id)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    if (Common.Common.CanRead("/Limit/LimitMain") != null)
                    {
                        string[] str = client.LimitEditData(id);
                        HtLimit htLimit = JsonConvert.DeserializeObject<HtLimit>(str[0]);
                        ViewData["rolename"] = htLimit.RoleName;
                        ViewData["common"] = htLimit.Common;
                        ViewData["limit_id"] = id;
                        List<HtLimitDts> limitDetaillist = JsonConvert.DeserializeObject<List<HtLimitDts>>(str[1]);
                        string value = "";
                        foreach (var i in limitDetaillist)
                        {
                            value += i.MenuID.ToString() + ",";
                            if (i.funAdd == 1)
                            {
                                value += i.MenuID.ToString() + "_" + 0 + ",";
                            }
                            if (i.funDelete == 1)
                            {
                                value += i.MenuID.ToString() + "_" + 1 + ",";
                            }
                            if (i.funSelect == 1)
                            {
                                value += i.MenuID.ToString() + "_" + 2 + ",";
                            }
                            if (i.funUpdate == 1)
                            {
                                value += i.MenuID.ToString() + "_" + 3 + ",";
                            }
                        }
                        ViewData["value"] = value;
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
                client.ErrorlogAsync("Limit" + "|" + "权限信息编辑_LimitEdit" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
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
                HtLimit htLimit = new HtLimit();
                if (Convert.ToInt32(fc["limit_role"]) == 0)
                {
                    return "请选择角色";
                }
                htLimit.RoleID = Convert.ToInt32(fc["limit_role"]);
                if (JsonConvert.DeserializeObject<List<HtLimit>>(client.LimitAll()).Where(x => x.RoleID == htLimit.RoleID).Count() > 0)
                {
                    return "该角色已经授予权限，请选择其他角色";
                }
                htLimit.Common = fc["limit_common"] == null ? "" : fc["limit_common"].ToString();
                menu = fc["menu_id"] == null ? "" : fc["menu_id"].ToString();
                if (menu == "")
                {
                    return "该角色没有授予任何权限，请为该角色选择相应的权限";
                }
                List<HtLimitDts> htLimitDtslist = new List<HtLimitDts>();

                if (!string.IsNullOrEmpty(menu))
                {
                    string[] menuids = menu.Remove(menu.Length - 1).Split(',');
                    List<Fun> funlist = new List<Fun>();
                    foreach (string i in menuids)
                    {
                        if (i.Contains("_"))
                        {
                            string[] a = i.Split('_');

                            Fun fun = new Fun();
                            fun.menuid = Convert.ToInt32(a[0]);
                            fun.funid = Convert.ToInt32(a[1]);
                            funlist.Add(fun);
                        }
                        else
                        {
                            HtLimitDts htLimitDts = new HtLimitDts();
                            htLimitDts.MenuID = Convert.ToInt32(i);
                            htLimitDtslist.Add(htLimitDts);
                        }
                    }

                    if (funlist.Count > 0)
                    {
                        foreach (Fun fun in funlist)
                        {
                            var a = htLimitDtslist.Where(x => x.MenuID == fun.menuid).FirstOrDefault();
                            if (fun.funid == 0)
                            {
                                a.funAdd = 1;
                            }
                            else if (fun.funid == 1)
                            {
                                a.funDelete = 1;
                            }
                            else if (fun.funid == 2)
                            {
                                a.funSelect = 1;
                            }
                            else if (fun.funid == 3)
                            {
                                a.funUpdate = 1;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                string str = JsonConvert.SerializeObject(htLimit);
                string str1 = JsonConvert.SerializeObject(htLimitDtslist);

                if (client.LimitAdd(str, str1))
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
                client.ErrorlogAsync("Limit" + "|" + "权限保存_Save" + "|" + ex.ToString());
                return "保存失败";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string LimitDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            return client.LimitDelete(i);
        }
    }
}