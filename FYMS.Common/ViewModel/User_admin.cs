using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class User_admin
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime CT { get; set; }
        [DataMember]
        public string user_code { get; set; }
        [DataMember]
        public string user_email { get; set; }
        [DataMember]
        public string user_phone { get; set; }
        [DataMember]
        public string user_password { get; set; }
        [DataMember]
        public string user_name { get; set; }
        [DataMember]
        public string user_idno { get; set; }
        [DataMember]
        public DateTime user_birthday { get; set; }
        [DataMember]
        public string user_common { get; set; }
        [DataMember]
        public string user_photo { get; set; }
        [DataMember]
        public int user_gender { get; set; }

        public string picname
        {
            get;set;
        }
        
        public int number
        {
            get ;set;
        }

        public string confirmpassword
        {
            get;set;
        }

    }
}
