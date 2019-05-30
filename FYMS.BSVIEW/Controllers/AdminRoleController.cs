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
    public class AdminRoleController : Controller
    {
        // GET: AdminRole

        /// <summary>
        /// 编辑时记录ID
        /// </summary>
        public static int Id;

        public static DateTime CT;

        MainService.MainServiceClient client = new MainService.MainServiceClient();
        /// <summary>
        /// 主页面
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AdminRoleMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            
            List<Admin_Role> list = new List<Admin_Role>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var i = client.AdminRoleAll();
            //var i = BLL.ht_admin_userBLL.All();

            if (i != null)
            {
                list = JsonConvert.DeserializeObject<List<Admin_Role>>(i);
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
                    
                    x.number = j++;
                }
                int pageSize = 50;
                int pageNumber = (page ?? 1);
                return View(users.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                IQueryable<Admin_Role> iq = null;
                IPagedList<Admin_Role> pglist = new PagedList<Admin_Role>(iq, 1, 1);
                list = null;
                return View(pglist);
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminRoleCreate()
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
            
            Admin_Role user = new Admin_Role();
            user.role_name = fc["role_name"].ToString();
            user.role_code = fc["role_code"].ToString();
            user.role_common = fc["role_common"].ToString();
            string str = JsonConvert.SerializeObject(user).ToString();
            string i = client.AdminRoleAdd(str);
            ViewData["message"] = i;
            return Content(i);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(Admin_Role admin_Role, FormCollection fc)
        {
            string i = "";
            try
            {
                admin_Role.ID = Id;
                admin_Role.CT = CT;
                admin_Role.role_common = fc["role_common"] == null ? "" : fc["role_common"].ToString();
                string str = JsonConvert.SerializeObject(admin_Role).ToString();
                i = client.AdminRoleUpdate(str);
                //ViewData["message"] = i;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Content(i);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AdminRoleEdit(int id)
        {
            Id = id;
            Admin_Role admin_Role = JsonConvert.DeserializeObject<Admin_Role>(client.AdminRoleByID(id));
            CT = admin_Role.CT;
            return View(admin_Role);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminRoleDetails(int id)
        {
            Admin_Role admin_Role = JsonConvert.DeserializeObject<Admin_Role>(client.AdminRoleByID(id));
            return View(admin_Role);
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
            return client.AdminRoleDelete(i);
        }
    }
}