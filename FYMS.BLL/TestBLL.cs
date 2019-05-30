using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYMS.DAL;
using Newtonsoft.Json;

namespace FYMS.BLL
{
    public class TestBLL:PublicBLL.PubBll<Test>
    {
        TestDAL dal = new TestDAL();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="testEntitys"></param>
        /// <returns></returns>
        public bool Add(string testEntitys)
        {
            return dal.Add(base.ReturnEntity(testEntitys));
        }
    }
}
