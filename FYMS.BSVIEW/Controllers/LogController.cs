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
    public class LogController : Controller
    {
        // GET: Log

        MainService.MainServiceClient client = new MainService.MainServiceClient();


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="searchbean"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public ActionResult ErrorLogMain(SearchCon searchbean, int? page, string sortOrder)
        {

            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/ErrorLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<ErrorLog> list = new List<ErrorLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.ErrorLogAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<ErrorLog>>(i);
                            foreach (var a in list)
                            {
                                if (!string.IsNullOrEmpty(a.error_catch))
                                {
                                    string[] str = a.error_catch.Split('|');
                                    if (str.Length == 1)
                                    {
                                        if (str[0].Length > 20)
                                            a.errorcommon = str[0].Substring(0, 20) + "...";
                                        else
                                            a.errorcommon = str[0];
                                        a.errorfun = "";
                                        a.errorroute = "";
                                    }
                                    else
                                    {
                                        a.errorroute = str[0];
                                        a.errorfun = str[1];
                                        if (str[2].Length > 20)
                                            a.errorcommon = str[2].Substring(0, 20) + "...";
                                        else
                                            a.errorcommon = str[2];
                                    }
                                }
                            }



                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u =>  u.errorroute.Contains(searchbean.SearchString) || u.errorfun.Contains(searchbean.SearchString) || u.errorcommon.Contains(searchbean.SearchString));
                            }

                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                           
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }

                            int j = 1;

                            foreach (var x in users)
                            {
                                x.number = j++;
                            }

                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean));
                        }
                        else
                        {
                            IQueryable<ErrorLog> iq = null;
                            IPagedList<ErrorLog> pglist = new PagedList<ErrorLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean));
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
                client.ErrorlogAsync("Log" + "|" + "错误日志查询_ErrorLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "错误日志查询_ErrorLogMain", "");
            }
        }

        /// <summary>
        /// 错误日志查看明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ErrorLogMainDeatils(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/ErrorLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        ErrorLog errorLog = JsonConvert.DeserializeObject<List<ErrorLog>>(client.ErrorLogByID(ID)).FirstOrDefault();

                        string[] str = errorLog.error_catch.Split('|');
                        if (str.Length == 1)
                        {
                            errorLog.errorcommon = str[0];
                            errorLog.errorfun = "";
                            errorLog.errorroute = "";
                        }
                        else
                        {
                            errorLog.errorroute = str[0];
                            errorLog.errorfun = str[1];
                            errorLog.errorcommon = str[2];
                        }

                        return View(errorLog);
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
                client.ErrorlogAsync("Log" + "|" + "错误日志明细_ErrorLogMainDetails" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }

        }


        /// <summary>
        /// 后台登录日志
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminLogMain(SearchCon searchbean, int? page, string sortOrder)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/AdminLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<AdminLog> list = new List<AdminLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.AdminLogAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<AdminLog>>(i);

                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u => u.log_ip.Contains(searchbean.SearchString) || u.log_mac.Contains(searchbean.SearchString) || u.log_name.Contains(searchbean.SearchString));
                            }

                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                            //u.error_name.Contains(searchbean.SearchString)||u.errorroute.Contains(searchbean.SearchString)||u.errorfun.Contains(searchbean.SearchString)||u.errorcommon.Contains(searchbean.SearchString)).ToList();
                            {
                                page = 1;
                                users = users.Where(u => u.log_date >= searchbean.StartDate && u.log_date <= searchbean.EndDate);
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.log_date);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.log_date);
                                    break;
                            }

                            int j = 1;

                            foreach (var x in users)
                            {
                                x.number = j++;
                            }

                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean));
                        }
                        else
                        {
                            IQueryable<ErrorLog> iq = null;
                            IPagedList<ErrorLog> pglist = new PagedList<ErrorLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean));
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
                client.ErrorlogAsync("Log" + "|" + "后台登录日志查询_AdminLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "后台登录日志查询_AdminLogMain", "");
            }
        }


        /// <summary>
        /// 后台操作日志
        /// </summary>
        /// <returns></returns>
        public ActionResult ControlLogMain(SearchCon searchbean, int? page, string sortOrder)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/ControlLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;


                        List<ControlLog> list = new List<ControlLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.ControlLogAll();
                        if (i != null)
                        {
                            List<SelectListItem> itemList1 = new List<SelectListItem>()
                                                   {
                                                         new SelectListItem(){Text = "选择操作类型", Value = ""},
                                                         new SelectListItem() { Text = "新增", Value = "add" },
                                                         new SelectListItem() { Text = "删除", Value = "delete" },
                                                          new SelectListItem() { Text = "修改", Value = "modify" },
                                                         new SelectListItem() { Text = "查询", Value = "select" }
                                                    };
                            List<SelectListItem> itemList2 = new List<SelectListItem>()
                                                   {
                                                         new SelectListItem(){Text = "选择操作结果", Value = ""},
                                                         new SelectListItem() { Text = "操作成功", Value = "1" },
                                                         new SelectListItem() { Text = "操作失败", Value = "0" }
                                                    };

                            list = JsonConvert.DeserializeObject<List<ControlLog>>(i);
                            foreach (var a in list)
                            {
                                if (a.ctrl_bool == 0)
                                {
                                    a.ctrl_succ = "操作失败";
                                }
                                else
                                {
                                    a.ctrl_succ = "操作成功";
                                }
                                string[] str = a.ctrl_name.Split('|');
                                a.ctrlType = str[0] == null ? "" : str[0];
                                a.ctrlFun = str[1] == null ? "" : str[1];
                                a.user_name = a.user_name == null ? "" : a.user_name;
                                a.user_phone = a.user_phone == null ? "" : a.user_phone;
                                switch (a.ctrlType)
                                {
                                    case "add":
                                        a.ctrlTypeName = "新增";
                                        break;
                                    case "modify":
                                        a.ctrlTypeName = "修改";
                                        break;
                                    case "delete":
                                        a.ctrlTypeName = "删除";
                                        break;
                                    case "select":
                                        a.ctrlTypeName = "查询";
                                        break;
                                    default:
                                        a.ctrlTypeName = "其他";
                                        break;
                                }

                            }
                            var users = from u in list select u;
                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u => u.user_name.Contains(searchbean.SearchString) || u.user_phone.Contains(searchbean.SearchString) || u.ctrlFun.Contains(searchbean.SearchString));
                            }
                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)                          
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            if(!string.IsNullOrEmpty(searchbean.CtrlType))
                            {
                                page = 1;
                                users = users.Where(u => u.ctrlType==searchbean.CtrlType);
                            }
                            if(!string.IsNullOrEmpty(searchbean.CtrlResult))
                            {
                                page = 1;
                                users = users.Where(u => u.ctrl_bool == Convert.ToInt32(searchbean.CtrlResult));
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }
                            int j = 1;
                            foreach (var x in users)
                            {
                                x.number = j++;
                            }
                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean, itemList1, itemList2));
                        }
                        else
                        {
                            List<SelectListItem> list1 = new List<SelectListItem>();
                            List<SelectListItem> list2 = new List<SelectListItem>();
                            IQueryable<ErrorLog> iq = null;
                            IPagedList<ErrorLog> pglist = new PagedList<ErrorLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean, list1, list2));
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
                client.ErrorlogAsync("Log" + "|" + "后台操作日志查询_ControlLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "后台操作日志查询_ControlLogMain", "");
            }
        }

        /// <summary>
        /// 操作日志查看明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ControlLogDeatils(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/ErrorLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        ControlLog controlLog = JsonConvert.DeserializeObject<List<ControlLog>>(client.ControlLogByID(ID)).FirstOrDefault();

                        
                        if (controlLog.ctrl_bool == 0)
                        {
                            controlLog.ctrl_succ = "操作失败";
                        }
                        else
                        {
                            controlLog.ctrl_succ = "操作成功";
                        }
                        string[] str = controlLog.ctrl_name.Split('|');
                        controlLog.ctrlType = str[0] == null ? "" : str[0];
                        controlLog.ctrlFun = str[1] == null ? "" : str[1];
                        controlLog.ctrlData = str[2] == null ? "" : str[2];
                        controlLog.user_name = controlLog.user_name == null ? "" : controlLog.user_name;
                        controlLog.user_phone = controlLog.user_phone == null ? "" : controlLog.user_phone;
                        switch (controlLog.ctrlType)
                        {
                            case "add":
                                controlLog.ctrlTypeName = "新增";
                                break;
                            case "modify":
                                controlLog.ctrlTypeName = "修改";
                                break;
                            case "delete":
                                controlLog.ctrlTypeName = "删除";
                                break;
                            case "select":
                                controlLog.ctrlTypeName = "查询";
                                break;
                            default:
                                controlLog.ctrlTypeName = "其他";
                                break;
                        }

                        return View(controlLog);
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
                client.ErrorlogAsync("Log" + "|" + "操作日志明细_ControlLogDeatils" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }

        }


        /// <summary>
        /// 前台错误日志
        /// </summary>
        /// <param name="searchbean"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public ActionResult CTErrorLogMain(SearchCon searchbean, int? page, string sortOrder)
        {

            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/CTErrorLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<CTErrorLog> list = new List<CTErrorLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.CTErrorLogAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<CTErrorLog>>(i);
                            foreach (var a in list)
                            {
                                if (!string.IsNullOrEmpty(a.error_catch))
                                {
                                    string[] str = a.error_catch.Split('|');
                                    if (str.Length == 1)
                                    {
                                        if (str[0].Length > 20)
                                            a.errorcommon = str[0].Substring(0, 20) + "...";
                                        else
                                            a.errorcommon = str[0];
                                        a.errorfun = "";
                                        a.errorroute = "";
                                    }
                                    else
                                    {
                                        a.errorroute = str[0];
                                        a.errorfun = str[1];
                                        if (str[2].Length > 20)
                                            a.errorcommon = str[2].Substring(0, 20) + "...";
                                        else
                                            a.errorcommon = str[2];
                                    }
                                }
                            }



                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u =>u.CompanyName.Contains(searchbean.SearchString)|| u.errorfun.Contains(searchbean.SearchString) || u.errorroute.Contains(searchbean.SearchString) || u.errorfun.Contains(searchbean.SearchString) || u.errorcommon.Contains(searchbean.SearchString));
                            }

                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                            //u.error_name.Contains(searchbean.SearchString)||u.errorroute.Contains(searchbean.SearchString)||u.errorfun.Contains(searchbean.SearchString)||u.errorcommon.Contains(searchbean.SearchString)).ToList();
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }

                            int j = 1;

                            foreach (var x in users)
                            {
                                x.number = j++;
                            }

                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean));
                        }
                        else
                        {
                            IQueryable<CTErrorLog> iq = null;
                            IPagedList<CTErrorLog> pglist = new PagedList<CTErrorLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean));
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
                client.ErrorlogAsync("Log" + "|" + "前台错误日志查询_CTErrorLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "前台错误日志查询_CTErrorLogMain", "");
            }
        }

        /// <summary>
        /// 错误日志查看明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult CTErrorLogDeatils(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/CTErrorLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        CTErrorLog errorLog = JsonConvert.DeserializeObject<List<CTErrorLog>>(client.CTErrorLogByID(ID)).FirstOrDefault();

                        string[] str = errorLog.error_catch.Split('|');
                        if (str.Length == 1)
                        {
                            errorLog.errorcommon = str[0];
                            errorLog.errorfun = "";
                            errorLog.errorroute = "";
                        }
                        else
                        {
                            errorLog.errorroute = str[0];
                            errorLog.errorfun = str[1];
                            errorLog.errorcommon = str[2];
                        }

                        return View(errorLog);
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
                client.ErrorlogAsync("Log" + "|" + "前台错误日志明细_CTErrorLogDeatils" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }

        }

        /// <summary>
        /// 前台登录日志
        /// </summary>
        /// <returns></returns>
        public ActionResult CTLoginLogMain(SearchCon searchbean, int? page, string sortOrder)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/CTLoginLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<CTLoginLog> list = new List<CTLoginLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.CTLoginLogAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<CTLoginLog>>(i);

                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u =>u.companyName.Contains(searchbean.SearchString)|| u.log_ip.Contains(searchbean.SearchString) || u.log_mac.Contains(searchbean.SearchString) || u.log_name.Contains(searchbean.SearchString));
                            }

                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }

                            int j = 1;

                            foreach (var x in users)
                            {
                                x.number = j++;
                            }

                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean));
                        }
                        else
                        {
                            IQueryable<CTLoginLog> iq = null;
                            IPagedList<CTLoginLog> pglist = new PagedList<CTLoginLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean));
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
                client.ErrorlogAsync("Log" + "|" + "前台登录日志查询_CTLoginLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "前台登录日志查询_CTLoginLogMain", "");
            }
        }

        /// <summary>
        /// 前台操作日志
        /// </summary>
        /// <returns></returns>
        public ActionResult CTControlLogMain(SearchCon searchbean, int? page, string sortOrder)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/CTControlLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;


                        List<CTControlLog> list = new List<CTControlLog>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.CTControlLogAll();
                        if (i != null)
                        {
                            List<SelectListItem> itemList1 = new List<SelectListItem>()
                                                   {
                                                         new SelectListItem(){Text = "选择操作类型", Value = ""},
                                                         new SelectListItem() { Text = "新增", Value = "add" },
                                                         new SelectListItem() { Text = "删除", Value = "delete" },
                                                          new SelectListItem() { Text = "修改", Value = "modify" },
                                                         new SelectListItem() { Text = "查询", Value = "select" }
                                                    };
                            List<SelectListItem> itemList2 = new List<SelectListItem>()
                                                   {
                                                         new SelectListItem(){Text = "选择操作结果", Value = ""},
                                                         new SelectListItem() { Text = "操作成功", Value = "1" },
                                                         new SelectListItem() { Text = "操作失败", Value = "0" }
                                                    };

                            list = JsonConvert.DeserializeObject<List<CTControlLog>>(i);
                            foreach (var a in list)
                            {
                                if (a.ctrl_bool == 0)
                                {
                                    a.ctrl_succ = "操作失败";
                                }
                                else
                                {
                                    a.ctrl_succ = "操作成功";
                                }
                                string[] str = a.ctrl_name.Split('|');
                                a.ctrlType = str[0] == null ? "" : str[0];
                                a.ctrlFun = str[1] == null ? "" : str[1];
                                a.user_name = a.user_name == null ? "" : a.user_name;
                                a.user_phone = a.user_phone == null ? "" : a.user_phone;
                                switch (a.ctrlType)
                                {
                                    case "add":
                                        a.ctrlTypeName = "新增";
                                        break;
                                    case "modify":
                                        a.ctrlTypeName = "修改";
                                        break;
                                    case "delete":
                                        a.ctrlTypeName = "删除";
                                        break;
                                    case "select":
                                        a.ctrlTypeName = "查询";
                                        break;
                                    default:
                                        a.ctrlTypeName = "其他";
                                        break;
                                }

                            }
                            var users = from u in list select u;
                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u => u.user_name.Contains(searchbean.SearchString) || u.user_phone.Contains(searchbean.SearchString) || u.ctrlFun.Contains(searchbean.SearchString));
                            }
                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            if (!string.IsNullOrEmpty(searchbean.CtrlType))
                            {
                                page = 1;
                                users = users.Where(u => u.ctrlType == searchbean.CtrlType);
                            }
                            if (!string.IsNullOrEmpty(searchbean.CtrlResult))
                            {
                                page = 1;
                                users = users.Where(u => u.ctrl_bool == Convert.ToInt32(searchbean.CtrlResult));
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }
                            int j = 1;
                            foreach (var x in users)
                            {
                                x.number = j++;
                            }
                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean, itemList1, itemList2));
                        }
                        else
                        {
                            List<SelectListItem> list1 = new List<SelectListItem>();
                            List<SelectListItem> list2 = new List<SelectListItem>();
                            IQueryable<CTControlLog> iq = null;
                            IPagedList<CTControlLog> pglist = new PagedList<CTControlLog>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean, list1, list2));
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
                client.ErrorlogAsync("Log" + "|" + "前台操作日志查询_CTControlLogMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "前台操作日志查询_CTControlLogMain", "");
            }
        }

        /// <summary>
        /// 前台操作日志查看明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult CTControlLogDeatils(int ID)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Log/CTControlLogMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        CTControlLog controlLog = JsonConvert.DeserializeObject<List<CTControlLog>>(client.CTControlLogByID(ID)).FirstOrDefault();


                        if (controlLog.ctrl_bool == 0)
                        {
                            controlLog.ctrl_succ = "操作失败";
                        }
                        else
                        {
                            controlLog.ctrl_succ = "操作成功";
                        }
                        string[] str = controlLog.ctrl_name.Split('|');
                        controlLog.ctrlType = str[0] == null ? "" : str[0];
                        controlLog.ctrlFun = str[1] == null ? "" : str[1];
                        controlLog.ctrlData = str[2] == null ? "" : str[2];
                        controlLog.user_name = controlLog.user_name == null ? "" : controlLog.user_name;
                        controlLog.user_phone = controlLog.user_phone == null ? "" : controlLog.user_phone;
                        switch (controlLog.ctrlType)
                        {
                            case "add":
                                controlLog.ctrlTypeName = "新增";
                                break;
                            case "modify":
                                controlLog.ctrlTypeName = "修改";
                                break;
                            case "delete":
                                controlLog.ctrlTypeName = "删除";
                                break;
                            case "select":
                                controlLog.ctrlTypeName = "查询";
                                break;
                            default:
                                controlLog.ctrlTypeName = "其他";
                                break;
                        }

                        return View(controlLog);
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
                client.ErrorlogAsync("Log" + "|" + "前台操作日志明细_CTControlLogDeatils" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }

        }
    }
}