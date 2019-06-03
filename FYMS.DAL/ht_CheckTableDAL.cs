using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.DAL
{
    public class ht_CheckTableDAL : PublicDAL.PubClass<ht_CheckTable>
    {
        /// <summary>
        /// 审核成功(带日志)
        /// </summary>
        /// <param name="companybean"></param>
        /// <param name="adminbean"></param>
        /// <param name="CheckTableID"></param>
        /// <param name="actDetailbean"></param>
        /// <returns></returns>
        public bool CheckTableSuccess(ct_Company companybean, ct_Admin adminbean,ht_CheckTable checkTablebean, ct_ActDetail actDetailbean, ht_control_log ht)
        {
            dbcontext.ct_Company.Add(companybean);

            adminbean.ct_Company = companybean;
            dbcontext.ct_Admin.Add(adminbean);

            var a = dbcontext.ht_CheckTable.Where(y => y.ID == checkTablebean.ID).FirstOrDefault();
            a.ST = 1;
            a.LT = checkTablebean.LT;
            a.LU = checkTablebean.LU;
            dbcontext.Set<ht_CheckTable>().Attach(a);
            dbcontext.Entry<ht_CheckTable>(a).State = EntityState.Modified;

            

            dbcontext.ht_control_log.Add(ht);

            var x = dbcontext.ct_Account.Where(y => y.ID == actDetailbean.AccountID).FirstOrDefault();
            actDetailbean.StartDate = DateTime.Now.ToString("yyyy-MM-dd");

            
            DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            double days =Convert.ToDouble(x.AccountDate);
            dt=dt.AddDays(days+1);

            actDetailbean.EndDate = dt.ToString("yyyy-MM-dd");
            actDetailbean.ct_Company = companybean;
            dbcontext.ct_ActDetail.Add(actDetailbean);


            dbcontext.SaveChanges();

            return true;
        }
    }
}
