using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class HtMenu
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ST { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Floor { get; set; }
        [DataMember]
        public int FloorID { get; set; }
        [DataMember]
        public string Path { get; set; }
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

        public int number { get; set; }

        public List<DpMenu> Dplist { get; set; }
    }

    public class DpMenu
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class HtMenuJson
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
        public List<HtMenuJson> nodes { get; set; }
    }

    

    public class state
    {
        public bool @checked{get;set;}
    }
}
