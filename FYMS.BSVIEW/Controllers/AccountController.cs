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
    public class AccountController : Controller
    {
        // GET: Account
        MainService.MainServiceClient client = new MainService.MainServiceClient();

        /// <summary>
        /// 账号类型
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult CTAccountMain(string sortOrder, string searchString, string currentFilter, int? page)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/Account/CTAccountMain");
                    if (look != null)
                    {
                        ViewData["add"] =look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;


                        List<Account> list = new List<Account>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = client.AccountTypeAll();
                     
                        if (i != null)
                        {
                            list = JsonConvert.DeserializeObject<List<Account>>(i);
                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchString))
                            {
                                page = 1;
                                users = users.Where(u => u.AccountName.Contains(searchString));
                            }
                            else
                            {
                                searchString = currentFilter;
                            }
                            ViewBag.CurrentFilter = searchString;

                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.AccountName);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.AccountName);
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
                            IQueryable<Account> iq = null;
                            IPagedList<Account> pglist = new PagedList<Account>(iq, 1, 1);
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
                client.ErrorlogAsync("Account" + "|" + "账号类型查询_CTAccountMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "账号类型查询_CTAccountMain", "");
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
                Account account = new Account();
                account.AccountName = fc["cr_AccountName"].ToString();
                account.AccountDate =Convert.ToInt32(fc["cr_AccountDate"]);
                account.AccountNUM = Convert.ToInt32(fc["cr_AccountNUM"]);
                string str = JsonConvert.SerializeObject(account).ToString();
                string i = client.AccountTypeAdd(str); 
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Account" + "账号类型保存_Save" + ex.ToString());
                throw ex;
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
                Account account = new Account();
                account.ID = Convert.ToInt32(fc["editid"]);
                account.ST = 1;
                account.AccountName = fc["ed_AccountName"].ToString();
                account.AccountDate = Convert.ToInt32(fc["ed_AccountDate"]);
                account.AccountNUM = Convert.ToInt32(fc["ed_AccountNUM"]);
                string str = JsonConvert.SerializeObject(account).ToString();
                string i = client.AccountTypeUpdate(str);
           
                return Content(i);
            }
            catch (Exception ex)
            {
                client.ErrorlogAsync("Account" + "账号类型更新_Update" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 禁用 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string AccountTypeDelete()
        {
            string id = Request["id"].ToString();
            int i = Convert.ToInt32(id);
            return client.AccountTypeDelete(i);
        }
    }
}