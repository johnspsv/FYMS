//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


[DataContract]
 public partial class ct_LimtDetail
{
    [DataMember]
	 public int ID { get; set; }
    [DataMember]
	 public int CU { get; set; }
    [DataMember]
	 public System.DateTime CT { get; set; }
    [DataMember]
	 public int LU { get; set; }
    [DataMember]
	 public System.DateTime LT { get; set; }
    [DataMember]
	 public Nullable<int> ST { get; set; }
    [DataMember]
	 public Nullable<int> LimitID { get; set; }
    [DataMember]
	 public Nullable<int> MenuID { get; set; }

    public virtual ct_Limit ct_Limit { get; set; }
    public virtual ct_Menu ct_Menu { get; set; }
}
