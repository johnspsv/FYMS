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
    public class AdminCheckTableController : Controller
    {
        MainService.MainServiceClient client = new MainService.MainServiceClient();
        // GET: AdminCheckTable

        /// <summary>
        /// 人员审核
        /// </summary>
        /// <param name="searchbean"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public ActionResult AdminCheckTableMain(CheckTableSearch searchbean, int? page, string sortOrder)
        {
            try
            {
                if (Common.Common.UserID > 0)
                {
                    var look = Common.Common.CanRead("/AdminCheckTable/AdminCheckTableMain");
                    if (look != null)
                    {
                        ViewData["add"] = look.funAdd;
                        ViewData["delete"] = look.funDelete;
                        ViewData["select"] = look.funSelect;
                        ViewData["update"] = look.funUpdate;


                        List<CheckTable> list = new List<CheckTable>();
                        List<CheckT> tlist = new List<CheckT>();
                        ViewBag.CurrentSort = sortOrder;
                        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                        string i = BLL.ht_CheckTableBLL.CheckTableAll();

                        if (i != null)
                        {
                            List<SelectListItem> itemList1 = new List<SelectListItem>()
                                                   {
                                                         new SelectListItem(){Text = "选择审核状态", Value = ""},
                                                         new SelectListItem() { Text = "审核通过", Value = "1" },
                                                         new SelectListItem() { Text = "审核不通过", Value = "0" },
                                                          new SelectListItem() { Text = "待审核", Value = "2" }
                                                    };

                            tlist = JsonConvert.DeserializeObject<List<CheckT>>(i);
                            foreach (var a in tlist)
                            {
                                CheckTable checkTable = new CheckTable();
                                checkTable = JsonConvert.DeserializeObject<CheckTable>(a.Jsonstr);
                                checkTable.ID = a.ID;
                                checkTable.ST = a.ST;
                                checkTable.CT = a.CT;
                                if (checkTable.ST == 0)
                                    checkTable.CheckName = "审核不通过";
                                else if (checkTable.ST == 1)
                                    checkTable.CheckName = "审核通过";
                                else
                                    checkTable.CheckName = "待审核";
                                list.Add(checkTable);
                            }

                            var users = from u in list select u;

                            if (!string.IsNullOrEmpty(searchbean.SearchString))
                            {
                                page = 1;
                                users = users.Where(u => u.CompanyName.Contains(searchbean.SearchString) || u.name.Contains(searchbean.SearchString) || u.mobilephone.Contains(searchbean.SearchString));
                            }

                            if (searchbean.StartDate != null && searchbean.EndDate != null && searchbean.StartDate < searchbean.EndDate)
                            {
                                page = 1;
                                users = users.Where(u => u.CT >= searchbean.StartDate && u.CT <= searchbean.EndDate);
                            }
                            if (searchbean.STvalue != null)
                            {
                                page = 1;
                                users = users.Where(u => u.ID == searchbean.STvalue);
                            }
                            switch (sortOrder)
                            {
                                case "name_desc":
                                    users = users.OrderByDescending(u => u.CT);
                                    break;
                                default:
                                    users = users.OrderBy(u => u.CT);
                                    break;
                            }

                            int j = 1;

                            foreach (var x in users)
                            {
                                x.number = j++;
                            }

                            int pageSize = 50;
                            int pageNumber = (page ?? 1);
                            return View(Tuple.Create(users.ToPagedList(pageNumber, pageSize), searchbean, itemList1));
                        }
                        else
                        {
                            IQueryable<CheckTable> iq = null;
                            IPagedList<CheckTable> pglist = new PagedList<CheckTable>(iq, 1, 1);
                            list = null;
                            return View(Tuple.Create(pglist, searchbean));
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
                client.ErrorlogAsync("Log" + "|" + "人员审核查询_AdminCheckTableMain" + "|" + ex.ToString());
                return Redirect("/Index/ErrorPage404");
            }
            finally
            {
                client.controllog("select", "人员审核查询_AdminCheckTableMain", "");
            }
        }



        /// <summary>
        /// 人员审核查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult AdminCheckTableDeatils(int ID)
        {
            return View();
        }
        

        /// <summary>
        /// 人员审核查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult AdminCheckTableCheck(int ID)
        {
            return View();
        }
    }
}