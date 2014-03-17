// CategoryListItemDto.cs

namespace Northwind.WebApi2Services.Dto
{
    public class CategoryListItemDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public byte[] Picture { get; set; }
    }
}