//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Northwind.EF6Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            this.Customers = new HashSet<Customer>();
        }
    
        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
