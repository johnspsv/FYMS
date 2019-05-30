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
    public class AdminRoleRelController : Controller
    {
        // GET: AdminRoleRel
        MainService.MainServiceClient client = new MainService.MainServiceClient();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AdminRoleRelMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            
            List<AdminRoleRel> list = new List<AdminRoleRel>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";



            var i = client.AdminRoleRelAll();
            //var i = BLL.ht_userroleRelBLL.All();
            List<User_admin> userlist = JsonConvert.DeserializeObject<List<User_admin>>(client.AdminUserAll());
            List<Admin_Role> rolelist = JsonConvert.DeserializeObject<List<Admin_Role>>(client.AdminRoleAll());

            if (i != null)
            {
                list = JsonConvert.DeserializeObject<List<AdminRoleRel>>(i);
                var users = from u in list select u;

                if (!string.IsNullOrEmpty(searchString))
                {
                    page = 1;
                    users = users.Where(u => u.role_name.Contains(searchString));
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                switch (sortOrder)
                {
                    case "name_desc":
                        users = users.OrderByDescending(u => u.role_name);
                        break;
                    default:
                        users = users.OrderBy(u => u.role_name);
                        break;
                }
                int j = 1;
                foreach (var x in users)
                {
                    if (j == 1)
                    {
                        x.userlist = userlist;
                        x.rolelist = rolelist;
                    }
                    x.number = j++;
                }
                int pageSize = 50;
                int pageNumber = (page ?? 1);
                return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize),userlist,rolelist));
            }
            else
            {
                IQueryable<AdminRoleRel> iq = null;
                IPagedList<AdminRoleRel> pglist = new PagedList<AdminRoleRel>(iq, 1, 1);
                list = null;
                return View(Tuple.Create(pglist, userlist,rolelist));
            }
        }

        /// <summary>
        /// 人员关系创建
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminRoleRelCreate()
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
            AdminRoleRel rolerel = new AdminRoleRel();
            rolerel.admin_id  = Convert.ToInt32 (fc["username"]);
            rolerel.role_id = Convert.ToInt32(fc["rolename"]);
            List<User_admin> userlist = JsonConvert.DeserializeObject<List<User_admin>>(client.AdminUserAll());
            List<Admin_Role> rolelist = JsonConvert.DeserializeObject<List<Admin_Role>>(client.AdminRoleAll());
            var username = userlist.Where(x => x.ID == rolerel.admin_id).FirstOrDefault();
            rolerel.admin_name = username.user_name;
            var rolename = rolelist.Where(x => x.ID == rolerel.role_id).FirstOrDefault();
            rolerel.role_name = rolename.role_name;
            string str = JsonConvert.SerializeObject(rolerel).ToString();
            //string i = BLL.ht_userroleRelBLL.UserRoleRelAdd(str);
            string i = client.AdminRoleRelAdd(str);
            return Content(i);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection fc)
        {
            int id = Convert.ToInt32(fc["editid"]);
            AdminRoleRel rolerel = JsonConvert.DeserializeObject<AdminRoleRel>(client.AdminRoleRelEntity(id));
            rolerel.admin_id = Convert.ToInt32(fc["user_id"]);
            rolerel.role_id = Convert.ToInt32(fc["rolename"]);
            List<User_admin> userlist = JsonConvert.DeserializeObject<List<User_admin>>(client.AdminUserAll());
            List<Admin_Role> rolelist = JsonConvert.DeserializeObject<List<Admin_Role>>(client.AdminRoleAll());
            var username = userlist.Where(x => x.ID == rolerel.admin_id).FirstOrDefault();
            rolerel.admin_name = username.user_name;
            var rolename = rolelist.Where(x => x.ID == rolerel.role_id).FirstOrDefault();
            rolerel.role_name = rolename.role_name;
            string str = JsonConvert.SerializeObject(rolerel).ToString();
            //string i = BLL.ht_userroleRelBLL.UserRoleRelUpdate(str);
            string i = client.AdminRoleRelUpdate(str);
            return Content(i);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string RoleRelDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            return client.RoleRelOpen(i);
        }
    }
}