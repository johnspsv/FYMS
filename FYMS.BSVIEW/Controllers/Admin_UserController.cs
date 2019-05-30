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

        MainService.MainServiceClient client = new MainService.MainServiceClient();
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
            try
            {
                if (Common.Common.UserID > 0)
                {
                    if (Common.Common.CanRead("/Admin_User/Admin_UserMain") != null)
                    {
                        ViewData["add"] = Common.Common.CanRead("/Admin_User/Admin_UserMain").funAdd;
                        ViewData["delete"] = Common.Common.CanRead("/Admin_User/Admin_UserMain").funDelete;
                        ViewData["select"] = Common.Common.CanRead("/Admin_User/Admin_UserMain").funSelect;
                        ViewData["update"] = Common.Common.CanRead("/Admin_User/Admin_UserMain").funUpdate;


                        List<User_admin> list = new List<User_admin>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.AdminUserAll();
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
                            int pageSize = 10; 
                            int pageNumber = (page ?? 1);
                            return View(users.ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            IQueryable<User_admin> iq = null;
                            IPagedList<User_admin> pglist = new PagedList<User_admin>(iq, 1, 1);
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
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息查询_Admin_UserMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "人员信息查询_Admin_UserMain", "");
            }
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin_UserCreate()
        {
            if (Common.Common.UserID > 0)
            {
                if (Common.Common.CanRead("/Admin_User/Admin_UserMain") != null)
                {
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


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(FormCollection fc)
        {
            try
            {
                MainService.MainServiceClient client = new MainService.MainServiceClient();
                Common.ViewModel.User_admin user = new Common.ViewModel.User_admin();
                user.user_birthday = Convert.ToDateTime(fc["user_birthday"]);
                user.user_code = fc["user_code"].ToString();
                user.user_email = fc["user_email"].ToString();
                user.user_gender = Convert.ToInt32(fc["user_gender"]);
                user.user_idno = fc["user_idno"].ToString();
                user.user_name = fc["user_name"].ToString();
                user.user_password = Common.Common.GetMD5Str(fc["user_password"].ToString());
                user.user_phone = fc["user_phone"].ToString();
                user.user_photo = string.IsNullOrEmpty(picname) ? "" : picname;
                user.user_common = fc["user_commmon"] == null ? "" : fc["user_commmon"].ToString();

                string str = JsonConvert.SerializeObject(user).ToString();

                string i = client.Add(str);
                //ViewData["message"] = i;
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息保存_Save" + "|" + ex.ToString());
                return Content("保存失败");
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(User_admin user_Admin, FormCollection fc)
        {
            try
            {

                user_Admin.ID = Id;
                user_Admin.CT = CT;

                user_Admin.user_gender = Convert.ToInt32(fc["user_gender"]);
                string com = fc["user_commmon"] == null ? "" : fc["user_commmon"].ToString();
                user_Admin.user_common = com;
                user_Admin.user_photo = string.IsNullOrEmpty(picname) ? "" : picname;

                if (user_Admin.user_password != fc["user_password"].ToString())
                {
                    user_Admin.user_password = Common.Common.GetMD5Str(fc["user_password"].ToString());
                }
                string str = JsonConvert.SerializeObject(user_Admin).ToString();
                string i = BLL.ht_admin_userBLL.UserUpdate(str);
                //string i = client.Update(str);
                ViewData["message"] = i;
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息更新_Update" + "|" + ex.ToString());
                return Content("保存失败");
            }
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
                if (user_photo.ContentLength > 0)
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
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息更新_UpLoadFile" + "|" + ex.ToString());
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
            try
            {
                if (Common.Common.UserID > 0)
                {
                    if (Common.Common.CanRead("/Admin_User/Admin_UserMain") != null)
                    {
                        Id = id;
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
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息编辑_Admin_UserEdit" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin_UserDetails(int id)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    if (Common.Common.CanRead("/Admin_User/Admin_UserMain") != null)
                    {
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
                client.ErrorlogAsync("Admin_User" + "|" + "人员信息查看_Admin_UserDetails" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
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
            int i = Convert.ToInt32(id);
            return client.Delete(i);
        }

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public ActionResult PwModify(FormCollection fc)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {

                    string i = "";
                    User_admin user_Admin = JsonConvert.DeserializeObject<User_admin>(client.AdminByID(Common.Common.UserID));
                    string oldpassword = fc["oldpassword"] == null ? "" : fc["oldpassword"].ToString();
                    string newpassword = fc["newpassword"] == null ? "" : fc["newpassword"].ToString();
                    string conpassword = fc["conpassword"] == null ? "" : fc["conpassword"].ToString();
                    if (string.IsNullOrEmpty(oldpassword))
                    {
                        i = "请输入原密码！";
                    }
                    else if (oldpassword == newpassword)
                    {
                        i = "新密码不能与原密码一致";
                    }
                    else if (user_Admin.user_password != Common.Common.GetMD5Str(oldpassword))
                    {
                        i = "原密码输入错误！";
                    }
                    else if (newpassword != conpassword)
                    {
                        i = "新密码与旧密码不一致";
                    }
                    else if (newpassword == "")
                    {
                        i = "新密码不能为空";
                    }
                    else if (conpassword == "")
                    {
                        i = "确认密码不能为空";
                    }

                    else
                    {
                        user_Admin.user_password = Common.Common.GetMD5Str(newpassword);
                        string str = JsonConvert.SerializeObject(user_Admin);

                        i = client.Update(str);
                    }
                    return Content(i);
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }

            catch (Exception ex)
            {
                client.ErrorlogAsync("Admin_User" + "|" + "密码修改_PwModify" + "|" + ex.ToString());

                return Content("修改失败");
            }
        }
    }
}