using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FYMS.Common.ViewModel
{
    public class Log
    {
    }

    [DataContract]
    public class ErrorLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? CT { get; set; }

        [DataMember]
        public string error_name { get; set; }

        [DataMember]
        public string error_catch { get; set; }

        public string errorroute { get; set; }

        public string errorfun { get; set; }

        public string errorcommon { get; set; }
    }

    public class SearchCon
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string SearchString { get; set; }

        public string CtrlType { get; set; }

        public string CtrlResult { get; set; }


        public string DescTitle { get; set; }
    }

    [DataContract]
    public class AdminLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? log_date { get; set; }

        [DataMember]
        public string log_ip { get; set; }

        [DataMember]
        public string log_mac { get; set; }

        [DataMember]
        public string log_name { get; set; }
    }

    [DataContract]
    public class ControlLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? CT { get; set; }

        [DataMember]
        public int CU { get; set; }

        [DataMember]
        public string ctrl_name{ get; set; }

        [DataMember]
        public int  ctrl_bool { get; set; }

        [DataMember]
        public string ctrl_succ { get; set; }

        [DataMember]
        public string user_name { get; set; }

        [DataMember]
        public string user_phone { get; set; }

        [DataMember]
        public string ctrlType { get; set; }

        [DataMember]
        public string ctrlTypeName { get; set; }

        [DataMember]
        public string ctrlFun { get; set; }

        [DataMember]
        public string ctrlData { get; set; }
    }


    [DataContract]
    public class CTErrorLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? CT { get; set; }

        [DataMember]
        public string error_name { get; set; }

        [DataMember]
        public string CompanyName { get; set; }


        [DataMember]
        public string error_catch { get; set; }

        public string errorroute { get; set; }

        public string errorfun { get; set; }

        public string errorcommon { get; set; }
    }

    [DataContract]
    public class CTLoginLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? CT { get; set; }

        [DataMember]
        public string log_ip { get; set; }

        [DataMember]
        public string log_mac { get; set; }

        [DataMember]
        public string log_name { get; set; }

        [DataMember]
        public string companyName { get; set; }
    }


    [DataContract]
    public class CTControlLog
    {
        public int number;

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public DateTime? CT { get; set; }

        [DataMember]
        public int CU { get; set; }

        [DataMember]
        public string ctrl_name { get; set; }

        [DataMember]
        public int ctrl_bool { get; set; }

        [DataMember]
        public string ctrl_succ { get; set; }

        [DataMember]
        public string user_name { get; set; }

        [DataMember]
        public string user_phone { get; set; }

        [DataMember]
        public string ctrlType { get; set; }

        [DataMember]
        public string ctrlTypeName { get; set; }

        [DataMember]
        public string ctrlFun { get; set; }

        [DataMember]
        public string ctrlData { get; set; }

        [DataMember]
        public string userName { get; set; }

        [DataMember]
        public string CompanyName { get; set; }
    }
}
