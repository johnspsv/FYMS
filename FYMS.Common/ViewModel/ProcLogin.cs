using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class ProcLogin
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public Nullable<int> RoleID { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public Nullable<int> FloorID { get; set; }
        [DataMember]
        public Nullable<int> MenuID { get; set; }
        [DataMember]
        public Nullable<int> MenuFloor { get; set; }
        [DataMember]
        public string MenuPath { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public int funAdd { get; set; }
        [DataMember]
        public int funDelete { get; set; }
        [DataMember]
        public int funSelect { get; set; }
        [DataMember]
        public int funUpdate { get; set; }
    }
}
