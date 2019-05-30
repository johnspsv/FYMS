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
    public class MenuController : Controller
    {
        MainService.MainServiceClient client = new MainService.MainServiceClient();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        // GET: Menu
        public ActionResult MenuMain(string sortOrder, string searchString, string currentFilter, int? page)
        {

            List<HtMenu> list = new List<HtMenu>();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var i = client.MenuAll();
            
            List<DpMenu> dpmenulist = new List<DpMenu>();
            //var i = BLL.ht_admin_userBLL.All();

            if (i != null)
            {
                list = JsonConvert.DeserializeObject<List<HtMenu>>(i);
                var dplist = list.Where(x => x.FloorID == 0).ToList();
                foreach(var item in dplist)
                {
                    DpMenu dpMenu = new DpMenu();
                    dpMenu.ID = item.ID;
                    dpMenu.Name = item.Name;
                    dpmenulist.Add(dpMenu);
                }
                var users = from u in list select u;

                if (!string.IsNullOrEmpty(searchString))
                {
                    page = 1;
                    users = users.Where(u => u.Name.Contains(searchString));
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                switch (sortOrder)
                {
                    case "name_desc":
                        users = users.OrderByDescending(u => u.Name);
                        break;
                    default:
                        users = users.OrderBy(u => u.Name);
                        break;
                }
                int j = 1;
                foreach (var x in users)
                {
                    x.number = j++;
                }
                int pageSize = 50;
                int pageNumber = (page ?? 1);
                return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize),dpmenulist));
            }
            else
            {
                IQueryable<HtMenu> iq = null;
                IPagedList<HtMenu> pglist = new PagedList<HtMenu>(iq, 1, 1);
                list = null;
                return View(Tuple.Create(pglist,dpmenulist));
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
            HtMenu user = new HtMenu();
            user.Name = fc["crName"].ToString();
            user.Path = fc["crPath"].ToString();
            int count = fc["floorname"]!=null?Convert.ToInt32(fc["floorname"]):0;
            if (count > 0)
            {
                user.FloorID = count;
                List<HtMenu> menulist = JsonConvert.DeserializeObject<List<HtMenu>>(client.MenuAll());
                user.FloorName = (menulist.Where(x => x.ID == user.FloorID).FirstOrDefault()).Name;
                user.Floor = 2;
            }
            else
            {
                user.Floor = 1;
            }
            string str = JsonConvert.SerializeObject(user).ToString();
            string i = client.MenuAdd(str);
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
            HtMenu user = new HtMenu();
            user.ID = Convert.ToInt32(fc["editid"]);
            user.ST = 1;
            user.Name = fc["edName"].ToString();
            user.Path = fc["edPath"].ToString();
            int count = fc["edfloorname"] != null ? Convert.ToInt32(fc["edfloorname"]) : 0;
            if (count > 0)
            {
                user.FloorID = count;
                List<HtMenu> menulist = JsonConvert.DeserializeObject<List<HtMenu>>(client.MenuAll());
                user.FloorName = (menulist.Where(x => x.ID == user.FloorID).FirstOrDefault()).Name;
                user.Floor = 2;
            }
            else
            {
                user.Floor = 1;
            }
            string str = JsonConvert.SerializeObject(user).ToString();
            //string i = BLL.Ht_MenuBLL.MenuUpdate(str);
            string i = client.MenuUpdate(str);
            return Content(i);
        }

        /// <summary>
        /// 禁用 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string MenuDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            MainService.MainServiceClient client = new MainService.MainServiceClient();
            return client.MenuOpen(i);
        }
    }
}