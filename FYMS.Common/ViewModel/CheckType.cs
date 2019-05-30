using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class CheckType
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ST { get; set; }

        [DataMember]
        public string CheckName { get; set; }
        [DataMember]
        public string CheckCode { get; set; }

        public int number { get; set; }
    }

    [DataContract]
    public class CheckTypeJson
    {
        [DataMember]
        public string nodeid { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public state state { get; set; }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public List<CheckTypeJson> nodes { get; set; }
    }
}
