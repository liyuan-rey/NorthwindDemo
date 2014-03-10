// UpdateCategoryDto.cs

namespace Northwind.WebApi2Services.Dto
{
    using System.Collections.Generic;

    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public List<string> UpdatedProperties { get; set; }
    }
}
