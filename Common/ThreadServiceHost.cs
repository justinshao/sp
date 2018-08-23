using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.ServiceModel;
using System.Windows;
using Common.Core;

namespace Server.Common
{
    public class ThreadServiceHost : INotifyPropertyChanged
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

        Logger logger = LogManager.GetLogger("主程序");

        const int SleepTime = 100;

        private Thread _thread;
        private bool _isRunning;

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (value != _isRunning)
                {
                    _isRunning = value;
                    Notify("IsRunning");
                    Notify("IsVisible");
                    Notify("RunningName");
                }
            }
        }

        public string RunningName
        {
            get
            {
                if (_isRunning)
                {
                    return "正在运行";
                }
                else
                {
                    return "服务停止";
                }
            }
        }

        private string _serviceName;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName
        {
            get
            {
                return _serviceName;
            }
            private set
            {
                if (value != _serviceName)
                {
                    _serviceName = value;
                    Notify("ServiceName");
                }
            }
        }

        /// <summary>
        /// 是否可见,用于控制界面上的运行状态
        /// </summary>
        public Visibility IsVisible
        {
            get
            {
                if (_isRunning)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        /// 服务
        /// </summary>
        private ServiceHost _serviceHost = null;

        public ThreadServiceHost(string serviceName)
        {
            ServiceName = serviceName;
        }

        public void Start(ServiceHost serviceHost)
        {
            if (!IsRunning)
            {
                _serviceHost = serviceHost;
                _thread = new Thread(RunService);
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        public void Stop()
        {
            lock (this)
            {
                IsRunning = false;
                try
                {
                    _thread.Abort();
                }
                catch
                {
                }
            }
        }

        private void RunService()
        {
            try
            {
                _serviceHost.Open();
                IsRunning = true;
                while (_isRunning)
                {
                    Thread.Sleep(SleepTime);
                }
                _serviceHost.Close();
                ((IDisposable)_serviceHost).Dispose();
            }
            catch (Exception ex)
            {
                IsRunning = false;
                logger.Fatal("服务运行异常", ex);
                try
                {
                    if (_serviceHost != null)
                        _serviceHost.Close();
                }
                catch (Exception ex2)
                {
                    logger.Fatal("关闭服务异常", ex2);
                }
            }
        }
    }
}
