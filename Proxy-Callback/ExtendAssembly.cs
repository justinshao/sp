using Server.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;

namespace PlatformServers
{
    /// <summary>
    /// 外部文件类型
    /// </summary>
    public enum AssemblyType
    {
        /// <summary>
        /// 普通.net程序集
        /// </summary>
        CommonAssembly = 0,
        /// <summary>
        /// WCF服务库
        /// </summary>
        ServiceAssembly,
        /// <summary>
        /// 动态库
        /// </summary>
        DLL
    }

    /// <summary>
    /// 外部文件
    /// </summary>
    public class ExtendAssembly : INotifyPropertyChanged
    {
        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        public ExtendAssembly()
        {
            ServiceHosts = new ObservableCollection<ThreadServiceHost>();
            MenuItems = new List<MenuItem>();
            TabItems = new List<TabItem>();
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public AssemblyType Type { get; set; }
        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// 服务库中的一系列服务
        /// </summary>
        public ObservableCollection<ThreadServiceHost> ServiceHosts { get; private set; }
        /// <summary>
        /// 插件中的菜单项
        /// </summary>
        public List<MenuItem> MenuItems { get; private set; }
        /// <summary>
        /// 插件中的Tab项
        /// </summary>
        public List<TabItem> TabItems { get; private set; }

        private bool isEnabled = false;
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    Notify("IsEnabled");
                }
            }
        }
    }
}
