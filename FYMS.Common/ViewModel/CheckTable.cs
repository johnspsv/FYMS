using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class CheckTable
    {
        [DataMember]
        public int ID { get; set; }


        public int ST { get; set; }

        
        public DateTime CT { get; set; }

        [DataMember]
        public int Common { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CompanyNo { get; set; }

        [DataMember]
        public string CompanyPhoto { get; set; }

        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public string mobilephone { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int gender { get; set; }

        public int number { get; set; }

        public string CheckName { get; set; }
    }


    public class CheckTableSearch
    {

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string SearchString { get; set; }

        public int? STvalue { get; set; }
    }

    public class CheckT
    {
        public int ID { get; set; }

        public DateTime CT { get; set; }

        public int ST { get; set; }

        public string Jsonstr { get; set; }

        public string Common { get; set; }
    }
}
