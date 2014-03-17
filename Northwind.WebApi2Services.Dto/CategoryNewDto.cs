// CategoryNewDto.cs

namespace Northwind.WebApi2Services.Dto
{
    public class CategoryNewDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}