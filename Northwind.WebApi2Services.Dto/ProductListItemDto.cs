// ProductListItemDto.cs

namespace Northwind.WebApi2Services.Dto
{
    public class ProductListItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool Discontinued { get; set; }
    }
}