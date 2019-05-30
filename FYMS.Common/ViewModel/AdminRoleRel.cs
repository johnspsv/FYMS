using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class AdminRoleRel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public DateTime CT { get; set; }

        [DataMember]
        public int ST { get; set; }

        [DataMember]
        public int admin_id { get; set; }
        [DataMember]
        public string admin_name { get; set; }
        [DataMember]
        public int role_id { get; set; }
        [DataMember]
        public string role_name { get; set; }
        public int number
        {
            get; set;
        }

        public List<User_admin> userlist { get; set; }

        public List<Admin_Role> rolelist { get; set; }
    }
}
