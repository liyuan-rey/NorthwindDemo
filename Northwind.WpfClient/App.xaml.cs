// App.xaml.cs

namespace Northwind.WpfClient
{
    using System;
    using System.Configuration;
    using System.Windows;
    using System.Windows.Threading;
    using Northwind.WpfClient.Common;
    using Northwind.WpfClient.Properties;

    /// <summary>
    ///     App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            try
            {
                DispatcherUnhandledException += App_DispatcherUnhandledException;

                NorthwindApi.StringBaseAddress = Settings.Default.BaseAddress;
                if (string.IsNullOrWhiteSpace(NorthwindApi.StringBaseAddress))
                    throw new ConfigurationErrorsException("无效的 BaseAddress 配置数据.");

                //this.StartupUri = new Uri("MainWindow.xaml");
                //this.MainWindow = new MainWindow();
                //this.MainWindow.Show();
            }
            catch (Exception exception)
            {
                ReportError(exception);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ReportError(e.Exception);
            e.Handled = true;
        }

        private void ReportError(Exception exception)
        {
            string msg = string.Format("应用程序遇到以下错误, 可能无法正常运行:{0}{0}{1}",
                Environment.NewLine, exception);

            MessageBox.Show(msg, "警告", MessageBoxButton.OK,
                MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
