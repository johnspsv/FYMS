using FYMS.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.BLL
{
    public static class ht_CheckTableBLL
    {
        static ht_CheckTableDAL dal = new ht_CheckTableDAL();

        static PublicBLL.PubBll<ht_CheckTable> pubbll = new PublicBLL.PubBll<ht_CheckTable>();


        /// <summary>
        /// 审核数据表保存
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ht_CheckTableSave(string str)
        {
            ht_CheckTable ht_CheckTable = new ht_CheckTable();
            ht_CheckTable.CU = 0;
            ht_CheckTable.CT = DateTime.Now;
            ht_CheckTable.LT = DateTime.Now;
            ht_CheckTable.LU = 0;
            ht_CheckTable.ST = 2;
            ht_CheckTable.CheckID = 1;//1 是公司管理员审核
            ht_CheckTable.Jsonstr = str;

            if (dal.Add(ht_CheckTable))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获取人员审核数据
        /// </summary>
        /// <returns></returns>
        public static string CheckTableAll()
        {
            return JsonConvert.SerializeObject(dal.SelectEntities(x => x.CheckID == 1));
        }

        /// <summary>
        /// 获取人员审核的明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string CheckTableByID(int id)
        {
            return JsonConvert.SerializeObject(dal.SelectEntities(x => x.CheckID == 1 && x.ID == id));
        }

        /// <summary>
        /// 人员审核通过
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckTableSave(string str)
        {
            try
            {
                ct_Company company = JsonConvert.DeserializeObject<ct_Company>(str);
                company.CU = Common.Common.UserID;
                company.CT = DateTime.Now;
                company.LU = Common.Common.UserID;
                company.LT = DateTime.Now;
                company.ST = 1;
                company.ID = 0;

                ct_Admin admin = JsonConvert.DeserializeObject<ct_Admin>(str);
                admin.CU = Common.Common.UserID;
                admin.CT = DateTime.Now;
                admin.LU = Common.Common.UserID;
                admin.LT = DateTime.Now;
                admin.ST = 1;
                admin.ID = 0;

                ht_CheckTable checkTable = JsonConvert.DeserializeObject<ht_CheckTable>(str);
                checkTable.LT = DateTime.Now;
                checkTable.LU = Common.Common.UserID;

                ct_ActDetail actDetail = JsonConvert.DeserializeObject<ct_ActDetail>(str);
                actDetail.CT = DateTime.Now;
                actDetail.CU = Common.Common.UserID;
                actDetail.LU = Common.Common.UserID;
                actDetail.LT = DateTime.Now;
                actDetail.ST = 1;
                actDetail.ID = 0;



                if (dal.CheckTableSuccess(company, admin,checkTable, actDetail, LogBLL.controllog("add", "人员审核保存_CheckTableSave", str)))
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            catch(Exception ex)
            {
                LogBLL.errorControlLog("add", "人员审核保存_CheckTableSave", str, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckTableNoSuccess(int id)
        {
            try
            {
                var bean = dal.SelectEntities(x => x.ID == id).FirstOrDefault();
                bean.ST = 0;
                bean.LU = Common.Common.UserID;
                bean.LT = DateTime.Now;
                return dal.Update(bean);
            }
            catch (Exception ex)
            {
                LogBLL.errorControlLog("add", "人员审核保存_CheckTableSave", id.ToString() , ex.ToString());
                return false;
            }
        }
    }
}
