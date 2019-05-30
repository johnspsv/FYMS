using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYMS.BSVIEW.MainService;
using FYMS.Common.ViewModel;
using Newtonsoft.Json;

namespace FYMS.BSVIEW.Controllers
{
    public class LoginController : Controller
    {
        MainServiceClient mainsv = new MainServiceClient();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]//添加的Action中增加了一个[HttpPost] ，表示这个Action只会接受http post请求
        public ActionResult Login(FormCollection fc)
        {
            try
            {
                string username = fc["inputEmail3"];
                string password = Common.Common.GetMD5Str(fc["inputPassword3"].ToString());
                string[] str = mainsv.login(username, password);
                if (username == "admin"&& str[0]!=null)
                {
                    User_admin entity = JsonConvert.DeserializeObject<User_admin>(str[0]);
                    if (entity != null)
                    {
                        Common.Common.UserID = entity.ID;
                        Common.Common.UserName = entity.user_name;
                        Common.Common.RoleID = 0;
                        Common.Common.RoleName = "admin";
                        Common.Common.menulist = new List<HtMenu>();
                        foreach (var item in JsonConvert.DeserializeObject<List<HtMenu>>(str[1]))
                        {
                            HtMenu htMenu = new HtMenu();
                            htMenu.ID = item.ID;
                            htMenu.Name = item.Name;
                            htMenu.Path = item.Path;
                            htMenu.Floor = item.Floor;
                            htMenu.FloorID = item.FloorID;
                            htMenu.FloorName = item.FloorName;
                            htMenu.funAdd = 1;
                            htMenu.funDelete = 1;
                            htMenu.funSelect = 1;
                            htMenu.funUpdate = 1;
                            Common.Common.menulist.Add(htMenu);
                        }
                        string[] strarry = new string[2];
                        strarry[0] = Common.ComputerInfo.IpAddress;
                        strarry[1] = Common.ComputerInfo.MacAddress;
                        mainsv.loginlogAsync(strarry);
                        return Content("1");
                    }
                    else
                    {                      
                        return Content("0");
                    }
                }
                else
                {
                    if (str[0] != null)
                    {
                        List<ProcLogin> list1 = JsonConvert.DeserializeObject<List<ProcLogin>>(str[0]);
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
                                htMenu.funAdd = item.funAdd;
                                htMenu.funDelete = item.funDelete;
                                htMenu.funSelect = item.funSelect;
                                htMenu.funUpdate = item.funUpdate;
                                Common.Common.menulist.Add(htMenu);
                            }
                            string[] strarry = new string[2];
                            strarry[0] = Common.ComputerInfo.IpAddress;
                            strarry[1] = Common.ComputerInfo.MacAddress;
                            mainsv.loginlogAsync(strarry);

                            return Content("1");
                        }
                        else
                        {                          
                            return Content("0");

                        }
                    }
                    else
                    {
                        return Content("0");

                    }
                }
            }
            catch(Exception ex)
            {
                mainsv.ErrorlogAsync(ex.ToString());
                ViewData["default"] = 1;
                ViewData["tips"] = "登录失败！error...";
                return Content("0");
            }
        }

        [HttpPost]
        /// <summary>
        /// 根据人员动态显示导航栏
        /// </summary>
        /// <returns></returns>
        public string Menulist()
        {
            string strhtml = "<ul class='nav nav-sidebar'  id='colul'>" +
                "<li><a href ='/Index/Index' ><span class='glyphicon glyphicon-th-list'>后台首页</span></a></li>";
            List<HtMenu> parentlist = Common.Common.menulist.Where(x => x.FloorID == 0).ToList();
            foreach (var i in parentlist)
            {
                if (Common.Common.menulist.Where(x => x.FloorID == i.ID).ToList().Count > 0)
                {
                    strhtml += "<li>" +
                                "<a href = '" + i.Path + i.ID + "'data-parent = '#colul' data-toggle = 'collapse'>" +
                                 "<span class='glyphicon glyphicon-th-list'>" + i.Name + "</span><b class='caret'></b>" +
                               "</a>" +
                               "<div id = '" + i.ID + "' class='panel-group collapse'>" +
                                "<ul class='nav navbar-collapse'>";
                    foreach (var j in Common.Common.menulist.Where(x => x.FloorID == i.ID).ToList())
                    {
                        strhtml += "<li><a href ='" + j.Path + "'>" +
                            "<span class='glyphicon glyphicon-th-list'>" + j.Name + "</span></a></li>";
                    }
                    strhtml += "</ul>" +
                            "</div>" +
                        "</li>";
                }
                else
                {
                    strhtml += "<li><a href ='" + i.Path + "'><span class='glyphicon glyphicon-th-list'>" + i.Name + "</span></a></li>";
                }
            }
            strhtml += "</ul>";
            return strhtml;
        }

        [HttpPost]
        /// <summary>
        /// 根据人员动态显示导航栏
        /// </summary>
        /// <returns></returns>
        public string MenuPlist()
        {
            string strhtml = "<ul class='nav nav-sidebar'  id='pcolul'>" +
                "<li><a href ='/Index/Index' ><span class='glyphicon glyphicon-th-list'>后台首页</span></a></li>";
            List<HtMenu> parentlist = Common.Common.menulist.Where(x => x.FloorID == 0).ToList();
            foreach (var i in parentlist)
            {
                if (Common.Common.menulist.Where(x => x.FloorID == i.ID).ToList().Count > 0)
                {
                    strhtml += "<li>" +
                                "<a href = '" + i.Path + "p" + i.ID + "' data-parent = '#pcolul' data-toggle = 'collapse'>" +
                                 "<span class='glyphicon glyphicon-th-list'>" + i.Name + "</span><b class='caret'></b>" +
                               "</a></li>" +
                               "<div id = '" + "p" + i.ID + "' class='panel-group collapse'>" +
                                "<ul class='nav navbar-collapse'>";
                    foreach (var j in Common.Common.menulist.Where(x => x.FloorID == i.ID).ToList())
                    {
                        strhtml += "<li><a href ='" + j.Path + "'><span class='glyphicon glyphicon-th-list'>" + j.Name + "</span></a></li>";
                    }
                    strhtml += "</ul>" +
                            "</div>" +
                           "</li>";
                    ;
                }
                else
                {
                    strhtml += "<li><a href ='" + i.Path + "' ><span class='glyphicon glyphicon-th-list'>" + i.Name + "</span></a></li>";
                }
            }
            strhtml += "</ul>";
            return strhtml;
        }

        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string baseinfo()
        {
            string strhtml = "<li><a href='#'><span class='glyphicon glyphicon-user'>" + Common.Common.UserName + "</span></a></li>" +
                    "<li><a href ='#' ><span class='glyphicon glyphicon-tag'>" + Common.Common.RoleName + "</span></a></li>" +
                    "<li><a id='pwedit' href ='#' data-toggle='modal' data-target='#pwmodify'><span class='glyphicon glyphicon-lock'>修改密码</span></a></li>" +
                    "<li><a href ='#' ><span class='glyphicon glyphicon-time' id='clock'></span></a></li>" +
                    "<li><a href ='#' id='btnreload' onclick='relogin()'><span class='glyphicon glyphicon-share-alt'>重新登录</span></a></li>" +
                    "<li><a href ='#' id='btnclose' onclick='pageclose()'><span class='glyphicon glyphicon-off'>关闭页面</span></a></li>";
            return strhtml;
        }
    }
}