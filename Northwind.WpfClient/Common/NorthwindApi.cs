// NorthwindApi.cs

namespace Northwind.WpfClient.Common
{
    public static class NorthwindApi
    {
        private const string GetCategoryList = "category";
        public static string StringBaseAddress { get; set; }

        public static string StringGetCategoryList
        {
            get { return StringBaseAddress + GetCategoryList; }
        }
    }
}
