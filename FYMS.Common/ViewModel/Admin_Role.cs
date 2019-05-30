using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class Admin_Role
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ST { get; set; }
        [DataMember]      
        public DateTime CT { get; set; }
        [DataMember]
        public string role_code { get; set; }
        [DataMember]
        public string role_name { get; set; }
        [DataMember]
        public string role_common { get; set; }

        public int number
        {
            get; set;
        }
    }
}
