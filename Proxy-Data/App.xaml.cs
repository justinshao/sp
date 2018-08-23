using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PlatformServers
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!e.Args.Contains("started_by_updater"))
            {
                MessageBox.Show("请使用更新程序启动。");
                Shutdown();
            }
        }
    }
}
