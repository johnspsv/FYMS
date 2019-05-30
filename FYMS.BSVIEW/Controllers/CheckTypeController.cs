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
    public class CheckTypeController : Controller
    {

        MainService.MainServiceClient client = new MainService.MainServiceClient();
        // GET: CheckType
        
        /// <summary>
        /// 审核类型
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckTypeMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {

                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/CheckType/CheckTypeMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;

                        List<CheckType> list = new List<CheckType>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

                        var i = client.CheckTypeAll();

                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<CheckType>>(i);

                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchString))
                            {
                                page = 1;
                                users = users.Where(u => u.CheckName.Contains(searchString));
                            }
                            else
                            {
                                searchString = currentFilter;
                            }
                            ViewBag.CurrentFilter = searchString;

                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CheckName);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CheckName);
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
                            IQueryable<CheckType> iq = null;
                            IPagedList<CheckType> pglist = new PagedList<CheckType>(iq, 1, 1);
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
                client.ErrorlogAsync("CheckType" + "," + "审核类型查询_CheckTypeMain" + "," + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "审核类型查询_CheckTypeMain", "");
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
                CheckType checkType = new CheckType();
                checkType.CheckName = fc["cr_CheckName"].ToString();
                checkType.CheckCode = fc["cr_CheckCode"].ToString();
                string str = JsonConvert.SerializeObject(checkType).ToString();
                string i = client.CheckTypeAdd(str);
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckType," + "审核类型保存_Save" + ex.ToString());
                throw ex;
                //return Redirect("/Index/ErrorPage404");
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection fc)
        {
            try
            {
                CheckType checkType = new CheckType();
                checkType.ID = Convert.ToInt32(fc["editid"]);
                checkType.ST = 1;
                checkType.CheckName = fc["ed_CheckName"].ToString();
                checkType.CheckCode = fc["ed_CheckCode"].ToString();
                string str = JsonConvert.SerializeObject(checkType).ToString();
                string i = client.CheckTypeUpdate(str);
                //string i = client.MenuUpdate(str);
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("CheckType," + "审核类型更新_Update" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 禁用 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckTypeDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            return client.CheckTypeDelete(i);
        }
    }
}