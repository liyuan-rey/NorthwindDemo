using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.WebApi2Services.Dto
{
    public class CategoryListItemDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte[] Picture { get; set; }
    }
}
