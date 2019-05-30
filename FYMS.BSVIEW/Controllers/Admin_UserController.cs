using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using FYMS.Common.ViewModel;
using System.IO;

namespace FYMS.BSVIEW.Controllers
{
    public class Admin_UserController : Controller
    {
        /// <summary>
        /// 记录上传图片名
        /// </summary>
        public static string picname;

        /// <summary>
        /// 编辑时记录ID
        /// </summary>
        public static int Id;

        public static DateTime CT;
        /// <summary>
        /// 排序，分页，搜索
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Admin_UserMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            List<User_admin> list = new List<User_admin>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var i = client.AdminUserAll();
            //var i = BLL.ht_admin_userBLL.All();

            if (i != null)
            {
                list = JsonConvert.DeserializeObject<List<User_admin>>(i);
                var users = from u in list select u;
                
                if (!string.IsNullOrEmpty(searchString))
                {
                    page = 1;
                    users = users.Where(u => u.user_name.Contains(searchString));
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                switch (sortOrder)
                {
                    case "name_desc":
                        users = users.OrderByDescending(u => u.user_name);
                        break;
                    default:
                        users = users.OrderBy(u => u.user_name);
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
                IQueryable<User_admin> iq = null;
                IPagedList<User_admin> pglist = new PagedList<User_admin>(iq,1,1);
                list = null;
                return View(pglist);
            }  
        }



        public ActionResult Admin_UserCreate()
        {
            return View();
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(FormCollection fc)
        {
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            Common.ViewModel.User_admin user = new Common.ViewModel.User_admin();
            user.user_birthday = Convert.ToDateTime(fc["user_birthday"]);
            user.user_code = fc["user_code"].ToString();
            user.user_email = fc["user_email"].ToString();
            user.user_gender = Convert.ToInt32(fc["user_gender"]);
            user.user_idno = fc["user_idno"].ToString();
            user.user_name = fc["user_name"].ToString();
            user.user_password =Common.Common.GetMD5Str(fc["user_password"].ToString());
            user.user_phone = fc["user_phone"].ToString();
            user.user_photo = string.IsNullOrEmpty(picname) ? "":picname ;
            user.user_common = fc["user_commmon"] == null ? "" : fc["user_commmon"].ToString();

            string str = JsonConvert.SerializeObject(user).ToString();

            string i = client.Add(str); 
            //ViewData["message"] = i;
            return Content(i);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(User_admin user_Admin, FormCollection fc)
        {
            
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            user_Admin.ID = Id;
            user_Admin.CT = CT;
            user_Admin.user_gender= Convert.ToInt32(fc["user_gender"]);
            string com = fc["user_commmon"]==null?"": fc["user_commmon"].ToString();
            user_Admin.user_common=com;
            user_Admin.user_photo= string.IsNullOrEmpty(picname) ? "" : picname;
        
            if (user_Admin.user_password!= fc["user_password"].ToString())
            {
                user_Admin.user_password= Common.Common.GetMD5Str(fc["user_password"].ToString());
            }
            string str = JsonConvert.SerializeObject(user_Admin).ToString();
            string i= BLL.ht_admin_userBLL.UserUpdate(str);
            //string i = client.Update(str);
            ViewData["message"] = i;
            return Content(i);
        }


        /// <summary>
        /// 上传单一文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpLoadFile(HttpPostedFileBase user_photo)
        {
            string json = "{\"msg\":\"上传成功!\"}";

            try
            {
                if(user_photo.ContentLength>0)
                {
                    
                    string name = DateTime.Now.ToString("yyyymmddhhmmss");

                    var filename = Path.GetFileName(user_photo.FileName);
                    if (filename.Contains(".jpg"))
                        name += ".jpg";
                    else if (filename.Contains(".png"))
                        name += ".png";
                    else
                        name += ".gif";
                    var path = Path.Combine(Server.MapPath("~/Temp"), name);
                    
                    user_photo.SaveAs(path);
                    picname = name;
                }
                return json;
            }
            catch (Exception ex)
            {
                //失败时返回的参数必须加 error键
                json = "{\"error\":\"" + ex.Message + "\"}";
                return json;
            }
           
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Admin_UserEdit(int id)
        {
            Id = id;
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            User_admin user_Admin = JsonConvert.DeserializeObject<User_admin>(client.AdminByID(id));
            if (string.IsNullOrEmpty(user_Admin.user_photo))
            {
                user_Admin.picname = "name.png";
            }
            else
            {
                user_Admin.picname = user_Admin.user_photo;
            }
            CT = user_Admin.CT;
            user_Admin.confirmpassword = user_Admin.user_password;
            return View(user_Admin);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin_UserDetails(int id)
        {
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            User_admin user_Admin = JsonConvert.DeserializeObject<User_admin>(client.AdminByID(id));
            if (string.IsNullOrEmpty(user_Admin.user_photo))
            {
                user_Admin.picname = "name.png";
            }
            else
            {
                user_Admin.picname = user_Admin.user_photo;
            }
            
            return View(user_Admin);
        }

       
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete()
        {
            string id = Request["id"].ToString();
            int i =Convert.ToInt32(id); 
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            return client.Delete(i);
        }
    }
}