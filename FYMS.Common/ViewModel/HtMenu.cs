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
}
