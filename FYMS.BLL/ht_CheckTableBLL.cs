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

    }
}
