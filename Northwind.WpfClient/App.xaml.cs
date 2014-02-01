// App.xaml.cs

namespace Northwind.WpfClient
{
    using System.Configuration;
    using System.Windows;
    using Common;

    /// <summary>
    ///     App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NorthwindApi.StringBaseAddress = ConfigurationManager.AppSettings.Get("BaseAddress");
            if (string.IsNullOrWhiteSpace(NorthwindApi.StringBaseAddress))
                NorthwindApi.StringBaseAddress = @"http://localhost:52279/api/";
        }
    }
}