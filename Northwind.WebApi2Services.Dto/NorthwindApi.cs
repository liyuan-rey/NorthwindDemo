// NorthwindApi.cs

namespace Northwind.WebApi2Services.Dto
{
    public static class NorthwindApi
    {
        private const string GetCategoryList = "warehouse/category";
        private const string GetProductList = "Warehouse/product";

        public static string StringBaseAddress { get; set; }

        public static string StringGetCategoryList
        {
            get { return StringBaseAddress + GetCategoryList; }
        }

        public static string StringGetProductList
        {
            get { return StringBaseAddress + GetProductList; }
        }
    }
}