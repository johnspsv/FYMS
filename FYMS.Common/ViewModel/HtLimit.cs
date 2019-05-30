using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class HtLimit
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ST { get; set; }
        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        public int MenuID { get; set; }
        [DataMember]
        public string Common { get; set; }
        [DataMember]
        public string RoleName { get; set; }

        public int number { get; set; }

        public List<Admin_Role> rolelist { get; set; }

        public List<HtMenu> menulist { get; set; }
    }


    [DataContract]
    public class HtLimitDts
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int LimitID { get; set; }
        [DataMember]
        public int MenuID { get; set; }
        [DataMember]
        public int funAdd { get; set; }
        [DataMember]
        public int funDelete { get; set; }
        [DataMember]
        public int funSelect { get; set; }
        [DataMember]
        public int funUpdate { get; set; }
    }

    public class Fun
    {
        public int menuid { get; set; }

        public int funid{ get; set; }
       
    }

}
