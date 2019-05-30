using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYMS.BSVIEW.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        /// <summary>
        /// 后台管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            if (Common.Common.UserID>-1)
            {
                ViewBag.Title = "首页";
                ViewData["UserName"] = Common.Common.UserName;
                return View();
            }
            else
            {
                return Redirect("/Login/Login");
            }
        }


        /// <summary>
        /// 404页
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorPage404()
        {
            return View();
        }
    }
}