using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ST { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int AccountDate { get; set; }
        [DataMember]
        public int AccountNUM { get; set; }

        public int number { get; set; }

    }
}
