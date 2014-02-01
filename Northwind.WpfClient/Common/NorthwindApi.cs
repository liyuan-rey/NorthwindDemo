
namespace Northwind.WpfClient.Common
{
    public static class NorthwindApi
    {
        public static string StringBaseAddress { get; set; }
        private const string GetCategoryList = "category";

        public static string StringGetCategoryList
        {
            get { return StringBaseAddress + GetCategoryList; }
        }
    }
}
