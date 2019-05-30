using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYMS.BSVIEW.MainService;


namespace FYMS.BSVIEW.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Login()
        {
            //CommonBase.loginID = 3;
            //CommonBase.loginName = "admin";
            //ViewBag.LoginState = "登陆前...";
            return View();
        }



        [HttpPost]//添加的Action中增加了一个[HttpPost] ，表示这个Action只会接受http post请求
        public ActionResult Login(FormCollection fc)
        {
            string email = fc["inputEmail3"];
            string password = fc["inputPassword3"];
            MainServiceClient mainsv = new MainServiceClient();
            ViewBag.Message = mainsv.login(email, password);
            return RedirectToAction("Index","Index");
        }
    }
}