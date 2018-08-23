using Common;
using Common.Core;
using Common.Core.Helpers;
using Common.DataAccess;
using Common.Entities;
using Server.Common;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace PlatformServers
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ObservableCollection<ExtendAssembly> ExtendAssemblys = new ObservableCollection<ExtendAssembly>();
        Logger logger = LogManager.GetLogger("主程序");
        /// <summary>
        ///是否只生成Http服务
        /// </summary>
        private bool _isBasicHttpOnly = true;

        /// <summary>
        /// BasicHttp服务端口
        /// </summary>
        private int _basicHttpPort = 80;

        /// <summary>
        /// Net.Tcp服务端口
        /// </summary>
        private int _netTcpPort = 4508;

        /// <summary>
        /// 本地服务器IP
        /// </summary>
        private string _ServerIP = "";

        /// <summary>
        /// 托盘控件
        /// </summary>
        private NotifyIcon _notifyIcon = new NotifyIcon();

        /// <summary>
        /// 记录窗口最小化前的状态
        /// </summary>
        private WindowState _windowState;

        private readonly ObservableCollection<PlatformWcfServers.WXClient> PlatformClientes = new ObservableCollection<PlatformWcfServers.WXClient>();

        private bool closeWithCloseButton = true;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //设置当前目录为应用程序目录
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                //更改系统时间格式
                SystemInfo.SetDateTimeFormat();
                //启动Socket策略服务
                //PolicyServer server = new PolicyServer();
                //启动Net.Tcp绑定时需要的端口共享服务
                StartNetTcpPortSharingService();
            }
            catch (Exception ex)
            {
                logger.Fatal("初始化异常", ex);
            }
        }

        void StartNetTcpPortSharingService()
        {
            try
            {
                string serviceName = "NetTcpPortSharing";
                if (ServiceControllerEx.GetServices().Any(sv => sv.ServiceName == serviceName))
                {
                    ServiceControllerEx sc = new ServiceControllerEx(serviceName);
                    if (sc.StartupType != "Automatic")
                    {
                        sc.StartupType = "Automatic";
                    }
                    if ((sc.Status.Equals(ServiceControllerStatus.Stopped)) || (sc.Status.Equals(ServiceControllerStatus.StopPending)))
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                        sc.Close();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("服务NetTcpPortSharing不存在,部分服务将无法启动!");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("打开端口共享服务异常", ex);
            }
        }

        /// <summary>
        /// 服务配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmConfig frm = new FrmConfig();
            frm.ShowDialog();
        }

        /// <summary>
        /// 全部停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var assembly in ExtendAssemblys)
            {
                //停止运行插件
                StopAssembly(assembly);
            }

        }

        /// <summary>
        /// 全部启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var assembly in ExtendAssemblys)
            {
                //停止运行插件
                StartAssembly(assembly);
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (closeWithCloseButton)
                {
                    e.Cancel = true;
                    this.Hide();
                    return;
                }

                if (!(e.Cancel = System.Windows.MessageBox.Show("是否确定要退出？", "提示", MessageBoxButton.OKCancel) != MessageBoxResult.OK))
                {
                    foreach (var assembly in ExtendAssemblys)
                    {
                        //停止运行插件
                        StopAssembly(assembly);
                    }
                    SetVillageNotOffLine();
                }
            }
            finally
            {
                closeWithCloseButton = true;
            }
        }

        /// <summary>
        /// 设置所有小区在线状态为离线状态
        /// </summary>
        private void SetVillageNotOffLine()
        {
            try
            {
                using (DbOperator dbOperator = ConnectionManager.CreateReadConnection())
                {
                    string strsql = "update BaseVillage set IsOnLine=0 ";
                    dbOperator.ClearParameters();

                    int res = dbOperator.ExecuteNonQuery(strsql);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 停止插件运行
        /// </summary>
        /// <param name="assembly">要移除插件</param>
        private void StopAssembly(ExtendAssembly assembly)
        {
            try
            {
                //关闭服务
                foreach (ThreadServiceHost host in assembly.ServiceHosts)
                {
                    host.Stop();
                }

            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        }

        /// <summary>
        /// 运行插件
        /// </summary>
        /// <param name="assembly">插件</param>
        private void StartAssembly(ExtendAssembly assembly)
        {
            try
            {
                if (assembly.Type == AssemblyType.ServiceAssembly)
                {
                    //将外部程序集载入
                    assembly.Assembly = System.Reflection.Assembly.LoadFrom(assembly.FileName);
                    //服务库初始化
                    foreach (Type type in assembly.Assembly.GetTypes())
                    {
                        System.Reflection.MethodInfo isCallbackServiceMethod = null;
                        System.Reflection.MethodInfo getName = null;
                        Object obj = null;
                        try
                        {
                            getName = type.GetMethod("GetName");
                            isCallbackServiceMethod = type.GetMethod("IsCallbackService");
                            if (getName != null)
                            {
                                obj = Activator.CreateInstance(type);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal("加载服务插件", ex);
                            continue;
                        }

                        string httpUri = "http://" + _ServerIP + ":" + _basicHttpPort.ToString() + "/" + type.Name;
                        string netTcpUri = "net.tcp://" + _ServerIP + ":" + _netTcpPort.ToString() + "/" + type.Name;
                        ServiceHost host = GetHost(type, new Uri[] { new Uri(httpUri), new Uri(netTcpUri) });

                        XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas() {
                            MaxDepth = int.MaxValue,
                            MaxStringContentLength = int.MaxValue,
                            MaxArrayLength = int.MaxValue,
                            MaxBytesPerRead = int.MaxValue,
                            MaxNameTableCharCount = int.MaxValue
                        };

                        BasicHttpBinding basicHttpBinding = new BasicHttpBinding() {
                            ReaderQuotas = quotas,
                            MaxReceivedMessageSize = int.MaxValue
                        };

                        NetTcpBinding netTcpBinding = new NetTcpBinding() {
                            ReaderQuotas = quotas,
                            MaxBufferSize = int.MaxValue,
                            MaxBufferPoolSize = int.MaxValue,
                            MaxReceivedMessageSize = int.MaxValue,
                            PortSharingEnabled = true,
                            Security = new NetTcpSecurity { Mode = SecurityMode.None }
                        };
                        
                        string name = getName.Invoke(obj, null) as string;
                        bool isCallbackService = (bool)isCallbackServiceMethod.Invoke(obj, null);

                        if (type.GetCustomAttributes(typeof(ServiceContractAttribute), true).Length > 0)
                        {
                            if (!isCallbackService)
                            {
                                host.AddServiceEndpoint(type, basicHttpBinding, "");
                            }
                            if (!_isBasicHttpOnly)
                            {
                                host.AddServiceEndpoint(type, netTcpBinding, "");
                            }
                        }

                        if (!assembly.ServiceHosts.Any(a => a.ServiceName == name))
                        {
                            assembly.ServiceHosts.Add(new ThreadServiceHost(name));
                        }

                        var threadServiceHost = assembly.ServiceHosts.Single(a => a.ServiceName == name);

                        if (!threadServiceHost.IsRunning)
                        {
                            threadServiceHost.Start(host);
                        }

                        if (isCallbackService)
                        {
                            type.GetMethod("StartListenClients").Invoke(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(assembly.Name + " 启动异常\r\n", ex);

                System.Windows.MessageBox.Show(assembly.Name + " 启动异常," + ex.Message, "提示");
            }
        }


        /// <summary>
        /// 生成一个ServiceHost
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="baseAddresses">基地址</param>
        /// <returns>ServiceHost</returns>
        private ServiceHost GetHost(Type type, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(type, baseAddresses);
            
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() {
                HttpGetEnabled = true
            });
            
            host.Description.Behaviors.Add(new ServiceThrottlingBehavior() {
                MaxConcurrentInstances = int.MaxValue,
                MaxConcurrentCalls = int.MaxValue,
                MaxConcurrentSessions = int.MaxValue
            });
            
            if (!host.Description.Behaviors.Contains(typeof(ServiceBehaviorAttribute)))
            {
                host.Description.Behaviors.Add(new ServiceBehaviorAttribute());
            }

            var serviceBehaviorAttribute = host.Description.Behaviors[typeof(ServiceBehaviorAttribute)] as ServiceBehaviorAttribute;
            serviceBehaviorAttribute.MaxItemsInObjectGraph = int.MaxValue;
            serviceBehaviorAttribute.IncludeExceptionDetailInFaults = true;
            serviceBehaviorAttribute.UseSynchronizationContext = true;

            return host;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SystemDefaultConfig.IsCS = true;
            SystemDefaultConfig.DataUpdateFlag = 3;
            //显示托盘。
            SetNotifyIcon();
            JudgeProcessExist();

            var _startTime = DateTime.Now;
            //状态定时器
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += (obj, args) =>
            {
                tbRunTime.Text = GetTimeString((int)(DateTime.Now - _startTime).TotalSeconds);
            };
            dt.Start();

            _basicHttpPort = OptionHelper.ReadInt("CenterServer", "BasicHttpPort", 80);
            _netTcpPort = OptionHelper.ReadInt("CenterServer", "NetTcpPort", 4508);
            _isBasicHttpOnly = OptionHelper.ReadBool("CenterServer", "IsBasicHttpOnly", false);
            this.Title = OptionHelper.ReadString("CenterServer", "Title", "智慧云平台服务");
            _ServerIP = OptionHelper.ReadString("CenterServer", "ServerIP", "");
            SystemInfo.ImgPath = OptionHelper.ReadString("CenterServer", "ImgPath", "");

            SystemDefaultConfig.ReadConnectionString = new ConnectionStringSettings("ReadConnectionString", "data source=" + OptionHelper.DataSource + ";database=" + OptionHelper.Database + ";user id=" + OptionHelper.LoginName + ";password=" + OptionHelper.Password + "", "System.Data.SqlClient").ToString();
            SystemDefaultConfig.WriteConnectionString = new ConnectionStringSettings("ConnectionString", "data source=" + OptionHelper.DataSource + ";database=" + OptionHelper.Database + ";user id=" + OptionHelper.LoginName + ";password=" + OptionHelper.Password + "", "System.Data.SqlClient").ToString();

            ThreadPool.QueueUserWorkItem(_ => TestDBConnectionState());
            
            LoadExtendConfig();

            foreach (var assembly in ExtendAssemblys.Where(a => a.IsEnabled))
            {
                StartAssembly(assembly);
            }

            lvservers.ItemsSource = ExtendAssemblys[0].ServiceHosts;
            PlatformWcfServers.WXServiceCallback.HeartChanged += WXServiceCallback_HeartChanged;
            PlatformWcfServers.WXServiceCallback.HeartdelteChanged += WXServiceCallback_HeartdelteChanged;
            lvconnection.ItemsSource = PlatformClientes;

            PlatformDownloadData.StartDownload();
        }

        /// <summary>
        /// 检测数据库连接是否正常
        /// </summary>
        private void TestDBConnectionState()
        {
            Action<bool> connState = (iscon) =>
            {
                if (iscon)
                {
                    dbconnection.Source = new BitmapImage(new Uri("/Proxy-Callback;component/Images/Device_Online.png", UriKind.Relative));
                }
                else
                {
                    dbconnection.Source = new BitmapImage(new Uri("/Proxy-Callback;component/Images/Device_Offline.png", UriKind.Relative));
                }
            };

            while (true)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(SystemDefaultConfig.ReadConnectionString))
                    {
                        con.Open();
                        dbconnection.Dispatcher.Invoke(connState, true);
                    }
                }
                catch
                {
                    dbconnection.Dispatcher.Invoke(connState, false);
                }

                Thread.Sleep(1000 * 60 * 1);
            }
        }

        void WXServiceCallback_HeartdelteChanged(object sender, EventArgs e)
        {
            PlatformWcfServers.WXClient client = sender as PlatformWcfServers.WXClient;
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (PlatformClientes.Any(a => a.VillageID == client.VillageID))
                {
                    PlatformClientes.Remove(client);
                }

                tbOnlineNum.Text = PlatformClientes.Count.ToString();
            }));
        }

        void WXServiceCallback_HeartChanged(object sender, EventArgs e)
        {
            PlatformWcfServers.WXClient client = sender as PlatformWcfServers.WXClient;
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (!PlatformClientes.Any(a => a.VillageID == client.VillageID))
                {
                    PlatformClientes.Add(client);
                }

                tbOnlineNum.Text = PlatformClientes.Count.ToString();
            }));
        }

        private string GetTimeString(int seconds)
        {
            return string.Format("{0}天{1}时{2}分{3}秒",
                        (int)(seconds / (3600 * 24)),
                        (int)((seconds - seconds / (3600 * 24) * 3600 * 24) / 3600),
                        (int)((seconds - seconds / 3600 * 3600) / 60),
                        (int)(seconds % 60));
        }

        /// <summary>
        /// 设置托盘
        /// </summary>
        private void SetNotifyIcon()
        {
            _notifyIcon.BalloonTipText = this.Title + "运行中";
            _notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.MouseDoubleClick += (obj, args) =>
            {
                if (this.Visibility == Visibility.Hidden)
                {
                    Show();
                    WindowState = _windowState;
                }
                else
                {
                    _windowState = WindowState;
                    Hide();
                }
            };
            _notifyIcon.ShowBalloonTip(1000);
            this.StateChanged += (obj, args) =>
            {
                if (WindowState == WindowState.Minimized)
                {
                    Hide();
                }
            };

            var mnuItms = new System.Windows.Forms.MenuItem[1];
            mnuItms[0] = new System.Windows.Forms.MenuItem();
            mnuItms[0].Text = "退出";
            mnuItms[0].Click += (sender, e) => {
                closeWithCloseButton = false;
                this.Close();
            };

            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(mnuItms);
        }

        /// <summary>
        /// 判断重复运行
        /// </summary>
        public void JudgeProcessExist()
        {
            string strFullPath, strFileName;
            int count = 0;
            strFullPath = System.Windows.Forms.Application.ExecutablePath;
            strFileName = System.IO.Path.GetFileNameWithoutExtension(strFullPath);
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                if (p.ProcessName == strFileName)
                {
                    count++;
                    if (count > 1)
                    {
                        System.Windows.MessageBox.Show("中心模块已经运行!");
                        Environment.Exit(0);
                    }
                }
            }
        }

        /// <summary>
        /// 加载插件信息
        /// </summary>
        private void LoadExtendConfig()
        {
            if (!File.Exists("Services.config")) return;

            try
            {
                XDocument doc = XDocument.Load("Services.config");
                XElement element = doc.Root.Element("ExtendAssemblys");
                if (element != null)
                {
                    foreach (var ae in element.Elements("Assembly"))
                    {
                        ExtendAssembly assembly = new ExtendAssembly();
                        ExtendAssemblys.Add(assembly);
                        assembly.Name = ae.Attribute("Name").Value;
                        assembly.FileName = ae.Attribute("FileName").Value;
                        assembly.IsEnabled = bool.Parse(ae.Attribute("IsEnabled").Value);
                        int type = int.Parse(ae.Attribute("Type").Value);
                        assembly.Type = (AssemblyType)type;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("读取外部程序集异常!", e);
            }
        }
    }
}
