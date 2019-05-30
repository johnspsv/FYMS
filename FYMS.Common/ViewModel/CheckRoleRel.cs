using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class CheckRoleRel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ST { get; set; }
        [DataMember]
        public int CheckID { get; set; }
        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string CheckName { get; set; }

        public int number { get; set; }
    }


    public class CheckRole
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public int ST { get; set; }

        public int number { get; set; }
    }
}
