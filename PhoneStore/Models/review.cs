//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhoneStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class review
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> phone_id { get; set; }
        public Nullable<int> rating { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual phone phone { get; set; }
        public virtual user user { get; set; }
    }
}
